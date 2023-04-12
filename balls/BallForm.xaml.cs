﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace balls
{
    /// <summary>
    /// Interaction logic for BallForm.xaml
    /// </summary>
    public partial class BallForm : Window
    {
        Thread FirstThread = null; 
        Thread SecondThread = null;

        Color[,] colMat = new Color[MainWindow.BoxWidth, MainWindow.BoxHeigth];

        int colorCount = 4;

        public BallForm()
        {
            InitializeComponent();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            for(var row = 0; row< MainWindow.BoxWidth; row++)
            {
                for (var col =0;col< MainWindow.BoxHeigth;col++)
                {
                    colMat[row, col] = Color.White;
                }
            }
        }
    }
}
