using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper.Contrib.Extensions;
namespace ProjetOCR.BEL
{

    [Dapper.Contrib.Extensions.Table("DossierOCR")]
   public class DossierOCR: BindableBase
    {
        private int id_DossierOCR;
        private string type_Source;
        private string chemin;
        private string login;
        private string motDePass;
        [Dapper.Contrib.Extensions.Key]
        public int ID_DossierOCR
        {
            get
            {
                return id_DossierOCR;
            }

            set
            {
                id_DossierOCR = value;
                OnProperyChanged("ID_DossierOCR");
            }
        }

        public string Type_Source
        {
            get
            {
                return type_Source;
            }

            set
            {
                type_Source = value;
                OnProperyChanged("Type_Source");
            }
        }

        public string Chemin
        {
            get
            {
                return chemin;
            }

            set
            {
                chemin = value;
                OnProperyChanged("Chemin");
            }
        }

        public string Login
        {
            get
            {
                return login;
            }

            set
            {
                login = value;
                OnProperyChanged("Login");
            }
        }

        public string MotDePass
        {
            get
            {
                return motDePass;
            }

            set
            {
                motDePass = value;
                OnProperyChanged("MotDePass");
            }
        }
    }
}
