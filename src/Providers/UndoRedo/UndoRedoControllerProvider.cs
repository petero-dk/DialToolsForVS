using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio.Shell;

namespace DialControllerTools
{
    [DialControllerProvider(order: 10)]
    internal class UndoRedoControllerProvider : BaseDialControllerProvider
    {
        public static string Moniker = nameof(KnownProviders.UndoRedo);

        public UndoRedoControllerProvider() { }

        protected override async Task<IDialController> TryCreateControllerAsyncOverride(IAsyncServiceProvider provider, CancellationToken cancellationToken)
        {
            string iconFilePath = VsHelpers.GetFileInVsix(@"Providers\UndoRedo\icon.png");
            var menuItem = await CreateMenuItemAsync(Moniker, iconFilePath);
            var dte = await provider.GetDteAsync(cancellationToken);
            return new UndoRedoController(menuItem, dte);
        }
    }
}
