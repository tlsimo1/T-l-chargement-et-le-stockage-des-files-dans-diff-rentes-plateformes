using ProjetOCR.BEL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace ProjetOCR
{
    public class RaderFiles
    {
        DossierOCR _dossierOCR;

        //public int Count { get { return FilesPaths.Count; } }
        //public abstract FileStream GetFileStream(int index);
        //public FileStream this[int index] { get { return GetFileStream(index); } }
        public RaderFiles(DossierOCR dossierOCR)
        {
            _dossierOCR = dossierOCR;
        }
        public RaderFiles()
        {
        }
        public List<string> GetFilesName()
        {
            List<string> ListFiles = new List<string>();
            switch (_dossierOCR.Type_Source)
            {
                case "DossierPartager":
                    ListFiles = GetFileFromPartage();
                    break;
                case "FTP":
                    ListFiles = ListFilesFromFTP();
                    break;
                case "DropBox":
                    ListFiles = ListFileFromDropBox();
                    break;
            }
            return ListFiles;
        }
        public Stream GetFileByName(string filename)
        {
            switch (_dossierOCR.Type_Source)
            {
                case "DossierPartager":
                    return new FileStream(filename, FileMode.Open, FileAccess.Read);
                case "FTP":
                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create(filename);
                    request.Method = WebRequestMethods.Ftp.DownloadFile;
                    request.Credentials = new NetworkCredential(_dossierOCR.Login, _dossierOCR.MotDePass);
                    FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                    return response.GetResponseStream();

                case "DropBox":
                    return new FileStream(filename, FileMode.Open, FileAccess.Read); 
            }
            return null;
        }

        private List<string> ListFilesFromFTP()
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(_dossierOCR.Chemin);
                request.Method = WebRequestMethods.Ftp.ListDirectory;
                request.Credentials = new NetworkCredential(_dossierOCR.Login, _dossierOCR.MotDePass);
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
        private List<string> GetFileFromPartage()
        {
            List<string> Listfilename = new List<string>();
            string[] file = Directory.GetFiles(_dossierOCR.Chemin);
            if (file != null)
            {
                foreach (var item in file)
                {
                    Listfilename.Add(System.IO.Path.GetFileName(item).ToString());
                }
            }
            return Listfilename;
        }
        public List<string> ListFileFromDropBox()
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Custom Description";
            var result = fbd.ShowDialog();
            string sSelectedPath = fbd.SelectedPath;
            List<string> list = new List<string>();
            string filename = Environment.ExpandEnvironmentVariables(sSelectedPath);
            List<string> list11 = new List<string>();
            string[] file = Directory.GetFiles(sSelectedPath);
            if (file != null)
            {
                foreach (var item in file)
                {
                    list11.Add(System.IO.Path.GetFileName(item).ToString());
                }
            }
            return list11;
        }
    }
}
