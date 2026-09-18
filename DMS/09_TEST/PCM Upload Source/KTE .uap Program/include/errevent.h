/*

    errevent.h

    Tester API (TAPI) Error Channel Declarations.


    Copyright (c) 1997 by Keithley Instruments, Inc. Cleveland, Ohio

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
   $Source: /cm/test/build/S600/v420/PROJCOM/RCS/errevent.h,v $
 $Revision: 1.4 $
 Rev $Date: 1997/01/21 19:12:15 $

 Change History
 * $Log: errevent.h,v $
 * Revision 1.4  1997/01/21 19:12:15  clark
 * Fixed misspelling of "TesterEventInfo" as "TesterEventInto"
 *
 * Revision 1.3  1997/01/21 19:08:31  clark
 * Fixed another missing ';'
 *
 * Revision 1.2  1997/01/21 19:06:36  clark
 * Fixed compile bug (no ; on line 45)
 *
 * Revision 1.1  1997/01/21 18:48:08  hayes
 * Initial revision
 *
*/

#ifndef ERREVENT_H
#define ERREVENT_H

#include <tc_error.h>

typedef struct
{
    int     errornum;
    int     task;
    unsigned long   timestamp;
    char    *header;
    char    *message;
} TesterErrorInfo;

typedef struct
{
    int     eventnum;
    int     task;
    unsigned long   timestamp;
    char    *header;
    char    *message;
} TesterEventInfo;

typedef (*TesterErrorCallback)(TesterErrorInfo *info);
typedef (*TesterEventCallback)(TesterEventInfo *info);
typedef (*ErrorStateCallback)(int fatal_flags, int error_flags);

#endif

