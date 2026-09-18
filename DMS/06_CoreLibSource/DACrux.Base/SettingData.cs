using System;
using System.ComponentModel;
using System.Data;
using System.IO;

namespace DACrux.Base
{
    // 2019.07.21 Taihi,Kim.
    /// <summary>
    /// 값을 파일로 저장하고 로드할 수 있는 클래스 입니다.<para />
    /// 파일 저장 위치는 내문서 이며, 하위 경로로 저장하려면 파일명 앞에 경로명을 같이 입력합니다.<para />
    /// </summary>
    public class SettingData
    {
        public static readonly string EXT = ".xml";
        public static readonly string SPLITTER = "^$$$^";

        /// <summary>
        /// 생성자. 파일명을 지정하지 않으면 '실행파일명.exe.xml' 형식으로 저장됩니다.
        /// </summary>
        public SettingData(string fileName)
        {
            if (String.IsNullOrEmpty(fileName))
                FileName = Path.GetFileName(System.Windows.Forms.Application.ExecutablePath + EXT);
            else
                FileName = fileName;

            Read();
        }

        /// <summary>
        /// 생성자. 파일명을 지정하지 않으면 '실행파일명.exe.xml' 형식으로 저장됩니다.
        /// </summary>
        public SettingData()
            : this((string)null)
        {
        }

        /// <summary>
        /// 생성자. Type명.xml 형식으로 저장됩니다.
        /// </summary>
        public SettingData(Type type)
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
                string dir = Path.GetDirectoryName(FullPath);

                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                DataSource.WriteXml(FullPath, XmlWriteMode.WriteSchema);
            }
            catch { }
        }

        public bool HasValue(string name)
        {
            if (DataSource.Columns.Contains(name))
                return !String.IsNullOrEmpty(Row()[name].ToString());

            return false;
        }

        public void SetValue(string name, object value)
        {
            if (String.IsNullOrEmpty(name))
                return;

            if (!DataSource.Columns.Contains(name))
                DataSource.Columns.Add(name);

            if (value == null)
                value = String.Empty;

            Row()[name] = value;
        }

        public void SetArrayValue(string name, object[] values)
        {
            string text = null;

            if (values != null && values.Length > 0)
                text = String.Join(SPLITTER, values);

            SetValue(name, text);
        }

        public string GetValue(string name)
        {
            if (!DataSource.Columns.Contains(name))
                return null;

            return Row()[name].ToString();
        }

        /// <summary>
        /// 값을 가져옵니다.
        /// </summary>
        public T GetValue<T>(string name) where T : struct
        {
            T t;
            if (TryConvert<T>(GetValue(name), out t))
                return t;

            return default(T);
        }

        /// <summary>
        /// 값을 가져옵니다. 값이 비어있거나 변환에 실패하는 경우 전달된 값이 리턴됩니다.
        /// </summary>
        public T GetValue<T>(string name, T defaultValue) where T : struct
        {
            T t;
            if (TryConvert<T>(GetValue(name), out t))
                return t;

            return defaultValue;
        }

        public string[] GetArrayValue(string name)
        {
            if (!DataSource.Columns.Contains(name))
                return new string[] { };

            string text = GetValue(name);

            if (String.IsNullOrEmpty(text))
                return new string[] { };

            return text.Split(new string[] { SPLITTER }, StringSplitOptions.RemoveEmptyEntries);
        }

        public T[] GetArrayValue<T>(string name) where T : struct
        {
            string[] arr = GetArrayValue(name);

            if (arr == null || arr.Length == 0)
                return new T[] { };

            T[] returnArr = new T[arr.Length];

            for (int i = 0; i < arr.Length; i++)
            {
                T t;
                if (!TryConvert<T>(arr[i], out t))
                    t = default(T);

                returnArr[i] = t;
            }

            return returnArr;
        }

        private bool TryConvert<T>(string value, out T returnValue) where T : struct
        {
            returnValue = default(T);

            if (String.IsNullOrEmpty(value))
                return false;

            try
            {
                var converter = TypeDescriptor.GetConverter(typeof(T));

                if (converter != null)
                    returnValue = (T)converter.ConvertFromString(value);

                return true;
            }
            catch
            {
                return false;
            }
        }

        private void Read()
        {
            FullPath = GetFullPath();

            DataSource = new DataTable(Path.GetFileNameWithoutExtension(FileName));

            if (File.Exists(FullPath))
            {
                try
                {
                    DataSource.ReadXml(FullPath);
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
            string fullPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), FileName);

            if (Path.GetExtension(FileName).ToLower() != EXT)
                fullPath += EXT;

            return fullPath;
        }

        public string FileName
        {
            get;
            private set;
        }

        public string FullPath
        {
            get;
            private set;
        }

        public DataTable DataSource
        {
            get;
            private set;
        }
    }
}
