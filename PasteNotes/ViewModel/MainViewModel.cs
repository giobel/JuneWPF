using Autodesk.Revit.UI;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Linq;
using System.Windows.Data;
using System.Collections.ObjectModel;
using System.Windows;


// C:\Revit 2019.2 SDK\Samples\ModelessDialog\ModelessForm_ExternalEvent

namespace JuneWPF.ViewModel
{

    public class MainViewModel : ViewModelBase
    {
        private ExternalEvent exEvent;
        public string WindowTitle { get; private set; }
        public int Progress { get; set; }

        private UIDocument uidoc = null;
        public List<KeyValuePair<string, string>> nvc { get; set; }
        public string _result { get; set; }

        private Model.RequestHandler handler { get; set; }

        public bool TextArrowLeft { get; set; }
        public bool TextArrowRight { get; set; }
        public double TextWidth { get; set; }
        public double LeaderLength { get; set; }
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            //if (IsInDesignMode)
            //{
            //    WindowTitle = "design-mode title";
            //    Progress = 25;
            //}
            //else
            //{
            //    WindowTitle = " run model title";
            //    Task.Delay(2000).ContinueWith(t =>
            //    {
            //        while (Progress < 100)
            //        {
            //            Progress += 5;
            //            Task.Delay(500).Wait();
            //        }
            //    });
            //}
            WindowTitle = "Place TextNote From Clipboard";
            
            handler = new Model.RequestHandler();

            exEvent = ExternalEvent.Create(handler);

            uidoc = Command._activeRevitUIDoc;

            TextWidth = 1.2;
            LeaderLength = 3;
                     
            //ListCollectionView collectionView = new ListCollectionView(employees);
        }


        private void MakeRequest(Model.Request.RequestId request)
        {
            handler.Request.Make(request);
            exEvent.Raise();
            //DozeOff();
        }



        public RelayCommand PlaceTextNodeCommand {
            get
            {
                return new RelayCommand(LaunchCommand);
            }
        }
        public RelayCommand UpdateTextNodeCommand
        {
            get
            {
                return new RelayCommand(UpdateNoteCommand);
            }
        }
        public RelayCommand OpenViewCommand
        {
            get
            {
                return new RelayCommand(OpenCommand);
            }
        }
        public RelayCommand OpenSheetCommand
        {
            get
            {
                return new RelayCommand(OpenSheet);
            }
        }

        private void LaunchCommand()
        {
            if (TextArrowLeft && TextArrowRight)
            {
                handler.NoteLeaderPosition = Model.TextNoteObject.TextLeaderPosition.Both;
            }
            else if (TextArrowRight)
            {
                handler.NoteLeaderPosition = Model.TextNoteObject.TextLeaderPosition.Right;
            }
            else if (TextArrowLeft)
            {
                handler.NoteLeaderPosition = Model.TextNoteObject.TextLeaderPosition.Left;
            }
            else
            {
                handler.NoteLeaderPosition = Model.TextNoteObject.TextLeaderPosition.None;
            }
            handler.m_TextNoteWidth = TextWidth;
            handler.m_LeaderLenght = LeaderLength;

            MakeRequest(Model.Request.RequestId.TextNote);
            /*
            string textBox1 = "";
            if (Clipboard.GetDataObject().GetDataPresent(DataFormats.Text))
                textBox1 = Clipboard.GetDataObject().GetData(DataFormats.Text).ToString();
            else
                textBox1 = "The clipboad does not contain any text";
            TaskDialog.Show("sss", textBox1);*/
        }        
        private void UpdateNoteCommand()
        {
            MakeRequest(Model.Request.RequestId.UpdateNote);
        }
        private void OpenCommand()
        {
            MakeRequest(Model.Request.RequestId.OpenView);
        }
        private void OpenSheet()
        {
            MakeRequest(Model.Request.RequestId.BackToSheet);
        }




    }
}