using System.Windows.Input;

using EnvDTE;

using EnvDTE80;

using Windows.UI.Input;

namespace DialControllerTools
{
    internal class CopilotController : BaseController
    {
        private readonly Commands _commands;

        public override string Moniker => CopilotControllerProvider.Moniker;
        public override bool CanHandleClick => true;
        public override bool CanHandleRotate => true;

        public CopilotController(RadialControllerMenuItem menuItem, DTE2 dte) : base(menuItem)
        {
            _commands = dte.Commands;
        }

        public override bool OnClick()
        {
            bool isShiftPressed = Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift);

            if (isShiftPressed)
            {
                _commands.ExecuteCommand("Edit.DismissSuggestion");
            }
            else
            {
                _commands.ExecuteCommand("Edit.AcceptSuggestion");
            }

            return true;
        }

        public override bool OnRotate(RotationDirection direction)
        {
            switch (direction)
            {
                case RotationDirection.Right:
                    _commands.ExecuteCommand("Edit.NextSuggestion");
                    break;
                case RotationDirection.Left:
                    _commands.ExecuteCommand("Edit.PreviousSuggestion");
                    break;
            }

            return true;
        }
    }
}
