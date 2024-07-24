using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Courses.WinAppsOnCSharp.Lab1Exercise1RectangularForm
{
    public partial class nForm : Form
    {
        private bool nclose = false;
        public new void Close()
        { 
            nclose = true;
            base.Close();
        }
        private void CloseButton1_Click(object sender, EventArgs e)
        {
            Close();
        }
        public nForm()
        {
            InitializeComponent();
            //this.FormClosing += nForm_FormClosing; //p.27 - do not work without this // added via the form events
        }
        private void nForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (nclose) return;
            //if (checkBox1.Checked) return; //p.29 changed on "nclose"
            e.Cancel = true;
            Hide();
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
        }        
    }
}
