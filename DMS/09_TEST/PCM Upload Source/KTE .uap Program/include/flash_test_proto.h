/* flash_test function prototype and KITT header file */

/* COPY OF USRLIB SETTINGS INFORMATION */
/* [VISIBILITY] */
/* HIDDEN=0 */
/* [DEPENDENCIES] */
/* LIBS=-lHP8110u -lktest -lLBC5 */
/* OBJECTS= */
/* HEADERS= */
/* [NON-KULT SEARCH PATH] */
/* PATHS= */

/* USRLIB MODULE INFORMATION

	MODULE NAME: bvd
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 10
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		vgstart,	double,	Input,	,	,	
		vgstop,	double,	Input,	,	,	
		vgstep,	double,	Input,	,	,	
		icomp,	double,	Input,	,	,	
		vbd,	double *,	Output,	,	,	
		ibd,	double *,	Output,	,	,	
		debug_print,	int,	Input,	,	0,	1
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void bvd(int hi, int lo, int well, double vgstart, double vgstop, double vgstep, double icomp, double * vbd, double * ibd, int debug_print);


/* USRLIB MODULE INFORMATION

	MODULE NAME: bvd_time
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 14
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		vgstart,	double,	Input,	,	,	
		vgstop,	double,	Input,	,	,	
		vgstep,	double,	Input,	,	,	
		icomp,	double,	Input,	,	,	
		area,	double,	Input,	,	,	
		delaytime,	double,	Input,	,	,	
		vbd,	double *,	Output,	,	,	
		ibd,	double *,	Output,	,	,	
		bd_time,	double *,	Output,	,	,	
		Qbd,	double *,	Output,	,	,	
		debug_print,	int,	Input,	,	0,	1
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void bvd_time(int hi, int lo, int well, double vgstart, double vgstop, double vgstep, double icomp, double area, double delaytime, double * vbd, double * ibd, double * bd_time, double * Qbd, int debug_print);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Dist_Eon
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 10
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		ga_delay,	double,	Input,	,	,	
		ga_high,	double,	Input,	,	,	
		ga_width,	double,	Input,	,	,	
		ga_rise,	double,	Input,	,	,	
		ga_fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
void Dist_Eon(int drain, int gate, int source, int well, double ga_delay, double ga_high, double ga_width, double ga_rise, double ga_fall, double holdtime);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Dist_Eon_k
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 10
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		ga_delay,	double,	Input,	,	,	
		ga_high,	double,	Input,	,	,	
		ga_width,	double,	Input,	,	,	
		ga_rise,	double,	Input,	,	,	
		ga_fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
void Dist_Eon_k(int drain, int gate, int source, int well, double ga_delay, double ga_high, double ga_width, double ga_rise, double ga_fall, double holdtime);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Dist_Eon_r1
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 13
	ARGUMENTS:
		gate,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		ga_delay,	double,	Input,	,	,	
		ga_high,	double,	Input,	,	,	
		ga_width,	double,	Input,	,	,	
		ga_rise,	double,	Input,	,	,	
		ga_fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
		vb_high,	double,	Input,	,	,	
		vb_width,	double,	Input,	,	,	
		TRGholdtime,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
void Dist_Eon_r1(int gate, int drain, int source, int well, double ga_delay, double ga_high, double ga_width, double ga_rise, double ga_fall, double holdtime, double vb_high, double vb_width, double TRGholdtime);


/* USRLIB MODULE INFORMATION

	MODULE NAME: E1s_shape2
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 21
	ARGUMENTS:
		CG,	int,	Input,	-1,	,	
		DRAIN,	int,	Input,	-1,	,	
		SOURCE,	int,	Input,	-1,	,	
		TG,	int,	Input,	-1,	,	
		lo1,	int,	Input,	-1,	,	
		lo2,	int,	Input,	-1,	,	
		gnd,	int,	Input,	-1,	,	
		gnd1,	int,	Input,	-1,	,	
		Loop,	long,	Input,	1,	,	
		pgm_volt,	double,	Input,	17,	,	
		pgm_time,	double,	Input,	10e-3,	,	
		erase_volt,	double,	Input,	5,	,	
		erase_time,	double,	Input,	10e-3,	,	
		pgm1_volt,	double,	Input,	5,	,	
		pgm1_time,	double,	Input,	10e-3,	,	
		erase1_volt,	double,	Input,	0,	,	
		erase1_time,	double,	Input,	0,	,	
		drain_volt,	int,	Input,	0,	,	
		drain_time,	double,	Input,	0,	,	
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
#define MAX(a,b)	((a)>(b) ? (a) : (b))
#include <par_util.h>
static char const vcid[] ="$Id: Local $";
	END USRLIB MODULE INFORMATION
*/
double  E1s_shape2(int, int, int, int, int, int, int, int, long, double, double, double, double, double, double, double, double, int, double, char *, char *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: E2p2_spot
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 30
	ARGUMENTS:
		class,	char *,	Input,	"AA",	,	
		hi,	int,	Input,	-1,	,	
		lo,	int,	Input,	-1,	,	
		lo1,	int,	Input,	-1,	,	
		lo2,	int,	Input,	-1,	,	
		s1_hi,	int,	Input,	-1,	,	
		s2_hi,	int,	Input,	-1,	,	
		s1_lo,	int,	Input,	-1,	,	
		s2_lo,	int,	Input,	-1,	,	
		PS1,	int,	Input,	-1,	,	
		PS2,	int,	Input,	-1,	,	
		g1,	int,	Input,	-1,	,	
		g2,	int,	Input,	-1,	,	
		g3,	int,	Input,	-1,	,	
		hi_delay,	double,	Input,	0,	,	
		hi_height,	double,	Input,	1,	,	
		hi_width,	double,	Input,	1e-3,	,	
		lo_delay,	double,	Input,	0,	,	
		lo_height,	double,	Input,	2,	,	
		lo_width,	double,	Input,	1e-3,	,	
		lo1_delay,	double,	Input,	0,	,	
		lo1_height,	double,	Input,	3,	,	
		lo1_width,	double,	Input,	1e-3,	,	
		lo2_delay,	double,	Input,	0,	,	
		lo2_height,	double,	Input,	4,	,	
		lo2_width,	double,	Input,	1e-3,	,	
		rise,	double,	Input,	1e-8,	,	
		fall,	double,	Input,	1e-8,	,	
		PS1_V,	double,	Input,	0,	,	
		PS2_V,	double,	Input,	0,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
#define MAX(a,b)        ((a)>(b) ? (a) : (b))
	END USRLIB MODULE INFORMATION
*/
void  E2p2_spot(char *, int, int, int, int, int, int, int, int, int, int, int, int, int, double, double, double, double, double, double, double, double, double, double, double, double, double, double, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: E2p_spot
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 14
	ARGUMENTS:
		class,	char *,	Input,	"AA",	,	
		hi,	int,	Input,	-1,	,	
		lo,	int,	Input,	-1,	,	
		g1,	int,	Input,	-1,	,	
		g2,	int,	Input,	-1,	,	
		g3,	int,	Input,	-1,	,	
		hi_delay,	double,	Input,	1e-8,	,	
		hi_height,	double,	Input,	4,	,	
		hi_width,	double,	Input,	1e-3,	,	
		lo_delay,	double,	Input,	1e-8,	,	
		lo_height,	double,	Input,	5,	,	
		lo_width,	double,	Input,	1e-3,	,	
		rise,	double,	Input,	1e-8,	,	
		fall,	double,	Input,	1e-8,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
#define MAX(a,b)        ((a)>(b) ? (a) : (b))
	END USRLIB MODULE INFORMATION
*/
void  E2p_spot(char *, int, int, int, int, int, double, double, double, double, double, double, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: E2pL_shape2
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 21
	ARGUMENTS:
		cg,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		tg,	int,	Input,	,	,	
		c1_height,	double,	Input,	,	,	
		c1_width,	double,	Input,	,	,	
		c1_delay,	double,	Input,	,	,	
		c2_height,	double,	Input,	,	,	
		c2_width,	double,	Input,	,	,	
		c2_delay,	double,	Input,	,	,	
		c11_height,	double,	Input,	,	,	
		c11_width,	double,	Input,	,	,	
		c11_delay,	double,	Input,	,	,	
		c21_height,	double,	Input,	,	,	
		c21_width,	double,	Input,	,	,	
		c21_delay,	double,	Input,	,	,	
		cm_rise,	double,	Input,	,	,	
		cm_fall,	double,	Input,	,	,	
		loop,	int,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include <LBC5_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
#define CRTLIM 50.0e-3
#define MAX(a,b)   ((a)>(b) ? (a) : (b))

	END USRLIB MODULE INFORMATION
*/
void E2pL_shape2(int cg, int drain, int source, int sub, int tg, double c1_height, double c1_width, double c1_delay, double c2_height, double c2_width, double c2_delay, double c11_height, double c11_width, double c11_delay, double c21_height, double c21_width, double c21_delay, double cm_rise, double cm_fall, int loop, double holdtime);


/* USRLIB MODULE INFORMATION

	MODULE NAME: E4p2_spot
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 30
	ARGUMENTS:
		class,	char *,	Input,	"AA",	,	
		hi,	int,	Input,	-1,	,	
		lo,	int,	Input,	-1,	,	
		lo1,	int,	Input,	-1,	,	
		lo2,	int,	Input,	-1,	,	
		s1_hi,	int,	Input,	-1,	,	
		s2_hi,	int,	Input,	-1,	,	
		s1_lo,	int,	Input,	-1,	,	
		s2_lo,	int,	Input,	-1,	,	
		PS1,	int,	Input,	-1,	,	
		PS2,	int,	Input,	-1,	,	
		g1,	int,	Input,	-1,	,	
		g2,	int,	Input,	-1,	,	
		g3,	int,	Input,	-1,	,	
		hi_delay,	double,	Input,	0,	,	
		hi_height,	double,	Input,	1,	,	
		hi_width,	double,	Input,	1e-3,	,	
		lo_delay,	double,	Input,	0,	,	
		lo_height,	double,	Input,	2,	,	
		lo_width,	double,	Input,	1e-3,	,	
		lo1_delay,	double,	Input,	0,	,	
		lo1_height,	double,	Input,	3,	,	
		lo1_width,	double,	Input,	1e-3,	,	
		lo2_delay,	double,	Input,	0,	,	
		lo2_height,	double,	Input,	4,	,	
		lo2_width,	double,	Input,	1e-3,	,	
		rise,	double,	Input,	1e-8,	,	
		fall,	double,	Input,	1e-8,	,	
		PS1_V,	double,	Input,	0,	,	
		PS2_V,	double,	Input,	0,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
#define MAX(a,b)        ((a)>(b) ? (a) : (b))
	END USRLIB MODULE INFORMATION
*/
void  E4p2_spot(char *, int, int, int, int, int, int, int, int, int, int, int, int, int, double, double, double, double, double, double, double, double, double, double, double, double, double, double, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: E4p3_spot
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 30
	ARGUMENTS:
		class,	char *,	Input,	"AA",	,	
		hi,	int,	Input,	-1,	,	
		lo,	int,	Input,	-1,	,	
		lo1,	int,	Input,	-1,	,	
		lo2,	int,	Input,	-1,	,	
		s1_hi,	int,	Input,	-1,	,	
		s2_hi,	int,	Input,	-1,	,	
		s1_lo,	int,	Input,	-1,	,	
		s2_lo,	int,	Input,	-1,	,	
		smu_P1,	int,	Input,	-1,	,	
		smu_P2,	int,	Input,	-1,	,	
		g1,	int,	Input,	-1,	,	
		g2,	int,	Input,	-1,	,	
		g3,	int,	Input,	-1,	,	
		hi_delay,	double,	Input,	0,	,	
		hi_height,	double,	Input,	1,	,	
		hi_width,	double,	Input,	1e-3,	,	
		lo_delay,	double,	Input,	0,	,	
		lo_height,	double,	Input,	2,	,	
		lo_width,	double,	Input,	1e-3,	,	
		lo1_delay,	double,	Input,	0,	,	
		lo1_height,	double,	Input,	3,	,	
		lo1_width,	double,	Input,	1e-3,	,	
		lo2_delay,	double,	Input,	0,	,	
		lo2_height,	double,	Input,	4,	,	
		lo2_width,	double,	Input,	1e-3,	,	
		rise,	double,	Input,	1e-8,	,	
		fall,	double,	Input,	1e-8,	,	
		smu1_V,	double,	Input,	0,	,	
		smu2_V,	double,	Input,	0,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
#define MAX(a,b)        ((a)>(b) ? (a) : (b))
	END USRLIB MODULE INFORMATION
*/
double  E4p3_spot(char *, int, int, int, int, int, int, int, int, int, int, int, int, int, double, double, double, double, double, double, double, double, double, double, double, double, double, double, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: E4p_
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 48
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		n,	int,	Input,	,	,	
		hi_sync,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		lo_sync,	int,	Input,	,	,	
		lo1,	int,	Input,	,	,	
		lo1_sync,	int,	Input,	,	,	
		lo2,	int,	Input,	,	,	
		lo2_sync,	int,	Input,	,	,	
		Psmu1,	int,	Input,	,	,	
		Psmu2,	int,	Input,	,	,	
		gnd1,	int,	Input,	,	,	
		gnd2,	int,	Input,	,	,	
		hi_delay,	double,	Input,	,	,	
		hi_height,	double,	Input,	,	,	
		hi_width,	double,	Input,	,	,	
		s_hi_delay,	double,	Input,	,	,	
		s_hi_height,	double,	Input,	,	,	
		s_hi_width,	double,	Input,	,	,	
		lo_delay,	double,	Input,	,	,	
		lo_height,	double,	Input,	,	,	
		lo_width,	double,	Input,	,	,	
		s_lo_delay,	double,	Input,	,	,	
		s_lo_height,	double,	Input,	,	,	
		s_lo_width,	double,	Input,	,	,	
		lo1_delay,	double,	Input,	,	,	
		lo1_height,	double,	Input,	,	,	
		lo1_width,	double,	Input,	,	,	
		s_lo1_delay,	double,	Input,	,	,	
		s_lo1_height,	double,	Input,	,	,	
		s_lo1_width,	double,	Input,	,	,	
		lo2_delay,	double,	Input,	,	,	
		lo2_height,	double,	Input,	,	,	
		lo2_width,	double,	Input,	,	,	
		s_lo2_delay,	double,	Input,	,	,	
		s_lo2_height,	double,	Input,	,	,	
		s_lo2_width,	double,	Input,	,	,	
		rise1,	double,	Input,	,	,	
		rise2,	double,	Input,	,	,	
		fall1,	double,	Input,	,	,	
		fall2,	double,	Input,	,	,	
		s1_v,	double,	Input,	,	,	
		s1_limit,	double,	Input,	,	,	
		s2_v,	double,	Input,	,	,	
		s2_ilimit,	double,	Input,	,	,	
		flag1,	char,	Input,	,	,	
		mode,	char,	Input,	,	,	
		debug,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
void E4p_(int hi, int n, int hi_sync, int lo, int lo_sync, int lo1, int lo1_sync, int lo2, int lo2_sync, int Psmu1, int Psmu2, int gnd1, int gnd2, double hi_delay, double hi_height, double hi_width, double s_hi_delay, double s_hi_height, double s_hi_width, double lo_delay, double lo_height, double lo_width, double s_lo_delay, double s_lo_height, double s_lo_width, double lo1_delay, double lo1_height, double lo1_width, double s_lo1_delay, double s_lo1_height, double s_lo1_width, double lo2_delay, double lo2_height, double lo2_width, double s_lo2_delay, double s_lo2_height, double s_lo2_width, double rise1, double rise2, double fall1, double fall2, double s1_v, double s1_limit, double s2_v, double s2_ilimit, char flag1, char mode, int debug);


/* USRLIB MODULE INFORMATION

	MODULE NAME: E4p_Sm2Gn3
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 49
	ARGUMENTS:
		n,	int,	Input,	1,	,	
		period,	double,	Input,	,	,	
		hi,	int,	Input,	,	,	
		hi_sync,	int,	Input,	-1,	,	
		lo,	int,	Input,	,	,	
		lo_sync,	int,	Input,	-1,	,	
		lo1,	int,	Input,	,	,	
		lo1_sync,	int,	Input,	-1,	,	
		lo2,	int,	Input,	,	,	
		lo2_sync,	int,	Input,	-1,	,	
		Psmu1,	int,	Input,	-1,	,	
		Psmu2,	int,	Input,	-1,	,	
		gnd1,	int,	Input,	-1,	,	
		gnd2,	int,	Input,	-1,	,	
		hi_delay,	double,	Input,	1e-6,	,	
		hi_height,	double,	Input,	5,	,	
		hi_width,	double,	Input,	1e-3,	,	
		s_hi_delay,	double,	Input,	1e-6,	,	
		s_hi_height,	double,	Input,	5,	,	
		s_hi_width,	double,	Input,	1e-3,	,	
		lo_delay,	double,	Input,	1e-6,	,	
		lo_height,	double,	Input,	5,	,	
		lo_width,	double,	Input,	1e-3,	,	
		s_lo_delay,	double,	Input,	1e-6,	,	
		s_lo_height,	double,	Input,	5,	,	
		s_lo_width,	double,	Input,	1e-3,	,	
		lo1_delay,	double,	Input,	1e-6,	,	
		lo1_height,	double,	Input,	5,	,	
		lo1_width,	double,	Input,	1e-3,	,	
		s_lo1_delay,	double,	Input,	1e-6,	,	
		s_lo1_height,	double,	Input,	5,	,	
		s_lo1_width,	double,	Input,	1e-3,	,	
		lo2_delay,	double,	Input,	1e-6,	,	
		lo2_height,	double,	Input,	5,	,	
		lo2_width,	double,	Input,	1e-3,	,	
		s_lo2_delay,	double,	Input,	1e-6,	,	
		s_lo2_height,	double,	Input,	5,	,	
		s_lo2_width,	double,	Input,	1e-3,	,	
		rise1,	double,	Input,	1e-6,	,	
		rise2,	double,	Input,	1e-6,	,	
		fall1,	double,	Input,	1e-6,	,	
		fall2,	double,	Input,	1e-6,	,	
		s1_v,	double,	Input,	5,	,	
		s1_limit,	double,	Input,	0.001,	,	
		s2_v,	double,	Input,	3,	,	
		s2_ilimit,	double,	Input,	0.001,	,	
		flag1,	char,	Input,	,	,	
		mode,	char,	Input,	,	,	
		debug,	int,	Input,	1,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
void  E4p_Sm2Gn3(int, double, int, int, int, int, int, int, int, int, int, int, int, int, double, double, double, double, double, double, double, double, double, double, double, double, double, double, double, double, double, double, double, double, double, double, double, double, double, double, double, double, double, double, double, double, char, char, int);

/* USRLIB MODULE INFORMATION

	MODULE NAME: E4p_spot
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 28
	ARGUMENTS:
		class,	char *,	Input,	"AA",	,	
		hi,	int,	Input,	-1,	,	
		lo,	int,	Input,	-1,	,	
		lo1,	int,	Input,	-1,	,	
		lo2,	int,	Input,	-1,	,	
		slo1,	int,	Input,	-1,	,	
		slo2,	int,	Input,	-1,	,	
		PS1,	int,	Input,	-1,	,	
		PS2,	int,	Input,	-1,	,	
		g1,	int,	Input,	-1,	,	
		g2,	int,	Input,	-1,	,	
		g3,	int,	Input,	-1,	,	
		hi_delay,	double,	Input,	0,	,	
		hi_height,	double,	Input,	1,	,	
		hi_width,	double,	Input,	1e-3,	,	
		lo_delay,	double,	Input,	0,	,	
		lo_height,	double,	Input,	2,	,	
		lo_width,	double,	Input,	1e-3,	,	
		lo1_delay,	double,	Input,	0,	,	
		lo1_height,	double,	Input,	3,	,	
		lo1_width,	double,	Input,	1e-3,	,	
		lo2_delay,	double,	Input,	0,	,	
		lo2_height,	double,	Input,	4,	,	
		lo2_width,	double,	Input,	1e-3,	,	
		rise,	double,	Input,	1e-8,	,	
		fall,	double,	Input,	1e-8,	,	
		PS1_V,	double,	Input,	0,	,	
		PS2_V,	double,	Input,	0,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
#define MAX(a,b)        ((a)>(b) ? (a) : (b))
	END USRLIB MODULE INFORMATION
*/
void  E4p_spot(char *, int, int, int, int, int, int, int, int, int, int, int, double, double, double, double, double, double, double, double, double, double, double, double, double, double, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: E4p_spot_aa
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 28
	ARGUMENTS:
		class,	char *,	Input,	"AA",	,	
		hi,	int,	Input,	-1,	,	
		lo,	int,	Input,	-1,	,	
		lo1,	int,	Input,	-1,	,	
		lo2,	int,	Input,	-1,	,	
		slo1,	int,	Input,	-1,	,	
		slo2,	int,	Input,	-1,	,	
		PS1,	int,	Input,	-1,	,	
		PS2,	int,	Input,	-1,	,	
		g1,	int,	Input,	-1,	,	
		g2,	int,	Input,	-1,	,	
		g3,	int,	Input,	-1,	,	
		hi_delay,	double,	Input,	1e-8,	,	
		hi_height,	double,	Input,	4,	,	
		hi_width,	double,	Input,	1e-3,	,	
		lo_delay,	double,	Input,	1e-8,	,	
		lo_height,	double,	Input,	5,	,	
		lo_width,	double,	Input,	1e-3,	,	
		lo1_delay,	double,	Input,	1e-8,	,	
		lo1_height,	double,	Input,	7,	,	
		lo1_width,	double,	Input,	1e-3,	,	
		lo2_delay,	double,	Input,	1e-8,	,	
		lo2_height,	double,	Input,	10,	,	
		lo2_width,	double,	Input,	1e-3,	,	
		rise,	double,	Input,	1e-8,	,	
		fall,	double,	Input,	1e-8,	,	
		PS1_V,	double,	Input,	0,	,	
		PS2_V,	double,	Input,	0,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
#define MAX(a,b)        ((a)>(b) ? (a) : (b))
	END USRLIB MODULE INFORMATION
*/
void  E4p_spot_aa(char *, int, int, int, int, int, int, int, int, int, int, int, double, double, double, double, double, double, double, double, double, double, double, double, double, double, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: E4pL_Sm2Gn3
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 21
	ARGUMENTS:
		cg,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		tg,	int,	Input,	,	,	
		c1_height,	double,	Input,	,	,	
		c1_width,	double,	Input,	,	,	
		c1_delay,	double,	Input,	,	,	
		c2_height,	double,	Input,	,	,	
		c2_width,	double,	Input,	,	,	
		c2_delay,	double,	Input,	,	,	
		c11_height,	double,	Input,	,	,	
		c11_width,	double,	Input,	,	,	
		c11_delay,	double,	Input,	,	,	
		c21_height,	double,	Input,	,	,	
		c21_width,	double,	Input,	,	,	
		c21_delay,	double,	Input,	,	,	
		cm_rise,	double,	Input,	,	,	
		cm_fall,	double,	Input,	,	,	
		loop,	int,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include <LBC5_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
#define CRTLIM 50.0e-3
#define MAX(a,b)   ((a)>(b) ? (a) : (b))

	END USRLIB MODULE INFORMATION
*/
void E4pL_Sm2Gn3(int cg, int drain, int source, int sub, int tg, double c1_height, double c1_width, double c1_delay, double c2_height, double c2_width, double c2_delay, double c11_height, double c11_width, double c11_delay, double c21_height, double c21_width, double c21_delay, double cm_rise, double cm_fall, int loop, double holdtime);


/* USRLIB MODULE INFORMATION

	MODULE NAME: E4pL_sync
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 48
	ARGUMENTS:
		n,	int,	Input,	1,	0,	30000
		period,	double,	Input,	30e-3,	,	
		hi,	int,	Input,	-1,	,	
		hi_sync,	int,	Input,	-1,	,	
		lo,	int,	Input,	-1,	,	
		lo_sync,	int,	Input,	-1,	,	
		lo1,	int,	Input,	-1,	,	
		lo1_sync,	int,	Input,	-1,	,	
		lo2,	int,	Input,	-1,	,	
		lo2_sync,	int,	Input,	-1,	,	
		Psmu1,	int,	Input,	-1,	,	
		Psmu2,	int,	Input,	-1,	,	
		gnd1,	int,	Input,	-1,	,	
		gnd2,	int,	Input,	-1,	,	
		hi_delay,	double,	Input,	1e-6,	,	
		hi_height,	double,	Input,	5,	-20,	+20
		hi_width,	double,	Input,	5e-3,	,	
		s_hi_delay,	double,	Input,	1e-6,	,	
		s_hi_height,	double,	Input,	-5,	-20,	+20
		s_hi_width,	double,	Input,	5e-3,	,	
		lo_delay,	double,	Input,	1e-6,	,	
		lo_height,	double,	Input,	5,	-20,	+20
		lo_width,	double,	Input,	5e-3,	,	
		s_lo_delay,	double,	Input,	1e-6,	,	
		s_lo_height,	double,	Input,	5,	-20,	+20
		s_lo_width,	double,	Input,	5e-3,	,	
		lo1_delay,	double,	Input,	1e-6,	,	
		lo1_height,	double,	Input,	5,	-20,	+20
		lo1_width,	double,	Input,	5e-3,	,	
		s_lo1_delay,	double,	Input,	1e-6,	,	
		s_lo1_height,	double,	Input,	5,	-20,	+20
		s_lo1_width,	double,	Input,	5e-3,	,	
		lo2_delay,	double,	Input,	1e-6,	,	
		lo2_height,	double,	Input,	5,	-20,	+20
		lo2_width,	double,	Input,	5e-3,	,	
		s_lo2_delay,	double,	Input,	1e-6,	,	
		s_lo2_height,	double,	Input,	5,	-20,	+20
		s_lo2_width,	double,	Input,	5e-3,	,	
		rise1,	double,	Input,	1e-6,	,	
		rise2,	double,	Input,	1e-6,	,	
		fall1,	double,	Input,	1e-6,	,	
		fall2,	double,	Input,	1e-6,	,	
		s1_v,	double,	Input,	5,	,	
		s2_v,	double,	Input,	5,	,	
		flag1,	char,	Input,	's',	,	
		mode,	char,	Input,	'd',	,	
		debug,	int,	Input,	1,	,	
		n_delay,	double,	Input,	0.1,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
void E4pL_sync(int n, double period, int hi, int hi_sync, int lo, int lo_sync, int lo1, int lo1_sync, int lo2, int lo2_sync, int Psmu1, int Psmu2, int gnd1, int gnd2, double hi_delay, double hi_height, double hi_width, double s_hi_delay, double s_hi_height, double s_hi_width, double lo_delay, double lo_height, double lo_width, double s_lo_delay, double s_lo_height, double s_lo_width, double lo1_delay, double lo1_height, double lo1_width, double s_lo1_delay, double s_lo1_height, double s_lo1_width, double lo2_delay, double lo2_height, double lo2_width, double s_lo2_delay, double s_lo2_height, double s_lo2_width, double rise1, double rise2, double fall1, double fall2, double s1_v, double s2_v, char flag1, char mode, int debug, double n_delay);


/* USRLIB MODULE INFORMATION

	MODULE NAME: E4pL_sync_back
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 49
	ARGUMENTS:
		n,	int,	Input,	,	,	
		period,	double,	Input,	,	,	
		hi,	int,	Input,	,	,	
		hi_sync,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		lo_sync,	int,	Input,	,	,	
		lo1,	int,	Input,	,	,	
		lo1_sync,	int,	Input,	,	,	
		lo2,	int,	Input,	,	,	
		lo2_sync,	int,	Input,	,	,	
		Psmu1,	int,	Input,	,	,	
		Psmu2,	int,	Input,	,	,	
		gnd1,	int,	Input,	,	,	
		gnd2,	int,	Input,	,	,	
		hi_delay,	double,	Input,	,	,	
		hi_height,	double,	Input,	,	,	
		hi_width,	double,	Input,	,	,	
		s_hi_delay,	double,	Input,	,	,	
		s_hi_height,	double,	Input,	,	,	
		s_hi_width,	double,	Input,	,	,	
		lo_delay,	double,	Input,	,	,	
		lo_height,	double,	Input,	,	,	
		lo_width,	double,	Input,	,	,	
		s_lo_delay,	double,	Input,	,	,	
		s_lo_height,	double,	Input,	,	,	
		s_lo_width,	double,	Input,	,	,	
		lo1_delay,	double,	Input,	,	,	
		lo1_height,	double,	Input,	,	,	
		lo1_width,	double,	Input,	,	,	
		s_lo1_delay,	double,	Input,	,	,	
		s_lo1_height,	double,	Input,	,	,	
		s_lo1_width,	double,	Input,	,	,	
		lo2_delay,	double,	Input,	,	,	
		lo2_height,	double,	Input,	,	,	
		lo2_width,	double,	Input,	,	,	
		s_lo2_delay,	double,	Input,	,	,	
		s_lo2_height,	double,	Input,	,	,	
		s_lo2_width,	double,	Input,	,	,	
		rise1,	double,	Input,	,	,	
		rise2,	double,	Input,	,	,	
		fall1,	double,	Input,	,	,	
		fall2,	double,	Input,	,	,	
		s1_v,	double,	Input,	,	,	
		s1_limit,	double,	Input,	,	,	
		s2_v,	double,	Input,	,	,	
		s2_ilimit,	double,	Input,	,	,	
		flag1,	char,	Input,	,	,	
		mode,	char,	Input,	,	,	
		debug,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
void E4pL_sync_back(int n, double period, int hi, int hi_sync, int lo, int lo_sync, int lo1, int lo1_sync, int lo2, int lo2_sync, int Psmu1, int Psmu2, int gnd1, int gnd2, double hi_delay, double hi_height, double hi_width, double s_hi_delay, double s_hi_height, double s_hi_width, double lo_delay, double lo_height, double lo_width, double s_lo_delay, double s_lo_height, double s_lo_width, double lo1_delay, double lo1_height, double lo1_width, double s_lo1_delay, double s_lo1_height, double s_lo1_width, double lo2_delay, double lo2_height, double lo2_width, double s_lo2_delay, double s_lo2_height, double s_lo2_width, double rise1, double rise2, double fall1, double fall2, double s1_v, double s1_limit, double s2_v, double s2_ilimit, char flag1, char mode, int debug);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Er_Pg_Wakeup
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 28
	ARGUMENTS:
		cgate,	int,	Input,	,	,	
		sgate,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		tr,	int,	Input,	,	,	
		Er_vsgate,	double,	Input,	,	,	
		Er_vdrain,	double,	Input,	,	,	
		Er_vwell,	double,	Input,	,	,	
		Er_cg_delay,	double,	Input,	,	,	
		Er_cg_high,	double,	Input,	,	,	
		Er_cg_width,	double,	Input,	,	,	
		Er_cg_rise,	double,	Input,	,	,	
		Er_cg_fall,	double,	Input,	,	,	
		Er_holdtime,	double,	Input,	,	,	
		Er_tr_high,	double,	Input,	,	,	
		Er_tr_width,	double,	Input,	,	,	
		pg_vsgate,	double,	Input,	,	,	
		pg_vdrain,	double,	Input,	,	,	
		pg_vwell,	double,	Input,	,	,	
		pg_cg_high,	double,	Input,	,	,	
		pg_cg_width,	double,	Input,	,	,	
		pg_cg_rise,	double,	Input,	,	,	
		pg_cg_fall,	double,	Input,	,	,	
		pg_holdtime,	double,	Input,	,	,	
		pg_tr_high,	double,	Input,	,	,	
		pg_tr_width,	double,	Input,	,	,	
		pg_tr_delay,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
#define CRTLIM 50.0e-3

	END USRLIB MODULE INFORMATION
*/
void Er_Pg_Wakeup(int cgate, int sgate, int drain, int source, int well, int tr, double Er_vsgate, double Er_vdrain, double Er_vwell, double Er_cg_delay, double Er_cg_high, double Er_cg_width, double Er_cg_rise, double Er_cg_fall, double Er_holdtime, double Er_tr_high, double Er_tr_width, double pg_vsgate, double pg_vdrain, double pg_vwell, double pg_cg_high, double pg_cg_width, double pg_cg_rise, double pg_cg_fall, double pg_holdtime, double pg_tr_high, double pg_tr_width, double pg_tr_delay);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Er_Pg_Wakeup_199cy
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 28
	ARGUMENTS:
		cgate,	int,	Input,	,	,	
		sgate,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		tr,	int,	Input,	,	,	
		Er_vsgate,	double,	Input,	,	,	
		Er_vdrain,	double,	Input,	,	,	
		Er_vwell,	double,	Input,	,	,	
		Er_cg_delay,	double,	Input,	,	,	
		Er_cg_high,	double,	Input,	,	,	
		Er_cg_width,	double,	Input,	,	,	
		Er_cg_rise,	double,	Input,	,	,	
		Er_cg_fall,	double,	Input,	,	,	
		Er_holdtime,	double,	Input,	,	,	
		Er_tr_high,	double,	Input,	,	,	
		Er_tr_width,	double,	Input,	,	,	
		pg_vsgate,	double,	Input,	,	,	
		pg_vdrain,	double,	Input,	,	,	
		pg_vwell,	double,	Input,	,	,	
		pg_cg_high,	double,	Input,	,	,	
		pg_cg_width,	double,	Input,	,	,	
		pg_cg_rise,	double,	Input,	,	,	
		pg_cg_fall,	double,	Input,	,	,	
		pg_holdtime,	double,	Input,	,	,	
		pg_tr_high,	double,	Input,	,	,	
		pg_tr_width,	double,	Input,	,	,	
		pg_tr_delay,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
#define CRTLIM 50.0e-3

	END USRLIB MODULE INFORMATION
*/
void Er_Pg_Wakeup_199cy(int cgate, int sgate, int drain, int source, int well, int tr, double Er_vsgate, double Er_vdrain, double Er_vwell, double Er_cg_delay, double Er_cg_high, double Er_cg_width, double Er_cg_rise, double Er_cg_fall, double Er_holdtime, double Er_tr_high, double Er_tr_width, double pg_vsgate, double pg_vdrain, double pg_vwell, double pg_cg_high, double pg_cg_width, double pg_cg_rise, double pg_cg_fall, double pg_holdtime, double pg_tr_high, double pg_tr_width, double pg_tr_delay);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Er_Pg_Wakeup_19cy
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 28
	ARGUMENTS:
		cgate,	int,	Input,	,	,	
		sgate,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		tr,	int,	Input,	,	,	
		Er_vsgate,	double,	Input,	,	,	
		Er_vdrain,	double,	Input,	,	,	
		Er_vwell,	double,	Input,	,	,	
		Er_cg_delay,	double,	Input,	,	,	
		Er_cg_high,	double,	Input,	,	,	
		Er_cg_width,	double,	Input,	,	,	
		Er_cg_rise,	double,	Input,	,	,	
		Er_cg_fall,	double,	Input,	,	,	
		Er_holdtime,	double,	Input,	,	,	
		Er_tr_high,	double,	Input,	,	,	
		Er_tr_width,	double,	Input,	,	,	
		pg_vsgate,	double,	Input,	,	,	
		pg_vdrain,	double,	Input,	,	,	
		pg_vwell,	double,	Input,	,	,	
		pg_cg_high,	double,	Input,	,	,	
		pg_cg_width,	double,	Input,	,	,	
		pg_cg_rise,	double,	Input,	,	,	
		pg_cg_fall,	double,	Input,	,	,	
		pg_holdtime,	double,	Input,	,	,	
		pg_tr_high,	double,	Input,	,	,	
		pg_tr_width,	double,	Input,	,	,	
		pg_tr_delay,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
#define CRTLIM 50.0e-3

	END USRLIB MODULE INFORMATION
*/
void Er_Pg_Wakeup_19cy(int cgate, int sgate, int drain, int source, int well, int tr, double Er_vsgate, double Er_vdrain, double Er_vwell, double Er_cg_delay, double Er_cg_high, double Er_cg_width, double Er_cg_rise, double Er_cg_fall, double Er_holdtime, double Er_tr_high, double Er_tr_width, double pg_vsgate, double pg_vdrain, double pg_vwell, double pg_cg_high, double pg_cg_width, double pg_cg_rise, double pg_cg_fall, double pg_holdtime, double pg_tr_high, double pg_tr_width, double pg_tr_delay);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Er_Pg_Wakeup_299cy
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 28
	ARGUMENTS:
		cgate,	int,	Input,	,	,	
		sgate,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		tr,	int,	Input,	,	,	
		Er_vsgate,	double,	Input,	,	,	
		Er_vdrain,	double,	Input,	,	,	
		Er_vwell,	double,	Input,	,	,	
		Er_cg_delay,	double,	Input,	,	,	
		Er_cg_high,	double,	Input,	,	,	
		Er_cg_width,	double,	Input,	,	,	
		Er_cg_rise,	double,	Input,	,	,	
		Er_cg_fall,	double,	Input,	,	,	
		Er_holdtime,	double,	Input,	,	,	
		Er_tr_high,	double,	Input,	,	,	
		Er_tr_width,	double,	Input,	,	,	
		pg_vsgate,	double,	Input,	,	,	
		pg_vdrain,	double,	Input,	,	,	
		pg_vwell,	double,	Input,	,	,	
		pg_cg_high,	double,	Input,	,	,	
		pg_cg_width,	double,	Input,	,	,	
		pg_cg_rise,	double,	Input,	,	,	
		pg_cg_fall,	double,	Input,	,	,	
		pg_holdtime,	double,	Input,	,	,	
		pg_tr_high,	double,	Input,	,	,	
		pg_tr_width,	double,	Input,	,	,	
		pg_tr_delay,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
#define CRTLIM 50.0e-3

	END USRLIB MODULE INFORMATION
*/
void Er_Pg_Wakeup_299cy(int cgate, int sgate, int drain, int source, int well, int tr, double Er_vsgate, double Er_vdrain, double Er_vwell, double Er_cg_delay, double Er_cg_high, double Er_cg_width, double Er_cg_rise, double Er_cg_fall, double Er_holdtime, double Er_tr_high, double Er_tr_width, double pg_vsgate, double pg_vdrain, double pg_vwell, double pg_cg_high, double pg_cg_width, double pg_cg_rise, double pg_cg_fall, double pg_holdtime, double pg_tr_high, double pg_tr_width, double pg_tr_delay);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Er_Pg_Wakeup_29cy
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 28
	ARGUMENTS:
		cgate,	int,	Input,	,	,	
		sgate,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		tr,	int,	Input,	,	,	
		Er_vsgate,	double,	Input,	,	,	
		Er_vdrain,	double,	Input,	,	,	
		Er_vwell,	double,	Input,	,	,	
		Er_cg_delay,	double,	Input,	,	,	
		Er_cg_high,	double,	Input,	,	,	
		Er_cg_width,	double,	Input,	,	,	
		Er_cg_rise,	double,	Input,	,	,	
		Er_cg_fall,	double,	Input,	,	,	
		Er_holdtime,	double,	Input,	,	,	
		Er_tr_high,	double,	Input,	,	,	
		Er_tr_width,	double,	Input,	,	,	
		pg_vsgate,	double,	Input,	,	,	
		pg_vdrain,	double,	Input,	,	,	
		pg_vwell,	double,	Input,	,	,	
		pg_cg_high,	double,	Input,	,	,	
		pg_cg_width,	double,	Input,	,	,	
		pg_cg_rise,	double,	Input,	,	,	
		pg_cg_fall,	double,	Input,	,	,	
		pg_holdtime,	double,	Input,	,	,	
		pg_tr_high,	double,	Input,	,	,	
		pg_tr_width,	double,	Input,	,	,	
		pg_tr_delay,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
#define CRTLIM 50.0e-3

	END USRLIB MODULE INFORMATION
*/
void Er_Pg_Wakeup_29cy(int cgate, int sgate, int drain, int source, int well, int tr, double Er_vsgate, double Er_vdrain, double Er_vwell, double Er_cg_delay, double Er_cg_high, double Er_cg_width, double Er_cg_rise, double Er_cg_fall, double Er_holdtime, double Er_tr_high, double Er_tr_width, double pg_vsgate, double pg_vdrain, double pg_vwell, double pg_cg_high, double pg_cg_width, double pg_cg_rise, double pg_cg_fall, double pg_holdtime, double pg_tr_high, double pg_tr_width, double pg_tr_delay);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Er_Pg_Wakeup_39cy
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 28
	ARGUMENTS:
		cgate,	int,	Input,	,	,	
		sgate,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		tr,	int,	Input,	,	,	
		Er_vsgate,	double,	Input,	,	,	
		Er_vdrain,	double,	Input,	,	,	
		Er_vwell,	double,	Input,	,	,	
		Er_cg_delay,	double,	Input,	,	,	
		Er_cg_high,	double,	Input,	,	,	
		Er_cg_width,	double,	Input,	,	,	
		Er_cg_rise,	double,	Input,	,	,	
		Er_cg_fall,	double,	Input,	,	,	
		Er_holdtime,	double,	Input,	,	,	
		Er_tr_high,	double,	Input,	,	,	
		Er_tr_width,	double,	Input,	,	,	
		pg_vsgate,	double,	Input,	,	,	
		pg_vdrain,	double,	Input,	,	,	
		pg_vwell,	double,	Input,	,	,	
		pg_cg_high,	double,	Input,	,	,	
		pg_cg_width,	double,	Input,	,	,	
		pg_cg_rise,	double,	Input,	,	,	
		pg_cg_fall,	double,	Input,	,	,	
		pg_holdtime,	double,	Input,	,	,	
		pg_tr_high,	double,	Input,	,	,	
		pg_tr_width,	double,	Input,	,	,	
		pg_tr_delay,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
#define CRTLIM 50.0e-3

	END USRLIB MODULE INFORMATION
*/
void Er_Pg_Wakeup_39cy(int cgate, int sgate, int drain, int source, int well, int tr, double Er_vsgate, double Er_vdrain, double Er_vwell, double Er_cg_delay, double Er_cg_high, double Er_cg_width, double Er_cg_rise, double Er_cg_fall, double Er_holdtime, double Er_tr_high, double Er_tr_width, double pg_vsgate, double pg_vdrain, double pg_vwell, double pg_cg_high, double pg_cg_width, double pg_cg_rise, double pg_cg_fall, double pg_holdtime, double pg_tr_high, double pg_tr_width, double pg_tr_delay);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Er_Pg_Wakeup_49cy
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 28
	ARGUMENTS:
		cgate,	int,	Input,	,	,	
		sgate,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		tr,	int,	Input,	,	,	
		Er_vsgate,	double,	Input,	,	,	
		Er_vdrain,	double,	Input,	,	,	
		Er_vwell,	double,	Input,	,	,	
		Er_cg_delay,	double,	Input,	,	,	
		Er_cg_high,	double,	Input,	,	,	
		Er_cg_width,	double,	Input,	,	,	
		Er_cg_rise,	double,	Input,	,	,	
		Er_cg_fall,	double,	Input,	,	,	
		Er_holdtime,	double,	Input,	,	,	
		Er_tr_high,	double,	Input,	,	,	
		Er_tr_width,	double,	Input,	,	,	
		pg_vsgate,	double,	Input,	,	,	
		pg_vdrain,	double,	Input,	,	,	
		pg_vwell,	double,	Input,	,	,	
		pg_cg_high,	double,	Input,	,	,	
		pg_cg_width,	double,	Input,	,	,	
		pg_cg_rise,	double,	Input,	,	,	
		pg_cg_fall,	double,	Input,	,	,	
		pg_holdtime,	double,	Input,	,	,	
		pg_tr_high,	double,	Input,	,	,	
		pg_tr_width,	double,	Input,	,	,	
		pg_tr_delay,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
#define CRTLIM 50.0e-3

	END USRLIB MODULE INFORMATION
*/
void Er_Pg_Wakeup_49cy(int cgate, int sgate, int drain, int source, int well, int tr, double Er_vsgate, double Er_vdrain, double Er_vwell, double Er_cg_delay, double Er_cg_high, double Er_cg_width, double Er_cg_rise, double Er_cg_fall, double Er_holdtime, double Er_tr_high, double Er_tr_width, double pg_vsgate, double pg_vdrain, double pg_vwell, double pg_cg_high, double pg_cg_width, double pg_cg_rise, double pg_cg_fall, double pg_holdtime, double pg_tr_high, double pg_tr_width, double pg_tr_delay);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Er_Pg_Wakeup_99cy
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 28
	ARGUMENTS:
		cgate,	int,	Input,	,	,	
		sgate,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		tr,	int,	Input,	,	,	
		Er_vsgate,	double,	Input,	,	,	
		Er_vdrain,	double,	Input,	,	,	
		Er_vwell,	double,	Input,	,	,	
		Er_cg_delay,	double,	Input,	,	,	
		Er_cg_high,	double,	Input,	,	,	
		Er_cg_width,	double,	Input,	,	,	
		Er_cg_rise,	double,	Input,	,	,	
		Er_cg_fall,	double,	Input,	,	,	
		Er_holdtime,	double,	Input,	,	,	
		Er_tr_high,	double,	Input,	,	,	
		Er_tr_width,	double,	Input,	,	,	
		pg_vsgate,	double,	Input,	,	,	
		pg_vdrain,	double,	Input,	,	,	
		pg_vwell,	double,	Input,	,	,	
		pg_cg_high,	double,	Input,	,	,	
		pg_cg_width,	double,	Input,	,	,	
		pg_cg_rise,	double,	Input,	,	,	
		pg_cg_fall,	double,	Input,	,	,	
		pg_holdtime,	double,	Input,	,	,	
		pg_tr_high,	double,	Input,	,	,	
		pg_tr_width,	double,	Input,	,	,	
		pg_tr_delay,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
#define CRTLIM 50.0e-3

	END USRLIB MODULE INFORMATION
*/
void Er_Pg_Wakeup_99cy(int cgate, int sgate, int drain, int source, int well, int tr, double Er_vsgate, double Er_vdrain, double Er_vwell, double Er_cg_delay, double Er_cg_high, double Er_cg_width, double Er_cg_rise, double Er_cg_fall, double Er_holdtime, double Er_tr_high, double Er_tr_width, double pg_vsgate, double pg_vdrain, double pg_vwell, double pg_cg_high, double pg_cg_width, double pg_cg_rise, double pg_cg_fall, double pg_holdtime, double pg_tr_high, double pg_tr_width, double pg_tr_delay);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Er_Pg_Wakeup_9cy
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 28
	ARGUMENTS:
		cgate,	int,	Input,	,	,	
		sgate,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		tr,	int,	Input,	,	,	
		Er_vsgate,	double,	Input,	,	,	
		Er_vdrain,	double,	Input,	,	,	
		Er_vwell,	double,	Input,	,	,	
		Er_cg_delay,	double,	Input,	,	,	
		Er_cg_high,	double,	Input,	,	,	
		Er_cg_width,	double,	Input,	,	,	
		Er_cg_rise,	double,	Input,	,	,	
		Er_cg_fall,	double,	Input,	,	,	
		Er_holdtime,	double,	Input,	,	,	
		Er_tr_high,	double,	Input,	,	,	
		Er_tr_width,	double,	Input,	,	,	
		pg_vsgate,	double,	Input,	,	,	
		pg_vdrain,	double,	Input,	,	,	
		pg_vwell,	double,	Input,	,	,	
		pg_cg_high,	double,	Input,	,	,	
		pg_cg_width,	double,	Input,	,	,	
		pg_cg_rise,	double,	Input,	,	,	
		pg_cg_fall,	double,	Input,	,	,	
		pg_holdtime,	double,	Input,	,	,	
		pg_tr_high,	double,	Input,	,	,	
		pg_tr_width,	double,	Input,	,	,	
		pg_tr_delay,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
#define CRTLIM 50.0e-3

	END USRLIB MODULE INFORMATION
*/
void Er_Pg_Wakeup_9cy(int cgate, int sgate, int drain, int source, int well, int tr, double Er_vsgate, double Er_vdrain, double Er_vwell, double Er_cg_delay, double Er_cg_high, double Er_cg_width, double Er_cg_rise, double Er_cg_fall, double Er_holdtime, double Er_tr_high, double Er_tr_width, double pg_vsgate, double pg_vdrain, double pg_vwell, double pg_cg_high, double pg_cg_width, double pg_cg_rise, double pg_cg_fall, double pg_holdtime, double pg_tr_high, double pg_tr_width, double pg_tr_delay);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Erase_1O2PS2_K
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 17
	ARGUMENTS:
		cgate,	int,	Input,	,	,	
		sgate,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		tr,	int,	Input,	,	,	
		vsgate,	double,	Input,	,	,	
		vdrain,	double,	Input,	,	,	
		vwell,	double,	Input,	,	,	
		cg_delay,	double,	Input,	,	,	
		cg_high,	double,	Input,	,	,	
		cg_width,	double,	Input,	,	,	
		cg_rise,	double,	Input,	,	,	
		cg_fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
		tr_high,	double,	Input,	,	,	
		tr_width,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
void Erase_1O2PS2_K(int cgate, int sgate, int drain, int source, int well, int tr, double vsgate, double vdrain, double vwell, double cg_delay, double cg_high, double cg_width, double cg_rise, double cg_fall, double holdtime, double tr_high, double tr_width);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Erase_DP
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 13
	ARGUMENTS:
		cgate,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		tr,	int,	Input,	,	,	
		cg_delay,	double,	Input,	,	,	
		cg_high,	double,	Input,	,	,	
		cg_width,	double,	Input,	,	,	
		cg_rise,	double,	Input,	,	,	
		cg_fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
		tr_high,	double,	Input,	,	,	
		tr_width,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
void Erase_DP(int cgate, int drain, int source, int well, int tr, double cg_delay, double cg_high, double cg_width, double cg_rise, double cg_fall, double holdtime, double tr_high, double tr_width);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Erase_E1C5
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 12
	ARGUMENTS:
		cgate,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		cg_delay,	double,	Input,	,	,	
		cg_high,	double,	Input,	,	,	
		cg_width,	double,	Input,	,	,	
		cg_rise,	double,	Input,	,	,	
		cg_fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
		oth_high,	double,	Input,	,	,	
		oth_width,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
#define MAX(a,b)   ((a)>(b) ? (a) : (b))
	END USRLIB MODULE INFORMATION
*/
void  Erase_E1C5(int, int, int, int, double, double, double, double, double, double, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: Erase_E1C5_type1
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 10
	ARGUMENTS:
		cgate,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		tr,	int,	Input,	,	,	
		tr_delay,	double,	Input,	,	,	
		tr_high,	double,	Input,	,	,	
		tr_width,	double,	Input,	,	,	
		tr_rise,	double,	Input,	,	,	
		tr_fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
void Erase_E1C5_type1(int cgate, int drain, int source, int tr, double tr_delay, double tr_high, double tr_width, double tr_rise, double tr_fall, double holdtime);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Erase_E1C5_type2
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 12
	ARGUMENTS:
		cgate,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		cg_delay,	double,	Input,	,	,	
		cg_high,	double,	Input,	,	,	
		cg_width,	double,	Input,	,	,	
		cg_rise,	double,	Input,	,	,	
		cg_fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
		oth_high,	double,	Input,	,	,	
		oth_width,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
void Erase_E1C5_type2(int cgate, int drain, int source, int well, double cg_delay, double cg_high, double cg_width, double cg_rise, double cg_fall, double holdtime, double oth_high, double oth_width);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Erase_E1C5_type3
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 18
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		lo1,	int,	Input,	,	,	
		lo2,	int,	Input,	,	,	
		hi_delay,	double,	Input,	,	,	
		hi_high,	double,	Input,	,	,	
		hi_width,	double,	Input,	,	,	
		hi_rise,	double,	Input,	,	,	
		hi_fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
		lo_delay,	double,	Input,	,	,	
		lo_high,	double,	Input,	,	,	
		lo_width,	double,	Input,	,	,	
		lo_rise,	double,	Input,	,	,	
		lo_fall,	double,	Input,	,	,	
		dual,	int,	Input,	,	,	
		smu1,	int,	Input,	,	,	
		smu1_v,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
void Erase_E1C5_type3(int hi, int lo, int lo1, int lo2, double hi_delay, double hi_high, double hi_width, double hi_rise, double hi_fall, double holdtime, double lo_delay, double lo_high, double lo_width, double lo_rise, double lo_fall, int dual, int smu1, double smu1_v);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Erase_Eon
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 12
	ARGUMENTS:
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		ga_delay,	double,	Input,	,	,	
		ga_high,	double,	Input,	,	,	
		ga_width,	double,	Input,	,	,	
		ga_rise,	double,	Input,	,	,	
		ga_fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
		vb_high,	double,	Input,	,	,	
		vb_width,	double,	Input,	,	,	
		TRGholdtime,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
void Erase_Eon(int gate, int source, int well, double ga_delay, double ga_high, double ga_width, double ga_rise, double ga_fall, double holdtime, double vb_high, double vb_width, double TRGholdtime);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Erase_Eon_k
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 11
	ARGUMENTS:
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		ga_delay,	double,	Input,	,	,	
		ga_high,	double,	Input,	,	,	
		ga_width,	double,	Input,	,	,	
		ga_rise,	double,	Input,	,	,	
		ga_fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
		vb_high,	double,	Input,	,	,	
		vb_width,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
void Erase_Eon_k(int gate, int source, int well, double ga_delay, double ga_high, double ga_width, double ga_rise, double ga_fall, double holdtime, double vb_high, double vb_width);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Erase_init
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 18
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		cgate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		tr,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		cg_high_pg,	double,	Input,	,	,	
		cg_width_pg,	double,	Input,	,	,	
		cg_high,	double,	Input,	,	,	
		cg_width,	double,	Input,	,	,	
		cg_rise,	double,	Input,	,	,	
		cg_fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
		tr_high_pg,	double,	Input,	,	,	
		tr_width_pg,	double,	Input,	,	,	
		tr_high,	double,	Input,	,	,	
		tr_width,	double,	Input,	,	,	
		tr_delay,	double,	Input,	,	,	
		cg_delay,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
#define CRTLIM 50.0e-3

	END USRLIB MODULE INFORMATION
*/
void Erase_init(int drain, int cgate, int source, int tr, int well, double cg_high_pg, double cg_width_pg, double cg_high, double cg_width, double cg_rise, double cg_fall, double holdtime, double tr_high_pg, double tr_width_pg, double tr_high, double tr_width, double tr_delay, double cg_delay);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Erase_init_DP
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 18
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		cgate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		tr,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		cg_high_pg,	double,	Input,	,	,	
		cg_width_pg,	double,	Input,	,	,	
		cg_high,	double,	Input,	,	,	
		cg_width,	double,	Input,	,	,	
		cg_rise,	double,	Input,	,	,	
		cg_fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
		tr_high_pg,	double,	Input,	,	,	
		tr_width_pg,	double,	Input,	,	,	
		tr_high,	double,	Input,	,	,	
		tr_width,	double,	Input,	,	,	
		tr_delay,	double,	Input,	,	,	
		cg_delay,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
#define CRTLIM 50.0e-3

	END USRLIB MODULE INFORMATION
*/
void Erase_init_DP(int drain, int cgate, int source, int tr, int well, double cg_high_pg, double cg_width_pg, double cg_high, double cg_width, double cg_rise, double cg_fall, double holdtime, double tr_high_pg, double tr_width_pg, double tr_high, double tr_width, double tr_delay, double cg_delay);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Erase_ks
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 17
	ARGUMENTS:
		cgate,	int,	Input,	,	,	
		sgate,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		tr,	int,	Input,	,	,	
		vsgate,	double,	Input,	,	,	
		vdrain,	double,	Input,	,	,	
		vwell,	double,	Input,	,	,	
		cg_delay,	double,	Input,	,	,	
		cg_high,	double,	Input,	,	,	
		cg_width,	double,	Input,	,	,	
		cg_rise,	double,	Input,	,	,	
		cg_fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
		tr_high,	double,	Input,	,	,	
		tr_width,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
void Erase_ks(int cgate, int sgate, int drain, int source, int well, int tr, double vsgate, double vdrain, double vwell, double cg_delay, double cg_high, double cg_width, double cg_rise, double cg_fall, double holdtime, double tr_high, double tr_width);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Erase_ks_0627
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 17
	ARGUMENTS:
		cgate,	int,	Input,	,	,	
		sgate,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		tr,	int,	Input,	,	,	
		vsgate,	double,	Input,	,	,	
		vdrain,	double,	Input,	,	,	
		vwell,	double,	Input,	,	,	
		cg_delay,	double,	Input,	,	,	
		cg_high,	double,	Input,	,	,	
		cg_width,	double,	Input,	,	,	
		cg_rise,	double,	Input,	,	,	
		cg_fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
		tr_high,	double,	Input,	,	,	
		tr_width,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
void Erase_ks_0627(int cgate, int sgate, int drain, int source, int well, int tr, double vsgate, double vdrain, double vwell, double cg_delay, double cg_high, double cg_width, double cg_rise, double cg_fall, double holdtime, double tr_high, double tr_width);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Erase_ks_1O2P
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 17
	ARGUMENTS:
		cgate,	int,	Input,	,	,	
		sgate,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		tr,	int,	Input,	,	,	
		vsgate,	double,	Input,	,	,	
		vdrain,	double,	Input,	,	,	
		vwell,	double,	Input,	,	,	
		cg_delay,	double,	Input,	,	,	
		cg_high,	double,	Input,	,	,	
		cg_width,	double,	Input,	,	,	
		cg_rise,	double,	Input,	,	,	
		cg_fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
		tr_high,	double,	Input,	,	,	
		tr_width,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
void Erase_ks_1O2P(int cgate, int sgate, int drain, int source, int well, int tr, double vsgate, double vdrain, double vwell, double cg_delay, double cg_high, double cg_width, double cg_rise, double cg_fall, double holdtime, double tr_high, double tr_width);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Erase_ks_1O2P_2
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 17
	ARGUMENTS:
		cgate,	int,	Input,	,	,	
		sgate,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		tr,	int,	Input,	,	,	
		vsgate,	double,	Input,	,	,	
		vdrain,	double,	Input,	,	,	
		vwell,	double,	Input,	,	,	
		cg_delay,	double,	Input,	,	,	
		cg_high,	double,	Input,	,	,	
		cg_width,	double,	Input,	,	,	
		cg_rise,	double,	Input,	,	,	
		cg_fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
		tr_high,	double,	Input,	,	,	
		tr_width,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
void Erase_ks_1O2P_2(int cgate, int sgate, int drain, int source, int well, int tr, double vsgate, double vdrain, double vwell, double cg_delay, double cg_high, double cg_width, double cg_rise, double cg_fall, double holdtime, double tr_high, double tr_width);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Erase_ks_1O2P_confirm
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 17
	ARGUMENTS:
		cgate,	int,	Input,	,	,	
		sgate,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		tr,	int,	Input,	,	,	
		vsgate,	double,	Input,	,	,	
		vdrain,	double,	Input,	,	,	
		vwell,	double,	Input,	,	,	
		cg_delay,	double,	Input,	,	,	
		cg_high,	double,	Input,	,	,	
		cg_width,	double,	Input,	,	,	
		cg_rise,	double,	Input,	,	,	
		cg_fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
		tr_high,	double,	Input,	,	,	
		tr_width,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
void Erase_ks_1O2P_confirm(int cgate, int sgate, int drain, int source, int well, int tr, double vsgate, double vdrain, double vwell, double cg_delay, double cg_high, double cg_width, double cg_rise, double cg_fall, double holdtime, double tr_high, double tr_width);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Erase_ks_1O2PS
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 17
	ARGUMENTS:
		cgate,	int,	Input,	,	,	
		sgate,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		tr,	int,	Input,	,	,	
		vsgate,	double,	Input,	,	,	
		vdrain,	double,	Input,	,	,	
		vwell,	double,	Input,	,	,	
		cg_delay,	double,	Input,	,	,	
		cg_high,	double,	Input,	,	,	
		cg_width,	double,	Input,	,	,	
		cg_rise,	double,	Input,	,	,	
		cg_fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
		tr_high,	double,	Input,	,	,	
		tr_width,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
void Erase_ks_1O2PS(int cgate, int sgate, int drain, int source, int well, int tr, double vsgate, double vdrain, double vwell, double cg_delay, double cg_high, double cg_width, double cg_rise, double cg_fall, double holdtime, double tr_high, double tr_width);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Erase_ks_2O3P
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 17
	ARGUMENTS:
		cgate,	int,	Input,	,	,	
		sgate,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		tr,	int,	Input,	,	,	
		vsgate,	double,	Input,	,	,	
		vdrain,	double,	Input,	,	,	
		vwell,	double,	Input,	,	,	
		cg_delay,	double,	Input,	,	,	
		cg_high,	double,	Input,	,	,	
		cg_width,	double,	Input,	,	,	
		cg_rise,	double,	Input,	,	,	
		cg_fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
		tr_high,	double,	Input,	,	,	
		tr_width,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
void Erase_ks_2O3P(int cgate, int sgate, int drain, int source, int well, int tr, double vsgate, double vdrain, double vwell, double cg_delay, double cg_high, double cg_width, double cg_rise, double cg_fall, double holdtime, double tr_high, double tr_width);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Erase_ks_imsi
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 13
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		cgate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		tr,	int,	Input,	,	,	
		vs,	double,	Input,	,	,	
		cg_high,	double,	Input,	,	,	
		cg_width,	double,	Input,	,	,	
		cg_rise,	double,	Input,	,	,	
		cg_fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
		tr_high,	double,	Input,	,	,	
		tr_width,	double,	Input,	,	,	
		tr_delay,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
void Erase_ks_imsi(int drain, int cgate, int source, int tr, double vs, double cg_high, double cg_width, double cg_rise, double cg_fall, double holdtime, double tr_high, double tr_width, double tr_delay);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Erase_TM
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 13
	ARGUMENTS:
		cgate,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		tr,	int,	Input,	,	,	
		cg_delay,	double,	Input,	,	,	
		cg_high,	double,	Input,	,	,	
		cg_width,	double,	Input,	,	,	
		cg_rise,	double,	Input,	,	,	
		cg_fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
		tr_high,	double,	Input,	,	,	
		tr_width,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
void Erase_TM(int cgate, int drain, int source, int well, int tr, double cg_delay, double cg_high, double cg_width, double cg_rise, double cg_fall, double holdtime, double tr_high, double tr_width);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Erase_TM_ks
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 17
	ARGUMENTS:
		cgate,	int,	Input,	,	,	
		sgate,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		tr,	int,	Input,	,	,	
		vsgate,	double,	Input,	,	,	
		vdrain,	double,	Input,	,	,	
		vwell,	double,	Input,	,	,	
		cg_delay,	double,	Input,	,	,	
		cg_high,	double,	Input,	,	,	
		cg_width,	double,	Input,	,	,	
		cg_rise,	double,	Input,	,	,	
		cg_fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
		tr_high,	double,	Input,	,	,	
		tr_width,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
void Erase_TM_ks(int cgate, int sgate, int drain, int source, int well, int tr, double vsgate, double vdrain, double vwell, double cg_delay, double cg_high, double cg_width, double cg_rise, double cg_fall, double holdtime, double tr_high, double tr_width);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Erase_TM_SO
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 13
	ARGUMENTS:
		cgate2,	int,	Input,	,	,	
		drain2,	int,	Input,	,	,	
		source2,	int,	Input,	,	,	
		well2,	int,	Input,	,	,	
		tr2,	int,	Input,	,	,	
		cg_delay,	double,	Input,	,	,	
		cg_high,	double,	Input,	,	,	
		cg_width,	double,	Input,	,	,	
		cg_rise,	double,	Input,	,	,	
		cg_fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
		tr_high,	double,	Input,	,	,	
		tr_width,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
void Erase_TM_SO(int cgate2, int drain2, int source2, int well2, int tr2, double cg_delay, double cg_high, double cg_width, double cg_rise, double cg_fall, double holdtime, double tr_high, double tr_width);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Erase_TM_src
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 13
	ARGUMENTS:
		cgate,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		tr,	int,	Input,	,	,	
		cg_delay,	double,	Input,	,	,	
		cg_high,	double,	Input,	,	,	
		cg_width,	double,	Input,	,	,	
		cg_rise,	double,	Input,	,	,	
		cg_fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
		tr_high,	double,	Input,	,	,	
		tr_width,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
void Erase_TM_src(int cgate, int drain, int source, int well, int tr, double cg_delay, double cg_high, double cg_width, double cg_rise, double cg_fall, double holdtime, double tr_high, double tr_width);


/* USRLIB MODULE INFORMATION

	MODULE NAME: foly_fuse
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 10
	ARGUMENTS:
		v1,	int,	Input,	,	,	
		v2,	int,	Input,	,	,	
		v0,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		ga_delay,	double,	Input,	,	,	
		ga_high,	double,	Input,	,	,	
		ga_width,	double,	Input,	,	,	
		ga_rise,	double,	Input,	,	,	
		ga_fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
void foly_fuse(int v1, int v2, int v0, int well, double ga_delay, double ga_high, double ga_width, double ga_rise, double ga_fall, double holdtime);


/* USRLIB MODULE INFORMATION

	MODULE NAME: goi_bv
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 14
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		vgstart,	double,	Input,	,	,	
		vgstop,	double,	Input,	,	,	
		vgstep,	double,	Input,	,	,	
		icomp,	double,	Input,	,	,	
		area,	double,	Input,	,	,	
		delaytime,	double,	Input,	,	,	
		vbd,	double *,	Output,	,	,	
		ibd,	double *,	Output,	,	,	
		bd_time,	double *,	Output,	,	,	
		Qbd,	double *,	Output,	,	,	
		debug_print,	int,	Input,	,	0,	1
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void goi_bv(int hi, int lo, int well, double vgstart, double vgstop, double vgstep, double icomp, double area, double delaytime, double * vbd, double * ibd, double * bd_time, double * Qbd, int debug_print);


/* USRLIB MODULE INFORMATION

	MODULE NAME: i4v_ee
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 21
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		body,	int,	Input,	,	,	
		subst,	int,	Input,	-1,	,	
		gnd1,	int,	Input,	-1,	,	
		gnd2,	int,	Input,	-1,	,	
		plc_val,	double,	Input,	1,	,	
		ilimit,	double,	Input,	0.001,	,	
		lo_range,	double,	Input,	1e-8,	,	
		idpgm,	double,	Input,	1e-6,	,	
		steps,	int,	Input,	15,	,	
		delay_time,	double,	Input,	0.01,	,	
		vgs,	double,	Input,	0,	,	
		vdmin,	double,	Input,	0,	,	
		vds,	double,	Input,	8,	,	
		vbs,	double,	Input,	0,	,	
		vbody,	double,	Input,	0,	,	
		mode,	char,	Input,	,	,	
		noise,	char,	Input,	,	,	
		debug,	int,	Input,	0,	,	
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
double  i4v_ee(int, int, int, int, int, int, int, double, double, double, double, int, double, double, double, double, double, double, char, char, int);

/* USRLIB MODULE INFORMATION

	MODULE NAME: ID_Erase_loop
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 28
	ARGUMENTS:
		icgate,	int,	Input,	,	,	
		isgate,	int,	Input,	,	,	
		idrain,	int,	Input,	,	,	
		isource,	int,	Input,	,	,	
		iwell,	int,	Input,	,	,	
		idtarget,	double,	Input,	,	,	
		vicgs,	double,	Input,	,	,	
		vids,	double,	Input,	,	,	
		visgs,	double,	Input,	,	,	
		vibs,	double,	Input,	,	,	
		cgate,	int,	Input,	,	,	
		sgate,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		tr,	int,	Input,	,	,	
		vsgate,	double,	Input,	,	,	
		vdrain,	double,	Input,	,	,	
		vwell,	double,	Input,	,	,	
		cg_delay,	double,	Input,	,	,	
		cg_high,	double,	Input,	,	,	
		cg_width,	double,	Input,	,	,	
		cg_rise,	double,	Input,	,	,	
		cg_fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
		tr_high,	double,	Input,	,	,	
		tr_width,	double,	Input,	,	,	
		ide,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;

	END USRLIB MODULE INFORMATION
*/
void ID_Erase_loop(int icgate, int isgate, int idrain, int isource, int iwell, double idtarget, double vicgs, double vids, double visgs, double vibs, int cgate, int sgate, int drain, int source, int well, int tr, double vsgate, double vdrain, double vwell, double cg_delay, double cg_high, double cg_width, double cg_rise, double cg_fall, double holdtime, double tr_high, double tr_width, double * ide);


/* USRLIB MODULE INFORMATION

	MODULE NAME: kong_init
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
int kong_init();


/* USRLIB MODULE INFORMATION

	MODULE NAME: Loop1
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 21
	ARGUMENTS:
		cg,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		tg,	int,	Input,	,	,	
		c1_height,	double,	Input,	,	,	
		c1_width,	double,	Input,	,	,	
		c1_delay,	double,	Input,	,	,	
		c2_height,	double,	Input,	,	,	
		c2_width,	double,	Input,	,	,	
		c2_delay,	double,	Input,	,	,	
		c11_height,	double,	Input,	,	,	
		c11_width,	double,	Input,	,	,	
		c11_delay,	double,	Input,	,	,	
		c21_height,	double,	Input,	,	,	
		c21_width,	double,	Input,	,	,	
		c21_delay,	double,	Input,	,	,	
		cm_rise,	double,	Input,	,	,	
		cm_fall,	double,	Input,	,	,	
		loop,	int,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include <LBC5_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
#define CRTLIM 50.0e-3
#define MAX(a,b)   ((a)>(b) ? (a) : (b))

Single_vth5_local( int gate, int drain, int source, int well, int tr, double vgstart, double vgstop, double vgstep, double vds, double idtarget, int debug_print )
{
int k;
double co_a, co_b;
int vgsteps;
int k_vg;
double iread;
double vf;
double vt_temp;
double id_temp[1024];
double vg_temp[1024];
double *vth;

vgsteps = (int)(( (vgstop - vgstart)/vgstep ) +0.5);

conpin(SMU1, drain, KI_EOC);
conpin(SMU2, gate,  KI_EOC);
conpin(GND, SMU1L, SMU2L, source, well, tr, KI_EOC);

rangei(SMU1, idtarget * 10);
limiti(SMU1, 1e-3);
limiti(SMU2, 1e-3);

forcev(SMU1, vds);
for (k= 0; k <= vgsteps; k++){
    vf =  vgstart + (vgstep * k);
    forcev(SMU2, vf);
    intgi(SMU1, &iread);

    vg_temp[k] = vf;
    id_temp[k] = iread;
    if(debug_print) printf ("Vg = %g  Id = %g \n",vg_temp[k],id_temp[k]);


    if ( fabs(id_temp[0]) > idtarget) {
        if(debug_print) printf("### Vth = %g\n", vgstart);
        *vth = vgstart;
        break;
        }

    if( (fabs(id_temp[k-1]) < fabs(idtarget)) && ( fabs(id_temp[k]) > fabs(idtarget))){
        k_vg = k;
        co_a = (id_temp[k_vg-1] - id_temp[k_vg]) / (vg_temp[k_vg-1] - vg_temp[k_vg]) ;
        co_b = ((vg_temp[k_vg-1] * id_temp[k_vg]) - (vg_temp[k_vg] * id_temp[k_vg-1]) ) / (vg_temp[k_vg-1] - vg_temp[k_vg]);
        break;
        }
    }


if (((fabs (co_a) > 1e23) || (fabs(co_b)> 1e23))  ){
    if(debug_print) printf("### Vth = %g Out of Range!!\n", -999.999);
    *vth = -999.999;
    }

else{
    if(debug_print) printf("### Vth = %g\n", (idtarget - co_b) / co_a);
    *vth = (idtarget - co_b) / co_a;
    }
}               
	END USRLIB MODULE INFORMATION
*/
void Loop1(int cg, int drain, int source, int sub, int tg, double c1_height, double c1_width, double c1_delay, double c2_height, double c2_width, double c2_delay, double c11_height, double c11_width, double c11_delay, double c21_height, double c21_width, double c21_delay, double cm_rise, double cm_fall, int loop, double holdtime);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Loop_E1C5
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 12
	ARGUMENTS:
		cgate,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		delay,	double,	Input,	,	,	
		positive_voltage,	double,	Input,	,	,	
		negative_voltage,	double,	Input,	,	,	
		width,	double,	Input,	,	,	
		rise,	double,	Input,	,	,	
		fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
		Loop,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
#define CRTLIM 50.0e-3

	END USRLIB MODULE INFORMATION
*/
void  Loop_E1C5(int, int, int, int, double, double, double, double, double, double, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: Loop_E1C5_type1
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 13
	ARGUMENTS:
		cgate,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		tr,	int,	Input,	,	,	
		delay,	double,	Input,	,	,	
		cg_high,	double,	Input,	,	,	
		cg_width,	double,	Input,	,	,	
		tr_high,	double,	Input,	,	,	
		tr_width,	double,	Input,	,	,	
		rise,	double,	Input,	,	,	
		fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
		Loop,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
#define CRTLIM 50.0e-3

	END USRLIB MODULE INFORMATION
*/
void Loop_E1C5_type1(int cgate, int drain, int source, int tr, double delay, double cg_high, double cg_width, double tr_high, double tr_width, double rise, double fall, double holdtime, double Loop);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Loop_E1C5_type18
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 18
	ARGUMENTS:
		cgate,	int,	Input,	,	,	
		sgate,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		tr,	int,	Input,	,	,	
		vsgate,	double,	Input,	,	,	
		vdrain,	double,	Input,	,	,	
		vwell,	double,	Input,	,	,	
		cg_delay,	double,	Input,	,	,	
		cg_high,	double,	Input,	,	,	
		cg_width,	double,	Input,	,	,	
		cg_rise,	double,	Input,	,	,	
		cg_fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
		tr_high,	double,	Input,	,	,	
		tr_width,	double,	Input,	,	,	
		Loop,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
#define CRTLIM 50.0e-3

	END USRLIB MODULE INFORMATION
*/
void Loop_E1C5_type18(int cgate, int sgate, int drain, int source, int well, int tr, double vsgate, double vdrain, double vwell, double cg_delay, double cg_high, double cg_width, double cg_rise, double cg_fall, double holdtime, double tr_high, double tr_width, int Loop);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Loop_E1C5_type19
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 17
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		cgate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		cg_high,	double,	Input,	,	,	
		cg_width,	double,	Input,	,	,	
		cg_rise,	double,	Input,	,	,	
		cg_fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
		sou_high,	double,	Input,	,	,	
		sou_width,	double,	Input,	,	,	
		sou_delay,	double,	Input,	,	,	
		ee_cg_high,	double,	Input,	,	,	
		ee_cg_width,	double,	Input,	,	,	
		ee_oth_high,	double,	Input,	,	,	
		ee_oth_width,	double,	Input,	,	,	
		Loop,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
#define CRTLIM 50.0e-3

	END USRLIB MODULE INFORMATION
*/
void Loop_E1C5_type19(int drain, int cgate, int source, int well, double cg_high, double cg_width, double cg_rise, double cg_fall, double holdtime, double sou_high, double sou_width, double sou_delay, double ee_cg_high, double ee_cg_width, double ee_oth_high, double ee_oth_width, int Loop);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Loop_E1C5_type2
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 21
	ARGUMENTS:
		cgate,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		Loop,	double,	Input,	,	,	
		cg_nw_high_PG,	double,	Input,	,	,	
		cg_nw_width_PG,	double,	Input,	,	,	
		cg_nw_rise_PG,	double,	Input,	,	,	
		cg_nw_fall_PG,	double,	Input,	,	,	
		holdtime_PG,	double,	Input,	,	,	
		sou_high_PG,	double,	Input,	,	,	
		sou_width_PG,	double,	Input,	,	,	
		sou_delay_PG,	double,	Input,	,	,	
		cg_delay_ER,	double,	Input,	,	,	
		cg_high_ER,	double,	Input,	,	,	
		cg_width_ER,	double,	Input,	,	,	
		cg_rise_ER,	double,	Input,	,	,	
		cg_fall_ER,	double,	Input,	,	,	
		holdtime_ER,	double,	Input,	,	,	
		oth_high_ER,	double,	Input,	,	,	
		oth_width_ER,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
#define CRTLIM 50.0e-3

	END USRLIB MODULE INFORMATION
*/
void Loop_E1C5_type2(int cgate, int drain, int source, int well, double Loop, double cg_nw_high_PG, double cg_nw_width_PG, double cg_nw_rise_PG, double cg_nw_fall_PG, double holdtime_PG, double sou_high_PG, double sou_width_PG, double sou_delay_PG, double cg_delay_ER, double cg_high_ER, double cg_width_ER, double cg_rise_ER, double cg_fall_ER, double holdtime_ER, double oth_high_ER, double oth_width_ER);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Loop_E1C5_type3
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 23
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		lo1,	int,	Input,	,	,	
		lo2,	int,	Input,	,	,	
		hi_erase_height,	double,	Input,	,	,	
		hi_erase_width,	double,	Input,	,	,	
		hi_delay,	double,	Input,	,	,	
		hi_height,	double,	Input,	,	,	
		hi_width,	double,	Input,	,	,	
		hi_rise,	double,	Input,	,	,	
		hi_fall,	double,	Input,	,	,	
		lo_delay,	double,	Input,	,	,	
		lo_height,	double,	Input,	,	,	
		lo_width,	double,	Input,	,	,	
		lo_rise,	double,	Input,	,	,	
		lo_fall,	double,	Input,	,	,	
		lo1_delay,	double,	Input,	,	,	
		lo1_height,	double,	Input,	,	,	
		lo1_width,	double,	Input,	,	,	
		lo1_rise,	double,	Input,	,	,	
		lo1_fall,	double,	Input,	,	,	
		loop,	int,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
#define CRTLIM 50.0e-3

	END USRLIB MODULE INFORMATION
*/
void Loop_E1C5_type3(int hi, int lo, int lo1, int lo2, double hi_erase_height, double hi_erase_width, double hi_delay, double hi_height, double hi_width, double hi_rise, double hi_fall, double lo_delay, double lo_height, double lo_width, double lo_rise, double lo_fall, double lo1_delay, double lo1_height, double lo1_width, double lo1_rise, double lo1_fall, int loop, double holdtime);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Loop_E1C5_type4
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 20
	ARGUMENTS:
		g,	int,	Input,	,	,	
		d,	int,	Input,	,	,	
		s,	int,	Input,	,	,	
		psub,	int,	Input,	,	,	
		erase_height,	double,	Input,	,	,	
		erase_width,	double,	Input,	,	,	
		erase_delay,	double,	Input,	,	,	
		g_height,	double,	Input,	,	,	
		g_width,	double,	Input,	,	,	
		g_delay,	double,	Input,	,	,	
		d_height,	double,	Input,	,	,	
		d_width,	double,	Input,	,	,	
		d_delay,	double,	Input,	,	,	
		ps_height,	double,	Input,	,	,	
		ps_width,	double,	Input,	,	,	
		ps_delay,	double,	Input,	,	,	
		cm_rise,	double,	Input,	,	,	
		cm_fall,	double,	Input,	,	,	
		loop,	int,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110u_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
#define CRTLIM 50.0e-3
#define MAX(a,b)   ((a)>(b) ? (a) : (b))
	END USRLIB MODULE INFORMATION
*/
void Loop_E1C5_type4(int g, int d, int s, int psub, double erase_height, double erase_width, double erase_delay, double g_height, double g_width, double g_delay, double d_height, double d_width, double d_delay, double ps_height, double ps_width, double ps_delay, double cm_rise, double cm_fall, int loop, double holdtime);


/* USRLIB MODULE INFORMATION

	MODULE NAME: MTP_ide
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 8
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		cgate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		tr,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		vgs,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vbs,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#define CRTLIM 50.0e-3
	END USRLIB MODULE INFORMATION
*/
double MTP_ide(int drain, int cgate, int source, int tr, int well, double vgs, double vds, double vbs);


/* USRLIB MODULE INFORMATION

	MODULE NAME: OTP_E197
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 10
	ARGUMENTS:
		SelectedGate,	int,	Input,	,	,	
		BinLine,	int,	Input,	,	,	
		SelectedLine,	int,	Input,	,	,	
		Nwell,	int,	Input,	,	,	
		pls_delay,	double,	Input,	,	,	
		pls_high,	double,	Input,	,	,	
		pls_width,	double,	Input,	,	,	
		pls_rise,	double,	Input,	,	,	
		pls_fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
void  OTP_E197(int, int, int, int, double, double, double, double, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: OTP_E197_r1
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 10
	ARGUMENTS:
		cgate,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		cg_delay,	double,	Input,	,	,	
		cg_high,	double,	Input,	,	,	
		cg_width,	double,	Input,	,	,	
		cg_rise,	double,	Input,	,	,	
		cg_fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
void  OTP_E197_r1(int, int, int, int, double, double, double, double, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: OTP_E197_r2
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 12
	ARGUMENTS:
		cgate,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		cg_delay,	double,	Input,	,	,	
		cg_high,	double,	Input,	,	,	
		cg_width,	double,	Input,	,	,	
		cg_rise,	double,	Input,	,	,	
		cg_fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
		oth_high,	double,	Input,	,	,	
		oth_width,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
void OTP_E197_r2(int cgate, int drain, int source, int well, double cg_delay, double cg_high, double cg_width, double cg_rise, double cg_fall, double holdtime, double oth_high, double oth_width);


/* USRLIB MODULE INFORMATION

	MODULE NAME: pgm_otp2
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
void pgm_otp2(int drn, int cg, int src, int tun, double vcg, int tp);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Prog_E1C5
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 12
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		cgate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		cg_high,	double,	Input,	,	,	
		cg_width,	double,	Input,	,	,	
		cg_rise,	double,	Input,	,	,	
		cg_fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
		sou_high,	double,	Input,	,	,	
		sou_width,	double,	Input,	,	,	
		sou_delay,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
#define MAX(a,b)   ((a)>(b) ? (a) : (b))
	END USRLIB MODULE INFORMATION
*/
void  Prog_E1C5(int, int, int, int, double, double, double, double, double, double, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: Prog_E1C5_type1
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 10
	ARGUMENTS:
		cgate,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		tr,	int,	Input,	,	,	
		cg_delay,	double,	Input,	,	,	
		cg_high,	double,	Input,	,	,	
		cg_width,	double,	Input,	,	,	
		cg_rise,	double,	Input,	,	,	
		cg_fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
void Prog_E1C5_type1(int cgate, int drain, int source, int tr, double cg_delay, double cg_high, double cg_width, double cg_rise, double cg_fall, double holdtime);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Prog_E1C5_type2
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 12
	ARGUMENTS:
		cgate,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		cg_nw_high,	double,	Input,	,	,	
		cg_nw_width,	double,	Input,	,	,	
		cg_nw_rise,	double,	Input,	,	,	
		cg_nw_fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
		sou_high,	double,	Input,	,	,	
		sou_width,	double,	Input,	,	,	
		sou_delay,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
void Prog_E1C5_type2(int cgate, int drain, int source, int well, double cg_nw_high, double cg_nw_width, double cg_nw_rise, double cg_nw_fall, double holdtime, double sou_high, double sou_width, double sou_delay);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Prog_E1C5_type3
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 13
	ARGUMENTS:
		cgate,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		cg_nw_high,	double,	Input,	,	,	
		cg_nw_width,	double,	Input,	,	,	
		cg_nw_rise,	double,	Input,	,	,	
		cg_nw_fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
		sou_high,	double,	Input,	,	,	
		sou_width,	double,	Input,	,	,	
		sou_delay,	double,	Input,	,	,	
		drain_v_smu1,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
void Prog_E1C5_type3(int cgate, int drain, int source, int well, double cg_nw_high, double cg_nw_width, double cg_nw_rise, double cg_nw_fall, double holdtime, double sou_high, double sou_width, double sou_delay, double drain_v_smu1);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Prog_Eon
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 13
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		ga_high,	double,	Input,	,	,	
		ga_width,	double,	Input,	,	,	
		ga_rise,	double,	Input,	,	,	
		ga_fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
		vd_high,	double,	Input,	,	,	
		vd_width,	double,	Input,	,	,	
		dr_delay,	double,	Input,	,	,	
		TRGholdtime,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
void Prog_Eon(int drain, int gate, int source, int well, double ga_high, double ga_width, double ga_rise, double ga_fall, double holdtime, double vd_high, double vd_width, double dr_delay, double TRGholdtime);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Prog_Eon_k
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 12
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		ga_high,	double,	Input,	,	,	
		ga_width,	double,	Input,	,	,	
		ga_rise,	double,	Input,	,	,	
		ga_fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
		vd_high,	double,	Input,	,	,	
		vd_width,	double,	Input,	,	,	
		dr_delay,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
void Prog_Eon_k(int drain, int gate, int source, int well, double ga_high, double ga_width, double ga_rise, double ga_fall, double holdtime, double vd_high, double vd_width, double dr_delay);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Prog_Eon_r1
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 13
	ARGUMENTS:
		gate,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		ga_delay,	double,	Input,	,	,	
		ga_high,	double,	Input,	,	,	
		ga_width,	double,	Input,	,	,	
		ga_rise,	double,	Input,	,	,	
		ga_fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
		vb_high,	double,	Input,	,	,	
		vb_width,	double,	Input,	,	,	
		TRGholdtime,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
void Prog_Eon_r1(int gate, int drain, int source, int well, double ga_delay, double ga_high, double ga_width, double ga_rise, double ga_fall, double holdtime, double vb_high, double vb_width, double TRGholdtime);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Prog_Erase_imsi
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 25
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		cgate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		tr,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		cg_high,	double,	Input,	,	,	
		cg_width,	double,	Input,	,	,	
		cg_rise,	double,	Input,	,	,	
		cg_fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
		tr_high,	double,	Input,	,	,	
		tr_width,	double,	Input,	,	,	
		tr_delay,	double,	Input,	,	,	
		cg_delay,	double,	Input,	,	,	
		vgstart,	double,	Input,	,	,	
		vgstop,	double,	Input,	,	,	
		vgstep,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vgs,	double,	Input,	,	,	
		vbs,	double,	Input,	,	,	
		idtarget,	double,	Input,	,	,	
		debug_print,	int,	Input,	,	0,	1
		ide,	double *,	Output,	,	,	
		vth,	double *,	Output,	,	,	
		p_v,	int *,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
void Prog_Erase_imsi(int drain, int cgate, int source, int tr, int well, double cg_high, double cg_width, double cg_rise, double cg_fall, double holdtime, double tr_high, double tr_width, double tr_delay, double cg_delay, double vgstart, double vgstop, double vgstep, double vds, double vgs, double vbs, double idtarget, int debug_print, double * ide, double * vth, int * p_v);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Prog_Erase_set
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 30
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		cgate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		tr,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		cg_high_pg,	double,	Input,	,	,	
		cg_width_pg,	double,	Input,	,	,	
		cg_high,	double,	Input,	,	,	
		cg_width,	double,	Input,	,	,	
		cg_rise,	double,	Input,	,	,	
		cg_fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
		tr_high_pg,	double,	Input,	,	,	
		tr_width_pg,	double,	Input,	,	,	
		tr_high,	double,	Input,	,	,	
		tr_width,	double,	Input,	,	,	
		tr_delay,	double,	Input,	,	,	
		cg_delay,	double,	Input,	,	,	
		vgstart,	double,	Input,	,	,	
		vgstop,	double,	Input,	,	,	
		vgstep,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vgs,	double,	Input,	,	,	
		vbs,	double,	Input,	,	,	
		idtarget,	double,	Input,	,	,	
		debug_print,	int,	Input,	,	0,	1
		ide,	double *,	Output,	,	,	
		vte,	double *,	Output,	,	,	
		vtp,	double *,	Output,	,	,	
		iders,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
#define CRTLIM 50.0e-3

	END USRLIB MODULE INFORMATION
*/
void Prog_Erase_set(int drain, int cgate, int source, int tr, int well, double cg_high_pg, double cg_width_pg, double cg_high, double cg_width, double cg_rise, double cg_fall, double holdtime, double tr_high_pg, double tr_width_pg, double tr_high, double tr_width, double tr_delay, double cg_delay, double vgstart, double vgstop, double vgstep, double vds, double vgs, double vbs, double idtarget, int debug_print, double * ide, double * vte, double * vtp, double * iders);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Prog_Erase_set_DP
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 30
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		cgate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		tr,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		cg_high_pg,	double,	Input,	,	,	
		cg_width_pg,	double,	Input,	,	,	
		cg_high,	double,	Input,	,	,	
		cg_width,	double,	Input,	,	,	
		cg_rise,	double,	Input,	,	,	
		cg_fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
		tr_high_pg,	double,	Input,	,	,	
		tr_width_pg,	double,	Input,	,	,	
		tr_high,	double,	Input,	,	,	
		tr_width,	double,	Input,	,	,	
		tr_delay,	double,	Input,	,	,	
		cg_delay,	double,	Input,	,	,	
		vgstart,	double,	Input,	,	,	
		vgstop,	double,	Input,	,	,	
		vgstep,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vgs,	double,	Input,	,	,	
		vbs,	double,	Input,	,	,	
		idtarget,	double,	Input,	,	,	
		debug_print,	int,	Input,	,	0,	1
		ide,	double *,	Output,	,	,	
		vte,	double *,	Output,	,	,	
		vtp,	double *,	Output,	,	,	
		iders,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
#define CRTLIM 50.0e-3

	END USRLIB MODULE INFORMATION
*/
void Prog_Erase_set_DP(int drain, int cgate, int source, int tr, int well, double cg_high_pg, double cg_width_pg, double cg_high, double cg_width, double cg_rise, double cg_fall, double holdtime, double tr_high_pg, double tr_width_pg, double tr_high, double tr_width, double tr_delay, double cg_delay, double vgstart, double vgstop, double vgstep, double vds, double vgs, double vbs, double idtarget, int debug_print, double * ide, double * vte, double * vtp, double * iders);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Prog_Erase_set_so
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 35
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		cgate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		tr,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		drain2,	int,	Input,	,	,	
		cgate2,	int,	Input,	,	,	
		source2,	int,	Input,	,	,	
		well2,	int,	Input,	,	,	
		tr2,	int,	Input,	,	,	
		cg_high_pg,	double,	Input,	,	,	
		cg_width_pg,	double,	Input,	,	,	
		cg_high,	double,	Input,	,	,	
		cg_width,	double,	Input,	,	,	
		cg_rise,	double,	Input,	,	,	
		cg_fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
		tr_high_pg,	double,	Input,	,	,	
		tr_width_pg,	double,	Input,	,	,	
		tr_high,	double,	Input,	,	,	
		tr_width,	double,	Input,	,	,	
		tr_delay,	double,	Input,	,	,	
		cg_delay,	double,	Input,	,	,	
		vgstart,	double,	Input,	,	,	
		vgstop,	double,	Input,	,	,	
		vgstep,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vgs,	double,	Input,	,	,	
		vbs,	double,	Input,	,	,	
		idtarget,	double,	Input,	,	,	
		debug_print,	int,	Input,	,	0,	1
		ide,	double *,	Output,	,	,	
		vte,	double *,	Output,	,	,	
		vtp,	double *,	Output,	,	,	
		iders,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
#define CRTLIM 50.0e-3

	END USRLIB MODULE INFORMATION
*/
void Prog_Erase_set_so(int drain, int cgate, int source, int tr, int well, int drain2, int cgate2, int source2, int well2, int tr2, double cg_high_pg, double cg_width_pg, double cg_high, double cg_width, double cg_rise, double cg_fall, double holdtime, double tr_high_pg, double tr_width_pg, double tr_high, double tr_width, double tr_delay, double cg_delay, double vgstart, double vgstop, double vgstep, double vds, double vgs, double vbs, double idtarget, int debug_print, double * ide, double * vte, double * vtp, double * iders);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Prog_init
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 18
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		cgate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		tr,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		cg_high_pg,	double,	Input,	,	,	
		cg_width_pg,	double,	Input,	,	,	
		cg_high,	double,	Input,	,	,	
		cg_width,	double,	Input,	,	,	
		cg_rise,	double,	Input,	,	,	
		cg_fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
		tr_high_pg,	double,	Input,	,	,	
		tr_width_pg,	double,	Input,	,	,	
		tr_high,	double,	Input,	,	,	
		tr_width,	double,	Input,	,	,	
		tr_delay,	double,	Input,	,	,	
		cg_delay,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
#define CRTLIM 50.0e-3

	END USRLIB MODULE INFORMATION
*/
void Prog_init(int drain, int cgate, int source, int tr, int well, double cg_high_pg, double cg_width_pg, double cg_high, double cg_width, double cg_rise, double cg_fall, double holdtime, double tr_high_pg, double tr_width_pg, double tr_high, double tr_width, double tr_delay, double cg_delay);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Prog_init_DP
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 18
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		cgate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		tr,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		cg_high_pg,	double,	Input,	,	,	
		cg_width_pg,	double,	Input,	,	,	
		cg_high,	double,	Input,	,	,	
		cg_width,	double,	Input,	,	,	
		cg_rise,	double,	Input,	,	,	
		cg_fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
		tr_high_pg,	double,	Input,	,	,	
		tr_width_pg,	double,	Input,	,	,	
		tr_high,	double,	Input,	,	,	
		tr_width,	double,	Input,	,	,	
		tr_delay,	double,	Input,	,	,	
		cg_delay,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
#define CRTLIM 50.0e-3

	END USRLIB MODULE INFORMATION
*/
void Prog_init_DP(int drain, int cgate, int source, int tr, int well, double cg_high_pg, double cg_width_pg, double cg_high, double cg_width, double cg_rise, double cg_fall, double holdtime, double tr_high_pg, double tr_width_pg, double tr_high, double tr_width, double tr_delay, double cg_delay);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Prog_ks
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 17
	ARGUMENTS:
		cgate,	int,	Input,	,	,	
		sgate,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		tr,	int,	Input,	,	,	
		vsgate,	double,	Input,	,	,	
		vdrain,	double,	Input,	,	,	
		vwell,	double,	Input,	,	,	
		cg_high,	double,	Input,	,	,	
		cg_width,	double,	Input,	,	,	
		cg_rise,	double,	Input,	,	,	
		cg_fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
		tr_high,	double,	Input,	,	,	
		tr_width,	double,	Input,	,	,	
		tr_delay,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
void Prog_ks(int cgate, int sgate, int drain, int source, int well, int tr, double vsgate, double vdrain, double vwell, double cg_high, double cg_width, double cg_rise, double cg_fall, double holdtime, double tr_high, double tr_width, double tr_delay);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Prog_ks_0627
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 17
	ARGUMENTS:
		cgate,	int,	Input,	,	,	
		sgate,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		tr,	int,	Input,	,	,	
		vsgate,	double,	Input,	,	,	
		vdrain,	double,	Input,	,	,	
		vwell,	double,	Input,	,	,	
		cg_high,	double,	Input,	,	,	
		cg_width,	double,	Input,	,	,	
		cg_rise,	double,	Input,	,	,	
		cg_fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
		tr_high,	double,	Input,	,	,	
		tr_width,	double,	Input,	,	,	
		tr_delay,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
void Prog_ks_0627(int cgate, int sgate, int drain, int source, int well, int tr, double vsgate, double vdrain, double vwell, double cg_high, double cg_width, double cg_rise, double cg_fall, double holdtime, double tr_high, double tr_width, double tr_delay);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Prog_ks_1O2P
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 17
	ARGUMENTS:
		cgate,	int,	Input,	,	,	
		sgate,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		tr,	int,	Input,	,	,	
		vsgate,	double,	Input,	,	,	
		vdrain,	double,	Input,	,	,	
		vwell,	double,	Input,	,	,	
		cg_high,	double,	Input,	,	,	
		cg_width,	double,	Input,	,	,	
		cg_rise,	double,	Input,	,	,	
		cg_fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
		tr_high,	double,	Input,	,	,	
		tr_width,	double,	Input,	,	,	
		tr_delay,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
void Prog_ks_1O2P(int cgate, int sgate, int drain, int source, int well, int tr, double vsgate, double vdrain, double vwell, double cg_high, double cg_width, double cg_rise, double cg_fall, double holdtime, double tr_high, double tr_width, double tr_delay);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Prog_ks_1O2P_C
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 18
	ARGUMENTS:
		cgate,	int,	Input,	,	,	
		sgate,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		tr,	int,	Input,	,	,	
		vsgate,	double,	Input,	,	,	
		vdrain,	double,	Input,	,	,	
		vwell,	double,	Input,	,	,	
		cg_high,	double,	Input,	,	,	
		cg_width,	double,	Input,	,	,	
		cg_rise,	double,	Input,	,	,	
		cg_fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
		tr_high,	double,	Input,	,	,	
		tr_width,	double,	Input,	,	,	
		tr_delay,	double,	Input,	,	,	
		TRGholdtime,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
void Prog_ks_1O2P_C(int cgate, int sgate, int drain, int source, int well, int tr, double vsgate, double vdrain, double vwell, double cg_high, double cg_width, double cg_rise, double cg_fall, double holdtime, double tr_high, double tr_width, double tr_delay, double TRGholdtime);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Prog_OTP
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 13
	ARGUMENTS:
		V_PulsePin,	int,	Input,	,	,	
		V_ForcePin,	int,	Input,	,	,	
		GroundPin1,	int,	Input,	,	,	
		GroundPin2,	int,	Input,	,	,	
		GroundPin3,	int,	Input,	,	,	
		GroundPin4,	int,	Input,	,	,	
		Pulse_delay,	double,	Input,	,	,	
		Pulse_high,	double,	Input,	,	,	
		Pulse_width,	double,	Input,	,	,	
		Pulse_rise,	double,	Input,	,	,	
		Pulse_fall,	double,	Input,	,	,	
		VoltageForce,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
void  Prog_OTP(int, int, int, int, int, int, double, double, double, double, double, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: Prog_TM
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 13
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		cgate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		tr,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		cg_high,	double,	Input,	,	,	
		cg_width,	double,	Input,	,	,	
		cg_rise,	double,	Input,	,	,	
		cg_fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
		tr_high,	double,	Input,	,	,	
		tr_width,	double,	Input,	,	,	
		tr_delay,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
void Prog_TM(int drain, int cgate, int source, int tr, int well, double cg_high, double cg_width, double cg_rise, double cg_fall, double holdtime, double tr_high, double tr_width, double tr_delay);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Prog_TM_imsi
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 12
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		cgate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		tr,	int,	Input,	,	,	
		cg_high,	double,	Input,	,	,	
		cg_width,	double,	Input,	,	,	
		cg_rise,	double,	Input,	,	,	
		cg_fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
		tr_high,	double,	Input,	,	,	
		tr_width,	double,	Input,	,	,	
		tr_delay,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
void Prog_TM_imsi(int drain, int cgate, int source, int tr, double cg_high, double cg_width, double cg_rise, double cg_fall, double holdtime, double tr_high, double tr_width, double tr_delay);


/* USRLIB MODULE INFORMATION

	MODULE NAME: pulse2s1
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 18
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		lo1,	int,	Input,	,	,	
		lo2,	int,	Input,	,	,	
		hi_delay,	double,	Input,	,	,	
		hi_high,	double,	Input,	,	,	
		hi_width,	double,	Input,	,	,	
		hi_rise,	double,	Input,	,	,	
		hi_fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
		lo_delay,	double,	Input,	,	,	
		lo_high,	double,	Input,	,	,	
		lo_width,	double,	Input,	,	,	
		lo_rise,	double,	Input,	,	,	
		lo_fall,	double,	Input,	,	,	
		dual,	int,	Input,	,	,	
		smu1_set,	int,	Input,	,	,	
		smu1_v,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
void pulse2s1(int hi, int lo, int lo1, int lo2, double hi_delay, double hi_high, double hi_width, double hi_rise, double hi_fall, double holdtime, double lo_delay, double lo_high, double lo_width, double lo_rise, double lo_fall, int dual, int smu1_set, double smu1_v);


/* USRLIB MODULE INFORMATION

	MODULE NAME: pulse3
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 20
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		lo1,	int,	Input,	,	,	
		lo2,	int,	Input,	,	,	
		hi_delay,	double,	Input,	,	,	
		hi_height,	double,	Input,	,	,	
		hi_width,	double,	Input,	,	,	
		hi_rise,	double,	Input,	,	,	
		hi_fall,	double,	Input,	,	,	
		lo_delay,	double,	Input,	,	,	
		lo_height,	double,	Input,	,	,	
		lo_width,	double,	Input,	,	,	
		lo_rise,	double,	Input,	,	,	
		lo_fall,	double,	Input,	,	,	
		lo1_delay,	double,	Input,	,	,	
		lo1_height,	double,	Input,	,	,	
		lo1_width,	double,	Input,	,	,	
		lo1_rise,	double,	Input,	,	,	
		lo1_fall,	double,	Input,	,	,	
		pulse_count,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
void pulse3(int hi, int lo, int lo1, int lo2, double hi_delay, double hi_height, double hi_width, double hi_rise, double hi_fall, double lo_delay, double lo_height, double lo_width, double lo_rise, double lo_fall, double lo1_delay, double lo1_height, double lo1_width, double lo1_rise, double lo1_fall, int pulse_count);


/* USRLIB MODULE INFORMATION

	MODULE NAME: pulse3_loop
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 20
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		lo1,	int,	Input,	,	,	
		lo2,	int,	Input,	,	,	
		hi_height,	double,	Input,	,	,	
		hi_width,	double,	Input,	,	,	
		lo_height,	double,	Input,	,	,	
		lo_width,	double,	Input,	,	,	
		lo1_height,	double,	Input,	,	,	
		lo1_width,	double,	Input,	,	,	
		hi_height_c1,	double,	Input,	,	,	
		hi_width_c1,	double,	Input,	,	,	
		lo_height_c1,	double,	Input,	,	,	
		lo_width_c1,	double,	Input,	,	,	
		lo1_width_c1,	double,	Input,	,	,	
		lo1_height_c1,	double,	Input,	,	,	
		period,	double,	Input,	,	,	
		vtati,	double,	Input,	,	,	
		ide,	double,	Input,	,	,	
		loop_count,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
#define sRange1 1e-8
#define mRange1 1e-6
#define lRange1 1e-3


#define sRange2 1e-8
#define mRange2 1e-6
#define lRange2 1e-3

#define mDelay1 1e-6
#define mDelay2 1e-6

#define tRange 1e-6

	END USRLIB MODULE INFORMATION
*/
void pulse3_loop(int hi, int lo, int lo1, int lo2, double hi_height, double hi_width, double lo_height, double lo_width, double lo1_height, double lo1_width, double hi_height_c1, double hi_width_c1, double lo_height_c1, double lo_width_c1, double lo1_width_c1, double lo1_height_c1, double period, double vtati, double ide, int loop_count);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Single_Erase
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 10
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		ga_delay,	double,	Input,	,	,	
		ga_high,	double,	Input,	,	,	
		ga_width,	double,	Input,	,	,	
		ga_rise,	double,	Input,	,	,	
		ga_fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
void Single_Erase(int drain, int gate, int source, int well, double ga_delay, double ga_high, double ga_width, double ga_rise, double ga_fall, double holdtime);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Single_erase_loop
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 23
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		prog_delay,	double,	Input,	,	,	
		prog_high,	double,	Input,	,	,	
		prog_width,	double,	Input,	,	,	
		prog_rise,	double,	Input,	,	,	
		prog_fall,	double,	Input,	,	,	
		erase_delay,	double,	Input,	,	,	
		erase_high,	double,	Input,	,	,	
		erase_width,	double,	Input,	,	,	
		erase_rise,	double,	Input,	,	,	
		erase_fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
		itarget,	double,	Input,	,	,	
		vgstart,	double,	Input,	,	,	
		vgstop,	double,	Input,	,	,	
		vgstep,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		debug_print,	int,	Input,	,	0,	1
		vt_loop,	D_ARRAY_T,	Output,	,	,	
		loopcnt,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
void Single_erase_loop(int drain, int gate, int source, int well, double prog_delay, double prog_high, double prog_width, double prog_rise, double prog_fall, double erase_delay, double erase_high, double erase_width, double erase_rise, double erase_fall, double holdtime, double itarget, double vgstart, double vgstop, double vgstep, double vds, int debug_print, double *vt_loop, int loopcnt);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Single_Prog
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 10
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		ga_delay,	double,	Input,	,	,	
		ga_high,	double,	Input,	,	,	
		ga_width,	double,	Input,	,	,	
		ga_rise,	double,	Input,	,	,	
		ga_fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
void Single_Prog(int drain, int gate, int source, int well, double ga_delay, double ga_high, double ga_width, double ga_rise, double ga_fall, double holdtime);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Single_Prog_org
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 10
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		ga_delay,	double,	Input,	,	,	
		ga_high,	double,	Input,	,	,	
		ga_width,	double,	Input,	,	,	
		ga_rise,	double,	Input,	,	,	
		ga_fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
void Single_Prog_org(int drain, int gate, int source, int well, double ga_delay, double ga_high, double ga_width, double ga_rise, double ga_fall, double holdtime);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Single_Pulse_loop
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 17
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		loopcount,	int,	Input,	,	,	
		prog_delay,	double,	Input,	,	,	
		prog_high,	double,	Input,	,	,	
		prog_width,	double,	Input,	,	,	
		prog_rise,	double,	Input,	,	,	
		prog_fall,	double,	Input,	,	,	
		erase_delay,	double,	Input,	,	,	
		erase_high,	double,	Input,	,	,	
		erase_width,	double,	Input,	,	,	
		erase_rise,	double,	Input,	,	,	
		erase_fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
		debug_print,	int,	Input,	,	0,	1
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
void Single_Pulse_loop(int drain, int gate, int source, int well, int loopcount, double prog_delay, double prog_high, double prog_width, double prog_rise, double prog_fall, double erase_delay, double erase_high, double erase_width, double erase_rise, double erase_fall, double holdtime, int debug_print);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Single_vth
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 11
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		vgstart,	double,	Input,	,	,	
		vgstop,	double,	Input,	,	,	
		vgstep,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		idtarget,	double,	Input,	,	,	
		vth,	double *,	Output,	,	,	
		debug_print,	int,	Input,	,	0,	1
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void Single_vth(int drain, int gate, int source, int well, double vgstart, double vgstop, double vgstep, double vds, double idtarget, double * vth, int debug_print);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Single_vth5
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 12
	ARGUMENTS:
		gate,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		tr,	int,	Input,	,	,	
		vgstart,	double,	Input,	,	,	
		vgstop,	double,	Input,	,	,	
		vgstep,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		idtarget,	double,	Input,	,	,	
		vth,	double *,	Output,	,	,	
		debug_print,	int,	Input,	,	0,	1
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void Single_vth5(int gate, int drain, int source, int well, int tr, double vgstart, double vgstop, double vgstep, double vds, double idtarget, double * vth, int debug_print);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Single_vth5_ide
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 13
	ARGUMENTS:
		cgate,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		tr,	int,	Input,	,	,	
		vgstart,	double,	Input,	,	,	
		vgstop,	double,	Input,	,	,	
		vgstep,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		idtarget,	double,	Input,	,	,	
		ide,	double *,	Output,	,	,	
		vth,	double *,	Output,	,	,	
		debug_print,	int,	Input,	,	0,	1
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void Single_vth5_ide(int cgate, int drain, int source, int well, int tr, double vgstart, double vgstop, double vgstep, double vds, double idtarget, double * ide, double * vth, int debug_print);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Single_vth5_imsi
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 13
	ARGUMENTS:
		gate,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		tr,	int,	Input,	,	,	
		vgstart,	double,	Input,	,	,	
		vgstop,	double,	Input,	,	,	
		vgstep,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vbs,	double,	Input,	,	,	
		idtarget,	double,	Input,	,	,	
		vth,	double *,	Output,	,	,	
		debug_print,	int,	Input,	,	0,	1
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void Single_vth5_imsi(int gate, int drain, int source, int well, int tr, double vgstart, double vgstop, double vgstep, double vds, double vbs, double idtarget, double * vth, int debug_print);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Single_vth5_ks
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 15
	ARGUMENTS:
		gate,	int,	Input,	,	,	
		sgate,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		tr,	int,	Input,	,	,	
		vgstart,	double,	Input,	,	,	
		vgstop,	double,	Input,	,	,	
		vgstep,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vsgate,	double,	Input,	,	,	
		vwell,	double,	Input,	,	,	
		idtarget,	double,	Input,	,	,	
		vth,	double *,	Output,	,	,	
		debug_print,	int,	Input,	,	0,	1
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void  Single_vth5_ks(int, int, int, int, int, int, double, double, double, double, double, double, double, double *, int);

/* USRLIB MODULE INFORMATION

	MODULE NAME: Single_vth_celli
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 11
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		vgstart,	double,	Input,	,	,	
		vgstop,	double,	Input,	,	,	
		vgstep,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		idtarget,	double,	Input,	,	,	
		vth,	double *,	Output,	,	,	
		cell_i,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void Single_vth_celli(int drain, int gate, int source, int well, double vgstart, double vgstop, double vgstep, double vds, double idtarget, double * vth, double * cell_i);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Single_vth_imsi
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 12
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		vgstart,	double,	Input,	,	,	
		vgstop,	double,	Input,	,	,	
		vgstep,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vbs,	double,	Input,	,	,	
		idtarget,	double,	Input,	,	,	
		vth,	double *,	Output,	,	,	
		debug_print,	int,	Input,	,	0,	1
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void Single_vth_imsi(int drain, int gate, int source, int well, double vgstart, double vgstop, double vgstep, double vds, double vbs, double idtarget, double * vth, int debug_print);


/* USRLIB MODULE INFORMATION

	MODULE NAME: smu1_pulse_loop
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 5
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		low,	int,	Input,	,	,	
		bt,	int,	Input,	,	,	
		force,	double,	Input,	,	,	
		sec,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
void smu1_pulse_loop(int hi, int low, int bt, double force, double sec);


/* USRLIB MODULE INFORMATION

	MODULE NAME: smu_pulse_loop
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 5
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		low,	int,	Input,	,	,	
		bt,	int,	Input,	,	,	
		force,	double,	Input,	,	,	
		sec,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
void smu_pulse_loop(int hi, int low, int bt, double force, double sec);


/* USRLIB MODULE INFORMATION

	MODULE NAME: sn_pulse_init
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void sn_pulse_init();


/* USRLIB MODULE INFORMATION

	MODULE NAME: TriErase
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 16
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		grcl,	int,	Input,	,	,	
		gse,	int,	Input,	,	,	
		gstr,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vrcl,	double,	Input,	,	,	
		vstr,	double,	Input,	,	,	
		vss,	double,	Input,	,	,	
		vse_delay,	double,	Input,	,	,	
		vse_high,	double,	Input,	,	,	
		vse_width,	double,	Input,	,	,	
		vse_rise,	double,	Input,	,	,	
		vse_fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
void TriErase(int drain, int grcl, int gse, int gstr, int source, int well, double vds, double vrcl, double vstr, double vss, double vse_delay, double vse_high, double vse_width, double vse_rise, double vse_fall, double holdtime);


/* USRLIB MODULE INFORMATION

	MODULE NAME: TriErase_071211
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 16
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		grcl,	int,	Input,	,	,	
		gse,	int,	Input,	,	,	
		gstr,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vrcl,	double,	Input,	,	,	
		vstr,	double,	Input,	,	,	
		vss,	double,	Input,	,	,	
		vse_delay,	double,	Input,	,	,	
		vse_high,	double,	Input,	,	,	
		vse_width,	double,	Input,	,	,	
		vse_rise,	double,	Input,	,	,	
		vse_fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
void TriErase_071211(int drain, int grcl, int gse, int gstr, int source, int well, double vds, double vrcl, double vstr, double vss, double vse_delay, double vse_high, double vse_width, double vse_rise, double vse_fall, double holdtime);


/* USRLIB MODULE INFORMATION

	MODULE NAME: TriErase_1207
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 16
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		grcl,	int,	Input,	,	,	
		gse,	int,	Input,	,	,	
		gstr,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vrcl,	double,	Input,	,	,	
		vstr,	double,	Input,	,	,	
		vss,	double,	Input,	,	,	
		vse_delay,	double,	Input,	,	,	
		vse_high,	double,	Input,	,	,	
		vse_width,	double,	Input,	,	,	
		vse_rise,	double,	Input,	,	,	
		vse_fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
void TriErase_1207(int drain, int grcl, int gse, int gstr, int source, int well, double vds, double vrcl, double vstr, double vss, double vse_delay, double vse_high, double vse_width, double vse_rise, double vse_fall, double holdtime);


/* USRLIB MODULE INFORMATION

	MODULE NAME: TriErase_org
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 16
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		grcl,	int,	Input,	,	,	
		gse,	int,	Input,	,	,	
		gstr,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vrcl,	double,	Input,	,	,	
		vstr,	double,	Input,	,	,	
		vss,	double,	Input,	,	,	
		vse_delay,	double,	Input,	,	,	
		vse_high,	double,	Input,	,	,	
		vse_width,	double,	Input,	,	,	
		vse_rise,	double,	Input,	,	,	
		vse_fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
void TriErase_org(int drain, int grcl, int gse, int gstr, int source, int well, double vds, double vrcl, double vstr, double vss, double vse_delay, double vse_high, double vse_width, double vse_rise, double vse_fall, double holdtime);


/* USRLIB MODULE INFORMATION

	MODULE NAME: TriErase_smu
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 16
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		grcl,	int,	Input,	,	,	
		gse,	int,	Input,	,	,	
		gstr,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vrcl,	double,	Input,	,	,	
		vstr,	double,	Input,	,	,	
		vss,	double,	Input,	,	,	
		vse_delay,	double,	Input,	,	,	
		vse_high,	double,	Input,	,	,	
		vse_width,	double,	Input,	,	,	
		vse_rise,	double,	Input,	,	,	
		vse_fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
void TriErase_smu(int drain, int grcl, int gse, int gstr, int source, int well, double vds, double vrcl, double vstr, double vss, double vse_delay, double vse_high, double vse_width, double vse_rise, double vse_fall, double holdtime);


/* USRLIB MODULE INFORMATION

	MODULE NAME: TriErase_time
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 16
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		grcl,	int,	Input,	,	,	
		gse,	int,	Input,	,	,	
		gstr,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vrcl,	double,	Input,	,	,	
		vstr,	double,	Input,	,	,	
		vss,	double,	Input,	,	,	
		vse_delay,	double,	Input,	,	,	
		vse_high,	double,	Input,	,	,	
		vse_width,	double,	Input,	,	,	
		vse_rise,	double,	Input,	,	,	
		vse_fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
void TriErase_time(int drain, int grcl, int gse, int gstr, int source, int well, double vds, double vrcl, double vstr, double vss, double vse_delay, double vse_high, double vse_width, double vse_rise, double vse_fall, double holdtime);


/* USRLIB MODULE INFORMATION

	MODULE NAME: TriEraseON
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 16
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		grcl,	int,	Input,	,	,	
		gse,	int,	Input,	,	,	
		gstr,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vrcl,	double,	Input,	,	,	
		vstr,	double,	Input,	,	,	
		vss,	double,	Input,	,	,	
		vse_delay,	double,	Input,	,	,	
		vse_high,	double,	Input,	,	,	
		vse_width,	double,	Input,	,	,	
		vse_rise,	double,	Input,	,	,	
		vse_fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
void TriEraseON(int drain, int grcl, int gse, int gstr, int source, int well, double vds, double vrcl, double vstr, double vss, double vse_delay, double vse_high, double vse_width, double vse_rise, double vse_fall, double holdtime);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Triloop
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 27
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		grcl,	int,	Input,	,	,	
		gse,	int,	Input,	,	,	
		gstr,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		loopcount,	int,	Input,	,	,	
		vds_prog,	double,	Input,	,	,	
		vrcl_prog,	double,	Input,	,	,	
		vstr_prog,	double,	Input,	,	,	
		vss_prog,	double,	Input,	,	,	
		prog_delay,	double,	Input,	,	,	
		prog_high,	double,	Input,	,	,	
		prog_width,	double,	Input,	,	,	
		prog_rise,	double,	Input,	,	,	
		prog_fall,	double,	Input,	,	,	
		vds_erase,	double,	Input,	,	,	
		vrcl_erase,	double,	Input,	,	,	
		vstr_erase,	double,	Input,	,	,	
		vss_erase,	double,	Input,	,	,	
		erase_delay,	double,	Input,	,	,	
		erase_high,	double,	Input,	,	,	
		erase_width,	double,	Input,	,	,	
		erase_rise,	double,	Input,	,	,	
		erase_fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
		debug_print,	int,	Input,	,	0,	1
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
void Triloop(int drain, int grcl, int gse, int gstr, int source, int well, int loopcount, double vds_prog, double vrcl_prog, double vstr_prog, double vss_prog, double prog_delay, double prog_high, double prog_width, double prog_rise, double prog_fall, double vds_erase, double vrcl_erase, double vstr_erase, double vss_erase, double erase_delay, double erase_high, double erase_width, double erase_rise, double erase_fall, double holdtime, int debug_print);


/* USRLIB MODULE INFORMATION

	MODULE NAME: TriLoop
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 27
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		grcl,	int,	Input,	,	,	
		gse,	int,	Input,	,	,	
		gstr,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		loopcount,	int,	Input,	,	,	
		vds_prog,	double,	Input,	,	,	
		vrcl_prog,	double,	Input,	,	,	
		vstr_prog,	double,	Input,	,	,	
		vss_prog,	double,	Input,	,	,	
		prog_delay,	double,	Input,	,	,	
		prog_high,	double,	Input,	,	,	
		prog_width,	double,	Input,	,	,	
		prog_rise,	double,	Input,	,	,	
		prog_fall,	double,	Input,	,	,	
		vds_erase,	double,	Input,	,	,	
		vrcl_erase,	double,	Input,	,	,	
		vstr_erase,	double,	Input,	,	,	
		vss_erase,	double,	Input,	,	,	
		erase_delay,	double,	Input,	,	,	
		erase_high,	double,	Input,	,	,	
		erase_width,	double,	Input,	,	,	
		erase_rise,	double,	Input,	,	,	
		erase_fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
		debug_print,	int,	Input,	,	0,	1
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
void TriLoop(int drain, int grcl, int gse, int gstr, int source, int well, int loopcount, double vds_prog, double vrcl_prog, double vstr_prog, double vss_prog, double prog_delay, double prog_high, double prog_width, double prog_rise, double prog_fall, double vds_erase, double vrcl_erase, double vstr_erase, double vss_erase, double erase_delay, double erase_high, double erase_width, double erase_rise, double erase_fall, double holdtime, int debug_print);


/* USRLIB MODULE INFORMATION

	MODULE NAME: TriProg
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 16
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		grcl,	int,	Input,	,	,	
		gse,	int,	Input,	,	,	
		gstr,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vrcl,	double,	Input,	,	,	
		vstr,	double,	Input,	,	,	
		vss,	double,	Input,	,	,	
		vse_delay,	double,	Input,	,	,	
		vse_high,	double,	Input,	,	,	
		vse_width,	double,	Input,	,	,	
		vse_rise,	double,	Input,	,	,	
		vse_fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
void TriProg(int drain, int grcl, int gse, int gstr, int source, int well, double vds, double vrcl, double vstr, double vss, double vse_delay, double vse_high, double vse_width, double vse_rise, double vse_fall, double holdtime);


/* USRLIB MODULE INFORMATION

	MODULE NAME: TriProg_071211
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 16
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		grcl,	int,	Input,	,	,	
		gse,	int,	Input,	,	,	
		gstr,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vrcl,	double,	Input,	,	,	
		vstr,	double,	Input,	,	,	
		vss,	double,	Input,	,	,	
		vse_delay,	double,	Input,	,	,	
		vse_high,	double,	Input,	,	,	
		vse_width,	double,	Input,	,	,	
		vse_rise,	double,	Input,	,	,	
		vse_fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
void TriProg_071211(int drain, int grcl, int gse, int gstr, int source, int well, double vds, double vrcl, double vstr, double vss, double vse_delay, double vse_high, double vse_width, double vse_rise, double vse_fall, double holdtime);


/* USRLIB MODULE INFORMATION

	MODULE NAME: TriProg_1207
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 16
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		grcl,	int,	Input,	,	,	
		gse,	int,	Input,	,	,	
		gstr,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vrcl,	double,	Input,	,	,	
		vstr,	double,	Input,	,	,	
		vss,	double,	Input,	,	,	
		vse_delay,	double,	Input,	,	,	
		vse_high,	double,	Input,	,	,	
		vse_width,	double,	Input,	,	,	
		vse_rise,	double,	Input,	,	,	
		vse_fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
void TriProg_1207(int drain, int grcl, int gse, int gstr, int source, int well, double vds, double vrcl, double vstr, double vss, double vse_delay, double vse_high, double vse_width, double vse_rise, double vse_fall, double holdtime);


/* USRLIB MODULE INFORMATION

	MODULE NAME: TriProg_org
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 16
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		grcl,	int,	Input,	,	,	
		gse,	int,	Input,	,	,	
		gstr,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vrcl,	double,	Input,	,	,	
		vstr,	double,	Input,	,	,	
		vss,	double,	Input,	,	,	
		vse_delay,	double,	Input,	,	,	
		vse_high,	double,	Input,	,	,	
		vse_width,	double,	Input,	,	,	
		vse_rise,	double,	Input,	,	,	
		vse_fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
void TriProg_org(int drain, int grcl, int gse, int gstr, int source, int well, double vds, double vrcl, double vstr, double vss, double vse_delay, double vse_high, double vse_width, double vse_rise, double vse_fall, double holdtime);


/* USRLIB MODULE INFORMATION

	MODULE NAME: TriProg_smu
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 16
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		grcl,	int,	Input,	,	,	
		gse,	int,	Input,	,	,	
		gstr,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vrcl,	double,	Input,	,	,	
		vstr,	double,	Input,	,	,	
		vss,	double,	Input,	,	,	
		vse_delay,	double,	Input,	,	,	
		vse_high,	double,	Input,	,	,	
		vse_width,	double,	Input,	,	,	
		vse_rise,	double,	Input,	,	,	
		vse_fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
void TriProg_smu(int drain, int grcl, int gse, int gstr, int source, int well, double vds, double vrcl, double vstr, double vss, double vse_delay, double vse_high, double vse_width, double vse_rise, double vse_fall, double holdtime);


/* USRLIB MODULE INFORMATION

	MODULE NAME: TriProg_time
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 16
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		grcl,	int,	Input,	,	,	
		gse,	int,	Input,	,	,	
		gstr,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vrcl,	double,	Input,	,	,	
		vstr,	double,	Input,	,	,	
		vss,	double,	Input,	,	,	
		vse_delay,	double,	Input,	,	,	
		vse_high,	double,	Input,	,	,	
		vse_width,	double,	Input,	,	,	
		vse_rise,	double,	Input,	,	,	
		vse_fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
void TriProg_time(int drain, int grcl, int gse, int gstr, int source, int well, double vds, double vrcl, double vstr, double vss, double vse_delay, double vse_high, double vse_width, double vse_rise, double vse_fall, double holdtime);


/* USRLIB MODULE INFORMATION

	MODULE NAME: TriProgON
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 16
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		grcl,	int,	Input,	,	,	
		gse,	int,	Input,	,	,	
		gstr,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vrcl,	double,	Input,	,	,	
		vstr,	double,	Input,	,	,	
		vss,	double,	Input,	,	,	
		vse_delay,	double,	Input,	,	,	
		vse_high,	double,	Input,	,	,	
		vse_width,	double,	Input,	,	,	
		vse_rise,	double,	Input,	,	,	
		vse_fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
void TriProgON(int drain, int grcl, int gse, int gstr, int source, int well, double vds, double vrcl, double vstr, double vss, double vse_delay, double vse_high, double vse_width, double vse_rise, double vse_fall, double holdtime);


/* USRLIB MODULE INFORMATION

	MODULE NAME: TriPulse_loop
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 27
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		grcl,	int,	Input,	,	,	
		gse,	int,	Input,	,	,	
		gstr,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		loopcount,	int,	Input,	,	,	
		vds_prog,	double,	Input,	,	,	
		vrcl_prog,	double,	Input,	,	,	
		vstr_prog,	double,	Input,	,	,	
		vss_prog,	double,	Input,	,	,	
		prog_delay,	double,	Input,	,	,	
		prog_high,	double,	Input,	,	,	
		prog_width,	double,	Input,	,	,	
		prog_rise,	double,	Input,	,	,	
		prog_fall,	double,	Input,	,	,	
		vds_erase,	double,	Input,	,	,	
		vrcl_erase,	double,	Input,	,	,	
		vstr_erase,	double,	Input,	,	,	
		vss_erase,	double,	Input,	,	,	
		erase_delay,	double,	Input,	,	,	
		erase_high,	double,	Input,	,	,	
		erase_width,	double,	Input,	,	,	
		erase_rise,	double,	Input,	,	,	
		erase_fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
		debug_print,	int,	Input,	,	0,	1
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
void TriPulse_loop(int drain, int grcl, int gse, int gstr, int source, int well, int loopcount, double vds_prog, double vrcl_prog, double vstr_prog, double vss_prog, double prog_delay, double prog_high, double prog_width, double prog_rise, double prog_fall, double vds_erase, double vrcl_erase, double vstr_erase, double vss_erase, double erase_delay, double erase_high, double erase_width, double erase_rise, double erase_fall, double holdtime, int debug_print);


/* USRLIB MODULE INFORMATION

	MODULE NAME: TriPulse_loop2
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 27
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		grcl,	int,	Input,	,	,	
		gse,	int,	Input,	,	,	
		gstr,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		loopcount,	int,	Input,	,	,	
		vds_prog,	double,	Input,	,	,	
		vrcl_prog,	double,	Input,	,	,	
		vstr_prog,	double,	Input,	,	,	
		vss_prog,	double,	Input,	,	,	
		prog_delay,	double,	Input,	,	,	
		prog_high,	double,	Input,	,	,	
		prog_width,	double,	Input,	,	,	
		prog_rise,	double,	Input,	,	,	
		prog_fall,	double,	Input,	,	,	
		vds_erase,	double,	Input,	,	,	
		vrcl_erase,	double,	Input,	,	,	
		vstr_erase,	double,	Input,	,	,	
		vss_erase,	double,	Input,	,	,	
		erase_delay,	double,	Input,	,	,	
		erase_high,	double,	Input,	,	,	
		erase_width,	double,	Input,	,	,	
		erase_rise,	double,	Input,	,	,	
		erase_fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
		debug_print,	int,	Input,	,	0,	1
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
void TriPulse_loop2(int drain, int grcl, int gse, int gstr, int source, int well, int loopcount, double vds_prog, double vrcl_prog, double vstr_prog, double vss_prog, double prog_delay, double prog_high, double prog_width, double prog_rise, double prog_fall, double vds_erase, double vrcl_erase, double vstr_erase, double vss_erase, double erase_delay, double erase_high, double erase_width, double erase_rise, double erase_fall, double holdtime, int debug_print);


/* USRLIB MODULE INFORMATION

	MODULE NAME: TriPulse_loop_new
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 27
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		grcl,	int,	Input,	,	,	
		gse,	int,	Input,	,	,	
		gstr,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		loopcount,	int,	Input,	,	,	
		vds_prog,	double,	Input,	,	,	
		vrcl_prog,	double,	Input,	,	,	
		vstr_prog,	double,	Input,	,	,	
		vss_prog,	double,	Input,	,	,	
		prog_delay,	double,	Input,	,	,	
		prog_high,	double,	Input,	,	,	
		prog_width,	double,	Input,	,	,	
		prog_rise,	double,	Input,	,	,	
		prog_fall,	double,	Input,	,	,	
		vds_erase,	double,	Input,	,	,	
		vrcl_erase,	double,	Input,	,	,	
		vstr_erase,	double,	Input,	,	,	
		vss_erase,	double,	Input,	,	,	
		erase_delay,	double,	Input,	,	,	
		erase_high,	double,	Input,	,	,	
		erase_width,	double,	Input,	,	,	
		erase_rise,	double,	Input,	,	,	
		erase_fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
		debug_print,	int,	Input,	,	0,	1
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
void TriPulse_loop_new(int drain, int grcl, int gse, int gstr, int source, int well, int loopcount, double vds_prog, double vrcl_prog, double vstr_prog, double vss_prog, double prog_delay, double prog_high, double prog_width, double prog_rise, double prog_fall, double vds_erase, double vrcl_erase, double vstr_erase, double vss_erase, double erase_delay, double erase_high, double erase_width, double erase_rise, double erase_fall, double holdtime, int debug_print);


/* USRLIB MODULE INFORMATION

	MODULE NAME: TriPulse_loop_org
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 27
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		grcl,	int,	Input,	,	,	
		gse,	int,	Input,	,	,	
		gstr,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		loopcount,	int,	Input,	,	,	
		vds_prog,	double,	Input,	,	,	
		vrcl_prog,	double,	Input,	,	,	
		vstr_prog,	double,	Input,	,	,	
		vss_prog,	double,	Input,	,	,	
		prog_delay,	double,	Input,	,	,	
		prog_high,	double,	Input,	,	,	
		prog_width,	double,	Input,	,	,	
		prog_rise,	double,	Input,	,	,	
		prog_fall,	double,	Input,	,	,	
		vds_erase,	double,	Input,	,	,	
		vrcl_erase,	double,	Input,	,	,	
		vstr_erase,	double,	Input,	,	,	
		vss_erase,	double,	Input,	,	,	
		erase_delay,	double,	Input,	,	,	
		erase_high,	double,	Input,	,	,	
		erase_width,	double,	Input,	,	,	
		erase_rise,	double,	Input,	,	,	
		erase_fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
		debug_print,	int,	Input,	,	0,	1
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
void TriPulse_loop_org(int drain, int grcl, int gse, int gstr, int source, int well, int loopcount, double vds_prog, double vrcl_prog, double vstr_prog, double vss_prog, double prog_delay, double prog_high, double prog_width, double prog_rise, double prog_fall, double vds_erase, double vrcl_erase, double vstr_erase, double vss_erase, double erase_delay, double erase_high, double erase_width, double erase_rise, double erase_fall, double holdtime, int debug_print);


/* USRLIB MODULE INFORMATION

	MODULE NAME: TriPulse_loop_smu
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 27
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		grcl,	int,	Input,	,	,	
		gse,	int,	Input,	,	,	
		gstr,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		loopcount,	int,	Input,	,	,	
		vds_prog,	double,	Input,	,	,	
		vrcl_prog,	double,	Input,	,	,	
		vstr_prog,	double,	Input,	,	,	
		vss_prog,	double,	Input,	,	,	
		prog_delay,	double,	Input,	,	,	
		prog_high,	double,	Input,	,	,	
		prog_width,	double,	Input,	,	,	
		prog_rise,	double,	Input,	,	,	
		prog_fall,	double,	Input,	,	,	
		vds_erase,	double,	Input,	,	,	
		vrcl_erase,	double,	Input,	,	,	
		vstr_erase,	double,	Input,	,	,	
		vss_erase,	double,	Input,	,	,	
		erase_delay,	double,	Input,	,	,	
		erase_high,	double,	Input,	,	,	
		erase_width,	double,	Input,	,	,	
		erase_rise,	double,	Input,	,	,	
		erase_fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
		debug_print,	int,	Input,	,	0,	1
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
void TriPulse_loop_smu(int drain, int grcl, int gse, int gstr, int source, int well, int loopcount, double vds_prog, double vrcl_prog, double vstr_prog, double vss_prog, double prog_delay, double prog_high, double prog_width, double prog_rise, double prog_fall, double vds_erase, double vrcl_erase, double vstr_erase, double vss_erase, double erase_delay, double erase_high, double erase_width, double erase_rise, double erase_fall, double holdtime, int debug_print);


/* USRLIB MODULE INFORMATION

	MODULE NAME: TriPulse_loop_time
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 27
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		grcl,	int,	Input,	,	,	
		gse,	int,	Input,	,	,	
		gstr,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		loopcount,	int,	Input,	,	,	
		vds_prog,	double,	Input,	,	,	
		vrcl_prog,	double,	Input,	,	,	
		vstr_prog,	double,	Input,	,	,	
		vss_prog,	double,	Input,	,	,	
		prog_delay,	double,	Input,	,	,	
		prog_high,	double,	Input,	,	,	
		prog_width,	double,	Input,	,	,	
		prog_rise,	double,	Input,	,	,	
		prog_fall,	double,	Input,	,	,	
		vds_erase,	double,	Input,	,	,	
		vrcl_erase,	double,	Input,	,	,	
		vstr_erase,	double,	Input,	,	,	
		vss_erase,	double,	Input,	,	,	
		erase_delay,	double,	Input,	,	,	
		erase_high,	double,	Input,	,	,	
		erase_width,	double,	Input,	,	,	
		erase_rise,	double,	Input,	,	,	
		erase_fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
		debug_print,	int,	Input,	1,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
void TriPulse_loop_time(int drain, int grcl, int gse, int gstr, int source, int well, int loopcount, double vds_prog, double vrcl_prog, double vstr_prog, double vss_prog, double prog_delay, double prog_high, double prog_width, double prog_rise, double prog_fall, double vds_erase, double vrcl_erase, double vstr_erase, double vss_erase, double erase_delay, double erase_high, double erase_width, double erase_rise, double erase_fall, double holdtime, int debug_print);


/* USRLIB MODULE INFORMATION

	MODULE NAME: TriPulse_loop_time2
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 35
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		grcl,	int,	Input,	,	,	
		gse,	int,	Input,	,	,	
		gstr,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		vds_prog,	double,	Input,	,	,	
		vrcl_prog,	double,	Input,	,	,	
		vstr_prog,	double,	Input,	,	,	
		vss_prog,	double,	Input,	,	,	
		prog_delay,	double,	Input,	,	,	
		prog_high,	double,	Input,	,	,	
		prog_width,	double,	Input,	,	,	
		prog_rise,	double,	Input,	,	,	
		prog_fall,	double,	Input,	,	,	
		vds_erase,	double,	Input,	,	,	
		vrcl_erase,	double,	Input,	,	,	
		vstr_erase,	double,	Input,	,	,	
		vss_erase,	double,	Input,	,	,	
		erase_delay,	double,	Input,	,	,	
		erase_high,	double,	Input,	,	,	
		erase_width,	double,	Input,	,	,	
		erase_rise,	double,	Input,	,	,	
		erase_fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
		itarget,	double,	Input,	,	,	
		vds_vt,	double,	Input,	,	,	
		vrcl_vt,	double,	Input,	,	,	
		vstr_vt,	double,	Input,	,	,	
		vse_start,	double,	Input,	,	,	
		vse_stop,	double,	Input,	,	,	
		vse_step,	double,	Input,	,	,	
		debug_print,	int,	Input,	,	0,	1
		vt_loop,	D_ARRAY_T,	Output,	,	,	
		loopcnt,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
void TriPulse_loop_time2(int drain, int grcl, int gse, int gstr, int source, int well, double vds_prog, double vrcl_prog, double vstr_prog, double vss_prog, double prog_delay, double prog_high, double prog_width, double prog_rise, double prog_fall, double vds_erase, double vrcl_erase, double vstr_erase, double vss_erase, double erase_delay, double erase_high, double erase_width, double erase_rise, double erase_fall, double holdtime, double itarget, double vds_vt, double vrcl_vt, double vstr_vt, double vse_start, double vse_stop, double vse_step, int debug_print, double *vt_loop, int loopcnt);


/* USRLIB MODULE INFORMATION

	MODULE NAME: TriPulse_loop_time3
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 35
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		grcl,	int,	Input,	,	,	
		gse,	int,	Input,	,	,	
		gstr,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		vds_prog,	double,	Input,	,	,	
		vrcl_prog,	double,	Input,	,	,	
		vstr_prog,	double,	Input,	,	,	
		vss_prog,	double,	Input,	,	,	
		prog_delay,	double,	Input,	,	,	
		prog_high,	double,	Input,	,	,	
		prog_width,	double,	Input,	,	,	
		prog_rise,	double,	Input,	,	,	
		prog_fall,	double,	Input,	,	,	
		vds_erase,	double,	Input,	,	,	
		vrcl_erase,	double,	Input,	,	,	
		vstr_erase,	double,	Input,	,	,	
		vss_erase,	double,	Input,	,	,	
		erase_delay,	double,	Input,	,	,	
		erase_high,	double,	Input,	,	,	
		erase_width,	double,	Input,	,	,	
		erase_rise,	double,	Input,	,	,	
		erase_fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
		itarget,	double,	Input,	,	,	
		vds_vt,	double,	Input,	,	,	
		vrcl_vt,	double,	Input,	,	,	
		vstr_vt,	double,	Input,	,	,	
		vse_start,	double,	Input,	,	,	
		vse_stop,	double,	Input,	,	,	
		vse_step,	double,	Input,	,	,	
		debug_print,	int,	Input,	,	0,	1
		vt_loop,	D_ARRAY_T,	Output,	 ,	 ,	 
		loopcount,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
void TriPulse_loop_time3(int drain, int grcl, int gse, int gstr, int source, int well, double vds_prog, double vrcl_prog, double vstr_prog, double vss_prog, double prog_delay, double prog_high, double prog_width, double prog_rise, double prog_fall, double vds_erase, double vrcl_erase, double vstr_erase, double vss_erase, double erase_delay, double erase_high, double erase_width, double erase_rise, double erase_fall, double holdtime, double itarget, double vds_vt, double vrcl_vt, double vstr_vt, double vse_start, double vse_stop, double vse_step, int debug_print, double *vt_loop, int loopcount);


/* USRLIB MODULE INFORMATION

	MODULE NAME: TriPulsweep
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 34
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		grcl,	int,	Input,	,	,	
		gse,	int,	Input,	,	,	
		gstr,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		loopcount,	int,	Input,	,	,	
		vds_prog,	double,	Input,	,	,	
		vrcl_prog,	double,	Input,	,	,	
		vstr_prog,	double,	Input,	,	,	
		vss_prog,	double,	Input,	,	,	
		prog_delay,	double,	Input,	,	,	
		prog_high,	double,	Input,	,	,	
		prog_width,	double,	Input,	,	,	
		prog_rise,	double,	Input,	,	,	
		prog_fall,	double,	Input,	,	,	
		vds_erase,	double,	Input,	,	,	
		vrcl_erase,	double,	Input,	,	,	
		vstr_erase,	double,	Input,	,	,	
		vss_erase,	double,	Input,	,	,	
		erase_delay,	double,	Input,	,	,	
		erase_high,	double,	Input,	,	,	
		erase_width,	double,	Input,	,	,	
		erase_rise,	double,	Input,	,	,	
		erase_fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
		debug_print,	int,	Input,	,	0,	1
		idtarget,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vrcl,	double,	Input,	,	,	
		vstr,	double,	Input,	,	,	
		vse_start,	double,	Input,	,	,	
		vse_stop,	double,	Input,	,	,	
		vse_step,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
void TriPulsweep(int drain, int grcl, int gse, int gstr, int source, int well, int loopcount, double vds_prog, double vrcl_prog, double vstr_prog, double vss_prog, double prog_delay, double prog_high, double prog_width, double prog_rise, double prog_fall, double vds_erase, double vrcl_erase, double vstr_erase, double vss_erase, double erase_delay, double erase_high, double erase_width, double erase_rise, double erase_fall, double holdtime, int debug_print, double idtarget, double vds, double vrcl, double vstr, double vse_start, double vse_stop, double vse_step);


/* USRLIB MODULE INFORMATION

	MODULE NAME: TriVth
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 15
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		grcl,	int,	Input,	,	,	
		gse,	int,	Input,	,	,	
		gstr,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		idtarget,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vrcl,	double,	Input,	,	,	
		vstr,	double,	Input,	,	,	
		vse_start,	double,	Input,	,	,	
		vse_stop,	double,	Input,	,	,	
		vse_step,	double,	Input,	,	,	
		vth,	double *,	Output,	,	,	
		debug_print,	int,	Input,	,	0,	1
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void TriVth(int drain, int grcl, int gse, int gstr, int source, int well, double idtarget, double vds, double vrcl, double vstr, double vse_start, double vse_stop, double vse_step, double * vth, int debug_print);


/* USRLIB MODULE INFORMATION

	MODULE NAME: TriVth_celli
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 16
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		grcl,	int,	Input,	,	,	
		gse,	int,	Input,	,	,	
		gstr,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		idtarget,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vrcl,	double,	Input,	,	,	
		vstr,	double,	Input,	,	,	
		vse_start,	double,	Input,	,	,	
		vse_stop,	double,	Input,	,	,	
		vse_step,	double,	Input,	,	,	
		vth,	double *,	Output,	,	,	
		cell_i,	double *,	Output,	,	,	
		debug_print,	int,	Input,	,	0,	1
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void TriVth_celli(int drain, int grcl, int gse, int gstr, int source, int well, double idtarget, double vds, double vrcl, double vstr, double vse_start, double vse_stop, double vse_step, double * vth, double * cell_i, int debug_print);


/* USRLIB MODULE INFORMATION

	MODULE NAME: TriVth_celli_llsq
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 16
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		grcl,	int,	Input,	,	,	
		gse,	int,	Input,	,	,	
		gstr,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		idtarget,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vrcl,	double,	Input,	,	,	
		vstr,	double,	Input,	,	,	
		vse_start,	double,	Input,	,	,	
		vse_stop,	double,	Input,	,	,	
		vse_step,	double,	Input,	,	,	
		vth,	double *,	Output,	,	,	
		cell_i,	double *,	Output,	,	,	
		debug_print,	int,	Input,	,	0,	1
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void TriVth_celli_llsq(int drain, int grcl, int gse, int gstr, int source, int well, double idtarget, double vds, double vrcl, double vstr, double vse_start, double vse_stop, double vse_step, double * vth, double * cell_i, int debug_print);


/* USRLIB MODULE INFORMATION

	MODULE NAME: TriVth_celli_r
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 16
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		grcl,	int,	Input,	,	,	
		gse,	int,	Input,	,	,	
		gstr,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		idtarget,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vrcl,	double,	Input,	,	,	
		vstr,	double,	Input,	,	,	
		vse_start,	double,	Input,	,	,	
		vse_stop,	double,	Input,	,	,	
		vse_step,	double,	Input,	,	,	
		vth,	double *,	Output,	,	,	
		cell_i,	double *,	Output,	,	,	
		debug_print,	int,	Input,	,	0,	1
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void TriVth_celli_r(int drain, int grcl, int gse, int gstr, int source, int well, double idtarget, double vds, double vrcl, double vstr, double vse_start, double vse_stop, double vse_step, double * vth, double * cell_i, int debug_print);


/* USRLIB MODULE INFORMATION

	MODULE NAME: TriVth_celli_sweep
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 14
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		grcl,	int,	Input,	,	,	
		gse,	int,	Input,	,	,	
		gstr,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		idtarget,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vrcl,	double,	Input,	,	,	
		vstr,	double,	Input,	,	,	
		vse_start,	double,	Input,	,	,	
		vse_stop,	double,	Input,	,	,	
		vse_step,	double,	Input,	,	,	
		debug_print,	int,	Input,	,	0,	1
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void TriVth_celli_sweep(int drain, int grcl, int gse, int gstr, int source, int well, double idtarget, double vds, double vrcl, double vstr, double vse_start, double vse_stop, double vse_step, int debug_print);


/* USRLIB MODULE INFORMATION

	MODULE NAME: TriVth_celli_swp
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 14
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		grcl,	int,	Input,	,	,	
		gse,	int,	Input,	,	,	
		gstr,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		idtarget,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vrcl,	double,	Input,	,	,	
		vstr,	double,	Input,	,	,	
		vse_start,	double,	Input,	,	,	
		vse_stop,	double,	Input,	,	,	
		vse_step,	double,	Input,	,	,	
		debug_print,	int,	Input,	,	0,	1
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void TriVth_celli_swp(int drain, int grcl, int gse, int gstr, int source, int well, double idtarget, double vds, double vrcl, double vstr, double vse_start, double vse_stop, double vse_step, int debug_print);


/* USRLIB MODULE INFORMATION

	MODULE NAME: vtati_loop_flash
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
#define MAX(a,b)	((a)>(b) ? (a) : (b))

static char const vcid[] ="$Id: Local $";





double leak4_local( int hi, int  lo1,int  lo2,int  subst, double v )
{

    extern int devint();
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
double vtati_loop_flash(int CG, int DRAIN, int SOURCE, int TG, long Loop, double pgm_volt, double pgm_time, double erase_volt, double erase_time, char * mod, char * dut);


