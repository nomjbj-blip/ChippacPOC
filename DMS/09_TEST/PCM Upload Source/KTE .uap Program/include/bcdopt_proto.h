/* bcdopt function prototype and KITT header file */

/* COPY OF USRLIB SETTINGS INFORMATION */
/* [VISIBILITY] */
/* HIDDEN=0 */
/* [DEPENDENCIES] */
/* LIBS=-ldb_opt -lktest */
/* OBJECTS= */
/* HEADERS= */
/* [NON-KULT SEARCH PATH] */
/* PATHS= */

/* USRLIB MODULE INFORMATION

	MODULE NAME: BREAKV_check
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
		delaytime,	double,	Input,	,	,	
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
void BREAKV_check(char * devname, int top, int bot, int sub, int chuckcon, char type, double imax, double vmin, double vmax, double vf, double intrange, double delaytime, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: id1_opt
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 12
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		body,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		plc_val,	double,	Input,	,	,	
		ilimit,	double,	Input,	,	,	
		lo_range,	double,	Input,	,	,	
		delay_time,	double,	Input,	,	,	
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
double id1_opt(int drain, int gate, int source, int body, int subst, double plc_val, double ilimit, double lo_range, double delay_time, double vgs, double vds, double vbs);


/* USRLIB MODULE INFORMATION

	MODULE NAME: leak_opt
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 10
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo1,	int,	Input,	,	,	
		lo2,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		isl,	int,	Input,	,	,	
		floatpin_set,	int,	Input,	,	,	
		lrange,	double,	Input,	,	,	
		delaytime,	double,	Input,	,	,	
		integ,	double,	Input,	,	,	
		v,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double leak_opt(int hi, int lo1, int lo2, int subst, int isl, int floatpin_set, double lrange, double delaytime, double integ, double v);


/* USRLIB MODULE INFORMATION

	MODULE NAME: pn2swp_check3
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
double  pn2swp_check3(int, int, int, double, double, double, int, double, double, char);

/* USRLIB MODULE INFORMATION

	MODULE NAME: pn2swp_check3_kstmp
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 11
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
		flaggg,	double *,	Output,	,	,	
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
double  pn2swp_check3_kstmp(int, int, int, double, double, double, int, double, double, char, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: pn2swp_check3_kstmp_r
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 11
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
		flaggg,	double *,	Output,	,	,	
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
double  pn2swp_check3_kstmp_r(int, int, int, double, double, double, int, double, double, char, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: pn2swp_check3_kstmp_r2
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 11
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
		flaggg,	double *,	Output,	,	,	
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
double  pn2swp_check3_kstmp_r2(int, int, int, double, double, double, int, double, double, char, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: vtext4_10m
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
double  vtext4_10m(int, int, int, int, double, double, double, double, double, double, double, double, int, double *, int *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: vtext4_opt
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
#include <ksox_def.h>
	END USRLIB MODULE INFORMATION
*/
double vtext4_opt(int drain, int gate, int source, int subst, double w, double l, double vlow, double vhigh, double vds, double vbs, double ithr, double vstep, int npts, double * slope, int * kflag);


