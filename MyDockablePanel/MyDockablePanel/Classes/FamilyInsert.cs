using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB.Structure;
using System.IO;

//Insert Family instance to project from selected .rfa

namespace MyDockablePanel
{
    [Transaction(TransactionMode.Manual)]
   // [Regeneration(RegenerationOption.Manual)]
    public class FamilyInsert : IExternalCommand
    {
        public string FamilyPath { get; set; }       

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;          

            StreamReader reader = new StreamReader(@"C:\Users\user\AppData\Roaming\Autodesk\Revit\Addins\2016\Buffer.txt");
            FamilyPath = reader.ReadToEnd();
            reader.Close();
            Family family = null;
            // TaskDialog.Show("Family Path", FamilyPath);

            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfCategory(BuiltInCategory.OST_ElectricalFixtures);
            collector.OfClass(typeof(FamilySymbol));

            //List<string> Symbols = null;
            //foreach (var item in collector)
            //{                
            //    if (item.Name != "GFCI" &&
            //        item.Name != "Standard" &&
            //        item.Name != "Plain")
            //        Symbols.Add(item.Name);
            //}

            //foreach (var item in Symbols)
            //{
            //    FileWriter(item);
            //}
            //TaskDialog.Show("Family symbols", Symbols);

            FamilySymbol symbol = collector. FirstElement() as FamilySymbol;

            TaskDialog.Show("Family Path", FamilyPath);

            using (var trans = new Transaction(doc, "Insert Transaction"))
            {
                trans.Start();
               
               
                if (!doc.LoadFamily(FamilyPath, out family))
                {
                    TaskDialog.Show("Loading", "Unable to load " + family);
                }

                ISet<ElementId> familySymbolId = family.GetFamilySymbolIds();

                foreach (ElementId id in familySymbolId)
                {
                    symbol = family.Document.GetElement(id) as FamilySymbol;
                }
                trans.Commit();
            }
            TaskDialog.Show("Symbol", symbol.Name);
            uidoc.PromptForFamilyInstancePlacement(symbol);           
            return Result.Succeeded;
        }

        private void FileWriter (string data)
        {
            StreamWriter writer = new StreamWriter(@"C:\Users\user\AppData\Roaming\Autodesk\Revit\Addins\2016\Buffer2.txt", false);
            writer.Write(data);
            writer.Close();
        }
    }
}
