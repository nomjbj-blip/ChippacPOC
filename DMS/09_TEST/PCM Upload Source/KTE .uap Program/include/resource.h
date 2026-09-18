/*

    resource.h

    Tester API (TAPI) Resource Channel Declarations.


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
   $Source: /cm/test/build/S600/v420/PROJCOM/RCS/resource.h,v $
 $Revision: 1.10 $
 Rev $Date: 2000/06/27 20:48:19 $

 Change History
 * $Log: resource.h,v $
 * Revision 1.10  2000/06/27 20:48:19  hayes
 * PCID enhancements.
 *
 * Revision 1.9  1999/04/08 20:31:45  furio
 * Added TAPI functions for Probe Card Model Support.
 *
 * Revision 1.8  1998/06/25 19:04:32  furio
 * Added prototypes for new TAPI functions resourceErrorStateTextGet() and
 * resourceFatalStateTextGet().
 *
 * Revision 1.7  1997/04/23 14:11:03  hayes
 * #include lines in header files MUST use <> not ""!!!!!!!!!
 *
 * Revision 1.6  1997/01/13 15:57:10  clark
 * Fixed Typo.
 *
 * Revision 1.5  1997/01/13 15:54:48  clark
 * Added Probcard prototypes
 *
 * Revision 1.4  1997/01/13 15:44:03  clark
 * Added typedef void (*ResourceProbecardCallback)(int state)
 * from use by external programs
 *
 * Revision 1.3  1996/05/15 18:25:10  hayes
 * Split out tester/host common stuff.
 *
 * Revision 1.2  1996/02/29  19:50:34  chaplin
 * need to include the new channel.h file
 *
 * Revision 1.1  1996/02/28  20:29:51  chaplin
 * Initial revision
 *
*/

#ifndef RESOURCE_H
#define RESOURCE_H

#include <channel.h>
#include <tc_resource.h>

typedef void (*ResourceProbecardCallback)(int state);
typedef void (*ResourceProbecardChangeCallback)(int model_id, int leakage_id,
        char *serial_number, char *type, char *comment);


/* resource channel function prototypes */
extern int      resourceServerFindFirst(CHANNEL channel, int filter, int n, int *ids);
extern int      resourceProbecardStatus(CHANNEL channel);
extern int      resourceProbecardEventBind(CHANNEL channel, ResourceProbecardCallback callback);
extern int      resourceServerFindNext(CHANNEL channel, int n, int *ids);
extern int      resourceUnitFindFirst(CHANNEL channel, int filter, int n, int *units);
extern int      resourceUnitFindNext(CHANNEL channel, int n, int *units);
extern int      resourceUnitInfoGet(CHANNEL channel, int unit, UnitInfo *info);
extern int      resourceErrorStateTextGet(CHANNEL channel, unsigned int bit, char *buffer, int buflen);
extern int      resourceFatalStateTextGet(CHANNEL channel, unsigned int bit, char *buffer, int buflen);
extern int      resourceProbecardInfoGet(CHANNEL channel, ProbecardInfo *info);
extern int      resourceProbecardLeakageSetFindFirst(CHANNEL channel, int filter, int n, int *leakage_ids);
extern int      resourceProbecardLeakageSetFindNext(CHANNEL channel, int n, int *leakage_ids);
extern int      resourceProbecardLeakageSetInfoGet(CHANNEL channel, int leakage_id, ProbecardLeakageSetInfo *info);
extern int      resourceProbecardModelFindFirst(CHANNEL channel, int filter, int n, int *model_ids);
extern int      resourceProbecardModelFindNext(CHANNEL channel, int n, int *model_ids);
extern int      resourceProbecardModelInfoGet(CHANNEL channel, int model_id, ProbecardModelInfo *info);
extern int      resourceProbecardChangeEventBind (CHANNEL channel, ResourceProbecardChangeCallback callback);

#endif
