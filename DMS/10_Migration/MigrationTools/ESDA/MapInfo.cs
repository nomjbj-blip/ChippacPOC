using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace MigrationTools.MapInfo
{

    [Serializable]
    public struct MapInfo
    {
        public string Device;
        public int Max_rows;
        public int Max_cols;
        public decimal X_period;
        public decimal Y_period;
        public decimal X_os;
        public decimal Y_os;
        public decimal Street_width;
        public decimal Street_height;

    }

    [Serializable]
    public struct Map
    {
        public int Num_tagvalue_pairs;
        public int Num_columns;
        public int Max_x;
        public int Max_y;
        public int Num_die;
        public DIEINFO_TAG[] TestDieInfo;

    }

    public struct DIEINFO_TAG
    {
        public int Index;						
        public int DX;
        public int DY;
    }

    //========================================

    [Serializable]
    public struct MAP_SETUP
    {
        public int TEST_ANGLE;
        public int WAFER_SIZE;
        public string NOTCH_TYPE;
        public decimal ORIGIN_X;
        public decimal ORIGIN_Y;
        public int TEST_X;
        public int TEST_Y;
    }

    [Serializable]
    public struct DEFECT_INFO
    {
        public int DefectCount;
        public int DefectDieCount;
        public DEFECT[] DefectList;
    }

    public struct DEFECT
    {
        public int Index;
        public int DIE_X;
        public int DIE_Y;
        public decimal Xmicron;
        public decimal Ymicron;
        public decimal DefectSize;
        public decimal Xsize;
        public decimal Ysize;
        public string Newdefect;
        public string Image_file_name;
        public string Thumb_file_name;

        public string First_level;
        public int Defect_category;
        public int ADC;
        public int Defect_cluster;
        public int REV;
        public int Roughbin;
    }
    
}
