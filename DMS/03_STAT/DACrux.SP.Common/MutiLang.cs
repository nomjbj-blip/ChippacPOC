using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Windows.Forms;
using System.Data;

namespace DACrux.SP.Common
{
    
    public class MutiLang
    {
        string formatfile = Application.StartupPath + "\\" + "Lang.xml";
        public static Dictionary<string, string> SelectLang = new Dictionary<string, string>();
        private string LangPath = Application.StartupPath + "\\" + "lang";
        public void CheckLnag()
        {
            try
            {
                if (!File.Exists(LangPath) || !File.Exists(formatfile))
                    return;

                StreamReader sr = new StreamReader(LangPath);
                string Lang = sr.ReadToEnd();
                sr.Dispose();
                SelectLang.Clear();


                XmlDocument xmlDatadoc = new XmlDocument();
                //XmlDataDocument xmlDatadoc = new XmlDataDocument();

                xmlDatadoc.DataSet.ReadXml(formatfile);

                DataSet ds = new DataSet();
                
                ds = xmlDatadoc.DataSet;
                if(string.IsNullOrEmpty(ds.Tables[0].Rows[ds.Tables[0].Rows.Count-1][0].ToString()))
                    ds.Tables[0].Rows[ds.Tables[0].Rows.Count-1].Delete();
                if (Lang.ToLower().Equals("kor"))
                    SelectLang.Add("Lang", "Kor");
                else if (Lang.ToLower().Equals("eng"))
                    SelectLang.Add("Lang", "Eng");
                else
                    SelectLang.Add("Lang", "Other");
                for (int tableCnt = 0; tableCnt < ds.Tables[0].Rows.Count; tableCnt++)
                {
                    switch (Lang)
                    {
                        case "Kor":
                            SelectLang.Add(ds.Tables[0].Rows[tableCnt]["Item"].ToString(), ds.Tables[0].Rows[tableCnt]["Kor"].ToString());
                            break;

                        case "Eng":
                            SelectLang.Add(ds.Tables[0].Rows[tableCnt]["Item"].ToString(), ds.Tables[0].Rows[tableCnt]["Eng"].ToString());
                            break;
                        case "Other" :
                            SelectLang.Add(ds.Tables[0].Rows[tableCnt]["Item"].ToString(), ds.Tables[0].Rows[tableCnt]["Other"].ToString());
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
 
        }
    }
}
