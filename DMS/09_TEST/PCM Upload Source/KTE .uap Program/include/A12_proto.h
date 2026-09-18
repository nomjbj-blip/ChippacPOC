/* A12 function prototype and KITT header file */

/* COPY OF USRLIB SETTINGS INFORMATION */
/* [VISIBILITY] */
/* HIDDEN=0 */
/* [DEPENDENCIES] */
/* LIBS=-lHP4284 -lPARLIB400 */
/* OBJECTS= */
/* HEADERS= */
/* [NON-KULT SEARCH PATH] */
/* PATHS= */

/* USRLIB MODULE INFORMATION

	MODULE NAME: bvdss1_400_m
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
double  bvdss1_400_m(int, int, int, int, double, double, int, double, double, char);

/* USRLIB MODULE INFORMATION

	MODULE NAME: bvdss_400_m
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
#include "lptdef.h"
#include "PARLIB400_proto.h"

	END USRLIB MODULE INFORMATION
*/
double  bvdss_400_m(int, int, int, int, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: cbd
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 6
	ARGUMENTS:
		pin1,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		sign,	int,	Input,	,	,	
		vlf,	double *,	Output,	,	,	
		vbd,	double *,	Output,	,	,	
		ibd,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <stdlib.h>
#include <string.h>
#include <prb.h>
	END USRLIB MODULE INFORMATION
*/
int  cbd(int, int, int, double *, double *, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: cbd2
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 4
	ARGUMENTS:
		pin1,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		sign,	int,	Input,	,	,	
		vlf,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <stdlib.h>
#include <string.h>
#include <prb.h>
	END USRLIB MODULE INFORMATION
*/
int  cbd2(int, int, int, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: cbd2_20
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 4
	ARGUMENTS:
		pin1,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		sign,	int,	Input,	,	,	
		vlf,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <stdlib.h>
#include <string.h>
#include <prb.h>
	END USRLIB MODULE INFORMATION
*/
int  cbd2_20(int, int, int, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: cbd2_30
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 4
	ARGUMENTS:
		pin1,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		sign,	int,	Input,	,	,	
		vlf,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <stdlib.h>
#include <string.h>
#include <prb.h>
	END USRLIB MODULE INFORMATION
*/
int  cbd2_30(int, int, int, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: dis
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
void  dis();

/* USRLIB MODULE INFORMATION

	MODULE NAME: idsat_400_m
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
#include <math.h>	                
#include "lptdef.h"

	END USRLIB MODULE INFORMATION
*/
double  idsat_400_m(int, int, int, int, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: idsat_400_mg
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
#include <math.h>	                
#include "lptdef.h"

	END USRLIB MODULE INFORMATION
*/
double  idsat_400_mg(int, int, int, int, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: idsat_400h
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
#include <math.h>	                
#include "lptdef.h"

#define CRTLIM 100.0E-3
	END USRLIB MODULE INFORMATION
*/
double  idsat_400h(int, int, int, int, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: isubmx_400_m
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
#include "lptdef.h"
#include <stdlib.h>
#include "PARLIB400_proto.h"

#define MAXPTS 100		                                   
#define CRTLIM 10.E-3		                         
#define ICOMP 0.98*CRTLIM	                        
	END USRLIB MODULE INFORMATION
*/
void  isubmx_400_m(int, int, int, int, double, double, double, double, int, double *, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: offset
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 3
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		vbias,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <prb_proto.h>
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include        <string.h>
#include        "prb.h"
#include        "prb_msg.h"
                              
#include "/opt/kiS600/usrlib/prbTSK9_proto.h"
#include        "prb_extern.h"
#define CRTLIM 1.0E-6
	END USRLIB MODULE INFORMATION
*/
double offset(int hi, int lo, double vbias);


/* USRLIB MODULE INFORMATION

	MODULE NAME: resi2
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 4
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		i,	double,	Input,	,	,	
	INCLUDES:
#include <math.h>
#include "lptdef.h"

#define CRTLIM 5.0
	END USRLIB MODULE INFORMATION
*/
double  resi2(int, int, int, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: resi4
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 8
	ARGUMENTS:
		hi1,	int,	Input,	,	,	
		hi2,	int,	Input,	,	,	
		lo1,	int,	Input,	,	,	
		lo2,	int,	Input,	,	,	
		type,	char,	Input,	,	,	
		subst,	int,	Input,	,	,	
		vsubst,	double,	Input,	,	,	
		itest,	double,	Input,	,	,	
	INCLUDES:
#include <math.h>
#include "lptdef.h"

#define CRTLIM 15.0
	END USRLIB MODULE INFORMATION
*/
double  resi4(int, int, int, int, char, int, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: resv_400_m
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

#define CRTLIM 10.0E-3

	END USRLIB MODULE INFORMATION
*/
double  resv_400_m(int, int, int, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: resv_400h
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
double  resv_400h(int, int, int, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: resv_400h_d10
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
double  resv_400h_d10(int, int, int, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: resv_400h_d100
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
double  resv_400h_d100(int, int, int, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: resv_400h_d500
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
double  resv_400h_d500(int, int, int, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: rvdp_400_m
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
#include "lptdef.h"
#include <stdlib.h>
#include "PARLIB400_proto.h"

#define VLTLIM 20.0		                              
	END USRLIB MODULE INFORMATION
*/
double  rvdp_400_m(int, int, int, int, int, double, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: SaveCapOffSet
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 5
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		type,	char,	Input,	,	,	
		vbias,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <ktxe_types.h>
#include <COM_usrlib.h>
	END USRLIB MODULE INFORMATION
*/
void SaveCapOffSet(int hi, int lo, int sub, char type, double vbias);


/* USRLIB MODULE INFORMATION

	MODULE NAME: tdelay_400_m
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
double  tdelay_400_m(int, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: ti_bvdss1
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 8
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		ipgm,	double,	Input,	,	,	
		vlim,	double,	Input,	,	,	
		slew,	double,	Input,	,	,	
		dly,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <prb.h>
#include <lptdef.h>
#include <math.h>
#include <lptdef_lowercase.h>
	END USRLIB MODULE INFORMATION
*/
double  ti_bvdss1(int, int, int, int, double, double, double, int);

/* USRLIB MODULE INFORMATION

	MODULE NAME: ti_bvdss1_400
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 8
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		ipgm,	double,	Input,	,	,	
		vlim,	double,	Input,	,	,	
		slew,	double,	Input,	,	,	
		dly,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <prb.h>
#include <lptdef.h>
#include <math.h>
#include <lptdef_lowercase.h>
	END USRLIB MODULE INFORMATION
*/
double  ti_bvdss1_400(int, int, int, int, double, double, double, int);

/* USRLIB MODULE INFORMATION

	MODULE NAME: ti_bvdss1_m
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 8
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		ipgm,	double,	Input,	,	,	
		vlim,	double,	Input,	,	,	
		slew,	double,	Input,	,	,	
		dly,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <prb.h>
#include <lptdef.h>
#include <math.h>
#include <lptdef_lowercase.h>
	END USRLIB MODULE INFORMATION
*/
double  ti_bvdss1_m(int, int, int, int, double, double, double, int);

/* USRLIB MODULE INFORMATION

	MODULE NAME: ti_bvdss2
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
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <prb.h>
#include <lptdef.h>
#include <math.h>
#include <lptdef_lowercase.h>
	END USRLIB MODULE INFORMATION
*/
double  ti_bvdss2(int, int, int, int, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: toxc
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 6
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		vbias,	double,	Input,	,	,	
		area,	double,	Input,	,	,	
		offset,	double,	Input,	,	,	
	INCLUDES:
#include <math.h>
#include "lptdef.h"
#include <stdlib.h>

#define CAPFREQ 1.0E6		                    
#define CRTLIM 1.0E-6		                              
	END USRLIB MODULE INFORMATION
*/
double toxc(int hi, int lo, int sub, double vbias, double area, double offset);


/* USRLIB MODULE INFORMATION

	MODULE NAME: vtati_400_m
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
double  vtati_400_m(int, int, int, int, double, double, double, double, double, int);

/* USRLIB MODULE INFORMATION

	MODULE NAME: vton
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 13
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		type,	char,	Input,	,	,	
		vgsmin,	double,	Input,	,	,	
		vgsmax,	double,	Input,	,	,	
		vgstp,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vbs,	double,	Input,	,	,	
		test,	char,	Input,	,	,	
		vt,	double *,	Output,	,	,	
		beta,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
int  vton(int, int, int, int, char, double, double, double, double, double, char, double *, double *);

