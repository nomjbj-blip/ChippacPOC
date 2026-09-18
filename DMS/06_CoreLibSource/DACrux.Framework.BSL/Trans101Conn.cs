#define WINDOWS2003
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Data;
using System.EnterpriseServices;
using System.Runtime.InteropServices;
using Miracom.H101Core;
using com.miracom.transceiverx.session;
using DACrux.Base;

namespace QMS.Common.BSL
{
    public class Trans101Conn
    {
        private string m_h101_Session = "MESServer_01";
        private DeliveryType m_h101_Session_Mode = new DeliveryType(DeliveryType.REQUEST);
        private string m_h101_Server = "12.230.58.145";
        private string m_h101_Port = "10101";
        private string m_h101_Channel = "/MPYJ/MESServer";
        private int m_h101_TimeOut = 60000;
        private string m_ConnString = "Session=MESServer_01;SessionMode=Request;Server=12.230.58.145;Port=10101;Channel=/MPYJ/MESServer;Timeout=60000";
        //private int m_Port = 0;
        
        private const string FACTORY = "HMKT1";
        private char LANGUAGE = '1';

        public bool ListenerStart(string ConnectString, int Port)
        {
            try
            {
                m_ConnString = ConnectString;
                
                //System.Threading.Thread.Sleep(10000);

                string[] tmpStr = null;
                string[] tmpPara = null;
                m_ConnString = ConnectString;
                tmpStr = m_ConnString.Split(';');
                for (int i = 0; i < tmpStr.Length; i++)
                {
                    tmpPara = tmpStr[i].Split('=');
                    if (tmpPara.Length != 2) continue;
                    try
                    {
                        switch (tmpPara[0].Trim().ToUpper())
                        {
                            case "SESSION":
                                m_h101_Session = tmpPara[1].Trim().ToUpper();
                                break;
                            case "SESSIONMODE":
                                m_h101_Session_Mode = GetSessionMode(tmpPara[1].Trim().ToUpper());
                                break;
                            case "SERVER":
                                m_h101_Server = tmpPara[1].Trim().ToUpper();
                                break;
                            case "PORT":
                                m_h101_Port = tmpPara[1].Trim().ToUpper();
                                break;
                            case "CHANNEL":
                                m_h101_Channel = tmpPara[1].Trim();
                                break;
                            case "TIMEOUT":
                                m_h101_TimeOut = int.Parse(tmpPara[1].Trim().ToUpper());
                                break;
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }

                if (h101stub.Instance.init(m_h101_Session,
                                           com.miracom.transceiverx.session.Session_Fields.SESSION_INNER_STATION_MODE |
                                           com.miracom.transceiverx.session.Session_Fields.SESSION_PUSH_DELIVERY_MODE,
                                           string.Format("{0}:{1}", m_h101_Server, m_h101_Port),
                                           0) == false)
                {
                    // 101 Server 접속 실패
                    return false;
                }

                WIPCaster.WIPChannel = m_h101_Channel;

                WIP_Start_Lot_In_Tag sdhs = new WIP_Start_Lot_In_Tag();
                Cmn_Out_Tag sdghs = new Cmn_Out_Tag();

                WIPCaster.WIP_Start_Lot(sdhs, ref sdghs);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool ListenerStop()
        {
            try
            {
                h101stub.Instance.term();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool InitH101()
        {
            string[] tmpStr = null;
            string[] tmpPara = null;

            try
            {
                //m_ConnString = System.Configuration.ConfigurationSettings.AppSettings["CONNECTSTRING"].ToString();
                //m_Port = int.Parse(System.Configuration.ConfigurationSettings.AppSettings["SERVERPORT"].ToString());

                tmpStr = m_ConnString.Split(';');
                for (int i = 0; i < tmpStr.Length; i++)
                {
                    tmpPara = tmpStr[i].Split('=');
                    if (tmpPara.Length != 2) continue;
                    try
                    {
                        switch (tmpPara[0].Trim().ToUpper())
                        {
                            case "SESSION":
                                m_h101_Session = tmpPara[1].Trim().ToUpper();
                                break;
                            case "SESSIONMODE":
                                m_h101_Session_Mode = GetSessionMode(tmpPara[1].Trim().ToUpper());
                                break;
                            case "SERVER":
                                m_h101_Server = tmpPara[1].Trim().ToUpper();
                                break;
                            case "PORT":
                                m_h101_Port = tmpPara[1].Trim().ToUpper();
                                break;
                            case "CHANNEL":
                                m_h101_Channel = tmpPara[1].Trim();
                                break;
                            case "TIMEOUT":
                                m_h101_TimeOut = int.Parse(tmpPara[1].Trim().ToUpper());
                                break;
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }

                if (h101stub.Instance.init(m_h101_Session,
                                               com.miracom.transceiverx.session.Session_Fields.SESSION_INNER_STATION_MODE |
                                               com.miracom.transceiverx.session.Session_Fields.SESSION_PUSH_DELIVERY_MODE,
                                               string.Format("{0}:{1}", m_h101_Server, m_h101_Port),
                                               0) == false)
                {
                    // 101 Server 접속 실패
                    return false;
                }

                WIPCaster.WIPChannel = m_h101_Channel;

                return true;
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        private DeliveryType GetSessionMode(string strSessionMode)
        {
            switch (strSessionMode)
            {
                case "REPLY":
                    return new DeliveryType(DeliveryType.REPLY);
                case "REQUEST":
                    return new DeliveryType(DeliveryType.REQUEST);
                case "MULTICAST":
                    return new DeliveryType(DeliveryType.MULTICAST);
                case "UNICAST":
                    return new DeliveryType(DeliveryType.UNICAST);
                default:
                    return new DeliveryType(DeliveryType.REQUEST);
            }
        }

        public string HoldLot(string strLot, string strUser, string strPassWD, string strHoldCode)
        {
            #region valiable
            WIP_View_Lot_In_Tag View_Lot_In = null;
            WIP_View_Lot_Out_Tag View_Lot_Out = null;
            WIP_Hold_Lot_In_Tag in_hold = null;
            Cmn_Out_Tag out_hold = null;
            StringBuilder sb = null;

            bool bh101req = false;

            /// LOG
            ///////////////////////////////////////////////////////////////
            #endregion

            try
            {
                View_Lot_In = new WIP_View_Lot_In_Tag();
                View_Lot_Out = new WIP_View_Lot_Out_Tag();

                in_hold = new WIP_Hold_Lot_In_Tag();
                out_hold = new Cmn_Out_Tag();

                sb = new StringBuilder();

                /// Get Program Information
                //////////////////////////////////////////////////////////////////////////
                View_Lot_In.h_proc_step = '1';
                View_Lot_In.h_language = '1';
                View_Lot_In.h_user_id = strUser;
                View_Lot_In.h_factory = "HMKT1";
                View_Lot_In.lot_id = strLot;

                #region LOG [INPUT VALUE]
                //////////////////////////////////////////////////////////////////////////////////////////
                if (!InitH101())
                {
                    // 101 Server 접속 실패
                    sb.AppendFormat("{0,-1}", "1");
                    sb.AppendFormat("{0,-200}", "H101 Init error");

                    return sb.ToString();
                }
                #endregion

                WIPCaster.WIP_View_Lot(View_Lot_In, ref View_Lot_Out);

                #region LOG [OUTPUT VALUE]

                if (View_Lot_Out.h_status_value != '0')
                {
                    // Error
                    sb.AppendFormat("{0,-1}", "1");
                    if (View_Lot_Out.h_msg_code == null)
                        sb.AppendFormat("{0,-200}", h101stub.StatusMessage);
                    else
                        sb.AppendFormat("{0,-200}", View_Lot_Out.h_msg_code + View_Lot_Out.h_msg);

                    return sb.ToString();
                }

                #endregion

                in_hold._C.h_proc_step = '1';
                in_hold._C.h_language = '1';
                in_hold._C.h_factory = "HMKT1";
                in_hold._C.lot_id = strLot;
                in_hold._C.h_user_id = strUser;
                in_hold._C.oper = View_Lot_Out.oper;
                in_hold._C.mat_id = View_Lot_Out.mat_id;
                in_hold._C.mat_ver = View_Lot_Out.mat_ver;
                in_hold._C.flow = View_Lot_Out.flow;
                in_hold._C.flow_seq_num = View_Lot_Out.flow_seq_num;
                in_hold._C.last_active_hist_seq = View_Lot_Out.last_active_hist_seq;
                in_hold._C.hold_code = strHoldCode;
                in_hold._C.h_password = strPassWD;

                in_hold._C.tran_cmf_1 = "";
                in_hold._C.tran_cmf_2 = "";
                in_hold._C.tran_cmf_3 = "";
                in_hold._C.tran_cmf_4 = "";
                in_hold._C.tran_cmf_5 = "";
                in_hold._C.tran_cmf_6 = "";
                in_hold._C.tran_cmf_7 = "";
                in_hold._C.tran_cmf_8 = "";
                in_hold._C.tran_cmf_9 = "";
                in_hold._C.tran_cmf_10 = "";
                in_hold._C.tran_cmf_11 = "";
                in_hold._C.tran_cmf_12 = "";
                in_hold._C.tran_cmf_13 = "";
                in_hold._C.tran_cmf_14 = "";
                in_hold._C.tran_cmf_15 = "";
                in_hold._C.tran_cmf_16 = "";
                in_hold._C.tran_cmf_17 = "";
                in_hold._C.tran_cmf_18 = "";
                in_hold._C.tran_cmf_19 = "";
                in_hold._C.tran_cmf_20 = "";

                in_hold._C.comment = "";

                #region LOG [INPUT VALUE]
                //////////////////////////////////////////////////////////////////////////////////////////
                #endregion
                bh101req = WIPCaster.WIP_Hold_Lot(in_hold, ref out_hold);

                #region LOG [OUTPUT VALUE] & Exception
                //////////////////////////////////////////////////////////////////////////////////////////

                if (!bh101req || out_hold.h_status_value == '1')
                {
                    //Error
                    sb.AppendFormat("{0,-1}", "1");
                    sb.AppendFormat("{0,-200}", out_hold.h_msg_code + out_hold.h_msg);

                    return sb.ToString();
                }
                //////////////////////////////////////////////////////////////////////////////////////////
                #endregion

                // Success
                return "0";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        public string ReleaseLot(string strLot, string strUser, string strPassWD, bool bPassFail, string strReleaseCode)
        {
            #region valiable
            WIP_View_Lot_In_Tag View_Lot_In = null;
            WIP_View_Lot_Out_Tag View_Lot_Out = null;

            WIP_Release_Lot_In_Tag Release_Lot_In = null;
            Cmn_Out_Tag out_hold = null;

            StringBuilder sb = null;

            bool bh101req = false;
            #endregion

            try
            {
                View_Lot_In = new WIP_View_Lot_In_Tag();
                View_Lot_Out = new WIP_View_Lot_Out_Tag();

                Release_Lot_In = new WIP_Release_Lot_In_Tag();
                out_hold = new Cmn_Out_Tag();

                sb = new StringBuilder();

                /// Get Program Information
                //////////////////////////////////////////////////////////////////////////
                View_Lot_In.h_proc_step = '6';
                View_Lot_In.h_language = '1';
                View_Lot_In.h_user_id = strUser;
                View_Lot_In.h_factory = "HMKT1";
                View_Lot_In.lot_id = strLot;

                #region LOG [INPUT VALUE]
                //////////////////////////////////////////////////////////////////////////////////////////
                if (!InitH101())
                {
                    sb.AppendFormat("{0,-1}", "1");
                    sb.AppendFormat("{0,-200}", "H101 Init error");

                    return sb.ToString();
                }
                //////////////////////////////////////////////////////////////////////////////////////////
                #endregion
                
                WIPCaster.WIP_View_Lot(View_Lot_In, ref View_Lot_Out);

                #region LOG [OUTPUT VALUE]

                if (View_Lot_Out.h_status_value != '0' && View_Lot_Out.h_msg_code != "WIP-0059")
                {
                    // Error
                    sb.AppendFormat("{0,-1}", "1");
                    if (View_Lot_Out.h_msg_code == null)
                        sb.AppendFormat("{0,-200}", h101stub.StatusMessage);
                    else
                        sb.AppendFormat("{0,-200}", View_Lot_Out.h_msg_code + View_Lot_Out.h_msg);

                    return sb.ToString();
                }

                /////////////////////////////////////////////////////////////////////////////////////////////////
                #endregion

                Release_Lot_In._C.h_proc_step = '1';
                Release_Lot_In._C.h_language = '1';
                Release_Lot_In._C.h_factory = "HMKT1";
                Release_Lot_In._C.lot_id = strLot;
                Release_Lot_In._C.h_user_id = strUser;
                Release_Lot_In._C.oper = View_Lot_Out.oper;
                Release_Lot_In._C.mat_id = View_Lot_Out.mat_id;
                Release_Lot_In._C.mat_ver = View_Lot_Out.mat_ver;
                Release_Lot_In._C.flow = View_Lot_Out.flow;
                Release_Lot_In._C.flow_seq_num = View_Lot_Out.flow_seq_num;
                Release_Lot_In._C.oper = View_Lot_Out.oper;
                Release_Lot_In._C.last_active_hist_seq = View_Lot_Out.last_active_hist_seq;
                Release_Lot_In._C.h_password = strPassWD;
                Release_Lot_In._C.back_time = DateTime.Now.ToString();
                Release_Lot_In._C.release_code = strReleaseCode;

                Release_Lot_In._C.tran_cmf_1 = "";
                Release_Lot_In._C.tran_cmf_2 = "";
                Release_Lot_In._C.tran_cmf_3 = "";
                Release_Lot_In._C.tran_cmf_4 = "";
                Release_Lot_In._C.tran_cmf_5 = "";
                Release_Lot_In._C.tran_cmf_6 = "";
                Release_Lot_In._C.tran_cmf_7 = "";
                Release_Lot_In._C.tran_cmf_8 = "";
                Release_Lot_In._C.tran_cmf_9 = "";
                Release_Lot_In._C.tran_cmf_10 = "";
                Release_Lot_In._C.tran_cmf_11 = "";
                Release_Lot_In._C.tran_cmf_12 = "";
                Release_Lot_In._C.tran_cmf_13 = "";
                Release_Lot_In._C.tran_cmf_14 = "";
                Release_Lot_In._C.tran_cmf_15 = "";
                Release_Lot_In._C.tran_cmf_16 = "";
                Release_Lot_In._C.tran_cmf_17 = "";
                Release_Lot_In._C.tran_cmf_18 = "";
                Release_Lot_In._C.tran_cmf_19 = "";

                if (bPassFail)
                    Release_Lot_In._C.tran_cmf_20 = "PASS";
                else
                    Release_Lot_In._C.tran_cmf_20 = "FAIL";

                Release_Lot_In._C.comment = "";

                #region LOG [INPUT VALUE]
                //////////////////////////////////////////////////////////////////////////////////////////
                #endregion

                bh101req = WIPCaster.WIP_Release_Lot(Release_Lot_In, ref out_hold);

                #region LOG [OUTPUT VALUE] & Exception
                //////////////////////////////////////////////////////////////////////////////////////////

                if (!bh101req || out_hold.h_status_value == '1')
                {
                    // Error
                    sb.AppendFormat("{0,-1}", "1");
                    sb.AppendFormat("{0,-200}", out_hold.h_msg_code + out_hold.h_msg);

                    return sb.ToString();
                }
                //////////////////////////////////////////////////////////////////////////////////////////
                #endregion

                // Success
                return "0";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        public string MVOU(string strLot, string strUser, string strEquip, string strHandler, LOSS_BIN_TAG[] oLossBins, RETEST_IN_TAG[] oRetest, string strSample)
        {
            #region Valiable

            WIP_View_Bin_Setup_In_Tag Bin_Setup_In = null;
            WIP_View_Bin_Setup_List_Out_Tag Bin_Setup_Out = null;

            WIP_View_Lot_In_Tag View_Lot_In = null;
            WIP_View_Lot_Out_Tag View_Lot_Out = null;

            ctlgt_in_tag oLog;

            WIP_End_Lot_In_Tag in_mvou = new WIP_End_Lot_In_Tag();
            Cmn_Out_Tag out_mvou = new Cmn_Out_Tag();

            PROD_TAG oProdTag;
            StringBuilder sb = null;
            //DataTable dt = null;
            #endregion

            /// LOG
            ///////////////////////////////////////////////////////////////

            try
            {
                Bin_Setup_In = new WIP_View_Bin_Setup_In_Tag();
                Bin_Setup_Out = new WIP_View_Bin_Setup_List_Out_Tag();
                oProdTag = new PROD_TAG();
                oProdTag.init();
                oLog = new ctlgt_in_tag();

                View_Lot_In = new WIP_View_Lot_In_Tag();
                View_Lot_Out = new WIP_View_Lot_Out_Tag();

                sb = new StringBuilder();

                #region LOG [Initialize H101]
                //////////////////////////////////////////////////////////////////////////////////////////
                if (!InitH101())
                {
                    sb.AppendFormat("{0,-1}", "1");
                    sb.AppendFormat("{0,-200}", "H101 Init error");

                    return sb.ToString();
                }
                //////////////////////////////////////////////////////////////////////////////////////////
                #endregion

                Bin_Setup_In.h_proc_step = '1';
                Bin_Setup_In.h_language = LANGUAGE;
                Bin_Setup_In.h_factory = FACTORY;
                Bin_Setup_In.lot_id = strLot;
                Bin_Setup_In.h_user_id = strUser;
                Bin_Setup_In.res_id = strEquip;

                #region LOG [INPUT VALUE]
                //////////////////////////////////////////////////////////////////////////////////////////
                //////////////////////////////////////////////////////////////////////////////////////////
                #endregion

                WIPCaster.WIP_View_Bin_Setup_By_Lot(Bin_Setup_In, ref Bin_Setup_Out);

                if (Bin_Setup_Out.h_status_value != '0')
                {
                    sb.AppendFormat("{0,-5}", FACTORY);
                    sb.AppendFormat("{0,-5}", "MVOU");
                    sb.AppendFormat("{0,-1}", Bin_Setup_Out.h_status_value);
                    sb.AppendFormat("{0,-200}", Bin_Setup_Out.h_msg);
                    sb.AppendFormat("{0,-200}", Bin_Setup_Out.h_db_err_msg);
                    sb.AppendFormat("{0,-30}", Bin_Setup_Out.h_msg_code);
                    sb.AppendFormat("{0,-30}", Bin_Setup_Out.h_field_msg);
                    sb.AppendFormat("{0,-30}", "");
                    if (Bin_Setup_Out.h_status_value != '0')
                    {
                        oLog.status = string.Format("{0}", Bin_Setup_Out.h_status_value);
                        oLog.error_msg = Bin_Setup_Out.h_field_msg;
                    }
                    return sb.ToString();
                }

                /// Get Program Information
                //////////////////////////////////////////////////////////////////////////
                View_Lot_In.h_proc_step = '1';
                View_Lot_In.h_language = '1';
                View_Lot_In.h_user_id = strUser;
                View_Lot_In.h_factory = "HMKT1";
                View_Lot_In.lot_id = strLot;

                #region LOG [INPUT VALUE]
                //////////////////////////////////////////////////////////////////////////////////////////
                //////////////////////////////////////////////////////////////////////////////////////////
                #endregion

                WIPCaster.WIP_View_Lot(View_Lot_In, ref View_Lot_Out);

                #region LOG [OUTPUT VALUE]

                if (View_Lot_Out.h_status_value != '0')
                {
                    // Error
                    sb.AppendFormat("{0,-1}", "1");
                    if (View_Lot_Out.h_msg_code == null)
                        sb.AppendFormat("{0,-200}", h101stub.StatusMessage);
                    else
                        sb.AppendFormat("{0,-200}", View_Lot_Out.h_msg_code + View_Lot_Out.h_msg);

                    return sb.ToString();
                }

                /////////////////////////////////////////////////////////////////////////////////////////////////
                #endregion

                /// LOG
                /// 
                //////////////////////////////////////////////////////////
                oLog.recv_time = DateTime.Now.ToString("yyyyMMddHHmmss");
                oLog.lot_id = strLot;
                oLog.user_name = strUser;
                oLog.oper = View_Lot_Out.oper.ToString().Trim();
                oLog.mat_id = View_Lot_Out.mat_id.ToString().Trim();
                oLog.mat_ver = View_Lot_Out.mat_ver.ToString().Trim();
                oLog.flow = View_Lot_Out.flow.ToString().Trim();
                oLog.flow_seq_num = View_Lot_Out.flow_desc.ToString().Trim();
                oLog.qty1 = View_Lot_Out.qty_1.ToString().Trim();
                oLog.qty2 = View_Lot_Out.qty_2.ToString().Trim();
                oLog.qty3 = View_Lot_Out.qty_3.ToString().Trim();
                oLog.tot_split = Bin_Setup_Out.data_tbl[0].tot_split.ToString();
                oLog.last_active_hist_seq = View_Lot_Out.last_active_hist_seq.ToString().Trim();
                oLog.equip_id = strEquip;
                oLog.handler_id = strHandler;
                //////////////////////////////////////////////////////////


                WIP_END_LOT_IN_TAG_bin_list[] oInBinTbl = new WIP_END_LOT_IN_TAG_bin_list[0];

                #region Loss Bin
                in_mvou.unit1_code_1 = oLossBins[0].LOSS_CODE.Trim();
                in_mvou.unit1_qty_1 = oLossBins[0].LOSS_COUNT;
                in_mvou.unit1_code_2 = oLossBins[1].LOSS_CODE.Trim(); ;
                in_mvou.unit1_qty_2 = oLossBins[1].LOSS_COUNT;
                in_mvou.unit1_code_3 = oLossBins[2].LOSS_CODE.Trim();
                in_mvou.unit1_qty_3 = oLossBins[2].LOSS_COUNT;
                in_mvou.unit1_code_4 = "";
                in_mvou.unit1_qty_4 = 0;
                in_mvou.unit1_code_5 = "";
                in_mvou.unit1_qty_5 = 0;
                in_mvou.unit1_code_6 = "";
                in_mvou.unit1_qty_6 = 0;
                in_mvou.unit1_code_7 = "";
                in_mvou.unit1_qty_7 = 0;
                in_mvou.unit1_code_8 = "";
                in_mvou.unit1_qty_8 = 0;
                in_mvou.unit1_code_9 = "";
                in_mvou.unit1_qty_9 = 0;
                in_mvou.unit1_code_10 = "";
                in_mvou.unit1_qty_10 = 0;
                #endregion

                in_mvou._C.h_factory = FACTORY;
                in_mvou._C.h_proc_step = '1';
                in_mvou._C.h_language = LANGUAGE;
                in_mvou._C.lot_id = oLog.lot_id;
                in_mvou._C.h_user_id = oLog.user_name;
                in_mvou._C.res_id = oLog.equip_id;
                in_mvou._C.tran_cmf_2 = oLog.handler_id;
                in_mvou._C.oper = oLog.oper;
                in_mvou._C.mat_id = oLog.mat_id;
                in_mvou._C.mat_ver = DACrux.Base.ConvertNumeric.ConvertInt(oLog.mat_ver);
                in_mvou._C.flow = oLog.flow;
                in_mvou._C.flow_seq_num = DACrux.Base.ConvertNumeric.ConvertInt(oLog.flow_seq_num);
                in_mvou._C.last_active_hist_seq = DACrux.Base.ConvertNumeric.ConvertInt(oLog.last_active_hist_seq);
                in_mvou._C.bin_split = oLog.tot_split;

                if (oRetest.Length > 0)
                {
                    in_mvou.retest_list = new WIP_End_Lot_In_Tag_retest_list[oRetest.Length];

                    for (int iRow = 0; iRow < oRetest.Length; iRow++)
                    {
                        in_mvou.retest_list[iRow] = new WIP_End_Lot_In_Tag_retest_list();

                        in_mvou.retest_list[iRow].factory = FACTORY;
                        in_mvou.retest_list[iRow].flow = oLog.flow;
                        in_mvou.retest_list[iRow].flow_seq_num = DACrux.Base.ConvertNumeric.ConvertInt(oLog.flow_seq_num);
                        in_mvou.retest_list[iRow].lot_id = oLog.lot_id;
                        in_mvou.retest_list[iRow].mat_id = oLog.mat_id;
                        in_mvou.retest_list[iRow].mat_ver = DACrux.Base.ConvertNumeric.ConvertInt(oLog.mat_ver);
                        in_mvou.retest_list[iRow].oper = oLog.oper;
                        in_mvou.retest_list[iRow].res_id = oLog.equip_id;
                        in_mvou.retest_list[iRow].retest_count = oRetest[iRow].SEQ.ToString().Trim();
                        in_mvou.retest_list[iRow].retest_inqty = DACrux.Base.ConvertNumeric.Convertdouble(oRetest[iRow].RETEST_IN.ToString().Trim());
                        in_mvou.retest_list[iRow].retest_goodqty = DACrux.Base.ConvertNumeric.Convertdouble(oRetest[iRow].RETEST_OUT.ToString().Trim());
                        in_mvou.retest_list[iRow].retest_rejectqty = DACrux.Base.ConvertNumeric.ConvertInt(oRetest[iRow].RETEST_REJECT.ToString().Trim());
                        in_mvou.retest_list[iRow].retest_yield = DACrux.Base.ConvertNumeric.Convertdouble(oRetest[iRow].RETEST_YIELD.ToString().Trim());
                        in_mvou.retest_list[iRow].seq_num = iRow;
                    }

                    in_mvou.retest_cnt = oRetest.Length;
                    in_mvou._size_retest_list = oRetest.Length;
                }

                in_mvou._C.comment = strSample;

                in_mvou._C.bin_list = oInBinTbl;

                WIPCaster.WIP_End_Lot(in_mvou, ref out_mvou);
                #region LOG [OUTPUT VALUE]
                //////////////////////////////////////////////////////////////////////////////////////////
                sb.AppendFormat("{0,-5}", FACTORY);
                sb.AppendFormat("{0,-5}", "MVOU");
                sb.AppendFormat("{0,-1}", out_mvou.h_status_value);
                sb.AppendFormat("{0,-200}", out_mvou.h_msg);
                sb.AppendFormat("{0,-200}", out_mvou.h_db_err_msg);
                sb.AppendFormat("{0,-30}", out_mvou.h_msg_code);
                sb.AppendFormat("{0,-30}", out_mvou.h_field_msg);
                sb.AppendFormat("{0,-30}", out_mvou.h_warn_msg);
                if (out_mvou.h_status_value != '0')
                {
                    oLog.status = string.Format("{0}", out_mvou.h_status_value);
                    oLog.error_msg = out_mvou.h_field_msg;
                }
                //////////////////////////////////////////////////////////////////////////////////////////
                #endregion

                oProdTag.lot_id = oLog.lot_id;
                oProdTag.end_time = oLog.recv_time;
                oProdTag.device = oLog.mat_id;
                oProdTag.oper = oLog.oper;
                TimeSpan Inter_Time = DateTime.ParseExact(oLog.recv_time, "yyyyMMddHHmmss", null).Subtract(DateTime.ParseExact(oLog.send_time, "yyyyMMddHHmmss", null));
                oProdTag.tst_time = Inter_Time.TotalSeconds.ToString();
                
                oProdTag.in_qty = oLog.qty1;
                oProdTag.out_qty = string.Format("{0}", DACrux.Base.ConvertNumeric.ConvertInt(oLog.qty1) - in_mvou.unit1_qty_2);
                oProdTag.good_qty = string.Format("{0}", DACrux.Base.ConvertNumeric.ConvertInt(oLog.qty1) - in_mvou.unit1_qty_1 - in_mvou.unit1_qty_2);
                oProdTag.yield = string.Format("{0}", 100 - (in_mvou.unit1_qty_1 + in_mvou.unit1_qty_2) / DACrux.Base.ConvertNumeric.ConvertInt(oLog.qty1) * 100);
                oProdTag.hbin01 = oProdTag.good_qty;
                oProdTag.hbin02 = in_mvou.unit1_qty_1.ToString();
                oProdTag.hbin03 = in_mvou.unit1_qty_2.ToString();
                oProdTag.retest_cnt = oRetest.Length.ToString();

                //UpdateOeeProd(oProdTag);

                return sb.ToString();
            }
            catch (Exception ex)
            {
                sb.AppendFormat("{0,-5}", FACTORY);
                sb.AppendFormat("{0,-5}", "MVOU");
                sb.AppendFormat("{0,-1}", "1");
                sb.AppendFormat("{0,-200}", ex.Message);
                sb.AppendFormat("{0,-200}", " ");

                oLog.status = "1";
                oLog.error_msg = ex.Message;

                return sb.ToString();
            }
        }
    }
}
