using Google.Apis.Auth.OAuth2;
using Google.Apis.Download;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Logging;
using Google.Apis.Requests;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
namespace GoogleApiTest2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            string id = "F4gCM1pCIvZ77NvCGbWyDFfB";
            InitializeComponent();
            var service =AuthenticateOauth(@"C:\Users\Generafi\Desktop\GoogleApiTest2\GoogleApiTest2\Generafi_id.json", "test");
            var files = ListFiles(service);
            Google.Apis.Drive.v3.Data.File file = new Google.Apis.Drive.v3.Data.File();
            foreach (var item in files.Files)
            {
                string permissionid = GetPermissionIdForEmail(service, "generafi123@gmail.com");
                Permission permission = service.Permissions.Get(item.Id, permissionid).Execute();
                service.Files.Delete(id).Execute();
                //if (item.Name == "ttt999.zip")
                //{
                    //service.Permissions.Delete(item.Id, permissionid).Execute();
                    
                
                    DeleteFile(service, item);
                   // DownloadFile(service, item, string.Format(@"E:\GoogleDriveTest\{0}", item.Name));
                //}
            }
        }
        public static string GetPermissionIdForEmail(DriveService service, string emailAddress)
        {
            string pageToken = null;
            do
            {
                var request = service.Files.List();
                request.Q = $"'{emailAddress}' in writers or '{emailAddress}' in readers or '{emailAddress}' in owners";
                request.Spaces = "drive";
                request.Fields = "nextPageToken, files(id, name, permissions)";
                request.PageToken = pageToken;

                var result = request.Execute();

                foreach (var file in result.Files.Where(f => f.Permissions != null))
                {
                    var permission = file.Permissions.SingleOrDefault(p => string.Equals(p.EmailAddress, emailAddress, StringComparison.InvariantCultureIgnoreCase));

                    if (permission != null)
                        return permission.Id;
                }

                pageToken = result.NextPageToken;

            } while (pageToken != null);

            return null;
        }
        public static FileList ListFiles(DriveService service)
        {
            try
            {
                if (service == null)
                    throw new ArgumentNullException("service");
                var request = service.Files.List();
                return request.Execute();
            }
            catch (Exception ex)
            {
                throw new Exception("Request Files.List failed.", ex);
            }
        }
        public static DriveService AuthenticateOauth(string clientSecretJson, string userName)
        {
            try
            {
                if (string.IsNullOrEmpty(userName))
                    throw new ArgumentNullException("userName");
                if (string.IsNullOrEmpty(clientSecretJson))
                    throw new ArgumentNullException("clientSecretJson");
                if (!System.IO.File.Exists(clientSecretJson))
                    throw new Exception("clientSecretJson file does not exist.");
                string[] scopes = new string[] { DriveService.Scope.DriveReadonly };         	                                                
                UserCredential credential;
                using (var stream = new FileStream(clientSecretJson, FileMode.Open, FileAccess.Read))
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
                    ApplicationName = "GenerafiApi"
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Create Oauth2 account DriveService failed" + ex.Message);
                throw new Exception("CreateServiceAccountDriveFailed", ex);
            }
        }
        private static void DownloadFile(DriveService service, Google.Apis.Drive.v3.Data.File file, string saveTo)
        {

            var request = service.Files.Get(file.Id);
            var stream = new System.IO.MemoryStream();
            request.MediaDownloader.ProgressChanged += (Google.Apis.Download.IDownloadProgress progress) =>
            {
                switch (progress.Status)
                {
                    case Google.Apis.Download.DownloadStatus.Downloading:
                        {
                            Console.WriteLine(progress.BytesDownloaded);
                            break;
                        }
                    case Google.Apis.Download.DownloadStatus.Completed:
                        {
                            Console.WriteLine("Download complete.");
                            SaveStream(stream, saveTo);
                            break;
                        }
                    case Google.Apis.Download.DownloadStatus.Failed:
                        {
                            Console.WriteLine("Download failed.");
                            break;
                        }
                }
            };
            request.Download(stream);
        }
        private static void SaveStream(System.IO.MemoryStream stream, string saveTo)
        {
            using (System.IO.FileStream file = new System.IO.FileStream(saveTo, System.IO.FileMode.Create, System.IO.FileAccess.Write))
            {
                stream.WriteTo(file);
            }
        }
        public static void DeleteFile(DriveService service, Google.Apis.Drive.v3.Data.File file)
        {
            try
            {
                service.Files.Delete(file.Id).Execute();
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: " + e.Message);
            }
        }


    }
}

    

