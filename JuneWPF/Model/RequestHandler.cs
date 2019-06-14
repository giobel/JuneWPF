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
            Document doc = uiapp.ActiveUIDocument.Document;

            using (Transaction transaction = new Transaction(doc))
            {
                transaction.Start("DeleteDWG");
                doc.Delete(dwg.Id);
                transaction.Commit();
            }


        }

        public string GetName()
        {
            return "External Event MVVM";
        }
    }
}
