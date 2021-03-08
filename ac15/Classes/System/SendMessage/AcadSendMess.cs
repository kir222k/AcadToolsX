using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;



// [assembly: CommandClass(typeof(ac15.AcadSendMess))]
//
namespace ac15
{
    /// <summary>
    /// Класс для работы с сообщениями в коммандной строке AutoCAD.
    /// </summary>
    public class AcadSendMess
    {
        // ПОЛЯ
        /// <value>doc - ссылка  на активный открытый чертеж AutoCAD</value>
        public Document doc = Application.DocumentManager.MdiActiveDocument;
        // Поле ed - ссылка на Editor активного чертежа
        private Editor ed;

        // МЕТОДЫ
        /// <summary>
        /// Отправка простого сообщения в ком.строку AutoCAD
        /// </summary>
        public virtual void SendStringDebug(string messText)
        {
            // Проверим, что ссылка на текущий чертеж реальная 
            if (doc != null)
            {
                // получаем в ed Editor текущего чертежа
                ed = doc.Editor;
                // Шлем сообщение
                ed.WriteMessage(messText);
            }
        }

        /// <summary>
        /// Отправка сообщения в ком.строку AutoCAD в обрамлении * и надписи DEBUG
        /// </summary>
        public void SendStringDebugStars(string messText)
        {
            if (doc != null)
            {
                doc.Editor.WriteMessage(StringMetods.GetStringFromListStars(new List<string> { messText }, false));
            }
        }
        /// <summary>
        /// Отправка сообщения в ком.строку AutoCAD в обрамлении * и надписи DEBUG. Принимает список строк типа List
        /// </summary>
        public void SendStringDebugStars(List<string> listMess)
        {
            if (doc != null)
            {
                doc.Editor.WriteMessage(StringMetods.GetStringFromListStars(listMess, false));
            }
        }

    }

    /// <summary>
    /// Класс наследован от AcadSendMess, используется для отправки составного сообщения: 
    /// используется информация о блоке кода, откуда вызываются методы данного класса.
    /// Предназначен для отладки. Пример создания экземпляра: 
    /// <code> AcadSendMessDebug AcSM = new AcadSendMessDebug("TIExAcSend", $"{this}"); </code>
    /// или
    /// <code>  AcadSendMessDebug AcSM = new AcadSendMessDebug($"{this}"); </code>
    /// </summary>
    internal class AcadSendMessDebug : AcadSendMess // наследуем,  чтобы сразу использовать ссылку на активный чертеж doc
    {

        // СВОЙСТВА

        /// <summary>
        /// Имя метода, откуда посылается сообщение
        /// </summary>
        public string NameSourceMetod { get; set; }
        /// <summary>
        /// Имя класса, откуда посылается сообщение
        /// </summary>
        public string NameSourceClass { get; set; }

        // КОНСТРУКТОРЫ

        /// <summary>
        /// При создании экз класса имя класса=Неизвестно, имя метода=Неизвестно
        /// </summary>
        public AcadSendMessDebug() : this("Неизвестно") { }
        /// <summary>
        /// При создании экз класса требуется задать имя класса источника, имя метода=Неизвестно
        /// </summary>
        public AcadSendMessDebug(string cn) : this("Неизвестно", cn) { }
        /// <summary>
        /// При создании экз класса требуется задать имя метода, имя класса источника сообщения
        /// </summary>
        public AcadSendMessDebug(string mn, string cn) { NameSourceMetod = mn; NameSourceClass = cn; }

        // МЕТОДЫ

        /// <summary>
        /// Отправка  сообщения в ком.строку AutoCAD. Метод переопределен, добавлена информация, откуда шлется сообщение: Класс, Метод
        /// </summary>
        public override void SendStringDebug(string messText)
        {
            // Проверим состояние активного чертежа, соберем список и отправим в AutoCAD
            if (doc != null) // doc определен в базовом классе, просто используем
            {
                // строка Класс
                string strClass = "Класс:     ";
                // строка Метод
                string strMetod = "Метод:     ";
                // строка Сообщение
                string strMess = "Сообщение: ";

                // строка Класс+
                string strClassFull = strClass + NameSourceClass;
                // строка Метод+
                string strMetodFull = strMetod + NameSourceMetod;
                // строка Сообщение+
                string strMessFull = strMess + messText;

                // Сформируем список
                List<string> listText = new List<string> { strClassFull, strMetodFull, strMessFull };

                // Получим строку с обрамлением *
                string stringStars = StringMetods.GetStringFromListStars(listText, true);

                // Отправим сообщение в ком строку AutoCAD
                doc.Editor.WriteMessage(stringStars);
            }
        }


    }
}
