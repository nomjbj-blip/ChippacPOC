

/*

    wtype.c

    Wtype is used in the UNIX SECS/GEM application to provide equivalent
    or emulated functionality to the original windows 3.1 version.
    
    Copyright (c) 1995 by Keithley Instruments, Inc. Cleveland, Ohio.
     
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

$Revision: 1.7 $
 *
 *    Rev 2.0   8 Aug 1996 13:52:58   Troy
 *
 *    QSC fmt initial release.
 */
 
#ifndef WTYPED
#define WTYPED

#include "kdf.h"

#define BOOL 		int 
#define TRUE 		1
#define FALSE 		0
#define	FAIL		1 
#define RECV_FAIL	0
#define _MAX_PATH 	260
#define BYTE 		char

#ifndef LONG
#define LONG		long
#endif

#define  wsprintf	sprintf
#define DEFAULTCALLER   1
#define SHAREDMEMSIZE   64000		/* Must be larger than AssignSharedMem() */
#define GEMCPID		4
#define EXTSOCKET	"sextn"		/* extension socket name */
#define MSGSOCKET	"smesg"		/* message socket name */
#define TEMPLTSOCKET	"stmpl"		/* template socket name */
#define GEMCPPIDFNAME	"gemcppid"	/* template descriptor filename */

typedef int  WPARAM;
typedef long LPARAM;
typedef long LRESULT;
typedef void (*FARPROC)(void);

typedef struct tagPOINT
{
    int x;
    int y;
} POINT;

/* Queued message structure */
typedef struct tagMSG
{
    int        hwnd;
    int        message;
    WPARAM      wParam;
    LPARAM      lParam;
    int        time;
    POINT       pt;
} MSG;

typedef struct tagRECT{
	int left;
	int top;
	int right;
	int bottom;
}RECT;

/* _find_t emulates PC file_find structure */
struct _find_t
{
   	char attrib;
	unsigned wr_time;
	unsigned wr_date;
	long size;
	char name[80];
};

/* Control panel GEM states */
#define COMM_STATE  	1201
#define CONTROL_STATE  	1202
#define SPOOL_STATE	1203
#define	EQUIPMENT_STATE	1204

#define HWND int
#define WORD unsigned short 
#define UINT unsigned short
#define __export
#define _export   
#define pascal
#define PASCAL  
#define GWGEM_ENDRTN  void *      
#define far
#define FAR  
#define _far
#define __far
#define LPVOID void *
#define HANDLE void *

#define LPSTR char *
#define LPCSTR char *
#define DWORD   unsigned long
#define MB_OK   0
#define MB_ICONSTOP   0x10
#define SW_SHOW		5

/* event codes */
#define WM_USER		0x0400
#define DO_DSNAME	0x0999
#define LB_ADDSTRING    0x401

#define EQUIP_STATE     (WM_USER + 1)
#define KTP_EVENT       (WM_USER + 2)
#define CLEAR_DATA      (WM_USER + 4)
#define CLEAR_RESULTS   (WM_USER + 5)

#define WM_CLOSE	(WM_USER + 11)
#define	WM_DESTROY	(WM_USER + 12)
#define WM_TIMER	(WM_USER + 13)
#define WM_COMMAND	(WM_USER + 14)
#define WM_STOP_TEMPLATE (WM_USER + 15)
#define DISPLAY_MESSAGE  (WM_USER + 16)
#define WM_DLGSTATE      (WM_USER + 17)
#define CSTM_REMOTE     (WM_USER + 18)

#define WM_COPYDATA     (WM_USER + 301)

#define MB_ICONEXCLAMATION	0x0030
#define LB_GETCOUNT	0
#define LB_SETCURSEL	0
#define WM_MOUSEACTIVE  0
#define WS_VISIBLE      0
#define WS_CHILD        0
#define LBS_NOTIFY      0
#define WS_SCROLL       0
#define WS_DLGFRAME     0
#define WM_MOUSEACTIVATE 0
#define WM_NACTIVATE    0
#define WS_OVERLAPPEDWINDOW 0
#define CW_USERDEFAULT  0
#define HBRUSH          0
#define COLOR_WINDOW    0
#define SM_CXSCREEN     0
#define SM_CYSCREEN     0
#define SW_NORMAL	0
#define WS_VSCROLL      0
#define WM_NCACTIVATE	0
#define S_STARTED
#define GMEM_SHARE	0x2000
#define _A_NORMAL	0
#define HRGN  
#define CALLBACK

/* string equivalents */
#define _stricmp strcasecmp
#define strcmpi strcasecmp
#define strncmpi strncasecmp
#define _fstrcpy strcpy
#define _fstrcat strcat


unsigned long EolConfig;
unsigned long EowConfig;
unsigned long DSNumLots;

#endif  /* WTYPED */


