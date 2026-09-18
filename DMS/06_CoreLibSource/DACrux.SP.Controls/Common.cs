using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Data;

using DACrux.SP.Controls.Configuration;

namespace DACrux.SP.Controls
{
    #region " ENUM "

    /// <summary>
    /// ICondition Group type
    /// </summary>
    public enum IConditionCollectionType { All, From, To }

    /// <summary>
    /// GraphInformation Value type
    /// </summary>
    public enum ValueTypes { Sum, Average, Max, Min, Count };

    /// <summary>
    /// DateTime Format type
    /// </summary>
    public enum DateTimeFormat { Full, LongDate, ShortDate, LongTime, ShortTime, }

    /// <summary>
    /// Graph Type
    /// </summary>
    public enum GraphType { None, Pie, Bar, Scatter, Line, BoxPlot, Histogram, Pareto }

    /// <summary>
    /// This enum is used for uclJudgeGradePicker.
    /// </summary>
    public enum UclSelectionMode { SingleCode, MultiCodeInSingleGroup, MultiCodeInMultiGroup }

    /// <summary>
    /// Gradient Mode
    /// </summary>
    public enum GradientMode { Horizontal, Vertical, ForwardDiagonal, BackwardDiagonal }

    #endregion

    #region " DELEGATE "

    /// <summary>
    /// Text Changed Event Haldler
    /// </summary>
    /// <param name="strChanged">changed string</param>
    public delegate void TextChangedHandler(string strChanged);

    /// <summary>
    /// Condition Type Changed Event Handler
    /// </summary>
    /// <param name="conditionType">condition type</param>
    public delegate void ConditionTypeChangedHandler(ConditionTypeItems conditionType);

    /// <summary>
    /// ConditionValue Changed Event Handler
    /// </summary>
    /// <param name="conditionValueString">condition value string</param>
    public delegate void ConditionValueStringChangedHandler(string conditionValueString);

    /// <summary>
    /// Image Clicked Event Handler
    /// </summary>
    /// <param name="imagePath"></param>
    public delegate void ImageClickedHandler(string imagePath);

    #endregion

    #region " INTERFACE "

    /// <summary>
    /// Every controls which have ICondition member must implements IConditionOwner Interface.
    /// </summary>
    public interface IConditionOwner
    {
        ICondition ConditionInfo { get; }
        void Reset();
    }

    /// <summary>
    /// Specifies the behaviors of the Common condition requirements.
    /// </summary>
    [TypeConverterAttribute(typeof(System.ComponentModel.ExpandableObjectConverter))]
    public interface ICondition
    {
        #region " Property "

        IDictionary<ConditionTypeItems, string> ConditionData { get; }
        ConditionTypeItems A_ConditionType { get; set; }
        ConditionTypeItems B_CheckList { get; set; }
        string C_SupplementQuery { get; set; }

        string ConditionValueString { get; set; }
        object ConditionValueObject { get; set; }
        int[] ConditionValueIndices { get; set; }

        List<IConditionOwner> RefConditionsFrom { get; }
        List<IConditionOwner> RefConditionsTo { get; }

        Control RefFromCondition_01 { get; set; }
        Control RefFromCondition_02 { get; set; }
        Control RefFromCondition_03 { get; set; }
        Control RefFromCondition_04 { get; set; }
        Control RefFromCondition_05 { get; set; }
        Control RefFromCondition_06 { get; set; }
        Control RefFromCondition_07 { get; set; }
        Control RefFromCondition_08 { get; set; }
        Control RefFromCondition_09 { get; set; }

        Control RefToCondition_01 { get; set; }
        Control RefToCondition_02 { get; set; }
        Control RefToCondition_03 { get; set; }
        Control RefToCondition_04 { get; set; }
        Control RefToCondition_05 { get; set; }
        Control RefToCondition_06 { get; set; }
        Control RefToCondition_07 { get; set; }
        Control RefToCondition_08 { get; set; }
        Control RefToCondition_09 { get; set; }
        Control RefToCondition_10 { get; set; }
        Control RefToCondition_11 { get; set; }
        Control RefToCondition_12 { get; set; }
        Control RefToCondition_13 { get; set; }
        Control RefToCondition_14 { get; set; }
        Control RefToCondition_15 { get; set; }

        #endregion

        #region " METHOD "

        IConditionOwner GetIConditionOwner(ConditionTypeItems conditionType, IConditionCollectionType collectionType);
        void UpdateRefToConditions();
        void ResetItems();

        #endregion
    }

    /// <summary>
    /// Specifies the common property of PeriodCondition control.
    /// </summary>
    public interface IPeriodCondition
    {
        string StartDate { get; }
        string EndDate { get; }
    }

    #endregion

    #region " CLASS "

    /// <summary>
    /// Class Name : ConditionHelper<br/>
    /// Summary    : Dynamic Condition control helper Class<br/>
    /// Author     : Miracom HyungSuk, Yang<br/>
    /// First Date : 2009-12-01<br/>
    /// Description: Every controls that have the instance of this class and implement IConditionOwner interface can be connected with each other.<br/>
    /// History    : <br/>
    /// </summary>
    [TypeConverterAttribute(typeof(System.ComponentModel.ExpandableObjectConverter))]
    public class ConditionHelper : ICondition
    {
        #region " MEMBER FIELD "

        protected IDictionary<ConditionTypeItems, string> oConditionData = null;
        protected ConditionTypeItems oConditionType = ConditionTypeItems.None;
        protected ConditionTypeItems oCheckList = ConditionTypeItems.None;

        protected string strSupplementQuery = string.Empty;

        protected List<IConditionOwner> lstRefFromConditions = new List<IConditionOwner>();
        protected List<IConditionOwner> lstRefToConditions = new List<IConditionOwner>();

        System.Data.DataTable dtDataSource = null;
        string strValue = string.Empty;
        int[] arrIndex = null;

        #endregion

        #region " PROPERTY "

        /// <summary>
        /// Contains each condition's value.
        /// </summary>
        [Browsable(false)]
        public virtual IDictionary<ConditionTypeItems, string> ConditionData
        {
            get { return oConditionData; }
        }

        /// <summary>
        /// Gets or Sets what purpose this selector is being used for.
        /// </summary>
        [Category("Setup")]
        public virtual ConditionTypeItems A_ConditionType
        {
            get
            {
                return oConditionType;
            }
            set
            {
                oConditionType = value;

                if (ConditionTypeChanged != null)
                    ConditionTypeChanged(oConditionType);
            }
        }

        /// <summary>
        /// Specifies the essential ConditionTypes.
        /// If those conditions have no selected item, the control will stop retrieving data.
        /// </summary>
        [Category("Setup")]
        [Editor(typeof(QMS.WinControl.FlagEnumUITypeEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public virtual ConditionTypeItems B_CheckList
        {
            get { return oCheckList; }
            set
            {
                oCheckList = value;

                if (CheckListChanged != null)
                    CheckListChanged(oCheckList);
            }
        }

        /// <summary>
        /// Gets or sets the dynamic query snippet to be added to the main query.
        /// This string will be included in 'WHERE' Phrase of the main query.
        /// </summary>
        [Category("Setup")]
        public virtual string C_SupplementQuery
        {
            get
            {
                return strSupplementQuery;
            }
            set
            {
                strSupplementQuery = value;
            }
        }

        /// <summary>
        /// Selected condition value string
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual string ConditionValueString
        {
            get { return strValue; }
            set
            {
                strValue = value;

                if (ConditionValueStringChanged != null)
                    ConditionValueStringChanged(strValue);
            }
        }

        /// <summary>
        /// Selected value object. 
        /// Usally, it is the datatable.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual object ConditionValueObject
        {
            get { return dtDataSource; }
            set { dtDataSource = (System.Data.DataTable)value; }
        }

        /// <summary>
        /// Selected Value index list.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual int[] ConditionValueIndices
        {
            get { return arrIndex; }
            set { arrIndex = value; }
        }

        /// <summary>
        /// List of Reference From controls.
        /// </summary>
        [Browsable(false)]
        public virtual List<IConditionOwner> RefConditionsFrom
        {
            get { return lstRefFromConditions; }
        }

        /// <summary>
        /// List of Reference To controls.
        /// </summary>
        [Browsable(false)]
        public virtual List<IConditionOwner> RefConditionsTo
        {
            get { return lstRefToConditions; }
        }

        #endregion

        #region " CREATOR "

        internal ConditionHelper()
        {
            InitConditionValue();
        }

        #endregion

        #region " EVENT "

        public event ConditionTypeChangedHandler ConditionTypeChanged;
        public event ConditionTypeChangedHandler CheckListChanged;
        public event ConditionValueStringChangedHandler ConditionValueStringChanged;

        #endregion

        #region " METHOD "

        /// <summary>
        /// Initialize condition data dictionary collection.
        /// </summary>
        public virtual void InitConditionValue()
        {
            if (oConditionData == null)
                oConditionData = new Dictionary<ConditionTypeItems, string>();
            else
                oConditionData.Clear();

            foreach (string name in Enum.GetNames(typeof(ConditionTypeItems)))
            {
                ConditionTypeItems item = (ConditionTypeItems)Enum.Parse(typeof(ConditionTypeItems), name);
                oConditionData.Add(new KeyValuePair<ConditionTypeItems, string>(item, string.Empty));
            }
        }

        /// <summary>
        /// Read the data from RefFromControls.
        /// </summary>
        public virtual void ReadRefValues()
        {
            foreach (IConditionOwner owner in RefConditionsFrom)
            {
                switch (owner.ConditionInfo.A_ConditionType)
                {
                    case ConditionTypeItems.Period:
                        string[] dates = (string[])owner.ConditionInfo.ConditionValueObject;
                        oConditionData[ConditionTypeItems.StartDate] = dates[0];
                        oConditionData[ConditionTypeItems.EndDate] = dates[1];
                        break;
                    case ConditionTypeItems.StartDate:
                        oConditionData[ConditionTypeItems.StartDate] = ((string[])owner.ConditionInfo.ConditionValueObject)[0];
                        break;
                    case ConditionTypeItems.EndDate:
                        oConditionData[ConditionTypeItems.EndDate] = ((string[])owner.ConditionInfo.ConditionValueObject)[1];
                        break;
                    default:
                        oConditionData[owner.ConditionInfo.A_ConditionType] = (owner.ConditionInfo.ConditionValueString == "All") ? string.Empty : owner.ConditionInfo.ConditionValueString;
                        break;
                }
            }
        }

        /// <summary>
        /// Update Reference To controls to reset their data.
        /// </summary>
        public virtual void UpdateRefToConditions()
        {
            foreach (IConditionOwner condition in lstRefToConditions)
            {
                condition.Reset();
                condition.ConditionInfo.ResetItems();
            }
        }

        /// <summary>
        /// Reset Items.
        /// </summary>
        public virtual void ResetItems()
        {
            try
            {
                ReadRefValues();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Gets datatable for the condition item.
        /// </summary>
        /// <param name="item">condition type item</param>
        /// <returns>result datatable</returns>
        public virtual DataTable GetDataTable(ConditionTypeItems item)
        {
            DataTable dtData = null;

            try
            {
                string strMessage = GetEmptyHintItemMessage(oCheckList);

                if (!string.IsNullOrEmpty(strMessage))
                {
                    MessageBox.Show(strMessage, MessageBoxIcon.Information.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return null;
                }

                dtData = ConditionUtil.GetDataTable(item, oConditionData, strSupplementQuery);

                return dtData;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        protected virtual string GetEmptyHintItemMessage(ConditionTypeItems oConditionHint)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                ConditionTypeItems oType;

                foreach (string itemName in Enum.GetNames(typeof(ConditionTypeItems)))
                {
                    oType = (ConditionTypeItems)Enum.Parse(typeof(ConditionTypeItems), itemName);

                    if (oType != ConditionTypeItems.None
                        && (oConditionHint & oType) == oType
                        && string.IsNullOrEmpty(oConditionData[oType]))
                        sb.AppendFormat(", {0}", itemName);
                }

                if (sb.Length > 0)
                    return string.Format("Please input {0}.", sb.ToString(2, sb.Length - 2));
                else
                    return string.Empty;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Gets the owner of the conditionhelper's owner instance.
        /// </summary>
        /// <param name="conditionType">condition type</param>
        /// <param name="collectionType">search area</param>
        /// <returns>ICondition Owner instance</returns>
        public IConditionOwner GetIConditionOwner(ConditionTypeItems conditionType, IConditionCollectionType collectionType)
        {
            switch (collectionType)
            {
                case IConditionCollectionType.All:
                    IConditionOwner owner = GetIConditionOwner(conditionType, IConditionCollectionType.From);
                    if (owner != null) return owner;
                    else return GetIConditionOwner(conditionType, IConditionCollectionType.To);
                case IConditionCollectionType.From:
                    return lstRefFromConditions.Find(delegate(IConditionOwner condtionOwner) { return (condtionOwner.ConditionInfo.A_ConditionType.Equals(conditionType)); });
                case IConditionCollectionType.To:
                    return lstRefToConditions.Find(delegate(IConditionOwner condtionOwner) { return (condtionOwner.ConditionInfo.A_ConditionType.Equals(conditionType)); });
                default:
                    return null;
            }
        }

        #endregion

        #region [ Reference From / To ]

        Control ctlFrom_01 = null;
        Control ctlFrom_02 = null;
        Control ctlFrom_03 = null;
        Control ctlFrom_04 = null;
        Control ctlFrom_05 = null;
        Control ctlFrom_06 = null;
        Control ctlFrom_07 = null;
        Control ctlFrom_08 = null;
        Control ctlFrom_09 = null;

        Control ctlTo_01 = null;
        Control ctlTo_02 = null;
        Control ctlTo_03 = null;
        Control ctlTo_04 = null;
        Control ctlTo_05 = null;
        Control ctlTo_06 = null;
        Control ctlTo_07 = null;
        Control ctlTo_08 = null;
        Control ctlTo_09 = null;
        Control ctlTo_10 = null;
        Control ctlTo_11 = null;
        Control ctlTo_12 = null;
        Control ctlTo_13 = null;
        Control ctlTo_14 = null;
        Control ctlTo_15 = null;

        [Category("From Condition"), DefaultValue(null)]
        public Control RefFromCondition_01
        {
            get
            {
                return ctlFrom_01;
            }
            set
            {
                if (value is IConditionOwner && !lstRefFromConditions.Contains((IConditionOwner)value))
                {
                    lstRefFromConditions.Remove((IConditionOwner)ctlFrom_01);
                    lstRefFromConditions.Add((IConditionOwner)value);

                    ctlFrom_01 = value;
                }
                else if (value == null)
                {
                    lstRefFromConditions.Remove((IConditionOwner)ctlFrom_01);
                    ctlFrom_01 = null;
                }
            }
        }

        [Category("From Condition"), DefaultValue(null)]
        public Control RefFromCondition_02
        {
            get
            {
                return ctlFrom_02;
            }
            set
            {
                if (value is IConditionOwner && !lstRefFromConditions.Contains((IConditionOwner)value))
                {
                    lstRefFromConditions.Remove((IConditionOwner)ctlFrom_02);
                    lstRefFromConditions.Add((IConditionOwner)value);

                    ctlFrom_02 = value;
                }
                else if (value == null)
                {
                    lstRefFromConditions.Remove((IConditionOwner)ctlFrom_02);
                    ctlFrom_02 = null;
                }
            }
        }

        [Category("From Condition"), DefaultValue(null)]
        public Control RefFromCondition_03
        {
            get
            {
                return ctlFrom_03;
            }
            set
            {
                if (value is IConditionOwner && !lstRefFromConditions.Contains((IConditionOwner)value))
                {
                    lstRefFromConditions.Remove((IConditionOwner)ctlFrom_03);
                    lstRefFromConditions.Add((IConditionOwner)value);

                    ctlFrom_03 = value;
                }
                else if (value == null)
                {
                    lstRefFromConditions.Remove((IConditionOwner)ctlFrom_03);
                    ctlFrom_03 = null;
                }
            }
        }

        [Category("From Condition"), DefaultValue(null)]
        public Control RefFromCondition_04
        {
            get
            {
                return ctlFrom_04;
            }
            set
            {
                if (value is IConditionOwner && !lstRefFromConditions.Contains((IConditionOwner)value))
                {
                    lstRefFromConditions.Remove((IConditionOwner)ctlFrom_04);
                    lstRefFromConditions.Add((IConditionOwner)value);

                    ctlFrom_04 = value;
                }
                else if (value == null)
                {
                    lstRefFromConditions.Remove((IConditionOwner)ctlFrom_04);
                    ctlFrom_04 = null;
                }
            }
        }

        [Category("From Condition"), DefaultValue(null)]
        public Control RefFromCondition_05
        {
            get
            {
                return ctlFrom_05;
            }
            set
            {
                if (value is IConditionOwner && !lstRefFromConditions.Contains((IConditionOwner)value))
                {
                    lstRefFromConditions.Remove((IConditionOwner)ctlFrom_05);
                    lstRefFromConditions.Add((IConditionOwner)value);

                    ctlFrom_05 = value;
                }
                else if (value == null)
                {
                    lstRefFromConditions.Remove((IConditionOwner)ctlFrom_05);
                    ctlFrom_05 = null;
                }
            }
        }

        [Category("From Condition"), DefaultValue(null)]
        public Control RefFromCondition_06
        {
            get
            {
                return ctlFrom_06;
            }
            set
            {
                if (value is IConditionOwner && !lstRefFromConditions.Contains((IConditionOwner)value))
                {
                    lstRefFromConditions.Remove((IConditionOwner)ctlFrom_06);
                    lstRefFromConditions.Add((IConditionOwner)value);

                    ctlFrom_06 = value;
                }
                else if (value == null)
                {
                    lstRefFromConditions.Remove((IConditionOwner)ctlFrom_06);
                    ctlFrom_06 = null;
                }
            }
        }

        [Category("From Condition"), DefaultValue(null)]
        public Control RefFromCondition_07
        {
            get
            {
                return ctlFrom_07;
            }
            set
            {
                if (value is IConditionOwner && !lstRefFromConditions.Contains((IConditionOwner)value))
                {
                    lstRefFromConditions.Remove((IConditionOwner)ctlFrom_07);
                    lstRefFromConditions.Add((IConditionOwner)value);

                    ctlFrom_07 = value;
                }
                else if (value == null)
                {
                    lstRefFromConditions.Remove((IConditionOwner)ctlFrom_07);
                    ctlFrom_07 = null;
                }
            }
        }

        [Category("From Condition"), DefaultValue(null)]
        public Control RefFromCondition_08
        {
            get
            {
                return ctlFrom_08;
            }
            set
            {
                if (value is IConditionOwner && !lstRefFromConditions.Contains((IConditionOwner)value))
                {
                    lstRefFromConditions.Remove((IConditionOwner)ctlFrom_08);
                    lstRefFromConditions.Add((IConditionOwner)value);

                    ctlFrom_08 = value;
                }
                else if (value == null)
                {
                    lstRefFromConditions.Remove((IConditionOwner)ctlFrom_08);
                    ctlFrom_08 = null;
                }
            }
        }

        [Category("From Condition"), DefaultValue(null)]
        public Control RefFromCondition_09
        {
            get
            {
                return ctlFrom_09;
            }
            set
            {
                if (value is IConditionOwner && !lstRefFromConditions.Contains((IConditionOwner)value))
                {
                    lstRefFromConditions.Remove((IConditionOwner)ctlFrom_09);
                    lstRefFromConditions.Add((IConditionOwner)value);

                    ctlFrom_09 = value;
                }
                else if (value == null)
                {
                    lstRefFromConditions.Remove((IConditionOwner)ctlFrom_09);
                    ctlFrom_09 = null;
                }
            }
        }

        [Category("To Condition"), DefaultValue(null)]
        public Control RefToCondition_01
        {
            get
            {
                return ctlTo_01;
            }
            set
            {
                if (value is IConditionOwner && !lstRefToConditions.Contains((IConditionOwner)value))
                {
                    lstRefToConditions.Remove((IConditionOwner)ctlTo_01);
                    lstRefToConditions.Add((IConditionOwner)value);

                    ctlTo_01 = value;
                }
                else if (value == null)
                {
                    lstRefToConditions.Remove((IConditionOwner)ctlTo_01);
                    ctlTo_01 = null;
                }
            }
        }

        [Category("To Condition"), DefaultValue(null)]
        public Control RefToCondition_02
        {
            get
            {
                return ctlTo_02;
            }
            set
            {
                if (value is IConditionOwner && !lstRefToConditions.Contains((IConditionOwner)value))
                {
                    lstRefToConditions.Remove((IConditionOwner)ctlTo_02);
                    lstRefToConditions.Add((IConditionOwner)value);

                    ctlTo_02 = value;
                }
                else if (value == null)
                {
                    lstRefToConditions.Remove((IConditionOwner)ctlTo_02);
                    ctlTo_02 = null;
                }
            }
        }

        [Category("To Condition"), DefaultValue(null)]
        public Control RefToCondition_03
        {
            get
            {
                return ctlTo_03;
            }
            set
            {
                if (value is IConditionOwner && !lstRefToConditions.Contains((IConditionOwner)value))
                {
                    lstRefToConditions.Remove((IConditionOwner)ctlTo_03);
                    lstRefToConditions.Add((IConditionOwner)value);

                    ctlTo_03 = value;
                }
                else if (value == null)
                {
                    lstRefToConditions.Remove((IConditionOwner)ctlTo_03);
                    ctlTo_03 = null;
                }
            }
        }

        [Category("To Condition"), DefaultValue(null)]
        public Control RefToCondition_04
        {
            get
            {
                return ctlTo_04;
            }
            set
            {
                if (value is IConditionOwner && !lstRefToConditions.Contains((IConditionOwner)value))
                {
                    lstRefToConditions.Remove((IConditionOwner)ctlTo_04);
                    lstRefToConditions.Add((IConditionOwner)value);

                    ctlTo_04 = value;
                }
                else if (value == null)
                {
                    lstRefToConditions.Remove((IConditionOwner)ctlTo_04);
                    ctlTo_04 = null;
                }
            }
        }

        [Category("To Condition"), DefaultValue(null)]
        public Control RefToCondition_05
        {
            get
            {
                return ctlTo_05;
            }
            set
            {
                if (value is IConditionOwner && !lstRefToConditions.Contains((IConditionOwner)value))
                {
                    lstRefToConditions.Remove((IConditionOwner)ctlTo_05);
                    lstRefToConditions.Add((IConditionOwner)value);

                    ctlTo_05 = value;
                }
                else if (value == null)
                {
                    lstRefToConditions.Remove((IConditionOwner)ctlTo_05);
                    ctlTo_05 = null;
                }
            }
        }

        [Category("To Condition"), DefaultValue(null)]
        public Control RefToCondition_06
        {
            get
            {
                return ctlTo_06;
            }
            set
            {
                if (value is IConditionOwner && !lstRefToConditions.Contains((IConditionOwner)value))
                {
                    lstRefToConditions.Remove((IConditionOwner)ctlTo_06);
                    lstRefToConditions.Add((IConditionOwner)value);

                    ctlTo_06 = value;
                }
                else if (value == null)
                {
                    lstRefToConditions.Remove((IConditionOwner)ctlTo_06);
                    ctlTo_06 = null;
                }
            }
        }

        [Category("To Condition"), DefaultValue(null)]
        public Control RefToCondition_07
        {
            get
            {
                return ctlTo_07;
            }
            set
            {
                if (value is IConditionOwner && !lstRefToConditions.Contains((IConditionOwner)value))
                {
                    lstRefToConditions.Remove((IConditionOwner)ctlTo_07);
                    lstRefToConditions.Add((IConditionOwner)value);

                    ctlTo_07 = value;
                }
                else if (value == null)
                {
                    lstRefToConditions.Remove((IConditionOwner)ctlTo_07);
                    ctlTo_07 = null;
                }
            }
        }

        [Category("To Condition"), DefaultValue(null)]
        public Control RefToCondition_08
        {
            get
            {
                return ctlTo_08;
            }
            set
            {
                if (value is IConditionOwner && !lstRefToConditions.Contains((IConditionOwner)value))
                {
                    lstRefToConditions.Remove((IConditionOwner)ctlTo_08);
                    lstRefToConditions.Add((IConditionOwner)value);

                    ctlTo_08 = value;
                }
                else if (value == null)
                {
                    lstRefToConditions.Remove((IConditionOwner)ctlTo_08);
                    ctlTo_08 = null;
                }
            }
        }

        [Category("To Condition"), DefaultValue(null)]
        public Control RefToCondition_09
        {
            get
            {
                return ctlTo_09;
            }
            set
            {
                if (value is IConditionOwner && !lstRefToConditions.Contains((IConditionOwner)value))
                {
                    lstRefToConditions.Remove((IConditionOwner)ctlTo_09);
                    lstRefToConditions.Add((IConditionOwner)value);

                    ctlTo_09 = value;
                }
                else if (value == null)
                {
                    lstRefToConditions.Remove((IConditionOwner)ctlTo_09);
                    ctlTo_09 = null;
                }
            }
        }

        [Category("To Condition"), DefaultValue(null)]
        public Control RefToCondition_10
        {
            get
            {
                return ctlTo_10;
            }
            set
            {
                if (value is IConditionOwner && !lstRefToConditions.Contains((IConditionOwner)value))
                {
                    lstRefToConditions.Remove((IConditionOwner)ctlTo_10);
                    lstRefToConditions.Add((IConditionOwner)value);

                    ctlTo_10 = value;
                }
                else if (value == null)
                { lstRefToConditions.Remove((IConditionOwner)ctlTo_10); ctlTo_10 = null; }
            }
        }

        [Category("To Condition"), DefaultValue(null)]
        public Control RefToCondition_11
        {
            get
            {
                return ctlTo_11;
            }
            set
            {
                if (value is IConditionOwner && !lstRefToConditions.Contains((IConditionOwner)value))
                {
                    lstRefToConditions.Remove((IConditionOwner)ctlTo_11);
                    lstRefToConditions.Add((IConditionOwner)value);

                    ctlTo_11 = value;
                }
                else if (value == null)
                { lstRefToConditions.Remove((IConditionOwner)ctlTo_11); ctlTo_11 = null; }
            }
        }

        [Category("To Condition"), DefaultValue(null)]
        public Control RefToCondition_12
        {
            get
            {
                return ctlTo_12;
            }
            set
            {
                if (value is IConditionOwner && !lstRefToConditions.Contains((IConditionOwner)value))
                {
                    lstRefToConditions.Remove((IConditionOwner)ctlTo_12);
                    lstRefToConditions.Add((IConditionOwner)value);

                    ctlTo_12 = value;
                }
                else if (value == null)
                { lstRefToConditions.Remove((IConditionOwner)ctlTo_12); ctlTo_12 = null; }
            }
        }

        [Category("To Condition"), DefaultValue(null)]
        public Control RefToCondition_13
        {
            get
            {
                return ctlTo_13;
            }
            set
            {
                if (value is IConditionOwner && !lstRefToConditions.Contains((IConditionOwner)value))
                {
                    lstRefToConditions.Remove((IConditionOwner)ctlTo_13);
                    lstRefToConditions.Add((IConditionOwner)value);

                    ctlTo_13 = value;
                }
                else if (value == null)
                { lstRefToConditions.Remove((IConditionOwner)ctlTo_13); ctlTo_13 = null; }
            }
        }

        [Category("To Condition"), DefaultValue(null)]
        public Control RefToCondition_14
        {
            get
            {
                return ctlTo_14;
            }
            set
            {
                if (value is IConditionOwner && !lstRefToConditions.Contains((IConditionOwner)value))
                {
                    lstRefToConditions.Remove((IConditionOwner)ctlTo_14);
                    lstRefToConditions.Add((IConditionOwner)value);

                    ctlTo_14 = value;
                }
                else if (value == null)
                { lstRefToConditions.Remove((IConditionOwner)ctlTo_14); ctlTo_14 = null; }
            }
        }

        [Category("To Condition"), DefaultValue(null)]
        public Control RefToCondition_15
        {
            get
            {
                return ctlTo_15;
            }
            set
            {
                if (value is IConditionOwner && !lstRefToConditions.Contains((IConditionOwner)value))
                {
                    lstRefToConditions.Remove((IConditionOwner)ctlTo_15);
                    lstRefToConditions.Add((IConditionOwner)value);

                    ctlTo_15 = value;
                }
                else if (value == null)
                { lstRefToConditions.Remove((IConditionOwner)ctlTo_15); ctlTo_15 = null; }
            }
        }

        #endregion
    }

    /// <summary>
    /// Class Name : PeriodConditionHelper<br/>
    /// Summary    : Dynamic Period Condition control helper Class<br/>
    /// Author     : Miracom HyungSuk, Yang<br/>
    /// First Date : 2009-12-01<br/>
    /// Description: Every controls that have the instance of this class and implement IConditionOwner interface can be connected with each other.<br/>
    /// History    : <br/>
    /// </summary>
    public class PeriodConditionHelper : ConditionHelper
    {
        #region " MEMBER FIELD "

        IPeriodCondition parent;

        #endregion

        #region " CREATOR "

        /// <summary>
        /// Initialize PeriodCondition
        /// </summary>
        /// <param name="parent"></param>
        public PeriodConditionHelper(IPeriodCondition parent)
        {
            this.parent = parent;
        }

        #endregion

        #region " PROPERTY "

        /// <summary>
        /// Gets the start date.
        /// </summary>
        public string StartDate
        {
            get { return parent.StartDate; }
        }

        /// <summary>
        /// Gets the end date.
        /// </summary>
        public string EndDate
        {
            get { return parent.EndDate; }
        }

        /// <summary>
        /// Gets the selected date string.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override string ConditionValueString
        {
            get { return StartDate + "~" + EndDate; }
            set { }
        }

        /// <summary>
        /// Gets the string array.(0=StartDate, 1=EndDate)
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override object ConditionValueObject
        {
            get { return (new string[] { StartDate, EndDate }); }
        }

        /// <summary>
        /// Gets the condition Type
        /// </summary>
        public override ConditionTypeItems A_ConditionType
        {
            get
            {
                return ConditionTypeItems.Period;
            }
            set
            {
            }
        }

        #endregion

        #region " METHOD "

        public override void UpdateRefToConditions()
        {
            foreach (IConditionOwner condition in lstRefToConditions)
            {
                condition.ConditionInfo.ResetItems();
            }
        }

        public override void ResetItems()
        {
        }

        #endregion
    }

    #endregion

}

namespace DACrux.SP.Controls.Configuration
{
    /// <summary>
    /// Condition types.
    /// </summary>
    [Flags]
    public enum ConditionTypeItems
    {
        None = 0,
        Facility = 1 << 0,
        Device = 1 << 1,
        Product = 1 << 2,
        Corporation = 1 << 3,
        Customer = 1 << 4,
        Vendor = 1 << 5,
        Flow = 1 << 6,
        Oper = 1 << 7,
        Resource = 1 << 8,
        StartDate = 1 << 9,
        EndDate = 1 << 10,
        Period = StartDate | EndDate,
        Para = 1 << 11,
        ParaGroup = 1 << 12,
        User = 1 << 13,
        Material = 1 << 14,
        MaterialType = 1 << 15,
        EndUserProduct = 1 << 16,
        InspType = 1 << 17,
        InspLevel = 1 << 18,
        SecurityGroup = 1 << 19
    }

    /// <summary>
    /// Company types.
    /// </summary>
    public enum CorporationType { All, Customer, Vendor }

    /// <summary>
    /// Class Name : ConditionUtil<br/>
    /// Summary    : ConditionHelper data logic Class<br/>
    /// Author     : 미라콤 양형석<br/>
    /// First Date : 2010-01-10<br/>
    /// Description: Designed for the seperation of the Behaviour logic and the Data logic.<br/>
    /// History    : <br/>
    /// </summary>
    public static class ConditionUtil
    {
        /// <summary>
        /// Gets the datatable from the Database.
        /// </summary>
        /// <param name="item">condition type</param>
        /// <param name="oConditionData">user input condition data</param>
        /// <param name="strSupplementQuery">supplement query string</param>
        /// <returns>datatable</returns>
        public static DataTable GetDataTable(ConditionTypeItems item, IDictionary<ConditionTypeItems, string> oConditionData, string strSupplementQuery)
        {
            return null;

            //    QMS.Common.RO.UserControl oUserControl = null;
            //    QMS.Common.RO.Sampling oSampling = null;

            //    DataTable dtDataSource = null;

            //    try
            //    {
            //        oUserControl = new QMS.Common.RO.UserControl();

            //        switch (item)
            //        {
            //            case ConditionTypeItems.Facility:
            //                dtDataSource = oUserControl.GetFacilityList(strSupplementQuery);
            //                break;
            //            case ConditionTypeItems.Corporation:
            //                dtDataSource = oUserControl.GetCorpList(
            //                    oConditionData[ConditionTypeItems.Facility]
            //                    , string.Empty
            //                    , strSupplementQuery);
            //                break;
            //            case ConditionTypeItems.Customer:
            //                dtDataSource = oUserControl.GetCorpList(
            //                    oConditionData[ConditionTypeItems.Facility]
            //                    , "C"
            //                    , strSupplementQuery);
            //                break;
            //            case ConditionTypeItems.Vendor:
            //                dtDataSource = oUserControl.GetCorpList(
            //                    oConditionData[ConditionTypeItems.Facility]
            //                    , "V"
            //                    , strSupplementQuery);
            //                break;
            //            case ConditionTypeItems.Device:
            //                dtDataSource = oUserControl.GetDeviceList(
            //                    oConditionData[ConditionTypeItems.Facility]
            //                    , oConditionData[ConditionTypeItems.Customer]
            //                    , strSupplementQuery);
            //                break;
            //            case ConditionTypeItems.Product:
            //                dtDataSource = oUserControl.GetProductList(
            //                    oConditionData[ConditionTypeItems.Facility]
            //                    , oConditionData[ConditionTypeItems.Customer]
            //                    , oConditionData[ConditionTypeItems.Device]
            //                    , strSupplementQuery);
            //                break;
            //            case ConditionTypeItems.Flow:
            //                dtDataSource = oUserControl.GetFlowList(
            //                    oConditionData[ConditionTypeItems.Facility]
            //                    , oConditionData[ConditionTypeItems.Product]
            //                    , "1"
            //                    , strSupplementQuery);
            //                break;
            //            case ConditionTypeItems.Oper:
            //                dtDataSource = oUserControl.GetOperList(
            //                    oConditionData[ConditionTypeItems.Facility]
            //                    , oConditionData[ConditionTypeItems.Flow]
            //                    , strSupplementQuery);
            //                break;
            //            case ConditionTypeItems.Resource:
            //                dtDataSource = oUserControl.GetResList(
            //                    oConditionData[ConditionTypeItems.Facility]
            //                    , oConditionData[ConditionTypeItems.Oper]
            //                    , strSupplementQuery);
            //                break;
            //            case ConditionTypeItems.Para:
            //                dtDataSource = oUserControl.GetParaList(
            //                    oConditionData[ConditionTypeItems.Facility]
            //                    , oConditionData[ConditionTypeItems.Flow]
            //                    , oConditionData[ConditionTypeItems.Oper]
            //                    , strSupplementQuery); // paragroup, defectflag 조건 추가시..
            //                break;
            //            case ConditionTypeItems.ParaGroup:
            //                dtDataSource = oUserControl.GetParaGroupList(
            //                    oConditionData[ConditionTypeItems.Facility]
            //                    , oConditionData[ConditionTypeItems.Flow]
            //                    , oConditionData[ConditionTypeItems.Oper]
            //                    , strSupplementQuery);
            //                break;
            //            case ConditionTypeItems.User:
            //                dtDataSource = oUserControl.GetUserList(strSupplementQuery);
            //                break;
            //            case ConditionTypeItems.MaterialType:
            //                dtDataSource = oUserControl.GetMaterialTypeList(
            //                    oConditionData[ConditionTypeItems.Facility]
            //                    , strSupplementQuery);
            //                break;
            //            case ConditionTypeItems.Material:
            //                dtDataSource = oUserControl.GetMaterialList(
            //                    oConditionData[ConditionTypeItems.Facility]
            //                    , oConditionData[ConditionTypeItems.MaterialType]
            //                    , strSupplementQuery);
            //                break;
            //            case ConditionTypeItems.EndUserProduct:
            //                dtDataSource = oUserControl.GetEndModel(
            //                    oConditionData[ConditionTypeItems.Facility]
            //                    , oConditionData[ConditionTypeItems.Customer]
            //                    , strSupplementQuery);
            //                break;
            //            case ConditionTypeItems.InspType:
            //                oSampling = new QMS.Common.RO.Sampling();
            //                dtDataSource = oSampling.GetInspType();
            //                break;
            //            case ConditionTypeItems.InspLevel:
            //                oSampling = new QMS.Common.RO.Sampling();
            //                dtDataSource = oSampling.GetInspLevel(
            //                    oConditionData[ConditionTypeItems.InspType]);
            //                break;
            //            case ConditionTypeItems.SecurityGroup:
            //                dtDataSource = oUserControl.GetSecurityGroup(strSupplementQuery);
            //                break;
            //            default:
            //                break;
            //        }

            //        return dtDataSource;
            //    }
            //    catch (Exception ex)
            //    {
            //        throw ex;
            //    }
            //    finally
            //    {
            //        oUserControl = null;
            //    }
            //}
        }

        #region " ~~~ "
        //public static class QMSCommonRO
        //{
        //    public static DataTable GetConfigValue(string strCategory)
        //    {
        //        return new QMS.Common.RO.CodeConfig().GetConfigValue(strCategory);
        //    }

        //    public static DataTable GetConfigValue(string strCategory, string strSupplementQuery)
        //    {
        //        return new QMS.Common.RO.CodeConfig().GetConfigValue(strCategory, strSupplementQuery);
        //    }

        //    public static void DeleteOperRouting(string strFacility, string strFlow)
        //    {
        //        new QMS.Common.RO.UserControl().DeleteOperRouting(strFacility, strFlow);
        //    }

        //    public static void DeleteResRouting(string strFacility, string strOper)
        //    {
        //        new QMS.Common.RO.UserControl().DeleteResRouting(strFacility, strOper);
        //    }

        //    public static void DeleteFlowRouting(string strFacility, string strProduct, int iVersion)
        //    {
        //        new QMS.Common.RO.UserControl().DeleteFlowRouting(strFacility, strProduct, iVersion);
        //    }

        //    public static void SetFlowRouting(string[,] arrParam)
        //    {
        //        new QMS.Common.RO.UserControl().SetFlowRouting(arrParam);
        //    }

        //    public static void SetOperRouting(string[,] arrParam)
        //    {
        //        new QMS.Common.RO.UserControl().SetOperRouting(arrParam);
        //    }

        //    public static void SetResRouting(string[,] arrParam)
        //    {
        //        new QMS.Common.RO.UserControl().SetResRouting(arrParam);
        //    }
        //}
        #endregion
    }
}
namespace DACrux.SP.Controls
{
    public class RegexBuilders
    {
        public string RegularExpression;
        public string ignore;
        public string etc;
        public string Qu;
        public string group;
        public string zero;
        public string one;
        public string zerone;
        public string Ntime;
        public string Nleasttime;
        public string Nfromtime;
        public string groupname;
        public string groupequalstring;
        public string tab;
        public string returnHome;
        public string newline;
        public string Word;
        public string nonWord;
        public string space;
        public string nonspace;
        public string decimals;
        public string nondecimals;
        public string StartLine;
        public string EndLine;
    }
    public class RexEntry
    {
        public RegexBuilders regbuilder = new RegexBuilders();
        public string name;
        public string label;
        public string valuegroup;
        public string valuetype;
        public string sep;
        public string array;
        public string valuemusst;
        public string streammode;
        public string save;
        public string remove;
        public string find;
    }
}
