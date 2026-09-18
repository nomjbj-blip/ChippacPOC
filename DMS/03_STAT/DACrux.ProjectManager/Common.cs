using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace DACrux.ProjectManager
{
    #region " ENUM "

    public enum DataSourceType
    {
        External,
        Internal
    }

    public enum ModelType
    {
        None = 0,
        Project,
        WorkSheet,
        GraphAnalysis,
        StatAnalysis,
        Log
    }

    public enum MenuType
    {
        None,
        FILE_NEW_PROJECT,
        FILE_NEW_WORKSHEET,
        FILE_OPEN_PROJECT,
        FILE_IMPORT_FILE,
        FILE_IMPORT_DATABASE_SOURCE,
        FILE_SAVE_PROJECT,
        FILE_SAVE_PROJECT_AS,
        FILE_EXPORT_WORKSHEET,
        FILE_RENAME_PROJECT,
        FILE_RENAME_WORKSHEET,
        FILE_REMOVE_PROJECT,
        FILE_REMOVE_WORKSHEET,
        FILE_CLOSE_PROJECT,
        FILE_CLOSE_WORKSHEET,
        DATA_COLUMN_SETTING,
        DATA_INSERT,
        DATA_REMOVE,
        DATA_MOVE_COLUMN,
        DATA_SORT,
        DATA_SPLIT,
        DATA_SPLIT_SELECTED,
        DATA_TRANSPOSE,
        DATA_EXPORT_TO_EXCEL,
        STAT_DESC_ANALYSIS,
        STAT_HYPOTHESIS,
        STAT_CORRELATION,
        STAT_REGRESSION,
        STAT_ANOVA,
        STAT_CCONTINUOUS,
        STAT_ATTRIBUTE,
        STAT_DATA_DESIGN,
        STAT_GAGE_RR,
        STAT_RUN_CHART,
        STAT_LIN_ACC,
        STAT_FACTOR_DESIGN,
        STAT_FACTOR_ANALYSIS,
        STAT_TAGUCHI_DESIGN,
        STAT_TAGUCHI_ANALYSIS,
        STAT_ORTHO_DESIGN,
        STAT_ORTHO_ANALYSIS,
        STAT_1_SAMPLE_Z,
        STAT_1_SAMPLE_T,
        STAT_1_PROPOSITION,
        STAT_2_PROPOSITION,
        GRAPH_PIE,
        GRAPH_BAR,
        GRAPH_SCATTER,
        GRAPH_LINE,
        GRAPH_BOXPLOT,
        GRAPH_PARETO,
        GRAPH_HISTOGRAM
    }

    #endregion

    #region " INTERFACE "

    public interface ITreeViewDrawable
    {
        string Name { get; set; }
        string Parent { get; }
        int ItemCount { get; }
        ModelType ModelType { get; }

        ITreeViewDrawable GetItemAt(int index);
    }

    #endregion

    #region " DELEGATE "

    public delegate void ProjectCreatedHandler(Project project);
    public delegate void ProjectRenamedHandler(string name);

    public delegate void WorkSheetAddedHandler(WorkSheet workSheet);
    public delegate void WorkSheetRemovedHandler(WorkSheet workSheet);
    public delegate void WorkSheetRenamedHandler(string name);
    public delegate void WorkSheetNameDuplicatedHandler(string inputName, string alterName);

    public delegate void AnalysisRenamedHandler(string name);
    public delegate void AnalysisAddedHandler(Analysis analysis, bool bFocus);
    public delegate void AnalysisRemovedHandler(Analysis analysis);

    public delegate void NewDataSourceAddedHandler(string name, DataTable dt);

    #endregion
}
