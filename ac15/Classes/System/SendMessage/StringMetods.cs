using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ac15
{
    /// <summary>
    /// Статический класс для работы со строками: 
    /// поиск наиболее длинной строки из списка, 
    /// получение длины наиболее длинной строки из списка, 
    /// получить многострочный текст с обрамлением *
    /// </summary>
    internal static class StringMetods
    {
        /// <summary>
        /// Из списка строк выбирает самую длинную, возращает ее длину
        /// </summary>
        internal static int GetLenMaxStringOfList(List<string> listString)
        {
            int lenMaxStr = 0;
            // по списку
            foreach (string sitem in listString)
            {
                lenMaxStr = (sitem.Length > lenMaxStr) ? sitem.Length : lenMaxStr;
                // maxString = (maxString.Length)
            }
            return lenMaxStr;
        }

        /// <summary>
        /// Из списка строк ищет самую длинную
        /// <returns>Строка по длине наибольшая из списка</returns>
        /// </summary>
        internal static string GetMaxStringOfList(List<string> listString)
        {
            string maxStr = "";
            foreach (string sitem in listString)
            {
                maxStr = (sitem.Length > maxStr.Length) ? sitem : maxStr;
            }
            return maxStr;
        }

        /// <summary>
        /// Собирает строку из списка строк с обрамлением.
        /// </summary>
        /// <remarks>
        /// <para>*******************************</para>
        /// <para>*      D   E   B   U   G      *</para>
        /// <para>*******************************</para>
        /// <para>* Строка 1                    *</para>
        /// <para>* Строка 2                    *</para>
        /// <para>* Строка 3                    *</para>
        /// <para>*******************************</para>
        /// </remarks>
        /// <returns> Строка с обрамлением *</returns>
        /// <param name="listString">Список строк</param>
        /// <param name="printHeader">True =  выводить заголовок "Debug"</param>
        internal static string GetStringFromListStars(List<string> listString, bool printHeader)
        {
            // Найдем строку с максимальной длиной, получим ее длину
            int lenMaxStr = GetLenMaxStringOfList(listString);
            string strDebugPrint = "\n| D E B U G";

            if (printHeader)
            {
                // Заголовок

                // Проверим, если самая длинная строка короче заголовка, то 
                lenMaxStr = (lenMaxStr < strDebugPrint.Length) ? strDebugPrint.Length : lenMaxStr;
                Console.WriteLine(lenMaxStr);

                // Добавим в заголовок закрывающий символ
                //string strSpaceDebug = "";
                for (int i = 0; i < lenMaxStr - 9; i++)
                {
                    strDebugPrint = $"{strDebugPrint} ";
                }
                strDebugPrint = strDebugPrint + " |";
                Console.WriteLine(strDebugPrint.Length);
            }

            // Список для обработанных строк
            List<string> listWithStars = new List<string>();
            //foreach (var s in listWithStars) { Write(s); }

            // Разница в длине между самой длинной и текущей строкой
            int deltaSymString;
            // Строка с нужным кол-вом пробелов
            string strSpaceNeed;
            // Добавим необходимое кол-во звездочек в остальные строки и получим нужный список строк
            foreach (string sitem in listString)
            {
                strSpaceNeed = "";
                // Если текущая строка  не равна наиболее длинной
                // https://docs.microsoft.com/ru-ru/dotnet/standard/base-types/comparing
                if (sitem.Length < lenMaxStr)
                {
                    // Разница в длине между самой длинной и текущей строкой
                    deltaSymString = lenMaxStr - sitem.Length;

                    for (int i = 0; i < deltaSymString; i++)
                    {
                        strSpaceNeed = strSpaceNeed + " ";
                    }
                    listWithStars.Add($"\n| {sitem} {strSpaceNeed}|");
                }
                else // Для самой длинной строки
                {
                    listWithStars.Add($"\n| {sitem} |");
                }
            }

            // Превратим в строку
            string strFullWithStars = "";
            foreach (var s in listWithStars) { strFullWithStars = strFullWithStars + s; }


            // Разделитель
            string strTol = "\n----";
            for (int i = 0; i < lenMaxStr; i++)
            {
                strTol = strTol + "-";
            }
            //strTol = strTol + "|";

            if (printHeader)
            {
                return strTol + strDebugPrint + strTol + strFullWithStars + $"{strTol}\n";
            }
            else
            {
                return strTol + strFullWithStars + $"{strTol}\n";
            }
        }

    }
}
