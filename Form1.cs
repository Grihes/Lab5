using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu;
using Emgu.CV;
using Emgu.Util;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace Lab5
{

    public partial class Form1 : Form
    {
        private Image<Bgr, byte> inputImage = null;


        public Form1()
        {
            InitializeComponent();
        }


        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();
            inputImage = new Image<Bgr, byte>(openFileDialog1.FileName);
            pictureBox1.Image = inputImage.Bitmap;
        }

        private void найтиКонтурыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Image<Gray, byte> outputImage = inputImage.Mul(1).SmoothGaussian(1).Convert<Gray, byte>().ThresholdBinaryInv(new Gray(50), new Gray(255));
            VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
            Mat hieratchy = new Mat();
            Image<Gray, byte> blackBackground = new Image<Gray, byte>(inputImage.Width, inputImage.Height, new Gray(0));
            CvInvoke.FindContours(outputImage, contours, hieratchy, Emgu.CV.CvEnum.RetrType.Tree, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxNone);
            int n = 0;

            double generalSquare = 0;
            double imageSquare = 125000;
            Random rnd = new Random();
            int m = 10;
            double[] LakeSquares = new double[m];
            
            double square = double.MinValue;
            for (int i = 0; i < contours.Size; i++)
            {
                if (CvInvoke.ContourArea(contours[i]) > square)
                {
                    square = CvInvoke.ContourArea(contours[i]);
                    n = i;
                }
            }
            CvInvoke.DrawContours(blackBackground, contours,n, new MCvScalar(255, 0, 0));
            pictureBox2.Image = blackBackground.Bitmap;
            int count = 0;
            int number = 170000;
            for (int k = 0; k < m; k++)
                {
                 count = 0;
                 for (int j = 0; j < number; j++)
                  {
                    if (CvInvoke.PointPolygonTest(contours[n], new PointF(rnd.Next(inputImage.Width), rnd.Next(inputImage.Height)), false) > 0)
                    count++;
                  }
                  LakeSquares[k] = imageSquare * count / number;
                  generalSquare += LakeSquares[k];
                }
             double LakeSquare = generalSquare / m;
             textBox1.Text = "Square of Lake is: " + LakeSquare.ToString();
            double sigma = 0;
            double sumSquare=0;
            for (int i = 0; i < m; i++)
                sumSquare += LakeSquares[i] * LakeSquares[i];
            sigma = Math.Sqrt(sumSquare / m - LakeSquare * LakeSquare);
            textBox2.Text = "Sigma is: "+ sigma.ToString();
        
            }

        }
    }

