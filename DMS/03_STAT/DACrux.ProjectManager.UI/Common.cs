using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace DACrux.ProjectManager.UI
{
    #region " ENUM "
    public enum StatType
    {
        None,
        DescriptiveAnalysis,
        HypothesisTesting,
        CorrelationAnalysis,
        ANOVA,
        RegressionAnalysis,
        DecisionOfSampleSize_1_SampleZ,
        DecisionOfSampleSize_1_SampleT,
        DecisionOfSampleSize_1_Proposition,
        DecisionOfSampleSize_2_Proposition,
        CpCpkAnalysis_Continuous,
        CpCpkAnalysis_Attribute,
        MSA_DataDesign,
        MSA_GageRR,
        MSA_RunChart,
        MSA_LineateAccuracy,
        DOE_FactorialDesign,
        DOE_FactorialAnalysis,
        DOE_TaguchiDesign,
        DOE_TaguchiAnalysis
    }

    public enum GraphType
    {
        None,
        Pie,
        Bar,
        Scatter,
        Line,
        BoxPlot,
        Histogram,
        Pareto,
        Point,
        Line4Taguchi
    }

    public enum DateTimeFormat
    {
        Full,
        LongDate,
        ShortDate,
        LongTime,
        ShortTime,
    }

    public enum DataType
    {
        NUMBER = 0,
        TEXT = 1,
        DATETIME = 2
    }

    #endregion 

    #region " DELEGATE "

    public delegate void NewDataSourceSplitedHandler(string name, DataTable dt);
    public delegate void GraphDeletedHandler(GraphInformation graphInfo);
    public delegate void StatDeletedHandler(StatInformation statInfo);
    public delegate void StatChangedHandler(StatInformation statInfo);
    public delegate void ImageClickedHandler(string imagePath);
    public delegate void ResultObjsRemovedHandler(string[] imagePath);

    #endregion

    #region " CLASS "

    public static class Common
    {
        public static readonly string TempPath = Application.StartupPath + @"\Temp\";

        public static readonly List<string> DateTimeFormats = new List<string>(new string[] 
        {
            "yyyy-MM-dd HH:mm:ss"
            , "yyyy-MM-dd"
            , "MM-dd"
            , "HH:mm:ss"
            , "HH:mm"
        });

        #region [ ListView Util ]

        public static void MoveListViewItem(ListView lvSource, ListView lvTarget)
        {
            MoveListViewItem(lvSource, lvTarget, null);
        }

        public static void MoveListViewItem(ListView lvSource, ListView lvTarget, ListViewItem lvSourceItem)
        {
            ListViewItem lvItem = lvSourceItem;
            int index;

            if (lvItem != null)
            {
                index = lvItem.Index;
                lvSource.Items.Remove(lvItem);
                lvTarget.Items.Add(lvItem);
                lvItem.Selected = false;

                if (lvSource.Items.Count > 0)
                {
                    if (index == lvSource.Items.Count)
                        index--;

                    lvSource.Focus();
                    lvSource.Items[index].Selected = true;
                    lvSource.Items[index].Focused = true;
                }
            }
            else
            {
                if (lvSource.SelectedItems.Count > 0)
                {
                    lvItem = lvSource.SelectedItems[0];
                    index = lvItem.Index;
                    lvSource.Items.Remove(lvItem);
                    lvTarget.Items.Add(lvItem);
                    lvItem.Selected = false;

                    if (lvSource.Items.Count > 0)
                    {
                        if (index == lvSource.Items.Count)
                            index--;

                        lvSource.Focus();
                        lvSource.Items[index].Selected = true;
                        lvSource.Items[index].Focused = true;
                    }
                }
            }
        }

        public static void MoveListViewItems(ListView lvSource, ListView lvTarget, IList lvSourceItems)
        {
            ListViewItem lvItem;
            int index;

            if (lvSourceItems != null)
            {
                foreach (object obj in lvSourceItems)
                {
                    lvItem = (ListViewItem)obj;

                    index = lvItem.Index;
                    lvSource.Items.Remove(lvItem);
                    lvTarget.Items.Add(lvItem);
                    lvItem.Selected = false;

                    if (lvSource.Items.Count > 0)
                    {
                        if (index == lvSource.Items.Count)
                            index--;

                        lvSource.Focus();
                        lvSource.Items[index].Selected = true;
                        lvSource.Items[index].Focused = true;
                    }
                }
            }
        }

        #endregion

        #region [ DataTable Util ]

        public static DataTable ConvertToDataViewCompatibleDataTable(DataTable dtSource)
        {
            DataTable dtResult = null;
            DataColumn column = null;

            string strTemp = "T@E#M$P%_";

            try
            {
                if (dtSource == null || dtSource.Rows.Count < 1)
                    return null;

                dtSource.AcceptChanges();
                dtResult = dtSource.Copy();

                for (int i = dtResult.Columns.Count - 1; i >= 0; i--)
                {
                    if (dtResult.Columns[i].DataType != typeof(string))
                    {
                        column = new DataColumn(strTemp + dtResult.Columns[i].ColumnName, typeof(string));
                        dtResult.Columns.Add(column);

                        int iNewColIndex = column.Ordinal;
                        for (int j = 0; j < dtResult.Rows.Count; j++)
                        {
                            dtResult.Rows[j][iNewColIndex] = dtResult.Rows[j][i];
                        }

                        dtResult.Columns.RemoveAt(i);
                        column.SetOrdinal(i);

                        dtResult.AcceptChanges();
                    }
                }

                for (int i = 0; i < dtResult.Columns.Count; i++)
                    dtResult.Columns[i].ColumnName = dtResult.Columns[i].ColumnName.Replace(strTemp, "");
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            return dtResult;
        }

        public static void ShowDataTable(string name, DataTable dtBind)
        {
            string strDtContent = string.Empty;

            for (int j = 0; j < dtBind.Columns.Count; j++)
            {
                strDtContent += "\t" + dtBind.Columns[j].ColumnName;
            }
            strDtContent += "\r\n1";

            for (int i = 0; i < dtBind.Rows.Count; i++)
            {
                for (int j = 0; j < dtBind.Columns.Count; j++)
                {
                    strDtContent += "\t" + dtBind.Rows[i][j].ToString();
                }
                strDtContent += "\r\n" + (i + 2).ToString();
            }

            MessageBox.Show(strDtContent.Substring(0, strDtContent.LastIndexOf("\r\n")), name, MessageBoxButtons.OK);
        }

        #endregion
    }

    #endregion
}
