/**************************************************************************
 *
 *       COPYRIGHT (C) 1998  by  KEITHLEY INSTRUMENTS, INC.
 *       Cleveland, Ohio
 *
 *       This software is furnished under a license and may
 *       be used and copied only in accordance with the terms
 *       of such license, and with the inclusion of the above
 *       COPYRIGHT notice.  This software or any other copies
 *       thereof may not be provided or otherwise made
 *       available to any other person.  No title to and
 *       ownership of the software is hereby transferred.
 *       The information in this software is subject to
 *       change without notice, and should not be construed
 *       as a commitment by KEITHLEY INSTRUMENTS, INC.
 *
 *       KEITHLEY assumes no responsibility for the use or
 *       reliability of its software on equipment which is
 *       not supplied by KEITHLEY.
 *
 **************************************************************************
 File:     $Source: /cm/test/build/S600/v420/COMMON/RCS/ksox_def.h,v $
 Current $Revision: 1.33 $
 Curent     $State: REL $
 Last Rev    $Date: 2000/09/11 17:17:49 $

 * $Log: ksox_def.h,v $
 * Revision 1.33  2000/09/11 17:17:49  williamson
 * PR13343  added define for datapool id length
 *
 * Revision 1.32  1999/06/03 12:02:28  williamson
 * PR 9571 Added MAX_MACRO_LINE_LEN define for error checking
 *
 * Revision 1.31  1999/03/18 18:37:09  williamson
 * Removed min/max macro definitions
 *
 * Revision 1.30  1999/01/13 16:27:02  jlilie
 * Remove #ifdef MSDOS stuff!
 *
 * Revision 1.29  1998/06/29 17:27:25  williamson
 * PR 3783  Added defines for lib locking
 *
 * Revision 1.28  1998/04/27 17:14:03  williamson
 * Added parameter set support and changed sizes of defines
 *
 * Revision 1.27  1998/03/05 15:44:12  williamson
 * PR 6061 again.. forgot parm_io_type while merging...
 *
 * Revision 1.26  1998/03/05 14:14:18  williamson
 * PR 6061, 6074  Added TSF and Parameter Range checking support
 *
 Change       $Log: ksox_def.h,v $
 Change       Revision 1.33  2000/09/11 17:17:49  williamson
 Change       PR13343  added define for datapool id length
 Change
 Change       Revision 1.32  1999/06/03 12:02:28  williamson
 Change       PR 9571 Added MAX_MACRO_LINE_LEN define for error checking
 Change
 Change       Revision 1.31  1999/03/18 18:37:09  williamson
 Change       Removed min/max macro definitions
 Change
 Change       Revision 1.30  1999/01/13 16:27:02  jlilie
 Change       Remove #ifdef MSDOS stuff!
 Change
 Change       Revision 1.29  1998/06/29 17:27:25  williamson
 Change       PR 3783  Added defines for lib locking
 Change
 Change       Revision 1.28  1998/04/27 17:14:03  williamson
 Change       Added parameter set support and changed sizes of defines
 Change
 Change       Revision 1.27  1998/03/05 15:44:12  williamson
 Change       PR 6061 again.. forgot parm_io_type while merging...
 Change
 Change       Revision 1.26  1998/03/05 14:14:18  williamson
 Change       PR 6061, 6074  Added TSF and Parameter Range checking support
 Change
 Change       Revision 1.25  1998/02/25 15:04:01  witzke
 Change       PR6343 Corrected problem with conpin parameter entry,
 Change       now 32 parameters are shown.
 Change
 Change       Revision 1.24  1997/10/27 17:12:10  moore
 Change       NT code cleanup
 Change
 Change       Revision 1.23  1997/09/04 12:58:42  moore
 Change       PR05121 - Added support for default parameter values in KITT
 Change
 Change       Revision 1.22  1997/08/15 21:29:16  meffinge
 Change       added a variable to store the time a library
 Change       is opened in the usrlib_t structure.
 Change
 Change       Revision 1.21  1997/04/15 18:16:18  jain
 Change       PR4151 added the second parameter "status" to the GetReusableParameter.
 Change
 Change       Revision 1.20  1997/04/04 20:34:14  jain
 Change       changes for kte3.2 for s400
 Change
 Change       Revision 1.19  1997/03/18 20:01:58  williamson
 Change       PR 2790 Added defines for syntax only mode of execution
 Change
 Change       Revision 1.18  1997/02/17 15:22:08  jain
 Change       PR2980 implemented wrappers to handle ibup functions.
 Change
 Change       Revision 1.17  1997/01/08 21:44:25  jain
 Change       PR507 changes for removing the 32 parameter limit.
 Change
 * Revision 1.16  1996/12/04  14:53:21  williamson
 * Adding HASH functionality...look for ljl comments for changes
 *
 * Revision 1.15  1996/11/19  15:03:42  jain
 * PR2672 made minor changes for the s600.
 *
 * Revision 1.14  1996/08/08  18:57:06  jain
 * added defines for EXECKTXE_KTM, EXECKTXE_COMMAND, EXECKTXE_KTM_wRESULTS, used by KTXEKTXESUP and KSOX
 *
 * Revision 1.13  1996/07/26  15:49:26  jain
 * moved the #define for MAX_KTM_SIZE from KITT and KSOX to here.
 *
 * Revision 1.12  1996/06/20  19:24:14  jain
 * PR1875 increased the MAX_LOADABLE_FILE_SIZE from 8k to 50k.  This is done because when a big
 * ktm is loaded into memory 8k is not enough.  50k should be more than enough.
 *
 * Revision 1.11  1996/05/08  15:09:41  witzke
 * PR1293 updated for kte30 added datapool data types
 * definitions and dp prototypes
 *
 * Revision 1.10  1995/11/03  19:04:09  tufte
 * pr1123  added two fields to the constants_info_t struct
 * to allow for passing dato from and into PDIs
 *
 * Revision 1.9  1995/08/22  12:33:33  tufte
 * pr648 array support
 *
 * Revision 1.8  1995/08/10  13:37:45  tufte
 * pr520 etc -- new support for arrays and doubles
 *
 * Revision 1.7  1995/07/18  14:30:13  tufte
 * pr00549 -- added defines for USER_OFF and USER_ON
 *
 * Revision 1.6  1995/07/11  22:03:12  tufte
 * PR00549 added user field to select_info_t struct
 *
 * Revision 1.5  1995/06/22  19:00:04  jain
 * added a #define for PRACTICE_TASK
 *
 * Revision 1.4  1995/02/28  22:26:57  szanto
 * SPR95U070: Added global int flag to constants type
 *
 * Revision 1.3  1994/08/04  13:36:01  szanto
 * Moved items into COM_usrlib.h so they would exist in only one place.  See Version 1.1 of
 * COM_usrlib.h in COMMON for details.
 *
 * Revision 1.2  1994/07/11  13:19:39  williams
 * PC Port (94u138): added #define strdup _strdup for MSDOS only
 *
 * Revision 1.1  1994/06/17  17:29:22  szanto
 * Initial revision
 *
 */

#ifndef _KSOX_DEF_H
#define _KSOX_DEF_H

#include <COM_usrlib.h>
#include <time.h>
#include <ts.h>
#include <ktxe_types.h>

#define CMP_EQUAL   0

#define MAXFUNCS    255 /* max # of functions (aka "commands") */

#define SWEEP_ARRDIM    8000    /* max num values in a sweep command */

#define MAX_PARM_LEN    512 /* the max # of chars in a single parm */
#define PARMARR_MAX_ELEMS   256 /* max elements in an array parm */
#define MAX_LOG_FILE_SIZE   8192    /* max bytes in a file view win */
#define DEFAULT_PROBER_ERRLEVEL 3   /* error level to initially select */
#define NUM_BOXES_FOR_VARARG_CMDS MAXPARMS /* # parm boxes for conpin etc. */
#define MAXFILENAME 256

#define SMALLSTR    32
#define BIGSTR      256
#define GIANTSTR    2048
#define SUPERGIANTSTR   8192
#define INS_SYM_STRLEN  32      /* 16 would be acceptable */
#define MAX_SINGLE_RESULT_LEN   512 /* array results can be long */
#define MAX_SINGLE_DESC_LEN 8192

#define MAX_MACRO_LINE_LEN  4096

#define TS_HASH 43
#define PS_HASH 43

#define MAX_LOADABLE_FILE_SIZE  51200    /* uses a lot of auto storage */

#define MAX_KTM_SIZE 51200
#define DP_NAME_LEN 64


/*
 *  The select_info_t is used to store selection settings while tests are
 *  being run.  The results settings are stored here because the results
 *  are cleared before each execution
 */

typedef struct _select_info {
	char *result_name;  /* "tag"; could be anything */
	int log;
	int plot;
	int user;
	struct _select_info *nextp; /* link to next element in list */
	struct _select_info *nexth;	/* hash link, ljl */
} select_info_t;

/*
 *  The in_array_info_t is used to store information about static arrays
 *  used in functions like asweepi, asweepx and adelay
 */
 
typedef struct _in_array_info {
	char    *array_name;
	int numpoints;
#ifdef LPTLIB600
	double	*arrayp;
#else
	float   *arrayp;
#endif
	struct  _in_array_info  *nextp;
} in_array_info_t;

typedef struct _constants_info {
	char    *constant_name;
	int     type;
	int		global;
	int		size;
	int		runflag;
	void    *valuep,*wval;
        struct  _constants_info *nextp;
  	struct  _constants_info *nexth;   /* hash  ljl */
} constants_info_t;


/* The _tech_info struct holds the information from the TDF that will
   be substituted in the real test */

typedef struct _ts_info  {
	char   *ts_name;
	int    type;
	int    size;
	void   *valuep;
	enum where_type where;
	struct _ts_info *nextp;
	struct _ts_info *nexth;			
}  ts_info_t;

typedef struct _parameter {
    char *parm_name;
    int parm_type;
    int parm_info;
    int parm_io_type ;
    char *default_value;
    char *range_low;
    char *range_high;
  
} parameter_t;

typedef struct _oldstyle_cmd_table {
	char *cmd_name;
	int (*cmd_function_ptr)(callinfo_t *callinfo);  /* ptr to wrap fn */
	int flags;      /* enabled? (probers.c); useful_retval? */
	int num_parms;      /* if >= 0, exact # parms; see cmd_disp.c */
	parameter_t parms[MAXPARMS];    /* parameter names */
} oldstyle_cmd_table_t;

typedef struct _cmd_table {
	char *cmd_name;
	int (*cmd_function_ptr)(callinfo_t *callinfo);  /* ptr to wrap fn */
	int flags;      /* enabled? (probers.c); useful_retval? */
	int num_parms;      /* if >= 0, exact # parms; see cmd_disp.c */
	/* parameter_t parms[MAXPARMS]; */ /* Allocated dynamically now */
	parameter_t *parms;    /* parameter names */
} cmd_table_t;

union  _dpValue {
	int	int_val;
	float	float_val;
	long	long_val;
	double	double_val;
	char	char_val;
	void	*valuep;
};

typedef struct _data_pool {
    char data_name[ DP_NAME_LEN ];
    int type;
    union _dpValue dpValue;
    int elements;
    struct _data_pool *nextp;
    struct _data_pool *nexth;	/* ljl */
} data_pool_t;


/* HASHing structure */
struct keywrd {	/* predefined symbols for translate_all() ljl */
	char *kw_name;			/* original name */
	char *kw_val;			/* constant integer as text */
	struct keywrd *kw_nexth;	/* hash link */
};


#ifdef USRLIB

/*
 *  The usrlib_cmds_t is a USRLIB version of the cmd_table_t.  Each node 
 *  has a pointer to the appropriate cmd_table_t information for the
 *  parameter window.  Every node also has a pointer to the usrlib wrapper
 *  function loaded from that user library
 */

typedef struct _usrlib_cmds
{
	cmd_table_t *cmd;
	struct  _usrlib_cmds *nextp;
} usrlib_cmds_t;




/*
 *  The usrlib_t struct is a linked list of libraries to support multiple
 *  user libraries.  It holds the head and tail of a linked list of 
 *  usrlib_cmds_t that holds the modules of the library, as well as the
 *  library name and a handle to the actual usrlib loaded by the dynamic linker
 */
typedef struct  _usrlib
{
    usrlib_cmds_t   *usrlib_cmds_head;
    usrlib_cmds_t   *usrlib_cmds_tail;
    char            *libname;
    time_t          openTime;
    void            *usrlib;
    psf_mod_t       *psfModList ;   /* param set data structure for library */
    struct  _usrlib *nextp;
} usrlib_t;


#endif /* USRLIB */

struct cmd_hash {			/* usrlib cmd list ljl */
	struct cmd_hash *ch_nexth;	/* hash link */
	cmd_table_t	*ch_cmd;	/* command table item */
	int		ch_num;		/* original table id, or -1 for user */
#ifdef USRLIB
	usrlib_t	*ch_lib;	/* user's library */
	usrlib_cmds_t	*ch_libcmd;	/* library command list */
#else
	void		*ch_lib;	/* size alignment precaution */
	void		*ch_libcmd;	/* size alignment precaution */
#endif
};

/* possible values for parm_type -- See COM_usrlib.h */


/* used for the global arrays */
typedef struct _array {
	void *array;
	int size, type;
} array_t;


/* possible values for flags (these must be powers of two so that they
 * can be ORed together)
 */
#define ENABLED_FLAG        1
#define USEFUL_RETVAL_FLAG  2

/* abbreviations */
#define EN  ENABLED_FLAG

#define ENRV 3

#if 0
#ifdef PRBLIB
extern cmd_table_t prober_cmds[];
#endif

#if defined LPTLIB || defined TCLIB
extern cmd_table_t lpt_cmds[];
#endif

#ifdef PARLIB
extern cmd_table_t parlib_cmds[];
#endif
#endif


/*
 *  Plot flags for selection window/list, KTM file, etc.
 */

#define PLOT_OFF    0
#define PLOT_X      1
#define PLOT_Y      2
#define	PLOT_MAKE_X	3
#define	PLOT_MAKE_Y	4

#define USER_OFF	0
#define USER_ON		1

#define Tolower(x) (isupper(x) ? tolower(x) : (x))

#define NORMAL_EXECUTION    0
#define SAVEAS_C            1
#define SAVEAS_KTM          2
#define SAVE_KTM            3
#define INTERNAL_SAVE       4
#define PRACTICE_TASK	    5




/* Library locking defines
 */
#define NO_LOCKS_SET   1
#define READ_LOCK_SET  2
#define WRITE_LOCK_SET 3



#endif /*! _KSOX_DEF_H*/

/***** Functions available for the data pool *****/
int		dpAddData (char *name, int type, ...); 
int		dpAddPointer(char *name, int type, void *valuep);
int		dpAddArray(char *name, int type, void *valuep, int elements);
data_pool_t	*dpGetDataNode(char *name, int type);
void		*dpGetDataPtr(char *name, int type);
void		*dpGetPointer(char *name, int type);
void		*dpGetArrayElement(char *arrname, int type, int element);
void		dpRemoveData(char *name, int type);
void		dpRemoveAllData(void);
void		dpPrintDataNode (data_pool_t *data);
void		dpPrintData(char *name, int type);
void 		dpPrintAllData(void);

#define dpDATA_TYPE 1
#define dpPTR_TYPE 2

#define	EXECKTXE_KTM	        1
#define	EXECKTXE_COMMAND        2
#define	EXECKTXE_KTM_wRESULTS   3

#define	EXECKTXE_KTM_SYNTAX            4
#define	EXECKTXE_COMMAND_SYNTAX        5
#define	EXECKTXE_KTM_wRESULTS_SYNTAX   6


/* ibup */
#define IBUP_WRITE	0
#define IBUP_READ	1
#define IBUP_CLEAR	2
#define IBUP_TRIG	3
#define IBUP_REMOTE	4
#define IBUP_LOCAL	5
#define IBUP_SRPOLL	6
#define IBUP_DEFINE	9
#define IBUP_FINISH	10
#define IBUP_SRQWAIT	11

/*GetReusableParm status */
#define PARM_EXIST	1
#define PARM_NOTEXIST	-1
