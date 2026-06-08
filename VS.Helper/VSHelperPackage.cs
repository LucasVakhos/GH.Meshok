
global using Community.VisualStudio.Toolkit;
global using Microsoft.VisualStudio.Shell;
global using System;
global using Task = System.Threading.Tasks.Task;

using System.Runtime.InteropServices;
using System.Threading;

namespace VS.Helper;

[PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
[InstalledProductRegistration("VS.Helper", "VS.Helper", "1.0")]
[ProvideMenuResource("Menus.ctmenu", 1)]
[Guid("7D9B4F6E-6B2A-4D8A-8A4B-111111111111")]
public sealed class VSHelperPackage : ToolkitPackage
{
    protected override async Task InitializeAsync(
        CancellationToken cancellationToken,
        IProgress<ServiceProgressData> progress)
    {
        await this.RegisterCommandsAsync();

        await Commands.CreateCommentCommand.StartGitChangesAutoPasteAsync();
    }
}