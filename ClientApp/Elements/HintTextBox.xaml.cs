﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClientApp.Elements
{
    /// <summary>
    /// Interaction logic for HintTextBox.xaml
    /// </summary>
    public partial class HintTextBox : Grid
    {
        public string Hint { set { promptBlock.Text = value; } }
        public string Text
        {
            get { return txtUserEntry.Text; }
            set {
                txtUserEntry.Text = value;
                promptBlock.Visibility = (txtUserEntry.Text.Length == 0) ? Visibility.Visible : Visibility.Hidden;
            }
        }
        public bool Optional { get; set; }
        public HintTextBox()
        {
            InitializeComponent();
        }



        private void TxtUserEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            promptBlock.Visibility = (txtUserEntry.Text.Length == 0) ? Visibility.Visible : Visibility.Hidden;

        }
    }
}
