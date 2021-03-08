using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.Windows;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using Autodesk.AutoCAD.ApplicationServices;
using AcadApp = Autodesk.AutoCAD.ApplicationServices.Application;
using Autodesk.AutoCAD.DatabaseServices;
using acad = Autodesk.AutoCAD;
using TIExCAD.Generic;

namespace ACADTOOLSX.Classes.Sys
{
    public  static class WindowsCollection
    {
       // [CommandMethod("DDEGETWINDOWCOLLECTION")]
        public static List<Document> GetAcadAppWindowCollection()
        {
            //AcadSendMess AcSM = new AcadSendMess();

            List<Document> AcDocRes =null;
            foreach (Document Adoc in AcadApp.DocumentManager)
            {
                AcDocRes.Add(Adoc);

                //AcSM.SendStringDebugStars(new List<string> {

                //    Adoc.ToString(),
                //    Adoc.Name,
                //   // Adoc.Database.Filename,
                //    Adoc.Database.Insbase.ToString(),
                //    Adoc.Database.VersionGuid


                //});
            }

            return AcDocRes;
        }

    }
}
