/* kdf.h */
/*************************************************************************

       COPYRIGHT (C) 1996  by  KEITHLEY INSTRUMENTS, INC.
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

 File:     $Source: /cm/test/build/S600/v420/COMMON/RCS/kdf.h,v $
 Current $Revision: 1.40 $
 Current    $State: REL $
 Last Rev    $Date: 2000/08/09 13:07:14 $

 * $Log: kdf.h,v $
 * Revision 1.40  2000/08/09 13:07:14  williamson
 * PR13102 bumped up version for LOT files
 * since we support user tags now...
 *
 * Revision 1.39  1999/11/10 18:15:07  williamson
 * PR 11038  Added usr tag support
 *
 Change       Revision 1.38  1999/04/23 15:13:49  williamson
 Change       AIX port
 Change
 Change       Revision 1.37  1998/08/28 13:49:47  witzke
 Change       PR4722 converted row and col to float for multi-project support
 Change
 Change       Revision 1.36  1998/07/08 18:05:27  jlilie
 Change       NT-add proto for kdf_fflush().
 Change
 Change       Revision 1.35  1998/07/08 15:48:19  williamson
 Change       Changed FILE_LENGTH_MAX to 256 from 31
 Change
 Change       Revision 1.34  1998/06/09 19:34:31  aujla
 Change       made some changes to certain defines for the NT side.
 Change
 Change       Revision 1.33  1998/04/27 17:14:03  williamson
 Change       Added parameter set support and changed sizes of defines
 Change
 Change       Revision 1.32  1998/02/12 20:34:49  witzke
 Change       PR6378 Increase SITE_ID_LENGTH and SS_ID_LENGTH to 33
 Change
 Change       Revision 1.31  1997/11/17 20:06:21  williamson
 Change       PR 5313  Expanded size of wafer id and split fields.  Also added split to
 Change       the slot list structure
 Change
 Change       Revision 1.30  1997/10/28 13:10:38  moore
 Change       Removed ^M characters
 Change
 Change       Revision 1.29  1997/10/27 17:12:10  moore
 Change       NT code cleanup
 Change
 Change       Revision 1.28  1997/08/28 17:50:39  aujla
 Change       Updated with the NT changes.
 Change
 Change       Revision 1.27  1997/04/11 14:03:33  williamson
 Change       Expanded SITE_ID_LENGTH to match subsite id length
 Change
 Change       Revision 1.26  1997/03/12 17:50:24  williamson
 Change       PR 3407  Spliting KIDB into KI_KTXE_KDF and KI_KTXE_KLF
 Change
 * Revision 1.25  1997/01/13  14:38:33  moore
 * PR03209: Modifed LOT_TESTNAME_LENGTH from 51 to 255
 *
 * Revision 1.24  1997/01/10  17:49:43  williamson
 * PR 3408  Changed extension of backup file for limit files to .klf%, not kl%
 * Also changed extension for kdf backup files .kdf%, not kd%
 *
 * Revision 1.23  1996/12/16  20:19:18  williamson
 * Changed testname length back to 51...Going to PR this, KSU doesn't work
 *
 * Revision 1.22  1996/12/16  18:15:07  williamson
 * Increased the size of LOT_TESTNAME_LENGTH to 256.  Philips found it too small..
 *
 * Revision 1.21  1996/12/04  14:53:21  williamson
 * Adding HASH functionality...look for ljl comments for changes
 *
 * Revision 1.20  1996/12/04  14:29:16  williamson
 * Added HASHing link(s) for results
 *
 * Revision 1.19  1996/09/25  17:21:18  moore
 * *** empty log message ***
 *
 * Revision 1.18  1996/09/25  13:55:34  moore
 * Added proto for COM_GetRealFileName()
 *
 * Revision 1.17  1996/06/03  18:37:25  williamson
 * Changed LIMIT_NAME_LENGTH from 31 to 41 to allow names of 40 characters
 *
 * Revision 1.16  1996/05/08  15:07:12  witzke
 * PR1293 updated for kte30 added data types and definitions
 *
 * Revision 1.15  1995/11/08  16:38:12  rao
 * No PR - Added a new function GetIniStringWithoutEnv
 *
 * Revision 1.14  1995/11/02  16:56:45  rao
 * PR 1014 - Corrected the LogLot Prototype
 *
 * Revision 1.13  1995/11/01  21:23:04  rao
 * Changed the prototype of LogLot function
 *
 * Revision 1.12  1995/08/23  17:36:27  tufte
 * adding waferdesc field to LOT struct
 *
 * Revision 1.11  1994/12/29  17:05:26  wang
 * 94u219:increase PARAM_ID_LENGTH to 41.
 *
 * Revision 1.10  1994/03/05  23:39:13  williams
 * added GetLimit prototype
 *
 * Revision 1.9  1994/03/03  16:33:01  williams
 * added MANY missing function prototypes
 * set the INI file to "KTH.INI" and datapath to "." in the dos
 *
 * Revision 1.8  1993/12/28  15:22:10  ewaska
 * Include kisighan.h Signal Handler declarations.
 *
 * Revision 1.7  1993/10/10  19:21:38  SCMO
 * added DEFAULT_NUM_DEC_PLACES
 *
 * Revision 1.6  1993/07/03  21:54:50  williams
 * Added a NOTE about the consequences of changing the LOT structure
 *
 * Revision 1.5  1993/06/23  16:52:24  williams
 * changed default directory to '.'
 *
 * Revision 1.4  1993/06/17  20:06:48  williams
 * Added function prototypes for LogPtr and LogPta
 *
 * Revision 1.3  1993/05/02  15:52:29  SCMO
 * Can't use REVID in header file unless you like lots of Multiply Defined err msgs!
 *
 * Revision 1.2  1993/04/26  19:07:00  SCMO
 * Fixed prototype for kdferror.h and corrected "REVID_"
 *
 * Revision 1.1  1993/04/26  18:18:20  SCMO
 * Initial revision
 *
 * Revision 1.2  1993/04/22  20:51:19  williams
 * changed kdf_filename declaration
 *
 * Revision 1.1  1993/04/16  18:24:21  williams
 * Initial revision
 *
...............................................................................

 Function:  This is the kdf header file which contains all the structure 
	definitions, the function prototypes, magic numbers, and the OS 
	specific definitions.  This header file should be included in any
	C program that uses the KDF routines. 
   	 

.............................................................................*/

#ifndef _KDF_H
#define _KDF_H

#include "kisighan.h"
/* #include "wdf.h" */

/* MSDOS */
#ifdef _MSDOS
#define DIR_DELIMITER "\\"
#define FILE_LENGTH_MAX 8
#define INITIALIZATION_FILE "KTH.INI"
#define DEFAULT_DATA_PATH "."

/* VMS */
#elif defined VMS
#define DIR_DELIMITER ""
#define FILE_LENGTH_MAX 31
#define INITIALIZATION_FILE "SYS$LOGIN:KTH.INI"
#define DEFAULT_DATA_PATH "SYS$LOGIN"

/* WIN32  */
#elif WIN32
#define DIR_DELIMITER "\\"
#define FILE_LENGTH_MAX 256
#define INITIALIZATION_FILE "kth.ini"
#define DEFAULT_DATA_PATH "."

/*  Unix  */
#else
#define DIR_DELIMITER "/"
#define FILE_LENGTH_MAX 256
#define INITIALIZATION_FILE "kth.ini"
#define DEFAULT_DATA_PATH "."
#endif

#define EXTENSION_LENGTH 5
#define DEFAULT_NUM_DEC_PLACES "4"

#define KI_LONG_STRING_LENGTH 1024
#define KI_MED_STRING_LENGTH 81
#define COMMAND_LENGTH 256

#define MAXFILENAMELENGTH 256
#define KIDATAPATHLENGTH 81

#define CREATELOT 0
#define APPENDLOT 1

#define DATA_FILE_EXTENSION ".kdf"
#define LIMIT_FILE_EXTENSION ".klf"
#define DATA_BACKUP_EXTENSION ".kdf%"
#define LIMIT_BACKUP_EXTENSION ".klf%"
#define KDF_VERSION "KDF V1.2"

/* #if ((defined VMS) || (defined WIN32)) */
#if ( VMS || AIX )
enum BOOLN { KI_FALSE, KI_TRUE };
#else
#if !((defined FALSE) && (defined TRUE))
enum BOOLN { FALSE, TRUE };
#endif
#endif


/* Global Variables */
#ifndef WIN32
extern char kdf_filename[MAXFILENAMELENGTH];
extern FILE *kdf_channel;
extern int putlotcalled;
extern int putwafercalled;
extern int putsitecalled;
#endif

typedef struct _lotstruct LOTLIST;
typedef struct _waferstruct WAFERLIST;
typedef struct _sitestruct SITELIST;
typedef struct _paramstruct PARAMLIST;
typedef struct _limitcodestruct LIMITCODELIST;
typedef struct _limitstruct LIMITLIST;

/* Definition of LOT array sizes */
#define LOT_ID_LENGTH 51
#define LOT_PROCESS_LENGTH 51
#define LOT_DEVICE_LENGTH  51
#define LOT_TESTNAME_LENGTH 256
#define LOT_SYSTEM_LENGTH 21
#define LOT_OPERATOR_LENGTH 31
#define LOT_STARTTIME_LENGTH 21
#define LOT_STOPTIME_LENGTH 21
#define LOT_SK1_LENGTH 31
#define LOT_SK2_LENGTH 21
#define LOT_SK3_LENGTH 11
#define LIMITCODE_LENGTH 81    /* both LOT & LIMITCODE */
#define LOT_COMMENT_LENGTH 257
#define LIMITCODE_PATH_LENGTH 256
#define LIMIT_COMMENT_LENGTH 257

/* Definition of WAFER array sizes */
#define WAFER_ID_LENGTH 33
#define WAFER_SPLIT_LENGTH 33

/* Definition of SITE array sizes */
#define SITE_ID_LENGTH 33

/* Definition of PARAM array sizes */
#define PARAM_ID_LENGTH 129   /* both PARAM & LIMIT */

/* Definition of LIMIT array sizes */
#define LIMIT_NAME_LENGTH 41
#define LIMIT_UNITS_LENGTH 11
#define LIMIT_CATEGORY_LENGTH 21
#define LIMIT_ABORTSTR_LENGTH 5

#define USR_TAG_STR "<TAG>"
#define USR_TAG_STR_MAX_LEN     512
#define USR_TAG_NAM_MAX_LEN     PARAM_ID_LENGTH

/*NOTE:  When PutLot checks for invalid characters in the LOT structure, the
 *	algorhythm that is used is very closely tied to the ordering of the 
 *	structure. It searches through the whole structure except for the last four
 *	items which are pointers.  If the data structure for the LOT is changed
 *	in any way, this error checking for valid characters will need to be
 *	updated.
 */

typedef struct _lotstruct
{
    char	id[LOT_ID_LENGTH];
    char	process[LOT_PROCESS_LENGTH];
    char	device[LOT_DEVICE_LENGTH];
    char	testname[LOT_TESTNAME_LENGTH];
    char	system[LOT_SYSTEM_LENGTH];
    int 	teststation;
    char	operator[LOT_OPERATOR_LENGTH];
    char	starttime[LOT_STARTTIME_LENGTH];
    char	stoptime[LOT_STOPTIME_LENGTH];
    char	sk1[LOT_SK1_LENGTH];
    char	sk2[LOT_SK2_LENGTH];
    char	sk3[LOT_SK3_LENGTH];
    char	limitcode[LIMITCODE_LENGTH];
    char	comment[LOT_COMMENT_LENGTH];
    char	disposition;
    char	waferdesc[LIMITCODE_LENGTH];
    int 	lot_sid;
    LOTLIST	*next;
    LOTLIST	*prev;
    WAFERLIST	*wafers;
    PARAMLIST	*params;
}LOT ;


typedef struct _waferstruct
{
    char	id[WAFER_ID_LENGTH];
    char	split[WAFER_SPLIT_LENGTH];
    int 	boat;
    int 	slot;
    char	disposition;
    int 	wnum;
    WAFERLIST	*next;
    WAFERLIST	*prev;
    SITELIST	*sites;
    PARAMLIST	*params;
}WAFER;

/** SUBSITE STRUCTURE */

typedef struct _subsitestruct SUBSITELIST;

/*** SUBSITE member limits, this # matches the max len for the cell(15) */
#define SS_ID_LENGTH 33

typedef struct _subsitestruct
{
    char id[SS_ID_LENGTH];
    float x_coord;
    float y_coord;
    SUBSITELIST *next;
    SUBSITELIST *prev;
} SUBSITE;

typedef struct	_sitestruct
{
    char	id[SITE_ID_LENGTH];
    float 	row;		/* y */
    float 	column;		/* x */
    char	disposition;
    SITELIST	*next;
    SITELIST	*prev;
    PARAMLIST	*params;
} SITE;

typedef struct _paramstruct
{
    char	id[PARAM_ID_LENGTH];
    float	value;
    char	disposition;
    float	count;
    float	mean;
    float	min;
    float	max;
    float	stdev;
    PARAMLIST	*next;
    PARAMLIST	*prev;
   LIMITLIST	*lim;
} PARAM;

typedef struct _limitcodestruct
{
    char	id[LIMITCODE_LENGTH];
    char	path[LIMITCODE_PATH_LENGTH];
    char      comment[LIMIT_COMMENT_LENGTH];
    LIMITCODELIST   *next;
    LIMITCODELIST   *prev;
    LIMITLIST	*limits;
}LIMITCODE;


typedef struct _limitstruct
{
    char	id[PARAM_ID_LENGTH];
    char	name[LIMIT_NAME_LENGTH];
    char	units[LIMIT_UNITS_LENGTH];
    char	category[LIMIT_CATEGORY_LENGTH];
    float	target;
    float	validhigh;
    float	validlow;
    float	spechigh;
    float	speclow;
    float	engrhigh;
    float	engrlow;
    float	controlhigh;
    float	controllow;
    char	abortaction[LIMIT_ABORTSTR_LENGTH];
    char	abortlimit[LIMIT_ABORTSTR_LENGTH];
    enum BOOLN	report;
    enum BOOLN	critical;
    LIMITLIST	*next;
    LIMITLIST	*prev;
    PARAM	*params;
    LIMITLIST   *nexth;	/* ljl Hash link */
}LIMIT;



/* User tag structure
 */
typedef struct _tagList
{
    char *tagName ;
    char *tagString ;
    struct _tagList *next ;
} tagList ;






/* Function prototypes */
int GetLot (LOT *wanted, LOT *got);
int GetWafer (LOT *lotin, WAFER *wanted, WAFER *got);
int GetSite (LOT *lotin, WAFER *wafin,SITE *wanted, SITE *got);
int GetParam (LOT *lotin, WAFER *wafin,SITE *sitein,PARAM *wanted, PARAM *got);
int GetParamList(LOT *lotin,WAFER *wafin,SITE *sitein,PARAM *wanted,PARAM *got);
int GetLimitCode( LIMITCODE *wanted, LIMITCODE *got);
int GetLimit( LIMITCODE *codein, LIMIT *got);
int GetLotData(LOT *wanted);
int PutLot( LOT *lotin, int lotadd);
int PutWafer( LOT *lotin, WAFER *wafin);
int PutSite ( LOT *lotin, WAFER *wafin, SITE *sitein);
int PutParam ( LOT *lotin, WAFER *wafin, SITE *sitein, PARAM *paramin);
int PutParamList(LOT *lotin,WAFER *wafin,SITE *sitein,PARAM *paramin);
int PutLimit( LIMITCODE *code, LIMIT *limitin);
int LogLot (char lotid[LOT_ID_LENGTH], int replicate, 
	    char process[LOT_PROCESS_LENGTH], char device[LOT_DEVICE_LENGTH],	
	    char testname[LOT_TESTNAME_LENGTH], char sk1[LOT_SK1_LENGTH],
	    char sk2[LOT_SK2_LENGTH], char limit[LIMITCODE_LENGTH],
	    char waferdesc[LIMITCODE_LENGTH],
	    char comment[LOT_COMMENT_LENGTH], int lotadd);
int LogWaf(char id[10],int bin);
int LogSit(char id[10],int min);
int LogPtr(int tag, float value);
void LogPta(int tag, float results[],int numresults);
int EndWafer (void);
int EndSite (void);
int EndLot	(void);
int kdf_strcmpwild(char *str1,char *str2);
int MatchParam2Limit(PARAM *plist,LIMIT *lcodes);

int PutTag( char*, char* ) ;
int GetTag(LOT *lotin,
	   WAFER *wafin,
	   SITE *sitein,
	   char *tagName,
	   tagList **got);

void PrintTagList( tagList * head ) ;
void ClearTagList( tagList **head ) ;


/* prototypes for KDF get filename routines.
 * kdf_getfilename was changed to getGENfilename to find uses.  This
 * routine has been replaces with getKDFfilename and getKLFfilename
 * routines.  The new routines will use the appropriate environment variables
 * for the pathname
 */
int kdf_getGENfilename(char id[MAXFILENAMELENGTH],char extension[4],
		    char name[MAXFILENAMELENGTH]);
int kdf_getlotfilename(LOT *lotin,char extension[4],
		       char name[MAXFILENAMELENGTH]);
int kdf_getKDFfilename( char id[MAXFILENAMELENGTH],
			char name[MAXFILENAMELENGTH] ) ;
int kdf_getlimitfilename(LIMITCODE *codein, char extension[4],
			 char name[MAXFILENAMELENGTH]);
int kdf_getKLFfilename(char id[MAXFILENAMELENGTH],
		       char name[MAXFILENAMELENGTH] ) ;
int kdf_checkfilename (char *name);

int LotExist(LOT *lotin);
int FileExist(char name[MAXFILENAMELENGTH]);
int UsrTagIncluded( char *str ) ;

void Wrtred(char *prompt,char *answer, int nmax, int nrcvd);
int getlimitcodedos(LIMITCODE *wanted, LIMITCODE *got);
int getlotheader(FILE *temp,LOT *lotin);
int getlotdos (LOT *wanted, LOT *got);
void kdf_dec();

int GetIniString(char *component, char *item, char *value,int maxlen, char *defvalue, char *initfile);

int GetIniStringWithoutEnv(char *component, char *item, char *value,int maxlen, char *defvalue, char *initfile);


void kdferror(int, ...);

void COM_GetRealFileName( char *infilename, char *filewext, char *filenoext,
                          char *ext);
void GetStartTime(char []);
void kdf_freenode(void *ptr);

void kdf_getGENtmpfile(char tmpfile[MAXFILENAMELENGTH]);
void kdf_getKDFtmpfile(char tmpfile[MAXFILENAMELENGTH]);
void kdf_getKLFtmpfile(char tmpfile[MAXFILENAMELENGTH]);

void *kdf_getnewnode(int);

void AddNewLimitCode(LIMITCODE *, LIMITCODE *);
void AddNewLimit(LIMIT *, LIMIT *);
void AddNewLot  (LOT   *, LOT   *);
void AddNewWafer(WAFER *, WAFER *);
void AddNewSite (SITE  *, SITE  *);
void AddNewParam(PARAM *, PARAM *);

int DeleteLimitCode(LIMITCODE *);
int DeleteLimit(LIMITCODE *, LIMIT *);
int DeleteLot(LOT *);
int DeleteWafer(LOT *, WAFER *);
int DeleteSite(LOT *, WAFER *, SITE *);
int DeleteParam(LOT *, WAFER *, SITE *, PARAM *);

void InsertNewLimitCode(LIMITCODE *, LIMITCODE *);
void InsertNewLimit(LIMIT *, LIMIT *);
void InsertNewLot  (LOT   *, LOT   *);
void InsertNewWafer(WAFER *, WAFER *);
void InsertNewSite (SITE  *, SITE  *);
void InsertNewParam(PARAM *, PARAM *);

LIMITCODE *CreateNewLimitCode(void);
LIMITCODE *FindNextLimitCode(LIMITCODE *current);
LIMITCODE *FindPrevLimitCode(LIMITCODE *current);
LIMITCODE *FindFirstLimitCode(LIMITCODE *current);
LIMITCODE *FindLastLimitCode(LIMITCODE *current);
LIMITCODE *RemoveLimitCode(LIMITCODE *current);

LIMIT *CreateNewLimit(void);
LIMIT *FindNextLimit(LIMIT *current);
LIMIT *FindPrevLimit(LIMIT *current);
LIMIT *FindFirstLimit(LIMIT *current);
LIMIT *FindLastLimit(LIMIT *current);
LIMIT *RemoveLimit(LIMIT *current);

LOT *CreateNewLot(void);
LOT *FindNextLot(LOT *current);
LOT *FindPrevLot(LOT *current);
LOT *FindFirstLot(LOT *current);
LOT *FindLastLot(LOT *current);
LOT *RemoveLot(LOT *current);

WAFER *CreateNewWafer(void);
WAFER *FindNextWafer(WAFER *current);
WAFER *FindPrevWafer(WAFER *current);
WAFER *FindFirstWafer(WAFER *current);
WAFER *FindLastWafer(WAFER *current);
WAFER *RemoveWafer(WAFER *current);

SITE *CreateNewSite(void);
SITE *FindNextSite(SITE *current);
SITE *FindPrevSite(SITE *current);
SITE *FindFirstSite(SITE *current);
SITE *FindLastSite(SITE *current);
SITE *RemoveSite(SITE *current);

PARAM *CreateNewParam(void);
PARAM *FindNextParam(PARAM *current);
PARAM *FindPrevParam(PARAM *current);
PARAM *FindFirstParam(PARAM *current);
PARAM *FindLastParam(PARAM *current);
PARAM *RemoveParam(PARAM *current);

#ifdef WIN32
int kdf_fflush(FILE *);
#endif
#endif	/* _KDF_H */
