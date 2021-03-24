using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.Windows;
//using Autodesk.AutoCAD.ApplicationServices.Application;
using AdW = Autodesk.Windows;
using acadApp = Autodesk.AutoCAD.ApplicationServices.Application;
using ACADTOOLSX.GUI.Model;
using Autodesk.AutoCAD.DatabaseServices;

namespace ACADTOOLSX
{

    //public class DrawingWithLays : IDrawingLayouts
    //{
    //    //private string fullPathDrawing;
    //    // список полных путей (выбранных в гриде)
    //    private List<AcDocsData> listAcDocs;


    //    public DrawingLayouts(List<AcDocsData> listAcDocs)
    //    {
    //        this.listAcDocs = listAcDocs;
    //    }

    //    /// <summary>
    //    /// Листы чертежа.
    //    /// </summary>
    //    /// <returns>Коллекция типа Чертеж.список_его_листов </returns>
    //    //List<AcDocWithLayouts> IDrawingLayouts.GetListLayouts()
    //    public List<AcDocWithLayouts> GetListLayouts()
    //    {
    //        string fullPathDrawing;
    //        List<string> listLays = new List<string>(); ;
    //        List<AcDocWithLayouts> acDocWithLayouts = new List<AcDocWithLayouts>();

    //        // пройтись по всем открытым чертежам
    //        foreach (AcDocsData acData in listAcDocs)
    //        {
    //            // задать fullPathDrawing
    //            fullPathDrawing = acData.PathAcDoc; // забрали из источника грида пути к выбранным файлам


    //            // задать коллекцию listLays
    //            // по коллекции листов пространства листов 
    //            listLays.Add("Лист1");
    //            listLays.Add("Лист2");
    //            listLays.Add("Лист3");

    //            // задать переменную типа AcDocWithLayouts
    //            AcDocWithLayouts acDocLays;
    //            acDocLays.fullPathDrawing = fullPathDrawing;
    //            acDocLays.listLayouts = listLays;
    //            // добавить к коллекции типа AcDocWithLayouts
    //            acDocWithLayouts.Add(acDocLays);
    //        }

    //        //throw new NotImplementedException();
    //        return acDocWithLayouts;
    //    }
    //}

    ///<inheritdoc/>
    public class DrawingLayouts : IListDrawingLayouts
    {
        //private  List<Layout> listDrawingLayouts;
        private Document acDoc;

        /// <summary>
        /// Коллекция листов чертежа<br/>
        /// <see href="https://docs.microsoft.com/ru-ru/dotnet/csharp/programming-guide/classes-and-structs/properties"/>
        /// </summary>
        
        //public List<Layout> ListDrawingLayouts => listDrawingLayouts;
        public Document AcDoc { get => acDoc; set => acDoc=value; }

        public DrawingLayouts() { }
        public DrawingLayouts(Document acDoc) { this.acDoc = acDoc; }

        public List<Layout> GetListDrawingLayouts()
        {
            List<Layout> listLays = new List<Layout>();
            //Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = acDoc.Database;

            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                DBDictionary layoutDic =
                    tr.GetObject(db.LayoutDictionaryId, OpenMode.ForRead, false) as DBDictionary;

                foreach (DBDictionaryEntry entry in layoutDic)

                {
                    ObjectId layoutId = entry.Value;
                    Layout layout
                        = tr.GetObject(
                                        layoutId,
                                        OpenMode.ForRead
                                      ) as Layout;

                    listLays.Add(layout);
                }

                tr.Commit();
            }
            //listDrawingLayouts = listLays;
            return listLays;
        }

        public override string ToString()
        {
            return "DrawingLayouts.ToString";
        }
    }

}
