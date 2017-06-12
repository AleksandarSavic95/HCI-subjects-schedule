using System;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls.Primitives;
//using Microsoft.Test;
using System.Threading;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;

namespace SubjectsSchedule.InputHelper
{
    public static class InputHelper
    {
        /// <summary>
        /// Check to see if the mouse click point is within wpf client area.
        /// </summary>
        /// <param name="window">wpf window.</param>
        /// <param name="clickPoint">wpf logical pixel.</param>
        /// <returns>
        /// Returns true if the point is within the wpf client area.
        /// Returns false if the point is not within the wpf client area.
        /// </returns>
        public static bool IsMouseClickWithinClientArea(Window window, Point clickPoint)
        {
            FlowDirection windowFlowDirection = window.FlowDirection;
            switch (windowFlowDirection)
            {
                case FlowDirection.LeftToRight:
                    if (clickPoint.X < window.PointToScreen(new Point()).X || clickPoint.X > window.PointToScreen(new Point()).X + window.ActualWidth || clickPoint.Y < window.PointToScreen(new Point()).Y || clickPoint.Y > window.PointToScreen(new Point()).Y + window.ActualHeight)
                    {
                        return false;
                    }
                    break;
                case FlowDirection.RightToLeft:
                    if (clickPoint.X < window.PointToScreen(new Point()).X || clickPoint.X < window.PointToScreen(new Point()).X - window.ActualWidth || clickPoint.Y < window.PointToScreen(new Point()).Y || clickPoint.Y > window.PointToScreen(new Point()).Y + window.ActualHeight)
                    {
                        return false;
                    }
                    break;
            }

            return true;
        }

        private static void HelpMoveMouse(System.Drawing.Point mousePoint, System.Drawing.Point componentPoint, 
            FrameworkElement targetElement, Dispatcher dispatcher)
        {
            Console.WriteLine("T A C K E  sleep 0.5 s");
            Thread.Sleep(500);

            bool mouseXSmaller = false;
            bool mouseYSmaller = false;
            if (mousePoint.X < componentPoint.X) mouseXSmaller = true;
            if (mousePoint.Y < componentPoint.Y) mouseYSmaller = true;
            bool xReached = false;
            bool yReached = false;

            // mis na manjoj poziciji po x i po y
            #region mis x i y manji
            if (mouseXSmaller && mouseYSmaller)
            {
                while (xReached != true)
                {
                    if (mousePoint.X != componentPoint.X) mousePoint.X = mousePoint.X + 1;
                    else xReached = true;
                    //Microsoft.Test.Input.Mouse.MoveTo(mousePoint);
                    Thread.Sleep(3);
                    //Console.WriteLine("mousePoint X: " + mousePoint.X + " mousePoint Y: " + mousePoint.Y);
                    //Console.WriteLine("componentP X: " + componentPoint.X + " componentP Y: " + componentPoint.Y);
                }
                while (yReached != true)
                {
                    if (mousePoint.Y != componentPoint.Y) mousePoint.Y = mousePoint.Y + 1;
                    else yReached = true;
                    //Microsoft.Test.Input.Mouse.MoveTo(mousePoint);
                    Thread.Sleep(3);
                }
            }
            #endregion

            #region mis x manji, mis y NIJE manji
            if (mouseXSmaller && !mouseYSmaller)
            {
                while (xReached != true)
                {
                    if (mousePoint.X != componentPoint.X) mousePoint.X = mousePoint.X + 1;
                    else xReached = true;
                    //Microsoft.Test.Input.Mouse.MoveTo(mousePoint);
                    Thread.Sleep(3);
                }
                while (yReached != true)
                {
                    if (mousePoint.Y != componentPoint.Y) mousePoint.Y = mousePoint.Y - 1;
                    else yReached = true;
                    //Microsoft.Test.Input.Mouse.MoveTo(mousePoint);
                    Thread.Sleep(3);
                }
            }
            #endregion

            #region mis x NIJE manji, mis y manji
            if (!mouseXSmaller && mouseYSmaller)
            {
                while (xReached != true)
                {
                    if (mousePoint.X != componentPoint.X) mousePoint.X = mousePoint.X - 1;
                    else xReached = true;
                    //Microsoft.Test.Input.Mouse.MoveTo(mousePoint);
                    Thread.Sleep(3);
                }
                while (yReached != true)
                {
                    if (mousePoint.Y != componentPoint.Y) mousePoint.Y = mousePoint.Y + 1;
                    else yReached = true;
                    //Microsoft.Test.Input.Mouse.MoveTo(mousePoint);
                    Thread.Sleep(3);
                }
            }
            #endregion

            #region  mis x NIJE manji, mis y NIJE manji
            if (!mouseXSmaller && !mouseYSmaller)
            {
                while (xReached != true)
                {
                    if (mousePoint.X != componentPoint.X) mousePoint.X = mousePoint.X - 1;
                    else xReached = true;
                    //Microsoft.Test.Input.Mouse.MoveTo(mousePoint);
                    Thread.Sleep(3);
                }
                while (yReached != true)
                {
                    if (mousePoint.Y != componentPoint.Y) mousePoint.Y = mousePoint.Y - 1;
                    else yReached = true;
                    //Microsoft.Test.Input.Mouse.MoveTo(mousePoint);
                    Thread.Sleep(3);
                }
            }
            #endregion

            dispatcher.Invoke(() =>
            {
                ButtonAutomationPeer peer =
                new ButtonAutomationPeer((System.Windows.Controls.Button)targetElement);
                IInvokeProvider invokeProv = peer.GetPattern(PatternInterface.Invoke)
                    as IInvokeProvider;

                peer.SetFocus();
                invokeProv.Invoke();
            });
        }

        public static void MoveMouse(Dispatcher dispatcher, FrameworkElement targetElement)
        {
            Point elementPointToScreen = targetElement.PointToScreen(new Point());
            Point elementMiddlePointToScreen = default(Point);

            FlowDirection windowFlowDirection = Window.GetWindow(targetElement).FlowDirection;

            // We need to convert element width and height logical pixel to screen pixel.
            double convertedHalfWidth = DpiHelper.ConvertToPhysicalPixel(targetElement.ActualWidth / 2);
            double convertedhalfHeight = DpiHelper.ConvertToPhysicalPixel(targetElement.ActualHeight / 2);

            // We are handling the LTR and RTL flowdirection scenarios.
            switch (windowFlowDirection)
            {
                case FlowDirection.LeftToRight:
                    elementMiddlePointToScreen = new Point(elementPointToScreen.X + convertedHalfWidth, elementPointToScreen.Y + convertedhalfHeight);
                    break;
                case FlowDirection.RightToLeft:
                    elementMiddlePointToScreen = new Point(elementPointToScreen.X - convertedHalfWidth, elementPointToScreen.Y + convertedhalfHeight);
                    break;
            }

            System.Drawing.Point elementCenterPointToScreen = new System.Drawing.Point
                (Convert.ToInt32(elementMiddlePointToScreen.X), Convert.ToInt32(elementMiddlePointToScreen.Y));
            System.Drawing.Point mouseStartPoint = System.Windows.Forms.Control.MousePosition;

            HelpMoveMouse(mouseStartPoint, elementCenterPointToScreen, targetElement, dispatcher);
        }

        /// <summary>
        /// Mouse move to center of element
        /// </summary>
        /// <param name="targetElement">A FrameworkElement reference</param>
        public static void MouseMoveToElementCenter(FrameworkElement targetElement) //, Dispatcher dispatcher
        {
            Point elementPointToScreen = targetElement.PointToScreen(new Point());
            Point elementMiddlePointToScreen = default(Point);

            FlowDirection windowFlowDirection = Window.GetWindow(targetElement).FlowDirection;

            // We need to convert element width and height logical pixel to screen pixel.
            double convertedHalfWidth = DpiHelper.ConvertToPhysicalPixel(targetElement.ActualWidth / 2);
            double convertedhalfHeight = DpiHelper.ConvertToPhysicalPixel(targetElement.ActualHeight / 2);

            // We are handling the LTR and RTL flowdirection scenarios.
            switch (windowFlowDirection)
            {
                case FlowDirection.LeftToRight:
                    elementMiddlePointToScreen = new Point(elementPointToScreen.X + convertedHalfWidth, elementPointToScreen.Y + convertedhalfHeight);
                    break;
                case FlowDirection.RightToLeft:
                    elementMiddlePointToScreen = new Point(elementPointToScreen.X - convertedHalfWidth, elementPointToScreen.Y + convertedhalfHeight);
                    break;
            }

            System.Drawing.Point elementCenterPointToScreen = new System.Drawing.Point
                (Convert.ToInt32(elementMiddlePointToScreen.X), Convert.ToInt32(elementMiddlePointToScreen.Y));
            System.Drawing.Point mouseStartPoint = System.Windows.Forms.Control.MousePosition;

            //dispatcher.Invoke(() => HelpMoveMouse(mouseStartPoint, elementCenterPointToScreen, targetElement));
            //Thread t = new Thread( () => HelpMoveMouse(mouseStartPoint, elementCenterPointToScreen, targetElement));
            //t.Start();

        }

        /// <summary>
        /// Mouse click on center of Button
        /// </summary>
        /// <param name="targetElement">A ButtonBase reference</param>
        public static void MouseClickButtonCenter(ButtonBase targetElement, ClickMode clickMode, MouseButton mouseButton)
        {
            InputHelper.MouseMoveToElementCenter(targetElement);
            //DispatcherOperations.WaitFor(DispatcherPriority.ApplicationIdle);

            switch (clickMode)
            {
                case ClickMode.Release:
                    //Microsoft.Test.Input.Mouse.Click(mouseButton);
                    break;
                case ClickMode.Press:
                    //Microsoft.Test.Input.Mouse.Down(mouseButton);
                    break;
                case ClickMode.Hover:
                    throw new Exception("Fail: ClickMode.Hover is not supported in the MouseClickCenter method");
            }
            //DispatcherOperations.WaitFor(DispatcherPriority.ApplicationIdle);

            // Cleanup
            if (clickMode == ClickMode.Press)
            {
                //Microsoft.Test.Input.Mouse.Up(mouseButton);
                //DispatcherOperations.WaitFor(DispatcherPriority.ApplicationIdle);
            }
        }

        /// <summary>
        /// Mouse click on center of FrameworkElement
        /// </summary>
        /// <param name="targetElement">A FrameworkElement reference</param>
        public static void MouseClickCenter(FrameworkElement targetElement, MouseButton mouseButton)
        {
            InputHelper.MouseMoveToElementCenter(targetElement);
            //DispatcherOperations.WaitFor(DispatcherPriority.ApplicationIdle);
            //Microsoft.Test.Input.Mouse.Click(mouseButton);
            //DispatcherOperations.WaitFor(DispatcherPriority.ApplicationIdle);
        }

        /// <summary>
        /// Click on the middle of window's chrome to make sure the window has focus because vista window doesn't always actived
        /// </summary>
        /// <param name="window">A window reference</param>
        public static void MouseClickWindowChrome(Window window)
        {
            window.Topmost = true;
            Point point = window.PointToScreen(new Point());
            Point logicalPoint = new Point(point.X + window.ActualWidth / 2, point.Y - 8);
            System.Drawing.Point clickPoint = new System.Drawing.Point(DpiHelper.ConvertToPhysicalPixel(logicalPoint.X), DpiHelper.ConvertToPhysicalPixel(logicalPoint.Y));

            //Microsoft.Test.Input.Mouse.MoveTo(clickPoint);
            //DispatcherOperations.WaitFor(DispatcherPriority.SystemIdle);
            //Microsoft.Test.Input.Mouse.Click(MouseButton.Left);
            //DispatcherOperations.WaitFor(DispatcherPriority.SystemIdle);
        }
    }
}
