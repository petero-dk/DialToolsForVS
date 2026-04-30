using System;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DialControllerTools.Helpers;
using Microsoft.VisualStudio.Threading;
using ThreadHelper = Microsoft.VisualStudio.Shell.ThreadHelper;
//Timer idea from https://stackoverflow.com/questions/26020799/enforcing-a-delay-on-textbox-textchanged
namespace DialControllerTools
{
    public partial class CustomOptionsControl : UserControl
    {
        private string _selectedText;
        private Timer _timer;
        private readonly string commandsString;
        private int _currentSlot = 1;

        private ImmutableArray<string> commands;
        private ImmutableArray<string> Commands
         => commands.IsDefaultOrEmpty
            ? commands = VsCommands.ParseCommands(commandsString)
            : commands;

        private CustomOptions customOptions;
        internal CustomOptions CustomOptions
        {
            get => customOptions;
            set
            {
                customOptions = value;
                LoadSlot(_currentSlot);
            }
        }

        public CustomOptionsControl()
        {
            InitializeComponent();
            CommandsBox.Text = commandsString = VsCommands.ReadCommandsAsString();
            VsCommands.CheckEmptyEntries(commandsString);

            _timer = new Timer();
            _timer.Interval = 300;
            _timer.Tick += Timer_Tick;

            SlotSelector.Items.AddRange(new object[] { "Custom 1", "Custom 2", "Custom 3" });
            SlotSelector.SelectedIndex = 0;
            SlotSelector.SelectedIndexChanged += SlotSelector_SelectedIndexChanged;
        }

        private void SlotSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            _currentSlot = SlotSelector.SelectedIndex + 1;
            if (customOptions != null)
                LoadSlot(_currentSlot);
        }

        private void LoadSlot(int slot)
        {
            AssignedClickLabel.Text = customOptions.GetClickAction(slot);
            AssignedRightLabel.Text = customOptions.GetRightAction(slot);
            AssignedLeftLabel.Text = customOptions.GetLeftAction(slot);
        }

        private void AssignClickAction_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_selectedText))
            {
                SetClickAction(_currentSlot, _selectedText);
                AssignedClickLabel.Text = _selectedText;
            }
        }

        private void AssignRightAction_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_selectedText))
            {
                SetRightAction(_currentSlot, _selectedText);
                AssignedRightLabel.Text = _selectedText;
            }
        }

        private void AssignLeftAction_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_selectedText))
            {
                SetLeftAction(_currentSlot, _selectedText);
                AssignedLeftLabel.Text = _selectedText;
            }
        }

        private void SetClickAction(int slot, string value)
        {
            switch (slot)
            {
                case 2: CustomOptions.ClickAction2 = value; break;
                case 3: CustomOptions.ClickAction3 = value; break;
                default: CustomOptions.ClickAction = value; break;
            }
        }

        private void SetRightAction(int slot, string value)
        {
            switch (slot)
            {
                case 2: CustomOptions.RightAction2 = value; break;
                case 3: CustomOptions.RightAction3 = value; break;
                default: CustomOptions.RightAction = value; break;
            }
        }

        private void SetLeftAction(int slot, string value)
        {
            switch (slot)
            {
                case 2: CustomOptions.LeftAction2 = value; break;
                case 3: CustomOptions.LeftAction3 = value; break;
                default: CustomOptions.LeftAction = value; break;
            }
        }

        private void SearchBox_TextChanged(object sender, EventArgs e)
        {
            _timer.Stop();
            _timer.Tag = SearchBox.Text;
            _timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            var searchText = _timer.Tag?.ToString() ?? string.Empty;
            var results = Commands.Where(c => c.IndexOf(searchText, StringComparison.InvariantCultureIgnoreCase) > -1);
            int newLineSymbolsLength = Environment.NewLine.Length;
            CommandsBox.Text = results.Any()
                ? results.Aggregate(
                    new StringBuilder(results.Sum(r => r.Length + newLineSymbolsLength)),
                    (accum, item) =>
                    {
                        accum.Append(item);
                        accum.Append(Environment.NewLine);
                        return accum;
                    },
                    accum =>
                    {
                        accum.Length -= newLineSymbolsLength;
                        return accum.ToString();
                    })
                : string.Empty;
        }

        private void CommandsBox_MouseClick(object sender, MouseEventArgs e)
        {
            var current_line = CommandsBox.GetLineFromCharIndex(CommandsBox.SelectionStart);
            _selectedText = CommandsBox.Lines[current_line];
            CommandsBox.SelectionStart = CommandsBox.GetFirstCharIndexFromLine(current_line);
            CommandsBox.SelectionLength = _selectedText?.Length ?? 0;
        }
    }
}
