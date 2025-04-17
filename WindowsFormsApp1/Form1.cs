using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Numerics;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public bool FlagStop, FlagPause;
        public double Kampl, Ugen, Kloss, TimeStepWidth, Kampl0, Ugen0, B32, x0, y0, u;
        public double R1, R2, L, Kp, KLossEffective, S;

        private void button5_Click(object sender, EventArgs e)
        {
            Form2 f = new Form2();
            f.Owner = this;
            f.Show();
        }
       
        public double[] k = new double[8];
        public double[] l = new double[8];
        public int i, j, StepNumber = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FlagStop = true;
            this.button1.Text = "Start";
            this.chart1.Series[0].Points.Clear();
            this.chart2.Series[0].Points.Clear();
            Kampl0 = Convert.ToDouble(this.textBox1.Text);
            Ugen0 = Convert.ToDouble(this.textBox2.Text);
            Kloss = Convert.ToDouble(this.textBox3.Text);
            B32 = Convert.ToDouble(this.textBox4.Text);
            TimeStepWidth = Convert.ToDouble(this.textBox5.Text);
            x0 = Kampl0;
            y0 = -Kampl0 * B32 * Ugen0;
            u = Ugen0;
            StepNumber = 0;
            
            for (i = 1; i <= 4; i++)
            {
                k[i] = 0;
                l[i] = 0;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Kampl0 = Convert.ToDouble(this.textBox1.Text);
            Kloss = Convert.ToDouble(this.textBox3.Text);             
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FlagStop = false;

            k[1] = dK(TimeStepWidth, B32, Ugen0, Kampl0); 
            l[1] = dU(TimeStepWidth, Kloss, Ugen0, Kampl0);

            k[2] = dK(TimeStepWidth, B32, Ugen0 + TimeStepWidth * l[1] / 3, Kampl0 + TimeStepWidth * k[1] / 3); 
            l[2] = dU(TimeStepWidth, Kloss, Ugen0 + TimeStepWidth * l[1] / 3, Kampl0 + TimeStepWidth * k[1] / 3);

            k[3] = dK(TimeStepWidth, B32, Ugen0 - (TimeStepWidth * l[1] / 3) + TimeStepWidth * l[2], Kampl0 - (TimeStepWidth * k[1] / 3) + TimeStepWidth * k[2]);
            l[3] = dU(TimeStepWidth, Kloss, Ugen0 - (TimeStepWidth * l[1] / 3) + TimeStepWidth * l[2], Kampl0 - (TimeStepWidth * k[1] / 3) + TimeStepWidth * k[2]);

            k[4] = dK(TimeStepWidth, B32, Ugen0 + (TimeStepWidth * l[1]) - (TimeStepWidth * l[2]) + (TimeStepWidth * l[3]), Kampl0 + (TimeStepWidth * k[1]) - (TimeStepWidth * k[2]) + (TimeStepWidth * k[3]));
            l[4] = dU(TimeStepWidth, Kloss, Ugen0 + (TimeStepWidth * l[1]) - (TimeStepWidth * l[2]) + (TimeStepWidth * l[3]), Kampl0 + (TimeStepWidth * k[1]) - (TimeStepWidth * k[2]) + (TimeStepWidth * k[3]));

            Kampl = Kampl0 + TimeStepWidth / 8 * (k[1] + 3 * k[2] + 3 * k[3] + k[4]);
            Ugen = Ugen0 + TimeStepWidth / 8 * (l[1] + 3 * l[2] + 3 * l[3] + l[4]);

            Kampl0 = Kampl;
            Ugen0 = Ugen;

            Ugen = Ugen * 3 * Math.Pow(10, 8) * S * (KLossEffective / Kloss);

            this.chart1.Series[0].Points.AddXY(StepNumber * TimeStepWidth, Kampl);
            this.chart2.Series[0].Points.AddXY(StepNumber* TimeStepWidth, Ugen);


            double[] AVal = new double[0];
            Array.Resize(ref AVal, StepNumber);

            while (FlagStop == false)
            {
                StepNumber += 1;

                k[1] = dK(TimeStepWidth, B32, Ugen0, Kampl0);
                l[1] = dU(TimeStepWidth, Kloss, Ugen0, Kampl0);

                k[2] = dK(TimeStepWidth, B32, Ugen0 + TimeStepWidth * l[1] / 3, Kampl0 + TimeStepWidth * k[1] / 3);
                l[2] = dU(TimeStepWidth, Kloss, Ugen0 + TimeStepWidth * l[1] / 3, Kampl0 + TimeStepWidth * k[1] / 3);

                k[3] = dK(TimeStepWidth, B32, Ugen0 - (TimeStepWidth * l[1] / 3) + TimeStepWidth * l[2], Kampl0 - (TimeStepWidth * k[1] / 3) + TimeStepWidth * k[2]);
                l[3] = dU(TimeStepWidth, Kloss, Ugen0 - (TimeStepWidth * l[1] / 3) + TimeStepWidth * l[2], Kampl0 - (TimeStepWidth * k[1] / 3) + TimeStepWidth * k[2]);

                k[4] = dK(TimeStepWidth, B32, Ugen0 + (TimeStepWidth * l[1]) - (TimeStepWidth * l[2]) + (TimeStepWidth * l[3]), Kampl0 + (TimeStepWidth * k[1]) - (TimeStepWidth * k[2]) + (TimeStepWidth * k[3]));
                l[4] = dU(TimeStepWidth, Kloss, Ugen0 + (TimeStepWidth * l[1]) - (TimeStepWidth * l[2]) + (TimeStepWidth * l[3]), Kampl0 + (TimeStepWidth * k[1]) - (TimeStepWidth * k[2]) + (TimeStepWidth * k[3]));

                Kampl = Kampl0 + TimeStepWidth / 8 * (k[1] + 3 * k[2] + 3 * k[3] + k[4]);
                Ugen = Ugen0 + TimeStepWidth / 8 * (l[1] + 3 * l[2] + 3 * l[3] + l[4]);

                Kampl0 = Kampl;
                Ugen0 = Ugen;

                Ugen = Ugen * 3 * Math.Pow(10, 8) * S * (KLossEffective / Kloss);

                this.chart1.Series[0].Points.AddXY(StepNumber * TimeStepWidth, Kampl);
                this.chart2.Series[0].Points.AddXY(StepNumber * TimeStepWidth, Ugen);
                this.textBox8.Text = Convert.ToString(StepNumber);

                Application.DoEvents();
                if (FlagStop == true) {
                    break;
                }
            }
        }// добавить возможность расчета кэоффициентов
         // усиления и внутрирезонаторных потерь с учетом характеристик резонатора лазера

        private void button2_Click(object sender, EventArgs e)
        {
            this.button1.Text = "Continue";
            FlagStop = true;
        }

        public double dU(double Time, double Kloss, double Ugen, double Kampl)
        {
            double funct = 3*Math.Pow(10, 8)*(Kampl - Kloss)* Ugen;
            return funct;
        }
        public double dK(double Time,  double B32, double Ugen, double Kampl)
        {
            double funct = -B32* Ugen* Kampl;
            return funct;
        }
    }
}
