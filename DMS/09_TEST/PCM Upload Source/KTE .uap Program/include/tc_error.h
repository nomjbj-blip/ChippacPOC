/*

    tc_error.h

    Tester Common error/event channel declarations.
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
   $Source: /cm/test/build/S600/v420/PROJCOM/RCS/tc_error.h,v $
 $Revision: 1.5 $
 Rev $Date: 1998/08/27 15:24:30 $

 Change History
 * $Log: tc_error.h,v $
 * Revision 1.5  1998/08/27 15:24:30  hayes
 * PR02992 Made unit hung an error instead of a fatal error.
 *
 * Revision 1.4  1998/04/13 15:41:13  hayes
 * PR02992 Added new system fatal states.
 *
 * Revision 1.3  1997/04/18 18:35:30  hayes
 * Added fatal error state flag for lost unread messages.
 *
 * Revision 1.2  1997/04/09 19:59:04  hayes
 * Added #define for the maximum text size in an error/event event.
 *
 * Revision 1.1  1997/01/21 18:48:08  hayes
 * Initial revision
 *
*/

#ifndef TC_ERROR_H
#define TC_ERROR_H

typedef struct
{
    int fatal_flags;
    int error_flags;
} errorStateXfer;

/*
    The errorEventXfer structure is used to pass both tester error info and
    tester event info across the event transport.
*/
typedef struct
{
    int             eventnum;
    int             task;
    unsigned long   timestamp;
    unsigned int    headerlen;
    unsigned int    textlen;
    /* followed by header string */
    /* followed by message text string */
} errorEventXfer;

#define MAX_EVENT_TEXT 2048

enum FatalConditions
{
    FSEF_INIT           = 0x0001,
    FSEF_CONFIG         = 0x0002,
    FSEF_CONFIG_CHANGE  = 0x0004,
    FSEF_DIAG           = 0x0008,
    FSEF_PSA_FAULT      = 0x0010,
    FSEF_INTERLOCK      = 0x0020,
    FSEF_UNREAD_LOST    = 0x0040,
    FSEF_THERMAL        = 0x0080
};

enum ErrorFlags
{
    SEF_DIAG            = 0x0001,
    SEF_UNREAD          = 0x0002,
    SEF_HUNG_UNIT       = 0x0004
};

#endif

