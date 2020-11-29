using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OxyPlot;
using OxyPlot.Series;
using System.Numerics;
using DFT2;

namespace SpectralDerivative
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }

        private void buttonTest_Click(object sender, EventArgs e)
        {
            double x_lb = 0;
            double x_ub = 6;

            double L = x_ub - x_lb;

            int N = 1024;

            Complex[] fvalues = new Complex[N];

            double stepSize = L / N;
            double x = 0;
            int i;
            
            for(i = 0; x <= 1; i++)
            {
                fvalues[i] = new Complex(0,0);
                x += stepSize;
            }
            for(; x <= 3; i++)
            {
                fvalues[i] = new Complex(x - 1, 0);
                x += stepSize;
            }
            for (; x <= 5; i++)
            {
                fvalues[i] = new Complex(-x + 5, 0);
                x += stepSize;
            }
            for (; x < 6; i++)
            {
                fvalues[i] = new Complex(0, 0);
                x += stepSize;
            }

            var myModel = new PlotModel { Title = "Example 1" };
            LineSeries series = new LineSeries();
            for(i = 0; i < fvalues.Length; i++)
            {
                series.Points.Add(new DataPoint(i*stepSize, fvalues[i].Real));
            }
            
            myModel.Series.Add(series);
            this.plotView1.Model = myModel;

            //derivative calculation
            var fhat = FFT.Compute(fvalues);
            Complex[] dfhat = new Complex[fhat.Length];
            double[] kappa = new double[fhat.Length];
            Complex imaj = new Complex(0, 1);
            for(i = 0; i < kappa.Length; i++)
            {
                kappa[i] = (2 * Math.PI / L) * ((-N / 2.0) + i);
            }

            kappa = FFT_Utility<double>.Shift(kappa, (kappa.Length / 2) + 1);
            for(i = 0; i < dfhat.Length; i++)
            {
                dfhat[i] = imaj * kappa[i] * fhat[i];
            }

            var dfFFT = IFFT.Compute(dfhat);

            LineSeries dfseries = new LineSeries();
            for (i = 0; i < fvalues.Length; i++)
            {
                dfseries.Points.Add(new DataPoint(i * stepSize, dfFFT[i].Real));
            }
            myModel.Series.Add(dfseries);
        }
    }
}
