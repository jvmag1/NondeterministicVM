using System.Windows.Forms;
using NondeterministicVM.BLL;

namespace NondeterministicVM
{
    public class GuiOutput : IOutput
    {
        private TextBox _textBox;

        public GuiOutput(TextBox textBox)
        {
            _textBox = textBox;
        }

        public void Write(string text)
        {
            _textBox.Text += text;
        }
    }
}
