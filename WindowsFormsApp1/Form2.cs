using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Numerics;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

namespace WindowsFormsApp1
{

    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 Form = this;
            Form1 main = this.Owner as Form1;
            main.R1 = Convert.ToDouble(Form.textBox1.Text);
            main.R2 = Convert.ToDouble(Form.textBox2.Text);
            main.L = Convert.ToDouble(Form.textBox3.Text);
            main.Kp = Convert.ToDouble(Form.textBox4.Text);
            main.S = Convert.ToDouble(Form.textBox6.Text);
            main.KLossEffective = (1 / (2 * main.L)) * Math.Log(1 / (main.R1 * main.R2));
            Form.textBox5.Text = Convert.ToString(main.KLossEffective);
            main.Controls["textBox3"].Text = Convert.ToString(main.KLossEffective+main.Kp);
        }          
    }
}
