using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;

[assembly: CommandClass(typeof(ac15.AcadDocInfoLayers))]


namespace ac15
{
    /// <summary>
    /// Для вывода данных о слоях.
    /// </summary>
    public class AcadDocInfoLayers
    {
        /// <summary>
        /// Пример работы LINQ с коллекциями чертежа. Афигенно.
        /// </summary>
        [CommandMethod("LINQ")]
        public static void LINQExample()
        {
            dynamic db = HostApplicationServices.WorkingDatabase;
            dynamic doc = Application.DocumentManager.MdiActiveDocument;

            var layers = db.LayerTableId;
            for (int i = 0; i < 2; i++)
            {
                var newrec = layers.Add(new LayerTableRecord());
                newrec.Name = "Layer" + i.ToString();
                if (i == 0)
                    newrec.IsFrozen = true;
                if (i == 1)
                    newrec.IsOff = true;
            }

            var OffLayers = from l in (IEnumerable<dynamic>)layers
                            where l.IsOff
                            select l;

            doc.Editor.WriteMessage("\nLayers Turned Off:");

            foreach (dynamic rec in OffLayers)
                doc.Editor.WriteMessage("\n - " + rec.Name);

            var frozenOrOffNames = from l in (IEnumerable<dynamic>)layers
                                   where l.IsFrozen == true || l.IsOff == true
                                   select l;

            doc.Editor.WriteMessage("\nLayers Frozen or Turned Off:");

            foreach (dynamic rec in frozenOrOffNames)
                doc.Editor.WriteMessage("\n - " + rec.Name);
        }
    }
}
