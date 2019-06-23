using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace JuneWPF.Model
{
    class DeleteDWG
    {
        public static void Delete(UIApplication _uiapp, Element _dwg)
        {
            Document doc = _uiapp.ActiveUIDocument.Document;

            using (Transaction transaction = new Transaction(doc))
            {
                transaction.Start("DeleteDWG");
                doc.Delete(_dwg.Id);
                transaction.Commit();
            }
        }

        
    }
}
