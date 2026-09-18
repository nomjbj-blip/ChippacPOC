using System;
using System.Collections.Generic;
using System.Text;

namespace DACrux.ProjectManager
{
    public sealed class Project : ITreeViewDrawable
    {
        #region " MEMBER FIELD "

        private static Project project = null;
        private string strName = string.Empty;
        private bool isDirty = false;

        public List<WorkSheet> lstWorkSheet = new List<WorkSheet>();

        private ModelType modelType = ModelType.Project;

        private bool isSaved = false;
        private string saveFilePath = string.Empty;

        #endregion

        #region " CREATOR "

        private Project()
            : this("UntitledProject")
        {
        }

        private Project(string name)
        {
            if (name == null || name == string.Empty)
                strName = "UntitledProject";
            else
                strName = name;

            if(OnProjectCreated != null)
                OnProjectCreated(this);
        }

        public static Project GetInstance()
        {
            if (project == null)
                project = new Project();

            return project;
        }

        public static Project GetInstance(string name)
        {
            if (project != null)
            {
                project.Close();
                project = null;
            }

            project = new Project(name);

            return project;
        }

        internal void Close()
        {
            for (int i = 0; i < lstWorkSheet.Count; i++)
            {
                lstWorkSheet[i].Close();
            }
            lstWorkSheet.Clear();
            lstWorkSheet = null;

            project = null;
        }

        #endregion

        #region " PROPERTY  "

        public string Name
        {
            get { return strName; }
            set
            {
                strName = value;

                if (OnProjectRenamed != null)
                    OnProjectRenamed(strName);
            }
        }       

        public string Parent
        {
            get { return string.Empty; }
        }

        public ModelType ModelType
        {
            get { return modelType; }
        }

        public int ItemCount
        {
            get { return lstWorkSheet.Count; }
        }

        public bool Dirty
        {
            get { return isDirty; }
            set { isDirty = value; }
        }

        public string FilePath
        {
            get { return saveFilePath; }
            set { saveFilePath = value; }
        }

        public bool Saved
        {
            get { return isSaved; }
            set { isSaved = value; }
        }

        #endregion

        #region " EVENT & DELEGATE "

        public static event ProjectCreatedHandler OnProjectCreated;
        public static event WorkSheetAddedHandler OnWorkSheetAdded;
        public static event WorkSheetRemovedHandler OnWorkSheetRemoved;

        public static event ProjectRenamedHandler OnProjectRenamed;

        #endregion

        #region " METHOD "

        public void AddWorkSheet(WorkSheet workSheet)
        {
            try
            {
                lstWorkSheet.Add(workSheet);

                if (OnWorkSheetAdded != null)
                    OnWorkSheetAdded(workSheet);
            }
            catch (Exception ex)
            {
                throw (new Exception(ex.Message + " \r\n\t: Project.AddWorkSheet(WorkSheet workSheet)"));
            }
        }

        public void RemoveWorkSheet(WorkSheet workSheet)
        {
            try
            {
                if (OnWorkSheetRemoved != null)
                    OnWorkSheetRemoved(workSheet);

                workSheet.Close();
                lstWorkSheet.Remove(workSheet);
            }
            catch (Exception ex)
            {
                throw (new Exception(ex.Message + " \r\n\t: Project.RemoveWorkSheet(WorkSheet workSheet)"));
            }
        }

        public void RemoveWorkSheetAt(int index)
        {
            WorkSheet workSheet;

            try
            {
                workSheet = lstWorkSheet[index];

                if (OnWorkSheetRemoved != null)
                    OnWorkSheetRemoved(workSheet);

                lstWorkSheet.RemoveAt(index);
            }
            catch (Exception ex)
            {
                throw (new Exception(ex.Message + " \r\n\t: Project.RemoveWorkSheetAt(int index)"));
            }
        }

        public WorkSheet GetWorkSheetAt(int index)
        {
            WorkSheet workSheet;

            try
            {
                workSheet = lstWorkSheet[index];
            }
            catch (Exception ex)
            {
                throw (new Exception(ex.Message + " \r\n\t: Project.GetWorkSheeetAt(int index)"));
            }

            return workSheet;
        }

        public bool CheckWorkSheetExist(string name)
        {
            foreach (WorkSheet workSheet in lstWorkSheet)
            {
                if (workSheet.Name == name)
                    return true;
            }
            return false;
        }

        public WorkSheet GetWorkSheet(string name)
        {
            foreach (WorkSheet workSheet in lstWorkSheet)
            {
                if (workSheet.Name == name)
                    return workSheet;
            }
            return null;
        }

        public int GetWorkSheetIndex(WorkSheet workSheet)
        {
            return lstWorkSheet.IndexOf(workSheet);
        }

        public int GetWorkSheetIndex(string name)
        {
            WorkSheet workSheet = GetWorkSheet(name);

            if (workSheet != null)
                return lstWorkSheet.IndexOf(workSheet);
            else
                return -1;
        }

        public ITreeViewDrawable GetItemAt(int index)
        {
            ITreeViewDrawable drawable;

            try
            {
                drawable = lstWorkSheet[index] as ITreeViewDrawable;
            }
            catch (Exception ex)
            {
                throw (new Exception(ex.Message + " \r\n\t: Project.GetItemAt(int index)"));
            }

            return drawable;
        }

        #endregion
    }
}
