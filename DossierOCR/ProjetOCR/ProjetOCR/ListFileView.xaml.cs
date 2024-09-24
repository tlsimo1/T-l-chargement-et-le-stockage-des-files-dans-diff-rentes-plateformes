using Google.Apis.Drive.v3.Data;
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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ProjetOCR
{
    /// <summary>
    /// Logique d'interaction pour ListFileView.xaml
    /// </summary>
    public partial class ListFileView : Window
    {
        public ListFileView(FileList listFile)
        {
            InitializeComponent();

            LstFolder.ItemsSource = listFile.Files;
        }
    }
}
