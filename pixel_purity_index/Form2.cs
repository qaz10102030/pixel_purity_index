using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace pixel_purity_index
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        List<External> originColor = new List<External>();
        int K = 0;
        public List<External> histData
        {
            set
            {
                originColor = value;
            }
        }
        public int setK
        {
            set
            {
                K = value;
            }
        }
        public void SetDiagram()
        {
            int[] count = new int[2 * K];
            string[] xValue = new string[2 * K];
            for (int i = 0; i < originColor.Count; i++)
            {
                count[originColor[i].NPPI]++;
                xValue[originColor[i].NPPI] = "K=" + originColor[i].NPPI;
            }
            List<int> temp = new List<int>();
            List<string> tempS = new List<string>();
            for (int i = 0; i < count.Length; i++)
            {
                if (count[i] > 0)
                {
                    temp.Add(count[i]);
                    tempS.Add(xValue[i]);
                }
            }
            Series _series = new Series();
            for (int i = 0; i < temp.Count; i++)
            {
                _series.Color = Color.BlueViolet;
                _series.ChartType = SeriesChartType.Column;
                _series.IsValueShownAsLabel = true;
                _series.Points.AddY(temp[i]);
            }
            _series.BorderWidth = 1;
            _series.Points.DataBindXY(tempS, temp);
            chart1.Series.Add(_series);
            chart1.ChartAreas[0].AxisX.MajorGrid.LineWidth = 0;
            chart1.ChartAreas[0].AxisY.MajorGrid.LineWidth = 0;
        }  
    }
}
