using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Get information about item in project by click.

namespace MyDockablePanel
{
    [Transaction(TransactionMode.Manual)]
    public class DBElement : IExternalCommand
    {
        Application my_App;
        Document my_Doc;

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication UI_App = commandData.Application;
            UIDocument UI_Doc = UI_App.ActiveUIDocument;
            my_App = UI_App.Application;
            my_Doc = UI_Doc.Document;

            Reference refPick = UI_Doc. Selection.PickObject(ObjectType.Element, "Pick an element");
            Element elem = my_Doc.GetElement(refPick);
            ShowInfo(elem);
            return Result.Succeeded;
        }

        public void ShowInfo(Element elem)
        {
            string s = "You Picked:" + "\n";
            s += "Class name = " + elem.GetType().Name + "\n";
            s += "Category   = " + elem.Category.Name + "\n";
            s += "Element id = " + elem.Id.ToString() + "\n";
            s += "Family Type = " + elem.Name + "\n";
           
            TaskDialog.Show("Basic Info", s);
        }
    }
}
