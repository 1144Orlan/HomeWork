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
    public partial class ParentForm : Form
    {
        private int openDocuments = 0;

        nForm myF2;
        public ParentForm()
        {
            InitializeComponent();
            myF2 = new nForm(); //moved back to event of button4
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.Sizable;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Size = new Size(300, 500);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Opacity = 1;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try 
            { 
            //nForm myF2 = new nForm(); //at first moved to body and constructor Form1, for single window
            //myF2 = new nForm(); //create new form
            myF2.StartPosition = FormStartPosition.Manual;
            myF2.Location = new Point(this.Location.X + this.Width, this.Location.Y);
            myF2.Show(); //moved to try                        
            myF2.Activate();
            }
            catch (ObjectDisposedException ex) 
            {
                myF2 = new nForm();
                myF2.Text = "Повторное создание формы";
                myF2.Show();
                myF2.Activate();
            }
        }

        private void ExitMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void WindowCascadeMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi (System.Windows.Forms.MdiLayout.Cascade);
        }

        private void WindowTileMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(System.Windows.Forms.MdiLayout.TileHorizontal);
        }

        private void NewMenuItem_Click(object sender, EventArgs e)
        {
            ChildForm newChild = new ChildForm();
            newChild.MdiParent = this;
            newChild.Text = newChild.Text + " " + ++openDocuments;
            newChild.Show();
        }
    }
}
