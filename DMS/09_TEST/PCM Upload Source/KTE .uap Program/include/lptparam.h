/*

    lptparam.h 

    Parameter definitions.
    Linear Parametric Test Library.


    Copyright (c) 1986, 1987, 1988, 1989, 1993 by Keithley Instruments, 
    Inc. Cleveland, Ohio

    This software is furnished under a license and may be used and copied 
    only in accordance with the terms of such license, and with the 
    inclusion of the above copyright notice.  This software or any other 
    copies hereof may not be provided or otherwise made available to any 
    other person.  No title to and ownership of the software is hereby 
    transferred.  The information in this software is subject to change 
    without notice, and should not be construed as a commitment by 
    Keithley Instruments, Inc.

    Keithley assumes no responsibility for the use or reliability of its 
    software on equipment which is not supplied by Keithley.

*/     
/*

 $Workfile:$
   $Source: /cm/test/build/S600/v420/PROJCOM/RCS/lptparam.h,v $
 $Revision: 1.25 $
 Rev $Date: 2000/08/03 17:54:10 $

 Change History
 * $Log: lptparam.h,v $
 * Revision 1.25  2000/08/03 17:54:10  hayes
 * PR12889 Added parameters for setting IMTR and VMTR modes.
 *
 * Revision 1.24  2000/07/18 20:53:01  hayes
 * Added LPT feature id codes for licensing.
 *
 * Revision 1.23  2000/06/27 19:42:14  hayes
 * Added new IDs for PCID.
 *
 * Revision 1.22  1999/03/24 16:35:13  hayes
 * PR08536 Added new stat codes for getting instrument information.
 *
 * Revision 1.21  1999/03/08 19:38:54  hayes
 * PR03286 Added KI_SENSE enumeration.
 *
 * Revision 1.20  1999/01/25 21:36:34  hayes
 * PR08801 Updated #defines which keep track of number of entries for each
 * section.
 *
 * Revision 1.19  1999/01/14 22:24:22  hayes
 * Added new setmode identifiers for the matrix driver.
 *
 * Revision 1.18  1998/08/07 17:39:34  furio
 * Added KI_REF_SELECT and KI_REF_MOVE modes for SRU setmode support.
 * Added KI_REF_VALUE modifier for SRU getstatus support.
 *
 * Revision 1.17  1998/05/18 20:03:16  hayes
 * PR06029  Added identifiers to control checking a bias source if it is
 * in limit before taking a measurement on the CGK.
 *
 * Revision 1.16  1998/01/14 21:39:03  chaplin
 * 1. Added KI_LIM_INDCTR setmode modifier, to tell the SMU what measure value
 *    to return when it is at the programmed limit.
 * 2. Removed KI_SETTLE_{SHORT,MEDIUM,LONG} value #defines -- they are
 *    obsolete -- the setmode(KI_RANGE_SETTLE) now takes just the percent
 *    accuracy as a number.
 *
 * Revision 1.15  1997/11/13 23:06:38  chaplin
 * Added KI_AVGMODE - valid values are KI_MEASX and KI_INTEGRATE; controls the
 * behavior of AVGx, no matter how AVGx ended up being called (i.e. directly
 * or via trigger)
 *
 * Revision 1.14  1997/09/18 13:59:38  chaplin
 * added KI_FILTER because 60120CGK cmeter needs it
 *
 * Revision 1.13  1997/05/30 17:32:49  hayes
 * Put back some defines which should not have been removed.
 *
 * Revision 1.12  1997/05/28 21:11:13  hayes
 * Added KI_CC_xxx modifiers for setmode.
 *
 * Revision 1.11  1997/02/26 23:28:08  hayes
 * Added KI_OPMODE to allow control over the preamp opmode for 590 C/V support.
 *
 * Revision 1.10  1997/01/06 14:49:28  hayes
 * Added identifiers for refctrl.
 *
 * Revision 1.9  1996/10/23 19:06:31  tobin
 * added enumerated data types for the sync modes.
 *
 * Revision 1.8  1996/10/23  18:51:23  chaplin
 * added #defines for new setmode KI_RANGE_SETTLE and KI_RANGE_DELAY (for SMUs)
 *
 * Revision 1.7  1996/10/08  22:12:31  hayes
 * Formatting only.
 *
 * Revision 1.6  1996/09/27  13:37:53  furio
 * Added #define of KI_PCASTATE for getstatus data request option.
 *
 * Revision 1.5  1996/08/16  15:20:45  chaplin
 * added getstatus parameter KI_CMPLNC_EVER
 *
 * Revision 1.4  1996/05/30  14:54:22  chaplin
 * added KI_COMPLNC
 *
 * Revision 1.3  1996/05/13  20:44:16  chaplin
 * added modifier for SMU integration time (setmode call)
 *
 * Revision 1.2  1996/04/01  14:22:55  hayes
 * First working MATRIX driver.
 *
 * Revision 1.1  1995/08/14  16:06:54  hayes
 * Initial revision
 *
*/

#ifndef LPTPARAM_H
#define LPTPARAM_H

/****************************************************************************
*
* Enumeration name: SYNCMODE - These values are used as arguments to 
*                              syncmode() to set the synchronization mode. 
*
*       SYNC_NONE - all lpt commands will be executed asynchronously
*       SYNC_FULL - all lpt commands will complete before the next lpt 
*                   command will be executed.
*     SYNC_RESULT - only lpt commands that return a value will be 
*                   synchronized (i.e after receiving the result from the 
*                   tester then subsequent lpt commands will be allowed 
*                   to execute).
*
****************************************************************************/

enum SYNC_MODE
{
    SYNC_NONE   = 0x00000000,
    SYNC_FULL   = 0x00000001,
    SYNC_RESULT = 0x00000002
};


/*
    refctrl parameters
*/
enum REFCTRL_CMDS
{
    REF_ON              = 0x0001,
    REF_OFF             = 0x0002,
    REF_REFRESH_MEAS    = 0x0010,
    REF_REFRESH_INTG    = 0x0020
};

/*
    conpin pin options
*/
enum PINOPTS
{
    KI_SENSE            = 0x0800
};

enum lpt_features_codes
{
    LPT_FEATURE_PCID    = 0x00000001
};


/*****************************************************************************
   Modifier Flag Field Definitions
*****************************************************************************/


#define MINMODS      10001

#define KI_TRIGMODE     10001    /* Define TRIGGER acquisition modes */
#define KI_AVGNUMBER    10002    /* Number of points to AVG for TRIG */
#define KI_AVGTIME      10003    /* Time delay per reading for TRIG */
#define KI_REVIMEAS     10004    /* ForceMEASI VIMS Current Pol */
#define KI_MEASURE      10005    /* Used for 775/617 */
#define KI_IRANGE       10006    /* Used for 617 */
#define KI_QRANGE       10007    /* Coloumb range fo 617 */
#define KI_ZEROCHECK    10008    /* Turn ON ZeroCheck for 617 */
#define KI_ZEROCORRECT  10009    /* Turn ON ZeroCorrect for 617 */
#define KI_SUPPRESS     10010    /* BaseLine Suppression */
#define KI_VFORCE       10011    /* ForceV for 617 bias. */
#define KI_SETTRIGA     10012    /* 775 trigger A/B */
#define KI_SETTRIGB     10013    /* 775 trigger A/B */
#define KI_CHANNELA     10014    /* 775 channel selection */
#define KI_CHANNELB     10015    /* 775 channel selection */
#define KI_SETGATE      10016    /* Define GATE sample */
#define KI_POINTSWP     10017    /* Points instead of Steps */
#define KI_SMARTCLEAR   10018    /* Dynamic selection of DEVCLR (1/2) */
#define KI_PSCLEAR      10019    /* Permanent selection of DEVCLR. 'System default'. Tweaked by SMARTCLEAR */
#define KI_EXTINPUT     10020    /* External Input selection (PAU) */
#define KI_SAUTO        10021    /* Sticky Autoranging. (VIMS) */
#define KI_HIRES        10022    /* High Resolution (VIMS) */
#define KI_HICURRENT    10023    /* Matrix High current mode */
#define KI_INTGPLC      10024    /* SMU integration time (for INTGx) */
#define KI_RANGE_SETTLE 10025    /* SMU-preamp range change settle */
#define KI_RANGE_DELAY  10026    /* SMU-preamp range change delay */
#define KI_OPMODE       10027    /* Preamp Opmode -- for internal use only */
#define KI_CC_AUTO      10030    /* Compliance clear automatically called */
#define KI_CC_SRC_DLY   10031    /* Compliance clear source delay */
#define KI_CC_COMP_DLY  10032    /* Compliance clear interscan delay */
#define KI_CC_MEAS_DLY  10033    /* Compliance clear pre-measure delay */
#define KI_FILTER       10034    /* Filter state */
#define KI_AVGMODE      10035    /* AVGx uses MEASx or INTGx */
#define KI_LIM_INDCTR   10036    /* Specify meas value to return when in limit */
#define KI_LIM_MODE     10037    /* Return actual value or special indicator when in limit */
#define KI_LIM_CHECK    10038    /* Disable limit checks for bias source */
#define KI_REF_SELECT   10039    /* Select a SRU reference component */
#define KI_REF_MOVE     10040    /* Move selected SRU ref to specified pin */
#define KI_MX_DEFMODE   10041    /* Default matrix mode */
#define KI_PCAINCCNT    10042    /* For internal use only */
#define KI_VMTR         10043    /* vmtr mode */
#define KI_IMTR         10044    /* imtr mode */

#define MAXMODS KI_IMTR
#define NUMMODS (MAXMODS - MINMODS + 1)


/*   Modifier 'Values' */
#define KI_OFF          1.0     /* General enable/disable */
#define KI_ON           2.0     /* General enable/disable */
#define KI_DCON         3.0     /* CHANNELA/CHANNELB 775 control. */
#define KI_ACON         4.0     /* CHANNELA/CHANNELB 775 control. */
#define KI_ATTEN1X      5.0     /* CHANNELA/CHANNELB 775 control. */
#define KI_ATTEN10X     6.0     /* CHANNELA/CHANNELB 775 control. */
#define KI_FLTON        7.0     /* CHANNELA/CHANNELB 775 control. */
#define KI_FLTOFF       8.0     /* CHANNELA/CHANNELB 775 control. */
#define KI_PSLOPE       9.0     /* CHANNELA/CHANNELB 775 control. */
#define KI_NSLOPE       10.0    /* CHANNELA/CHANNELB 775 control. */
#define KI_AMPS         11.0    /* 617 Measure mode (Ammeter measurement). */
#define KI_COUL         12.0    /* 617 Measure mode (Coulombs measurement). */
#define KI_FREQA        13.0    /* 775 Measure mode (Frequency on Channel A). */
#define KI_FREQB        14.0    /* 775 Measure mode (Frequency on Channel B). */
#define KI_FREQC        15.0    /* 775 Measure mode (Frequency on Channel C). */
#define KI_PERIODA      16.0    /* 775 Measure mode (Period on Channel A). */
#define KI_AVGPERIODA   17.0    /* 775 Measure mode (Period average on Channel A). */
#define KI_TIMEATOB     18.0    /* 775 Measure mode (Time interval from A to B). */
#define KI_PULSEA       19.0    /* 775 Measure mode (Pulse Width on Channel A). */
#define KI_TOTABYB      20.0    /* 775 Measure mode (Totalized on Channel A gated by Channel B). */
#define KI_TOTCUM       21.0    /* 775 Measure mode (Cumulative Total on Channel A). */
#define KI_ABSOLUTE     22.0    /* Absolute Value trigger comparision. */
#define KI_NORMAL       23.0    /* Normal 'signed' trigger comparision. */
#define KI_INTEGRATE    24.0    /* Use INTGx measurement for 'trigger acquisition' or AVGx. */
#define KI_AVERAGE      25.0    /* Use AVGx measurement for 'trigger acquisition'. */
#define KI_MEASX        26.0    /* Use MEASx measurement for 'trigger acquisition' or AVGx. */
#define KI_VALUE        27.0    /* Use actual value */
#define KI_INDICATOR    28.0    /* Use indicator value */
#define KI_HIGH         29.0
#define KI_LOW          30.0
#define KI_S400         31.0    /* S400 compatible mode */
#define KI_DMM          32.0    /* DMM vmtr and imtr modes */
#define KI_ELECTROMETER 33.0    /* Electrometer vmtr and imtr modes */

/*   I*4 versions of Modifier 'Values' */
#define KI_IOFF         1       /* General enable/disable. */
#define KI_ION          2       /* General enable/disable. */
#define KI_IDCON        3       /* CHANNELA/CHANNELB 775 control. */
#define KI_IACON        4       /* CHANNELA/CHANNELB 775 control. */
#define KI_IATTEN1X     5       /* CHANNELA/CHANNELB 775 control. */
#define KI_IATTEN10X    6       /* CHANNELA/CHANNELB 775 control. */
#define KI_IFLTON       7       /* CHANNELA/CHANNELB 775 control. */
#define KI_IFLTOFF      8       /* CHANNELA/CHANNELB 775 control. */
#define KI_IPSLOPE      9       /* CHANNELA/CHANNELB 775 control. */
#define KI_INSLOPE      10      /* CHANNELA/CHANNELB 775 control. */
#define KI_IAMPS        11      /* 617 Measure mode (Ammeter measurement). */
#define KI_ICOUL        12      /* 617 Measure mode (Coulombs measurement). */
#define KI_IFREQA       13      /* 775 Measure mode (Freq on Channel A). */
#define KI_IFREQB       14      /* 775 Measure mode (Frequency on Channel B). */
#define KI_IFREQC       15      /* 775 Measure mode (Freq on Channel C). */
#define KI_IPERIODA     16      /* 775 Measure mode (Period on Channel A). */
#define KI_IAVGPERIODA  17      /* 775 Measure mode (Period avg on Channel A). */
#define KI_ITIMEATOB    18      /* 775 Measure mode (Time interval from A to B). */
#define KI_IPULSEA      19      /* 775 Measure mode (Pulse Width on Channel A). */
#define KI_ITOTABYB     20      /* 775 Measure mode (Totalized on Channel A gated by Channel B). */
#define KI_ITOTCUM      21      /* 775 Measure mode (Cumulative Total on Channel A). */
#define KI_IABSOLUTE    22      /* Absolute Value trigger comparision. */
#define KI_INORMAL      23      /* Normal 'signed' trigger comparision. */
#define KI_IINTEGRATE   24      /* Use INTGx measurement for 'trigger acquisition'. */
#define KI_IAVERAGE     25      /* Use AVGx measurement for 'trigger acquisition'. */
#define KI_IMEASX       26      /* Use MEASx measurement for 'trigger acquisition'. */
#define KI_IVALUE       27      /* Use actual value */
#define KI_IINDICATOR   28      /* Use indicator value */
#define KI_IHIGH        29
#define KI_ILOW         30
#define KI_IS400        31      /* S400 compatible mode */
#define KI_IDMM         32      /* DMM vmtr and imtr modes */
#define KI_IELECTROMETER 33     /* Electrometer vmtr and imtr modes */

#define NUMMODVAL KI_IELECTROMETER   /* Number of these 'values' for INS_COM */

/*****************************************************************************
   Return Status Flag Field Definitions
*****************************************************************************/

#define MINSTATS  12001

#define KI_IPVALUE  12001   /* Current programmed I (Current) value (I output value R*4). */
#define KI_VPVALUE  12002   /* Current programmed V (Voltage) value (V output value R*4). */
#define KI_CPRANGE  12003   /* Current programmed C (Capacitance) range (FS range value R*4 or 0.0 for autorange). */
#define KI_GPRANGE  12004   /* Current programmed G (Conductance) range (FS range value R*4 or 0.0 for autorange). */
#define KI_IPRANGE  12005   /* Current programmed I (Current) range (FS range value R*4 or 0.0 for autorange). */
#define KI_VPRANGE  12006   /* Current programmed V (Voltage) range (FS range value R*4 or 0.0 for autorange). */
#define KI_CARANGE  12007   /* Current active C (Capacitance) range (FS range value R*4). */
#define KI_GARANGE  12008   /* Current active G (Conductance) range (FS range value R*4). */
#define KI_IARANGE  12009   /* Current active I (Current) range (FS range value R*4). */
#define KI_VARANGE  12010   /* Current active V (Voltage) range (FS range value R*4). */
#define KI_CMRANGE  12011	/* Current programmed C (Capacitance) measurement range. Range which previous C/G measurement was taken on (for autorange) (R*4). */
#define KI_GMRANGE	12012	/* Current programmed G (Conductance) measurement range. Range which previous C/G measurement was taken on (for autorange) (R*4). */
#define KI_IMRANGE	12013	/* Current programmed I (Current) measurement range. Range which previous I measurement was taken on (for autorange) (R*4). */
#define KI_VMRANGE	12014	/* Current programmed V (Voltage) measurement range. Range which previous V measurement was taken on (for autorange) (R*4). */
#define KI_UCBPTR	12015	/* Return current UCB pointer for this instrument */
#define KI_MODEL	12016	/* Model ID string. */
#define KI_COMPLNC	    12017	/* Compliance status */
#define KI_COMPLNC_EVER	12018	/* Compliance history */
#define KI_PCASTATE	    12019   /* PCA State */
#define KI_REF_VALUE    12020   /* Return actual value of SRU reference */
#define KI_MODELCODE    12021   /* Model code (integer) */
#define KI_INSNAME      12022   /* Instrument name, SMU1 etc. */
#define KI_SERIALNO     12023   /* Serial number */
#define KI_REVISION     12024   /* Revision string */
#define KI_MXMAP        12025   /* For internal use only */
#define KI_INSLIST      12026   /* All configured instrument ids in the system */
#define KI_PINLIST      12027   /* All configured pins in the system */
#define KI_PCACOUNTCYCL 12028   /* Probecard cycle touchdown counts */
#define KI_PCACOUNTLIFE 12029   /* Probecard lifetime touchdown counts */
#define KI_PCAUSERSER   12030   /* Probecard user serial number */
#define KI_PCAUSERTYPE  12031   /* Probecard user type */
#define KI_PCAUSERCOM   12032   /* Probecard user comment */

#define MAXSTATS (KI_PCAUSERCOM)
#define NUMSTATS ((MAXSTATS - MINSTATS) + 1)

#endif

