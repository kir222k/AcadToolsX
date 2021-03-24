//  Файл инициализации библиотеки
// https://overcoder.net/q/236774/%D0%B4%D0%BE%D0%B1%D0%B0%D0%B2%D0%B8%D1%82%D1%8C-resourcedictionary-%D0%B2-%D0%B1%D0%B8%D0%B1%D0%BB%D0%B8%D0%BE%D1%82%D0%B5%D0%BA%D1%83-%D0%BA%D0%BB%D0%B0%D1%81%D1%81%D0%BE%D0%B2
// ПОЛЯ
// СВОЙЙСТВА
// КОНСТРУКТОРЫ
// МЕТОДЫ

using System;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using System.Collections.Generic;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;
using System.Reflection;
using Autodesk.Windows;
using acadApp = Autodesk.AutoCAD.ApplicationServices.Application;
using TIExCAD;
using TIExCAD.Generic;
using ACADTOOLSX.GUI.Model;

// This line is not mandatory, but improves loading performances (?)
//[assembly: ExtensionApplication(typeof(ac15.InitSelf))]

namespace ACADTOOLSX
{
    /// <summary>
    /// Запускаемый класс - точка входа.
    /// При загрузке данной dll в AutoCAD выполняется код в методе IExtensionApplication.Initialize()
    /// </summary>
    public class InitSelf : IExtensionApplication
    {
        /// <summary>
        /// Инициализация.
        /// </summary>
        void IExtensionApplication.Initialize()

        {
            InitThis InThis = new InitThis();

            // Вывод данных о приложении в ком строку AutoCAD и регистрация
            //InThis.InitReg();
           // AcadComponentManagerInit.AcadIntefaceLoad();

            // Подключение обработчиков основных событий.
            InThis.BasicEventHadlerlersConnect();

            // Загрузка интерфейса
            InThis.LoadUserInterface();
        }

        /// <summary>
        /// Метод, выполняемый при выгрузке плагина
        /// в нашем случае, при выгрузке экземляра acad.exe
        /// </summary>
        void IExtensionApplication.Terminate()
        {


        }
    }

/// <summary>
/// Дествия при загрузки сборки.
/// </summary>
/// <remarks>Подключение обработчиков основных событий, 
/// Загрузка интерфейса пользователя,
/// чтение ini-файлов, выполнение затем каких-то настроек и др.</remarks>
internal class InitThis
    {
        ViewBaseControl ViewBasePaletteSet;

        internal  void InitReg()
        {
            // Сообщение в ком строку AutoCAD
            AcadSendMess AcSM = new AcadSendMess();
            // Регистрация сборок в автозагрузке AutoCAD.
            RegtoolsCMDF RegCMD = new RegtoolsCMDF(Constantes.ConstNameCustomApp);

            // Проверка регистрации сборки в автозагрузке AutoCAD.
            RegGeneric RegGen = new RegGeneric();
                
            // Вызывается регистрация сборки: 
            if (RegGen.GetRegisterCustomApp(Constantes.ConstNameCustomApp,
                Assembly.GetExecutingAssembly().Location)) // true
                                                            // если регистрация прошла успешно, то уведомляем
            {
                AcSM.SendStringDebugStars("Приложение зарегистрировано. " +
                    "\nПри следуюющем запуске AutoCAD будет загружно автоматически!");
                // выведем список зарег приложений, кот в автозагрузке AutoCAD.
                RegCMD.GetRegistryKeyAppsCMD();

            }
            // Иначе ничего не делаем, т.к. наше приложение уже есть в автозагрузке AutoCAD.
        }

        /// <summary>
        /// Подключение обработчиков основных событий.
        /// </summary>
        internal  void BasicEventHadlerlersConnect()
        {
            // Подключим автосоздание вкладки ленты.
            AcadComponentManagerInit.AcadComponentManagerInit_ConnectHandler();

            // ИЗМЕНЕНИЯ СИСТЕМНЫХ ПЕРЕМЕННЫХ
            // Подключим пересоздание вкладки ленты.
            // В случае вкладки ленты, отслеживается переменная WSCURRENT.
            AcadSystemVarChanged.AcadSystemVariableChanged_ConnectHandler();
        }

        internal void LoadUserInterface()
        {

            // если файла usercadr.ini нет в папке /sys, то загрузка в соотв. с настройками cadr.ini (кот. исп. при инсталяции)
            // usercadr.ini создается при первой запуске окна настроек, или при "сбросить" в онке настроек (заново создается)

            #region ЛЕНТА

            // Загрузка выполняется в методе 
            // AcadComponentManager_ItemInitialized
            // Перезагрузка при смене раб. пр. - в методе
            // AcadSysVarChangedEvHr_WSCURRENT
            #endregion

            // Меню

            // Другие элементы интерфейса

            // ПАЛИТРА.
            ViewBasePaletteSet = new ViewBaseControl();
        }
    }

}
    

