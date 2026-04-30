using EnvDTE;

using EnvDTE80;

using Windows.UI.Input;

namespace DialControllerTools
{
    internal class UndoRedoController : BaseController
    {
        private readonly Commands _commands;

        public override string Moniker => UndoRedoControllerProvider.Moniker;
        public override bool CanHandleClick => true;
        public override bool CanHandleRotate => true;

        public UndoRedoController(RadialControllerMenuItem menuItem, DTE2 dte) : base(menuItem)
        {
#pragma warning disable VSTHRD010 // Invoke single-threaded types on Main thread
            _commands = dte.Commands;
#pragma warning restore VSTHRD010 // Invoke single-threaded types on Main thread
        }

        public override bool OnClick()
        {
            _commands.ExecuteCommand("View.UndoHistory");
            return true;
        }

        public override bool OnRotate(RotationDirection direction)
        {
            switch (direction)
            {
                case RotationDirection.Left:
                    _commands.ExecuteCommand("Edit.Undo");
                    break;
                case RotationDirection.Right:
                    _commands.ExecuteCommand("Edit.Redo");
                    break;
            }

            return true;
        }
    }
}
