using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

using DACrux.ProjectManager.UI;

namespace DACrux.BStats
{
    public static class GraphAnalysisManager
    {
        public static GraphInformation GetGraphInformation(GraphType type, DataTable dataSource, List<DACrux.ProjectManager.UI.DataView.ColumnInfo> lstValidColumnInfo)
        {
            GraphInformation graphInfo = null;
            iGraphAnalysis dlg = null;

            try
            {
                switch (type)
                {
                    case GraphType.Line:
                        dlg = new GraphDialog.DlgLineGraph(dataSource, lstValidColumnInfo);
                        break;
                    case GraphType.Bar:
                        dlg = new GraphDialog.DlgBarGraph(dataSource, lstValidColumnInfo);
                        break;
                    case GraphType.Pie:
                        dlg = new GraphDialog.DlgPieGraph(dataSource, lstValidColumnInfo);
                        break;
                    case GraphType.Scatter:
                        dlg = new GraphDialog.DlgScatterGraph(dataSource, lstValidColumnInfo);
                        break;
                    case GraphType.Pareto:
                        dlg = new GraphDialog.DlgParetoGraph(dataSource, lstValidColumnInfo);
                        break;
                    case GraphType.Histogram:
                        dlg = new GraphDialog.DlgHistogramGraph(dataSource, lstValidColumnInfo);
                        break;
                    case GraphType.BoxPlot:
                        dlg = new GraphDialog.DlgBoxPlotGraph(dataSource, lstValidColumnInfo);
                        break;
                    default:
                        dlg = null;
                        break;
                }

                if (dlg != null && (dlg as System.Windows.Forms.Form).ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    graphInfo = dlg.GraphInfo;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }

            if(graphInfo != null)
                graphInfo.ViewProperty = true;

            return graphInfo;
        }

        public static void UpdateGraphInformation(GraphInformation graphInfo)
        {
            iGraphAnalysis dlg = null;

            try
            {
                switch (graphInfo.Type)
                {
                    case GraphType.Line:
                        dlg = new GraphDialog.DlgLineGraph(graphInfo);
                        break;
                    case GraphType.Bar:
                        dlg = new GraphDialog.DlgBarGraph(graphInfo);
                        break;
                    case GraphType.Pie:
                        dlg = new GraphDialog.DlgPieGraph(graphInfo);
                        break;
                    case GraphType.Scatter:
                        dlg = new GraphDialog.DlgScatterGraph(graphInfo);
                        break;
                    case GraphType.Pareto:
                        dlg = new GraphDialog.DlgParetoGraph(graphInfo);
                        break;
                    case GraphType.Histogram:
                        dlg = new GraphDialog.DlgHistogramGraph(graphInfo);
                        break;
                    case GraphType.BoxPlot:
                        dlg = new GraphDialog.DlgBoxPlotGraph(graphInfo);
                        break;
                    default:
                        dlg = null;
                        break;
                }

                if (dlg != null && (dlg as System.Windows.Forms.Form).ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    graphInfo = dlg.GraphInfo;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
