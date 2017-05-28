﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace SubjectsSchedule
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        #region Enable_komande

        private bool _enable;

        public bool MenuEnabled
        {
            get
            {
                return _enable;
            }
            set
            {
                if (_enable != value)
                {
                    _enable = value;
                    OnPropertyChanged("MenuEnabled");
                }
            }
        }
        #endregion

        public MainWindow()
        {
            InitializeComponent();

            //newClassroom = new FormaClassroom.FormaClassroom();
            
            this.DataContext = this;
        }

        private void HelloWorld_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void HelloWorld_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Hello world!");
        }

        private void Enable_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Enable_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            // pogledati MenuItem za komandu "RoutedCommands.Enable"
        }

        private void Komanda_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Komanda!");
        }

        private void Komanda_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = MenuEnabled;
        }

        private void Ugradjene_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var w = new MyCommands.UgradjeneKomande();
            w.ShowDialog();
        }

        private void Ugradjene_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        #region Menu item click actions

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            //var w = new Grafika.CustomRender();
            //w.ShowDialog();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            //var w = new Table.TableExample();
            //w.ShowDialog();
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            //var w = new Grafika3D.Kocka();
            //w.ShowDialog();
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            // prije bilo reakcija za Stilovi -> Stilovi i triggeri
            //var w = new Stil.StilPrimer();
            //w.ShowDialog();

            // stara forma za unos studenta - radi Help
            var w = new StudentHelpOld.StudentHelpOld();
            w.ShowDialog();
        }

        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {
            //var w = new DDrop.DragDropWindow();
            //w.ShowDialog();
        }

        private void MenuItem_Click_5(object sender, RoutedEventArgs e)
        {
            //var w = new Kontrole.ToolbarTreeContext();
            //w.ShowDialog();
        }
        #endregion

        /// <summary>
        /// Help command event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            IInputElement focusedControl = FocusManager.GetFocusedElement(Application.Current.Windows[0]);
            // da radi za sve kontrole
            // DependencyObject focusedControl = FocusManager.GetFocusScope(Application.Current.Windows[0]);
            if (focusedControl is DependencyObject)
            {
                Console.WriteLine("focusedControl je DependencyObject");
                string str = HelpProvider.GetHelpKey((DependencyObject)focusedControl);
                HelpProvider.ShowHelp(str, this);
            }
        }

        /// <summary>
        /// Dodatak za help - sprega između C# i JavaScripta (?)
        /// </summary>
        /// <param name="param"></param>
        public void doThings(string param)
        {
            //btnOK.Background = new SolidColorBrush(Color.FromRgb(32, 64, 128));
            Title = param;
        }

        private void New_Classroom_Click(object sender, RoutedEventArgs e) { }

        private void New_Subject_Click(object sender, RoutedEventArgs e) { }

        private void New_FoS_Click(object sender, RoutedEventArgs e) { }

        private void New_Software_Click(object sender, RoutedEventArgs e) { }

        private void NovaUcionica_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        /// <summary>
        /// Sakriva sve (četiri) forme za dodavanje entiteta.
        /// </summary>
        private void HideAllForms()
        {
            ClassroomForma.Visibility = Visibility.Collapsed;
            SubjectForma.Visibility = Visibility.Collapsed;
            SoftverForma.Visibility = Visibility.Collapsed;
            FieldOfStudyForma.Visibility = Visibility.Collapsed;
        }

        private void NovaUcionica_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            HideAllForms();
            ClassroomForma.Visibility = Visibility.Visible;
        }

        private void NoviPredmet_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void NoviPredmet_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            HideAllForms();
            SubjectForma.Visibility = Visibility.Visible;
        }

        private void NoviSmjer_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void NoviSmjer_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            HideAllForms();
            FieldOfStudyForma.Visibility = Visibility.Visible;
        }

        private void NoviSoftver_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void NoviSoftver_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            HideAllForms();
            SoftverForma.Visibility = Visibility.Visible;
        }

        private void PregledSheme_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void PregledSheme_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            HideAllForms();
            ShemaRasporeda.Visibility = Visibility.Visible;
        }
    }
}