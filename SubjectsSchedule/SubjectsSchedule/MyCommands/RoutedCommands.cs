using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace SubjectsSchedule.MyCommands
{
    public static class RoutedCommands
    {
        public static readonly RoutedUICommand HelloWorld = new RoutedUICommand(
            "Hello World",
            "HelloWorld",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.H, ModifierKeys.Control),
                new KeyGesture(Key.H, ModifierKeys.Alt | ModifierKeys.Control)
            }
            );

        public static readonly RoutedUICommand Enable = new RoutedUICommand(
            "Enable",
            "Enable",
            typeof(RoutedCommands)
            );

        public static readonly RoutedUICommand Komanda = new RoutedUICommand(
            "Komanda",
            "Komanda",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.K, ModifierKeys.Control),
            }
            );

        public static readonly RoutedUICommand Ugradjene = new RoutedUICommand(
            "Ugradjene Komande",
            "UgradjeneKomande",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.U, ModifierKeys.Control),
            }
            );

        public static readonly RoutedUICommand NovaUcionica = new RoutedUICommand(
            "Nova ucionica",
            "NovaUcionica",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.U, ModifierKeys.Shift | ModifierKeys.Alt),
            }
            );

        public static readonly RoutedUICommand NoviPredmet = new RoutedUICommand(
            "Novi Predmet",
            "NoviPredmet",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.P, ModifierKeys.Shift | ModifierKeys.Alt),
            }
            );

        public static readonly RoutedUICommand NoviSmjer = new RoutedUICommand(
            "Novi Smjer",
            "NoviSmjer",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.S, ModifierKeys.Shift | ModifierKeys.Alt),
            }
            );

        public static readonly RoutedUICommand NoviSoftver = new RoutedUICommand(
            "Novi Softver",
            "NoviSoftver",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.O, ModifierKeys.Shift | ModifierKeys.Alt),
            }
            );
        public static readonly RoutedUICommand PregledSheme = new RoutedUICommand(
            "Pregled Sheme",
            "PregledSheme",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.R, ModifierKeys.Shift | ModifierKeys.Alt),
            }
            );
        public static readonly RoutedUICommand AbortDemo = new RoutedUICommand(
            "Abort Demo",
            "AbortDemo",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.D, ModifierKeys.Control),
            }
            );
    }
}
