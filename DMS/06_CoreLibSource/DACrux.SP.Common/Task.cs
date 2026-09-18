using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DACrux.SP.Common;
using System.Data.OracleClient;

namespace DACrux.SP.Common
{
    [Serializable]
    public abstract class Task : Data, ITask
    {
        #region " Member Field "

        private string name = string.Empty;
        private List<Data> arguments = new List<Data>();
        protected ResultUseTypeItem resultUseType = ResultUseTypeItem.General;

        #endregion

        #region " Creator "

        public Task() { }

        #endregion

        #region " Property "

        public abstract TaskTypeItem Type { get; }

        public ResultUseTypeItem ResultUsage
        {
            get { return resultUseType; }
            set { resultUseType = value; }
        }

        public abstract string Result { get; set; }

        public abstract ILogger Logger { get; set; }

        public List<Data> Arguments
        {
            get { return arguments; }
            set { arguments = value; }
        }

        public string Name
        {
            get { return name; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    return;

                if (name == value)
                    return;

                string prevName = name;
                name = value;

                if (Renamed != null)
                    Renamed(this, prevName);
            }
        }

        #endregion

        #region " Method "

        public abstract ITask Excute(TaskRunningModeItem requestType);

        public abstract string CheckAndGetErrorMessage();

        public abstract IEnumerator<Data> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        #region " Event "

        public event ItemRenamedEventHandler<Task> Renamed;

        #endregion
    }

    [Serializable]
    public class COMPlusTask : Task
    {
        #region " Member Field "

        string comName = string.Empty;

        public string ComponentName
        {
            get { return comName; }
            set { comName = value; }
        }
        string methodName = string.Empty;

        public string MethodName
        {
            get { return methodName; }
            set { methodName = value; }
        }

        ILogger logger = new ConsoleLogger();

        #endregion

        #region " Creator "

        public COMPlusTask()
        {
        }

        #endregion

        #region " Property "

        public override TaskTypeItem Type
        {
            get
            {
                return TaskTypeItem.COM_PLUS;
            }
        }

        public override string Result
        {
            get { return value; }
            set { this.value = value; }
        }

        public override ILogger Logger
        {
            get { return logger; }
            set { logger = value; }
        }

        public string Seperator { get; set; }

        #endregion

        #region " Method "

        public override ITask Excute(TaskRunningModeItem requestType)
        {
            switch (requestType)
            {
                case TaskRunningModeItem.Always:
                    value = ExcuteCOMPlus();
                    break;
                case TaskRunningModeItem.Once:
                    if (string.IsNullOrEmpty(value))
                        value = ExcuteCOMPlus();
                    break;
                default:
                    break;
            }

            return this;
        }

        private string ExcuteCOMPlus()
        {
            string value = string.Empty;

            COMAdminHelper helper = new COMAdminHelper();
            COMWrapper com = new COMWrapper(comName);

            var parameters = from Data data in Arguments
                             select data.Value as object;
            value = com.CallMethod(methodName, parameters.ToArray()).ToString();

            return value;
        }

        public override IEnumerator<Data> GetEnumerator()
        {
            if (string.IsNullOrEmpty(value))
                Excute(TaskRunningModeItem.Always);

            if (string.IsNullOrEmpty(value))
                yield break;

            switch (resultUseType)
            {
                case ResultUseTypeItem.General:
                    yield break;
                case ResultUseTypeItem.Count:

                    int iOut;
                    if (!int.TryParse(value, out iOut))
                        yield break;

                    for (int i = 0; i < iOut; i++)
                    {
                        value = i.ToString();
                        yield return new Data(i);
                    }

                    break;
                case ResultUseTypeItem.SeperatedValue:

                    string[] arrSplit = value.Split(Seperator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    foreach (string split in arrSplit)
                    {
                        value = split;
                        yield return new Data(split);
                    }

                    break;
                case ResultUseTypeItem.CharArray:

                    foreach (char ch in value.ToCharArray())
                    {
                        ch.ToString();
                        yield return new Data(ch);

                        
                    }

                    break;
                default:
                    yield break;
            }
        }

        public override string CheckAndGetErrorMessage()
        {
            if (string.IsNullOrEmpty(comName) || string.IsNullOrEmpty(methodName))
                return string.Format("[{0}]\tCannot Excute : {1}", TaskTypeItem.COM_PLUS.ToString(), "'Component Name' or 'Method Name' is not specified.");

            return string.Empty;
        }

        #endregion
    }

    [Serializable]
    public class DBTask : Task
    {
        #region " Member Field "

        string query = string.Empty;
        ConnectionInformation connInfo = null;
        List<string> parameterNames = new List<string>();
        QueryTypeItem queryType = QueryTypeItem.Dynamic;
        ILogger logger = new ConsoleLogger();

        #endregion

        #region " Creator "

        public DBTask()
        {
            connInfo = Analysis.GetInstance().ConnectionInfo;
        }

        #endregion

        #region " Property "

        public QueryTypeItem QueryType
        {
            get { return queryType; }
            set { queryType = value; }
        }

        public string Query
        {
            get { return query; }
            set { query = value; }
        }

        public List<string> ParameterNames
        {
            get { return parameterNames; }
            set { parameterNames = value; }
        }

        public override TaskTypeItem Type
        {
            get
            {
                return TaskTypeItem.DATABASE;
            }
        }

        public override string Result
        {
            get { return value; }
            set { this.value = value; }
        }

        public override ILogger Logger
        {
            get { return logger; }
            set { logger = value; }
        }

        public string TargetColumn { get; set; }

        public string Seperator { get; set; }

        #endregion

        #region " Method "

        public override ITask Excute(TaskRunningModeItem requestType)
        {
            switch (requestType)
            {
                case TaskRunningModeItem.Always:
                    value = ExcuteQuery();
                    break;
                case TaskRunningModeItem.Once:
                    if (string.IsNullOrEmpty(value))
                        value = ExcuteQuery();
                    break;
                default:
                    break;
            }

            return this;
        }

        private string ExcuteQuery()
        {
            string value = string.Empty;

            switch (queryType)
            {
                case QueryTypeItem.Select:
                    value = GetDBData();
                    break;
                case QueryTypeItem.Insert:
                case QueryTypeItem.Update:
                case QueryTypeItem.Delete:
                case QueryTypeItem.Dynamic:
                    value = ExcuteDBCommand();
                    break;
                default:
                    break;
            }

            return value;
        }

        private OracleParameter[] GetOracleParameters(List<string> parameters)
        {
            var iteraterParams = from Data data in Arguments
                                 join string name in parameters on Arguments.IndexOf(data) equals parameters.IndexOf(name)
                                 select new OracleParameter(name, data.Value);

            if (iteraterParams == null || iteraterParams.Count() < 1)
                return null;
            else
                return iteraterParams.ToArray();
        }

        private string GetDBData()
        {
            OracleConnector oc = new OracleConnector(connInfo);
            DataTable dt = null;

            string value = string.Empty;

            var parameters = GetOracleParameters(parameterNames);

            string strRealQuery = GetStaticQuery(this.query, parameters);
            if (parameters != null)
                parameters = Array.FindAll<OracleParameter>(parameters, op => !op.ParameterName.StartsWith("DYNAMIC_"));

            dt = oc.GetDataTable(strRealQuery, parameters);

            if (string.IsNullOrEmpty(TargetColumn))
                TargetColumn = dt.Columns[0].ColumnName;

            if (dt == null || dt.Rows.Count < 1 || !dt.Columns.Contains(TargetColumn))
                return string.Empty;
            else if (dt.Rows.Count == 1)
                value = dt.Rows[0][TargetColumn].ToString();
            else
                value = string.Join(Seperator, Utility.GetColumnValues(dt, TargetColumn, false, Utility.SortTypes.None));

            return value;
        }

        private string GetStaticQuery(string query, OracleParameter[] parameters)
        {
            string strFinalQuery = string.Empty;

            try
            {

                string[] arrSplit = query.Split(new string[] { "??" }, StringSplitOptions.None);

                for (int i = 0; i < arrSplit.Length - 1; i++)
                {
                    strFinalQuery += arrSplit[i]
                        + Array.Find<OracleParameter>(parameters, op => op.ParameterName == "DYNAMIC_" + (i + 1).ToString()).Value;
                }
                strFinalQuery += arrSplit[arrSplit.Length - 1];
            }
            catch (Exception ex)
            {
                Utility.ShowMessageBox(ex.Message, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return strFinalQuery;
        }

        private string ExcuteDBCommand()
        {
            OracleConnector oc = new OracleConnector(connInfo);
            string value = string.Empty;

            var parameters = GetOracleParameters(parameterNames);

            string strRealQuery = GetStaticQuery(this.query, parameters);

            if(parameters != null)
                parameters = Array.FindAll<OracleParameter>(parameters, op => !op.ParameterName.StartsWith("DYNAMIC_"));

            value = oc.Excute(strRealQuery, parameters).ToString();

            return value;
        }

        public override IEnumerator<Data> GetEnumerator()
        {
            if (string.IsNullOrEmpty(value))
                Excute(TaskRunningModeItem.Always);

            if (string.IsNullOrEmpty(value))
                yield break;

            switch (resultUseType)
            {
                case ResultUseTypeItem.General:
                    yield break;
                case ResultUseTypeItem.Count:

                    int iOut;
                    if (!int.TryParse(value, out iOut))
                        yield break;

                    for (int i = 0; i < iOut; i++)
                        yield return new Data(i);

                    break;
                case ResultUseTypeItem.SeperatedValue:

                    string[] arrSplit = value.Split(Seperator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    foreach (string split in arrSplit)
                        yield return new Data(split);

                    break;
                case ResultUseTypeItem.CharArray:

                    foreach (char ch in value.ToCharArray())
                        yield return new Data(ch);

                    break;
                default:
                    yield break;
            }

        }

        public override string CheckAndGetErrorMessage()
        {
            if (string.IsNullOrEmpty(query.Trim()))
                return string.Format("[{0}]\tCannot Excute : {1}"
                    , TaskTypeItem.DATABASE.ToString()
                    , "Query string is not specified.");

            if (parameterNames.Count != Arguments.Count)
                return string.Format("[{0}]\tCannot Excute : {1}"
                    , TaskTypeItem.DATABASE.ToString()
                    , "Wrong parameter count. " + parameterNames.Count.ToString() + " parameters expected but " + Arguments.Count + " received.");

            return string.Empty;
        }

        #endregion
    }

    [Serializable]
    public class EntityValueTask : Task
    {
        #region " Member Field "

        string entityName = string.Empty;
        int matchIndex = 0;

        ILogger logger = new ConsoleLogger();

        #endregion

        #region " Creator "

        public EntityValueTask() { }

        #endregion

        #region " Property "

        public override TaskTypeItem Type
        {
            get
            {
                return TaskTypeItem.ENTITY_VALUE;
            }
        }

        public override string Result
        {
            get { return value; }
            set { this.value = value; }
        }

        public override ILogger Logger
        {
            get { return logger; }
            set { logger = value; }
        }

        public string Seperator { get; set; }

        public RegexEntity Entity
        {
            get { return Analysis.GetInstance().Entities[entityName] as RegexEntity; }
            set { entityName = value.Name; }
        }

        public int MatchIndex
        {
            get { return matchIndex; }
            set { matchIndex = value; }
        }

        #endregion

        #region " Method "

        public override ITask Excute(TaskRunningModeItem requestType)
        {
            Analysis analysis = Analysis.GetInstance();

            int matchIndex = 0;

            if (Arguments.Count > 0)
                if (!int.TryParse(Arguments[0].Value, out matchIndex))
                    matchIndex = 0;

            string strTargetText = analysis.CurrentFileNavigator.TargetText;

            if (string.IsNullOrEmpty(strTargetText))
                return null;

            Token capture = analysis.GetCapture(Entity, matchIndex);

            switch (requestType)
            {
                case TaskRunningModeItem.Always:
                    value = strTargetText.Substring(capture.Index, capture.Length); //CurrentNavigator.GetCapture(entity, matchIndex).Value;
                    break;
                case TaskRunningModeItem.Once:
                    if (string.IsNullOrEmpty(value))
                        value = strTargetText.Substring(capture.Index, capture.Length); //CurrentNavigator.GetCapture(entity, matchIndex).Value;
                    break;
                default:
                    break;
            }

            return this;
        }

        public override IEnumerator<Data> GetEnumerator()
        {
            if (string.IsNullOrEmpty(value))
                Excute(TaskRunningModeItem.Always);

            if (string.IsNullOrEmpty(value))
                yield break;

            switch (resultUseType)
            {
                case ResultUseTypeItem.General:
                    yield break;
                case ResultUseTypeItem.Count:

                    int iOut;
                    if (!int.TryParse(value, out iOut))
                        yield break;

                    for (int i = 0; i < iOut; i++)
                    {
                        value = i.ToString();
                        yield return new Data(i);
                    }
                    break;
                case ResultUseTypeItem.SeperatedValue:

                    string[] arrSplit = value.Split(Seperator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    foreach (string split in arrSplit)
                    {
                        value = split;
                        yield return new Data(split);
                    }
                    break;
                case ResultUseTypeItem.CharArray:

                    foreach (char ch in value.ToCharArray())
                    {
                        value = ch.ToString();
                        yield return new Data(ch);
                    }

                    break;
                default:
                    yield break;
            }
        }

        public override string CheckAndGetErrorMessage()
        {
            if (Entity == null)
                return string.Format("[{0}]\tCannot Excute : {1}"
                    , TaskTypeItem.ENTITY_VALUE.ToString()
                    , "Entity is not specified.");

            return string.Empty;
        }

        #endregion
    }

    [Serializable]
    public class EntityTraverseTask : Task
    {
        #region " Member Field "

        string entityName = string.Empty;
        ILogger logger = new ConsoleLogger();

        #endregion

        #region " Creator "

        public EntityTraverseTask() { Seperator = ","; }

        #endregion

        #region " Property "

        public RegexEntity Entity
        {
            get { return Analysis.GetInstance().Entities[entityName] as RegexEntity; }
            set { entityName = value.Name; }
        }

        public override TaskTypeItem Type
        {
            get
            {
                return TaskTypeItem.ENTITY_TRAVERSE;
            }
        }

        public string Seperator { get; set; }

        public override string Result
        {
            get { return value; }
            set { this.value = value; }
        }

        public override ILogger Logger
        {
            get { return logger; }
            set { logger = value; }
        }

        #endregion

        #region " Method "

        public override ITask Excute(TaskRunningModeItem requestType)
        {
            Analysis analysis = Analysis.GetInstance();

            string strTargetText = analysis.CurrentFileNavigator.TargetText;

            if (string.IsNullOrEmpty(strTargetText))
                return null;

            switch (requestType)
            {
                case TaskRunningModeItem.Always:
                    value = string.Join(Seperator
                        , (from Token capture in analysis.GetCaptures(Entity) //CurrentNavigator.GetCaptures(entity)
                           select strTargetText.Substring(capture.Index, capture.Length)).ToArray());
                    break;
                case TaskRunningModeItem.Once:
                    if (string.IsNullOrEmpty(value))
                        value = string.Join(Seperator
                            , (from Token capture in analysis.GetCaptures(Entity) //CurrentNavigator.GetCaptures(entity)
                               select strTargetText.Substring(capture.Index, capture.Length)).ToArray());
                    break;
                default:
                    break;
            }

            return this;
        }

        public override IEnumerator<Data> GetEnumerator()
        {
            if (string.IsNullOrEmpty(value))
                yield break;

            string[] arrSplit = null;

            switch (resultUseType)
            {
                case ResultUseTypeItem.General:
                    yield break;
                case ResultUseTypeItem.Count:

                    arrSplit = value.Split(Seperator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < arrSplit.Length; i++)
                    {
                        value = i.ToString();
                        yield return new Data(i);
                    }
                    break;
                case ResultUseTypeItem.SeperatedValue:

                    arrSplit = value.Split(Seperator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    foreach (string split in arrSplit)
                    {
                        value = split;
                        yield return new Data(split);
                    }
                    break;
                case ResultUseTypeItem.CharArray:
                    yield break;
                default:
                    yield break;
            }
        }

        public override string CheckAndGetErrorMessage()
        {
            if (Entity == null)
                return string.Format("[{0}]\tCannot Excute : {1}"
                    , TaskTypeItem.ENTITY_TRAVERSE.ToString()
                    , "Entity is not specified.");

            return string.Empty;
        }

        #endregion
    }

    //[Serializable]
    //public class QueryMakerTask : Task
    //{
    //    #region " Member Field "

    //    List<string> lstQueryPart = new List<string>();
    //    ILogger logger = new ConsoleLogger();

    //    #endregion

    //    #region " Creator "

    //    public QueryMakerTask() { }

    //    #endregion

    //    #region " Property "

    //    public override TaskTypeItem Type
    //    {
    //        get
    //        {
    //            return TaskTypeItem.QUERY_BUILDER;
    //        }
    //    }

    //    public override string Result
    //    {
    //        get { return value; }
    //        set { this.value = value; }
    //    }

    //    public override ILogger Logger
    //    {
    //        get { return logger; }
    //        set { logger = value; }
    //    }

    //    public string FormatString { get; set; }

    //    #endregion

    //    #region " Method "

    //    public override ITask Excute(RunningModeItem requestType)
    //    {
    //        switch (requestType)
    //        {
    //            case RunningModeItem.Always:
    //                value = MakeQuery();
    //                break;
    //            case RunningModeItem.Once:
    //                if (string.IsNullOrEmpty(value))
    //                    value = MakeQuery();
    //                break;
    //            default:
    //                break;
    //        }

    //        return this;
    //    }

    //    private string MakeQuery()
    //    {
    //        StringBuilder sb = new StringBuilder();

    //        for (int i = 0; i < lstQueryPart.Count; i++)
    //        {
    //            sb.Append(lstQueryPart[i]);

    //            if (Arguments.Count > i)
    //                sb.Append(Arguments[i].Value);
    //        }

    //        return sb.ToString();
    //    }

    //    public override IEnumerator<Data> GetEnumerator()
    //    {
    //        yield break;
    //    }

    //    #endregion
    //}

    #region " NOT USING & PRESERVED "

    [Serializable]
    public class QueryParameter
    {
        string name = string.Empty;
        string value = string.Empty;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        public OracleParameter ToOracleParameter()
        {
            if (string.IsNullOrEmpty(name))
                return null;

            return new OracleParameter(name, value);
        }

        //public MSSQLParameter ToMSSQLParameter()
        //{
        //    return null;
        //}
    }

    #endregion
}
