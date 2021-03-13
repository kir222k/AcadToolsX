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



namespace ACADTOOLSX.GUI.Model
{
    internal class ViewBaseControl 
    {

        internal CustomPaletteSetAcad PalSet;
        //internal SizePaletteSet SizePal;
        internal MzBaseWindow BaseWindow;

        //список документов
        internal  List<AcDocsData> AcDocsDataList = new List<AcDocsData>();

        /// <summary>
        /// Создание палитры и подписка на события
        /// </summary>
        /// <remarks>Выполняется из Init.cs </remarks>
        internal  void ViewBaseCreate()
        {
            if (PalSet == null)
            { 
                BaseWindow = new MzBaseWindow();

                // Подпишем наш метод  UpdateGrid на событие 'update'кнопка в форме MzBaseWindow
                UpdateGridEventHandler UpdateGridEvHdr = new UpdateGridEventHandler(UpdateGrid);
                BaseWindow.UpdateGridEvent += UpdateGridEvHdr;

                // Подпишем наш метод SelectAllRowGrid на событие 'check_all'кнопка грида в форме MzBaseWindow
                SelectAllGridRowsEventHandler SelectGridEvHdr = new SelectAllGridRowsEventHandler(SelectAllRowGrid);
                BaseWindow.SelectAllGridRowsEvent += SelectGridEvHdr;

                // Подпишем наш метод ExplodeDrawing на событие 'ExplodeDrawing'кнопка грида в форме MzBaseWindow
                ExplodeDrawingEventHandler ExplodeDrawingEvHdr = new ExplodeDrawingEventHandler(ExplodeDrawing);
                BaseWindow.ExplodeDrawingEvent += ExplodeDrawingEvHdr;

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
            AcDocsDataList.Clear();
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
                    AcDocsDataList.Add(AcData);
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
            BaseWindow.AcDocsGrid.ItemsSource = AcDocsDataList;
            BaseWindow.AcDocsGrid.Items.Refresh();
        }

        internal void SelectAllRowGrid()
        {
            List<AcDocsData> AcDocsDataListx = new List<AcDocsData>();
            // пройдемся по коллекции м заменим чеки 
            foreach (AcDocsData Ad in AcDocsDataList) 
            {
                //_ = Ad.SelectState == false ? true : false;
                if (Ad.SelectState == false)
                    Ad.SelectState = true;

                AcDocsData AcData = new AcDocsData();
                AcData.PathAcDoc = Ad.PathAcDoc;
                AcData.SelectState = true;

                AcDocsDataListx.Add(AcData);
            }

            // очистим грид 
            AcDocsDataList.Clear();
            BaseWindow.AcDocsGrid.ItemsSource = null;
            BaseWindow.AcDocsGrid.Items.Clear();

            AcDocsDataList = AcDocsDataListx;
            BaseWindow.AcDocsGrid.ItemsSource = AcDocsDataList;
            BaseWindow.AcDocsGrid.Items.Refresh();


            // зададим грид заново


        }

        internal void ExplodeDrawing()
        {
            // BaseWindow.AcDocsGrid.Items.Refresh();
            //BaseWindow.AcDocsGrid.Items.Refresh();
            //BaseWindow.AcDocsGrid.Refresh();

            AcadSendMess AcSM = new AcadSendMess();
            AcSM.SendStringDebugStars(new List<string> { "закомменчен => BaseWindow.AcDocsGrid.Items.Refresh();" });

            List<AcDocsData> listCheckedData = new List<AcDocsData>();
            // выберем только отмеченные в гиде записи
            //foreach (AcDocsData It in AcDocsDataList)
            //{
            //    if (It.SelectState==true)
            //    {
            //        listCheckedData.Add(It);
            //    }
            //}
            listCheckedData = BaseWindow.AcDocsGrid.Items.OfType<AcDocsData>().ToList();

            foreach (var item in listCheckedData)
            {

                //AcadSendMess AcSM = new AcadSendMess();
                AcSM.SendStringDebugStars(new List<string> {
                    item.PathAcDoc,
                    item.SelectState.ToString()

                }); ;
            }

            // List<pojo> list = calendarmstrDG.Items.OfType<pojo>().ToList();
            
            /*
            DrawingLayouts DrLs = new DrawingLayouts(listCheckedData);
            foreach (AcDocWithLayouts It in DrLs.GetListLayouts())
            {

                AcadSendMess AcSM = new AcadSendMess();
                AcSM.SendStringDebugStars(new List<string> {
                It.fullPathDrawing,
                It.listLayouts.ToString(),


                });
            }
            */

            //AcadSendMess AcSM = new AcadSendMess();
            //AcSM.SendStringDebugStars(new List<string> { "ExplodeDrawing" });
        }
    }

    public class AcDocsData
    {
        public bool SelectState { get; set; }
        public string PathAcDoc { get; set; }
    }

}
