/* PARLIB400 function prototype and KITT header file */

/* USRLIB PROTOTYPES VERSION CONTROL */
/* static char const libproto_vcid[] ="$Id: PARLIB400_proto.h,v 1.6 2000/10/10 14:10:20 williamson REL $"; */

/* COPY OF USRLIB SETTINGS INFORMATION */
/* [VISIBILITY] */
/* HIDDEN=0 */
/* [DEPENDENCIES] */
/* LIBS= */
/* OBJECTS= */
/* HEADERS= */
/* [NON-KULT SEARCH PATH] */
/* PATHS= */

/* USRLIB MODULE INFORMATION

	MODULE NAME: beta1_400
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 7
	ARGUMENTS:
		em,	int,	Input,	,	,	
		base,	int,	Input,	,	,	
		coll,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		ie,	double,	Input,	,	,	
		vcb,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "PARLIB400_proto.h"
#include <stdlib.h>
	END USRLIB MODULE INFORMATION
*/
double beta1_400(int em, int base, int coll, int subst, double ie, double vcb, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: beta2_400
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
#include "PARLIB400_proto.h"
#include <stdlib.h>
	END USRLIB MODULE INFORMATION
*/
double beta2_400(int em, int base, int coll, int subst, double ic, double vce, double * vbeout, double * icout, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: beta2a_400
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 12
	ARGUMENTS:
		e,	int,	Input,	,	,	
		b,	int,	Input,	,	,	
		c,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		ice,	double,	Input,	,	,	
		vcb,	double,	Input,	,	,	
		ie1,	double,	Input,	,	,	
		ie2,	double,	Input,	,	,	
		vsub,	double,	Input,	,	,	
		icmeas,	double *,	Output,	,	,	
		ieout,	double *,	Output,	,	,	
		error,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "PARLIB400_proto.h"
#include <stdlib.h>
	END USRLIB MODULE INFORMATION
*/
double beta2a_400(int e, int b, int c, int sub, double ice, double vcb, double ie1, double ie2, double vsub, double * icmeas, double * ieout, double * error);


/* USRLIB MODULE INFORMATION

	MODULE NAME: beta3a_400
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 12
	ARGUMENTS:
		e,	int,	Input,	,	,	
		b,	int,	Input,	,	,	
		c,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		ice,	double,	Input,	,	,	
		vce,	double,	Input,	,	,	
		ibe1,	double,	Input,	,	,	
		ibe2,	double,	Input,	,	,	
		vsub,	double,	Input,	,	,	
		ibe,	double *,	Output,	,	,	
		icmeas,	double *,	Output,	,	,	
		error,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "PARLIB400_proto.h"
	END USRLIB MODULE INFORMATION
*/
double beta3a_400(int e, int b, int c, int sub, double ice, double vce, double ibe1, double ibe2, double vsub, double * ibe, double * icmeas, double * error);


/* USRLIB MODULE INFORMATION

	MODULE NAME: bkdn_400
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
#include "PARLIB400_proto.h"
	END USRLIB MODULE INFORMATION
*/
double bkdn_400(int hi, int lo, int subst, double ipgm, double vlim);


/* USRLIB MODULE INFORMATION

	MODULE NAME: bvcbo1_400
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 10
	ARGUMENTS:
		em,	int,	Input,	,	,	
		base,	int,	Input,	,	,	
		coll,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		vcbstart,	double,	Input,	,	,	
		vcbstop,	double,	Input,	,	,	
		nstep,	int,	Input,	,	,	
		ipgm,	double,	Input,	,	,	
		udelay,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "PARLIB400_proto.h"
#include <stdlib.h>
	END USRLIB MODULE INFORMATION
*/
double bvcbo1_400(int em, int base, int coll, int subst, double vcbstart, double vcbstop, int nstep, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: bvcbo_400
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
#include "PARLIB400_proto.h"
#include <stdlib.h>
	END USRLIB MODULE INFORMATION
*/
double bvcbo_400(int em, int base, int coll, int subst, double ipgm, double vlim, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: bvceo2_400
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
#include "PARLIB400_proto.h"
#include <stdlib.h>
	END USRLIB MODULE INFORMATION
*/
double bvceo2_400(int e, int b, int c, int s, double vcestart, double vcestop, int nstep, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: bvceo_400
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
#include "PARLIB400_proto.h"
	END USRLIB MODULE INFORMATION
*/
double bvceo_400(int em, int base, int coll, int subst, double ipgm, double vlim, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: bvces1_400
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
#include "PARLIB400_proto.h"
#include <stdlib.h>
	END USRLIB MODULE INFORMATION
*/
double bvces1_400(int e, int b, int c, int s, double vcestart, double vcestop, int nstep, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: bvces_400
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 7
	ARGUMENTS:
		e,	int,	Input,	,	,	
		b,	int,	Input,	,	,	
		c,	int,	Input,	,	,	
		s,	int,	Input,	,	,	
		ipgm,	double,	Input,	,	,	
		vlim,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "PARLIB400_proto.h"
#include <stdlib.h>
	END USRLIB MODULE INFORMATION
*/
double bvces_400(int e, int b, int c, int s, double ipgm, double vlim, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: bvdss1_400
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
#include "PARLIB400_proto.h"
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <stdlib.h>
#include <stdio.h>
	END USRLIB MODULE INFORMATION
*/
double bvdss1_400(int d, int g, int s, int sub, double vdsstart, double vdsstop, int nstep, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: bvdss_400
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 6
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		ipgm,	double,	Input,	,	,	
		vlim,	double,	Input,	,	,	
	INCLUDES:
#include <math.h>
#include "PARLIB400_proto.h"
#include <stdio.h>                
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <stdlib.h>
	END USRLIB MODULE INFORMATION
*/
double bvdss_400(int drain, int gate, int source, int subst, double ipgm, double vlim);


/* USRLIB MODULE INFORMATION

	MODULE NAME: bvebo_400
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 7
	ARGUMENTS:
		e,	int,	Input,	,	,	
		b,	int,	Input,	,	,	
		c,	int,	Input,	,	,	
		s,	int,	Input,	,	,	
		ipgm,	double,	Input,	,	,	
		vlim,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "PARLIB400_proto.h"
#include <stdlib.h>
	END USRLIB MODULE INFORMATION
*/
double bvebo_400(int e, int b, int c, int s, double ipgm, double vlim, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: cap_400
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 4
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		vbias,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "PARLIB400_proto.h"
#include <stdlib.h>
	END USRLIB MODULE INFORMATION
*/
double cap_400(int hi, int lo, int subst, double vbias);


/* USRLIB MODULE INFORMATION

	MODULE NAME: deltl1_400
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 18
	ARGUMENTS:
		d1,	int,	Input,	,	,	
		g1,	int,	Input,	,	,	
		s1,	int,	Input,	,	,	
		sub1,	int,	Input,	,	,	
		l1,	double,	Input,	,	,	
		d2,	int,	Input,	,	,	
		g2,	int,	Input,	,	,	
		s2,	int,	Input,	,	,	
		sub2,	int,	Input,	,	,	
		l2,	double,	Input,	,	,	
		vlow,	double,	Input,	,	,	
		vhigh,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vbs,	double,	Input,	,	,	
		ithr,	double,	Input,	,	,	
		vstep,	double,	Input,	,	,	
		npts,	int,	Input,	,	,	
		kflag,	int *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "PARLIB400_proto.h"
#include <stdlib.h>
	END USRLIB MODULE INFORMATION
*/
double deltl1_400(int d1, int g1, int s1, int sub1, double l1, int d2, int g2, int s2, int sub2, double l2, double vlow, double vhigh, double vds, double vbs, double ithr, double vstep, int npts, int * kflag);


/* USRLIB MODULE INFORMATION

	MODULE NAME: deltw1_400
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 18
	ARGUMENTS:
		d1,	int,	Input,	,	,	
		g1,	int,	Input,	,	,	
		s1,	int,	Input,	,	,	
		b1,	int,	Input,	,	,	
		w1,	double,	Input,	,	,	
		d2,	int,	Input,	,	,	
		g2,	int,	Input,	,	,	
		s2,	int,	Input,	,	,	
		b2,	int,	Input,	,	,	
		w2,	double,	Input,	,	,	
		vlow,	double,	Input,	,	,	
		vhigh,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vbs,	double,	Input,	,	,	
		ithr,	double,	Input,	,	,	
		vstep,	double,	Input,	,	,	
		npts,	int,	Input,	,	,	
		kflag,	int *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "PARLIB400_proto.h"
#include <stdlib.h>
	END USRLIB MODULE INFORMATION
*/
double deltw1_400(int d1, int g1, int s1, int b1, double w1, int d2, int g2, int s2, int b2, double w2, double vlow, double vhigh, double vds, double vbs, double ithr, double vstep, int npts, int * kflag);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ev_400
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
		iflag,	double *,	Output,	,	,	
		r,	double *,	Output,	,	,	
		early,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "PARLIB400_proto.h"
#include <stdlib.h>
	END USRLIB MODULE INFORMATION
*/
void ev_400(int e, int b, int c, int s, double ibe, double vstart, double vstop, int npts, double vsub, double * slope, double * iflag, double * r, double * early);


/* USRLIB MODULE INFORMATION

	MODULE NAME: fimv_400
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 10
	ARGUMENTS:
		h1,	int,	Input,	,	,	
		h2,	int,	Input,	,	,	
		h3,	int,	Input,	,	,	
		h4,	int,	Input,	,	,	
		l1,	int,	Input,	,	,	
		l2,	int,	Input,	,	,	
		l3,	int,	Input,	,	,	
		l4,	int,	Input,	,	,	
		v,	double *,	Output,	,	,	
		i,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "PARLIB400_proto.h"
#include <stdlib.h>
	END USRLIB MODULE INFORMATION
*/
double fimv_400(int h1, int h2, int h3, int h4, int l1, int l2, int l3, int l4, double * v, double i);


/* USRLIB MODULE INFORMATION

	MODULE NAME: fnddat_400
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
#include "PARLIB400_proto.h"
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <stdlib.h>
	END USRLIB MODULE INFORMATION
*/
void fnddat_400(double *x, int npts, double *y, int npts1, double x1, double x2, double *xnew, int np1, double *ynew, int np2, int * np, char code);


/* USRLIB MODULE INFORMATION

	MODULE NAME: fndpt_400
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
#include "PARLIB400_proto.h"
	END USRLIB MODULE INFORMATION
*/
void fndpt_400(double *values, int npts, double target, int * j);


/* USRLIB MODULE INFORMATION

	MODULE NAME: fndslp_400
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 6
	ARGUMENTS:
		x,	double *,	Input,	,	,	
		y,	double *,	Input,	,	,	
		npts,	int,	Input,	,	,	
		slope,	double *,	Output,	,	,	
		yinter,	double *,	Output,	,	,	
		r,	double *,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "PARLIB400_proto.h"
	END USRLIB MODULE INFORMATION
*/
void fndslp_400(double * x, double * y, int npts, double * slope, double * yinter, double * r);


/* USRLIB MODULE INFORMATION

	MODULE NAME: fndtrg_400
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 2
	ARGUMENTS:
		low,	double,	Input,	,	,	
		high,	double,	Input,	,	,	
	INCLUDES:
#include <math.h>
#include "PARLIB400_proto.h"
#include <stdio.h>
	END USRLIB MODULE INFORMATION
*/
int fndtrg_400(double low, double high);


/* USRLIB MODULE INFORMATION

	MODULE NAME: fvmi_400
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 10
	ARGUMENTS:
		h1,	int,	Input,	,	,	
		h2,	int,	Input,	,	,	
		h3,	int,	Input,	,	,	
		h4,	int,	Input,	,	,	
		l1,	int,	Input,	,	,	
		l2,	int,	Input,	,	,	
		l3,	int,	Input,	,	,	
		l4,	int,	Input,	,	,	
		v,	double,	Input,	,	,	
		i,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "PARLIB400_proto.h"
#include <stdlib.h>
	END USRLIB MODULE INFORMATION
*/
double fvmi_400(int h1, int h2, int h3, int h4, int l1, int l2, int l3, int l4, double v, double * i);


/* USRLIB MODULE INFORMATION

	MODULE NAME: gamma1_400
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 14
	ARGUMENTS:
		d,	int,	Input,	,	,	
		g,	int,	Input,	,	,	
		s,	int,	Input,	,	,	
		sb,	int,	Input,	,	,	
		vlow,	double,	Input,	,	,	
		vhigh,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vbs1,	double,	Input,	,	,	
		vbs2,	double,	Input,	,	,	
		phip,	double,	Input,	,	,	
		ithr,	double,	Input,	,	,	
		vstep,	double,	Input,	,	,	
		npts,	int,	Input,	,	,	
		kflag,	int *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "PARLIB400_proto.h"
#include <stdlib.h>
	END USRLIB MODULE INFORMATION
*/
double gamma1_400(int d, int g, int s, int sb, double vlow, double vhigh, double vds, double vbs1, double vbs2, double phip, double ithr, double vstep, int npts, int * kflag);


/* USRLIB MODULE INFORMATION

	MODULE NAME: gd_400
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 8
	ARGUMENTS:
		d,	int,	Input,	,	,	
		g,	int,	Input,	,	,	
		s,	int,	Input,	,	,	
		sb,	int,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vgs,	double,	Input,	,	,	
		vbs,	double,	Input,	,	,	
		ids,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "PARLIB400_proto.h"
#include <stdlib.h>
	END USRLIB MODULE INFORMATION
*/
double gd_400(int d, int g, int s, int sb, double vds, double vgs, double vbs, double * ids);


/* USRLIB MODULE INFORMATION

	MODULE NAME: gm_400
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 10
	ARGUMENTS:
		d,	int,	Input,	,	,	
		g,	int,	Input,	,	,	
		s,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		vds,	double,	Input,	,	,	
		idlim,	double,	Input,	,	,	
		vgs,	double,	Input,	,	,	
		vgstep,	double,	Input,	,	,	
		iglim,	double,	Input,	,	,	
		iflag,	int *,	Output,	,	,	
	INCLUDES:
#include <math.h>
#include "PARLIB400_proto.h"
#include <lptdef.h>
#include <stdlib.h>
#include <lptdef_lowercase.h>
	END USRLIB MODULE INFORMATION
*/
double gm_400(int d, int g, int s, int sub, double vds, double idlim, double vgs, double vgstep, double iglim, int * iflag);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ibic1_400
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 10
	ARGUMENTS:
		e,	int,	Input,	,	,	
		b,	int,	Input,	,	,	
		c,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		vce,	double,	Input,	,	,	
		vbe,	double,	Input,	,	,	
		vsub,	double,	Input,	,	,	
		ibe,	double *,	Output,	,	,	
		ice,	double *,	Output,	,	,	
		beta,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "PARLIB400_proto.h"
#include <stdlib.h>
	END USRLIB MODULE INFORMATION
*/
void ibic1_400(int e, int b, int c, int sub, double vce, double vbe, double vsub, double * ibe, double * ice, double * beta);


/* USRLIB MODULE INFORMATION

	MODULE NAME: icbo_400
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
#include "PARLIB400_proto.h"
#include <stdlib.h>
	END USRLIB MODULE INFORMATION
*/
double icbo_400(int e, int b, int c, int s, double vcbo, double vsub);


/* USRLIB MODULE INFORMATION

	MODULE NAME: iceo_400
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
#include "PARLIB400_proto.h"
	END USRLIB MODULE INFORMATION
*/
double iceo_400(int e, int b, int c, int sub, double vce, double vsub);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ices_400
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
#include "PARLIB400_proto.h"
#include <stdlib.h>
	END USRLIB MODULE INFORMATION
*/
double ices_400(int e, int b, int c, int s, double vces, double vsub);


/* USRLIB MODULE INFORMATION

	MODULE NAME: id1_400
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
#include <math.h>	                
#include "PARLIB400_proto.h"
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <stdio.h>
	END USRLIB MODULE INFORMATION
*/
double id1_400(int drain, int gate, int source, int subst, double vgs, double vds, double vbs);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ids
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 11
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		w,	double,	Input,	,	,	
		l,	double,	Input,	,	,	
		vgs,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vbs,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
		flag,	int,	Input,	,	,	
	INCLUDES:
#include <math.h>	                
#include "PARLIB400_proto.h"
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <stdio.h>
	END USRLIB MODULE INFORMATION
*/
double  ids(int, int, int, int, double, double, double, double, double, char, int);

/* USRLIB MODULE INFORMATION

	MODULE NAME: idsat_400
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 6
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vbs,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <math.h>	                
#include "PARLIB400_proto.h"
#include <lptdef.h>
#include <lptdef_lowercase.h>

	END USRLIB MODULE INFORMATION
*/
double idsat_400(int drain, int gate, int source, int subst, double vds, double vbs);


/* USRLIB MODULE INFORMATION

	MODULE NAME: idss_400
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 9
	ARGUMENTS:
		d,	int,	Input,	,	,	
		g,	int,	Input,	,	,	
		s,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		vdss,	double,	Input,	,	,	
		idlim,	double,	Input,	,	,	
		f,	double,	Input,	,	,	
		idsat,	double *,	Output,	,	,	
		vdsat,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "PARLIB400_proto.h"
#include <stdlib.h>
	END USRLIB MODULE INFORMATION
*/
double idss_400(int d, int g, int s, int sub, double vdss, double idlim, double f, double * idsat, double * vdsat);


/* USRLIB MODULE INFORMATION

	MODULE NAME: iebo_400
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 6
	ARGUMENTS:
		e,	int,	Input,	,	,	
		b,	int,	Input,	,	,	
		c,	int,	Input,	,	,	
		s,	int,	Input,	,	,	
		vebo,	double,	Input,	,	,	
		vsub,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "PARLIB400_proto.h"
#include <stdlib.h>
	END USRLIB MODULE INFORMATION
*/
double iebo_400(int e, int b, int c, int s, double vebo, double vsub);


/* USRLIB MODULE INFORMATION

	MODULE NAME: isubmx_400
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
void isubmx_400(int d, int g, int s, int sub, double vds, double vbs, double vlow, double vhigh, int npts, double * ismax, double * vgmax);


/* USRLIB MODULE INFORMATION

	MODULE NAME: kdelay_400
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 3
	ARGUMENTS:
		npin,	int,	Input,	,	,	
		i,	double,	Input,	,	,	
		v,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>	                
#include <math.h>	              
#include "PARLIB400_proto.h"
#include <lptdef.h>
#include <lptdef_lowercase.h>

#define MAX(a,b)	((a)>(b) ? (a) : (b))
#define CDELAY 	100.0E-12
#define ILEAK  	1.E-12 
	END USRLIB MODULE INFORMATION
*/
void kdelay_400(int npin, double i, double v);


/* USRLIB MODULE INFORMATION

	MODULE NAME: leak_400
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
#include "PARLIB400_proto.h"
	END USRLIB MODULE INFORMATION
*/
double leak_400(int hi, int lo, int subst, double v, double ilim);


/* USRLIB MODULE INFORMATION

	MODULE NAME: linmmx_400
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 4
	ARGUMENTS:
		a,	D_ARRAY_T,	Input,	,	,	
		n,	int,	Input,	,	,	
		amin,	double *,	Output,	,	,	
		amax,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "PARLIB400_proto.h"
	END USRLIB MODULE INFORMATION
*/
double linmmx_400(double *a, int n, double * amin, double * amax);


/* USRLIB MODULE INFORMATION

	MODULE NAME: llsq_400
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
#include "PARLIB400_proto.h"
	END USRLIB MODULE INFORMATION
*/
void llsq_400(double * x, double * y, int npts, double * a, double * b, double * r);


/* USRLIB MODULE INFORMATION

	MODULE NAME: logstp_400
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 4
	ARGUMENTS:
		xstart,	double,	Input,	,	,	
		xstop,	double,	Input,	,	,	
		steps,	D_ARRAY_T,	Output,	,	,	
		npts,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "PARLIB400_proto.h"
#include <stdlib.h>
	END USRLIB MODULE INFORMATION
*/
int logstp_400(double xstart, double xstop, double *steps, int npts);


/* USRLIB MODULE INFORMATION

	MODULE NAME: rcsat_400
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 11
	ARGUMENTS:
		e,	int,	Input,	,	,	
		b,	int,	Input,	,	,	
		c,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		ice1,	double,	Input,	,	,	
		ice2,	double,	Input,	,	,	
		beta,	double,	Input,	,	,	
		vsub,	double,	Input,	,	,	
		npts,	int,	Input,	,	,	
		r,	double *,	Output,	,	,	
		iflag,	int *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "PARLIB400_proto.h"
	END USRLIB MODULE INFORMATION
*/
double rcsat_400(int e, int b, int c, int sub, double ice1, double ice2, double beta, double vsub, int npts, double * r, int * iflag);


/* USRLIB MODULE INFORMATION

	MODULE NAME: re_400
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 10
	ARGUMENTS:
		e,	int,	Input,	,	,	
		b,	int,	Input,	,	,	
		c,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		ib1,	double,	Input,	,	,	
		ib2,	double,	Input,	,	,	
		vsub,	double,	Input,	,	,	
		npts,	int,	Input,	,	,	
		iflag,	int *,	Output,	,	,	
		r,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "PARLIB400_proto.h"
#include <stdlib.h>
	END USRLIB MODULE INFORMATION
*/
double re_400(int e, int b, int c, int sub, double ib1, double ib2, double vsub, int npts, int * iflag, double * r);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res2_400
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 5
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		itest,	double,	Input,	,	,	
		vlimit,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "PARLIB400_proto.h"
	END USRLIB MODULE INFORMATION
*/
double res2_400(int hi, int lo, int subst, double itest, double vlimit);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res2_i
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 5
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		itest,	double,	Input,	,	,	
		vlimit,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "PARLIB400_proto.h"
	END USRLIB MODULE INFORMATION
*/
double  res2_i(int, int, int, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: res4_400
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 6
	ARGUMENTS:
		his,	int,	Input,	,	,	
		him,	int,	Input,	,	,	
		los,	int,	Input,	,	,	
		lom,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		itest,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "PARLIB400_proto.h"
	END USRLIB MODULE INFORMATION
*/
double res4_400(int his, int him, int los, int lom, int subst, double itest);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res_400
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
#include "PARLIB400_proto.h"
#include <stdlib.h>
	END USRLIB MODULE INFORMATION
*/
double res_400(int hi, int lo, int subst, double itest);


/* USRLIB MODULE INFORMATION

	MODULE NAME: resv_400
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 4
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		v,	double,	Input,	,	,	
	INCLUDES:
#include <math.h>
#include "PARLIB400_proto.h"
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <stdio.h>

#define CRTLIM 10.0E-3

	END USRLIB MODULE INFORMATION
*/
double resv_400(int hi, int lo, int subst, double v);


/* USRLIB MODULE INFORMATION

	MODULE NAME: rvdp_400
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 7
	ARGUMENTS:
		pin1,	int,	Input,	,	,	
		pin2,	int,	Input,	,	,	
		pin3,	int,	Input,	,	,	
		pin4,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		itest,	double,	Input,	,	,	
		ratio,	double *,	Output,	,	,	
	INCLUDES:
#include <math.h>
#include "PARLIB400_proto.h"
#include "lptdef.h"
#include <stdlib.h>

#define VLTLIM 20.0		                              
	END USRLIB MODULE INFORMATION
*/
double rvdp_400(int pin1, int pin2, int pin3, int pin4, int subst, double itest, double * ratio);


/* USRLIB MODULE INFORMATION

	MODULE NAME: tdelay_400
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 3
	ARGUMENTS:
		npin,	int,	Input,	,	,	
		i,	double,	Input,	,	,	
		v,	double,	Input,	,	,	
	INCLUDES:
#include <math.h>                     
#include "PARLIB400_proto.h"
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <stdio.h>

#define MAX(a,b)	((a)>(b) ? (a) : (b))
#define CDELAY  100.E-12
#define ILEAK   1.E-12 
	END USRLIB MODULE INFORMATION
*/
double tdelay_400(int npin, double i, double v);


/* USRLIB MODULE INFORMATION

	MODULE NAME: tox_400
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 5
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		vbias,	double,	Input,	,	,	
		area,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "PARLIB400_proto.h"
#include <stdlib.h>
	END USRLIB MODULE INFORMATION
*/
double tox_400(int hi, int lo, int sub, double vbias, double area);


/* USRLIB MODULE INFORMATION

	MODULE NAME: vbes_400
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 6
	ARGUMENTS:
		e,	int,	Input,	,	,	
		b,	int,	Input,	,	,	
		c,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		ipgm,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "PARLIB400_proto.h"
#include <stdlib.h>
	END USRLIB MODULE INFORMATION
*/
double vbes_400(int e, int b, int c, int sub, double ipgm, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: vf_400
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
#include "PARLIB400_proto.h"
	END USRLIB MODULE INFORMATION
*/
double vf_400(int hi, int lo, int subst, double itest);


/* USRLIB MODULE INFORMATION

	MODULE NAME: vg2_400
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 14
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		type,	char,	Input,	,	,	
		idspec,	double,	Input,	,	,	
		errpct,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vbs,	double,	Input,	,	,	
		vglo,	double,	Input,	,	,	
		vghi,	double,	Input,	,	,	
		maxitr,	int,	Input,	,	,	
		idmeas,	double *,	Output,	,	,	
		istat,	int *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "PARLIB400_proto.h"
	END USRLIB MODULE INFORMATION
*/
double vg2_400(int drain, int gate, int source, int subst, char type, double idspec, double errpct, double vds, double vbs, double vglo, double vghi, int maxitr, double * idmeas, int * istat);


/* USRLIB MODULE INFORMATION

	MODULE NAME: vg2a_400
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
#include "PARLIB400_proto.h"
#include <stdlib.h>
	END USRLIB MODULE INFORMATION
*/
double vg2a_400(int d, int g, int s, int sub, char type, double id, double vlim, double vds, double vbs, double * ids, int * istat);


/* USRLIB MODULE INFORMATION

	MODULE NAME: vgsat_400
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 7
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		ipgm,	double,	Input,	,	,	
		vlim,	double,	Input,	,	,	
		vsub,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "PARLIB400_proto.h"
	END USRLIB MODULE INFORMATION
*/
double vgsat_400(int drain, int gate, int source, int subst, double ipgm, double vlim, double vsub);


/* USRLIB MODULE INFORMATION

	MODULE NAME: vp1_400
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
#include "PARLIB400_proto.h"
#include <stdlib.h>
	END USRLIB MODULE INFORMATION
*/
void vp1_400(int d, int g, int s, int sub, double ids, double vdlim, double vg1, double vg2, double iglim, double * iflag, double * vp);


/* USRLIB MODULE INFORMATION

	MODULE NAME: vp_400
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 12
	ARGUMENTS:
		d,	int,	Input,	,	,	
		g,	int,	Input,	,	,	
		s,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		vdss,	double,	Input,	,	,	
		idlim,	double,	Input,	,	,	
		factor,	double,	Input,	,	,	
		v1,	double,	Input,	,	,	
		v2,	double,	Input,	,	,	
		idss,	double *,	Output,	,	,	
		ip,	double *,	Output,	,	,	
		iflag,	int *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "PARLIB400_proto.h"
#include <stdlib.h>
	END USRLIB MODULE INFORMATION
*/
double vp_400(int d, int g, int s, int sub, double vdss, double idlim, double factor, double v1, double v2, double * idss, double * ip, int * iflag);


/* USRLIB MODULE INFORMATION

	MODULE NAME: vt14_400
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
		niter,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "PARLIB400_proto.h"
	END USRLIB MODULE INFORMATION
*/
double vt14_400(int drain, int gate, int source, int subst, double vlow, double vhigh, double vds, double vbs, double ithr, double niter);


/* USRLIB MODULE INFORMATION

	MODULE NAME: vtati_400
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
#include "PARLIB400_proto.h"
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <stdio.h>

#define MAX(a,b)	((a)>(b) ? (a) : (b))

	END USRLIB MODULE INFORMATION
*/
double vtati_400(int drain, int gate, int source, int subst, double vlow, double vhigh, double vds, double vbs, double ithr, int niter);


/* USRLIB MODULE INFORMATION

	MODULE NAME: VTATI_ISO
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
#include "PARLIB400_proto.h"
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <stdio.h>

#define MAX(a,b)	((a)>(b) ? (a) : (b))

	END USRLIB MODULE INFORMATION
*/
double  VTATI_ISO(int, int, int, int, double, double, double, double, double, int);

/* USRLIB MODULE INFORMATION

	MODULE NAME: vtext2_400
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 13
	ARGUMENTS:
		d,	int,	Input,	,	,	
		g,	int,	Input,	,	,	
		s,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
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
#include "PARLIB400_proto.h"
#include <stdlib.h>
	END USRLIB MODULE INFORMATION
*/
double vtext2_400(int d, int g, int s, int sub, double vlow, double vhigh, double vds, double vbs, double ithr, double vstep, int npts, double * slope, int * kflag);


/* USRLIB MODULE INFORMATION

	MODULE NAME: vtext_400
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 14
	ARGUMENTS:
		d,	int,	Input,	,	,	
		g,	int,	Input,	,	,	
		s,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		type,	char,	Input,	,	,	
		vlow,	double,	Input,	,	,	
		vhigh,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vbs,	double,	Input,	,	,	
		ithr,	double,	Input,	,	,	
		vstep,	double,	Input,	,	,	
		nmax,	int,	Input,	,	,	
		slope,	double *,	Output,	,	,	
		kflag,	int *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "PARLIB400_proto.h"
	END USRLIB MODULE INFORMATION
*/
double vtext_400(int d, int g, int s, int sub, char type, double vlow, double vhigh, double vds, double vbs, double ithr, double vstep, int nmax, double * slope, int * kflag);


