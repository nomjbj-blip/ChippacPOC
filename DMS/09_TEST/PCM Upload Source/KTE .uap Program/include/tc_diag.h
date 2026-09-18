/*

    tc_diag.h

    Tester common declarations.
    Tester API (TAPI) diag channel declarations.


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
/*

 $Workfile:$
   $Source: /cm/test/build/S600/v420/PROJCOM/RCS/tc_diag.h,v $
 $Revision: 1.12 $
 Rev $Date: 1997/04/22 20:12:29 $

 Change History
 * $Log: tc_diag.h,v $
 * Revision 1.12  1997/04/22 20:12:29  hayes
 * Move diagnostic result defines here from diagsys.h
 *
 * Revision 1.11  1997/04/09 19:59:04  hayes
 * Added support for Lockout and Suspend events.
 *
 * Revision 1.10  1997/02/06 00:50:48  hayes
 * Added filters for diagnostic tests in a fault state and tests
 * in a fatal state.
 *
 * Revision 1.9  1997/01/08 16:03:03  hayes
 *  Updated data structures.
 * Added protocol version checking.
 *
 * Revision 1.8  1997/01/07 18:34:34  hayes
 * Cleanup only.
 *
 * Revision 1.7  1996/11/20 21:32:15  clark
 * Modified to support the new channel design.
 *
 * Revision 1.6  1996/10/07 18:08:07  clark
 * modified to use the new structure for DiagTestInfo
 * 
 * Revision 1.5  1996/10/04 20:51:09  clark
 * Added now fields diags required
 *
 * Revision 1.4  1996/09/18 22:18:24  hayes
 * OOPS, spelling error.
 *
 * Revision 1.3  1996/09/18  21:55:15  hayes
 * Added schedule structures.
 *
 * Revision 1.2  1996/08/29  19:57:10  clark
 * Added enum of DIAGTEST_SCHEDINDIV to diag_find_filters.
 *
 * Revision 1.1  1996/04/22  22:04:40  hayes
 * Initial revision
 *
*/


#ifndef TC_DIAG_H
#define TC_DIAG_H

#define DIAGTEST_EOL -1

enum diag_find_filters
{
    DIAGTEST_DISABLED   = 0x0001,
    DIAGTEST_ENABLED    = 0x0002,
    DIAGTEST_INHIBITED  = 0x0004,
    DIAGTEST_SCHEDINDIV = 0x0008,
    DIAGTEST_FAULT      = 0x0010,
    DIAGTEST_FATAL      = 0x0020,

    DIAGTEST_ALL        = (DIAGTEST_ENABLED | DIAGTEST_DISABLED)
};

/* diagnostic status values */
#define DIAG_PASS	0
#define DIAG_FAIL	3
#define DIAG_FATAL	5


typedef struct
{
    char    name[80];
    int     group_member_count;
    int     test_member_count;
} DiagGroupInfo;

typedef struct
{
    char          name[80];
    int           id;
    int           code;
    int           unit_tested;
    int           mode;
    int           level;
    int           flags;
    int           fault_code;
    long          fault_time;
    long          next_sched;
    unsigned int  interval;
    unsigned int  exec_time;
    unsigned int  calc_time;
    int           desc_length;
    int           dependency_count;
    int           dependent_count;
} DiagTestInfo;

typedef struct
{
    unsigned int    level;
    unsigned long   start_time;
    unsigned int    interval;
} DiagLevelSched;

typedef struct
{
    unsigned int    test_id;
    unsigned long   start_time;
    unsigned int    interval;
} DiagTestSched;



typedef struct
{
    int             cmdID;
    int             len;
    int             filter;
    int             n;
} findfirst;

typedef struct
{
    int             cmdID;
    int             len;
    int             n;
} findnext;

typedef struct
{
    int             cmdID;
    int             len;
    int             id;
} infoget;

typedef struct
{
    int             cmdID;
    int             len;
} testcount;

typedef struct
{
    int             cmdID;
    int             len;
    int             n;
} testrun;

typedef struct
{
    int             cmdID;
    int             len;
    int             elmts;
    int *           data;
} diagfaultcmd;

typedef struct
{
    int             status;
    int             level;
    int             max_level;
} diaglevel;


typedef struct
{
    int             cmdID;
    int             len;
    int             level;
}               diaglevelset;

typedef struct
{
    int             cmdID;
    int             len;
    int             elmts;
    char *          data;
    
} diaglevelschedset;

typedef struct
{
    int    cmdID;
    int    len;
    int    level;
    int    lockout;
} diagsweep;

typedef struct
{
    int             cmdID;
    int             len;
    int             elmts;
    char *          data;
} diagtestschedset;

typedef struct
{
    int             cmdID;
    int             len;
    unsigned int    elmts;
} diagtestcontrol;


#endif
