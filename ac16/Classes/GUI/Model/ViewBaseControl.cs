﻿using System;
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
using System.IO;
using System.Threading;
//using Atalasoft.Converters;

namespace ACADTOOLSX.GUI.Model
{
    internal class ViewBaseControl 
    {
        internal CustomPaletteSetAcad PalSet;
        internal MzBaseWindow BaseWindow;

        //список документов
        internal  List<AcDocsData> AcDocsDataList = new List<AcDocsData>();
        //список отмеченных документов (их путей)
         List<string> PathDocCheckList = new List<string>();

        UpdateAfterCheckEventHandler UpdateAfterCheckEvHdr;
        UpdateAfterUnCheckEventHandler UpdateAfterUnCheckEvHdr;

        GridRowSelectEventHandler GridRowSelectEvHdr;

        /// <summary>
        /// Создание палитры и подписка на события
        /// </summary>
        /// <remarks>Выполняется из Init.cs </remarks>
        internal  void ViewBaseCreate()
        {
            PathDocCheckList.Add("");

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

                // Подпишем наш метод  на событие 
                UpdateAfterCheckEvHdr = new UpdateAfterCheckEventHandler(UpdateGridAfterCheck);
                BaseWindow.UpdateAfterCheckEvent+= UpdateAfterCheckEvHdr;

                // Подпишем наш метод  на событие 
                UpdateAfterUnCheckEvHdr = new UpdateAfterUnCheckEventHandler(UpdateGridAfterUnCheck);
                BaseWindow.UpdateAfterUnCheckEvent += UpdateAfterUnCheckEvHdr;

                // Подпишем наш метод  на событие 
                GridRowSelectEvHdr = new GridRowSelectEventHandler(GridRowSelect);
                BaseWindow.GridRowSelectEvent += GridRowSelectEvHdr;

                // Создадим палитру и вставим в нее MzBaseWindow
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
            PathDocCheckList.RemoveAll(s => s !="");

            BaseWindow.UpdateAfterCheckEvent -= UpdateAfterCheckEvHdr;
            BaseWindow.UpdateAfterUnCheckEvent -= UpdateAfterUnCheckEvHdr;
            BaseWindow.GridRowSelectEvent -= GridRowSelectEvHdr;

            // очистим грид 
            AcDocsDataList.Clear();
            BaseWindow.AcDocsGrid.ItemsSource = null;
            BaseWindow.AcDocsGrid.Items.Clear();
            BaseWindow.AcDocsGrid.Items.Refresh();

            AcadSendMess AcSM = new AcadSendMess();
            // загоним в грид
            foreach (Document AcDoc in AcadApp.DocumentManager)
            {
                AcDocsData AcData = new AcDocsData
                {
                    PathAcDoc = AcDoc.Name,
                    SelectState = false
                };

                AcDocsDataList.Add(AcData);
            }

            BaseWindow.AcDocsGrid.ItemsSource = AcDocsDataList;
            BaseWindow.AcDocsGrid.Items.Refresh();

            BaseWindow.UpdateAfterCheckEvent += UpdateAfterCheckEvHdr;
            BaseWindow.UpdateAfterUnCheckEvent += UpdateAfterUnCheckEvHdr;
            BaseWindow.GridRowSelectEvent += GridRowSelectEvHdr;
        }

        internal void SelectAllRowGrid()
        {
            BaseWindow.UpdateAfterCheckEvent -= UpdateAfterCheckEvHdr;
            BaseWindow.UpdateAfterUnCheckEvent -= UpdateAfterUnCheckEvHdr;

            List<string> PathDocCheckListX = new List<string>();
                        
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
                // заполним коллекцию путей отмеченных документов
                PathDocCheckListX.Add(Ad.PathAcDoc);
            }

            // очистим грид 
            AcDocsDataList.Clear();
            BaseWindow.AcDocsGrid.ItemsSource = null;
            BaseWindow.AcDocsGrid.Items.Clear();

            AcDocsDataList = AcDocsDataListx;
            BaseWindow.AcDocsGrid.ItemsSource = AcDocsDataList;
            BaseWindow.AcDocsGrid.Items.Refresh();

            if (PathDocCheckList != null)
            {
                PathDocCheckList.Clear();
                PathDocCheckList = PathDocCheckListX;
            }
            else { PathDocCheckList = PathDocCheckListX; }

            BaseWindow.UpdateAfterCheckEvent += UpdateAfterCheckEvHdr;
            BaseWindow.UpdateAfterUnCheckEvent += UpdateAfterUnCheckEvHdr;
        }

        internal  void ExplodeDrawing()
        {
            AcadSendMess AcSM = new AcadSendMess();
            if (PathDocCheckList!=null)
            {

                //AcSM.SendStringDebugStars(new List<string> { PathDocCheckList.Count.ToString() });

                DocumentCollection docs = AcadApp.DocumentManager;

                //foreach (string item in PathDocCheckList)
                //{
                List<Document> listAcDocs = new List<Document>();
                //AcSM.SendStringDebugStars(new List<string> { item });

                // по списку отмеченных чертежей
                foreach (string checkedDrawing in PathDocCheckList)
                {

                    // получим список документов, соотв. списку отм. чертежей.
                    foreach (Document doc in docs)
                    {
                        //if ((checkedDrawing == doc.Name) && (!checkedDrawing.Contains(doc.Name) ))
                        if ((checkedDrawing == doc.Name) && (!listAcDocs.Contains(doc) ))
                        {
                            //if (listAcDocs)
                            listAcDocs.Add(doc);

                        }
                    }
                }
                

            //}
                // список чертежей для работы
                //foreach (Document dd in listAcDocs)
                //{

                //    AcSM.SendStringDebugStars(new List<string> { dd.Name});
                //}

                //List<Document> listAcDocsX = new List<Document>();
                // IEnumerable<Document> listAcDocsX = listAcDocs.Distinct();

                // var results = table2.GroupBy(x => x.ParamId).Select(x => x.First()).ToList();
                // var listAcDocsX = listAcDocs.GroupBy(x => x.Name).Select(x => x.First()).ToList();

                //AcSM.SendStringDebugStars(new List<string> { "******************************" });
                // по списку документов
                foreach (Document checkedDoc in listAcDocs)
                {


                    //AcadSendMess AcSM = new AcadSendMess();
                    //AcSM.SendStringDebugStars(new List<string> { checkedDoc.Name , "new"});

                    // Atalasoft.Converters.RectangleFTypeConverter cw = new RectangleFTypeConverter();


                    // список созданных из листов файлов
                    List<PathDrawingWithLaysForDelete> listFilesFromLays = new List<PathDrawingWithLaysForDelete>();
                        
                    DrawingLayouts DrLays = new DrawingLayouts(checkedDoc);


                    // по списку листов
                    foreach (Layout lay in DrLays.GetListDrawingLayouts())
                    {
                        // Берем имя листа и сохраняем документ с именем листа.
                        if ((lay.LayoutName != "Model") && (lay.LayoutName != Path.GetFileNameWithoutExtension (checkedDoc.Name) ))
                        {
                            ////  acDoc.Database.SaveAs(strDWGName, true, DwgVersion.Current, acDoc.Database.SecurityParameters);
                            //checkedDoc.Database.SaveAs(System.IO.Path.GetDirectoryName(checkedDoc.Name)+                           }
                            string nameLayForSave = lay.LayoutName;

                            string newPathFile = System.IO.Path.GetDirectoryName(checkedDoc.Name) + "\\" + lay.LayoutName + ".dwg";
                            string newPathFileX = System.IO.Path.GetFullPath(newPathFile);
                            //AcadSendMess AcSM = new AcadSendMess();
                            /*
                            AcSM.SendStringDebugStars(new List<string> {
                                //checkedDoc.Name,
                                newPathFileX

                            }); */

                            // сделаем лист, кот. совпадает с новым файлом текущим
                            // всегда блокируем документ для операций, кот. его изменяют (Ривиллис гуру)
                            // https://adn-cis.org/forum/index.php?topic=7794.0

                            Database currentDatabase = HostApplicationServices.WorkingDatabase;
                            var checkDb = checkedDoc.Database;
                            using (DocumentLock dLock2 = checkedDoc.LockDocument()) 
                            {
                                Transaction tr = checkDb.TransactionManager.StartTransaction();
                                using (tr)
                                {
                                    DBDictionary layoutDic
                                        = tr.GetObject(
                                                        checkedDoc.Database.LayoutDictionaryId,
                                                        OpenMode.ForWrite,
                                                        false, false
                                                      ) as DBDictionary;

                                    foreach (DBDictionaryEntry entry in layoutDic)
                                    {
                                        string nameLay = entry.Key;
                                        // Если текущее имя листа 
                                        if (nameLayForSave == nameLay)
                                        {
                                            if (HostApplicationServices.WorkingDatabase != checkDb)
                                                HostApplicationServices.WorkingDatabase = checkDb;
                                            LayoutManager.Current.CurrentLayout = nameLay;

                                            
                                        }
                                    }

                                    tr.Commit();
                                }
                            }
                            if (HostApplicationServices.WorkingDatabase != currentDatabase)
                                HostApplicationServices.WorkingDatabase = currentDatabase;

                            // сохранить новый файл
                            checkedDoc.Database.SaveAs(newPathFileX, DwgVersion.Current);
                            string fileNameLikeLayName = Path.GetFileNameWithoutExtension(newPathFileX);

                            PathDrawingWithLaysForDelete DrLaysDel = new PathDrawingWithLaysForDelete();
                            DrLaysDel.fullPathDrawing = newPathFileX;
                            List<string> delLays = new List<string>();

                            foreach (Layout layZ in DrLays.GetListDrawingLayouts())
                            {
                                if ((layZ.LayoutName != "Model") && (layZ.LayoutName != fileNameLikeLayName))
                                {
                                    delLays.Add(layZ.LayoutName);
                                }
                            }
                            DrLaysDel.listLayoutNamesForDelete = delLays;

                            listFilesFromLays.Add(DrLaysDel);
                        }
                    }

                    // Удалим в производных файлах ненужные листы.
                    foreach (PathDrawingWithLaysForDelete dls in listFilesFromLays)
                    {
                         DeleteLayoutsAsync(dls.fullPathDrawing, dls.listLayoutNamesForDelete);
                    }

                    // удалим в текущем файле ненужные листы
                    Database currentDatabase2 = HostApplicationServices.WorkingDatabase;
                    var checkDb2 = checkedDoc.Database;
                    using (DocumentLock dLock2 = checkedDoc.LockDocument())
                    {
                        Transaction tr = checkDb2.TransactionManager.StartTransaction();
                        using (tr)
                        {
                            DBDictionary layoutDic
                                = tr.GetObject(
                                                checkedDoc.Database.LayoutDictionaryId,
                                                OpenMode.ForWrite,
                                                false, false
                                              ) as DBDictionary;

                            foreach (DBDictionaryEntry entry in layoutDic)
                            {
                                string nameLay = entry.Key;
                                // Если текущее имя листа 
                                if (Path.GetFileNameWithoutExtension(checkedDoc.Name) == nameLay)
                                {
                                    if (HostApplicationServices.WorkingDatabase != checkDb2)
                                        HostApplicationServices.WorkingDatabase = checkDb2;
                                    LayoutManager.Current.CurrentLayout = nameLay;


                                }
                            }
                            tr.Commit();
                        }

                        Transaction tr2 = checkDb2.TransactionManager.StartTransaction();
                        using (tr2)
                        {
                            DBDictionary layoutDic
                                = tr2.GetObject(
                                                checkedDoc.Database.LayoutDictionaryId,
                                                OpenMode.ForWrite,
                                                false, false
                                              ) as DBDictionary;

                            foreach (DBDictionaryEntry entry in layoutDic)
                            {
                                string nameLay = entry.Key;
                                // Если текущее имя листа 
                                if ((Path.GetFileNameWithoutExtension(checkedDoc.Name) != nameLay) &&
                                    (nameLay != "Model"))
                                {
                                    if (HostApplicationServices.WorkingDatabase != checkDb2)
                                        HostApplicationServices.WorkingDatabase = checkDb2;
                                    LayoutManager.Current.DeleteLayout(nameLay);


                                }
                            }
                            tr2.Commit();
                        }



                    }


                    if (HostApplicationServices.WorkingDatabase != currentDatabase2)
                        HostApplicationServices.WorkingDatabase = currentDatabase2;

                    checkedDoc.CloseAndSave(checkedDoc.Name);


                    //// checkedDoc.Database.SaveAs(checkedDoc.Name, DwgVersion.Current);
                    //using (DocumentLock dLock3 = checkedDoc.LockDocument())
                    //{

                    //    using (checkedDoc.Database)
                    //    {

                    //    }
                    //}
                }

            }
        }

        private string DeleteLayout(string fileName, string layoutName)
        {
            Database currentDatabase = HostApplicationServices.WorkingDatabase;
            try
            {
                using (Database targetDatabase = new Database(false, true))
                {
                    targetDatabase.ReadDwgFile(fileName, System.IO.FileShare.ReadWrite, false, null);
                    HostApplicationServices.WorkingDatabase = targetDatabase;
                    LayoutManager lm = LayoutManager.Current;
                    lm.DeleteLayout(layoutName);
                    targetDatabase.Save();
                }
                return "Delete layout succeeded";
            }
            catch (System.Exception ex)
            {
                return "\nDelete layout failed: " + ex.Message;
            }
            finally
            {
                HostApplicationServices.WorkingDatabase = currentDatabase;
            }
        }


        private async void DeleteLayoutsAsync(string fileName, List<string> layoutNames)
        {
            Database currentDatabase = HostApplicationServices.WorkingDatabase;
            try
            {
                using (Database targetDatabase = new Database(false, true))
                {
                    //await Task.Run(() =>
                    //{
                        //targetDatabase.ReadDwgFile(fileName, System.IO.FileShare.ReadWrite, false, null);

                        // Открываем файл на чтение.
                        targetDatabase.ReadDwgFile(fileName, FileOpenMode.OpenForReadAndAllShare, false, null);
                        //targetDatabase.ReadDwgFile(fileName, FileOpenMode.OpenForReadAndWriteNoShare, false, null);
                    //});

                    // Делаем рабочей базой БД из открытого файла.
                    HostApplicationServices.WorkingDatabase = targetDatabase;
                    // Хрен знает что такое.
                    targetDatabase.CloseInput(true);

                    /*
                    // Cделаем лист, кот. совпадает с именем файла текущим.
                    using (DocumentLock dLock = )
                    using (Transaction tr2 = targetDatabase.TransactionManager.StartTransaction())
                    { // Получаем словарь листов.
                        DBDictionary layoutDic2
                            = tr2.GetObject(
                                            targetDatabase.LayoutDictionaryId,
                                            OpenMode.ForWrite,
                                            false
                                          ) as DBDictionary;
                        // Проход по словарю.
                        foreach (DBDictionaryEntry entry2 in layoutDic2)
                        {
                            // Получим имя листа.
                            string nameLayDel = entry2.Key;
                            // Если список листов на удаление содержит текущее имя из коллекции.
                            if (layoutNames.Contains(nameLayDel))
                            {
                                //ObjectId layoutId = entry.Value;
                                //lm.SetCurrentLayoutId(layoutId);
                                // Удаляем слой.
                                LayoutManager.Current.DeleteLayout(nameLayDel);
                            }
                        }
                        // Завершаем транзакцию.
                        tr2.Commit();
                    }
                    */



                    // Транзакция.
                    using (Transaction tr2 = targetDatabase.TransactionManager.StartTransaction())
                    { // Получаем словарь листов.
                        DBDictionary layoutDic2
                            = tr2.GetObject(
                                            targetDatabase.LayoutDictionaryId,
                                            OpenMode.ForWrite,
                                            false
                                          ) as DBDictionary;
                        // Проход по словарю.
                        foreach (DBDictionaryEntry entry2 in layoutDic2)
                        {
                            // Получим имя листа.
                            string nameLayDel = entry2.Key;
                            // Если список листов на удаление содержит текущее имя из коллекции.
                            if (layoutNames.Contains(nameLayDel))
                            {
                                //ObjectId layoutId = entry.Value;
                                //lm.SetCurrentLayoutId(layoutId);
                                // Удаляем слой.
                                LayoutManager.Current.DeleteLayout(nameLayDel);
                            }
                        }
                        // Завершаем транзакцию.
                        tr2.Commit();
                    }

                    #region INFO
                    // https://forums.autodesk.com/t5/net/remove-layout/td-p/5824050

                    // https://adn-cis.org/forum/index.php?topic=2723.0

                    // https://stackoverflow.com/questions/32113068/how-to-insert-an-entity-in-a-specific-layout-in-autocad-using-net

                    // https://www.keanw.com/2007/09/driving-a-basic.html

                    // https://spiderinnet1.typepad.com/blog/2013/03/autocad-net-read-dwg-into-memory-using-databasereaddwgfile.html

                    // https://forums.autodesk.com/t5/net/readdwgfile-efilesharingviolation/td-p/2076678
                    #endregion

                    //await Task.Run(() =>
                    //{
                    targetDatabase.SaveAs(fileName, DwgVersion.Current);
                    //targetDatabase.Save();
                    //});
                }
            }

            #region CATCH
            catch (Autodesk.AutoCAD.Runtime.Exception exa)
            {
                //AcadSendMess AcSM = new AcadSendMess();
                //AcSM.SendStringDebugStars(new List<string> {exa.ToString() });
                HostApplicationServices.WorkingDatabase = currentDatabase;
            }

            catch (System.Exception ex)
            {
               // return "\nDelete layout failed: " + ex.Message;
            }
            finally
            {
                HostApplicationServices.WorkingDatabase = currentDatabase;
            }
            #endregion
        }



        internal void UpdateGridAfterCheck(int row, int col)
        {
            // if (!list.Exists(x => x.ID == 1))
            AcadSendMess AcSM = new AcadSendMess();

            if (PathDocCheckList!=null)
            {
                // если отмеченного файла нет в списке путей
                if(!PathDocCheckList.Exists(x => x.Equals(AcDocsDataList[row].PathAcDoc)))
                {
                    // добавим путь
                    PathDocCheckList.Add(AcDocsDataList[row].PathAcDoc);
                    //AcSM.SendStringDebugStars(new List<string> { "!PathDocCheckList.Exists добавляется путь"}); 
                }
            }
        }
       
        internal void UpdateGridAfterUnCheck (int row, int col)
        {
            AcadSendMess AcSM = new AcadSendMess();
            // если пути в списке нет
            if (PathDocCheckList != null)
            {
                // если отмеченный файл есть в списке путей
                if (PathDocCheckList.Exists(x => x.Equals(AcDocsDataList[row].PathAcDoc)))
                {
                    PathDocCheckList.RemoveAll(s => s == AcDocsDataList[row].PathAcDoc);
                }
            }
        }
    
        internal void GridRowSelect (int row)
        {
            //DrawingLayouts DrLays = new DrawingLayouts();
            //Document currDoc = new Document();

            BaseWindow.ListDrawingLayouts.Items.Clear();
            //BaseWindow.ListDrawingLayouts.Items.Add(row.ToString() + DrLays.ToString());

            // получить значение-путь по номеру строки (row)
            string pathFile = AcDocsDataList[row].PathAcDoc;
            //BaseWindow.ListDrawingLayouts.Items.Add(pathFile);

            DocumentCollection docs = AcadApp.DocumentManager;

            foreach (Document doc in docs)
            {
                //Application.ShowAlertDialog(doc.Name);
                if (doc.Name == pathFile) 
                {
                    //currDoc = doc;
                    DrawingLayouts DrLaysX = new DrawingLayouts(doc);

                    foreach (Layout lay in DrLaysX.GetListDrawingLayouts())
                    {
                        if (lay.LayoutName != "Model")
                        {
                            BaseWindow.ListDrawingLayouts.Items.Add(lay.LayoutName);
                        }
                    }
                }
            }



        }
    }

    public class AcDocsData : IAcDocsData
    {
        public bool SelectState { get; set; }
        public string PathAcDoc { get; set; }
    }

}
