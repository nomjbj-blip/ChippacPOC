/* BD350 function prototype and KITT header file */

/* COPY OF USRLIB SETTINGS INFORMATION */
/* [VISIBILITY] */
/* HIDDEN=0 */
/* [DEPENDENCIES] */
/* LIBS=-lA07 -lHP4284 -lktest -lLBC5 */
/* OBJECTS= */
/* HEADERS= */
/* [NON-KULT SEARCH PATH] */
/* PATHS= */

/* USRLIB MODULE INFORMATION

	MODULE NAME: BD350_id1
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 7
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		vgs,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vbs,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double BD350_id1(int drain, int gate, int source, int subst, double vgs, double vds, double vbs);


/* USRLIB MODULE INFORMATION

	MODULE NAME: BD350_leak_float14_dren
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 17
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo1,	int,	Input,	,	,	
		f1,	int,	Input,	,	,	
		f2,	int,	Input,	,	,	
		f3,	int,	Input,	,	,	
		f4,	int,	Input,	,	,	
		f5,	int,	Input,	,	,	
		f6,	int,	Input,	,	,	
		f7,	int,	Input,	,	,	
		f8,	int,	Input,	,	,	
		f9,	int,	Input,	,	,	
		f10,	int,	Input,	,	,	
		f11,	int,	Input,	,	,	
		f12,	int,	Input,	,	,	
		f13,	int,	Input,	,	,	
		f14,	int,	Input,	,	,	
		v,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <usrlib_proto.h>
	END USRLIB MODULE INFORMATION
*/
double BD350_leak_float14_dren(int hi, int lo1, int f1, int f2, int f3, int f4, int f5, int f6, int f7, int f8, int f9, int f10, int f11, int f12, int f13, int f14, double v);


/* 
 
    BD350_PARLIB_id1.c
  
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
 * File:     $Source: /cm/test/build/S600/v420/PARLIB/RCS/BD350_PARLIB_id1.c,v $
 * Current $Revision: 1.10 $
 * Curent     $State: REL $
 * Last Rev    $Date: 1998/03/13 18:10:12 $
 *
 * Change       $Log: BD350_PARLIB_id1.c,v $
 * Change       Revision 1.10  1998/03/13 18:10:12  nagy
 * Change       Fixed chuck name in parameter list
 * Change
 * Change       Revision 1.9  1998/02/04 21:54:51  nagy
 * Change        Fixed chuck name PR6218
 * Change
 * Change       Revision 1.8  1998/01/08 15:22:05  witzke
 * Change       Included Manual text as help.
 * Change       Tab conversion with: expand -4,16,28 file
 * Change
 * Change       Revision 1.7  1997/07/21 16:06:42  nagy
 * Change       Changed debug file definition
 * Change
 * Change       Revision 1.6  1997/07/08 17:50:12  nagy
 * Change       Added polarity test
 * Change
 * Change       Revision 1.5  1997/06/07 18:42:53  nagy
 * Change       Added PARLIB_proto.h for function prototype.
 * Change
 * Change       Revision 1.4  1997/06/06 20:16:26  nagy
 * Change       General cleanup for initial release.
 * Change
 * Change       Revision 1.3  1997/01/10 20:20:09  flowers
 * Change       Added enable(TIMER1)
 * Change       Added check for valid polarity
 * Change
 * Revision 1.2  1996/12/18  17:20:37  flowers
 * Drain on SMU1 and gate on SMU2 for consistancy
 * Used bulk_connect instead of different connection scheme
 * rdelay() instead of delay()
 * W_FLOAT to W_DOUBLE
 *
 * Change       Revision 1.1  1996/10/22 20:33:42  flowers
 * Change       Initial revision
 * Change
 *
 **************************************************************************
 */
/* USRLIB MODULE INFORMATION

	MODULE NAME: BD350_PARLIB_id1
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
		Vbs,	double ,	Input
		Vds,	double ,	Input
		Igcomp,	double ,	Input
		Idcomp,	double ,	Input
		isubchk,	double ,	Input
		range,	double ,	Input
		lo_range,	double ,	Input
		mode,	double ,	Input
		delay_val,	double ,	Input
		debug,	double ,	Input
		ID,	double *,	Output
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#include <PARLIB_proto.h>
	END USRLIB MODULE INFORMATION
*/
int BD350_PARLIB_id1(char * devname, int  drain, int  gate, int  source, int  well, int  sub, int  chuck_pin, int  polarity, double  Vgs, double  Vbs, double  Vds, double  Igcomp, double  Idcomp, double  isubchk, double  range, double  lo_range, double  mode, double  delay_val, double  debug, double * ID);


/* USRLIB MODULE INFORMATION

	MODULE NAME: BD350_pn2swp_swp_log
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 9
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		vstart,	double,	Input,	,	,	
		vstop,	double,	Input,	,	,	
		nstep,	int,	Input,	,	,	
		ipgm,	double,	Input,	,	,	
		udelay,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "lptdef.h"
#include "LBC5_proto.h"
#include <stdlib.h>
#include <ktxe_proto.h>
#include <ksox_def.h>

	END USRLIB MODULE INFORMATION
*/
double BD350_pn2swp_swp_log(int hi, int lo, int subst, double vstart, double vstop, int nstep, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: beta2_sub
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 13
	ARGUMENTS:
		em,	int,	Input,	,	,	
		base,	int,	Input,	,	,	
		coll,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		addcon,	int,	Input,	,	,	
		ic,	double,	Input,	,	,	
		vce,	double,	Input,	,	,	
		vsub,	double,	Input,	,	,	
		vsub_ilimit,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
		debug,	int,	Input,	,	,	
		vbeout,	double *,	Output,	,	,	
		icout,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double  beta2_sub(int, int, int, int, int, double, double, double, double, char, int, double *, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: beta2_Vcsub
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 9
	ARGUMENTS:
		em,	int,	Input,	,	,	
		base,	int,	Input,	,	,	
		coll,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		ic,	double,	Input,	,	,	
		vce,	double,	Input,	,	,	
		vbeout,	double *,	Output,	,	,	
		icout,	double *,	Output,	,	,	
		type,	char,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double  beta2_Vcsub(int, int, int, int, double, double, double *, double *, char);

/* USRLIB MODULE INFORMATION

	MODULE NAME: bv2_v2i
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 13
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		lo1,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		vstart,	double,	Input,	,	,	
		vstop,	double,	Input,	,	,	
		nstep,	int,	Input,	,	,	
		ipgm,	double,	Input,	,	,	
		udelay,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
		debug,	int,	Input,	,	,	
		dum1,	double *,	Output,	,	,	
		dum2,	double *,	Output,	,	,	
	INCLUDES:
#include <sys/systeminfo.h>
#include <sys/utsname.h>
#include <stdlib.h>
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>

typedef struct{
    int command;
    char *string;
}Info;

Info hn[] = {
    SI_SYSNAME,          "SI_SYSNAME",
    SI_HOSTNAME,        "SI_HOSTNAME",
    SI_RELEASE,           "SI_RELEASE",
    SI_VERSION,           "SI_VERSION",
    SI_MACHINE,           "SI_MACHINE",
    SI_ARCHITECTURE,    "SI_ARCHITECTURE",
    SI_HW_PROVIDER,     "SI_HW_PROVIDER",
    SI_HW_SERIAL,        "SI_HW_SERIAL",
    SI_SRPC_DOMAIN,     "SI_SRPC_DOMAIN",
    0,                  NULL
};
	END USRLIB MODULE INFORMATION
*/
double  bv2_v2i(int, int, int, int, double, double, int, double, double, char, int, double *, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: bvv1i1_dio
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 18
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		top,	int,	Input,	,	,	
		bot,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		top1,	int,	Input,	,	,	
		area,	double,	Input,	,	,	
		per,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
		btype,	char,	Input,	,	,	
		imax,	double,	Input,	,	,	
		top1v,	char,	Input,	,	,	
		vmin,	double,	Input,	,	,	
		vmax,	double,	Input,	,	,	
		multiplier,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		result,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#include <tidp1.h>
	END USRLIB MODULE INFORMATION
*/
void  bvv1i1_dio(char *, int, int, int, int, double, double, char, char, double, char, double, double, double, double, double, double, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: cap_2spo3_cap
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 22
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		area,	double,	Input,	,	,	
		per,	double,	Input,	,	,	
		tty,	char,	Input,	,	,	
		bty,	char,	Input,	,	,	
		vacc,	double,	Input,	,	,	
		vinv,	double,	Input,	,	,	
		freq,	double,	Input,	,	,	
		sig,	double,	Input,	,	,	
		stray,	double,	Input,	,	,	
		integ,	int,	Input,	,	,	
		ddebug,	int,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		ca,	double *,	Output,	,	,	
		ta,	double *,	Output,	,	,	
		ci,	double *,	Output,	,	,	
		ti,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#define CRTLIM 1.0E-6		                              
#include <tidp1.h>
#include "ktxe_types.h"
#include "COM_usrlib.h"
#define MAX(a,b)	((a)>(b) ? (a) : (b))
#include <cmtr_hp4284.h>
#define __4284__

#define Eo 8.854E-14
#define PI 3.14159265358979323846264338327950288419716939937510582
#define DONTCARE 0
#define CV_XMT 11
	END USRLIB MODULE INFORMATION
*/
void  cap_2spo3_cap(char *, int, int, int, int, double, double, char, char, double, double, double, double, double, int, int, double, double, double *, double *, double *, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: cap_4284_1Mhz_500mv_confirm
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 6
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		vbias,	double,	Input,	,	,	
		c2,	double *,	Output,	,	,	
		z2,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <LBC5_proto.h>

#define CRTLIM 1.0E-6
#include <tidp1.h>
#include "ktxe_types.h"
#include "COM_usrlib.h"
#define MAX(a,b)        ((a)>(b) ? (a) : (b))
#include <cmtr_hp4284.h>
#define __4284__

#define Eo 8.854E-14
#define PI 3.14159265358979323846264338327950288419716939937510582
#define DONTCARE 0
#define CV_XMT 11


	END USRLIB MODULE INFORMATION
*/
double  cap_4284_1Mhz_500mv_confirm(int, int, int, double, double *, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: erase_eeprom_bd
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 6
	ARGUMENTS:
		drn,	int,	Input,	,	,	
		cg,	int,	Input,	,	,	
		src,	int,	Input,	,	,	
		tun,	int,	Input,	,	,	
		vtun,	double,	Input,	,	,	
		te,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "COM_usrlib.h"
	END USRLIB MODULE INFORMATION
*/
void erase_eeprom_bd(int drn, int cg, int src, int tun, double vtun, int te);


/* USRLIB MODULE INFORMATION

	MODULE NAME: g_sw
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 28
	ARGUMENTS:
		Vstart,	double,	Input,	,	,	
		Vstop,	double,	Input,	,	,	
		Vstep,	double,	Input,	,	,	
		Hold,	double,	Input,	,	,	
		Delay,	double,	Input,	,	,	
		Hc,	double,	Input,	,	,	
		Mc,	double,	Input,	,	,	
		Lc,	double,	Input,	,	,	
		Hr,	double,	Input,	,	,	
		Mr,	double,	Input,	,	,	
		Lr,	double,	Input,	,	,	
		M_i,	double,	Input,	,	,	
		M_q,	double,	Input,	,	,	
		Intrange,	int,	Input,	,	,	
		Top,	int,	Input,	,	,	
		Bot,	int,	Input,	,	,	
		Sub,	int,	Input,	,	,	
		Chuck,	int,	Input,	,	,	
		Area,	double,	Input,	,	,	
		Per,	double,	Input,	,	,	
		Tty,	char,	Input,	,	,	
		Bty,	char,	Input,	,	,	
		Vlf,	double *,	Output,	,	,	
		Vfail,	double *,	Output,	,	,	
		Ifail,	double *,	Output,	,	,	
		Qbd,	double *,	Output,	,	,	
		first,	double *,	Output,	,	,	
		second,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <stdlib.h>
#include <par_util.h>

#include <ktxe_proto.h>
#include <ksox_def.h>
#include <kui_proto.h>

#define MAX(a,b)	((a)>(b) ? (a) : (b))
#define CDELAY  125.E-12
#define ILEAK   20.E-12 

	END USRLIB MODULE INFORMATION
*/
void  g_sw(double, double, double, double, double, double, double, double, double, double, double, double, double, int, int, int, int, int, double, double, char, char, double *, double *, double *, double *, double *, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: leak4_bd
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 5
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo1,	int,	Input,	,	,	
		lo2,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		v,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double  leak4_bd(int, int, int, int, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: leak6_3v1i
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 11
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo1,	int,	Input,	,	,	
		lo2,	int,	Input,	,	,	
		lo3,	int,	Input,	,	,	
		lo4,	int,	Input,	,	,	
		lo5,	int,	Input,	,	,	
		i1,	double,	Input,	,	,	
		fv2,	double,	Input,	,	,	
		fv3,	double,	Input,	,	,	
		fv4,	double,	Input,	,	,	
		i1_vlimit,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double  leak6_3v1i(int, int, int, int, int, int, double, double, double, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: leakage2
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 22
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		top,	int,	Input,	,	,	
		bot,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		area,	double,	Input,	,	,	
		per,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
		vrev,	double,	Input,	,	,	
		ilim,	double,	Input,	,	,	
		icomp,	double,	Input,	,	,	
		irange,	double,	Input,	,	,	
		delaytime,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		log,	double,	Input,	,	,	
		debug,	double,	Input,	,	,	
		dum1,	double *,	Output,	,	,	
		dum2,	double *,	Output,	,	,	
		Irev,	double *,	Output,	,	,	
		Ip,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#include "kdf.h"
#include "ktxe_types.h"
#include "ksox_def.h"
#include "COM_usrlib.h"
	END USRLIB MODULE INFORMATION
*/
void  leakage2(char *, int, int, int, int, double, double, char, double, double, double, double, double, double, double, double, double, double, double *, double *, double *, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: pgm_eeprom_bd
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 6
	ARGUMENTS:
		drn,	int,	Input,	,	,	
		cg,	int,	Input,	,	,	
		src,	int,	Input,	,	,	
		tun,	int,	Input,	,	,	
		vcg,	double,	Input,	,	,	
		tp,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "COM_usrlib.h"
	END USRLIB MODULE INFORMATION
*/
void pgm_eeprom_bd(int drn, int cg, int src, int tun, double vcg, int tp);


/* USRLIB MODULE INFORMATION

	MODULE NAME: pn2swp_fbs
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 11
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		sub_v,	double,	Input,	,	,	
		sub_ilimit,	double,	Input,	,	,	
		vstart,	double,	Input,	,	,	
		vstop,	double,	Input,	,	,	
		nstep,	int,	Input,	,	,	
		ipgm,	double,	Input,	,	,	
		udelay,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double  pn2swp_fbs(int, int, int, double, double, double, double, int, double, double, char);

/* USRLIB MODULE INFORMATION

	MODULE NAME: pn2swp_pt12
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 9
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		vstart,	double,	Input,	,	,	
		vstop,	double,	Input,	,	,	
		nstep,	int,	Input,	,	,	
		ipgm,	double,	Input,	,	,	
		udelay,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
	INCLUDES:
#include <sys/systeminfo.h>
#include <sys/utsname.h>
#include <stdlib.h>
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>

typedef struct{
    int command;
    char *string;
}Info;
Info info[] = {
    SI_SYSNAME,          "SI_SYSNAME",
    SI_HOSTNAME,        "SI_HOSTNAME",
    SI_RELEASE,           "SI_RELEASE",
    SI_VERSION,           "SI_VERSION",
    SI_MACHINE,           "SI_MACHINE",
    SI_ARCHITECTURE,    "SI_ARCHITECTURE",
    SI_HW_PROVIDER,     "SI_HW_PROVIDER",
    SI_HW_SERIAL,        "SI_HW_SERIAL",
    SI_SRPC_DOMAIN,     "SI_SRPC_DOMAIN",
    0,                  NULL
};
	END USRLIB MODULE INFORMATION
*/
double  pn2swp_pt12(int, int, int, double, double, int, double, double, char);

/* USRLIB MODULE INFORMATION

	MODULE NAME: pn2swp_vramp
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 9
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		vstart,	double,	Input,	,	,	
		vstop,	double,	Input,	,	,	
		nstep,	int,	Input,	,	,	
		ipgm,	double,	Input,	,	,	
		udelay,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "lptdef.h"
#include "LBC5_proto.h"
#include <stdlib.h>
#include <ktxe_proto.h>
#include <ksox_def.h>

	END USRLIB MODULE INFORMATION
*/
double  pn2swp_vramp(int, int, int, double, double, int, double, double, char);

/* USRLIB MODULE INFORMATION

	MODULE NAME: RES_I2V
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 5
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		itest,	double,	Input,	,	,	
		TT,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double RES_I2V(int hi, int lo, int subst, double itest, double * TT);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Rsheet_wid_r1
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 19
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		forcehi,	int,	Input,	,	,	
		meashi,	int,	Input,	,	,	
		forcelo,	int,	Input,	,	,	
		measlo,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		bulk,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		length,	double,	Input,	,	,	
		width,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
		iforce,	double,	Input,	,	,	
		vlimit,	double,	Input,	,	,	
		vsub,	double,	Input,	,	,	
		delaytime,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		result,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#include <tidp1.h>
	END USRLIB MODULE INFORMATION
*/
void  Rsheet_wid_r1(char *, int, int, int, int, int, int, int, double, double, char, double, double, double, double, double, double, double, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: vt_lin
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 15
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		w,	double,	Input,	,	,	
		l,	double,	Input,	,	,	
		vlow,	double,	Input,	,	,	
		vhigh,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vbs,	double,	Input,	,	,	
		ithr,	double,	Input,	,	,	
		vstep,	double,	Input,	,	,	
		npts,	int,	Input,	,	,	
		slope,	double *,	Output,	,	,	
		kflag,	int *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>

#include <par_util.h>
#include <stdlib.h>
#include <ktxe_proto.h>
#include <ksox_def.h>
#include <kui_proto.h>

#define MAX(a,b)	((a)>(b) ? (a) : (b))
#define CDELAY  125.E-12
#define ILEAK   20.E-12 
	END USRLIB MODULE INFORMATION
*/
double  vt_lin(int, int, int, int, double, double, double, double, double, double, double, double, int, double *, int *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: vtati_bd
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 10
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		vlow,	double,	Input,	,	,	
		vhigh,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vbs,	double,	Input,	,	,	
		ithr,	double,	Input,	,	,	
		niter,	int,	Input,	,	,	
	INCLUDES:
#include <math.h>	                
#include "lptdef.h"
#include "PARLIB400_proto.h"

#define MAX(a,b)	((a)>(b) ? (a) : (b))

	END USRLIB MODULE INFORMATION
*/
double vtati_bd(int drain, int gate, int source, int subst, double vlow, double vhigh, double vds, double vbs, double ithr, int niter);


/* USRLIB MODULE INFORMATION

	MODULE NAME: vtati_loop_bd
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 11
	ARGUMENTS:
		CG,	int,	Input,	,	,	
		DRAIN,	int,	Input,	,	,	
		SOURCE,	int,	Input,	,	,	
		TG,	int,	Input,	,	,	
		Loop,	long,	Input,	,	,	
		pgm_volt,	double,	Input,	,	,	
		pgm_time,	double,	Input,	,	,	
		erase_volt,	double,	Input,	,	,	
		erase_time,	double,	Input,	,	,	
		mod,	char *,	Input,	,	,	
		dut,	char *,	Input,	,	,	
	INCLUDES:
#include <math.h>	                
#include "lptdef.h"
#include "PARLIB400_proto.h"
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include "COM_usrlib.h"
#include "LBC5_proto.h"
#define MAX(a,b)	((a)>(b) ? (a) : (b))

static char const vcid[] ="$Id: Local $";
double leak4_local( int hi, int  lo1,int  lo2,int  subst, double v )
{
    double    i;                              
    double leak_fnc;                                
    double sign;
    if (v >= 0) 
        sign = 1.0;
    else
        sign = -1.0;

    if (subst > 0) {
        conpin (SMU1L, lo1, lo2, subst, GND, KI_EOC);
    }
    else {
        conpin (SMU1L,lo1, lo2, GND, KI_EOC);
    }
    conpin (SMU1, hi, KI_EOC);
    lorangei(SMU1, 100.0e-12);
    forcev (SMU1, v);
    delay (500);
    intgi (SMU1, &i);
    (void)devint();
    leak_fnc = sign * fabs(i);
    printf("leak func is %1.4e\n",leak_fnc);
    return leak_fnc;
}                        






double vtati_local( int drain, int gate, int source, int subst, double vlow, double vhigh, double vds, double vbs, double ithr, int niter )
{
                        
 
    extern     int fndtrg();                          

    int     lowt;                                         
    int    num;                                              
    double    avmax;                                                       
    double    sdelay;                                                     
    double    iltd;                                            
    double     crtlmt = 10.E-6;                       
    double    vtati_fnc;                                        
    double    trigger_current;                                  


                                    

    num = niter;
    if (num < 2) num = 2;
    if (num > 16) num = 16;

    if (subst < 1)    {        
        conpin (GND, SMU1L, SMU2L, source, KI_EOC);
    }
    else if (fabs(vbs) < .9e-3) {     
        conpin (GND, SMU1L, SMU2L, source, subst, KI_EOC);
    }
     else {
        conpin (GND, SMU1L, SMU2L, SMU3L, source, KI_EOC);
        conpin (SMU3, subst, KI_EOC);
    }
    conpin  (SMU2, gate, KI_EOC);
    conpin  (SMU1, drain, KI_EOC);                                               

    lowt = fndtrg(vlow, vhigh);
    trigger_current = ithr;
    if (lowt) {
        trigig(SMU1, trigger_current);
    }
    else {
        trigil(SMU1, trigger_current);
    }
    limiti (SMU2, crtlmt);


                                                    

    avmax = MAX(fabs(vlow), fabs(vhigh));
    sdelay = tdelay(1, crtlmt, avmax);
                        

                      

    if (subst > 0 && fabs(vbs) >= .9e-3) forcev (SMU3, vbs);
      forcev (SMU1, vds);
    searchv (SMU2, vlow, vhigh, num, sdelay, &vtati_fnc);
    intgi (SMU2, &iltd);
    devint();


                                                                                                                                              

    if (fabs(vlow - vtati_fnc) <= 1.e-3) vtati_fnc = 1.e+21;
    if (fabs(vhigh - vtati_fnc) <= 1.e-3) vtati_fnc = 2.e+21;
    if (iltd == 7.0e+22) vtati_fnc = 4.0e+21;

    return(vtati_fnc);

                        
} 		                    






















void pgm_eeprom_local( int drn, int cg, int src, int tun, double vcg, int tp )
{
                        
    extern int devint();
    double    v;                              
    double    pgm_eeprom_fnc;                                

                         

    conpin( SMU1L, drn, src, tun, GND, KI_EOC);
    conpin (SMU1, cg, KI_EOC);


                        

    forcev(SMU1, vcg);
    delay (tp);

                                   
    (void)devint();

    return;
                        
} 		                      
void erase_eeprom_local( int drn, int cg, int src, int tun, double vtun, int te )
{
                        
    extern int devint();
    double    v;                              
    double    erase_eeprom_fnc;                                

                         

    conpin( SMU1L, drn, cg, src, GND, KI_EOC);
    conpin (SMU1, tun, KI_EOC);


                        

     forcev(SMU1, vtun);
     delay (te);

                                 
    (void)devint();

    return;
                        
} 		                        


















	END USRLIB MODULE INFORMATION
*/
double vtati_loop_bd(int CG, int DRAIN, int SOURCE, int TG, long Loop, double pgm_volt, double pgm_time, double erase_volt, double erase_time, char * mod, char * dut);


/* USRLIB MODULE INFORMATION

	MODULE NAME: vtext5_sw
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 16
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		body,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		w,	double,	Input,	,	,	
		l,	double,	Input,	,	,	
		vlow,	double,	Input,	,	,	
		vhigh,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vbs,	double,	Input,	,	,	
		ithr,	double,	Input,	,	,	
		vstep,	double,	Input,	,	,	
		npts,	int,	Input,	,	,	
		slope,	double *,	Output,	,	,	
		kflag,	int *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#include <stdlib.h>
#include <ktxe_proto.h>
#include <ksox_def.h>
#include <kui_proto.h>
	END USRLIB MODULE INFORMATION
*/
double  vtext5_sw(int, int, int, int, int, double, double, double, double, double, double, double, double, int, double *, int *);

