/* res4_test function prototype and KITT header file */

/* COPY OF USRLIB SETTINGS INFORMATION */
/* [VISIBILITY] */
/* HIDDEN=0 */
/* [DEPENDENCIES] */
/* LIBS=-lA07 -lHP4284 -lktest -lPARLIB400 */
/* OBJECTS= */
/* HEADERS= */
/* [NON-KULT SEARCH PATH] */
/* PATHS= */

/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_beta2
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
double res4_test_beta2(int em, int base, int coll, int subst, double ic, double vce, double * vbeout, double * icout, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_beta2_cy
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
double res4_test_beta2_cy(int em, int base, int coll, int subst, double ic, double vce, double * vbeout, double * icout, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_beta2_kj
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 12
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
		IEFORCE,	double *,	Output,	,	,	
		ICPGMS,	double *,	Output,	,	,	
		IC3,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double res4_test_beta2_kj(int em, int base, int coll, int subst, double ic, double vce, double * vbeout, double * icout, char type, double * IEFORCE, double * ICPGMS, double * IC3);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_beta2_raw
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
double  res4_test_beta2_raw(int, int, int, int, double, double, double *, double *, char);

/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_beta_Jb
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 10
	ARGUMENTS:
		emit,	int,	Input,	,	,	
		base,	int,	Input,	,	,	
		coll,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		vce,	double,	Input,	,	,	
		ib,	double,	Input,	,	,	
		vs,	double,	Input,	,	,	
		delaytime,	double,	Input,	,	,	
		ic,	double *,	Output,	,	,	
		beta,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void res4_test_beta_Jb(int emit, int base, int coll, int sub, double vce, double ib, double vs, double delaytime, double * ic, double * beta);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_beta_lpnp
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 10
	ARGUMENTS:
		em,	int,	Input,	,	,	
		base,	int,	Input,	,	,	
		coll,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		ic,	double,	Input,	,	,	
		vce,	double,	Input,	,	,	
		vsub,	double,	Input,	,	,	
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
double res4_test_beta_lpnp(int em, int base, int coll, int subst, double ic, double vce, double vsub, double * vbeout, double * icout, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_bip_median
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 12
	ARGUMENTS:
		num,	int,	Input,	,	,	
		e,	int,	Input,	,	,	
		b,	int,	Input,	,	,	
		c,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		vcb,	double,	Input,	,	,	
		ie,	double,	Input,	,	,	
		vsub,	double,	Input,	,	,	
		ibe,	double *,	Output,	,	,	
		ice,	double *,	Output,	,	,	
		vbe,	double *,	Output,	,	,	
		beta,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <stdlib.h>
#include "COM_usrlib.h"
                             

	END USRLIB MODULE INFORMATION
*/
void res4_test_bip_median(int num, int e, int b, int c, int sub, double vcb, double ie, double vsub, double * ibe, double * ice, double * vbe, double * beta);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_bkdn
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 5
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		ipgm,	double,	Input,	,	,	
		vlim,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "COM_usrlib.h"
	END USRLIB MODULE INFORMATION
*/
double res4_test_bkdn(int hi, int lo, int subst, double ipgm, double vlim);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_bvceoI
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 7
	ARGUMENTS:
		em,	int,	Input,	,	,	
		base,	int,	Input,	,	,	
		coll,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		ipgm,	double,	Input,	,	,	
		vlim,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double res4_test_bvceoI(int em, int base, int coll, int subst, double ipgm, double vlim, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_bvd1isl
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 11
	ARGUMENTS:
		d,	int,	Input,	,	,	
		g,	int,	Input,	,	,	
		s,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		isol,	int,	Input,	,	,	
		vdsstart,	double,	Input,	,	,	
		vdsstop,	double,	Input,	,	,	
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
double res4_test_bvd1isl(int d, int g, int s, int sub, int isol, double vdsstart, double vdsstop, int nstep, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_bvdii
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 11
	ARGUMENTS:
		d,	int,	Input,	,	,	
		g,	int,	Input,	,	,	
		s,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		vdsstart,	double,	Input,	,	,	
		vdsstop,	double,	Input,	,	,	
		nstep,	int,	Input,	,	,	
		ipgm,	double,	Input,	,	,	
		vg,	double,	Input,	,	,	
		udelay,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
	INCLUDES:
#include <math.h>		                
#include "lptdef.h"
#include <stdlib.h>
#include "LBC5_proto.h"
	
	END USRLIB MODULE INFORMATION
*/
double res4_test_bvdii(int d, int g, int s, int sub, double vdsstart, double vdsstop, int nstep, double ipgm, double vg, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_bvdii_deplpm
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 13
	ARGUMENTS:
		d,	int,	Input,	,	,	
		g,	int,	Input,	,	,	
		s,	int,	Input,	,	,	
		body,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		vdsstart,	double,	Input,	,	,	
		vdsstop,	double,	Input,	,	,	
		nstep,	int,	Input,	,	,	
		ipgm,	double,	Input,	,	,	
		vg,	double,	Input,	,	,	
		vsub,	double,	Input,	,	,	
		udelay,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
	INCLUDES:
#include <math.h>		                
#include "lptdef.h"
#include <stdlib.h>
#include "LBC5_proto.h"
	
	END USRLIB MODULE INFORMATION
*/
double res4_test_bvdii_deplpm(int d, int g, int s, int body, int sub, double vdsstart, double vdsstop, int nstep, double ipgm, double vg, double vsub, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_bvdii_iso
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 12
	ARGUMENTS:
		d,	int,	Input,	,	,	
		g,	int,	Input,	,	,	
		s,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		isl,	int,	Input,	,	,	
		vdsstart,	double,	Input,	,	,	
		vdsstop,	double,	Input,	,	,	
		nstep,	int,	Input,	,	,	
		ipgm,	double,	Input,	,	,	
		vg,	double,	Input,	,	,	
		udelay,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
	INCLUDES:
#include <math.h>		                
#include "lptdef.h"
#include <stdlib.h>
#include "LBC5_proto.h"
	
	END USRLIB MODULE INFORMATION
*/
double res4_test_bvdii_iso(int d, int g, int s, int sub, int isl, double vdsstart, double vdsstop, int nstep, double ipgm, double vg, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_bvdss1_deplpm
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 12
	ARGUMENTS:
		d,	int,	Input,	,	,	
		g,	int,	Input,	,	,	
		s,	int,	Input,	,	,	
		body,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		vdsstart,	double,	Input,	,	,	
		vdsstop,	double,	Input,	,	,	
		vsub,	double,	Input,	,	,	
		nstep,	int,	Input,	,	,	
		ipgm,	double,	Input,	,	,	
		udelay,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
	INCLUDES:
#include <math.h>		                
#include "lptdef.h"
#include <stdlib.h>
#include "LBC5_proto.h"
	
	END USRLIB MODULE INFORMATION
*/
double res4_test_bvdss1_deplpm(int d, int g, int s, int body, int sub, double vdsstart, double vdsstop, double vsub, int nstep, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_bvdss1_iso
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 11
	ARGUMENTS:
		d,	int,	Input,	,	,	
		g,	int,	Input,	,	,	
		s,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		isl,	int,	Input,	,	,	
		vdsstart,	double,	Input,	,	,	
		vdsstop,	double,	Input,	,	,	
		nstep,	int,	Input,	,	,	
		ipgm,	double,	Input,	,	,	
		udelay,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
	INCLUDES:
#include <math.h>		                
#include "lptdef.h"
#include <stdlib.h>
#include "LBC5_proto.h"
	
	END USRLIB MODULE INFORMATION
*/
double res4_test_bvdss1_iso(int d, int g, int s, int sub, int isl, double vdsstart, double vdsstop, int nstep, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_bvdss1_isosub
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 12
	ARGUMENTS:
		d,	int,	Input,	,	,	
		g,	int,	Input,	,	,	
		s,	int,	Input,	,	,	
		body,	int,	Input,	,	,	
		isl,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		vdsstart,	double,	Input,	,	,	
		vdsstop,	double,	Input,	,	,	
		nstep,	int,	Input,	,	,	
		ipgm,	double,	Input,	,	,	
		udelay,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>

#include <stdlib.h>
#include "LBC5_proto.h"
	
	END USRLIB MODULE INFORMATION
*/
double res4_test_bvdss1_isosub(int d, int g, int s, int body, int isl, int sub, double vdsstart, double vdsstop, int nstep, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_bvdss2
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 10
	ARGUMENTS:
		d,	int,	Input,	,	,	
		g,	int,	Input,	,	,	
		s,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		vdsstart,	double,	Input,	,	,	
		vdsstop,	double,	Input,	,	,	
		nstep,	int,	Input,	,	,	
		ipgm,	double,	Input,	,	,	
		udelay,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
	INCLUDES:
#include <math.h>		                
#include "lptdef.h"
#include <stdlib.h>
#include "LBC5_proto.h"
	
	END USRLIB MODULE INFORMATION
*/
double res4_test_bvdss2(int d, int g, int s, int sub, double vdsstart, double vdsstop, int nstep, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_Calc_Pct_Mismatch
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 4
	ARGUMENTS:
		a11,	double,	Input,	,	,	
		a21,	double,	Input,	,	,	
		a12,	double,	Input,	,	,	
		a22,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double res4_test_Calc_Pct_Mismatch(double a11, double a21, double a12, double a22);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_Calc_Reproduc
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 4
	ARGUMENTS:
		a11,	double,	Input,	,	,	
		a21,	double,	Input,	,	,	
		a12,	double,	Input,	,	,	
		a22,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double res4_test_Calc_Reproduc(double a11, double a21, double a12, double a22);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_cap_internal
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 6
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		vacc,	double,	Input,	,	,	
		ca,	double *,	Output,	,	,	
		ga,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#define CRTLIM 1.0E-6		                              
#include "ktxe_types.h"
#include "COM_usrlib.h"
#define MAX(a,b)	((a)>(b) ? (a) : (b))
	END USRLIB MODULE INFORMATION
*/
void res4_test_cap_internal(int hi, int lo, int sub, double vacc, double * ca, double * ga);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_cap_vcc
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 10
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		v1,	double,	Input,	,	,	
		v2,	double,	Input,	,	,	
		sdelay,	double,	Input,	,	,	
		v,	D_ARRAY_T,	Output,	,	,	
		npts,	int,	Input,	,	,	
		c,	D_ARRAY_T,	Output,	,	,	
		npts2,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <PARLIB400_proto.h>
	END USRLIB MODULE INFORMATION
*/
void res4_test_cap_vcc(int hi, int lo, int subst, double v1, double v2, double sdelay, double *v, int npts, double *c, int npts2);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_CAPOFFSET
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "kdf.h"
#include "ktxe_types.h"
#include "ksox_def.h"
#include "COM_usrlib.h"
#define DEBUG 1
#define _REENTRANT
	END USRLIB MODULE INFORMATION
*/
void res4_test_CAPOFFSET();


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_ceoswp
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 10
	ARGUMENTS:
		e,	int,	Input,	,	,	
		b,	int,	Input,	,	,	
		c,	int,	Input,	,	,	
		s,	int,	Input,	,	,	
		vcestart,	double,	Input,	,	,	
		vcestop,	double,	Input,	,	,	
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
double res4_test_ceoswp(int e, int b, int c, int s, double vcestart, double vcestop, int nstep, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_CheckKdf
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		filename,	char *,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <stdlib.h>
#include <string.h>

	END USRLIB MODULE INFORMATION
*/
int res4_test_CheckKdf(char * filename);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_custom_PrAutoAlign
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <prb.h>
	END USRLIB MODULE INFORMATION
*/
int res4_test_custom_PrAutoAlign();


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_custom_PrLoad
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <prb.h>
	END USRLIB MODULE INFORMATION
*/
int res4_test_custom_PrLoad();


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_custom_PrProfile
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <prb.h>

	END USRLIB MODULE INFORMATION
*/
int res4_test_custom_PrProfile();


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_diode_beta
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 9
	ARGUMENTS:
		anode,	int,	Input,	,	,	
		cathode,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		vcat,	double,	Input,	,	,	
		ianode,	double,	Input,	,	,	
		delaytime,	double,	Input,	,	,	
		icat,	double *,	Output,	,	,	
		isub,	double *,	Output,	,	,	
		beta,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>

#define CRTLIM 200.0e-3


	END USRLIB MODULE INFORMATION
*/
void res4_test_diode_beta(int anode, int cathode, int sub, double vcat, double ianode, double delaytime, double * icat, double * isub, double * beta);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_erase_eeprom
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
void res4_test_erase_eeprom(int drn, int cg, int src, int tun, double vtun, int te);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_ev
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 13
	ARGUMENTS:
		e,	int,	Input,	,	,	
		b,	int,	Input,	,	,	
		c,	int,	Input,	,	,	
		s,	int,	Input,	,	,	
		ibe,	double,	Input,	,	,	
		vstart,	double,	Input,	,	,	
		vstop,	double,	Input,	,	,	
		npts,	int,	Input,	,	,	
		vsub,	double,	Input,	,	,	
		slope,	double *,	Output,	,	,	
		iflag,	int *,	Output,	,	,	
		r,	double *,	Output,	,	,	
		early,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void res4_test_ev(int e, int b, int c, int s, double ibe, double vstart, double vstop, int npts, double vsub, double * slope, int * iflag, double * r, double * early);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_fblow6_a07
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 7
	ARGUMENTS:
		hi1,	int,	Input,	,	,	
		hi2,	int,	Input,	,	,	
		lo1,	int,	Input,	,	,	
		lo2,	int,	Input,	,	,	
		vtest,	double,	Input,	,	,	
		wtime,	double,	Input,	,	,	
		ilim,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double  res4_test_fblow6_a07(int, int, int, int, double, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_fblow_smu
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 4
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		vbias,	double,	Input,	,	,	
		udelay,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "COM_usrlib.h"
	END USRLIB MODULE INFORMATION
*/
void res4_test_fblow_smu(int hi, int lo, double vbias, int udelay);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_fltCapVds
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 16
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		hi_gate,	int,	Input,	,	,	
		lo_gate,	int,	Input,	,	,	
		s1,	int,	Input,	,	,	
		s2,	int,	Input,	,	,	
		klvs1,	int,	Input,	,	,	
		klvs2,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		ids,	double,	Input,	,	,	
		vgstart,	double,	Input,	,	,	
		vgstop,	double,	Input,	,	,	
		udelay,	double,	Input,	,	,	
		Vds1,	D_ARRAY_T,	Output,	,	,	
		npts1,	int,	Input,	,	,	
		Vds2,	D_ARRAY_T,	Output,	,	,	
		npts2,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>

#define VLTLIM 40.0

#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#define VLTLIM 40.0
	END USRLIB MODULE INFORMATION
*/
void res4_test_fltCapVds(int drain, int hi_gate, int lo_gate, int s1, int s2, int klvs1, int klvs2, int subst, double ids, double vgstart, double vgstop, double udelay, double *Vds1, int npts1, double *Vds2, int npts2);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_fnddat
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 12
	ARGUMENTS:
		x,	D_ARRAY_T,	Input,	,	,	
		npts,	int,	Input,	,	,	
		y,	D_ARRAY_T,	Input,	,	,	
		npts1,	int,	Input,	,	,	
		x1,	double,	Input,	,	,	
		x2,	double,	Input,	,	,	
		xnew,	D_ARRAY_T,	Output,	,	,	
		np1,	int,	Input,	,	,	
		ynew,	D_ARRAY_T,	Output,	,	,	
		np2,	int,	Input,	,	,	
		np,	int *,	Output,	,	,	
		code,	char,	Input,	,	,	
	INCLUDES:
#include <math.h>
#include "lptdef.h"
#include <stdlib.h>
	END USRLIB MODULE INFORMATION
*/
void res4_test_fnddat(double *x, int npts, double *y, int npts1, double x1, double x2, double *xnew, int np1, double *ynew, int np2, int * np, char code);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_fndpt
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 4
	ARGUMENTS:
		values,	F_ARRAY_T,	Input,	,	,	
		npts,	int,	Input,	,	,	
		target,	double,	Input,	,	,	
		j,	int *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void res4_test_fndpt(float *values, int npts, double target, int * j);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_fndpt_1
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 4
	ARGUMENTS:
		values,	D_ARRAY_T,	Input,	,	,	
		npts,	int,	Input,	,	,	
		target,	double,	Input,	,	,	
		j,	int *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void res4_test_fndpt_1(double *values, int npts, double target, int * j);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_fndtrg
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 2
	ARGUMENTS:
		low,	double,	Input,	,	,	
		high,	double,	Input,	,	,	
	INCLUDES:

	END USRLIB MODULE INFORMATION
*/
int res4_test_fndtrg(double low, double high);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_get_site_row_col
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 2
	ARGUMENTS:
		row,	float *,	Output,	,	,	
		col,	float *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <kdf.h>
#include <COM_usrlib.h>
	END USRLIB MODULE INFORMATION
*/
void res4_test_get_site_row_col(float * row, float * col);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_handleControlC
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		sig,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <kdf.h>
#include <signal.h>
#include <COM_usrlib.h>

	END USRLIB MODULE INFORMATION
*/
void res4_test_handleControlC(int sig);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_ibicvbe
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 13
	ARGUMENTS:
		e,	int,	Input,	,	,	
		b,	int,	Input,	,	,	
		c,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		vcb,	double,	Input,	,	,	
		ie,	double,	Input,	,	,	
		vsub,	double,	Input,	,	,	
		avgNum,	int,	Input,	,	,	
		tdelay,	double,	Input,	,	,	
		ibe,	double *,	Output,	,	,	
		ice,	double *,	Output,	,	,	
		vbe,	double *,	Output,	,	,	
		beta,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#define VLTLIM 3.0
#define VCOMP 0.98*VLTLIM
#define CRTLIM (200.0E-3)
#define ICOMP 0.98*CRTLIM

	END USRLIB MODULE INFORMATION
*/
void res4_test_ibicvbe(int e, int b, int c, int sub, double vcb, double ie, double vsub, int avgNum, double tdelay, double * ibe, double * ice, double * vbe, double * beta);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_ibvc
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 6
	ARGUMENTS:
		emitt,	int,	Input,	,	,	
		base,	int,	Input,	,	,	
		coll,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		Vbe,	double,	Input,	,	,	
		ib,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double res4_test_ibvc(int emitt, int base, int coll, int sub, double Vbe, double * ib);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_ic2
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 10
	ARGUMENTS:
		emitt,	int,	Input,	,	,	
		base,	int,	Input,	,	,	
		col1,	int,	Input,	,	,	
		col2,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		vce1,	double,	Input,	,	,	
		vce2,	double,	Input,	,	,	
		vbe,	double,	Input,	,	,	
		ice1,	double *,	Output,	,	,	
		ice2,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>

#define CRTLIM 200.0e-3
#define ICOMP 0.98*CRTLIM
	END USRLIB MODULE INFORMATION
*/
void res4_test_ic2(int emitt, int base, int col1, int col2, int sub, double vce1, double vce2, double vbe, double * ice1, double * ice2);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_icbo
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 6
	ARGUMENTS:
		e,	int,	Input,	,	,	
		b,	int,	Input,	,	,	
		c,	int,	Input,	,	,	
		s,	int,	Input,	,	,	
		vcbo,	double,	Input,	,	,	
		vsub,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double res4_test_icbo(int e, int b, int c, int s, double vcbo, double vsub);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_iceo
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 6
	ARGUMENTS:
		e,	int,	Input,	,	,	
		b,	int,	Input,	,	,	
		c,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		vce,	double,	Input,	,	,	
		vsub,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double res4_test_iceo(int e, int b, int c, int sub, double vce, double vsub);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_ices
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 6
	ARGUMENTS:
		e,	int,	Input,	,	,	
		b,	int,	Input,	,	,	
		c,	int,	Input,	,	,	
		s,	int,	Input,	,	,	
		vces,	double,	Input,	,	,	
		vsub,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double res4_test_ices(int e, int b, int c, int s, double vces, double vsub);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_id1
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
double res4_test_id1(int drain, int gate, int source, int subst, double vgs, double vds, double vbs);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_id1_5
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 8
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		body,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
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
double res4_test_id1_5(int drain, int gate, int source, int body, int subst, double vgs, double vds, double vbs);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_id1_a
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
double res4_test_id1_a(int drain, int gate, int source, int subst, double vgs, double vds, double vbs);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_id1_iso
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 8
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		isl,	int,	Input,	,	,	
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
double res4_test_id1_iso(int drain, int gate, int source, int subst, int isl, double vgs, double vds, double vbs);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_id1_isosub
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 9
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		body,	int,	Input,	,	,	
		isl,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
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
double res4_test_id1_isosub(int drain, int gate, int source, int body, int isl, int subst, double vgs, double vds, double vbs);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_id2
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
double res4_test_id2(int drain, int gate, int source, int subst, double vgs, double vds, double vbs);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_ioff
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 5
	ARGUMENTS:
		DRAIN,	int,	Input,	,	,	
		GATE,	int,	Input,	,	,	
		SOURCE,	int,	Input,	,	,	
		SUBST,	int,	Input,	,	,	
		VDS,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double res4_test_ioff(int DRAIN, int GATE, int SOURCE, int SUBST, double VDS);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_ioffis
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 6
	ARGUMENTS:
		DRAIN,	int,	Input,	,	,	
		GATE,	int,	Input,	,	,	
		SOURCE,	int,	Input,	,	,	
		SUBST,	int,	Input,	,	,	
		isol,	int,	Input,	,	,	
		VDS,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double res4_test_ioffis(int DRAIN, int GATE, int SOURCE, int SUBST, int isol, double VDS);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_isub
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 7
	ARGUMENTS:
		DRAIN,	int,	Input,	,	,	
		GATE,	int,	Input,	,	,	
		SOURCE,	int,	Input,	,	,	
		SUBST,	int,	Input,	,	,	
		VGS,	double,	Input,	,	,	
		VDS,	double,	Input,	,	,	
		VBS,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double  res4_test_isub(int, int, int, int, double, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_isub5
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 8
	ARGUMENTS:
		DRAIN,	int,	Input,	,	,	
		GATE,	int,	Input,	,	,	
		SOURCE,	int,	Input,	,	,	
		BODY,	int,	Input,	,	,	
		SUBST,	int,	Input,	,	,	
		VGS,	double,	Input,	,	,	
		VDS,	double,	Input,	,	,	
		VBS,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double res4_test_isub5(int DRAIN, int GATE, int SOURCE, int BODY, int SUBST, double VGS, double VDS, double VBS);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_isub_iso
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 8
	ARGUMENTS:
		DRAIN,	int,	Input,	,	,	
		GATE,	int,	Input,	,	,	
		SOURCE,	int,	Input,	,	,	
		SUBST,	int,	Input,	,	,	
		ISL,	int,	Input,	,	,	
		VGS,	double,	Input,	,	,	
		VDS,	double,	Input,	,	,	
		VBS,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double res4_test_isub_iso(int DRAIN, int GATE, int SOURCE, int SUBST, int ISL, double VGS, double VDS, double VBS);



/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_kdelay
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 3
	ARGUMENTS:
		npin,	int,	Input,	,	,	
		i,	double,	Input,	,	,	
		v,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>	                
#include <math.h>	              
#include "lptdef.h"
#define MAX(a,b)	((a)>(b) ? (a) : (b))

#define CDELAY 	100.0E-12
#define ILEAK  	1.E-12 
	END USRLIB MODULE INFORMATION
*/
void res4_test_kdelay(int npin, double i, double v);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_lambda
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 12
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		vstart,	double,	Input,	,	,	
		vstop,	double,	Input,	,	,	
		vgs,	double,	Input,	,	,	
		vsub,	double,	Input,	,	,	
		npts,	int,	Input,	,	,	
		slope,	double *,	Output,	,	,	
		kflag,	int *,	Output,	,	,	
		r,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>

#define MAXPTS 101
#define VLTLIM 30.0
	END USRLIB MODULE INFORMATION
*/
double res4_test_lambda(int drain, int gate, int source, int subst, double vstart, double vstop, double vgs, double vsub, int npts, double * slope, int * kflag, double * r);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_lambda_iso
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 13
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		isl,	int,	Input,	,	,	
		vstart,	double,	Input,	,	,	
		vstop,	double,	Input,	,	,	
		vgs,	double,	Input,	,	,	
		vsub,	double,	Input,	,	,	
		npts,	int,	Input,	,	,	
		slope,	double *,	Output,	,	,	
		kflag,	int *,	Output,	,	,	
		r,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>

#define MAXPTS 101
#define VLTLIM 30.0
	END USRLIB MODULE INFORMATION
*/
double res4_test_lambda_iso(int drain, int gate, int source, int subst, int isl, double vstart, double vstop, double vgs, double vsub, int npts, double * slope, int * kflag, double * r);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_leak
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 5
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		v,	double,	Input,	,	,	
		ilim,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double res4_test_leak(int hi, int lo, int subst, double v, double ilim);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_leak1
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 4
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		v,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double  res4_test_leak1(int, int, int, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_leak1_2hi
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 5
	ARGUMENTS:
		hi1,	int,	Input,	,	,	
		hi2,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		v,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double  res4_test_leak1_2hi(int, int, int, int, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_leak1_float14
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 18
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
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
		v,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double  res4_test_leak1_float14(int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_leak1_high_imp
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 5
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		dummy1,	int,	Input,	,	,	
		v,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double  res4_test_leak1_high_imp(int, int, int, int, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_leak1_high_imp2
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 6
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		dummy1,	int,	Input,	,	,	
		dummy2,	int,	Input,	,	,	
		v,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double  res4_test_leak1_high_imp2(int, int, int, int, int, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_leak1_kj
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 4
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		v,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double  res4_test_leak1_kj(int, int, int, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_leak4
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
double res4_test_leak4(int hi, int lo1, int lo2, int subst, double v);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_leak4_high_imp
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 6
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo1,	int,	Input,	,	,	
		lo2,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		dummy1,	int,	Input,	,	,	
		v,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double  res4_test_leak4_high_imp(int, int, int, int, int, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_leak4_lc
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 6
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo1,	int,	Input,	,	,	
		lo2,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		v,	double,	Input,	,	,	
		ilim,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double res4_test_leak4_lc(int hi, int lo1, int lo2, int subst, double v, double ilim);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_leak4_lc1
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 7
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		hi2,	int,	Input,	,	,	
		lo1,	int,	Input,	,	,	
		lo2,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		v,	double,	Input,	,	,	
		ilim,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double  res4_test_leak4_lc1(int, int, int, int, int, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_leak4_lc_a07
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 22
	ARGUMENTS:
		hi1,	int,	Input,	,	,	
		hi2,	int,	Input,	,	,	
		lo1,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		v,	double,	Input,	,	,	
		ilim,	double,	Input,	,	,	
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
double  res4_test_leak4_lc_a07(int, int, int, int, double, double, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int);

/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_leak_float14
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 17
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
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
		v,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double  res4_test_leak_float14(int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_leak_iso
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 6
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo1,	int,	Input,	,	,	
		lo2,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		isl,	int,	Input,	,	,	
		v,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double res4_test_leak_iso(int hi, int lo1, int lo2, int subst, int isl, double v);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_leak_ph
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 6
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		v,	double,	Input,	,	,	
		ilim,	double,	Input,	,	,	
		area,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double  res4_test_leak_ph(int, int, int, double, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_leakpa5
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 6
	ARGUMENTS:
		HI1,	int,	Input,	,	,	
		LO1,	int,	Input,	,	,	
		LO2,	int,	Input,	,	,	
		SUBST,	int,	Input,	,	,	
		pin5,	int,	Input,	,	,	
		V1,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double res4_test_leakpa5(int HI1, int LO1, int LO2, int SUBST, int pin5, double V1);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_leakpag
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 6
	ARGUMENTS:
		HI1,	int,	Input,	,	,	
		HI2,	int,	Input,	,	,	
		LO1,	int,	Input,	,	,	
		SUBST,	int,	Input,	,	,	
		vtest1,	double,	Input,	,	,	
		vtest2,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double res4_test_leakpag(int HI1, int HI2, int LO1, int SUBST, double vtest1, double vtest2);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_linmmx
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 4
	ARGUMENTS:
		a,	F_ARRAY_T,	Input,	,	,	
		n,	int,	Input,	,	,	
		amin,	double *,	Output,	,	,	
		amax,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double res4_test_linmmx(float *a, int n, double * amin, double * amax);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_llsq
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 6
	ARGUMENTS:
		x,	double *,	Input,	,	,	
		y,	double *,	Input,	,	,	
		npts,	int,	Input,	,	,	
		a,	double *,	Output,	,	,	
		b,	double *,	Output,	,	,	
		r,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void res4_test_llsq(double * x, double * y, int npts, double * a, double * b, double * r);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_logstp
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 4
	ARGUMENTS:
		xstart,	double,	Input,	,	,	
		xstop,	double,	Input,	,	,	
		steps,	F_ARRAY_T,	Output,	,	,	
		npts,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
int res4_test_logstp(double xstart, double xstop, float *steps, int npts);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_num_pts_array
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 4
	ARGUMENTS:
		num_low,	double,	Input,	,	,	
		num_hi,	double,	Input,	,	,	
		arr_out,	D_ARRAY_T,	Output,	,	,	
		npts,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void res4_test_num_pts_array(double num_low, double num_hi, double *arr_out, int npts);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_pgm_eeprom
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
void res4_test_pgm_eeprom(int drn, int cg, int src, int tun, double vcg, int tp);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_pgm_eeprom_floating_gate
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 6
	ARGUMENTS:
		drn,	int,	Input,	,	,	
		src,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		vd,	double,	Input,	,	,	
		td,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "COM_usrlib.h"
	END USRLIB MODULE INFORMATION
*/
void res4_test_pgm_eeprom_floating_gate(int drn, int src, int well, int sub, double vd, double td);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_pn2hiswp
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 12
	ARGUMENTS:
		hi1,	int,	Input,	,	,	
		hi2,	int,	Input,	,	,	
		lo1,	int,	Input,	,	,	
		lo2,	int,	Input,	,	,	
		lo3,	int,	Input,	,	,	
		lo4,	int,	Input,	,	,	
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
double res4_test_pn2hiswp(int hi1, int hi2, int lo1, int lo2, int lo3, int lo4, double vstart, double vstop, int nstep, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_pn2swp
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
	END USRLIB MODULE INFORMATION
*/
double res4_test_pn2swp(int hi, int lo, int subst, double vstart, double vstop, int nstep, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_pn2swp_5
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 11
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo1,	int,	Input,	,	,	
		lo2,	int,	Input,	,	,	
		lo3,	int,	Input,	,	,	
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
	END USRLIB MODULE INFORMATION
*/
double res4_test_pn2swp_5(int hi, int lo1, int lo2, int lo3, int subst, double vstart, double vstop, int nstep, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_pn2swp_himp
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
	END USRLIB MODULE INFORMATION
*/
double res4_test_pn2swp_himp(int hi, int lo, int subst, double vstart, double vstop, int nstep, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_pn2swp_old
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
	END USRLIB MODULE INFORMATION
*/
double res4_test_pn2swp_old(int hi, int lo, int subst, double vstart, double vstop, int nstep, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_pn4hiswp
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 11
	ARGUMENTS:
		hi1,	int,	Input,	,	,	
		hi2,	int,	Input,	,	,	
		hi3,	int,	Input,	,	,	
		hi4,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
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
double res4_test_pn4hiswp(int hi1, int hi2, int hi3, int hi4, int lo, double vstart, double vstop, int nstep, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_pn5hiswp
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 12
	ARGUMENTS:
		hi1,	int,	Input,	,	,	
		hi2,	int,	Input,	,	,	
		hi3,	int,	Input,	,	,	
		hi4,	int,	Input,	,	,	
		hi5,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
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
double res4_test_pn5hiswp(int hi1, int hi2, int hi3, int hi4, int hi5, int lo, double vstart, double vstop, int nstep, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_pn5swpgv
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 12
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		hi1,	int,	Input,	,	,	
		lo1,	int,	Input,	,	,	
		lo2,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		vstart,	double,	Input,	,	,	
		vstop,	double,	Input,	,	,	
		vfrc,	double,	Input,	,	,	
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
double res4_test_pn5swpgv(int hi, int hi1, int lo1, int lo2, int subst, double vstart, double vstop, double vfrc, int nstep, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_pulse_custom
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 9
	ARGUMENTS:
		hi_pin,	int,	Input,	,	,	
		low_pin,	int,	Input,	,	,	
		ground_pin1,	int,	Input,	,	,	
		ground_pin2,	int,	Input,	,	,	
		pulse_top,	double,	Input,	,	,	
		pulse_bottom,	double,	Input,	,	,	
		pulse_time,	double,	Input,	,	,	
		current_array,	D_ARRAY_T,	Input,	,	,	
		num_pulses,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void res4_test_pulse_custom(int hi_pin, int low_pin, int ground_pin1, int ground_pin2, double pulse_top, double pulse_bottom, double pulse_time, double *current_array, int num_pulses);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_r2pvsa6_a07
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 8
	ARGUMENTS:
		his,	int,	Input,	,	,	
		him,	int,	Input,	,	,	
		los,	int,	Input,	,	,	
		lom,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		vbb,	double,	Input,	,	,	
		vtest,	double,	Input,	,	,	
		dvb,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>

#define CRTLIM 100.0e-3

	END USRLIB MODULE INFORMATION
*/
double  res4_test_r2pvsa6_a07(int, int, int, int, int, double, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_res
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 4
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		itest,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double  res4_test_res(int, int, int, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_res4
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 6
	ARGUMENTS:
		hi_source,	int,	Input,	,	,	
		hi_meas,	int,	Input,	,	,	
		lo_source,	int,	Input,	,	,	
		lo_meas,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		itest,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>

#define VLTLIM 40.0

	END USRLIB MODULE INFORMATION
*/
double res4_test_res4(int hi_source, int hi_meas, int lo_source, int lo_meas, int subst, double itest);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_res4_a07
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 6
	ARGUMENTS:
		hi_source,	int,	Input,	,	,	
		hi_meas,	int,	Input,	,	,	
		lo_source,	int,	Input,	,	,	
		lo_meas,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		itest,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>

#define VLTLIM 40.0

	END USRLIB MODULE INFORMATION
*/
double  res4_test_res4_a07(int, int, int, int, int, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_res4_ph
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 8
	ARGUMENTS:
		hi_source,	int,	Input,	,	,	
		hi_meas,	int,	Input,	,	,	
		lo_source,	int,	Input,	,	,	
		lo_meas,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		itest,	double,	Input,	,	,	
		vcomp,	double,	Input,	,	,	
		delaytime,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>

#define VLTLIM 40.0

	END USRLIB MODULE INFORMATION
*/
double  res4_test_res4_ph(int, int, int, int, int, double, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_res4b
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 7
	ARGUMENTS:
		his,	int,	Input,	,	,	
		him,	int,	Input,	,	,	
		los,	int,	Input,	,	,	
		lom,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		itest,	double,	Input,	,	,	
		vsub,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>

#define VLTLIM 40.0
	END USRLIB MODULE INFORMATION
*/
double res4_test_res4b(int his, int him, int los, int lom, int subst, double itest, double vsub);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_res4v
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 6
	ARGUMENTS:
		his,	int,	Input,	,	,	
		him,	int,	Input,	,	,	
		los,	int,	Input,	,	,	
		lom,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		vtest,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>

#define CRTLIM 100.0e-3

	END USRLIB MODULE INFORMATION
*/
double res4_test_res4v(int his, int him, int los, int lom, int subst, double vtest);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_res_de500_ra20
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 4
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		itest,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double  res4_test_res_de500_ra20(int, int, int, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_res_delay_500
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 4
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		itest,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double  res4_test_res_delay_500(int, int, int, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_res_j
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 7
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		fpin1,	int,	Input,	,	,	
		fpin2,	int,	Input,	,	,	
		dlytime,	double,	Input,	,	,	
		itest,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double  res4_test_res_j(int, int, int, int, int, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_res_org
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 4
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		itest,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double  res4_test_res_org(int, int, int, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_res_ph
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 6
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		itest,	double,	Input,	,	,	
		vcomp,	double,	Input,	,	,	
		delaytime,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double  res4_test_res_ph(int, int, int, double, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_res_range_20
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 4
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		itest,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double  res4_test_res_range_20(int, int, int, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_res_smu2
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 4
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		itest,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double  res4_test_res_smu2(int, int, int, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_res_smu3
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 4
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		itest,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double  res4_test_res_smu3(int, int, int, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_res_smu4
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 4
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		itest,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double  res4_test_res_smu4(int, int, int, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_resdeltw
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 6
	ARGUMENTS:
		numbs,	int,	Input,	,	,	
		resx,	D_ARRAY_T,	Input,	,	,	
		pts1,	int,	Input,	,	,	
		widthy,	D_ARRAY_T,	Input,	,	,	
		pts2,	int,	Input,	,	,	
		corr,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double res4_test_resdeltw(int numbs, double *resx, int pts1, double *widthy, int pts2, double * corr);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_reshr
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 9
	ARGUMENTS:
		numbs,	int,	Input,	,	,	
		lengthx,	D_ARRAY_T,	Input,	,	,	
		pts1,	int,	Input,	,	,	
		resy,	D_ARRAY_T,	Input,	,	,	
		pts2,	int,	Input,	,	,	
		wid,	double,	Input,	,	,	
		dwid,	double,	Input,	,	,	
		rslope,	double *,	Output,	,	,	
		corr,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double res4_test_reshr(int numbs, double *lengthx, int pts1, double *resy, int pts2, double wid, double dwid, double * rslope, double * corr);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_resistance_rs2
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 18
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		bulk,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		width,	double,	Input,	,	,	
		length,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
		vforce,	double,	Input,	,	,	
		ilimit,	double,	Input,	,	,	
		vsub,	double,	Input,	,	,	
		delaytime,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		normalize,	char,	Input,	,	,	
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
void res4_test_resistance_rs2(char * devname, int hi, int lo, int sub, int bulk, int chuckcon, double width, double length, char type, double vforce, double ilimit, double vsub, double delaytime, double intrange, char normalize, double mrange, double lorange, double * R);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_resv
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 4
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		v,	double,	Input,	,	,	
	INCLUDES:
#include <math.h>
#include "lptdef.h"

#define CRTLIM 100.0E-3

	END USRLIB MODULE INFORMATION
*/
double  res4_test_resv(int, int, int, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_resv_2hi
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 5
	ARGUMENTS:
		hi1,	int,	Input,	,	,	
		hi2,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		v,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>

#define CRTLIM 100.0E-3

	END USRLIB MODULE INFORMATION
*/
double res4_test_resv_2hi(int hi1, int hi2, int lo, int subst, double v);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_resv_fnc
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 4
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		v,	double,	Input,	,	,	
	INCLUDES:
#include <math.h>
#include "lptdef.h"

#define CRTLIM 100.0E-3

	END USRLIB MODULE INFORMATION
*/
double res4_test_resv_fnc(int hi, int lo, int subst, double v);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_resv_raw
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 6
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		vmin,	double,	Input,	,	,	
		Vmax,	double,	Input,	,	,	
		vstep,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "lptdef.h"

#define CRTLIM 100.0E-3

#include <par_util.h>
#include <stdlib.h>
#include <ktxe_proto.h>
#include <ksox_def.h>
#include <kui_proto.h>

#define MAX(a,b)	((a)>(b) ? (a) : (b))
#define ILEAK   20.E-12 

	END USRLIB MODULE INFORMATION
*/
double  res4_test_resv_raw(int, int, int, double, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_ron
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 8
	ARGUMENTS:
		DRN1,	int,	Input,	,	,	
		DRN2,	int,	Input,	,	,	
		GATE,	int,	Input,	,	,	
		SRC1,	int,	Input,	,	,	
		SRC2,	int,	Input,	,	,	
		SUB,	int,	Input,	,	,	
		VDS,	double,	Input,	,	,	
		VGS,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double res4_test_ron(int DRN1, int DRN2, int GATE, int SRC1, int SRC2, int SUB, double VDS, double VGS);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_ron5
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 9
	ARGUMENTS:
		DRN1,	int,	Input,	,	,	
		DRN2,	int,	Input,	,	,	
		GATE,	int,	Input,	,	,	
		SRC1,	int,	Input,	,	,	
		SRC2,	int,	Input,	,	,	
		BODY,	int,	Input,	,	,	
		SUB,	int,	Input,	,	,	
		VDS,	double,	Input,	,	,	
		VGS,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double res4_test_ron5(int DRN1, int DRN2, int GATE, int SRC1, int SRC2, int BODY, int SUB, double VDS, double VGS);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_ron5_vsub
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 10
	ARGUMENTS:
		DRN1,	int,	Input,	,	,	
		DRN2,	int,	Input,	,	,	
		GATE,	int,	Input,	,	,	
		SRC1,	int,	Input,	,	,	
		SRC2,	int,	Input,	,	,	
		BODY,	int,	Input,	,	,	
		SUB,	int,	Input,	,	,	
		VDS,	double,	Input,	,	,	
		VGS,	double,	Input,	,	,	
		VSUB,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double res4_test_ron5_vsub(int DRN1, int DRN2, int GATE, int SRC1, int SRC2, int BODY, int SUB, double VDS, double VGS, double VSUB);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_ronrf
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 8
	ARGUMENTS:
		DRN1,	int,	Input,	,	,	
		DRN2,	int,	Input,	,	,	
		GATE,	int,	Input,	,	,	
		SRC1,	int,	Input,	,	,	
		SRC2,	int,	Input,	,	,	
		SUB,	int,	Input,	,	,	
		VDS,	double,	Input,	,	,	
		VGS,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double res4_test_ronrf(int DRN1, int DRN2, int GATE, int SRC1, int SRC2, int SUB, double VDS, double VGS);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_ronrfis
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 9
	ARGUMENTS:
		DRN1,	int,	Input,	,	,	
		DRN2,	int,	Input,	,	,	
		GATE,	int,	Input,	,	,	
		SRC1,	int,	Input,	,	,	
		SRC2,	int,	Input,	,	,	
		SUB,	int,	Input,	,	,	
		isol,	int,	Input,	,	,	
		VDS,	double,	Input,	,	,	
		VGS,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double res4_test_ronrfis(int DRN1, int DRN2, int GATE, int SRC1, int SRC2, int SUB, int isol, double VDS, double VGS);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_setcmtr
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 4
	ARGUMENTS:
		cmtrx,	int,	Input,	,	,	
		subfcn,	int,	Input,	,	,	
		param1,	float,	Input,	,	,	
		param2,	float,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "hp4284_internal.h"
char FuncImp[10]="FUNC:IMP ";
char SelectMode[13][5]={"CsQ ","CsRs","RX  ","CpD ","CpQ ","CpG ","CpRp",
                        "CsD ","ZTD ","GB  ","YTR ","YTD ","ZTR "};
char FuncRng[15]="FUNC:IMP:RANG ";
char AutoRng[22]="FUNC:IMP:RANG:AUTO ON";
char IntegrationTime[4][11]={"APER SHOR","APER SHOR,","APER MED ,","APER LONG,"};
char CorrUse[10]="CORR:USE ";
char CorrectionCmd[4][16]={"CORR:OPEN:STAT ","CORR:LOAD:STAT ","CORR:SHOR:STAT "};
char Enable[4]="ON";
char Disable[4]="OFF";
char Freq[6] = "FREQ ";
	END USRLIB MODULE INFORMATION
*/
int res4_test_setcmtr(int cmtrx, int subfcn, float param1, float param2);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_sort
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 2
	ARGUMENTS:
		arr,	D_ARRAY_T,	Input,	,	,	
		num,	int,	Input,	,	,	
	INCLUDES:

	END USRLIB MODULE INFORMATION
*/
void res4_test_sort(double *arr, int num);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_StartCapOffsetDebug
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
void res4_test_StartCapOffsetDebug();


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_StopCapOffsetDebug
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
void res4_test_StopCapOffsetDebug();


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_svmi
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 15
	ARGUMENTS:
		h1,	int,	Input,	,	,	
		h2,	int,	Input,	,	,	
		h3,	int,	Input,	,	,	
		h4,	int,	Input,	,	,	
		l1,	int,	Input,	,	,	
		l2,	int,	Input,	,	,	
		l3,	int,	Input,	,	,	
		l4,	int,	Input,	,	,	
		v1,	double,	Input,	,	,	
		v2,	double,	Input,	,	,	
		v,	D_ARRAY_T,	Output,	,	,	
		npts,	int,	Input,	,	,	
		i,	D_ARRAY_T,	Output,	,	,	
		npts2,	int,	Input,	,	,	
		ilim,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <PARLIB400_proto.h>
	END USRLIB MODULE INFORMATION
*/
void res4_test_svmi(int h1, int h2, int h3, int h4, int l1, int l2, int l3, int l4, double v1, double v2, double *v, int npts, double *i, int npts2, double ilim);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_tdelay
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 3
	ARGUMENTS:
		npin,	int,	Input,	,	,	
		i,	double,	Input,	,	,	
		v,	double,	Input,	,	,	
	INCLUDES:
#include <math.h>                     
#include "lptdef.h"
#include <stdio.h>
#define MAX(a,b)	((a)>(b) ? (a) : (b))
#define CDELAY  100.E-12
#define ILEAK   1.E-12 
	END USRLIB MODULE INFORMATION
*/
double res4_test_tdelay(int npin, double i, double v);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_ti_print_time
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 2
	ARGUMENTS:
		in_str,	char *,	Input,	,	,	
		in_data,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#include <ktxe_types.h>
#include "COM_usrlib.h"
#define TI_MAX_LOT_ID_LENGTH 8
#include "kdf.h"
#include <sys/types.h>
#include <sys/stat.h>
#include <sys/errno.h>
#include <unistd.h>
#include <macros.h>
	END USRLIB MODULE INFORMATION
*/
void res4_test_ti_print_time(char * in_str, double in_data);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_ti_set_print_flag
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
int ti_print_flag;
	END USRLIB MODULE INFORMATION
*/
void res4_test_ti_set_print_flag();


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_TimeStamp_msp
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 2
	ARGUMENTS:
		status,	char *,	Input,	,	,	
		endline,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <kdf.h>
#include <signal.h>
#include <COM_usrlib.h>

	END USRLIB MODULE INFORMATION
*/
void res4_test_TimeStamp_msp(char * status, int endline);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_vbeswp
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 11
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		col,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		vstart,	double,	Input,	,	,	
		vstop,	double,	Input,	,	,	
		vc,	double,	Input,	,	,	
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
double res4_test_vbeswp(int hi, int lo, int col, int subst, double vstart, double vstop, double vc, int nstep, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_vbridge_ilo_rs2
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 17
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		bulk,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		width,	double,	Input,	,	,	
		length,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
		iforce,	double,	Input,	1e-03,	-1,	1
		vlimit,	double,	Input,	5,	-10,	100
		vsub,	double,	Input,	0,	-100,	100
		delaytime,	double,	Input,	0.01,	0,	1
		intrange,	double,	Input,	0,	0,	10
		mrange,	double,	Input,	1e-08,	,	
		lorange,	double,	Input,	1e-08,	,	
		V,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void res4_test_vbridge_ilo_rs2(char * devname, int hi, int lo, int sub, int bulk, int chuckcon, double width, double length, char type, double iforce, double vlimit, double vsub, double delaytime, double intrange, double mrange, double lorange, double * V);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_vc
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 5
	ARGUMENTS:
		emitt,	int,	Input,	,	,	
		base,	int,	Input,	,	,	
		coll,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		ib,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double res4_test_vc(int emitt, int base, int coll, int sub, double ib);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_vector_median
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 2
	ARGUMENTS:
		arr,	double *,	Input,	,	,	
		sz,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double res4_test_vector_median(double * arr, int sz);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_vf
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 4
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		itest,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double res4_test_vf(int hi, int lo, int subst, double itest);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_vg1a
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 12
	ARGUMENTS:
		DRAIN,	int,	Input,	,	,	
		GATE,	int,	Input,	,	,	
		SOURCE,	int,	Input,	,	,	
		SUB,	int,	Input,	,	,	
		VLOW,	double,	Input,	,	,	
		VHIGH,	double,	Input,	,	,	
		VD,	double,	Input,	,	,	
		VSUB,	double,	Input,	,	,	
		NSTEP,	int,	Input,	,	,	
		IPGM,	double,	Input,	,	,	
		UDELAY,	double,	Input,	,	,	
		TYPE,	char,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double res4_test_vg1a(int DRAIN, int GATE, int SOURCE, int SUB, double VLOW, double VHIGH, double VD, double VSUB, int NSTEP, double IPGM, double UDELAY, char TYPE);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_vg2a
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 11
	ARGUMENTS:
		d,	int,	Input,	,	,	
		g,	int,	Input,	,	,	
		s,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		type,	char,	Input,	,	,	
		id,	double,	Input,	,	,	
		vlim,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vbs,	double,	Input,	,	,	
		ids,	double *,	Output,	,	,	
		istat,	int *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double res4_test_vg2a(int d, int g, int s, int sub, char type, double id, double vlim, double vds, double vbs, double * ids, int * istat);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_vg2a_iso
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 12
	ARGUMENTS:
		d,	int,	Input,	,	,	
		g,	int,	Input,	,	,	
		s,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		isl,	int,	Input,	,	,	
		type,	char,	Input,	,	,	
		id,	double,	Input,	,	,	
		vlim,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vbs,	double,	Input,	,	,	
		ids,	double *,	Output,	,	,	
		istat,	int *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double res4_test_vg2a_iso(int d, int g, int s, int sub, int isl, char type, double id, double vlim, double vds, double vbs, double * ids, int * istat);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_vg3
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
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "PARLIB400_proto.h"

#define MAX(a,b)	((a)>(b) ? (a) : (b))

	END USRLIB MODULE INFORMATION
*/
double res4_test_vg3(int drain, int gate, int source, int subst, double vlow, double vhigh, double vds, double vbs, double ithr, int niter);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_vg3_isl
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 11
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		isol,	int,	Input,	,	,	
		vlow,	double,	Input,	,	,	
		vhigh,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vbs,	double,	Input,	,	,	
		ithr,	double,	Input,	,	,	
		niter,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "PARLIB400_proto.h"

#define MAX(a,b)	((a)>(b) ? (a) : (b))

	END USRLIB MODULE INFORMATION
*/
double res4_test_vg3_isl(int drain, int gate, int source, int subst, int isol, double vlow, double vhigh, double vds, double vbs, double ithr, int niter);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_vgsweep
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 11
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		vglow,	double,	Input,	,	,	
		vghigh,	double,	Input,	,	,	
		vd,	double,	Input,	,	,	
		nstep,	int,	Input,	,	,	
		idpgm,	double,	Input,	,	,	
		udelay,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double res4_test_vgsweep(int drain, int gate, int source, int sub, double vglow, double vghigh, double vd, int nstep, double idpgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_vgsweep_abs
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 10
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		vglow,	double,	Input,	,	,	
		vghigh,	double,	Input,	,	,	
		vd,	double,	Input,	,	,	
		nstep,	int,	Input,	,	,	
		idpgm,	double,	Input,	,	,	
		udelay,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double res4_test_vgsweep_abs(int drain, int gate, int source, int sub, double vglow, double vghigh, double vd, int nstep, double idpgm, double udelay);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_vgsweep_isl
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 12
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		isol,	int,	Input,	,	,	
		vglow,	double,	Input,	,	,	
		vghigh,	double,	Input,	,	,	
		vd,	double,	Input,	,	,	
		nstep,	int,	Input,	,	,	
		idpgm,	double,	Input,	,	,	
		udelay,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double res4_test_vgsweep_isl(int drain, int gate, int source, int sub, int isol, double vglow, double vghigh, double vd, int nstep, double idpgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_vgswp3
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 13
	ARGUMENTS:
		ds,	int,	Input,	,	,	
		g,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		v1,	double,	Input,	,	,	
		v2,	double,	Input,	,	,	
		v,	D_ARRAY_T,	Output,	,	,	
		npts1,	int,	Input,	,	,	
		ig,	D_ARRAY_T,	Output,	,	,	
		npts2,	int,	Input,	,	,	
		isub,	D_ARRAY_T,	Output,	,	,	
		npts3,	int,	Input,	,	,	
		isrc,	D_ARRAY_T,	Output,	,	,	
		npts4,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <PARLIB400_proto.h>
				
	END USRLIB MODULE INFORMATION
*/
void res4_test_vgswp3(int ds, int g, int sub, double v1, double v2, double *v, int npts1, double *ig, int npts2, double *isub, int npts3, double *isrc, int npts4);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_vp1
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 11
	ARGUMENTS:
		d,	int,	Input,	,	,	
		g,	int,	Input,	,	,	
		s,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		ids,	double,	Input,	,	,	
		vdlim,	double,	Input,	,	,	
		vg1,	double,	Input,	,	,	
		vg2,	double,	Input,	,	,	
		iglim,	double,	Input,	,	,	
		iflag,	double *,	Output,	,	,	
		vp,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void res4_test_vp1(int d, int g, int s, int sub, double ids, double vdlim, double vg1, double vg2, double iglim, double * iflag, double * vp);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_vp_1gate
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 10
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		vglow,	double,	Input,	,	,	
		vghigh,	double,	Input,	,	,	
		vd,	double,	Input,	,	,	
		nstep,	int,	Input,	,	,	
		ipgm,	double,	Input,	,	,	
		udelay,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double res4_test_vp_1gate(int drain, int gate, int source, int sub, double vglow, double vghigh, double vd, int nstep, double ipgm, double udelay);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_vt_lin_LBC5_raw
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 22
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
		intrange,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		Vtl,	double *,	Output,	,	,	
		K,	double *,	Output,	,	,	
		Gm,	double *,	Output,	,	,	
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
void  res4_test_vt_lin_LBC5_raw(char *, int, int, int, int, int, int, double, double, char, double, double, int, double, double, double, double, double, double, double *, double *, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_vtati
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
double res4_test_vtati(int drain, int gate, int source, int subst, double vlow, double vhigh, double vds, double vbs, double ithr, int niter);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_vtexis
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 16
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		isol,	int,	Input,	,	,	
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
	END USRLIB MODULE INFORMATION
*/
double res4_test_vtexis(int drain, int gate, int source, int subst, int isol, double w, double l, double vlow, double vhigh, double vds, double vbs, double ithr, double vstep, int npts, double * slope, int * kflag);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_vtext4
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
	END USRLIB MODULE INFORMATION
*/
double  res4_test_vtext4(int, int, int, int, double, double, double, double, double, double, double, double, int, double *, int *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_vtext4_iso
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 16
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		isl,	int,	Input,	,	,	
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
	END USRLIB MODULE INFORMATION
*/
double res4_test_vtext4_iso(int drain, int gate, int source, int subst, int isl, double w, double l, double vlow, double vhigh, double vds, double vbs, double ithr, double vstep, int npts, double * slope, int * kflag);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_vtext4_raw
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
double  res4_test_vtext4_raw(int, int, int, int, double, double, double, double, double, double, double, double, int, double *, int *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_vtext4_raw_org
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
double  res4_test_vtext4_raw_org(int, int, int, int, double, double, double, double, double, double, double, double, int, double *, int *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_vtext5
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
	END USRLIB MODULE INFORMATION
*/
double res4_test_vtext5(int drain, int gate, int source, int body, int subst, double w, double l, double vlow, double vhigh, double vds, double vbs, double ithr, double vstep, int npts, double * slope, int * kflag);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_vtexts
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
	END USRLIB MODULE INFORMATION
*/
double res4_test_vtexts(int drain, int gate, int source, int subst, double w, double l, double vlow, double vhigh, double vds, double vbs, double ithr, double vstep, int npts, double * slope, int * kflag);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_vtexts5
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
	END USRLIB MODULE INFORMATION
*/
double res4_test_vtexts5(int drain, int gate, int source, int body, int subst, double w, double l, double vlow, double vhigh, double vds, double vbs, double ithr, double vstep, int npts, double * slope, int * kflag);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_vtexts_iso
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 16
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		isl,	int,	Input,	,	,	
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
	END USRLIB MODULE INFORMATION
*/
double res4_test_vtexts_iso(int drain, int gate, int source, int subst, int isl, double w, double l, double vlow, double vhigh, double vds, double vbs, double ithr, double vstep, int npts, double * slope, int * kflag);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_vtfswp
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 11
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		vlow,	double,	Input,	,	,	
		vhigh,	double,	Input,	,	,	
		vd,	double,	Input,	,	,	
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
double  res4_test_vtfswp(int, int, int, int, double, double, double, int, double, double, char);

/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_test_vtfswp_iso
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 12
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		isl,	int,	Input,	,	,	
		vlow,	double,	Input,	,	,	
		vhigh,	double,	Input,	,	,	
		vd,	double,	Input,	,	,	
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
double res4_test_vtfswp_iso(int drain, int gate, int source, int sub, int isl, double vlow, double vhigh, double vd, int nstep, double ipgm, double udelay, char type);


