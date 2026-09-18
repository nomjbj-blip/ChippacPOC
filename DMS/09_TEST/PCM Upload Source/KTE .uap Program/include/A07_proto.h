/* A07 function prototype and KITT header file */

/* COPY OF USRLIB SETTINGS INFORMATION */
/* [VISIBILITY] */
/* HIDDEN=0 */
/* [DEPENDENCIES] */
/* LIBS=-lA12 -lPARLIB */
/* OBJECTS= */
/* HEADERS= */
/* [NON-KULT SEARCH PATH] */
/* PATHS= */

/* USRLIB MODULE INFORMATION

	MODULE NAME: A07_koma2
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 8
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		type,	char,	Input,	,	,	
		vbb,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vg,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double  A07_koma2(int, int, int, int, char, double, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: A07_VTL
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 12
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		type,	char,	Input,	,	,	
		vbs,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vgsmin,	double,	Input,	,	,	
		vgsmax,	double,	Input,	,	,	
		vgstp,	double,	Input,	,	,	
		K,	double *,	Output,	,	,	
		Vg_gmmax,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <stdlib.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#define MAX(a,b)	((a)>(b) ? (a) : (b))
#define CDELAY  125.E-12
#define ILEAK   20.E-12 
	END USRLIB MODULE INFORMATION
*/
double  A07_VTL(int, int, int, int, char, double, double, double, double, double, double *, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: eprpulse6_a07
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 8
	ARGUMENTS:
		hi1,	int,	Input,	,	,	
		hi2,	int,	Input,	,	,	
		lo1,	int,	Input,	,	,	
		lo2,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		type,	char,	Input,	,	,	
		vin,	double,	Input,	,	,	
		tp,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "COM_usrlib.h"
	END USRLIB MODULE INFORMATION
*/
double  eprpulse6_a07(int, int, int, int, int, char, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: eprpulse6_a071
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 8
	ARGUMENTS:
		hi1,	int,	Input,	,	,	
		hi2,	int,	Input,	,	,	
		lo1,	int,	Input,	,	,	
		lo2,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		type,	char,	Input,	,	,	
		vin,	double,	Input,	,	,	
		tp,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "COM_usrlib.h"
	END USRLIB MODULE INFORMATION
*/
double  eprpulse6_a071(int, int, int, int, int, char, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: eprpulse6_a071_dmd
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 7
	ARGUMENTS:
		hi1,	int,	Input,	,	,	
		hi2,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		type,	char,	Input,	,	,	
		vin,	double,	Input,	,	,	
		tp,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "COM_usrlib.h"
	END USRLIB MODULE INFORMATION
*/
double  eprpulse6_a071_dmd(int, int, int, int, char, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: gdsvt6_a07
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 16
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		type,	char,	Input,	,	,	
		vbs,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vgsmin,	double,	Input,	,	,	
		vgsmax,	double,	Input,	,	,	
		vgstp,	double,	Input,	,	,	
		Vtl,	double *,	Output,	,	,	
		K,	double *,	Output,	,	,	
		Vg_gmmax,	double *,	Output,	,	,	
		vd1,	double,	Input,	,	,	
		vd2,	double,	Input,	,	,	
		width,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <stdlib.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#define MAX(a,b)	((a)>(b) ? (a) : (b))
#define CDELAY  125.E-12
#define ILEAK   20.E-12 
	END USRLIB MODULE INFORMATION
*/
double  gdsvt6_a07(int, int, int, int, char, double, double, double, double, double, double *, double *, double *, double, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: hbvdsv6_a07
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 9
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		type,	char,	Input,	,	,	
		vdsstop,	double,	Input,	,	,	
		vbb,	double,	Input,	,	,	
		ipgm,	double,	Input,	,	,	
		udelay,	double,	Input,	,	,	
	INCLUDES:
#include <math.h>		                
#include "lptdef.h"
#include <stdlib.h>
#include "LBC5_proto.h"
	
	END USRLIB MODULE INFORMATION
*/
double  hbvdsv6_a07(int, int, int, int, char, double, double, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: hvtlgm
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 12
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		type,	char,	Input,	,	,	
		vbs,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vgsmin,	double,	Input,	,	,	
		vgsmax,	double,	Input,	,	,	
		vgstp,	double,	Input,	,	,	
		K,	double *,	Output,	,	,	
		Vg_gmmax,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <stdlib.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#define MAX(a,b)	((a)>(b) ? (a) : (b))
	END USRLIB MODULE INFORMATION
*/
double  hvtlgm(int, int, int, int, char, double, double, double, double, double, double *, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: koma2
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 8
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		type,	char,	Input,	,	,	
		vbb,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vg,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double  koma2(int, int, int, int, char, double, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: koma2_a07
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 8
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		type,	char,	Input,	,	,	
		vbb,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vg,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double  koma2_a07(int, int, int, int, char, double, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: koma2_a071
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 9
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		type,	char,	Input,	,	,	
		vbb,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vg,	double,	Input,	,	,	
		dtime,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double  koma2_a071(int, int, int, int, char, double, double, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: koma2_a072
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 24
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		type,	char,	Input,	,	,	
		vbb,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vg,	double,	Input,	,	,	
		d1,	int,	Input,	,	,	
		d2,	int,	Input,	,	,	
		d3,	int,	Input,	,	,	
		d4,	int,	Input,	,	,	
		d5,	int,	Input,	,	,	
		d6,	int,	Input,	,	,	
		d7,	int,	Input,	,	,	
		d8,	int,	Input,	,	,	
		d9,	int,	Input,	,	,	
		d10,	int,	Input,	,	,	
		d11,	int,	Input,	,	,	
		d12,	int,	Input,	,	,	
		d13,	int,	Input,	,	,	
		d14,	int,	Input,	,	,	
		d15,	int,	Input,	,	,	
		d16,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double  koma2_a072(int, int, int, int, char, double, double, double, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int);

/* USRLIB MODULE INFORMATION

	MODULE NAME: koma2_a07s
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 8
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		type,	char,	Input,	,	,	
		vbb,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vg,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double  koma2_a07s(int, int, int, int, char, double, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: koma2_a07s1
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 8
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		type,	char,	Input,	,	,	
		vbb,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vg,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double  koma2_a07s1(int, int, int, int, char, double, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: koma2_kj
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 8
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		type,	char,	Input,	,	,	
		vbb,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vg,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double  koma2_kj(int, int, int, int, char, double, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: koma2_kj1
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 20
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		type,	char,	Input,	,	,	
		vbb,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vg,	double,	Input,	,	,	
		d1,	int,	Input,	,	,	
		d2,	int,	Input,	,	,	
		d3,	int,	Input,	,	,	
		d4,	int,	Input,	,	,	
		d5,	int,	Input,	,	,	
		d6,	int,	Input,	,	,	
		d7,	int,	Input,	,	,	
		d8,	int,	Input,	,	,	
		d9,	int,	Input,	,	,	
		d10,	int,	Input,	,	,	
		d11,	int,	Input,	,	,	
		d12,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double  koma2_kj1(int, int, int, int, char, double, double, double, int, int, int, int, int, int, int, int, int, int, int, int);

/* USRLIB MODULE INFORMATION

	MODULE NAME: koma2_kjl
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 24
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		type,	char,	Input,	,	,	
		vbb,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vg,	double,	Input,	,	,	
		d1,	int,	Input,	,	,	
		d2,	int,	Input,	,	,	
		d3,	int,	Input,	,	,	
		d4,	int,	Input,	,	,	
		d5,	int,	Input,	,	,	
		d6,	int,	Input,	,	,	
		d7,	int,	Input,	,	,	
		d8,	int,	Input,	,	,	
		d9,	int,	Input,	,	,	
		d10,	int,	Input,	,	,	
		d11,	int,	Input,	,	,	
		d12,	int,	Input,	,	,	
		d13,	int,	Input,	,	,	
		d14,	int,	Input,	,	,	
		d15,	int,	Input,	,	,	
		d16,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double  koma2_kjl(int, int, int, int, char, double, double, double, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int);

/* USRLIB MODULE INFORMATION

	MODULE NAME: leakq6
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 10
	ARGUMENTS:
		hi1,	int,	Input,	,	,	
		hi2,	int,	Input,	,	,	
		lo1,	int,	Input,	,	,	
		lo2,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		type,	char,	Input,	,	,	
		v,	double,	Input,	,	,	
		ilim,	double,	Input,	,	,	
		wtime,	double,	Input,	,	,	
		width,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double  leakq6(int, int, int, int, int, char, double, double, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: leakq6_a07
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 10
	ARGUMENTS:
		hi1,	int,	Input,	,	,	
		hi2,	int,	Input,	,	,	
		lo1,	int,	Input,	,	,	
		lo2,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		type,	char,	Input,	,	,	
		v,	double,	Input,	,	,	
		ilim,	double,	Input,	,	,	
		wtime,	double,	Input,	,	,	
		width,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double  leakq6_a07(int, int, int, int, int, char, double, double, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: subq6_a07
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 10
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		type,	char,	Input,	,	,	
		vbb,	double,	Input,	,	,	
		ipgm,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vgsstart,	double,	Input,	,	,	
		vgsstop,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double  subq6_a07(int, int, int, int, char, double, double, double, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: uisubq6_a07
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 10
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		type,	char,	Input,	,	,	
		vbb,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vlow,	double,	Input,	,	,	
		vhigh,	double,	Input,	,	,	
		width,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double  uisubq6_a07(int, int, int, int, char, double, double, double, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: ukoma2_a07
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 10
	ARGUMENTS:
		type,	char,	Input,	,	,	
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		vbb,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vg,	double,	Input,	,	,	
		ri,	double,	Input,	,	,	
		dtime,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double  ukoma2_a07(char, int, int, int, int, double, double, double, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: ukoma2_a07s
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 10
	ARGUMENTS:
		type,	char,	Input,	,	,	
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		vbb,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vg,	double,	Input,	,	,	
		ri,	double,	Input,	,	,	
		dtime,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double  ukoma2_a07s(char, int, int, int, int, double, double, double, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: vt_lin_a07_new
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 23
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		gate,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		bulk,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		width,	double,	Input,	,	,	
		length,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
		vmin,	double,	Input,	,	,	
		vmax,	double,	Input,	,	,	
		npoints,	int,	Input,	,	,	
		idmax,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vbs,	double,	Input,	,	,	
		vgstp,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		Vtl,	double *,	Output,	,	,	
		K,	double *,	Output,	,	,	
		Gm,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <stdlib.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#define MAX(a,b)	((a)>(b) ? (a) : (b))
#define CDELAY  125.E-12
#define ILEAK   20.E-12 
	END USRLIB MODULE INFORMATION
*/
void vt_lin_a07_new(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vmin, double vmax, int npoints, double idmax, double vds, double vbs, double vgstp, double intrange, double mrange, double lorange, double * Vtl, double * K, double * Gm);


/* USRLIB MODULE INFORMATION

	MODULE NAME: vtfhq6
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 10
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		type,	char,	Input,	,	,	
		vbb,	double,	Input,	,	,	
		ipgm,	double,	Input,	,	,	
		vdd,	double,	Input,	,	,	
		smin,	double,	Input,	,	,	
		smax,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double  vtfhq6(int, int, int, int, char, double, double, double, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: vtfhq6_a07
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 10
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		type,	char,	Input,	,	,	
		vbb,	double,	Input,	,	,	
		ipgm,	double,	Input,	,	,	
		vdd,	double,	Input,	,	,	
		smin,	double,	Input,	,	,	
		smax,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double  vtfhq6_a07(int, int, int, int, char, double, double, double, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: vtfhq6_a07_dmd
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 9
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		type,	char,	Input,	,	,	
		vbb,	double,	Input,	,	,	
		ipgm,	double,	Input,	,	,	
		vdd,	double,	Input,	,	,	
		smin,	double,	Input,	,	,	
		smax,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double  vtfhq6_a07_dmd(int, int, int, char, double, double, double, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: vtfhq6_a07_exS
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 10
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		type,	char,	Input,	,	,	
		vbb,	double,	Input,	,	,	
		ipgm,	double,	Input,	,	,	
		vdd,	double,	Input,	,	,	
		smin,	double,	Input,	,	,	
		smax,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double  vtfhq6_a07_exS(int, int, int, int, char, double, double, double, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: vtfhq6_a07s
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 10
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		type,	char,	Input,	,	,	
		vbb,	double,	Input,	,	,	
		ipgm,	double,	Input,	,	,	
		vdd,	double,	Input,	,	,	
		smin,	double,	Input,	,	,	
		smax,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double  vtfhq6_a07s(int, int, int, int, char, double, double, double, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: vtl_a07
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 12
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		type,	char,	Input,	,	,	
		vbs,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vgsmin,	double,	Input,	,	,	
		vgsmax,	double,	Input,	,	,	
		vgstp,	double,	Input,	,	,	
		K,	double *,	Output,	,	,	
		Vg_gmmax,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <stdlib.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#define MAX(a,b)	((a)>(b) ? (a) : (b))
#define CDELAY  125.E-12
#define ILEAK   20.E-12 
	END USRLIB MODULE INFORMATION
*/
double  vtl_a07(int, int, int, int, char, double, double, double, double, double, double *, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: vtl_a07_0407
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 14
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		type,	char,	Input,	,	,	
		vbs,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vgsmin,	double,	Input,	,	,	
		vgsmax,	double,	Input,	,	,	
		vgstp,	double,	Input,	,	,	
		Vtl,	double *,	Output,	,	,	
		K,	double *,	Output,	,	,	
		Vg_gmmax,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <stdlib.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#define MAX(a,b)	((a)>(b) ? (a) : (b))
#define CDELAY  125.E-12
#define ILEAK   20.E-12 
	END USRLIB MODULE INFORMATION
*/
double  vtl_a07_0407(char *, int, int, int, int, char, double, double, double, double, double, double *, double *, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: vtl_a07_0418
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 12
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		type,	char,	Input,	,	,	
		vbs,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vgsmin,	double,	Input,	,	,	
		vgsmax,	double,	Input,	,	,	
		vgstp,	double,	Input,	,	,	
		K,	double *,	Output,	,	,	
		Vg_gmmax,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <stdlib.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#define MAX(a,b)	((a)>(b) ? (a) : (b))
#define CDELAY  125.E-12
#define ILEAK   20.E-12 
	END USRLIB MODULE INFORMATION
*/
double  vtl_a07_0418(int, int, int, int, char, double, double, double, double, double, double *, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: vtl_a07_ju
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 12
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		type,	char,	Input,	,	,	
		vbs,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vgsmin,	double,	Input,	,	,	
		vgsmax,	double,	Input,	,	,	
		vgstp,	double,	Input,	,	,	
		K,	double *,	Output,	,	,	
		Vg_gmmax,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <stdlib.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#define MAX(a,b)	((a)>(b) ? (a) : (b))
#define CDELAY  125.E-12
#define ILEAK   20.E-12 
	END USRLIB MODULE INFORMATION
*/
double  vtl_a07_ju(int, int, int, int, char, double, double, double, double, double, double *, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: vtl_a07_kj
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 12
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		type,	char,	Input,	,	,	
		vbs,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vgsmin,	double,	Input,	,	,	
		vgsmax,	double,	Input,	,	,	
		vgstp,	double,	Input,	,	,	
		K,	double *,	Output,	,	,	
		Vg_gmmax,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <stdlib.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#define MAX(a,b)	((a)>(b) ? (a) : (b))
#define CDELAY  125.E-12
#define ILEAK   20.E-12 
	END USRLIB MODULE INFORMATION
*/
double  vtl_a07_kj(int, int, int, int, char, double, double, double, double, double, double *, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: vtl_a07_kj2
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 12
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		type,	char,	Input,	,	,	
		vbs,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vgsmin,	double,	Input,	,	,	
		vgsmax,	double,	Input,	,	,	
		vgstp,	double,	Input,	,	,	
		K,	double *,	Output,	,	,	
		Vg_gmmax,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <stdlib.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#define MAX(a,b)	((a)>(b) ? (a) : (b))
#define CDELAY  125.E-12
#define ILEAK   20.E-12 
	END USRLIB MODULE INFORMATION
*/
double  vtl_a07_kj2(int, int, int, int, char, double, double, double, double, double, double *, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: vtl_a07_kj3
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 12
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		type,	char,	Input,	,	,	
		vbs,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vgsmin,	double,	Input,	,	,	
		vgsmax,	double,	Input,	,	,	
		vgstp,	double,	Input,	,	,	
		K,	double *,	Output,	,	,	
		Vg_gmmax,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <stdlib.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#define MAX(a,b)	((a)>(b) ? (a) : (b))
#define CDELAY  125.E-12
#define ILEAK   20.E-12 
	END USRLIB MODULE INFORMATION
*/
double  vtl_a07_kj3(int, int, int, int, char, double, double, double, double, double, double *, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: vtl_a07_kjl
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 12
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		type,	char,	Input,	,	,	
		vbs,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vgsmin,	double,	Input,	,	,	
		vgsmax,	double,	Input,	,	,	
		vgstp,	double,	Input,	,	,	
		K,	double *,	Output,	,	,	
		Vg_gmmax,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <stdlib.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#define MAX(a,b)	((a)>(b) ? (a) : (b))
#define CDELAY  125.E-12
#define ILEAK   20.E-12 
	END USRLIB MODULE INFORMATION
*/
double  vtl_a07_kjl(int, int, int, int, char, double, double, double, double, double, double *, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: vtl_a07_raw
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 12
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		type,	char,	Input,	,	,	
		vbs,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vgsmin,	double,	Input,	,	,	
		vgsmax,	double,	Input,	,	,	
		vgstp,	double,	Input,	,	,	
		K,	double *,	Output,	,	,	
		Vg_gmmax,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <stdlib.h>
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
double  vtl_a07_raw(int, int, int, int, char, double, double, double, double, double, double *, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: vtl_a07_sweep
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 18
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		type,	char,	Input,	,	,	
		vbs,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vgsmin,	double,	Input,	,	,	
		vgsmax,	double,	Input,	,	,	
		vgstp,	double,	Input,	,	,	
		ids,	D_ARRAY_T,	Output,	,	,	
		ids_size,	int,	Input,	,	,	
		vgs,	D_ARRAY_T,	Output,	,	,	
		vgs_size,	int,	Input,	,	,	
		slope,	D_ARRAY_T,	Output,	,	,	
		slope_size,	int,	Input,	,	,	
		K,	double *,	Output,	,	,	
		Vg_gmmax,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <stdlib.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#define MAX(a,b)	((a)>(b) ? (a) : (b))
#define CDELAY  125.E-12
#define ILEAK   20.E-12 
	END USRLIB MODULE INFORMATION
*/
double  vtl_a07_sweep(int, int, int, int, char, double, double, double, double, double, double *, int, double *, int, double *, int, double *, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: vtlq6_a07
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 14
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		type,	char,	Input,	,	,	
		vbs,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		start,	double,	Input,	,	,	
		step,	double,	Input,	,	,	
		vg_shift,	double,	Input,	,	,	
		Vtl,	double *,	Output,	,	,	
		K,	double *,	Output,	,	,	
		Theta,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <stdlib.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void  vtlq6_a07(char *, int, int, int, int, char, double, double, double, double, double, double *, double *, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: vts_a07
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 12
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		type,	char,	Input,	,	,	
		vbs,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vgsmin,	double,	Input,	,	,	
		vgsmax,	double,	Input,	,	,	
		vgstp,	double,	Input,	,	,	
		K,	double *,	Output,	,	,	
		Vg_gmmax,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <stdlib.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#define MAX(a,b)	((a)>(b) ? (a) : (b))
#define CDELAY  125.E-12
#define ILEAK   20.E-12 
	END USRLIB MODULE INFORMATION
*/
double  vts_a07(int, int, int, int, char, double, double, double, double, double, double *, double *);

