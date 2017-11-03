using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;




namespace AccountDemo
{
    public partial class FormCloese : Form
    {


        public bool ifright = true;
        public bool isright =false ;
      
        public FormCloese()
        {
            InitializeComponent();
        }
      
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            isright = true;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (ISExit.Checked == true)
            {
               this.Close();
                ifright =false ;
            }
            if (ISNOExit.Checked == true)
            { 
                this.Close();
                ifright = true; 
               
            }
        }
       
    }
}
