using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.Windows;
using AdW = Autodesk.Windows;
using acadApp = Autodesk.AutoCAD.ApplicationServices.Application;
using ACADTOOLSX.GUI.Model;

namespace ACADTOOLSX
{
    /// <summary>
    /// 
    /// </summary>
    public class DrawingLayouts : IDrawingLayouts
    {
        //private string fullPathDrawing;
        // список полных путей (выбранных в гриде)
        private List<AcDocsData> listAcDocs;


        public DrawingLayouts(List<AcDocsData> listAcDocs)
        {
            this.listAcDocs = listAcDocs;
        }

        /// <summary>
        /// Листы чертежа.
        /// </summary>
        /// <returns>Коллекция типа Чертеж.список_его_листов </returns>
        //List<AcDocWithLayouts> IDrawingLayouts.GetListLayouts()
       public  List<AcDocWithLayouts> GetListLayouts()
        {
            string fullPathDrawing;
            List<string> listLays = new List<string>(); ;
            List<AcDocWithLayouts> acDocWithLayouts = new List<AcDocWithLayouts>();

            // пройтись по всем открытым чертежам
            foreach (AcDocsData acData in listAcDocs)
            {
                // задать fullPathDrawing
                fullPathDrawing = acData.PathAcDoc; // забрали из источника грида пути к выбранным файлам


                // задать коллекцию listLays
                // по коллекции листов пространства листов 
                listLays.Add("Лист1");
                listLays.Add("Лист2");
                listLays.Add("Лист3");

                // задать переменную типа AcDocWithLayouts
                AcDocWithLayouts acDocLays;
                acDocLays.fullPathDrawing = fullPathDrawing;
                acDocLays.listLayouts = listLays;
                // добавить к коллекции типа AcDocWithLayouts
                acDocWithLayouts.Add(acDocLays);
            }

            //throw new NotImplementedException();
            return acDocWithLayouts;
        }
    }

    /*
     *  struct AcDocWithLayouts 
    {
        internal string fullPathDrawing;

        internal List<string> listLayouts;
    }
}

    */
}
