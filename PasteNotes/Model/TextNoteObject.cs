using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Automation;

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

        public static void Place(UIApplication uiapp, TextLeaderPosition _textLeader, double _textWidth, double _leaderLength)
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
            try
            {
                using (Transaction tran = new Transaction(doc, "Text note created"))
                {
                    if (Clipboard.GetDataObject().GetDataPresent(DataFormats.Text))
                    {
                        //uses PresentationCore
                        string textNoteContent = Clipboard.GetDataObject().GetData(DataFormats.Text).ToString();

                        //double width = 3.0 / 12.0; // feet on paper

                        TextNoteOptions options = new TextNoteOptions();
                        options.HorizontalAlignment = HorizontalTextAlignment.Left;
                        
                        options.TypeId = doc.GetDefaultElementTypeId(ElementTypeGroup.TextNoteType);
                        double noteWidth = _textWidth/12;

                        tran.Start();
                        TextNote note = TextNote.Create(doc, doc.ActiveView.Id, center, textNoteContent, options);
                        note.Width = noteWidth;

                        

                        if (_textLeader == TextLeaderPosition.Both)
                        {
                            note.AddLeader(TextNoteLeaderTypes.TNLT_STRAIGHT_L);
                            Leader lead = note.AddLeader(TextNoteLeaderTypes.TNLT_STRAIGHT_R);
                            note.LeaderLeftAttachment = LeaderAtachement.TopLine;
                            note.LeaderRightAttachment = LeaderAtachement.TopLine;
                            note.Width = noteWidth;

                            lead.End = new XYZ(center.X + _leaderLength, center.Y - 0.8, center.Z);

                        }
                        else if (_textLeader == TextLeaderPosition.Left)
                        {
                            note.AddLeader(TextNoteLeaderTypes.TNLT_STRAIGHT_L);
                            note.LeaderLeftAttachment = LeaderAtachement.TopLine;
                            note.Width = noteWidth;
                        }
                        else if (_textLeader == TextLeaderPosition.Right)
                        {
                            Leader lead = note.AddLeader(TextNoteLeaderTypes.TNLT_STRAIGHT_R);
                            note.LeaderRightAttachment = LeaderAtachement.TopLine;
                            note.Width = noteWidth;


                            //lead.Elbow = new XYZ(center.X, center.Y, center.Z);
                            
                            lead.End = new XYZ(center.X+ _leaderLength, center.Y-0.8, center.Z);
                        }

                        tran.Commit();
                    }
                    else
                    {
                        TaskDialog.Show("Warning", "Clipboard is empty. Try to copy something.");
                    }
                }
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", ex.Message);
            }
            
        }

        public static void Update(UIApplication uiapp)
        {
            Document doc = uiapp.ActiveUIDocument.Document;
            ElementId eid = uiapp.ActiveUIDocument.Selection.GetElementIds().FirstOrDefault();

            try
            {
                using (Transaction tran = new Transaction(doc, "Text note updated"))
                {
                    if (Clipboard.GetDataObject().GetDataPresent(DataFormats.Text))
                    {
                        tran.Start();
                        string textNoteContent = Clipboard.GetDataObject().GetData(DataFormats.Text).ToString();
                        TextNote existingNote = doc.GetElement(eid) as TextNote;
                        existingNote.Text = textNoteContent;
                        tran.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", ex.Message);
            }

        }//close method

        public static void OpenView(UIApplication uiapp)
        {
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            try
            {
                IList<UIView> uiviews = uidoc.GetOpenUIViews();

                UIView uiview = null;

                View currentView = null;

                foreach (UIView uv in uiviews)
                {
                    if (uv.ViewId.Equals(doc.ActiveView.Id))
                    {
                        currentView = doc.ActiveView;
                        uiview = uv;
                        break;
                    }
                }

                uiview.Close();

                uidoc.ActiveView = currentView;
            }
            catch
            {

            }
        }

        public static void OpenSheet(UIApplication uiapp)
        {
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;
            try
            {
                string sheetNumber = doc.ActiveView.get_Parameter(BuiltInParameter.VIEWPORT_SHEET_NUMBER).AsString();

                ViewSheet viewSh = null;

                FilteredElementCollector sheets = new FilteredElementCollector(doc).OfClass(typeof(ViewSheet));

                foreach (ViewSheet sht in sheets)
                {
                    if (sht.SheetNumber == sheetNumber)
                    {
                        viewSh = sht;
                        break;
                    }
                        
                }

                uidoc.ActiveView = viewSh;

            }
            catch { }
        }
    }//close class
}//close namespace
