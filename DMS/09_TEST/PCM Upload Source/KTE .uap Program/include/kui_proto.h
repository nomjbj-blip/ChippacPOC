/* kui_proto.h */
/**************************************************************************
 *
 *       COPYRIGHT (C) 1993  by  KEITHLEY INSTRUMENTS, INC.
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
 *
 * File:     $Source: /cm/test/build/S600/v420/COMMON/RCS/kui_proto.h,v $
 * Current $Revision: 1.18 $
 * Curent     $State: REL $
 * Last Rev    $Date: 2000/09/12 18:50:41 $
 *
 * $Log: kui_proto.h,v $
 * Revision 1.18  2000/09/12 18:50:41  williamson
 * pr13359  Added support for user fields for Status dialog window
 *
 * Revision 1.17  1999/08/03 13:56:10  williamson
 * Added usrWinDlg structure definition
 *
 * Revision 1.16  1999/07/14 15:39:43  ewaszkow
 * Add 1 function declaration
 *
 * Revision 1.15  1999/04/14 17:22:14  williamson
 * Added lboxdlg stuff
 *
 * Revision 1.14  1998/06/11 14:27:47  williamson
 * PR 5944  Added suspend/resume support
 *
 * Revision 1.13  1997/11/07 16:25:02  williamson
 * Added lot_dlg_fields back to the mix...
 *
 * Change       Revision 1.12  1997/10/27 17:12:10  moore
 * Change       NT code cleanup
 * Change
 * Change       Revision 1.11  1997/10/02 16:01:30  williamson
 * Change       Changed DLG_LOOK_XXX defines for ND 4.0 differences...
 * Change
 * Change       Revision 1.10  1997/09/12 20:07:42  williamson
 * Change       Removed ^M left over from PC editing...
 * Change
 * Change       Revision 1.9  1997/09/03 12:29:21  kdev
 * Change       Add KTE_IMPORT for global lot_dlg_fields.
 * Change
 * Change       Revision 1.8  1997/01/09 16:26:24  williamson
 * Change       Changed prototypes for SetStatusDlgPause and SetStatusDlgContinue calls
 * Change
 * Revision 1.7  1997/01/09  15:45:14  williamson
 * Added SetStatusDlgPause and SetStatusDlgContinue prototypes
 *
 * Revision 1.6  1996/09/13  13:14:57  moore
 * PR02360 - Added a Dialog return flag constant : DLG_SKIP 2
 * This is for skipping the current wafer in ExecWWP()
 *
 * Revision 1.5  1996/08/14  17:45:43  williamson
 * PR 1861  Added structure to allow supporting files to be displayed on the
 * status dialog screen.
 *
 * Revision 1.4  1996/05/08  15:13:53  witzke
 * pr1293 updated for kte30 added ScrollError prototypes
 *
 * Revision 1.3  1994/03/29  21:17:54  wang
 * Change kui_proto.h
 *
 * Revision 1.2  1993/12/01  17:33:53  ewaska
 * Add RCS headers and SSMove function prototype.
 *
 *
 *************************************************************************/
#ifndef _KUI_PROTO_H_
#define _KUI_PROTO_H_

#include <string.h>

#ifndef KDF_VERSION
#include <kdf.h>
#endif

#ifndef _KWF_PROTO_H_
#include <wdf.h>
#endif

#ifndef _GUIDEDEFH_
#include <guidedef.h>
#endif



/**** COMMON USER INTERFACE ****/

/* 
  The following macro replicates strncpy functionality, except that it copies
  up to maxlen - 1 characters from src into dest with the understanding that 
  the given maxlen is inclusive of null. Since it does call strncpy, dest will
  be truncated or null-padded. 

  In addition, it address a quirk in strncpy and guarantees that dest will be
  terminated with null for those cases where the length of src is maxlen -1 or
  more.
*/  

#ifndef KI_Strncpy
#define KI_Strncpy(d,s,l) (void) strncpy(d,s,l-1); *(d+l-1) = (char) NULL;
#endif


/* dialog field editing enable constants */

#ifndef FIELD_ENABLED
#define FIELD_ENABLED 1
#endif

#ifndef FIELD_DISABLED
#define FIELD_DISABLED 0
#endif


/* xxxDlg() function return flags */

#ifndef DLG_ABORT
#define DLG_ABORT     KI_ABORT
#endif

#ifndef DLG_EXIT
#define DLG_EXIT     -1
#endif

#ifndef DLG_NO
#define DLG_NO        0
#endif

#ifndef DLG_YES
#define DLG_YES       1
#endif

#ifndef DLG_SKIP
#define DLG_SKIP      2
#endif

#ifndef DLG_SUSPEND
#define DLG_SUSPEND   3
#endif

#ifndef DLG_OK
#define DLG_OK        DLG_YES
#endif


/*** dialog "LOOK" and feel constants */

#ifndef DLG_LOOK_MOTIF     
/*#define DLG_LOOK_MOTIF     0*/
#define DLG_LOOK_MOTIF     4  /* ND 4.0 */
#endif

#ifndef DLG_LOOK_OPENLOOK  
/*#define DLG_LOOK_OPENLOOK  1 */
#define DLG_LOOK_OPENLOOK  5 /* ND 4.0 */
#endif

#ifndef DLG_LOOK_MSW       
/*#define DLG_LOOK_MSW       2*/
#define DLG_LOOK_MSW       1   /* ND 4.0   MSW 3.1 */
#endif

#ifndef DLG_LOOK_PM        
/*#define DLG_LOOK_PM        3*/
#define DLG_LOOK_PM        0   /* ND 4.0  WIN95 */
#endif

#ifndef DLG_LOOK_PM2       
/*#define DLG_LOOK_PM2       4*/
#define DLG_LOOK_PM2       2   /* ND 4.0 */
#endif

#ifndef MAX_DLG_LOOK       
/*#define MAX_DLG_LOOK       4*/
#define MAX_DLG_LOOK       5  /* ND 4.0 */
#endif



/**** USER INTERFACE INITIALIZATION AND RELEASE MODULE ****/

/* function prototypes */

void InitUI(int look);

void UpdateModelessDlgs();

int QuitUI();



/**** OPERATOR LOT DIALOG MODULE ****/

/* enumerated lot dialog field labels */

enum lot_fields
{
  EXIT_LOT_DLG,
  OPERATOR,   LOT_ID,       PROCESS,     DEVICE,      TEST_NAME,  
  SYSTEM_ID,  TEST_STATION, SEARCH_KEY1, SEARCH_KEY2, SEARCH_KEY3, 
  LIMIT_FILE, LOT_COMMENT, 
  NUM_LOT_FIELDS
};


/* lot dialog field edit enable array */
#ifndef WIN32
extern char lot_dlg_fields[NUM_LOT_FIELDS];
#endif
/* function prototype */

int LotDlg( LOT *lot, char lot_fields[NUM_LOT_FIELDS], int max_teststation );



/**** TEST PROGRAM STATUS MODULE ****/

/*** pause - continue - abort constants */

#ifndef KI_ABORT
#define KI_ABORT    -32767
#endif

#ifndef KI_CONTINUE
#define KI_CONTINUE  0
#endif

#ifndef KI_PAUSE
#define KI_PAUSE     1
#endif


/*** pause - continue - abort flag variable */

extern int pca_flag;


/* KUI status dialog support info structure definition
 */
typedef struct _kui_support
{
    char cpf[ MAXFILENAMESIZE ] ;
    char wpf[ MAXFILENAMESIZE ] ;
    char wdf[ MAXFILENAMESIZE ] ;
    char pcf[ MAXFILENAMESIZE ] ;
    char gdf[ MAXFILENAMESIZE ] ;
    char klf[ MAXFILENAMESIZE ] ;
} kui_support_t ;

typedef struct _kui_user
{
    char label_1[ MAXFILENAMESIZE ] ;
    char data_1[ MAXFILENAMESIZE ] ;
    char label_2[ MAXFILENAMESIZE ] ;
    char data_2[ MAXFILENAMESIZE ] ;
} kui_user_t ;


/* LBOX dlg structure definition
 */
typedef struct _lboxDlg
{
    char  *label ;
    int selected ;
    struct _lboxDlg *next ;
} LBOXDLG_ListPtr ;

#define LBOXDLG_MULTI_SELECT  1
#define LBOXDLG_SINGLE_SELECT 0


/* User window structure definitions
 */
typedef struct _usrWinDlg
{
    char  *funcName ;
    char  *libName ;
    void *userData ;
} usrWinDlgPtr ;




 
/** function prototypes */

void StatusDlg( LOT     **lot, 
		WAFER   **wafer, 
		SITE    **site, 
		SUBSITE **subsite,
		int     *total_wafers,
		int     *wafers_tested,
		int     *total_sites,
		int     *sites_tested,
		kui_support_t **KUI_Support,
		kui_user_t **KUI_User
		) ;

int UpdateStatusDlg( char *user_msg );
 


/**** GENERIC MESSAGE DIALOGS MODULE ****/

/** FUNCTION PROTOTYPES */

void OkMsgDlg( char *msgstr );
int  OkCancelMsgDlg( char *msgstr );
int  YesNoCancelMsgDlg( char *msgstr );

int InputMsgDlg( char *msgstr, char *inputstr);

int  VerifyAbort();

int  OkCancelAbortMsgDlg( char *msgstr );
int  YesNoAbortMsgDlg( char *msgstr );

void	ScrollMsgDlg( char *label );
void	ScrollMsgDlgMsg( char *msgstr );
void	ScrollMsgDlgClr();
void	ScrollMsgDlgExit();

void SetStatusDlgPause( char *msgstr ) ;
void SetStatusDlgContinue( char *msgsr ) ;

void	ScrollErrorMsgDlg( char *label );
void	ScrollErrorMsgDlgMsg( char *msgstr );
void	ScrollErrorMsgDlgClr();

SUBSITE *SSMove( SUBSITE *, char * );


/**** SINGLE WAFER DEFINITION DIALOG MODULE ****/

#ifndef MAX_SLOT
#define MAX_SLOT 25
#endif

/* function prototype */

int WfrIdDlg(WAFER **wafer_ptr, int max_cassette);



/**** MULTIPLE WAFER DEFINITION DIALOG MODULE ****/

/* function prototype */

int WfrIdsDlg(WAFER **wafer_ptr, int max_cassette, int *total_ptr);



/**** GET PROGRAM ARGUMENTS MODULE ****/

/* function prototype */

void GetProgramArgs(int argc, char *argv[], 
                    int  *debug,
                    int  *err_report_mode,
                    char **err_log_fname,
                    int  *gui_look,
                    LOT  **lot,
                    char **sum_report_options,
                    char **kwf_fname,
                    char *user_arg );

#endif /* _KUI_PROTO_H_ */
