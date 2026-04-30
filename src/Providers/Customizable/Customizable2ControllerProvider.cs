using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio.Shell;

namespace DialControllerTools
{
    [DialControllerProvider(order: 12)]
    internal class Customizable2ControllerProvider : BaseDialControllerProvider
    {
        public static string Moniker = nameof(KnownProviders.Custom2);

        public Customizable2ControllerProvider() { }

        protected override async Task<IDialController> TryCreateControllerAsyncOverride(IAsyncServiceProvider provider, CancellationToken cancellationToken)
        {
            string iconFilePath = VsHelpers.GetFileInVsix(@"Providers\Customizable\icon.png");
            var menuItem = await CreateMenuItemAsync(Moniker, iconFilePath);
            var dte = await provider.GetDteAsync(cancellationToken);
            return new CustomizableController(menuItem, dte, Moniker, 2);
        }
    }
}
