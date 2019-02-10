using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Globalization;
using System.Diagnostics;

namespace NotatnikWPF
{
    public static class Settings
    {
        public static void ReadLanguage()
        {
            Properties.Settings settings = Properties.Settings.Default;
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(settings.Language);
        }

        public static void SaveLanguage(string lang)
        {
            Properties.Settings settings = Properties.Settings.Default;
            settings.Language = lang;
            settings.Save();
        }
    }
}
