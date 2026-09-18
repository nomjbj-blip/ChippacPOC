/* a507A function prototype and KITT header file */

/* COPY OF USRLIB SETTINGS INFORMATION */
/* [VISIBILITY] */
/* HIDDEN=0 */
/* [DEPENDENCIES] */
/* LIBS=-lHP8110u -loptlib */
/* OBJECTS= */
/* HEADERS= */
/* [NON-KULT SEARCH PATH] */
/* PATHS= */

/* USRLIB MODULE INFORMATION

	MODULE NAME: bvdss6_vg
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 12
	ARGUMENTS:
		d,	int,	Input,	-1,	,	
		g,	int,	Input,	-1,	,	
		s,	int,	Input,	-1,	,	
		sub,	int,	Input,	-1,	,	
		vdsstart,	double,	Input,	0,	,	
		vdsstop,	double,	Input,	20,	,	
		nstep,	int,	Input,	100,	,	
		vgs,	double,	Input,	0,	,	
		vsub,	double,	Input,	0,	,	
		ipgm,	double,	Input,	1E-6,	,	
		udelay,	double,	Input,	0.005,	,	
		type,	char,	Input,	'n',	,	
	INCLUDES:
#include <math.h>		                
#include "lptdef.h"
#include <stdlib.h>
#include "LBC5_proto.h"
	
	END USRLIB MODULE INFORMATION
*/
double  bvdss6_vg(int, int, int, int, double, double, int, double, double, double, double, char);

/* USRLIB MODULE INFORMATION

	MODULE NAME: discharge_7a
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
void discharge_7a();


/* USRLIB MODULE INFORMATION

	MODULE NAME: E2prom
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 20
	ARGUMENTS:
		Devname,	char *,	Input,	"ee",	,	
		hi,	int,	Input,	-1,	,	
		lo,	int,	Input,	-1,	,	
		s_hi,	int,	Input,	-1,	,	
		s_lo,	int,	Input,	-1,	,	
		Psmu1,	int,	Input,	-1,	,	
		Psmu2,	int,	Input,	-1,	,	
		g1,	int,	Input,	-1,	,	
		g2,	int,	Input,	-1,	,	
		g3,	int,	Input,	-1,	,	
		hi_delay,	double,	Input,	0,	,	
		hi_height,	double,	Input,	1,	-20,	20
		hi_width,	double,	Input,	1e-3,	,	
		lo_delay,	double,	Input,	0,	,	
		lo_height,	double,	Input,	2,	-20,	20
		lo_width,	double,	Input,	1e-3,	,	
		rise,	double,	Input,	1e-8,	1e-8,	1e-7
		fall,	double,	Input,	1e-8,	1e-8,	1e-7
		smu1_v,	double,	Input,	0,	,	
		smu2_v,	double,	Input,	0,	,	
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
void E2prom(char * Devname, int hi, int lo, int s_hi, int s_lo, int Psmu1, int Psmu2, int g1, int g2, int g3, double hi_delay, double hi_height, double hi_width, double lo_delay, double lo_height, double lo_width, double rise, double fall, double smu1_v, double smu2_v);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ee
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
void ee(char * class, int hi, int lo, int lo1, int lo2, int s1_hi, int s2_hi, int s1_lo, int s2_lo, int PS1, int PS2, int g1, int g2, int g3, double hi_delay, double hi_height, double hi_width, double lo_delay, double lo_height, double lo_width, double lo1_delay, double lo1_height, double lo1_width, double lo2_delay, double lo2_height, double lo2_width, double rise, double fall, double PS1_V, double PS2_V);


/* USRLIB MODULE INFORMATION

	MODULE NAME: getGlobal
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 5
	ARGUMENTS:
		itemName,	char *,	Input,	"no name",	,	
		slotid,	char *,	Input,	"8,16,24",	,	
		sampleClass,	int,	Input,	0,	,	
		dummy1,	int,	Input,	0,	,	
		dummy2,	char *,	Input,	"dummy",	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <ksox_def.h>
#include<stdio.h>
#include<sys/types.h>
#include<dirent.h>
#include<sys/stat.h>
#include<fcntl.h>
#include<string.h>
#include<stdlib.h>
#include <ktxe_proto.h>
	END USRLIB MODULE INFORMATION
*/
int getGlobal(char * itemName, char * slotid, int sampleClass, int dummy1, char * dummy2);


/* USRLIB MODULE INFORMATION

	MODULE NAME: getSelect
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		itemName,	char *,	Input,	"no name",	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <ksox_def.h>
#include<sys/types.h>
#include<dirent.h>
#include<sys/stat.h>
#include<fcntl.h>
#include<string.h>
#include<stdlib.h>
#include <ktxe_proto.h>
#include <ksox_def.h>
	END USRLIB MODULE INFORMATION
*/
int  getSelect(char *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: getSelect2
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		itemName,	char *,	Input,	"no name",	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <ksox_def.h>
#include<sys/types.h>
#include<dirent.h>
#include<sys/stat.h>
#include<fcntl.h>
#include<string.h>
#include<stdlib.h>
#include <ktxe_proto.h>
#include <ksox_def.h>
	END USRLIB MODULE INFORMATION
*/
int getSelect2(char * itemName);


/* USRLIB MODULE INFORMATION

	MODULE NAME: gmmax_vt
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 16
	ARGUMENTS:
		devname,	char *,	Input,	"aa",	,	
		drain,	int,	Input,	-1,	,	
		gate,	int,	Input,	-1,	,	
		source,	int,	Input,	-1,	,	
		subst,	int,	Input,	-1,	,	
		w,	double,	Input,	1,	,	
		l,	double,	Input,	1,	,	
		vlow,	double,	Input,	0,	,	
		vhigh,	double,	Input,	2,	,	
		vds,	double,	Input,	0.1,	,	
		vbs,	double,	Input,	0,	,	
		ithr,	double,	Input,	40e-9,	,	
		vstep,	double,	Input,	0.05,	,	
		npts,	int,	Input,	15,	,	
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
double gmmax_vt(char * devname, int drain, int gate, int source, int subst, double w, double l, double vlow, double vhigh, double vds, double vbs, double ithr, double vstep, int npts, double * slope, int * kflag);


/* USRLIB MODULE INFORMATION

	MODULE NAME: gndall_7a
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <COM_usrlib.h>
	END USRLIB MODULE INFORMATION
*/
void gndall_7a();


/* USRLIB MODULE INFORMATION

	MODULE NAME: i4vO
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 23
	ARGUMENTS:
		devname,	char *,	Input,	"i4vO",	,	
		drain,	int,	Input,	-1,	,	
		gate,	int,	Input,	-1,	,	
		source,	int,	Input,	-1,	,	
		subst,	int,	Input,	-1,	,	
		g1,	int,	Input,	-1,	,	
		g2,	int,	Input,	-1,	,	
		s_drain,	int,	Input,	-1,	,	
		s_gate,	int,	Input,	-1,	,	
		type,	char,	Input,	'n',	,	
		plc,	double,	Input,	1,	,	
		ilimit,	double,	Input,	1e-6,	,	
		irange,	double,	Input,	1e-9,	,	
		delay,	double,	Input,	0.2,	,	
		vgs,	double,	Input,	0,	,	
		vds,	double,	Input,	1.8,	,	
		vsub,	double,	Input,	0,	,	
		sd,	char,	Input,	'd',	,	
		unitc,	int,	Input,	0,	,	
		width,	double,	Input,	0,	,	
		hp,	double,	Input,	0,	,	
		vstep,	double,	Input,	0.3,	,	
		noise,	char,	Input,	'x',	,	
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
#include <COM_usrlib.h>
	END USRLIB MODULE INFORMATION
*/
double  i4vO(char *, int, int, int, int, int, int, int, int, char, double, double, double, double, double, double, double, char, int, double, double, double, char);

/* USRLIB MODULE INFORMATION

	MODULE NAME: id7
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 7
	ARGUMENTS:
		drain,	int,	Input,	-1,	,	
		gate,	int,	Input,	-1,	,	
		source,	int,	Input,	-1,	,	
		subst,	int,	Input,	-1,	,	
		vgs,	double,	Input,	0,	,	
		vds,	double,	Input,	1.8,	,	
		vbs,	double,	Input,	0,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double id7(int drain, int gate, int source, int subst, double vgs, double vds, double vbs);


/* USRLIB MODULE INFORMATION

	MODULE NAME: JEDEC_I2_sweep
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 27
	ARGUMENTS:
		devname,	char *,	Input,	"Jramp",	,	
		hi,	int,	Input,	-1,	,	
		lo,	int,	Input,	-1,	,	
		sub,	int,	Input,	-1,	,	
		chuckcon,	int,	Input,	-1,	,	
		area,	double,	Input,	1,	,	
		per,	double,	Input,	1,	,	
		tty,	char,	Input,	'n',	,	
		bty,	char,	Input,	'p',	,	
		imin,	double,	Input,	1e-6,	,	
		imax,	double,	Input,	0.5,	,	
		istop,	double,	Input,	0.0503,	,	
		imult,	double,	Input,	1.259,	,	
		hold,	double,	Input,	0.2,	,	
		stepdelay,	double,	Input,	0.034,	,	
		vmax,	double,	Input,	35,	,	
		ratio,	double,	Input,	0.15,	,	
		m_i,	double,	Input,	1,	,	
		m_q,	double,	Input,	1,	,	
		intrange,	double,	Input,	1,	,	
		mrange,	double,	Input,	1E-6,	,	
		lorange,	double,	Input,	1E-8,	,	
		vlf,	double *,	Output,	,	,	
		vhf,	double *,	Output,	,	,	
		vfail,	double *,	Output,	,	,	
		ifail,	double *,	Output,	,	,	
		qbd,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#define CRTLIM 1.0E-6		  

#include <stdlib.h>
#include <ktxe_proto.h>
#include <ksox_def.h>
#include <kui_proto.h>

#define MAX(a,b)	((a)>(b) ? (a) : (b))
#define CDELAY  125.E-12
#define ILEAK   20.E-12 
                            
	END USRLIB MODULE INFORMATION
*/
void JEDEC_I2_sweep(char * devname, int hi, int lo, int sub, int chuckcon, double area, double per, char tty, char bty, double imin, double imax, double istop, double imult, double hold, double stepdelay, double vmax, double ratio, double m_i, double m_q, double intrange, double mrange, double lorange, double * vlf, double * vhf, double * vfail, double * ifail, double * qbd);


/* USRLIB MODULE INFORMATION

	MODULE NAME: LEAK_dio_log1
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 19
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		top,	int,	Input,	,	,	
		bot,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		area,	double,	Input,	,	,	
		per,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
		btype,	char,	Input,	,	,	
		vrev,	double,	Input,	,	,	
		ilim,	double,	Input,	,	,	
		icomp,	double,	Input,	,	,	
		irange,	double,	Input,	,	,	
		delaytime,	double,	Input,	,	,	
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
#include "kdf.h"
#include "ktxe_types.h"
#include "ksox_def.h"
#include "COM_usrlib.h"
	END USRLIB MODULE INFORMATION
*/
void  LEAK_dio_log1(char *, int, int, int, int, double, double, char, char, double, double, double, double, double, double, double, double, double, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: LEAK_dio_log2
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 19
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		top,	int,	Input,	,	,	
		bot,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		area,	double,	Input,	,	,	
		per,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
		btype,	char,	Input,	,	,	
		vrev,	double,	Input,	,	,	
		ilim,	double,	Input,	,	,	
		icomp,	double,	Input,	,	,	
		irange,	double,	Input,	,	,	
		delaytime,	double,	Input,	,	,	
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
#include "kdf.h"
#include "ktxe_types.h"
#include "ksox_def.h"
#include "COM_usrlib.h"
	END USRLIB MODULE INFORMATION
*/
void  LEAK_dio_log2(char *, int, int, int, int, double, double, char, char, double, double, double, double, double, double, double, double, double, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: LEAK_dio_log3
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 19
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		top,	int,	Input,	,	,	
		bot,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		area,	double,	Input,	,	,	
		per,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
		btype,	char,	Input,	,	,	
		vrev,	double,	Input,	,	,	
		ilim,	double,	Input,	,	,	
		icomp,	double,	Input,	,	,	
		irange,	double,	Input,	,	,	
		delaytime,	double,	Input,	,	,	
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
#include "kdf.h"
#include "ktxe_types.h"
#include "ksox_def.h"
#include "COM_usrlib.h"
	END USRLIB MODULE INFORMATION
*/
void  LEAK_dio_log3(char *, int, int, int, int, double, double, char, char, double, double, double, double, double, double, double, double, double, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: mem_access
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 4
	ARGUMENTS:
		gdf_name,	char *,	Input,	,	,	
		load,	int,	Input,	,	,	
		load_value,	double,	Input,	,	,	
		download,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <ksox_def.h>
	END USRLIB MODULE INFORMATION
*/
double  mem_access(char *, int, double, int);

/* USRLIB MODULE INFORMATION

	MODULE NAME: open_scrap
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <ktxe_proto.h>
#include <ksox_def.h>
#include <sys/types.h>
#include <dirent.h>
#include <fcntl.h>
#include <string.h>
#include <sys/stat.h>
	END USRLIB MODULE INFORMATION
*/
void  open_scrap();

/* USRLIB MODULE INFORMATION

	MODULE NAME: open_scrap32
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <ktxe_proto.h>
#include <ksox_def.h>
#include <sys/types.h>
#include <dirent.h>
#include <fcntl.h>
#include <string.h>
#include <sys/stat.h>
	END USRLIB MODULE INFORMATION
*/
void  open_scrap32();

/* USRLIB MODULE INFORMATION

	MODULE NAME: RES_2_I_RES_SMU
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 14
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		sq,	double,	Input,	,	,	
		iforce,	double,	Input,	,	,	
		vlimit,	double,	Input,	,	,	
		vsub,	double,	Input,	,	,	
		delaytime,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		multiplier,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		R,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void  RES_2_I_RES_SMU(char *, int, int, int, double, double, double, double, double, double, double, double, double, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: res_7a
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 4
	ARGUMENTS:
		hi,	int,	Input,	-1,	,	
		lo,	int,	Input,	-1,	,	
		subst,	int,	Input,	-1,	,	
		itest,	double,	Input,	1e-3,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <stdlib.h>
#include <par_util.h>
#include <stdlib.h>
#include <ktxe_proto.h>
#include <ksox_def.h>
#include <kui_proto.h>
	END USRLIB MODULE INFORMATION
*/
double res_7a(int hi, int lo, int subst, double itest);


/* USRLIB MODULE INFORMATION

	MODULE NAME: SI
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 5
	ARGUMENTS:
		itemName,	char *,	Input,	"no name",	,	
		slotid,	char *,	Input,	"8,16,24",	,	
		sampleClass,	int,	Input,	0,	,	
		dummy1,	int,	Input,	0,	,	
		dummy2,	char *,	Input,	"dummy",	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <ksox_def.h>
#include<stdio.h>
#include<sys/types.h>
#include<dirent.h>
#include<sys/stat.h>
#include<fcntl.h>
#include<string.h>
#include<stdlib.h>
#include <ktxe_proto.h>
	END USRLIB MODULE INFORMATION
*/
int SI(char * itemName, char * slotid, int sampleClass, int dummy1, char * dummy2);


