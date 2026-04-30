
using System.ComponentModel.Composition;

using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;

using Windows.UI.Input;

namespace DialControllerTools
{
    public abstract class BaseTextController : BaseController
    {
        private readonly IVsTextManager textManager;

#pragma warning disable IDE0044 // Add readonly modifier
#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value null
        [Import]
        private IVsEditorAdaptersFactoryService editorAdapter;
#pragma warning restore CS0649
#pragma warning restore IDE0044 // Add readonly modifier

        internal BaseTextController(RadialControllerMenuItem menuItem, IVsTextManager textManager) : base(menuItem)
        {
            this.textManager = textManager;
        }

        public IVsTextView GetCurrentNativeTextView()
        {
            ErrorHandler.ThrowOnFailure(textManager.GetActiveView(1, null, out IVsTextView activeView));
            return activeView;
        }

        ///<summary>Gets the TextView for the active document.</summary>
        public IWpfTextView GetCurrentTextView() => GetTextView(GetCurrentNativeTextView());

        public IWpfTextView GetTextView(IVsTextView nativeView)
        {
            if (nativeView == null)
                return null;

            return editorAdapter.GetWpfTextView(nativeView);
        }
    }
}
