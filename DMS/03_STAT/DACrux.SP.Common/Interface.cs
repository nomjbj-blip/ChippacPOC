using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DACrux.SP.Common
{
    public interface IStatusBar
    {
        void SetMessage(string message);
    }

    public interface ITreeViewDrawable
    {
        string Name { get; set; }
        string Parent { get; }
        int ItemCount { get; }

        TreeviewItemType DrawableType { get; }
        ITreeViewDrawable GetItemAt(int index);
    }

    public interface ISectionItem
    {
        string Name { get; }
        Section Parent { get; set; }
        string DefaultValue { get; }
        bool Force { get; set; }
    }

    public interface ITask : IEnumerable<Data>
    {
        ResultUseTypeItem ResultUsage { get; set; }
        TaskTypeItem Type { get; }
        string Name { get; set; }
        string Result { get; set; }
        List<Data> Arguments { get; set; }

        ITask Excute(TaskRunningModeItem requestType);
        string CheckAndGetErrorMessage();

        //event ItemRenamedEventHandler<Task> Renamed;
    }

    [Serializable]
    public class Data
    {
        protected string value = string.Empty;

        public string Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        public Data() { }

        public Data(string value)
        {
            this.value = value;
        }

        public Data(object value)
        {
            this.value = value.ToString();
        }

        public override string ToString()
        {
            return value;
        }
    }

    public interface ILogger
    {
        void WriteLog(LogTypeItem type, string message);
    }

    public interface ITaskDialog
    {
        ITask Task { get; }
        DialogResult ShowDialog();
    }
}
