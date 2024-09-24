using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace GeneraFiEFWPF
{
    /// <summary>
    /// Logique d'interaction pour Window1.xaml
    /// </summary>
    public partial class WindowsPopUp : ResourceDictionary
    {
         System.Windows.Media.Animation.Storyboard closeStoryBoard = null;
        Window window;
        public WindowsPopUp()
        {
            InitializeComponent();
        }
        private void PART_CLOSE_Click(object sender, RoutedEventArgs e)
        {
            window = (Window)((FrameworkElement)sender).TemplatedParent;
           //var screen = System.Windows.Forms.Screen.PrimaryScreen.Bounds;
            //var height = screen.Height;
            var fade = new DoubleAnimation()
            {
                From = window.Top,
                //To = height,
                Duration = TimeSpan.FromSeconds(0.2),
            };
            Storyboard.SetTarget(fade, window);
            Storyboard.SetTargetProperty(fade, new PropertyPath(Window.TopProperty));
            closeStoryBoard = new Storyboard();
            closeStoryBoard.Children.Add(fade);
            closeStoryBoard.Completed += closeStoryBoard_Completed;
            closeStoryBoard.Begin();
        }
        private void closeStoryBoard_Completed(object sender, EventArgs e)
        {
            window.Close();
        }
        private void MoveWindow(object sender, MouseButtonEventArgs e)
        {
            var window = (Window)((FrameworkElement)sender).TemplatedParent;

            // Check if the control have been double clicked.
            if (e.ClickCount == 2)
            {
                // If double clicked then maximize the window.
            }
            else
            {
                // If not double clicked then just drag the window around.
                window.DragMove();
            }
        }

        private void OnBorderMouseMove(object sender, MouseEventArgs e)
        {
            var window = (Window)((FrameworkElement)sender).TemplatedParent;

            if (window != null)
            {
                if (e.LeftButton == MouseButtonState.Pressed && window.WindowState == WindowState.Maximized)
                {
                    Size maxSize = new Size(window.ActualWidth, window.ActualHeight);
                    Size resSize = window.RestoreBounds.Size;

                    double curX = e.GetPosition(window).X;
                    double curY = e.GetPosition(window).Y;

                    double newX = curX / maxSize.Width * resSize.Width;
                    double newY = curY;

                    window.WindowState = WindowState.Normal;

                    window.Left = curX - newX;
                    window.Top = curY - newY;
                    window.DragMove();
                }
            }
        }

        private void PART_CLOSE_Max(object sender, RoutedEventArgs e)
        {
            var window = (Window)((FrameworkElement)sender).TemplatedParent;
            if (window.WindowState == WindowState.Maximized)
            {
                window.WindowState = WindowState.Normal;
            }
            else
            {
                window.WindowState = WindowState.Maximized;
            }
        }
    }
}
