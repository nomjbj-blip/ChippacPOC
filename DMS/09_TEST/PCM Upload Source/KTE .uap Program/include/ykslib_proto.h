/* ykslib function prototype and KITT header file */

/* COPY OF USRLIB SETTINGS INFORMATION */
/* [VISIBILITY] */
/* HIDDEN=0 */
/* [DEPENDENCIES] */
/* LIBS=-lA07 -ldb_opt -lHP4284 -lktest -lLBC5 -loptlib */
/* OBJECTS= */
/* HEADERS= */
/* [NON-KULT SEARCH PATH] */
/* PATHS= */

/* USRLIB MODULE INFORMATION

	MODULE NAME: beta2_tester
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
#include <unistd.h>
#include <netdb.h>
	END USRLIB MODULE INFORMATION
*/
double  beta2_tester(int, int, int, int, double, double, double *, double *, char);

/* USRLIB MODULE INFORMATION

	MODULE NAME: beta3
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
double beta3(int em, int base, int coll, int subst, double ic, double vce, double * vbeout, double * icout, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: BREAKV_tsweep
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 13
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		top,	int,	Input,	,	,	
		bot,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		type,	char,	Input,	,	,	
		imax,	double,	Input,	,	,	
		vmin,	double,	Input,	,	,	
		vmax,	double,	Input,	,	,	
		vf,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		nstep,	int,	Input,	,	,	
		result,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#include "lptdef.h"
#include <tidp1.h>
#include "LBC5_proto.h"
#include <stdlib.h>
#include <ktxe_proto.h>
#include <ksox_def.h>
	END USRLIB MODULE INFORMATION
*/
void  BREAKV_tsweep(char *, int, int, int, int, char, double, double, double, double, double, int, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: bvcex_ext_c
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
#include <stdlib.h>
#include "LBC5_proto.h"
#include <stdio.h>	
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <par_util.h>
#include <stdlib.h>
#include <ktxe_proto.h>
#include <ksox_def.h>
#include <kui_proto.h>
	END USRLIB MODULE INFORMATION
*/
double  bvcex_ext_c(int, int, int, int, int, double, double, double, char, double, double, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: dpt
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 15
	ARGUMENTS:
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
double dpt(int drain, int gate, int source, int subst, double w, double l, double vlow, double vhigh, double vds, double vbs, double ithr, double vstep, int npts, double * slope, int * kflag);


/* USRLIB MODULE INFORMATION

	MODULE NAME: dpt5
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 17
	ARGUMENTS:
		drain,	int,	Input,	-1,	,	
		gate,	int,	Input,	-1,	,	
		source,	int,	Input,	-1,	,	
		body,	int,	Input,	-1,	,	
		subst,	int,	Input,	-1,	,	
		g1,	int,	Input,	-1,	,	
		w,	double,	Input,	1,	,	
		l,	double,	Input,	1,	,	
		vlow,	double,	Input,	0,	,	
		vhigh,	double,	Input,	2,	,	
		vds,	double,	Input,	0.1,	,	
		vbs,	double,	Input,	0,	,	
		ithr,	double,	Input,	1e-06,	,	
		vstep,	double,	Input,	0.1,	,	
		npts,	int,	Input,	100,	,	
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
double dpt5(int drain, int gate, int source, int body, int subst, int g1, double w, double l, double vlow, double vhigh, double vds, double vbs, double ithr, double vstep, int npts, double * slope, int * kflag);


/* USRLIB MODULE INFORMATION

	MODULE NAME: GSite
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 2
	ARGUMENTS:
		MaxSite,	int,	Input,	5,	,	
		ItemName,	char *,	Input,	"aa",	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <ksox_def.h>
#include<sys/types.h>
#include<sys/stat.h>
#include<string.h>
#include<stdlib.h>
	END USRLIB MODULE INFORMATION
*/
int GSite(int MaxSite, char * ItemName);


/* USRLIB MODULE INFORMATION

	MODULE NAME: id1_5g
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
double id1_5g(int drain, int gate, int source, int body, int subst, double vgs, double vds, double vbs);


/* USRLIB MODULE INFORMATION

	MODULE NAME: id1_9g
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 12
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		body,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		lo1,	int,	Input,	,	,	
		lo2,	int,	Input,	,	,	
		lo3,	int,	Input,	,	,	
		lo4,	int,	Input,	,	,	
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
double id1_9g(int drain, int gate, int source, int body, int subst, int lo1, int lo2, int lo3, int lo4, double vgs, double vds, double vbs);


/* USRLIB MODULE INFORMATION

	MODULE NAME: id1_ks
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		pins,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void  id1_ks(int);

/* USRLIB MODULE INFORMATION

	MODULE NAME: ID_mos_4p
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
		vds,	double,	Input,	,	,	
		idrange,	double,	Input,	,	,	
		vgs,	double,	Input,	,	,	
		vbs,	double,	Input,	,	,	
		vss,	double,	Input,	,	,	
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
	END USRLIB MODULE INFORMATION
*/
void ID_mos_4p(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vds, double idrange, double vgs, double vbs, double vss, double multiplier, double intrange, double mrange, double lorange, double * result, double * result2, double * result3, double * result4);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ks
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
double ks(int hi, int lo, int subst, double itest);


/* USRLIB MODULE INFORMATION

	MODULE NAME: kstest
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 3
	ARGUMENTS:
		input,	double,	Input,	,	,	
		devname,	char *,	Input,	,	,	
		output,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <unistd.h>
#include <netdb.h>
#include <string.h>
	END USRLIB MODULE INFORMATION
*/
double  kstest(double, char *, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: kstest2
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 3
	ARGUMENTS:
		input,	double,	Input,	,	,	
		t1,	int,	Input,	,	,	
		output,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <unistd.h>
#include <netdb.h>
#include <string.h>
	END USRLIB MODULE INFORMATION
*/
double  kstest2(double, int, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: kstest3
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 3
	ARGUMENTS:
		devn,	char *,	Input,	,	,	
		input,	double,	Input,	,	,	
		output,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <unistd.h>
#include <netdb.h>
#include <string.h>

	END USRLIB MODULE INFORMATION
*/
double  kstest3(char *, double, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: kstest4
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 3
	ARGUMENTS:
		devn,	char *,	Input,	,	,	
		ksks,	char *,	Input,	,	,	
		output,	char *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <unistd.h>
#include <netdb.h>
#include <string.h>
#include "kui_proto.h"
#include "ktxe_types.h"
#include "COM_usrlib.h"

	END USRLIB MODULE INFORMATION
*/
double  kstest4(char *, char *, char *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: leak_nati
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
double leak_nati(int drain, int gate, int source, int subst, double vgs, double vds, double vbs);


/* USRLIB MODULE INFORMATION

	MODULE NAME: LKG_pn2swp_ext
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 11
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		vstart,	double,	Input,	,	,	
		vstop,	double,	Input,	,	,	
		nstep,	int,	Input,	,	,	
		lkg_vtg,	double,	Input,	,	,	
		ipgm,	double,	Input,	,	,	
		udelay,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
		Ir,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double  LKG_pn2swp_ext(int, int, int, double, double, int, double, double, double, char, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: pin
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 10
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		vgs,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vsstart,	double,	Input,	,	,	
		vsstop,	double,	Input,	,	,	
		vsstep,	double,	Input,	,	,	
		Vpin,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void pin(int drain, int gate, int source, int sub, double vgs, double vds, double vsstart, double vsstop, double vsstep, double * Vpin);


/* USRLIB MODULE INFORMATION

	MODULE NAME: pin2
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
double pin2(int drain, int gate, int source, int subst, double w, double l, double vlow, double vhigh, double vds, double vbs, double ithr, double vstep, int npts, double * slope, int * kflag);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PIN_GND
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		pin_ea,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void  PIN_GND(int);

/* USRLIB MODULE INFORMATION

	MODULE NAME: pn2swp_check3_confirm
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 10
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		target_val,	double,	Input,	,	,	
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
double  pn2swp_check3_confirm(int, int, int, double, double, double, int, double, double, char);

/* USRLIB MODULE INFORMATION

	MODULE NAME: pn2swp_check3_confirm2
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 10
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		target_val,	double,	Input,	,	,	
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
#include <unistd.h>
#include <netdb.h>
#include <stdlib.h>
	END USRLIB MODULE INFORMATION
*/
double  pn2swp_check3_confirm2(int, int, int, double, double, double, int, double, double, char);

/* USRLIB MODULE INFORMATION

	MODULE NAME: pn2swp_miho2_r1
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
double  pn2swp_miho2_r1(int, int, int, double, double, double, double, double, char);

/* USRLIB MODULE INFORMATION

	MODULE NAME: pn2swp_swp_log_Range_4
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 10
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		body,	int,	Input,	,	,	
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
double pn2swp_swp_log_Range_4(int hi, int lo, int subst, int body, double vstart, double vstop, int nstep, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: pn2swp_swp_log_Range_x
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
double  pn2swp_swp_log_Range_x(int, int, int, double, double, int, double, double, char);

/* USRLIB MODULE INFORMATION

	MODULE NAME: pn2swp_swp_log_Range_x_vg
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 11
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		vg,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		vstart,	double,	Input,	,	,	
		vstop,	double,	Input,	,	,	
		nstep,	int,	Input,	,	,	
		Vgforce,	double,	Input,	,	,	
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
double  pn2swp_swp_log_Range_x_vg(int, int, int, int, double, double, int, double, double, double, char);

/* USRLIB MODULE INFORMATION

	MODULE NAME: pn2swp_swp_out_VHF
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 12
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		vstart,	double,	Input,	,	,	
		vstop,	double,	Input,	,	,	
		nstep,	int,	Input,	,	,	
		ipgm,	double,	Input,	,	,	
		Ivhf,	double,	Input,	,	,	
		udelay,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
		VLF,	double *,	Output,	,	,	
		VHF,	double *,	Output,	,	,	
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
double  pn2swp_swp_out_VHF(int, int, int, double, double, int, double, double, double, char, double *, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: pot
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 10
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		vgs,	double,	Input,	,	,	
		vdpot,	double,	Input,	,	,	
		vsstart,	double,	Input,	,	,	
		vsstop,	double,	Input,	,	,	
		vsstep,	double,	Input,	,	,	
		Vpot,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void pot(int drain, int gate, int source, int sub, double vgs, double vdpot, double vsstart, double vsstop, double vsstep, double * Vpot);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Prog_OTP_SMU
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 10
	ARGUMENTS:
		V_PulsePin,	int,	Input,	,	,	
		V_ForcePin,	int,	Input,	,	,	
		GroundPin1,	int,	Input,	,	,	
		GroundPin2,	int,	Input,	,	,	
		GroundPin3,	int,	Input,	,	,	
		GroundPin4,	int,	Input,	,	,	
		VoltageForce,	double,	Input,	,	,	
		Pulse_high,	double,	Input,	,	,	
		Pulse_width,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "COM_usrlib.h"
	END USRLIB MODULE INFORMATION
*/
void Prog_OTP_SMU(int V_PulsePin, int V_ForcePin, int GroundPin1, int GroundPin2, int GroundPin3, int GroundPin4, double VoltageForce, double Pulse_high, double Pulse_width, double holdtime);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Single_Vtsweep_5
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 14
	ARGUMENTS:
		cg,	int,	Input,	,	,	
		sg,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		cgstart,	double,	Input,	,	,	
		cgstop,	double,	Input,	,	,	
		cgstep,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vsg,	double,	Input,	,	,	
		vwel,	double,	Input,	,	,	
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
void Single_Vtsweep_5(int cg, int sg, int drain, int source, int well, double cgstart, double cgstop, double cgstep, double vds, double vsg, double vwel, double idtarget, double * vth, int debug_print);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Single_Vtsweep_9
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 18
	ARGUMENTS:
		cg,	int,	Input,	,	,	
		sg,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		lo1,	int,	Input,	,	,	
		lo2,	int,	Input,	,	,	
		lo3,	int,	Input,	,	,	
		lo4,	int,	Input,	,	,	
		cgstart,	double,	Input,	,	,	
		cgstop,	double,	Input,	,	,	
		cgstep,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vsg,	double,	Input,	,	,	
		vwel,	double,	Input,	,	,	
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
void Single_Vtsweep_9(int cg, int sg, int drain, int source, int well, int lo1, int lo2, int lo3, int lo4, double cgstart, double cgstop, double cgstep, double vds, double vsg, double vwel, double idtarget, double * vth, int debug_print);


/* USRLIB MODULE INFORMATION

	MODULE NAME: swp_I4lead
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 13
	ARGUMENTS:
		swppin,	int,	Input,	,	,	
		readpin1,	int,	Input,	,	,	
		readpin2,	int,	Input,	,	,	
		readpin3,	int,	Input,	,	,	
		vstart,	double,	Input,	,	,	
		vstop,	double,	Input,	,	,	
		nstep,	int,	Input,	,	,	
		readforce1,	double,	Input,	,	,	
		readforce2,	double,	Input,	,	,	
		readforce3,	double,	Input,	,	,	
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
double swp_I4lead(int swppin, int readpin1, int readpin2, int readpin3, double vstart, double vstop, int nstep, double readforce1, double readforce2, double readforce3, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: vramp_MTA4
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 12
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		vgate,	double,	Input,	,	,	
		vsource,	double,	Input,	,	,	
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
double vramp_MTA4(int drain, int gate, int source, int subst, double vgate, double vsource, double vstart, double vstop, int nstep, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: vtsslp_mos_re
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 21
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
		idsearch,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vbs,	double,	Input,	,	,	
		multiplier,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		delaytime,	double,	Input,	,	,	
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
#define MAX(a,b)	((a)>(b) ? (a) : (b))
#define CDELAY  125.E-12
#define ILEAK   20.E-12 
	END USRLIB MODULE INFORMATION
*/
void vtsslp_mos_re(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vmin, double vmax, double idsearch, double vds, double vbs, double multiplier, double intrange, double delaytime, double mrange, double lorange, double * result);


