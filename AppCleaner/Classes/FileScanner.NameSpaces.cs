using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AppCleaner
{
    public partial class FileScanner
    {
        #region Namespace operations

        private int NormalizeNamespacesInDirectory(bool dryRun, CancellationToken cancellationToken)
        {
            // Раньше нормализатор namespace сканировал все *.cs в подпапках.
            // Теперь он работает только по файлам проекта, если проект найден.
            var files = GetFilesForOperation(
                    _store.SearchFolder,
                    "*.cs",
                    cancellationToken,
                    preferProjectFiles: true,
                    includeDesignerFiles: false)
                .ToArray();

            _store.SetProgressMaximum(files.Length);

            var changedFiles = 0;

            for (var i = 0; i < files.Length; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                try
                {
                    if (NormalizeNamespacesInFile(files[i], dryRun, cancellationToken))
                        changedFiles++;
                }
                catch (OperationCanceledException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    AddToLog($"[Ошибка] namespace-normalizer: {files[i]} - {ex.Message}");
                }
                finally
                {
                    CountProcessedFile(files[i]);
                }
            }

            AddToLog($"Namespace-normalizer завершён. Изменено файлов: {changedFiles}");
            return changedFiles;
        }

        private bool NormalizeNamespacesInFile(string filePath, bool dryRun, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var encoding = DetectFileEncoding(filePath);
            var originalSource = File.ReadAllText(filePath, encoding);
            var normalizedSource = NamespaceNormalizer.NormalizeSameNamespacePrefixes(originalSource, out var changed);

            if (!changed)
            {
                AddToLog($"[Пропуск] {Path.GetFileName(filePath)} - namespace-префиксы не найдены.");
                return false;
            }

            if (dryRun)
            {
                AddToLog($"[DRY-RUN] Был бы обновлён namespace: {filePath}");
                return true;
            }

            CreateBackup(filePath);
            File.WriteAllText(filePath, normalizedSource, encoding);

            AddToLog($"[Обновлено] namespace: {Path.GetFileName(filePath)}");
            return true;
        }

        private void CollectAllNamespaces(CancellationToken cancellationToken)
        {
            // Сбор namespace тоже больше не обходит всё дерево без необходимости.
            var files = GetFilesForOperation(
                    _store.SearchFolder,
                    "*.cs",
                    cancellationToken,
                    preferProjectFiles: true,
                    includeDesignerFiles: false)
                .ToArray();

            _store.SetProgressMaximum(files.Length);

            var namespaces = new SortedDictionary<string, int>(StringComparer.Ordinal);

            for (var i = 0; i < files.Length; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                try
                {
                    var source = File.ReadAllText(files[i], DetectFileEncoding(files[i]));

                    foreach (var ns in NamespaceNormalizer.GetDeclaredNamespaces(source))
                        namespaces[ns] = namespaces.TryGetValue(ns, out var count) ? count + 1 : 1;
                }
                catch (Exception ex) when (ex is not OperationCanceledException)
                {
                    AddToLog($"[Ошибка] при сборе namespace из {files[i]}: {ex.Message}");
                }
                finally
                {
                    CountProcessedFile(files[i]);
                }
            }

            AddToLog(namespaces.Count == 0
                ? "Namespace не найдены."
                : string.Join(Environment.NewLine, namespaces.Select(x => $"{x.Key} : {x.Value}")));
        }

        private static class NamespaceNormalizer
        {
            public static string NormalizeSameNamespacePrefixes(string source, out bool changed)
            {
                changed = false;

                if (string.IsNullOrWhiteSpace(source))
                    return source;

                var tree = CSharpSyntaxTree.ParseText(source);
                var root = tree.GetCompilationUnitRoot();

                var namespaces = root.DescendantNodes()
                    .OfType<BaseNamespaceDeclarationSyntax>()
                    .Select(x => x.Name.ToString())
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Distinct(StringComparer.Ordinal)
                    .OrderByDescending(x => x.Length)
                    .ToArray();

                if (namespaces.Length == 0)
                    return source;

                var rewriter = new SameNamespacePrefixRewriter(namespaces);
                var newRoot = rewriter.Visit(root);

                if (newRoot is null || !rewriter.Changed)
                    return source;

                var normalized = newRoot.ToFullString();

                changed = !string.Equals(source, normalized, StringComparison.Ordinal);

                return normalized;
            }

            public static IReadOnlyCollection<string> GetDeclaredNamespaces(string source)
            {
                if (string.IsNullOrWhiteSpace(source))
                    return Array.Empty<string>();

                var tree = CSharpSyntaxTree.ParseText(source);
                var root = tree.GetCompilationUnitRoot();

                return root.DescendantNodes()
                    .OfType<BaseNamespaceDeclarationSyntax>()
                    .Select(x => x.Name.ToString())
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Distinct(StringComparer.Ordinal)
                    .OrderBy(x => x, StringComparer.Ordinal)
                    .ToArray();
            }

            private sealed class SameNamespacePrefixRewriter : CSharpSyntaxRewriter
            {
                private readonly IReadOnlyList<string> _namespaces;

                public SameNamespacePrefixRewriter(IReadOnlyList<string> namespaces)
                {
                    _namespaces = namespaces;
                }

                public bool Changed { get; private set; }

                public override SyntaxNode? VisitUsingDirective(UsingDirectiveSyntax node) => node;

                public override SyntaxNode? VisitAliasQualifiedName(AliasQualifiedNameSyntax node) => node;

                public override SyntaxNode? VisitQualifiedName(QualifiedNameSyntax node)
                {
                    var visited = (QualifiedNameSyntax?)base.VisitQualifiedName(node) ?? node;
                    return TryShortenName(visited.ToString(), visited) ?? visited;
                }

                public override SyntaxNode? VisitMemberAccessExpression(MemberAccessExpressionSyntax node)
                {
                    var visited = (MemberAccessExpressionSyntax?)base.VisitMemberAccessExpression(node) ?? node;
                    var fullName = visited.ToString();

                    foreach (var ns in _namespaces)
                    {
                        var prefix = ns + ".";

                        if (!fullName.StartsWith(prefix, StringComparison.Ordinal))
                            continue;

                        var shortName = fullName[prefix.Length..];

                        if (string.IsNullOrWhiteSpace(shortName) || !LooksLikeTypeOrNamespaceName(shortName))
                            return visited;

                        Changed = true;
                        return SyntaxFactory.ParseExpression(shortName).WithTriviaFrom(visited);
                    }

                    return visited;
                }

                private SyntaxNode? TryShortenName(string fullName, SyntaxNode originalNode)
                {
                    foreach (var ns in _namespaces)
                    {
                        var prefix = ns + ".";

                        if (!fullName.StartsWith(prefix, StringComparison.Ordinal))
                            continue;

                        var shortName = fullName[prefix.Length..];

                        if (string.IsNullOrWhiteSpace(shortName))
                            return null;

                        Changed = true;
                        return SyntaxFactory.ParseName(shortName).WithTriviaFrom(originalNode);
                    }

                    return null;
                }

                private static bool LooksLikeTypeOrNamespaceName(string value)
                {
                    var firstIdentifier = value.Split('.')[0];
                    return firstIdentifier.Length > 0 && char.IsUpper(firstIdentifier[0]);
                }
            }
        }

        #endregion
    }
}
