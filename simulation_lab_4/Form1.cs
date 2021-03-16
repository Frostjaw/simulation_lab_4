using MathNet.Numerics.Distributions;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace simulation_lab_4
{
    public partial class Form1 : Form
    {
        private NormalDistributionRNG _rng;

        public Form1()
        {
            InitializeComponent();

            _rng = new NormalDistributionRNG();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;

            if (!char.IsDigit(number) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;

            if (!char.IsDigit(number) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;

            if (!char.IsDigit(number) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ClearHistogram();

            var meanString = textBox1.Text;
            if (string.IsNullOrEmpty(meanString))
            {
                MessageBox.Show("Enter mean");

                return;
            }

            var varianceString = textBox2.Text;
            if (string.IsNullOrEmpty(varianceString))
            {
                MessageBox.Show("Enter variance");

                return;
            }

            var experimentsCountString = textBox3.Text;
            if (string.IsNullOrEmpty(experimentsCountString))
            {
                MessageBox.Show("Enter number of experiments");

                return;
            }

            if (experimentsCountString == "0")
            {
                MessageBox.Show("Enter valid number of experiments");

                return;
            }

            var theoreticalMean = Convert.ToInt32(textBox1.Text);
            var theoreticalVariance = Convert.ToInt32(textBox2.Text);
            var experimentsCount = Convert.ToInt32(textBox3.Text);

            var sample = _rng.GetRandomNumberList(theoreticalMean, theoreticalVariance, experimentsCount);

            var statistics = new NormalDistributionStatistics(sample, theoreticalMean, theoreticalVariance);

            var empiricalMean = statistics.EmpiricalMean;
            var meanError = statistics.MeanError;
            var empiricalVariance = statistics.EmpiricalVariance;
            var varianceError = statistics.VarianceError;
            var occurences = statistics.Occurrences;

            textBox4.Text = empiricalMean.ToString("N3");
            textBox9.Text = meanError.ToString("N3");
            textBox5.Text = empiricalVariance.ToString("N3");
            textBox10.Text = varianceError.ToString("N3");

            var chiSquared = statistics.ChiSquared;
            double criticalValue = 0;
            if (experimentsCount > 2)
            {
                criticalValue = ChiSquared.InvCDF(experimentsCount - 2, 0.95);
            }
            else if (experimentsCount == 2)
            {
                criticalValue = ChiSquared.InvCDF(experimentsCount - 1, 0.95);
            }
            else if (experimentsCount == 1)
            {
                criticalValue = ChiSquared.InvCDF(experimentsCount, 0.95);
            }

            textBox6.Text = chiSquared.ToString("N3");
            textBox7.Text = criticalValue.ToString("N3");

            if (chiSquared > criticalValue)
            {
                textBox8.Text = "FALSE";
                textBox8.BackColor = Color.Red;
                textBox8.Show();
            }
            else
            {
                textBox8.Text = "TRUE";
                textBox8.BackColor = Color.Green;
                textBox8.Show();
            }

            var bottomBound = occurences.Keys.First().Item1;
            var upperBound = occurences.Keys.Last().Item2;
            for (var i = bottomBound; i < upperBound; i++)
            {
                chart1.Series[0].Points.AddXY(i, occurences[(i, i + 1)] / (float)experimentsCount);
            }
        }

        private void ClearHistogram()
        {
            chart1.Series["Series1"].Points.Clear();
        }
    }
}
