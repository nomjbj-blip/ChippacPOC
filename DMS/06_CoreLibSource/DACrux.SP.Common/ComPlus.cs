using System;
using System.Xml;
using System.Xml.Xsl;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.InteropServices.ComTypes;
using System.Diagnostics;


namespace DACrux.SP.Common
{
    public class COMWrapper
    {
        #region " Member Field "

        private Object comObject;
        private Type comType;

        #endregion

        #region " Property "

        public Object ComObject
        {
            get { return comObject; }
        }

        public Type ComType
        {
            get { return comType; }
        }

        #endregion

        #region " Creator "

        public COMWrapper(Object o)
        {
            
             
            this.comObject = o;
            this.comType = this.comObject.GetType();
        }

        public COMWrapper(String programID)
        {
            try
            {
                Type type;
                type = Type.GetTypeFromProgID(programID, true);
                this.comObject = Activator.CreateInstance(type);
                this.comType = this.comObject.GetType();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region " Method "

        public Object GetProperty(String strPropName)
        {
            Object propertyValue = this.comType.InvokeMember(strPropName,
                BindingFlags.InvokeMethod | BindingFlags.GetProperty,
                null,
                this.comObject,
                new object[] { });

            return propertyValue;
        }

        public Object GetProperty(String strPropName, String strPropType)
        {
            Object propertyValue = this.comType.InvokeMember(strPropName,
                BindingFlags.InvokeMethod | BindingFlags.GetProperty,
                null,
                this.comObject,
                new object[] { strPropType });

            return propertyValue;
        }

        public Object GetProperty(String strPropName, int iPropType)
        {
            Object propertyValue = this.comType.InvokeMember(strPropName,
                BindingFlags.InvokeMethod | BindingFlags.GetProperty,
                null,
                this.comObject,
                new object[] { iPropType });

            return propertyValue;
        }

        public void SetProperty(String strPropName, Object[] args)
        {
            this.comType.InvokeMember(strPropName,
                BindingFlags.Default | BindingFlags.SetProperty,
                null,
                this.comObject,
                args);

            return;
        }

        public Object CallMethod(String strMethodName, Object[] args)
        {
            Object objRetVal = this.comType.InvokeMember(strMethodName,
                BindingFlags.Default | BindingFlags.InvokeMethod,
                null,
                this.comObject,
                args);

            return objRetVal;
        }

        #endregion
    }

    public class COMAdminHelper
    {
        #region " Member Field "

        private COMWrapper comCatalog;
        private COMWrapper comApps;

        #endregion

        #region " Creator "

        public COMAdminHelper()
        {
            try
            {
                comCatalog = new COMWrapper("COMAdmin.COMAdminCatalog");
                comApps = new COMWrapper(comCatalog.CallMethod("GetCollection", new Object[] { "Applications" }));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        #endregion

        #region " Method "

        public string[] GetCOMApplicationList()
        {
            try
            {
                comApps.CallMethod("Populate", new object[] { });
                int count = (int)comApps.GetProperty("Count");

                string[] apps = new string[count];
                for (int i = 0; i < count; i++)
                {
                    Object objTempApp = comApps.GetProperty("item", i);
                    COMWrapper oDispTempApp = new COMWrapper(objTempApp);
                    apps[i] = oDispTempApp.GetProperty("Name").ToString();
                }

                return apps;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        public Dictionary<string, string[]> GetCOMComponentList()
        {
            Dictionary<string, string[]> dicCOMAppComponents = new Dictionary<string, string[]>();

            try
            {
                comApps.CallMethod("Populate", new object[] { });
                int cntApp = (int)comApps.GetProperty("Count");

                string strComAppName = string.Empty;
                string strComAppKey = string.Empty;
                string[] arrComponent = null;

                for (int i = 0; i < cntApp; i++)
                {
                    Object objApp = comApps.GetProperty("item", i);
                    COMWrapper oApp = new COMWrapper(objApp);

                    strComAppName = oApp.GetProperty("Name").ToString();
                    strComAppKey = oApp.GetProperty("Key").ToString();

                    COMWrapper comComponents = new COMWrapper(comApps.CallMethod("GetCollection", new Object[] { "Components", strComAppKey }));
                    comComponents.CallMethod("Populate", new object[] { });
                    int cntComponent = (int)comComponents.GetProperty("Count");
                    arrComponent = new string[cntComponent];

                    for (int j = 0; j < cntComponent; j++)
                    {
                        Object objComponent = comComponents.GetProperty("item", j);
                        COMWrapper oComponent = new COMWrapper(objComponent);
                        arrComponent[j] = oComponent.GetProperty("Name").ToString();
                    }

                    dicCOMAppComponents.Add(strComAppName, arrComponent);
                }

                return dicCOMAppComponents;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }
        
        public bool DeleteComPlusApp(String strAppName)
        {
            bool blnRetVal = false;
            comApps.CallMethod("Populate", new object[] { });
            int j = (int)comApps.GetProperty("Count"), i = 0;
            while (i < j)
            {
                Object objTempApp = comApps.GetProperty("item", i);
                COMWrapper oDispTempApp = new COMWrapper(objTempApp);

                if (oDispTempApp.GetProperty("Name").ToString() == strAppName)
                {
                    comApps.CallMethod("Remove", new object[] { i });
                    comApps.CallMethod("SaveChanges", new object[] { });
                    blnRetVal = true;
                    return blnRetVal;
                }
                i = i + 1;
            }
            return blnRetVal;
        }

        public void InstallComponent(String strAppName, String strComponentPath)
        {
            comCatalog.CallMethod("InstallComponent", new Object[] { strAppName, strComponentPath, "", "" });
        }

        #endregion
    }
}