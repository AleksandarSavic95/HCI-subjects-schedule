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

namespace SubjectsSchedule.Forme
{
    /// <summary>
    /// Interaction logic for FormaFieldOfStudy.xaml
    /// </summary>
    public partial class FormaFieldOfStudy : UserControl
    {
        public FormaFieldOfStudy()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("aco");
            Console.WriteLine(Naziv_Input.Text);
            Console.WriteLine(Opis_Input.Text);
            Console.WriteLine(Datum_Input.Text);
            Console.WriteLine(Dugme_Button.ClickMode);
            Console.WriteLine("aco");
        }
    }
}
