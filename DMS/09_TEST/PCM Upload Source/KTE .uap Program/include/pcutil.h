/*
 
    pcutil.h
 
    Copyright (c) 1996 by Keithley Instruments, Inc. Cleveland, Ohio
 
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
/*******************************************************************************
 
 $Workfile: $
   $Source: /cm/test/build/S600/v420/PROJCOM/RCS/pcutil.h,v $
 $Revision: 1.1 $
 Rev $Date: 2000/08/24 21:47:00 $
 
 Change History
 * $Log: pcutil.h,v $
 * Revision 1.1  2000/08/24 21:47:00  scmo
 * Initial revision
 *
 * Revision 1.9  2000/07/21 14:11:33  hayes
 * Final changes for KTXEAddIn probe change support.
 *
 * Revision 1.8  2000/07/17 17:46:38  furio
 * Added new typedefs.
 *
 * Revision 1.7  2000/07/12 18:45:04  hayes
 * Added new routines to manage Model and Leakage Set lists.  Also added routines
 * to convert model/leakage set ids to names and visa versa.
 *
 * Revision 1.6  2000/07/11 14:55:21  hayes
 * Modified to use "leakage sets" rather than probe card models.
 *
 * Revision 1.5  2000/07/06 18:37:31  hayes
 * Minimal changes to compile with PCID changes.  More work is
 * required to make leakage set selection work properly.
 *
 * Revision 1.4  1999/05/27 17:16:55  jfrese
 * PR09513 Removed prototypes for StateEvent_CB and EventEvent_CB.
 *
 * Revision 1.3  1999/04/29 20:01:22  jfrese
 * PR08727 Added prototypes for new functions needed for
 * probecard model selection.
 *
 * Revision 1.2  1998/06/15 18:21:02  furio
 * Corrected typo in ProbeDetectFunc typedef.
 *
 * Revision 1.1  1998/06/15 14:43:51  furio
 * Initial revision
 *
 * Revision 1.3  1998/06/12 15:41:19  furio
 * Replaced ProbeChange's user command-line request/responses with
 * pop-up windows using the KUI library calls (i.e. GUIized ProbeChange).
 * Can now be called from KOP.
 * Also, moved configuration file and IP address retrieval code into the
 * ProbeDetect library routine.
 *
 * Revision 1.2  1998/06/09 16:34:44  furio
 * Incorrect comment referred to file as ProbeChange.h.
 *
 * Revision 1.1  1998/06/09 16:23:31  furio
 * Initial revision
 *
 * Revision 1.2  1997/07/28 14:54:36  moore
 * Cleaned up code and made application simpler to use
 *
 * Revision 1.1  1997/02/12 13:46:57  moore
 * Initial revision
 *
*/

#include <channel.h>
#include <errevent.h>
#include <resource.h>
#include <config.h>
#include <diag.h>
#include "kui_proto.h"

#define KELVINCON   60000000
#define TEST_BLK_SZ 64
#define IPSTR_LEN   41

typedef struct ProbecardModelInfoList
{
    ProbecardModelInfo                  *info;
    struct ProbecardModelInfoList       *next;
} ProbecardModelInfoList;

typedef struct ProbecardLeakageSetInfoList
{
    ProbecardLeakageSetInfo             *info;
    struct ProbecardLeakageSetInfoList  *next;
} ProbecardLeakageSetInfoList;

typedef struct ActiveProbecardInfo
{
    int  model_id;
    int  leakage_id;
    char user_type[32];
    char user_serial[32];
    char user_comment[32];
} ActiveProbecardInfo;

/*
// Function prototypes
*/
void DiagExecEvent_CB(int iStatus, int iTotalTests, 
                      int iFaultCount, long lTimeStamp);
void ErrorEvent_CB(TesterErrorInfo *Info);
void PCAEvent_CB(int iStatus);
void PCAChangeEvent_CB(int model_id, int leakage_id, char *serial_number, char *type, char *comment);

CHANNEL channelConfigOpen(char *cpHostname);
CHANNEL channelDiagOpen(char *cpHostname);
CHANNEL channelErrorOpen(char *cpHostname);
CHANNEL channelResourceOpen(char *cpHostname);

int ListBuild(CHANNEL channel);
ProbecardLeakageSetInfoList *ProbeListCreate(void);
LBOXDLG_ListPtr *ListBoxCreate(ProbecardLeakageSetInfoList *currentleakageset, int activeLeakageSet);
int ProbeListDestroy(ProbecardLeakageSetInfoList *list);
int ListBoxDestroy(LBOXDLG_ListPtr *head);
int ProbeSelect(int leakage_id);

extern int                              ProbeActiveGet(ActiveProbecardInfo *info);
extern int                              ProbeChangeAcquire(void);
extern int                              ProbeChangeDisable(void);
extern int                              ProbeChangeEnable(void);
extern int                              ProbeLeakageSetIdGet(ProbecardLeakageSetInfoList *leakagelist, char *leakage_name, int *leakage_id);
extern ProbecardLeakageSetInfoList *    ProbeLeakageSetListCreate(void);
extern int                              ProbeLeakageSetListDestroy(ProbecardLeakageSetInfoList *list);
extern char *                           ProbeLeakageSetNameGet(ProbecardLeakageSetInfoList *leakagelist, int leakage_id);
extern int                              ProbeModelIdGet(ProbecardModelInfoList *modellist, char *model_name, int *model_id);
extern ProbecardModelInfoList *         ProbeModelListCreate(void);
extern int                              ProbeModelListDestroy(ProbecardModelInfoList *list);
extern char *                           ProbeModelNameGet(ProbecardModelInfoList *modellist, int model_id);

int channelCloseAll(CHANNEL channelError, 
                    CHANNEL channelResource, 
                    CHANNEL channelDiag);

int ProbeDetect(int debug_mode);
int testerAddrGet(char *ipstr, int len);

typedef int                             (* ProbeDetectFunc)(int debug_mode);
typedef ProbecardLeakageSetInfoList*    (* ProbeListCreateFunc)(void);
typedef int                             (* ProbeListDestroyFunc)(ProbecardLeakageSetInfoList *list);
typedef int                             (* ProbeSelectFunc)(int leakage_id);
typedef LBOXDLG_ListPtr*                (* ListBoxCreateFunc)(ProbecardLeakageSetInfoList *head, int activeLeakageSet);
typedef int                             (* ListBoxDestroyFunc)(LBOXDLG_ListPtr *head);

typedef int                             (* ProbeActiveGetFunc)(ActiveProbecardInfo *info);
typedef int                             (* ProbeChangeAcquireFunc)(void);
typedef int                             (* ProbeChangeDisableFunc)(void);
typedef int                             (* ProbeChangeEnableFunc)(void);
typedef int                             (* ProbeLeakageSetIdGetFunc)(ProbecardLeakageSetInfoList *leakagelist, char *leakage_name, int *leakage_id);
typedef ProbecardLeakageSetInfoList *   (* ProbeLeakageSetListCreateFunc)(void);
typedef int                             (* ProbeLeakageSetListDestroyFunc)(ProbecardLeakageSetInfoList *list);
typedef char *                          (* ProbeLeakageSetNameGetFunc)(ProbecardLeakageSetInfoList *leakagelist, int leakage_id);
typedef int                             (* ProbeModelIdGetFunc)(ProbecardModelInfoList *modellist, char *model_name, int *model_id);
typedef ProbecardModelInfoList *        (* ProbeModelListCreateFunc)(void);
typedef int                             (* ProbeModelListDestroyFunc)(ProbecardModelInfoList *list);
typedef char *                          (* ProbeModelNameGetFunc)(ProbecardModelInfoList *modellist, int model_id);
