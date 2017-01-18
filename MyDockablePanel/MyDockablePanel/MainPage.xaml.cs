using System;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Autodesk.Revit.UI;
using System.IO;
using Autodesk.Revit.DB;

//Global 17 docking panel with Family selector

namespace MyDockablePanel
{

    public partial class MainPage : Page, Autodesk.Revit.UI.IDockablePaneProvider
    {
        private Guid m_targetGuid;
        private DockPosition m_position = DockPosition.Bottom;
        private int m_left = 1;
        private int m_right = 1;
        private int m_top = 1;
        private int m_bottom = 1;
        List<Elektroinstallationen> sockets = new List<Elektroinstallationen>();
        public string FamilyPath { get; set; }
        public FamilyInsert FI { get; set; }
        List<string> FamilyList = new List<string>();

        static string initialDirectory = @"i:\Project\Revit\HHM Revit Famillien\2016.10.10 Familien Gianfranco\";
        string[] DirList = Directory.GetDirectories(initialDirectory);
        //string[] FamilyList = new string[DirList.Length];
        
        public MainPage()
        {
            InitializeComponent();
            FI = new FamilyInsert();
            FI.FamilyPath = "1";
            foreach (var item in DirList)
            {
                int indexSlash = item.LastIndexOf("\\") + 1;
                string DirName = item.Substring(indexSlash);
                comboBoxDirectories.Items.Add(DirName);
            }
        }

        public void SetupDockablePane(DockablePaneProviderData data)
        {
            data.FrameworkElement = this as FrameworkElement;
            data.InitialState = new DockablePaneState();
            data.InitialState.DockPosition = DockPosition.Tabbed;
            data.InitialState.TabBehind = DockablePanes.BuiltInDockablePanes.ProjectBrowser;
        }

        public void SetInitialDockingParameters(int left, int right, int top, int bottom, DockPosition position, Guid targetGuid)
        {
            m_position = position;
            m_left = left;
            m_right = right;
            m_top = top;
            m_bottom = bottom;
            m_targetGuid = targetGuid;
        }

        private void FileWrite(string familyPath)
        {
            StreamWriter write = new StreamWriter(@"C:\Users\user\AppData\Roaming\Autodesk\Revit\Addins\2016\Buffer.txt", false);
            write.Write(familyPath);
            write.Close();
        }

        private void comboBoxDirectories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            foreach (var item in Directory.GetFiles(DirList[comboBoxDirectories.SelectedIndex]))
            {
                FamilyList.Add(item);                
            }               
            
            //FamilyList.Add( Directory.GetFiles(DirList[comboBoxDirectories.SelectedIndex]));
            //comboBoxFamilies.Items.Clear();
            //string FamilyFiles = string.Empty;

            listView.Items.Clear();

            foreach (var item in FamilyList)
            {
                int indexSlash = item.LastIndexOf("\\") + 1;
                string FamilyName = item.Substring(indexSlash);
                FamilyName = FamilyName.Substring(0, FamilyName.Length - 4);
                listView.Items.Add(FamilyName);

                //comboBoxFamilies.Items.Add(FamilyName);
                //FamilyFiles += item + Environment.NewLine;
                //FileWrite(item);               
            }
        }

        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = listView.SelectedIndex;
            //MessageBox.Show(FamilyList[index]);
            FileWrite(FamilyList[index]);
            MessageBox.Show("Press Insert on Ribbon Panel Global 17 tab to insert selected item to project.");
        }

        //private void button_Click(object sender, RoutedEventArgs e)
        //{
        //    listView.Items.Clear();
        //    TaskDialog.Show("Show", "Button is clicked");
        //    StreamReader reader = new StreamReader(@"C:\Users\user\AppData\Roaming\Autodesk\Revit\Addins\2016\Buffer.txt");
        //    string buff;
        //    while ((buff = reader.ReadLine()) != null)
        //    {
        //        listView.Items.Add(buff);
        //    }
        //}

        public class Elektroinstallationen
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string Path { get; set; }
        }
    }
}
