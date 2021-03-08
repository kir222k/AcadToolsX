using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ac15
{
    /// <summary>
    /// Содержит статические поля и возращающие мнтоды.
    /// Разработан для хранения необходимых статических данных в теле сборки.
    /// </summary>
    internal static class StaticObjAcadRev
    {
        internal static readonly string Acad2015 = "20.0";
        internal static readonly string Acad2016 = "20.1";
        internal static readonly string Acad2017 = "21.0";
        internal static readonly string Acad2018 = "22.0";
        internal static readonly string Acad2019 = "23.0";
        internal static readonly string Acad2020 = "23.1";
        internal static readonly string Acad2021 = "24.0";

        internal static List<string> GetListAcadReleaseVersions()
        {
            List<string> listVersion = new List<string>
            {
                Acad2015,
                Acad2016,
                Acad2017,
                Acad2018,
                Acad2019,
                Acad2020,
                Acad2021
            };

            return listVersion;
        }

    }
}
