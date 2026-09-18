/*

    tc_resource.h

    Tester Common resource channel declarations.
    Tester API (TAPI) Resource Channel Declarations.


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
   $Source: /cm/test/build/S600/v420/PROJCOM/RCS/tc_resource.h,v $
 $Revision: 1.17 $
 Rev $Date: 2000/07/25 21:26:07 $

 Change History
 * $Log: tc_resource.h,v $
 * Revision 1.17  2000/07/25 21:26:07  furio
 * Added maxcount_cycle and maxcount_lifetime elements to ProbeCardInfo
 * structure.
 * Rearranged elements in ProbeCardInfo structure.
 *
 * Revision 1.16  2000/07/07 21:44:14  furio
 * Added sequence element to ProbecardInfo structure.
 *
 * Revision 1.15  2000/07/06 20:21:50  hayes
 * Undid last change.
 *
 * Revision 1.13  2000/06/27 19:45:55  hayes
 * Last ci was with the wrong file.  This is the right version.
 *
 * Revision 1.11  2000/06/19 19:53:34  hayes
 * Tweaked enum for model filter.
 *
 * Revision 1.10  2000/06/19 19:49:00  hayes
 * Added filter enumerations for resourceProbecardModelFindFirst.
 *
 * Revision 1.9  2000/06/16 20:36:29  hayes
 * Added support for new functions associated with automatic probecard
 * identification.
 *
 * Revision 1.8  1999/04/26 19:51:07  furio
 * Changed comment to old style.  KTE request.
 *
 * Revision 1.7  1999/04/20 02:30:43  furio
 * Added #define for PCAMODEL_EOL which is required for
 * resourceProbecardModelFindFirst() and resourceProbecardModelFindNext().
 *
 * Revision 1.6  1999/04/08 20:31:45  furio
 * Added TAPI functions for Probe Card Model Support.
 *
 * Revision 1.5  1997/04/22 21:02:32  hayes
 * Moved probe state names from resmgr.h to here.
 *
 * Revision 1.4  1997/03/17 22:56:26  hayes
 * Added UNIT_FAULT, UNIT_FATAL, and UNIT_UNTESTED find first filter flags.
 *
 * Revision 1.3  1997/01/16 18:44:02  hayes
 * Move probecard states here from resmgr.h
 *
 * Revision 1.2  1996/08/29 19:58:03  clark
 * Added last_cal and cal_due to UnitInfo structure.
 *
 * Revision 1.1  1996/05/15  18:09:50  hayes
 * Initial revision
 *
*/

#ifndef TC_RESOURCE_H
#define TC_RESOURCE_H

typedef struct
{
    int           group;
    char          name[32];
    char          model[32];
    char          revision[32];
    char          serial[32]; 
    unsigned long last_cal;
    unsigned long cal_due;
} UnitInfo;

typedef struct
{
    int             model_id;
    int             leakage_id;
    unsigned int    sequence;
    unsigned int    maxcount_cycle;
    unsigned int    maxcount_lifetime;
    unsigned int    maxlen_type;
    unsigned int    maxlen_serial;
    unsigned int    maxlen_comment;
    unsigned int    touchdowns_cycle;
    unsigned int    touchdowns_lifetime;
    char            user_type[32];
    char            user_serial[32];
    char            user_comment[32];
} ProbecardInfo;

typedef struct
{
    int     id;
    char    name[32];
    double  spec_gross;
    double  spec_interchan;
} ProbecardLeakageSetInfo;

typedef struct
{
    int     id;
    char    name[32];
} ProbecardModelInfo;

enum
{
    UNIT_REAL       = 0x0001,
    UNIT_VIRTUAL    = 0x0002,
    UNIT_ALIAS      = 0x0004,
    UNIT_ALL        = (UNIT_REAL | UNIT_VIRTUAL | UNIT_ALIAS),
    UNIT_FAULT      = 0x0010,
    UNIT_FATAL      = 0x0020,
    UNIT_UNTESTED   = 0x0040
};

enum
{
    LEAKAGE_ENCODABLE   = 0x0001,
    LEAKAGE_ANONYMOUS   = 0x0002,
    LEAKAGE_ALL         = (LEAKAGE_ENCODABLE | LEAKAGE_ANONYMOUS)
};

enum
{
    PCAMODEL_STANDARD   = 0x0001,
    PCAMODEL_SPECIAL    = 0x0002,
    PCAMODEL_ALL        = (PCAMODEL_STANDARD | PCAMODEL_SPECIAL)
};


#define UNIT_EOL 0
#define PCAMODEL_EOL -1    /* Could not use 0 because it is a valid model id */
#define PCALEAKAGE_EOL -1  /* Could not use 0 because it is a valid leakage id */

enum ProbecardStates
{
    PCA_PRESENT = 0,
    PCA_UNTESTED,
    PCA_MISSING = 3,
    PCA_ERROR   = 5
};

/*
    NOTE: Trying to go to PROBESTATE_UNKNOWN is forbidden.  This state is the
    power on state.  The probe state can be only be changed to WU, PU, or PD.
*/
enum ProbeStates
{
    PROBESTATE_WU = 0,  /* Wafer Unloaded */
    PROBESTATE_PU,      /* Pins Up */
    PROBESTATE_PD,      /* Pins Down */
    PROBESTATE_UNKNOWN  /* Power on state -- we just don't know */
};

#endif

