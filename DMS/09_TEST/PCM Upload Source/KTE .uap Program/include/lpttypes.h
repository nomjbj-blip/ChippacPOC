/*

    lpttypes.h 

    Type definitions.
    Linear Parametric Test Library.


    Copyright (c) 1993, 1996 by Keithley Instruments, Inc. Cleveland, Ohio
     
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
 Rev $Date: 1996/10/08 21:40:45 $

 Change $Log: lpttypes.h,v $
 Change Revision 1.3  1996/10/08 21:40:45  hayes
 Change Remove comment about making INSTR_ID an int.  It was an int.
 Change
 * Revision 1.2  1996/01/04  17:12:25  chaplin
 * changed INSTR_ID to int and FPTYPE to double
 *
 * Revision 1.1  1995/08/14  16:04:22  hayes
 * Initial revision
 *

*/

#ifndef LPTTYPES_H
#define LPTTYPES_H


#ifdef _WINDOWS
#define FAR _far
#define PASCAL _pascal
#define strcpy _fstrcpy
#else
#define FAR
#define PASCAL
#endif

typedef int    INSTR_ID;
typedef double FPTYPE;

#endif

