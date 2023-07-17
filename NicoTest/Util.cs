using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace NicoTest
{
    class Util
    {
        public static bool isOkDotNet()
        {
            var ver = Get45PlusFromRegistry();
            return ver >= 4.52;
        }
        public static double Get45PlusFromRegistry()
        {
            const string subkey = @"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\";

            using (RegistryKey ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(subkey))
            {
                if (ndpKey != null && ndpKey.GetValue("Release") != null)
                {
                    return CheckFor45PlusVersion((int)ndpKey.GetValue("Release"));
                    //			Console.WriteLine(".NET Framework Version: " + CheckFor45PlusVersion((int) ndpKey.GetValue("Release")));
                }
                else
                {
                    //			Console.WriteLine(".NET Framework Version 4.5 or later is not detected.");
                }
            }
            return -1;
        }
        private static double CheckFor45PlusVersion(int releaseKey)
        {
            if (releaseKey >= 461808)
                return 4.72; //later
            if (releaseKey >= 461308)
                return 4.71;
            if (releaseKey >= 460798)
                return 4.7;
            if (releaseKey >= 394802)
                return 4.62;
            if (releaseKey >= 394254)
                return 4.61;
            if (releaseKey >= 393295)
                return 4.6;
            if (releaseKey >= 379893)
                return 4.52;
            if (releaseKey >= 378675)
                return 4.51;
            if (releaseKey >= 378389)
                return 4.5;
            return -1;
        }

        // WMIのWin32_OperatingSystemクラスを使用
        // 以下の2つを使う場合は System.Management.dll を参照設定に追加してください
        // OS名、バージョンなどを返します
        public static string CheckOSName()
        {
            string result = "";

            System.Management.ManagementClass mc =
                new System.Management.ManagementClass("Win32_OperatingSystem");
            System.Management.ManagementObjectCollection moc = mc.GetInstances();

            try
            {
                foreach (System.Management.ManagementObject mo in moc)
                {
                    result = mo["Caption"].ToString();
                    if (mo["CSDVersion"] != null)
                        result += " " + mo["CSDVersion"].ToString();
                    result += " (" + mo["Version"].ToString() + "}";
                }
            }
            catch (Exception e)
            {
                //util.debugWriteLine(e.Message + e.Source + e.StackTrace + e.TargetSite);
                return result;
            }

            return result;
        }

        // OSのバージョンを文字列として返します
        // 返す値は直接ソースをみてください
        public static string CheckOSType()
        {
            string result = "";

            System.Management.ManagementClass mc =
                new System.Management.ManagementClass("Win32_OperatingSystem");
            System.Management.ManagementObjectCollection moc = mc.GetInstances();

            try
            {
                foreach (System.Management.ManagementObject mo in moc)
                {
                    if (mo["Version"].ToString().StartsWith("5.1"))
                        result = "XP";
                    else if (mo["Version"].ToString().StartsWith("6.0"))
                        result = "Vista";
                    else if (mo["Version"].ToString().StartsWith("6.1"))
                        result = "7";
                    else if (mo["Version"].ToString().StartsWith("6.2"))
                        result = "8";
                    else if (mo["Version"].ToString().StartsWith("6.3"))
                        result = "8.1";
                    else if (mo["Version"].ToString().StartsWith("10.0"))
                        result = "10";
                    else if (mo["Version"].ToString().StartsWith("11.0"))
                        result = "11";
                    else
                        result = "other";
                }
            }
            catch (Exception e)
            {
                //util.debugWriteLine(e.Message + e.Source + e.StackTrace + e.TargetSite);
                return result;
            }
            return result;
        }

    }
}
