using Dropbox.Api;
using Dropbox.Api.Files;
using ProjetOCR.BEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private string AppKey = "e5Ttvf8JMQAAAAAAAAABF_IHcyKUIGTrzAzkqqdlaCUzjmyBdTdayLHTqBfnnbfM";
        private string strAccessToken = string.Empty;
        private string strAuthenticationURL = string.Empty;
        private string NameApp = string.Empty;
        DossierOCR OCR = new DossierOCR();
        private DropBoxBase DBB;
        public MainWindow()
        {
            InitializeComponent();
            gbDropBox.IsEnabled = true;
        }
        public MainWindow(DossierOCR _DossierOCR)
        {
            InitializeComponent();
            OCR = _DossierOCR;
            gbDropBox.IsEnabled = true;
            
        }
        private void btnCreateFolder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DBB = new DropBoxBase(OCR.Login, "PTM_Centralized");
                if (DBB != null)
                {
                    if (OCR.Login != null)
                    {
                        if (DBB.FolderExists("/Dropbox/DotNetApi/app1") == false)
                        {
                            DBB.CreateFolder("/Dropbox/DotNetApi/app1", OCR.Login, OCR.MotDePass);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex = ex.InnerException ?? ex;
            }
        }
        private void btlUpload_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DBB = new DropBoxBase(OCR.Login, "PTM_Centralized");
                if (DBB != null)
                {
                    if (OCR.Login != null)
                    {
                        DBB.Upload("/Dropbox/DotNetApi", "test3.zip", @"E:\mohammed\test3.zip", OCR.Login, OCR.MotDePass);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        private void btnDownload_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DBB = new DropBoxBase(OCR.Login, "PTM_Centralized");
                if (DBB != null)
                {
                    if (OCR.Login != null)
                    {
                        DBB.Download("/Dropbox/DotNetApi", "tt.txt", @"E:\dropboxtest\", "tt.txt", OCR.Login, OCR.MotDePass);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                DBB = new DropBoxBase(OCR.Login, "PTM_Centralized");
                if (DBB != null)
                {
                    if (OCR.Login != null)
                    {
                        DBB.Deletee("/Dropbox/DotNetApi/test22.zip", OCR.Chemin, OCR.MotDePass);
                    }
                }
            }
            catch (Exception )
            {
                
                throw;
            }
        }
        private void btnListfile_Click(object sender, RoutedEventArgs e)
        {
            //    RaderFiles GetFiles = new RaderFiles();
            //    List<string> Listfolder = new List<string>();
            //    try
            //    {
            //        DBB = new DropBoxBase(OCR.Login, "PTM_Centralized");
            //        if (DBB != null)
            //        {
            //            if (OCR.Login != null)
            //            {
            //                 Listfolder = GetFiles.ListFileFromDropBox();
            //                //ListFileView ListFolder = new ListFileView(Listfolder);
            //                ListFolder.ShowDialog();
            //            }
            //        }
            //    }
            //    catch (Exception ex)
            //    {

            //        throw;
        }
    }
}

