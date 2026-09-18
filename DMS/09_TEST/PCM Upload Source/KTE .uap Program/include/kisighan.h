/* kisighan.h */
/*************************************************************************

	   COPYRIGHT (C) 1993  by  KEITHLEY INSTRUMENTS, INC.
	   Cleveland, Ohio

	   This software is furnished under a license and may
	   be used and copied only in accordance with the terms
	   of such license, and with the inclusion of the above
	   COPYRIGHT notice.  This software or any other copies
	   thereof may not be provided or otherwise made
	   available to any other person.  No title to and
	   ownership of the software is hereby transferred.
	   The information in this software is subject to
	   change without notice, and should not be construed
	   as a commitment by KEITHLEY INSTRUMENTS, INC.

	   KEITHLEY assumes no responsibility for the use or
	   reliability of its software on equipment which is
	   not supplied by KEITHLEY.

**************************************************************************

 File:     $Source: /cm/test/build/S600/v420/COMMON/RCS/kisighan.h,v $
 Current $Revision: 1.1 $
 Current    $State: REL $
 Last Rev    $Date: 1993/12/21 21:07:28 $

 Change       $Log: kisighan.h,v $
 Change       Revision 1.1  1993/12/21 21:07:28  witzke
 Change       Initial revision
 Change
 
...............................................................................

 Function: kisighan.h - ki signal handler header file

........................................................*/

typedef struct sighandstruct SIGHANDLIST;

typedef int (*SIGHANDADDR)();

typedef struct sighandstruct
{
	SIGHANDADDR 	handler_addr;
	SIGHANDLIST	*next;
} SIGHAND;


