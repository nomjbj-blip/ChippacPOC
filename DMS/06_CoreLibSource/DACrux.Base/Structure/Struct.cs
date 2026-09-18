using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.Globalization;

namespace DACrux.Base
{
    public delegate void ExtendsFunction(object sender, string FunctionID, TPWafer[] DataKeys);
    public delegate void ExtFnc(object sender, string newForm, object val, string dataType);
    public delegate void DPExtendsFunction(object sender, string FunctionID, DPWafer[] DataKeys);

    [Serializable]
    public struct TPWafer
    {
        public string Testarea;
        public string Product;
        public string Program;
        public string LotID;
        public string WaferID;
        public string WaferSeq;
        public string LotSeq;
        public string BIN;
    }

    [Serializable]
    public struct TPLot
    {
        public string Product;
        public string Program;
        public string LotID;
        public string LotSeq;
        public string BIN;
    }

    [Serializable]
    public struct DPWafer
    {
        public string Product;
        public string Program;
        public string LotID;
        public string WaferID;
        public string StepSeq;
        public string LotSeq;
    }

    [Serializable]
    public struct USEMAP_TAG
    {
        public string MAPID;
        public int X;
        public int Y;
        public int SHOT_ID;
        public int SHOT_X;
        public int SHOT_Y;
        public int DIE_SHOT_X;
        public int DIE_SHOT_Y;
        public int SITE_ID;
        public int SITE_X;
        public int SITE_Y;
        public int USECODE;
    }

    [Serializable]
    public struct MAPDEF_TAG
    {
        public string MAPID;
        public double WAFER_SIZE;
        public double CHIP_SIZE_X;
        public double CHIP_SIZE_Y;
        public double ORIGIN_MICRO_X;
        public double ORIGIN_MICRO_Y;
        public int ORIGIN_INDEX_X;
        public int ORIGIN_INDEX_Y;
        public double FIRST_MICRO_X;
        public double FIRST_MICRO_Y;
        public int FIRST_INDEX_X;
        public int FIRST_INDEX_Y;
        public double EDGE_SIZE;
        public int ANGLE;
        public int NETDIE;
        public int NOTCH_TYPE;
        public int ST_START;
        public int ST_INTYPE;
        public int ST_XCNT;
        public int ST_YCNT;
        public int ST_START_X;
        public int ST_START_Y;
        public int DIE_INDEX_MIN_X;
        public int DIE_INDEX_MAX_X;
        public int DIE_INDEX_MIN_Y;
        public int DIE_INDEX_MAX_Y;
        public int XY_DIRECTION;
        public int REFERENCEDIE_SETTING;
        public int GROSS;
        public USEMAP_TAG[] USEMAP;
    }

    [Serializable]
    public struct CUSTOMER_TAG
    {
        public string CUSTOMER_ID;
        public string CUSTOMER_NAME;
        public string CUSTOMER_DESC;
        public int DATA_SERVICE;
        public int USER_COUNT;
        public DateTime EXPIRE_DATE;
        public string FTPSITE;
        public string FTPUSER;
        public string FTPPASSWORD;
        public string FTPPATH;
        public string SENDTIME;
        public string AVIFORMAT;
        public string EDSFORMAT;
        public string INKLESSFORMAT;
        public string RAWDATA;
    }

    [Serializable]
    public struct SECUSR_TAG
    {
        public string FACTORY;
        public string USER_NAME;
        public string USER_GROUP;
        public string PASSWORD;
        public string USER_DESC;
        public string DEPARTMENT;
        public string PHONE_NUMBER;
        public string DEP_COMPUTE_ID;
        public string BAY_ID;
        public string EMAIL_ID;
        public string BIRTHDAY;
        public string PAGER_NO;
        public char ADMIN_FLAG;
        public char USER_LEVEL;
        public string RELATED_OPER1;
        public string RELATED_OPER2;
        public string RELATED_OPER3;
        public string RELATED_OPER4;
        public string RELATED_OPER5;
        public string BOSS_NAME1;
        public string BOSS_NAME2;
        public string BOSS_NAME3;
        public string BOSS_NAME4;
        public string BOSS_NAME5;
        public string JOIN_DATE;
        public char RETIRE_FLAG;
        public string RETIRE_DATE;
        public string POSITION;
        public char SEX;
        public string USER_CMF1;
        public string USER_CMF2;
        public string USER_CMF3;
        public string USER_CMF4;
        public string USER_CMF5;
        public string USER_CMF6;
        public string USER_CMF7;
        public string USER_CMF8;
        public string USER_CMF9;
        public string USER_CMF10;
        public string RESV_FIELD1;
        public string RESV_FIELD2;
        public string RESV_FIELD3;
        public string RESV_FIELD4;
        public string RESV_FIELD5;
    }

    [Serializable]
    public struct SECGRP_TAG
    {
        public string USER_GROUP;
        public string FUNCTION_LIST;
        public string GROUP_DESC;
    }

    [Serializable]
    public struct BINDESC_TAG
    {
        public string PROGRAM;
        public string PRODUCT;
        public string TESTAREA;
        public int BIN;
        public int BIN_GROUP;
        public string BIN_NAME;
        public string CHAR_BIN;
        public char HIGH_GEC;
        public char DISPLAY;
        public int UPPER_LIMIT_CNT;
        public int LOWER_LIMIT_CNT;
        public string COLOR;
        public string DESCRIPTION;
        public int DataStatus;
    }

    [Serializable]
    public struct PARASPEC_TAG
    {
        public string PROGRAM;
        public string TESTAREA;
        public string PRODUCT;
        public int ITEM_NUMBER;
        public string ITEM;
        public string DESCRIPTION;
        public double LOWER_LIMIT;
        public double UPPER_LIMIT;
        public double DIS_LOWER;
        public double DIS_UPPER;
        public string UNIT;
        public int DIRECTION;
        public string COMMENTS;
        public int DataStatus;
    }

    [Serializable]
    public struct PROGRAM_TAG
    {
        public string PROGRAM;
        public string VERSION;
        public string PRODUCT;
        public string TESTAREA;
        public DateTime CREATEDATE;
        public DateTime LASTMODIFY;
        public double TARGET_YIELD;
        public char USE_FLAG;
        public int SITE_CNT;
        public string CUSTOMER;
        public BINDESC_TAG[] BINDESC;
        public PARASPEC_TAG[] PARASPEC;
    }

    [Serializable]
    public struct PRODUCT_TAG
    {
        public string FACILITY;
        public string PRODUCT;
        public string CUSTOMER_ID;
        public string CUSTOMER_NAME;
        public string CUSTPROD;
        public string MAPID;
    }

    [Serializable]
    public struct WAFER_TAG
    {
        public long WAFER_SEQ;
        public long LOT_SEQ;
        public string WAFER_ID;
        public string TESTER;
        public string PROBE_CARD;
        public string OPERATOR;
        public int PROBE_CNT;
        public int TESTED_DIE;
        public DateTime START_TIME;
        public DateTime END_TIME;
        public int WAFER_CAT;
        public int LOSS_DIE;
        public int ISP_INITEM;
        public int VISUAL_ITEM;
        public int ISP_OUTITEM;
        public string ISP_INCMT;
        public string VISUAL_CMT;
        public string ISP_OUTCMT;
        public char FVI_FLAG;
    }

    [Serializable]
    public struct VISUALINSPSPEC_TAG
    {
        public string INSPTYPE;
        public int BIN;
        public string BIN_NAME;
        public string COLOR;
        public DateTime CREATE_DATE;
        public DateTime EDIT_DATE;
        public string DESCRIPTION;
        public string KEYMAP;
    }

    [Serializable]
    public struct AREAGROUP_TAG
    {
        public string AREAGROUP;
        public string TESTAREA;
        public string PRESTR;
        public string POSTSTR;
    }

    #region [QMS addtion 12/02/15]

    [Serializable]
    public struct LOSS_BIN_TAG
    {
        public string LOSS_CODE;
        public int LOSS_COUNT;
    }

    [Serializable]
    public struct RETEST_IN_TAG
    {
        public int SEQ;
        public double RETEST_IN;
        public double RETEST_OUT;
        public double RETEST_REJECT;
        public double RETEST_YIELD;
    }

    [Serializable]
    public struct PROD_TAG
    {
        public string tst_seq;
        public string lot_id;
        public string start_time;
        public string end_time;
        public string device;
        public string oper_group;
        public string oper;
        public string tester;
        public string handler;
        public string dutboard_id;
        public string program;
        public string pgm_seq;
        public string shift;
        public string tst_time;
        public string std_time;
        public string index_time;
        public string dt_qa;
        public string dt_pe;
        public string dt_pm;
        public string dt_cal;
        public string dt_down;
        public string dt_temp;
        public string ls_conv;
        public string ls_retest;
        public string ls_multiop;
        public string ls_hangup;
        public string ls_single;
        public string ls_sysdown;
        public string ls_jam;
        public string in_qty;
        public string out_qty;
        public string good_qty;
        public string goal_qty;
        public string tot_oee;
        public string act_ort;
        public string act_oee;
        public string user;
        public string yield;
        public string hbin01;
        public string hbin02;
        public string hbin03;
        public string hbin04;
        public string hbin05;
        public string hbin06;
        public string hbin07;
        public string hbin08;
        public string hbin09;
        public string hbin10;
        public string retest_cnt;

        public void init()
        {
            tst_seq = "";
            lot_id = "";
            start_time = "11111111111111";
            end_time = "11111111111111";
            device = "";
            oper_group = "";
            oper = "";
            tester = "";
            handler = "";
            dutboard_id = "";
            program = "";
            pgm_seq = "";
            shift = "";
            tst_time = "";
            std_time = "";
            index_time = "";
            dt_qa = "";
            dt_pe = "";
            dt_pm = "";
            dt_cal = "";
            dt_down = "";
            dt_temp = "";
            ls_conv = "";
            ls_retest = "";
            ls_multiop = "";
            ls_hangup = "";
            ls_single = "";
            ls_sysdown = "";
            ls_jam = "";
            in_qty = "";
            out_qty = "";
            good_qty = "";
            goal_qty = "";
            tot_oee = "";
            act_ort = "";
            act_oee = "";
            user = "";
            yield = "";
            hbin01 = "";
            hbin02 = "";
            hbin03 = "";
            hbin04 = "";
            hbin05 = "";
            hbin06 = "";
            hbin07 = "";
            hbin08 = "";
            hbin09 = "";
            hbin10 = "";
            retest_cnt = "";
        }
    }

    [Serializable]
    public struct UPDATESRV_TAG
    {
        #region Members

        public string EQUIP_ID;
        public string FACILITY;
        public string EQUIP_TYPE;
        public string EQUIP_MODEL;
        public string EQUIP_DESC;
        public string EQUIP_IP;
        public string ACTIVE_FLAG;
        public string SVC_FLAG;
        public string SVC_FLAG2;
        public string RESULT_PATH;
        public string SERVICE_PATH;
        public string SERVICE_PATH2;
        public string BACKUP_PATH;
        public string EQUIP_PATH;
        public string IMAGE_PATH;
        public string VIRTUAL_PATH;
        public string FTP_ID;
        public string FTP_PASSWORD;
        public string PORT_NO;
        public string PROTOCOL;
        public string METHOD;
        public string MAIN_EQ;
        public string OPER;

        #endregion

        #region Reset

        public void Reset()
        {
            EQUIP_ID = string.Empty;
            FACILITY = string.Empty;
            EQUIP_TYPE = string.Empty;
            EQUIP_MODEL = string.Empty;
            EQUIP_DESC = string.Empty;
            EQUIP_IP = string.Empty;
            ACTIVE_FLAG = string.Empty;
            SVC_FLAG = string.Empty;
            SVC_FLAG2 = string.Empty;
            RESULT_PATH = string.Empty;
            SERVICE_PATH = string.Empty;
            SERVICE_PATH2 = string.Empty;
            BACKUP_PATH = string.Empty;
            EQUIP_PATH = string.Empty;
            IMAGE_PATH = string.Empty;
            VIRTUAL_PATH = string.Empty;
            FTP_ID = string.Empty;
            FTP_PASSWORD = string.Empty;
            PORT_NO = string.Empty;
            PROTOCOL = string.Empty;
            METHOD = string.Empty;
            MAIN_EQ = string.Empty;
            OPER = string.Empty;
        }

        #endregion
    }

    [Serializable]
    public struct LOT_TAG
    {
        public string prod_seq;
        public string lot_id;
        public string lot_mode;
        public string comments;
    }

    [Serializable]
    public struct USERINFO_TAG
    {
        public string userid;
        public string username;
        public string password;
        public int grade;
        public string grp;
        public string team;
        public string part;
        public string emailaddress;
        public string phone;
    }

    [Serializable]
    public struct USERGROUP_TAG
    {
        public string factory;
        public string usergroup;
        public string grade;
        public string groupdesc;
        public string function_name;
        public string actionflag;
        public string resvfield1;
        public string resvfield2;
        public string resvfield3;
        public string resvfield4;
        public string resvfield5;
    }

    [Serializable]
    public struct DEFECTCODE_TAG
    {
        public string ClassNumber;
        public string Name;
        public string Shape;
        public string Description;
        public string GroupID;
        public string GroupName;
    }

    [Serializable]
    public struct UPDATELOG_TAG
    {
        public string factory;
        public string equipid;
        public string starttime;
        public string endtime;
        public string servicetime;
        public string trtype;
        public string successflag;
        public string stepseq;
        public string resultfile;
        public string errorfile;
        public string backupfile;
        public string defects;
        public string images;
    }

    [Serializable]
    public struct FPDLot
    {
        public string product;
        public string program;
        public string lotid;
        public string lotseq;
        public string bin;
    }

    [Serializable]
    public struct FPDCell
    {
        public string product;
        public string program;
        public string lotid;
        public string cellid;
        public string cellseq;
        public string stepseq;
        public string lotseq;
    }

    [Serializable]
    public struct FPDMAPDEF_TAG
    {
        public string mapid;
        public double glass_size_x;
        public double glass_size_y;
        public double cell_size_x;
        public double cell_size_y;
        public double origin_micro_x;
        public double origin_micro_y;
        public int resolution_x;
        public int resolution_y;
        public int cell_grid_x;
        public int cell_grid_y;
        public Cell[] cellolayouts;
    }


    [Serializable]
    public struct SPCDATA_TAG
    {
        public long data_seq;
        public long lot_seq;
        public long cell_seq;
        public DateTime resulttimestamp;
        public DateTime filetimestamp;
        public string product;
        public string lot_id;
        public string lot_mode;
        public string process;
        public string oper;
        public string equip_id;
        public string main_equip_id;
        public int measure_cnt;
        public string user_id;
        public int val_count;
        public string line;
        public string facility;
        public string msg_id;
        public string itc_id;
        public string dct_id;
        public string step;
        public string factory;
        public string model;
        public string product_code;
        public string buyer_name;
        public string pallet_id;
        public string part_ids;
        public DateTime inspect_date;
        public string inspector_name;
        public string shift;
        public string data_category;
        public int data_length;
    }

    [Serializable]
    public struct INSP_INFO_RAW_TAG
    {
        public long step_seq;
        public long cell_seq;
        public string inspection_time;
        public string technology;
        public string product;
        public string line;
        public string facility;
        public string lot_id;
        public string step_id;
        public string lot_mode;
        public int test;
        public int slot_id;
        public long setup_seq;
        public string last_update;
        public string status;
        public string main_eq;
        public string inspection_eq;
        public string insp_filename;
        public string inspection_param;
        public int defects;
        public int classfied_defects;
        public int killer_defects;
        public int random_defects;
        public int adder_killer_defects;
        public int adder_random_defects;
        public int clusters;
        public int adder_clusters;
        public int cluster_area;
        public double defect_dd;
        public double killer_defect_dd;
        public double random_defect_dd;
        public int inspected_cell;
        public int defective_cell;
        public int adder_def_cell;
        public int kill_def_cell;
        public int images;
        public int scan_area;
        public int adder_defects;
        public int kill_rnd_defects;
    }

    [Serializable]
    public struct DEFECTIMAGE_TAG
    {
        public int step_seq;
        public int defect_id;
        public int image_id;
        public int test;
        public string image_type;
        public string image_path;
        public string image_filename;
        public string image_server;
        public Stream small_image;
    }

    [Serializable]
    public struct SPCSPEC_TAG
    {
        public string spec_code;
        public string product;
        public string facility;
        public string prodcode;
        public string process;
        public string oper;
        public string equip_id;
        public string main_equip_id;
        public string data_category;
        public string para_id;
        public string resolution;
        public string action;
        public string lsl;
        public string usl;
        public string lcl;
        public string ucl;
        public string lcl2;
        public string ucl2;
        public string target;
        public string cl;
        public string cl2;
        public string remark;
        public string rule_s;
        public string rule_a;
        public string rule_b;
        public string rule_c;
        public string rule_d;
        public string rule_e;
        public string rule_f;
        public string rule_g;
        public string rule_h;
    }

    [Serializable]
    public struct OPTICAL_TAG
    {
        public string white_x;
        public string white_y;
        public string white_lv;
        public string white_color_temp;
        public string red_x;
        public string red_y;
        public string red_lv;
        public string green_x;
        public string green_y;
        public string green_lv;
        public string blue_x;
        public string blue_y;
        public string blue_lv;
    }

    [Serializable]
    public struct FAB_INSP_HEADER_TAG
    {
        public string equipment_id;
        public string date;
        public string time;
        public string glass_id;
        public string judgment;
        public string cell1_defect;
        public string cell2_defect;
        public string cell3_defect;
        public string cell4_defect;
        public string cell5_defect;
        public string cell6_defect;
        public string cell7_defect;
        public string cell8_defect;
        public string cell9_defect;
        public string cell10_defect;
        public string cell11_defect;
        public string cell12_defect;
        public string recipe_id;
        public string operator_id;
        public string line_number;
        public string process_id;
        public string product_code;
        public string total_defect_number;

    }

    [Serializable]
    public struct FAB_AUTO_OS_INSP_TAG
    {
        public string defect_num;
        public string x_coordinate;
        public string y_coordinate;
        public string detect_size_dx;
        public string detect_size_dy;
        public string detect_size_area;
        public string defect_code;
        public string cell_type;
        public string identited_defect;
        public string image_count;
        public string image_name_n;
    }

    [Serializable]
    public struct FAB_REVIEWER_REPAIR_INSP_TAG
    {
        public string defect_num;
        public string x_coordinate;
        public string y_coordinate;
        public string detect_size_dx;
        public string detect_size_dy;
        public string detect_size_area;
        public string defect_code;
        public string cell_type;
        public string identited_defect;
        public string reviewer_result;
        public string repair_result;
        public string image_count;
        public string image_name_n;
    }

    [Serializable]
    public struct FAB_LENGTH_MEASURE_INSP_TAG
    {
        public string postion_num;
        public string x_coordinate;
        public string y_coordinate;
        public string top_width;
        public string button_width;
        public string gap;
        public string height;
        public string cell_type;
        public string judgement_result;
    }

    [Serializable]
    public struct FAB_BONDING_INSP_TAG
    {
        public string tcp_number;
        public string left_measure_value;
        public string right_measure_value;
        public string judgement_result;
        public string image_count;
        public string image_name_n;
    }

    [Serializable]
    public struct DATA_INFO_RAWDATA_TAG
    {
        public string data_seq;
        public string lot_seq;
        public string resulttimestamp;
        public string filetimestamp;
        public string factory;
        public string product;
        public string prodcode;
        public string process;
        public string oper;
        public string equip_id;
        public string main_equip_id;
        public string user_id;
        public string lot_id;
        public string lot_mode;
        public string msg_id;
        public string itc_id;
        public string dct_id;
        public string pallet_id;
        public string cst_id;
        public string part_ids;
        public string val_count;
        public string measure_cnt;
        public string line;
        public string step;
        public string model;
        public string buyer_name;
        public string equip_position;
        public string inspector_name;
        public string inspect_date;
        public string insp_start_time;
        public string insp_end_time;
        public string insp_rst;
        public string loss_code;
        public string work_class;
        public string insp_seq;
        public string in_v;
        public string in_r;
        public string wb_result;
        public string shift;
        public string data_category;
        public string data_length;
    }

    [Serializable]
    public struct FAB_LIGHTING_407300_ATYPE_TAG
    {
        public string pattern_no;
        public string param_code;
        public string position;
        public string value;
    }

    [Serializable]
    public struct FAB_LIGHTING_407300_BTYPE_TAG
    {
        public string pattern_no;
        public string fault_code;
    }

    [Serializable]
    public struct FAB_LIGHTING_407300_CTYPE_TAG
    {
        public string data_size;
        public string insp_comment;
    }

    [Serializable]
    public struct FAB_LIGHTING_407300_DTYPE_TAG
    {
        public string dummy;
        public string customer;
    }

    [Serializable]
    public struct STEP_TAG
    {
        public long STEP_SEQ;
        public long LOT_SEQ;
        public long UNIT_SEQ;
        public long SETUP_SEQ;
        public string STEP_ID;
        public string PALLET_ID;
        public string SLOT_ID;
        public int TEST;
        public string PROCESS;
        public string OPERATION;
        public string RESULT_ID;
        public string INSPECT_EQ;
        public string REVIEW_EQ;
        public string REPAIR_EQ;
        public string RESULTTIMESTAMP;
        public string FILETIMESTAMP;
        public string MAIN_EQ;
        public int SCAN_AREA;
        public int DEFECTIVE_CELL;
        public int DEFECTS;
        public int IMAGES;
        public double DEFECT_DENSITY;
        public string RESULTFILENAME;
        public string SERVICEFILENAME;
    }

    [Serializable]
    public struct SETUP_GLASS_TAG
    {
        public string setup_seq;
        public string setup_id;
        public string step_id;
        public string setup_time;
        public int angle;
        public int cell_count;
        public string notch_type;
        public double origin_x;
        public double origin_y;
        public double street_x;
        public double street_y;
    }

    [Serializable]
    public struct SETUP_CELL_TAG
    {
        public string setup_seq;
        public int cell_num;
        public int cell_origin_x;
        public int cell_origin_y;
        public int cell_pitch_x;
        public int cell_pitch_y;
        public int array_pitch_x;
        public int array_pitch_y;
        public int array_count_x;
        public int array_count_y;
    }

    [Serializable]
    public struct SCAN_SAMPLE_TAG
    {
        public string setup_seq;
        public string test;
        public int cell_num;
        public int index_x;
        public int index_y;
    }

    [Serializable]
    public struct IMAGES_TAG
    {
        public long step_seq;
        public int defect_id;
        public int image_id;
        public int test;
        public long wafer_seq;
        public string image_type;
        public string image_path;
        public string image_filename;
        public string image_server;
        public string small_image;
    }

    [Serializable]
    public struct DEFECT_COMMENT_TAG
    {
        public string step_seq;
        public string lot_seq;
        public int defect_id;
        public int comment_id;
        public string comments;
        public string user_id;
        public string comment_cate;
        public string comment_time;
    }

    [Serializable]
    public struct MAP_IMAGES_TAG
    {
        public string step_seq;
        public string test;
        public string map_image_type;
        public string map_image_path;
        public string map_image_filename;
        public string map_image_server;
        public string small_map_image;
    }

    [Serializable]
    public struct FAB_LIGHTING_407500_TAG
    {
        public string msg_id;
        public string return_code;
        public string return_msg;
        public string itc_id;
        public string dct_id;
        public string user_id;
        public string trans_time;
        public string queue_time;
        public string step;
        public string factory;
        public string oper;
        public string glass_id;
        public string equip_id;
        public string insp_rst;
        public string loss_code;
        public string work_class;
        public string insp_manual;
    }

    [Serializable]
    public struct FAB_COMMON_INSPECTION
    {
        public string equipment_id;
        public string date;
        public string time;
        public string glass_id;
        public string judgment;
        public string cell1_defect;
        public string cell2_defect;
        public string cell3_defect;
        public string cell4_defect;
        public string cell5_defect;
        public string cell6_defect;
        public string cell7_defect;
        public string cell8_defect;
        public string cell9_defect;
        public string cell10_defect;
        public string cell11_defect;
        public string cell12_defect;
        public string recipe_id;
        public string operator_id;
        public string line_number;
        public string process_id;
        public string product_code;
        public string origin_x;
        public string origin_y;
        public string total_defect_number;
    }

    [Serializable]
    public struct FAB_TESTER
    {
        public string defect_num;
        public string x_coordi;
        public string y_coordi;
        public string detect_size_x;
        public string detect_size_y;
        public string detect_size_area;
        public string defect_code;
        public string cell_distribute;
        public string cluster_defect;
        public string reviewer_result;
        public string repair_result;
        public string image_count;
        public string[] image_name;
    }

    [Serializable]
    public struct FAB_MEASURE
    {
        public string sub_recipe_name;
        public string measure_point_count;
        public string sub_recipe_measure_list;
        public string point_number;
        public FAB_MEASURE_POINT[] measure_point;
    }

    [Serializable]
    public struct FAB_MEASURE_POINT
    {
        public string x_coordi;
        public string y_coordi;
        public string[] parameter;
        public string judge_result;
    }
    #endregion

 
}
