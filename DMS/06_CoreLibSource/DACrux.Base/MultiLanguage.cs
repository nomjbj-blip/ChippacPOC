/*----------------------------------------------------------------------------------------------------------
--  상기 프로그램에 대한 저작권을 포함한 지적재산권은 (주)미라콤아이앤씨에 있으며, (주)미라콤아이앤씨가
--  명시적으로 허용하지 않은 사용, 복사, 변경, 제3자에의 공개, 배포는 엄격히 금지되며, (주)미라콤아이앤씨의
--  지적재산권 침해에 해당됩니다.
--  (Copyright ⓒ 2014 Miracom Inc. All Rights Reserved | Confidential)
--
--  Program Id      : MultiLanguage.cs
--  Creator         : YoungShin Lim (YSIM)
--  Create Date     : 2014.11.14
--  Description     : DACrux Core::Multi Laguage 처리
--  History         : Created by YSIM at 2014.11.14
 * ********************************************************************************************************
 
----------------------------------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using System.Diagnostics;

namespace DACrux.Base
{
    public static class MultiLanguage
    {
        public static string[] LanguageList = null;
        private static string[,] m_ArrGBLSTring = null;
        private static string[] m_ArrKEYSTSring = null;

        public static DataSet LoadLanguage(string LanguageFile, bool CreateDefault)
        {
            try
            {
                DataSet m_ds = new DataSet();

                if (File.Exists(LanguageFile))
                {
                    m_ds.ReadXml(LanguageFile);
                }
                else
                {
                    if (CreateDefault)
                    {
                        #region ★ Default
                        m_ds.Tables.Add("LANG_LIST");
                        /// Column
                        m_ds.Tables["LANG_LIST"].Columns.Add(new DataColumn("NUMBER", typeof(int)));
                        m_ds.Tables["LANG_LIST"].Columns.Add(new DataColumn("COUNTRY", typeof(string)));
                        m_ds.Tables["LANG_LIST"].Columns.Add(new DataColumn("LANGUAGE", typeof(string)));
                        m_ds.Tables["LANG_LIST"].Columns.Add(new DataColumn("DESCRIPTION", typeof(string)));

                        /// Default Row
                        //m_ds.Tables["LANG_LIST"].Rows.Add(new object[] { 0, "Global", "English", "" });
                        //m_ds.Tables["LANG_LIST"].Rows.Add(new object[] { 1, "대한민국", "한글", "" });
                        //m_ds.Tables["LANG_LIST"].Rows.Add(new object[] { 2, "中國", "中國語", "" });
                        //메뉴언어 변경시 CAPTION00 + m_iSelectLang.ToString() 형태로 변경하므로 1부터 시작하도록 변경
                        m_ds.Tables["LANG_LIST"].Rows.Add(new object[] { 1, "Global", "English", "" });
                        m_ds.Tables["LANG_LIST"].Rows.Add(new object[] { 2, "대한민국", "한글", "" });
                        m_ds.Tables["LANG_LIST"].Rows.Add(new object[] { 3, "中國", "中國語", "" });

                        m_ds.Tables.Add("GLOBAL");
                        /// Column
                        m_ds.Tables["GLOBAL"].Columns.Add(new DataColumn("CAPTIONKEY", typeof(string)));
                        m_ds.Tables["GLOBAL"].Columns.Add(new DataColumn("CAPTION001", typeof(string)));
                        m_ds.Tables["GLOBAL"].Columns.Add(new DataColumn("CAPTION002", typeof(string)));
                        m_ds.Tables["GLOBAL"].Columns.Add(new DataColumn("CAPTION003", typeof(string)));
                        m_ds.Tables["GLOBAL"].Columns.Add(new DataColumn("CAPTION004", typeof(string)));
                        m_ds.Tables["GLOBAL"].Columns.Add(new DataColumn("CAPTION005", typeof(string)));

                        /// Default Row
                        m_ds.Tables["GLOBAL"].Rows.Add(new object[] { "System Option", "System Option", "시스템 설정", "系统设置", "", "" });
                        m_ds.Tables["GLOBAL"].Rows.Add(new object[] { "Language", "Language", "언어", "语言", "", "" });
                        m_ds.Tables["GLOBAL"].Rows.Add(new object[] { "Application Server", "Application Server", "어플리케이션 서버", "应用服务器", "", "" });
                        m_ds.Tables["GLOBAL"].Rows.Add(new object[] { "Server IP", "Server IP", "서버주소", "服务器地址", "", "" });
                        m_ds.Tables["GLOBAL"].Rows.Add(new object[] { "H101", "H101", "H101", "H101", "", "" });
                        m_ds.Tables["GLOBAL"].Rows.Add(new object[] { "Server Port", "Server Port", "서버포트", "服务器端口", "", "" });
                        m_ds.Tables["GLOBAL"].Rows.Add(new object[] { "Channel", "Channel", "채널", "频道", "", "" });

                        m_ds.Tables["GLOBAL"].Rows.Add(new object[] { "Factory", "Factory", "공장", "工厂", "", "" });
                        m_ds.Tables["GLOBAL"].Rows.Add(new object[] { "User ID", "User ID", "사용자아이디", "用户名", "", "" });
                        m_ds.Tables["GLOBAL"].Rows.Add(new object[] { "Password", "Password", "비밀번호", "密码", "", "" });
                        m_ds.Tables["GLOBAL"].Rows.Add(new object[] { "Option", "Option", "옵션", "选项", "", "" });
                        m_ds.Tables["GLOBAL"].Rows.Add(new object[] { "Cancel", "Cancel", "취소", "取消", "", "" });
                        m_ds.Tables["GLOBAL"].Rows.Add(new object[] { "OK", "OK", "승인", "确认", "", "" });

                        m_ds.WriteXml(LanguageFile, XmlWriteMode.WriteSchema);


                        #endregion
                    }
                }

                LanguageList = new string[m_ds.Tables["LANG_LIST"].Rows.Count];
                for (int i = 0; i < m_ds.Tables["LANG_LIST"].Rows.Count; i++)
                {
                    LanguageList[i] = m_ds.Tables["LANG_LIST"].Rows[i]["LANGUAGE"].ToString();
                }

                m_ArrKEYSTSring = new string[m_ds.Tables["GLOBAL"].Rows.Count];
                m_ArrGBLSTring = new string[m_ds.Tables["GLOBAL"].Rows.Count, 5];

                for (int i = 0; i < m_ds.Tables["GLOBAL"].Rows.Count; i++)
                {
                    m_ArrKEYSTSring[i] = m_ds.Tables["GLOBAL"].Rows[i]["CAPTIONKEY"].ToString();
                    m_ArrGBLSTring[i, 0] = m_ds.Tables["GLOBAL"].Rows[i]["CAPTION001"].ToString();
                    m_ArrGBLSTring[i, 1] = m_ds.Tables["GLOBAL"].Rows[i]["CAPTION002"].ToString();
                    m_ArrGBLSTring[i, 2] = m_ds.Tables["GLOBAL"].Rows[i]["CAPTION003"].ToString();
                    m_ArrGBLSTring[i, 3] = m_ds.Tables["GLOBAL"].Rows[i]["CAPTION004"].ToString();
                    m_ArrGBLSTring[i, 4] = m_ds.Tables["GLOBAL"].Rows[i]["CAPTION005"].ToString();
                }

                return m_ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string Translation(string KeyString, int LangNum = -1)
        {
            try
            {
                //이미 선택된 Laguage를 사용할때
                if (LangNum == -1)
                {
                    LangNum = DACrux.Base.GlobalVariable.LanguageNumber;
                }

                if (LangNum < 0) throw new Exception("Can't using language");
                if (m_ArrKEYSTSring == null) return KeyString;
                int idxWord = Array.IndexOf(m_ArrKEYSTSring, KeyString);
                if (idxWord < 0) return KeyString;
                if (m_ArrGBLSTring[idxWord, LangNum].Trim().Length == 0) return KeyString;
                return m_ArrGBLSTring[idxWord, LangNum];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string Translation(string KeyString)
        {
            try
            {
                if (m_ArrKEYSTSring == null) return KeyString;

                string tmpString = KeyString.Trim(); // 앞뒤 공백 취소 작업
                int idxWord = Array.IndexOf(m_ArrKEYSTSring, tmpString);

                // 동일 내용이 없거나 비어있으면 원래 값 Return
                if (idxWord < 0) return KeyString;
                if (m_ArrGBLSTring[idxWord, DACrux.Base.GlobalVariable.LanguageNumber].Trim().Length == 0) return KeyString;


                /// 앞뒤 공백을 그대로 살려서 처리함
                tmpString = KeyString.Replace(tmpString, m_ArrGBLSTring[idxWord, DACrux.Base.GlobalVariable.LanguageNumber]);
                return tmpString;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return KeyString;
            }
        }

        //public static void Translation(Control.ControlCollection oControls)
        //{
        //    if (m_ArrKEYSTSring == null) return;
        //    try
        //    {
        //        foreach (Control ct in oControls)
        //        {
        //            if (ct.Controls != null && ct.Controls.Count > 0)
        //            {
        //                Translation(ct.Controls);
        //            }

        //            if (ct.GetType() == typeof(Label)
        //                || ct.GetType() == typeof(Button)
        //                || ct.GetType() == typeof(CheckBox)
        //                || ct.GetType() == typeof(RadioButton))
        //            {
        //                if (ct.Text != null)
        //                {
        //                    ct.Text = Translation(ct.Text);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

    }
}
