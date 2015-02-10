using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranspaConsole
{
    class ExecuteCmd
    {
        public StreamReader ExecuteCommandSync(string command)
        {
            try
            {

                var procStartInfo =
                    new System.Diagnostics.ProcessStartInfo("cmd", "/c " + command);

                procStartInfo.RedirectStandardOutput = true;
                procStartInfo.UseShellExecute = false;
                procStartInfo.CreateNoWindow = true;
                // Now we create a process, assign its ProcessStartInfo and start it
                var proc = new System.Diagnostics.Process();
                proc.StartInfo = procStartInfo;
                proc.Start();
                return proc.StandardOutput;
                
            }
            catch (Exception objException)
            {
                // Log the exception
            }
            return null;
        }
    }
}
