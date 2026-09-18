/*----------------------------------------------------------------------------------------------------------
--  상기 프로그램에 대한 저작권을 포함한 지적재산권은 (주)미라콤아이앤씨에 있으며, (주)미라콤아이앤씨가
--  명시적으로 허용하지 않은 사용, 복사, 변경, 제3자에의 공개, 배포는 엄격히 금지되며, (주)미라콤아이앤씨의
--  지적재산권 침해에 해당됩니다.
--  (Copyright ⓒ 2014 Miracom Inc. All Rights Reserved | Confidential)
--
--  Program Id      : MenuHelper.cs
--  Creator         : YoungShin Lim (YSIM)
--  Create Date     : 2014.11.29
--  Description     : DACrux Framework  
--  History         : Created by YSIM at 2014.11.29
 * ********************************************************************************************************
 * 2014 년 DACrux V4 History Reset
 * 2015 년 DACrux V5 History Init
 * 2015.03.25 : YSIM
 *             - 다단계 SUB Menu 추가 가능하게 변경
 *             - 예)
 *                  File --> File Type -> A
 *                                     -> B
 *                                     -> C -> CA
 *                                          -> CB
 *                                          -> CC
----------------------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Drawing;
using System.Diagnostics;

namespace DACrux.Framework
{
    public static class MenuHelper
    {
        private const string ICO_FILE_NEW_PROJECT = "89 50 4E 47 0D 0A 1A 0A 00 00 00 0D 49 48 44 52 00 00 00 10 00 00 00 10 08 06 00 00 00 1F F3 FF 61 00 00 00 20 63 48 52 4D 00 00 7A 25 00 00 80 83 00 00 F9 FF 00 00 80 E9 00 00 75 30 00 00 EA 60 00 00 3A 98 00 00 17 6F 92 5F C5 46 00 00 00 09 70 48 59 73 00 00 0B 12 00 00 0B 12 01 D2 DD 7E FC 00 00 03 6F 49 44 41 54 38 4F 45 93 8D 4F 53 57 18 C6 FB 8F 2C DB 1F 30 13 92 09 66 66 A5 F1 03 75 CC 8C 8D 00 99 88 1A 17 35 BA 39 70 C3 F1 B1 8D 52 18 14 28 1F 45 C6 00 EB 44 C0 C1 C6 47 29 E2 10 28 F2 31 D8 A4 40 41 08 60 22 2C 84 DA 7B DB AA D3 80 14 EC 17 FC 76 7A 31 D9 CD 79 73 9F 73 CF 3D CF 79 9F E7 7D 8F CA 21 7B 78 2A 79 90 65 19 D9 E9 C2 E5 92 14 AC D5 EA 94 D0 69 73 D1 EA 72 90 5C 32 2E D9 29 D6 DC 02 3B 95 70 49 6E 54 61 D0 7B 23 83 A6 CC 83 34 67 44 73 3B 43 4D 56 5E 31 BD 7D 56 7A 7A 7A F9 A3 A7 9F FE 9E 7B 64 EB 0C 34 67 A9 69 CC DA 4F 53 76 0C BD A6 EF 04 81 1C 26 90 A9 CF 88 61 7D DE C2 DA EC 6F 78 E7 5A 29 29 AF 63 DC 36 8D 6D 62 4A BC ED 4C 4E 3C 44 5F 5E CB E6 6C 0B 1B B3 BF B3 31 D7 49 7D 7A F4 2E 81 EC 96 F8 25 5D 8D CF BD C8 50 47 0D ED 5D 77 A9 6D 34 53 D7 60 A6 B6 FE D7 DD B8 D5 44 CD CD 66 3A BA EE 30 D8 FE 13 5B D2 0C 75 97 F7 28 B2 54 61 DD 37 D3 A2 59 5B 79 40 7B B7 15 76 C2 23 48 F8 11 F0 CD 3C 0C B6 95 6F 16 CB 1D 5E AD FC 8D 29 ED 83 37 04 C2 B0 EB 69 6A DC 8F FA C4 09 FD CA 8F AB 0E 27 4F 24 27 0D 0D 0D 94 18 2A D0 17 95 52 54 6C C0 50 5A 4E 65 D5 35 3C 8B BD 98 AE 44 0B D3 25 91 81 70 D5 94 AA 41 9A B4 D0 DA 75 8F 9D 9D 1D 3C 9E 67 62 43 19 F7 47 C6 08 06 36 09 6E 07 08 85 42 04 7C 7E 06 C7 FE A4 AA A2 5C 1C AA C1 2D BB C2 04 32 B5 97 35 4C 8C 5A 69 69 33 63 BC 56 C9 0D D3 2D 86 87 87 F1 85 82 CC 48 EB 4C AE BE C4 B2 E0 66 DA ED C5 E7 DB 62 68 78 0C 83 2E E3 FF 2A 58 AD 7D 74 76 76 62 AA 6F E4 E2 17 97 C8 C9 F9 9E C1 C1 01 0A 7F 2C 60 61 F1 31 C3 33 73 F4 2C AF 61 75 BE 66 6C D9 CD 56 70 13 BD BE 18 87 2C 24 4C D9 67 A8 AE 2C 27 29 29 89 33 67 4F 93 98 98 48 CA A9 33 24 27 27 93 FE F5 55 26 1E AD 30 EA 7C C5 88 E4 65 FC B9 9F 0E FB 63 82 A1 2D C5 17 B7 EB C9 AE 84 B2 AB 29 9C 38 71 92 C4 84 78 D2 BE BA C2 A9 94 64 E2 E3 3F 21 BF AC 82 BA BB 7F D1 34 FA 90 A9 17 AF 99 F7 06 E9 9A 5E 54 3C D1 17 95 29 9D A9 FA 26 3D 95 F8 4F E3 38 7C E8 00 17 CE 5D E2 F9 8B 7F A9 31 5D 47 A3 51 13 FB 71 1C E7 3E 3F CF EC 7A 80 65 AF 9F 85 35 2F F6 A9 7E 7C FE 0D 21 41 8F EC 72 A0 6A 6E BE CD 0F A9 67 39 78 F8 10 B1 C7 3F A2 BA BA 8A F3 17 2E A2 7E 7F 3F 05 FA 3C 6A 84 2F DD B6 79 BA 1F CC 61 1B EF 66 7B D3 83 75 60 10 63 61 B6 72 2F 54 E1 34 4A B5 5F B2 2F 32 8A C8 7D 51 44 BC BB 87 C8 C8 48 22 22 22 C8 CA FC 96 A7 2F 9F 11 08 F9 09 6C 6F E1 F5 6F 62 BD 3F C0 CF A5 B9 6C 9B DF A1 23 F7 08 AA 7F 96 57 39 9D 94 C0 91 98 03 9C 4C 88 15 9B DF E3 F8 31 0D 29 9F C5 B1 77 6F 14 9D AD 2D 14 E9 CB 84 E6 12 0A F3 4B A8 2A C8 64 C7 FC 16 98 DF A6 4D 27 08 9C A2 14 E1 4A D8 6C 36 EC F6 49 8E 1E FD 10 A3 D1 C8 D2 D2 12 43 43 23 A2 23 1D 4A CB 4A 92 4B B9 E6 92 48 BB 3D 2F 86 B6 DC 63 E4 17 6B F9 0F 5C 44 06 C8 F5 A5 AF E1 00 00 00 00 49 45 4E 44 AE 42 60 82";

        public static void InitMenu(
            ref Infragistics.Win.UltraWinToolbars.UltraToolbarsManager TargetToolBarManager,
            DataTable dtMenu
            )
        {
            int iMnuOrder = 0;
            Infragistics.Win.UltraWinToolbars.PopupMenuTool pupMenu = null;
            string strCaption = string.Format("CAPTION00{0}", DACrux.Base.GlobalVariable.LanguageNumber + 1);

            DataRow[] drs = dtMenu.Select("POPUP_MENU = 'ROOT'", "MNU_ORDER");
            DataRow[] drs_sub = dtMenu.Select("POPUP_MENU <> 'ROOT'", "POPUP_MENU,MNU_ORDER");
            // Added By james Kwon 10/16 for order of toolbar
            DataRow[] drs_tool = dtMenu.Select("POPUP_MENU <> 'ROOT'", "RESV_04, MNU_ORDER");

            //--

            foreach (DataRow dr in drs)
            {
                iMnuOrder = DACrux.Base.Convert.intParse(dr["MNU_ORDER"].ToString());

                if (dr["MENU_KEY"].ToString().Length > 0)
                {
                    int nSearchTool = TargetToolBarManager.Tools.IndexOf(dr["MENU_KEY"].ToString());

                    if (nSearchTool < 0)
                    {
                        pupMenu = new Infragistics.Win.UltraWinToolbars.PopupMenuTool(dr["MENU_KEY"].ToString());
                        pupMenu.SharedProps.Caption = dr[strCaption].ToString();

                        if (!string.IsNullOrWhiteSpace(dr["ICON16"].ToString()))
                            pupMenu.SharedProps.AppearancesSmall.Appearance.Image = DACrux.Base.Convert.StringToImage(dr["ICON16"].ToString());
                        if (!string.IsNullOrWhiteSpace(dr["ICON32"].ToString()))
                            pupMenu.SharedProps.AppearancesLarge.Appearance.Image = DACrux.Base.Convert.StringToImage(dr["ICON32"].ToString());
                        pupMenu.SharedProps.Enabled = bool.Parse(dr["ENABLE"].ToString());
                        pupMenu.SharedProps.Visible = bool.Parse(dr["VISIBLE"].ToString());

                        TargetToolBarManager.Tools.Add(pupMenu);
                        TargetToolBarManager.Toolbars["UTB_MAINMENU"].Tools.InsertTool(iMnuOrder, dr["MENU_KEY"].ToString());
                    }
                    else
                    {
                        TargetToolBarManager.Tools[nSearchTool].SharedProps.Caption = dr[strCaption].ToString();
                        if (!string.IsNullOrWhiteSpace(dr["ICON16"].ToString()))
                            TargetToolBarManager.Tools[nSearchTool].SharedProps.AppearancesSmall.Appearance.Image = DACrux.Base.Convert.StringToImage(dr["ICON16"].ToString());
                        if (!string.IsNullOrWhiteSpace(dr["ICON32"].ToString()))
                            TargetToolBarManager.Tools[nSearchTool].SharedProps.AppearancesLarge.Appearance.Image = DACrux.Base.Convert.StringToImage(dr["ICON32"].ToString());
                        TargetToolBarManager.Tools[nSearchTool].SharedProps.Enabled = bool.Parse(dr["ENABLE"].ToString());
                        TargetToolBarManager.Tools[nSearchTool].SharedProps.Visible = bool.Parse(dr["VISIBLE"].ToString());
                    }
                }
            }

            //--

            foreach (DataRow dr in drs_sub)
            {
#if DEBUG
                Debug.WriteLine(String.Format("{0}, {1}, {2}", dr["MNU_ORDER"], dr["POPUP_MENU"], dr["MENU_KEY"]));
#endif
                iMnuOrder = DACrux.Base.Convert.intParse(dr["MNU_ORDER"].ToString());

                int nSearchTool = TargetToolBarManager.Tools.IndexOf(dr["MENU_KEY"].ToString());
                if (nSearchTool > -1) continue;

                // 자기가 POPUP인지 확인하는 Logic
                /////////////////////////////////////////////////////////////////
                DataRow[] drsTmp = dtMenu.Select(string.Format("POPUP_MENU = '{0}'", dr["MENU_KEY"].ToString()));

                if (drsTmp != null && drsTmp.Length > 0)
                {
                    // 자기자신 역시 Popup 이면...
                    pupMenu = new Infragistics.Win.UltraWinToolbars.PopupMenuTool(dr["MENU_KEY"].ToString());
                    pupMenu.SharedProps.Caption = dr[strCaption].ToString();

                    if (!string.IsNullOrWhiteSpace(dr["ICON16"].ToString()))
                        pupMenu.SharedProps.AppearancesSmall.Appearance.Image = DACrux.Base.Convert.StringToImage(dr["ICON16"].ToString());
                    if (!string.IsNullOrWhiteSpace(dr["ICON32"].ToString()))
                        pupMenu.SharedProps.AppearancesLarge.Appearance.Image = DACrux.Base.Convert.StringToImage(dr["ICON32"].ToString());

                    pupMenu.SharedProps.Enabled = bool.Parse(dr["ENABLE"].ToString());
                    pupMenu.SharedProps.Visible = bool.Parse(dr["VISIBLE"].ToString());

                    TargetToolBarManager.Tools.Add(pupMenu);

                    int iParentToolIdx = TargetToolBarManager.Tools.IndexOf(dr["POPUP_MENU"].ToString());
                    Infragistics.Win.UltraWinToolbars.PopupMenuTool pupParent = null;
                    pupParent = (Infragistics.Win.UltraWinToolbars.PopupMenuTool)TargetToolBarManager.Tools[iParentToolIdx];

                    /// 1. Menu를 Insert하려고 할때 내부 갯수보다 큰값으로 Insert를 하면 Error 발생
                    /// 2. 기본 Menu가 있으므로 Add가 아닌 Insert로 Tool을 추가 해야 함
                    //////////////////////////////////////////////////////////////////////////////////
                    if ((pupParent.Tools.Count + 1) < iMnuOrder) iMnuOrder = pupParent.Tools.Count;

                    pupParent.Tools.InsertTool(iMnuOrder, dr["MENU_KEY"].ToString());
                }
                else
                {
                    // 자기자신이 Popup이 아니면
                    Infragistics.Win.UltraWinToolbars.ButtonTool butMenu = new Infragistics.Win.UltraWinToolbars.ButtonTool(dr["MENU_KEY"].ToString());
                    butMenu.SharedProps.Caption = dr[strCaption].ToString();
                    switch (dr["ICON32"].ToString())
                    {
                        case "ICO_FILE_NEW_PROJECT":
                            butMenu.CustomizedImage = DACrux.Base.Convert.StringToImage(ICO_FILE_NEW_PROJECT);
                            break;
                        default:
                            if (!string.IsNullOrWhiteSpace(dr["ICON16"].ToString()))
                                butMenu.SharedProps.AppearancesSmall.Appearance.Image = DACrux.Base.Convert.StringToImage(dr["ICON16"].ToString());
                            if (!string.IsNullOrWhiteSpace(dr["ICON32"].ToString()))
                                butMenu.SharedProps.AppearancesLarge.Appearance.Image = DACrux.Base.Convert.StringToImage(dr["ICON32"].ToString());
                            break;
                    }

                    butMenu.SharedProps.Enabled = bool.Parse(dr["ENABLE"].ToString());
                    butMenu.SharedProps.Visible = bool.Parse(dr["VISIBLE"].ToString());

                    TargetToolBarManager.Tools.Add(butMenu);

                    int iParentToolIdx = TargetToolBarManager.Tools.IndexOf(dr["POPUP_MENU"].ToString());
                    pupMenu = (Infragistics.Win.UltraWinToolbars.PopupMenuTool)TargetToolBarManager.Tools[iParentToolIdx];

                    /// 1. Menu를 Insert하려고 할때 내부 갯수보다 큰값으로 Insert를 하면 Error 발생
                    /// 2. 기본 Menu가 있으므로 Add가 아닌 Insert로 Tool을 추가 해야 함
                    //////////////////////////////////////////////////////////////////////////////////
                    /*if ((pupMenu.Tools.Count + 1) < iMnuOrder)*/
                    iMnuOrder = pupMenu.Tools.Count;
                    pupMenu.Tools.InsertTool(iMnuOrder, dr["MENU_KEY"].ToString());
                    //pupMenu.Tools.AddTool( dr["MENU_KEY"].ToString());
                    if (dr["IS_START"].ToString() == "True")
                        pupMenu.Tools[dr["MENU_KEY"].ToString()].InstanceProps.IsFirstInGroup = true;
                }
            }

            // Inserted By James Kwon 10/16
            foreach (DataRow dr in drs_tool)
            {
                if (dr["STOCK_TOOLBAR"].ToString().Trim().Length > 0)
                {
                    if (TargetToolBarManager.Toolbars.IndexOf(dr["STOCK_TOOLBAR"].ToString()) < 0)
                    {
                        TargetToolBarManager.Toolbars.AddToolbar(dr["STOCK_TOOLBAR"].ToString());
                    }

                    TargetToolBarManager.Toolbars[dr["STOCK_TOOLBAR"].ToString()].Tools.AddTool(dr["MENU_KEY"].ToString());
                    TargetToolBarManager.Toolbars[dr["STOCK_TOOLBAR"].ToString()].DockedRow = 0;

                    // DB하이텍의 Defect 링크 기능 때문에 STOCK_TOOLBAR = DEFECT_LINK 인 경우 VISIBLE=FLASE 인 경우에도 보일 수 있도록 한다. 2019.06.21 Taihi,Kim.
                    if (dr["STOCK_TOOLBAR"].ToString() == DACruxMain.DEFECT_LINK_ITEM)
                    {
                        TargetToolBarManager.Toolbars[dr["STOCK_TOOLBAR"].ToString()].Tools[dr["MENU_KEY"].ToString()].SharedProps.Visible = true;
                    }
                }
            }

            TargetToolBarManager.UseLargeImagesOnMenu = true;
            TargetToolBarManager.UseLargeImagesOnToolbar = true;
        }

        #region Original
        //public static void InitMenu_ORG(ref Infragistics.Win.UltraWinToolbars.UltraToolbarsManager TargetToolBarManager, DataTable dtMenu)
        //{
        //    int iMnuOrder = 0;
        //    string strCaption = string.Format("CAPTION00{0}", DACrux.Base.GlobalVariable.LanguageNumber + 1);
        //    try
        //    {
        //        DataRow[] drs = dtMenu.Select("POPUP_MENU = 'ROOT'", "MNU_ORDER");
        //        DataRow[] drs_sub = dtMenu.Select("POPUP_MENU <> 'ROOT'", "MNU_ORDER, MENU_KEY");
        //        // Added By james Kwon 10/16 for order of toolbar
        //        DataRow[] drs_tool = dtMenu.Select("POPUP_MENU <> 'ROOT'", "RESV_04, MNU_ORDER, MENU_KEY");
        //        foreach (DataRow dr in drs)
        //        {
        //            try
        //            {
        //                iMnuOrder = DACrux.Base.Convert.intParse(dr["MNU_ORDER"].ToString());

        //                if (dr["MENU_KEY"].ToString().Length > 0)
        //                {
        //                    int nSearchTool = TargetToolBarManager.Tools.IndexOf(dr["MENU_KEY"].ToString());

        //                    if (nSearchTool < 0)
        //                    {
        //                        Infragistics.Win.UltraWinToolbars.PopupMenuTool pupMenu = null;

        //                        pupMenu = new Infragistics.Win.UltraWinToolbars.PopupMenuTool(dr["MENU_KEY"].ToString());
        //                        //pupMenu.CustomizedCaption = dr[strCaption].ToString();
        //                        pupMenu.SharedProps.Caption = dr[strCaption].ToString();

        //                        //pupMenu.CustomizedImage = DACrux.Base.Convert.StringToImage(dr["ICON32"].ToString());
        //                        if (!string.IsNullOrWhiteSpace(dr["ICON16"].ToString()))
        //                            pupMenu.SharedProps.AppearancesSmall.Appearance.Image = DACrux.Base.Convert.StringToImage(dr["ICON16"].ToString());
        //                        if (!string.IsNullOrWhiteSpace(dr["ICON32"].ToString()))
        //                            pupMenu.SharedProps.AppearancesLarge.Appearance.Image = DACrux.Base.Convert.StringToImage(dr["ICON32"].ToString());
        //                        pupMenu.SharedProps.Enabled = bool.Parse(dr["ENABLE"].ToString());
        //                        pupMenu.SharedProps.Visible = bool.Parse(dr["VISIBLE"].ToString());

        //                        //if (pupMenu.SharedProps.Visible)
        //                        //{
        //                        TargetToolBarManager.Tools.Add(pupMenu);
        //                        TargetToolBarManager.Toolbars["UTB_MAINMENU"].Tools.InsertTool(iMnuOrder, dr["MENU_KEY"].ToString());
        //                        //}
        //                    }
        //                    else
        //                    {
        //                        TargetToolBarManager.Tools[nSearchTool].SharedProps.Caption = dr[strCaption].ToString();
        //                        if (!string.IsNullOrWhiteSpace(dr["ICON16"].ToString()))
        //                            TargetToolBarManager.Tools[nSearchTool].SharedProps.AppearancesSmall.Appearance.Image = DACrux.Base.Convert.StringToImage(dr["ICON16"].ToString());
        //                        if (!string.IsNullOrWhiteSpace(dr["ICON32"].ToString()))
        //                            TargetToolBarManager.Tools[nSearchTool].SharedProps.AppearancesLarge.Appearance.Image = DACrux.Base.Convert.StringToImage(dr["ICON32"].ToString());
        //                        TargetToolBarManager.Tools[nSearchTool].SharedProps.Enabled = bool.Parse(dr["ENABLE"].ToString());
        //                        TargetToolBarManager.Tools[nSearchTool].SharedProps.Visible = bool.Parse(dr["VISIBLE"].ToString());
        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                throw ex;
        //            }
        //        }

        //        foreach (DataRow dr in drs_sub)
        //        {
        //            iMnuOrder = DACrux.Base.Convert.intParse(dr["MNU_ORDER"].ToString());

        //            if (dr["MENU_KEY"].ToString().Length > 0)
        //            {
        //                int nSearchTool = TargetToolBarManager.Tools.IndexOf(dr["MENU_KEY"].ToString());
        //                if (dr["MENU_KEY"].ToString().StartsWith("MNU_FILE_TEST"))
        //                {
        //                    int a = 1;
        //                    a = a + 1;
        //                }
        //                if (nSearchTool < 0)
        //                {
        //                    Infragistics.Win.UltraWinToolbars.PopupMenuTool pupMenu = null;

        //                    Infragistics.Win.UltraWinToolbars.ButtonTool butMenu = new Infragistics.Win.UltraWinToolbars.ButtonTool(dr["MENU_KEY"].ToString());
        //                    //butMenu.CustomizedCaption = dr[strCaption].ToString();
        //                    butMenu.SharedProps.Caption = dr[strCaption].ToString();

        //                    switch (dr["ICON32"].ToString())
        //                    {
        //                        case "ICO_FILE_NEW_PROJECT":
        //                            butMenu.CustomizedImage = DACrux.Base.Convert.StringToImage(ICO_FILE_NEW_PROJECT);
        //                            break;
        //                        default:
        //                            //if (dr["ICON32"].ToString().Length != 0)
        //                            //    butMenu.CustomizedImage = DACrux.Base.Convert.StringToImage(dr["ICON32"].ToString());
        //                            if (!string.IsNullOrWhiteSpace(dr["ICON16"].ToString()))
        //                                butMenu.SharedProps.AppearancesSmall.Appearance.Image = DACrux.Base.Convert.StringToImage(dr["ICON16"].ToString());
        //                            if (!string.IsNullOrWhiteSpace(dr["ICON32"].ToString()))
        //                                butMenu.SharedProps.AppearancesLarge.Appearance.Image = DACrux.Base.Convert.StringToImage(dr["ICON32"].ToString());
        //                            break;
        //                    }

        //                    butMenu.SharedProps.Enabled = bool.Parse(dr["ENABLE"].ToString());
        //                    butMenu.SharedProps.Visible = bool.Parse(dr["VISIBLE"].ToString());

        //                    //if (butMenu.SharedProps.Visible)
        //                    //{
        //                    TargetToolBarManager.Tools.Add(butMenu);

        //                    /* Deleted By James Kwon
        //                    if (dr["STOCK_TOOLBAR"].ToString().Trim().Length > 0)
        //                    {
        //                        if (TargetToolBarManager.Toolbars.IndexOf(dr["STOCK_TOOLBAR"].ToString()) < 0)
        //                        {
        //                            TargetToolBarManager.Toolbars.AddToolbar(dr["STOCK_TOOLBAR"].ToString());
        //                        }

        //                        TargetToolBarManager.Toolbars[dr["STOCK_TOOLBAR"].ToString()].Tools.AddTool(dr["MENU_KEY"].ToString());
        //                        TargetToolBarManager.Toolbars[dr["STOCK_TOOLBAR"].ToString()].DockedRow = 0;
        //                    }
        //                    */



        //                    pupMenu = (Infragistics.Win.UltraWinToolbars.PopupMenuTool)TargetToolBarManager.Toolbars["UTB_MAINMENU"].Tools[dr["POPUP_MENU"].ToString()];
        //                    pupMenu.Tools.InsertTool(iMnuOrder, dr["MENU_KEY"].ToString());
        //                    if (dr["RESV_02"].ToString() == "Y")
        //                        pupMenu.Tools[dr["MENU_KEY"].ToString()].InstanceProps.IsFirstInGroup = true;

        //                    //}
        //                }
        //                else
        //                {
        //                    TargetToolBarManager.Tools[nSearchTool].SharedProps.Caption = dr[strCaption].ToString();
        //                    if (!string.IsNullOrWhiteSpace(dr["ICON16"].ToString()))
        //                        TargetToolBarManager.Tools[nSearchTool].SharedProps.AppearancesSmall.Appearance.Image = DACrux.Base.Convert.StringToImage(dr["ICON16"].ToString());
        //                    if (!string.IsNullOrWhiteSpace(dr["ICON32"].ToString()))
        //                        TargetToolBarManager.Tools[nSearchTool].SharedProps.AppearancesLarge.Appearance.Image = DACrux.Base.Convert.StringToImage(dr["ICON32"].ToString());
        //                    TargetToolBarManager.Tools[nSearchTool].SharedProps.Enabled = bool.Parse(dr["ENABLE"].ToString());
        //                    TargetToolBarManager.Tools[nSearchTool].SharedProps.Visible = bool.Parse(dr["VISIBLE"].ToString());

        //                    if (dr["RESV_02"].ToString() == "Y")
        //                        ((Infragistics.Win.UltraWinToolbars.PopupMenuTool)TargetToolBarManager.Toolbars["UTB_MAINMENU"].Tools[dr["POPUP_MENU"].ToString()]).Tools[dr["MENU_KEY"].ToString()].InstanceProps.IsFirstInGroup = true;
        //                }
        //            }
        //        }

        //        // Inserted By James Kwon 10/16
        //        foreach (DataRow dr in drs_tool)
        //        {
        //            if (dr["STOCK_TOOLBAR"].ToString().Trim().Length > 0)
        //            {
        //                if (TargetToolBarManager.Toolbars.IndexOf(dr["STOCK_TOOLBAR"].ToString()) < 0)
        //                {
        //                    TargetToolBarManager.Toolbars.AddToolbar(dr["STOCK_TOOLBAR"].ToString());
        //                }

        //                TargetToolBarManager.Toolbars[dr["STOCK_TOOLBAR"].ToString()].Tools.AddTool(dr["MENU_KEY"].ToString());
        //                TargetToolBarManager.Toolbars[dr["STOCK_TOOLBAR"].ToString()].DockedRow = 0;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        #endregion

    }
}
