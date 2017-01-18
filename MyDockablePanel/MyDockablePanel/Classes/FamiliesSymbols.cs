using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Microsoft.Win32;
using System.IO;

namespace MyDockablePanel
{
    [Transaction(TransactionMode.Manual)]
    class FamiliesSymbols : IExternalCommand
    {
       // static string initialDirectory = @"i:\Project\Revit\HHM Revit Famillien\2016.10.10 Familien Gianfranco\";
      

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Family family = null;
            FamilySymbol symbol = null; 
            List<string> FamList = new List<string>();
            string buff;
        //    List<string> Symbols = null;
            //----------------------------------
            List<string> fam = new List<string>();
            StreamReader reader = new StreamReader(@"C:\Users\user\AppData\Roaming\Autodesk\Revit\Addins\2016\Buffer.txt");          

            while ((buff = reader.ReadLine()) != null)
            {
                FamList.Add(buff);
            }          
            reader.Close();

            string RetrievedFamilies = string.Empty;
            foreach (var item in FamList)
            {
                RetrievedFamilies += item;
            }
            TaskDialog.Show("Families in list", RetrievedFamilies);

            Document doc = commandData.Application.ActiveUIDocument.Document;
            Transaction trans = new Transaction(doc, "Open Family");

            string MySymbol = string.Empty;
            trans.Start();          
            foreach (var item in FamList)
            {
                if (!doc.LoadFamily(item, out family))
                {
                    TaskDialog.Show("Loading", "Unable to load " + item);
                }

                ISet<ElementId> familySymbolId = family.GetFamilySymbolIds();
               
                foreach(ElementId id in familySymbolId)
                {
                    symbol = family.Document.GetElement(id) as FamilySymbol;
                    MySymbol += symbol.Name  +"*\\t"+ symbol.GetTypeId().ToString() + Environment.NewLine;                    
                }
                // TaskDialog.Show("Symbol", MySymbol);
               

            }

          //  TaskDialog.Show("Symbol", MySymbol);
            StreamWriter writer = new StreamWriter(@"C:\Users\user\AppData\Roaming\Autodesk\Revit\Addins\2016\Buffer.txt");
            writer.Write(MySymbol);
            writer.Close();

            trans.RollBack();

            return Result.Succeeded;
        }
    }
}
