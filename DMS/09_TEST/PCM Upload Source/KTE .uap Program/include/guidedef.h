/* guidedef.h */
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
...............................................................................

 Function:  This is the guide header file which contains all the #define
        declarations applicable to the guide program
        This header file should be included in any C program based on the
        example guide template programs.   	 

.............................................................................*/

#ifndef _GUIDEDEFH_
#define _GUIDEDEFH_

#include <string.h>

/* 
  The following macro replicates strncpy functionality, except that it copies
  up to maxlen - 1 characters from src into dest with the understanding that 
  the given maxlen is inclusive of null. Since it does call strncpy, dest will
  be truncated or null-padded. 

  In addition, it address a quirk in strncpy and guarantees that dest will be
  terminated with null for those cases where the length of src is maxlen -1 or
  more.
*/  

#define KI_Strncpy(d,s,l) (void) strncpy(d,s,l-1); *(d+l-1) = (char) NULL;


/*** flags returned by functions indicating success or failure */
#define KI_ABORT -32767
#define KI_FATAL_ABORT -32766
#define KI_ERROR -1
#define KI_OK     0


/*** constants  to set loop, wafer & site  "while(xxxx_loop)" continue flags */ 
#define LOOP -1
#define QUIT_LOOP 0


/*** command line err_report_mode argument constants */
#define NO_ERR_ACTION       0
#define DISP_ERR_MSGS       1
#define LOG_ERR_MSGS        2
#define LOG_DISP_ERR_MSGS   3
#define MAX_ERR_REPORT_MODE 3

/*** command line evt_report_mode argument constants */
#define NO_EVT_ACTION       0
#define DISP_EVT_MSGS       1
#define LOG_EVT_MSGS        2
#define LOG_DISP_EVT_MSGS   3
#define MAX_EVT_REPORT_MODE 3


/*** test station bounds */
#define MAX_TESTSTATION 4


/*** command line user argument length */
#define USER_ARG_LENGTH 256


/*** maximum slot number allowed */
#ifndef MAX_SLOT
#define MAX_SLOT 25
#endif


typedef struct _VarMsgDlgData
{
    int no_buttons;
    int no_lines;
    char **button_labels;
    char *win_label;
    char *ted_string;

} VarMsgDlgDataRec, *VarMsgDlgDataPtr;
#endif /* _GUIDEDEFH_ */
