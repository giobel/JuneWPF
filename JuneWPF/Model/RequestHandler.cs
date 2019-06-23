using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace JuneWPF.Model
{
    public class RequestHandler : IExternalEventHandler
    {
        private Request m_request = new Request();

        public Element dwg { get; set; }

        public Request Request
        {
            get
            {
                return this.m_request;
            }
        }
        public void Execute(UIApplication uiapp)
        {
            try
            {
                switch (Request.Take())
                {
                    case Model.Request.RequestId.None:
                        {
                            return;  // no request at this time -> we can leave immediately
                        }
                    case Model.Request.RequestId.Delete:
                        {
                            Model.DeleteDWG.Delete(uiapp, dwg);
                            break;
                        }
                    case Model.Request.RequestId.TextNote:
                        {
                            Model.TextNoteObject.Place(uiapp);
                            break;
                        }
                    default:
                        {
                            // some kind of a warning here should
                            // notify us about an unexpected request 
                            break;
                        }
                }
            }
            finally
            {
                //Application.thisApp.WakeFormUp();
            }





        }

        public string GetName()
        {
            return "External Event MVVM";
        }
    }
}
