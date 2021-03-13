using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACADTOOLSX
{
    /// <summary>
    /// Получить список листов чертежа
    /// </summary>
    interface  IDrawingLayouts
    {
       
        //string FullPathDrawing { get; set; }
        //List<string> ListNameLayouts { get; set; }

        List<AcDocWithLayouts> GetListLayouts();
    }
}
