/*

    lptmsg.h 
   
    Symbolic error codes.
    Linear Parametric Test Library.
          

    Copyright (c) 1993 by Keithley Instruments, Inc. Cleveland, Ohio
     
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
   $Source: /cm/test/build/S600/v420/PROJCOM/RCS/lptmsg.h,v $
 $Revision: 1.72 $
 Rev $Date: 2000/09/27 22:08:34 $
     
 Change History
 * $Log: lptmsg.h,v $
 * Revision 1.72  2000/09/27 22:08:34  hayes
 * PR13418 Added CB_FWMISMATCH error code.
 *
 * Revision 1.71  2000/07/18 20:45:00  hayes
 * Added CB_NOTLIC error.
 *
 * Revision 1.70  2000/06/29 18:50:49  hayes
 * Added new error codes to support PCID.
 *
 * Revision 1.69  2000/03/24 18:49:47  hayes
 * PR11357 Added TAPI_CHANLIMIT error code.
 *
 * Revision 1.68  2000/02/17 20:20:24  hayes
 * PR11500 Added new error codes that might be generated with the use
 * of the CHUCKG pseudo-terminal identifier.
 *
 * Revision 1.67  1999/08/02 15:03:53  hayes
 * PR09758 Added error message for when a unit belongs to one context
 * but is requested to be assigned to another.
 *
 * Revision 1.66  1999/04/06 18:25:00  hayes
 * PR09180 Added new error code for sense line not connected.
 *
 * Revision 1.65  1999/03/11 16:14:34  hayes
 * PR03286 Added some new error codes.
 *
 * Revision 1.64  1999/03/02 18:41:31  hayes
 * PR03286 Added new error code for disallowing hot switching the matrix.
 *
 * Revision 1.63  1999/02/02 17:51:37  hayes
 * Added MIN_ERR_INDICATOR as define for 1e22.
 *
 * Revision 1.62  1998/06/09 22:01:34  hayes
 * PR07092 Added SRU_TIMEOUT error code.
 *
 * Revision 1.61  1998/04/22 14:51:19  hayes
 * PR02509 Added new error codes to support temp and rh measurements.
 *
 * Revision 1.60  1998/02/24 14:20:26  hayes
 * PR06412 Added some more code to also check the UNAVAILABLE flag
 * of the client side as well.  Also now distinguish between
 * incompatible units (no services that match) and unavailable
 * units (match found but all services unavailable).
 *
 * Revision 1.59  1998/02/23 18:37:54  hayes
 * PR06412 Added UT_BINDFAIL error code.
 *
 * Revision 1.58  1998/02/09 15:56:03  hayes
 * PR06269  Added LPT_TOMANYARGS error code.
 *
 * Revision 1.57  1998/01/27 19:39:41  hayes
 * PR05751  Added UT_TMRNOSTAMP error code.
 *
 * Revision 1.56  1998/01/14 21:38:45  chaplin
 * added SOURCE_LIMIT
 *
 * Revision 1.55  1998/01/14 17:11:56  hayes
 * PR06003 Removed some obsolete error messages.
 *
 * Revision 1.54  1998/01/13 19:38:10  hayes
 * PR06003 Moved some codes to new numbers.  Gave several new names (old ones
 * must still be removed after all code has been updated).
 *
 * Revision 1.53  1997/12/08 14:51:52  chaplin
 * Added EEPROM_READ_ERROR and EEPROM_WRITE_ERROR
 *
 * Revision 1.52  1997/11/17 20:00:31  chaplin
 * added CGK_MULTIRX
 *
 * Revision 1.51  1997/11/10 18:07:32  hayes
 * Added MX_NOBIASPATH error number.
 *
 * Revision 1.50  1997/11/07 22:09:01  chaplin
 * added UE_UNAVAIL
 *
 * Revision 1.49  1997/09/18 13:58:57  chaplin
 * added FM_CRTOBG and FM_GRTOBG
 *
 * Revision 1.48  1997/09/15 15:12:38  hayes
 * Added error message for a service not being supported.
 *
 * Revision 1.47  1997/09/12 20:10:06  chaplin
 * added FM_CGKERR and CGK_NOBIAS
 *
 * Revision 1.46  1997/07/11 22:53:56  chaplin
 * added EEPROM_ERROR (part of fix for PR4763 and PR4767)
 *
 * Revision 1.45  1997/06/30 20:08:24  chaplin
 * changed error -164 from CB_DEVAT255 to the more generic CB_DEVINVLDADDR
 * (device at invalid address)
 * part of fix for PR4628
 *
 * Revision 1.44  1997/05/09 19:30:01  hayes
 * Added UT_FILEIOERR error code.
 *
 * Revision 1.43  1997/05/02 13:08:33  chaplin
 * added FM_NOCON
 *
 * Revision 1.42  1997/04/25 15:21:42  chaplin
 * added CB_DEVAT255
 *
 * Revision 1.41  1997/04/18 18:35:30  hayes
 * Added error codes for invalid error and event numbers.
 *
 * Revision 1.40  1997/04/14 17:20:38  hayes
 * Added TAPI_BADUNIT error code.
 *
 * Revision 1.39  1997/04/11 15:19:42  chaplin
 * added IE_RBUFFOFLW define
 *
 * Revision 1.38  1997/03/19 21:39:58  chaplin
 * added IE_INVLDSECADDR and IE_INVLDPRIADDR
 *
 * Revision 1.37  1997/03/11 16:06:31  chaplin
 * added CB_BADDPRAM
 *
 * Revision 1.36  1997/03/06  19:57:46  hayes
 * Added KFP_PWRNEG error.
 *
 * Revision 1.35  1997/02/13  01:38:21  hayes
 * Added LPT_ABORT error code.
 *
 * Revision 1.34  1997/02/02 01:24:19  hayes
 * Added error message for entering fatal state while in testing state.
 *
 * Revision 1.33  1997/01/31 18:55:43  moore
 * added support for DIAG_FAULT and DIAG_FATAL_FAULT codes
 *
 * Revision 1.32  1997/01/31  15:40:49  chaplin
 * added FM_SMUERR
 *
 * Revision 1.31  1997/01/28  21:23:56  hayes
 * Added LPT_PREVERR and LPT_FATAL error codes.
 *
 * Revision 1.30  1997/01/08 16:03:03  hayes
 *  Updated data structures.
 * Added protocol version checking.
 *
 * Revision 1.29  1996/12/20 21:44:44  chaplin
 * 1. added IB_NO_UTIL_FUNC
 * 2. added comments on all the IB_ errors, equating them to their
 *    corresponding IBUP error numbers
 *
 * Revision 1.28  1996/12/20  00:30:01  furio
 * Added SRU Error Codes.
 * PR02609
 *
 * Revision 1.27  1996/12/13  19:26:22  hayes
 * Added MOD_NOTFOUND error.
 *
 * Revision 1.26  1996/12/13 19:22:37  chaplin
 * added FM_RNGTOSML
 *
 * Revision 1.25  1996/12/11  20:53:26  chaplin
 * added MX_OPINVLD
 *
 * Revision 1.24  1996/12/11  16:22:14  hayes
 * Added MX_NOPIN to replace MX_NOMOREPINS as this more acurately
 * reflects the architecture of the S600.
 *
 * Revision 1.23  1996/12/02 21:45:47  hayes
 * New TAPI errors.
 *
 * Revision 1.22  1996/12/02 14:57:34  chaplin
 * added CB_DEVFAIL and CB_DEVNOTREC
 *
 * Revision 1.21  1996/10/28  22:38:26  hayes
 * Added MX_ILLGTSN id.
 *
 * Revision 1.20  1996/10/08 22:13:52  hayes
 * Formatting only.
 *
 * Revision 1.19  1996/08/26  19:15:19  clark
 * Added Error Type for Bad Level values for diagnostics on DiagLevelSet()
 *
 * Revision 1.18  1996/08/26  17:46:18  clark
 * Added Error code for Passing a NULL pointer to a global access diaganostics functions.
 *
 * Revision 1.17  1996/08/26  14:29:46  tobin
 * added pnuemonics for IBUP GPIB error messages (-510 through -523).
 *
 * Revision 1.16  1996/08/21  18:42:36  tobin
 * added CB_NOFILE and CB_FORMAT
 *
 * Revision 1.15  1996/07/26  20:22:41  hayes
 * More LPT functionality.  Also added fvmi and fimv.
 *
 * Revision 1.14  1996/07/11  21:25:35  hayes
 * Added versions of IDs with spelling consistent with the S400.
 *
 * Revision 1.13  1996/05/30  14:53:53  chaplin
 * added FM_VLTOSML
 *
 * Revision 1.12  1996/05/15  22:04:14  hayes
 * Added new S600 error codes.
 *
 * Revision 1.11  1996/04/16  19:17:24  hayes
 * Added buffex error logging support.
 *
 * Revision 1.10  1996/04/01  14:22:55  hayes
 * First working MATRIX driver.
 *
 * Revision 1.9  1996/02/08  19:29:11  hayes
 * Added KLP error codes for buffer execution support
 *
 * Revision 1.8  1996/01/10  22:26:13  hayes
 * Added error codes used by serialcom.c
 *
 * Revision 1.7  1996/01/10  19:55:14  chaplin
 * added LAST_ERROR
 *
 * Revision 1.6  1996/01/10  19:48:47  chaplin
 * KFP_LOGNET should have been KFP_LOGNEG
 *
 * Revision 1.5  1996/01/10  19:34:13  hayes
 * Added new error codes for driver load errors.
 *
 * Revision 1.4  1995/12/28  17:18:58  hayes
 * Added UT_SPRUINT
 *
 * Revision 1.3  1995/12/22  20:00:47  chaplin
 * fixed a typo: FM_OSCDET
 *
 * Revision 1.2  1995/08/22  19:29:24  chaplin
 * Added LPT_NORMAL.
 * Also, changed a PVCS keyword to its matching RCS keyword (I can't say
 * here what it is, or else RCS will expand it in my comment, heh heh).
 *
 * Revision 1.1  1995/08/14  16:05:30  hayes
 * Initial revision
 *
*/

#ifndef LPTMSG_H
#define LPTMSG_H

#define NOT_OK                1 /* This MUST be; it is the same as !OK */
#define OK                    0
#define LPT_NORMAL            OK

/* 
    Generic Error is what !OK or NOT_OK get translated to by the error
    logger.  The error logger requires that all error codes be negative!
*/
#define GENERIC_ERR          -1
#define TOO_MANY_INSTR       -4
#define MEM_ALLOC_ERR        -5
#define SYS_MEM_ALLOC_ERR    -5

#define LPT_PREVERR         -20
#define LPT_FATAL           -21
#define LPT_FATALINTEST     -22
#define LPT_ABORT           -23
#define LPT_TOMANYARGS      -24
#define LPT_INSINUSE        -25
/* here through 74 reserved for lpt errors */

#define KI_SNDTIMOUT        -77
#define KI_RCVTIMOUT        -78
#define KI_VMESNDERR        -82
#define KI_VMERCVERR        -83
#define KI_BADCONFIG        -88

#define MX_INVLDCNT        -100
#define LPT_INVLDCNT       -100     /* for S400 compatibility */
#define MX_NOPIN           -101
#define MX_NOMOREPINS      -101     /* for S400 compatibility */
#define MX_MULTICON        -102
#define MX_DANGCON         -103
#define MX_UNRECDEV        -104
#define MX_NOPATHASN       -105
#define MX_PTHPREASN       -106
#define MX_NOTENFPATH      -107
#define MX_ARGNOTDEF       -108
#define MX_ILLGLTSN        -109
#define MX_NOGND           -110
#define MX_NOLOW           -111
#define MX_NOSWITCH        -113
#define MX_ILLGLCON        -114
#define MX_OPINVLD         -115
#define MX_NOBIASPATH      -116
#define MX_BUSINUSE        -117
#define MX_HOTSWITCH       -118
#define MX_PININUSE        -119

#define UT_RSLTOFLW        -120
#define UT_TRGTBLOVR       -121
#define UT_INVLDPRM        -122
#define UT_SCNTBLOVR       -124
#define UT_NOURAM          -126
#define UT_SPURINT         -127
#define UT_TMRIVLD         -129
#define UT_TRIGTOBIG       -130
#define UT_CLRTBLOVR       -133
#define UT_INTTBLOVR       -134
#define UT_INVLDVAL        -137
#define UT_TOMANYPTS       -138
#define UT_FILEIOERR       -139
#define UT_UNAVAIL         -140
#define UT_TMRNOSTAMP      -141
#define UT_BINDFAIL        -142
#define UT_BINDREFUSED     -143
/* here through 149 reserved */

#define CB_DEVNTFND        -150
#define CB_UNKFUNC         -151
#define CB_BADFUNC         -152
#define CB_NOTCFG          -153
#define CB_NOTCNFG         -153     /* This is the spelling on the S400 */
#define CB_UNKDEV          -154
#define CB_NOFILE          -156
#define CB_FORMAT          -157
#define CB_DEVFAIL         -158
#define CB_DEVNOTVXI       -159
#define CB_FUNCCNFLCT      -160
#define CB_BADDPRAM        -161
#define CB_INVLDERROR      -162
#define CB_INVLDEVENT      -163
#define CB_DEVINVLDADDR    -164
#define CB_SVCNOTSUP       -165
#define CB_INSNOTREC       -166
#define CB_NOHWSUP         -167
#define CB_NOTLIC          -168
#define CB_FWMISMATCH      -169
/* here through 189 reserved for configuration/boot errors */

#define MX_ILLFRMCON       -190
#define MX_MODECONFLICT    -191
#define MX_NOSENSE         -192
#define MX_MUTEX           -193
#define MX_INVLDTRM        -194
/* here through 199 reserved for matrix errors */

#define FM_FTOBG           -200
#define FM_RNGCHNG         -201
#define FM_ILTOSML         -202
#define FM_ILTOBG          -203
#define FM_IRTOBG          -204
#define FM_MXERR           -205
#define FM_VLTOBG          -206
#define FM_VRTOBG          -207
#define FM_SETMTR          -208
#define FM_LIMMTR          -209
#define FM_SRCMTR          -210
#define FM_MIVMTR          -211
#define FM_MVIMTR          -212
#define FM_VALTOBIG        -213
#define FM_CMCTOBIG        -214
#define FM_CMGTOBIG        -215
#define FM_CMNOSRC         -216
#define FM_NODRNG          -217
#define FM_SOA             -218
#define FM_OSCDET          -219
#define FM_SHUTDOWN        -220
#define FM_THERMAL         -221
#define FM_PULSE           -222
#define FM_LIMTOBIG        -224
#define FM_AUTOLIMIT       -228
#define FM_QRTOBG          -229
#define FM_VLTOSML         -230
#define FM_RNGTOSML        -231
#define FM_NOCON           -233
#define FM_CRTOBG          -235
#define FM_GRTOBG          -236
#define FM_NOBIAS          -237
#define FM_NOVMTR          -238
/* here through 249 reserved for force/measure errors */

#define IE_TIMOUT          -250
#define IE_NOLISTEN        -251
#define IE_NOHRDWAR        -252
#define IE_INVLDSECADDR    -253
#define IE_INVLDPRIADDR    -254
#define IE_RBUFFOFLW       -255

#define SRU_NOSMU		   -261
#define SRU_TIMEOUT        -262
#define SRU_NODMM	   	   -263
#define SRU_GPIBFAIL	   -264
#define SRU_NOCAL		   -265
#define SRU_CMDERR		   -266
#define SRU_HRDFAIL		   -267
#define SRU_KELVIN		   -268
#define SRU_GENERR		   -269

#define KFP_DIVZERO        -270
#define KFP_LOGNEG         -271
#define KFP_SQRTNEG        -272
#define KFP_PWRNEG         -273

#define KLP_NOLBLDEF       -280
#define KLP_LBLREDEF       -281
#define KLP_BADLABEL       -282
/* 285 through 299 open */

/* 300 through 449 reserved for VME errors */

/* 400 to 454 reserved for S400 compatible errors */
#define ECP_PROTOVER       -455
/* 456 to 495 reserved for S400 compatible errors */
/* 496 through 499 reserved for ethernet communications protocol errors */

/* Error codes to handle the IBUP GPIB return */
#define IB_NOCMD_BYTE_AVAIL           -510  /* ibup error -1 */
#define IB_CAC_CONFLICT               -511  /* ibup error -2 */
#define IB_NOTCAC                     -512  /* ibup error -3 */
#define IB_NOTSAC                     -513  /* ibup error -4 */
#define IB_IFC_ABORT                  -514  /* ibup error -5 */
#define IB_TIMEOUT                    -515  /* ibup error -6 */
#define IB_BAD_FUNC_CODE              -516  /* ibup error -7 */
#define IB_TCT_TIMEOUT                -517  /* ibup error -8 */
#define IB_NOLISTNERS_ON_BUS          -518  /* ibup error -9 */
#define IB_DRIVER_PROBLEM             -519  /* ibup error -17 */
#define IB_BAD_SLOT_NUM               -520  /* ibup error -21 */
#define IB_NOLISTEN_ADDRESS           -521  /* ibup error -22 */
#define IB_NOTALK_ADDRESS             -522  /* ibup error -23 */
#define IB_SFTWR_CONFIG_PROBLEM       -523  /* ibup error -100 */
#define IB_NO_UTIL_FUNC               -524  /* ibup error -20 */
/* here through -529 reserved for instrument bus errors */

#define EEPROM_ERROR        -550
#define HW_EEPROMCHKSM      -550
#define EEPROM_READ_ERROR   -551
#define HW_EEPROMRDERR      -551
#define EEPROM_WRITE_ERROR  -552
#define HW_EEPROMWRERR      -552
#define HW_UNEXPERR         -553
/* here through 599 reserved for hardware errors */

#define INTERNAL_ERR        -601
#define SYS_INTERNAL_ERR    -601
#define MOD_LOAD_ERR        -602
#define SYS_MOD_LOAD_ERR    -602
#define MOD_FMT_ERR         -603
#define SYS_MOD_FMT_ERR     -603
#define MOD_NOTFOUND        -604
#define SYS_MOD_NOTFOUND    -604

#define SPAWN_ERR           -610
#define SYS_SPAWN_ERR       -610
#define NETWORK_ERR         -611
#define SYS_NETWORK_ERR     -611
#define PROTOCOL_ERR        -612
#define SYS_PROTOCOL_ERR    -612
/* here through 639 reserved for system errors */

#define DIAG_FAULT          -640
#define DIAG_FATAL_FAULT    -641
/* here through 649 reserved for diagnostic errors */

#define TAPI_BADCHANNEL     -650
#define TAPI_BADTESTER      -651
#define TAPI_NOTFOUND       -652
#define TAPI_REFUSED        -653
#define TAPI_NOFIBER        -654
#define TAPI_FIBERCON       -655
#define TAPI_CHANLIMIT      -656
#define TAPI_BUFOFLOW       -657

#define TAPI_BADGROUPID     -660
#define TAPI_BADTESTID      -661
#define TAPI_BADLIST        -662
#define TAPI_EXEC_BUSY      -663
#define TAPI_BADUNIT        -664
/* here through 699 reserved for TAPI errors */

/* 700 through 999 open */

#define LAST_ERROR         -999

#define MIN_ERR_INDICATOR   1.0e22

#define INST_OVERRANGE      1.0e22
#define CURRENT_OVERLOAD    2.0e22
#define OSC_DETECT          3.0e22
#define THERMAL_SHUTDOWN    4.0e22
#define SOA_EXCEEDED        5.0e22
#define PWID_TOO_SHORT      6.0e22
#define SOURCE_LIMIT        7.0e22
#define MEAS_NOT_PERFORMED  1.0e23

#endif

