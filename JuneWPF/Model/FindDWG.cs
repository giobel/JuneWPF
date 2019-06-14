using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JuneWPF.Model
{
    public class FindDWG
    {
        public static List<KeyValuePair<string, string>> listImports(Document doc)
        {
            

            List<KeyValuePair<string, string>> listOfViewSpecificImports = new List<KeyValuePair<string, string>>();

            List<KeyValuePair<string, string>> listOfModelImports = new List<KeyValuePair<string, string>>();

            List<KeyValuePair<string, string>> listOfUnidentifiedImports = new List<KeyValuePair<string, string>>();

            FilteredElementCollector col = new FilteredElementCollector(doc).OfClass(typeof(ImportInstance));

            foreach (Element e in col)
            {


                if (e.Category != null)
                {
                    if (e.ViewSpecific)
                    {
                        string viewName = null;

                        try
                        {
                            Element viewElement = doc.GetElement(
                              e.OwnerViewId);
                            viewName = viewElement.Name;
                        }
                        catch (Autodesk.Revit.Exceptions
                          .ArgumentNullException) // just in case
                        {
                            viewName = String.Concat(
                              "Invalid View ID: ",
                              e.OwnerViewId.ToString());
                        }


                        if (null != e.Category)
                        {
                            try
                            {
                                listOfViewSpecificImports.Add(new KeyValuePair<string, string>( viewName, importCategoryNameToFileName(e.Category.Name)));
                            }
                            catch { }
                        }
                            

                        else
                        {
                            try
                            {
                                listOfUnidentifiedImports.Add(new KeyValuePair<string, string>(viewName, e.Id.ToString()));
                            }
                            catch { }

                        }

                    }

                    else
                    {
                        try
                        {
                            listOfModelImports.Add(new KeyValuePair<string, string>(e.Name, importCategoryNameToFileName(e.Category.Name)));
                        }
                        catch { }

                    }

                }
                else
                {
                    //TaskDialog.Show("result",e.Id.ToString());
                }

            }
            
            return listOfViewSpecificImports;

        }

        public static ObservableCollection<DWGcontainer> listEmployees(Document doc)
        {
            ObservableCollection<DWGcontainer> dwgs = new ObservableCollection<DWGcontainer>();

            FilteredElementCollector col = new FilteredElementCollector(doc).OfClass(typeof(ImportInstance));

            foreach (Element e in col)
            {


                if (e.Category != null)
                {
                    if (e.ViewSpecific)
                    {
                        string viewName = null;

                        Element viewElement = null;
                        try
                        {
                            viewElement = doc.GetElement(e.OwnerViewId);
                            viewName = viewElement.Name;
                        }
                        catch (Autodesk.Revit.Exceptions.ArgumentNullException) // just in case
                        {
                            viewName = String.Concat("Invalid View ID: ", e.OwnerViewId.ToString());
                        }


                        if (null != e.Category)
                        {
                            try
                            {
                                dwgs.Add(new DWGcontainer { ViewName = viewName, DWGName = importCategoryNameToFileName(e.Category.Name), Type = DWGcontainer.ImportType.ViewSpecific, ViewElement =  viewElement as View, DWGElement = e});
                            }
                            catch { }
                        }


                        else
                        {
                            try
                            {
                                dwgs.Add(new DWGcontainer { ViewName = viewName, DWGName = e.Id.ToString(), Type = DWGcontainer.ImportType.Unidentified, DWGElement = e, ViewElement = viewElement as View});
                            }
                            catch { }

                        }

                    }

                    else
                    {
                        try
                        {
                            dwgs.Add(new DWGcontainer {ViewName = "Not View specific", DWGName = importCategoryNameToFileName(e.Category.Name), Type = DWGcontainer.ImportType.ModelImport, DWGElement = e });
                        }
                        catch { }

                    }

                }
                else
                {
                    //TaskDialog.Show("result",e.Id.ToString());
                }

            }

            return dwgs;
        }

        private static string importCategoryNameToFileName(string catName)
        {
            string fileName = catName;
            fileName = fileName.Trim();

            if (fileName.EndsWith(")"))
            {
                int lastLeftBracket = fileName.LastIndexOf("(");

                if (-1 != lastLeftBracket)
                    fileName = fileName.Remove(lastLeftBracket); // remove left bracket
            }

            return fileName.Trim();
        }
    }//close class
    }//close namespace
