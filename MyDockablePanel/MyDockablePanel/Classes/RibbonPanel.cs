#region Namespaces
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Diagnostics;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using MyDockablePanel.Properties;
#endregion

//Create UI control elements in project (Ribbon panel buttons and related commands)

namespace MyDockablePanel
{
    public class Ribbon : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication app)
        {
            //Allow to create own tab with given name
            app.CreateRibbonTab("Global17"); 
            RibbonPanel G17PanelDebug = app.CreateRibbonPanel("Global17", "Global17");
            string path = Assembly.GetExecutingAssembly().Location;


            //Register Dockable Pane into Revit on startup
            DockablePaneProviderData data = new DockablePaneProviderData();
            MainPage MDW = new MainPage();
            data.FrameworkElement = MDW as System.Windows.FrameworkElement;
            data.InitialState = new DockablePaneState();
            data.InitialState.DockPosition = DockPosition.Tabbed;
            data.InitialState.TabBehind = DockablePanes.BuiltInDockablePanes.ProjectBrowser;
            DockablePaneId dpid = new DockablePaneId(new Guid("{D7C963CE-B7CA-426A-8D51-6E8254D21157}"));
            app.RegisterDockablePane(dpid, "Custom Families Global17", MDW as IDockablePaneProvider);

            //Create Button to show panel
            PushButtonData btnShow = new PushButtonData("ShowPanel", "ShowPanel", path, "MyDockablePanel.ShowPanel");           
            btnShow.LargeImage = GetImage( Resources.green.GetHbitmap());

            PushButtonData btnInfo = new PushButtonData("ShowInfo", "ShowInfo", path, "MyDockablePanel.DBElement");
            btnInfo.LargeImage = GetImage(Resources.green.GetHbitmap());

            PushButtonData btnInsert = new PushButtonData("InsertFamily", "InsertFamily", path, "MyDockablePanel.FamilyInsert");
            btnInsert.LargeImage = GetImage(Resources.green.GetHbitmap());

            //Add button to ribbon tab          
            RibbonItem ri1 = G17PanelDebug.AddItem(btnShow);
            RibbonItem ri2 = G17PanelDebug.AddItem(btnInfo);
            RibbonItem ri3 = G17PanelDebug.AddItem(btnInsert);
            return Result.Succeeded;
        }        

        public Result OnShutdown(UIControlledApplication app)
        {
            return Result.Succeeded;
        }

        private System.Windows.Media.Imaging.BitmapSource GetImage(IntPtr bm)
        {
            System.Windows.Media.Imaging.BitmapSource bmSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                bm,
                IntPtr.Zero,
                System.Windows.Int32Rect.Empty,
                System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());

            return bmSource;
        }
    }
    //Buttons functionality  

    [Transaction(TransactionMode.ReadOnly)]
    public class ShowPanel : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {           
            DockablePaneId dpid = new DockablePaneId(new Guid("{D7C963CE-B7CA-426A-8D51-6E8254D21157}"));           
            DockablePane dp = commandData.Application.GetDockablePane(dpid);                      
            dp.Show();           
            return Result.Succeeded;
        }
    }

  
}