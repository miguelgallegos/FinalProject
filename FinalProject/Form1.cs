using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FinalProject
{
    public partial class Form1 : Form
    {
 
        public Form1()
        {
            InitializeComponent();

        }


        protected override void OnClosing(CancelEventArgs e)
        {
        }



    }
}

///stop threads or put to wait using wait handle? - waitAll waitAny?