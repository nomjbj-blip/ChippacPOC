/* ron200 function prototype and KITT header file */

/* COPY OF USRLIB SETTINGS INFORMATION */
/* [VISIBILITY] */
/* HIDDEN=0 */
/* [DEPENDENCIES] */
/* LIBS=-lA07 -lHP4284 -lktest */
/* OBJECTS= */
/* HEADERS= */
/* [NON-KULT SEARCH PATH] */
/* PATHS= */

/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_A07_leak4
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
double ron200_A07_leak4(int hi, int lo1, int lo2, int subst, double v, double ilim);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_beta2
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
double ron200_beta2(int em, int base, int coll, int subst, double ic, double vce, double * vbeout, double * icout, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_beta2_cy
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
double ron200_beta2_cy(int em, int base, int coll, int subst, double ic, double vce, double * vbeout, double * icout, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_beta2_kj
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
double ron200_beta2_kj(int em, int base, int coll, int subst, double ic, double vce, double * vbeout, double * icout, char type, double * IEFORCE, double * ICPGMS, double * IC3);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_beta2_swp
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
double ron200_beta2_swp(int em, int base, int coll, int subst, double ic, double vce, double * vbeout, double * icout, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_beta_Jb
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
void ron200_beta_Jb(int emit, int base, int coll, int sub, double vce, double ib, double vs, double delaytime, double * ic, double * beta);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_beta_lpnp
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
double ron200_beta_lpnp(int em, int base, int coll, int subst, double ic, double vce, double vsub, double * vbeout, double * icout, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_bip_median
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
void ron200_bip_median(int num, int e, int b, int c, int sub, double vcb, double ie, double vsub, double * ibe, double * ice, double * vbe, double * beta);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_BJT_Sweep
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 11
	ARGUMENTS:
		em,	int,	Input,	,	,	
		base,	int,	Input,	,	,	
		coll,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		vbe_start,	double,	Input,	,	,	
		vbe_end,	double,	Input,	,	,	
		vbe_step,	double,	Input,	,	,	
		vce_start,	double,	Input,	,	,	
		vce_end,	double,	Input,	,	,	
		vce_step,	double,	Input,	,	,	
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
void ron200_BJT_Sweep(int em, int base, int coll, int subst, double vbe_start, double vbe_end, double vbe_step, double vce_start, double vce_end, double vce_step, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_bkdn
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
double ron200_bkdn(int hi, int lo, int subst, double ipgm, double vlim);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_bkdnq6_a07
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 9
	ARGUMENTS:
		hi1,	int,	Input,	,	,	
		hi2,	int,	Input,	,	,	
		lo1,	int,	Input,	,	,	
		lo2,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		ipgm,	double,	Input,	,	,	
		vmax,	double,	Input,	,	,	
		vbb,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#define ILOOP 10
	END USRLIB MODULE INFORMATION
*/
double ron200_bkdnq6_a07(int hi1, int hi2, int lo1, int lo2, int subst, double ipgm, double vmax, double vbb, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_bvceo_miho
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 12
	ARGUMENTS:
		em,	int,	Input,	,	,	
		ba,	int,	Input,	,	,	
		co,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		lowt,	int,	Input,	,	,	
		ipgm,	double,	Input,	,	,	
		vlimit,	double,	Input,	,	,	
		nstep,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
		wtime,	double,	Input,	,	,	
		vbb,	double,	Input,	,	,	
		vbe,	double *,	Output,	,	,	
	INCLUDES:
#include <math.h>		                
#include "lptdef.h"
#include <stdlib.h>
#include "LBC5_proto.h"
#include <stdio.h>	

	END USRLIB MODULE INFORMATION
*/
double ron200_bvceo_miho(int em, int ba, int co, int sub, int lowt, double ipgm, double vlimit, double nstep, char type, double wtime, double vbb, double * vbe);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_bvceo_miho2
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 12
	ARGUMENTS:
		em,	int,	Input,	,	,	
		ba,	int,	Input,	,	,	
		co,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		lowt,	int,	Input,	,	,	
		ipgm,	double,	Input,	,	,	
		vlimit,	double,	Input,	,	,	
		nstep,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
		wtime,	double,	Input,	,	,	
		vbb,	double,	Input,	,	,	
		vbe,	double *,	Output,	,	,	
	INCLUDES:
#include <math.h>		                
#include "lptdef.h"
#include <stdlib.h>
#include "LBC5_proto.h"
#include <stdio.h>	

	END USRLIB MODULE INFORMATION
*/
double ron200_bvceo_miho2(int em, int ba, int co, int sub, int lowt, double ipgm, double vlimit, double nstep, char type, double wtime, double vbb, double * vbe);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_bvceoI
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
double ron200_bvceoI(int em, int base, int coll, int subst, double ipgm, double vlim, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_BVD
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
double ron200_BVD(int d, int g, int s, int sub, double vdsstart, double vdsstop, int nstep, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_bvd1isl
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
double ron200_bvd1isl(int d, int g, int s, int sub, int isol, double vdsstart, double vdsstop, int nstep, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_bvdii
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
double ron200_bvdii(int d, int g, int s, int sub, double vdsstart, double vdsstop, int nstep, double ipgm, double vg, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_bvdii_deplpm
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
double ron200_bvdii_deplpm(int d, int g, int s, int body, int sub, double vdsstart, double vdsstop, int nstep, double ipgm, double vg, double vsub, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_bvdii_iso
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
double ron200_bvdii_iso(int d, int g, int s, int sub, int isl, double vdsstart, double vdsstop, int nstep, double ipgm, double vg, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_bvdss1_deplpm
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
double ron200_bvdss1_deplpm(int d, int g, int s, int body, int sub, double vdsstart, double vdsstop, double vsub, int nstep, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_bvdss1_iso
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
double ron200_bvdss1_iso(int d, int g, int s, int sub, int isl, double vdsstart, double vdsstop, int nstep, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_bvdss1_isosub
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
double ron200_bvdss1_isosub(int d, int g, int s, int body, int isl, int sub, double vdsstart, double vdsstop, int nstep, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_bvdss2
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
double ron200_bvdss2(int d, int g, int s, int sub, double vdsstart, double vdsstop, int nstep, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_bvdss2_bef
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
double ron200_bvdss2_bef(int d, int g, int s, int sub, double vdsstart, double vdsstop, int nstep, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_bvdss2_bs
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
double ron200_bvdss2_bs(int d, int g, int s, int sub, double vdsstart, double vdsstop, int nstep, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_bvdss2_ju_swp
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
double  ron200_bvdss2_ju_swp(int, int, int, int, double, double, int, double, double, char);

/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_bvdss2_kj
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
double ron200_bvdss2_kj(int d, int g, int s, int sub, double vdsstart, double vdsstop, int nstep, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_bvdss2_miho
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 10
	ARGUMENTS:
		d,	int,	Input,	,	,	
		g,	int,	Input,	,	,	
		s,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		vdsstart,	double,	Input,	,	,	
		vdsstop,	double,	Input,	,	,	
		ipgm,	double,	Input,	,	,	
		udelay,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
		vbb,	double,	Input,	,	,	
	INCLUDES:
#include <math.h>		                
#include "lptdef.h"
#include <stdlib.h>
#include "LBC5_proto.h"
	
	END USRLIB MODULE INFORMATION
*/
double ron200_bvdss2_miho(int d, int g, int s, int sub, double vdsstart, double vdsstop, double ipgm, double udelay, char type, double vbb);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_bvdss2_miho_h
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 12
	ARGUMENTS:
		d,	int,	Input,	,	,	
		g,	int,	Input,	,	,	
		s,	int,	Input,	,	,	
		bulk,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		vdsstart,	double,	Input,	,	,	
		vdsstop,	double,	Input,	,	,	
		ipgm,	double,	Input,	,	,	
		udelay,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
		vbb,	double,	Input,	,	,	
	INCLUDES:
#include <math.h>		                
#include "lptdef.h"
#include <stdlib.h>
#include "LBC5_proto.h"
	
	END USRLIB MODULE INFORMATION
*/
double ron200_bvdss2_miho_h(int d, int g, int s, int bulk, int sub, int well, double vdsstart, double vdsstop, double ipgm, double udelay, char type, double vbb);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_bvdss2_miho_h_s
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 12
	ARGUMENTS:
		d,	int,	Input,	,	,	
		g,	int,	Input,	,	,	
		s,	int,	Input,	,	,	
		bulk,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		vdsstart,	double,	Input,	,	,	
		vdsstop,	double,	Input,	,	,	
		ipgm,	double,	Input,	,	,	
		udelay,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
		vbb,	double,	Input,	,	,	
	INCLUDES:
#include <math.h>		                
#include "lptdef.h"
#include <stdlib.h>
#include "LBC5_proto.h"
	
	END USRLIB MODULE INFORMATION
*/
double ron200_bvdss2_miho_h_s(int d, int g, int s, int bulk, int sub, int well, double vdsstart, double vdsstop, double ipgm, double udelay, char type, double vbb);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_bvdss2_miho_h_swp
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 12
	ARGUMENTS:
		d,	int,	Input,	,	,	
		g,	int,	Input,	,	,	
		s,	int,	Input,	,	,	
		bulk,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		vdsstart,	double,	Input,	,	,	
		vdsstop,	double,	Input,	,	,	
		ipgm,	double,	Input,	,	,	
		udelay,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
		vbb,	double,	Input,	,	,	
	INCLUDES:
#include <math.h>		                
#include "lptdef.h"
#include <stdlib.h>
#include "LBC5_proto.h"
	
	END USRLIB MODULE INFORMATION
*/
double ron200_bvdss2_miho_h_swp(int d, int g, int s, int bulk, int sub, int well, double vdsstart, double vdsstop, double ipgm, double udelay, char type, double vbb);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_bvdss2_miho_s
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 10
	ARGUMENTS:
		d,	int,	Input,	,	,	
		g,	int,	Input,	,	,	
		s,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		vdsstart,	double,	Input,	,	,	
		vdsstop,	double,	Input,	,	,	
		ipgm,	double,	Input,	,	,	
		udelay,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
		vbb,	double,	Input,	,	,	
	INCLUDES:
#include <math.h>		                
#include "lptdef.h"
#include <stdlib.h>
#include "LBC5_proto.h"
	
	END USRLIB MODULE INFORMATION
*/
double ron200_bvdss2_miho_s(int d, int g, int s, int sub, double vdsstart, double vdsstop, double ipgm, double udelay, char type, double vbb);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_bvdss2_org
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
double ron200_bvdss2_org(int d, int g, int s, int sub, double vdsstart, double vdsstop, int nstep, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_bvdss2_se
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
double ron200_bvdss2_se(int d, int g, int s, int sub, double vdsstart, double vdsstop, int nstep, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_bvdss3
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
		udelay,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
		vref,	double,	Input,	,	,	
	INCLUDES:
#include <math.h>		                
#include "lptdef.h"
#include <stdlib.h>
#include "LBC5_proto.h"
	
	END USRLIB MODULE INFORMATION
*/
double ron200_bvdss3(int d, int g, int s, int sub, double vdsstart, double vdsstop, int nstep, double ipgm, double udelay, char type, double vref);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_bvdss_ph
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 20
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
		idmax,	double,	Input,	,	,	
		vdsmin,	double,	Input,	,	,	
		vdsmax,	double,	Input,	,	,	
		vgs,	double,	Input,	,	,	
		vbs,	double,	Input,	,	,	
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
void ron200_bvdss_ph(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double idmax, double vdsmin, double vdsmax, double vgs, double vbs, double multiplier, double intrange, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_Calc_Pct_Mismatch
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
double ron200_Calc_Pct_Mismatch(double a11, double a21, double a12, double a22);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_Calc_Reproduc
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
double ron200_Calc_Reproduc(double a11, double a21, double a12, double a22);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_cap_2spo_cap_lbc5
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
void ron200_cap_2spo_cap_lbc5(char * devname, int hi, int lo, int sub, int chuckcon, double area, double per, char tty, char bty, double vacc, double vinv, double freq, double sig, double stray, int integ, int ddebug, double mrange, double lorange, double * ca, double * ta, double * ci, double * ti);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_cap_2spoex_cap
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
void ron200_cap_2spoex_cap(char * devname, int hi, int lo, int sub, int chuckcon, double area, double per, char tty, char bty, double vacc, double vinv, double freq, double sig, double stray, int integ, int ddebug, double mrange, double lorange, double * ca, double * ta, double * ci, double * ti);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_cap_4284_100K_500_nost
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
	END USRLIB MODULE INFORMATION
*/
double ron200_cap_4284_100K_500_nost(int hi, int lo, int subst, double vbias, double * c2, double * z2);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_cap_4284_100K_50_nost
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
	END USRLIB MODULE INFORMATION
*/
double ron200_cap_4284_100K_50_nost(int hi, int lo, int subst, double vbias, double * c2, double * z2);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_cap_4284_100K_50mv
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
#include <cmtr_hp4284.h>

	END USRLIB MODULE INFORMATION
*/
double  ron200_cap_4284_100K_50mv(int, int, int, double, double *, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_cap_4284_100K_50mv_addpin
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 8
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		hi1,	int,	Input,	,	,	
		hi2,	int,	Input,	,	,	
		hi3,	int,	Input,	,	,	
		vbias,	double,	Input,	,	,	
		c2,	double *,	Output,	,	,	
		z2,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <LBC5_proto.h>
#include <cmtr_hp4284.h>
	END USRLIB MODULE INFORMATION
*/
double ron200_cap_4284_100K_50mv_addpin(int hi, int lo, int hi1, int hi2, int hi3, double vbias, double * c2, double * z2);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_cap_4284_100K_50mv_addpin6
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 11
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		hi1,	int,	Input,	,	,	
		hi2,	int,	Input,	,	,	
		hi3,	int,	Input,	,	,	
		hi4,	int,	Input,	,	,	
		hi5,	int,	Input,	,	,	
		hi6,	int,	Input,	,	,	
		vbias,	double,	Input,	,	,	
		c2,	double *,	Output,	,	,	
		z2,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <LBC5_proto.h>
#include <cmtr_hp4284.h>
	END USRLIB MODULE INFORMATION
*/
double ron200_cap_4284_100K_50mv_addpin6(int hi, int lo, int hi1, int hi2, int hi3, int hi4, int hi5, int hi6, double vbias, double * c2, double * z2);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_cap_4284_100K_50mv_addpin7
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 13
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		hi1,	int,	Input,	,	,	
		hi2,	int,	Input,	,	,	
		hi3,	int,	Input,	,	,	
		hi4,	int,	Input,	,	,	
		hi5,	int,	Input,	,	,	
		hi6,	int,	Input,	,	,	
		vbias,	double,	Input,	,	,	
		stra,	double *,	Output,	,	,	
		stat,	int *,	Output,	,	,	
		c2,	double *,	Output,	,	,	
		z2,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <LBC5_proto.h>
#include <cmtr_hp4284.h>
	END USRLIB MODULE INFORMATION
*/
double ron200_cap_4284_100K_50mv_addpin7(int hi, int lo, int hi1, int hi2, int hi3, int hi4, int hi5, int hi6, double vbias, double * stra, int * stat, double * c2, double * z2);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_cap_4284_100K_50mv_ju
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
#include <cmtr_hp4284.h>

	END USRLIB MODULE INFORMATION
*/
double ron200_cap_4284_100K_50mv_ju(int hi, int lo, int subst, double vbias, double * c2, double * z2);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_cap_4284_1_500_nost
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
	END USRLIB MODULE INFORMATION
*/
double ron200_cap_4284_1_500_nost(int hi, int lo, int subst, double vbias, double * c2, double * z2);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_cap_4284_1_50_nost
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
	END USRLIB MODULE INFORMATION
*/
double ron200_cap_4284_1_50_nost(int hi, int lo, int subst, double vbias, double * c2, double * z2);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_cap_4284_1M_500
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
	END USRLIB MODULE INFORMATION
*/
double ron200_cap_4284_1M_500(int hi, int lo, int subst, double vbias, double * c2, double * z2);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_cap_4284_1Mhz_45mv
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
	END USRLIB MODULE INFORMATION
*/
double ron200_cap_4284_1Mhz_45mv(int hi, int lo, int subst, double vbias, double * c2, double * z2);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_cap_4284_1Mhz_500
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
	END USRLIB MODULE INFORMATION
*/
double ron200_cap_4284_1Mhz_500(int hi, int lo, int subst, double vbias, double * c2, double * z2);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_cap_4284_1Mhz_500mv
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
double ron200_cap_4284_1Mhz_500mv(int hi, int lo, int subst, double vbias, double * c2, double * z2);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_cap_4284_1Mhz_500mv_stray
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
double ron200_cap_4284_1Mhz_500mv_stray(int hi, int lo, int subst, double vbias, double * c2, double * z2);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_cap_4284_1Mhz_50mv
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
#include <cmtr_hp4284.h>
	END USRLIB MODULE INFORMATION
*/
double ron200_cap_4284_1Mhz_50mv(int hi, int lo, int subst, double vbias, double * c2, double * z2);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_cap_internal
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
void ron200_cap_internal(int hi, int lo, int sub, double vacc, double * ca, double * ga);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_cap_vcc
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
void ron200_cap_vcc(int hi, int lo, int subst, double v1, double v2, double sdelay, double *v, int npts, double *c, int npts2);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_CAPOFFSET
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
void ron200_CAPOFFSET();


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_ce_hfe
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 11
	ARGUMENTS:
		em,	int,	Input,	,	,	
		base,	int,	Input,	,	,	
		coll,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		vbe,	double,	Input,	,	,	
		vce,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
		delay,	double,	Input,	,	,	
		icout,	double *,	Output,	,	,	
		ibout,	double *,	Output,	,	,	
		beta,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>

	END USRLIB MODULE INFORMATION
*/
void ron200_ce_hfe(int em, int base, int coll, int subst, double vbe, double vce, char type, double delay, double * icout, double * ibout, double * beta);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_ceoswp
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
double ron200_ceoswp(int e, int b, int c, int s, double vcestart, double vcestop, int nstep, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_Chain_cst
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 17
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		t1,	int,	Input,	,	,	
		t2,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		bulk,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		num_contacts,	double,	Input,	,	,	
		area,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
		vforce,	double,	Input,	,	,	
		ilimit,	double,	Input,	,	,	
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
void ron200_Chain_cst(char * devname, int t1, int t2, int sub, int bulk, int chuckcon, double num_contacts, double area, char type, double vforce, double ilimit, double vsub, double delaytime, double intrange, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_Chain_csts
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 17
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		t1,	int,	Input,	,	,	
		t2,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		bulk,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		num_contacts,	double,	Input,	,	,	
		area,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
		vforce,	double,	Input,	,	,	
		ilimit,	double,	Input,	,	,	
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
double ron200_Chain_csts(char * devname, int t1, int t2, int sub, int bulk, int chuckcon, double num_contacts, double area, char type, double vforce, double ilimit, double vsub, double delaytime, double intrange, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_CheckKdf
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
int ron200_CheckKdf(char * filename);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_custom_PrAutoAlign
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
int ron200_custom_PrAutoAlign();


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_custom_PrLoad
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
int ron200_custom_PrLoad();


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_custom_PrProfile
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
int ron200_custom_PrProfile();


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_diode_beta
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
void ron200_diode_beta(int anode, int cathode, int sub, double vcat, double ianode, double delaytime, double * icat, double * isub, double * beta);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_erase_eeprom
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
void ron200_erase_eeprom(int drn, int cg, int src, int tun, double vtun, int te);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_ev
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
void ron200_ev(int e, int b, int c, int s, double ibe, double vstart, double vstop, int npts, double vsub, double * slope, int * iflag, double * r, double * early);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_fblow6_a07
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
double ron200_fblow6_a07(int hi1, int hi2, int lo1, int lo2, double vtest, double wtime, double ilim);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_fblow_smu
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
void ron200_fblow_smu(int hi, int lo, double vbias, int udelay);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_fltCapVds
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
void ron200_fltCapVds(int drain, int hi_gate, int lo_gate, int s1, int s2, int klvs1, int klvs2, int subst, double ids, double vgstart, double vgstop, double udelay, double *Vds1, int npts1, double *Vds2, int npts2);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_fnddat
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
void ron200_fnddat(double *x, int npts, double *y, int npts1, double x1, double x2, double *xnew, int np1, double *ynew, int np2, int * np, char code);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_fndpt_1
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
void ron200_fndpt_1(double *values, int npts, double target, int * j);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_fndtrg
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 2
	ARGUMENTS:
		low,	double,	Input,	,	,	
		high,	double,	Input,	,	,	
	INCLUDES:

	END USRLIB MODULE INFORMATION
*/
int ron200_fndtrg(double low, double high);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_get_site_row_col
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
void ron200_get_site_row_col(float * row, float * col);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_handleControlC
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
void ron200_handleControlC(int sig);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_ibicvbe
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
void ron200_ibicvbe(int e, int b, int c, int sub, double vcb, double ie, double vsub, int avgNum, double tdelay, double * ibe, double * ice, double * vbe, double * beta);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_ibvc
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
double ron200_ibvc(int emitt, int base, int coll, int sub, double Vbe, double * ib);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_ic2
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
void ron200_ic2(int emitt, int base, int col1, int col2, int sub, double vce1, double vce2, double vbe, double * ice1, double * ice2);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_icbo
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
double ron200_icbo(int e, int b, int c, int s, double vcbo, double vsub);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_iceo
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
double ron200_iceo(int e, int b, int c, int sub, double vce, double vsub);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_ices
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
double ron200_ices(int e, int b, int c, int s, double vces, double vsub);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_id1
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
double ron200_id1(int drain, int gate, int source, int subst, double vgs, double vds, double vbs);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_id1_5
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
double ron200_id1_5(int drain, int gate, int source, int body, int subst, double vgs, double vds, double vbs);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_id1_5_dren
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
double  ron200_id1_5_dren(int, int, int, int, int, double, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_id1_a
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
double ron200_id1_a(int drain, int gate, int source, int subst, double vgs, double vds, double vbs);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_id1_dren
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
double  ron200_id1_dren(int, int, int, int, double, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_id1_himp_dren
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 8
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		flt,	int,	Input,	,	,	
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
double  ron200_id1_himp_dren(int, int, int, int, int, double, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_id1_iso
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
double ron200_id1_iso(int drain, int gate, int source, int subst, int isl, double vgs, double vds, double vbs);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_id1_iso_dren
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
double  ron200_id1_iso_dren(int, int, int, int, int, double, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_id1_isosub
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
double ron200_id1_isosub(int drain, int gate, int source, int body, int isl, int subst, double vgs, double vds, double vbs);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_id1_ph
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 8
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		bulk,	int,	Input,	,	,	
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
double ron200_id1_ph(int drain, int gate, int source, int subst, int bulk, double vgs, double vds, double vbs);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_id2
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
double ron200_id2(int drain, int gate, int source, int subst, double vgs, double vds, double vbs);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_id2_a07s
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
double ron200_id2_a07s(int drain, int gate, int source, int subst, double vgs, double vds, double vbs);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_id2_dren
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
double  ron200_id2_dren(int, int, int, int, double, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_id2_k
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
double ron200_id2_k(int drain, int gate, int source, int subst, double vgs, double vds, double vbs);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_id2_kc
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
double ron200_id2_kc(int drain, int gate, int source, int subst, double vgs, double vds, double vbs);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_id2_kc1
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
double ron200_id2_kc1(int drain, int gate, int source, int subst, double vgs, double vds, double vbs);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_id3
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
double ron200_id3(int drain, int gate, int source, int subst, double vgs, double vds, double vbs);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_id3_dren
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
double  ron200_id3_dren(int, int, int, int, double, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_ioff
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
double ron200_ioff(int DRAIN, int GATE, int SOURCE, int SUBST, double VDS);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_ioffis
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
double ron200_ioffis(int DRAIN, int GATE, int SOURCE, int SUBST, int isol, double VDS);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_iso_leak
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 8
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		grnd1,	int,	Input,	,	,	
		grnd2,	int,	Input,	,	,	
		ids,	double,	Input,	,	,	
		vs,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double  ron200_iso_leak(int, int, int, int, int, int, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_iso_leak_bck
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 9
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		back,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		grnd1,	int,	Input,	,	,	
		grnd2,	int,	Input,	,	,	
		ibs,	double,	Input,	,	,	
		vd,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double  ron200_iso_leak_bck(int, int, int, int, int, int, int, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_isub
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
double ron200_isub(int DRAIN, int GATE, int SOURCE, int SUBST, double VGS, double VDS, double VBS);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_isub5
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
double ron200_isub5(int DRAIN, int GATE, int SOURCE, int BODY, int SUBST, double VGS, double VDS, double VBS);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_isub_iso
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
double ron200_isub_iso(int DRAIN, int GATE, int SOURCE, int SUBST, int ISL, double VGS, double VDS, double VBS);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_isub_max_philips
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
		vgstart,	double,	Input,	0,	,	
		vgstop,	double,	Input,	10,	,	
		vds,	double,	Input,	1.0,	,	
		vbulk,	double,	Input,	0,	,	
		icomp,	double,	Input,	1e-06,	,	
		sdelay,	double,	Input,	0.003,	,	
		intrange,	double,	Input,	1,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		isubmax,	double *,	Output,	,	,	
		vgmax,	double *,	Output,	,	,	
		idmax,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#define MAXPTS 100
	END USRLIB MODULE INFORMATION
*/
void ron200_isub_max_philips(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vgstart, double vgstop, double vds, double vbulk, double icomp, double sdelay, double intrange, double mrange, double lorange, double * isubmax, double * vgmax, double * idmax);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_isubmax_ph
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 18
	ARGUMENTS:
		gate,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		bulk,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		width,	double,	Input,	,	,	
		length,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
		vgstart,	double,	Input,	0,	,	
		vgstop,	double,	Input,	10,	,	
		vds,	double,	Input,	1.0,	,	
		vbulk,	double,	Input,	0,	,	
		icomp,	double,	Input,	1e-06,	,	
		sdelay,	double,	Input,	0.003,	,	
		intrange,	double,	Input,	1,	,	
		isubmax,	double *,	Output,	,	,	
		ib_id,	double *,	Output,	,	,	
		vgmax,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#define MAXPTS 100
	END USRLIB MODULE INFORMATION
*/
void ron200_isubmax_ph(int gate, int drain, int source, int bulk, int sub, double width, double length, char type, double vgstart, double vgstop, double vds, double vbulk, double icomp, double sdelay, double intrange, double * isubmax, double * ib_id, double * vgmax);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_isubmax_ph_cf
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 18
	ARGUMENTS:
		gate,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		bulk,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		width,	double,	Input,	,	,	
		length,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
		vgstart,	double,	Input,	0,	,	
		vgstop,	double,	Input,	10,	,	
		vds,	double,	Input,	1.0,	,	
		vbulk,	double,	Input,	0,	,	
		icomp,	double,	Input,	1e-06,	,	
		sdelay,	double,	Input,	0.003,	,	
		intrange,	double,	Input,	1,	,	
		isubmax,	double *,	Output,	,	,	
		ib_id,	double *,	Output,	,	,	
		vgmax,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#define MAXPTS 100
	END USRLIB MODULE INFORMATION
*/
void ron200_isubmax_ph_cf(int gate, int drain, int source, int bulk, int sub, double width, double length, char type, double vgstart, double vgstop, double vds, double vbulk, double icomp, double sdelay, double intrange, double * isubmax, double * ib_id, double * vgmax);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_isubmx
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 11
	ARGUMENTS:
		d,	int,	Input,	,	,	
		g,	int,	Input,	,	,	
		s,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vbs,	double,	Input,	,	,	
		vlow,	double,	Input,	,	,	
		vhigh,	double,	Input,	,	,	
		npts,	int,	Input,	,	,	
		ismax,	double *,	Output,	,	,	
		vgmax,	double *,	Output,	,	,	
	INCLUDES:
#include <math.h>
#include "PARLIB400_proto.h"
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <stdlib.h>

#define MAXPTS 100		                                   
#define CRTLIM 10.E-3		                         
#define ICOMP 0.98*CRTLIM	                        
	END USRLIB MODULE INFORMATION
*/
void ron200_isubmx(int d, int g, int s, int sub, double vds, double vbs, double vlow, double vhigh, int npts, double * ismax, double * vgmax);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_jsub_lpnp
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 7
	ARGUMENTS:
		em,	int,	Input,	,	,	
		ba,	int,	Input,	,	,	
		col,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		vc,	double,	Input,	,	,	
		vsub,	double,	Input,	,	,	
		ib,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double ron200_jsub_lpnp(int em, int ba, int col, int subst, double vc, double vsub, double ib);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_kdelay
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
void ron200_kdelay(int npin, double i, double v);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_lambda
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
double ron200_lambda(int drain, int gate, int source, int subst, double vstart, double vstop, double vgs, double vsub, int npts, double * slope, int * kflag, double * r);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_lambda_iso
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
double ron200_lambda_iso(int drain, int gate, int source, int subst, int isl, double vstart, double vstop, double vgs, double vsub, int npts, double * slope, int * kflag, double * r);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_ld300_ph
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 9
	ARGUMENTS:
		src,	int,	Input,	,	,	
		gat,	int,	Input,	,	,	
		drn,	int,	Input,	,	,	
		wel,	int,	Input,	,	,	
		type,	char,	Input,	,	,	
		vgs,	double,	Input,	,	,	
		vbs,	double,	Input,	,	,	
		vmax,	double,	Input,	,	,	
		ids,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double ron200_ld300_ph(int src, int gat, int drn, int wel, char type, double vgs, double vbs, double vmax, double ids);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_ld300_phb
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 10
	ARGUMENTS:
		src,	int,	Input,	,	,	
		gat,	int,	Input,	,	,	
		drn,	int,	Input,	,	,	
		wel,	int,	Input,	,	,	
		bulk,	int,	Input,	,	,	
		type,	char,	Input,	,	,	
		vgs,	double,	Input,	,	,	
		vbs,	double,	Input,	,	,	
		vmax,	double,	Input,	,	,	
		ids,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double ron200_ld300_phb(int src, int gat, int drn, int wel, int bulk, char type, double vgs, double vbs, double vmax, double ids);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_ld300_phg
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 10
	ARGUMENTS:
		src,	int,	Input,	,	,	
		gat,	int,	Input,	,	,	
		drn,	int,	Input,	,	,	
		wel,	int,	Input,	,	,	
		gnd,	int,	Input,	,	,	
		type,	char,	Input,	,	,	
		vgs,	double,	Input,	,	,	
		vbs,	double,	Input,	,	,	
		vmax,	double,	Input,	,	,	
		ids,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double ron200_ld300_phg(int src, int gat, int drn, int wel, int gnd, char type, double vgs, double vbs, double vmax, double ids);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_le200c
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 12
	ARGUMENTS:
		source,	int,	Input,	,	,	
		gate1,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		gate2,	int,	Input,	,	,	
		sign,	int,	Input,	,	,	
		width,	float,	Input,	,	,	
		i_force,	float,	Input,	,	,	
		vbs,	float,	Input,	,	,	
		vmax,	float,	Input,	,	,	
		param,	F_ARRAY_T,	Output,	,	,	
		param_size,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void ron200_le200c(int source, int gate1, int drain, int sub, int gate2, int sign, float width, float i_force, float vbs, float vmax, float *param, int param_size);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_le200c_ahn
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 6
	ARGUMENTS:
		lo,	int,	Input,	,	,	
		hi,	int,	Input,	,	,	
		hi2,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		itest,	double,	Input,	,	,	
		VLTLIM,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double ron200_le200c_ahn(int lo, int hi, int hi2, int subst, double itest, double VLTLIM);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_leak
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
double ron200_leak(int hi, int lo, int subst, double v, double ilim);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_leak1
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
double ron200_leak1(int hi, int lo, int subst, double v);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_leak1_2hi
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
double ron200_leak1_2hi(int hi1, int hi2, int lo, int subst, double v);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_leak1_2hi_dren
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
double  ron200_leak1_2hi_dren(int, int, int, int, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_leak1_dren
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
double  ron200_leak1_dren(int, int, int, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_leak1_float14
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
double ron200_leak1_float14(int hi, int lo, int subst, int d1, int d2, int d3, int d4, int d5, int d6, int d7, int d8, int d9, int d10, int d11, int d12, int d13, int d14, double v);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_leak1_float14_dren
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
double  ron200_leak1_float14_dren(int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_leak1_high_imp
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
double ron200_leak1_high_imp(int hi, int lo, int subst, int dummy1, double v);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_leak1_high_imp2
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
double ron200_leak1_high_imp2(int hi, int lo, int subst, int dummy1, int dummy2, double v);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_leak1_kj
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
double ron200_leak1_kj(int hi, int lo, int subst, double v);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_leak4
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
double ron200_leak4(int hi, int lo1, int lo2, int subst, double v);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_leak4_dren
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
double  ron200_leak4_dren(int, int, int, int, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_leak4_high_imp
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
double ron200_leak4_high_imp(int hi, int lo1, int lo2, int subst, int dummy1, double v);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_leak4_lc
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
double ron200_leak4_lc(int hi, int lo1, int lo2, int subst, double v, double ilim);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_leak4_lc1
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
double ron200_leak4_lc1(int hi, int hi2, int lo1, int lo2, int subst, double v, double ilim);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_leak4_lc2
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
double ron200_leak4_lc2(int hi, int lo1, int lo2, int subst, double v, double ilim);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_leak4_lc3
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
double ron200_leak4_lc3(int hi, int lo1, int lo2, int subst, double v, double ilim);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_leak4_lc_a07
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
double ron200_leak4_lc_a07(int hi1, int hi2, int lo1, int subst, double v, double ilim, int d1, int d2, int d3, int d4, int d5, int d6, int d7, int d8, int d9, int d10, int d11, int d12, int d13, int d14, int d15, int d16);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_leak4_lc_a071
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
double ron200_leak4_lc_a071(int hi1, int hi2, int lo1, int subst, double v, double ilim, int d1, int d2, int d3, int d4, int d5, int d6, int d7, int d8, int d9, int d10, int d11, int d12, int d13, int d14, int d15, int d16);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_leak4_lc_a072
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
double ron200_leak4_lc_a072(int hi1, int hi2, int lo1, int subst, double v, double ilim, int d1, int d2, int d3, int d4, int d5, int d6, int d7, int d8, int d9, int d10, int d11, int d12, int d13, int d14, int d15, int d16);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_leak4_lc_a07_hb
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
double  ron200_leak4_lc_a07_hb(int, int, int, int, double, double, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int);

/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_leak4_lc_a07_swp
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 24
	ARGUMENTS:
		hi1,	int,	Input,	,	,	
		hi2,	int,	Input,	,	,	
		lo1,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		v,	double,	Input,	,	,	
		ilim,	double,	Input,	,	,	
		fltpin_ch,	int,	Input,	,	,	
		delaytime,	double,	Input,	,	,	
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
#include <par_util.h>
#include <stdlib.h>
#include <ktxe_proto.h>
#include <ksox_def.h>
#include <kui_proto.h>

#define MAX(a,b)        ((a)>(b) ? (a) : (b))
#define CDELAY  125.E-12
#define ILEAK   20.E-12
	END USRLIB MODULE INFORMATION
*/
double ron200_leak4_lc_a07_swp(int hi1, int hi2, int lo1, int subst, double v, double ilim, int fltpin_ch, double delaytime, int d1, int d2, int d3, int d4, int d5, int d6, int d7, int d8, int d9, int d10, int d11, int d12, int d13, int d14, int d15, int d16);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_leak4_lc_a07k
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
double ron200_leak4_lc_a07k(int hi1, int hi2, int lo1, int subst, double v, double ilim, int d1, int d2, int d3, int d4, int d5, int d6, int d7, int d8, int d9, int d10, int d11, int d12, int d13, int d14, int d15, int d16);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_leak4_lc_a07s
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 23
	ARGUMENTS:
		hi1,	int,	Input,	,	,	
		hi2,	int,	Input,	,	,	
		lo1,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		v,	double,	Input,	,	,	
		ilim,	double,	Input,	,	,	
		n,	int,	Input,	,	,	
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
double ron200_leak4_lc_a07s(int hi1, int hi2, int lo1, int subst, double v, double ilim, int n, int d1, int d2, int d3, int d4, int d5, int d6, int d7, int d8, int d9, int d10, int d11, int d12, int d13, int d14, int d15, int d16);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_leak4_lc_comb1
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
double ron200_leak4_lc_comb1(int hi1, int hi2, int lo1, int subst, double v, double ilim, int d1, int d2, int d3, int d4, int d5, int d6, int d7, int d8, int d9, int d10, int d11, int d12, int d13, int d14, int d15, int d16);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_leak4_lc_comb2
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
double ron200_leak4_lc_comb2(int hi1, int hi2, int lo1, int subst, double v, double ilim, int d1, int d2, int d3, int d4, int d5, int d6, int d7, int d8, int d9, int d10, int d11, int d12, int d13, int d14, int d15, int d16);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_leak4_lc_comb3
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
double ron200_leak4_lc_comb3(int hi1, int hi2, int lo1, int subst, double v, double ilim, int d1, int d2, int d3, int d4, int d5, int d6, int d7, int d8, int d9, int d10, int d11, int d12, int d13, int d14, int d15, int d16);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_leak4_lc_comb31
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
double ron200_leak4_lc_comb31(int hi1, int hi2, int lo1, int subst, double v, double ilim, int d1, int d2, int d3, int d4, int d5, int d6, int d7, int d8, int d9, int d10, int d11, int d12, int d13, int d14, int d15, int d16);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_leak4_lc_dren
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
double  ron200_leak4_lc_dren(int, int, int, int, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_leak4_lc_lrng
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
double ron200_leak4_lc_lrng(int hi, int lo1, int lo2, int subst, double v, double ilim);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_leak4_lj_a07
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
double ron200_leak4_lj_a07(int hi1, int hi2, int lo1, int subst, double v, double ilim, int d1, int d2, int d3, int d4, int d5, int d6, int d7, int d8, int d9, int d10, int d11, int d12, int d13, int d14, int d15, int d16);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_leak4_lk_a07
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
double ron200_leak4_lk_a07(int hi1, int hi2, int lo1, int subst, double v, double ilim, int d1, int d2, int d3, int d4, int d5, int d6, int d7, int d8, int d9, int d10, int d11, int d12, int d13, int d14, int d15, int d16);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_leak4_lm_a07
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
double ron200_leak4_lm_a07(int hi1, int hi2, int lo1, int subst, double v, double ilim, int d1, int d2, int d3, int d4, int d5, int d6, int d7, int d8, int d9, int d10, int d11, int d12, int d13, int d14, int d15, int d16);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_leak_4ph
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 7
	ARGUMENTS:
		hi1,	int,	Input,	,	,	
		hi2,	int,	Input,	,	,	
		lo1,	int,	Input,	,	,	
		lo2,	int,	Input,	,	,	
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
double ron200_leak_4ph(int hi1, int hi2, int lo1, int lo2, double v, double ilim, double area);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_leak_dren
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
double  ron200_leak_dren(int, int, int, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_leak_float14
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
double ron200_leak_float14(int hi, int lo, int d1, int d2, int d3, int d4, int d5, int d6, int d7, int d8, int d9, int d10, int d11, int d12, int d13, int d14, double v);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_leak_float14_dren
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
double  ron200_leak_float14_dren(int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_leak_float_dren
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 6
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo1,	int,	Input,	,	,	
		lo2,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		flt,	int,	Input,	,	,	
		v,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double  ron200_leak_float_dren(int, int, int, int, int, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_leak_iso
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
double ron200_leak_iso(int hi, int lo1, int lo2, int subst, int isl, double v);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_leak_iso_dren
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
double  ron200_leak_iso_dren(int, int, int, int, int, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_leak_ph
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
double ron200_leak_ph(int hi, int lo, int subst, double v, double ilim, double area);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_leakpa5
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
double ron200_leakpa5(int HI1, int LO1, int LO2, int SUBST, int pin5, double V1);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_leakpag
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
double ron200_leakpag(int HI1, int HI2, int LO1, int SUBST, double vtest1, double vtest2);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_lg130c
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 27
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
		imin,	double,	Input,	,	,	
		imax,	double,	Input,	,	,	
		istop,	double,	Input,	,	,	
		imult,	double,	Input,	,	,	
		hold,	double,	Input,	,	,	
		stepdelay,	double,	Input,	,	,	
		vmax,	double,	Input,	,	,	
		ratio,	double,	Input,	,	,	
		m_i,	double,	Input,	,	,	
		m_q,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
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
	END USRLIB MODULE INFORMATION
*/
void ron200_lg130c(char * devname, int hi, int lo, int sub, int chuckcon, double area, double per, char tty, char bty, double imin, double imax, double istop, double imult, double hold, double stepdelay, double vmax, double ratio, double m_i, double m_q, double intrange, double mrange, double lorange, double * vlf, double * vhf, double * vfail, double * ifail, double * qbd);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_lg130c_f
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 23
	ARGUMENTS:
		hi_pin,	int,	Input,	,	,	
		lo_pin,	int,	Input,	,	,	
		jun_pin,	int,	Input,	,	,	
		extra_pins,	char *,	Input,	,	,	
		select,	int,	Input,	,	,	
		jmax,	double,	Input,	,	,	
		vopmax,	double,	Input,	,	,	
		ehigh,	double,	Input,	,	,	
		vjun,	double,	Input,	,	,	
		total_step_time,	double,	Input,	,	,	
		total_time,	double,	Input,	,	,	
		area,	double,	Input,	,	,	
		tox,	double,	Input,	,	,	
		c_extra,	double,	Input,	,	,	
		criterium,	double,	Input,	,	,	
		pin_array1,	I_ARRAY_T,	Input,	,	,	
		pin_array1_size,	int,	Input,	,	,	
		pin_array2,	I_ARRAY_T,	Input,	,	,	
		pin_array2_size,	int,	Input,	,	,	
		meas_mode,	char,	Input,	,	,	
		tid,	int *,	Output,	,	,	
		param,	F_ARRAY_T,	Output,	,	,	
		param_size,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <time.h>
#include <phillib.h>
#include <system_config.h>
	END USRLIB MODULE INFORMATION
*/
void ron200_lg130c_f(int hi_pin, int lo_pin, int jun_pin, char * extra_pins, int select, double jmax, double vopmax, double ehigh, double vjun, double total_step_time, double total_time, double area, double tox, double c_extra, double criterium, int *pin_array1, int pin_array1_size, int *pin_array2, int pin_array2_size, char meas_mode, int * tid, float *param, int param_size);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_lg130c_fc
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 15
	ARGUMENTS:
		hi_pin,	int,	Input,	,	,	
		lo_pin,	int,	Input,	,	,	
		select,	int,	Input,	,	,	
		jmax,	double,	Input,	,	,	
		vopmax,	double,	Input,	,	,	
		ehigh,	double,	Input,	,	,	
		total_step_time,	double,	Input,	,	,	
		total_time,	double,	Input,	,	,	
		area,	double,	Input,	,	,	
		tox,	double,	Input,	,	,	
		c_extra,	double,	Input,	,	,	
		criterium,	double,	Input,	,	,	
		meas_mode,	char,	Input,	,	,	
		param,	F_ARRAY_T,	Output,	,	,	
		param_size,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <time.h>
#include <phillib.h>
#include <system_config.h>
	END USRLIB MODULE INFORMATION
*/
void ron200_lg130c_fc(int hi_pin, int lo_pin, int select, double jmax, double vopmax, double ehigh, double total_step_time, double total_time, double area, double tox, double c_extra, double criterium, char meas_mode, float *param, int param_size);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_lg130c_fg
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 17
	ARGUMENTS:
		hi_pin,	int,	Input,	,	,	
		lo_pin,	int,	Input,	,	,	
		hi_pin2,	int,	Input,	,	,	
		lo_pin2,	int,	Input,	,	,	
		select,	int,	Input,	,	,	
		jmax,	double,	Input,	,	,	
		vopmax,	double,	Input,	,	,	
		ehigh,	double,	Input,	,	,	
		total_step_time,	double,	Input,	,	,	
		total_time,	double,	Input,	,	,	
		area,	double,	Input,	,	,	
		tox,	double,	Input,	,	,	
		c_extra,	double,	Input,	,	,	
		criterium,	double,	Input,	,	,	
		meas_mode,	char,	Input,	,	,	
		param,	F_ARRAY_T,	Output,	,	,	
		param_size,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <time.h>
#include <phillib.h>
#include <system_config.h>
	END USRLIB MODULE INFORMATION
*/
void ron200_lg130c_fg(int hi_pin, int lo_pin, int hi_pin2, int lo_pin2, int select, double jmax, double vopmax, double ehigh, double total_step_time, double total_time, double area, double tox, double c_extra, double criterium, char meas_mode, float *param, int param_size);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_li100c
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 15
	ARGUMENTS:
		beta_in,	F_ARRAY_T,	Input,	,	,	
		beta_len,	int,	Input,	,	,	
		w_in,	F_ARRAY_T,	Input,	,	,	
		w_len,	int,	Input,	,	,	
		l_in,	F_ARRAY_T,	Input,	,	,	
		l_len,	int,	Input,	,	,	
		dim,	int,	Input,	,	,	
		minlim,	double,	Input,	,	,	
		maxlim,	double,	Input,	,	,	
		wmin,	double,	Input,	,	,	
		lmin,	double,	Input,	,	,	
		meas_mode,	char,	Input,	,	,	
		TID,	int *,	Output,	,	,	
		param,	F_ARRAY_T,	Output,	,	,	
		param_size,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void ron200_li100c(float *beta_in, int beta_len, float *w_in, int w_len, float *l_in, int l_len, int dim, double minlim, double maxlim, double wmin, double lmin, char meas_mode, int * TID, float *param, int param_size);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_li300c
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 10
	ARGUMENTS:
		linx_in,	F_ARRAY_T,	Input,	,	,	
		linx_len,	int,	Input,	,	,	
		liny_in,	F_ARRAY_T,	Input,	,	,	
		liny_len,	int,	Input,	,	,	
		dim,	int,	Input,	,	,	
		func,	int,	Input,	,	,	
		meas_mode,	char,	Input,	,	,	
		TID,	int *,	Output,	,	,	
		param,	F_ARRAY_T,	Output,	,	,	
		param_size,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void ron200_li300c(float *linx_in, int linx_len, float *liny_in, int liny_len, int dim, int func, char meas_mode, int * TID, float *param, int param_size);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_li500c
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 9
	ARGUMENTS:
		res1,	F_ARRAY_T,	Input,	,	,	
		res1_len,	int,	Input,	,	,	
		res2,	F_ARRAY_T,	Input,	,	,	
		res2_len,	int,	Input,	,	,	
		select,	int,	Input,	,	,	
		meas_mode,	char,	Input,	,	,	
		TID,	int *,	Output,	,	,	
		param,	F_ARRAY_T,	Output,	,	,	
		param_size,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void ron200_li500c(float *res1, int res1_len, float *res2, int res2_len, int select, char meas_mode, int * TID, float *param, int param_size);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_li500c_f
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 4
	ARGUMENTS:
		RES1,	double *,	Input,	,	,	
		RES2,	double *,	Input,	,	,	
		SELECT,	int,	Input,	,	,	
		PARAM,	double *,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void ron200_li500c_f(double * RES1, double * RES2, int SELECT, double * PARAM);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_logstp
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
int ron200_logstp(double xstart, double xstop, float *steps, int npts);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_num_pts_array
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
void ron200_num_pts_array(double num_low, double num_hi, double *arr_out, int npts);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_Pcap_4284_100K_50mv
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 12
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		vbias,	int,	Input,	,	,	
		c2,	F_ARRAY_T,	Output,	,	,	
		c2_size,	int,	Input,	,	,	
		Forv,	F_ARRAY_T,	Output,	,	,	
		Forv_size,	int,	Input,	,	,	
		z2,	F_ARRAY_T,	Output,	,	,	
		z2_size,	int,	Input,	,	,	
		Tox,	F_ARRAY_T,	Output,	,	,	
		Tox_size,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <LBC5_proto.h>
#include <cmtr_hp4284.h>
	END USRLIB MODULE INFORMATION
*/
void ron200_Pcap_4284_100K_50mv(int hi, int lo, int subst, int vbias, float *c2, int c2_size, float *Forv, int Forv_size, float *z2, int z2_size, float *Tox, int Tox_size);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_PF010
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 11
	ARGUMENTS:
		PinString,	char *,	Input,	,	,	
		Pins,	I_ARRAY_T,	Input,	,	,	
		Nr_Pins,	int,	Input,	,	,	
		Type,	char *,	Input,	,	,	
		Flags,	char *,	Input,	,	,	
		Vdrai,	float,	Input,	,	,	
		Vgate,	float,	Input,	,	,	
		Vsour,	float,	Input,	,	,	
		Vwell,	float,	Input,	,	,	
		Params,	F_ARRAY_T,	Output,	,	,	
		Params_len,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <time.h>
#include <phillib.h>
#include <system_config.h>
	END USRLIB MODULE INFORMATION
*/
void ron200_PF010(char * PinString, int *Pins, int Nr_Pins, char * Type, char * Flags, float Vdrai, float Vgate, float Vsour, float Vwell, float *Params, int Params_len);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_PF010_ex
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 11
	ARGUMENTS:
		PinString,	char *,	Input,	,	,	
		Pins,	I_ARRAY_T,	Input,	,	,	
		Nr_Pins,	int,	Input,	,	,	
		Type,	char *,	Input,	,	,	
		Flags,	char *,	Input,	,	,	
		Vdrai,	float,	Input,	,	,	
		Vgate,	float,	Input,	,	,	
		Vsour,	float,	Input,	,	,	
		Vwell,	float,	Input,	,	,	
		Params,	F_ARRAY_T,	Output,	,	,	
		Params_len,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <time.h>
#include <phillib.h>
#include <system_config.h>
	END USRLIB MODULE INFORMATION
*/
void ron200_PF010_ex(char * PinString, int *Pins, int Nr_Pins, char * Type, char * Flags, float Vdrai, float Vgate, float Vsour, float Vwell, float *Params, int Params_len);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_PF010_Id
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 13
	ARGUMENTS:
		PinString,	char *,	Input,	,	,	
		Pins,	I_ARRAY_T,	Input,	,	,	
		Nr_Pins,	int,	Input,	,	,	
		Type,	char *,	Input,	,	,	
		Flags,	char *,	Input,	,	,	
		Vdrai,	float,	Input,	,	,	
		Vgate,	float,	Input,	,	,	
		Vsour,	float,	Input,	,	,	
		Vwell,	float,	Input,	,	,	
		W,	float,	Input,	,	,	
		L,	float,	Input,	,	,	
		Params,	F_ARRAY_T,	Output,	,	,	
		Params_len,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <time.h>
#include <phillib.h>
#include <system_config.h>
	END USRLIB MODULE INFORMATION
*/
void ron200_PF010_Id(char * PinString, int *Pins, int Nr_Pins, char * Type, char * Flags, float Vdrai, float Vgate, float Vsour, float Vwell, float W, float L, float *Params, int Params_len);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_PF100
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 10
	ARGUMENTS:
		PinString,	char *,	Input,	,	,	
		Pins,	I_ARRAY_T,	Input,	,	,	
		Nr_Pins,	int,	Input,	,	,	
		Type,	char *,	Input,	,	,	
		Flags,	char *,	Input,	,	,	
		Vd,	float,	Input,	,	,	
		W,	float,	Input,	,	,	
		L,	float,	Input,	,	,	
		Params,	F_ARRAY_T,	Output,	,	,	
		Params_len,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <time.h>
#include <phillib.h>
#include <system_config.h>
	END USRLIB MODULE INFORMATION
*/
void ron200_PF100(char * PinString, int *Pins, int Nr_Pins, char * Type, char * Flags, float Vd, float W, float L, float *Params, int Params_len);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_PF100_ex
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 10
	ARGUMENTS:
		PinString,	char *,	Input,	,	,	
		Pins,	I_ARRAY_T,	Input,	,	,	
		Nr_Pins,	int,	Input,	,	,	
		Type,	char *,	Input,	,	,	
		Flags,	char *,	Input,	,	,	
		Vd,	float,	Input,	,	,	
		W,	float,	Input,	,	,	
		L,	float,	Input,	,	,	
		Params,	F_ARRAY_T,	Output,	,	,	
		Params_len,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <time.h>
#include <phillib.h>
#include <system_config.h>
	END USRLIB MODULE INFORMATION
*/
void ron200_PF100_ex(char * PinString, int *Pins, int Nr_Pins, char * Type, char * Flags, float Vd, float W, float L, float *Params, int Params_len);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_pgm_eeprom
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
void ron200_pgm_eeprom(int drn, int cg, int src, int tun, double vcg, int tp);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_pgm_eeprom_floating_gate
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
void ron200_pgm_eeprom_floating_gate(int drn, int src, int well, int sub, double vd, double td);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_pn2hiswp
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
double ron200_pn2hiswp(int hi1, int hi2, int lo1, int lo2, int lo3, int lo4, double vstart, double vstop, int nstep, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_pn2swp
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
double ron200_pn2swp(int hi, int lo, int subst, double vstart, double vstop, int nstep, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_pn2swp_5
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
double ron200_pn2swp_5(int hi, int lo1, int lo2, int lo3, int subst, double vstart, double vstop, int nstep, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_pn2swp_bef
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
double ron200_pn2swp_bef(int hi, int lo, int subst, double vstart, double vstop, int nstep, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_pn2swp_bs
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
double ron200_pn2swp_bs(int hi, int lo, int subst, double vstart, double vstop, int nstep, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_pn2swp_bs1
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
double ron200_pn2swp_bs1(int hi, int lo, int subst, double vstart, double vstop, int nstep, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_pn2swp_bs2
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
double ron200_pn2swp_bs2(int hi, int lo, int subst, double vstart, double vstop, int nstep, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_pn2swp_dren
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
double  ron200_pn2swp_dren(int, int, int, double, double, int, double, double, char);

/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_pn2swp_goi
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 9
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		vmin,	double,	Input,	,	,	
		vmax,	double,	Input,	,	,	
		step,	double,	Input,	,	,	
		itest,	double,	Input,	,	,	
		wait,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double ron200_pn2swp_goi(int hi, int lo, int subst, double vmin, double vmax, double step, double itest, double wait, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_pn2swp_himp
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
double ron200_pn2swp_himp(int hi, int lo, int subst, double vstart, double vstop, int nstep, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_pn2swp_miho
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 9
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		vmin,	double,	Input,	,	,	
		vmax,	double,	Input,	,	,	
		step,	double,	Input,	,	,	
		itest,	double,	Input,	,	,	
		wait,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double ron200_pn2swp_miho(int hi, int lo, int subst, double vmin, double vmax, double step, double itest, double wait, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_pn2swp_miho1
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 23
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		vmin,	double,	Input,	,	,	
		vmax,	double,	Input,	,	,	
		step,	double,	Input,	,	,	
		itest,	double,	Input,	,	,	
		wait,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
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
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double ron200_pn2swp_miho1(int hi, int lo, int subst, double vmin, double vmax, double step, double itest, double wait, char type, int d1, int d2, int d3, int d4, int d5, int d6, int d7, int d8, int d9, int d10, int d11, int d12, int d13, int d14);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_pn2swp_miho2
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 9
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		vmin,	double,	Input,	,	,	
		vmax,	double,	Input,	,	,	
		step,	double,	Input,	,	,	
		itest,	double,	Input,	,	,	
		wait,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double ron200_pn2swp_miho2(int hi, int lo, int subst, double vmin, double vmax, double step, double itest, double wait, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_pn2swp_miho_sam
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 10
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		vmin,	double,	Input,	,	,	
		vmax,	double,	Input,	,	,	
		vsub,	double,	Input,	,	,	
		step,	double,	Input,	,	,	
		itest,	double,	Input,	,	,	
		wait,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double ron200_pn2swp_miho_sam(int hi, int lo, int subst, double vmin, double vmax, double vsub, double step, double itest, double wait, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_pn2swp_mihoj
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 9
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		vmin,	double,	Input,	,	,	
		vmax,	double,	Input,	,	,	
		step,	double,	Input,	,	,	
		itest,	double,	Input,	,	,	
		wait,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double ron200_pn2swp_mihoj(int hi, int lo, int subst, double vmin, double vmax, double step, double itest, double wait, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_pn2swp_old
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
double ron200_pn2swp_old(int hi, int lo, int subst, double vstart, double vstop, int nstep, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_pn2swp_ph
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 10
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		type,	char,	Input,	,	,	
		vstart,	double,	Input,	,	,	
		vstop,	double,	Input,	,	,	
		nstep,	int,	Input,	,	,	
		JBD_LIM,	double,	Input,	,	,	
		area,	double,	Input,	,	,	
		udelay,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double ron200_pn2swp_ph(int hi, int lo, int subst, char type, double vstart, double vstop, int nstep, double JBD_LIM, double area, double udelay);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_pn2swp_sea
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
double ron200_pn2swp_sea(int hi, int lo, int subst, double vstart, double vstop, int nstep, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_pn2swp_swp
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
	END USRLIB MODULE INFORMATION
*/
double ron200_pn2swp_swp(int hi, int lo, int subst, double vstart, double vstop, int nstep, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_pn4hiswp
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
double ron200_pn4hiswp(int hi1, int hi2, int hi3, int hi4, int lo, double vstart, double vstop, int nstep, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_pn5hiswp
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
double ron200_pn5hiswp(int hi1, int hi2, int hi3, int hi4, int hi5, int lo, double vstart, double vstop, int nstep, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_pn5swpgv
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
double ron200_pn5swpgv(int hi, int hi1, int lo1, int lo2, int subst, double vstart, double vstop, double vfrc, int nstep, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_pulse_custom
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
void ron200_pulse_custom(int hi_pin, int low_pin, int ground_pin1, int ground_pin2, double pulse_top, double pulse_bottom, double pulse_time, double *current_array, int num_pulses);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_PX010
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 4
	ARGUMENTS:
		input1,	float,	Input,	,	,	
		input2,	float,	Input,	,	,	
		output,	float *,	Output,	,	,	
		action,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <phillib.h>
	END USRLIB MODULE INFORMATION
*/
void ron200_PX010(float input1, float input2, float * output, int action);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_PX100
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 8
	ARGUMENTS:
		Res1,	F_ARRAY_T,	Input,	,	,	
		Nr_Res1,	int,	Input,	,	,	
		Res2,	F_ARRAY_T,	Input,	,	,	
		Nr_Res2,	int,	Input,	,	,	
		Type,	char *,	Input,	,	,	
		Flags,	char *,	Input,	,	,	
		Params,	F_ARRAY_T,	Output,	,	,	
		Params_len,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <time.h>
#include <system_config.h>
#include <phillib.h>
	END USRLIB MODULE INFORMATION
*/
void ron200_PX100(float *Res1, int Nr_Res1, float *Res2, int Nr_Res2, char * Type, char * Flags, float *Params, int Params_len);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_r2pvs6_a07s
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 10
	ARGUMENTS:
		hi1,	int,	Input,	,	,	
		hi2,	int,	Input,	,	,	
		lo1,	int,	Input,	,	,	
		lo2,	int,	Input,	,	,	
		v,	double,	Input,	,	,	
		imax,	double,	Input,	,	,	
		vbb,	double,	Input,	,	,	
		dvb,	double,	Input,	,	,	
		irange,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
	INCLUDES:
#include <math.h>
#include "lptdef.h"

#define CRTLIM 100.0E-3

	END USRLIB MODULE INFORMATION
*/
double ron200_r2pvs6_a07s(int hi1, int hi2, int lo1, int lo2, double v, double imax, double vbb, double dvb, double irange, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_r2pvsa6_a07
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
double ron200_r2pvsa6_a07(int his, int him, int los, int lom, int subst, double vbb, double vtest, double dvb);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_res
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
double ron200_res(int hi, int lo, int subst, double itest);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_res4
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
double ron200_res4(int hi_source, int hi_meas, int lo_source, int lo_meas, int subst, double itest);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_res4_a07
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
double ron200_res4_a07(int hi_source, int hi_meas, int lo_source, int lo_meas, int subst, double itest);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_res4_ph
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


	END USRLIB MODULE INFORMATION
*/
double ron200_res4_ph(int hi_source, int hi_meas, int lo_source, int lo_meas, int subst, double itest, double vcomp, double delaytime);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_res4_phg
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


	END USRLIB MODULE INFORMATION
*/
double ron200_res4_phg(int hi_source, int hi_meas, int lo_source, int lo_meas, int subst, double itest, double vcomp, double delaytime);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_res4b
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
double ron200_res4b(int his, int him, int los, int lom, int subst, double itest, double vsub);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_res4v
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
double ron200_res4v(int his, int him, int los, int lom, int subst, double vtest);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_res_de500_ra20
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
double ron200_res_de500_ra20(int hi, int lo, int subst, double itest);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_res_delay_500
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
double ron200_res_delay_500(int hi, int lo, int subst, double itest);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_res_j
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
double ron200_res_j(int hi, int lo, int subst, int fpin1, int fpin2, double dlytime, double itest);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_res_org
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
double ron200_res_org(int hi, int lo, int subst, double itest);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_res_ph
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
double ron200_res_ph(int hi, int lo, int subst, double itest, double vcomp, double delaytime);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_res_ph1
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
double ron200_res_ph1(int hi, int lo, int subst, double itest, double vcomp, double delaytime);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_res_phg
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 7
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		itest,	double,	Input,	,	,	
		vcomp,	double,	Input,	,	,	
		delaytime,	double,	Input,	,	,	
		igate,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double ron200_res_phg(int hi, int lo, int subst, double itest, double vcomp, double delaytime, double * igate);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_res_phg1
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 6
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		itest,	double,	Input,	,	,	
		vcomp,	double,	Input,	,	,	
		delaytime,	double,	Input,	,	,	
		igate,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double ron200_res_phg1(int hi, int lo, double itest, double vcomp, double delaytime, double * igate);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_res_phg2
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 6
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		itest,	double,	Input,	,	,	
		vcomp,	double,	Input,	,	,	
		delaytime,	double,	Input,	,	,	
		igate,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double ron200_res_phg2(int hi, int lo, double itest, double vcomp, double delaytime, double * igate);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_res_phg3
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 6
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		itest,	double,	Input,	,	,	
		vcomp,	double,	Input,	,	,	
		delaytime,	double,	Input,	,	,	
		igate,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double ron200_res_phg3(int hi, int subst, double itest, double vcomp, double delaytime, double * igate);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_res_phg9
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 7
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		itest,	double,	Input,	,	,	
		vcomp,	double,	Input,	,	,	
		delaytime,	double,	Input,	,	,	
		igate,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double ron200_res_phg9(int hi, int lo, int subst, double itest, double vcomp, double delaytime, double * igate);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_res_phg_org
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 7
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		itest,	double,	Input,	,	,	
		vcomp,	double,	Input,	,	,	
		delaytime,	double,	Input,	,	,	
		igate,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double ron200_res_phg_org(int hi, int lo, int subst, double itest, double vcomp, double delaytime, double * igate);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_res_phgv
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 8
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		vg,	double,	Input,	,	,	
		itest,	double,	Input,	,	,	
		vcomp,	double,	Input,	,	,	
		delaytime,	double,	Input,	,	,	
		igate,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double ron200_res_phgv(int hi, int lo, int gate, double vg, double itest, double vcomp, double delaytime, double * igate);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_res_range_20
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
double ron200_res_range_20(int hi, int lo, int subst, double itest);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_res_smu2
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
double ron200_res_smu2(int hi, int lo, int subst, double itest);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_res_smu3
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
double ron200_res_smu3(int hi, int lo, int subst, double itest);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_res_smu4
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
double ron200_res_smu4(int hi, int lo, int subst, double itest);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_resdeltw
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
double ron200_resdeltw(int numbs, double *resx, int pts1, double *widthy, int pts2, double * corr);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_reshr
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
double ron200_reshr(int numbs, double *lengthx, int pts1, double *resy, int pts2, double wid, double dwid, double * rslope, double * corr);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_resistance_rs2
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
void ron200_resistance_rs2(char * devname, int hi, int lo, int sub, int bulk, int chuckcon, double width, double length, char type, double vforce, double ilimit, double vsub, double delaytime, double intrange, char normalize, double mrange, double lorange, double * R);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_resv
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
double ron200_resv(int hi, int lo, int subst, double v);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_resv_2hi
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
double ron200_resv_2hi(int hi1, int hi2, int lo, int subst, double v);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_resv_dmd
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
double ron200_resv_dmd(int hi, int lo, int subst, double v);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_resv_fnc
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
double ron200_resv_fnc(int hi, int lo, int subst, double v);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_resv_range
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
double ron200_resv_range(int hi, int lo, int subst, double v);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_resv_raw
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
double ron200_resv_raw(int hi, int lo, int subst, double vmin, double Vmax, double vstep);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_resv_sweep
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
double ron200_resv_sweep(int hi, int lo, int subst, double v);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_resvo
	MODULE RETURN TYPE: float 
	NUMBER OF PARMS: 5
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		d_time,	int,	Input,	,	,	
		v,	double,	Input,	,	,	
	INCLUDES:
#include <math.h>
#include "lptdef.h"

#define CRTLIM 100.0E-3

	END USRLIB MODULE INFORMATION
*/
float ron200_resvo(int hi, int lo, int subst, int d_time, double v);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_ron
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
double ron200_ron(int DRN1, int DRN2, int GATE, int SRC1, int SRC2, int SUB, double VDS, double VGS);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_ron5
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
double ron200_ron5(int DRN1, int DRN2, int GATE, int SRC1, int SRC2, int BODY, int SUB, double VDS, double VGS);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_ron5_vsub
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
double ron200_ron5_vsub(int DRN1, int DRN2, int GATE, int SRC1, int SRC2, int BODY, int SUB, double VDS, double VGS, double VSUB);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_ronrf
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
double ron200_ronrf(int DRN1, int DRN2, int GATE, int SRC1, int SRC2, int SUB, double VDS, double VGS);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_ronrfis
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
double ron200_ronrfis(int DRN1, int DRN2, int GATE, int SRC1, int SRC2, int SUB, int isol, double VDS, double VGS);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_setcmtr
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
int ron200_setcmtr(int cmtrx, int subfcn, float param1, float param2);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_sheet_vdp_rs4_ph
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 17
	ARGUMENTS:
		f1,	int,	Input,	,	,	
		f2,	int,	Input,	,	,	
		s1,	int,	Input,	,	,	
		s2,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		bulk,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		width,	double,	Input,	,	,	
		length,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
		iforce,	double,	Input,	,	,	
		vlimit,	double,	Input,	,	,	
		vsub,	double,	Input,	,	,	
		delaytime,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#define R_CONST 4.53
	END USRLIB MODULE INFORMATION
*/
double ron200_sheet_vdp_rs4_ph(int f1, int f2, int s1, int s2, int sub, int bulk, int chuckcon, double width, double length, char type, double iforce, double vlimit, double vsub, double delaytime, double intrange, double mrange, double lorange);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_sort
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 2
	ARGUMENTS:
		arr,	D_ARRAY_T,	Input,	,	,	
		num,	int,	Input,	,	,	
	INCLUDES:

	END USRLIB MODULE INFORMATION
*/
void ron200_sort(double *arr, int num);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_SP000
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 6
	ARGUMENTS:
		InputString,	char *,	Input,	,	,	
		Pins,	I_ARRAY_T,	Input,	,	,	
		Nr_Pins,	int,	Input,	,	,	
		Error,	float *,	Output,	,	,	
		OutputArray,	I_ARRAY_T,	Output,	,	,	
		Nr_OutputArray,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <string.h>
#include <phillib.h>
#include <system_config.h>
	END USRLIB MODULE INFORMATION
*/
void ron200_SP000(char * InputString, int *Pins, int Nr_Pins, float * Error, int *OutputArray, int Nr_OutputArray);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_ST000
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <time.h>
	END USRLIB MODULE INFORMATION
*/
double ron200_ST000();


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_StartCapOffsetDebug
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
void ron200_StartCapOffsetDebug();


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_StopCapOffsetDebug
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
void ron200_StopCapOffsetDebug();


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_svmi
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
void ron200_svmi(int h1, int h2, int h3, int h4, int l1, int l2, int l3, int l4, double v1, double v2, double *v, int npts, double *i, int npts2, double ilim);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_SX010
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 16
	ARGUMENTS:
		Vgs,	F_ARRAY_T,	Input,	,	,	
		Vgs_len,	int,	Input,	,	,	
		Ids,	F_ARRAY_T,	Input,	,	,	
		Ids_len,	int,	Input,	,	,	
		Vsb,	float,	Input,	,	,	
		K0,	float,	Input,	,	,	
		K,	float,	Input,	,	,	
		Vsbx,	float,	Input,	,	,	
		Vtmin,	float,	Input,	,	,	
		Vtmax,	float,	Input,	,	,	
		Phib,	float,	Input,	,	,	
		Vdw,	float,	Input,	,	,	
		Vt1,	float *,	Output,	,	,	
		Trans,	float *,	Output,	,	,	
		Curvat,	float *,	Output,	,	,	
		Error,	float *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <time.h>
#include <phillib.h>
	END USRLIB MODULE INFORMATION
*/
void ron200_SX010(float *Vgs, int Vgs_len, float *Ids, int Ids_len, float Vsb, float K0, float K, float Vsbx, float Vtmin, float Vtmax, float Phib, float Vdw, float * Vt1, float * Trans, float * Curvat, float * Error);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_SX020
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 10
	ARGUMENTS:
		Vsb1,	float,	Input,	,	,	
		Vsb2,	float,	Input,	,	,	
		dVt01,	float,	Input,	,	,	
		dVt02,	float,	Input,	,	,	
		DelV,	float,	Input,	,	,	
		Phib,	float,	Input,	,	,	
		K0,	float *,	Output,	,	,	
		K,	float *,	Output,	,	,	
		Vsbx,	float *,	Output,	,	,	
		Error,	float *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <time.h>
#include <phillib.h>
	END USRLIB MODULE INFORMATION
*/
void ron200_SX020(float Vsb1, float Vsb2, float dVt01, float dVt02, float DelV, float Phib, float * K0, float * K, float * Vsbx, float * Error);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_tdelay
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
double ron200_tdelay(int npin, double i, double v);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_testdi
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		Parm1,	char *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void ron200_testdi(char * Parm1);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_ti_print_time
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
void ron200_ti_print_time(char * in_str, double in_data);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_ti_set_print_flag
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
void ron200_ti_set_print_flag();


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_TimeStamp_msp
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
void ron200_TimeStamp_msp(char * status, int endline);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_vbeswp
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
double ron200_vbeswp(int hi, int lo, int col, int subst, double vstart, double vstop, double vc, int nstep, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_vbridge_ilo_rs2
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
void ron200_vbridge_ilo_rs2(char * devname, int hi, int lo, int sub, int bulk, int chuckcon, double width, double length, char type, double iforce, double vlimit, double vsub, double delaytime, double intrange, double mrange, double lorange, double * V);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_vc
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
double ron200_vc(int emitt, int base, int coll, int sub, double ib);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_vdp_rs4_ph
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 18
	ARGUMENTS:
		f1,	int,	Input,	,	,	
		f2,	int,	Input,	,	,	
		s1,	int,	Input,	,	,	
		s2,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		bulk,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		width,	double,	Input,	,	,	
		length,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
		iforce,	double,	Input,	,	,	
		vlimit,	double,	Input,	,	,	
		vsub,	double,	Input,	,	,	
		delaytime,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		Rsheet,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#define R_CONST 4.53
	END USRLIB MODULE INFORMATION
*/
void ron200_vdp_rs4_ph(int f1, int f2, int s1, int s2, int sub, int bulk, int chuckcon, double width, double length, char type, double iforce, double vlimit, double vsub, double delaytime, double intrange, double mrange, double lorange, double * Rsheet);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_vdp_rs4_ph_org
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 19
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		f1,	int,	Input,	,	,	
		f2,	int,	Input,	,	,	
		s1,	int,	Input,	,	,	
		s2,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		bulk,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		width,	double,	Input,	,	,	
		length,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
		iforce,	double,	Input,	,	,	
		vlimit,	double,	Input,	,	,	
		vsub,	double,	Input,	,	,	
		delaytime,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		Rsheet,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#define R_CONST 4.53
	END USRLIB MODULE INFORMATION
*/
void ron200_vdp_rs4_ph_org(char * devname, int f1, int f2, int s1, int s2, int sub, int bulk, int chuckcon, double width, double length, char type, double iforce, double vlimit, double vsub, double delaytime, double intrange, double mrange, double lorange, double * Rsheet);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_vdp_rs4_ph_swp
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 18
	ARGUMENTS:
		f1,	int,	Input,	,	,	
		f2,	int,	Input,	,	,	
		s1,	int,	Input,	,	,	
		s2,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		bulk,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		width,	double,	Input,	,	,	
		length,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
		iforce,	double,	Input,	,	,	
		vlimit,	double,	Input,	,	,	
		vsub,	double,	Input,	,	,	
		delaytime,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		Rsheet,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#define R_CONST 4.53
	END USRLIB MODULE INFORMATION
*/
void ron200_vdp_rs4_ph_swp(int f1, int f2, int s1, int s2, int sub, int bulk, int chuckcon, double width, double length, char type, double iforce, double vlimit, double vsub, double delaytime, double intrange, double mrange, double lorange, double * Rsheet);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_vector_median
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
double ron200_vector_median(double * arr, int sz);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_vf
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
double ron200_vf(int hi, int lo, int subst, double itest);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_vf_ph
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 6
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		type,	char,	Input,	,	,	
		itest,	double,	Input,	,	,	
		VLTLIM,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double ron200_vf_ph(int hi, int lo, int subst, char type, double itest, double VLTLIM);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_vg1a
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
double ron200_vg1a(int DRAIN, int GATE, int SOURCE, int SUB, double VLOW, double VHIGH, double VD, double VSUB, int NSTEP, double IPGM, double UDELAY, char TYPE);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_vg2a
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
double ron200_vg2a(int d, int g, int s, int sub, char type, double id, double vlim, double vds, double vbs, double * ids, int * istat);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_vg2a_iso
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
double ron200_vg2a_iso(int d, int g, int s, int sub, int isl, char type, double id, double vlim, double vds, double vbs, double * ids, int * istat);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_vg3
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
double ron200_vg3(int drain, int gate, int source, int subst, double vlow, double vhigh, double vds, double vbs, double ithr, int niter);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_vg3_isl
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
double ron200_vg3_isl(int drain, int gate, int source, int subst, int isol, double vlow, double vhigh, double vds, double vbs, double ithr, int niter);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_vgsweep
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
double ron200_vgsweep(int drain, int gate, int source, int sub, double vglow, double vghigh, double vd, int nstep, double idpgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_vgsweep_abs
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
double ron200_vgsweep_abs(int drain, int gate, int source, int sub, double vglow, double vghigh, double vd, int nstep, double idpgm, double udelay);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_vgsweep_cst_vt
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 13
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		vglow,	double,	Input,	,	,	
		vghigh,	double,	Input,	,	,	
		vd,	double,	Input,	,	,	
		nstep,	int,	Input,	,	,	
		width,	double,	Input,	,	,	
		length,	double,	Input,	,	,	
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
double ron200_vgsweep_cst_vt(int drain, int gate, int source, int sub, double vglow, double vghigh, double vd, int nstep, double width, double length, double idpgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_vgsweep_isl
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
double ron200_vgsweep_isl(int drain, int gate, int source, int sub, int isol, double vglow, double vghigh, double vd, int nstep, double idpgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_vgswp3
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
void ron200_vgswp3(int ds, int g, int sub, double v1, double v2, double *v, int npts1, double *ig, int npts2, double *isub, int npts3, double *isrc, int npts4);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_vp1
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
void ron200_vp1(int d, int g, int s, int sub, double ids, double vdlim, double vg1, double vg2, double iglim, double * iflag, double * vp);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_vp_1gate
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
double ron200_vp_1gate(int drain, int gate, int source, int sub, double vglow, double vghigh, double vd, int nstep, double ipgm, double udelay);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_vt_lin_LBC5_raw
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
void ron200_vt_lin_LBC5_raw(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vmin, double vmax, int npoints, double idmax, double vds, double vbs, double intrange, double mrange, double lorange, double * Vtl, double * K, double * Gm);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_vtati
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
double ron200_vtati(int drain, int gate, int source, int subst, double vlow, double vhigh, double vds, double vbs, double ithr, int niter);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_vtati_con
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
double ron200_vtati_con(int drain, int gate, int source, int subst, double vlow, double vhigh, double vds, double vbs, double ithr, int niter);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_vtati_org
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
double ron200_vtati_org(int drain, int gate, int source, int subst, double vlow, double vhigh, double vds, double vbs, double ithr, int niter);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_vtati_raw
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
double ron200_vtati_raw(int drain, int gate, int source, int subst, double vlow, double vhigh, double vds, double vbs, double ithr, int niter);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_vtexis
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
double ron200_vtexis(int drain, int gate, int source, int subst, int isol, double w, double l, double vlow, double vhigh, double vds, double vbs, double ithr, double vstep, int npts, double * slope, int * kflag);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_vtext4
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
double ron200_vtext4(int drain, int gate, int source, int subst, double w, double l, double vlow, double vhigh, double vds, double vbs, double ithr, double vstep, int npts, double * slope, int * kflag);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_vtext4_dren
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
double  ron200_vtext4_dren(int, int, int, int, double, double, double, double, double, double, double, double, int, double *, int *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_vtext4_iso
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
double ron200_vtext4_iso(int drain, int gate, int source, int subst, int isl, double w, double l, double vlow, double vhigh, double vds, double vbs, double ithr, double vstep, int npts, double * slope, int * kflag);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_vtext4_iso_dren
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
double  ron200_vtext4_iso_dren(int, int, int, int, int, double, double, double, double, double, double, double, double, int, double *, int *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_vtext4_raw
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
double ron200_vtext4_raw(int drain, int gate, int source, int subst, double w, double l, double vlow, double vhigh, double vds, double vbs, double ithr, double vstep, int npts, double * slope, int * kflag);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_vtext4_raw_org
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
double ron200_vtext4_raw_org(int drain, int gate, int source, int subst, double w, double l, double vlow, double vhigh, double vds, double vbs, double ithr, double vstep, int npts, double * slope, int * kflag);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_vtext5
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
double ron200_vtext5(int drain, int gate, int source, int body, int subst, double w, double l, double vlow, double vhigh, double vds, double vbs, double ithr, double vstep, int npts, double * slope, int * kflag);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_vtext5_dren
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
double  ron200_vtext5_dren(int, int, int, int, int, double, double, double, double, double, double, double, double, int, double *, int *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_vtexts
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
double ron200_vtexts(int drain, int gate, int source, int subst, double w, double l, double vlow, double vhigh, double vds, double vbs, double ithr, double vstep, int npts, double * slope, int * kflag);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_vtexts5
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
double ron200_vtexts5(int drain, int gate, int source, int body, int subst, double w, double l, double vlow, double vhigh, double vds, double vbs, double ithr, double vstep, int npts, double * slope, int * kflag);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_vtexts5_dren
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
double  ron200_vtexts5_dren(int, int, int, int, int, double, double, double, double, double, double, double, double, int, double *, int *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_vtexts_dren
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
double  ron200_vtexts_dren(int, int, int, int, double, double, double, double, double, double, double, double, int, double *, int *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_vtexts_iso
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
double ron200_vtexts_iso(int drain, int gate, int source, int subst, int isl, double w, double l, double vlow, double vhigh, double vds, double vbs, double ithr, double vstep, int npts, double * slope, int * kflag);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_vtexts_iso_dren
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
double  ron200_vtexts_iso_dren(int, int, int, int, int, double, double, double, double, double, double, double, double, int, double *, int *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_vtfswp
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
double ron200_vtfswp(int drain, int gate, int source, int sub, double vlow, double vhigh, double vd, int nstep, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_vtfswp_himp
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 12
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		flt,	int,	Input,	,	,	
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
double  ron200_vtfswp_himp(int, int, int, int, int, double, double, double, int, double, double, char);

/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_vtfswp_iso
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
double ron200_vtfswp_iso(int drain, int gate, int source, int sub, int isl, double vlow, double vhigh, double vd, int nstep, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_vtfswp_ph
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 12
	ARGUMENTS:
		source,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		type,	char,	Input,	,	,	
		vd,	double,	Input,	,	,	
		vlow,	double,	Input,	,	,	
		vhigh,	double,	Input,	,	,	
		nstep,	int,	Input,	,	,	
		vbs,	double,	Input,	,	,	
		ipgm,	double,	Input,	,	,	
		udelay,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double ron200_vtfswp_ph(int source, int gate, int drain, int sub, char type, double vd, double vlow, double vhigh, int nstep, double vbs, double ipgm, double udelay);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron200_vtfswp_ph_cf
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 12
	ARGUMENTS:
		source,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		type,	char,	Input,	,	,	
		vd,	double,	Input,	,	,	
		vlow,	double,	Input,	,	,	
		vhigh,	double,	Input,	,	,	
		nstep,	int,	Input,	,	,	
		vbs,	double,	Input,	,	,	
		ipgm,	double,	Input,	,	,	
		udelay,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double ron200_vtfswp_ph_cf(int source, int gate, int drain, int sub, char type, double vd, double vlow, double vhigh, int nstep, double vbs, double ipgm, double udelay);


