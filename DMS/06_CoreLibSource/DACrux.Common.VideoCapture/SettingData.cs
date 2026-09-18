using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace DACrux.Common.VideoCapture
{
    internal class SettingData
    {
        public static readonly string EXT = ".xml";
        public static readonly string SPLITTER = "^$$$^";

        public SettingData(string fileName)
        {
            FileName = !string.IsNullOrEmpty(fileName) ? fileName : Path.GetFileName(Assembly.GetEntryAssembly().Location) + SettingData.EXT;
            Read();
        }

        public SettingData()
            : this((string)null)
        {
        }

        public SettingData(System.Type type)
            : this(type.Name)
        {
        }

        private void CreateNewTable()
        {
            DataSource = new DataTable();
            DataSource.Rows.Add(DataSource.NewRow());
        }

        public void Save()
        {
            try
            {
                string directoryName = Path.GetDirectoryName(FullPath);

                if (!Directory.Exists(directoryName))
                    Directory.CreateDirectory(directoryName);

                using (TextWriter writer = new StreamWriter(FullPath, false, Encoding.Default))
                {
                    DataSource.WriteXml(writer, XmlWriteMode.WriteSchema);
                }
            }
            catch
            {
            }
        }

        public bool HasValue(string name)
        {
            return DataSource.Columns.Contains(name) && !string.IsNullOrEmpty(Row()[name].ToString());
        }

        public void SetValue(string name, object value)
        {
            if (string.IsNullOrEmpty(name))
                return;
            if (!DataSource.Columns.Contains(name))
                DataSource.Columns.Add(name);
            if (value == null)
                value = (object)string.Empty;
            Row()[name] = value;
        }

        public void SetArrayValue(string name, object[] values)
        {
            string str = (string)null;
            if (values != null && values.Length > 0)
                str = string.Join(SettingData.SPLITTER, values);
            SetValue(name, (object)str);
        }

        public string GetValue(string name)
        {
            return !DataSource.Columns.Contains(name) ? (string)null : Row()[name].ToString();
        }

        public T GetValue<T>(string name) where T : struct
        {
            T returnValue;
            return TryConvert<T>(GetValue(name), out returnValue) ? returnValue : default(T);
        }

        public T GetValue<T>(string name, T defaultValue) where T : struct
        {
            T returnValue;
            return TryConvert<T>(GetValue(name), out returnValue) ? returnValue : defaultValue;
        }

        public string[] GetArrayValue(string name)
        {
            if (!DataSource.Columns.Contains(name))
                return new string[0];
            string str = GetValue(name);
            if (string.IsNullOrEmpty(str))
                return new string[0];
            return str.Split(new string[1] { SettingData.SPLITTER }, StringSplitOptions.RemoveEmptyEntries);
        }

        public T[] GetArrayValue<T>(string name) where T : struct
        {
            string[] arrayValue1 = GetArrayValue(name);
            if (arrayValue1 == null || arrayValue1.Length == 0)
                return new T[0];
            T[] arrayValue2 = new T[arrayValue1.Length];
            for (int index = 0; index < arrayValue1.Length; ++index)
            {
                T returnValue;
                if (!TryConvert<T>(arrayValue1[index], out returnValue))
                    returnValue = default(T);
                arrayValue2[index] = returnValue;
            }
            return arrayValue2;
        }

        private bool TryConvert<T>(string value, out T returnValue) where T : struct
        {
            returnValue = default(T);
            if (string.IsNullOrEmpty(value))
                return false;
            try
            {
                TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
                if (converter != null)
                    returnValue = (T)converter.ConvertFromString(value);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool ExistsData()
        {
            return DataSource != null && DataSource.Rows.Count > 0 && DataSource.Columns.Count > 0;
        }

        private void Read()
        {
            FullPath = GetFullPath();
            DataSource = new DataTable(Path.GetFileNameWithoutExtension(FileName));
            if (File.Exists(FullPath))
            {
                try
                {
                    using (StreamReader reader = new StreamReader(FullPath, Encoding.Default))
                    {
                        DataSource.ReadXml(reader);
                    }
                }
                catch
                {
                    DataSource.Rows.Add(DataSource.NewRow());
                }
            }
            else
            {
                DataSource.Rows.Add(DataSource.NewRow());
            }
        }

        private DataRow Row()
        {
            return DataSource.Rows[0];
        }

        private string GetFullPath()
        {
            string fullPath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), FileName);
            if (Path.GetExtension(FileName).ToLower() != SettingData.EXT)
                fullPath += SettingData.EXT;
            return fullPath;
        }

        public string FileName { get; private set; }

        public string FullPath { get; private set; }

        public DataTable DataSource { get; private set; }
    }
}
