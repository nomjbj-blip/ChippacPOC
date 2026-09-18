/* prb_msg.h */
/*************************************************************************

       COPYRIGHT (C) 1993  by  KEITHLEY INSTRUMENTS, INC.
       Cleveland, Ohio

       This software is furnished under a license and may
       be used and copied only in accordance with the terms
       of such license, and with the inclusion of the above
       COPYRIGHT notice.  This software or any other copies
       thereof may not be provided or otherwise made
       available to any other person.  No title to and
       ownership of the software is hereby transferred.
       The information in this software is subject to
       change without notice, and should not be construed
       as a commitment by KEITHLEY INSTRUMENTS, INC.

       KEITHLEY assumes no responsibility for the use or
       reliability of its software on equipment which is
       not supplied by KEITHLEY.

**************************************************************************

 File:     $Source: /cm/test/build/S600/v420/COMMON/RCS/prb_msg.h,v $
 Current $Revision: 1.11 $
 Current    $State: REL $
 Last Rev    $Date: 2000/02/28 15:29:15 $

 Change       $Log: prb_msg.h,v $
 Change       Revision 1.11  2000/02/28 15:29:15  rybka
 Change       added specific srq error msgs
 Change
 Change       Revision 1.10  1999/09/01 14:05:18  rybka
 Change       added SMIF specific constants
 Change
 Change       Revision 1.9  1999/06/04 18:39:28  rybka
 Change       add error 62 SMIF_LOCK_FAIL
 Change
 Change       Revision 1.8  1998/12/01 20:22:40  rybka
 Change       added errors 60 & 61 set/query chuck temp fail
 Change
 Change       Revision 1.7  1998/09/17 12:46:03  williamson
 Change       updates by davej
 Change
 Change       Revision 1.5  1997/12/02 20:01:08  rybka
 Change       added error offset 59
 Change
 Change       Revision 1.3  1997/05/21 19:54:55  rybka
 Change       added error codes for max slot and cassette failures (read from prbcnfg.dat)
 Change
 Change       Revision 1.2  1997/04/22 13:04:14  williamson
 Change       Moved from S600 projcom area
 Change
 Change       Revision 1.4  1997/02/20 15:56:08  rybka
 Change       added err msg 38 thru 55
 Change
 * Revision 1.3  1997/02/07  22:32:10  rybka
 * added errors 34 - 37
 * .\
 *
 * Revision 1.2  1997/01/13  13:49:54  witzke
 * PR3328 Added offset of 1000 and changed MEM_ALLOC_ERR to PR_MEM_ALLOC_ERR
 *
 * Revision 1.1  1996/10/25  17:20:32  witzke
 * Initial revision
 *
 * Revision 1.2  1993/04/27  13:26:48  witzke
 * checked to match VMS version
 *
 * Revision 1.1  1993/02/06  17:39:50  beecher
 * Initial revision
 *
...............................................................................

 Function: prb_msg.h - Maps symbolic error to error number   	 

.............................................................................*/


#define PR_ERROR_OFFSET	-1000

#define BAD_TST_NUM		PR_ERROR_OFFSET - 1
#define BAD_CONFIG_DAT		PR_ERROR_OFFSET - 2
#define FEAT_NOT_SUPPORT	PR_ERROR_OFFSET - 3	
#define PORT_ASS_FAIL		PR_ERROR_OFFSET - 4
#define SET_UNITS_FAIL		PR_ERROR_OFFSET - 5
#define SET_MODE_FAIL		PR_ERROR_OFFSET - 6
#define CLR_WAF_MAP		PR_ERROR_OFFSET - 7
#define INVAL_MODE		PR_ERROR_OFFSET - 8
#define SET_DIE_FAIL		PR_ERROR_OFFSET - 9
#define SET_PRESET_FAIL		PR_ERROR_OFFSET - 10
#define BAD_MODE		PR_ERROR_OFFSET - 11
#define TST_COMPL_FAIL		PR_ERROR_OFFSET - 12
#define UNINTEL_RESP		PR_ERROR_OFFSET - 13
#define MOVE_FAIL		PR_ERROR_OFFSET - 14
#define UNEXPE_ERROR		PR_ERROR_OFFSET - 15
#define LOAD_ERROR		PR_ERROR_OFFSET - 16
#define BAD_CHUCK		PR_ERROR_OFFSET - 17
#define CHUCK_NOT_UP		PR_ERROR_OFFSET - 18
#define INK_FAIL		PR_ERROR_OFFSET - 19
#define TIME_GONE		PR_ERROR_OFFSET - 20
#define BAD_LEARN_FUNCT		PR_ERROR_OFFSET - 21
#define SET_DIAM_FAIL		PR_ERROR_OFFSET - 22
#define SET_MATRIX_FAIL		PR_ERROR_OFFSET - 23
#define BAD_Z_PARAM		PR_ERROR_OFFSET - 24
#define NO_RESPONSE		PR_ERROR_OFFSET - 25
#define TOO_MANY_NAKS		PR_ERROR_OFFSET - 26
#define INVAL_PARAM		PR_ERROR_OFFSET - 27
#define ERR_LOAD_FILE		PR_ERROR_OFFSET - 28
#define ALIGN_FAIL		PR_ERROR_OFFSET - 29
#define	GPIB_ERROR		PR_ERROR_OFFSET - 30

#define CHK_PORT_ASS		PR_ERROR_OFFSET - 31
#define CHK_DEV_PROT		PR_ERROR_OFFSET - 32
#define PR_MEM_ALLOC_ERR	PR_ERROR_OFFSET - 33
/* added for s600 support mrybka feb-97*/
#define PR_NO_CASSETTE		PR_ERROR_OFFSET - 34
#define SET_FLAT_FAIL		PR_ERROR_OFFSET - 35
#define SET_REF_DIE_FAIL	PR_ERROR_OFFSET - 36
#define Z_PARAM_FAIL		PR_ERROR_OFFSET - 37

#define ERR_OPEN_PRBCNFG	PR_ERROR_OFFSET - 38
#define NO_CNFG_DATA		PR_ERROR_OFFSET - 39
#define NO_PRB_TYPE		PR_ERROR_OFFSET - 40
#define INVAL_PRB_MODE		PR_ERROR_OFFSET - 41 
#define INVAL_PRB_OPTIONS	PR_ERROR_OFFSET - 42 
#define INVAL_DEVICE_NAME	PR_ERROR_OFFSET - 43 
#define INVAL_DEVICE_IRQ	PR_ERROR_OFFSET - 44 
#define INVAL_BAUD_RATE		PR_ERROR_OFFSET - 45 
#define INVAL_TIMEOUT		PR_ERROR_OFFSET - 46 
#define INVAL_GPIB_UNIT		PR_ERROR_OFFSET - 47 
#define INVAL_GPIB_SLOT		PR_ERROR_OFFSET - 48 
#define INVAL_GPIB_ADDRESS	PR_ERROR_OFFSET - 49 
#define INVAL_GPIB_WRITE_MODE	PR_ERROR_OFFSET - 50 
#define INVAL_GPIB_READ_MODE	PR_ERROR_OFFSET - 51 
#define INVAL_GPIB_TERMINATOR	PR_ERROR_OFFSET - 52 
#define INVAL_KULT_PATH		PR_ERROR_OFFSET - 53
#define INVAL_PRB_LIB		PR_ERROR_OFFSET - 54
#define INVAL_PRB_CNFG_FUNC	PR_ERROR_OFFSET - 55
#define INVAL_PRB_MAX_SLOTS	PR_ERROR_OFFSET - 56
#define INVAL_PRB_MAX_CASSETTES	PR_ERROR_OFFSET - 57
/* addded 10-28-97 rybka */
#define SET_QUAD_FAIL		PR_ERROR_OFFSET - 58
#define NO_UNPROBED_WAFERS	PR_ERROR_OFFSET - 59
#define SET_TEMPERATURE_FAIL	PR_ERROR_OFFSET - 60
#define QUERY_TEMPERATURE_FAIL	PR_ERROR_OFFSET - 61
#define SMIF_LOCK_FAIL		PR_ERROR_OFFSET - 62
#define INVALID_ARRAY_SIZE	PR_ERROR_OFFSET - 63
#define NO_SRQ_FOUND 		PR_ERROR_OFFSET - 64
#define UNKNOWN_SRQ_FOUND	PR_ERROR_OFFSET - 65
#define INVALID_SRQ_FILE	PR_ERROR_OFFSET - 66
