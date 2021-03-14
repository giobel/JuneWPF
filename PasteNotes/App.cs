#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.UI;
#endregion

namespace JuneWPF
{
    public class App : IExternalApplication
    {
        internal static Application thisApp = null;
        private MainWindow m_MyForm;

        public Result OnStartup(UIControlledApplication a)
        {
            m_MyForm = null;
            //thisApp = this;
            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication a)
        {
            if (m_MyForm != null && m_MyForm.IsVisible)
            {
                m_MyForm.Close();
            }
            return Result.Succeeded;
        }

        public void ShowWindow(UIApplication uiapp)
        {
            if (m_MyForm == null)
            {
                Model.RequestHandler handler = new Model.RequestHandler();

                ExternalEvent exEvent = ExternalEvent.Create(handler);
            }
        }
    }
}
