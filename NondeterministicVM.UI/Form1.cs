using System;
using System.Windows.Forms;
using NondeterministicVM.BLL;

namespace NondeterministicVM
{
    public partial class Form1 : Form
    {
        private CPU _cpu;
        private VirtualMachine _vm;
        private bool _fileLoaded;
        private Memory _memory;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = @"HEX Files|*.hex";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            button3.Enabled = checkBox1.Checked;

            button2.Text = checkBox1.Checked ? "Load" : "Run";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Please select a file first");
            }

            NewVirtualMachine();
        }

        private void NewVirtualMachine()
        {
            _cpu = new CPU();
            var random = new Random();
            _memory = new Memory(random);

            var guiOutput = new GuiOutput(textBox18);

            _vm = new VirtualMachine(_cpu, _memory, random, guiOutput);

            _vm.LoadFromFile(textBox1.Text);

            _fileLoaded = true;

            dataGridView1.DataSource = new BindingSource(_memory._dataStore, null);

            dataGridView1.Columns[0].HeaderText = "Address";
            dataGridView1.Columns[1].HeaderText = "Content";

            textBox18.Text = "";

            try
            {
                ShowRegisters(_cpu);

                if (!checkBox1.Checked)
                {
                    while (true)
                    {
                        NextStep(_vm, _cpu, _memory);
                    }
                }
            }
            catch (VmHaltException)
            {
                MessageBox.Show("Virtual machine halted execution");
            }
        }

        private void NextStep(VirtualMachine vm, CPU cpu, Memory memory)
        {
            vm.NextStep();

            ShowRegisters(cpu);            
        }

        private void ShowRegisters(CPU cpu)
        {
            textBox2.Text = "0x" + cpu.R[0].ToString("X8");
            textBox3.Text = "0x" + cpu.R[1].ToString("X8");
            textBox4.Text = "0x" + cpu.R[2].ToString("X8");
            textBox5.Text = "0x" + cpu.R[3].ToString("X8");
            textBox6.Text = "0x" + cpu.R[4].ToString("X8");
            textBox7.Text = "0x" + cpu.R[5].ToString("X8");
            textBox8.Text = "0x" + cpu.R[6].ToString("X8");
            textBox9.Text = "0x" + cpu.R[7].ToString("X8");
            textBox10.Text = "0x" + cpu.R[8].ToString("X8");
            textBox11.Text = "0x" + cpu.R[9].ToString("X8");
            textBox12.Text = "0x" + cpu.R[10].ToString("X8");
            textBox13.Text = "0x" + cpu.R[11].ToString("X8");
            textBox14.Text = "0x" + cpu.R[12].ToString("X8");
            textBox15.Text = "0x" + cpu.R[13].ToString("X8");
            textBox16.Text = "0x" + cpu.R[14].ToString("X8");
            textBox17.Text = "0x" + cpu.R[15].ToString("X8");
            textBox19.Text = "0x" + cpu.PC.ToString("X8");
            textBox20.Text = _cpu.C.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (! _fileLoaded)
            {
                MessageBox.Show("Please load application first");

                return;
            }
            
            try
            {
                NextStep(_vm, _cpu, _memory);
            }
            catch (VmHaltException)
            {
                MessageBox.Show("Virtual machine halted execution");
            }            
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value != null)
            {
                int value = 0;

                if (int.TryParse(e.Value.ToString(), out value))
                {
                    if (e.ColumnIndex == 0)
                    {
                        e.Value = value.ToString("X");
                    }
                    else
                    {
                        e.Value = value.ToString("X8");
                    }                    
                    e.FormattingApplied = true;
                }
            }
        }
    }
}
