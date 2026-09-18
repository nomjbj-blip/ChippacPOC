using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using System.Reflection;

namespace DACrux.Data.Handler
{
    public static class HandlerFactory
    {
        private static string m_garbageExtensions;
        private static string[] m_garbageExtensionArr;

        public delegate EquipInfo GetEquipInfoDelegate(string factory, string equipID);

        /// <summary>
        /// 해당 파일에 맞는 Handler 를 가져옵니다.
        /// </summary>
        /// <returns></returns>
        public static HandlerList CreateInstance(string factory, string directory, GetEquipInfoDelegate del)
        {
            /*
             * 모든 디렉토리 가져오기
             * 각 디렉토리 별 파일 리스트가 있는지 체크하여 있는 경우에만 HandlerList에 추가한다.
             */
            string[] dirs = Directory.GetDirectories(directory, "*", SearchOption.AllDirectories);

            HandlerList list = new HandlerList();

            foreach (string dir in dirs)
            {
                string[] files = Directory.GetFiles(dir);

                if (files == null || files.Length == 0)
                    continue;

                // 디렉토리 배열
                string[] arr = dir.Split(Path.DirectorySeparatorChar);

                if (arr == null || arr.Length == 0)
                    continue;

                // 마지막 디렉토리가 설비ID
                string equipID = arr[arr.Length - 1];

                if (del == null)
                    throw new Exception("GetEquipInfoDelegate 델리게이트가 null 입니다.");

                EquipInfo info = del.Invoke(factory, equipID);

                // 설비 정보가 없으면 continue
                if (info == null)
                    continue;

                // 원본 데이터 파일을 복사할지를 나타냅니다.
                if (!String.IsNullOrEmpty(OriginalFileBackupPath))
                {
                    if (!Directory.Exists(Path.Combine(OriginalFileBackupPath, equipID)))
                        Directory.CreateDirectory(Path.Combine(OriginalFileBackupPath, equipID));

                    foreach (string file in Directory.GetFiles(dir))
                    {
                        try
                        {
                            File.Copy(file, Path.Combine(OriginalFileBackupPath, equipID, Path.GetFileName(file)), true);
                        }
                        catch
                        {
                        }
                    }
                }

                // 삭제할 가비지 파일이 있는 경우 삭제
                if (m_garbageExtensionArr != null)
                {
                    foreach (string garbageExt in m_garbageExtensionArr)
                    {
                        string[] garbages = Directory.GetFiles(dir, "*" + garbageExt);

                        foreach (string file in garbages)
                        {
                            try { File.Delete(file); }
                            catch { }
                        }
                    }
                }

                // Handler
                Handler handler = GetHandlerInstance(info.Handler);
                
                if (handler == null)
                    throw new Exception(String.Format("해당하는 Handler를 찾지 못했습니다. ({0})", info.ToString()));

                handler.FileNames = files;
                handler.EquipInfo = info;
                list.Add(handler);
            }

            return list;
        }

        /// <summary>
        /// 해당 파일에 맞는 Handler 를 가져옵니다.
        /// </summary>
        /// <returns></returns>
        public static HandlerList CreateInstance(string factory, string equipID, string directory, GetEquipInfoDelegate del)
        {
            /*
             * 모든 파일 가져오기
             * 해당 디렉토리에 파일 리스트가 있는지 체크하여 있는 경우에만 HandlerList에 추가한다.
             */
            HandlerList list = new HandlerList();

            string dir = Path.Combine(directory, equipID);
            string[] files = Directory.GetFiles(dir);

            if (files == null || files.Length == 0)
                return list;

            if (del == null)
                throw new Exception("GetEquipInfoDelegate 델리게이트가 null 입니다.");

            EquipInfo info = del.Invoke(factory, equipID);

            // 설비 정보가 없으면 continue
            if (info == null)
                return list;

            // 원본 데이터 파일을 복사할지를 나타냅니다.
            if (!String.IsNullOrEmpty(OriginalFileBackupPath))
            {
                if (!Directory.Exists(Path.Combine(OriginalFileBackupPath, equipID)))
                    Directory.CreateDirectory(Path.Combine(OriginalFileBackupPath, equipID));

                foreach (string file in Directory.GetFiles(dir))
                {
                    try
                    {
                        File.Copy(file, Path.Combine(OriginalFileBackupPath, equipID, Path.GetFileName(file)), true);
                    }
                    catch
                    {
                    }
                }
            }

            // 삭제할 가비지 파일이 있는 경우 삭제
            if (m_garbageExtensionArr != null)
            {
                foreach (string garbageExt in m_garbageExtensionArr)
                {
                    string[] garbages = Directory.GetFiles(dir, "*" + garbageExt);

                    foreach (string file in garbages)
                    {
                        try { File.Delete(file); }
                        catch { }
                    }
                }
            }

            // Handler
            Handler handler = GetHandlerInstance(info.Handler);

            if (handler == null)
                throw new Exception(String.Format("해당하는 Handler를 찾지 못했습니다. ({0})", info.ToString()));

            handler.FileNames = files;
            handler.EquipInfo = info;
            list.Add(handler);

            return list;
        }

        /// <summary>
        /// 클래스명에 해당하는 Handler 인스턴스를 가져옵니다.
        /// </summary>
        private static Handler GetHandlerInstance(string className)
        {
            if (String.IsNullOrEmpty(className))
                throw new Exception("Handler 클래스명이 없습니다.");

            Type t = Type.GetType(String.Format("{0}.{1}", typeof(Handler).Namespace, className));

            try
            {
                return Activator.CreateInstance(t) as Handler;
            }
            catch
            {
                throw new Exception(String.Format("정의된 Hanlder 클래스가 없습니다. ({0})", className));
            }
        }

        /// <summary>
        /// 삭제할 가비지 파일 확장자를 설정합니다.
        /// </summary>
        public static void SetGarbageExtension(string garbageExtensions)
        {
            if (m_garbageExtensions == garbageExtensions)
                return;

            m_garbageExtensions = garbageExtensions;

            if (String.IsNullOrEmpty(garbageExtensions))
            {
                m_garbageExtensionArr = new string[0];
                return;
            }

            string[] arr = m_garbageExtensions.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            List<string> list = new List<string>();

            foreach (string ext in arr)
            {
                if (String.IsNullOrEmpty(ext) || ext.IndexOf('.') != 0 || ext.IndexOf('*') >= 0)
                    continue;

                list.Add(ext);
            }

            m_garbageExtensionArr = list.ToArray();
        }

        /// <summary>
        /// 원본 데이터 파일을 복사할 위치를 나타냅니다.
        /// </summary>
        public static string OriginalFileBackupPath
        {
            get { return DACrux.Framework.Server.Utility.GetConfigValue("ORIGINAL_FILE_BACKUP_PATH"); }
        }
    }
}
