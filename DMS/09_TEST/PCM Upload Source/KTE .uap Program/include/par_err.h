/* 

	par_err.h

	Error messages returned by PARLIB routines.

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




/* Error messages for PARLIB routines */

#define COMPLIANCE_ERROR		1.0e22

#define	DEVICE_OPEN				1.0e24
	/* open devices */
#define GATE_OPEN				1.1e24
#define DRAIN_OPEN				1.2e24
#define SOURCE_OPEN				1.3e24
#define SUBSTRATE_OPEN			1.4e24
#define BASE_OPEN				1.5e24
#define COLLECTOR_OPEN			1.6e24
#define EMITTER_OPEN			1.7e24

#define DEVICE_SHORTED			2.0e24
	/* Shorted devices */
#define GATE_SHORT				2.1e24
#define DRAIN_SHORT				2.2e24
#define SOURCE_SHORT			2.3e24
#define SUBSTRATE_SHORT			2.4e24
#define BASE_SHORT				2.5e24
#define COLLECTOR_SHORT			2.6e24
#define EMITTER_SHORT			2.7e24
#define HIGH_SUB_CURRENT 		2.8e24

#define DEVICE_PASS				3.0e24
#define DEVICE_FAIL				4.0e24
#define NO_BKDN     			3.1e24

#define DEV_NOT_TEST			5.0e24
#define BAD_CONTACT				5.4e24
#define NO_WELL_CON				6.0e24
#define NO_SUB_CON				6.5e24
#define INPUT_ERROR				7.0e24
#define INPUT_OUT_OF_RANGE1		7.11e24
#define INPUT_OUT_OF_RANGE2		7.12e24
#define INPUT_OUT_OF_RANGE3		7.13e24
#define INPUT_OUT_OF_RANGE4		7.14e24
#define INPUT_OUT_OF_RANGE5		7.15e24
#define INPUT_OUT_OF_RANGE6		7.16e24
#define INVLD_POLARITY				7.2e24
#define NPTS_TOO_SMALL				7.3e24
#define	NPTS_TOO_BIG				7.4e24
#define INVLD_DEV				8.0e24
#define NOT_ENF_PTS				8.1e24
#define NO_SLOPE				8.2e24
#define DIV_BY_ZERO 			8.3e24
#define NO_INFLECT_FND  		8.4e24

#define INVLD_RANGE				9.0e24
#define INVLD_COMP				9.1e24
#define HIGH_ASSYM				9.2e24
#define INVLD_STEP				9.3e24
#define POLARITY_ERR			9.4e24
#define TOO_MANY_DEV			9.5e24
#define OUT_OF_MEMORY			9.6e24
#define ARRAY_COMP_START		2.5e25
#define ARRAY_COMP_END			2.6e25
#define ARRAY_COMP_INOUT		2.7e25
#define MAXSLOPE_AT_FIRST		2.8e25
#define MAXSLOPE_AT_LAST		2.9e25
