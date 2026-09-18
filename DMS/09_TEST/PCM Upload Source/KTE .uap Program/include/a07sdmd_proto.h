/* a07sdmd function prototype and KITT header file */

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

	MODULE NAME: A07_F7N_V_MM
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 13
	ARGUMENTS:
		p1,	int,	Input,	,	,	
		p3,	int,	Input,	,	,	
		p8,	int,	Input,	,	,	
		p10,	int,	Input,	,	,	
		SpecLShort,	double,	Input,	,	,	
		SpecHShort,	double,	Input,	,	,	
		SpecLdegr,	double,	Input,	,	,	
		SpecHdegr,	double,	Input,	,	,	
		SpecLopen,	double,	Input,	,	,	
		SpecHopen,	double,	Input,	,	,	
		Short_Flag,	int *,	Output,	,	,	
		Degr_Flag,	int *,	Output,	,	,	
		Open_Flag,	int *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double  A07_F7N_V_MM(int, int, int, int, double, double, double, double, double, double, int *, int *, int *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: A07_F7N_V_TM
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 13
	ARGUMENTS:
		p2,	int,	Input,	,	,	
		p3,	int,	Input,	,	,	
		p8,	int,	Input,	,	,	
		p10,	int,	Input,	,	,	
		SpecLShort,	double,	Input,	,	,	
		SpecHShort,	double,	Input,	,	,	
		SpecLdegr,	double,	Input,	,	,	
		SpecHdegr,	double,	Input,	,	,	
		SpecLopen,	double,	Input,	,	,	
		SpecHopen,	double,	Input,	,	,	
		Short_Flag,	int *,	Output,	,	,	
		Degr_Flag,	int *,	Output,	,	,	
		Open_Flag,	int *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double  A07_F7N_V_TM(int, int, int, int, double, double, double, double, double, double, int *, int *, int *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: A07_F7P_V_MM
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 13
	ARGUMENTS:
		p1,	int,	Input,	,	,	
		p3,	int,	Input,	,	,	
		p8,	int,	Input,	,	,	
		p10,	int,	Input,	,	,	
		SpecLShort,	double,	Input,	,	,	
		SpecHShort,	double,	Input,	,	,	
		SpecLdegr,	double,	Input,	,	,	
		SpecHdegr,	double,	Input,	,	,	
		SpecLopen,	double,	Input,	,	,	
		SpecHopen,	double,	Input,	,	,	
		Short_Flag,	int *,	Output,	,	,	
		Degr_Flag,	int *,	Output,	,	,	
		Open_Flag,	int *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double  A07_F7P_V_MM(int, int, int, int, double, double, double, double, double, double, int *, int *, int *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: A07_F7P_V_TM
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 13
	ARGUMENTS:
		p2,	int,	Input,	,	,	
		p3,	int,	Input,	,	,	
		p8,	int,	Input,	,	,	
		p10,	int,	Input,	,	,	
		SpecLShort,	double,	Input,	,	,	
		SpecHShort,	double,	Input,	,	,	
		SpecLdegr,	double,	Input,	,	,	
		SpecHdegr,	double,	Input,	,	,	
		SpecLopen,	double,	Input,	,	,	
		SpecHopen,	double,	Input,	,	,	
		Short_Flag,	int *,	Output,	,	,	
		Degr_Flag,	int *,	Output,	,	,	
		Open_Flag,	int *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double  A07_F7P_V_TM(int, int, int, int, double, double, double, double, double, double, int *, int *, int *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: beta2_ksc
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
double  beta2_ksc(int, int, int, int, double, double, double *, double *, char);

/* USRLIB MODULE INFORMATION

	MODULE NAME: BREAKV_ks
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 17
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
		imax,	double,	Input,	,	,	
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
void  BREAKV_ks(char *, int, int, int, int, double, double, char, char, double, double, double, double, double, double, double, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: bv2sweep
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 13
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		vstart,	double,	Input,	,	,	
		vstop,	double,	Input,	,	,	
		vstep1,	int,	Input,	,	,	
		vstep2,	int,	Input,	,	,	
		ipgm1,	double,	Input,	,	,	
		ipgm2,	double,	Input,	,	,	
		udelay,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
		rawdata,	int,	Input,	,	,	
		firstBV,	double *,	Input,	,	,	
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
double  bv2sweep(int, int, int, double, double, int, int, double, double, double, char, int, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: LEAK_kkk
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
		result2,	double *,	Output,	,	,	
		result3,	double *,	Output,	,	,	
		result4,	double *,	Output,	,	,	
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
void  LEAK_kkk(char *, int, int, int, int, double, double, char, char, double, double, double, double, double, double, double, double, double, double *, double *, double *, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: VTXPL_mos_sample
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
		mvtx,	double,	Input,	,	,	
		mk,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		result_vt,	double *,	Output,	,	,	
		result_k,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <par_util.h>
#include <ktxe_proto.h>
#include <ksox_def.h>
#include <kui_proto.h>
#include "LBC5_proto.h"
#include <math.h>
#define MAX(a,b)	((a)>(b) ? (a) : (b))
#define CDELAY  125.E-12
#define ILEAK   20.E-12 
	END USRLIB MODULE INFORMATION
*/
void  VTXPL_mos_sample(char *, int, int, int, int, int, int, double, double, char, double, double, int, double, double, double, double, double, double, double, double, double *, double *);

