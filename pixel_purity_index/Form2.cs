using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pixel_purity_index
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        List<External> originColor = new List<External>();
        public List<External> histData
        {
            set
            {
                originColor = value;
            }
        }
        public void SetDiagram()
        {
            //this.chart1
        }  
    }
}
