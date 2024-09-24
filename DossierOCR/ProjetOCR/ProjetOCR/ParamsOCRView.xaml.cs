using ProjetOCR.BEL;
using ProjetOCR.BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using UCGeneraFi;

namespace ProjetOCR
{
    /// <summary>
    /// Logique d'interaction pour ParamsOCRView.xaml
    /// </summary>
    public partial class ParamsOCRView : Page
    {
        BParamsOCR _BParamsOCR = new BParamsOCR();
        private string NameApp = string.Empty;
        private string strAccessToken = string.Empty;
        private string strAuthenticationURL = string.Empty;
        private string OperationName = string.Empty;
        private string SelectedCombo = string.Empty;
        private string CodeAccessTokenDropBox = "-FmwGyK-VUAAAAAAAAAAFjabIiaFqsCEhg5ZUPEJejxq6EVPmzX3jzH0ppg16UOt";
        private string NomAppDropBox = "GenerafiApiDR";
        private string NomAppGoogleDrive = "GenerafiApi";
        List<string> filename = new List<string>();
        DossierOCR _DossierOCR = new DossierOCR();
        private string Chemin;
        private string Login;
        private string MotDePass;
        private BackgroundWorker backgroundWorkerOCR = new BackgroundWorker();
        PageChargement page;
        public ParamsOCRView()
        {
            InitializeComponent();
            DataContext = _DossierOCR;
            backgroundWorkerOCR.DoWork += BackgroundWorkerOCR_DoWork;
            backgroundWorkerOCR.RunWorkerCompleted += BackgroundWorkerOCR_RunWorkerCompleted;
            backgroundWorkerOCR.WorkerReportsProgress = true;
        }
        private void BackgroundWorkerOCR_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
            }
            else if (e.Error != null)
            {
                GeneraFiMessageBox.Show("Une erreur s'est produite lors de l'execution de cette opération");
            }
            else
            {
                page.Close();
            }
            if (page != null)
                page.Close();
            GeneraFiMessageBox.Show("Enregistrer avec succès");
        }
        private void BackgroundWorkerOCR_DoWork(object sender, DoWorkEventArgs e)
        {
            InsertDossierOCR();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadPage();
            ComboOCR.SelectionChanged += ComboOCR_SelectionChanged;
        }
        private void LoadPage()
        {
            _DossierOCR = _BParamsOCR.GetDossierOCR();
            if (_DossierOCR == null)
            {
                TxtKeyAppDropBox.Visibility = Visibility.Collapsed;
                TxtNomAppDropBox.Visibility = Visibility.Collapsed;
                TxtNameAppGoogleDrive.Visibility = Visibility.Collapsed;
                TxtChemin.Visibility = Visibility.Visible;
                TxtLogin.Visibility = Visibility.Visible;
                TxtMotDePass.Visibility = Visibility.Visible;
                btnCnx.SetValue(Grid.RowProperty, 2);
                btnCnx.SetValue(Grid.ColumnProperty, 3);
                btnCnx.IsEnabled = false;
                OperationName = "DossierPartager";
                EnableButton();
                return;
            }
            if (_DossierOCR.Type_Source == "DossierPartager" || _DossierOCR.Type_Source == "FTP")
            {
                ComboOCR.Text = _DossierOCR.Type_Source;
                TxtChemin.Text = _DossierOCR.Chemin;
                TxtLogin.Text = _DossierOCR.Login;
                TxtMotDePass.Text = _DossierOCR.MotDePass;
                TxtKeyAppDropBox.Visibility = Visibility.Collapsed;
                TxtNomAppDropBox.Visibility = Visibility.Collapsed;
                TxtNameAppGoogleDrive.Visibility = Visibility.Collapsed;
                OperationName = _DossierOCR.Type_Source;
                EnableButton();
            }

            if (_DossierOCR.Type_Source == "DropBox")
            {
                ComboOCR.Text = _DossierOCR.Type_Source;
                TxtKeyAppDropBox.Text = _DossierOCR.Login;
                TxtNomAppDropBox.Text = _DossierOCR.Chemin;
                TxtChemin.Visibility = Visibility.Collapsed;
                TxtLogin.Visibility = Visibility.Collapsed;
                TxtMotDePass.Visibility = Visibility.Collapsed;
                TxtNameAppGoogleDrive.Visibility = Visibility.Collapsed;
                TxtKeyAppDropBox.Visibility = Visibility.Visible;
                TxtNomAppDropBox.Visibility = Visibility.Visible;
                TxtKeyAppDropBox.SetValue(Grid.RowProperty, 1);
                TxtKeyAppDropBox.SetValue(Grid.ColumnProperty, 1);
                TxtNomAppDropBox.SetValue(Grid.RowProperty, 0);
                TxtNomAppDropBox.SetValue(Grid.ColumnProperty, 3);
                OperationName = _DossierOCR.Type_Source;
                btnCnx.SetValue(Grid.RowProperty, 1);
                btnCnx.SetValue(Grid.ColumnProperty, 3);
                EnableButton();
            }
            if (_DossierOCR.Type_Source == "GoogleDrive")
            {
                ComboOCR.Text = _DossierOCR.Type_Source;
                TxtNameAppGoogleDrive.Text = _DossierOCR.Login;
                TxtChemin.Visibility = Visibility.Collapsed;
                TxtLogin.Visibility = Visibility.Collapsed;
                TxtMotDePass.Visibility = Visibility.Collapsed;
                TxtKeyAppDropBox.Visibility = Visibility.Collapsed;
                TxtNomAppDropBox.Visibility = Visibility.Collapsed;
                TxtNameAppGoogleDrive.Visibility = Visibility.Visible;
                TxtNameAppGoogleDrive.SetValue(Grid.RowProperty, 0);
                TxtNameAppGoogleDrive.SetValue(Grid.ColumnProperty, 3);
                btnCnx.SetValue(Grid.RowProperty, 1);
                btnCnx.SetValue(Grid.ColumnProperty, 3);
                OperationName = _DossierOCR.Type_Source;
                EnableButton();
            }
        }
        private void ChangeVisibilityControlsByTypeOCR()
        {
            if (SelectedCombo == "DossierPartager")
            {
                TxtKeyAppDropBox.Visibility = Visibility.Collapsed;
                TxtNomAppDropBox.Visibility = Visibility.Collapsed;
                TxtNameAppGoogleDrive.Visibility = Visibility.Collapsed;
                TxtChemin.Visibility = Visibility.Visible;
                TxtLogin.Visibility = Visibility.Visible;
                TxtMotDePass.Visibility = Visibility.Visible;
                btnCnx.SetValue(Grid.RowProperty, 2);
                btnCnx.SetValue(Grid.ColumnProperty, 3);
                btnCnx.IsEnabled = false;
                OperationName = "DossierPartager";
            }
            else if (SelectedCombo == "FTP")
            {
                TxtKeyAppDropBox.Visibility = Visibility.Collapsed;
                TxtNomAppDropBox.Visibility = Visibility.Collapsed;
                TxtNameAppGoogleDrive.Visibility = Visibility.Collapsed;
                TxtChemin.Visibility = Visibility.Visible;
                TxtLogin.Visibility = Visibility.Visible;
                TxtMotDePass.Visibility = Visibility.Visible;
                btnCnx.SetValue(Grid.RowProperty, 2);
                btnCnx.SetValue(Grid.ColumnProperty, 3);
                OperationName = "Dossier Partager";
                OperationName = "FTP";
            }
            else if (SelectedCombo == "DropBox")
            {

                TxtChemin.Visibility = Visibility.Collapsed;
                TxtLogin.Visibility = Visibility.Collapsed;
                TxtMotDePass.Visibility = Visibility.Collapsed;
                TxtNameAppGoogleDrive.Visibility = Visibility.Collapsed;
                TxtKeyAppDropBox.Visibility = Visibility.Visible;
                TxtNomAppDropBox.Visibility = Visibility.Visible;
                TxtKeyAppDropBox.SetValue(Grid.RowProperty, 1);
                TxtKeyAppDropBox.SetValue(Grid.ColumnProperty, 1);
                TxtNomAppDropBox.SetValue(Grid.RowProperty, 0);
                TxtNomAppDropBox.SetValue(Grid.ColumnProperty, 3);
                btnCnx.SetValue(Grid.RowProperty, 1);
                btnCnx.SetValue(Grid.ColumnProperty, 3);
                OperationName = "DropBox";
            }
            else if (SelectedCombo == "GoogleDrive")
            {

                TxtChemin.Visibility = Visibility.Collapsed;
                TxtLogin.Visibility = Visibility.Collapsed;
                TxtMotDePass.Visibility = Visibility.Collapsed;
                TxtKeyAppDropBox.Visibility = Visibility.Collapsed;
                TxtNomAppDropBox.Visibility = Visibility.Collapsed;
                TxtNameAppGoogleDrive.Visibility = Visibility.Visible;
                TxtNameAppGoogleDrive.SetValue(Grid.RowProperty, 0);
                TxtNameAppGoogleDrive.SetValue(Grid.ColumnProperty, 3);
                btnCnx.SetValue(Grid.RowProperty, 1);
                btnCnx.SetValue(Grid.ColumnProperty, 3);
                btnEnregistrer.SetValue(Grid.RowProperty, 2);
                btnEnregistrer.SetValue(Grid.ColumnProperty, 3);
                OperationName = "GoogleDrive";
            }
        }
        private void EnableButton()
        {

            switch (OperationName)
            {
                case "DossierPartager":
                case "FTP":
                    btnCnx.IsEnabled = !(TxtChemin.Text == "" || TxtLogin.Text == "" || TxtMotDePass.Text == "");
                    btnEnregistrer.IsEnabled = !(TxtChemin.Text == "" || TxtLogin.Text == "" || TxtMotDePass.Text == "");
                    break;
                case "DropBox":
                    btnCnx.IsEnabled = !(TxtKeyAppDropBox.Text == "" || TxtNomAppDropBox.Text == "");
                    btnEnregistrer.IsEnabled = !(TxtKeyAppDropBox.Text == "" || TxtNomAppDropBox.Text == "");
                    break;
                case "GoogleDrive":
                    btnCnx.IsEnabled = !(TxtNameAppGoogleDrive.Text == "");
                    btnEnregistrer.IsEnabled = !(TxtNameAppGoogleDrive.Text == "");
                    break;
            }
        }
        private void ViderText()
        {
            TxtKeyAppDropBox.Text = "";
            TxtNomAppDropBox.Text = "";
            TxtNameAppGoogleDrive.Text = "";
            TxtChemin.Text = "";
            TxtLogin.Text = "";
            TxtMotDePass.Text = "";
            btnCnx.IsEnabled = false;
        }
        private void btnConnexion_Click(object sender, RoutedEventArgs e)
        {
            string Type_Source = ComboOCR.Text;
            try
            {
                switch (Type_Source)
                {
                    case "DossierPartager":
                        try
                        {
                            using (NetworkConnection nc = new NetworkConnection(TxtChemin.Text.Trim(), new NetworkCredential(TxtLogin.Text.Trim(), TxtMotDePass.Text.Trim())))
                            {
                                GeneraFiMessageBox.Show("Connexion Valide");
                            }
                        }
                        catch (Exception)
                        {
                            GeneraFiMessageBox.Show("Connexion non Valide");
                        }
                        break;
                    case "FTP":
                        bool TestConxFtp;
                        TestConxFtp = _BParamsOCR.FtpDirectoryExists(TxtChemin.Text.Trim(), TxtLogin.Text.Trim(), TxtMotDePass.Text.Trim());
                        if (TestConxFtp)
                            GeneraFiMessageBox.Show("Connexion Valide");
                        else
                            GeneraFiMessageBox.Show("Connexion non Valide");
                        break;
                    case "DropBox":
                        if (TxtKeyAppDropBox.Text.Trim() == CodeAccessTokenDropBox && TxtNomAppDropBox.Text.Trim() == NomAppDropBox)
                            GeneraFiMessageBox.Show("Connexion Valide");
                        else
                            GeneraFiMessageBox.Show("Connexion non Valide");
                        ; break;
                    case "GoogleDrive":
                        //if (TxtNameAppGoogleDrive.Text.Trim() == NomAppGoogleDrive)
                        //    GeneraFiMessageBox.Show("Connexion Valide");
                        //else
                        //    GeneraFiMessageBox.Show("Connexion non Valide");
                        #region authentification  retrive file  download file Delete File 
                        var service = _BParamsOCR.AuthenticateOauth(@"C:\Users\Generafi\Desktop\DossierOCR\ProjetOCR\ProjetOCR\Generafi_id.json", "test", _DossierOCR.Login);
                        var files = _BParamsOCR.ListFiles(service);
                        ListFileView ListFileView = new ListFileView(files);
                        ListFileView.Show();
                        Google.Apis.Drive.v3.Data.File file = new Google.Apis.Drive.v3.Data.File();
                        foreach (var item in files.Files)
                        {
                            _BParamsOCR.DownloadFile(service, item, string.Format(@"E:\GoogleDriveTest\{0}", item.Name));
                        }
                        foreach (var item in files.Files)
                        {
                            service.Files.Delete(item.Id).Execute();
                        }
                        #endregion
                        break;
                }
            }
            catch (Exception ex)
            {
                GeneraFiMessageBox.Show(ex.Message);
            }
        }
        private void InsertDossierOCR()
        {
            DossierOCR dossierOCR = new DossierOCR();
            dossierOCR.Type_Source = SelectedCombo;
            switch (SelectedCombo)
            {
                case "FTP":
                case "DossierPartager":
                    dossierOCR.Chemin = Chemin;
                    dossierOCR.Login = Login;
                    dossierOCR.MotDePass = MotDePass;
                    break;
                case "DropBox":
                    dossierOCR.Chemin = Chemin;
                    dossierOCR.Login = Login;
                    break;
                case "GoogleDrive":
                    dossierOCR.Login = Login;
                    break;
            }
            _BParamsOCR.InsertInfoOCR(dossierOCR);
        }
        private void ComboOCR_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedCombo = (ComboOCR.SelectedItem as ComboBoxItem).Content.ToString();
            ChangeVisibilityControlsByTypeOCR();
            ViderText();
            EnableButton();
        }
        private void btnEnregistrer_Click(object sender, RoutedEventArgs e)
        {
            page = new PageChargement(Window.GetWindow(this), true);
            page.Show();
            if (!backgroundWorkerOCR.IsBusy)
            {
                backgroundWorkerOCR.RunWorkerAsync();
            }
            switch (SelectedCombo)
            {
                case "FTP":
                case "DossierPartager":
                    Chemin= TxtChemin.Text;
                    Login= TxtLogin.Text;
                    MotDePass= TxtMotDePass.Text;
                    break;
                case "DropBox":
                    Chemin= TxtNomAppDropBox.Text;
                    Login= TxtKeyAppDropBox.Text;
                    break;
                case "GoogleDrive":
                    Login= TxtNameAppGoogleDrive.Text;
                    break;
            }
        }
        private void TxtNomAppDropBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            EnableButton();
        }
    }
}
