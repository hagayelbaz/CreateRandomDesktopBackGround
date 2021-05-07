using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using Application = System.Windows.Forms.Application;

namespace CreateRandomDesktopBackGround
{
    class Program
    {
        static void Main(string[] args)
        {
            //run application while desktop setup
            //Create a registry key, then save your program path
            RegistryKey reg = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            reg.SetValue("CreateRandomDesktopBackGround.exe", Application.ExecutablePath.ToString());

            //hide window from user
            var handle = GetConsoleWindow();
            ShowWindow(handle , 0);
                               
            //random picture from website
            string url = string.Format("https://kbdevstorage1.blob.core.windows.net/asset-blobs/{0}_en_1", new Random().Next(19000, 19980));
            // picture path
            string main_path = Environment.GetFolderPath(Environment.SpecialFolder.Cookies);
            string path = main_path + @"\image.png";
            //replace picture
            using (WebClient client = new WebClient())
            {
                try { client.DownloadFile(new Uri(url), main_path + @"\image1.png");}
                catch { return; }

                //delete for replace picture
                if (File.Exists(path))
                    File.Delete(path);

                //rename picture file to image.png
                File.Move(main_path + @"\image1.png", path);
            }
            //set desktop background
            set_Desktop_Background(path);       
            return ;
        }
        //set desktop background
        private static void set_Desktop_Background(string sFile_FullPath)
        {
            win32.SystemParametersInfo(20, 0, sFile_FullPath, 1 | 2);
        }
        //helpes for hide window
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
    }  
    // for esktop backcolor
    internal sealed class win32
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern int SystemParametersInfo(int uAction, int uParam, String lpvParam, int fuWinIni);
    }    
}
