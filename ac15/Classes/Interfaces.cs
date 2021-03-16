using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.Windows;
using AcWin = Autodesk.AutoCAD.Windows;
using System.Drawing;
using System.Windows;
using SysWin = System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using AcadApp = Autodesk.AutoCAD.ApplicationServices.Application;
using Autodesk.AutoCAD.DatabaseServices;
//using KR.CAD.NET.ACADTOOLSX;
using acad = Autodesk.AutoCAD;
//using System.Windows;
using System.Runtime.Remoting;
using System.Runtime.InteropServices;
using ACADTOOLSX.Classes.GUI.Windows;
using ACADTOOLSX;
using TIExCAD.Generic;


namespace ACADTOOLSX
{
    /// <summary>
    /// Коллекция листов чертежа Autocad<br/>
    /// <see href="https://adn-cis.org/forum/index.php?topic=2738.0"></see><br/>
    /// <see href="https://adndevblog.typepad.com/autocad/2012/05/listing-the-layout-names.html"/>
    /// </summary>
    interface IListDrawingLayouts
    {
        // коллекция листов чертежа
        /// <summary>
        /// Коллекция листов чертежа.
        /// </summary>
        List<Autodesk.AutoCAD.DatabaseServices.Layout> ListDrawingLayouts { get; }

        /// <summary>
        /// Чертеж AutoCAD.
        /// </summary>
        Autodesk.AutoCAD.ApplicationServices.Document AcDoc { get; set; }

        // вернуть коллекцию листов чертежа
        /// <summary>
        /// Возращает коллекцию листов чертежа.
        /// </summary>
        /// <returns>Коллекция листов чертежа.</returns>
        Autodesk.AutoCAD.DatabaseServices.Layout GetListDrawingLayouts();
    }

    interface IAcDocsData
    {
         bool SelectState { get; set; }
         string PathAcDoc { get; set; }
    }
}
