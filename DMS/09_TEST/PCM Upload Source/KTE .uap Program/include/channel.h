/* 

    channel.h
   
    Channel declarations.
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
   $Source: /cm/test/build/S600/v420/PROJCOM/RCS/channel.h,v $
 $Revision: 1.9 $
 Rev $Date: 1997/03/13 19:27:32 $
     
 Change History
 * $Log: channel.h,v $
 * Revision 1.9  1997/03/13 19:27:32  hayes
 * Added testerProbe support.
 *
*/

#ifndef CHANNEL_H
#define CHANNEL_H

#include <tc_channel.h>

struct channel_struct;
typedef struct channel_struct *CHANNEL;

typedef void (*ChannelCriticalCallback)(CHANNEL channel);

typedef struct 
{
    unsigned long   ip_address;
    char            model[32];
    char            name[64];
} TesterInfo;

int  channelClose(CHANNEL channel);
int  channelCriticalEventBind(CHANNEL channel, ChannelCriticalCallback callback);
int  channelOpen(char *tester_name, int channel_type, CHANNEL *channel);
int  channelServerGet(CHANNEL channel, int *id);
int  testerProbe(char *tester_name, TesterInfo *info);

#endif



