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

[assembly: CommandClass(typeof(ac15.AcadDocReport))]

namespace ac15
{
    /// <summary>
    /// Предназначен для вывода отчетов о свойствах и содержимом чертежа.
    /// </summary>
    public class AcadDocReport
    {
        // ПОЛЯ
        /// <value>doc - ссылка  на активный открытый чертеж AutoCAD</value>
        public Document doc = Application.DocumentManager.MdiActiveDocument;
        // Поле ed - ссылка на Editor активного чертежа
        private Editor ed;

        /// <summary>
        /// Выводит сообщение в ком строку AutoCAD о сойствах активного чертежа
        /// </summary>
        /// <remarks>Следует написать возращающий метод с параментрами и использовать уже здесь.</remarks>
        [CommandMethod("TIExCADdocreport")]
        public void AcadDocGetReport()
        {
            if (doc != null)
            {
                ed = doc.Editor;

                AcadSendMess AcSM = new AcadSendMess();
                AcSM.SendStringDebugStars(new List<string>
                {
                    //"Шаблон текущего чертежа:",
                    //{doc.},
                    "Полный путь к файлу текущего чертежа:",
                    {doc.Database.Filename.ToString()}
                });

                //ed.Command("_.purge"); // 

                //Application.ShowAlertDialog("OK");
            }
        }


    }
}
