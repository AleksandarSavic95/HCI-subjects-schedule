using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace SubjectsSchedule.MyCommands
{
    public static class CustomCommands
    {
        public static readonly ICommand testKomanda = new TestKomanda();
    }

    // da hocemo da radimo i sa precicama, trebalo bi da nasledimo RoutedUICommand, recimo
    public class TestKomanda : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            //ovde ide model-level obrada komande,
            //tj. dio koji ne radi sa interfejsom
            Console.WriteLine("Test");
            Console.Beep();
        }
    }
}
