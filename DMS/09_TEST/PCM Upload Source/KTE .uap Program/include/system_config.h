/*
 -----------------------------------------------------------------------------

 File          :  System_Config.h

 Author        :  MOS4YOU PhilLib Team

 Creation Date :  11-Mar-1998

 Modification  :  <date>, <user>

 Contents:        Model 9110 VIMS    4  VIMS1, VIMS2, VIMS3, VIMS4
                  Model 9121 CGK     1  CMTR1
                  Model 9150 SRU     1  REF1
                  Model 9162 PAU     1  IMTR1
                  Model 9195 ETM     1  TIMER1
                  Model 9345 MIVS    1  VRSC1
                  Model 9350 FCM     1  FMTR1

                  Model 9130 Matrix  32 DUT Pins
                                     6  Pathways

 -----------------------------------------------------------------------------
*/

#define TOTAL_DUT            32
#define TOTAL_PTH             6
#define TOTAL_SMU             5
#define TOTAL_VIMS            4
#define TOTAL_MIVS            1
#define TOTAL_HIVS            0

#define ENABLE_CGK            1
#define ENABLE_PAU            1
#define ENABLE_ETM            1
#define ENABLE_FCM            1

/*
#define SMU1                 VIMS1
#define SMU1H                VIMS1H
#define SMU1L                VIMS1L

#define SMU2                 VIMS2
#define SMU2H                VIMS2H
#define SMU2L                VIMS2L

#define SMU3                 VIMS3
#define SMU3H                VIMS3H
#define SMU3L                VIMS3L

#define SMU4                 VIMS4
#define SMU4H                VIMS4H
#define SMU4L                VIMS4L

#define SMU5                 VRSC1
#define SMU5H                VRSC1H
#define SMU5L                VRSC1L
*/

#define SYS_CAP              1.0e-12
#define SYS_LEAK             1.0e-12
#define SYS_RES              1.0e11

/* -------------------------------------------------------------------------- */
