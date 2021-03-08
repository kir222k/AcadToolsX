using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

using System.Reflection;
using Autodesk.AutoCAD.Runtime;
using AcRt = Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;


namespace ac15
{
    /// <summary>
    /// Работа с реестром. Автозапуск.
    /// </summary>
    public class RegGeneric
    {
        /// <summary>
        /// Плучение ключа реестра AutoCAD.
        /// </summary>
        /// <returns>Ключ реестра AutoCAD тип Autodesk.AutoCAD.Runtime.RegistryKey.</returns>
        internal virtual AcRt.RegistryKey GetAcadRegKey()
        {
            string sProdKey = HostApplicationServices.Current.UserRegistryProductRootKey;
            AcRt.RegistryKey regAcadProdKey = AcRt.Registry.CurrentUser.OpenSubKey(sProdKey);
            AcRt.RegistryKey regAcadAppKey = regAcadProdKey.OpenSubKey("Applications", true);
            // Ключ реестра AutoCAD для регистрации своего приложения и вообще для доступа к этому месту реестра.
            return regAcadAppKey;
        }

        /// <summary>
        /// Регистрирует сборку (dll файл) приложения в реестре для ее автозапуска при старте AutoCAD.
        /// </summary>
        /// <param name="nameCustomApp">Имя приложения для регистрации, может быть любое.</param>
        /// <param name="pathAssembly">Путь к dll приложения. Нужно указывать путь именно к той dll, кот. должна быть в автозапуске AutoCAD.</param>
        public virtual bool GetRegirterCustomApp(string nameCustomApp, string pathAssembly)
        {
            // Get the AutoCAD Applications key
            AcRt.RegistryKey regAcadAppKey = GetAcadRegKey();

            // Check to see if the "MyApp" key exists
            //string[] subKeys = regAcadAppKey.GetSubKeyNames();

            foreach (string subKey in GetRegisteredApps())
            {
                // If the application is already registered, exit
                if (subKey.Equals(nameCustomApp))
                {
                    regAcadAppKey.Close();
                    return false;
                }
            }

            // Register the application
            AcRt.RegistryKey regAppAddInKey = regAcadAppKey.CreateSubKey(nameCustomApp);
            regAppAddInKey.SetValue("DESCRIPTION", nameCustomApp, RegistryValueKind.String);
            regAppAddInKey.SetValue("LOADCTRLS", 14, RegistryValueKind.DWord);
            regAppAddInKey.SetValue("LOADER", pathAssembly, RegistryValueKind.String);
            regAppAddInKey.SetValue("MANAGED", 1, RegistryValueKind.DWord);

            regAcadAppKey.Close();

            return true;
        }

        /// <summary>
        /// Отменяет регистрацию сборки (dll файл) приложения в реестре для отмены ее автозапуска при старте AutoCAD.
        /// </summary>
        /// <param name="nameCustomApp">Имя приложения, кот было ранее зарегистрированно для автозапуска.</param>
        public virtual bool GetUnRegisterCustomApp(string nameCustomApp)
        {

            AcRt.RegistryKey regAcadAppKey = GetAcadRegKey();

            // Delete the key for the application
            //string[] subKeys = regAcadAppKey.GetSubKeyNames();
            foreach (string subKey in GetRegisteredApps())
            {
                if (subKey.Equals(nameCustomApp))
                {
                    regAcadAppKey.DeleteSubKeyTree(nameCustomApp);
                    return true;
                }
            }
            regAcadAppKey.Close();
            return false;
        }

        /// <summary>
        /// Формирует и выдает в виде списка List имена зарег. приложений
        /// </summary>
        /// <returns>Список строк-названий зарег. приложений в автозагрузке AutoCAD.</returns>
        public virtual List<string> GetRegisteredApps()
        {
            AcRt.RegistryKey regAcadAppKey = GetAcadRegKey();
            string[] keys = regAcadAppKey.GetSubKeyNames();
            List<string> listKeys = new List<string>();

            foreach (var item in keys)
            {
                listKeys.Add(item);
            }
            regAcadAppKey.Close();

            return listKeys;
        }
    }
}
