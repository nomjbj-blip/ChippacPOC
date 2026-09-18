using System;
using System.Collections.Generic;
using System.Text;

namespace DACrux.ProjectManager
{
    public abstract class Analysis
    {
        #region " MEMBER FIELD "

        public static Project oProject;
        public WorkSheet oWorkSheet;
        
        protected string strName;
        protected ModelType modelType;
        
        #endregion

        #region " PROPERTY "
        
        public string Name
        {
            get { return strName; }
            set
            {
                strName = value;

                if (OnAnalysisRenamed != null)
                    OnAnalysisRenamed(strName);
            }
        }

        public abstract object AnalysisData
        {
            get;
        }

        public string Parent
        {
            get { return oWorkSheet.Name; }
        }

        public int ItemCount
        {
            get { return 0; }
        }

        public ModelType ModelType
        {
            get { return modelType; }
        }

        #endregion

        #region " METHOD "

        public ITreeViewDrawable GetItemAt(int index)
        {
            return null;
        }

        abstract internal bool Saving();

        abstract internal void Saved();

        abstract internal void Close();

        #endregion

        #region " EVENT & DELEGATE "

        public event AnalysisRenamedHandler OnAnalysisRenamed;

        #endregion
    }
}
