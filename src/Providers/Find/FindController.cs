using EnvDTE;

using EnvDTE80;

using Windows.UI.Input;

namespace DialControllerTools
{
    internal class FindController : BaseController, IContextAwareController
    {
        private readonly DTE2 _dte;
        private readonly Commands _commands;

        public override string Moniker => FindControllerProvider.Moniker;
        public override bool CanHandleClick => true;
        public override bool CanHandleRotate => true;

        public FindController(RadialControllerMenuItem menuItem, DTE2 dte) : base(menuItem)
        {
            _dte = dte;
#pragma warning disable VSTHRD010 // Invoke single-threaded types on Main thread
            _commands = dte.Commands;
#pragma warning restore VSTHRD010 // Invoke single-threaded types on Main thread
        }

#pragma warning disable VSTHRD010
        public int GetContextRelevance(Window activeWindow)
        {
            return activeWindow.IsFindResults() ? 100 : 0;
        }
#pragma warning restore VSTHRD010

        public override bool OnClick()
        {
            _commands.ExecuteCommand("Edit.FindInFiles");
            return true;
        }

        public override bool OnRotate(RotationDirection direction)
        {
            switch (direction)
            {
                case RotationDirection.Left:
                    _commands.ExecuteCommand("Edit.GoToPrevLocation");
                    break;
                case RotationDirection.Right:
                    _commands.ExecuteCommand("Edit.GoToNextLocation");
                    break;
            }

            return true;
        }
    }
}
