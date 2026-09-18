/* 

    lptdef_lowercase.h

    Copyright (c) 1986, 1987, 1988, 1989, 1993, 1996 by Keithley Instruments,
    Inc. Cleveland, Ohio
 
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
 $Revision: 1.3 $
 Rev $Date: 1997/03/18 20:58:19 $
 
 Change History
 * $Log: lptdef_lowercase.h,v $
 * Revision 1.3  1997/03/18 20:58:19  williamson
 * Removed extra comment terminator at end of file...
 *
 * Revision 1.2  1997/03/17 11:26:37  moore
 * PR03840 - Added support for lower case instrument IDs, Parameter Defs and
 * Error Codes
 *

*/
 
#ifndef LPTDEF_LOWERCASE_H
#define LPTDEF_LOWERCASE_H
 
#include <lptinstr_lowercase.h>   /* Instrument IDS */
#include <lptparam_lowercase.h>   /* Parameter definitions */
#include <lptmsg_lowercase.h>     /* Error codes */
#include <lptproto.h>   /* Function Prototypes */
 
#endif
