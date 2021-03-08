﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Acad
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.Windows;
using AdW = Autodesk.Windows;
using acadApp = Autodesk.AutoCAD.ApplicationServices.Application;
using ACADTOOLSX.GUI.Model;

namespace ACADTOOLSX
{
    static class AcadComponentManagerInit
    {

        /// <summary>
        /// Подключение обоработчика к событию создания ленты, для автоподключения нашей вкладки
        /// </summary>
        internal static void AcadComponentManagerInit_ConnectHandler()
        {
            Autodesk.Windows.ComponentManager.ItemInitialized +=
                new EventHandler<RibbonItemEventArgs>(AcadComponentManager_ItemInitialized);
        }

        /// <summary>
        /// Автосоздание вкладки ленты.
        /// </summary>
        internal static void AcadComponentManager_ItemInitialized(object sender, Autodesk.Windows.RibbonItemEventArgs e)
        {
            // Создать и показать палитру
            var ViewBase = new ViewBaseControl();
            ViewBase.ViewBaseCreate();


            // Создать и загрузить вкладку
            //SampleCreateRibbonTabClass2 SampleRibTab =
            //    new SampleCreateRibbonTabClass2();
            //SampleRibTab.TiexTestRibCreate3();
            RibbonTabBuildDDEMZ RibTab = new RibbonTabBuildDDEMZ(ViewBase);
            RibTab.RibbonTabBuild();



            // Отключить обработчик загрузки ленты, т.к. он вызвается
            // только 1 раз при инициализации DLL.
            ComponentManager.ItemInitialized -=
                new EventHandler<RibbonItemEventArgs>(AcadComponentManager_ItemInitialized);
        }
    }

}
