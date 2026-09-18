/* 

	par_util.h

	This file contains function prototypes for all PARLIB utility functions.
	It also #includes par_err and par_struct, so this is the only file needed
	to include when working with PARLIB.

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
 *
 **************************************************************************
 */



#include <par_struct.h>
#include <par_err.h>

int dev_parse( char *devname, char *class, pin_struct **pins, char *type );
int add_usrdef( char *dev_class, char *type, int p1, int p2, int p3, int p4,
		int p5, int p6, int p7 );
int macro_parse( char *macro_name, macro_struct** devices, int num );
void dev_init( void );
void dev_cleanup( void );
double get_length( char *devname );
double get_width(  char *devname );
void mosfet_pins( pin_struct *pins, int *gate, int *drain, int *source, 
		int *well, int *sub, int *chuck );
void res_pins( pin_struct *pins, int *f1, int *s1, int *f2, int *s2,
		int *well, int *sub, int *chuck );

int par_hash( char *s );
void learn_init( void );
void learn_cleanup( void );
int GetComplianceStatus( int devid );
int write_learn( char *learnname, double learned, double meas );
double getrange( double rswitch, char *learned, double limit );

int dbg_time( double seconds, int debug );
int dbg_call( char *to_file, int debug );
int dbg_data( char title[20], int type, void *data, int num, int debug );
int dbg_plot( char title[20], int type, void *x, void *y, int num, int debug );

void llsq( double *x, double *y, int npts, double *a, double *b, double *r );
void fndslp( double *x, double *y, int npts, double *slope, 
			double *yinter, double *r );
void fndpt( double *values, int npts, double target, int *j );
void fndcj( double *v, double *c, int npts, double c0, double *pout, double *mout );
int linmmx( double *a, int n, double *amin, double *amax );
double evalcj(double *v, double *c, int npts, double p, double m, double c0);

