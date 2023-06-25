using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace AppCommon
{
    public static class RuntimeInfo
    {
        public static string FrameworkDescription
        {
            get
            {
                return RuntimeInformation.FrameworkDescription;
            }
        }

        public static string OSName
        {
            get
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    return RuntimeInformation.OSDescription;
                }
                else
                {
                    Process p = new Process();
                    p.StartInfo.FileName = "/bin/sh";
                    p.StartInfo.Arguments = "-c \". /etc/os-release && echo $PRETTY_NAME\"";

                    p.StartInfo.RedirectStandardOutput = true;
                    p.StartInfo.RedirectStandardError = true;
                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.CreateNoWindow = true;

                    p.Start();
                    p.WaitForExit();

                    return p.StandardOutput.ReadToEnd();
                }
            }
        }
    }
}
