/*  ibup.h */
/**************************************************************************

	   COPYRIGHT (C) 1996  by  KEITHLEY INSTRUMENTS, INC.
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

 $Workfile:$
   $Source: /cm/test/build/S600/v420/PROJCOM/RCS/ibup.h,v $
 $Revision: 1.3 $
 Rev $Date: 1997/01/03 18:17:57 $

 Change History
 * $Log: ibup.h,v $
 * Revision 1.3  1997/01/03 18:17:57  SCMO
 * Corrected comment delimiter character for ENODEV
 *
 * Revision 1.2  1996/12/20  21:58:09  chaplin
 * 1. removed slot table offsets -- not needed, and they were incorrect for
 *    the S600 implementation to boot!
 * 2. added a couple of error number definitions (as listed in the IBUP
 *    document provided to me by Dave Rose)
 * 3. commented out ENODEV, since this #define conflicts with POSIX
 *    errno.h -- just have to use magic number -21 to refer to this
 *    error until we figure out how to solve the problem for good
 *
 * Revision 1.1  1996/11/12  18:52:41  witzke
 * Initial revision
 *
 
 Function: ibup.h is the header file needed by the NA GPIB-232ct modules.
   It includes constant definitions for ibup functions, as well as the
   function prototypes for sendrec_232ct, gpib_232ct, etc.

..........................................................................*/



#ifndef IBUPX
#define IBUPX

#define  IBUP_WRITE         0
#define  IBUP_READ          1
#define  IBUP_CLEAR         2
#define  IBUP_TRIGGER       3
#define  IBUP_REMOTE        4
#define  IBUP_LOCAL         5
#define  IBUP_POLL          6
#define  IBUP_CONFIGURE     7
#define  IBUP_PASSCONTROL   8
#define  IBUP_DEFINE        9
#define  IBUP_FINISH        10
#define  IBUP_SRQWAIT		11	/* s600_IC only */

/*  Following error codes taken directly from the National Instruments  */
/*  GPIB11V-2 Operating and service manual, APPENDIX D - Error codes    */

#define IBUP_OK       1    /*  No error                                     */
#define ENONE        -1    /*  SRQ not asserted (TEST SRQ)                  */
#define ECACC        -2    /*  CAC conflict (ATN remains asserted after IFC */
#define ENCAC        -3    /*  Not CAC                                      */
#define ENSAC        -4    /*  Not SAC                                      */
#define EIFCABRT     -5    /*  IFC abort                                    */
#define ETIMO        -6    /*  Op did not complete in time, bus problem     */
#define ENOFUN       -7    /*  Non-exist driver function code, software prob*/
#define ETCTTO       -8    /*  TCT timeout                                  */
#define ENOIBDEV     -9    /*  No listeners addressed or no devices cnctd   */
#define EOPEN       -17    /*  IB handler cannot be opened, software prob   */
#define ENOUFN      -20    /*  Non-exist utility function code, software    */
/*#define ENODEV      -21    /*  Illegal device slot number, software prob    */
#define ENOLAD      -22    /*  No listen address for device, software prob  */
#define ENOTAD      -23    /*  No talk address for device, software prob    */
#define EHDLR      -100    /*  Communications problem with handler          */
#define NULL_UNIT  -999    /*  error returned in IBUP function passing      */

/*
 * NOTE: ENODEV is commented out above because it conflicts with a POSIX error
 * number (defined in errno.h).  Existing prober drivers which use IBUP already
 * make use of ENODEV, so we can't just rename it (at least, not now).  One
 * of these days we will clean up this mess.
 */


#endif
