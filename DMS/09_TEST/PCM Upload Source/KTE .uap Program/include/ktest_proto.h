/* ktest function prototype and KITT header file */

/* COPY OF USRLIB SETTINGS INFORMATION */
/* [VISIBILITY] */
/* HIDDEN=0 */
/* [DEPENDENCIES] */
/* LIBS=-lA07 -lHP4284 */
/* OBJECTS= */
/* HEADERS= */
/* [NON-KULT SEARCH PATH] */
/* PATHS= */

/* USRLIB MODULE INFORMATION

	MODULE NAME: algo_cap
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
void algo_cap(char * devname, int hi, int lo, int sub, int chuckcon, double area, double per, char tty, char bty, double vacc, double vinv, double freq, double sig, double stray, int integ, int ddebug, double mrange, double lorange, double * ca, double * ta, double * ci, double * ti);


/* USRLIB MODULE INFORMATION

	MODULE NAME: bpi_sanity_pad
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 28
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		pn1,	int,	Input,	,	,	
		pn2,	int,	Input,	,	,	
		pn3,	int,	Input,	,	,	
		pn4,	int,	Input,	,	,	
		pn5,	int,	Input,	,	,	
		pn6,	int,	Input,	,	,	
		pn7,	int,	Input,	,	,	
		pn8,	int,	Input,	,	,	
		pn9,	int,	Input,	,	,	
		pn10,	int,	Input,	,	,	
		pn11,	int,	Input,	,	,	
		pn12,	int,	Input,	,	,	
		pn13,	int,	Input,	,	,	
		pn14,	int,	Input,	,	,	
		pn15,	int,	Input,	,	,	
		pn16,	int,	Input,	,	,	
		pn17,	int,	Input,	,	,	
		pn18,	int,	Input,	,	,	
		pn19,	int,	Input,	,	,	
		pn20,	int,	Input,	,	,	
		Ipad,	double,	Input,	,	,	
		Vcomp,	double,	Input,	,	,	
		Debug,	int,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		St,	int *,	Output,	,	,	
		Pad,	int *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#include "ktxe_types.h"
	END USRLIB MODULE INFORMATION
*/
void bpi_sanity_pad(char * devname, int pn1, int pn2, int pn3, int pn4, int pn5, int pn6, int pn7, int pn8, int pn9, int pn10, int pn11, int pn12, int pn13, int pn14, int pn15, int pn16, int pn17, int pn18, int pn19, int pn20, double Ipad, double Vcomp, int Debug, double mrange, double lorange, int * St, int * Pad);


/* USRLIB MODULE INFORMATION

	MODULE NAME: bpi_sanity_pad_dbg
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 28
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		pn1,	int,	Input,	,	,	
		pn2,	int,	Input,	,	,	
		pn3,	int,	Input,	,	,	
		pn4,	int,	Input,	,	,	
		pn5,	int,	Input,	,	,	
		pn6,	int,	Input,	,	,	
		pn7,	int,	Input,	,	,	
		pn8,	int,	Input,	,	,	
		pn9,	int,	Input,	,	,	
		pn10,	int,	Input,	,	,	
		pn11,	int,	Input,	,	,	
		pn12,	int,	Input,	,	,	
		pn13,	int,	Input,	,	,	
		pn14,	int,	Input,	,	,	
		pn15,	int,	Input,	,	,	
		pn16,	int,	Input,	,	,	
		pn17,	int,	Input,	,	,	
		pn18,	int,	Input,	,	,	
		pn19,	int,	Input,	,	,	
		pn20,	int,	Input,	,	,	
		Ipad,	double,	Input,	,	,	
		Vcomp,	double,	Input,	,	,	
		Debug2,	int,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		St,	int *,	Output,	,	,	
		Pad,	int *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#include "ktxe_types.h"
	END USRLIB MODULE INFORMATION
*/
void bpi_sanity_pad_dbg(char * devname, int pn1, int pn2, int pn3, int pn4, int pn5, int pn6, int pn7, int pn8, int pn9, int pn10, int pn11, int pn12, int pn13, int pn14, int pn15, int pn16, int pn17, int pn18, int pn19, int pn20, double Ipad, double Vcomp, int Debug2, double mrange, double lorange, int * St, int * Pad);


/* USRLIB MODULE INFORMATION

	MODULE NAME: BREAKI_dio
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
void BREAKI_dio(char * devname, int top, int bot, int sub, int chuckcon, double area, double per, char type, char btype, double imax, double vmin, double vmax, double multiplier, double intrange, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: BREAKV_dio
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
void BREAKV_dio(char * devname, int top, int bot, int sub, int chuckcon, double area, double per, char type, char btype, double imax, double vmin, double vmax, double multiplier, double intrange, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: breakv_dio
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
void breakv_dio(char * devname, int top, int bot, int sub, int chuckcon, double area, double per, char type, char btype, double imax, double vmin, double vmax, double intrange, double mrange, double lorange, double * Bdv);


/* USRLIB MODULE INFORMATION

	MODULE NAME: BREAKV_dio2
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
void BREAKV_dio2(char * devname, int top, int bot, int sub, int chuckcon, double area, double per, char type, char btype, double imax, double vmin, double vmax, double multiplier, double intrange, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: BREAKV_dio4p
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 19
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
		vsub,	double,	Input,	,	,	
		vchuck,	double,	Input,	,	,	
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
void  BREAKV_dio4p(char *, int, int, int, int, double, double, char, char, double, double, double, double, double, double, double, double, double, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: breakv_dio_ju
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 21
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
		vmax,	double,	Input,	,	,	
		vstep,	double,	Input,	,	,	
		nsteps,	int,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		Im,	D_ARRAY_T,	Output,	 ,	 ,	 
		Im_ary,	int,	Input,	,	,	
		Fv,	D_ARRAY_T,	Output,	 ,	 ,	 
		Fv_ary,	int,	Input,	,	,	
		Bdv,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void breakv_dio_ju(char * devname, int top, int bot, int sub, int chuckcon, double area, double per, char type, char btype, double imax, double vmax, double vstep, int nsteps, double intrange, double mrange, double lorange, double *Im, int Im_ary, double *Fv, int Fv_ary, double * Bdv);


/* USRLIB MODULE INFORMATION

	MODULE NAME: BREAKV_dio_org
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
void BREAKV_dio_org(char * devname, int top, int bot, int sub, int chuckcon, double area, double per, char type, char btype, double imax, double vmin, double vmax, double multiplier, double intrange, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: breakv_dio_sc
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 21
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
		vmax,	double,	Input,	,	,	
		vstep,	double,	Input,	,	,	
		nsteps,	int,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		Im,	D_ARRAY_T,	Output,	,	,	
		Im_ary,	int,	Input,	,	,	
		Fv,	D_ARRAY_T,	Output,	,	,	
		Fv_ary,	int,	Input,	,	,	
		Bdv,	double *,	Output,	,	,	
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

#define MAX(a,b)	((a)>(b) ? (a) : (b))
#define CDELAY  125.E-12
#define ILEAK   20.E-12 

	END USRLIB MODULE INFORMATION
*/
void breakv_dio_sc(char * devname, int top, int bot, int sub, int chuckcon, double area, double per, char type, char btype, double imax, double vmax, double vstep, int nsteps, double intrange, double mrange, double lorange, double *Im, int Im_ary, double *Fv, int Fv_ary, double * Bdv);


/* USRLIB MODULE INFORMATION

	MODULE NAME: BREAKV_dio_swp
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
void BREAKV_dio_swp(char * devname, int top, int bot, int sub, int chuckcon, double area, double per, char type, char btype, double imax, double vmin, double vmax, double multiplier, double intrange, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: BREAKV_dio_swp_VC5421
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 19
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
		imeas,	I_ARRAY_T,	Output,	 ,	 ,	 
		step,	int,	Input,	,	,	
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
void BREAKV_dio_swp_VC5421(char * devname, int top, int bot, int sub, int chuckcon, double area, double per, char type, char btype, double imax, double vmin, double vmax, int *imeas, int step, double multiplier, double intrange, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: BREAKV_sw_dio
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
void BREAKV_sw_dio(char * devname, int top, int bot, int sub, int chuckcon, double area, double per, char type, char btype, double imax, double vmin, double vmax, double multiplier, double intrange, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: breakv_vb_dio
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
		Vsub,	double,	Input,	,	,	
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
void breakv_vb_dio(char * devname, int top, int bot, int sub, int chuckcon, double area, double per, char type, char btype, double imax, double vmin, double vmax, double Vsub, double intrange, double mrange, double lorange, double * Bdv);


/* USRLIB MODULE INFORMATION

	MODULE NAME: brkdn_cap
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 18
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
		ibr,	double,	Input,	,	,	
		vmin,	double,	Input,	,	,	
		vmax,	double,	Input,	,	,	
		vstep,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		debug2,	int,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		vbr,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <stdlib.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void brkdn_cap(char * devname, int top, int bot, int sub, int chuckcon, double area, double per, char type, char btype, double ibr, double vmin, double vmax, double vstep, double intrange, int debug2, double mrange, double lorange, double * vbr);


/* USRLIB MODULE INFORMATION

	MODULE NAME: bvcbo_bip
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 16
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		col,	int,	Input,	,	,	
		base,	int,	Input,	,	,	
		emit,	int,	Input,	,	,	
		bulk,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		type,	char,	Input,	,	,	
		area,	double,	Input,	,	,	
		ic,	double,	Input,	,	,	
		vlim,	double,	Input,	,	,	
		delaytime,	double,	Input,	,	,	
		vbulk,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		Bvcbo,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void bvcbo_bip(char * devname, int col, int base, int emit, int bulk, int sub, char type, double area, double ic, double vlim, double delaytime, double vbulk, double intrange, double mrange, double lorange, double * Bvcbo);


/* USRLIB MODULE INFORMATION

	MODULE NAME: bvceo_bip
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 16
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		col,	int,	Input,	,	,	
		base,	int,	Input,	,	,	
		emit,	int,	Input,	,	,	
		bulk,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		type,	char,	Input,	,	,	
		area,	double,	Input,	,	,	
		ic,	double,	Input,	,	,	
		vlim,	double,	Input,	,	,	
		delaytime,	double,	Input,	,	,	
		vbulk,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		Bvceo,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>

	END USRLIB MODULE INFORMATION
*/
void bvceo_bip(char * devname, int col, int base, int emit, int bulk, int sub, char type, double area, double ic, double vlim, double delaytime, double vbulk, double intrange, double mrange, double lorange, double * Bvceo);


/* USRLIB MODULE INFORMATION

	MODULE NAME: BVDS1_mos
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
void BVDS1_mos(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double idmax, double vdsmin, double vdsmax, double vgs, double vbs, double multiplier, double intrange, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: BVDS_mos
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
void  BVDS_mos(char *, int, int, int, int, int, int, double, double, char, double, double, double, double, double, double, double, double, double, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: BVDS_mos_bvii
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
void BVDS_mos_bvii(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double idmax, double vdsmin, double vdsmax, double vgs, double vbs, double multiplier, double intrange, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: BVDS_mos_hb
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
void BVDS_mos_hb(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double idmax, double vdsmin, double vdsmax, double vgs, double vbs, double multiplier, double intrange, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: BVDS_mos_iswp
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
		imin,	double,	Input,	,	,	
		imax,	double,	Input,	,	,	
		delay,	double,	Input,	,	,	
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
void BVDS_mos_iswp(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double idmax, double imin, double imax, double delay, double vmax, double multiplier, double intrange, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: BVDS_mos_org
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
void  BVDS_mos_org(char *, int, int, int, int, int, int, double, double, char, double, double, double, double, double, double, double, double, double, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: BVDS_mos_org161104
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
void  BVDS_mos_org161104(char *, int, int, int, int, int, int, double, double, char, double, double, double, double, double, double, double, double, double, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: BVDS_mos_subgnd
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
void BVDS_mos_subgnd(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double idmax, double vdsmin, double vdsmax, double vgs, double vbs, double multiplier, double intrange, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: BVDS_mos_vswp
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
		vmin,	double,	Input,	,	,	
		vmax,	double,	Input,	,	,	
		npoints,	int,	Input,	,	,	
		idmax,	double,	Input,	,	,	
		vgs,	double,	Input,	,	,	
		vbs,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		result_vt,	double *,	Output,	,	,	
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

#define MAX(a,b)	((a)>(b) ? (a) : (b))
#define CDELAY  125.E-12
#define ILEAK   20.E-12 


	END USRLIB MODULE INFORMATION
*/
void BVDS_mos_vswp(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vmin, double vmax, int npoints, double idmax, double vgs, double vbs, double intrange, double mrange, double lorange, double * result_vt);


/* USRLIB MODULE INFORMATION

	MODULE NAME: BVDS_mos_vswp1
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
		nstep,	int,	Input,	,	,	
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
void BVDS_mos_vswp1(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, int nstep, char type, double idmax, double vdsmin, double vdsmax, double vgs, double vbs, double multiplier, double intrange, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: bvds_vsw_mos
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 15
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		gate,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		bulk,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		type,	char,	Input,	,	,	
		idtarget,	double,	Input,	,	,	
		vstart,	double,	Input,	,	,	
		vstop,	double,	Input,	,	,	
		step,	double,	Input,	,	,	
		intrange,	int,	Input,	,	,	
		bvds,	double *,	Output,	,	,	
		debug,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void bvds_vsw_mos(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, char type, double idtarget, double vstart, double vstop, double step, int intrange, double * bvds, int debug);


/* USRLIB MODULE INFORMATION

	MODULE NAME: BVDS_Vswp_mos
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
		idmax,	double,	Input,	,	,	
		vdsmin,	double,	Input,	,	,	
		vdsmax,	double,	Input,	,	,	
		nsteps,	int,	Input,	,	,	
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
void BVDS_Vswp_mos(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double idmax, double vdsmin, double vdsmax, int nsteps, double vgs, double vbs, double multiplier, double intrange, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: bvdss1_mos
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 24
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
		vdsmin,	double,	Input,	,	,	
		vdsmax,	double,	Input,	,	,	
		vdstep,	double,	Input,	,	,	
		ilimit,	double,	Input,	,	,	
		icomp,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		vbulk,	double,	Input,	,	,	
		vgate,	double,	Input,	,	,	
		sd,	char,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		Bvdss,	double *,	Output,	,	,	
		Id,	double *,	Output,	,	,	
		Is,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <stdlib.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void bvdss1_mos(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vdsmin, double vdsmax, double vdstep, double ilimit, double icomp, double intrange, double vbulk, double vgate, char sd, double mrange, double lorange, double * Bvdss, double * Id, double * Is);


/* USRLIB MODULE INFORMATION

	MODULE NAME: bvebo_bip
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 16
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		col,	int,	Input,	,	,	
		base,	int,	Input,	,	,	
		emit,	int,	Input,	,	,	
		bulk,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		type,	char,	Input,	,	,	
		area,	double,	Input,	,	,	
		ie,	double,	Input,	,	,	
		vlim,	double,	Input,	,	,	
		delaytime,	double,	Input,	,	,	
		vbulk,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		Bvebo,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void bvebo_bip(char * devname, int col, int base, int emit, int bulk, int sub, char type, double area, double ie, double vlim, double delaytime, double vbulk, double intrange, double mrange, double lorange, double * Bvebo);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Bvii_mos
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 17
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
		vdd,	double,	Input,	,	,	
		vbs,	double,	Input,	,	,	
		xid,	double,	Input,	1.5,	,	
		delaytime,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		bvii,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void Bvii_mos(int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vdd, double vbs, double xid, double delaytime, double intrange, double mrange, double lorange, double * bvii);


/* USRLIB MODULE INFORMATION

	MODULE NAME: BVII_mos_vswp
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
		vgstep,	double,	Input,	,	,	
		vbs,	double,	Input,	,	,	
		vgmin,	double,	Input,	,	,	
		gstep,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		result_vt,	double *,	Output,	,	,	
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

#define MAX(a,b)	((a)>(b) ? (a) : (b))
#define CDELAY  125.E-12
#define ILEAK   20.E-12 


	END USRLIB MODULE INFORMATION
*/
void BVII_mos_vswp(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vmin, double vmax, int npoints, double idmax, double vgstep, double vbs, double vgmin, double gstep, double intrange, double mrange, double lorange, double * result_vt);


/* USRLIB MODULE INFORMATION

	MODULE NAME: bvii_spot_mos
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
		vdd,	double,	Input,	,	,	
		vbs,	double,	Input,	,	,	
		xid,	double,	Input,	1.5,	,	
		delaytime,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		bvii,	double *,	Output,	,	,	
		isub,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void bvii_spot_mos(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vdd, double vbs, double xid, double delaytime, double intrange, double mrange, double lorange, double * bvii, double * isub);


/* USRLIB MODULE INFORMATION

	MODULE NAME: C_OX_FR_cap
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
		vbias,	double,	Input,	,	,	
		ed,	double,	Input,	,	,	
		crange,	double,	Input,	,	,	
		c_mult,	double,	Input,	,	,	
		s_mult,	double,	Input,	,	,	
		t_mult,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		c_frequency,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		cap,	double *,	Output,	,	,	
		cond,	double *,	Output,	,	,	
		tox,	double *,	Output,	,	,	
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
#include <cmtr_hp4284.h>
#define __4284__

#define Eo 8.854E-14
#define PI 3.14159265358979323846264338327950288419716939937510582
#define DONTCARE 0
#define CV_XMT 11
	END USRLIB MODULE INFORMATION
*/
void C_OX_FR_cap(char * devname, int hi, int lo, int sub, int chuckcon, double area, double per, char tty, char bty, double vbias, double ed, double crange, double c_mult, double s_mult, double t_mult, double intrange, double c_frequency, double mrange, double lorange, double * cap, double * cond, double * tox);


/* USRLIB MODULE INFORMATION

	MODULE NAME: C_OX_FR_cap_1M_swp
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
		step,	int,	Input,	,	,	
		vmin,	double,	Input,	,	,	
		vmax,	double,	Input,	,	,	
		crange,	double,	Input,	,	,	
		c_mult,	double,	Input,	,	,	
		s_mult,	double,	Input,	,	,	
		t_mult,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		c_frequency,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		cap,	double *,	Output,	,	,	
		cond,	double *,	Output,	,	,	
		tox,	double *,	Output,	,	,	
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

#define CRTLIM 1.0E-6		                              
#include <tidp1.h>
#include "ktxe_types.h"
#include "COM_usrlib.h"
#include <cmtr_hp4284.h>
#define __4284__

#define Eo 8.854E-14
#define PI 3.14159265358979323846264338327950288419716939937510582
#define DONTCARE 0
#define CV_XMT 11
	END USRLIB MODULE INFORMATION
*/
void C_OX_FR_cap_1M_swp(char * devname, int hi, int lo, int sub, int chuckcon, double area, double per, char tty, int step, double vmin, double vmax, double crange, double c_mult, double s_mult, double t_mult, double intrange, double c_frequency, double mrange, double lorange, double * cap, double * cond, double * tox);


/* USRLIB MODULE INFORMATION

	MODULE NAME: C_OX_FR_cap_org
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
		vbias,	double,	Input,	,	,	
		ed,	double,	Input,	,	,	
		crange,	double,	Input,	,	,	
		c_mult,	double,	Input,	,	,	
		s_mult,	double,	Input,	,	,	
		t_mult,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		c_frequency,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		cap,	double *,	Output,	,	,	
		cond,	double *,	Output,	,	,	
		tox,	double *,	Output,	,	,	
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
#include <cmtr_hp4284.h>
#define __4284__

#define Eo 8.854E-14
#define PI 3.14159265358979323846264338327950288419716939937510582
#define DONTCARE 0
#define CV_XMT 11
	END USRLIB MODULE INFORMATION
*/
void C_OX_FR_cap_org(char * devname, int hi, int lo, int sub, int chuckcon, double area, double per, char tty, char bty, double vbias, double ed, double crange, double c_mult, double s_mult, double t_mult, double intrange, double c_frequency, double mrange, double lorange, double * cap, double * cond, double * tox);


/* USRLIB MODULE INFORMATION

	MODULE NAME: C_OX_FR_cap_swp
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
		step,	int,	Input,	,	,	
		vmin,	double,	Input,	,	,	
		vmax,	double,	Input,	,	,	
		crange,	double,	Input,	,	,	
		c_mult,	double,	Input,	,	,	
		s_mult,	double,	Input,	,	,	
		t_mult,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		c_frequency,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		cap,	double *,	Output,	,	,	
		cond,	double *,	Output,	,	,	
		tox,	double *,	Output,	,	,	
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

#define CRTLIM 1.0E-6		                              
#include <tidp1.h>
#include "ktxe_types.h"
#include "COM_usrlib.h"
#include <cmtr_hp4284.h>
#define __4284__

#define Eo 8.854E-14
#define PI 3.14159265358979323846264338327950288419716939937510582
#define DONTCARE 0
#define CV_XMT 11
	END USRLIB MODULE INFORMATION
*/
void C_OX_FR_cap_swp(char * devname, int hi, int lo, int sub, int chuckcon, double area, double per, char tty, int step, double vmin, double vmax, double crange, double c_mult, double s_mult, double t_mult, double intrange, double c_frequency, double mrange, double lorange, double * cap, double * cond, double * tox);


/* USRLIB MODULE INFORMATION

	MODULE NAME: C_OX_FR_cap_TOX
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
		vbias,	double,	Input,	,	,	
		ed,	double,	Input,	,	,	
		crange,	double,	Input,	,	,	
		c_mult,	double,	Input,	,	,	
		s_mult,	double,	Input,	,	,	
		t_mult,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		c_frequency,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		cap,	double *,	Output,	,	,	
		cond,	double *,	Output,	,	,	
		tox,	double *,	Output,	,	,	
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
#include <cmtr_hp4284.h>
#define __4284__

#define Eo 8.854E-14
#define PI 3.14159265358979323846264338327950288419716939937510582
#define DONTCARE 0
#define CV_XMT 11
	END USRLIB MODULE INFORMATION
*/
void C_OX_FR_cap_TOX(char * devname, int hi, int lo, int sub, int chuckcon, double area, double per, char tty, char bty, double vbias, double ed, double crange, double c_mult, double s_mult, double t_mult, double intrange, double c_frequency, double mrange, double lorange, double * cap, double * cond, double * tox);


/* USRLIB MODULE INFORMATION

	MODULE NAME: C_OX_FR_cap_wellf
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
		vbias,	double,	Input,	,	,	
		ed,	double,	Input,	,	,	
		crange,	double,	Input,	,	,	
		c_mult,	double,	Input,	,	,	
		s_mult,	double,	Input,	,	,	
		t_mult,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		c_frequency,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		cap,	double *,	Output,	,	,	
		cond,	double *,	Output,	,	,	
		tox,	double *,	Output,	,	,	
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
#include <cmtr_hp4284.h>
#define __4284__

#define Eo 8.854E-14
#define PI 3.14159265358979323846264338327950288419716939937510582
#define DONTCARE 0
#define CV_XMT 11
	END USRLIB MODULE INFORMATION
*/
void C_OX_FR_cap_wellf(char * devname, int hi, int lo, int sub, int chuckcon, double area, double per, char tty, char bty, double vbias, double ed, double crange, double c_mult, double s_mult, double t_mult, double intrange, double c_frequency, double mrange, double lorange, double * cap, double * cond, double * tox);


/* USRLIB MODULE INFORMATION

	MODULE NAME: CALC_ADD
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 3
	ARGUMENTS:
		a,	double,	Input,	,	,	
		b,	double,	Input,	,	,	
		result,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void CALC_ADD(double a, double b, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: CALC_DIV
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 3
	ARGUMENTS:
		a,	double,	Input,	,	,	
		b,	double,	Input,	,	,	
		result,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void CALC_DIV(double a, double b, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: CALC_DWR
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 3
	ARGUMENTS:
		a,	double,	Input,	,	,	
		b,	double,	Input,	,	,	
		result,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void CALC_DWR(double a, double b, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: CALC_ECHO1_dum
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 7
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		Input,	double,	Input,	,	,	
		Inh,	double,	Input,	,	,	
		Inl,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		Output,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <tidp1.h>
	END USRLIB MODULE INFORMATION
*/
void CALC_ECHO1_dum(char * devname, double Input, double Inh, double Inl, double mrange, double lorange, double * Output);


/* USRLIB MODULE INFORMATION

	MODULE NAME: calc_echo_dum
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 8
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		Input,	double,	Input,	,	,	
		abs,	char,	Input,	,	,	
		Inh,	double,	Input,	,	,	
		Inl,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		Output,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void calc_echo_dum(char * devname, double Input, char abs, double Inh, double Inl, double mrange, double lorange, double * Output);


/* USRLIB MODULE INFORMATION

	MODULE NAME: calc_echo_ews
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 17
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		owl,	int,	Input,	,	,	
		ewl,	int,	Input,	,	,	
		obl,	int,	Input,	,	,	
		ebl,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		ipwl,	int,	Input,	,	,	
		dnwl,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		exwl,	int,	Input,	,	,	
		input,	double,	Input,	,	,	
		abs,	char,	Input,	,	,	
		Inh,	double,	Input,	,	,	
		Inl,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		result,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void calc_echo_ews(char * devname, int owl, int ewl, int obl, int ebl, int source, int ipwl, int dnwl, int sub, int exwl, double input, char abs, double Inh, double Inl, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: CALC_ECHO_mos
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 16
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
		input,	double,	Input,	,	,	
		Inh,	double,	Input,	,	,	
		Inl,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		result,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <tidp1.h>
	END USRLIB MODULE INFORMATION
*/
void CALC_ECHO_mos(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double input, double Inh, double Inl, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: calc_echo_mos
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 17
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
		input,	double,	Input,	,	,	
		abs,	char,	Input,	,	,	
		Inh,	double,	Input,	,	,	
		Inl,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		result,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void calc_echo_mos(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double input, char abs, double Inh, double Inl, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: calc_input
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 4
	ARGUMENTS:
		numerator,	double,	Input,	,	,	
		denominator,	double,	Input,	,	,	
		input,	double,	Input,	,	,	
		output,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void calc_input(double numerator, double denominator, double input, double * output);


/* USRLIB MODULE INFORMATION

	MODULE NAME: CALC_MET
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
void CALC_MET(int metal, int * mtl1, int * mtl2, int * mtl3, int * mtl4, int * mtl5);


/* USRLIB MODULE INFORMATION

	MODULE NAME: CALC_METAL
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
void CALC_METAL(int metal, int * mtl1, int * mtl2, int * mtl3, int * mtl4, int * mtl5);


/* USRLIB MODULE INFORMATION

	MODULE NAME: CALC_MULT
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 3
	ARGUMENTS:
		a,	double,	Input,	,	,	
		b,	double,	Input,	,	,	
		result,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void CALC_MULT(double a, double b, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: CALC_MULT_SUB
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 3
	ARGUMENTS:
		a,	double,	Input,	,	,	
		b,	double,	Input,	,	,	
		result,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void CALC_MULT_SUB(double a, double b, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: CALC_SHEET
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 3
	ARGUMENTS:
		a,	double,	Input,	,	,	
		b,	double,	Input,	,	,	
		result,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void CALC_SHEET(double a, double b, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: CALC_SUB
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 3
	ARGUMENTS:
		a,	double,	Input,	,	,	
		b,	double,	Input,	,	,	
		result,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void CALC_SUB(double a, double b, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: CALC_WR
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 3
	ARGUMENTS:
		a,	double,	Input,	,	,	
		b,	double,	Input,	,	,	
		result,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void CALC_WR(double a, double b, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: cap_100K_50mv
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 10
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		vbias,	double,	Input,	,	,	
		area,	double,	Input,	,	,	
		bty,	char,	Input,	,	,	
		ca,	double *,	Output,	,	,	
		ci,	double *,	Output,	,	,	
		ta,	double *,	Output,	,	,	
		ti,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <LBC5_proto.h>
#include <cmtr_hp4284.h>
#include <tidp1.h>
#include "ktxe_types.h"
#include "COM_usrlib.h"
#define DONTCARE 0

#define MAX(a,b)	((a)>(b) ? (a) : (b))

	END USRLIB MODULE INFORMATION
*/
void cap_100K_50mv(int hi, int lo, int subst, double vbias, double area, char bty, double * ca, double * ci, double * ta, double * ti);


/* USRLIB MODULE INFORMATION

	MODULE NAME: cap_1spo_cap
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 18
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
		vbias,	double,	Input,	,	,	
		freq,	double,	Input,	,	,	
		sig,	double,	Input,	,	,	
		stray,	double,	Input,	,	,	
		intrange,	int,	Input,	,	,	
		ddebug,	int,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		cm,	double *,	Output,	,	,	
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
#include <cmtr_hp4284.h>
#define __4284__

#define Eo 8.854E-14
#define PI 3.14159265358979323846264338327950288419716939937510582
#define DONTCARE 0
#define CV_XMT 11

	END USRLIB MODULE INFORMATION
*/
void cap_1spo_cap(char * devname, int hi, int lo, int sub, int chuckcon, double area, double per, char tty, char bty, double vbias, double freq, double sig, double stray, int intrange, int ddebug, double mrange, double lorange, double * cm);


/* USRLIB MODULE INFORMATION

	MODULE NAME: cap_2spo1c_cap
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 25
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
		stray_inv,	double,	Input,	,	,	
		stray_acc,	double,	Input,	,	,	
		intrange,	int,	Input,	,	,	
		ddebug,	int,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		ca,	double *,	Output,	,	,	
		ta,	double *,	Output,	,	,	
		ga,	double *,	Output,	,	,	
		ci,	double *,	Output,	,	,	
		ti,	double *,	Output,	,	,	
		gi,	double *,	Output,	,	,	
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
#include <cmtr_hp4284.h>
#define __4284__

#define Eo 8.854E-14
#define PI 3.14159265358979323846264338327950288419716939937510582
#define DONTCARE 0
#define CV_XMT 11

	END USRLIB MODULE INFORMATION
*/
void cap_2spo1c_cap(char * devname, int hi, int lo, int sub, int chuckcon, double area, double per, char tty, char bty, double vacc, double vinv, double freq, double sig, double stray_inv, double stray_acc, int intrange, int ddebug, double mrange, double lorange, double * ca, double * ta, double * ga, double * ci, double * ti, double * gi);


/* USRLIB MODULE INFORMATION

	MODULE NAME: cap_2spo_cap
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
void cap_2spo_cap(char * devname, int hi, int lo, int sub, int chuckcon, double area, double per, char tty, char bty, double vacc, double vinv, double freq, double sig, double stray, int integ, int ddebug, double mrange, double lorange, double * ca, double * ta, double * ci, double * ti);


/* USRLIB MODULE INFORMATION

	MODULE NAME: cap_2spo_cap_ju
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
void cap_2spo_cap_ju(char * devname, int hi, int lo, int sub, int chuckcon, double area, double per, char tty, char bty, double vacc, double vinv, double freq, double sig, double stray, int integ, int ddebug, double mrange, double lorange, double * ca, double * ta, double * ci, double * ti);


/* USRLIB MODULE INFORMATION

	MODULE NAME: cap_2spo_cap_org
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
void cap_2spo_cap_org(char * devname, int hi, int lo, int sub, int chuckcon, double area, double per, char tty, char bty, double vacc, double vinv, double freq, double sig, double stray, int integ, int ddebug, double mrange, double lorange, double * ca, double * ta, double * ci, double * ti);


/* USRLIB MODULE INFORMATION

	MODULE NAME: cap_2spo_cap_prt
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

#include <stdlib.h>
#include <ktxe_proto.h>
#include <ksox_def.h>
#include <kui_proto.h>
#define CDELAY  125.E-12
#define ILEAK   20.E-12


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
void cap_2spo_cap_prt(char * devname, int hi, int lo, int sub, int chuckcon, double area, double per, char tty, char bty, double vacc, double vinv, double freq, double sig, double stray, int integ, int ddebug, double mrange, double lorange, double * ca, double * ta, double * ci, double * ti);


/* USRLIB MODULE INFORMATION

	MODULE NAME: cap_2spo_cap_re1
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
void cap_2spo_cap_re1(char * devname, int hi, int lo, int sub, int chuckcon, double area, double per, char tty, char bty, double vacc, double vinv, double freq, double sig, double stray, int integ, int ddebug, double mrange, double lorange, double * ca, double * ta, double * ci, double * ti);


/* USRLIB MODULE INFORMATION

	MODULE NAME: cap_2spo_cap_sec
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
void cap_2spo_cap_sec(char * devname, int hi, int lo, int sub, int chuckcon, double area, double per, char tty, char bty, double vacc, double vinv, double freq, double sig, double stray, int integ, int ddebug, double mrange, double lorange, double * ca, double * ta, double * ci, double * ti);


/* USRLIB MODULE INFORMATION

	MODULE NAME: cap_2spo_cap_sec_stry
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
void cap_2spo_cap_sec_stry(char * devname, int hi, int lo, int sub, int chuckcon, double area, double per, char tty, char bty, double vacc, double vinv, double freq, double sig, double stray, int integ, int ddebug, double mrange, double lorange, double * ca, double * ta, double * ci, double * ti);


/* USRLIB MODULE INFORMATION

	MODULE NAME: cap_2spo_cap_stray_hc
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
		str,	double *,	Output,	,	,	
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
void cap_2spo_cap_stray_hc(char * devname, int hi, int lo, int sub, int chuckcon, double area, double per, char tty, char bty, double vacc, double vinv, double freq, double sig, double stray, int integ, int ddebug, double mrange, double lorange, double * ca, double * ta, double * ci, double * ti, double * str);


/* USRLIB MODULE INFORMATION

	MODULE NAME: cap_2spo_kjm
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
void cap_2spo_kjm(char * devname, int hi, int lo, int sub, int chuckcon, double area, double per, char tty, char bty, double vacc, double vinv, double freq, double sig, double stray, int integ, int ddebug, double mrange, double lorange, double * ca, double * ta, double * ci, double * ti);


/* USRLIB MODULE INFORMATION

	MODULE NAME: cap_2spo_para_cap
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
		intrange,	int,	Input,	,	,	
		ddebug,	int,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		ca,	double *,	Output,	,	,	
		ga,	double *,	Output,	,	,	
		ta,	double *,	Output,	,	,	
		ci,	double *,	Output,	,	,	
		gi,	double *,	Output,	,	,	
		ti,	double *,	Output,	,	,	
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
#include <cmtr_hp4284.h>
#define __4284__

#define Eo 8.854E-14
#define PI 3.14159265358979323846264338327950288419716939937510582
#define DONTCARE 0
#define CV_XMT 11

	END USRLIB MODULE INFORMATION
*/
void cap_2spo_para_cap(char * devname, int hi, int lo, int sub, int chuckcon, double area, double per, char tty, char bty, double vacc, double vinv, double freq, double sig, int intrange, int ddebug, double mrange, double lorange, double * ca, double * ga, double * ta, double * ci, double * gi, double * ti);


/* USRLIB MODULE INFORMATION

	MODULE NAME: cap_2spo_port3
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
void cap_2spo_port3(char * devname, int hi, int lo, int sub, int chuckcon, double area, double per, char tty, char bty, double vacc, double vinv, double freq, double sig, double stray, int integ, int ddebug, double mrange, double lorange, double * ca, double * ta, double * ci, double * ti);


/* USRLIB MODULE INFORMATION

	MODULE NAME: cap_2spo_sig
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
void cap_2spo_sig(char * devname, int hi, int lo, int sub, int chuckcon, double area, double per, char tty, char bty, double vacc, double vinv, double freq, double sig, double stray, int integ, int ddebug, double mrange, double lorange, double * ca, double * ta, double * ci, double * ti);


/* USRLIB MODULE INFORMATION

	MODULE NAME: cap_2spo_swp
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
void cap_2spo_swp(char * devname, int hi, int lo, int sub, int chuckcon, double area, double per, char tty, char bty, double vacc, double vinv, double freq, double sig, double stray, int integ, int ddebug, double mrange, double lorange, double * ca, double * ta, double * ci, double * ti);


/* USRLIB MODULE INFORMATION

	MODULE NAME: cap_2spo_tsb
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
void cap_2spo_tsb(char * devname, int hi, int lo, int sub, int chuckcon, double area, double per, char tty, char bty, double vacc, double vinv, double freq, double sig, double stray, int integ, int ddebug, double mrange, double lorange, double * ca, double * ta, double * ci, double * ti);


/* USRLIB MODULE INFORMATION

	MODULE NAME: cap_4284_100K_30mv
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 10
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		vstart,	float,	Input,	,	,	
		vstop,	float,	Input,	,	,	
		vstep,	float,	Input,	,	,	
		area,	float,	Input,	,	,	
		meascap,	F_ARRAY_T,	Output,	,	,	
		cap_size,	int,	Input,	,	,	
		meastox,	float *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <LBC5_proto.h>
#include <cmtr_hp4284.h>

	END USRLIB MODULE INFORMATION
*/
void cap_4284_100K_30mv(int hi, int lo, int subst, float vstart, float vstop, float vstep, float area, float *meascap, int cap_size, float * meastox);


/* USRLIB MODULE INFORMATION

	MODULE NAME: cap_4284_100K_30mv_50mv
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 10
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		vstart,	float,	Input,	,	,	
		vstop,	float,	Input,	,	,	
		vstep,	float,	Input,	,	,	
		area,	float,	Input,	,	,	
		meascap,	F_ARRAY_T,	Output,	,	,	
		cap_size,	int,	Input,	,	,	
		meastox,	float *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <LBC5_proto.h>
#include <cmtr_hp4284.h>

	END USRLIB MODULE INFORMATION
*/
void cap_4284_100K_30mv_50mv(int hi, int lo, int subst, float vstart, float vstop, float vstep, float area, float *meascap, int cap_size, float * meastox);


/* USRLIB MODULE INFORMATION

	MODULE NAME: cap_4284_100K_30mv_610B
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 11
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		vstart,	float,	Input,	,	,	
		vstop,	float,	Input,	,	,	
		vstep,	float,	Input,	,	,	
		area,	float,	Input,	,	,	
		Btype,	char,	Input,	,	,	
		meascap,	F_ARRAY_T,	Output,	,	,	
		cap_size,	int,	Input,	,	,	
		meastox,	float *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <LBC5_proto.h>
#include <cmtr_hp4284.h>

	END USRLIB MODULE INFORMATION
*/
void cap_4284_100K_30mv_610B(int hi, int lo, int subst, float vstart, float vstop, float vstep, float area, char Btype, float *meascap, int cap_size, float * meastox);


/* USRLIB MODULE INFORMATION

	MODULE NAME: cap_4284_100K_50mv_sonos
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 7
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		vbias,	double,	Input,	,	,	
		area,	double,	Input,	,	,	
		c2,	double *,	Output,	,	,	
		t2,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <LBC5_proto.h>
#include <cmtr_hp4284.h>

	END USRLIB MODULE INFORMATION
*/
void cap_4284_100K_50mv_sonos(int hi, int lo, int subst, double vbias, double area, double * c2, double * t2);


/* USRLIB MODULE INFORMATION

	MODULE NAME: cap_4284_100K_50mv_sonos_sweep
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 10
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		vstart,	double,	Input,	,	,	
		vstop,	double,	Input,	,	,	
		vstep,	double,	Input,	,	,	
		area,	double,	Input,	,	,	
		meascap,	D_ARRAY_T,	Output,	,	,	
		cap_size,	int,	Input,	,	,	
		meastox,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <LBC5_proto.h>
#include <cmtr_hp4284.h>

	END USRLIB MODULE INFORMATION
*/
void cap_4284_100K_50mv_sonos_sweep(int hi, int lo, int subst, double vstart, double vstop, double vstep, double area, double *meascap, int cap_size, double * meastox);


/* USRLIB MODULE INFORMATION

	MODULE NAME: cap_dio
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 19
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
		vbias,	double,	Input,	,	,	
		ileak,	double,	Input,	,	,	
		freq,	double,	Input,	,	,	
		sig,	double,	Input,	,	,	
		stray,	double,	Input,	,	,	
		intrange,	int,	Input,	,	,	
		ddebug,	int,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		cm,	double *,	Output,	,	,	
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
#include <cmtr_hp4284.h>
#define __4284__

#define Eo 8.854E-14
#define PI 3.14159265358979323846264338327950288419716939937510582
#define DONTCARE 0
#define CV_XMT 11

	END USRLIB MODULE INFORMATION
*/
void cap_dio(char * devname, int hi, int lo, int sub, int chuckcon, double area, double per, char tty, char bty, double vbias, double ileak, double freq, double sig, double stray, int intrange, int ddebug, double mrange, double lorange, double * cm);


/* USRLIB MODULE INFORMATION

	MODULE NAME: cap_forcev
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 2
	ARGUMENTS:
		cmtrx,	int,	Input,	,	,	
		voltage,	float,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "hp4284_internal.h"
	END USRLIB MODULE INFORMATION
*/
int cap_forcev(int cmtrx, float voltage);


/* USRLIB MODULE INFORMATION

	MODULE NAME: cap_freq
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 2
	ARGUMENTS:
		ieee_addr,	int,	Input,	,	,	
		freq,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void cap_freq(int ieee_addr, double freq);


/* USRLIB MODULE INFORMATION

	MODULE NAME: cap_initialize
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 2
	ARGUMENTS:
		cmtrx,	int,	Input,	,	,	
		initfile,	char *,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "hp4284_internal.h"
#define LINELEN 80
	END USRLIB MODULE INFORMATION
*/
int cap_initialize(int cmtrx, char * initfile);


/* USRLIB MODULE INFORMATION

	MODULE NAME: cap_met
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 18
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		ma,	int,	Input,	,	,	
		mb,	int,	Input,	,	,	
		up,	int,	Input,	,	,	
		lp,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		length,	double,	Input,	,	,	
		vbias,	double,	Input,	,	,	
		freq,	double,	Input,	,	,	
		sig,	double,	Input,	,	,	
		stray,	double,	Input,	,	,	
		up_plt,	char,	Input,	,	,	
		lo_plt,	char,	Input,	,	,	
		intrange,	int,	Input,	,	,	
		ddebug,	int,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		cm,	double *,	Output,	,	,	
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
#include <cmtr_hp4284.h>
#define __4284__

#define Eo 8.854E-14
#define PI 3.14159265358979323846264338327950288419716939937510582
#define DONTCARE 0
#define CV_XMT 11
	END USRLIB MODULE INFORMATION
*/
void cap_met(char * devname, int ma, int mb, int up, int lp, int chuckcon, double length, double vbias, double freq, double sig, double stray, char up_plt, char lo_plt, int intrange, int ddebug, double mrange, double lorange, double * cm);


/* USRLIB MODULE INFORMATION

	MODULE NAME: cap_ono_cap
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
void cap_ono_cap(char * devname, int hi, int lo, int sub, int chuckcon, double area, double per, char tty, char bty, double vacc, double vinv, double freq, double sig, double stray, int integ, int ddebug, double mrange, double lorange, double * ca, double * ta, double * ci, double * ti);


/* USRLIB MODULE INFORMATION

	MODULE NAME: CapInit
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
void CapInit();


/* USRLIB MODULE INFORMATION

	MODULE NAME: Caplp_DUM
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 23
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		pgwb,	double,	Input,	,	,	
		pgwf,	double,	Input,	,	,	
		pgl,	double,	Input,	,	,	
		npf,	double,	Input,	,	,	
		ca1_nch,	double,	Input,	,	,	
		ca1_pch,	double,	Input,	,	,	
		ci1_nch,	double,	Input,	,	,	
		ci1_pch,	double,	Input,	,	,	
		ca3_nch,	double,	Input,	,	,	
		ca3_pch,	double,	Input,	,	,	
		ci3_nch,	double,	Input,	,	,	
		ci3_pch,	double,	Input,	,	,	
		canz1,	double,	Input,	,	,	
		capz1,	double,	Input,	,	,	
		cinz1,	double,	Input,	,	,	
		cipz1,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		lgagn3,	double *,	Output,	,	,	
		lgagp3,	double *,	Output,	,	,	
		lgign3,	double *,	Output,	,	,	
		lgigp3,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void Caplp_DUM(char * devname, double pgwb, double pgwf, double pgl, double npf, double ca1_nch, double ca1_pch, double ci1_nch, double ci1_pch, double ca3_nch, double ca3_pch, double ci3_nch, double ci3_pch, double canz1, double capz1, double cinz1, double cipz1, double mrange, double lorange, double * lgagn3, double * lgagp3, double * lgign3, double * lgigp3);


/* USRLIB MODULE INFORMATION

	MODULE NAME: CAPOFFSET
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
void CAPOFFSET();


/* USRLIB MODULE INFORMATION

	MODULE NAME: cgb_ghi_mos
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
		vbias,	double,	Input,	,	,	
		freq,	double,	Input,	,	,	
		sig,	double,	Input,	,	,	
		stray,	double,	Input,	,	,	
		vbulk,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		debug2,	int,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		gb,	double *,	Output,	,	,	
		g,	double *,	Output,	,	,	
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
#include <cmtr_hp4284.h>
#define __4284__

#define Eo 8.854E-14
#define PI 3.14159265358979323846264338327950288419716939937510582
#define DONTCARE 0
#define CV_XMT 11
	END USRLIB MODULE INFORMATION
*/
void cgb_ghi_mos(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vbias, double freq, double sig, double stray, double vbulk, double intrange, int debug2, double mrange, double lorange, double * gb, double * g);


/* USRLIB MODULE INFORMATION

	MODULE NAME: cgd_cap
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 18
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
		vbias,	double,	Input,	,	,	
		freq,	double,	Input,	,	,	
		sig,	double,	Input,	,	,	
		stray,	double,	Input,	,	,	
		intrange,	int,	Input,	,	,	
		ddebug,	int,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		cm,	double *,	Output,	,	,	
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

#include <cmtr_hp4284.h>
#define __4284__

#define Eo 8.854E-14
#define PI 3.14159265358979323846264338327950288419716939937510582
#define DONTCARE 0
#define CV_XMT 11

	END USRLIB MODULE INFORMATION
*/
void cgd_cap(char * devname, int hi, int lo, int sub, int chuckcon, double area, double per, char tty, char bty, double vbias, double freq, double sig, double stray, int intrange, int ddebug, double mrange, double lorange, double * cm);


/* USRLIB MODULE INFORMATION

	MODULE NAME: cgd_ghi_mos
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
		vbias,	double,	Input,	,	,	
		freq,	double,	Input,	,	,	
		sig,	double,	Input,	,	,	
		stray,	double,	Input,	,	,	
		vbulk,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		debug2,	int,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		dg,	double *,	Output,	,	,	
		g,	double *,	Output,	,	,	
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
#include <cmtr_hp4284.h>
#define __4284__

#define Eo 8.854E-14
#define PI 3.14159265358979323846264338327950288419716939937510582
#define DONTCARE 0
#define CV_XMT 11
	END USRLIB MODULE INFORMATION
*/
void cgd_ghi_mos(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vbias, double freq, double sig, double stray, double vbulk, double intrange, int debug2, double mrange, double lorange, double * dg, double * g);


/* USRLIB MODULE INFORMATION

	MODULE NAME: cgd_mos
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
		vbias,	double,	Input,	,	,	
		freq,	double,	Input,	,	,	
		sig,	double,	Input,	,	,	
		stray,	double,	Input,	,	,	
		vbulk,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		debug2,	int,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		cm,	double *,	Output,	,	,	
		g,	double *,	Output,	,	,	
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

#include <cmtr_hp4284.h>
#define __4284__

#define Eo 8.854E-14
#define PI 3.14159265358979323846264338327950288419716939937510582
#define DONTCARE 0
#define CV_XMT 11

	END USRLIB MODULE INFORMATION
*/
void cgd_mos(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vbias, double freq, double sig, double stray, double vbulk, double intrange, int debug2, double mrange, double lorange, double * cm, double * g);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Chain_cst
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
void  Chain_cst(char *, int, int, int, int, int, double, double, char, double, double, double, double, double, double, double, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: chain_cst
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
		vforce,	double,	Input,	5,	,	
		ilimit,	double,	Input,	1e-06,	,	
		vsub,	double,	Input,	0,	,	
		delaytime,	double,	Input,	0.01,	,	
		intrange,	double,	Input,	1,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		Rch,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void chain_cst(char * devname, int t1, int t2, int sub, int bulk, int chuckcon, double num_contacts, double area, char type, double vforce, double ilimit, double vsub, double delaytime, double intrange, double mrange, double lorange, double * Rch);


/* USRLIB MODULE INFORMATION

	MODULE NAME: chain_cst_icms
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 17
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		T1,	int,	Input,	,	,	
		T2,	int,	Input,	,	,	
		Sub,	int,	Input,	,	,	
		Bulk,	int,	Input,	,	,	
		Chuck,	int,	Input,	,	,	
		N,	double,	Input,	,	,	
		Area,	double,	Input,	,	,	
		Type,	char,	Input,	,	,	
		vforce,	double,	Input,	5,	,	
		ilimit,	double,	Input,	1e-06,	,	
		vsub,	double,	Input,	0,	,	
		delaytime,	double,	Input,	0.01,	,	
		intrange,	double,	Input,	0,	,	
		mrange,	double,	Input,	 ,	,	
		lorange,	double,	Input,	 ,	,	
		Rch,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void chain_cst_icms(char * devname, int T1, int T2, int Sub, int Bulk, int Chuck, double N, double Area, char Type, double vforce, double ilimit, double vsub, double delaytime, double intrange, double mrange, double lorange, double * Rch);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Chain_cst_org
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
void Chain_cst_org(char * devname, int t1, int t2, int sub, int bulk, int chuckcon, double num_contacts, double area, char type, double vforce, double ilimit, double vsub, double delaytime, double intrange, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Chain_cst_org160909
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
void  Chain_cst_org160909(char *, int, int, int, int, int, double, double, char, double, double, double, double, double, double, double, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: Chain_cst_tester_pin_change
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 22
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
		change_on_PT11,	double,	Input,	,	,	
		change_on_PT12,	double,	Input,	,	,	
		change_on_PT13,	double,	Input,	,	,	
		change_on_PT14,	double,	Input,	,	,	
		change_on_PT16,	double,	Input,	,	,	
		result,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#include <tidp1.h>
#include <unistd.h>
#include <netdb.h>
	END USRLIB MODULE INFORMATION
*/
void  Chain_cst_tester_pin_change(char *, int, int, int, int, int, double, double, char, double, double, double, double, double, double, double, double, double, double, double, double, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: chain_vi_cst
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 18
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
		vforce,	double,	Input,	5,	,	
		ilimit,	double,	Input,	1e-06,	,	
		vsub,	double,	Input,	0,	,	
		delaytime,	double,	Input,	0.01,	,	
		intrange,	double,	Input,	1,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		Rch,	double *,	Output,	,	,	
		FVI,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void chain_vi_cst(char * devname, int t1, int t2, int sub, int bulk, int chuckcon, double num_contacts, double area, char type, double vforce, double ilimit, double vsub, double delaytime, double intrange, double mrange, double lorange, double * Rch, double * FVI);


/* USRLIB MODULE INFORMATION

	MODULE NAME: chain_vi_cst_icms
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 18
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		T1,	int,	Input,	,	,	
		T2,	int,	Input,	,	,	
		Sub,	int,	Input,	,	,	
		Bulk,	int,	Input,	,	,	
		Chuck,	int,	Input,	,	,	
		N,	double,	Input,	,	,	
		Area,	double,	Input,	,	,	
		Type,	char,	Input,	,	,	
		vforce,	double,	Input,	5,	,	
		ilimit,	double,	Input,	1e-06,	,	
		vsub,	double,	Input,	0,	,	
		delaytime,	double,	Input,	0.01,	,	
		intrange,	double,	Input,	0,	,	
		mrange,	double,	Input,	 ,	,	
		lorange,	double,	Input,	 ,	,	
		Rch,	double *,	Output,	,	,	
		FVI,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void chain_vi_cst_icms(char * devname, int T1, int T2, int Sub, int Bulk, int Chuck, double N, double Area, char Type, double vforce, double ilimit, double vsub, double delaytime, double intrange, double mrange, double lorange, double * Rch, double * FVI);


/* USRLIB MODULE INFORMATION

	MODULE NAME: chuck_dn
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
void chuck_dn();


/* USRLIB MODULE INFORMATION

	MODULE NAME: chuck_up
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
void chuck_up();


/* USRLIB MODULE INFORMATION

	MODULE NAME: cont_klv_cer
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
void cont_klv_cer(char * devname, int f1, int f2, int s1, int s2, int sub, int bulk, int chuckcon, double area, char type, double iforce, double vlimit, double vsub, double delaytime, double intrange, double mrange, double lorange, double * Rc, double * R1, double * R2);


/* USRLIB MODULE INFORMATION

	MODULE NAME: cont_res_cer
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 18
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
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void cont_res_cer(char * devname, int f1, int f2, int s1, int s2, int sub, int bulk, int chuckcon, double area, char type, double iforce, double vlimit, double vsub, double delaytime, double intrange, double mrange, double lorange, double * Rc);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Cont_res_cer
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 18
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		force1,	int,	Input,	,	,	
		force2,	int,	Input,	,	,	
		meas1,	int,	Input,	,	,	
		meas2,	int,	Input,	,	,	
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
void Cont_res_cer(char * devname, int force1, int force2, int meas1, int meas2, int sub, int bulk, int chuckcon, double area, char type, double iforce, double vlimit, double vsub, double delaytime, double intrange, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: cont_res_cer_cro
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 18
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
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void cont_res_cer_cro(char * devname, int f1, int f2, int s1, int s2, int sub, int bulk, int chuckcon, double area, char type, double iforce, double vlimit, double vsub, double delaytime, double intrange, double mrange, double lorange, double * Rc);


/* USRLIB MODULE INFORMATION

	MODULE NAME: cont_res_cer_icms
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 18
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		TA,	int,	Input,	,	,	
		BA,	int,	Input,	,	,	
		TB,	int,	Input,	,	,	
		BB,	int,	Input,	,	,	
		Sub,	int,	Input,	,	,	
		Bulk,	int,	Input,	,	,	
		Chuck,	int,	Input,	,	,	
		Area,	double,	Input,	,	,	
		Type,	char,	Input,	,	,	
		iforce,	double,	Input,	,	,	
		vlimit,	double,	Input,	,	,	
		vsub,	double,	Input,	,	,	
		delaytime,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		Rc,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void cont_res_cer_icms(char * devname, int TA, int BA, int TB, int BB, int Sub, int Bulk, int Chuck, double Area, char Type, double iforce, double vlimit, double vsub, double delaytime, double intrange, double mrange, double lorange, double * Rc);


/* USRLIB MODULE INFORMATION

	MODULE NAME: cont_res_cer_sw
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 18
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
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void cont_res_cer_sw(char * devname, int f1, int f2, int s1, int s2, int sub, int bulk, int chuckcon, double area, char type, double iforce, double vlimit, double vsub, double delaytime, double intrange, double mrange, double lorange, double * Rc);


/* USRLIB MODULE INFORMATION

	MODULE NAME: cox_cap
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
		vbias,	double,	Input,	,	,	
		ed,	double,	Input,	3.9,	,	
		crange,	double,	Input,	,	,	
		c_mult,	double,	Input,	,	,	
		s_mult,	double,	Input,	,	,	
		t_mult,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		freq,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		cap,	double *,	Output,	,	,	
		cond,	double *,	Output,	,	,	
		tox,	double *,	Output,	,	,	
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

#include <cmtr_hp4284.h>
#define __4284__

#define Eo 8.854E-14
#define PI 3.14159265358979323846264338327950288419716939937510582
#define DONTCARE 0
#define CV_XMT 11

	END USRLIB MODULE INFORMATION
*/
void cox_cap(char * devname, int hi, int lo, int sub, int chuckcon, double area, double per, char tty, char bty, double vbias, double ed, double crange, double c_mult, double s_mult, double t_mult, double intrange, double freq, double mrange, double lorange, double * cap, double * cond, double * tox);


/* USRLIB MODULE INFORMATION

	MODULE NAME: cv_g2b_cap
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 27
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		top,	int,	Input,	,	,	
		bot,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		area,	double,	Input,	,	,	
		perimeter,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
		btype,	char,	Input,	,	,	
		vstart,	double,	Input,	,	,	
		vstop,	double,	Input,	,	,	
		vstep,	double,	Input,	,	,	
		freq,	double,	Input,	,	,	
		sig,	double,	Input,	,	,	
		stray,	double,	Input,	,	,	
		hold,	double,	Input,	,	,	
		delay,	double,	Input,	,	,	
		intrange,	int,	Input,	,	,	
		ddebug,	int,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		npt,	D_ARRAY_T,	Output,	,	,	
		ary_size,	int,	Input,	100,	,	
		v1,	D_ARRAY_T,	Output,	,	,	
		ary_size2,	int,	Input,	100,	,	
		c1,	D_ARRAY_T,	Output,	,	,	
		ary_size3,	int,	Input,	100,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
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
void cv_g2b_cap(char * devname, int top, int bot, int sub, int chuckcon, double area, double perimeter, char type, char btype, double vstart, double vstop, double vstep, double freq, double sig, double stray, double hold, double delay, int intrange, int ddebug, double mrange, double lorange, double *npt, int ary_size, double *v1, int ary_size2, double *c1, int ary_size3);


/* USRLIB MODULE INFORMATION

	MODULE NAME: cv_g2c_cap
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 27
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		top,	int,	Input,	,	,	
		bot,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		area,	double,	Input,	,	,	
		perimeter,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
		btype,	char,	Input,	,	,	
		vstart,	double,	Input,	,	,	
		vstop,	double,	Input,	,	,	
		vstep,	double,	Input,	,	,	
		freq,	double,	Input,	,	,	
		sig,	double,	Input,	,	,	
		stray,	double,	Input,	,	,	
		hold,	double,	Input,	,	,	
		delaytime,	double,	Input,	,	,	
		intrange,	int,	Input,	,	,	
		ddebug,	int,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		npt,	D_ARRAY_T,	Output,	,	,	
		ary_size,	int,	Input,	100,	,	
		v1,	D_ARRAY_T,	Output,	,	,	
		ary_size2,	int,	Input,	100,	,	
		c1,	D_ARRAY_T,	Output,	,	,	
		ary_size3,	int,	Input,	100,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
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
void cv_g2c_cap(char * devname, int top, int bot, int sub, int chuckcon, double area, double perimeter, char type, char btype, double vstart, double vstop, double vstep, double freq, double sig, double stray, double hold, double delaytime, int intrange, int ddebug, double mrange, double lorange, double *npt, int ary_size, double *v1, int ary_size2, double *c1, int ary_size3);


/* USRLIB MODULE INFORMATION

	MODULE NAME: cv_gate_cap
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 28
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		top,	int,	Input,	,	,	
		bot,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		area,	double,	Input,	,	,	
		perimeter,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
		btype,	char,	Input,	,	,	
		vstart,	double,	Input,	,	,	
		vstop,	double,	Input,	,	,	
		vstep,	double,	Input,	,	,	
		freq,	double,	Input,	,	,	
		sig,	double,	Input,	,	,	
		stray,	double,	Input,	,	,	
		hold,	double,	Input,	,	,	
		delaytime,	double,	Input,	,	,	
		intrange,	int,	Input,	,	,	
		ddebug,	int,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		vfb,	double *,	Output,	,	,	
		npt,	D_ARRAY_T,	Output,	,	,	
		ary_size,	int,	Input,	100,	,	
		v1,	D_ARRAY_T,	Output,	,	,	
		ary_size2,	int,	Input,	100,	,	
		c1,	D_ARRAY_T,	Output,	,	,	
		ary_siz3,	int,	Input,	100,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
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
void cv_gate_cap(char * devname, int top, int bot, int sub, int chuckcon, double area, double perimeter, char type, char btype, double vstart, double vstop, double vstep, double freq, double sig, double stray, double hold, double delaytime, int intrange, int ddebug, double mrange, double lorange, double * vfb, double *npt, int ary_size, double *v1, int ary_size2, double *c1, int ary_siz3);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ddtime
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		Parm1,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void ddtime(double * Parm1);


/* USRLIB MODULE INFORMATION

	MODULE NAME: dio_1cap_dio
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 19
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
		vbias,	double,	Input,	,	,	
		ileak,	double,	Input,	,	,	
		freq,	double,	Input,	,	,	
		sig,	double,	Input,	,	,	
		stray,	double,	Input,	,	,	
		intrange,	int,	Input,	,	,	
		ddebug,	int,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		cm,	double *,	Output,	,	,	
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

#include <cmtr_hp4284.h>
#define __4284__

#define Eo 8.854E-14
#define PI 3.14159265358979323846264338327950288419716939937510582
#define DONTCARE 0
#define CV_XMT 11

	END USRLIB MODULE INFORMATION
*/
void dio_1cap_dio(char * devname, int hi, int lo, int sub, int chuckcon, double area, double per, char tty, char bty, double vbias, double ileak, double freq, double sig, double stray, int intrange, int ddebug, double mrange, double lorange, double * cm);


/* USRLIB MODULE INFORMATION

	MODULE NAME: dio_1spo_dio
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 19
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
		vbias,	double,	Input,	,	,	
		ileak,	double,	Input,	,	,	
		freq,	double,	Input,	,	,	
		sig,	double,	Input,	,	,	
		stray,	double,	Input,	,	,	
		intrange,	int,	Input,	,	,	
		ddebug,	int,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		cm,	double *,	Output,	,	,	
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

#include <cmtr_hp4284.h>
#define __4284__

#define Eo 8.854E-14
#define PI 3.14159265358979323846264338327950288419716939937510582
#define DONTCARE 0
#define CV_XMT 11


	END USRLIB MODULE INFORMATION
*/
void dio_1spo_dio(char * devname, int hi, int lo, int sub, int chuckcon, double area, double per, char tty, char bty, double vbias, double ileak, double freq, double sig, double stray, int intrange, int ddebug, double mrange, double lorange, double * cm);


/* USRLIB MODULE INFORMATION

	MODULE NAME: dsweep_mos
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 35
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
		Vdmin,	double,	Input,	,	,	
		Vdmax,	double,	Input,	,	,	
		Vdstp,	double,	Input,	,	,	
		Icmp,	double,	Input,	,	,	
		Vb,	double,	Input,	,	,	
		Vg,	double,	Input,	,	,	
		Vs,	double,	Input,	,	,	
		Dly,	double,	Input,	,	,	
		ddebug,	int,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		Flg,	double *,	Output,	,	,	
		npt,	D_ARRAY_T,	Output,	,	,	
		ary_size,	int,	Input,	100,	,	
		Ig,	D_ARRAY_T,	Output,	,	,	
		asize2,	int,	Input,	100,	,	
		Id,	D_ARRAY_T,	Output,	,	,	
		asize3,	int,	Input,	100,	,	
		Is,	D_ARRAY_T,	Output,	,	,	
		asize4,	int,	Input,	100,	,	
		Ib,	D_ARRAY_T,	Output,	,	,	
		asize5,	int,	Input,	100,	,	
		Vd,	D_ARRAY_T,	Output,	,	,	
		asize6,	int,	Input,	100,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void dsweep_mos(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double Vdmin, double Vdmax, double Vdstp, double Icmp, double Vb, double Vg, double Vs, double Dly, int ddebug, double intrange, double mrange, double lorange, double * Flg, double *npt, int ary_size, double *Ig, int asize2, double *Id, int asize3, double *Is, int asize4, double *Ib, int asize5, double *Vd, int asize6);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ews_lk1_ews
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 26
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		owl,	int,	Input,	,	,	
		ewl,	int,	Input,	,	,	
		obl,	int,	Input,	,	,	
		ebl,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		ipwl,	int,	Input,	,	,	
		dnwl,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		exwl,	int,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vgs,	double,	Input,	,	,	
		vbs,	double,	Input,	,	,	
		ilimit,	double,	Input,	,	,	
		icomp,	double,	Input,	,	,	
		irange,	double,	Input,	,	,	
		delaytime,	double,	Input,	,	,	
		multiplier,	double,	Input,	,	,	
		vss,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		id1,	double *,	Output,	,	,	
		is1,	double *,	Output,	,	,	
		ig1,	double *,	Output,	,	,	
		isub1,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void ews_lk1_ews(char * devname, int owl, int ewl, int obl, int ebl, int source, int ipwl, int dnwl, int sub, int exwl, double vds, double vgs, double vbs, double ilimit, double icomp, double irange, double delaytime, double multiplier, double vss, double intrange, double mrange, double lorange, double * id1, double * is1, double * ig1, double * isub1);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ews_lk1_ews_n
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 26
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		owl,	int,	Input,	,	,	
		ewl,	int,	Input,	,	,	
		obl,	int,	Input,	,	,	
		ebl,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		ipwl,	int,	Input,	,	,	
		dnwl,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		exwl,	int,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vgs,	double,	Input,	,	,	
		vbs,	double,	Input,	,	,	
		ilimit,	double,	Input,	,	,	
		icomp,	double,	Input,	,	,	
		irange,	double,	Input,	,	,	
		delaytime,	double,	Input,	,	,	
		multiplier,	double,	Input,	,	,	
		vss,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		id1,	double *,	Output,	,	,	
		is1,	double *,	Output,	,	,	
		ig1,	double *,	Output,	,	,	
		isub1,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void ews_lk1_ews_n(char * devname, int owl, int ewl, int obl, int ebl, int source, int ipwl, int dnwl, int sub, int exwl, double vds, double vgs, double vbs, double ilimit, double icomp, double irange, double delaytime, double multiplier, double vss, double intrange, double mrange, double lorange, double * id1, double * is1, double * ig1, double * isub1);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ews_lk2_ews
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 25
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		owl,	int,	Input,	,	,	
		ewl,	int,	Input,	,	,	
		obl,	int,	Input,	,	,	
		ebl,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		ipwl,	int,	Input,	,	,	
		dnwl,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		exwl,	int,	Input,	,	,	
		vdodd,	double,	Input,	,	,	
		vgodd,	double,	Input,	,	,	
		vgeven,	double,	Input,	,	,	
		ilimit,	double,	Input,	,	,	
		icomp,	double,	Input,	,	,	
		irange,	double,	Input,	,	,	
		delaytime,	double,	Input,	,	,	
		multiplier,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		id2odd,	double *,	Output,	,	,	
		ig2odd,	double *,	Output,	,	,	
		ig2even,	double *,	Output,	,	,	
		icom2,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void ews_lk2_ews(char * devname, int owl, int ewl, int obl, int ebl, int source, int ipwl, int dnwl, int sub, int exwl, double vdodd, double vgodd, double vgeven, double ilimit, double icomp, double irange, double delaytime, double multiplier, double intrange, double mrange, double lorange, double * id2odd, double * ig2odd, double * ig2even, double * icom2);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ews_lk2_ews_n
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 25
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		owl,	int,	Input,	,	,	
		ewl,	int,	Input,	,	,	
		obl,	int,	Input,	,	,	
		ebl,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		ipwl,	int,	Input,	,	,	
		dnwl,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		exwl,	int,	Input,	,	,	
		vdodd,	double,	Input,	,	,	
		vgodd,	double,	Input,	,	,	
		vgeven,	double,	Input,	,	,	
		ilimit,	double,	Input,	,	,	
		icomp,	double,	Input,	,	,	
		irange,	double,	Input,	,	,	
		delaytime,	double,	Input,	,	,	
		multiplier,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		id2odd,	double *,	Output,	,	,	
		ig2odd,	double *,	Output,	,	,	
		ig2even,	double *,	Output,	,	,	
		icom2,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void ews_lk2_ews_n(char * devname, int owl, int ewl, int obl, int ebl, int source, int ipwl, int dnwl, int sub, int exwl, double vdodd, double vgodd, double vgeven, double ilimit, double icomp, double irange, double delaytime, double multiplier, double intrange, double mrange, double lorange, double * id2odd, double * ig2odd, double * ig2even, double * icom2);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ews_lk3_ews
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 24
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		owl,	int,	Input,	,	,	
		ewl,	int,	Input,	,	,	
		obl,	int,	Input,	,	,	
		ebl,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		ipwl,	int,	Input,	,	,	
		dnwl,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		exwl,	int,	Input,	,	,	
		vdodd,	double,	Input,	,	,	
		vg,	double,	Input,	,	,	
		ilimit,	double,	Input,	,	,	
		icomp,	double,	Input,	,	,	
		irange,	double,	Input,	,	,	
		delaytime,	double,	Input,	,	,	
		multiplier,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		id3odd,	double *,	Output,	,	,	
		id3even,	double *,	Output,	,	,	
		ig3,	double *,	Output,	,	,	
		icom3,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void ews_lk3_ews(char * devname, int owl, int ewl, int obl, int ebl, int source, int ipwl, int dnwl, int sub, int exwl, double vdodd, double vg, double ilimit, double icomp, double irange, double delaytime, double multiplier, double intrange, double mrange, double lorange, double * id3odd, double * id3even, double * ig3, double * icom3);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ews_lk3_ews_n
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 24
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		owl,	int,	Input,	,	,	
		ewl,	int,	Input,	,	,	
		obl,	int,	Input,	,	,	
		ebl,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		ipwl,	int,	Input,	,	,	
		dnwl,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		exwl,	int,	Input,	,	,	
		vdodd,	double,	Input,	,	,	
		vg,	double,	Input,	,	,	
		ilimit,	double,	Input,	,	,	
		icomp,	double,	Input,	,	,	
		irange,	double,	Input,	,	,	
		delaytime,	double,	Input,	,	,	
		multiplier,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		id3odd,	double *,	Output,	,	,	
		id3even,	double *,	Output,	,	,	
		ig3,	double *,	Output,	,	,	
		icom3,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void ews_lk3_ews_n(char * devname, int owl, int ewl, int obl, int ebl, int source, int ipwl, int dnwl, int sub, int exwl, double vdodd, double vg, double ilimit, double icomp, double irange, double delaytime, double multiplier, double intrange, double mrange, double lorange, double * id3odd, double * id3even, double * ig3, double * icom3);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ews_pe_f05_ews
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 18
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		owl,	int,	Input,	,	,	
		ewl,	int,	Input,	,	,	
		obl,	int,	Input,	,	,	
		ebl,	int,	Input,	,	,	
		src,	int,	Input,	,	,	
		ipwl,	int,	Input,	,	,	
		dnwl,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		exwl,	int,	Input,	,	,	
		vgate,	double,	Input,	,	,	
		vsub,	double,	Input,	,	,	
		pls_width,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		vt,	double *,	Output,	,	,	
		vtp,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void ews_pe_f05_ews(char * devname, int owl, int ewl, int obl, int ebl, int src, int ipwl, int dnwl, int sub, int exwl, double vgate, double vsub, double pls_width, double intrange, double mrange, double lorange, double * vt, double * vtp);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Ews_pe_f05_ews_dbg
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 18
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		owl,	int,	Input,	,	,	
		ewl,	int,	Input,	,	,	
		obl,	int,	Input,	,	,	
		ebl,	int,	Input,	,	,	
		src,	int,	Input,	,	,	
		ipwl,	int,	Input,	,	,	
		dnwl,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		exwl,	int,	Input,	,	,	
		vgate,	double,	Input,	,	,	
		vsub,	double,	Input,	,	,	
		pls_width,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		vtp,	double *,	Output,	,	,	
		vt,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void Ews_pe_f05_ews_dbg(char * devname, int owl, int ewl, int obl, int ebl, int src, int ipwl, int dnwl, int sub, int exwl, double vgate, double vsub, double pls_width, double intrange, double mrange, double lorange, double * vtp, double * vt);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Fetcheck_d_mos_tsb
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
void Fetcheck_d_mos_tsb(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vdsf, double vgs_off, double vgs_on, double vbsf, double intrange, double mrange, double lorange, double * ioff, double * ids);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Fetcheck_eABS_mos
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
void Fetcheck_eABS_mos(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vdsf, double vgs_off, double vgs_on, double vbsf, double intrange, double mrange, double lorange, double * ioff, double * ids);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Fetcheck_mcram_mos
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
		vds_off,	double,	Input,	,	,	
		vds_on,	double,	Input,	,	,	
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
void Fetcheck_mcram_mos(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vds_off, double vds_on, double vgs_off, double vgs_on, double vbsf, double intrange, double mrange, double lorange, double * ioff, double * ids);


/* USRLIB MODULE INFORMATION

	MODULE NAME: fetcheck_mos
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
		ids,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void fetcheck_mos(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vdsf, double vgs_off, double vgs_on, double vbsf, double factor, double delay, double intrange, double mrange, double lorange, double * ioff, double * ids);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Fetcheck_mos
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
void Fetcheck_mos(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vdsf, double vgs_off, double vgs_on, double vbsf, double intrange, double mrange, double lorange, double * ioff, double * ids);


/* USRLIB MODULE INFORMATION

	MODULE NAME: fetcheck_mos_c
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
		ids,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void fetcheck_mos_c(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vdsf, double vgs_off, double vgs_on, double vbsf, double factor, double delay, double intrange, double mrange, double lorange, double * ioff, double * ids);


/* USRLIB MODULE INFORMATION

	MODULE NAME: fetcheck_mos_sf
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
void fetcheck_mos_sf(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vdsf, double vgs_off, double vgs_on, double vbsf, double factor, double delay, double intrange, double mrange, double lorange, double * ioff, double * ids);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Fetcheck_mos_time
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
void Fetcheck_mos_time(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vdsf, double vgs_off, double vgs_on, double vbsf, double intrange, double mrange, double lorange, double * ioff, double * ids);


/* USRLIB MODULE INFORMATION

	MODULE NAME: fetcheck_mos_TTR_DI
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
		ids,	double *,	Output,	,	,	
		Parm22,	D_ARRAY_T,	Output,	 ,	 ,	 
		Parm22_size,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void fetcheck_mos_TTR_DI(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vdsf, double vgs_off, double vgs_on, double vbsf, double factor, double delay, double intrange, double mrange, double lorange, double * ioff, double * ids, double *Parm22, int Parm22_size);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Fetcheck_n_mos
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
void Fetcheck_n_mos(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vdsf, double vgs_off, double vgs_on, double vbsf, double intrange, double mrange, double lorange, double * ioff, double * ids);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Fetcheck_n_mos_40V
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
void Fetcheck_n_mos_40V(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vdsf, double vgs_off, double vgs_on, double vbsf, double intrange, double mrange, double lorange, double * ioff, double * ids);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Fetcheck_n_mos_re1
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
void Fetcheck_n_mos_re1(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vdsf, double vgs_off, double vgs_on, double vbsf, double intrange, double mrange, double lorange, double * ioff, double * ids);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Fetcheck_n_mos_sc
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
void Fetcheck_n_mos_sc(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vdsf, double vgs_off, double vgs_on, double vbsf, double intrange, double mrange, double lorange, double * ioff, double * ids);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Fetcheck_n_mos_tsb
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
void Fetcheck_n_mos_tsb(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vdsf, double vgs_off, double vgs_on, double vbsf, double intrange, double mrange, double lorange, double * ioff, double * ids);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Fetcheck_n_mos_tsb_ft
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
void Fetcheck_n_mos_tsb_ft(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vdsf, double vgs_off, double vgs_on, double vbsf, double intrange, double mrange, double lorange, double * ioff, double * ids);


/* USRLIB MODULE INFORMATION

	MODULE NAME: fetcheck_ro
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
		vdd,	int,	Input,	,	,	
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
		ids,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void fetcheck_ro(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, int vdd, double width, double length, char type, double vdsf, double vgs_off, double vgs_on, double vbsf, double factor, double delay, double intrange, double mrange, double lorange, double * ioff, double * ids);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Fetcheck_SW_mos
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
void Fetcheck_SW_mos(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vdsf, double vgs_off, double vgs_on, double vbsf, double intrange, double mrange, double lorange, double * ioff, double * ids);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Fetcheck_SW_mos_re1
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
void Fetcheck_SW_mos_re1(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vdsf, double vgs_off, double vgs_on, double vbsf, double intrange, double mrange, double lorange, double * ioff, double * ids);


/* USRLIB MODULE INFORMATION

	MODULE NAME: gdbreakv_gdi
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 25
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		top,	int,	Input,	,	,	
		bot,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		Darea,	double,	Input,	,	,	
		Dper,	double,	Input,	,	,	
		Garea,	double,	Input,	,	,	
		Gper,	double,	Input,	,	,	
		Ovrlap,	double,	Input,	,	,	
		Dtype,	char,	Input,	,	,	
		Gtype,	char,	Input,	,	,	
		imax,	double,	Input,	,	,	
		vmin,	double,	Input,	,	,	
		vmax,	double,	Input,	,	,	
		vstep,	double,	Input,	,	,	
		vgt,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		Bdv,	double *,	Output,	,	,	
		Ibot,	double *,	Output,	,	,	
		Itop,	double *,	Output,	,	,	
		Igate,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void gdbreakv_gdi(char * devname, int top, int bot, int gate, int sub, int chuckcon, double Darea, double Dper, double Garea, double Gper, double Ovrlap, char Dtype, char Gtype, double imax, double vmin, double vmax, double vstep, double vgt, double intrange, double mrange, double lorange, double * Bdv, double * Ibot, double * Itop, double * Igate);


/* USRLIB MODULE INFORMATION

	MODULE NAME: gdiocap_gdi
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 24
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		darea,	double,	Input,	,	,	
		dper,	double,	Input,	,	,	
		garea,	double,	Input,	,	,	
		gper,	double,	Input,	,	,	
		ovrlap,	double,	Input,	,	,	
		dty,	char,	Input,	,	,	
		gty,	char,	Input,	,	,	
		vmeas,	double,	Input,	,	,	
		ileak,	double,	Input,	,	,	
		vgb,	double,	Input,	,	,	
		freq,	double,	Input,	,	,	
		sig,	double,	Input,	,	,	
		stray,	double,	Input,	,	,	
		intrange,	int,	Input,	,	,	
		ddebug,	int,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		cm,	double *,	Output,	,	,	
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

#include <cmtr_hp4284.h>
#define __4284__

#define Eo 8.854E-14
#define PI 3.14159265358979323846264338327950288419716939937510582
#define DONTCARE 0
#define CV_XMT 11


	END USRLIB MODULE INFORMATION
*/
void gdiocap_gdi(char * devname, int hi, int lo, int gate, int sub, int chuckcon, double darea, double dper, double garea, double gper, double ovrlap, char dty, char gty, double vmeas, double ileak, double vgb, double freq, double sig, double stray, int intrange, int ddebug, double mrange, double lorange, double * cm);


/* USRLIB MODULE INFORMATION

	MODULE NAME: gdiolk_gdi
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 30
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		top,	int,	Input,	,	,	
		bot,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		darea,	double,	Input,	,	,	
		dper,	double,	Input,	,	,	
		garea,	double,	Input,	,	,	
		gper,	double,	Input,	,	,	
		ovrlap,	double,	Input,	,	,	
		dty,	char,	Input,	,	,	
		gty,	char,	Input,	,	,	
		vrv,	double,	Input,	,	,	
		vgt,	double,	Input,	,	,	
		ilo,	double,	Input,	,	,	
		vfr,	double,	Input,	,	,	
		igd,	double,	Input,	,	,	
		igtgd,	double,	Input,	,	,	
		itplim,	double,	Input,	,	,	
		igtlim,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		debug2,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		ir1,	double *,	Output,	,	,	
		ir2,	double *,	Output,	,	,	
		if1,	double *,	Output,	,	,	
		if2,	double *,	Output,	,	,	
		igt,	double *,	Output,	,	,	
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
void gdiolk_gdi(char * devname, int top, int bot, int gate, int sub, int chuckcon, double darea, double dper, double garea, double gper, double ovrlap, char dty, char gty, double vrv, double vgt, double ilo, double vfr, double igd, double igtgd, double itplim, double igtlim, double intrange, double debug2, double mrange, double lorange, double * ir1, double * ir2, double * if1, double * if2, double * igt);


/* USRLIB MODULE INFORMATION

	MODULE NAME: gdleak_gdi
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 22
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		top,	int,	Input,	,	,	
		bot,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		Darea,	double,	Input,	,	,	
		Dper,	double,	Input,	,	,	
		Garea,	double,	Input,	,	,	
		Gper,	double,	Input,	,	,	
		Ovrlap,	double,	Input,	,	,	
		Dtype,	char,	Input,	,	,	
		Gtype,	char,	Input,	,	,	
		vrev,	double,	Input,	,	,	
		vgt,	double,	Input,	,	,	
		Delay,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		Ibot,	double *,	Output,	,	,	
		Itop,	double *,	Output,	,	,	
		Igate,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void gdleak_gdi(char * devname, int top, int bot, int gate, int sub, int chuckcon, double Darea, double Dper, double Garea, double Gper, double Ovrlap, char Dtype, char Gtype, double vrev, double vgt, double Delay, double intrange, double mrange, double lorange, double * Ibot, double * Itop, double * Igate);


/* USRLIB MODULE INFORMATION

	MODULE NAME: GETCAPOFFSET
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 3
	ARGUMENTS:
		TestName,	char *,	Input,	 ,	 ,	 
		MeasuredCap,	double,	Input,	 ,	 ,	 
		Offset,	double *,	Output,	 ,	 ,	 
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "ktxe_types.h"
#include "COM_usrlib.h"
#include <string.h>
	END USRLIB MODULE INFORMATION
*/
void GETCAPOFFSET(char * TestName, double MeasuredCap, double * Offset);


/* USRLIB MODULE INFORMATION

	MODULE NAME: GETCAPOFFSETARRAY
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 5
	ARGUMENTS:
		TestName,	char *,	Input,	,	,	
		MeasuredCapArray,	D_ARRAY_T,	Input,	,	,	
		npts,	int,	Input,	,	,	
		OffsetArray,	D_ARRAY_T,	Output,	,	,	
		npts1,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "ktxe_types.h"
#include "COM_usrlib.h"
#include <string.h>
	END USRLIB MODULE INFORMATION
*/
void GETCAPOFFSETARRAY(char * TestName, double *MeasuredCapArray, int npts, double *OffsetArray, int npts1);


/* USRLIB MODULE INFORMATION

	MODULE NAME: GETIRANGE
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 2
	ARGUMENTS:
		ResultName,	char *,	Input,	,	,	
		CurrentRange,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <string.h>
#include "ktxe_types.h"
#include "COM_usrlib.h"
	END USRLIB MODULE INFORMATION
*/
void GETIRANGE(char * ResultName, double * CurrentRange);


/* USRLIB MODULE INFORMATION

	MODULE NAME: gmgds_wl_mos
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 24
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
		intrange,	int,	Input,	,	,	
		mxi,	int,	Input,	,	,	
		vdn,	double,	Input,	,	,	
		cdn,	double,	Input,	,	,	
		cdt,	double,	Input,	,	,	
		vbs,	double,	Input,	,	,	
		mv,	double,	Input,	,	,	
		w_mult,	char *,	Input,	,	,	
		delaytime,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		vgn,	double *,	Output,	,	,	
		gm,	double *,	Output,	,	,	
		gds,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void gmgds_wl_mos(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, int intrange, int mxi, double vdn, double cdn, double cdt, double vbs, double mv, char * w_mult, double delaytime, double lorange, double mrange, double * vgn, double * gm, double * gds);


/* USRLIB MODULE INFORMATION

	MODULE NAME: gndall
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
void  gndall();

/* USRLIB MODULE INFORMATION

	MODULE NAME: GOI_FV
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 25
	ARGUMENTS:
		Top,	int,	Input,	,	,	
		Bot,	int,	Input,	,	,	
		Sub,	int,	Input,	,	,	
		Chuck,	int,	Input,	,	,	
		Vstart,	double,	Input,	,	,	
		Vstop,	double,	Input,	,	,	
		Vstep,	double,	Input,	,	,	
		Hold,	double,	Input,	,	,	
		Delay,	double,	Input,	,	,	
		Hc,	double,	Input,	,	,	
		Mc,	double,	Input,	,	,	
		Lc,	double,	Input,	,	,	
		Hr,	double,	Input,	,	,	
		Mr,	double,	Input,	,	,	
		Lr,	double,	Input,	,	,	
		M_i,	double,	Input,	,	,	
		M_q,	double,	Input,	,	,	
		Intrange,	int,	Input,	,	,	
		Area,	double,	Input,	,	,	
		Per,	double,	Input,	,	,	
		Tty,	char,	Input,	,	,	
		Bty,	char,	Input,	,	,	
		Vfail,	double *,	Output,	,	,	
		Ifail,	double *,	Output,	,	,	
		Qbd,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <stdlib.h>

	END USRLIB MODULE INFORMATION
*/
void GOI_FV(int Top, int Bot, int Sub, int Chuck, double Vstart, double Vstop, double Vstep, double Hold, double Delay, double Hc, double Mc, double Lc, double Hr, double Mr, double Lr, double M_i, double M_q, int Intrange, double Area, double Per, char Tty, char Bty, double * Vfail, double * Ifail, double * Qbd);


/* USRLIB MODULE INFORMATION

	MODULE NAME: goi_i_cap
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 25
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
void goi_i_cap(char * devname, int hi, int lo, int sub, int chuckcon, double area, double per, char tty, char bty, double imin, double imax, double imult, double hold, double stepdelay, double vmax, double ratio, double m_i, double m_q, double intrange, double mrange, double lorange, double * vlf, double * vfail, double * ifail, double * qbd);


/* USRLIB MODULE INFORMATION

	MODULE NAME: goi_v_cap
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 20
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
		wox,	double,	Input,	,	,	
		jcrit,	double,	Input,	,	,	
		check,	int,	Input,	,	,	
		debug2,	int,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		vct,	double *,	Output,	,	,	
		vsf,	double *,	Output,	,	,	
		vf,	double *,	Output,	,	,	
		jf,	double *,	Output,	,	,	
		fc,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void goi_v_cap(char * devname, int top, int bot, int sub, int chuckcon, double area, double per, char type, char btype, double wox, double jcrit, int check, int debug2, double mrange, double lorange, double * vct, double * vsf, double * vf, double * jf, double * fc);


/* USRLIB MODULE INFORMATION

	MODULE NAME: goi_v_cap_new
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 28
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
		wox,	double,	Input,	,	,	
		jcrit,	double,	Input,	,	,	
		check,	int,	Input,	,	,	
		debug2,	int,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		vct,	double *,	Output,	,	,	
		vsf,	double *,	Output,	,	,	
		vf,	double *,	Output,	,	,	
		jf,	double *,	Output,	,	,	
		fc,	double *,	Output,	,	,	
		npt,	D_ARRAY_T,	Output,	,	,	
		a,	int,	Input,	500,	,	
		vfr,	D_ARRAY_T,	Output,	,	,	
		a2,	int,	Input,	500,	,	
		im,	D_ARRAY_T,	Output,	,	,	
		a3,	int,	Input,	500,	,	
		tstep,	D_ARRAY_T,	Output,	,	,	
		a4,	int,	Input,	500,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void goi_v_cap_new(char * devname, int top, int bot, int sub, int chuckcon, double area, double per, char type, char btype, double wox, double jcrit, int check, int debug2, double mrange, double lorange, double * vct, double * vsf, double * vf, double * jf, double * fc, double *npt, int a, double *vfr, int a2, double *im, int a3, double *tstep, int a4);


/* USRLIB MODULE INFORMATION

	MODULE NAME: goi_v_cap_old
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 28
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
		wox,	double,	Input,	,	,	
		jcrit,	double,	Input,	,	,	
		check,	int,	Input,	,	,	
		debug2,	int,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		vct,	double *,	Output,	,	,	
		vsf,	double *,	Output,	,	,	
		vf,	double *,	Output,	,	,	
		jf,	double *,	Output,	,	,	
		fc,	double *,	Output,	,	,	
		npt,	D_ARRAY_T,	Output,	,	,	
		a,	int,	Input,	500,	,	
		vfr,	D_ARRAY_T,	Output,	,	,	
		a2,	int,	Input,	500,	,	
		im,	D_ARRAY_T,	Output,	,	,	
		a3,	int,	Input,	500,	,	
		tstep,	D_ARRAY_T,	Output,	,	,	
		a4,	int,	Input,	500,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void goi_v_cap_old(char * devname, int top, int bot, int sub, int chuckcon, double area, double per, char type, char btype, double wox, double jcrit, int check, int debug2, double mrange, double lorange, double * vct, double * vsf, double * vf, double * jf, double * fc, double *npt, int a, double *vfr, int a2, double *im, int a3, double *tstep, int a4);


/* USRLIB MODULE INFORMATION

	MODULE NAME: goi_v_cap_raw
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 20
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
		wox,	double,	Input,	,	,	
		jcrit,	double,	Input,	,	,	
		check,	int,	Input,	,	,	
		debug2,	int,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		vct,	double *,	Output,	,	,	
		vsf,	double *,	Output,	,	,	
		vf,	double *,	Output,	,	,	
		jf,	double *,	Output,	,	,	
		fc,	double *,	Output,	,	,	
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
void goi_v_cap_raw(char * devname, int top, int bot, int sub, int chuckcon, double area, double per, char type, char btype, double wox, double jcrit, int check, int debug2, double mrange, double lorange, double * vct, double * vsf, double * vf, double * jf, double * fc);


/* USRLIB MODULE INFORMATION

	MODULE NAME: goi_v_cap_ttr
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 20
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
		wox,	double,	Input,	,	,	
		jcrit,	double,	Input,	,	,	
		check,	int,	Input,	,	,	
		debug2,	int,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		vct,	double *,	Output,	,	,	
		vsf,	double *,	Output,	,	,	
		vf,	double *,	Output,	,	,	
		jf,	double *,	Output,	,	,	
		fc,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void goi_v_cap_ttr(char * devname, int top, int bot, int sub, int chuckcon, double area, double per, char type, char btype, double wox, double jcrit, int check, int debug2, double mrange, double lorange, double * vct, double * vsf, double * vf, double * jf, double * fc);


/* USRLIB MODULE INFORMATION

	MODULE NAME: goi_v_cap_ttr_org
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 20
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
		wox,	double,	Input,	,	,	
		jcrit,	double,	Input,	,	,	
		check,	int,	Input,	,	,	
		debug2,	int,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		vct,	double *,	Output,	,	,	
		vsf,	double *,	Output,	,	,	
		vf,	double *,	Output,	,	,	
		jf,	double *,	Output,	,	,	
		fc,	double *,	Output,	,	,	
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
void goi_v_cap_ttr_org(char * devname, int top, int bot, int sub, int chuckcon, double area, double per, char type, char btype, double wox, double jcrit, int check, int debug2, double mrange, double lorange, double * vct, double * vsf, double * vf, double * jf, double * fc);


/* USRLIB MODULE INFORMATION

	MODULE NAME: GOI_V_Sweep
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 10
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		tty,	char,	Input,	,	,	
		bty,	char,	Input,	,	,	
		vmin,	double,	Input,	,	,	
		vmax,	double,	Input,	,	,	
		stepv,	double,	Input,	,	,	
		i_meas,	F_ARRAY_T,	Output,	,	,	
		i_m_size,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#include <time.h>
#include <COM_usrlib.h>
                            
	END USRLIB MODULE INFORMATION
*/
void GOI_V_Sweep(int hi, int lo, int sub, char tty, char bty, double vmin, double vmax, double stepv, float *i_meas, int i_m_size);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Goigox_cap
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 26
	ARGUMENTS:
		Vstart,	double,	Input,	,	,	
		Vstop,	double,	Input,	,	,	
		Vstep,	double,	Input,	,	,	
		Hold,	double,	Input,	,	,	
		Delay,	double,	Input,	,	,	
		Hc,	double,	Input,	,	,	
		Mc,	double,	Input,	,	,	
		Lc,	double,	Input,	,	,	
		Hr,	double,	Input,	,	,	
		Mr,	double,	Input,	,	,	
		Lr,	double,	Input,	,	,	
		M_i,	double,	Input,	,	,	
		M_q,	double,	Input,	,	,	
		Intrange,	int,	Input,	,	,	
		Top,	int,	Input,	,	,	
		Bot,	int,	Input,	,	,	
		Sub,	int,	Input,	,	,	
		Chuck,	int,	Input,	,	,	
		Area,	double,	Input,	,	,	
		Per,	double,	Input,	,	,	
		Tty,	char,	Input,	,	,	
		Bty,	char,	Input,	,	,	
		Vlf,	double *,	Output,	,	,	
		Vfail,	double *,	Output,	,	,	
		Ifail,	double *,	Output,	,	,	
		Qbd,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <stdlib.h>

	END USRLIB MODULE INFORMATION
*/
void Goigox_cap(double Vstart, double Vstop, double Vstep, double Hold, double Delay, double Hc, double Mc, double Lc, double Hr, double Mr, double Lr, double M_i, double M_q, int Intrange, int Top, int Bot, int Sub, int Chuck, double Area, double Per, char Tty, char Bty, double * Vlf, double * Vfail, double * Ifail, double * Qbd);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Goigox_cap_swp
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 26
	ARGUMENTS:
		Vstart,	double,	Input,	,	,	
		Vstop,	double,	Input,	,	,	
		Vstep,	double,	Input,	,	,	
		Hold,	double,	Input,	,	,	
		Delay,	double,	Input,	,	,	
		Hc,	double,	Input,	,	,	
		Mc,	double,	Input,	,	,	
		Lc,	double,	Input,	,	,	
		Hr,	double,	Input,	,	,	
		Mr,	double,	Input,	,	,	
		Lr,	double,	Input,	,	,	
		M_i,	double,	Input,	,	,	
		M_q,	double,	Input,	,	,	
		Intrange,	int,	Input,	,	,	
		Top,	int,	Input,	,	,	
		Bot,	int,	Input,	,	,	
		Sub,	int,	Input,	,	,	
		Chuck,	int,	Input,	,	,	
		Area,	double,	Input,	,	,	
		Per,	double,	Input,	,	,	
		Tty,	char,	Input,	,	,	
		Bty,	char,	Input,	,	,	
		Vlf,	double *,	Output,	,	,	
		Vfail,	double *,	Output,	,	,	
		Ifail,	double *,	Output,	,	,	
		Qbd,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <stdlib.h>

#include <par_util.h>

#include <ktxe_proto.h>
#include <ksox_def.h>
#include <kui_proto.h>

#define MAX(a,b)	((a)>(b) ? (a) : (b))
#define CDELAY  125.E-12
#define ILEAK   20.E-12 


	END USRLIB MODULE INFORMATION
*/
void Goigox_cap_swp(double Vstart, double Vstop, double Vstep, double Hold, double Delay, double Hc, double Mc, double Lc, double Hr, double Mr, double Lr, double M_i, double M_q, int Intrange, int Top, int Bot, int Sub, int Chuck, double Area, double Per, char Tty, char Bty, double * Vlf, double * Vfail, double * Ifail, double * Qbd);


/* USRLIB MODULE INFORMATION

	MODULE NAME: gsweep_mos
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 35
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
		Vgmin,	double,	Input,	,	,	
		Vgmax,	double,	Input,	,	,	
		Vgstp,	double,	Input,	,	,	
		Icmp,	double,	Input,	,	,	
		Vb,	double,	Input,	,	,	
		Vd,	double,	Input,	,	,	
		Vs,	double,	Input,	,	,	
		Dly,	double,	Input,	,	,	
		ddebug,	int,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		Flg,	double *,	Output,	,	,	
		npt,	D_ARRAY_T,	Output,	,	,	
		ary_size,	int,	Input,	100,	,	
		Ig,	D_ARRAY_T,	Output,	,	,	
		asize2,	int,	Input,	100,	,	
		Id,	D_ARRAY_T,	Output,	,	,	
		asize3,	int,	Input,	100,	,	
		Is,	D_ARRAY_T,	Output,	,	,	
		asize4,	int,	Input,	100,	,	
		Ib,	D_ARRAY_T,	Output,	,	,	
		asize5,	int,	Input,	100,	,	
		Vg,	D_ARRAY_T,	Output,	,	,	
		asize6,	int,	Input,	100,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void gsweep_mos(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double Vgmin, double Vgmax, double Vgstp, double Icmp, double Vb, double Vd, double Vs, double Dly, int ddebug, double intrange, double mrange, double lorange, double * Flg, double *npt, int ary_size, double *Ig, int asize2, double *Id, int asize3, double *Is, int asize4, double *Ib, int asize5, double *Vg, int asize6);


/* USRLIB MODULE INFORMATION

	MODULE NAME: hfe_bip
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 21
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		c,	int,	Input,	,	,	
		b,	int,	Input,	,	,	
		e,	int,	Input,	,	,	
		bulk,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		type,	char,	Input,	,	,	
		area,	double,	Input,	,	,	
		vce,	double,	Input,	,	,	
		vbelim,	double,	Input,	,	,	
		iclim,	double,	Input,	,	,	
		ictarget,	double,	Input,	,	,	
		ibstart,	double,	Input,	,	,	
		ibstop,	double,	Input,	,	,	
		ibstep,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		vbulk,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		hfe,	double *,	Output,	,	,	
		vbe,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void hfe_bip(char * devname, int c, int b, int e, int bulk, int sub, char type, double area, double vce, double vbelim, double iclim, double ictarget, double ibstart, double ibstop, double ibstep, double intrange, double vbulk, double mrange, double lorange, double * hfe, double * vbe);


/* USRLIB MODULE INFORMATION

	MODULE NAME: hfe_bip_j
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 18
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		Col,	int,	Input,	,	,	
		Base,	int,	Input,	,	,	
		Emit,	int,	Input,	,	,	
		Bulk,	int,	Input,	,	,	
		Sub,	int,	Input,	,	,	
		Type,	char,	Input,	,	,	
		Area,	double,	Input,	,	,	
		vcb,	double,	Input,	1,	,	
		iclim,	double,	Input,	0.01,	,	
		jetarget,	double,	Input,	0.001,	,	
		intrange,	double,	Input,	0,	,	
		vbulk,	double,	Input,	0,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		hfe,	double *,	Output,	,	,	
		vbe,	double *,	Output,	,	,	
		dvbe,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void hfe_bip_j(char * devname, int Col, int Base, int Emit, int Bulk, int Sub, char Type, double Area, double vcb, double iclim, double jetarget, double intrange, double vbulk, double mrange, double lorange, double * hfe, double * vbe, double * dvbe);


/* USRLIB MODULE INFORMATION

	MODULE NAME: i1_spot_mos
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
		vgs,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vbs,	double,	Input,	,	,	
		term,	char,	Input,	,	,	
		ilim,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		debug2,	int,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		result,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void i1_spot_mos(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vgs, double vds, double vbs, char term, double ilim, double intrange, int debug2, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: i2_spot_mos
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 25
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
		vgs1,	double,	Input,	,	,	
		vds1,	double,	Input,	,	,	
		vbs1,	double,	Input,	,	,	
		term1,	char,	Input,	,	,	
		vgs2,	double,	Input,	,	,	
		vds2,	double,	Input,	,	,	
		vbs2,	double,	Input,	,	,	
		term2,	char,	Input,	,	,	
		ilim,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		debug2,	int,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		result1,	double *,	Output,	,	,	
		result2,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void i2_spot_mos(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vgs1, double vds1, double vbs1, char term1, double vgs2, double vds2, double vbs2, char term2, double ilim, double intrange, int debug2, double mrange, double lorange, double * result1, double * result2);


/* USRLIB MODULE INFORMATION

	MODULE NAME: i_all_mos
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
		vgs,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vbs,	double,	Input,	,	,	
		ilim,	double,	Input,	,	,	
		delaytime,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		debug2,	int,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		id,	double *,	Output,	,	,	
		ig,	double *,	Output,	,	,	
		is,	double *,	Output,	,	,	
		ib,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void i_all_mos(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vgs, double vds, double vbs, double ilim, double delaytime, double intrange, int debug2, double mrange, double lorange, double * id, double * ig, double * is, double * ib);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ibridge_dd_gangi_rs2
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 39
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		f1,	int,	Input,	,	,	
		f2,	int,	Input,	,	,	
		f3,	int,	Input,	,	,	
		f4,	int,	Input,	,	,	
		f5,	int,	Input,	,	,	
		f6,	int,	Input,	,	,	
		f7,	int,	Input,	,	,	
		s1,	int,	Input,	,	,	
		s2,	int,	Input,	,	,	
		s3,	int,	Input,	,	,	
		s4,	int,	Input,	,	,	
		s5,	int,	Input,	,	,	
		s6,	int,	Input,	,	,	
		s7,	int,	Input,	,	,	
		s8,	int,	Input,	,	,	
		s9,	int,	Input,	,	,	
		s10,	int,	Input,	,	,	
		s11,	int,	Input,	,	,	
		s12,	int,	Input,	,	,	
		topin,	int,	Input,	,	,	
		width,	double,	Input,	,	,	
		length,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
		vbridge,	double,	Input,	,	,	
		icomp,	double,	Input,	,	,	
		vsub,	double,	Input,	,	,	
		delaytime,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		normalize,	char,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		topo,	double *,	Output,	,	,	
		ib1,	double *,	Output,	,	,	
		ib2,	double *,	Output,	,	,	
		ib3,	double *,	Output,	,	,	
		ib4,	double *,	Output,	,	,	
		ib5,	double *,	Output,	,	,	
		ib6,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void ibridge_dd_gangi_rs2(char * devname, int f1, int f2, int f3, int f4, int f5, int f6, int f7, int s1, int s2, int s3, int s4, int s5, int s6, int s7, int s8, int s9, int s10, int s11, int s12, int topin, double width, double length, char type, double vbridge, double icomp, double vsub, double delaytime, double intrange, char normalize, double mrange, double lorange, double * topo, double * ib1, double * ib2, double * ib3, double * ib4, double * ib5, double * ib6);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ibridge_dd_rs2
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 18
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		f1,	int,	Input,	,	,	
		s1,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		bulk,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		width,	double,	Input,	,	,	
		length,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
		vbridge,	double,	Input,	,	,	
		icomp,	double,	Input,	,	,	
		vsub,	double,	Input,	,	,	
		delaytime,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		normalize,	char,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		ib,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void ibridge_dd_rs2(char * devname, int f1, int s1, int sub, int bulk, int chuckcon, double width, double length, char type, double vbridge, double icomp, double vsub, double delaytime, double intrange, char normalize, double mrange, double lorange, double * ib);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ibridge_rs2
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 18
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		f1,	int,	Input,	,	,	
		s1,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		bulk,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		width,	double,	Input,	,	,	
		length,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
		vbridge,	double,	Input,	,	,	
		icomp,	double,	Input,	,	,	
		vsub,	double,	Input,	,	,	
		delaytime,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		normalize,	char,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		ib,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void ibridge_rs2(char * devname, int f1, int s1, int sub, int bulk, int chuckcon, double width, double length, char type, double vbridge, double icomp, double vsub, double delaytime, double intrange, char normalize, double mrange, double lorange, double * ib);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ibridge_rs4
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 25
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		forcehi,	int,	Input,	,	,	
		forcelo,	int,	Input,	,	,	
		meashi,	int,	Input,	,	,	
		measlo,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		bulk,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		width,	double,	Input,	,	,	
		length,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
		vbridge,	double,	Input,	,	,	
		icomp,	double,	Input,	,	,	
		isub_hi,	double,	Input,	,	,	
		isub_lo,	double,	Input,	,	,	
		brdg_lim,	double,	Input,	,	,	
		cont_lim,	double,	Input,	,	,	
		vlimit,	double,	Input,	,	,	
		vsub,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		debug2,	double,	Input,	,	,	
		sub_leak,	int,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		result,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void ibridge_rs4(char * devname, int forcehi, int forcelo, int meashi, int measlo, int sub, int bulk, int chuckcon, double width, double length, char type, double vbridge, double icomp, double isub_hi, double isub_lo, double brdg_lim, double cont_lim, double vlimit, double vsub, double intrange, double debug2, int sub_leak, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ibridge_viachn
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 11
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		chain_A,	int,	Input,	,	,	
		chain_B,	int,	Input,	,	,	
		metal1,	int,	Input,	,	,	
		type,	char,	Input,	,	,	
		vbridge,	double,	Input,	,	,	
		icomp,	double,	Input,	,	,	
		brdg_lim,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		debug2,	double,	Input,	,	,	
		result,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void ibridge_viachn(char * devname, int chain_A, int chain_B, int metal1, char type, double vbridge, double icomp, double brdg_lim, double intrange, double debug2, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ID_LOG_N_mos
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
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#include <tidp1.h>
	END USRLIB MODULE INFORMATION
*/
void ID_LOG_N_mos(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vds, double idrange, double vgs, double vbs, double vss, double multiplier, double intrange, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: id_mos
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
		vds,	double,	Input,	0.1,	,	
		idrange,	double,	Input,	1e-09,	,	
		idmax,	double,	Input,	0.001,	,	
		vgs,	double,	Input,	0.1,	,	
		vbs,	double,	Input,	0,	,	
		delay,	double,	Input,	0.01,	,	
		normalize,	char,	Input,	Y,	,	
		intrange,	double,	Input,	1,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		Id,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void id_mos(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vds, double idrange, double idmax, double vgs, double vbs, double delay, char normalize, double intrange, double mrange, double lorange, double * Id);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ID_mos
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 20
	ARGUMENTS:
		devname,	char *,	Input
		gate,	int,	Input
		drain,	int,	Input
		source,	int,	Input
		bulk,	int,	Input
		sub,	int,	Input
		chuckcon,	int,	Input
		width,	double,	Input
		length,	double,	Input
		type,	char,	Input
		vds,	double,	Input
		idrange,	double,	Input
		vgs,	double,	Input
		vbs,	double,	Input
		vss,	double,	Input
		multiplier,	double,	Input
		intrange,	double,	Input
		mrange,	double,	Input
		lorange,	double,	Input
		result,	double *,	Output
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#include <tidp1.h>
	END USRLIB MODULE INFORMATION
*/
void ID_mos(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vds, double idrange, double vgs, double vbs, double vss, double multiplier, double intrange, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: id_mos_bulk
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
		vds,	double,	Input,	0.1,	,	
		idrange,	double,	Input,	1e-09,	,	
		idmax,	double,	Input,	0.001,	,	
		vgs,	double,	Input,	0.1,	,	
		vbs,	double,	Input,	0,	,	
		delay,	double,	Input,	0.01,	,	
		normalize,	char,	Input,	Y,	,	
		intrange,	double,	Input,	1,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		Id,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void id_mos_bulk(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vds, double idrange, double idmax, double vgs, double vbs, double delay, char normalize, double intrange, double mrange, double lorange, double * Id);


/* USRLIB MODULE INFORMATION

	MODULE NAME: id_mos_sec
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
		vds,	double,	Input,	0.1,	,	
		idrange,	double,	Input,	1e-09,	,	
		idmax,	double,	Input,	0.001,	,	
		vgs,	double,	Input,	0.1,	,	
		vbs,	double,	Input,	0,	,	
		delay,	double,	Input,	0.01,	,	
		normalize,	char,	Input,	Y,	,	
		intrange,	double,	Input,	1,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		Id,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void id_mos_sec(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vds, double idrange, double idmax, double vgs, double vbs, double delay, char normalize, double intrange, double mrange, double lorange, double * Id);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Id_mrm_flash
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 19
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		cgate,	int,	Input,	,	,	
		sgate,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		bulk,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		width,	double,	Input,	,	,	
		length,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
		vdsf,	double,	Input,	,	,	
		vcgs,	double,	Input,	,	,	
		vsgs,	double,	Input,	,	,	
		vbsf,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		id,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#include <tidp1.h>
	END USRLIB MODULE INFORMATION
*/
void Id_mrm_flash(char * devname, int cgate, int sgate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vdsf, double vcgs, double vsgs, double vbsf, double intrange, double mrange, double lorange, double * id);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ID_N_mos
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 18
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
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
		vbs,	double,	Input,	,	,	
		vss,	double,	Input,	,	,	
		factor,	double,	Input,	,	,	
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
void ID_N_mos(char * devname, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vds, double idrange, double vbs, double vss, double factor, double intrange, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ID_N_mos1
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
		vds,	double,	Input,	,	,	
		idrange,	double,	Input,	,	,	
		vgs,	double,	Input,	,	,	
		vbs,	double,	Input,	,	,	
		vss,	double,	Input,	,	,	
		factor,	double,	Input,	,	,	
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
void ID_N_mos1(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vds, double idrange, double vgs, double vbs, double vss, double factor, double intrange, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ID_N_mos_4p
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
		result1,	double *,	Output,	,	,	
		result2,	double *,	Output,	,	,	
		result3,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#include <tidp1.h>
	END USRLIB MODULE INFORMATION
*/
void ID_N_mos_4p(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vds, double idrange, double vgs, double vbs, double vss, double multiplier, double intrange, double mrange, double lorange, double * result, double * result1, double * result2, double * result3);


/* USRLIB MODULE INFORMATION

	MODULE NAME: if_2_bv
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 6
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		idtarget,	double,	Input,	,	,	
		vth,	double *,	Output,	,	,	
		debug,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void if_2_bv(int hi, int lo, int sub, double idtarget, double * vth, int debug);


/* USRLIB MODULE INFORMATION

	MODULE NAME: IG_MOS
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
		vds,	double,	Input,	0.1,	,	
		igrange,	double,	Input,	1e-09,	,	
		igmax,	double,	Input,	0.001,	,	
		vgs,	double,	Input,	0.1,	,	
		vbs,	double,	Input,	0,	,	
		delay,	double,	Input,	0.01,	,	
		normalize,	char,	Input,	Y,	,	
		intrange,	double,	Input,	1,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		Ig,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void IG_MOS(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vds, double igrange, double igmax, double vgs, double vbs, double delay, char normalize, double intrange, double mrange, double lorange, double * Ig);


/* USRLIB MODULE INFORMATION

	MODULE NAME: IG_MOS_tsb
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
		vds,	double,	Input,	0.1,	,	
		igrange,	double,	Input,	1e-09,	,	
		igmax,	double,	Input,	0.001,	,	
		vgs,	double,	Input,	0.1,	,	
		vbs,	double,	Input,	0,	,	
		delay,	double,	Input,	0.01,	,	
		normalize,	char,	Input,	Y,	,	
		intrange,	double,	Input,	1,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		Ig,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void IG_MOS_tsb(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vds, double igrange, double igmax, double vgs, double vbs, double delay, char normalize, double intrange, double mrange, double lorange, double * Ig);


/* USRLIB MODULE INFORMATION

	MODULE NAME: iramp1_rs2
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 22
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
		istart,	double,	Input,	0,	-1,	1
		istop,	double,	Input,	0.1,	-1,	1
		istep,	double,	Input,	2e-03,	-1,	1
		vlimit,	double,	Input,	10,	-100,	100
		vsub,	double,	Input,	0,	-100,	100
		delta,	double,	Input,	0.2,	0,	1
		delaytime,	double,	Input,	0.02,	0,	1
		intrange,	double,	Input,	0,	0,	10
		mrange,	double,	Input,	 ,	,	
		lorange,	double,	Input,	 ,	,	
		vbd,	double *,	Output,	 ,	,	
		ibd,	double *,	Output,	 ,	,	
		vbd1,	double *,	Output,	 ,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void iramp1_rs2(char * devname, int hi, int lo, int sub, int bulk, int chuckcon, double width, double length, char type, double istart, double istop, double istep, double vlimit, double vsub, double delta, double delaytime, double intrange, double mrange, double lorange, double * vbd, double * ibd, double * vbd1);


/* USRLIB MODULE INFORMATION

	MODULE NAME: iramp_rs2
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 21
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
		istart,	double,	Input,	0,	-1,	1
		istop,	double,	Input,	0.1,	-1,	1
		istep,	double,	Input,	2e-03,	-1,	1
		vlimit,	double,	Input,	10,	-100,	100
		vsub,	double,	Input,	0,	-100,	100
		delta,	double,	Input,	0.2,	0,	1
		delaytime,	double,	Input,	0.02,	0,	1
		intrange,	double,	Input,	0,	0,	10
		mrange,	double,	Input,	 ,	,	
		lorange,	double,	Input,	 ,	,	
		vbd,	double *,	Output,	 ,	,	
		ibd,	double *,	Output,	 ,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void iramp_rs2(char * devname, int hi, int lo, int sub, int bulk, int chuckcon, double width, double length, char type, double istart, double istop, double istep, double vlimit, double vsub, double delta, double delaytime, double intrange, double mrange, double lorange, double * vbd, double * ibd);


/* USRLIB MODULE INFORMATION

	MODULE NAME: IS_N_mos
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
		vds,	double,	Input,	,	,	
		isrange,	double,	Input,	,	,	
		vgs,	double,	Input,	,	,	
		vbs,	double,	Input,	,	,	
		vss,	double,	Input,	,	,	
		factor,	double,	Input,	,	,	
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
void IS_N_mos(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vds, double isrange, double vgs, double vbs, double vss, double factor, double intrange, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: IS_N_mos_gate_float
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 18
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		drain,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		bulk,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		width,	double,	Input,	,	,	
		length,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
		vds,	double,	Input,	,	,	
		isrange,	double,	Input,	,	,	
		vbs,	double,	Input,	,	,	
		vss,	double,	Input,	,	,	
		factor,	double,	Input,	,	,	
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
void IS_N_mos_gate_float(char * devname, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vds, double isrange, double vbs, double vss, double factor, double intrange, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: IS_N_mos_kj
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
		isrange,	double,	Input,	,	,	
		vgs,	double,	Input,	,	,	
		vbs,	double,	Input,	,	,	
		vss,	double,	Input,	,	,	
		factor,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		result,	double *,	Output,	,	,	
		idrain,	double *,	Output,	,	,	
		igate,	double *,	Output,	,	,	
		isub,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#include <tidp1.h>
	END USRLIB MODULE INFORMATION
*/
void IS_N_mos_kj(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vds, double isrange, double vgs, double vbs, double vss, double factor, double intrange, double mrange, double lorange, double * result, double * idrain, double * igate, double * isub);


/* USRLIB MODULE INFORMATION

	MODULE NAME: IS_N_mos_kj2
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
		isrange,	double,	Input,	,	,	
		vgs,	double,	Input,	,	,	
		vbs,	double,	Input,	,	,	
		vss,	double,	Input,	,	,	
		factor,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		result,	double *,	Output,	,	,	
		idrain,	double *,	Output,	,	,	
		igate,	double *,	Output,	,	,	
		isub,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#include <tidp1.h>
	END USRLIB MODULE INFORMATION
*/
void IS_N_mos_kj2(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vds, double isrange, double vgs, double vbs, double vss, double factor, double intrange, double mrange, double lorange, double * result, double * idrain, double * igate, double * isub);


/* USRLIB MODULE INFORMATION

	MODULE NAME: IS_N_mos_MV15
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
		vds,	double,	Input,	,	,	
		isrange,	double,	Input,	,	,	
		vgs,	double,	Input,	,	,	
		vbs,	double,	Input,	,	,	
		vss,	double,	Input,	,	,	
		factor,	double,	Input,	,	,	
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
void IS_N_mos_MV15(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vds, double isrange, double vgs, double vbs, double vss, double factor, double intrange, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: IS_N_mos_org
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
		vds,	double,	Input,	,	,	
		isrange,	double,	Input,	,	,	
		vgs,	double,	Input,	,	,	
		vbs,	double,	Input,	,	,	
		vss,	double,	Input,	,	,	
		factor,	double,	Input,	,	,	
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
void IS_N_mos_org(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vds, double isrange, double vgs, double vbs, double vss, double factor, double intrange, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ISO_BVDS
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 18
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
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
void ISO_BVDS(char * devname, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double idmax, double vdsmin, double vdsmax, double vbs, double multiplier, double intrange, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: iso_mos
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
		vds,	double,	Input,	0.1,	,	
		isrange,	double,	Input,	1e-09,	,	
		vgs,	double,	Input,	0.1,	,	
		vbs,	double,	Input,	0,	,	
		vss,	double,	Input,	0,	,	
		factor,	double,	Input,	1,	,	
		intrange,	double,	Input,	1,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		Is,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void iso_mos(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vds, double isrange, double vgs, double vbs, double vss, double factor, double intrange, double mrange, double lorange, double * Is);


/* USRLIB MODULE INFORMATION

	MODULE NAME: iso_mos_raw
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
		vds,	double,	Input,	0.1,	,	
		isrange,	double,	Input,	1e-09,	,	
		vgs,	double,	Input,	0.1,	,	
		vbs,	double,	Input,	0,	,	
		vss,	double,	Input,	0,	,	
		factor,	double,	Input,	1,	,	
		intrange,	double,	Input,	1,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		Is,	double *,	Output,	,	,	
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
void iso_mos_raw(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vds, double isrange, double vgs, double vbs, double vss, double factor, double intrange, double mrange, double lorange, double * Is);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ISOBV
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 8
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		vlow,	double,	Input,	,	,	
		vhigh,	double,	Input,	,	,	
		niter,	int,	Input,	,	,	
		type,	char,	Input,	,	,	
		ipgm,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double ISOBV(int hi, int lo, int sub, double vlow, double vhigh, int niter, char type, double ipgm);


/* USRLIB MODULE INFORMATION

	MODULE NAME: isub1pt_mos
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
		vds,	double,	Input,	,	,	
		isubrange,	double,	Input,	,	,	
		vgs,	double,	Input,	,	,	
		vbs,	double,	Input,	,	,	
		vss,	double,	Input,	,	,	
		normalize,	char,	Input,	,	,	
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
	END USRLIB MODULE INFORMATION
*/
void isub1pt_mos(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vds, double isubrange, double vgs, double vbs, double vss, char normalize, double intrange, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ISUB1PTN_MOS
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 20
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		gate,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		bulk,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		chuck,	int,	Input,	,	,	
		width,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
		id_max,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vgs,	double,	Input,	,	,	
		vbs,	double,	Input,	,	,	
		vss,	double,	Input,	,	,	
		multiplier,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		isub_range,	double,	Input,	,	,	
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
void ISUB1PTN_MOS(char * devname, int gate, int drain, int source, int bulk, int sub, int chuck, double width, char type, double id_max, double vds, double vgs, double vbs, double vss, double multiplier, double intrange, double isub_range, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: isub_max_mos
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
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#define MAXPTS 100
	END USRLIB MODULE INFORMATION
*/
void isub_max_mos(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vgstart, double vgstop, double vds, double vbulk, double icomp, double sdelay, double intrange, double mrange, double lorange, double * isubmax, double * vgmax);


/* USRLIB MODULE INFORMATION

	MODULE NAME: isub_max_sweep_mos
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
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#define MAXPTS 100
	END USRLIB MODULE INFORMATION
*/
void isub_max_sweep_mos(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vgstart, double vgstop, double vds, double vbulk, double icomp, double sdelay, double intrange, double mrange, double lorange, double * isubmax, double * vgmax);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ISUB_mos
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 20
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		gate,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		bulk,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		chuck,	int,	Input,	,	,	
		width,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
		id_max,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vgs,	double,	Input,	,	,	
		vbs,	double,	Input,	,	,	
		vss,	double,	Input,	,	,	
		multiplier,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		isub_range,	double,	Input,	,	,	
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
void ISUB_mos(char * devname, int gate, int drain, int source, int bulk, int sub, int chuck, double width, char type, double id_max, double vds, double vgs, double vbs, double vss, double multiplier, double intrange, double isub_range, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: iv_plot_rs2
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 25
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		f1,	int,	Input,	,	,	
		s1,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		bulk,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		width,	double,	Input,	,	,	
		length,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
		FIV,	char,	Input,	,	,	
		ivstart,	double,	Input,	,	,	
		ivstop,	double,	Input,	,	,	
		ivstep,	double,	Input,	,	,	
		ivcomp,	double,	Input,	,	,	
		vsub,	double,	Input,	,	,	
		delaytime,	double,	Input,	,	,	
		intrange,	double,	Input,	1,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		npt,	D_ARRAY_T,	Output,	,	,	
		ary_size,	int,	Input,	100,	,	
		I_ary,	D_ARRAY_T,	Output,	,	,	
		ary_size1,	int,	Input,	100,	,	
		V_ary,	D_ARRAY_T,	Output,	,	,	
		ary_size2,	int,	Input,	100,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void iv_plot_rs2(char * devname, int f1, int s1, int sub, int bulk, int chuckcon, double width, double length, char type, char FIV, double ivstart, double ivstop, double ivstep, double ivcomp, double vsub, double delaytime, double intrange, double mrange, double lorange, double *npt, int ary_size, double *I_ary, int ary_size1, double *V_ary, int ary_size2);


/* USRLIB MODULE INFORMATION

	MODULE NAME: iv_rs2
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 18
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		f1,	int,	Input,	,	,	
		s1,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		bulk,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		width,	double,	Input,	,	,	
		length,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
		vbridge,	double,	Input,	,	,	
		icomp,	double,	Input,	,	,	
		vsub,	double,	Input,	,	,	
		delaytime,	double,	Input,	,	,	
		intrange,	double,	Input,	1,	,	
		normalize,	char,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		ib,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void iv_rs2(char * devname, int f1, int s1, int sub, int bulk, int chuckcon, double width, double length, char type, double vbridge, double icomp, double vsub, double delaytime, double intrange, char normalize, double mrange, double lorange, double * ib);


/* USRLIB MODULE INFORMATION

	MODULE NAME: iv_save_ary
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 9
	ARGUMENTS:
		ary1,	D_ARRAY_T,	Input,	,	,	
		size1,	int,	Input,	,	,	
		ary2,	D_ARRAY_T,	Input,	,	,	
		size2,	int,	Input,	,	,	
		vlf,	double,	Input,	,	,	
		vhf,	double,	Input,	,	,	
		vfail,	double,	Input,	,	,	
		ifail,	double,	Input,	,	,	
		qbd,	double,	Input,	,	,	
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
void iv_save_ary(double *ary1, int size1, double *ary2, int size2, double vlf, double vhf, double vfail, double ifail, double qbd);


/* USRLIB MODULE INFORMATION

	MODULE NAME: JEDEC_4p_cap
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
		istart,	double,	Input,	,	,	
		ihigh1,	double,	Input,	,	,	
		ihigh2,	double,	Input,	,	,	
		ihigh3,	double,	Input,	,	,	
		hold,	double,	Input,	,	,	
		stepdelay,	double,	Input,	,	,	
		vmax,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		vlf,	double *,	Output,	,	,	
		vhf1,	double *,	Output,	,	,	
		vhf3,	double *,	Output,	,	,	
		vhf5,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#define CRTLIM 1.0E-6		                              
#define MAX(a,b)	((a)>(b) ? (a) : (b))
	END USRLIB MODULE INFORMATION
*/
void  JEDEC_4p_cap(char *, int, int, int, int, double, double, char, char, double, double, double, double, double, double, double, double, double, double, double *, double *, double *, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: JEDEC_4p_cap_org170110
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
		istart,	double,	Input,	,	,	
		ihigh1,	double,	Input,	,	,	
		ihigh2,	double,	Input,	,	,	
		ihigh3,	double,	Input,	,	,	
		hold,	double,	Input,	,	,	
		stepdelay,	double,	Input,	,	,	
		vmax,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		vlf,	double *,	Output,	,	,	
		vhf1,	double *,	Output,	,	,	
		vhf3,	double *,	Output,	,	,	
		vhf5,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#define CRTLIM 1.0E-6		                              
#define MAX(a,b)	((a)>(b) ? (a) : (b))
	END USRLIB MODULE INFORMATION
*/
void  JEDEC_4p_cap_org170110(char *, int, int, int, int, double, double, char, char, double, double, double, double, double, double, double, double, double, double, double *, double *, double *, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: JEDEC_I2_cap
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
void JEDEC_I2_cap(char * devname, int hi, int lo, int sub, int chuckcon, double area, double per, char tty, char bty, double imin, double imax, double istop, double imult, double hold, double stepdelay, double vmax, double ratio, double m_i, double m_q, double intrange, double mrange, double lorange, double * vlf, double * vhf, double * vfail, double * ifail, double * qbd);


/* USRLIB MODULE INFORMATION

	MODULE NAME: JEDEC_I2_cap_r1
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
void JEDEC_I2_cap_r1(char * devname, int hi, int lo, int sub, int chuckcon, double area, double per, char tty, char bty, double imin, double imax, double istop, double imult, double hold, double stepdelay, double vmax, double ratio, double m_i, double m_q, double intrange, double mrange, double lorange, double * vlf, double * vhf, double * vfail, double * ifail, double * qbd);


/* USRLIB MODULE INFORMATION

	MODULE NAME: JEDEC_I2_cap_raw
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

#include <stdlib.h>
#include <ktxe_proto.h>
#include <ksox_def.h>
#include <kui_proto.h>

#define MAX(a,b)	((a)>(b) ? (a) : (b))
#define CDELAY  125.E-12
#define ILEAK   20.E-12 
                            
	END USRLIB MODULE INFORMATION
*/
void JEDEC_I2_cap_raw(char * devname, int hi, int lo, int sub, int chuckcon, double area, double per, char tty, char bty, double imin, double imax, double istop, double imult, double hold, double stepdelay, double vmax, double ratio, double m_i, double m_q, double intrange, double mrange, double lorange, double * vlf, double * vhf, double * vfail, double * ifail, double * qbd);


/* USRLIB MODULE INFORMATION

	MODULE NAME: JEDEC_I2_ONO_cap
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
void JEDEC_I2_ONO_cap(char * devname, int hi, int lo, int sub, int chuckcon, double area, double per, char tty, char bty, double imin, double imax, double istop, double imult, double hold, double stepdelay, double vmax, double ratio, double m_i, double m_q, double intrange, double mrange, double lorange, double * vlf, double * vhf, double * vfail, double * ifail, double * qbd);


/* USRLIB MODULE INFORMATION

	MODULE NAME: JEDEC_I2_SAVE_cap
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 27
	ARGUMENTS:
		devname,	char *,	Input,	 ,	 ,	 
		hi,	int,	Input,	 ,	 ,	 
		lo,	int,	Input,	 ,	 ,	 
		sub,	int,	Input,	 ,	 ,	 
		chuckcon,	int,	Input,	 ,	 ,	 
		area,	double,	Input,	 ,	 ,	 
		per,	double,	Input,	 ,	 ,	 
		tty,	char,	Input,	 ,	 ,	 
		bty,	char,	Input,	 ,	 ,	 
		imin,	double,	Input,	 ,	 ,	 
		imax,	double,	Input,	 ,	 ,	 
		istop,	double,	Input,	 ,	 ,	 
		imult,	double,	Input,	 ,	 ,	 
		hold,	double,	Input,	 ,	 ,	 
		stepdelay,	double,	Input,	 ,	 ,	 
		vmax,	double,	Input,	 ,	 ,	 
		ratio,	double,	Input,	 ,	 ,	 
		m_i,	double,	Input,	 ,	 ,	 
		m_q,	double,	Input,	 ,	 ,	 
		intrange,	double,	Input,	 ,	 ,	 
		mrange,	double,	Input,	 ,	 ,	 
		lorange,	double,	Input,	 ,	 ,	 
		vlf,	double *,	Output,	 ,	 ,	 
		vhf,	double *,	Output,	 ,	 ,	 
		vfail,	double *,	Output,	 ,	 ,	 
		ifail,	double *,	Output,	 ,	 ,	 
		qbd,	double *,	Output,	 ,	 ,	 
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#define CRTLIM 1.0E-6		                              
	END USRLIB MODULE INFORMATION
*/
void JEDEC_I2_SAVE_cap(char * devname, int hi, int lo, int sub, int chuckcon, double area, double per, char tty, char bty, double imin, double imax, double istop, double imult, double hold, double stepdelay, double vmax, double ratio, double m_i, double m_q, double intrange, double mrange, double lorange, double * vlf, double * vhf, double * vfail, double * ifail, double * qbd);


/* USRLIB MODULE INFORMATION

	MODULE NAME: JEDEC_I2_SAVEH_cap
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 27
	ARGUMENTS:
		devname,	char *,	Input,	 ,	 ,	 
		hi,	int,	Input,	 ,	 ,	 
		lo,	int,	Input,	 ,	 ,	 
		sub,	int,	Input,	 ,	 ,	 
		chuckcon,	int,	Input,	 ,	 ,	 
		area,	double,	Input,	 ,	 ,	 
		per,	double,	Input,	 ,	 ,	 
		tty,	char,	Input,	 ,	 ,	 
		bty,	char,	Input,	 ,	 ,	 
		imin,	double,	Input,	 ,	 ,	 
		imax,	double,	Input,	 ,	 ,	 
		istop,	double,	Input,	 ,	 ,	 
		imult,	double,	Input,	 ,	 ,	 
		hold,	double,	Input,	 ,	 ,	 
		stepdelay,	double,	Input,	 ,	 ,	 
		vmax,	double,	Input,	 ,	 ,	 
		ratio,	double,	Input,	 ,	 ,	 
		m_i,	double,	Input,	 ,	 ,	 
		m_q,	double,	Input,	 ,	 ,	 
		intrange,	double,	Input,	 ,	 ,	 
		mrange,	double,	Input,	 ,	 ,	 
		lorange,	double,	Input,	 ,	 ,	 
		vlf,	double *,	Output,	 ,	 ,	 
		vhf,	double *,	Output,	 ,	 ,	 
		vfail,	double *,	Output,	 ,	 ,	 
		ifail,	double *,	Output,	 ,	 ,	 
		qbd,	double *,	Output,	 ,	 ,	 
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#define CRTLIM 1.0E-6		                              
	END USRLIB MODULE INFORMATION
*/
void JEDEC_I2_SAVEH_cap(char * devname, int hi, int lo, int sub, int chuckcon, double area, double per, char tty, char bty, double imin, double imax, double istop, double imult, double hold, double stepdelay, double vmax, double ratio, double m_i, double m_q, double intrange, double mrange, double lorange, double * vlf, double * vhf, double * vfail, double * ifail, double * qbd);


/* USRLIB MODULE INFORMATION

	MODULE NAME: JEDEC_ONO_cap
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 17
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		area,	double,	Input,	,	,	
		per,	double,	Input,	,	,	
		tty,	char,	Input,	,	,	
		bty,	char,	Input,	,	,	
		mini,	double,	Input,	,	,	
		maxi,	double,	Input,	,	,	
		istop,	double,	Input,	,	,	
		imult,	double,	Input,	,	,	
		hold,	double,	Input,	,	,	
		stepdelay,	double,	Input,	,	,	
		vmax,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		Vmeas,	D_ARRAY_T,	Output,	,	,	
		Vm_size,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#define CRTLIM 1.0E-6		                              
	END USRLIB MODULE INFORMATION
*/
void JEDEC_ONO_cap(int hi, int lo, int sub, double area, double per, char tty, char bty, double mini, double maxi, double istop, double imult, double hold, double stepdelay, double vmax, double intrange, double *Vmeas, int Vm_size);


/* USRLIB MODULE INFORMATION

	MODULE NAME: JEDEC_V2_BVD
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 13
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		area,	double,	Input,	,	,	
		tty,	char,	Input,	,	,	
		bty,	char,	Input,	,	,	
		vmin,	double,	Input,	,	,	
		vmax,	double,	Input,	,	,	
		ratio,	double,	Input,	,	,	
		stepv,	double,	Input,	,	,	
		vfail,	double *,	Output,	,	,	
		ifail,	double *,	Output,	,	,	
		qbd,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#include <time.h>
#include <COM_usrlib.h>
                            
	END USRLIB MODULE INFORMATION
*/
void JEDEC_V2_BVD(int hi, int lo, int sub, double area, char tty, char bty, double vmin, double vmax, double ratio, double stepv, double * vfail, double * ifail, double * qbd);


/* USRLIB MODULE INFORMATION

	MODULE NAME: latchup_iv_bip
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 35
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		col,	int,	Input,	,	,	
		base,	int,	Input,	,	,	
		emit,	int,	Input,	,	,	
		bulk,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		type,	char,	Input,	,	,	
		area,	float,	Input,	,	,	
		ic_start,	double,	Input,	,	,	
		ic_stop,	double,	Input,	,	,	
		vcomp,	double,	Input,	,	,	
		vb,	double,	Input,	,	,	
		ve,	double,	Input,	,	,	
		dly,	double,	Input,	,	,	
		vbulk,	double,	Input,	,	,	
		icomp,	double,	Input,	,	,	
		steps,	int,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		debug2,	int,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		itrig,	double *,	Output,	,	,	
		vhold,	double *,	Output,	,	,	
		npt,	D_ARRAY_T,	Output,	,	,	
		ary_size,	int,	Input,	500,	,	
		vc,	D_ARRAY_T,	Output,	,	,	
		ary_size2,	int,	Input,	500,	,	
		ic,	D_ARRAY_T,	Output,	,	,	
		ary_size3,	int,	Input,	500,	,	
		ib,	D_ARRAY_T,	Output,	,	,	
		ary_size4,	int,	Input,	500,	,	
		ie,	D_ARRAY_T,	Output,	,	,	
		ary_size5,	int,	Input,	500,	,	
		ik,	D_ARRAY_T,	Output,	,	,	
		ary_size6,	int,	Input,	500,	,	
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
void latchup_iv_bip(char * devname, int col, int base, int emit, int bulk, int sub, char type, float area, double ic_start, double ic_stop, double vcomp, double vb, double ve, double dly, double vbulk, double icomp, int steps, double intrange, int debug2, double mrange, double lorange, double * itrig, double * vhold, double *npt, int ary_size, double *vc, int ary_size2, double *ic, int ary_size3, double *ib, int ary_size4, double *ie, int ary_size5, double *ik, int ary_size6);


/* USRLIB MODULE INFORMATION

	MODULE NAME: LDIGOI
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 18
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
		vmax,	double,	Input,	,	,	
		vstep,	double,	Input,	,	,	
		ith,	double,	Input,	,	,	
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
void LDIGOI(char * devname, int top, int bot, int sub, int chuckcon, double area, double per, char type, char btype, double imax, double vmax, double vstep, double ith, double multiplier, double intrange, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: LEAK2h_dio_log
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 19
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
void LEAK2h_dio_log(char * devname, int top, int bot, int sub, int chuckcon, double area, double per, char type, char btype, double vrev, double ilim, double icomp, double irange, double delaytime, double multiplier, double intrange, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: leak_bs_2_dio
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 20
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
		intrange,	double,	Input,	,	,	
		normalize,	char,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		Ia,	double *,	Output,	,	,	
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
void leak_bs_2_dio(char * devname, int top, int bot, int sub, int chuckcon, double area, double per, char type, char btype, double vrev, double ilim, double icomp, double irange, double delaytime, double intrange, char normalize, double mrange, double lorange, double * Ia, double * Ip);


/* USRLIB MODULE INFORMATION

	MODULE NAME: LEAK_dio
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 19
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
void  LEAK_dio(char *, int, int, int, int, double, double, char, char, double, double, double, double, double, double, double, double, double, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: LEAK_dio_delay_swp
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 19
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
void LEAK_dio_delay_swp(char * devname, int top, int bot, int sub, int chuckcon, double area, double per, char type, char btype, double vrev, double ilim, double icomp, double irange, double delaytime, double multiplier, double intrange, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: LEAK_dio_log
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 19
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
void  LEAK_dio_log(char *, int, int, int, int, double, double, char, char, double, double, double, double, double, double, double, double, double, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: LEAK_dio_log_float
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 19
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
void LEAK_dio_log_float(char * devname, int top, int bot, int sub, int chuckcon, double area, double per, char type, char btype, double vrev, double ilim, double icomp, double irange, double delaytime, double multiplier, double intrange, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: LEAK_dio_log_jm
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 19
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
void LEAK_dio_log_jm(char * devname, int top, int bot, int sub, int chuckcon, double area, double per, char type, char btype, double vrev, double ilim, double icomp, double irange, double delaytime, double multiplier, double intrange, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: LEAK_dio_log_MV15
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 19
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
void LEAK_dio_log_MV15(char * devname, int top, int bot, int sub, int chuckcon, double area, double per, char type, char btype, double vrev, double ilim, double icomp, double irange, double delaytime, double multiplier, double intrange, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: LEAK_dio_swp
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 19
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
void LEAK_dio_swp(char * devname, int top, int bot, int sub, int chuckcon, double area, double per, char type, char btype, double vrev, double ilim, double icomp, double irange, double delaytime, double multiplier, double intrange, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: LEAK_dio_swphc
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 19
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
void LEAK_dio_swphc(char * devname, int top, int bot, int sub, int chuckcon, double area, double per, char type, char btype, double vrev, double ilim, double icomp, double irange, double delaytime, double multiplier, double intrange, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: leak_pA_dio
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 19
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
		intrange,	double,	Input,	,	,	
		normalize,	char,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		Irev,	double *,	Output,	,	,	
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
void leak_pA_dio(char * devname, int top, int bot, int sub, int chuckcon, double area, double per, char type, char btype, double vrev, double ilim, double icomp, double irange, double delaytime, double intrange, char normalize, double mrange, double lorange, double * Irev);


/* USRLIB MODULE INFORMATION

	MODULE NAME: leakage_2_dio
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 20
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
		intrange,	double,	Input,	,	,	
		normalize,	char,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		Irev,	double *,	Output,	,	,	
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
void leakage_2_dio(char * devname, int top, int bot, int sub, int chuckcon, double area, double per, char type, char btype, double vrev, double ilim, double icomp, double irange, double delaytime, double intrange, char normalize, double mrange, double lorange, double * Irev, double * Ip);


/* USRLIB MODULE INFORMATION

	MODULE NAME: leakage_dio
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 19
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
		intrange,	double,	Input,	,	,	
		normalize,	char,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		Irev,	double *,	Output,	,	,	
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
void leakage_dio(char * devname, int top, int bot, int sub, int chuckcon, double area, double per, char type, char btype, double vrev, double ilim, double icomp, double irange, double delaytime, double intrange, char normalize, double mrange, double lorange, double * Irev);


/* USRLIB MODULE INFORMATION

	MODULE NAME: LIN_FIT
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 8
	ARGUMENTS:
		xx1,	double,	Input,	 ,	,	
		xx2,	double,	Input,	,	,	
		xx3,	double,	Input,	,	,	
		yy1,	double,	Input,	 ,	,	
		yy2,	double,	Input,	,	,	
		yy3,	double,	Input,	,	,	
		N_coord,	int,	Input,	,	,	
		LR_TLD,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <stdlib.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void LIN_FIT(double xx1, double xx2, double xx3, double yy1, double yy2, double yy3, int N_coord, double * LR_TLD);


/* USRLIB MODULE INFORMATION

	MODULE NAME: LIN_FIT_dum
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 26
	ARGUMENTS:
		devname,	char *,	Input,	 ,	 ,	 
		x0,	double,	Input,	 ,	 ,	 
		x1,	double,	Input,	 ,	 ,	 
		x2,	double,	Input,	 ,	 ,	 
		x3,	double,	Input,	 ,	 ,	 
		x4,	double,	Input,	 ,	 ,	 
		x5,	double,	Input,	 ,	 ,	 
		x6,	double,	Input,	 ,	 ,	 
		x7,	double,	Input,	 ,	 ,	 
		x8,	double,	Input,	 ,	 ,	 
		y0,	double,	Input,	 ,	 ,	 
		y1,	double,	Input,	 ,	 ,	 
		y2,	double,	Input,	 ,	 ,	 
		y3,	double,	Input,	 ,	 ,	 
		y4,	double,	Input,	 ,	 ,	 
		y5,	double,	Input,	 ,	 ,	 
		y6,	double,	Input,	 ,	 ,	 
		y7,	double,	Input,	 ,	 ,	 
		y8,	double,	Input,	 ,	 ,	 
		points,	int,	Input,	 ,	 ,	 
		dotest,	char,	Input,	 ,	 ,	 
		mrange,	double,	Input,	 ,	 ,	 
		lorange,	double,	Input,	 ,	 ,	 
		CC,	double *,	Output,	 ,	 ,	 
		Y_intercept,	double *,	Output,	 ,	 ,	 
		Slope,	double *,	Output,	 ,	 ,	 
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
# define MAXINPUT   1.0E25
# define MININPUT   1.0E-25
	END USRLIB MODULE INFORMATION
*/
void LIN_FIT_dum(char * devname, double x0, double x1, double x2, double x3, double x4, double x5, double x6, double x7, double x8, double y0, double y1, double y2, double y3, double y4, double y5, double y6, double y7, double y8, int points, char dotest, double mrange, double lorange, double * CC, double * Y_intercept, double * Slope);


/* USRLIB MODULE INFORMATION

	MODULE NAME: lin_fit_dum
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 26
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		x0,	double,	Input,	,	,	
		x1,	double,	Input,	,	,	
		x2,	double,	Input,	,	,	
		x3,	double,	Input,	,	,	
		x4,	double,	Input,	,	,	
		x5,	double,	Input,	,	,	
		x6,	double,	Input,	,	,	
		x7,	double,	Input,	,	,	
		x8,	double,	Input,	,	,	
		y0,	double,	Input,	,	,	
		y1,	double,	Input,	,	,	
		y2,	double,	Input,	,	,	
		y3,	double,	Input,	,	,	
		y4,	double,	Input,	,	,	
		y5,	double,	Input,	,	,	
		y6,	double,	Input,	,	,	
		y7,	double,	Input,	,	,	
		y8,	double,	Input,	,	,	
		points,	int,	Input,	,	,	
		dotest,	char,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		CC,	double *,	Output,	,	,	
		Y_intercept,	double *,	Output,	,	,	
		Slope,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
# define MAXINPUT   1.0E25
# define MININPUT   1.0E-25
	END USRLIB MODULE INFORMATION
*/
void lin_fit_dum(char * devname, double x0, double x1, double x2, double x3, double x4, double x5, double x6, double x7, double x8, double y0, double y1, double y2, double y3, double y4, double y5, double y6, double y7, double y8, int points, char dotest, double mrange, double lorange, double * CC, double * Y_intercept, double * Slope);


/* USRLIB MODULE INFORMATION

	MODULE NAME: linfit_xy_mos
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 36
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		gate,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		bulk,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		chuck,	int,	Input,	,	,	
		width,	double,	Input,	,	,	
		length,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
		x0,	double,	Input,	,	,	
		x1,	double,	Input,	,	,	
		x2,	double,	Input,	,	,	
		x3,	double,	Input,	,	,	
		x4,	double,	Input,	,	,	
		x5,	double,	Input,	,	,	
		x6,	double,	Input,	,	,	
		x7,	double,	Input,	,	,	
		x8,	double,	Input,	,	,	
		y0,	double,	Input,	,	,	
		y1,	double,	Input,	,	,	
		y2,	double,	Input,	,	,	
		y3,	double,	Input,	,	,	
		y4,	double,	Input,	,	,	
		y5,	double,	Input,	,	,	
		y6,	double,	Input,	,	,	
		y7,	double,	Input,	,	,	
		y8,	double,	Input,	,	,	
		points,	int,	Input,	,	,	
		int_xy,	char,	Input,	,	,	
		dotest,	char,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		cc,	double *,	Output,	,	,	
		offset,	double *,	Output,	,	,	
		slope,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
# define MAXINPUT   1.0E25
# define MININPUT   1.0E-25
	END USRLIB MODULE INFORMATION
*/
void linfit_xy_mos(char * devname, int gate, int drain, int source, int bulk, int sub, int chuck, double width, double length, char type, double x0, double x1, double x2, double x3, double x4, double x5, double x6, double x7, double x8, double y0, double y1, double y2, double y3, double y4, double y5, double y6, double y7, double y8, int points, char int_xy, char dotest, double mrange, double lorange, double * cc, double * offset, double * slope);


/* USRLIB MODULE INFORMATION

	MODULE NAME: log_mos
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 16
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		Gate,	int,	Input,	,	,	
		Drain,	int,	Input,	,	,	
		Source,	int,	Input,	,	,	
		Bulk,	int,	Input,	,	,	
		Sub,	int,	Input,	,	,	
		Chuck,	int,	Input,	,	,	
		Width,	double,	Input,	,	,	
		Length,	double,	Input,	,	,	
		Type,	char,	Input,	,	,	
		Input,	double,	Input,	,	,	
		Inh,	double,	Input,	,	,	
		Inl,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		result,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void log_mos(char * devname, int Gate, int Drain, int Source, int Bulk, int Sub, int Chuck, double Width, double Length, char Type, double Input, double Inh, double Inl, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: LOG_N_dio
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 15
	ARGUMENTS:
		devname,	char *,	Input
		Top,	int,	Input
		Bot,	int,	Input
		Sub,	int,	Input
		chuckcon,	int,	Input
		Area,	double,	Input
		Per,	double,	Input
		Type,	char,	Input
		Btype,	char,	Input
		Input,	double,	Input
		Inh,	double,	Input
		Inl,	double,	Input
		mrange,	double,	Input
		lorange,	double,	Input
		result,	double *,	Output
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <tidp1.h>
	END USRLIB MODULE INFORMATION
*/
void LOG_N_dio(char * devname, int Top, int Bot, int Sub, int chuckcon, double Area, double Per, char Type, char Btype, double Input, double Inh, double Inl, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: log_n_mos
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 16
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		Gate,	int,	Input,	,	,	
		Drain,	int,	Input,	,	,	
		Source,	int,	Input,	,	,	
		Bulk,	int,	Input,	,	,	
		Sub,	int,	Input,	,	,	
		Chuck,	int,	Input,	,	,	
		Width,	double,	Input,	,	,	
		Length,	double,	Input,	,	,	
		Type,	char,	Input,	,	,	
		Input,	double,	Input,	,	,	
		Inh,	double,	Input,	,	,	
		Inl,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		result,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void log_n_mos(char * devname, int Gate, int Drain, int Source, int Bulk, int Sub, int Chuck, double Width, double Length, char Type, double Input, double Inh, double Inl, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: LOG_N_mos
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 16
	ARGUMENTS:
		devname,	char *,	Input
		Gate,	int,	Input
		Drain,	int,	Input
		Source,	int,	Input
		Bulk,	int,	Input
		Sub,	int,	Input
		Chuck,	int,	Input
		Width,	double,	Input
		Length,	double,	Input
		Type,	char,	Input
		Input,	double,	Input
		Inh,	double,	Input
		Inl,	double,	Input
		mrange,	double,	Input
		lorange,	double,	Input
		result,	double *,	Output
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void LOG_N_mos(char * devname, int Gate, int Drain, int Source, int Bulk, int Sub, int Chuck, double Width, double Length, char Type, double Input, double Inh, double Inl, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: MET_YIELD_DUM
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 4
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		Ressrp,	double,	Input,	,	,	
		Rescmb,	double,	Input,	,	,	
		Yield,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void MET_YIELD_DUM(char * devname, double Ressrp, double Rescmb, double * Yield);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Metal_ser_ser
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 19
	ARGUMENTS:
		devname,	char *,	Input,	 ,	 ,	 
		forcehi,	int,	Input,	 ,	 ,	 
		forcelo,	int,	Input,	 ,	 ,	 
		meashi,	int,	Input,	 ,	 ,	 
		measlo,	int,	Input,	 ,	 ,	 
		sub,	int,	Input,	 ,	 ,	 
		bulk,	int,	Input,	 ,	 ,	 
		chuckcon,	int,	Input,	 ,	 ,	 
		lenght,	double,	Input,	 ,	 ,	 
		space,	double,	Input,	 ,	 ,	 
		type,	char,	Input,	 ,	 ,	 
		vforce,	double,	Input,	 ,	 ,	 
		ilimit,	double,	Input,	 ,	 ,	 
		vsub,	double,	Input,	 ,	 ,	 
		delaytime,	double,	Input,	 ,	 ,	 
		intrange,	double,	Input,	 ,	 ,	 
		mrange,	double,	Input,	 ,	 ,	 
		lorange,	double,	Input,	 ,	 ,	 
		result,	double *,	Output,	 ,	 ,	 
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#include <tidp1.h>
	END USRLIB MODULE INFORMATION
*/
void Metal_ser_ser(char * devname, int forcehi, int forcelo, int meashi, int measlo, int sub, int bulk, int chuckcon, double lenght, double space, char type, double vforce, double ilimit, double vsub, double delaytime, double intrange, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Metal_ser_ser_swp
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 19
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		forcehi,	int,	Input,	,	,	
		forcelo,	int,	Input,	,	,	
		meashi,	int,	Input,	,	,	
		measlo,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		bulk,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		lenght,	double,	Input,	,	,	
		step,	double,	Input,	,	,	
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

#include <stdlib.h>
#include <ktxe_proto.h>
#include <ksox_def.h>
#define MAX(a,b)	((a)>(b) ? (a) : (b))
#define CDELAY  125.E-12
#define ILEAK   20.E-12 
	END USRLIB MODULE INFORMATION
*/
void Metal_ser_ser_swp(char * devname, int forcehi, int forcelo, int meashi, int measlo, int sub, int bulk, int chuckcon, double lenght, double step, char type, double vforce, double ilimit, double vsub, double delaytime, double intrange, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: nvsram
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 16
	ARGUMENTS:
		vf1,	int,	Input,	,	,	
		vf2,	int,	Input,	,	,	
		vf3,	int,	Input,	,	,	
		vf4,	int,	Input,	,	,	
		vlow1,	int,	Input,	,	,	
		vlow2,	int,	Input,	,	,	
		vlow3,	int,	Input,	,	,	
		fv1,	double,	Input,	,	,	
		fv2,	double,	Input,	,	,	
		fv3,	double,	Input,	,	,	
		fv4,	double,	Input,	 ,	,	
		imeas1,	double *,	Output,	,	,	
		imeas2,	double *,	Output,	,	,	
		imeas3,	double *,	Output,	,	,	
		imeas4,	double *,	Output,	,	,	
		debugflag,	int,	Input,	,	0,	1
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#include <tidp1.h>
	END USRLIB MODULE INFORMATION
*/
void nvsram(int vf1, int vf2, int vf3, int vf4, int vlow1, int vlow2, int vlow3, double fv1, double fv2, double fv3, double fv4, double * imeas1, double * imeas2, double * imeas3, double * imeas4, int debugflag);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ONBVDS_mos
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
void ONBVDS_mos(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double idmax, double vdsmin, double vdsmax, double vgs, double vbs, double multiplier, double intrange, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ono_rup
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 17
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		area,	double,	Input,	,	,	
		per,	double,	Input,	,	,	
		tty,	char,	Input,	,	,	
		bty,	char,	Input,	,	,	
		mini,	double,	Input,	,	,	
		maxi,	double,	Input,	,	,	
		istop,	double,	Input,	,	,	
		imult,	double,	Input,	,	,	
		hold,	double,	Input,	,	,	
		stepdelay,	double,	Input,	,	,	
		vmax,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		Vmeas,	D_ARRAY_T,	Output,	,	,	
		Vm_size,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#define CRTLIM 1.0E-6		                              
	END USRLIB MODULE INFORMATION
*/
void ono_rup(int hi, int lo, int sub, double area, double per, char tty, char bty, double mini, double maxi, double istop, double imult, double hold, double stepdelay, double vmax, double intrange, double *Vmeas, int Vm_size);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PUTIRANGE
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 2
	ARGUMENTS:
		ResultName,	char *,	Input,	 ,	 ,	 
		CurrentRange,	double,	Input,	 ,	 ,	 
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <string.h>
#include "ktxe_types.h"
#include "COM_usrlib.h"
	END USRLIB MODULE INFORMATION
*/
void PUTIRANGE(char * ResultName, double CurrentRange);


/* USRLIB MODULE INFORMATION

	MODULE NAME: qbd_sk_mos
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 31
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
		Jinit,	double,	Input,	,	,	
		Vuse,	double,	Input,	,	,	
		Istep,	double,	Input,	,	,	
		V_flag,	double,	Input,	,	,	
		Vs,	double,	Input,	,	,	
		Vr,	double,	Input,	,	,	
		Ir,	double,	Input,	,	,	
		Tox,	double,	Input,	,	,	
		Emax,	double,	Input,	,	,	
		D1,	double,	Input,	,	,	
		D2,	double,	Input,	,	,	
		Lp,	int,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		debug2,	int,	Input,	,	,	
		mode,	int,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		Q,	double *,	Output,	,	,	
		Vb,	double *,	Output,	,	,	
		Ib,	double *,	Output,	,	,	
		flag,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void qbd_sk_mos(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double Jinit, double Vuse, double Istep, double V_flag, double Vs, double Vr, double Ir, double Tox, double Emax, double D1, double D2, int Lp, double intrange, int debug2, int mode, double mrange, double lorange, double * Q, double * Vb, double * Ib, double * flag);


/* USRLIB MODULE INFORMATION

	MODULE NAME: qbd_sk_mos_di
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 31
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
		Jinit,	double,	Input,	,	,	
		Vuse,	double,	Input,	,	,	
		Istep,	double,	Input,	,	,	
		V_flag,	double,	Input,	,	,	
		Vs,	double,	Input,	,	,	
		Vr,	double,	Input,	,	,	
		Ir,	double,	Input,	,	,	
		Tox,	double,	Input,	,	,	
		Emax,	double,	Input,	,	,	
		D1,	double,	Input,	,	,	
		D2,	double,	Input,	,	,	
		Lp,	int,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		debug2,	int,	Input,	,	,	
		mode,	int,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		Q,	double *,	Output,	,	,	
		Vb,	double *,	Output,	,	,	
		Ib,	double *,	Output,	,	,	
		flag,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void qbd_sk_mos_di(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double Jinit, double Vuse, double Istep, double V_flag, double Vs, double Vr, double Ir, double Tox, double Emax, double D1, double D2, int Lp, double intrange, int debug2, int mode, double mrange, double lorange, double * Q, double * Vb, double * Ib, double * flag);


/* USRLIB MODULE INFORMATION

	MODULE NAME: readi_sra
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 23
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		vdd,	int,	Input,	,	,	
		bit,	int,	Input,	,	,	
		bib,	int,	Input,	,	,	
		wl,	int,	Input,	,	,	
		vss,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		area,	double,	Input,	,	,	
		pass,	double,	Input,	,	,	
		driver,	double,	Input,	,	,	
		load,	double,	Input,	,	,	
		supply,	double,	Input,	,	,	
		vbtln,	double,	Input,	,	,	
		vwl,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		debug2,	int,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		it0,	double *,	Output,	,	,	
		ib0,	double *,	Output,	,	,	
		it1,	double *,	Output,	,	,	
		ib1,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void readi_sra(char * devname, int vdd, int bit, int bib, int wl, int vss, int sub, int chuckcon, double area, double pass, double driver, double load, double supply, double vbtln, double vwl, double intrange, int debug2, double mrange, double lorange, double * it0, double * ib0, double * it1, double * ib1);


/* USRLIB MODULE INFORMATION

	MODULE NAME: RES_2_I_RES
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 14
	ARGUMENTS:
		devname,	char *,	Input,	 ,	 ,	 
		hi,	int,	Input,	 ,	 ,	 
		lo,	int,	Input,	 ,	 ,	 
		sub,	int,	Input,	 ,	 ,	 
		sq,	double,	Input,	 ,	 ,	 
		iforce,	double,	Input,	 ,	 ,	 
		vlimit,	double,	Input,	 ,	 ,	 
		vsub,	double,	Input,	 ,	 ,	 
		delaytime,	double,	Input,	 ,	 ,	 
		intrange,	double,	Input,	 ,	 ,	 
		multiplier,	double,	Input,	 ,	 ,	 
		mrange,	double,	Input,	 ,	 ,	 
		lorange,	double,	Input,	 ,	 ,	 
		R,	double *,	Output,	 ,	 ,	 
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void RES_2_I_RES(char * devname, int hi, int lo, int sub, double sq, double iforce, double vlimit, double vsub, double delaytime, double intrange, double multiplier, double mrange, double lorange, double * R);


/* USRLIB MODULE INFORMATION

	MODULE NAME: RES_2_V_RES
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 14
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		sq,	double,	Input,	,	,	
		vforce,	double,	Input,	,	,	
		ilimit,	double,	Input,	,	,	
		vsub,	double,	Input,	,	,	
		delaytime,	double,	Input,	,	,	
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
void RES_2_V_RES(char * devname, int hi, int lo, int sub, double sq, double vforce, double ilimit, double vsub, double delaytime, double multiplier, double intrange, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: RES_2_V_RES_org140515
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 14
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		sq,	double,	Input,	,	,	
		vforce,	double,	Input,	,	,	
		ilimit,	double,	Input,	,	,	
		vsub,	double,	Input,	,	,	
		delaytime,	double,	Input,	,	,	
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
void RES_2_V_RES_org140515(char * devname, int hi, int lo, int sub, double sq, double vforce, double ilimit, double vsub, double delaytime, double multiplier, double intrange, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: RES_2_V_RES_re1
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 14
	ARGUMENTS:
		devname,	char *,	Input,	"RES",	,	
		hi,	int,	Input,	-1,	,	
		lo,	int,	Input,	-1,	,	
		sub,	int,	Input,	-1,	,	
		sq,	double,	Input,	0,	,	
		vforce,	double,	Input,	0.1,	,	
		ilimit,	double,	Input,	0.05,	,	
		vsub,	double,	Input,	0,	,	
		delaytime,	double,	Input,	0.05,	,	
		multiplier,	double,	Input,	1,	,	
		intrange,	double,	Input,	1,	,	
		mrange,	double,	Input,	1E-8,	,	
		lorange,	double,	Input,	1E-9,	,	
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
void RES_2_V_RES_re1(char * devname, int hi, int lo, int sub, double sq, double vforce, double ilimit, double vsub, double delaytime, double multiplier, double intrange, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: RES_3_I_RES
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 15
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		sense,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		sq,	double,	Input,	,	,	
		iforce,	double,	Input,	,	,	
		vlimit,	double,	Input,	,	,	
		vsub,	double,	Input,	,	,	
		delaytime,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		multiplier,	double,	Input,	,	,	
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
void RES_3_I_RES(char * devname, int hi, int lo, int sense, int sub, double sq, double iforce, double vlimit, double vsub, double delaytime, double intrange, double multiplier, double mrange, double lorange, double * R);


/* USRLIB MODULE INFORMATION

	MODULE NAME: RES_4_I_R_4
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 16
	ARGUMENTS:
		devname,	char *,	Input
		forcehi,	int,	Input
		meashi,	int,	Input
		forcelow,	int,	Input
		measlow,	int,	Input
		sub,	int,	Input
		sq,	double,	Input
		vlimit,	double,	Input
		iforce,	double,	Input
		delaytime,	double,	Input
		intrange,	double,	Input
		vsub,	double,	Input
		multiplier,	double,	Input
		mrange,	double,	Input
		lorange,	double,	Input
		result,	double *,	Output
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#include <tidp1.h>
	END USRLIB MODULE INFORMATION
*/
void RES_4_I_R_4(char * devname, int forcehi, int meashi, int forcelow, int measlow, int sub, double sq, double vlimit, double iforce, double delaytime, double intrange, double vsub, double multiplier, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Res_4t_res
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 18
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
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void Res_4t_res(char * devname, int f1, int f2, int s1, int s2, int sub, int bulk, int chuckcon, double area, char type, double iforce, double vlimit, double vsub, double delaytime, double intrange, double mrange, double lorange, double * Rc);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res_i2
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
		iforce,	double,	Input,	,	,	
		vlimit,	double,	Input,	,	,	
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
void res_i2(char * devname, int hi, int lo, int sub, int bulk, int chuckcon, double width, double length, char type, double iforce, double vlimit, double vsub, double delaytime, double intrange, char normalize, double mrange, double lorange, double * R);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res_i2_hc
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
		iforce,	double,	Input,	,	,	
		vlimit,	double,	Input,	,	,	
		vsub,	double,	Input,	,	,	
		delaytime,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		normalize,	char,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		Vmeas,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void res_i2_hc(char * devname, int hi, int lo, int sub, int bulk, int chuckcon, double width, double length, char type, double iforce, double vlimit, double vsub, double delaytime, double intrange, char normalize, double mrange, double lorange, double * Vmeas);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res_i2_swp
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 20
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
		istart,	double,	Input,	,	,	
		istop,	double,	Input,	,	,	
		step,	int,	Input,	,	,	
		vlimit,	double,	Input,	,	,	
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

#include <stdlib.h>
#include <ktxe_proto.h>
#include <ksox_def.h>
#include <kui_proto.h>

#include <tidp1.h>
#include "ktxe_types.h"
#include "COM_usrlib.h"
#include <cmtr_hp4284.h>
	END USRLIB MODULE INFORMATION
*/
void res_i2_swp(char * devname, int hi, int lo, int sub, int bulk, int chuckcon, double width, double length, char type, double istart, double istop, int step, double vlimit, double vsub, double delaytime, double intrange, char normalize, double mrange, double lorange, double * R);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res_i_rs2
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
		iforce,	double,	Input,	,	,	
		vlimit,	double,	Input,	,	,	
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
void res_i_rs2(char * devname, int hi, int lo, int sub, int bulk, int chuckcon, double width, double length, char type, double iforce, double vlimit, double vsub, double delaytime, double intrange, char normalize, double mrange, double lorange, double * R);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res_r2b
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 19
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		F1,	int,	Input,	,	,	
		S1,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		bulk1,	int,	Input,	,	,	
		bulk2,	int,	Input,	,	,	
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
void res_r2b(char * devname, int F1, int S1, int sub, int bulk1, int bulk2, int chuckcon, double width, double length, char type, double vforce, double ilimit, double vsub, double delaytime, double intrange, char normalize, double mrange, double lorange, double * R);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res_r2f
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 21
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		F1,	int,	Input,	,	,	
		F2,	int,	Input,	,	,	
		S1,	int,	Input,	,	,	
		S2,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		bulk1,	int,	Input,	,	,	
		bulk2,	int,	Input,	,	,	
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
void res_r2f(char * devname, int F1, int F2, int S1, int S2, int sub, int bulk1, int bulk2, int chuckcon, double width, double length, char type, double vforce, double ilimit, double vsub, double delaytime, double intrange, char normalize, double mrange, double lorange, double * R);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res_r4f
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 24
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		F1,	int,	Input,	,	,	
		F2,	int,	Input,	,	,	
		F3,	int,	Input,	,	,	
		F4,	int,	Input,	,	,	
		S1,	int,	Input,	,	,	
		S2,	int,	Input,	,	,	
		S3,	int,	Input,	,	,	
		S4,	int,	Input,	,	,	
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
void res_r4f(char * devname, int F1, int F2, int F3, int F4, int S1, int S2, int S3, int S4, int sub, int bulk, int chuckcon, double width, double length, char type, double vforce, double ilimit, double vsub, double delaytime, double intrange, char normalize, double mrange, double lorange, double * R);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res_v2_swp
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 20
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
		vstart,	double,	Input,	,	,	
		vstop,	double,	Input,	,	,	
		step,	int,	Input,	,	,	
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

#include <stdlib.h>
#include <ktxe_proto.h>
#include <ksox_def.h>
#include <kui_proto.h>

#include <tidp1.h>
#include "ktxe_types.h"
#include "COM_usrlib.h"
	END USRLIB MODULE INFORMATION
*/
void res_v2_swp(char * devname, int hi, int lo, int sub, int bulk, int chuckcon, double width, double length, char type, double vstart, double vstop, int step, double ilimit, double vsub, double delaytime, double intrange, char normalize, double mrange, double lorange, double * R);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res_vf1_is1_rs2
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 19
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		F1,	int,	Input,	,	,	
		S1,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		bulk,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		width,	double,	Input,	,	,	
		length,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
		vforce,	double,	Input,	,	,	
		ilimit,	double,	Input,	,	,	
		vbulk,	double,	Input,	,	,	
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
void res_vf1_is1_rs2(char * devname, int F1, int S1, int sub, int bulk, int chuckcon, double width, double length, char type, double vforce, double ilimit, double vbulk, double vsub, double delaytime, double intrange, char normalize, double mrange, double lorange, double * R);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res_vf1_is1_rs2_dbg
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 19
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		F1,	int,	Input,	,	,	
		S1,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		bulk,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		width,	double,	Input,	,	,	
		length,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
		vforce,	double,	Input,	,	,	
		ilimit,	double,	Input,	,	,	
		vbulk,	double,	Input,	,	,	
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
void res_vf1_is1_rs2_dbg(char * devname, int F1, int S1, int sub, int bulk, int chuckcon, double width, double length, char type, double vforce, double ilimit, double vbulk, double vsub, double delaytime, double intrange, char normalize, double mrange, double lorange, double * R);


/* USRLIB MODULE INFORMATION

	MODULE NAME: resistance_mtl
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 21
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
		vlimit,	double,	Input,	10,	,	
		iforce,	double,	Input,	1e-03,	,	
		delaytime,	double,	Input,	0.01,	,	
		intrange,	double,	Input,	1,	,	
		vsub,	double,	Input,	0,	,	
		multiplier,	double,	Input,	1,	,	
		normalize,	char,	Input,	Y,	,	
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
void resistance_mtl(char * devname, int f1, int f2, int s1, int s2, int sub, int bulk, int chuckcon, double width, double length, char type, double vlimit, double iforce, double delaytime, double intrange, double vsub, double multiplier, char normalize, double mrange, double lorange, double * R);


/* USRLIB MODULE INFORMATION

	MODULE NAME: resistance_mtl_07
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 21
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
		vlimit,	double,	Input,	10,	,	
		iforce,	double,	Input,	1e-03,	,	
		delaytime,	double,	Input,	0.01,	,	
		intrange,	double,	Input,	1,	,	
		vsub,	double,	Input,	0,	,	
		multiplier,	double,	Input,	1,	,	
		normalize,	char,	Input,	Y,	,	
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
void resistance_mtl_07(char * devname, int f1, int f2, int s1, int s2, int sub, int bulk, int chuckcon, double width, double length, char type, double vlimit, double iforce, double delaytime, double intrange, double vsub, double multiplier, char normalize, double mrange, double lorange, double * R);


/* USRLIB MODULE INFORMATION

	MODULE NAME: resistance_rs2
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
void resistance_rs2(char * devname, int hi, int lo, int sub, int bulk, int chuckcon, double width, double length, char type, double vforce, double ilimit, double vsub, double delaytime, double intrange, char normalize, double mrange, double lorange, double * R);


/* USRLIB MODULE INFORMATION

	MODULE NAME: resistance_rs2_icms
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 18
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		F1,	int,	Input,	,	,	
		S1,	int,	Input,	,	,	
		Sub,	int,	Input,	,	,	
		Bulk,	int,	Input,	,	,	
		Chuck,	int,	Input,	,	,	
		Width,	double,	Input,	,	,	
		Length,	double,	Input,	,	,	
		Type,	char,	Input,	,	,	
		vforce,	double,	Input,	5,	,	
		ilimit,	double,	Input,	0.01,	,	
		vsub,	double,	Input,	0,	,	
		delaytime,	double,	Input,	0.01,	,	
		intrange,	double,	Input,	0,	,	
		normalize,	char,	Input,	'Y',	,	
		mrange,	double,	Input,	 ,	,	
		lorange,	double,	Input,	 ,	,	
		R,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void resistance_rs2_icms(char * devname, int F1, int S1, int Sub, int Bulk, int Chuck, double Width, double Length, char Type, double vforce, double ilimit, double vsub, double delaytime, double intrange, char normalize, double mrange, double lorange, double * R);


/* USRLIB MODULE INFORMATION

	MODULE NAME: resistance_rs4
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 21
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
		vlimit,	double,	Input,	10,	,	
		iforce,	double,	Input,	1e-03,	,	
		delaytime,	double,	Input,	0.01,	,	
		intrange,	double,	Input,	1,	,	
		vsub,	double,	Input,	0,	,	
		multiplier,	double,	Input,	1,	,	
		normalize,	char,	Input,	Y,	,	
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
void resistance_rs4(char * devname, int f1, int f2, int s1, int s2, int sub, int bulk, int chuckcon, double width, double length, char type, double vlimit, double iforce, double delaytime, double intrange, double vsub, double multiplier, char normalize, double mrange, double lorange, double * R);


/* USRLIB MODULE INFORMATION

	MODULE NAME: resistance_v_rs4
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 21
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
		vforce,	double,	Input,	10,	,	
		ilimit,	double,	Input,	1e-03,	,	
		delaytime,	double,	Input,	0.01,	,	
		intrange,	double,	Input,	1,	,	
		vsub,	double,	Input,	0,	,	
		multiplier,	double,	Input,	1,	,	
		normalize,	char,	Input,	Y,	,	
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
void resistance_v_rs4(char * devname, int f1, int f2, int s1, int s2, int sub, int bulk, int chuckcon, double width, double length, char type, double vforce, double ilimit, double delaytime, double intrange, double vsub, double multiplier, char normalize, double mrange, double lorange, double * R);


/* USRLIB MODULE INFORMATION

	MODULE NAME: resistance_v_rs4_dbg
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 23
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
		vforce,	double,	Input,	10,	,	
		ilimit,	double,	Input,	1e-03,	,	
		delaytime,	double,	Input,	0.01,	,	
		intrange,	double,	Input,	1,	,	
		vsub,	double,	Input,	0,	,	
		multiplier,	double,	Input,	1,	,	
		normalize,	char,	Input,	Y,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		R,	double *,	Output,	,	,	
		Vm,	double *,	Output,	,	,	
		Im,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void resistance_v_rs4_dbg(char * devname, int f1, int f2, int s1, int s2, int sub, int bulk, int chuckcon, double width, double length, char type, double vforce, double ilimit, double delaytime, double intrange, double vsub, double multiplier, char normalize, double mrange, double lorange, double * R, double * Vm, double * Im);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ro_freq1
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 11
	ARGUMENTS:
		vforce,	double,	Input,	,	,	
		vdd_pin,	int,	Input,	,	,	
		lo_meas_pin,	int,	Input,	,	,	
		hi_meas_pin,	int,	Input,	,	,	
		enable_pin,	int,	Input,	,	,	
		nd24_pin,	int,	Input,	,	,	
		ngate_pin,	int,	Input,	,	,	
		nd12_pin,	int,	Input,	,	,	
		Id,	double *,	Output,	,	,	
		measured_freq,	double *,	Output,	,	,	
		test_status,	int *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <ki776.h>
	END USRLIB MODULE INFORMATION
*/
void ro_freq1(double vforce, int vdd_pin, int lo_meas_pin, int hi_meas_pin, int enable_pin, int nd24_pin, int ngate_pin, int nd12_pin, double * Id, double * measured_freq, int * test_status);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Rsheet_wid
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 19
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		forcehi,	int,	Input,	,	,	
		meashi,	int,	Input,	,	,	
		forcelo,	int,	Input,	,	,	
		measlo,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		bulk,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		length,	double,	Input,	,	,	
		width,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
		iforce,	double,	Input,	,	,	
		vlimit,	double,	Input,	,	,	
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
void Rsheet_wid(char * devname, int forcehi, int meashi, int forcelo, int measlo, int sub, int bulk, int chuckcon, double length, double width, char type, double iforce, double vlimit, double vsub, double delaytime, double intrange, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Rsheet_wid_4trm
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 19
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		forcehi,	int,	Input,	,	,	
		meashi,	int,	Input,	,	,	
		forcelo,	int,	Input,	,	,	
		measlo,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		bulk,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		length,	double,	Input,	,	,	
		width,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
		iforce,	double,	Input,	,	,	
		vlimit,	double,	Input,	,	,	
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
void  Rsheet_wid_4trm(char *, int, int, int, int, int, int, int, double, double, char, double, double, double, double, double, double, double, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: Rsheet_wid_4trm_org
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 19
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		forcehi,	int,	Input,	,	,	
		meashi,	int,	Input,	,	,	
		forcelo,	int,	Input,	,	,	
		measlo,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		bulk,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		length,	double,	Input,	,	,	
		width,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
		iforce,	double,	Input,	,	,	
		vlimit,	double,	Input,	,	,	
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
void  Rsheet_wid_4trm_org(char *, int, int, int, int, int, int, int, double, double, char, double, double, double, double, double, double, double, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: Rsheet_wid_icms
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 19
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		forcehi,	int,	Input,	,	,	
		meashi,	int,	Input,	,	,	
		forcelo,	int,	Input,	,	,	
		measlo,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		bulk,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		length,	double,	Input,	,	,	
		width,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
		iforce,	double,	Input,	,	,	
		vlimit,	double,	Input,	,	,	
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
void Rsheet_wid_icms(char * devname, int forcehi, int meashi, int forcelo, int measlo, int sub, int bulk, int chuckcon, double length, double width, char type, double iforce, double vlimit, double vsub, double delaytime, double intrange, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Rsheet_wid_MV15
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 19
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		forcehi,	int,	Input,	,	,	
		meashi,	int,	Input,	,	,	
		forcelo,	int,	Input,	,	,	
		measlo,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		bulk,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		length,	double,	Input,	,	,	
		width,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
		iforce,	double,	Input,	,	,	
		vlimit,	double,	Input,	,	,	
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
void Rsheet_wid_MV15(char * devname, int forcehi, int meashi, int forcelo, int measlo, int sub, int bulk, int chuckcon, double length, double width, char type, double iforce, double vlimit, double vsub, double delaytime, double intrange, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Rsheet_wid_swp
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 19
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		forcehi,	int,	Input,	,	,	
		meashi,	int,	Input,	,	,	
		forcelo,	int,	Input,	,	,	
		measlo,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		bulk,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		length,	double,	Input,	,	,	
		width,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
		iforce,	double,	Input,	,	,	
		vlimit,	double,	Input,	,	,	
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
void Rsheet_wid_swp(char * devname, int forcehi, int meashi, int forcelo, int measlo, int sub, int bulk, int chuckcon, double length, double width, char type, double iforce, double vlimit, double vsub, double delaytime, double intrange, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: sbv_mos
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
		vsmin,	double,	Input,	,	,	
		vsmax,	double,	Input,	,	,	
		vsstep,	double,	Input,	,	,	
		ilimit,	double,	Input,	,	,	
		vbulk,	double,	Input,	,	,	
		vgate,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		Sbv,	double *,	Output,	,	,	
		Di,	double *,	Output,	,	,	
		Si,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <stdlib.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void sbv_mos(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vsmin, double vsmax, double vsstep, double ilimit, double vbulk, double vgate, double mrange, double lorange, double * Sbv, double * Di, double * Si);


/* USRLIB MODULE INFORMATION

	MODULE NAME: sheet_v_rs4
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
		v,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void sheet_v_rs4(char * devname, int f1, int f2, int s1, int s2, int sub, int bulk, int chuckcon, double width, double length, char type, double iforce, double vlimit, double vsub, double delaytime, double intrange, double mrange, double lorange, double * v);


/* USRLIB MODULE INFORMATION

	MODULE NAME: sheet_vdp_rs4
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
void sheet_vdp_rs4(char * devname, int f1, int f2, int s1, int s2, int sub, int bulk, int chuckcon, double width, double length, char type, double iforce, double vlimit, double vsub, double delaytime, double intrange, double mrange, double lorange, double * Rsheet);


/* USRLIB MODULE INFORMATION

	MODULE NAME: sheet_vdp_rs4_cro
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
void sheet_vdp_rs4_cro(char * devname, int f1, int f2, int s1, int s2, int sub, int bulk, int chuckcon, double width, double length, char type, double iforce, double vlimit, double vsub, double delaytime, double intrange, double mrange, double lorange, double * Rsheet);


/* USRLIB MODULE INFORMATION

	MODULE NAME: sheet_vdp_rs4_new
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
void sheet_vdp_rs4_new(char * devname, int f1, int f2, int s1, int s2, int sub, int bulk, int chuckcon, double width, double length, char type, double iforce, double vlimit, double vsub, double delaytime, double intrange, double mrange, double lorange, double * Rsheet);


/* USRLIB MODULE INFORMATION

	MODULE NAME: sheet_vdp_rs4_offset
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
void sheet_vdp_rs4_offset(char * devname, int f1, int f2, int s1, int s2, int sub, int bulk, int chuckcon, double width, double length, char type, double iforce, double vlimit, double vsub, double delaytime, double intrange, double mrange, double lorange, double * Rsheet);


/* USRLIB MODULE INFORMATION

	MODULE NAME: sonos_bv
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 9
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		vstart,	double,	Input,	,	,	
		vstop,	double,	Input,	,	,	
		step,	double,	Input,	,	,	
		idtarget,	double,	Input,	,	,	
		vth,	double *,	Output,	,	,	
		debug,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void sonos_bv(int hi, int lo, int sub, double vstart, double vstop, double step, double idtarget, double * vth, int debug);


/* USRLIB MODULE INFORMATION

	MODULE NAME: sonos_bv4
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 12
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		W,	double,	Input,	,	,	
		L,	double,	Input,	,	,	
		vstart,	double,	Input,	,	,	
		vstop,	double,	Input,	,	,	
		step,	double,	Input,	,	,	
		idtarget,	double,	Input,	,	,	
		vth,	double *,	Output,	,	,	
		debug,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void sonos_bv4(int drain, int gate, int source, int sub, double W, double L, double vstart, double vstop, double step, double idtarget, double * vth, int debug);


/* USRLIB MODULE INFORMATION

	MODULE NAME: sonos_cap
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
void sonos_cap(char * devname, int hi, int lo, int sub, int chuckcon, double area, double per, char tty, char bty, double vacc, double vinv, double freq, double sig, double stray, int integ, int ddebug, double mrange, double lorange, double * ca, double * ta, double * ci, double * ti);


/* USRLIB MODULE INFORMATION

	MODULE NAME: sonos_iso
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 9
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		width,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vgs,	double,	Input,	,	,	
		vbs,	double,	Input,	,	,	
		iread,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void sonos_iso(int drain, int gate, int source, int sub, double width, double vds, double vgs, double vbs, double * iread);


/* USRLIB MODULE INFORMATION

	MODULE NAME: sonos_vth
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 11
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		vgstart,	double,	Input,	,	,	
		vgstop,	double,	Input,	,	,	
		vgstep,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
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
void sonos_vth(int drain, int gate, int source, int well, double vgstart, double vgstop, double vgstep, double vds, double idtarget, double * vth, int debug_print);


/* USRLIB MODULE INFORMATION

	MODULE NAME: StartCapOffsetDebug
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
void StartCapOffsetDebug();


/* USRLIB MODULE INFORMATION

	MODULE NAME: StopCapOffsetDebug
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
void StopCapOffsetDebug();


/* USRLIB MODULE INFORMATION

	MODULE NAME: sweep_goi_cap
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
void sweep_goi_cap(char * devname, int hi, int lo, int sub, int chuckcon, double area, double per, char tty, char bty, double imin, double imax, double istop, double imult, double hold, double stepdelay, double vmax, double ratio, double m_i, double m_q, double intrange, double mrange, double lorange, double * vlf, double * vhf, double * vfail, double * ifail, double * qbd);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ti_print_time
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
void ti_print_time(char * in_str, double in_data);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ti_set_print_flag
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
void ti_set_print_flag();


/* USRLIB MODULE INFORMATION

	MODULE NAME: TIC05_fetcheck_mos
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
		ids,	double *,	Output,	,	,	
		result,	D_ARRAY_T,	Output,	,	,	
		range_size,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void TIC05_fetcheck_mos(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vdsf, double vgs_off, double vgs_on, double vbsf, double factor, double delay, double intrange, double mrange, double lorange, double * ioff, double * ids, double *result, int range_size);


/* USRLIB MODULE INFORMATION

	MODULE NAME: TriErase_ktest
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 16
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		grcl,	int,	Input,	,	,	
		gse,	int,	Input,	,	,	
		gstr,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vrcl,	double,	Input,	,	,	
		vstr,	double,	Input,	,	,	
		vss,	double,	Input,	,	,	
		vse_delay,	double,	Input,	,	,	
		vse_high,	double,	Input,	,	,	
		vse_width,	double,	Input,	,	,	
		vse_rise,	double,	Input,	,	,	
		vse_fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void TriErase_ktest(int drain, int grcl, int gse, int gstr, int source, int well, double vds, double vrcl, double vstr, double vss, double vse_delay, double vse_high, double vse_width, double vse_rise, double vse_fall, double holdtime);


/* USRLIB MODULE INFORMATION

	MODULE NAME: TriProg_ktest
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 16
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		grcl,	int,	Input,	,	,	
		gse,	int,	Input,	,	,	
		gstr,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vrcl,	double,	Input,	,	,	
		vstr,	double,	Input,	,	,	
		vss,	double,	Input,	,	,	
		vse_delay,	double,	Input,	,	,	
		vse_high,	double,	Input,	,	,	
		vse_width,	double,	Input,	,	,	
		vse_rise,	double,	Input,	,	,	
		vse_fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void TriProg_ktest(int drain, int grcl, int gse, int gstr, int source, int well, double vds, double vrcl, double vstr, double vss, double vse_delay, double vse_high, double vse_width, double vse_rise, double vse_fall, double holdtime);


/* USRLIB MODULE INFORMATION

	MODULE NAME: TriPulse_loop_K
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 27
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		grcl,	int,	Input,	,	,	
		gse,	int,	Input,	,	,	
		gstr,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		loopcount,	int,	Input,	,	,	
		vds_prog,	double,	Input,	,	,	
		vrcl_prog,	double,	Input,	,	,	
		vstr_prog,	double,	Input,	,	,	
		vss_prog,	double,	Input,	,	,	
		prog_delay,	double,	Input,	,	,	
		prog_high,	double,	Input,	,	,	
		prog_width,	double,	Input,	,	,	
		prog_rise,	double,	Input,	,	,	
		prog_fall,	double,	Input,	,	,	
		vds_erase,	double,	Input,	,	,	
		vrcl_erase,	double,	Input,	,	,	
		vstr_erase,	double,	Input,	,	,	
		vss_erase,	double,	Input,	,	,	
		erase_delay,	double,	Input,	,	,	
		erase_high,	double,	Input,	,	,	
		erase_width,	double,	Input,	,	,	
		erase_rise,	double,	Input,	,	,	
		erase_fall,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
		debug_print,	int,	Input,	,	0,	1
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void TriPulse_loop_K(int drain, int grcl, int gse, int gstr, int source, int well, int loopcount, double vds_prog, double vrcl_prog, double vstr_prog, double vss_prog, double prog_delay, double prog_high, double prog_width, double prog_rise, double prog_fall, double vds_erase, double vrcl_erase, double vstr_erase, double vss_erase, double erase_delay, double erase_high, double erase_width, double erase_rise, double erase_fall, double holdtime, int debug_print);


/* USRLIB MODULE INFORMATION

	MODULE NAME: TriVth_celli_ktest
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 16
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		grcl,	int,	Input,	,	,	
		gse,	int,	Input,	,	,	
		gstr,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		idtarget,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vrcl,	double,	Input,	,	,	
		vstr,	double,	Input,	,	,	
		vse_start,	double,	Input,	,	,	
		vse_stop,	double,	Input,	,	,	
		vse_step,	double,	Input,	,	,	
		vth,	double *,	Output,	,	,	
		cell_i,	double *,	Output,	,	,	
		debug_print,	int,	Input,	,	0,	1
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void TriVth_celli_ktest(int drain, int grcl, int gse, int gstr, int source, int well, double idtarget, double vds, double vrcl, double vstr, double vse_start, double vse_stop, double vse_step, double * vth, double * cell_i, int debug_print);


/* USRLIB MODULE INFORMATION

	MODULE NAME: TriVth_ktest
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 15
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		grcl,	int,	Input,	,	,	
		gse,	int,	Input,	,	,	
		gstr,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		idtarget,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vrcl,	double,	Input,	,	,	
		vstr,	double,	Input,	,	,	
		vse_start,	double,	Input,	,	,	
		vse_stop,	double,	Input,	,	,	
		vse_step,	double,	Input,	,	,	
		vth,	double *,	Output,	,	,	
		debug_print,	int,	Input,	,	0,	1
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void TriVth_ktest(int drain, int grcl, int gse, int gstr, int source, int well, double idtarget, double vds, double vrcl, double vstr, double vse_start, double vse_stop, double vse_step, double * vth, int debug_print);


/* USRLIB MODULE INFORMATION

	MODULE NAME: UUGOI
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 26
	ARGUMENTS:
		Vstart,	double,	Input,	,	,	
		Vstop,	double,	Input,	,	,	
		Vstep,	double,	Input,	,	,	
		Hold,	double,	Input,	,	,	
		Delay,	double,	Input,	,	,	
		Hc,	double,	Input,	,	,	
		Mc,	double,	Input,	,	,	
		Lc,	double,	Input,	,	,	
		Hr,	double,	Input,	,	,	
		Mr,	double,	Input,	,	,	
		Lr,	double,	Input,	,	,	
		M_i,	double,	Input,	,	,	
		M_q,	double,	Input,	,	,	
		Intrange,	int,	Input,	,	,	
		Top,	int,	Input,	,	,	
		Bot,	int,	Input,	,	,	
		Sub,	int,	Input,	,	,	
		Chuck,	int,	Input,	,	,	
		Area,	double,	Input,	,	,	
		Per,	double,	Input,	,	,	
		Tty,	char,	Input,	,	,	
		Bty,	char,	Input,	,	,	
		Vlf,	double *,	Output,	,	,	
		Vfail,	double *,	Output,	,	,	
		Ifail,	double *,	Output,	,	,	
		Qbd,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <stdlib.h>

	END USRLIB MODULE INFORMATION
*/
void UUGOI(double Vstart, double Vstop, double Vstep, double Hold, double Delay, double Hc, double Mc, double Lc, double Hr, double Mr, double Lr, double M_i, double M_q, int Intrange, int Top, int Bot, int Sub, int Chuck, double Area, double Per, char Tty, char Bty, double * Vlf, double * Vfail, double * Ifail, double * Qbd);


/* USRLIB MODULE INFORMATION

	MODULE NAME: UUGOI_vlf
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 26
	ARGUMENTS:
		Vstart,	double,	Input,	,	,	
		Vstop,	double,	Input,	,	,	
		Vstep,	double,	Input,	,	,	
		Hold,	double,	Input,	,	,	
		Delay,	double,	Input,	,	,	
		Hc,	double,	Input,	,	,	
		Mc,	double,	Input,	,	,	
		Lc,	double,	Input,	,	,	
		Hr,	double,	Input,	,	,	
		Mr,	double,	Input,	,	,	
		Lr,	double,	Input,	,	,	
		M_i,	double,	Input,	,	,	
		M_q,	double,	Input,	,	,	
		Intrange,	int,	Input,	,	,	
		Top,	int,	Input,	,	,	
		Bot,	int,	Input,	,	,	
		Sub,	int,	Input,	,	,	
		Chuck,	int,	Input,	,	,	
		Area,	double,	Input,	,	,	
		Per,	double,	Input,	,	,	
		Tty,	char,	Input,	,	,	
		Bty,	char,	Input,	,	,	
		Vlf,	double *,	Output,	,	,	
		Vfail,	double *,	Output,	,	,	
		Ifail,	double *,	Output,	,	,	
		Qbd,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <stdlib.h>

	END USRLIB MODULE INFORMATION
*/
void UUGOI_vlf(double Vstart, double Vstop, double Vstep, double Hold, double Delay, double Hc, double Mc, double Lc, double Hr, double Mr, double Lr, double M_i, double M_q, int Intrange, int Top, int Bot, int Sub, int Chuck, double Area, double Per, char Tty, char Bty, double * Vlf, double * Vfail, double * Ifail, double * Qbd);


/* USRLIB MODULE INFORMATION

	MODULE NAME: vbridge_ilo_rs2
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
		mrange,	double,	Input,	 ,	,	
		lorange,	double,	Input,	 ,	,	
		V,	double *,	Output,	 ,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void vbridge_ilo_rs2(char * devname, int hi, int lo, int sub, int bulk, int chuckcon, double width, double length, char type, double iforce, double vlimit, double vsub, double delaytime, double intrange, double mrange, double lorange, double * V);


/* USRLIB MODULE INFORMATION

	MODULE NAME: vbridge_rs2
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
		mrange,	double,	Input,	 ,	,	
		lorange,	double,	Input,	 ,	,	
		V,	double *,	Output,	 ,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void vbridge_rs2(char * devname, int hi, int lo, int sub, int bulk, int chuckcon, double width, double length, char type, double iforce, double vlimit, double vsub, double delaytime, double intrange, double mrange, double lorange, double * V);


/* USRLIB MODULE INFORMATION

	MODULE NAME: vt_field2_mos
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
		vmin,	double,	Input,	,	,	
		vmax,	double,	Input,	,	,	
		issearch,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vsub,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		vtf,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#define MAX(a,b)	((a)>(b) ? (a) : (b))
#define CDELAY  125.E-12
#define ILEAK   20.E-12 
	END USRLIB MODULE INFORMATION
*/
void vt_field2_mos(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vmin, double vmax, double issearch, double vds, double vsub, double intrange, double mrange, double lorange, double * vtf);


/* USRLIB MODULE INFORMATION

	MODULE NAME: vt_field_mos
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
		vmin,	double,	Input,	,	,	
		vmax,	double,	Input,	,	,	
		issearch,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vsub,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		vtf,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#define MAX(a,b)	((a)>(b) ? (a) : (b))
#define CDELAY  125.E-12
#define ILEAK   20.E-12 
	END USRLIB MODULE INFORMATION
*/
void vt_field_mos(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vmin, double vmax, double issearch, double vds, double vsub, double intrange, double mrange, double lorange, double * vtf);


/* USRLIB MODULE INFORMATION

	MODULE NAME: vt_field_mos_bk
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
		vmin,	double,	Input,	,	,	
		vmax,	double,	Input,	,	,	
		issearch,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vsub,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		vtf,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#define MAX(a,b)	((a)>(b) ? (a) : (b))
#define CDELAY  125.E-12
#define ILEAK   20.E-12 
	END USRLIB MODULE INFORMATION
*/
void vt_field_mos_bk(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vmin, double vmax, double issearch, double vds, double vsub, double intrange, double mrange, double lorange, double * vtf);


/* USRLIB MODULE INFORMATION

	MODULE NAME: vt_gmlin_mos
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 24
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
		IGm,	double *,	Output,	,	,	
		VGm,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#define MAX(a,b)	((a)>(b) ? (a) : (b))
#define CDELAY  125.E-12
#define ILEAK   20.E-12 
	END USRLIB MODULE INFORMATION
*/
void vt_gmlin_mos(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vmin, double vmax, int npoints, double idmax, double vds, double vbs, double intrange, double mrange, double lorange, double * Vtl, double * K, double * Gm, double * IGm, double * VGm);


/* USRLIB MODULE INFORMATION

	MODULE NAME: vt_lin_mos
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
#define MAX(a,b)	((a)>(b) ? (a) : (b))
#define CDELAY  125.E-12
#define ILEAK   20.E-12 
	END USRLIB MODULE INFORMATION
*/
void vt_lin_mos(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vmin, double vmax, int npoints, double idmax, double vds, double vbs, double intrange, double mrange, double lorange, double * Vtl, double * K, double * Gm);


/* USRLIB MODULE INFORMATION

	MODULE NAME: vt_lin_mos_hc
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
void vt_lin_mos_hc(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vmin, double vmax, int npoints, double idmax, double vds, double vbs, double intrange, double mrange, double lorange, double * Vtl, double * K, double * Gm);


/* USRLIB MODULE INFORMATION

	MODULE NAME: vt_lin_mos_multi
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 30
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		gate,	int,	Input,	,	,	
		drain1,	int,	Input,	,	,	
		drain2,	int,	Input,	,	,	
		drain3,	int,	Input,	,	,	
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
		Vtl1,	double *,	Output,	,	,	
		K1,	double *,	Output,	,	,	
		Gm1,	double *,	Output,	,	,	
		Vtl2,	double *,	Output,	,	,	
		K2,	double *,	Output,	,	,	
		Gm2,	double *,	Output,	,	,	
		Vtl3,	double *,	Output,	,	,	
		K3,	double *,	Output,	,	,	
		Gm3,	double *,	Output,	,	,	
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
void vt_lin_mos_multi(char * devname, int gate, int drain1, int drain2, int drain3, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vmin, double vmax, int npoints, double idmax, double vds, double vbs, double intrange, double mrange, double lorange, double * Vtl1, double * K1, double * Gm1, double * Vtl2, double * K2, double * Gm2, double * Vtl3, double * K3, double * Gm3);


/* USRLIB MODULE INFORMATION

	MODULE NAME: vt_lin_mos_raw
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
void vt_lin_mos_raw(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vmin, double vmax, int npoints, double idmax, double vds, double vbs, double intrange, double mrange, double lorange, double * Vtl, double * K, double * Gm);


/* USRLIB MODULE INFORMATION

	MODULE NAME: vt_lin_mos_raw_bjt
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
void vt_lin_mos_raw_bjt(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vmin, double vmax, int npoints, double idmax, double vds, double vbs, double intrange, double mrange, double lorange, double * Vtl, double * K, double * Gm);


/* USRLIB MODULE INFORMATION

	MODULE NAME: vt_lin_mos_rpt
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
void vt_lin_mos_rpt(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vmin, double vmax, int npoints, double idmax, double vds, double vbs, double intrange, double mrange, double lorange, double * Vtl, double * K, double * Gm);


/* USRLIB MODULE INFORMATION

	MODULE NAME: vt_lin_mos_term
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
void vt_lin_mos_term(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vmin, double vmax, int npoints, double idmax, double vds, double vbs, double intrange, double mrange, double lorange, double * Vtl, double * K, double * Gm);


/* USRLIB MODULE INFORMATION

	MODULE NAME: vt_lin_mos_ttr
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
#define MAX(a,b)	((a)>(b) ? (a) : (b))
#define CDELAY  125.E-12
#define ILEAK   20.E-12 
	END USRLIB MODULE INFORMATION
*/
void vt_lin_mos_ttr(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vmin, double vmax, int npoints, double idmax, double vds, double vbs, double intrange, double mrange, double lorange, double * Vtl, double * K, double * Gm);


/* USRLIB MODULE INFORMATION

	MODULE NAME: vt_lin_mos_ttr2
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
#define MAX(a,b)	((a)>(b) ? (a) : (b))
#define CDELAY  125.E-12
#define ILEAK   20.E-12 
	END USRLIB MODULE INFORMATION
*/
void vt_lin_mos_ttr2(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vmin, double vmax, int npoints, double idmax, double vds, double vbs, double intrange, double mrange, double lorange, double * Vtl, double * K, double * Gm);


/* USRLIB MODULE INFORMATION

	MODULE NAME: vt_lin_mos_ttr3
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
#define MAX(a,b)	((a)>(b) ? (a) : (b))
#define CDELAY  125.E-12
#define ILEAK   20.E-12 
	END USRLIB MODULE INFORMATION
*/
void vt_lin_mos_ttr3(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vmin, double vmax, int npoints, double idmax, double vds, double vbs, double intrange, double mrange, double lorange, double * Vtl, double * K, double * Gm);


/* USRLIB MODULE INFORMATION

	MODULE NAME: vt_lin_mos_ttr4
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
#define MAX(a,b)	((a)>(b) ? (a) : (b))
#define CDELAY  125.E-12
#define ILEAK   20.E-12 
	END USRLIB MODULE INFORMATION
*/
void vt_lin_mos_ttr4(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vmin, double vmax, int npoints, double idmax, double vds, double vbs, double intrange, double mrange, double lorange, double * Vtl, double * K, double * Gm);


/* USRLIB MODULE INFORMATION

	MODULE NAME: vt_lin_nosw
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
#define MAX(a,b)	((a)>(b) ? (a) : (b))
#define CDELAY  125.E-12
#define ILEAK   20.E-12 
	END USRLIB MODULE INFORMATION
*/
void vt_lin_nosw(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vmin, double vmax, int npoints, double idmax, double vds, double vbs, double intrange, double mrange, double lorange, double * Vtl, double * K, double * Gm);


/* USRLIB MODULE INFORMATION

	MODULE NAME: vt_lin_ro
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
		vdd,	int,	Input,	,	,	
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
#define MAX(a,b)	((a)>(b) ? (a) : (b))
#define CDELAY  125.E-12
#define ILEAK   20.E-12 
	END USRLIB MODULE INFORMATION
*/
void vt_lin_ro(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, int vdd, double width, double length, char type, double vmin, double vmax, int npoints, double idmax, double vds, double vbs, double intrange, double mrange, double lorange, double * Vtl, double * K, double * Gm);


/* USRLIB MODULE INFORMATION

	MODULE NAME: vt_meas_f05
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
		dummy_wordline1,	int,	Input,	,	,	
		dummy_wordline2,	int,	Input,	,	,	
		dummy_wordline3,	int,	Input,	,	,	
		dummy_wordline4,	int,	Input,	,	,	
		vmin,	double,	Input,	,	,	
		vmax,	double,	Input,	,	,	
		num,	int,	Input,	,	,	
		idf,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		icomp,	double,	Input,	,	,	
		delaytime,	double,	Input,	,	,	
		intrange,	int,	Input,	,	,	
		vt,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void vt_meas_f05(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, int dummy_wordline1, int dummy_wordline2, int dummy_wordline3, int dummy_wordline4, double vmin, double vmax, int num, double idf, double vds, double icomp, double delaytime, int intrange, double * vt);


/* USRLIB MODULE INFORMATION

	MODULE NAME: VTATI_D_ISO
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
		vmin,	double,	Input,	,	,	
		vmax,	double,	Input,	,	,	
		idsearch,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vsub,	double,	Input,	,	,	
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
#define MAX(a,b)	((a)>(b) ? (a) : (b))
#define CDELAY  125.E-12
#define ILEAK   20.E-12 
	END USRLIB MODULE INFORMATION
*/
void VTATI_D_ISO(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vmin, double vmax, double idsearch, double vds, double vsub, double multiplier, double intrange, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: VTATI_D_mos
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 20
	ARGUMENTS:
		devname,	char *,	Input,	 ,	 ,	 
		gate,	int,	Input,	 ,	 ,	 
		drain,	int,	Input,	 ,	 ,	 
		source,	int,	Input,	 ,	 ,	 
		bulk,	int,	Input,	 ,	 ,	 
		sub,	int,	Input,	 ,	 ,	 
		chuckcon,	int,	Input,	 ,	 ,	 
		width,	double,	Input,	 ,	 ,	 
		length,	double,	Input,	 ,	 ,	 
		type,	char,	Input,	 ,	 ,	 
		vmin,	double,	Input,	 ,	 ,	 
		vmax,	double,	Input,	 ,	 ,	 
		idsearch,	double,	Input,	 ,	 ,	 
		vds,	double,	Input,	 ,	 ,	 
		vsub,	double,	Input,	 ,	 ,	 
		multiplier,	double,	Input,	 ,	 ,	 
		intrange,	double,	Input,	 ,	 ,	 
		mrange,	double,	Input,	 ,	 ,	 
		lorange,	double,	Input,	 ,	 ,	 
		result,	double *,	Output,	 ,	 ,	 
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
void VTATI_D_mos(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vmin, double vmax, double idsearch, double vds, double vsub, double multiplier, double intrange, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: VTATI_D_tsb_mos
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
		vmin,	double,	Input,	,	,	
		vmax,	double,	Input,	,	,	
		idsearch,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vsub,	double,	Input,	,	,	
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
#define MAX(a,b)	((a)>(b) ? (a) : (b))
#define CDELAY  125.E-12
#define ILEAK   20.E-12 
	END USRLIB MODULE INFORMATION
*/
void VTATI_D_tsb_mos(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vmin, double vmax, double idsearch, double vds, double vsub, double multiplier, double intrange, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: VTATI_S_mos
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 20
	ARGUMENTS:
		devname,	char *,	Input,	,	 ,	 
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
		vsub,	double,	Input,	,	,	
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
#define MAX(a,b)	((a)>(b) ? (a) : (b))
#define CDELAY  125.E-12
#define ILEAK   20.E-12 
	END USRLIB MODULE INFORMATION
*/
void VTATI_S_mos(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vmin, double vmax, double idsearch, double vds, double vsub, double multiplier, double intrange, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: vti_mos
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 22
	ARGUMENTS:
		devname,	char *,	Input,	 ,	 ,	 
		gate,	int,	Input,	 ,	 ,	 
		drain,	int,	Input,	 ,	 ,	 
		source,	int,	Input,	 ,	 ,	 
		bulk,	int,	Input,	 ,	 ,	 
		sub,	int,	Input,	 ,	 ,	 
		chuckcon,	int,	Input,	 ,	 ,	 
		width,	double,	Input,	 ,	 ,	 
		length,	double,	Input,	 ,	 ,	 
		type,	char,	Input,	 ,	 ,	 
		vmin,	double,	Input,	 ,	 ,	 
		vmax,	double,	Input,	 ,	 ,	 
		sd,	char,	Input,	 ,	 ,	 
		i_search,	double,	Input,	 ,	 ,	 
		isearch_n,	char,	Input,	 ,	 ,	 
		vforce,	double,	Input,	 ,	 ,	 
		vbs,	double,	Input,	 ,	 ,	 
		intrange,	double,	Input,	 ,	 ,	 
		delaytime,	double,	Input,	 ,	 ,	 
		mrange,	double,	Input,	 ,	 ,	 
		lorange,	double,	Input,	 ,	 ,	 
		result,	double *,	Output,	 ,	 ,	 
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void vti_mos(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vmin, double vmax, char sd, double i_search, char isearch_n, double vforce, double vbs, double intrange, double delaytime, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: vtmin_ews
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 20
	ARGUMENTS:
		devname,	char *,	Input,	 ,	 ,	 
		owl,	int,	Input,	 ,	 ,	 
		ewl,	int,	Input,	 ,	 ,	 
		obl,	int,	Input,	 ,	 ,	 
		ebl,	int,	Input,	 ,	 ,	 
		source,	int,	Input,	 ,	 ,	 
		ipwl,	int,	Input,	 ,	 ,	 
		dnwl,	int,	Input,	 ,	 ,	 
		sub,	int,	Input,	 ,	 ,	 
		exwl,	int,	Input,	 ,	 ,	 
		vmin,	double,	Input,	 ,	 ,	 
		vmax,	double,	Input,	 ,	 ,	 
		idsearch,	double,	Input,	 ,	 ,	 
		vds,	double,	Input,	 ,	 ,	 
		vbs,	double,	Input,	 ,	 ,	 
		multiplier,	double,	Input,	 ,	 ,	 
		intrange,	double,	Input,	 ,	 ,	 
		mrange,	double,	Input,	 ,	 ,	 
		lorange,	double,	Input,	 ,	 ,	 
		result,	double *,	Output,	 ,	 ,	 
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#define MAX(a,b)	((a)>(b) ? (a) : (b))
#define CDELAY  125.E-12
#define ILEAK   20.E-12 
	END USRLIB MODULE INFORMATION
*/
void vtmin_ews(char * devname, int owl, int ewl, int obl, int ebl, int source, int ipwl, int dnwl, int sub, int exwl, double vmin, double vmax, double idsearch, double vds, double vbs, double multiplier, double intrange, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: vtmin_ews_cro
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 20
	ARGUMENTS:
		devname,	char *,	Input,	,	 ,	 
		owl,	int,	Input,	,	,	
		ewl,	int,	Input,	,	,	
		obl,	int,	Input,	,	,	
		ebl,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		ipwl,	int,	Input,	,	,	
		dnwl,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		exwl,	int,	Input,	,	,	
		vmin,	double,	Input,	,	,	
		vmax,	double,	Input,	,	,	
		idsearch,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
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
#define MAX(a,b)	((a)>(b) ? (a) : (b))
#define CDELAY  125.E-12
#define ILEAK   20.E-12 
	END USRLIB MODULE INFORMATION
*/
void vtmin_ews_cro(char * devname, int owl, int ewl, int obl, int ebl, int source, int ipwl, int dnwl, int sub, int exwl, double vmin, double vmax, double idsearch, double vds, double vbs, double multiplier, double intrange, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: vtsat_mos
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
		npoints,	double,	Input,	,	,	
		idmax,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
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
#define MAX(a,b)	((a)>(b) ? (a) : (b))
#define CDELAY  125.E-12
#define ILEAK   20.E-12 
	END USRLIB MODULE INFORMATION
*/
void vtsat_mos(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vmin, double vmax, double npoints, double idmax, double vds, double vbs, double multiplier, double intrange, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: VTSAT_mos
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
		npoints,	double,	Input,	,	,	
		idmax,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
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
#define MAX(a,b)	((a)>(b) ? (a) : (b))
#define CDELAY  125.E-12
#define ILEAK   20.E-12 
	END USRLIB MODULE INFORMATION
*/
void VTSAT_mos(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vmin, double vmax, double npoints, double idmax, double vds, double vbs, double multiplier, double intrange, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: VTSAT_mos_swp
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
		npoints,	double,	Input,	,	,	
		idmax,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
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
#include <stdlib.h>
#include <ktxe_proto.h>
#include <ksox_def.h>

#define MAX(a,b)	((a)>(b) ? (a) : (b))
#define CDELAY  125.E-12
#define ILEAK   20.E-12 
	END USRLIB MODULE INFORMATION
*/
void VTSAT_mos_swp(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vmin, double vmax, double npoints, double idmax, double vds, double vbs, double multiplier, double intrange, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: vtspot_mos
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 24
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
		idf,	double,	Input,	,	,	
		vdsf,	double,	Input,	,	,	
		vmin,	double,	Input,	,	,	
		vmax,	double,	Input,	,	,	
		vstep,	double,	Input,	,	,	
		vbsf,	double,	Input,	,	,	
		vckf,	double,	Input,	,	,	
		igood,	double,	Input,	,	,	
		icomp,	double,	Input,	,	,	
		sd,	char,	Input,	,	,	
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
	END USRLIB MODULE INFORMATION
*/
void vtspot_mos(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double idf, double vdsf, double vmin, double vmax, double vstep, double vbsf, double vckf, double igood, double icomp, char sd, double intrange, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: vtspot_mos_10ms
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 24
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
		idf,	double,	Input,	,	,	
		vdsf,	double,	Input,	,	,	
		vmin,	double,	Input,	,	,	
		vmax,	double,	Input,	,	,	
		vstep,	double,	Input,	,	,	
		vbsf,	double,	Input,	,	,	
		vckf,	double,	Input,	,	,	
		igood,	double,	Input,	,	,	
		icomp,	double,	Input,	,	,	
		sd,	char,	Input,	,	,	
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
	END USRLIB MODULE INFORMATION
*/
void vtspot_mos_10ms(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double idf, double vdsf, double vmin, double vmax, double vstep, double vbsf, double vckf, double igood, double icomp, char sd, double intrange, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: vtspot_mos_cro
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 24
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
		idf,	double,	Input,	,	,	
		vdsf,	double,	Input,	,	,	
		vmin,	double,	Input,	,	,	
		vmax,	double,	Input,	,	,	
		vstep,	double,	Input,	,	,	
		vbsf,	double,	Input,	,	,	
		vckf,	double,	Input,	,	,	
		igood,	double,	Input,	,	,	
		icomp,	double,	Input,	,	,	
		sd,	char,	Input,	,	,	
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
	END USRLIB MODULE INFORMATION
*/
void vtspot_mos_cro(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double idf, double vdsf, double vmin, double vmax, double vstep, double vbsf, double vckf, double igood, double icomp, char sd, double intrange, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: vtspot_mos_t
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 25
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
		idf,	double,	Input,	,	,	
		vdsf,	double,	Input,	,	,	
		vmin,	double,	Input,	,	,	
		vmax,	double,	Input,	,	,	
		vstep,	double,	Input,	,	,	
		vbsf,	double,	Input,	,	,	
		vckf,	double,	Input,	,	,	
		igood,	double,	Input,	,	,	
		icomp,	double,	Input,	,	,	
		sd,	char,	Input,	,	,	
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
	END USRLIB MODULE INFORMATION
*/
void vtspot_mos_t(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double idf, double vdsf, double vmin, double vmax, double vstep, double vbsf, double vckf, double igood, double icomp, char sd, double intrange, double delaytime, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: vtsslp_mos
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
void vtsslp_mos(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vmin, double vmax, double idsearch, double vds, double vbs, double multiplier, double intrange, double delaytime, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: vtsslp_sq_mos
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
void vtsslp_sq_mos(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vmin, double vmax, double idsearch, double vds, double vbs, double multiplier, double intrange, double delaytime, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: vtsslp_sq_mos_sw02
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
void vtsslp_sq_mos_sw02(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vmin, double vmax, double idsearch, double vds, double vbs, double multiplier, double intrange, double delaytime, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: vtsslp_sq_nati_mos
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
void  vtsslp_sq_nati_mos(char *, int, int, int, int, int, int, double, double, char, double, double, double, double, double, double, double, double, double, double, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: vtsslp_width_mos
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
void vtsslp_width_mos(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vmin, double vmax, double idsearch, double vds, double vbs, double multiplier, double intrange, double delaytime, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: VTXPL_mos
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
#include <math.h>
#include <par_util.h>
#include <tidp1.h>
#define MAX(a,b)	((a)>(b) ? (a) : (b))
#define CDELAY  125.E-12
#define ILEAK   20.E-12 
	END USRLIB MODULE INFORMATION
*/
void VTXPL_mos(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vmin, double vmax, int npoints, double idmax, double vds, double vbs, double mvtx, double mk, double intrange, double mrange, double lorange, double * result_vt, double * result_k);


/* USRLIB MODULE INFORMATION

	MODULE NAME: VTXPL_mos_2pad
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 26
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		gate,	int,	Input,	,	,	
		gate2,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		drain2,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		source2,	int,	Input,	,	,	
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
#include <math.h>
#include <par_util.h>
#include <tidp1.h>
#define MAX(a,b)	((a)>(b) ? (a) : (b))
#define CDELAY  125.E-12
#define ILEAK   20.E-12 
	END USRLIB MODULE INFORMATION
*/
void VTXPL_mos_2pad(char * devname, int gate, int gate2, int drain, int drain2, int source, int source2, int bulk, int sub, int chuckcon, double width, double length, char type, double vmin, double vmax, int npoints, double idmax, double vds, double vbs, double mvtx, double mk, double intrange, double mrange, double lorange, double * result_vt, double * result_k);


/* USRLIB MODULE INFORMATION

	MODULE NAME: VTXPL_mos_bjt
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
void VTXPL_mos_bjt(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vmin, double vmax, int npoints, double idmax, double vds, double vbs, double mvtx, double mk, double intrange, double mrange, double lorange, double * result_vt, double * result_k);


/* USRLIB MODULE INFORMATION

	MODULE NAME: VTXPL_mos_raw
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
#include <math.h>
#include <par_util.h>
#include <tidp1.h>

#include <stdlib.h>
#include <ktxe_proto.h>
#include <ksox_def.h>

#define MAX(a,b)	((a)>(b) ? (a) : (b))
#define CDELAY  125.E-12
#define ILEAK   20.E-12 


	END USRLIB MODULE INFORMATION
*/
void VTXPL_mos_raw(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vmin, double vmax, int npoints, double idmax, double vds, double vbs, double mvtx, double mk, double intrange, double mrange, double lorange, double * result_vt, double * result_k);


/* USRLIB MODULE INFORMATION

	MODULE NAME: VTXPL_mos_ttr
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
#include <math.h>
#include <par_util.h>
#include <tidp1.h>
#define MAX(a,b)	((a)>(b) ? (a) : (b))
#define CDELAY  125.E-12
#define ILEAK   20.E-12 
	END USRLIB MODULE INFORMATION
*/
void VTXPL_mos_ttr(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vmin, double vmax, int npoints, double idmax, double vds, double vbs, double mvtx, double mk, double intrange, double mrange, double lorange, double * result_vt, double * result_k);


/* USRLIB MODULE INFORMATION

	MODULE NAME: VTXPL_na_mos
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
#include <math.h>
#include <par_util.h>
#include <tidp1.h>
#define MAX(a,b)	((a)>(b) ? (a) : (b))
#define CDELAY  125.E-12
#define ILEAK   20.E-12 
	END USRLIB MODULE INFORMATION
*/
void VTXPL_na_mos(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vmin, double vmax, int npoints, double idmax, double vds, double vbs, double mvtx, double mk, double intrange, double mrange, double lorange, double * result_vt, double * result_k);


/* USRLIB MODULE INFORMATION

	MODULE NAME: VTXPL_sp0_mos
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
#include <math.h>
#include <par_util.h>
#include <tidp1.h>
#define MAX(a,b)	((a)>(b) ? (a) : (b))
#define CDELAY  125.E-12
#define ILEAK   20.E-12 
	END USRLIB MODULE INFORMATION
*/
void VTXPL_sp0_mos(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vmin, double vmax, int npoints, double idmax, double vds, double vbs, double mvtx, double mk, double intrange, double mrange, double lorange, double * result_vt, double * result_k);


/* USRLIB MODULE INFORMATION

	MODULE NAME: width_rs4
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 22
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
		rsheet,	double,	Input,	,	,	
		vsub,	double,	Input,	,	,	
		delaytime,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		Rsh,	double *,	Output,	,	,	
		WR,	double *,	Output,	,	,	
		W,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void width_rs4(char * devname, int f1, int f2, int s1, int s2, int sub, int bulk, int chuckcon, double width, double length, char type, double iforce, double vlimit, double rsheet, double vsub, double delaytime, double intrange, double mrange, double lorange, double * Rsh, double * WR, double * W);


/* USRLIB MODULE INFORMATION

	MODULE NAME: width_rs4_kj
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 22
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
		rsheet,	double,	Input,	,	,	
		vsub,	double,	Input,	,	,	
		delaytime,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		Rsh,	double *,	Output,	,	,	
		WR,	double *,	Output,	,	,	
		W,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void width_rs4_kj(char * devname, int f1, int f2, int s1, int s2, int sub, int bulk, int chuckcon, double width, double length, char type, double iforce, double vlimit, double rsheet, double vsub, double delaytime, double intrange, double mrange, double lorange, double * Rsh, double * WR, double * W);


/* USRLIB MODULE INFORMATION

	MODULE NAME: width_rs4_new
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 22
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
		rsheet,	double,	Input,	,	,	
		vsub,	double,	Input,	,	,	
		delaytime,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		Rsh,	double *,	Output,	,	,	
		WR,	double *,	Output,	,	,	
		W,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void width_rs4_new(char * devname, int f1, int f2, int s1, int s2, int sub, int bulk, int chuckcon, double width, double length, char type, double iforce, double vlimit, double rsheet, double vsub, double delaytime, double intrange, double mrange, double lorange, double * Rsh, double * WR, double * W);


/* USRLIB MODULE INFORMATION

	MODULE NAME: write_trip_sra
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 21
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		vdd,	int,	Input,	,	,	
		bit,	int,	Input,	,	,	
		bib,	int,	Input,	,	,	
		wl,	int,	Input,	,	,	
		vss,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		area,	double,	Input,	,	,	
		pass,	double,	Input,	,	,	
		driver,	double,	Input,	,	,	
		load,	double,	Input,	,	,	
		supply,	double,	Input,	,	,	
		vbtln,	double,	Input,	,	,	
		vwl,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		debug2,	int,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		vp0,	double *,	Output,	,	,	
		vp1,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void write_trip_sra(char * devname, int vdd, int bit, int bib, int wl, int vss, int sub, int chuckcon, double area, double pass, double driver, double load, double supply, double vbtln, double vwl, double intrange, int debug2, double mrange, double lorange, double * vp0, double * vp1);


/* USRLIB MODULE INFORMATION

	MODULE NAME: yield_cer
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 16
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		ta,	int,	Input,	,	,	
		ba,	int,	Input,	,	,	
		tb,	int,	Input,	,	,	
		bb,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		bulk,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		area,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
		input,	double,	Input,	5,	,	
		inh,	double,	Input,	1e-6,	,	
		inl,	double,	Input,	0,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		Yield,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void yield_cer(char * devname, int ta, int ba, int tb, int bb, int sub, int bulk, int chuckcon, double area, char type, double input, double inh, double inl, double mrange, double lorange, double * Yield);


/* USRLIB MODULE INFORMATION

	MODULE NAME: yield_cst
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 15
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		t1,	int,	Input,	,	,	
		t2,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		bulk,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		n,	double,	Input,	,	,	
		area,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
		input,	double,	Input,	5,	,	
		inh,	double,	Input,	1e-6,	,	
		inl,	double,	Input,	0,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		Yield,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void yield_cst(char * devname, int t1, int t2, int sub, int bulk, int chuckcon, double n, double area, char type, double input, double inh, double inl, double mrange, double lorange, double * Yield);


/* USRLIB MODULE INFORMATION

	MODULE NAME: yield_rs2
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 15
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
		input,	double,	Input,	5,	,	
		inh,	double,	Input,	1e-6,	,	
		inl,	double,	Input,	0,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		Yield,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void yield_rs2(char * devname, int hi, int lo, int sub, int bulk, int chuckcon, double width, double length, char type, double input, double inh, double inl, double mrange, double lorange, double * Yield);


/* USRLIB MODULE INFORMATION

	MODULE NAME: yield_rs4
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 17
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
		input,	double,	Input,	10,	,	
		inh,	double,	Input,	1e-03,	,	
		inl,	double,	Input,	0.01,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		Yield,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void yield_rs4(char * devname, int f1, int f2, int s1, int s2, int sub, int bulk, int chuckcon, double width, double length, char type, double input, double inh, double inl, double mrange, double lorange, double * Yield);


/* USRLIB MODULE INFORMATION

	MODULE NAME: zsave_ary
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 4
	ARGUMENTS:
		ary1,	D_ARRAY_T,	Input,	,	,	
		size1,	int,	Input,	,	,	
		ary2,	D_ARRAY_T,	Input,	,	,	
		size2,	int,	Input,	,	,	
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
void zsave_ary(double *ary1, int size1, double *ary2, int size2);


/* USRLIB MODULE INFORMATION

	MODULE NAME: zsave_ary2
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 4
	ARGUMENTS:
		ary1,	D_ARRAY_T,	Input,	,	,	
		size1,	int,	Input,	,	,	
		ary2,	D_ARRAY_T,	Input,	,	,	
		size2,	int,	Input,	,	,	
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
void zsave_ary2(double *ary1, int size1, double *ary2, int size2);


