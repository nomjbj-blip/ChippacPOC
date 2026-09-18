/*

    diag.h

    Tester API (TAPI) diag channel declarations.


    Copyright (c) 1995 by Keithley Instruments, Inc. Cleveland, Ohio

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
   $Source: /cm/test/build/S600/v420/PROJCOM/RCS/diag.h,v $
 $Revision: 1.12 $
 Rev $Date: 1997/04/23 19:17:06 $

 Change History
 * $Log: diag.h,v $
 * Revision 1.12  1997/04/23 19:17:06  hayes
 * Added diagQueueClear support.
 *
 * Revision 1.11  1997/04/09 19:59:04  hayes
 * Added support for Lockout and Suspend events.
 *
 * Revision 1.10  1997/03/11 18:03:17  hayes
 * Added prototype for diagTestAbort
 *
 * Revision 1.9  1996/11/21 17:15:00  clark
 * Modified make independent of Control Structures.
 *
 * Revision 1.8  1996/11/20 21:30:58  clark
 * Modified to support the new channel definition.
 *
 * Revision 1.7  1996/09/30 21:30:38  clark
 * Modified to add new function prototypes.
 *
 * Revision 1.6  1996/05/23 13:52:10  hayes
 * Partial work toward Data Channel support.
 *
 * Revision 1.5  1996/04/22  22:03:35  hayes
 * Split into tester common file and what was left.
 *
 * Revision 1.4  1996/03/01  21:17:31  chaplin
 * revised the API for diagMonitorStart and the way it works internally
 *
 * Revision 1.3  1996/03/01  17:51:19  chaplin
 * added the diagMonitorStart function
 *
 * Revision 1.2  1996/02/29  19:50:34  chaplin
 * need to include the new channel.h file
 *
 * Revision 1.1  1996/02/29  19:20:45  chaplin
 * Initial revision
 *
 * Revision 1.1  1996/02/28  20:29:51  chaplin
 * Initial revision
 *
*/


#ifndef DIAG_H
#define DIAG_H

#include <channel.h>    /* channel types */
#include <tc_diag.h>    /* tester common structures */

typedef void (*DiagDataCallback)(int test_id, int status, long timestamp, char * raw_results);
typedef void (*DiagExecCallback)(int status, int total_tests, int fault_count, long  timestamp);
typedef void (*DiagLockoutCallback)(int state);
typedef void (*DiagSuspendCallback)(int state);

/* diagnostic channel function prototypes */
extern int  diagDataEventBind(CHANNEL channel, DiagDataCallback callback);
extern int  diagExecEventBind(CHANNEL channel, DiagExecCallback callback);
extern int  diagFaultClear(CHANNEL channel, int n, int * ids);
extern int  diagGroupInfoGet(CHANNEL channel, int id, DiagGroupInfo *info);
extern int  diagGroupMemberGet(CHANNEL channel, int id, int *group_members, int *test_members);
extern int  diagLevelGet(CHANNEL channel, int *level, int * numlevels);
extern int  diagLevelSchedGet(CHANNEL channel, DiagLevelSched *info);
extern int  diagLevelSchedSet(CHANNEL channel, int n, DiagLevelSched *info);
extern int  diagLevelSet(CHANNEL channel, int level);
extern int  diagLockoutEventBind(CHANNEL channel, DiagLockoutCallback callback);
extern int  diagLockoutStatus(CHANNEL channel);
extern int  diagMonitorStart(CHANNEL channel);
extern int  diagQueueClear(CHANNEL channel);
extern int  diagResume(CHANNEL channel);
extern int  diagSuspend(CHANNEL channel);
extern int  diagSuspendEventBind(CHANNEL channel, DiagSuspendCallback callback);
extern int  diagSuspendStatus(CHANNEL channel);
extern int  diagSweepRun(CHANNEL channel, int level, int lockout);
extern int  diagTestAbort(CHANNEL channel);
extern int  diagTestDependentsGet(CHANNEL channel, int id, int *dependents, int *dependencies);
extern int  diagTestDisable(CHANNEL channel, int n, int * ids);
extern int  diagTestEnable(CHANNEL channel, int n, int * ids);
extern int  diagTestFindFirst(CHANNEL channel, int filter, int n, int *ids);
extern int  diagTestFindNext(CHANNEL channel, int n, int *ids);
extern int  diagTestInfoGet(CHANNEL channel, int id, DiagTestInfo *info);
extern int  diagTestRun(CHANNEL channel, int n, int *ids);
extern int  diagTestSchedSet(CHANNEL channel, int count, DiagTestSched * info);

#endif





