using System;
using System.Windows.Forms;

namespace TGDBHashTool
{
    public partial class NumberEntry : Form
    {
        public int Number
        {
            get => (int)numericUpDown1.Value;
            set => numericUpDown1.Value = value;
        }

        public NumberEntry()
        {
            InitializeComponent();
        }

        private void NumberEntry_Load(object sender, EventArgs e)
        {
            numericUpDown1.ResetText();
            numericUpDown1.Focus();
        }
    }
}
