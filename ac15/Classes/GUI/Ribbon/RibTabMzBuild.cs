using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Controls;

//using ac15.Generic;
// Acad
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.Windows;
using AdW = Autodesk.Windows;
using acadApp = Autodesk.AutoCAD.ApplicationServices.Application;

using TIExCAD;
using TIExCAD.Generic;
using ACADTOOLSX.GUI.Model; // ACADTOOLSX.GUI.Model
using ACADTOOLSX.Classes.GUI.Windows;

namespace ACADTOOLSX

{
    internal class RibbonTabBuildDDEMZ
    {

        private ViewBaseControl ViewBase;

        internal RibbonTabBuildDDEMZ() { }
        internal RibbonTabBuildDDEMZ (ViewBaseControl viewBasePaletteSet) { ViewBase = viewBasePaletteSet; }


        /// <summary>
        /// Создание вкладки ленты DDECAD-MZ.
        /// </summary>
        internal void RibbonTabBuild()
        {
            /**
            *    Текст кнопки. 
            *    Показать текст. 
            *    Размер кнопки. 
            *    Ориентация кнопки. 
            *    Показать картинку. 
            *    Имя файла большой картинки. 
            *    Имя файла малой картинки. 
            *    Экземпляр делегата
            */

            /// Объект для работы с лентой.
            CreateRibTabSpeed CrTabSpeed = new CreateRibTabSpeed();

            #region ПАНЕЛЬ 1
            //DelegateRibButtonHandler DelBtn1 = new DelegateRibButtonHandler(MzSticksFormShow);

            List<RibButtonMyFull> listBtn = new List<RibButtonMyFull>
            {
                // 1
                new RibButtonMyFull()
                {
                    //Текст кнопки.
                    ribButtonText = "  Открыть палитру  ",
                    //Показать текст.
                    showText = true,
                    //Размер кнопки.
                    ribButtonSize = RibbonItemSize.Large,
                    //Ориентация кнопки.
                    ribButtonOrientation = Orientation.Vertical,
                    //Показать картинку.
                    showImage = false,
                    //Имя файла большой картинки.
                    ribButtonLargeImageName = "image_large.png",
                    //Имя файла малой картинки. 
                    ribButtonImageName = "image_standart.png",
                    //Экземпляр делегата
                    delegateRibBtnEv = MzStickFormShowRun// GetStaticInfo.SendMessToAcad_test1
                },
                // 2
                /*
                new RibButtonMyFull()
                {
                    //Текст кнопки.
                    ribButtonText = "Настройки",
                    //Показать текст.
                    showText = true,
                    //Размер кнопки.
                    ribButtonSize = RibbonItemSize.Large,
                    //Ориентация кнопки.
                    ribButtonOrientation = Orientation.Vertical,
                    //Показать картинку.
                    showImage = true,
                    //Имя файла большой картинки.
                    ribButtonLargeImageName = "image_large.png",
                    //Имя файла малой картинки. 
                    ribButtonImageName = "image_standart.png",
                    //Экземпляр делегата
                    delegateRibBtnEv = RibbonTabButtonHandlers.MzZonesFormShow // GetStaticInfo.SendMessToAcad_test1
                 },*/
                // 3 кнопка
                new RibButtonMyFull()
                {
                    //Текст кнопки.
                    ribButtonText = "  Исключить из автозагрузки  ",
                    //Показать текст.
                    showText = true,
                    //Размер кнопки.
                    ribButtonSize = RibbonItemSize.Large,
                    //Ориентация кнопки.
                    ribButtonOrientation = Orientation.Vertical,
                    //Показать картинку.
                    showImage = false,
                    //Имя файла большой картинки.
                    ribButtonLargeImageName = "image_large.png",
                    //Имя файла малой картинки. 
                    ribButtonImageName = "image_standart.png",
                    //Экземпляр делегата
                    delegateRibBtnEv = RibbonTabButtonHandlers.MzExcludeFromRegApp // GetStaticInfo.SendMessToAcad_test1
                }
            };
            CrTabSpeed.CreateOrModifityRibbonTab("AC-FILE-EXPLODE", "drawingexplode", "Разделить файлы", listBtn);
            #endregion
        }

        internal void MzStickFormShowRun()
        {
            if (ViewBase != null)
                RibbonTabButtonHandlers.MzSticksFormShow(ViewBase);
            else
            {
                AcadSendMess AcSM = new AcadSendMess();
                AcSM.SendStringDebugStars(new List<string> {"Требуется перезапустить AutoCAD" });

            }
        }
    }
}


/// <summary>
/// Обработчики кнопок ленты.
/// </summary>
internal static class RibbonTabButtonHandlers
{
    internal static void MzSticksFormShow(ViewBaseControl viewBasePaletteSet)
    {
        //AcadSendMess AcSM = new AcadSendMess();
        //AcSM.SendStringDebugStars(new List<string> { "Обработчик", "MzSticksFormShow" });

        // Создать и показать палитру
        //var ViewBase = new ViewBaseControl();
        viewBasePaletteSet.ViewBaseCreate();
        //viewBasePaletteSet.
    }

    internal static void MzZonesFormShow()
    {
        
        //AcadSendMess AcSM = new AcadSendMess();
        //AcSM.SendStringDebugStars(new List<string> { "Обработчик", "MzZonesFormShow" });
       


    }

    internal static void MzExcludeFromRegApp()
    {
        TIExCAD.Generic.RegGeneric RegGen = new RegGeneric();
        if (!RegGen.GetUnRegisterCustomApp(ACADTOOLSX.Constantes.ConstNameCustomApp))
        {
            AcadSendMess AcSM = new AcadSendMess();
            AcSM.SendStringDebugStars(new List<string> { "Регистрация", $"{ ACADTOOLSX.Constantes.ConstNameCustomApp}", "отменена" });
        }
    }


}



