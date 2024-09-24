using Dropbox.Api;
using Dropbox.Api.Files;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Net;
using System.Threading.Tasks;
using Dropbox.Api.Team;
using System.Threading;
using System.Windows.Forms;

namespace ProjetOCR
{
    class DropBoxBase
    {
        private DropboxClient DBClient;
        string _strAccessToken = string.Empty;
        private const string RedirectUri = "https://localhost/authorize";
        public bool TestCnx=true; 
        public DropBoxBase(string ApiKey, string ApiSecret, string ApplicationName = "Generafitest")
        {
            try
            {
                AppKey = ApiKey;
                AppSecret = ApiSecret;
                AppName = ApplicationName;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public string AppName
        {
            get;
            private set;
        }
        public string AuthenticationURL
        {
            get;
            private set;
        }
        public string AppKey
        {
            get;
            private set;
        }
        public string AppSecret
        {
            get;
            private set;
        }
        public string AccessTocken
        {
            get;
            private set;
        }
        public string Uid
        {
            get;
            private set;
        }
        public bool CreateFolder(string path , string AccessTocken, string NameApp)
        {
            try
            {
                DropboxClientConfig CC = new DropboxClientConfig(NameApp, 1);
                HttpClient HTC = new HttpClient();
                HTC.Timeout = TimeSpan.FromMinutes(10);
                CC.HttpClient = HTC;
                DBClient = new DropboxClient(AccessTocken, CC);
                if (AccessTocken == null)
                {
                    throw new Exception("AccessToken not generated !");
                }
                var folderArg = new CreateFolderArg(path);
                var folder = DBClient.Files.CreateFolderV2Async(folderArg);
                var result = folder.Result;
                return true;
            }
            catch (Exception )
            {
                return false;
            }
        }
        public bool FolderExists(string path)
        {
            try
            {
                if (AccessTocken == null)
                {
                    throw new Exception("AccessToken not generated !");
                }
                if (AuthenticationURL == null)
                {
                    throw new Exception("AuthenticationURI not generated !");
                }
                var folders = DBClient.Files.ListFolderAsync(path);
                var result = folders.Result;
                return true;
            }
            catch (Exception )
            {
                return false;
            }
        }
        public bool Deletee(string path, string strAccessToken, string NameApp)
        {
            try
            {
                DropboxClientConfig CC = new DropboxClientConfig(NameApp, 1);
                HttpClient HTC = new HttpClient();
                HTC.Timeout = TimeSpan.FromMinutes(10);
                CC.HttpClient = HTC;
                DBClient = new DropboxClient(strAccessToken, CC);
                if (strAccessToken == null)
                {
                    throw new Exception("AccessToken not generated !");
                }
                var folders = DBClient.Files.DeleteV2Async(path);
                var result = folders.Result;
                return true;
            }
            catch (Exception )
            {
                return false;
            }
        } 
        public bool Upload(string UploadfolderPath, string UploadfileName, string SourceFilePath,string strAccessToken, string NameApp )
        {
            try
            {
                DropboxClientConfig CC = new DropboxClientConfig(NameApp, 1);
                HttpClient HTC = new HttpClient();
                HTC.Timeout = TimeSpan.FromMinutes(10);
                CC.HttpClient = HTC;
                DBClient = new DropboxClient(strAccessToken, CC);
                using (var stream = new MemoryStream(File.ReadAllBytes(SourceFilePath)))
                {
                    var response = DBClient.Files.UploadAsync(UploadfolderPath + "/" + UploadfileName, WriteMode.Overwrite.Instance, body: stream);
                    
                    var rest = response.Result;
                }

                return true;
            }
            catch (Exception )
            {
                return false;
            }
        }
        public bool Download(string DropboxFolderPath, string DropboxFileName, string DownloadFolderPath, string DownloadFileName,string strAccessToken,string NameApp)
        {
            try
            {
                DropboxClientConfig CC = new DropboxClientConfig(NameApp,1);
                HttpClient HTC = new HttpClient();
                HTC.Timeout = TimeSpan.FromMinutes(10);  
                CC.HttpClient = HTC;
                DBClient = new DropboxClient(strAccessToken, CC);
                using (var response = DBClient.Files.DownloadAsync(DropboxFolderPath + "/" + DropboxFileName))
                {
                        using (var fileStream = File.Create(@"E:\dropboxtest\tt.txt"))
                        {
                            response.Result.GetContentAsStreamAsync().Result.CopyTo(fileStream);
                        }
                }
                return true;
            }
            catch (Exception )
            {
                return false;
            }
        }
        public bool CanAuthenticate()
        {
            try
            {
                if (AppKey == null)
                {
                    throw new ArgumentNullException("AppKey");
                }
                if (AppSecret == null)
                {
                    throw new ArgumentNullException("AppSecret");
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<string> ListFolder(string path)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Custom Description";
            var result = fbd.ShowDialog();
            string sSelectedPath = fbd.SelectedPath;
            List<string> list = new List<string>();
            string filename = Environment.ExpandEnvironmentVariables(sSelectedPath);
            List<string >list11=new List<string>();
            string[] file = Directory.GetFiles(sSelectedPath);
            if (file != null)
            {
               foreach( var item in file)
               {
                   list11.Add(System.IO.Path.GetFileName(item).ToString());
               }
            }
            return list11;
        }
     
    }
}
