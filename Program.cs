using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Drawing;

namespace Blue_Screen_saver
{
    static class Program //UID1124204102
    {
        [STAThread]
        static void Main(string[] args) //UID2412304100
        {
            
            // process coman line parameters
            if (args.Length > 0)
            {
                if (args[0].ToLower().Trim().Substring(0, 2) == "/c") //configure
                {
                    SetVideoDirectory();
                    return;
                }                
            }

            string VideoPath = LoadVideoPathFromRegistry();
                      
            if (VideoPath == "" || !Directory.Exists(VideoPath))
            {
                return;
            }

            //run the screen saver
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ShowScreensaver(VideoPath);
            Application.Run();

        }
        
        ////UID4140004144
        static void SetVideoDirectory()
        {           
            //inform the user no options can be set in this screen 
            System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK && Directory.Exists(folderBrowserDialog1.SelectedPath))
            {                        
                SaveVideoPathToRegistry(folderBrowserDialog1.SelectedPath);               
            }
        }
        
                //UID2333332221
        static void SaveVideoPathToRegistry(string path)
        {
            Microsoft.Win32.RegistryKey VideoScreenSaverRegistryKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("Software\\VideoScreenSaver");
            VideoScreenSaverRegistryKey.SetValue("VideoPath", path);
            VideoScreenSaverRegistryKey.Close();          
        }
        
        //UID4111422114
        static string LoadVideoPathFromRegistry()
        {
            Microsoft.Win32.RegistryKey VideoScreenSaverRegistryKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("Software\\VideoScreenSaver");
            string VideoPath = VideoScreenSaverRegistryKey.GetValue("VideoPath", "").ToString();
            VideoScreenSaverRegistryKey.Close();
            
            return VideoPath;
        }

        static void ShowScreensaver(String VideoPath) //UID3030132331
        {

#if DEBUG
            Rectangle screen = new Rectangle(0,0, 400, 400);
            //creates a form just for that screen and passes it the bounds of that screen
            MainForm screensaver = new MainForm(screen, 0, VideoPath);
            screensaver.Show();
            return;
#else
            //loops through all the computer's screens (monitors)
            int screenNo = 0;
            foreach (Screen screen in Screen.AllScreens)
            {
                //creates a form just for that screen and passes it the bounds of that screen
                MainForm screensaver = new MainForm(screen.Bounds, screenNo, VideoPath);
                screensaver.Show();
                screenNo = screenNo + 1;
            }
#endif

        }


    }
}
