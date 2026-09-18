/*
    config.h

    Tester API (TAPI) CONFIG Channel Internal Declarations.

    Copyright (c) 1999 by Keithley Instruments, Inc. Cleveland, Ohio

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
   $Source: /cm/test/build/S600/v420/PROJCOM/RCS/config.h,v $
 $Revision: 1.3 $
 Rev $Date: 2000/06/29 18:50:49 $

 Change History
 * $Log: config.h,v $
 * Revision 1.3  2000/06/29 18:50:49  hayes
 * Added configProbecardUserPatternGet.
 *
 * Revision 1.2  2000/06/27 20:48:19  hayes
 * PCID enhancements.
 *
 * Revision 1.1  1999/04/08 20:33:56  furio
 * Initial revision
 *
*/

#ifndef CONFIG_H
#define CONFIG_H

#include <tc_config.h>
#include <tc_resource.h>
#include <channel.h>

/* config channel function prototypes */
extern int  configProbecardInfoAcquire(CHANNEL channel);
extern int  configProbecardInfoInit(CHANNEL channel, ProbecardInfo *info);
extern int  configProbecardModelSet(CHANNEL channel, int model_id);
extern int  configProbecardLeakageSetSelect(CHANNEL channel, int leakage_id);
extern int  configProbecardUserPatternGet(CHANNEL channel, char *pattern, int maxlen);

#endif

