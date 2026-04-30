using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using Microsoft.VisualStudio.Shell;

namespace DialControllerTools
{
    [ComVisible(true)]
    [Guid("6C43E501-226F-4E73-949C-BF8808A94B48")]
    public class CustomOptions : DialogPage
    {
        private string _clickAction = string.Empty;
        private string _rightAction = string.Empty;
        private string _leftAction = string.Empty;

        public string ClickAction { get => _clickAction; set => _clickAction = value; }
        public string RightAction { get => _rightAction; set => _rightAction = value; }
        public string LeftAction { get => _leftAction; set => _leftAction = value; }

        private string _clickAction2 = string.Empty;
        private string _rightAction2 = string.Empty;
        private string _leftAction2 = string.Empty;

        public string ClickAction2 { get => _clickAction2; set => _clickAction2 = value; }
        public string RightAction2 { get => _rightAction2; set => _rightAction2 = value; }
        public string LeftAction2 { get => _leftAction2; set => _leftAction2 = value; }

        private string _clickAction3 = string.Empty;
        private string _rightAction3 = string.Empty;
        private string _leftAction3 = string.Empty;

        public string ClickAction3 { get => _clickAction3; set => _clickAction3 = value; }
        public string RightAction3 { get => _rightAction3; set => _rightAction3 = value; }
        public string LeftAction3 { get => _leftAction3; set => _leftAction3 = value; }

        public string GetClickAction(int slot)
        {
            switch (slot)
            {
                case 2: return ClickAction2;
                case 3: return ClickAction3;
                default: return ClickAction;
            }
        }

        public string GetRightAction(int slot)
        {
            switch (slot)
            {
                case 2: return RightAction2;
                case 3: return RightAction3;
                default: return RightAction;
            }
        }

        public string GetLeftAction(int slot)
        {
            switch (slot)
            {
                case 2: return LeftAction2;
                case 3: return LeftAction3;
                default: return LeftAction;
            }
        }

        protected override IWin32Window Window
         => new CustomOptionsControl
         {
             CustomOptions = this
         };
    }
}
