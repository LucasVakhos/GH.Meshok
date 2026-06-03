using Microsoft.VisualStudio.Extensibility;
using Microsoft.VisualStudio.Extensibility.Commands;
using Microsoft.VisualStudio.Extensibility.Shell;
using Microsoft.VisualStudio.ProjectSystem.Query;
using System.IO;
using System.Windows;

namespace GitStampExtension;

[VisualStudioContribution]
public sealed class CopyGitStampCommand : Command
{
    public CopyGitStampCommand(VisualStudioExtensibility extensibility)
        : base(extensibility)
    {
    }

    public override CommandConfiguration CommandConfiguration => new("Copy Git Stamp")
    {
        Placements = [CommandPlacement.KnownPlacements.ExtensionsMenu],
        TooltipText = "Copy solution name and timestamp to clipboard"
    };

    public override async Task ExecuteCommandAsync(
        IClientContext context,
        CancellationToken cancellationToken)
    {
        string name = await GetSolutionNameAsync(cancellationToken);

        if (string.IsNullOrWhiteSpace(name))
            name = "Project";

        string stamp = $"{name} {DateTime.Now:yyyy-MM-dd HH:mm:ss}";

        Clipboard.SetText(stamp);

        await Extensibility.Shell().ShowPromptAsync(
            $"Скопировано в буфер:\n{stamp}",
            PromptOptions.OK,
            cancellationToken);
    }

    private async Task<string> GetSolutionNameAsync(CancellationToken cancellationToken)
    {
        var solution = await Extensibility.Workspaces().QuerySolutionAsync(
            query => query.With(solution => solution.Path),
            cancellationToken);

        string? solutionPath = solution.FirstOrDefault()?.Path;

        if (string.IsNullOrWhiteSpace(solutionPath))
            return string.Empty;

        return Path.GetFileNameWithoutExtension(solutionPath);
    }
}