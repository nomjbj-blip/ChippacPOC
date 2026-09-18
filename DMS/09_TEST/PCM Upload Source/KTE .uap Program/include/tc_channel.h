/* 

    tc_channel.h
   
    Tester Common channel declarations.
    Tester API (TAPI) Declarations.
          

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
   $Source: /cm/test/build/S600/v420/PROJCOM/RCS/tc_channel.h,v $
 $Revision: 1.7 $
 Rev $Date: 1999/04/08 20:31:45 $
     
 Change History
 * $Log: tc_channel.h,v $
 * Revision 1.7  1999/04/08 20:31:45  furio
 * Added TAPI functions for Probe Card Model Support.
 *
 * Revision 1.6  1997/01/22 14:45:29  hayes
 * Added support for error channel.
 *
 * Revision 1.5  1996/11/21 17:09:42  clark
 * Removed Trailing ',' in enumeration
 *
 * Revision 1.4  1996/11/20 21:36:57  clark
 * Made a typo on the initial checkin
 *
 * Revision 1.3  1996/11/20 21:31:42  clark
 * Modified to Add flag parameters for the New Channel.
 *
 * Revision 1.2  1996/06/14 18:57:31  hayes
 * Added tapi event management.
 *
 * Revision 1.1  1996/05/15  18:09:50  hayes
 * Initial revision
 *
*/


#ifndef TC_CHANNEL_H
#define TC_CHANNEL_H


/* Channel Types */
#define CHANNEL_CAL         0x00000001
#define CHANNEL_CAL_INFO    0x00000002
#define CHANNEL_DIAG        0x00000004
#define CHANNEL_DIAG_INFO   0x00000008
#define CHANNEL_HEALTH      0x00000010
#define CHANNEL_IMM         0x00000020
#define CHANNEL_RESOURCE    0x00000040
#define CHANNEL_RESULTS     0x00000080
#define CHANNEL_SYSSTATE    0x00000100
#define CHANNEL_TEC         0x00000200
#define CHANNEL_DATA        0x00000400	/* no server attached */
#define CHANNEL_BUFFEX      0x00000800
#define CHANNEL_ERROR       0x00001000
#define CHANNEL_CONFIG      0x00002000
#define CHANNEL_ALL         (0xffffffff & ~CHANNEL_DATA)


enum channel_flag_bits
{
    CHANNEL_VALID   = 0x0001,
    CHANNEL_INVALID = 0x0002
};


#endif





