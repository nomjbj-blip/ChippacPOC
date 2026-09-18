/* 

	par_struct.h

	Structures and pound defines for PARLIB internal use.
	Pound defines for LEARN and DEBUG switches for user.

    Copyright (c) 1995 by Keithley Instruments, Inc. Cleveland, Ohio
     
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
 **************************************************************************
 *
 * File:     $Source: 
 * Current $Revision: 
 * Curent     $State: 
 * Last Rev    $Date: 
 *
 * Change       $Log: 
 *
 **************************************************************************
 */


#ifndef _STDIO_H
#include <stdio.h>
#endif

#define HASHVAL 100
#define W_INT 0
#define W_DOUBLE 1
#define W_STR 2
#define MAX_DEV_NAME 50
#define MAX_TO_FILE  500

typedef struct  {
  char learnname[50];
  double value;
  void *next;
}	learn_struct;


typedef struct  {
  char line[255];
  void *next;
}	database_struct;


typedef struct  {
  int pin_num;
  void *next;
} 	pin_struct;


typedef struct  {
  char device[50];
  void *next;
}	macro_struct;


typedef struct  {
  learn_struct *table[HASHVAL];
}	learn_table;


typedef struct  {
  database_struct *table[HASHVAL];
}	dev_table;


/* Constant definitions...these will need to be added with new fuctions */
/* to KITT to allow users to use these and not some number */

#define PLOTF	20
#define PLOT	21
#define DATA	12
#define	TIME	11	/* DEBUG timing switch */
#define CALL	10	/* DEBUG plottin switch */

#define AUTO		0			/* RANGE auto switch */
#define LEARN		-2.0e25		/* RANGE learn switch */
#define FAST		-3.0e25		/* RANGE in fast mode */
#define FAIL_LIMIT	-4.0e25		/* RANGE in fast pass/fail mode */

#define VDS_SWEEP	1
#define VGS_SWEEP	2

/* This is not a complete list, only partial!! */



static char debugfile[80] = "debugmem";
static int initialized = 0;
static learn_table *lpointer = NULL;
static dev_table *device_table = NULL;
