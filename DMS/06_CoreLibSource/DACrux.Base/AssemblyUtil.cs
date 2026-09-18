using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;

namespace DACrux.Base
{
    public sealed class AssemblyUtil
    {
        public static Assembly LoadAssembly(string AssemblyFile)
        {
            FileStream fs = null;
            byte[] buffer = null;
            try
            {
                fs = new FileStream(AssemblyFile, FileMode.Open, FileAccess.Read);
                buffer = new byte[(int)fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                return Assembly.Load(buffer);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                    fs.Dispose();
                }
                fs = null;
                buffer = null;
            }
        }
    }
}
