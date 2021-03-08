using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.IO;
using System.Reflection;

// Acad
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.Windows;
using AdW = Autodesk.Windows;
using acadApp = Autodesk.AutoCAD.ApplicationServices.Application;

//[assembly: CommandClass(typeof(ac15.Generic.ExampleRibbon))]


namespace ac15.Generic
{
    /*
     //Удаление своей вкладки с ленты
     // В данном примере не используем

    [CommandMethod("RibDel")]
            public  void RemoveRibbonTab()
            {
                try
                {
                    RibbonControl ribCntrl = Autodesk.Windows.ComponentManager.Ribbon;
                    // Делаем итерацию по вкладкам ленты
                    foreach (RibbonTab tab in ribCntrl.Tabs)
                    {
                        if (tab.Id.Equals("RibbonExample_ID") & tab.Title.Equals("RibbonExample"))
                        {
                            // И если у вкладки совпадает идентификатор и заголовок, то удаляем эту вкладку
                            ribCntrl.Tabs.Remove(tab);
                            // Отключаем обработчик событий
                            acadApp.SystemVariableChanged -= new SystemVariableChangedEventHandler(acadApp_SystemVariableChanged);
                            // Останавливаем итерацию
                            break;
                        }
                    }
                }
                catch (Autodesk.AutoCAD.Runtime.Exception ex)
                {
                    Autodesk.AutoCAD.ApplicationServices.Application.
                      DocumentManager.MdiActiveDocument.Editor.WriteMessage(ex.Message);
                }
            }
        */

    /// <summary>
    /// Создание вкладки на ленте. Основной метод возращает настроенную вклвдку, кот. затем можно встроить в ленту Autodesk.Windows.ComponentManager.Ribbon.Tabs.Add  
    /// </summary>
    public class RibbonCreateEasy
    {
        private string pathImgFolder;
        /// <summary>
        /// Путь к папке с картинками.
        /// </summary>
        public string PathImgFolder
        {
            set
            {
                if (Directory.Exists(value))
                {
                    pathImgFolder = value;
                }
                else
                {
                    pathImgFolder = null;
                }
            }
        }

        /// <summary>
        /// Создание вкладки ленты, пустой, нужно добавить панель(панели), в напели добавить кнопки.
        /// </summary>
        /// <param name="ribbonTabTitle">Заголовок вкладки ленты</param>
        /// <param name="ribbonTabID">ID вкладки ленты</param>
        /// <returns>Ссылка на объект "Вкладка ленты"</returns>
        public RibbonTab GetRibTab(string ribbonTabTitle, string ribbonTabID)
        {
            try
            {
                if (GetIsRibbonTabLoadedRef(ribbonTabTitle, ribbonTabID) == null)
                // добавляем свою вкладку
                {
                    RibbonTab ribTab = new RibbonTab()
                    {
                        Title = ribbonTabTitle,
                        Id = ribbonTabID
                    };

                    return ribTab;
                }
                else
                {
                    return null;
                }
            }
            catch (System.Exception)
            {
                // return null;
                throw;
            }
        }
        /// <summary>
        /// Создание Панели вкладки ленты, пустой, нужно еще добавить кнопки. Саму панель нужно вставить во Вкладку ribbonTab.Panels.Add(ribPanel).
        /// </summary>
        /// <param name="ribPanelTitle"></param>
        /// <returns>Ссылка на объект "Панель кладки ленты"</returns>
        public RibbonPanel GetRibPanel(string ribPanelTitle)
        {
            try
            {
                // создаем panel source
                RibbonPanelSource ribSourcePanel = new RibbonPanelSource()
                {
                    Title = ribPanelTitle
                };

                // теперь саму панель
                RibbonPanel ribPanel = new RibbonPanel()
                {
                    Source = ribSourcePanel
                };

                return ribPanel;
            }

            catch (System.Exception)
            {
                return null;
                throw;
            }
        }

        /// <summary>
        /// Создание кнопки
        /// </summary>
        /// <param name="ribButtonText">Текст</param>
        /// <param name="showText">Показать текст</param>
        /// <param name="ribButtonSize">Размер</param>
        /// <param name="ribButtonOrientation">Ориентация</param>
        /// <param name="delegateRibBtnEv">Делегат на клик по кнопке</param>
        /// <param name="showImage">Показать картинку</param>
        /// <param name="ribButtonLargeImageName">имя файлв большой картинки</param>
        /// <param name="ribButtonImageName"></param>
        /// <returns></returns>
        public RibbonButton GetRibButton(string ribButtonText, bool showText,
            RibbonItemSize ribButtonSize, Orientation ribButtonOrientation,
            DelegateRibButtonHandler delegateRibBtnEv,
            bool showImage,
            string ribButtonLargeImageName = "image_large.png",
            string ribButtonImageName = "image_standart.png") // аргумент не обязательный

        {
            //// Для сообщений
            //AcadSendMess AcSM = new AcadSendMess();

            // Создадим объект кнопки.
            AdW.RibbonButton ribButton = new RibbonButton();

            try
            {
                // Полный путь к файлам картинок:
                string pathDirIm;

                // Проверим путь к папке img. 
                if (pathImgFolder == null || pathImgFolder == String.Empty) // если свойство PathImgFolder не задано.
                {
                    // Если никаких путей нет, то будем искать в папке <InstallDir>/img/ 
                    // Там должны быть файлы image_standart.png, image_large.png. Их мы и подключим.

                    // Зададим путь к папке, откуда запущена Dll.
                    DirectoryInfo dirDLL = new DirectoryInfo(Assembly.GetExecutingAssembly().Location);

                    // Зададим полный путь к папке img:  <InstallDir>/img/ 
                    pathDirIm = $"{dirDLL.Parent.Parent.Parent.FullName}/img/";
                    //AcSM.SendStringDebugStars(new List<string> { "Путь к папке картинок", $"{pathDirIm}" });
                }
                else // если свойство PathImgFolder не нулевое, т.е. мы прописали сами путь к папке картинок, то
                {
                    pathDirIm = pathImgFolder; // присвоим тот путь, кот. задали сами
                }

                // Теперь займемся файлами картинок.
                if (Directory.Exists(pathDirIm)) // Проверим, что в <InstallDir> есть папка img.
                {
                    // получим путь к файлу большой картинки.
                    string pathLargeImg = pathDirIm + ribButtonLargeImageName;
                    // получим путь к файлу малой картинки.
                    string pathImg = pathDirIm + ribButtonImageName;

                    // Проверим существование картинок в папке IMG.
                    if (File.Exists(pathLargeImg) && File.Exists(pathImg))
                    {
                        // создадим объекты BitmapImage
                        BitmapImage bitmapImage32 = new BitmapImage(new Uri(pathLargeImg)); // путь к большой картинке
                        BitmapImage bitmapImage16 = new BitmapImage(new Uri(pathImg));// путь к большой картинке

                        ribButton.Image = bitmapImage16;
                        ribButton.LargeImage = bitmapImage32;
                        ribButton.ShowImage = showImage;
                    }
                    else // Если картинок для кнопки так и не найдено, отключим их прорисовку
                    {
                        ribButton.ShowImage = false;
                        //AcSM.SendStringDebugStars("Картинок для кнопок не найдено");
                    }
                }
                else // если какой-то сбой и пути к картинкам вообще нигде не найдено
                {
                    ribButton.ShowImage = false;
                    //AcSM.SendStringDebugStars("Что-то не так с картинками!");
                }

                // С картинками закончено, настроим др. свойства кнопки.
                ribButton.Text = ribButtonText;
                ribButton.ShowText = showText;
                ribButton.Size = ribButtonSize;
                ribButton.Orientation = ribButtonOrientation;
                ribButton.CommandHandler = new RibBtnHdlrDel(delegateRibBtnEv);

                return ribButton;
            }

            #region CATCH
            catch (System.Exception)
            {

                //throw;
                //AcSM.SendStringDebugStars(new List<string>
                //{$"Создание кнопки {ribButtonText} прервано", "Класс:", $"{this}", "Метод:", "GetRibButton"});

                return null;
            }
            #endregion
        }


        /// <summary>
        /// Проверка существования вклвдки на ленте
        /// </summary>
        /// <param name="ribbonTabTitle">Название вкладки</param>
        /// <param name="ribbonTabID">ID вкладки</param>
        /// <returns>вкладка, тип RibbonTab, если найдена в ленте. Иначе - null</returns>
        public RibbonTab GetIsRibbonTabLoadedRef(string ribbonTabTitle, string ribbonTabID)
        {
            RibbonTab rbTab = null;
            //RibbonControl ribCntrl = Autodesk.Windows.ComponentManager.Ribbon;
            // Делаем итерацию по вкладкам ленты
            foreach (RibbonTab tab in Autodesk.Windows.ComponentManager.Ribbon.Tabs)
            {
                // И если у вкладки совпадает идентификатор и заголовок, то значит вкладка загружена
                if (tab.Id.Equals(ribbonTabID) & tab.Title.Equals(ribbonTabTitle))
                {
                    // return tab;
                    rbTab = tab;
                    // break;
                }
            }
            return rbTab; // вернем ее
        }

        /// <summary>
        /// Проверяет сущ. панели во вкладке.
        /// </summary>
        /// <param name="ribbonTab">Объект вкладки</param>
        /// <param name="ribbonPanelTitle">Заголовок панели</param>
        /// <returns>Объект Панели, если такая содержится в проверяемой вкладке. NULL - если такой панели нет.</returns>
        public RibbonPanel GetIsRibbonPanelLoadedRef(RibbonTab ribbonTab, string ribbonPanelTitle)
        {
            RibbonPanel RibPan = null; // = new RibbonPanel();

            // проверим вкладку, если она вообще не загружена, то и смысла нет проверять панель
            if (GetIsRibbonTabLoadedRef(ribbonTab.Title, ribbonTab.Id) != null) // если вкладка загружена в ленту
            {
                // Делаем итерацию по панелям вклвдки
                foreach (RibbonPanel pan in ribbonTab.Panels)
                {
                    if (pan.Source.Title.Equals(ribbonPanelTitle)) // если такая панель есть на сущ. вкладке.
                    {
                        RibPan = pan;
                        //break;
                    }
                }
            }

            return RibPan; // Если панели нет, возращ. null
        }

        public RibbonButton GetIsRibbonButtonLoadedRef(RibbonTab ribbonTab, RibbonPanel ribbonPanel, string ribbonButtonText)
        {
            RibbonButton RibBtn = null;
            // проверим панель, если она вообще не загружена, то и смысла нет проверять сущ. кнопки
            if (GetIsRibbonPanelLoadedRef(ribbonTab, ribbonPanel.Source.Title) != null)
            {
                foreach (RibbonButton rRtn in ribbonPanel.Source.Items) // проверим кнопки панели
                {
                    if (rRtn.Text.Equals(ribbonButtonText)) // если текст кнопки совпал, вернем эту кнопку.
                    {
                        RibBtn = rRtn;
                    }
                }
            }
            return RibBtn;
        }

    }

    /// <summary>
    /// Быстрое создание вкладки ленты
    /// </summary>
    public class CreateRibTabSpeed
    {
        // КОНСТРУКТОРЫ

        // ПОЛЯ

        /*
        private string ribTabTitle;
        private string ribTabID;
        private List<RibButtonMyShort> listRibButtons;
        */

        // создадим класс по работе с вкладками
        RibbonCreateEasy RibCr = new RibbonCreateEasy();


        // МЕТОДЫ

        internal void CreateOrModifityRibbonTab(string ribbonTabTitle, string ribbonTabID, string ribbonPanelTitle, List<RibButtonMyFull> listRibbonButtons, bool modifityPanel = false)
        {
            // проверим вкладку на существование в ленте
            RibbonTab RibTab = RibCr.GetIsRibbonTabLoadedRef(ribbonTabTitle, ribbonTabID);

            // если такой вкладки нет, 
            if (RibTab == null)
            {
                RibTab = RibCr.GetRibTab(ribbonTabTitle, ribbonTabID); //создадим вкладку
            } // иначе работаем с существующей.

            //  создадим/модифицируем панель.
            CreateOrModifityRibbonPanel(RibTab, ribbonPanelTitle, listRibbonButtons, modifityPanel);

        }

        internal void CreateOrModifityRibbonPanel(RibbonTab ribbonTab, string ribbonPanelTitle, List<RibButtonMyFull> listRibbonButtons, bool modifityPanel)
        {
            //  проверим панель на существование во вкладке. 
            RibbonPanel ribbonPan = RibCr.GetIsRibbonPanelLoadedRef(ribbonTab, ribbonPanelTitle);

            // если такой панели нет, создадим
            if (ribbonPan == null)
            {
                ribbonPan = RibCr.GetRibPanel(ribbonPanelTitle); //создадим панель
            }// иначе работаем с существующей.

            // передаем панель дальше, создаем кнопки
            //если установить  modifityPanel = true, то можно добавить кнопки в сущ. панель.
            CreateOrModifityRibbonButton(ribbonTab, ribbonPan, listRibbonButtons, modifityPanel);
        }

        internal void CreateOrModifityRibbonButton(RibbonTab ribbonTab, RibbonPanel ribbonPan, List<RibButtonMyFull> listRibbonButtons, bool modifityPanel)
        {
            bool isPanelExist; // если вкладка  существует

            // если вкладка  существует
            if (RibCr.GetIsRibbonPanelLoadedRef(ribbonTab, ribbonPan.Source.Title) != null) // панель есть
            {
                isPanelExist = true;
            }
            else { isPanelExist = false; }

            // если вкладка и панель существует и modifityPanel = true   ИЛИ   панели не существует
            if ((isPanelExist && modifityPanel) || (!isPanelExist))
            {

                foreach (RibButtonMyFull RbBtn in listRibbonButtons)
                {
                    // проверим, есть ли такая кнопка
                    if (RibCr.GetIsRibbonButtonLoadedRef(ribbonTab, ribbonPan, RbBtn.ribButtonText) == null)
                    {
                        // если кнопки нет, добавим ее.
                        ribbonPan.Source.Items.Add(
                            RibCr.GetRibButton(
                            RbBtn.ribButtonText, RbBtn.showText,
                            RbBtn.ribButtonSize, RbBtn.ribButtonOrientation,
                            RbBtn.delegateRibBtnEv,
                            RbBtn.showImage,
                            RbBtn.ribButtonLargeImageName, RbBtn.ribButtonImageName
                            ));
                    }
                }
            }

            //  если панели нет 
            if (!isPanelExist)
            {
                // Вставим панель во вкладку.
                ribbonTab.Panels.Add(ribbonPan);
                // добавляем вкладку в ленту, если такой еще не загружено
                if (RibCr.GetIsRibbonTabLoadedRef(ribbonTab.Title, ribbonTab.Id) == null)
                {
                    Autodesk.Windows.ComponentManager.Ribbon.Tabs.Add(ribbonTab);
                }
            }
            // если панель есть, она уже загружена и в коде выше уже добавлены кнопки.

            // Обновим ленту.
            Autodesk.Windows.ComponentManager.Ribbon.UpdateLayout();
        }

    }


    // -----------------------------------------------------------------------------------------------
    /// СТРУКТУРЫ

    /// <summary>
    ///  Кнопка ленты: Текст кнопки. Показать текст. Размер кнопки. Ориентация кнопки. Показать картинку. Имя файла большой картинки. Имя файла малой картинки. Экземпляр делегата
    /// </summary>
    internal struct RibButtonMyFull
    {
        /// <value>
        ///  Текст кнопки.
        /// </value>
        internal string ribButtonText;
        /// <value>
        ///  Показать текст.
        /// </value>
        internal bool showText;
        /// <value>
        ///  Размер кнопки.
        /// </value>
        internal RibbonItemSize ribButtonSize;
        /// <value>
        ///  Ориентация кнопки.
        /// </value>
        internal Orientation ribButtonOrientation;
        /// <value>
        ///  Показать картинку.
        /// </value>
        internal bool showImage;
        /// <value>
        ///  Имя файла большой картинки.
        /// </value>
        internal string ribButtonLargeImageName;
        /// <value>
        ///  Имя файла малой картинки.
        /// </value>
        internal string ribButtonImageName;
        /// <value>
        ///  Экземпляр делегата, кот. содержит методы-обработчики нажатия кнопки.
        /// </value>
        internal DelegateRibButtonHandler delegateRibBtnEv;
    }

}
