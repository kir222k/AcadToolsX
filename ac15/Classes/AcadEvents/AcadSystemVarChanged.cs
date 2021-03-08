using System;
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

using TIExCAD;
using TIExCAD.Generic;


namespace ACADTOOLSX
{
    /// <summary>
    /// Работа с событием изменения сист переменных.
    /// </summary>
    static partial class AcadSystemVarChanged
    {
        /// <summary>
        /// Подключение обоработчика к событию изменения системных переменных, 
        /// в т.ч., для переподключения нашей вкладки при смене раб. пр-ва.
        /// </summary>
        internal static void AcadSystemVariableChanged_ConnectHandler()
        {
            acadApp.SystemVariableChanged +=
                new SystemVariableChangedEventHandler(AcadSysVarChangedEventHandler);
        }


        /// <summary>
        /// Обработчик события изменения системных переменных.
        /// </summary>
        internal static void AcadSysVarChangedEventHandler(object sender, Autodesk.AutoCAD.ApplicationServices.SystemVariableChangedEventArgs e)
        {
            // e.Name  - проверим имя переменной
            switch (e.Name)
            {
                case "WSCURRENT": // название текущего раб. простраства.
                    AcadSysVarChangedEvHr_WSCURRENT();
                    break;
                //case 2:
                //    ;
                //    break;
                default:
                    AcadSendMess AcSM = new AcadSendMess();
                    AcSM.SendStringDebugStars(new List<string> {
                        ".." });
                    break;
            }
        }

        internal static void AcadSysVarChangedEvHr_WSCURRENT()
        {
            /// @TODO Нужно разобраться, что это.
            string cmdNames =
            (string)Autodesk.AutoCAD.ApplicationServices.Application.
                GetSystemVariable(
                    "CMDNAMES");

            /// @TODO Нужно разобраться, что это: 
            /// if the QUICKCUI or CUI command is active, returns
            if (cmdNames.ToUpper().IndexOf("QUICKCUI") >= 0 ||
                cmdNames.ToUpper().IndexOf("CUI") >= 0)
                return;


            // Создать и загрузить вкладку
            //SampleCreateRibbonTabClass2 SampleRibTab =
            //    new SampleCreateRibbonTabClass2();
            //SampleRibTab.TiexTestRibCreate3();
            RibbonTabBuildDDEMZ RibTab =
                new RibbonTabBuildDDEMZ();
            RibTab.RibbonTabBuild();


        }

    }


}
