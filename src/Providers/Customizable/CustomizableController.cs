using EnvDTE;

using EnvDTE80;

using Windows.UI.Input;

namespace DialControllerTools
{
    internal class CustomizableController : BaseController
    {
        private readonly Commands _commands;
        private readonly string _moniker;
        private readonly int _slot;

        public override string Moniker => _moniker;
        public override bool CanHandleClick => true;
        public override bool CanHandleRotate => true;

        public CustomizableController(RadialControllerMenuItem menuItem, DTE2 dte, string moniker, int slot = 1) : base(menuItem)
        {
#pragma warning disable VSTHRD010 // Invoke single-threaded types on Main thread
            _commands = dte.Commands;
#pragma warning restore VSTHRD010 // Invoke single-threaded types on Main thread
            _moniker = moniker;
            _slot = slot;
        }

        public override bool OnClick()
        {
            _commands.ExecuteCommand(DialPackage.CustomOptions.GetClickAction(_slot));
            return true;
        }

        public override bool OnRotate(RotationDirection direction)
        {
            switch (direction)
            {
                case RotationDirection.Left:
                    _commands.ExecuteCommand(DialPackage.CustomOptions.GetLeftAction(_slot));
                    break;
                case RotationDirection.Right:
                    _commands.ExecuteCommand(DialPackage.CustomOptions.GetRightAction(_slot));
                    break;
            }

            return true;
        }
    }
}
