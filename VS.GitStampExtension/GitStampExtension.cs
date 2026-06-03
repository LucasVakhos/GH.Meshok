using Microsoft.VisualStudio.Extensibility;

namespace GitStampExtension;

[VisualStudioContribution]
public sealed class GitStampExtension : Extension
{
    public override ExtensionConfiguration ExtensionConfiguration => new()
    {
        Metadata = new(
            id: "GitStampExtension.b450b6c5-07ce-43d4-a384-09f9110c775a",
            version: ExtensionAssemblyVersion,
            publisherName: "Вячеслав",
            displayName: "Git Stamp Extension",
            description: "Copies solution name with timestamp for Git commit messages.")
    };
}