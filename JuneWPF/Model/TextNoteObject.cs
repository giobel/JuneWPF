using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace JuneWPF.Model
{
    public class TextNoteObject
    {
        public enum TextLeaderPosition : int
        {
            Left = 0,
            Right = 1,
            Both = 2,
            None = 3
        }

        public static void Place(UIApplication uiapp, TextLeaderPosition _textLeader)
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
                    options.HorizontalAlignment = HorizontalTextAlignment.Left;
                    
                    options.TypeId = doc.GetDefaultElementTypeId(ElementTypeGroup.TextNoteType);

                    tran.Start();
                    TextNote note = TextNote.Create(doc, doc.ActiveView.Id, center, textNoteContent, options);

                    if (_textLeader == TextLeaderPosition.Both)
                    {
                        note.AddLeader(TextNoteLeaderTypes.TNLT_STRAIGHT_L);
                        note.AddLeader(TextNoteLeaderTypes.TNLT_STRAIGHT_R);
                    }
                    else if (_textLeader == TextLeaderPosition.Left)
                    {
                        note.AddLeader(TextNoteLeaderTypes.TNLT_STRAIGHT_L);
                    }
                    else if (_textLeader == TextLeaderPosition.Right)
                    {
                        note.AddLeader(TextNoteLeaderTypes.TNLT_STRAIGHT_R);
                    }
                    
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
