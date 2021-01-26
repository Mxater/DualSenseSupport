using System;
using System.Diagnostics;

namespace DSApps
{
    public static class Utils
    {
        public static void OpenUrl(Uri url)
        {
            Process myProcess = new Process();
            // true is the default, but it is important not to set it to false
            try
            {
                myProcess.StartInfo.UseShellExecute = true;
                myProcess.StartInfo.FileName = url.ToString();
                myProcess.Start();
            }
            catch
            {
                Debug.WriteLine("Cant open Link: " + url);
            }
            
        }

        public static void OpenUrl(string url)
        {
            OpenUrl(new Uri(url));
        }
    }
}