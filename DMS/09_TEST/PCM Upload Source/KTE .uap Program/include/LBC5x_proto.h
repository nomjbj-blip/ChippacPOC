/* LBC5x function prototype and KITT header file */

/* COPY OF USRLIB SETTINGS INFORMATION */
/* [VISIBILITY] */
/* HIDDEN=0 */
/* [DEPENDENCIES] */
/* LIBS=-leng_test -lHP4284 -lLBC5 -lprbgen */
/* OBJECTS= */
/* HEADERS= */
/* [NON-KULT SEARCH PATH] */
/* PATHS= */

/* USRLIB MODULE INFORMATION

	MODULE NAME: xbase_po
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 7
	ARGUMENTS:
		emit,	int,	Input,	,	,	
		ba1,	int,	Input,	,	,	
		ba2,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		ve,	double,	Input,	,	,	
		vb2,	double,	Input,	,	,	
		npts,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double xbase_po(int emit, int ba1, int ba2, int subst, double ve, double vb2, double npts);


/* USRLIB MODULE INFORMATION

	MODULE NAME: xbeta2
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
double xbeta2(int em, int base, int coll, int subst, double ic, double vce, double * vbeout, double * icout, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: xbeta_Jc
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 12
	ARGUMENTS:
		emit,	int,	Input,	,	,	
		base,	int,	Input,	,	,	
		coll,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		gnd1,	int,	Input,	,	,	
		gnd2,	int,	Input,	,	,	
		vbstart,	double,	Input,	,	,	
		vbstop,	double,	Input,	,	,	
		vce,	double,	Input,	,	,	
		vsub,	double,	Input,	,	,	
		ic_target,	double,	Input,	,	,	
		vbe_out,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double  xbeta_Jc(int, int, int, int, int, int, double, double, double, double, double, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: xbeta_lpnp
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
double xbeta_lpnp(int em, int base, int coll, int subst, double ic, double vce, double vsub, double * vbeout, double * icout, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: xbvdig
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
		ig,	double,	Input,	,	,	
		udelay,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef_lowercase.h>

#include <math.h>		                
#include "lptdef.h"
#include <stdlib.h>
#include "PARLIB400_proto.h"
	
	END USRLIB MODULE INFORMATION
*/
double xbvdig(int d, int g, int s, int sub, double vdsstart, double vdsstop, int nstep, double ipgm, double ig, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: xbvdii
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
#include "PARLIB400_proto.h"
	
	END USRLIB MODULE INFORMATION
*/
double xbvdii(int d, int g, int s, int sub, double vdsstart, double vdsstop, int nstep, double ipgm, double vg, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: xbvdii_2hi
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 12
	ARGUMENTS:
		d,	int,	Input,	,	,	
		g,	int,	Input,	,	,	
		s,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		hi2,	int,	Input,	,	,	
		vdsstart,	double,	Input,	,	,	
		vdsstop,	double,	Input,	,	,	
		nstep,	int,	Input,	,	,	
		ipgm,	double,	Input,	,	,	
		vg,	double,	Input,	,	,	
		udelay,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef_lowercase.h>

#include <math.h>		                
#include "lptdef.h"
#include <stdlib.h>
#include "PARLIB400_proto.h"
	
	END USRLIB MODULE INFORMATION
*/
double xbvdii_2hi(int d, int g, int s, int sub, int hi2, double vdsstart, double vdsstop, int nstep, double ipgm, double vg, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: xbvdisub
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 8
	ARGUMENTS:
		DRAIN,	int,	Input,	,	,	
		GATE,	int,	Input,	,	,	
		SOURCE,	int,	Input,	,	,	
		SUBST,	int,	Input,	,	,	
		ID,	double,	Input,	,	,	
		VBS,	double,	Input,	,	,	
		ISUB,	double *,	Output,	,	,	
		VDS,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void xbvdisub(int DRAIN, int GATE, int SOURCE, int SUBST, double ID, double VBS, double * ISUB, double * VDS);


/* USRLIB MODULE INFORMATION

	MODULE NAME: xbvdisub_new
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 8
	ARGUMENTS:
		DRAIN,	int,	Input,	,	,	
		GATE,	int,	Input,	,	,	
		SOURCE,	int,	Input,	,	,	
		SUBST,	int,	Input,	,	,	
		IS,	double,	Input,	,	,	
		VBS,	double,	Input,	,	,	
		ISUB,	double *,	Output,	,	,	
		VS,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void xbvdisub_new(int DRAIN, int GATE, int SOURCE, int SUBST, double IS, double VBS, double * ISUB, double * VS);


/* USRLIB MODULE INFORMATION

	MODULE NAME: xbvdss1_2hi
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
#include <lptdef.h>
#include <stdio.h>
#include <lptdef_lowercase.h>

#include <stdlib.h>
#include "PARLIB400_proto.h"
	
	END USRLIB MODULE INFORMATION
*/
double xbvdss1_2hi(int d, int g, int s, int sub, double vdsstart, double vdsstop, int nstep, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: xbvdss1_2hi_rng
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
#include <lptdef.h>
#include <stdio.h>
#include <lptdef_lowercase.h>

#include <stdlib.h>
#include "PARLIB400_proto.h"
	
	END USRLIB MODULE INFORMATION
*/
double xbvdss1_2hi_rng(int d, int g, int s, int sub, double vdsstart, double vdsstop, int nstep, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: xbvdss1_deplpm
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
#include "PARLIB400_proto.h"
	
	END USRLIB MODULE INFORMATION
*/
double xbvdss1_deplpm(int d, int g, int s, int body, int sub, double vdsstart, double vdsstop, double vsub, int nstep, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: xbvdss1_iso
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
#include "PARLIB400_proto.h"
	
	END USRLIB MODULE INFORMATION
*/
double xbvdss1_iso(int d, int g, int s, int sub, int isl, double vdsstart, double vdsstop, int nstep, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: xbvdss2
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
#include "PARLIB400_proto.h"
	
	END USRLIB MODULE INFORMATION
*/
double xbvdss2(int d, int g, int s, int sub, double vdsstart, double vdsstop, int nstep, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: xbvswp2hi_r00
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 16
	ARGUMENTS:
		hi1,	int,	Input,	,	,	
		hi2,	int,	Input,	,	,	
		lo1,	int,	Input,	,	,	
		lo2,	int,	Input,	,	,	
		lo3,	int,	Input,	,	,	
		lo4,	int,	Input,	,	,	
		lo5,	int,	Input,	,	,	
		vhi2common,	char,	Input,	,	,	
		vstart,	double,	Input,	,	,	
		vstop,	double,	Input,	,	,	
		vhi2,	double,	Input,	,	,	
		nstep,	int,	Input,	,	,	
		ipgm,	double,	Input,	,	,	
		udelay,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
		err,	char,	Input,	,	,	
	INCLUDES:
#include <math.h>		                
#include <lptdef.h>
#include <stdio.h>
#include <lptdef_lowercase.h>

#include <stdlib.h>
#include "PARLIB400_proto.h"
	
	END USRLIB MODULE INFORMATION
*/
double xbvswp2hi_r00(int hi1, int hi2, int lo1, int lo2, int lo3, int lo4, int lo5, char vhi2common, double vstart, double vstop, double vhi2, int nstep, double ipgm, double udelay, char type, char err);


/* USRLIB MODULE INFORMATION

	MODULE NAME: xbvswp_float_r00
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 15
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo1,	int,	Input,	,	,	
		lo2,	int,	Input,	,	,	
		lo3,	int,	Input,	,	,	
		lo4,	int,	Input,	,	,	
		lo5,	int,	Input,	,	,	
		lo6,	int,	Input,	,	,	
		float_unused,	char,	Input,	,	,	
		vstart,	double,	Input,	,	,	
		vstop,	double,	Input,	,	,	
		nstep,	int,	Input,	,	,	
		ipgm,	double,	Input,	,	,	
		udelay,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
		err,	char,	Input,	,	,	
	INCLUDES:
#include <math.h>		                
#include <lptdef.h>
#include <stdio.h>
#include <lptdef_lowercase.h>

#include <stdlib.h>
#include "PARLIB400_proto.h"
	
	END USRLIB MODULE INFORMATION
*/
double xbvswp_float_r00(int hi, int lo1, int lo2, int lo3, int lo4, int lo5, int lo6, char float_unused, double vstart, double vstop, int nstep, double ipgm, double udelay, char type, char err);


/* USRLIB MODULE INFORMATION

	MODULE NAME: xbvswp_r00
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 14
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo1,	int,	Input,	,	,	
		lo2,	int,	Input,	,	,	
		lo3,	int,	Input,	,	,	
		lo4,	int,	Input,	,	,	
		lo5,	int,	Input,	,	,	
		lo6,	int,	Input,	,	,	
		vstart,	double,	Input,	,	,	
		vstop,	double,	Input,	,	,	
		nstep,	int,	Input,	,	,	
		ipgm,	double,	Input,	,	,	
		udelay,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
		err,	char,	Input,	,	,	
	INCLUDES:
#include <math.h>		                
#include <lptdef.h>
#include <stdio.h>
#include <lptdef_lowercase.h>

#include <stdlib.h>
#include "PARLIB400_proto.h"
	
	END USRLIB MODULE INFORMATION
*/
double xbvswp_r00(int hi, int lo1, int lo2, int lo3, int lo4, int lo5, int lo6, double vstart, double vstop, int nstep, double ipgm, double udelay, char type, char err);


/* USRLIB MODULE INFORMATION

	MODULE NAME: xCALC_MET
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 6
	ARGUMENTS:
		metal,	int,	Input,	,	,	
		mtl1,	int *,	Output,	,	,	
		mtl2,	int *,	Output,	,	,	
		mtl3,	int *,	Output,	,	,	
		mtl4,	int *,	Output,	,	,	
		mtl5,	int *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void xCALC_MET(int metal, int * mtl1, int * mtl2, int * mtl3, int * mtl4, int * mtl5);


/* USRLIB MODULE INFORMATION

	MODULE NAME: xcaldufshift
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 4
	ARGUMENTS:
		bvd,	D_ARRAY_T,	Input,	,	,	
		npts1,	int,	Input,	,	,	
		shft,	D_ARRAY_T,	Input,	,	,	
		npts2,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
		
	END USRLIB MODULE INFORMATION
*/
double xcaldufshift(double *bvd, int npts1, double *shft, int npts2);


/* USRLIB MODULE INFORMATION

	MODULE NAME: xcap_internal
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
#include <unistd.h>
#include <netdb.h>
#include <cmtr_hp4284.h>
#include <par_util.h>
#define CRTLIM 1.0E-6		                              
#include "ktxe_types.h"
#include "COM_usrlib.h"
#define MAX(a,b)	((a)>(b) ? (a) : (b))
	END USRLIB MODULE INFORMATION
*/
void xcap_internal(int hi, int lo, int sub, double vacc, double * ca, double * ga);


/* USRLIB MODULE INFORMATION

	MODULE NAME: xdiode_beta
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
void xdiode_beta(int anode, int cathode, int sub, double vcat, double ianode, double delaytime, double * icat, double * isub, double * beta);


/* USRLIB MODULE INFORMATION

	MODULE NAME: xdiodebeta3
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 10
	ARGUMENTS:
		source,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		vdrain,	double,	Input,	,	,	
		isource,	double,	Input,	,	,	
		delaytime,	int,	Input,	,	,	
		isub,	double *,	Output,	,	,	
		vgate,	double *,	Output,	,	,	
		beta,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>

#define CRTLIM 200.0e-3


	END USRLIB MODULE INFORMATION
*/
void xdiodebeta3(int source, int gate, int drain, int sub, double vdrain, double isource, int delaytime, double * isub, double * vgate, double * beta);


/* USRLIB MODULE INFORMATION

	MODULE NAME: xerase_eeprom
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
void xerase_eeprom(int drn, int cg, int src, int tun, double vtun, int te);


/* USRLIB MODULE INFORMATION

	MODULE NAME: xev
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
void xev(int e, int b, int c, int s, double ibe, double vstart, double vstop, int npts, double vsub, double * slope, int * iflag, double * r, double * early);


/* USRLIB MODULE INFORMATION

	MODULE NAME: xgds_r00
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 15
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		grnd1,	int,	Input,	,	,	
		grnd2,	int,	Input,	,	,	
		grnd3,	int,	Input,	,	,	
		vgs,	double,	Input,	,	,	
		vds1,	double,	Input,	,	,	
		vds2,	double,	Input,	,	,	
		vbs,	double,	Input,	,	,	
		idlim,	double,	Input,	,	,	
		lo_range,	char,	Input,	,	,	
		width,	double,	Input,	,	,	
		mult,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double xgds_r00(int drain, int gate, int source, int subst, int grnd1, int grnd2, int grnd3, double vgs, double vds1, double vds2, double vbs, double idlim, char lo_range, double width, double mult);


/* USRLIB MODULE INFORMATION

	MODULE NAME: xid1_himp
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
double xid1_himp(int drain, int gate, int source, int subst, int flt, double vgs, double vds, double vbs);


/* USRLIB MODULE INFORMATION

	MODULE NAME: xid1_r00
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 14
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		grnd1,	int,	Input,	,	,	
		grnd2,	int,	Input,	,	,	
		grnd3,	int,	Input,	,	,	
		vgs,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vbs,	double,	Input,	,	,	
		idlim,	double,	Input,	,	,	
		lo_range,	char,	Input,	,	,	
		width,	double,	Input,	,	,	
		mult,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double xid1_r00(int drain, int gate, int source, int subst, int grnd1, int grnd2, int grnd3, double vgs, double vds, double vbs, double idlim, char lo_range, double width, double mult);


/* USRLIB MODULE INFORMATION

	MODULE NAME: xid1_r00_dren
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 14
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		grnd1,	int,	Input,	,	,	
		grnd2,	int,	Input,	,	,	
		grnd3,	int,	Input,	,	,	
		vgs,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vbs,	double,	Input,	,	,	
		idlim,	double,	Input,	,	,	
		lo_range,	char,	Input,	,	,	
		width,	double,	Input,	,	,	
		mult,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double xid1_r00_dren(int drain, int gate, int source, int subst, int grnd1, int grnd2, int grnd3, double vgs, double vds, double vbs, double idlim, char lo_range, double width, double mult);


/* USRLIB MODULE INFORMATION

	MODULE NAME: xid1g
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 7
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		igs,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vbs,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double xid1g(int drain, int gate, int source, int subst, double igs, double vds, double vbs);


/* USRLIB MODULE INFORMATION

	MODULE NAME: xid2
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
double xid2(int drain, int gate, int source, int subst, double vgs, double vds, double vbs);


/* USRLIB MODULE INFORMATION

	MODULE NAME: xid_ifsrc
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 5
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		isrc,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double xid_ifsrc(int drain, int gate, int source, int subst, double isrc);


/* USRLIB MODULE INFORMATION

	MODULE NAME: xiso_leak
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
double xiso_leak(int drain, int gate, int source, int subst, int grnd1, int grnd2, double ids, double vs);


/* USRLIB MODULE INFORMATION

	MODULE NAME: xiso_leak_bck
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
double xiso_leak_bck(int drain, int gate, int source, int back, int subst, int grnd1, int grnd2, double ibs, double vd);


/* USRLIB MODULE INFORMATION

	MODULE NAME: xisub_r00
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 13
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		grnd1,	int,	Input,	,	,	
		grnd2,	int,	Input,	,	,	
		grnd3,	int,	Input,	,	,	
		vgs,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vbs,	double,	Input,	,	,	
		lo_range,	char,	Input,	,	,	
		width,	double,	Input,	,	,	
		mult,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double xisub_r00(int drain, int gate, int source, int subst, int grnd1, int grnd2, int grnd3, double vgs, double vds, double vbs, char lo_range, double width, double mult);


/* USRLIB MODULE INFORMATION

	MODULE NAME: xjfet_vds
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 9
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		gnd1,	int,	Input,	,	,	
		gnd2,	int,	Input,	,	,	
		gnd3,	int,	Input,	,	,	
		vd,	double,	Input,	,	,	
		is,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double xjfet_vds(int drain, int gate, int source, int subst, int gnd1, int gnd2, int gnd3, double vd, double is);


/* USRLIB MODULE INFORMATION

	MODULE NAME: xleak4
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
double xleak4(int hi, int lo1, int lo2, int subst, double v);


/* USRLIB MODULE INFORMATION

	MODULE NAME: xleak4_dren
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
double xleak4_dren(int hi, int lo1, int lo2, int subst, double v);


/* USRLIB MODULE INFORMATION

	MODULE NAME: xleak4_lc
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
double xleak4_lc(int hi, int lo1, int lo2, int subst, double v, double ilim);


/* USRLIB MODULE INFORMATION

	MODULE NAME: xleak_dren
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
double xleak_dren(int hi, int lo, int subst, double v, double ilim);


/* USRLIB MODULE INFORMATION

	MODULE NAME: xleak_iso
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
double xleak_iso(int hi, int lo1, int lo2, int subst, int isl, double v);


/* USRLIB MODULE INFORMATION

	MODULE NAME: xleak_r00
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 13
	ARGUMENTS:
		hi1,	int,	Input,	,	,	
		hi2,	int,	Input,	,	,	
		lo1,	int,	Input,	,	,	
		lo2,	int,	Input,	,	,	
		lo3,	int,	Input,	,	,	
		lo4,	int,	Input,	,	,	
		lo5,	int,	Input,	,	,	
		float_unused,	char,	Input,	,	,	
		lo_range,	char,	Input,	,	,	
		log,	char,	Input,	,	,	
		w,	double,	Input,	,	,	
		v,	double,	Input,	,	,	
		icomp,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double xleak_r00(int hi1, int hi2, int lo1, int lo2, int lo3, int lo4, int lo5, char float_unused, char lo_range, char log, double w, double v, double icomp);


/* USRLIB MODULE INFORMATION

	MODULE NAME: xpgm_eeprom
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
void xpgm_eeprom(int drn, int cg, int src, int tun, double vcg, int tp);


/* USRLIB MODULE INFORMATION

	MODULE NAME: xpn2swp
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
double xpn2swp(int hi, int lo, int subst, double vstart, double vstop, int nstep, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: xpn2swp_jm
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
double xpn2swp_jm(int hi, int lo, int subst, double vstart, double vstop, int nstep, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: xpn4hiswp
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
double xpn4hiswp(int hi1, int hi2, int hi3, int hi4, int lo, double vstart, double vstop, int nstep, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: xres
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
double xres(int hi, int lo, int subst, double itest);


/* USRLIB MODULE INFORMATION

	MODULE NAME: xres4
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
double xres4(int hi_source, int hi_meas, int lo_source, int lo_meas, int subst, double itest);


/* USRLIB MODULE INFORMATION

	MODULE NAME: xres4v
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
double xres4v(int his, int him, int los, int lom, int subst, double vtest);


/* USRLIB MODULE INFORMATION

	MODULE NAME: xres_r00
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 8
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		grnd1,	int,	Input,	,	,	
		grnd2,	int,	Input,	,	,	
		grnd3,	int,	Input,	,	,	
		w,	double,	Input,	,	,	
		l,	double,	Input,	,	,	
		itest,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double xres_r00(int hi, int lo, int grnd1, int grnd2, int grnd3, double w, double l, double itest);


/* USRLIB MODULE INFORMATION

	MODULE NAME: xresv
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
double xresv(int hi, int lo, int subst, double v);


/* USRLIB MODULE INFORMATION

	MODULE NAME: xresv_r00
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 8
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		grnd1,	int,	Input,	,	,	
		grnd2,	int,	Input,	,	,	
		grnd3,	int,	Input,	,	,	
		w,	double,	Input,	,	,	
		l,	double,	Input,	,	,	
		v,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>

#define CRTLIM 100.0E-3

	END USRLIB MODULE INFORMATION
*/
double xresv_r00(int hi, int lo, int grnd1, int grnd2, int grnd3, double w, double l, double v);


/* USRLIB MODULE INFORMATION

	MODULE NAME: xron
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
double xron(int DRN1, int DRN2, int GATE, int SRC1, int SRC2, int SUB, double VDS, double VGS);


/* USRLIB MODULE INFORMATION

	MODULE NAME: xron5
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
double xron5(int DRN1, int DRN2, int GATE, int SRC1, int SRC2, int BODY, int SUB, double VDS, double VGS);


/* USRLIB MODULE INFORMATION

	MODULE NAME: xsslp_r00
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 16
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		grnd1,	int,	Input,	,	,	
		grnd2,	int,	Input,	,	,	
		grnd3,	int,	Input,	,	,	
		vlow,	double,	Input,	,	,	
		vhigh,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vbs,	double,	Input,	,	,	
		ithr1,	double,	Input,	,	,	
		ithr2,	double,	Input,	,	,	
		ilim,	double,	Input,	,	,	
		mult,	double,	Input,	,	,	
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
double xsslp_r00(int drain, int gate, int source, int subst, int grnd1, int grnd2, int grnd3, double vlow, double vhigh, double vds, double vbs, double ithr1, double ithr2, double ilim, double mult, int niter);


/* USRLIB MODULE INFORMATION

	MODULE NAME: xvf
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
double xvf(int hi, int lo, int subst, double itest);


/* USRLIB MODULE INFORMATION

	MODULE NAME: xvf_3gnd
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 5
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		lo2,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		itest,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double xvf_3gnd(int hi, int lo, int lo2, int subst, double itest);


/* USRLIB MODULE INFORMATION

	MODULE NAME: xvp_1gate_2hi
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
double xvp_1gate_2hi(int drain, int gate, int source, int sub, double vglow, double vghigh, double vd, int nstep, double ipgm, double udelay);


/* USRLIB MODULE INFORMATION

	MODULE NAME: xvtati
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
double xvtati(int drain, int gate, int source, int subst, double vlow, double vhigh, double vds, double vbs, double ithr, int niter);


/* USRLIB MODULE INFORMATION

	MODULE NAME: xvtext4
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
double xvtext4(int drain, int gate, int source, int subst, double w, double l, double vlow, double vhigh, double vds, double vbs, double ithr, double vstep, int npts, double * slope, int * kflag);


/* USRLIB MODULE INFORMATION

	MODULE NAME: xvtext4_dren
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
double xvtext4_dren(int drain, int gate, int source, int subst, double w, double l, double vlow, double vhigh, double vds, double vbs, double ithr, double vstep, int npts, double * slope, int * kflag);


/* USRLIB MODULE INFORMATION

	MODULE NAME: xvtext4_iso
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
double xvtext4_iso(int drain, int gate, int source, int subst, int isl, double w, double l, double vlow, double vhigh, double vds, double vbs, double ithr, double vstep, int npts, double * slope, int * kflag);


/* USRLIB MODULE INFORMATION

	MODULE NAME: xvtext4_r00
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 18
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		grnd1,	int,	Input,	,	,	
		grnd2,	int,	Input,	,	,	
		grnd3,	int,	Input,	,	,	
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
double xvtext4_r00(int drain, int gate, int source, int subst, int grnd1, int grnd2, int grnd3, double w, double l, double vlow, double vhigh, double vds, double vbs, double ithr, double vstep, int npts, double * slope, int * kflag);


/* USRLIB MODULE INFORMATION

	MODULE NAME: xvtexts
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
double xvtexts(int drain, int gate, int source, int subst, double w, double l, double vlow, double vhigh, double vds, double vbs, double ithr, double vstep, int npts, double * slope, int * kflag);


/* USRLIB MODULE INFORMATION

	MODULE NAME: xvtexts_r00
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 18
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		grnd1,	int,	Input,	,	,	
		grnd2,	int,	Input,	,	,	
		grnd3,	int,	Input,	,	,	
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
double xvtexts_r00(int drain, int gate, int source, int subst, int grnd1, int grnd2, int grnd3, double w, double l, double vlow, double vhigh, double vds, double vbs, double ithr, double vstep, int npts, double * slope, int * kflag);


/* USRLIB MODULE INFORMATION

	MODULE NAME: xvtfswp
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
double xvtfswp(int drain, int gate, int source, int sub, double vlow, double vhigh, double vd, int nstep, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: xvtfswp_himp
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
double xvtfswp_himp(int drain, int gate, int source, int sub, int flt, double vlow, double vhigh, double vd, int nstep, double ipgm, double udelay, char type);


