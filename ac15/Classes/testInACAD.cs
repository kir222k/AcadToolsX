

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

[assembly: CommandClass(typeof(ac15.TestInACAD))]

namespace ac15
{
    class TestInACAD
    {
        /// <summary>
        /// Проверка метода для отправки сообщений в ком строку AutoCAD 
        /// </summary>
        [CommandMethod("TIExAcSend")]
        public void TIExAcSend()
        {
            AcadSendMessDebug AcSM = new AcadSendMessDebug("TIExAcSend", $"{this}");
            AcSM.SendStringDebug("Тест!!!");



        }


    }

}
