using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjetOCR.BEL
{
    [Dapper.Contrib.Extensions.Table("Parametre")]
    class Parametre: BindableBase
    {
        
        public int id_Param { get; set; }

        string _nomParam;
        public string NomParam
        {
            get
            {
                return _nomParam;
            }
            set
            {
                if (_nomParam != value)
                {
                    _nomParam = value;
                    OnProperyChanged("NomParam");
                }
            }
        }

        string _valeurParam;
        public string ValeurParam
        {
            get
            {
                return _valeurParam;
            }
            set
            {
                if (_valeurParam != value)
                {
                    _valeurParam = value;
                    OnProperyChanged("ValeurParam");
                }
            }
        }
    }
}
