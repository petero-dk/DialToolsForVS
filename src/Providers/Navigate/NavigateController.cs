using EnvDTE;

using EnvDTE80;

using Microsoft.VisualStudio.TextManager.Interop;

using Windows.UI.Input;

namespace DialControllerTools
{
    internal class NavigateController : BaseTextController
    {
        private readonly Commands _commands;

        public override string Moniker => NavigateControllerProvider.Moniker;
        public override bool CanHandleRotate => true;

        public NavigateController(RadialControllerMenuItem menuItem, DTE2 dte, IVsTextManager textManager)
            : base(menuItem, textManager)
        {
#pragma warning disable VSTHRD010 // Invoke single-threaded types on Main thread
            _commands = dte.Commands;
#pragma warning restore VSTHRD010 // Invoke single-threaded types on Main thread
        }

        public override bool OnRotate(RotationDirection direction)
        {
            switch (direction)
            {
                case RotationDirection.Left:
                    _commands.ExecuteCommand("View.NavigateBackward");
                    break;
                case RotationDirection.Right:
                    _commands.ExecuteCommand("View.NavigateForward");
                    break;
            }

            return true;
        }
    }
}
