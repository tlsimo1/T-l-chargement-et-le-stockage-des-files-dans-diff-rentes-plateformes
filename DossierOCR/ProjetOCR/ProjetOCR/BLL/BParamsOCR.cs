using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using ProjetOCR.BEL;
using ProjetOCR.DATA;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace ProjetOCR.BLL
{
    public class BParamsOCR
    {
        GParamsOCR _GParamsOCR = new GParamsOCR();

        #region CRUD Operation
        public void InsertInfoOCR(DossierOCR _dossierOCR)
        {
            _GParamsOCR.InsertInfoOCR(_dossierOCR);
        }
        public DossierOCR GetDossierOCR()
        {
            return _GParamsOCR.GetDossierOCR();
        }
        #endregion
        #region Get Files Function
        public List<string> ListFilesFromFTP(DossierOCR dossierOCR)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(dossierOCR.Chemin);
                request.Method = WebRequestMethods.Ftp.ListDirectory;
                request.Credentials = new NetworkCredential(dossierOCR.Login, dossierOCR.MotDePass);
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                string names = reader.ReadToEnd();
                reader.Close();
                response.Close();
                return names.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<string> GetFileFromPartage(DossierOCR dossierOCR)
        {
            List<string> Listfilename = new List<string>();
            string[] file = Directory.GetFiles(dossierOCR.Chemin);
            if (file != null)
            {
                foreach (var item in file)
                {
                    Listfilename.Add(System.IO.Path.GetFileName(item).ToString());
                }
            }
            return Listfilename;
        }
        #endregion
        #region Test Connexion Function
        public bool FtpDirectoryExists(string Chemein,string Login,string MotDePass)
        {
            bool IsExists = true;
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(Chemein);
                request.Credentials = new NetworkCredential(Login, MotDePass);
                request.Method = WebRequestMethods.Ftp.PrintWorkingDirectory;

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            }
            catch (WebException )
            {
                IsExists = false;
            }
            return IsExists;
        }
        public void DeleteFileOnFtpServer(DossierOCR _DossierOCR)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(_DossierOCR.Chemin);
                request.Credentials = new NetworkCredential(_DossierOCR.Login, _DossierOCR.MotDePass);
                request.Method = WebRequestMethods.Ftp.DeleteFile;
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                response.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void downloadtest()
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://192.168.1.147/test2.zip");
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            request.Credentials = new NetworkCredential("talsi", "123");
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            var reader = new StreamReader(response.GetResponseStream());
            var bytes = default(byte[]);
            using (var memstream = new MemoryStream())
            {
                reader.BaseStream.CopyTo(memstream);
                bytes = memstream.ToArray();
            }
           System.IO.File.WriteAllBytes(@"E:\upload\mohammed\123.zip", bytes);
        }

        #region googleDrive
        public DriveService AuthenticateOauth(string clientSecretJson, string userName, string ApplicationName)
        {
            try
            {
                if (string.IsNullOrEmpty(userName))
                    throw new ArgumentNullException("userName");
                if (string.IsNullOrEmpty(clientSecretJson))
                    throw new ArgumentNullException("clientSecretJson");
                if (!System.IO.File.Exists(clientSecretJson))
                    throw new Exception("clientSecretJson file does not exist.");
                string[] scopes = new string[] { DriveService.Scope.Drive };
                UserCredential credential;
                using (var stream = new FileStream(clientSecretJson, FileMode.Open, FileAccess.ReadWrite))
                {
                    string credPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                    credPath = System.IO.Path.Combine(credPath, ".credentials/", System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(GoogleClientSecrets.Load(stream).Secrets,
                                                                             scopes,
                                                                             userName,
                                                                             CancellationToken.None,
                                                                             new FileDataStore(credPath, true)).Result;
                }
                return new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Create Oauth2 account DriveService failed" + ex.Message);
                throw new Exception("CreateServiceAccountDriveFailed", ex);
            }
        }

        public  FileList ListFiles(DriveService service)
        {
            return _GParamsOCR.ListFiles(service);
        }
        public void DownloadFile(DriveService service, Google.Apis.Drive.v3.Data.File file, string saveTo)
        {
            _GParamsOCR.DownloadFile(service, file, saveTo);
        }
        #endregion

        #endregion
    }
}
