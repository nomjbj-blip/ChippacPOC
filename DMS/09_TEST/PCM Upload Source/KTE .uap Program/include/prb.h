/* prb.h */
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

 File:     $Source: /cm/test/build/S600/v420/COMMON/RCS/prb.h,v $
 Current $Revision: 1.34 $
 Current    $State: REL $
 Last Rev    $Date: 2000/09/08 17:19:51 $

 Change       $Log: prb.h,v $
 Change       Revision 1.34  2000/09/08 17:19:51  rybka
 Change       added srq structure (from) PrTSK9.h
 Change
 Change       Revision 1.33  2000/07/11 14:20:48  rybka
 Change       added constants for needle cleaning
 Change
 Change       Revision 1.32  2000/02/28 15:29:15  rybka
 Change       added lot end, and pr wafer reject constants
 Change       to be used by the engine
 Change
 Change       Revision 1.31  1999/09/13 18:33:26  rybka
 Change       corrected smif clamp and unclamp constants
 Change
 Change       Revision 1.28  1999/09/01 14:05:18  rybka
 Change       added SMIF specific constants NO change to prb struct
 Change
 Change       Revision 1.27  1999/06/04 18:39:28  rybka
 Change       added comments to each field
 Change
 Change       Revision 1.25  1999/03/05 14:38:18  rybka
 Change       added new fields for die size
 Change
 Change       Revision 1.24  1998/05/29 19:33:54  rybka
 Change       added two fields (doubles) for die size PS21
 Change       also consts for ref in getPrbStruct function
 Change
 Change       Revision 1.23  1998/03/24 14:07:30  rybka
 Change       added additional comments
 Change
 Change       Revision 1.21  1998/03/17 20:12:08  williamson
 Change       PR 5701  Added include of kdf.h here for TRUE/FALSE stuff...
 Change
 Change       Revision 1.20  1998/02/06 21:32:50  rybka
 Change       added prb fields and const defs for SMIF apps
 Change
 Change       Revision 1.16  1997/12/16 18:17:56  rybka
 Change       moved externs for globals to prb_extern.h NT reasons
 Change
 Change       Revision 1.15  1997/10/27 17:11:13  moore
 Change       NT code cleanup
 Change
 Change       Revision 1.14  1997/09/05 13:50:13  rybka
 Change       changed the DEFAULT_GPIB_TERMINATOR from 'x' to 10 , linefeed
 Change
 Change       Revision 1.13  1997/08/28 17:51:55  aujla
 Change       Updated with the NT changes.
 Change
 Change       Revision 1.12  1997/08/22 15:05:41  moore
 Change       PR04798 - Changed prober.log location from KIDAT to KILOG
 Change
 Change       Revision 1.11  1997/08/08 16:46:02  rybka
 Change       added a short timeout field
 Change
 Change       Revision 1.10  1997/05/21 19:53:46  rybka
 Change       added fields to prb struc to hold max slot and max cassette
 Change
 Change       Revision 1.9  1997/04/22 12:53:14  williamson
 Change       Moved here from S600 PROJCOM area
 Change
 Change       Revision 1.2  1997/02/07 22:29:35  rybka
 Change       added 5 new fields to prb struct
 Change
 * Revision 1.1  1996/11/04  18:03:01  witzke
 * Initial revision
 *
 * Revision 1.8  1996/05/08  15:15:41  witzke
 * PR1293 updated for kte30 changed MAXFILENAMESIZE
 *
 * Revision 1.6  1996/01/23  21:28:51  witzke
 * PR1447 Corrected PR_MODe_aaaa values to match documentation
 *
 * Revision 1.5  1993/06/30  13:38:15  witzke
 * added gpib to Prb struct
 *
 * Revision 1.4  1993/04/26  19:22:53  witzke
 * Changed COMPLETE definitions
 *
 * Revision 1.3  1993/04/19  15:23:39  witzke
 * Added irq to prb structure
 *
 * Revision 1.2  1993/03/05  23:03:18  witzke
 * Updated file usage
 *
 * Revision 1.1  1993/02/06  17:39:47  beecher
 * Initial revision
 *
...............................................................................

 Function: prb.h - this file defines data structures and variables that 
	are general for all prober
	 
.............................................................................*/


/*

C!
C   THIS was PROBSTRUCT.HDR now PRB.H
C
C   IT CONTAINS THE PROBER STRUCTURE IN WHICH STATUS IS STORED
C
C   EDIT HISTORY:
C       Dark Ages           MAD Created.
C       Enlightened Times (21-MAR-91)   DWR Added new fields to
C                           structure; recall buffs
		Brave New World  (1-Oct-92) SAW C flavour

*/


/* PR 5701  Added include of kdf.h so that TRUE/FALSE can be defined
 */
#ifndef KDF_VERSION
#include <kdf.h>
#endif

#if 0
/* Don't need this anymore...
 */
#ifndef TRUE
#define TRUE 1
#endif
#ifndef FALSE
#define FALSE 0
#endif
#endif

#define MAX_OPTIONS_SIZE    32
#define TTY_DEV_NAME_LEN    30

#define PRB_LIBNAME_LEN		10
#define PRB_CNFGNAME_LEN	20

#ifndef MAXFILENAMESIZE
#define MAXFILENAMESIZE     256
#endif

#define PROBER_ERROR_OFFSET -1500 /* used for PrError */

typedef struct 
{
	int chuck_position; /*Current Chuck Position: init */
	int x_position; /*present x location: init, mv, setrefdie  */
	int y_position; /* present y location: init, mv, setrefdie  */
	int psxl;       /* present x subsite location: init  */
	int psyl;       /* present y subsite location: init  */
	int mode;       /* present probing mode: init, setmode  */
	int probtype;   /* prober type */
	char probname[PRB_LIBNAME_LEN];  /* prober's name (replaces type) */
	int subprobtype;    /* prober sub-type: init  */
	int ready;      /* ready to probe?: init  */
	int movestatus; /* Move Successful?: init  */
	int x_index;    /* number of unit size steps in x index: init  */
	int y_index;    /* number of unit size steps in y index: init  */
	int x_initial_location;     /* initial x coordinate: init, setrefdie  */
	int y_initial_location;     /* initial y coordinate: init, setrefdie  */
	int x_move_step;    /* number of ticks in x step: init  */
	int y_move_step;    /* number of ticks in y step: init  */
	int units;      /* prober units: init, setunits  */
	int number;     /* prober number (spelled funny) */
	int error_level;    /* current error reporting level */
	char    error_log_name[MAXFILENAMESIZE];    /* error log file name */
	FILE    *error_log_fp;  /* file pointer for error log file */
	FILE    *trans_log_fp;  /* file pointer for transaction log file */
	FILE    *fake_log_fp;  /* file pointer for FAKE debug only */
	char    trans_log_name[MAXFILENAMESIZE];    /* transaction log file name */
	char    fake_log_name[MAXFILENAMESIZE];    /* fake log file name */
	int trans_log_enabled;  /* Transaction Log Flag */
	int fake_log_enabled;  /* fake Log Flag */
	int file_des;   /* file descriptor */
	int io_mode;    /* ex: SERIAL GPIB */
	char    RS232_device[TTY_DEV_NAME_LEN];
	char    baudrate[TTY_DEV_NAME_LEN];
	int timeout;    /* I/O timeout value, settime */
	int device_irq;     /* i/o device interrupt */
	int gpib_unit;		/* converter box unix number */
	int gpib_slot;		/* gpib slot number */
	int gpib_address;	/* gpib address */
	int gpib_writemode;	/* gpib writemode */
	int gpib_readmode;	/* gpib readmode */
	int gpib_terminator; 	/* gpib terminator */
	char s_prb_options[MAX_OPTIONS_SIZE]; /* prober options:check options */
	double d_cur_pos_x;	/* current x subsite accum. rel move: init, relmv, relret  */
	double d_cur_pos_y;	/* current y subsite accum. rel move: init, relmv,relret  */
	double d_abs_site_x;	/* current y site location abs move: init, absmv  */
	double d_abs_site_y;	/* current y site location abs move: init, absmv  */
	int i_prb_max_slots;	/* max slots in a cassette from prbcnfg*/
	int i_prb_max_cassettes;  /* max cassettes on a prober from prbcnfg*/
	int short_timeout;    /* I/O timeout value */
	int smif_lock_status; /* status of the lock on the pod: init  */
	int smif_clamp_status; /* status of the clamp on the pod: init  */
	int smif_cassette_status; /* status of the cassette on the pod: init, prbstat  */
	int smif_machine_status; /* status of the machine: init, prbstat  */
	int smif_sense_wafer; /* sense wafer(s) in cassette: init, sense wafer  */
	int smif_stop_resume; /* probing stop(ped)/resume(d): init  */
	double d_die_size_x;	/* x die size real # of mm/mils: init, setdiesz   */
	double d_die_size_y;	/* y die size real # of mm/mils: init,setdiesz   */
	double d_init_coord_x;	/* initial X machine coord: init   */
	double d_init_coord_y;	/* initial Y machine coord: init    */

} prb_t ;
typedef struct
{
	char    name[8];
	int id;
} prb_ni_t;     /* this is a prober name to prober id number structure */

/******

******/
#define MAXSRQ 100
struct pr_known_srq_list
  {
   	int good_srq[MAXSRQ];
   	int bad_srq[MAXSRQ];
   	int err_code[MAXSRQ];
   	char c_func_name[32];
  } typedef pr_srq_list;




/**************************************************************************
 16-dec-97 rybka
 The following function prototypes were movedform prb_extern. to prb_drvadr.h.
 The NT development required this because of issues with IMPORT/EXPORT.
		getdrvadr, putdrvadr, getdsptab

 Also the externs for global variables were moved from prb.h to prb_extern.h
*****************************************************************************/

#define BUFFER_SIZE 1024    /* maximum I/O buffer length to and from prober */

 
#define XTester 1       /*  AUDIT FILE Tester comm. flag */
#define XProber 2       /*  AUDIT File Prober comm. flag */
 
#ifdef _MSDOS
#define MAXPROBERS  1
#else
#define MAXPROBERS  4
#endif

#define MAXPROBERFUNCTIONS  80
#define MINPROBERS  1
#define MINPROBERFUNCTIONS  1
#define TOTALPROBERS    ((MAXPROBERS)-(MINPROBERS))+1

/*
	MAXPROBERS = maximum numnber of probers (length of device defines)
		For ALL indexes into "MAXDEV" tables, index them by:
				for (i=0; i< MAXDEV; i++)
		Declare arrays, ARRAY[MAXDEV]
		All elements are from 0 to (MAXDEV-1) {or 1 to MAXDEV}.
*/

#define MAXCNFGSTR  50  /* max number of config items in file, no comments */
#define CNFGLINESIZE    80  /* buffer for config item lines */

#define MAXERRMESSAGESIZE   255

#define KIPRBERR_PATH_LOGICAL "KIDAT"
	
#ifdef _MSDOS
#define DEFAULT_KIPRBERR_PATH   "C:\\S900\\"    
#else
#define DEFAULT_KIPRBERR_PATH   ""  
#endif

#define DEFAULT_KIPRBERR_FILENAME   "prbmsg.msg" 
	/* same format as KILPTERRFILE */

/* environment variable that specifies the prober error log filename */
#define ERR_LOG_LOGICAL     "KI_PRB_ERROR_LOG"
#define ERR_LOG_PATH_LOGICAL    "KILOG" 

#ifdef _MSDOS
#define DEFAULT_ERR_LOG_PATH    "C:\\S900\\"
#else
#define DEFAULT_ERR_LOG_PATH    ""
#endif

#define DEFAULT_ERR_LOG_FILENAME    "prb_errs.log"

/* environment variable that specifies the transaction log filename */
#define AUDIT_LOG_LOGICAL   "KI_PRB_AUDIT_LOG"
#define AUDIT_LOG_PATH_LOGICAL  "KILOG"

#ifdef _MSDOS
#define DEFAULT_AUDIT_LOG_PATH "C:\\S900\\"
#else
#define DEFAULT_AUDIT_LOG_PATH ""
#endif

#define DEFAULT_AUDIT_LOG_FILENAME "prober.log"

/* environment variable that specifies the configuration filename */
#define CONFIG_LOGICAL  "KI_PRB_CONFIG"
#define CONFIG_PATH_LOGICAL "KIDAT"

#ifdef _MSDOS
#define DEFAULT_CONFIG_PATH "C:\\S900\\"
#else
#define DEFAULT_CONFIG_PATH ""
#endif

#define DEFAULT_CONFIG_FILENAME "prbcnfg.dat"


/* environment variable that specifies the error message filename */
#define ERRMESSAGE_LOGICAL  "KI_PRB_ERRM"
#define ERRMESSAGE_PATH_LOGICAL "KIDAT"

#ifdef _MSDOS
#define DEFAULT_ERRMESSAGE_PATH "C:\\S900\\"
#else
#define DEFAULT_ERRMESSAGE_PATH ""
#endif

#define DEFAULT_ERRMESSAGE_FILENAME "prb_errm.dat"

/* environment variable that specifies the error level */
#define ERRLEVEL_LOGICAL    "KI_PRB_ERROR_LEVEL"

/* environment variable that specifies the debug type */
#define DEBUG_LOGICAL   "KI_PRB_DEBUG"

/******************************************************************************** 
environment variable that specifies the debug type for FAKE prober driver only.

RULES:
	if the environment variable is NOT defined
		output to stdout
 	if the environment variable is defined but to nothing (NULL)
		output NOTHING
	if the environment variable is defined to a file path
		output to the file path provided (open with create append

********************************************************************************/
#define FAKE_DEBUG_LOGICAL   "KI_PRB_FAKE_OUTPUT"

#define DEFAULT_DEVICE_IRQ  0
#define DEFAULT_SERIAL_TIMEOUT 120
#define DEFAULT_SHORT_TIMEOUT 5

/* GPIB configuration default */
#define DEFAULT_GPIB_UNIT	0
#define DEFAULT_GPIB_SLOT	0
#define DEFAULT_GPIB_ADDRESS	0
#define DEFAULT_GPIB_WRITEMODE	0
#define DEFAULT_GPIB_READMODE	2
#define DEFAULT_GPIB_TERMINATOR	10 /* linefeed */
#define DEFAULT_GPIB_TIMEOUT	60

#define NO  0
#define YES 1
#define NOTCFIG -1
#define PR_OK  1

#define MALLOC_FAIL 0


#define SERIAL  232
#define GPIB    488
#define GPIBCT	256

#define PR_CHUCK_DOWN   0
#define PR_CHUCK_UP 1

#define PR_NOT_READY    0
#define PR_READY    1

#define PR_MODE_MANUAL  1
#define PR_MODE_EXTERNAL    2
#define PR_MODE_EDGE    3
#define PR_MODE_MATRIX  4
#define PR_MODE_CIRCULAR    5
#define PR_MODE_AUTO    6
#define PR_MODE_LEARN    PR_MODE_AUTO

#define PR_MOVECOMPLETE 2
#define PR_WAFERCOMPLETE  4
#define PR_CASSETTECOMPLETE    8
#define PR_LOTEND 10
#define PR_WAFER_REJECT_LOAD 12    /* if prober needs to have next wafer loaded explicitly*/
#define PR_WAFER_REJECT_NOLOAD 14   /* if prober DOES NOT need to have next wafer loaded explicitly*/

#define PR_ENGLISH	0
#define	PR_METRIC	1

#define MAXSLOTCOUNT	25

/* Defines for PrSetSlotStatus() */
#define PR_SLOT_SKIP	1
#define PR_SLOT_PROBED	2
#define PR_SLOT_UNPROBED	3

/* Returns from PrCassetteMap() */
#define PR_SLOT_STATUS_UNMAPPED	1
#define PR_SLOT_STATUS_INPROCESS	2
#define PR_SLOT_STATUS_EMPTY	3
#define PR_SLOT_STATUS_UNPROBED	4
#define PR_SLOT_STATUS_PROBED	5
#define PR_SLOT_STATUS_PROBLEM	6
#define PR_SLOT_STATUS_UNSCHEDULED	7
#define DEFAULT_SLOT_STATUS		""  /* Null */

/* PrZTravel mode operation parameters */
#define PR_Z_TRAVEL_LIMITS		0
#define PR_Z_TRAVEL_EDGE		1
#define PR_Z_TRAVEL_PROFILE		2

/* PrZParams operation parameter function inputs */
#define PR_Z_OVERTRAVEL		1
#define PR_Z_CLEARANCE		2
#define PR_Z_UP_LIMIT		3
#define PR_Z_DOWN_LIMIT		4
#define PR_Z_ALIGN_HEIGHT	5

/* PrPutNxtSlot and PrPutWafer Reason codes */
#define PR_NORMAL_UNLOAD	0
#define PR_PROFILE_FAIL		3
#define PR_ALIGN_FAIL		4

/* PrSetPipeLine input params */
#define PR_DISABLE_PIPELINE	0
#define PR_ENABLE_PIPELINE	1

/* prober quadrant numbers (fron the probe pins perspective) */
#define PR_FIRST_QUADRANT 	1
#define PR_SECOND_QUADRANT 	2
#define PR_THIRD_QUADRANT 	3
#define PR_FORTH_QUADRANT 	4

/*The Dr. J SMIF Section */
/* lock and lock status */
#define PR_SMIF_UNLOCK 1
#define PR_SMIF_LOCK 0
#define PR_SMIF_UNCLAMP 0
#define PR_SMIF_CLAMP 1
/* machine status */
#define PR_SMIF_WAITING		'I'
#define PR_SMIF_CARD_REPLACE	'C'
#define PR_SMIF_LOT_PROCESS	'R'
#define PR_SMIF_ERROR_STATE	'E'
#define PR_SMIF_UNKNOWN		'X'
    /* cassette status */
#define PR_SMIF_TESTING		'W'
#define PR_SMIF_NOT_TESTING	' '
/* smif sense wafer */
#define PR_SMIF_NOT_SENSE_WAFER 0
#define PR_SMIF_SENSE_WAFER	1
/* smif stop probing */
#define PR_SMIF_STOP_PROBING	0
#define PR_SMIF_RESUME_PROBING	1

/****************************************************************************
Now introducing the EG4090u.... and how EG impliments SMIF
*****************************************************************************/
#define MAX_SMIF_STATUS_ITEMS 6
#define SMIF_NORMAL_OPER_MODE 0
#define SMIF_GEM_OPER_MODE 1
#define SMIF_EXTRERNAL_OPER_MODE 2
#define SMIF_SORTLINK_OPER_MODE 3
#define SMIF_LATCH_STATUS_UNKNOWN 0
#define SMIF_LATCH_STATUS_UNLATCH 1
#define SMIF_LATCH_STATUS_LATCH 2
#define SMIF_CASSETTE_HOME 1
#define SMIF_CASSETTE_PRESENT 1
#define SMIF_CLAMP_STATUS_UNCLAMPED 0
#define SMIF_CLAMP_STATUS_CLAMPED 1

/*Offsets into the smif status array
  returned by PrSmifStatus     */
#define SMIF_OPER_MODE_OFFSET 1
#define SMIF_LATCH_STATUS_OFFSET 2
#define SMIF_CASS_HOME_OFFSET 3
#define SMIF_POD_PRESENT_OFFSET 4
#define SMIF_CLAMP_STATUS_OFFSET 5

/* Probe needle cleaning types:
brush, pad (fiber), ceramic, metal (sandpaper) sticky, blow....
*/
#define PR_CLEAN_NONE		0 
#define PR_CLEAN_BRUSH		1 
#define PR_CLEAN_FIBER		2
#define PR_CLEAN_CERAMIC	3
#define PR_CLEAN_METAL		4
#define PR_CLEAN_STICKY		5
#define PR_CLEAN_BLOW		6

/*
The following #defines are used to allow a user to get a copy of the 
data in the Prb structure for the current teststation.  The naming convension is the field name and the 
CONST name are the save (only CAPS versus lower case).  The function used 
to get the data is in the prbcom library and is called getPrbStruct.  
GetPrbStruct accepts 2 parameters: 1) int data type (use the following CONSTs
2) a pointer to a long.  The user will need to know the data type of the desired
field.  Cast the address of the users local data to a long and send it.

If an invalid number is supplied for the first paramerter, the getprbStruct will return NULL for the second parameter.
*/

#define CHUCK_POSITION		1
#define X_POSITION		2
#define Y_POSITION		3
#define PSXL			4
#define PSYL			5 
#define MODE			6
#define PROBTYPE		7
#define PROBNAME		8
#define SUBPROBTYPE		9
#define READY			10
#define MOVESTATUS		11
#define X_INDEX			12
#define Y_INDEX			13
#define X_INITIAL_LOCATION	14
#define Y_INITIAL_LOCATION	15
#define X_MOVE_STEP		16
#define Y_MOVE_STEP		17
#define UNITS			18
#define NUMBER			19
#define ERROR_LEVEL		20
#define ERROR_LOG_NAME		21
#define ERROR_LOG_FP		22
#define TRANS_LOG_FP		23
#define FAKE_LOG_FP		24
#define TRANS_LOG_NAME		25
#define FAKE_LOG_NAME		26
#define TRANS_LOG_ENABLED	27
#define FAKE_LOG_ENABLED	28
#define FILE_DES		29
#define IO_MODE			30
#define RS232_DEVICE		31
#define BAUDRATE		32
#define TIMEOUT			33
#define DEVICE_IRQ		34
#define GPIB_UNIT 		35
#define GPIB_SLOT		36
#define GPIB_ADDRESS		37
#define GPIB_WRITEMODE		38 
#define GPIB_READMODE		39
#define GPIB_TERMINATOR		40
#define S_PRB_OPTIONS		41
#define D_CUR_POS_X		42
#define D_CUR_POS_Y		43
#define D_ABS_SITE_X		44
#define D_ABS_SITE_Y		45
#define I_PRB_MAX_SLOTS		46
#define I_PRB_MAX_CASSETTES	47
#define SHORT_TIMEOUT		48
#define SMIF_LOCK_STATUS	49
#define SMIF_CLAMP_STATUS	50
#define SMIF_CASSETTE_STATUS	51
#define SMIF_MACHINE_STATUS	52
#define SMIF_SENSE_WAFER	53
#define SMIF_STOP_RESUME	54
#define D_DIE_SIZE_X		55
#define D_DIE_SIZE_Y		56
#define D_INIT_COORD_X		57
#define D_INIT_COORD_Y		58
