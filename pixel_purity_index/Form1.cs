using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace pixel_purity_index
{
    public struct External
    {
        public Color pixel { set; get; }
        public int NPPI { set; get; }
    }
    public struct rndCluster
    {
        public Vector rndColor { set; get; }
        public Color minColor { set; get; }
        public Color maxColor { set; get; }
    }
    public struct Vector
    {
        public double v1 { set; get; }
        public double v2 { set; get; }
        public double v3 { set; get; }
    }
    public partial class Form1 : Form
    {
        OpenFileDialog file = new OpenFileDialog();
        Random rnd = new Random(Guid.NewGuid().GetHashCode());
        Bitmap myPic;
        List<External> originColor = new List<External>();
        public Form1()
        {
            InitializeComponent();
            Form.CheckForIllegalCrossThreadCalls = false;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            file.Filter = "圖片檔|*.jpg;*.png";
            file.ShowDialog();
            if (file.FileName != "")
            {
                originColor.Clear();
                myPic = new Bitmap(file.FileName.ToString());
                pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;
                pictureBox1.BackgroundImage = myPic;

                //取圖片像素
                int width = myPic.Width;
                int height = myPic.Height;
                for (int i = 1; i < width; i++)
                {
                    for (int j = 1; j < height; j++)
                    {
                        Color temp = myPic.GetPixel(i, j);
                        originColor.Add(new External { pixel = temp, NPPI = 0 });
                    }
                }
                button2.Enabled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && IsNumeric(textBox1.Text))
            {
                pictureBox2.BackgroundImage = null; 
                listBox1.Items.Clear();
                button2.Enabled = false;
                int K = int.Parse(textBox1.Text);
                ThreadStart starter = () => Work(originColor, K);
                Thread doWork = new Thread(starter);
                doWork.Start();
            }
        }

        private void Work(List<External> originColor, int K)
        {
            listBox1.Items.Add("第一");
            listBox1.TopIndex = listBox1.Items.Count - 1;
            for (int i = 0; i < originColor.Count; i++)
            {
                External temp = originColor[i];
                temp.NPPI = 0;
                originColor[i] = temp;
            }
            List<rndCluster> rndColor = new List<rndCluster>();
            for (int i = 0; i < K; i++)
            {
                Vector temp = new Vector();
                temp.v1 = rnd.Next(0, 255);
                temp.v2 = rnd.Next(0, 255);
                temp.v3 = rnd.Next(0, 255);
                double length = Math.Sqrt(Math.Pow(temp.v1, 2) + Math.Pow(temp.v2, 2) + Math.Pow(temp.v3, 2));
                temp.v1 /= length;
                temp.v2 /= length;
                temp.v3 /= length;
                rndColor.Add(new rndCluster { rndColor = temp, minColor = Color.White, maxColor = Color.Black });
            }

            listBox1.Items.Add("第二");
            listBox1.TopIndex = listBox1.Items.Count - 1;
            for (int i = 0; i < K; i++)
            {
                double[] dis = new double[originColor.Count];
                for (int j = 0; j < originColor.Count; j++)
                {
                    dis[j] = calcLength(originColor[j].pixel, rndColor[i].rndColor);
                }
                int minIndex = Array.IndexOf(dis, dis.Min());
                int maxIndex = Array.IndexOf(dis, dis.Max());
                rndCluster temp = rndColor[i];
                temp.minColor = originColor[minIndex].pixel;
                temp.maxColor = originColor[maxIndex].pixel;
                rndColor[i] = temp;
            }

            listBox1.Items.Add("第三");
            listBox1.TopIndex = listBox1.Items.Count - 1;
            for (int i = 0; i < originColor.Count; i++)
            {
                for (int j = 0; j < K; j++)
                {
                    if(compareEqu(originColor[i].pixel,rndColor[j].maxColor) ||
                        compareEqu(originColor[i].pixel,rndColor[j].maxColor))
                    {
                        External temp = originColor[i];
                        temp.NPPI++;
                        originColor[i] = temp;
                    }
                }
            }

            listBox1.Items.Add("畫圖");
            listBox1.TopIndex = listBox1.Items.Count - 1;
            int width = myPic.Width;
            int height = myPic.Height;
            Bitmap tempBitmap = new Bitmap(width, height);
            int count = 0;
            for (int i = 1; i < width; i++)
            {
                for (int j = 1; j < height; j++)
                {
                    if (originColor[count].NPPI > 0)
                    {
                        tempBitmap.SetPixel(i, j, Color.Black);
                    }
                    else {
                        tempBitmap.SetPixel(i, j, Color.White);
                    }
                    count++;
                }
            }
            pictureBox2.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox2.BackgroundImage = tempBitmap;
            listBox1.Items.Add("完成");
            listBox1.TopIndex = listBox1.Items.Count - 1;
            button2.Enabled = true;
            Form2 lForm = new Form2();
            lForm.histData = originColor;  
            lForm.SetDiagram();
            lForm.ShowDialog();
        }

        public double calcLength(Color A, Vector B)
        {
            return ((A.R * B.v1) + (A.G * B.v2) + (A.B * B.v3)) / Math.Sqrt(Math.Pow(A.R, 2) + Math.Pow(A.G, 2) + Math.Pow(A.B, 2));
        }

        public bool compareEqu(Color c1,Color c2)
        {
            if (c1.R == c2.R && c1.G == c2.G && c1.B == c2.B)
                return true;
            else
                return false;
        }

        private static bool IsNumeric(string TextBoxValue)
        {
            try
            {
                int i = Convert.ToInt32(TextBoxValue);
                return true;
            }
            catch
            {
                try
                {
                    double i = Convert.ToDouble(TextBoxValue);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
    }
}