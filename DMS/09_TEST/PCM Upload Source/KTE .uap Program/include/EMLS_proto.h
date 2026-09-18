/* EMLS function prototype and KITT header file */

/* COPY OF USRLIB SETTINGS INFORMATION */
/* [VISIBILITY] */
/* HIDDEN=0 */
/* [DEPENDENCIES] */
/* LIBS=-lA07 -lHP4284 -ljay -lktest -lLBC5 */
/* OBJECTS= */
/* HEADERS= */
/* [NON-KULT SEARCH PATH] */
/* PATHS= */

/* USRLIB MODULE INFORMATION

	MODULE NAME: alg_4t_if
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 20
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		f1,	int,	Input,	,	,	
		f2,	int,	Input,	,	,	
		s1,	int,	Input,	,	,	
		s2,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		bulk,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		area,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
		iforce,	double,	Input,	,	,	
		vlimit,	double,	Input,	,	,	
		vsub,	double,	Input,	,	,	
		delaytime,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		Rc,	double *,	Output,	,	,	
		R1,	double *,	Output,	,	,	
		R2,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void alg_4t_if(char * devname, int f1, int f2, int s1, int s2, int sub, int bulk, int chuckcon, double area, char type, double iforce, double vlimit, double vsub, double delaytime, double intrange, double mrange, double lorange, double * Rc, double * R1, double * R2);


/* USRLIB MODULE INFORMATION

	MODULE NAME: alg_breakv
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 16
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		top,	int,	Input,	,	,	
		bot,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		area,	double,	Input,	,	,	
		per,	double,	Input,	,	,	
		captype,	int,	Input,	,	,	
		btype,	char,	Input,	,	,	
		imax,	double,	Input,	,	,	
		vmin,	double,	Input,	,	,	
		vmax,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		Bdv,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void alg_breakv(char * devname, int top, int bot, int sub, int chuckcon, double area, double per, int captype, char btype, double imax, double vmin, double vmax, double intrange, double mrange, double lorange, double * Bdv);


/* USRLIB MODULE INFORMATION

	MODULE NAME: alg_breakv_corr
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 16
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
		intrange,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		Bdv,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void alg_breakv_corr(char * devname, int top, int bot, int sub, int chuckcon, double area, double per, char type, char btype, double imax, double vmin, double vmax, double intrange, double mrange, double lorange, double * Bdv);


/* USRLIB MODULE INFORMATION

	MODULE NAME: alg_breakv_new
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 16
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		top,	int,	Input,	,	,	
		bot,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		width,	double,	Input,	,	,	
		length,	double,	Input,	,	,	
		captype,	int,	Input,	,	,	
		btype,	char,	Input,	,	,	
		imax,	double,	Input,	,	,	
		vmin,	double,	Input,	,	,	
		vmax,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		Bdv,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void alg_breakv_new(char * devname, int top, int bot, int sub, int chuckcon, double width, double length, int captype, char btype, double imax, double vmin, double vmax, double intrange, double mrange, double lorange, double * Bdv);


/* USRLIB MODULE INFORMATION

	MODULE NAME: alg_breakv_swp
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 16
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		top,	int,	Input,	,	,	
		bot,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		area,	double,	Input,	,	,	
		per,	double,	Input,	,	,	
		captype,	char,	Input,	,	,	
		btype,	char,	Input,	,	,	
		imax,	double,	Input,	,	,	
		vmin,	double,	Input,	,	,	
		vmax,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		Bdv,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void alg_breakv_swp(char * devname, int top, int bot, int sub, int chuckcon, double area, double per, char captype, char btype, double imax, double vmin, double vmax, double intrange, double mrange, double lorange, double * Bdv);


/* USRLIB MODULE INFORMATION

	MODULE NAME: alg_breakv_swpv
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 16
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		top,	int,	Input,	,	,	
		bot,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		area,	double,	Input,	,	,	
		per,	double,	Input,	,	,	
		captype,	char,	Input,	,	,	
		btype,	char,	Input,	,	,	
		imax,	double,	Input,	,	,	
		vmin,	double,	Input,	,	,	
		vmax,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		Bdv,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#include <stdlib.h>
#include <ktxe_proto.h>
#include <ksox_def.h>

#define MAX(a,b)        ((a)>(b) ? (a) : (b))
#define CDELAY  125.E-12
#define ILEAK   20.E-12


	END USRLIB MODULE INFORMATION
*/
void alg_breakv_swpv(char * devname, int top, int bot, int sub, int chuckcon, double area, double per, char captype, char btype, double imax, double vmin, double vmax, double intrange, double mrange, double lorange, double * Bdv);


/* USRLIB MODULE INFORMATION

	MODULE NAME: alg_breakv_swpv1
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
		captype,	int,	Input,	,	,	
		btype,	char,	Input,	,	,	
		imax,	double,	Input,	,	,	
		vmin,	double,	Input,	,	,	
		vmax,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		Bdv1,	double *,	Output,	,	,	
		Bdv2,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#include <stdlib.h>
#include <ktxe_proto.h>
#include <ksox_def.h>

#define MAX(a,b)        ((a)>(b) ? (a) : (b))
#define CDELAY  125.E-12
#define ILEAK   20.E-12


	END USRLIB MODULE INFORMATION
*/
void alg_breakv_swpv1(char * devname, int top, int bot, int sub, int chuckcon, double area, double per, int captype, char btype, double imax, double vmin, double vmax, double intrange, double mrange, double lorange, double * Bdv1, double * Bdv2);


/* USRLIB MODULE INFORMATION

	MODULE NAME: alg_breakv_swpv1_chck
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
		captype,	int,	Input,	,	,	
		btype,	char,	Input,	,	,	
		imax,	double,	Input,	,	,	
		vmin,	double,	Input,	,	,	
		vmax,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		Bdv1,	double *,	Output,	,	,	
		Bdv2,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#include <stdlib.h>
#include <ktxe_proto.h>
#include <ksox_def.h>

#define MAX(a,b)        ((a)>(b) ? (a) : (b))
#define CDELAY  125.E-12
#define ILEAK   20.E-12


	END USRLIB MODULE INFORMATION
*/
void alg_breakv_swpv1_chck(char * devname, int top, int bot, int sub, int chuckcon, double area, double per, int captype, char btype, double imax, double vmin, double vmax, double intrange, double mrange, double lorange, double * Bdv1, double * Bdv2);


/* USRLIB MODULE INFORMATION

	MODULE NAME: alg_breakv_vf
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 16
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		top,	int,	Input,	,	,	
		bot,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		area,	double,	Input,	,	,	
		per,	double,	Input,	,	,	
		captype,	int,	Input,	,	,	
		btype,	char,	Input,	,	,	
		imax,	double,	Input,	,	,	
		vmin,	double,	Input,	,	,	
		vmax,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		Bdv,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void alg_breakv_vf(char * devname, int top, int bot, int sub, int chuckcon, double area, double per, int captype, char btype, double imax, double vmin, double vmax, double intrange, double mrange, double lorange, double * Bdv);


/* USRLIB MODULE INFORMATION

	MODULE NAME: alg_bv
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 18
	ARGUMENTS:
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
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#include <tidp1.h>
	END USRLIB MODULE INFORMATION
*/
double alg_bv(int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double idmax, double vdsmin, double vdsmax, double vgs, double vbs, double multiplier, double intrange, double mrange, double lorange);


/* USRLIB MODULE INFORMATION

	MODULE NAME: alg_bv_corr
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 18
	ARGUMENTS:
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
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#include <tidp1.h>
	END USRLIB MODULE INFORMATION
*/
double alg_bv_corr(int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double idmax, double vdsmin, double vdsmax, double vgs, double vbs, double multiplier, double intrange, double mrange, double lorange);


/* USRLIB MODULE INFORMATION

	MODULE NAME: alg_bv_kj
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 18
	ARGUMENTS:
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
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#include <tidp1.h>
	END USRLIB MODULE INFORMATION
*/
double alg_bv_kj(int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double idmax, double vdsmin, double vdsmax, double vgs, double vbs, double multiplier, double intrange, double mrange, double lorange);


/* USRLIB MODULE INFORMATION

	MODULE NAME: alg_bv_new
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 18
	ARGUMENTS:
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
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#include <tidp1.h>
	END USRLIB MODULE INFORMATION
*/
double alg_bv_new(int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double idmax, double vdsmin, double vdsmax, double vgs, double vbs, double multiplier, double intrange, double mrange, double lorange);


/* USRLIB MODULE INFORMATION

	MODULE NAME: alg_bv_swpv
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 18
	ARGUMENTS:
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
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#include <tidp1.h>
#include <stdlib.h>
#include <ktxe_proto.h>
#include <ksox_def.h>

#define MAX(a,b)        ((a)>(b) ? (a) : (b))
#define CDELAY  125.E-12
#define ILEAK   20.E-12


	END USRLIB MODULE INFORMATION
*/
double alg_bv_swpv(int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double idmax, double vdsmin, double vdsmax, double vgs, double vbs, double multiplier, double intrange, double mrange, double lorange);


/* USRLIB MODULE INFORMATION

	MODULE NAME: alg_cap
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 23
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
		sca,	double *,	Output,	,	,	
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
void alg_cap(char * devname, int hi, int lo, int sub, int chuckcon, double area, double per, char tty, char bty, double vacc, double vinv, double freq, double sig, double stray, int integ, int ddebug, double mrange, double lorange, double * ca, double * ta, double * ci, double * ti, double * sca);


/* USRLIB MODULE INFORMATION

	MODULE NAME: alg_cap_corr
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
void alg_cap_corr(char * devname, int hi, int lo, int sub, int chuckcon, double area, double per, char tty, char bty, double vacc, double vinv, double freq, double sig, double stray, int integ, int ddebug, double mrange, double lorange, double * ca, double * ta, double * ci, double * ti);


/* USRLIB MODULE INFORMATION

	MODULE NAME: alg_cap_gnd
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
void alg_cap_gnd(char * devname, int hi, int lo, int sub, int chuckcon, double area, double per, char tty, char bty, double vacc, double vinv, double freq, double sig, double stray, int integ, int ddebug, double mrange, double lorange, double * ca, double * ta, double * ci, double * ti);


/* USRLIB MODULE INFORMATION

	MODULE NAME: alg_cap_sm01
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 12
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		area,	double,	Input,	,	,	
		vacc,	double,	Input,	,	,	
		vinv,	double,	Input,	,	,	
		integ,	int,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		ca,	double *,	Output,	,	,	
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
void alg_cap_sm01(char * devname, int hi, int lo, int sub, int chuckcon, double area, double vacc, double vinv, int integ, double mrange, double lorange, double * ca);


/* USRLIB MODULE INFORMATION

	MODULE NAME: alg_cap_sweep
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 16
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		area,	double,	Input,	,	,	
		startv,	double,	Input,	,	,	
		stopv,	double,	Input,	,	,	
		sign,	int,	Input,	,	,	
		step,	int,	Input,	,	,	
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
void alg_cap_sweep(char * devname, int hi, int lo, int sub, int chuckcon, double area, double startv, double stopv, int sign, int step, double mrange, double lorange, double * ca, double * ta, double * ci, double * ti);


/* USRLIB MODULE INFORMATION

	MODULE NAME: alg_capj_sm01
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 13
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		area,	double,	Input,	,	,	
		vacc,	double,	Input,	,	,	
		vinv,	double,	Input,	,	,	
		integ,	int,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		ca,	double *,	Output,	,	,	
		ci,	double *,	Output,	,	,	
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
void alg_capj_sm01(char * devname, int hi, int lo, int sub, int chuckcon, double area, double vacc, double vinv, int integ, double mrange, double lorange, double * ca, double * ci);


/* USRLIB MODULE INFORMATION

	MODULE NAME: alg_capmim_sm01
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 14
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		top,	int,	Input,	,	,	
		bot,	int,	Input,	,	,	
		area,	double,	Input,	,	,	
		vacc,	double,	Input,	,	,	
		vmid,	double,	Input,	,	,	
		vinv,	double,	Input,	,	,	
		integ,	int,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		ca,	double *,	Output,	,	,	
		c0,	double *,	Output,	,	,	
		ci,	double *,	Output,	,	,	
		sca,	double *,	Output,	,	,	
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
void alg_capmim_sm01(char * devname, int top, int bot, double area, double vacc, double vmid, double vinv, int integ, double mrange, double lorange, double * ca, double * c0, double * ci, double * sca);


/* USRLIB MODULE INFORMATION

	MODULE NAME: alg_capmim_sm01_st
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 14
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		top,	int,	Input,	,	,	
		bot,	int,	Input,	,	,	
		area,	double,	Input,	,	,	
		vacc,	double,	Input,	,	,	
		vmid,	double,	Input,	,	,	
		vinv,	double,	Input,	,	,	
		integ,	int,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		ca,	double *,	Output,	,	,	
		c0,	double *,	Output,	,	,	
		ci,	double *,	Output,	,	,	
		sca,	double *,	Output,	,	,	
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
void alg_capmim_sm01_st(char * devname, int top, int bot, double area, double vacc, double vmid, double vinv, int integ, double mrange, double lorange, double * ca, double * c0, double * ci, double * sca);


/* USRLIB MODULE INFORMATION

	MODULE NAME: alg_capol_sm01
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 16
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		sd,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		poly,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		area,	double,	Input,	,	,	
		vacc,	double,	Input,	,	,	
		vmid,	double,	Input,	,	,	
		vinv,	double,	Input,	,	,	
		integ,	int,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		ca,	double *,	Output,	,	,	
		c0,	double *,	Output,	,	,	
		ci,	double *,	Output,	,	,	
		sca,	double *,	Output,	,	,	
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
void alg_capol_sm01(char * devname, int sd, int sub, int poly, int chuckcon, double area, double vacc, double vmid, double vinv, int integ, double mrange, double lorange, double * ca, double * c0, double * ci, double * sca);


/* USRLIB MODULE INFORMATION

	MODULE NAME: alg_capol_sm01_sw
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 16
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		sd,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		poly,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		area,	double,	Input,	,	,	
		vacc,	double,	Input,	,	,	
		vmid,	double,	Input,	,	,	
		vinv,	double,	Input,	,	,	
		integ,	int,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		ca,	double *,	Output,	,	,	
		c0,	double *,	Output,	,	,	
		ci,	double *,	Output,	,	,	
		sca,	double *,	Output,	,	,	
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
void alg_capol_sm01_sw(char * devname, int sd, int sub, int poly, int chuckcon, double area, double vacc, double vmid, double vinv, int integ, double mrange, double lorange, double * ca, double * c0, double * ci, double * sca);


/* USRLIB MODULE INFORMATION

	MODULE NAME: alg_capol_sm01_temp
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 16
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		sd,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		poly,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		area,	double,	Input,	,	,	
		vacc,	double,	Input,	,	,	
		vmid,	double,	Input,	,	,	
		vinv,	double,	Input,	,	,	
		integ,	int,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		ca,	double *,	Output,	,	,	
		c0,	double *,	Output,	,	,	
		ci,	double *,	Output,	,	,	
		sca,	double *,	Output,	,	,	
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
void alg_capol_sm01_temp(char * devname, int sd, int sub, int poly, int chuckcon, double area, double vacc, double vmid, double vinv, int integ, double mrange, double lorange, double * ca, double * c0, double * ci, double * sca);


/* USRLIB MODULE INFORMATION

	MODULE NAME: alg_i2v
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 18
	ARGUMENTS:
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
		intrange,	double,	Input,	,	,	
		normalize,	char,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		Ip,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#include "kdf.h"
#include "ktxe_types.h"
#include "ksox_def.h"
#include "COM_usrlib.h"
	END USRLIB MODULE INFORMATION
*/
double alg_i2v(int top, int bot, int sub, int chuckcon, double area, double per, char type, char btype, double vrev, double ilim, double icomp, double irange, double delaytime, double intrange, char normalize, double mrange, double lorange, double * Ip);


/* USRLIB MODULE INFORMATION

	MODULE NAME: alg_i2v_corr
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 18
	ARGUMENTS:
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
		intrange,	double,	Input,	,	,	
		normalize,	char,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		Ip,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#include "kdf.h"
#include "ktxe_types.h"
#include "ksox_def.h"
#include "COM_usrlib.h"
	END USRLIB MODULE INFORMATION
*/
double alg_i2v_corr(int top, int bot, int sub, int chuckcon, double area, double per, char type, char btype, double vrev, double ilim, double icomp, double irange, double delaytime, double intrange, char normalize, double mrange, double lorange, double * Ip);


/* USRLIB MODULE INFORMATION

	MODULE NAME: alg_i2v_smu2
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 18
	ARGUMENTS:
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
		intrange,	double,	Input,	,	,	
		normalize,	char,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		Ip,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#include "kdf.h"
#include "ktxe_types.h"
#include "ksox_def.h"
#include "COM_usrlib.h"
	END USRLIB MODULE INFORMATION
*/
double alg_i2v_smu2(int top, int bot, int sub, int chuckcon, double area, double per, char type, char btype, double vrev, double ilim, double icomp, double irange, double delaytime, double intrange, char normalize, double mrange, double lorange, double * Ip);


/* USRLIB MODULE INFORMATION

	MODULE NAME: alg_i2v_sweep
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 18
	ARGUMENTS:
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
		intrange,	double,	Input,	,	,	
		normalize,	char,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		Ip,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#include "kdf.h"
#include "ktxe_types.h"
#include "ksox_def.h"
#include "COM_usrlib.h"
	END USRLIB MODULE INFORMATION
*/
double alg_i2v_sweep(int top, int bot, int sub, int chuckcon, double area, double per, char type, char btype, double vrev, double ilim, double icomp, double irange, double delaytime, double intrange, char normalize, double mrange, double lorange, double * Ip);


/* USRLIB MODULE INFORMATION

	MODULE NAME: alg_i4v
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 19
	ARGUMENTS:
		gate,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		bulk,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		width,	double,	Input,	,	,	
		length,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
		vdsf,	double,	Input,	0.1,	,	
		vgs_off,	double,	Input,	0,	,	
		vgs_on,	double,	Input,	1,	,	
		vbsf,	double,	Input,	0,	,	
		factor,	double,	Input,	0,	,	
		delay,	double,	Input,	0.03,	,	
		intrange,	double,	Input,	1,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		ioff,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
double alg_i4v(int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vdsf, double vgs_off, double vgs_on, double vbsf, double factor, double delay, double intrange, double mrange, double lorange, double * ioff);


/* USRLIB MODULE INFORMATION

	MODULE NAME: alg_i4v_corr
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 19
	ARGUMENTS:
		gate,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		bulk,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		width,	double,	Input,	,	,	
		length,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
		vdsf,	double,	Input,	0.1,	,	
		vgs_off,	double,	Input,	0,	,	
		vgs_on,	double,	Input,	1,	,	
		vbsf,	double,	Input,	0,	,	
		factor,	double,	Input,	0,	,	
		delay,	double,	Input,	0.03,	,	
		intrange,	double,	Input,	1,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		ioff,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
double alg_i4v_corr(int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vdsf, double vgs_off, double vgs_on, double vbsf, double factor, double delay, double intrange, double mrange, double lorange, double * ioff);


/* USRLIB MODULE INFORMATION

	MODULE NAME: alg_i4v_k
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 19
	ARGUMENTS:
		gate,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		bulk,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		width,	double,	Input,	,	,	
		length,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
		vdsf,	double,	Input,	0.1,	,	
		vgs_off,	double,	Input,	0,	,	
		vgs_on,	double,	Input,	1,	,	
		vbsf,	double,	Input,	0,	,	
		factor,	double,	Input,	0,	,	
		delay,	double,	Input,	0.03,	,	
		intrange,	double,	Input,	1,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		ioff,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
double alg_i4v_k(int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vdsf, double vgs_off, double vgs_on, double vbsf, double factor, double delay, double intrange, double mrange, double lorange, double * ioff);


/* USRLIB MODULE INFORMATION

	MODULE NAME: alg_i4v_sc
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 19
	ARGUMENTS:
		gate,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		bulk,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		width,	double,	Input,	,	,	
		length,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
		vdsf,	double,	Input,	0.1,	,	
		vgs_off,	double,	Input,	0,	,	
		vgs_on,	double,	Input,	1,	,	
		vbsf,	double,	Input,	0,	,	
		factor,	double,	Input,	0,	,	
		delay,	double,	Input,	0.03,	,	
		intrange,	double,	Input,	1,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		ioff,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
double alg_i4v_sc(int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vdsf, double vgs_off, double vgs_on, double vbsf, double factor, double delay, double intrange, double mrange, double lorange, double * ioff);


/* USRLIB MODULE INFORMATION

	MODULE NAME: alg_i4v_sweep
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 19
	ARGUMENTS:
		gate,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		bulk,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		width,	double,	Input,	,	,	
		length,	double,	Input,	,	,	
		step,	int,	Input,	,	,	
		vgsf,	double,	Input,	0.1,	,	
		v_start,	double,	Input,	0,	,	
		v_stop,	double,	Input,	1,	,	
		vbsf,	double,	Input,	0,	,	
		factor,	double,	Input,	0,	,	
		delay,	double,	Input,	0.03,	,	
		intrange,	double,	Input,	1,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		ioff,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
double alg_i4v_sweep(int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, int step, double vgsf, double v_start, double v_stop, double vbsf, double factor, double delay, double intrange, double mrange, double lorange, double * ioff);


/* USRLIB MODULE INFORMATION

	MODULE NAME: alg_i4v_vd_swp
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 21
	ARGUMENTS:
		gate,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		bulk,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		width,	double,	Input,	,	,	
		length,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
		vdmin,	double,	Input,	0,	,	
		vdmax,	double,	Input,	0.1,	,	
		vstep,	double,	Input,	,	,	
		vgs_off,	double,	Input,	0,	,	
		vgs_on,	double,	Input,	1,	,	
		vbsf,	double,	Input,	0,	,	
		factor,	double,	Input,	0,	,	
		delay,	double,	Input,	0.03,	,	
		intrange,	double,	Input,	1,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		ioff,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
double alg_i4v_vd_swp(int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vdmin, double vdmax, double vstep, double vgs_off, double vgs_on, double vbsf, double factor, double delay, double intrange, double mrange, double lorange, double * ioff);


/* USRLIB MODULE INFORMATION

	MODULE NAME: alg_i4v_vdid_swp
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 22
	ARGUMENTS:
		gate,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		bulk,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		width,	double,	Input,	,	,	
		length,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
		vdmin,	double,	Input,	0,	,	
		vdmax,	double,	Input,	0.1,	,	
		vstep,	double,	Input,	,	,	
		repeat,	int,	Input,	1,	,	
		vgs_off,	double,	Input,	0,	,	
		vgs_on,	double,	Input,	1,	,	
		vbsf,	double,	Input,	0,	,	
		factor,	double,	Input,	0,	,	
		delay,	double,	Input,	0.03,	,	
		intrange,	double,	Input,	1,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		ioff,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
double alg_i4v_vdid_swp(int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vdmin, double vdmax, double vstep, int repeat, double vgs_off, double vgs_on, double vbsf, double factor, double delay, double intrange, double mrange, double lorange, double * ioff);


/* USRLIB MODULE INFORMATION

	MODULE NAME: alg_issb
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 16
	ARGUMENTS:
		vss,	int,	Input,	,	,	
		pwel,	int,	Input,	,	,	
		nwel,	int,	Input,	,	,	
		bitb,	int,	Input,	,	,	
		bit,	int,	Input,	,	,	
		wl,	int,	Input,	,	,	
		vdd,	int,	Input,	,	,	
		width,	double,	Input,	,	,	
		length,	double,	Input,	,	,	
		vcc,	double,	Input,	1.8,	,	
		vwl,	double,	Input,	0,	,	
		vgnd,	double,	Input,	0,	,	
		delay,	double,	Input,	0.03,	,	
		intrange,	double,	Input,	5,	,	
		mrng_,	double,	Input,	,	,	
		lorng_,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
double alg_issb(int vss, int pwel, int nwel, int bitb, int bit, int wl, int vdd, double width, double length, double vcc, double vwl, double vgnd, double delay, double intrange, double mrng_, double lorng_);


/* USRLIB MODULE INFORMATION

	MODULE NAME: alg_isubmax
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 19
	ARGUMENTS:
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
double alg_isubmax(int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vgstart, double vgstop, double vds, double vbulk, double icomp, double sdelay, double intrange, double mrange, double lorange, double * vgmax);


/* USRLIB MODULE INFORMATION

	MODULE NAME: alg_isubmax_corr
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 19
	ARGUMENTS:
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
double alg_isubmax_corr(int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vgstart, double vgstop, double vds, double vbulk, double icomp, double sdelay, double intrange, double mrange, double lorange, double * vgmax);


/* USRLIB MODULE INFORMATION

	MODULE NAME: alg_isubmaxtmp
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 19
	ARGUMENTS:
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
double alg_isubmaxtmp(int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vgstart, double vgstop, double vds, double vbulk, double icomp, double sdelay, double intrange, double mrange, double lorange, double * vgmax);


/* USRLIB MODULE INFORMATION

	MODULE NAME: alg_r2v
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
		chain,	double,	Input,	,	,	
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
void alg_r2v(char * devname, int hi, int lo, int sub, int bulk, int chuckcon, double width, double length, char type, double vforce, double ilimit, double vsub, double delaytime, double intrange, double chain, double mrange, double lorange, double * R);


/* USRLIB MODULE INFORMATION

	MODULE NAME: alg_r2v_corr
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
void alg_r2v_corr(char * devname, int hi, int lo, int sub, int bulk, int chuckcon, double width, double length, char type, double vforce, double ilimit, double vsub, double delaytime, double intrange, char normalize, double mrange, double lorange, double * R);


/* USRLIB MODULE INFORMATION

	MODULE NAME: alg_r2v_swp
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
		chain,	double,	Input,	,	,	
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
void alg_r2v_swp(char * devname, int hi, int lo, int sub, int bulk, int chuckcon, double width, double length, char type, double vforce, double ilimit, double vsub, double delaytime, double intrange, double chain, double mrange, double lorange, double * R);


/* USRLIB MODULE INFORMATION

	MODULE NAME: alg_vth3
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
		kflag,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double  alg_vth3(int, int, int, int, double, double, double, double, double, double, double, double, int, double *, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: alg_vth3_ksc
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
		kflag,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double alg_vth3_ksc(int drain, int gate, int source, int subst, double w, double l, double vlow, double vhigh, double vds, double vbs, double ithr, double vstep, int npts, double * slope, double * kflag);


/* USRLIB MODULE INFORMATION

	MODULE NAME: alg_vth3_org
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
double alg_vth3_org(int drain, int gate, int source, int subst, double w, double l, double vlow, double vhigh, double vds, double vbs, double ithr, double vstep, int npts, double * slope, int * kflag);


/* USRLIB MODULE INFORMATION

	MODULE NAME: alg_vth3_swp
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

#define MAX(a,b)        ((a)>(b) ? (a) : (b))
#define CDELAY  125.E-12
#define ILEAK   20.E-12
	END USRLIB MODULE INFORMATION
*/
double alg_vth3_swp(int drain, int gate, int source, int subst, double w, double l, double vlow, double vhigh, double vds, double vbs, double ithr, double vstep, int npts, double * slope, int * kflag);


/* USRLIB MODULE INFORMATION

	MODULE NAME: alg_vth3_vtlp
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
		kflag,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double alg_vth3_vtlp(int drain, int gate, int source, int subst, double w, double l, double vlow, double vhigh, double vds, double vbs, double ithr, double vstep, int npts, double * slope, double * kflag);


/* USRLIB MODULE INFORMATION

	MODULE NAME: alg_vth5
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
double alg_vth5(int drain, int gate, int source, int subst, double w, double l, double vlow, double vhigh, double vds, double vbs, double ithr, double vstep, int npts, double * slope, int * kflag);


/* USRLIB MODULE INFORMATION

	MODULE NAME: alg_vth5_swp
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

#define MAX(a,b)        ((a)>(b) ? (a) : (b))
#define CDELAY  125.E-12
#define ILEAK   20.E-12
	END USRLIB MODULE INFORMATION
*/
double alg_vth5_swp(int drain, int gate, int source, int subst, double w, double l, double vlow, double vhigh, double vds, double vbs, double ithr, double vstep, int npts, double * slope, int * kflag);


/* USRLIB MODULE INFORMATION

	MODULE NAME: BVD_V2I
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 11
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		bot,	int,	Input,	,	,	
		vstop,	double,	Input,	,	,	
		vstep,	double,	Input,	,	,	
		ipgm,	double,	Input,	,	,	
		udelay,	double,	Input,	,	,	
		debug,	int,	Input,	,	,	
		type,	char,	Input,	,	,	
		result,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double BVD_V2I(int hi, int lo, int subst, int bot, double vstop, double vstep, double ipgm, double udelay, int debug, char type, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: BVDS_mos_sc
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
void BVDS_mos_sc(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double idmax, double vdsmin, double vdsmax, double vgs, double vbs, double multiplier, double intrange, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: cap_sweep
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
void cap_sweep(char * devname, int hi, int lo, int sub, int chuckcon, double area, double per, char tty, char bty, double vacc, double vinv, double freq, double sig, double stray, int integ, int ddebug, double mrange, double lorange, double * ca, double * ta, double * ci, double * ti);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Fetcheck_SWH_mos
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 19
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
		vdsf,	double,	Input,	,	,	
		vgs_off,	double,	Input,	,	,	
		vgs_on,	double,	Input,	,	,	
		vbsf,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		ioff,	double *,	Output,	,	,	
		ids,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#include <tidp1.h>
	END USRLIB MODULE INFORMATION
*/
void Fetcheck_SWH_mos(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vdsf, double vgs_off, double vgs_on, double vbsf, double intrange, double mrange, double lorange, double * ioff, double * ids);


/* USRLIB MODULE INFORMATION

	MODULE NAME: pn2swp_ld
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 9
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		bot,	int,	Input,	,	,	
		vstop,	double,	Input,	,	,	
		vstep,	double,	Input,	,	,	
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
double pn2swp_ld(int hi, int lo, int subst, int bot, double vstop, double vstep, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: pn2swp_save
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
double pn2swp_save(int hi, int lo, int subst, double vstart, double vstop, int nstep, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: pn2swp_save1
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
double pn2swp_save1(int hi, int lo, int subst, double vstart, double vstop, int nstep, double ipgm, double udelay, char type);


