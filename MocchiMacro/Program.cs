// The tool's entry point: deletes a damaged settings file, then opens the main window.
// Build the solution and run MocchiMacro/bin/Debug/MocchiMacro.exe (CLAUDE.md has the commands).

using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MocchiMacro.Properties;

namespace MocchiMacro
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            DeleteDamagedSettings();
            Application.Run(new MainWindow());
        }

        // A damaged user.config, such as one cut short by a power cut while it was saved, makes the
        // first read of a setting throw. Nothing would catch that, so the tool would fail on every
        // start until someone found and deleted the file. It's deleted here instead, and the tool
        // starts on the default settings.
        // It has to happen before anything reads Settings.Default: once the settings system has
        // failed to read the file, it keeps failing for the rest of the run, even after the file
        // is gone. So the file is opened separately here, first, which leaves that system untouched.
        static void DeleteDamagedSettings()
        {
            try
            {
                // Opening it reads the file's outline; getting the section reads the settings in it.
                Configuration userConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal);
                userConfig.GetSection("userSettings/" + typeof(Settings).FullName);
            }
            catch (ConfigurationErrorsException e)
            {
                // The damaged file is named by this exception or, depending on what's wrong with
                // it, by the one inside it. Only the tool's own settings file is ever deleted.
                string damagedFile = e.Filename ?? (e.InnerException as ConfigurationErrorsException)?.Filename;
                if (damagedFile != null && Path.GetFileName(damagedFile) == "user.config")
                {
                    // A file that can't be deleted either is left for the settings system to
                    // report, as it would without this check.
                    try
                    {
                        File.Delete(damagedFile);
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }
    }
}
