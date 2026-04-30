using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio.Shell;

namespace DialControllerTools
{
    [DialControllerProvider(order: 11)]
    internal class CopilotControllerProvider : BaseDialControllerProvider
    {
        public static string Moniker = nameof(KnownProviders.Copilot);

        public CopilotControllerProvider() { }

        protected override async Task<IDialController> TryCreateControllerAsyncOverride(IAsyncServiceProvider provider, CancellationToken cancellationToken)
        {
            string iconFilePath = VsHelpers.GetFileInVsix(@"Providers\Copilot\icon.png");
            var menuItem = await CreateMenuItemAsync(Moniker, iconFilePath);
            var dte = await provider.GetDteAsync(cancellationToken);
            return new CopilotController(menuItem, dte);
        }
    }
}
