using System;
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
using ACADTOOLSX.Classes.Sys;


namespace ACADTOOLSX.GUI.Model
{
    internal class ViewBaseControl 
    {

        internal CustomPaletteSetAcad PalSet;
        //internal SizePaletteSet SizePal;
        internal MzBaseWindow BaseWindow;

        //список документов
        internal readonly List<AcDocsData> AcDocsData = new List<AcDocsData>();

        /// <summary>
        /// Создание палитры и подписка на события
        /// </summary>
        /// <remarks>Выполняется из Init.cs </remarks>
        internal  void ViewBaseCreate()
        {
            if (PalSet == null)
            { 
                BaseWindow = new MzBaseWindow();

                // Подпишем наш метод на событие в форме MzBaseWindow
                UpdateGridEventHandler MzAddStickDel = new UpdateGridEventHandler(UpdateGrid);
                BaseWindow.UpdateGridEvent += MzAddStickDel;



                // Создадим палитру и вставим в нее MzBaseWindow
                //SizePaletteSet SizePal =new SizePaletteSet();
                PalSet = new CustomPaletteSetAcad(
                    "FILE-EXPLODE", new Guid("A4328512-75F5-4CBD-82D1-0D5CE09BD348"), WidthPaletteSet.WidthBig, HeigthPaletteSet.HeightBig, BaseWindow, "Control");
                PalSet.PaletteSetCreate();
            }
            else
            {
                PalSet.PaletteSetCreate();
            }

        }

        internal  void UpdateGrid()
        {
            // очистим грид 
            AcDocsData.Clear();
            BaseWindow.AcDocsGrid.ItemsSource = null;
            BaseWindow.AcDocsGrid.Items.Clear();
            BaseWindow.AcDocsGrid.Items.Refresh();

            AcadSendMess AcSM = new AcadSendMess();
            try
            {
                // загоним в грид
                foreach (Document AcDoc in AcadApp.DocumentManager)
                {
                    AcDocsData AcData = new AcDocsData();
                    AcData.PathAcDoc = AcDoc.Name;
                    AcData.SelectState = false;
                    AcDocsData.Add(AcData);
                }
            }
            catch (acad.Runtime.Exception ea)
            {
                AcSM.SendStringDebugStars(new List<string> { ea.Message });
            }
            catch (System.Exception e)
            {
                AcSM.SendStringDebugStars(new List<string> { e.Message});
            }
            BaseWindow.AcDocsGrid.ItemsSource = AcDocsData;
            BaseWindow.AcDocsGrid.Items.Refresh();
        }

    }

    internal class AcDocsData
    {
        public bool SelectState { get; set; }
        public string PathAcDoc { get; set; }
    }

}
