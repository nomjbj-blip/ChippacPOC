/* PARLIB function prototype and KITT header file */

/* COPY OF USRLIB SETTINGS INFORMATION */
/* [VISIBILITY] */
/* HIDDEN=0 */
/* [DEPENDENCIES] */
/* LIBS= */
/* OBJECTS= */
/* HEADERS= */
/* [NON-KULT SEARCH PATH] */
/* PATHS= */

/* 
 
    bulk_connect.c
  
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
 * File:     $Source: /cm/test/build/S600/v420/PARLIB/RCS/bulk_connect.c,v $
 * Current $Revision: 1.10 $
 * Curent     $State: REL $
 * Last Rev    $Date: 1998/03/13 20:16:00 $
 *
 * Change       $Log: bulk_connect.c,v $
 * Change       Revision 1.10  1998/03/13 20:16:00  nagy
 * Change       Fixed PR6217 and PR6585
 * Change
 * Change       Revision 1.9  1998/03/13 18:08:55  nagy
 * Change       Fixed chuck name in parameter list
 * Change
 * Change       Revision 1.8  1998/02/04 21:53:28  nagy
 * Change       Fixed id for chuck in kult list
 * Change
 * Change       Revision 1.7  1998/01/30 16:06:11  nagy
 * Change       Fixed a problem with connecting if all pins are -1 PR6217
 * Change       Fixed use of reserved word for chuck PR6218
 * Change
 * Change       Revision 1.6  1998/01/08 15:22:05  witzke
 * Change       Included Manual text as help.
 * Change       Tab conversion with: expand -4,16,28 file
 * Change
 * Change       Revision 1.5  1997/08/18 14:15:03  nagy
 * Change       Added check for sub, well and chuck_pin < 0 with Vbs > .0001 for
 * Change       PR5092
 * Change
 * Change       Revision 1.4  1997/07/16 18:56:08  nagy
 * Change       Reorder limit and force for Isubchk
 * Change
 * Change       Revision 1.3  1997/07/16 18:46:50  nagy
 * Change       Added minimum value for isubchk of 1.e-13 and added force for vbs
 * Change
 * Change       Revision 1.2  1997/06/06 20:15:06  nagy
 * Change       General cleanup for initial release.
 * Change
 * Change       Revision 1.1  1996/10/22 20:20:29  flowers
 * Change       Initial revision
 * Change
 *
 **************************************************************************
 */


/* USRLIB MODULE INFORMATION

	MODULE NAME: bulk_connect
	MODULE RETURN TYPE: double 
	ARGUMENTS:
		well,	int ,	Input
		sub,	int ,	Input
		chuck_pin,	int ,	Input
		Vbs,	double ,	Input
		isubchk,	double ,	Input
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
double bulk_connect(int  well, int  sub, int  chuck_pin, double  Vbs, double  isubchk);


/* 
 
    bvdss.c
  
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
 * File:     $Source: /cm/test/build/S600/v420/PARLIB/RCS/bvdss.c,v $
 * Current $Revision: 1.14 $
 * Curent     $State: REL $
 * Last Rev    $Date: 1998/06/03 15:37:24 $
 *
 * Change       $Log: bvdss.c,v $
 * Change       Revision 1.14  1998/06/03 15:37:24  nagy
 * Change       Repaired PR6219 - SMU2L connected to ground even if it's not used
 * Change
 * Change       Revision 1.13  1998/03/13 18:09:11  nagy
 * Change       Fixed chuck name in parameter list
 * Change
 * Change       Revision 1.12  1998/02/04 21:53:55  nagy
 * Change       Fixed id for chuck in kult list
 * Change
 * Change       Revision 1.11  1998/01/30 16:09:33  nagy
 * Change       Fixed a problem with use of reserved word chuck PR6218
 * Change
 * Change       Revision 1.10  1998/01/08 15:22:05  witzke
 * Change       Included Manual text as help.
 * Change       Tab conversion with: expand -4,16,28 file
 * Change
 * Change       Revision 1.9  1997/10/30 15:33:23  nagy
 * Change       Fixed PR5142 by adding a limit command
 * Change       Also cleaned up code for symetry test
 * Change
 * Change       Revision 1.8  1997/07/21 16:07:37  nagy
 * Change       Fixed limit for symmetry test
 * Change
 * Change       Revision 1.7  1997/07/08 17:46:52  nagy
 * Change       Added support for 3 terminal devices
 * Change
 * Change       Revision 1.6  1997/06/07 18:42:00  nagy
 * Change       Added PARLIB_proto.h for function prototype.
 * Change
 * Change       Revision 1.5  1997/06/06 20:15:29  nagy
 * Change       General cleanup for initial release.
 * Change
 * Change       Revision 1.4  1997/01/10 19:08:58  flowers
 * Change       Added call to enable( TIMER1 )
 * Change       Added test for valid pollarity
 * Change
 * Revision 1.3  1996/12/18  16:39:50  flowers
 * Used drainsmu and sourcesmu instead of SMU1 and SMU2
 * delay() to rdelay()
 * devint() to execut()
 *
 * Change       Revision 1.2  1996/10/24 20:10:54  flowers
 * Change       Added check if retsts == 0.0 to prevent overwriting earlier errors.
 * Change
 * Revision 1.1  1996/10/22  20:21:38  flowers
 * Initial revision
 *
 *
 **************************************************************************
 */


/* USRLIB MODULE INFORMATION

	MODULE NAME: bvdss
	MODULE RETURN TYPE: int 
	ARGUMENTS:
		devname,	char *,	Input
		drain,	int ,	Input
		gate,	int ,	Input
		source,	int ,	Input
		well,	int ,	Input
		sub,	int ,	Input
		chuck_pin,	int ,	Input
		polarity,	int ,	Input
		ipgm,	double ,	Input
		vlim,	double ,	Input
		Vbs,	double ,	Input
		symmetry,	int ,	Input
		mode,	double ,	Input
		delay_val,	double ,	Input
		debug,	int ,	Input
		SDBv,	double *,	Output
		SDBvsym,	double *,	Output
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#include <PARLIB_proto.h>
	END USRLIB MODULE INFORMATION
*/
int bvdss(char * devname, int  drain, int  gate, int  source, int  well, int  sub, int  chuck_pin, int  polarity, double  ipgm, double  vlim, double  Vbs, int  symmetry, double  mode, double  delay_val, int  debug, double * SDBv, double * SDBvsym);


/* 
 
    bvdss1.c
  
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
 * File:     $Source: /cm/test/build/S600/v420/PARLIB/RCS/bvdss1.c,v $
 * Current $Revision: 1.13 $
 * Curent     $State: REL $
 * Last Rev    $Date: 1998/03/30 15:45:29 $
 *
 * Change       $Log: bvdss1.c,v $
 * Change       Revision 1.13  1998/03/30 15:45:29  nagy
 * Change       Repaired typo in fix for 5094
 * Change
 * Change       Revision 1.12  1998/03/13 18:09:18  nagy
 * Change       Fixed chuck name in parameter list
 * Change
 * Change       Revision 1.11  1998/02/04 21:54:16  nagy
 * Change        Fixed chuck name PR6218
 * Change
 * Change       Revision 1.10  1998/01/08 15:22:05  witzke
 * Change       Included Manual text as help.
 * Change       Tab conversion with: expand -4,16,28 file
 * Change
 * Change       Revision 1.9  1997/08/18 14:10:11  nagy
 * Change       Added gnd connection for SMU4 when used
 * Change       fix for PR5094
 * Change
 * Change       Revision 1.8  1997/07/21 16:07:57  nagy
 * Change       Changed debug file name definition. Added missing smu low connects
 * Change       Change comparision for VPT1 to use fabs
 * Change
 * Change       Revision 1.7  1997/07/08 17:47:45  nagy
 * Change       Added support for 3 terminal devices
 * Change
 * Change       Revision 1.6  1997/06/07 18:42:42  nagy
 * Change       Added PARLIB_proto.h for function prototype.
 * Change
 * Change       Revision 1.5  1997/06/06 20:15:49  nagy
 * Change       General cleanup for initial release.
 * Change
 * Change       Revision 1.4  1997/01/10 19:11:24  flowers
 * Change       Added enable( TIMER1 )
 * Change       Added test for valid polarity
 * Change
 * Revision 1.3  1996/12/18  16:46:23  flowers
 * Small cleanup
 *
 * Change       Revision 1.2  1996/10/24 20:11:58  flowers
 * Change       Corrected function header to match the correct KULT header.
 * Change
 * Revision 1.1  1996/10/22  20:22:47  flowers
 * Initial revision
 *
 *
 **************************************************************************
 */


/* USRLIB MODULE INFORMATION

	MODULE NAME: bvdss1
	MODULE RETURN TYPE: int 
	ARGUMENTS:
		devname,	char *,	Input
		drain,	int ,	Input
		gate,	int ,	Input
		source,	int ,	Input
		well,	int ,	Input
		sub,	int ,	Input
		chuck_pin,	int ,	Input
		polarity,	int ,	Input
		VdStart,	double ,	Input
		VdStop,	double ,	Input
		npts,	int ,	Input
		Ibkdn,	double ,	Input
		Igcomp,	double ,	Input
		Vbs,	double ,	Input
		symmetry,	int ,	Input
		isubchk,	double ,	Input
		range,	double ,	Input
		lo_range,	double ,	Input
		mode,	double ,	Input
		delay_val,	double ,	Input
		debug,	int ,	Input
		VPT1,	double *,	Output
		VPT2,	double *,	Output
		VPTS1,	double *,	Output
		VPTS2,	double *,	Output
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#include <PARLIB_proto.h>
	END USRLIB MODULE INFORMATION
*/
int bvdss1(char * devname, int  drain, int  gate, int  source, int  well, int  sub, int  chuck_pin, int  polarity, double  VdStart, double  VdStop, int  npts, double  Ibkdn, double  Igcomp, double  Vbs, int  symmetry, double  isubchk, double  range, double  lo_range, double  mode, double  delay_val, int  debug, double * VPT1, double * VPT2, double * VPTS1, double * VPTS2);


/* 
 
    fieldVT.c
  
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
 * File:     $Source: /cm/test/build/S600/v420/PARLIB/RCS/fieldVT.c,v $
 * Current $Revision: 1.10 $
 * Curent     $State: REL $
 * Last Rev    $Date: 1998/03/13 18:09:49 $
 *
 * Change       $Log: fieldVT.c,v $
 * Change       Revision 1.10  1998/03/13 18:09:49  nagy
 * Change       Fixed chuck name in parameter list
 * Change
 * Change       Revision 1.9  1998/02/04 21:54:37  nagy
 * Change        Fixed chuck name PR6218
 * Change
 * Change       Revision 1.8  1998/01/08 15:22:05  witzke
 * Change       Included Manual text as help.
 * Change       Tab conversion with: expand -4,16,28 file
 * Change
 * Change       Revision 1.7  1997/07/21 16:09:10  nagy
 * Change       Changed debug file name definition
 * Change       Added function prototype for bulk_connect
 * Change
 * Change       Revision 1.6  1997/07/08 17:48:52  nagy
 * Change       Added support for 3 terminal devices and ability to
 * Change       skip fine search
 * Change
 * Change       Revision 1.5  1997/06/06 20:16:09  nagy
 * Change       General cleanup for initial release.
 * Change
 * Change       Revision 1.4  1997/01/10 19:44:43  flowers
 * Change       Added enable(TIMER1)
 * Change       Fixed trigcomp
 * Change       Added test for valid polarity
 * Change
 * Revision 1.3  1996/12/18  17:08:22  flowers
 * Put drain on SMU1 and gate on SMU2 for consistancy
 * W_FLOAT -> W_DOUBLE
 * devint() -> execut()
 *
 * Change       Revision 1.2  1996/10/24 20:17:12  flowers
 * Change       Changed all Vbs' to Vb to match actual variable name.
 * Change
 * Revision 1.1  1996/10/22  20:26:53  flowers
 * Initial revision
 *
 *
 **************************************************************************
 */


/* USRLIB MODULE INFORMATION

	MODULE NAME: fieldVT
	MODULE RETURN TYPE: int 
	ARGUMENTS:
		devname,	char *,	Input
		drain,	int ,	Input
		gate,	int ,	Input
		source,	int ,	Input
		well,	int ,	Input
		sub,	int ,	Input
		chuck_pin,	int ,	Input
		polarity,	int ,	Input
		VgStart,	double ,	Input
		Vgstpbig,	double ,	Input
		Vgstpsml,	double ,	Input
		Vgmax,	double ,	Input
		Vd,	double ,	Input
		IDtrig,	double ,	Input
		Vb,	double ,	Input
		Igcomp,	double ,	Input
		isubchk,	double ,	Input
		mode,	double ,	Input
		delay_val,	double ,	Input
		debug,	int ,	Input
		fVT,	double *,	Output
		STATUS,	int *,	Output
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#include <PARLIB_proto.h>
	END USRLIB MODULE INFORMATION
*/
int fieldVT(char * devname, int  drain, int  gate, int  source, int  well, int  sub, int  chuck_pin, int  polarity, double  VgStart, double  Vgstpbig, double  Vgstpsml, double  Vgmax, double  Vd, double  IDtrig, double  Vb, double  Igcomp, double  isubchk, double  mode, double  delay_val, int  debug, double * fVT, int * STATUS);


/* 
 
    idarray.c
  
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
 * File:     $Source: /cm/test/build/S600/v420/PARLIB/RCS/idarray.c,v $
 * Current $Revision: 1.13 $
 * Curent     $State: REL $
 * Last Rev    $Date: 1998/03/13 18:10:02 $
 *
 * Change       $Log: idarray.c,v $
 * Change       Revision 1.13  1998/03/13 18:10:02  nagy
 * Change       Fixed chuck name in parameter list
 * Change
 * Change       Revision 1.12  1998/02/04 21:55:01  nagy
 * Change        Fixed chuck name PR6218
 * Change
 * Change       Revision 1.11  1998/01/08 15:22:05  witzke
 * Change       Included Manual text as help.
 * Change       Tab conversion with: expand -4,16,28 file
 * Change
 * Change       Revision 1.10  1997/07/21 16:10:50  nagy
 * Change       Changed devint  to devclr for used when called from other functions.
 * Change
 * Change       Revision 1.9  1997/07/16 18:57:48  nagy
 * Change       Added type cast for force values
 * Change
 * Change       Revision 1.8  1997/07/08 17:45:53  nagy
 * Change       *** empty log message ***
 * Change
 * Change       Revision 1.7  1997/06/24 21:18:58  nagy
 * Change       Added support for 3 terminal devices, fixed return status
 * Change
 * Change       Revision 1.6  1997/06/07 18:43:09  nagy
 * Change       Added PARLIB_proto.h for function prototype.
 * Change
 * Change       Revision 1.5  1997/06/06 20:16:44  nagy
 * Change       General cleanup for initial release.
 * Change
 * Change       Revision 1.4  1997/01/10 17:12:38  flowers
 * Change       Added call to enable(TIMER1)
 * Change       Now allows test with no substrate connection, but Vb and isubchk
 * Change       must both be 0.
 * Change       Added tests for a valid polarity and AorD entry
 * Change
 * Revision 1.3  1996/12/18  17:28:09  flowers
 * Changed W_FLOAT to W_DOUBLE
 * devint() -> execut()
 *
 * Change       Revision 1.2  1996/10/24 20:58:44  flowers
 * Change       Added #define COMPLIANCE 0 to allow compilation until constant is defined.
 * Change
 * Revision 1.1  1996/10/22  20:29:47  flowers
 * Initial revision
 *
 *
 **************************************************************************
 */


/* USRLIB MODULE INFORMATION

	MODULE NAME: idarray
	MODULE RETURN TYPE: int 
	ARGUMENTS:
		devname,	char *,	Input
		drain,	int ,	Input
		gate,	int ,	Input
		source,	int ,	Input
		well,	int ,	Input
		sub,	int ,	Input
		chuck_pin,	int ,	Input
		polarity,	int ,	Input
		AorD,	int ,	Input
		Vlo,	double ,	Input
		Vhi,	double ,	Input
		Vconst,	double ,	Input
		to_meas,	int ,	Input
		Igcomp,	double ,	Input
		Idcomp,	double ,	Input
		Vb,	double ,	Input
		isubchk,	double ,	Input
		range,	double ,	Input
		lo_range,	double ,	Input
		mode,	double ,	Input
		delay_val,	double ,	Input
		debug,	int ,	Input
		id1,	D_ARRAY_T,	Output
		npts1,	int ,	Input
		v1,	D_ARRAY_T,	Output
		npts2,	int ,	Input
		arstat, double *,	Output
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#include <PARLIB_proto.h>
#define  COMPLIANCE 0	
	END USRLIB MODULE INFORMATION
*/
int idarray(char * devname, int  drain, int  gate, int  source, int  well, int  sub, int  chuck_pin, int  polarity, int  AorD, double  Vlo, double  Vhi, double  Vconst, int  to_meas, double  Igcomp, double  Idcomp, double  Vb, double  isubchk, double  range, double  lo_range, double  mode, double  delay_val, int  debug, double *id1, int  npts1, double *v1, int  npts2, double * arstat);


/* 
 
    Isubmx.c
  
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
 * File:     $Source: /cm/test/build/S600/v420/PARLIB/RCS/Isubmx.c,v $
 * Current $Revision: 1.10 $
 * Curent     $State: REL $
 * Last Rev    $Date: 1998/03/13 18:07:58 $
 *
 * $Log: Isubmx.c,v $
 * Revision 1.10  1998/03/13 18:07:58  nagy
 * Fixed chuck name in parameter list
 *
 * Revision 1.9  1998/02/04 21:52:12  nagy
 * Fixed chuck name PR6218
 *
 * Revision 1.8  1998/01/08 15:22:05  witzke
 * Included Manual text as help.
 * Tab conversion with: expand -4,16,28 file
 *
 * Change       Revision 1.7  1998/01/07 14:23:03  nagy
 * Change       Added test for LPT errors to handle PR5112
 * Change       Will now return Meas_not_performed if there is an LPT error
 * Change
 * Change       Revision 1.6  1997/07/08 17:49:39  nagy
 * Change       Added polarity test and test for NPTS < 2 or > MAXPTS
 * Change
 * Change       Revision 1.5  1997/06/06 20:13:35  nagy
 * Change       General cleanup for initial release
 * Change
 * Change       Revision 1.4  1997/01/10 18:51:32  flowers
 * Change       added enable( TIMER1 )
 * Change       Added in check for valid polarity
 * Change       Added debugger at end of routine
 * Change
 * Revision 1.3  1996/12/18  15:18:32  flowers
 * Added units to delay_val's comment (seconds)
 * Changed delay() to rdelay()
 *
 * Change       Revision 1.2  1996/12/18 14:58:16  flowers
 * Change       Fixed extra free's at end of code to account for the single malloc.
 * Change       Used execut instead of devint
 * Change
 * Change       Revision 1.1  1996/10/22 19:52:43  flowers
 * Change       Initial revision
 * Change
 *
 **************************************************************************
 */


/* USRLIB MODULE INFORMATION

	MODULE NAME: Isubmx
	MODULE RETURN TYPE: int 
	ARGUMENTS:
		devname,	char *,	Input
		drain,	int ,	Input
		gate,	int ,	Input
		source,	int ,	Input
		well,	int ,	Input
		sub,	int ,	Input
		chuck_pin,	int ,	Input
		polarity,	int ,	Input
		Vds,	double ,	Input
		Vbs,	double ,	Input
		Vlo,	double ,	Input
		Vhi,	double ,	Input
		npts,	int ,	Input
		Igcomp,	double ,	Input
		Idcomp,	double ,	Input
		Ibcomp,	double ,	Input
		IbsMin,	double ,	Input
		range,	double ,	Input
		lo_range,	double ,	Input
		mode,	double ,	Input
		delay_val,	double ,	Input
		debug,	int ,	Input
		IsubMax,	double *,	Output
		VgMax,	double *,	Output
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#define MAXPTS 100
	END USRLIB MODULE INFORMATION
*/
int Isubmx(char * devname, int  drain, int  gate, int  source, int  well, int  sub, int  chuck_pin, int  polarity, double  Vds, double  Vbs, double  Vlo, double  Vhi, int  npts, double  Igcomp, double  Idcomp, double  Ibcomp, double  IbsMin, double  range, double  lo_range, double  mode, double  delay_val, int  debug, double * IsubMax, double * VgMax);


/* 
 
    moscheck.c
  
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
 * File:     $Source: /cm/test/build/S600/v420/PARLIB/RCS/moscheck.c,v $
 * Current $Revision: 1.11 $
 * Curent     $State: REL $
 * Last Rev    $Date: 1998/03/13 18:10:51 $
 *
 * Change       $Log: moscheck.c,v $
 * Change       Revision 1.11  1998/03/13 18:10:51  nagy
 * Change       Fixed chuck name in parameter list
 * Change
 * Change       Revision 1.10  1998/02/04 21:55:13  nagy
 * Change        Fixed chuck name PR6218
 * Change
 * Change       Revision 1.9  1998/01/08 15:22:05  witzke
 * Change       Included Manual text as help.
 * Change       Tab conversion with: expand -4,16,28 file
 * Change
 * Change       Revision 1.8  1997/07/22 18:19:16  nagy
 * Change       Fixed typo for drainsmu
 * Change
 * Change       Revision 1.7  1997/07/21 16:20:55  nagy
 * Change       Added delay for compliance check
 * Change
 * Change       Revision 1.6  1997/07/08 17:47:16  nagy
 * Change       Added support for 3 terminal devices
 * Change
 * Change       Revision 1.5  1997/06/07 18:43:20  nagy
 * Change       Added PARLIB_proto.h for function prototype.
 * Change
 * Change       Revision 1.4  1997/06/06 20:17:03  nagy
 * Change       General cleanup for initial release.
 * Change
 * Change       Revision 1.3  1997/01/10 20:47:14  flowers
 * Change       Added enable(TIMER1)
 * Change       Added test for valid polarity
 * Change
 * Revision 1.2  1996/12/18  18:41:53  flowers
 * delay() -> rdelay()
 * W_FLOAT -> W_DOUBLE
 * Added tests to ensure lorange isn't passed 0
 *
 * Change       Revision 1.1  1996/10/22 20:36:32  flowers
 * Change       Initial revision
 * Change
 *
 **************************************************************************
 */


/* USRLIB MODULE INFORMATION

	MODULE NAME: moscheck
	MODULE RETURN TYPE: int 
	ARGUMENTS:
		devname,	char *,	Input
		drain,	int ,	Input
		gate,	int ,	Input
		source,	int ,	Input
		well,	int ,	Input
		sub,	int ,	Input
		chuck_pin,	int ,	Input
		polarity,	int ,	Input
		Vgs,	double ,	Input
		Vds,	double ,	Input
		Vbs,	double ,	Input
		Idoffmax,	double ,	Input
		Igmax,	double ,	Input
		Idonmin,	double ,	Input
		Icomp,	double ,	Input
		range,	double ,	Input
		d_lorange,	double ,	Input
		g_lorange,	double ,	Input
		mode,	double ,	Input
		delay_val,	double ,	Input
		debug,	int ,	Input
		Idoff,	double *,	Output
		Idon,	double *,	Output
		Igs,	double *,	Output
		Igd,	double *,	Output
		STOP,	int *,	Output
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#include <PARLIB_proto.h>
	END USRLIB MODULE INFORMATION
*/
int moscheck(char * devname, int  drain, int  gate, int  source, int  well, int  sub, int  chuck_pin, int  polarity, double  Vgs, double  Vds, double  Vbs, double  Idoffmax, double  Igmax, double  Idonmin, double  Icomp, double  range, double  d_lorange, double  g_lorange, double  mode, double  delay_val, int  debug, double * Idoff, double * Idon, double * Igs, double * Igd, int * STOP);


/* 
 
    Resi.c
  
    Copyright (c) 1997 by Keithley Instruments, Inc. Cleveland, Ohio
     
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
 * File:     $Source: /cm/test/build/S600/v420/PARLIB/RCS/Resi.c,v $
 * Current $Revision: 1.9 $
 * Curent     $State: REL $
 * Last Rev    $Date: 1998/03/13 18:08:33 $
 *
 * Change       $Log: Resi.c,v $
 * Change       Revision 1.9  1998/03/13 18:08:33  nagy
 * Change       Fixed chuck name in parameter list
 * Change
 * Change       Revision 1.8  1998/02/04 21:53:01  nagy
 * Change        Fixed chuck name PR6218
 * Change
 * Change       Revision 1.7  1998/01/08 15:22:05  witzke
 * Change       Included Manual text as help.
 * Change       Tab conversion with: expand -4,16,28 file
 * Change
 * Change       Revision 1.6  1997/07/08 17:45:30  nagy
 * Change       Added test for hiforce and loforce > 1
 * Change
 * Change       Revision 1.5  1997/06/24 21:17:46  nagy
 * Change       Fixed EMF calculations
 * Change
 * Change       Revision 1.4  1997/06/06 20:14:12  nagy
 * Change       General cleanup for initial release. Removed emf switch.
 * Change
 * Change       Revision 1.3  1997/01/10 16:49:17  flowers
 * Change       Changed hi and losource to hi and loforce, HIS->HIF, LOS->LOF.
 * Change       Now high pins (hiforce,loforce) are required and himeas,lomeas are optional.
 * Change       Added call to enable(TIMER1)
 * Change       Added test for valid polarity
 * Change       If only 2 terminal test, no longer uses SMU2, only SMU1
 * Change       Takes the absolute value of the calculated resistance.
 * Change       DOES NOT WORK WITH E06.  setauto needs to be called before
 * Change       forces and getstatus does not work.
 * Change
 * Change       Revision 1.2  1996/12/18 16:08:36  flowers
 * Change       delay() to rdelay()
 * Change       W_FLOAT to W_DOUBLE
 * Change
 * Change       Revision 1.1  1996/10/22 20:03:47  flowers
 * Change       Initial revision
 * Change
 *
 **************************************************************************
 */


/* USRLIB MODULE INFORMATION

	MODULE NAME: Resi
	MODULE RETURN TYPE: int 
	ARGUMENTS:
		devname,	char *,	Input
		hiforce,	int ,	Input
		himeas,	int ,	Input
		loforce,	int ,	Input
		lomeas,	int ,	Input
		well,	int ,	Input
		sub,	int ,	Input
		chuck_pin,	int ,	Input
		polarity,	int ,	Input
		Itest,	double ,	Input
		Vcomp,	double ,	Input
		measlo,	int ,	Input
		emf,	int ,	Input
		range,	double ,	Input
		lo_range,	double ,	Input
		mode,	double ,	Input
		delay_val,	double ,	Input
		debug,	int ,	Input
		resi1,	double *,	Output
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
int Resi(char * devname, int  hiforce, int  himeas, int  loforce, int  lomeas, int  well, int  sub, int  chuck_pin, int  polarity, double  Itest, double  Vcomp, int  measlo, int  emf, double  range, double  lo_range, double  mode, double  delay_val, int  debug, double * resi1);


/* 
 
    Resv.c
  
    Copyright (c) 1997 by Keithley Instruments, Inc. Cleveland, Ohio
     
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
 * File:     $Source: /cm/test/build/S600/v420/PARLIB/RCS/Resv.c,v $
 * Current $Revision: 1.12 $
 * Curent     $State: REL $
 * Last Rev    $Date: 1998/03/13 18:08:43 $
 *
 * Change       $Log: Resv.c,v $
 * Change       Revision 1.12  1998/03/13 18:08:43  nagy
 * Change       Fixed chuck name in parameter list
 * Change
 * Change       Revision 1.11  1998/02/04 21:53:13  nagy
 * Change        Fixed chuck name PR6218
 * Change
 * Change       Revision 1.10  1998/01/08 15:22:05  witzke
 * Change       Included Manual text as help.
 * Change       Tab conversion with: expand -4,16,28 file
 * Change
 * Change       Revision 1.9  1997/10/30 15:34:15  nagy
 * Change       Fixed PR 5119 by adding a test for short/open limits
 * Change       setting invalid limit
 * Change
 * Change       Revision 1.8  1997/07/08 17:45:10  nagy
 * Change       Added polarity check
 * Change
 * Change       Revision 1.7  1997/06/24 21:18:23  nagy
 * Change       Fixed Fail limits
 * Change
 * Change       Revision 1.6  1997/06/06 20:14:43  nagy
 * Change       General cleanup for initial release.
 * Change
 * Change       Revision 1.5  1997/01/21 21:41:41  williamson
 * Change       ifdef'd out calls to setimtr and setsmu
 * Change
 * Revision 1.4  1997/01/17  18:32:54  williamson
 * Removed close comment marker line 319
 *
 * Revision 1.3  1997/01/10  16:59:34  flowers
 * Removed inputs s1 & s2 since they had no meaning.  Uses f1 and f1
 * as the two resistor pins.
 * Added enable(TIMER1)
 * Returns the abs of the calculated resistance
 * WILL NOT WORK WITH E06.  Ranging problems in the SMU require setauto
 * calls before each force.  Any call to setimtr, setvmtr, or setsmu will
 * also fail
 *
 * Change       Revision 1.2  1996/12/18 16:16:46  flowers
 * Change       Always sets limit now if icomp > 0
 * Change       lorangei not called if lo_range = 0
 * Change       delay() changed to rdelay()
 * Change       devint() to execut()
 * Change       W_FLOAT to W_DOUBLE
 * Change
 * Change       Revision 1.1  1996/10/22 20:05:04  flowers
 * Change       Initial revision
 * Change
 *
 **************************************************************************
 */


/* USRLIB MODULE INFORMATION

	MODULE NAME: Resv
	MODULE RETURN TYPE: int 
	ARGUMENTS:
		devname,	char *,	Input
		f1,	int ,	Input
		f2,	int ,	Input
		well,	int ,	Input
		sub,	int ,	Input
		chuck_pin,	int ,	Input
		polarity,	int ,	Input
		vforce,	double ,	Input
		icomp,	double ,	Input
		openlim,	double ,	Input
		shortlim,	double ,	Input
		range,	double ,	Input
		lo_range,	double ,	Input
		measlo,	int ,	Input
		mode,	double ,	Input
		delay_val,	double ,	Input
		debug,	int ,	Input
		resv1,	double *,	Output
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
int Resv(char * devname, int  f1, int  f2, int  well, int  sub, int  chuck_pin, int  polarity, double  vforce, double  icomp, double  openlim, double  shortlim, double  range, double  lo_range, int  measlo, double  mode, double  delay_val, int  debug, double * resv1);


/* 
 
    subvtslope.c
  
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
 * File:     $Source: /cm/test/build/S600/v420/PARLIB/RCS/subvtslope.c,v $
 * Current $Revision: 1.11 $
 * Curent     $State: REL $
 * Last Rev    $Date: 1998/03/13 18:10:25 $
 *
 * Change       $Log: subvtslope.c,v $
 * Change       Revision 1.11  1998/03/13 18:10:25  nagy
 * Change       Fixed chuck name in parameter list
 * Change
 * Change       Revision 1.10  1998/02/04 21:55:23  nagy
 * Change        Fixed chuck name PR6218
 * Change
 * Change       Revision 1.9  1998/01/08 15:22:05  witzke
 * Change       Included Manual text as help.
 * Change       Tab conversion with: expand -4,16,28 file
 * Change
 * Change       Revision 1.8  1997/07/21 16:11:25  nagy
 * Change       *** empty log message ***
 * Change
 * Change       Revision 1.7  1997/07/08 17:50:52  nagy
 * Change       *** empty log message ***
 * Change
 * Change       Revision 1.6  1997/06/07 18:24:56  nagy
 * Change       Added PARLIB_proto.h for idarray function prototype.
 * Change
 * Change       Revision 1.5  1997/06/06 20:17:27  nagy
 * Change       General cleanup for initial release.
 * Change
 * Change       Revision 1.4  1997/01/07 19:39:52  flowers
 * Change       Added call to enable(TIMER1) so imeast would work.
 * Change       In internal calls to idarray, turned off debugging to keep
 * Change       from resetting the counter.
 * Change
 * Revision 1.3  1996/12/18  19:06:06  flowers
 * devint -> execut
 * W_FLOAT -> W_DOUBLE
 *
 * Change       Revision 1.2  1996/10/24 20:26:46  flowers
 * Change       fixed spelling of npts
 * Change
 * Revision 1.1  1996/10/22  20:45:03  flowers
 * Initial revision
 *
 *
 **************************************************************************
 */


/* USRLIB MODULE INFORMATION

	MODULE NAME: subvtslope
	MODULE RETURN TYPE: int 
	ARGUMENTS:
		devname,	char *,	Input
		drain,	int ,	Input
		gate,	int ,	Input
		source,	int ,	Input
		well,	int ,	Input
		sub,	int ,	Input
		chuck_pin,	int ,	Input
		polarity,	int ,	Input
		Vlo,	double ,	Input
		Vhi,	double ,	Input
		npts,	int ,	Input
		Vds,	double ,	Input
		Igcomp,	double ,	Input
		Idcomp,	double ,	Input
		Vb,	double ,	Input
		isubchk,	double ,	Input
		range,	double ,	Input
		lo_range,	double ,	Input
		mode,	double ,	Input
		delay_val,	double ,	Input
		debug,	int ,	Input
		Subslope,	double *,	Output
		SubVT,	double *,	Output
		IdVT,	double *,	Output
		arstat,	double *,	Output
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#include <PARLIB_proto.h>
	END USRLIB MODULE INFORMATION
*/
int subvtslope(char * devname, int  drain, int  gate, int  source, int  well, int  sub, int  chuck_pin, int  polarity, double  Vlo, double  Vhi, int  npts, double  Vds, double  Igcomp, double  Idcomp, double  Vb, double  isubchk, double  range, double  lo_range, double  mode, double  delay_val, int  debug, double * Subslope, double * SubVT, double * IdVT, double * arstat);


/* 
 
    vgsat.c
  
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
 * File:     $Source: /cm/test/build/S600/v420/PARLIB/RCS/vgsat.c,v $
 * Current $Revision: 1.11 $
 * Curent     $State: REL $
 * Last Rev    $Date: 1998/03/13 18:11:04 $
 *
 * Change       $Log: vgsat.c,v $
 * Change       Revision 1.11  1998/03/13 18:11:04  nagy
 * Change       Fixed chuck name in parameter list
 * Change
 * Change       Revision 1.10  1998/02/04 21:55:32  nagy
 * Change        Fixed chuck name PR6218
 * Change
 * Change       Revision 1.9  1998/01/08 15:22:05  witzke
 * Change       Included Manual text as help.
 * Change       Tab conversion with: expand -4,16,28 file
 * Change
 * Change       Revision 1.8  1997/07/21 16:05:35  nagy
 * Change       Fixed problem in symmetry test
 * Change
 * Change       Revision 1.7  1997/07/08 17:46:20  nagy
 * Change       Added support for 3 terminal devices
 * Change
 * Change       Revision 1.6  1997/06/07 18:43:29  nagy
 * Change       Added PARLIB_proto.h for function prototype.
 * Change
 * Change       Revision 1.5  1997/06/06 20:17:43  nagy
 * Change       General cleanup for initial release.
 * Change
 * Change       Revision 1.4  1997/01/21 21:47:01  williamson
 * Change       Commented out call to kdelay...This needs to be checked out
 * Change
 * Revision 1.3  1997/01/10  21:33:11  flowers
 * Added enable( TIMER1 )
 * added test for valid polarity
 *
 * Revision 1.2  1996/12/18  19:17:19  flowers
 * delay() -> rdelay()
 * W_FLOAT -> W_DOUBLE
 * devint -> execut at end
 *
 * Use bulk_connect for substrate connection
 *
 * Change       Revision 1.1  1996/10/22 20:48:33  flowers
 * Change       Initial revision
 * Change
 *
 **************************************************************************
 */


/* USRLIB MODULE INFORMATION

	MODULE NAME: vgsat
	MODULE RETURN TYPE: int 
	ARGUMENTS:
		devname,	char *,	Input
		drain,	int ,	Input
		gate,	int ,	Input
		source,	int ,	Input
		well,	int ,	Input
		sub,	int ,	Input
		chuck_pin,	int ,	Input
		polarity,	int ,	Input
		ipgm,	double ,	Input
		vlim,	double ,	Input
		Vbs,	double ,	Input
		symmetry,	int ,	Input
		range,	double ,	Input
		lo_range,	double ,	Input
		mode,	double ,	Input
		delay_val,	double ,	Input
		debug,	int ,	Input
		vsat,	double *,	Output
		vsatsym,	double *,	Output
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#include <PARLIB_proto.h>
	END USRLIB MODULE INFORMATION
*/
int vgsat(char * devname, int  drain, int  gate, int  source, int  well, int  sub, int  chuck_pin, int  polarity, double  ipgm, double  vlim, double  Vbs, int  symmetry, double  range, double  lo_range, double  mode, double  delay_val, int  debug, double * vsat, double * vsatsym);


/* 
 
    vtlinext.c
  
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
 * File:     $Source: /cm/test/build/S600/v420/PARLIB/RCS/vtlinext.c,v $
 * Current $Revision: 1.13 $
 * Curent     $State: REL $
 * Last Rev    $Date: 1998/03/17 15:26:40 $
 *
 * Change       $Log: vtlinext.c,v $
 * Change       Revision 1.13  1998/03/17 15:26:40  nagy
 * Change       Repaired PR 6218 problem issue
 * Change
 * Change       Revision 1.12  1998/03/13 18:10:37  nagy
 * Change       Fixed chuck name in parameter list
 * Change
 * Change       Revision 1.11  1998/02/04 21:55:42  nagy
 * Change        Fixed chuck name PR6218
 * Change
 * Change       Revision 1.10  1998/01/08 15:22:05  witzke
 * Change       Included Manual text as help.
 * Change       Tab conversion with: expand -4,16,28 file
 * Change
 * Change       Revision 1.9  1997/07/22 18:29:34  nagy
 * Change       Removed garbage at beginning of file
 * Change
 * Change       Revision 1.8  1997/07/21 16:09:53  nagy
 * Change       Change check for gmmin to use 1e-9 and added fabs to vgs scan
 * Change
 * Change       Revision 1.7  1997/07/08 17:48:23  nagy
 * Change       *** empty log message ***
 * Change
 * Change       Revision 1.6  1997/06/07 18:24:18  nagy
 * Change       Added PARLIB_proto.h for idarray function prototype
 * Change
 * Change       Revision 1.5  1997/06/06 20:17:58  nagy
 * Change       General cleanup for initial release.
 * Change
 * Change       Revision 1.4  1997/01/10 21:45:30  flowers
 * Change       added test for valid polarity
 * Change
 * Revision 1.3  1997/01/07  19:29:18  flowers
 * Added call to enable(TIMER1) so imeast would work.
 * In internal call to idarray, turned off debugging to keep
 * from resetting the timer.
 *
 * Revision 1.2  1996/12/18  19:36:25  flowers
 * W_FLOAT -> W_DOUBLE
 * devint -> execut
 *
 * Change       Revision 1.1  1996/10/22 20:53:36  flowers
 * Change       Initial revision
 * Change
 *
 **************************************************************************
 */


/* USRLIB MODULE INFORMATION

	MODULE NAME: vtlinext
	MODULE RETURN TYPE: int 
	ARGUMENTS:
		devname,	char *,	Input
		drain,	int ,	Input
		gate,	int ,	Input
		source,	int ,	Input
		well,	int ,	Input
		sub,	int ,	Input
		chuck_pin,	int ,	Input
		polarity,	int ,	Input
		Vlo,	double ,	Input
		Vhi,	double ,	Input
		npts,	int ,	Input
		Vds,	double ,	Input
		Igcomp,	double ,	Input
		Idcomp,	double ,	Input
		Vb,	double ,	Input
		isubchk,	double ,	Input
		range,	double ,	Input
		lo_range,	double ,	Input
		mode,	double ,	Input
		delay_val,	double ,	Input
		debug,	int ,	Input
		gm,	double *,	Output
		vt,	double *,	Output
		cor_coef,	double *,	Output
		arstat, double *,	Output
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#include <PARLIB_proto.h>
	END USRLIB MODULE INFORMATION
*/
int vtlinext(char * devname, int  drain, int  gate, int  source, int  well, int  sub, int  chuck_pin, int  polarity, double  Vlo, double  Vhi, int  npts, double  Vds, double  Igcomp, double  Idcomp, double  Vb, double  isubchk, double  range, double  lo_range, double  mode, double  delay_val, int  debug, double * gm, double * vt, double * cor_coef, double * arstat);


