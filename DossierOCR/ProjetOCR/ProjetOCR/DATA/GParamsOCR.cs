using Dapper;
using ProjetOCR.BEL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using Dapper.Contrib.Extensions;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Drive.v3;

namespace ProjetOCR.DATA
{
    public class GParamsOCR : Connexion
    {
        private List<DossierOCR> Lists = new List<DossierOCR>();
        public void InsertInfoOCR(DossierOCR _dossierOCR)
        {
            try
            {
                using (SqlConnection cn = con)
                {
                    SqlTransaction transact = null;
                    try
                    {
                        if (cn.State != ConnectionState.Open)
                            cn.Open();
                        transact = cn.BeginTransaction();

                        cn.Execute("Delete From DossierOCR",transaction: transact);
                        cn.Insert(_dossierOCR, transaction: transact);
                        transact.Commit();
                    }
                    catch (Exception)
                    {
                        transact?.Rollback();
                        throw;
                    }
                    finally
                    {
                        cn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        public DossierOCR GetDossierOCR()
        {
            DossierOCR _DossierOCR = new DossierOCR();
            try
            {
                _DossierOCR = con.Query<DossierOCR>("select *from DossierOCR").SingleOrDefault();

            }
            finally
            {
                CloseCon();
            }
            return _DossierOCR;
        }
        #region googleDrive 
        public  FileList ListFiles(DriveService service)
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
        public void DownloadFile(DriveService service, Google.Apis.Drive.v3.Data.File file, string saveTo)
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
        public void SaveStream(System.IO.MemoryStream stream, string saveTo)
        {
            using (System.IO.FileStream file = new System.IO.FileStream(saveTo, System.IO.FileMode.Create, System.IO.FileAccess.Write))
            {
                stream.WriteTo(file);
            }
        }
        #endregion
    }
}

