using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JuneWPF.Model
{

  public class DWGcontainer
        {
            public string DWGName { get; set; }

            public Element DWGElement { get; set; }

            public string ViewName { get; set; }

            public DWGcontainer.ImportType Type { get; set; }

            public View ViewElement { get; set; }

            public enum ImportType
            {
                ViewSpecific,
                Unidentified,
                ModelImport,
            }
        }
    
}
