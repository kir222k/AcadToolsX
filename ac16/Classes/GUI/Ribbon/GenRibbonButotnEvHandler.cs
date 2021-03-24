using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Acad
using ac15;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.Windows;
using AdW = Autodesk.Windows;
using acadApp = Autodesk.AutoCAD.ApplicationServices.Application;


namespace ac15.Generic
{
    /// <summary>
    /// Объявление делегата, для использования в привязке  методов к кнопкам на ленте. 
    /// 
    /// В методе, где настраивается кнопка: 
    /// 
    /// 1) создать экз DelegateRibButtonHandler, для каждой кнопки:
    ///             DelegateRibButtonHandler RibBtnMetod;
    /// 2) подключить к экз DelegateRibButtonHandler методы:
    ///             RibBtnMetod = new DelegateRibButtonHandler(MzSticksFormShow);
    /// 3) создать экз RibBtnHdlrDel (см. ниже), аргумент - экз. DelegateRibButtonHandler:
    ///             RibBtnHdlrDel RibBH2 = new RibBtnHdlrDel(RibBtnMetod);
    /// 4) привязать экз RibBtnHdlrDel к свойству CommandHandler кнопки:
    ///             RbB1.CommandHandler = RibBH1;
    /// </summary>
    public delegate void DelegateRibButtonHandler();


    /// <summary>
    /// Содержит переопределяемый метод, кот. выполняется при нажатии кнопки на ленте.
    /// Экз. данного класса (или его наследника) присваивается свойству CommandHandler кнопки ленты.
    /// Для использования в основном коде, чтобы использовать свои методы как обработчики, 
    /// нужно создать класс-наследник от RibButtonEventHandler, переопределить метод Execute
    /// </summary>
    public class RibButtonEventHandler : System.Windows.Input.ICommand
    {
        /// <summary>
        /// Определен в интерфейсе  System.Windows.Input.ICommand, не используется.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            return true;
        }
        /// <summary>
        /// Событие на нажатие кнопки
        /// </summary>
        public event EventHandler CanExecuteChanged;
        /// <summary>
        /// Стандартный метод.
        /// </summary>
        public virtual void Execute(object parameter)
        {
            Document doc = acadApp.DocumentManager.MdiActiveDocument;

            if (parameter is RibbonButton)
            {
                RibbonButton button = parameter as RibbonButton;
                doc.Editor.WriteMessage(
                  "\nRibbonButton Executed: " + button.Text + "\n");
            }
        }
    }

    /// <summary>
    /// К нажатию кнопки привызывается свой метод
    /// </summary>
    public class RibBtnHdlrDel : RibButtonEventHandler
    {
        private DelegateRibButtonHandler DelegateRibBtnEv;
        //public RibBtnHdlrDel() { }        

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="delegateRibBtnEv">Делегат, содержащий методы-обработчики</param>
        public RibBtnHdlrDel(DelegateRibButtonHandler delegateRibBtnEv)
        {
            DelegateRibBtnEv = delegateRibBtnEv;
        }

        /// <summary>
        /// Стандартный метод.
        /// </summary>

        public override void Execute(object parameter)
        {
            DelegateRibBtnEv();
        }
    }




    /// <summary>
    /// Приемы с делегатами.
    /// </summary>
    internal static class TestDelegateStandart
    {
        //delegate  Action TestDelegateAction();

        internal static void Test1()
        {
            Action del;
            del = TestDelegateStandart.Test2;
            del += Test2;
            //new DelegateRibButtonHandler(TestDEl2.Test2);
            del?.Invoke();
        }

        internal static void Test2()
        {
            Console.WriteLine("Test2");
            Console.ReadKey();
        }
        internal static void Test3()
        {

        }

    }

    /// <summary>
    /// Пример работы с вызовом событий.
    /// </summary>
    public class EventGeneratedMY
    {
        public delegate void MyDelegate();
        public event MyDelegate MyEvent;
        void GeneratedEvent(int i)
        {
            if (i > 3)
                MyEvent();
            // или, что одно и то же, но с проверкой делегата на null
            if (i > 3)
                MyEvent?.Invoke();
        }
    }


}
