using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace JuneWPF.Model
{
    class TextNoteObject
    {
        public static void Place(UIApplication uiapp)
        {
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            IList<UIView> uiviews = uidoc.GetOpenUIViews();
            UIView uiview = null;
            foreach (UIView uv in uiviews)
            {
                if (uv.ViewId.Equals(doc.ActiveView.Id))
                {
                    uiview = uv;
                    break;
                }
            }


            IList<XYZ> corners = uiview.GetZoomCorners();
            XYZ p = corners[0];
            XYZ q = corners[1];
            XYZ v = q - p;
            XYZ center = p + 0.5 * v;

            using (Transaction tran = new Transaction(doc, "Creating a Text note"))
            {
                if (Clipboard.GetDataObject().GetDataPresent(DataFormats.Text))
                {
                    string textNoteContent = Clipboard.GetDataObject().GetData(DataFormats.Text).ToString();

                    XYZ origin = new XYZ(10, 10, 0);
                    //double width = 3.0 / 12.0; // feet on paper

                    TextNoteOptions options = new TextNoteOptions();
                    options.HorizontalAlignment = HorizontalTextAlignment.Center;
                    options.TypeId = doc.GetDefaultElementTypeId(ElementTypeGroup.TextNoteType);

                    tran.Start();
                    TextNote note = TextNote.Create(doc, doc.ActiveView.Id, center, textNoteContent, options);
                    tran.Commit();
                }
                else
                {
                    TaskDialog.Show("Warning", "Clipboard is empty. Try to copy something.");
                }
            }
        }
    }
}
