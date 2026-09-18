/* haemil function prototype and KITT header file */

/* COPY OF USRLIB SETTINGS INFORMATION */
/* [VISIBILITY] */
/* HIDDEN=0 */
/* [DEPENDENCIES] */
/* LIBS=-lktest -lLBC5 -lLBC5x -loptlib */
/* OBJECTS= */
/* HEADERS= */
/* [NON-KULT SEARCH PATH] */
/* PATHS= */

/* USRLIB MODULE INFORMATION

	MODULE NAME: allgnd
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
void allgnd();


/* USRLIB MODULE INFORMATION

	MODULE NAME: AUTO_TEST
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 7
	ARGUMENTS:
		delaytime,	double,	Input,	,	,	
		intrange_value,	int,	Input,	,	1,	10
		lorange_value,	double,	Input,	,	,	
		current_spec,	double,	Input,	,	,	
		vstart,	double,	Input,	,	,	
		vstop,	double,	Input,	,	,	
		steps,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <stdlib.h>
#include <string.h>
#include <time.h>
	END USRLIB MODULE INFORMATION
*/
void AUTO_TEST(double delaytime, int intrange_value, double lorange_value, double current_spec, double vstart, double vstop, int steps);


/* USRLIB MODULE INFORMATION

	MODULE NAME: beta10
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 22
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		em,	int,	Input,	-1,	,	
		ba,	int,	Input,	-1,	,	
		co,	int,	Input,	-1,	,	
		sub,	int,	Input,	-1,	,	
		vbmin,	double,	Input,	0,	,	
		vbmax,	double,	Input,	1,	,	
		vbstep,	double,	Input,	0.05,	,	
		hfe_l,	double,	Input,	10e-9,	,	
		hfe_m,	double,	Input,	1e-9,	,	
		hfe_h,	double,	Input,	0.1e-6,	,	
		delaytime,	double,	Input,	0.01,	,	
		vce,	double,	Input,	0,	,	
		vsub,	double,	Input,	0,	,	
		plc_val,	int,	Input,	0,	,	
		type,	char,	Input,	,	,	
		vbe_l_out,	double *,	Output,	,	,	
		vbe_m_out,	double *,	Output,	,	,	
		vbe_h_out,	double *,	Output,	,	,	
		hfe_l_out,	double *,	Output,	,	,	
		hfe_m_out,	double *,	Output,	,	,	
		hfe_h_out,	double *,	Output,	,	,	
	INCLUDES:
#include <stdlib.h>
#include <stdio.h>	
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <par_util.h>
#include <ktxe_proto.h>
#include <ksox_def.h>
#include <kui_proto.h>
#include "LBC5_proto.h"
#include <math.h>  
	END USRLIB MODULE INFORMATION
*/
double beta10(char * devname, int em, int ba, int co, int sub, double vbmin, double vbmax, double vbstep, double hfe_l, double hfe_m, double hfe_h, double delaytime, double vce, double vsub, int plc_val, char type, double * vbe_l_out, double * vbe_m_out, double * vbe_h_out, double * hfe_l_out, double * hfe_m_out, double * hfe_h_out);


/* USRLIB MODULE INFORMATION

	MODULE NAME: beta2X
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 13
	ARGUMENTS:
		em,	int,	Input,	,	,	
		base,	int,	Input,	,	,	
		coll,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		G1,	int,	Input,	,	,	
		ic,	double,	Input,	,	,	
		vce,	double,	Input,	,	,	
		vsub,	double,	Input,	,	,	
		vsub_ilimit,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
		debug,	int,	Input,	,	,	
		vbeout,	double *,	Output,	,	,	
		icout,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double beta2X(int em, int base, int coll, int subst, int G1, double ic, double vce, double vsub, double vsub_ilimit, char type, int debug, double * vbeout, double * icout);


/* USRLIB MODULE INFORMATION

	MODULE NAME: beta9
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
	END USRLIB MODULE INFORMATION
*/
double beta9(int em, int base, int coll, int subst, double ic, double vce, double * vbeout, double * icout, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: betaH
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 22
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		em,	int,	Input,	-1,	,	
		ba,	int,	Input,	-1,	,	
		co,	int,	Input,	-1,	,	
		sub,	int,	Input,	-1,	,	
		vbmin,	double,	Input,	0,	,	
		vbmax,	double,	Input,	1,	,	
		vbstep,	double,	Input,	0.05,	,	
		hfe_l,	double,	Input,	10e-9,	,	
		hfe_m,	double,	Input,	1e-9,	,	
		hfe_h,	double,	Input,	0.1e-6,	,	
		delaytime,	double,	Input,	0.01,	,	
		vce,	double,	Input,	0,	,	
		vsub,	double,	Input,	0,	,	
		plc_val,	int,	Input,	0,	,	
		type,	char,	Input,	,	,	
		vbe_l_out,	double *,	Output,	,	,	
		vbe_m_out,	double *,	Output,	,	,	
		vbe_h_out,	double *,	Output,	,	,	
		hfe_l_out,	double *,	Output,	,	,	
		hfe_m_out,	double *,	Output,	,	,	
		hfe_h_out,	double *,	Output,	,	,	
	INCLUDES:
#include <stdlib.h>
#include <stdio.h>	
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <par_util.h>
#include <ktxe_proto.h>
#include <ksox_def.h>
#include <kui_proto.h>
#include "LBC5_proto.h"
#include <math.h>  
	END USRLIB MODULE INFORMATION
*/
double  betaH(char *, int, int, int, int, double, double, double, double, double, double, double, double, double, int, char, double *, double *, double *, double *, double *, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: betaH_ext
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 23
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		em,	int,	Input,	-1,	,	
		ba,	int,	Input,	-1,	,	
		co,	int,	Input,	-1,	,	
		sub,	int,	Input,	-1,	,	
		vbmin,	double,	Input,	0,	,	
		vbmax,	double,	Input,	1,	,	
		vbstep,	double,	Input,	0.05,	,	
		hfe_l,	double,	Input,	10e-9,	,	
		hfe_m,	double,	Input,	1e-9,	,	
		hfe_h,	double,	Input,	0.1e-6,	,	
		delaytime,	double,	Input,	0.01,	,	
		vce,	double,	Input,	0,	,	
		vsub,	double,	Input,	0,	,	
		plc_val,	int,	Input,	0,	,	
		type,	char,	Input,	,	,	
		debug_flag,	int,	Input,	,	,	
		vbe_l_out,	double *,	Output,	,	,	
		vbe_m_out,	double *,	Output,	,	,	
		vbe_h_out,	double *,	Output,	,	,	
		hfe_l_out,	double *,	Output,	,	,	
		hfe_m_out,	double *,	Output,	,	,	
		hfe_h_out,	double *,	Output,	,	,	
	INCLUDES:
#include <stdlib.h>
#include <stdio.h>	
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <par_util.h>
#include <ktxe_proto.h>
#include <ksox_def.h>
#include <kui_proto.h>
#include "LBC5_proto.h"
#include <math.h>  
	END USRLIB MODULE INFORMATION
*/
double  betaH_ext(char *, int, int, int, int, double, double, double, double, double, double, double, double, double, int, char, int, double *, double *, double *, double *, double *, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: betaH_k
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 23
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		em,	int,	Input,	-1,	,	
		ba,	int,	Input,	-1,	,	
		co,	int,	Input,	-1,	,	
		sub,	int,	Input,	-1,	,	
		vbmin,	double,	Input,	0,	,	
		vbmax,	double,	Input,	1,	,	
		vbstep,	double,	Input,	0.05,	,	
		hfe_l,	double,	Input,	10e-9,	,	
		hfe_m,	double,	Input,	1e-9,	,	
		hfe_h,	double,	Input,	0.1e-6,	,	
		delaytime,	double,	Input,	0.01,	,	
		vce,	double,	Input,	0,	,	
		vsub,	double,	Input,	0,	,	
		plc_val,	int,	Input,	0,	,	
		type,	char,	Input,	,	,	
		debug_flag,	int,	Input,	,	,	
		vbe_l_out,	double *,	Output,	,	,	
		vbe_m_out,	double *,	Output,	,	,	
		vbe_h_out,	double *,	Output,	,	,	
		hfe_l_out,	double *,	Output,	,	,	
		hfe_m_out,	double *,	Output,	,	,	
		hfe_h_out,	double *,	Output,	,	,	
	INCLUDES:
#include <stdlib.h>
#include <stdio.h>	
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <par_util.h>
#include <ktxe_proto.h>
#include <ksox_def.h>
#include <kui_proto.h>
#include "LBC5_proto.h"
#include <math.h>  
	END USRLIB MODULE INFORMATION
*/
double betaH_k(char * devname, int em, int ba, int co, int sub, double vbmin, double vbmax, double vbstep, double hfe_l, double hfe_m, double hfe_h, double delaytime, double vce, double vsub, int plc_val, char type, int debug_flag, double * vbe_l_out, double * vbe_m_out, double * vbe_h_out, double * hfe_l_out, double * hfe_m_out, double * hfe_h_out);


/* USRLIB MODULE INFORMATION

	MODULE NAME: betaH_k_ext
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 23
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		em,	int,	Input,	-1,	,	
		ba,	int,	Input,	-1,	,	
		co,	int,	Input,	-1,	,	
		sub,	int,	Input,	-1,	,	
		vbmin,	double,	Input,	0,	,	
		vbmax,	double,	Input,	1,	,	
		vbstep,	double,	Input,	0.05,	,	
		hfe_l,	double,	Input,	10e-9,	,	
		hfe_m,	double,	Input,	1e-9,	,	
		hfe_h,	double,	Input,	0.1e-6,	,	
		delaytime,	double,	Input,	0.01,	,	
		vce,	double,	Input,	0,	,	
		vsub,	double,	Input,	0,	,	
		plc_val,	int,	Input,	0,	,	
		type,	char,	Input,	,	,	
		debug_flag,	int,	Input,	,	,	
		vbe_l_out,	double *,	Output,	,	,	
		vbe_m_out,	double *,	Output,	,	,	
		vbe_h_out,	double *,	Output,	,	,	
		hfe_l_out,	double *,	Output,	,	,	
		hfe_m_out,	double *,	Output,	,	,	
		hfe_h_out,	double *,	Output,	,	,	
	INCLUDES:
#include <stdlib.h>
#include <stdio.h>	
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <par_util.h>
#include <ktxe_proto.h>
#include <ksox_def.h>
#include <kui_proto.h>
#include "LBC5_proto.h"
#include <math.h>  
	END USRLIB MODULE INFORMATION
*/
double  betaH_k_ext(char *, int, int, int, int, double, double, double, double, double, double, double, double, double, int, char, int, double *, double *, double *, double *, double *, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: betaH_org170609
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 22
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		em,	int,	Input,	-1,	,	
		ba,	int,	Input,	-1,	,	
		co,	int,	Input,	-1,	,	
		sub,	int,	Input,	-1,	,	
		vbmin,	double,	Input,	0,	,	
		vbmax,	double,	Input,	1,	,	
		vbstep,	double,	Input,	0.05,	,	
		hfe_l,	double,	Input,	10e-9,	,	
		hfe_m,	double,	Input,	1e-9,	,	
		hfe_h,	double,	Input,	0.1e-6,	,	
		delaytime,	double,	Input,	0.01,	,	
		vce,	double,	Input,	0,	,	
		vsub,	double,	Input,	0,	,	
		plc_val,	int,	Input,	0,	,	
		type,	char,	Input,	,	,	
		vbe_l_out,	double *,	Output,	,	,	
		vbe_m_out,	double *,	Output,	,	,	
		vbe_h_out,	double *,	Output,	,	,	
		hfe_l_out,	double *,	Output,	,	,	
		hfe_m_out,	double *,	Output,	,	,	
		hfe_h_out,	double *,	Output,	,	,	
	INCLUDES:
#include <stdlib.h>
#include <stdio.h>	
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <par_util.h>
#include <ktxe_proto.h>
#include <ksox_def.h>
#include <kui_proto.h>
#include "LBC5_proto.h"
#include <math.h>  
	END USRLIB MODULE INFORMATION
*/
double  betaH_org170609(char *, int, int, int, int, double, double, double, double, double, double, double, double, double, int, char, double *, double *, double *, double *, double *, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: BetaO
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 24
	ARGUMENTS:
		devname,	char *,	Input,	"PNP",	,	
		em,	int,	Input,	-1,	,	
		ba,	int,	Input,	-1,	,	
		co,	int,	Input,	-1,	,	
		sub,	int,	Input,	-1,	,	
		vbmin,	double,	Input,	0,	,	
		vbmax,	double,	Input,	1,	,	
		vbstep,	double,	Input,	0.05,	,	
		hfe_l,	double,	Input,	10e-9,	,	
		hfe_m,	double,	Input,	1e-9,	,	
		hfe_h,	double,	Input,	0.1e-6,	,	
		ps_th,	double,	Input,	0.1e-6,	,	
		delaytime,	double,	Input,	0.01,	,	
		vce,	double,	Input,	0,	,	
		vsub,	double,	Input,	0,	,	
		plc_val,	int,	Input,	0,	,	
		type,	char,	Input,	'n',	,	
		vbe_l_out,	double *,	Output,	,	,	
		vbe_m_out,	double *,	Output,	,	,	
		vbe_h_out,	double *,	Output,	,	,	
		hfe_l_out,	double *,	Output,	,	,	
		hfe_m_out,	double *,	Output,	,	,	
		hfe_h_out,	double *,	Output,	,	,	
		ps_gain,	double *,	Output,	,	,	
	INCLUDES:
#include <stdlib.h>
#include <stdio.h>	
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <par_util.h>
#include <ktxe_proto.h>
#include <ksox_def.h>
#include <kui_proto.h>
#include "LBC5_proto.h"
#include <math.h>  
	END USRLIB MODULE INFORMATION
*/
double BetaO(char * devname, int em, int ba, int co, int sub, double vbmin, double vbmax, double vbstep, double hfe_l, double hfe_m, double hfe_h, double ps_th, double delaytime, double vce, double vsub, int plc_val, char type, double * vbe_l_out, double * vbe_m_out, double * vbe_h_out, double * hfe_l_out, double * hfe_m_out, double * hfe_h_out, double * ps_gain);


/* USRLIB MODULE INFORMATION

	MODULE NAME: betaRxR
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 22
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		em,	int,	Input,	-1,	,	
		ba,	int,	Input,	-1,	,	
		co,	int,	Input,	-1,	,	
		sub,	int,	Input,	-1,	,	
		vbmin,	double,	Input,	0,	,	
		vbmax,	double,	Input,	1,	,	
		vbstep,	double,	Input,	0.05,	,	
		hfe_l,	double,	Input,	10e-9,	,	
		hfe_m,	double,	Input,	1e-9,	,	
		hfe_h,	double,	Input,	0.1e-6,	,	
		delaytime,	double,	Input,	0.01,	,	
		vce,	double,	Input,	0,	,	
		vsub,	double,	Input,	0,	,	
		plc_val,	int,	Input,	0,	,	
		type,	char,	Input,	,	,	
		vbe_l_out,	double *,	Output,	,	,	
		vbe_m_out,	double *,	Output,	,	,	
		vbe_h_out,	double *,	Output,	,	,	
		hfe_l_out,	double *,	Output,	,	,	
		hfe_m_out,	double *,	Output,	,	,	
		hfe_h_out,	double *,	Output,	,	,	
	INCLUDES:
#include <stdlib.h>
#include <stdio.h>	
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <par_util.h>
#include <ktxe_proto.h>
#include <ksox_def.h>
#include <kui_proto.h>
#include "LBC5_proto.h"
#include <math.h>  
	END USRLIB MODULE INFORMATION
*/
double betaRxR(char * devname, int em, int ba, int co, int sub, double vbmin, double vbmax, double vbstep, double hfe_l, double hfe_m, double hfe_h, double delaytime, double vce, double vsub, int plc_val, char type, double * vbe_l_out, double * vbe_m_out, double * vbe_h_out, double * hfe_l_out, double * hfe_m_out, double * hfe_h_out);


/* USRLIB MODULE INFORMATION

	MODULE NAME: betaS
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 13
	ARGUMENTS:
		em,	int,	Input,	-1,	,	
		base,	int,	Input,	-1,	,	
		coll,	int,	Input,	-1,	,	
		subst,	int,	Input,	-1,	,	
		addcon,	int,	Input,	-1,	,	
		ic,	double,	Input,	1e-6,	,	
		vce,	double,	Input,	5,	,	
		vsub,	double,	Input,	0,	,	
		limit,	double,	Input,	1e-3,	,	
		type,	char,	Input,	'n',	,	
		debug,	int,	Input,	1,	,	
		vbeout,	double *,	Output,	,	,	
		icout,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double betaS(int em, int base, int coll, int subst, int addcon, double ic, double vce, double vsub, double limit, char type, int debug, double * vbeout, double * icout);


/* USRLIB MODULE INFORMATION

	MODULE NAME: breakS
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 19
	ARGUMENTS:
		d,	int,	Input,	-1,	,	
		g,	int,	Input,	-1,	,	
		s,	int,	Input,	-1,	,	
		b,	int,	Input,	-1,	,	
		g1,	int,	Input,	-1,	,	
		g2,	int,	Input,	-1,	,	
		vdsmin,	double,	Input,	0,	-100,	100
		vdsmax,	double,	Input,	40,	-200,	200
		vdstep,	double,	Input,	1,	-3,	3
		Ith,	double,	Input,	1e-6,	,	
		plc_val,	double,	Input,	1,	0,	10
		Delay,	double,	Input,	0.001,	0,	0.3
		vg,	double,	Input,	0,	-60,	60
		vs,	double,	Input,	0,	-40,	40
		vb,	double,	Input,	0,	-40,	40
		mode,	char,	Input,	,	,	
		sd,	char,	Input,	'd',	,	
		Range,	double,	Input,	0.1,	-1,	1
		bvdss,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <stdlib.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#include <ksox_def.h>
#include <kui_proto.h>
#include <ktxe_proto.h>
#include "LBC5_proto.h"
	END USRLIB MODULE INFORMATION
*/
void breakS(int d, int g, int s, int b, int g1, int g2, double vdsmin, double vdsmax, double vdstep, double Ith, double plc_val, double Delay, double vg, double vs, double vb, char mode, char sd, double Range, double * bvdss);


/* USRLIB MODULE INFORMATION

	MODULE NAME: breakv
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 17
	ARGUMENTS:
		d,	int,	Input,	-1,	,	
		g,	int,	Input,	-1,	,	
		s,	int,	Input,	-1,	,	
		b,	int,	Input,	-1,	,	
		vdsmin,	double,	Input,	0,	-100,	100
		vdsmax,	double,	Input,	40,	-200,	200
		vdstep,	double,	Input,	1,	-3,	3
		Ith,	double,	Input,	1e-6,	,	
		plc_val,	double,	Input,	1,	0,	10
		Delay,	double,	Input,	0.001,	0,	0.3
		vg,	double,	Input,	0,	-60,	60
		vs,	double,	Input,	0,	-40,	40
		vb,	double,	Input,	0,	-40,	40
		mode,	char,	Input,	,	,	
		sd,	char,	Input,	'd',	,	
		Range,	double,	Input,	0.1,	-1,	1
		bvdss,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <stdlib.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#include <ksox_def.h>
#include <kui_proto.h>
#include <ktxe_proto.h>
#include "LBC5_proto.h"
	END USRLIB MODULE INFORMATION
*/
void breakv(int d, int g, int s, int b, double vdsmin, double vdsmax, double vdstep, double Ith, double plc_val, double Delay, double vg, double vs, double vb, char mode, char sd, double Range, double * bvdss);


/* USRLIB MODULE INFORMATION

	MODULE NAME: BREAKV_dioS
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 17
	ARGUMENTS:
		Devname,	char *,	Input,	"CEO",	,	
		E,	int,	Input,	-1,	,	
		B,	int,	Input,	-1,	,	
		C,	int,	Input,	-1,	,	
		Sub,	int,	Input,	-1,	,	
		area,	double,	Input,	1,	,	
		per,	double,	Input,	1,	,	
		Type,	char,	Input,	'n',	,	
		btype,	char,	Input,	'n',	,	
		imax,	double,	Input,	1e-6,	,	
		vmin,	double,	Input,	0,	,	
		vmax,	double,	Input,	60,	,	
		Multiplier,	double,	Input,	1,	,	
		intrange,	double,	Input,	1,	,	
		mrange,	double,	Input,	1e-8,	,	
		lorange,	double,	Input,	1e-9,	,	
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
void BREAKV_dioS(char * Devname, int E, int B, int C, int Sub, double area, double per, char Type, char btype, double imax, double vmin, double vmax, double Multiplier, double intrange, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: bvcbo_pnp
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
void bvcbo_pnp(char * devname, int col, int base, int emit, int bulk, int sub, char type, double area, double ic, double vlim, double delaytime, double vbulk, double intrange, double mrange, double lorange, double * Bvcbo);


/* USRLIB MODULE INFORMATION

	MODULE NAME: bvceo
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 11
	ARGUMENTS:
		em,	int,	Input,	,	,	
		ba,	int,	Input,	,	,	
		co,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
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
double bvceo(int em, int ba, int co, int sub, double ipgm, double vlimit, double nstep, char type, double wtime, double vbb, double * vbe);


/* USRLIB MODULE INFORMATION

	MODULE NAME: bvceo_miho2_ext
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
double  bvceo_miho2_ext(int, int, int, int, int, double, double, double, char, double, double, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: bvceo_miho2_pnp
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
double bvceo_miho2_pnp(int em, int ba, int co, int sub, int lowt, double ipgm, double vlimit, double nstep, char type, double wtime, double vbb, double * vbe);


/* USRLIB MODULE INFORMATION

	MODULE NAME: BvceoS
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 12
	ARGUMENTS:
		em,	int,	Input,	-1,	,	
		ba,	int,	Input,	-1,	,	
		co,	int,	Input,	-1,	,	
		sub,	int,	Input,	-1,	,	
		Vbe,	double,	Input,	0,	,	
		ipgm,	double,	Input,	10e-6,	,	
		vlimit,	double,	Input,	1e-4,	,	
		Vstep,	double,	Input,	0.3,	0,	1
		type,	char,	Input,	'n',	,	
		wtime,	double,	Input,	0.005,	0,	1
		Vs,	double,	Input,	0,	,	
		vbe_out,	double *,	Output,	,	,	
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
double  BvceoS(int, int, int, int, double, double, double, double, char, double, double, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: BvceoS_org
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 12
	ARGUMENTS:
		em,	int,	Input,	-1,	,	
		ba,	int,	Input,	-1,	,	
		co,	int,	Input,	-1,	,	
		sub,	int,	Input,	-1,	,	
		Vbe,	double,	Input,	0,	,	
		ipgm,	double,	Input,	10e-6,	,	
		vlimit,	double,	Input,	1e-4,	,	
		Vstep,	double,	Input,	0.3,	0,	1
		type,	char,	Input,	'n',	,	
		wtime,	double,	Input,	0.005,	0,	1
		Vs,	double,	Input,	0,	,	
		vbe_out,	double *,	Output,	,	,	
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
double  BvceoS_org(int, int, int, int, double, double, double, double, char, double, double, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: bvcex
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
double bvcex(int em, int ba, int co, int sub, int lowt, double ipgm, double vlimit, double nstep, char type, double wtime, double vbb, double * vbe);


/* USRLIB MODULE INFORMATION

	MODULE NAME: bvcex_ext
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
double  bvcex_ext(int, int, int, int, int, double, double, double, char, double, double, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: BVDS_gate
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 20
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
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
void BVDS_gate(char * devname, int drain, int gate, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double idmax, double vdsmin, double vdsmax, double vgs, double vbs, double delaytime, double intrange, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: BVDS_mos2
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
void BVDS_mos2(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double idmax, double vdsmin, double vdsmax, double vgs, double vbs, double delaytime, double intrange, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: BVDS_mos3
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 20
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		gate,	int,	Input,	-1,	,	
		drain,	int,	Input,	-1,	,	
		source,	int,	Input,	-1,	,	
		bulk,	int,	Input,	-1,	,	
		gnd1,	int,	Input,	-1,	,	
		gnd2,	int,	Input,	-1,	,	
		vdstep,	double,	Input,	1,	,	
		ration1,	double,	Input,	1,	,	
		type,	char,	Input,	,	,	
		ilimit,	double,	Input,	,	,	
		vdsmin,	double,	Input,	,	,	
		vdsmax,	double,	Input,	,	,	
		vgate,	double,	Input,	,	,	
		vbulk,	double,	Input,	,	,	
		delaytime1,	double,	Input,	,	,	
		plc_val,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		Bvdss,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <stdlib.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#include <ksox_def.h>
#include <kui_proto.h>
#include <ktxe_proto.h>
#include "LBC5_proto.h"
	END USRLIB MODULE INFORMATION
*/
void BVDS_mos3(char * devname, int gate, int drain, int source, int bulk, int gnd1, int gnd2, double vdstep, double ration1, char type, double ilimit, double vdsmin, double vdsmax, double vgate, double vbulk, double delaytime1, double plc_val, double mrange, double lorange, double * Bvdss);


/* USRLIB MODULE INFORMATION

	MODULE NAME: BVDS_mos5
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 20
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
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
void  BVDS_mos5(char *, int, int, int, int, int, int, double, double, char, double, double, double, double, double, double, double, double, double, double *);

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

	MODULE NAME: BVDS_mosS
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 20
	ARGUMENTS:
		Devname,	char *,	Input,	"BV",	,	
		Drain,	int,	Input,	-1,	,	
		Gate,	int,	Input,	-1,	,	
		Source,	int,	Input,	-1,	,	
		Bulk,	int,	Input,	-1,	,	
		G1,	int,	Input,	-1,	,	
		G2,	int,	Input,	-1,	,	
		Width,	double,	Input,	1,	,	
		Length,	double,	Input,	1,	,	
		Type,	char,	Input,	'n',	,	
		Idth,	double,	Input,	1e-6,	,	
		Vdmin,	double,	Input,	0,	,	
		Vdmax,	double,	Input,	40,	,	
		rVgs,	double,	Input,	0,	,	
		rVbulk,	double,	Input,	0,	,	
		Delaytime,	double,	Input,	0.005,	,	
		Intrange,	double,	Input,	1,	,	
		mrange,	double,	Input,	1e-8,	,	
		lorange,	double,	Input,	1e-9,	,	
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
void BVDS_mosS(char * Devname, int Drain, int Gate, int Source, int Bulk, int G1, int G2, double Width, double Length, char Type, double Idth, double Vdmin, double Vdmax, double rVgs, double rVbulk, double Delaytime, double Intrange, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: BVDS_mosX
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 19
	ARGUMENTS:
		Devname,	char *,	Input,	"EEP",	,	
		Drain,	int,	Input,	-1,	,	
		Gate,	int,	Input,	-1,	,	
		Source,	int,	Input,	-1,	,	
		Sub,	int,	Input,	-1,	,	
		Body,	int,	Input,	-1,	,	
		Gnd1,	int,	Input,	-1,	,	
		Gnd2,	int,	Input,	-1,	,	
		Vgmin,	double,	Input,	0,	,	
		Vgmax,	double,	Input,	2,	,	
		ith,	double,	Input,	1e-6,	,	
		Vgs,	double,	Input,	0.1,	,	
		Vsubst,	double,	Input,	0,	,	
		Vbody,	double,	Input,	0,	,	
		Delaytime,	double,	Input,	0.005,	,	
		Intrange,	int,	Input,	1,	,	
		lo_range,	double,	Input,	0,	,	
		mode,	char,	Input,	'x',	,	
		vstep,	double,	Input,	0.05,	,	
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
double BVDS_mosX(char * Devname, int Drain, int Gate, int Source, int Sub, int Body, int Gnd1, int Gnd2, double Vgmin, double Vgmax, double ith, double Vgs, double Vsubst, double Vbody, double Delaytime, int Intrange, double lo_range, char mode, double vstep);


/* USRLIB MODULE INFORMATION

	MODULE NAME: bvds_sweep_mos
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
void bvds_sweep_mos(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double idmax, double vdsmin, double vdsmax, double vgs, double vbs, double multiplier, double intrange, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: BVDSH_mos
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
void BVDSH_mos(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double idmax, double vdsmin, double vdsmax, double vgs, double vbs, double multiplier, double intrange, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: bvdss1_hae
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
void bvdss1_hae(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vdsmin, double vdsmax, double vdstep, double ilimit, double icomp, double intrange, double vbulk, double vgate, char sd, double mrange, double lorange, double * Bvdss, double * Id, double * Is);


/* USRLIB MODULE INFORMATION

	MODULE NAME: bvdss1_mos_hae
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
#include <ktxe_proto.h>
#include <ksox_def.h>
#include <kui_proto.h>

	END USRLIB MODULE INFORMATION
*/
void bvdss1_mos_hae(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vdsmin, double vdsmax, double vdstep, double ilimit, double icomp, double intrange, double vbulk, double vgate, char sd, double mrange, double lorange, double * Bvdss, double * Id, double * Is);


/* USRLIB MODULE INFORMATION

	MODULE NAME: bvdss2_jfet
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 22
	ARGUMENTS:
		d,	int,	Input,	,	,	
		g,	int,	Input,	,	,	
		s,	int,	Input,	,	,	
		sub,	int,	Input,	-1,	,	
		cogate,	int,	Input,	-1,	,	
		body,	int,	Input,	-1,	,	
		gnd1,	int,	Input,	-1,	,	
		gnd2,	int,	Input,	-1,	,	
		gnd3,	int,	Input,	-1,	,	
		vdsstart,	double,	Input,	0,	,	
		vdsstop,	double,	Input,	30,	,	
		vbody,	double,	Input,	0.,	,	
		vgate,	double,	Input,	0.,	,	
		vsub,	double,	Input,	0.,	,	
		micomp,	double,	Input,	0.001,	,	
		hicomp,	double,	Input,	0.01,	,	
		nstep,	int,	Input,	100,	,	
		debug,	int,	Input,	1,	,	
		sd,	char,	Input,	'd',	,	
		ipgm,	double,	Input,	10e-9,	,	
		udelay,	double,	Input,	0.01,	,	
		type,	char,	Input,	'n',	,	
	INCLUDES:
#include <math.h>		                
#include "lptdef.h"
#include <stdlib.h>
#include "PARLIB400_proto.h"
	
	END USRLIB MODULE INFORMATION
*/
double bvdss2_jfet(int d, int g, int s, int sub, int cogate, int body, int gnd1, int gnd2, int gnd3, double vdsstart, double vdsstop, double vbody, double vgate, double vsub, double micomp, double hicomp, int nstep, int debug, char sd, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: bvdss2_lkg
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 10
	ARGUMENTS:
		d,	int,	Input,	-1,	,	
		g,	int,	Input,	-1,	,	
		s,	int,	Input,	-1,	,	
		sub,	int,	Input,	-1,	,	
		vdsstart,	double,	Input,	0,	,	
		vdsstop,	double,	Input,	50,	,	
		nstep,	int,	Input,	101,	,	
		ipgm,	double,	Input,	1E-6,	,	
		udelay,	double,	Input,	0.005,	,	
		type,	char,	Input,	'n',	,	
	INCLUDES:
#include <math.h>		                
#include "lptdef.h"
#include <stdlib.h>
#include "LBC5_proto.h"
	#include <lptdef_lowercase.h>
#include <math.h>
#include <ktxe_proto.h>
#include <ksox_def.h>
#include <kui_proto.h>
#include <stdlib.h>

	END USRLIB MODULE INFORMATION
*/
double bvdss2_lkg(int d, int g, int s, int sub, double vdsstart, double vdsstop, int nstep, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: bvdss2_lkg2
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 11
	ARGUMENTS:
		devname,	char *,	Input,	"GLK",	,	
		d,	int,	Input,	-1,	,	
		g,	int,	Input,	-1,	,	
		s,	int,	Input,	-1,	,	
		sub,	int,	Input,	-1,	,	
		vdsstart,	double,	Input,	0,	,	
		vdsstop,	double,	Input,	50,	,	
		nstep,	int,	Input,	101,	,	
		ipgm,	double,	Input,	1E-6,	,	
		udelay,	double,	Input,	0.005,	,	
		type,	char,	Input,	'n',	,	
	INCLUDES:
#include <math.h>		                
#include "lptdef.h"
#include <stdlib.h>
#include "LBC5_proto.h"
	#include <lptdef_lowercase.h>
#include <math.h>
#include <ktxe_proto.h>
#include <ksox_def.h>
#include <kui_proto.h>
#include <stdlib.h>

	END USRLIB MODULE INFORMATION
*/
double  bvdss2_lkg2(char *, int, int, int, int, double, double, int, double, double, char);

/* USRLIB MODULE INFORMATION

	MODULE NAME: bvdss2_lkg3
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 16
	ARGUMENTS:
		d,	int,	Input,	-1,	,	
		g,	int,	Input,	-1,	,	
		s,	int,	Input,	-1,	,	
		sub,	int,	Input,	-1,	,	
		drain1,	int,	Input,	-1,	,	
		drain2,	int,	Input,	-1,	,	
		drain3,	int,	Input,	-1,	,	
		drain4,	int,	Input,	-1,	,	
		drain5,	int,	Input,	-1,	,	
		drain6,	int,	Input,	-1,	,	
		vdsstart,	double,	Input,	0,	,	
		vdsstop,	double,	Input,	50,	,	
		nstep,	int,	Input,	101,	,	
		ipgm,	double,	Input,	1E-6,	,	
		udelay,	double,	Input,	0.005,	,	
		type,	char,	Input,	'n',	,	
	INCLUDES:
#include <math.h>		                
#include "lptdef.h"
#include <stdlib.h>
#include "LBC5_proto.h"
	#include <lptdef_lowercase.h>
#include <math.h>
#include <ktxe_proto.h>
#include <ksox_def.h>
#include <kui_proto.h>
#include <stdlib.h>

	END USRLIB MODULE INFORMATION
*/
double bvdss2_lkg3(int d, int g, int s, int sub, int drain1, int drain2, int drain3, int drain4, int drain5, int drain6, double vdsstart, double vdsstop, int nstep, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: bvdss2_mos
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 24
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		gate,	int,	Input,	-1,	,	
		drain,	int,	Input,	-1,	,	
		source,	int,	Input,	-1,	,	
		bulk,	int,	Input,	-1,	,	
		sub,	int,	Input,	-1,	,	
		chuckcon,	int,	Input,	-1,	,	
		width,	double,	Input,	1,	,	
		length,	double,	Input,	1,	,	
		type,	char,	Input,	,	,	
		vdsmin,	double,	Input,	0,	,	
		vdsmax,	double,	Input,	40,	,	
		vdstep,	double,	Input,	0.3,	,	
		ilimit,	double,	Input,	1e-6,	,	
		icomp,	double,	Input,	0.05,	,	
		intrange,	double,	Input,	0,	,	
		vbulk,	double,	Input,	0,	,	
		vgate,	double,	Input,	0,	,	
		sd,	char,	Input,	'd',	,	
		mrange,	double,	Input,	1e-8,	,	
		lorange,	double,	Input,	1e-8,	,	
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
void bvdss2_mos(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vdsmin, double vdsmax, double vdstep, double ilimit, double icomp, double intrange, double vbulk, double vgate, char sd, double mrange, double lorange, double * Bvdss, double * Id, double * Is);


/* USRLIB MODULE INFORMATION

	MODULE NAME: bvdss4_mos
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 32
	ARGUMENTS:
		devname,	char *,	Input,	"BVD",	,	
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		bulk,	int,	Input,	-1,	,	
		sub,	int,	Input,	-1,	,	
		chuckcon,	int,	Input,	-1,	,	
		gnd1,	int,	Input,	-1,	,	
		gnd2,	int,	Input,	-1,	,	
		width,	double,	Input,	1,	,	
		length,	double,	Input,	1,	,	
		type,	char,	Input,	'n',	,	
		vdsmin,	double,	Input,	0,	,	
		vdsmax,	double,	Input,	30,	,	
		vdstep,	double,	Input,	0.1,	,	
		i_target,	double,	Input,	1e-6,	,	
		f_target,	double,	Input,	1e-9,	,	
		icomp,	double,	Input,	1e-4,	,	
		intrange,	double,	Input,	0,	,	
		delaytime,	double,	Input,	0.01,	,	
		debug,	int,	Input,	1,	,	
		mode,	char,	Input,	'p',	,	
		pmode,	char,	Input,	'p',	,	
		vbulk,	double,	Input,	0,	,	
		vgate,	double,	Input,	0,	,	
		sd,	char,	Input,	'd',	,	
		mrange,	double,	Input,	1e-8,	,	
		lorange,	double,	Input,	1e-8,	,	
		f_bv,	double *,	Output,	,	,	
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
void bvdss4_mos(char * devname, int drain, int gate, int source, int bulk, int sub, int chuckcon, int gnd1, int gnd2, double width, double length, char type, double vdsmin, double vdsmax, double vdstep, double i_target, double f_target, double icomp, double intrange, double delaytime, int debug, char mode, char pmode, double vbulk, double vgate, char sd, double mrange, double lorange, double * f_bv, double * Bvdss, double * Id, double * Is);


/* USRLIB MODULE INFORMATION

	MODULE NAME: bvdss5_mos
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 24
	ARGUMENTS:
		devname,	char *,	Input,	"bv",	,	
		gate,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		bulk,	int,	Input,	-1,	,	
		sub,	int,	Input,	-1,	,	
		chuckcon,	int,	Input,	-1,	,	
		width,	double,	Input,	1,	,	
		length,	double,	Input,	1,	,	
		type,	char,	Input,	'n',	,	
		vdsmin,	double,	Input,	0,	,	
		vdsmax,	double,	Input,	50,	,	
		vdstep,	double,	Input,	0.3,	,	
		ilimit,	double,	Input,	1e-6,	,	
		icomp,	double,	Input,	0.5,	,	
		intrange,	double,	Input,	0,	,	
		vbulk,	double,	Input,	0,	,	
		vgate,	double,	Input,	0,	,	
		sd,	char,	Input,	'd',	,	
		rdelay_time,	double,	Input,	0.001,	,	
		mode,	char,	Input,	's',	,	
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
void bvdss5_mos(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vdsmin, double vdsmax, double vdstep, double ilimit, double icomp, double intrange, double vbulk, double vgate, char sd, double rdelay_time, char mode, double * Bvdss, double * Id, double * Is);


/* USRLIB MODULE INFORMATION

	MODULE NAME: bvdss6
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
double bvdss6(int d, int g, int s, int sub, double vdsstart, double vdsstop, int nstep, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: bvdss7
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 18
	ARGUMENTS:
		dev,	char *,	Input,	"bv",	,	
		d,	int,	Input,	-1,	,	
		g,	int,	Input,	-1,	,	
		s,	int,	Input,	-1,	,	
		sub,	int,	Input,	-1,	,	
		g1,	int,	Input,	-1,	,	
		g2,	int,	Input,	-1,	,	
		g3,	int,	Input,	-1,	,	
		vdsstart,	double,	Input,	0,	,	
		vdsstop,	double,	Input,	50,	,	
		vgs,	double,	Input,	0,	,	
		vsub,	double,	Input,	0,	,	
		nstep,	int,	Input,	151,	,	
		ipgm,	double,	Input,	1E-8,	,	
		udelay,	double,	Input,	0.01,	,	
		type,	char,	Input,	'n',	,	
		integ,	int,	Input,	0,	,	
		irange,	double,	Input,	0,	,	
	INCLUDES:
#include <stdlib.h>
#include <stdio.h>
#include <math.h>		    
#include <lptdef_lowercase.h>
#include <ktxe_proto.h>
#include <ksox_def.h>
#include <kui_proto.h>
#include <COM_usrlib.h>
#include <par_util.h>
#include <lptdef.h>

	END USRLIB MODULE INFORMATION
*/
double  bvdss7(char *, int, int, int, int, int, int, int, double, double, double, double, int, double, double, char, int, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: bvdss9
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
void bvdss9(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vdsmin, double vdsmax, double vdstep, double ilimit, double icomp, double intrange, double vbulk, double vgate, char sd, double mrange, double lorange, double * Bvdss, double * Id, double * Is);


/* USRLIB MODULE INFORMATION

	MODULE NAME: bvdss_ext
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 11
	ARGUMENTS:
		d,	int,	Input,	,	,	
		g,	int,	Input,	,	,	
		s,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		body,	int,	Input,	,	,	
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
double  bvdss_ext(int, int, int, int, int, double, double, int, double, double, char);

/* USRLIB MODULE INFORMATION

	MODULE NAME: bvdss_hae
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 23
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		gate,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		bulk,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		bgate,	int,	Input,	,	,	
		width,	double,	Input,	,	,	
		length,	double,	Input,	,	,	
		vdsmin,	double,	Input,	,	,	
		vdsmax,	double,	Input,	,	,	
		vdstep,	double,	Input,	,	,	
		ilimit,	double,	Input,	,	,	
		icomp,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		vbulk,	double,	Input,	,	,	
		vgate,	double,	Input,	,	,	
		sd,	char,	Input,	,	,	
		Bvdss,	double *,	Output,	,	,	
		Id,	double *,	Output,	,	,	
		Is,	double *,	Output,	,	,	
		count1,	int,	Input,	,	,	
		type,	char,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <stdlib.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void bvdss_hae(char * devname, int gate, int drain, int source, int bulk, int sub, int bgate, double width, double length, double vdsmin, double vdsmax, double vdstep, double ilimit, double icomp, double intrange, double vbulk, double vgate, char sd, double * Bvdss, double * Id, double * Is, int count1, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: bvdss_sch
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 22
	ARGUMENTS:
		anode,	int,	Input,	,	,	
		cathode,	int,	Input,	,	,	
		subst,	int,	Input,	-1,	,	
		chuckcon,	int,	Input,	-1,	,	
		bulk,	int,	Input,	-1,	,	
		gnd1,	int,	Input,	-1,	,	
		gnd2,	int,	Input,	-1,	,	
		ano_width,	double,	Input,	1,	,	
		ano_length,	double,	Input,	1,	,	
		vforce_subst,	double,	Input,	0,	,	
		vforce_cathode,	double,	Input,	0,	,	
		vstart,	double,	Input,	0,	,	
		vstop,	double,	Input,	50,	,	
		nstep,	int,	Input,	,	,	
		fipgm,	double,	Input,	1e-6,	,	
		ipgm,	double,	Input,	100e-6,	,	
		udelay,	double,	Input,	0.01,	,	
		type,	char,	Input,	,	,	
		sweep,	char,	Input,	,	,	
		spot,	char,	Input,	,	,	
		first_volt,	double *,	Output,	,	,	
		sub_max,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <ktxe_proto.h>
#include <ksox_def.h>
#include <kui_proto.h>
#include <stdlib.h>

	END USRLIB MODULE INFORMATION
*/
double bvdss_sch(int anode, int cathode, int subst, int chuckcon, int bulk, int gnd1, int gnd2, double ano_width, double ano_length, double vforce_subst, double vforce_cathode, double vstart, double vstop, int nstep, double fipgm, double ipgm, double udelay, char type, char sweep, char spot, double * first_volt, double * sub_max);


/* USRLIB MODULE INFORMATION

	MODULE NAME: bvdss_sch_1
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 22
	ARGUMENTS:
		anode,	int,	Input,	,	,	
		cathode,	int,	Input,	,	,	
		subst,	int,	Input,	-1,	,	
		chuckcon,	int,	Input,	-1,	,	
		bulk,	int,	Input,	-1,	,	
		gnd1,	int,	Input,	-1,	,	
		gnd2,	int,	Input,	-1,	,	
		ano_width,	double,	Input,	1,	,	
		ano_length,	double,	Input,	1,	,	
		vforce_subst,	double,	Input,	0,	,	
		vforce_cathode,	double,	Input,	0,	,	
		vstart,	double,	Input,	0,	,	
		vstop,	double,	Input,	50,	,	
		nstep,	int,	Input,	,	,	
		fipgm,	double,	Input,	1e-6,	,	
		ipgm,	double,	Input,	100e-6,	,	
		udelay,	double,	Input,	0.01,	,	
		type,	char,	Input,	,	,	
		mode,	char,	Input,	,	,	
		flag,	char,	Input,	,	,	
		first_volt,	double *,	Output,	,	,	
		sub_max,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <ktxe_proto.h>
#include <ksox_def.h>
#include <kui_proto.h>
	END USRLIB MODULE INFORMATION
*/
double bvdss_sch_1(int anode, int cathode, int subst, int chuckcon, int bulk, int gnd1, int gnd2, double ano_width, double ano_length, double vforce_subst, double vforce_cathode, double vstart, double vstop, int nstep, double fipgm, double ipgm, double udelay, char type, char mode, char flag, double * first_volt, double * sub_max);


/* USRLIB MODULE INFORMATION

	MODULE NAME: bvdss_sch_sub
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 22
	ARGUMENTS:
		anode,	int,	Input,	-1,	,	
		cathode,	int,	Input,	-1,	,	
		subst,	int,	Input,	-1,	,	
		chuckcon,	int,	Input,	-1,	,	
		bulk,	int,	Input,	-1,	,	
		gnd1,	int,	Input,	-1,	,	
		gnd2,	int,	Input,	-1,	,	
		w,	double,	Input,	1,	,	
		l,	double,	Input,	1,	,	
		vsb,	double,	Input,	0,	,	
		vca,	double,	Input,	0,	,	
		vstart,	double,	Input,	0,	,	
		vstop,	double,	Input,	50,	,	
		nstep,	int,	Input,	100,	,	
		fipgm,	double,	Input,	1e-6,	,	
		ipgm,	double,	Input,	100e-6,	,	
		udelay,	double,	Input,	0.01,	,	
		type,	char,	Input,	'n',	,	
		sweep,	char,	Input,	's',	,	
		spot,	char,	Input,	'x',	,	
		first_volt,	double *,	Output,	,	,	
		sub_max,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <ktxe_proto.h>
#include <ksox_def.h>
#include <kui_proto.h>
#include <stdlib.h>

	END USRLIB MODULE INFORMATION
*/
double bvdss_sch_sub(int anode, int cathode, int subst, int chuckcon, int bulk, int gnd1, int gnd2, double w, double l, double vsb, double vca, double vstart, double vstop, int nstep, double fipgm, double ipgm, double udelay, char type, char sweep, char spot, double * first_volt, double * sub_max);


/* USRLIB MODULE INFORMATION

	MODULE NAME: bvdss_sch_tmp
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 22
	ARGUMENTS:
		anode,	int,	Input,	,	,	
		cathode,	int,	Input,	,	,	
		subst,	int,	Input,	-1,	,	
		chuckcon,	int,	Input,	-1,	,	
		bulk,	int,	Input,	-1,	,	
		gnd1,	int,	Input,	-1,	,	
		gnd2,	int,	Input,	-1,	,	
		ano_width,	double,	Input,	1,	,	
		ano_length,	double,	Input,	1,	,	
		vforce_subst,	double,	Input,	0,	,	
		vforce_cathode,	double,	Input,	0,	,	
		vstart,	double,	Input,	0,	,	
		vstop,	double,	Input,	50,	,	
		nstep,	int,	Input,	,	,	
		fipgm,	double,	Input,	1e-6,	,	
		ipgm,	double,	Input,	100e-6,	,	
		udelay,	double,	Input,	0.01,	,	
		type,	char,	Input,	,	,	
		mode,	char,	Input,	,	,	
		flag,	char,	Input,	,	,	
		first_volt,	double *,	Output,	,	,	
		sub_max,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <ktxe_proto.h>
#include <ksox_def.h>
#include <kui_proto.h>
	END USRLIB MODULE INFORMATION
*/
double bvdss_sch_tmp(int anode, int cathode, int subst, int chuckcon, int bulk, int gnd1, int gnd2, double ano_width, double ano_length, double vforce_subst, double vforce_cathode, double vstart, double vstop, int nstep, double fipgm, double ipgm, double udelay, char type, char mode, char flag, double * first_volt, double * sub_max);


/* USRLIB MODULE INFORMATION

	MODULE NAME: bvdss_vgvsb_ext
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 13
	ARGUMENTS:
		d,	int,	Input,	,	,	
		g,	int,	Input,	,	,	
		s,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		body,	int,	Input,	,	,	
		vgs,	double,	Input,	,	,	
		vsub,	double,	Input,	,	,	
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
double  bvdss_vgvsb_ext(int, int, int, int, int, double, double, double, double, int, double, double, char);

/* USRLIB MODULE INFORMATION

	MODULE NAME: bvdssgh
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 11
	ARGUMENTS:
		DRAIN,	int,	Input,	,	,	
		GATE,	int,	Input,	,	,	
		SOURCE,	int,	Input,	,	,	
		SUBST,	int,	Input,	,	,	
		VDSSTART,	double,	Input,	,	,	
		VDSSTOP,	double,	Input,	,	,	
		VG,	double,	Input,	,	,	
		NSTEP,	int,	Input,	,	,	
		IPGM,	double,	Input,	,	,	
		UDELAY,	double,	Input,	,	,	
		TYPE,	char,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <usrlib_proto.h>
	END USRLIB MODULE INFORMATION
*/
double bvdssgh(int DRAIN, int GATE, int SOURCE, int SUBST, double VDSSTART, double VDSSTOP, double VG, int NSTEP, double IPGM, double UDELAY, char TYPE);


/* USRLIB MODULE INFORMATION

	MODULE NAME: BvdsSss
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 22
	ARGUMENTS:
		Devname,	char *,	Input,	"bvdss",	,	
		Drain,	int,	Input,	-1,	,	
		Gate,	int,	Input,	-1,	,	
		Source,	int,	Input,	-1,	,	
		Sub,	int,	Input,	-1,	,	
		Body,	int,	Input,	-1,	,	
		VgDmin,	double,	Input,	0,	,	
		VgDmax,	double,	Input,	2,	,	
		VgDstep,	double,	Input,	0.05,	,	
		Ith0,	double,	Input,	1e-9,	,	
		Ith1,	double,	Input,	100e-9,	,	
		Ith,	double,	Input,	1e-6,	,	
		VgD,	double,	Input,	0.1,	,	
		Vsubst,	double,	Input,	0,	,	
		Vbody,	double,	Input,	0,	,	
		rDelay,	double,	Input,	0.005,	0,	1
		iNtrange,	int,	Input,	1,	0,	10
		LoR,	double,	Input,	0,	,	
		mO,	char,	Input,	'x',	,	
		dG,	char,	Input,	'D',	,	
		Ith0bv,	double *,	Output,	,	,	
		Ith1bv,	double *,	Output,	,	,	
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
#include <stdlib.h>    
	END USRLIB MODULE INFORMATION
*/
double BvdsSss(char * Devname, int Drain, int Gate, int Source, int Sub, int Body, double VgDmin, double VgDmax, double VgDstep, double Ith0, double Ith1, double Ith, double VgD, double Vsubst, double Vbody, double rDelay, int iNtrange, double LoR, char mO, char dG, double * Ith0bv, double * Ith1bv);


/* USRLIB MODULE INFORMATION

	MODULE NAME: calc_offset
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 7
	ARGUMENTS:
		input,	double,	Input,	,	,	
		Replace_h,	double,	Input,	,	,	
		Replace_l,	double,	Input,	,	,	
		High_limit,	double,	Input,	,	,	
		Low_limit,	double,	Input,	,	,	
		Multiplier,	double,	Input,	,	,	
		output,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <unistd.h>
#include <netdb.h>
	END USRLIB MODULE INFORMATION
*/
double  calc_offset(double, double, double, double, double, double, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: calc_tester
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 7
	ARGUMENTS:
		input,	double,	Input,	,	,	
		ampt11,	double,	Input,	,	,	
		ampt12,	double,	Input,	,	,	
		ampt13,	double,	Input,	,	,	
		ampt14,	double,	Input,	,	,	
		ampt16,	double,	Input,	,	,	
		output,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <unistd.h>
#include <netdb.h>
	END USRLIB MODULE INFORMATION
*/
double  calc_tester(double, double, double, double, double, double, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: cap_2spo_cap10
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
		vacc,	double,	Input,	,	,	
		freq,	double,	Input,	,	,	
		sig,	double,	Input,	,	,	
		stray,	double,	Input,	,	,	
		integ,	int,	Input,	,	,	
		ddebug,	int,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		ca,	double *,	Output,	,	,	
		ta,	double *,	Output,	,	,	
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
extern int HP4284LibInit;
	END USRLIB MODULE INFORMATION
*/
void cap_2spo_cap10(char * devname, int hi, int lo, int sub, int chuckcon, double area, double per, char tty, char bty, double vacc, double freq, double sig, double stray, int integ, int ddebug, double mrange, double lorange, double * ca, double * ta);


/* USRLIB MODULE INFORMATION

	MODULE NAME: cap_2spo_cap9
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
extern int HP4284LibInit;
	END USRLIB MODULE INFORMATION
*/
void cap_2spo_cap9(char * devname, int hi, int lo, int sub, int chuckcon, double area, double per, char tty, char bty, double vacc, double vinv, double freq, double sig, double stray, int integ, int ddebug, double mrange, double lorange, double * ca, double * ta, double * ci, double * ti);


/* USRLIB MODULE INFORMATION

	MODULE NAME: cap_2spo_cap_opt3
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
extern int HP4284LibInit;
	END USRLIB MODULE INFORMATION
*/
void  cap_2spo_cap_opt3(char *, int, int, int, int, double, double, char, char, double, double, double, double, double, int, int, double, double, double *, double *, double *, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: cap_2spo_cap_opt3_or
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
extern int HP4284LibInit;
	END USRLIB MODULE INFORMATION
*/
void  cap_2spo_cap_opt3_or(char *, int, int, int, int, double, double, char, char, double, double, double, double, double, int, int, double, double, double *, double *, double *, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: cap_2spo_cap_opt3_org
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
extern int HP4284LibInit;
	END USRLIB MODULE INFORMATION
*/
void cap_2spo_cap_opt3_org(char * devname, int hi, int lo, int sub, int chuckcon, double area, double per, char tty, char bty, double vacc, double vinv, double freq, double sig, double stray, int integ, int ddebug, double mrange, double lorange, double * ca, double * ta, double * ci, double * ti);


/* USRLIB MODULE INFORMATION

	MODULE NAME: cap_2spo_cap_optS
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
extern int HP4284LibInit;
	END USRLIB MODULE INFORMATION
*/
void cap_2spo_cap_optS(char * devname, int hi, int lo, int sub, int chuckcon, double area, double per, char tty, char bty, double vacc, double vinv, double freq, double sig, double stray, int integ, int ddebug, double mrange, double lorange, double * ca, double * ta, double * ci, double * ti);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ConstantVt
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 24
	ARGUMENTS:
		Devname,	char *,	Input,	"dev",	,	
		D,	int,	Input,	-1,	,	
		G,	int,	Input,	-1,	,	
		S,	int,	Input,	-1,	,	
		B,	int,	Input,	-1,	,	
		G1,	int,	Input,	-1,	,	
		G2,	int,	Input,	-1,	,	
		Vgmin,	double,	Input,	0,	,	
		Vgmax,	double,	Input,	2,	,	
		Vgstep,	double,	Input,	0.01,	,	
		Ith,	double,	Input,	0.1e-6,	,	
		IthSS,	double,	Input,	0,	,	
		IthSH,	double,	Input,	0,	,	
		Vd,	double,	Input,	0.1,	,	
		VdSH,	double,	Input,	0,	,	
		Vs,	double,	Input,	0,	-200,	200
		Vb,	double,	Input,	0,	-200,	200
		Intrange,	double,	Input,	1,	0,	10
		Num,	int,	Input,	10,	0,	16
		Delaytime,	double,	Input,	0.01,	0,	0.5
		Multiplier,	int,	Input,	0,	,	
		Type,	char,	Input,	'n',	,	
		SLPS,	double *,	Output,	,	,	
		SFTS,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <stdlib.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#include <tidp1.h>
#include <ksox_def.h>

#define MAX(a,b)	((a)>(b) ? (a) : (b))
#define CDELAY  125.E-12
#define ILEAK   20.E-12 
	END USRLIB MODULE INFORMATION
*/
double ConstantVt(char * Devname, int D, int G, int S, int B, int G1, int G2, double Vgmin, double Vgmax, double Vgstep, double Ith, double IthSS, double IthSH, double Vd, double VdSH, double Vs, double Vb, double Intrange, int Num, double Delaytime, int Multiplier, char Type, double * SLPS, double * SFTS);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Dibeta
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 10
	ARGUMENTS:
		em,	int,	Input,	,	,	
		base,	int,	Input,	,	,	
		coll,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		ic,	double,	Input,	,	,	
		vce,	double,	Input,	,	,	
		vb,	double,	Input,	,	,	
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
double  Dibeta(int, int, int, int, double, double, double, double *, double *, char);

/* USRLIB MODULE INFORMATION

	MODULE NAME: E1pL_smu
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 21
	ARGUMENTS:
		CG,	int,	Input,	-1,	,	
		DRAIN,	int,	Input,	-1,	,	
		SOURCE,	int,	Input,	-1,	,	
		TG,	int,	Input,	-1,	,	
		LO1,	int,	Input,	-1,	,	
		LO2,	int,	Input,	-1,	,	
		GND1,	int,	Input,	-1,	,	
		GND2,	int,	Input,	-1,	,	
		Loop,	long,	Input,	1,	,	
		PCG_V,	double,	Input,	17,	,	
		PCG_TIME,	double,	Input,	10e-3,	,	
		ECG_V,	double,	Input,	5,	,	
		ECG_TIME,	double,	Input,	10e-3,	,	
		PTG_V,	double,	Input,	5,	,	
		PTG_TIME,	double,	Input,	10e-3,	,	
		ETG_V,	double,	Input,	0,	,	
		ETG_TIME,	double,	Input,	0,	,	
		DRAIN_V,	int,	Input,	0,	,	
		DRAIN_TIME,	double,	Input,	0,	,	
		MOD,	char *,	Input,	"M1",	,	
		DUT,	char *,	Input,	"D1",	,	
	INCLUDES:
#include <math.h>	                
#include "lptdef.h"
#include "PARLIB400_proto.h"
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include "COM_usrlib.h"
#define MAX(a,b)	((a)>(b) ? (a) : (b))
#include <par_util.h>
static char const vcid[] ="$Id: Local $";
	END USRLIB MODULE INFORMATION
*/
double E1pL_smu(int CG, int DRAIN, int SOURCE, int TG, int LO1, int LO2, int GND1, int GND2, long Loop, double PCG_V, double PCG_TIME, double ECG_V, double ECG_TIME, double PTG_V, double PTG_TIME, double ETG_V, double ETG_TIME, int DRAIN_V, double DRAIN_TIME, char * MOD, char * DUT);


/* USRLIB MODULE INFORMATION

	MODULE NAME: early_jfet
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 23
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		bgate,	int,	Input,	,	,	
		gnd1,	int,	Input,	,	,	
		gnd2,	int,	Input,	,	,	
		vstart,	double,	Input,	,	,	
		vstop,	double,	Input,	,	,	
		npts,	int,	Input,	,	,	
		vgate,	double,	Input,	,	,	
		vsub,	double,	Input,	,	,	
		vbgate,	double,	Input,	,	,	
		igcomp,	double,	Input,	,	,	
		iscomp,	double,	Input,	,	,	
		ibacomp,	double,	Input,	,	,	
		debug,	int,	Input,	,	,	
		type,	char,	Input,	,	,	
		flag,	char,	Input,	,	,	
		slope,	double *,	Output,	,	,	
		iflag,	int *,	Output,	,	,	
		r,	double *,	Output,	,	,	
		early,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#include <ksox_def.h>
#include <kui_proto.h>
	END USRLIB MODULE INFORMATION
*/
void early_jfet(int drain, int gate, int source, int sub, int bgate, int gnd1, int gnd2, double vstart, double vstop, int npts, double vgate, double vsub, double vbgate, double igcomp, double iscomp, double ibacomp, int debug, char type, char flag, double * slope, int * iflag, double * r, double * early);


/* USRLIB MODULE INFORMATION

	MODULE NAME: EepromVtS
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 22
	ARGUMENTS:
		Devname,	char *,	Input,	"DEV",	,	
		D,	int,	Input,	-1,	,	
		CG,	int,	Input,	-1,	,	
		TG,	int,	Input,	-1,	,	
		S,	int,	Input,	-1,	,	
		B,	int,	Input,	-1,	,	
		ISO,	int,	Input,	-1,	,	
		PSUB,	int,	Input,	-1,	,	
		G1,	int,	Input,	-1,	,	
		G2,	int,	Input,	-1,	,	
		VCgmin,	double,	Input,	0,	,	
		VCgmax,	double,	Input,	7,	,	
		VCgstep,	double,	Input,	0.01,	,	
		Ith,	double,	Input,	1e-6,	,	
		Vd,	double,	Input,	0.1,	,	
		VTg,	double,	Input,	0,	-200,	200
		Vb,	double,	Input,	0,	-200,	200
		Intrange,	double,	Input,	1,	0,	10
		Num,	int,	Input,	10,	0,	16
		Delaytime,	double,	Input,	0.01,	0,	0.5
		Mode,	char,	Input,	'n',	,	
		Type,	char,	Input,	'n',	,	
	INCLUDES:
#include <stdio.h>
#include <stdlib.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#include <tidp1.h>
#include <ksox_def.h>
#define MAX(a,b)	((a)>(b) ? (a) : (b))
#define CDELAY  125.E-12
#define ILEAK   20.E-12 
	END USRLIB MODULE INFORMATION
*/
double EepromVtS(char * Devname, int D, int CG, int TG, int S, int B, int ISO, int PSUB, int G1, int G2, double VCgmin, double VCgmax, double VCgstep, double Ith, double Vd, double VTg, double Vb, double Intrange, int Num, double Delaytime, char Mode, char Type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: erase_eeprom8
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 10
	ARGUMENTS:
		CG,	int,	Input,	-1,	,	
		D,	int,	Input,	-1,	,	
		S,	int,	Input,	-1,	,	
		TG,	int,	Input,	-1,	,	
		ISO,	int,	Input,	-1,	,	
		PSUB,	int,	Input,	-1,	,	
		G1,	int,	Input,	-1,	,	
		G2,	int,	Input,	-1,	,	
		VTG,	double,	Input,	17,	,	
		TE,	double,	Input,	10,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "COM_usrlib.h"
	END USRLIB MODULE INFORMATION
*/
void erase_eeprom8(int CG, int D, int S, int TG, int ISO, int PSUB, int G1, int G2, double VTG, double TE);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ev5
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 13
	ARGUMENTS:
		e,	int,	Input,	-1,	,	
		b,	int,	Input,	-1,	,	
		c,	int,	Input,	-1,	,	
		s,	int,	Input,	-1,	,	
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
#include <par_util.h>
#include <ksox_def.h>
#include <kui_proto.h>
	END USRLIB MODULE INFORMATION
*/
void ev5(int e, int b, int c, int s, double ibe, double vstart, double vstop, int npts, double vsub, double * slope, int * iflag, double * r, double * early);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ev_jfet
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 21
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		bgate,	int,	Input,	,	,	
		vstart,	double,	Input,	,	,	
		vstop,	double,	Input,	,	,	
		npts,	int,	Input,	,	,	
		vgate,	double,	Input,	,	,	
		vsub,	double,	Input,	,	,	
		vbgate,	double,	Input,	,	,	
		igcomp,	double,	Input,	,	,	
		iscomp,	double,	Input,	,	,	
		ibacomp,	double,	Input,	,	,	
		debug,	int,	Input,	,	,	
		type,	char,	Input,	,	,	
		flag,	char,	Input,	,	,	
		slope,	double *,	Output,	,	,	
		iflag,	int *,	Output,	,	,	
		r,	double *,	Output,	,	,	
		early,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#include <ksox_def.h>
#include <kui_proto.h>
	END USRLIB MODULE INFORMATION
*/
void ev_jfet(int drain, int gate, int source, int sub, int bgate, double vstart, double vstop, int npts, double vgate, double vsub, double vbgate, double igcomp, double iscomp, double ibacomp, int debug, char type, char flag, double * slope, int * iflag, double * r, double * early);


/* USRLIB MODULE INFORMATION

	MODULE NAME: evS
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
void evS(int e, int b, int c, int s, double ibe, double vstart, double vstop, int npts, double vsub, double * slope, int * iflag, double * r, double * early);


/* USRLIB MODULE INFORMATION

	MODULE NAME: expo
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 17
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
		lsf,	char,	Input,	,	,	
		sweep,	char,	Input,	,	,	
		vtmax,	double *,	Output,	,	,	
		slope,	double *,	Output,	,	,	
		kflag,	double *,	Output,	,	,	
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
	END USRLIB MODULE INFORMATION
*/
double expo(int drain, int gate, int source, int subst, double w, double l, double vlow, double vhigh, double vds, double vbs, double ithr, double vstep, char lsf, char sweep, double * vtmax, double * slope, double * kflag);


/* USRLIB MODULE INFORMATION

	MODULE NAME: gd3id
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 22
	ARGUMENTS:
		Devname,	char *,	Input,	"bvdss",	,	
		Drain,	int,	Input,	-1,	,	
		Gate,	int,	Input,	-1,	,	
		Source,	int,	Input,	-1,	,	
		Sub,	int,	Input,	-1,	,	
		Body,	int,	Input,	-1,	,	
		VgDmin,	double,	Input,	0,	,	
		VgDmax,	double,	Input,	2,	,	
		VgDstep,	double,	Input,	0.05,	,	
		Ith0,	double,	Input,	1e-9,	,	
		Ith1,	double,	Input,	100e-9,	,	
		Ith,	double,	Input,	1e-6,	,	
		VgD,	double,	Input,	0.1,	,	
		Vsubst,	double,	Input,	0,	,	
		Vbody,	double,	Input,	0,	,	
		rDelay,	double,	Input,	0.005,	0,	1
		iNtrange,	int,	Input,	1,	0,	10
		LoR,	double,	Input,	0,	,	
		mO,	char,	Input,	'x',	,	
		dG,	char,	Input,	'D',	,	
		Ith0bv,	double *,	Output,	,	,	
		Ith1bv,	double *,	Output,	,	,	
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
#include <stdlib.h>    
	END USRLIB MODULE INFORMATION
*/
double gd3id(char * Devname, int Drain, int Gate, int Source, int Sub, int Body, double VgDmin, double VgDmax, double VgDstep, double Ith0, double Ith1, double Ith, double VgD, double Vsubst, double Vbody, double rDelay, int iNtrange, double LoR, char mO, char dG, double * Ith0bv, double * Ith1bv);


/* USRLIB MODULE INFORMATION

	MODULE NAME: gds
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 15
	ARGUMENTS:
		drain,	int,	Input,	-1,	,	
		gate,	int,	Input,	-1,	,	
		source,	int,	Input,	-1,	,	
		subst,	int,	Input,	-1,	,	
		grnd1,	int,	Input,	-1,	,	
		grnd2,	int,	Input,	-1,	,	
		grnd3,	int,	Input,	-1,	,	
		vgs,	double,	Input,	0.7,	,	
		vds1,	double,	Input,	3,	,	
		vds2,	double,	Input,	4,	,	
		vbs,	double,	Input,	0,	,	
		idlim,	double,	Input,	1e-2,	,	
		lo_range,	char,	Input,	'y',	,	
		width,	double,	Input,	1,	,	
		mult,	double,	Input,	0.2,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double gds(int drain, int gate, int source, int subst, int grnd1, int grnd2, int grnd3, double vgs, double vds1, double vds2, double vbs, double idlim, char lo_range, double width, double mult);


/* USRLIB MODULE INFORMATION

	MODULE NAME: GetTTR
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
int GetTTR(int MaxSite, char * ItemName);


/* USRLIB MODULE INFORMATION

	MODULE NAME: gg
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		gdf_name,	char *,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <ksox_def.h>
	END USRLIB MODULE INFORMATION
*/
int gg(char * gdf_name);


/* USRLIB MODULE INFORMATION

	MODULE NAME: GG
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		gdf_name,	char *,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <ksox_def.h>
	END USRLIB MODULE INFORMATION
*/
int GG(char * gdf_name);


/* USRLIB MODULE INFORMATION

	MODULE NAME: GL
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 7
	ARGUMENTS:
		delaytime,	double,	Input,	,	,	
		intrange_value,	int,	Input,	,	1,	10
		lorange_value,	double,	Input,	,	,	
		current_spec,	double,	Input,	,	,	
		vstart,	double,	Input,	,	,	
		vstop,	double,	Input,	,	,	
		steps,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <stdlib.h>
#include <string.h>
#include <time.h>
	END USRLIB MODULE INFORMATION
*/
void GL(double delaytime, int intrange_value, double lorange_value, double current_spec, double vstart, double vstop, int steps);


/* USRLIB MODULE INFORMATION

	MODULE NAME: gmlin
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
double  gmlin(int, int, int, int, double, double, double, double, double, double, double, double, int, double *, int *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: gmlin5
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
		kflag,	double *,	Output,	,	,	
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
	END USRLIB MODULE INFORMATION
*/
double  gmlin5(int, int, int, int, int, int, double, double, double, double, double, double, double, double, int, double *, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: gmlin5_ksnew
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
double  gmlin5_ksnew(int, int, int, int, int, int, double, double, double, double, double, double, double, double, int, double *, int *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: gmlin5_org
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
double gmlin5_org(int drain, int gate, int source, int body, int subst, int g1, double w, double l, double vlow, double vhigh, double vds, double vbs, double ithr, double vstep, int npts, double * slope, int * kflag);


/* USRLIB MODULE INFORMATION

	MODULE NAME: gmlin5_org140515
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
double gmlin5_org140515(int drain, int gate, int source, int body, int subst, int g1, double w, double l, double vlow, double vhigh, double vds, double vbs, double ithr, double vstep, int npts, double * slope, int * kflag);


/* USRLIB MODULE INFORMATION

	MODULE NAME: gmlin6
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
		kflag,	double *,	Output,	,	,	
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
double  gmlin6(int, int, int, int, int, int, double, double, double, double, double, double, double, double, int, double *, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: gmlin6_org
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
double gmlin6_org(int drain, int gate, int source, int body, int subst, int g1, double w, double l, double vlow, double vhigh, double vds, double vbs, double ithr, double vstep, int npts, double * slope, int * kflag);


/* USRLIB MODULE INFORMATION

	MODULE NAME: gmlin_ksnew
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
double  gmlin_ksnew(int, int, int, int, double, double, double, double, double, double, double, double, int, double *, int *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: gmlin_org
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
double gmlin_org(int drain, int gate, int source, int subst, double w, double l, double vlow, double vhigh, double vds, double vbs, double ithr, double vstep, int npts, double * slope, int * kflag);


/* USRLIB MODULE INFORMATION

	MODULE NAME: gmlin_org140515
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
double gmlin_org140515(int drain, int gate, int source, int subst, double w, double l, double vlow, double vhigh, double vds, double vbs, double ithr, double vstep, int npts, double * slope, int * kflag);


/* USRLIB MODULE INFORMATION

	MODULE NAME: gmlin_thre
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 21
	ARGUMENTS:
		drain,	int,	Input,	-1,	,	
		gate,	int,	Input,	-1,	,	
		source,	int,	Input,	-1,	,	
		subst,	int,	Input,	-1,	,	
		body,	int,	Input,	-1,	,	
		bulk,	int,	Input,	-1,	,	
		width,	double,	Input,	1,	,	
		length,	double,	Input,	1,	,	
		vlow,	double,	Input,	0,	,	
		vhigh,	double,	Input,	2,	,	
		vds,	double,	Input,	0.1,	,	
		vbs,	double,	Input,	0,	,	
		ithr,	double,	Input,	1E-8,	,	
		comp,	double,	Input,	30E-3,	,	
		range,	double,	Input,	1E-9,	,	
		fun,	int,	Input,	1,	,	
		delay,	double,	Input,	0.005,	,	
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
double gmlin_thre(int drain, int gate, int source, int subst, int body, int bulk, double width, double length, double vlow, double vhigh, double vds, double vbs, double ithr, double comp, double range, int fun, double delay, double vstep, int npts, double * slope, int * kflag);


/* USRLIB MODULE INFORMATION

	MODULE NAME: gmlinRR
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
double gmlinRR(int drain, int gate, int source, int subst, double w, double l, double vlow, double vhigh, double vds, double vbs, double ithr, double vstep, int npts, double * slope, int * kflag);


/* USRLIB MODULE INFORMATION

	MODULE NAME: gmlinT
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
double gmlinT(int drain, int gate, int source, int subst, double w, double l, double vlow, double vhigh, double vds, double vbs, double ithr, double vstep, int npts, double * slope, int * kflag);


/* USRLIB MODULE INFORMATION

	MODULE NAME: GmVt
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 18
	ARGUMENTS:
		Drain,	int,	Input,	-1,	,	
		Gate,	int,	Input,	-1,	,	
		Source,	int,	Input,	-1,	,	
		Body,	int,	Input,	-1,	,	
		Subst,	int,	Input,	-1,	,	
		Gnd1,	int,	Input,	-1,	,	
		Gnd2,	int,	Input,	-1,	,	
		w,	double,	Input,	10,	,	
		l,	double,	Input,	0.18,	,	
		vlow,	double,	Input,	0,	,	
		vhigh,	double,	Input,	2,	,	
		vds,	double,	Input,	0.1,	,	
		vbs,	double,	Input,	0,	,	
		ithr,	double,	Input,	1e-6,	,	
		vstep,	double,	Input,	0.05,	,	
		npts,	int,	Input,	15,	,	
		slope,	double *,	Output,	,	,	
		kflag,	int *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <lptinstr_lowercase.h>
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
double GmVt(int Drain, int Gate, int Source, int Body, int Subst, int Gnd1, int Gnd2, double w, double l, double vlow, double vhigh, double vds, double vbs, double ithr, double vstep, int npts, double * slope, int * kflag);


/* USRLIB MODULE INFORMATION

	MODULE NAME: GS
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 3
	ARGUMENTS:
		itemName,	char *,	Input,	,	,	
		slotid,	char *,	Input,	,	,	
		sampleClass,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <ksox_def.h>
#include<stdio.h>
#include<sys/types.h>
#include<dirent.h>
#include<sys/stat.h>
#include<fcntl.h>
#include<string.h>
#include<stdlib.h>
#include <ktxe_proto.h>
	END USRLIB MODULE INFORMATION
*/
int GS(char * itemName, char * slotid, int sampleClass);


/* USRLIB MODULE INFORMATION

	MODULE NAME: H_cap_2spo_cap
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 24
	ARGUMENTS:
		devname,	char *,	Input,	"cap",	,	
		hi,	int,	Input,	-1,	,	
		lo,	int,	Input,	-1,	,	
		sub,	int,	Input,	-1,	,	
		chuckcon,	int,	Input,	-1,	,	
		area,	double,	Input,	0.0003,	,	
		per,	double,	Input,	1,	,	
		tty,	char,	Input,	'n',	,	
		bty,	char,	Input,	'p',	,	
		sp,	char,	Input,	'p',	,	
		vacc,	double,	Input,	-3.3,	,	
		vinv,	double,	Input,	3.3,	,	
		vstep,	double,	Input,	0.2,	,	
		freq,	double,	Input,	100000,	,	
		sig,	double,	Input,	0.03,	,	
		stray,	double,	Input,	1,	,	
		integ,	int,	Input,	2,	,	
		ddebug,	int,	Input,	1,	,	
		mrange,	double,	Input,	1E-6,	,	
		lorange,	double,	Input,	1E-9,	,	
		ca,	double *,	Output,	,	,	
		ta,	double *,	Output,	,	,	
		ci,	double *,	Output,	,	,	
		ti,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <unistd.h>
#include <netdb.h>
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
extern int HP4284LibInit;
	END USRLIB MODULE INFORMATION
*/
void H_cap_2spo_cap(char * devname, int hi, int lo, int sub, int chuckcon, double area, double per, char tty, char bty, char sp, double vacc, double vinv, double vstep, double freq, double sig, double stray, int integ, int ddebug, double mrange, double lorange, double * ca, double * ta, double * ci, double * ti);


/* USRLIB MODULE INFORMATION

	MODULE NAME: haemil_pn2swp_vramp
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
double haemil_pn2swp_vramp(int hi, int lo, int subst, double vstart, double vstop, int nstep, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: haemilpn2swp_vramp
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
double haemilpn2swp_vramp(int hi, int lo, int subst, double vstart, double vstop, int nstep, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: i2v
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 30
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		d,	int,	Input,	-1,	,	
		g,	int,	Input,	-1,	,	
		s,	int,	Input,	-1,	,	
		b,	int,	Input,	-1,	,	
		g1,	int,	Input,	-1,	,	
		g2,	int,	Input,	-1,	,	
		g3,	int,	Input,	-1,	,	
		vdsmin,	double,	Input,	0,	,	
		vdsmax,	double,	Input,	40,	,	
		vdstep,	double,	Input,	1,	-3,	3
		Ith1,	double,	Input,	1e-8,	-1,	1
		Ith2,	double,	Input,	1e-7,	-1,	1
		Ith,	double,	Input,	1e-6,	-1,	1
		ration,	double,	Input,	0.9,	0.01,	0.9
		plc_val,	double,	Input,	1,	0,	10
		mDelay,	double,	Input,	0.001,	0,	0.3
		hDelay,	double,	Input,	0.05,	0,	0.7
		vg,	double,	Input,	0,	,	
		vs,	double,	Input,	0,	,	
		vb,	double,	Input,	0,	,	
		mode,	int,	Input,	1,	1,	5
		sd,	char,	Input,	'd',	,	
		mRange,	double,	Input,	1e-6,	-1,	1
		hRange,	double,	Input,	0.1,	-1,	1
		Id,	double *,	Output,	,	,	
		Is,	double *,	Output,	,	,	
		Ith_bv1,	double *,	Output,	,	,	
		Ith_bv2,	double *,	Output,	,	,	
		bvdss,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <stdlib.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#include <ksox_def.h>
#include <kui_proto.h>
#include <ktxe_proto.h>
#include "LBC5_proto.h"
	END USRLIB MODULE INFORMATION
*/
void i2v(char * devname, int d, int g, int s, int b, int g1, int g2, int g3, double vdsmin, double vdsmax, double vdstep, double Ith1, double Ith2, double Ith, double ration, double plc_val, double mDelay, double hDelay, double vg, double vs, double vb, int mode, char sd, double mRange, double hRange, double * Id, double * Is, double * Ith_bv1, double * Ith_bv2, double * bvdss);


/* USRLIB MODULE INFORMATION

	MODULE NAME: I2v
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 21
	ARGUMENTS:
		d,	int,	Input,	-1,	,	
		g,	int,	Input,	-1,	,	
		s,	int,	Input,	-1,	,	
		b,	int,	Input,	-1,	,	
		g1,	int,	Input,	-1,	,	
		g2,	int,	Input,	-1,	,	
		vdL,	double,	Input,	0,	,	
		vdM,	double,	Input,	20,	,	
		vdH,	double,	Input,	40,	-180,	180
		step,	double,	Input,	1,	,	
		goal,	double,	Input,	1e-6,	,	
		ra,	double,	Input,	0.01,	,	
		pv,	double,	Input,	1,	,	
		dt,	double,	Input,	0.01,	,	
		vg,	double,	Input,	0,	,	
		vs,	double,	Input,	0,	,	
		vb,	double,	Input,	0,	,	
		sd,	char,	Input,	'd',	,	
		rg,	double,	Input,	0.01,	,	
		Bvdss,	double *,	Output,	,	,	
		LKG,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <stdlib.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#include <ksox_def.h>
#include <kui_proto.h>
#include <ktxe_proto.h>
#include "LBC5_proto.h"
	END USRLIB MODULE INFORMATION
*/
void I2v(int d, int g, int s, int b, int g1, int g2, double vdL, double vdM, double vdH, double step, double goal, double ra, double pv, double dt, double vg, double vs, double vb, char sd, double rg, double * Bvdss, double * LKG);


/* USRLIB MODULE INFORMATION

	MODULE NAME: I2v_breakv
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 21
	ARGUMENTS:
		d,	int,	Input,	-1,	,	
		g,	int,	Input,	-1,	,	
		s,	int,	Input,	-1,	,	
		b,	int,	Input,	-1,	,	
		g1,	int,	Input,	-1,	,	
		g2,	int,	Input,	-1,	,	
		vdL,	double,	Input,	0,	,	
		vdM,	double,	Input,	20,	,	
		vdH,	double,	Input,	40,	-180,	180
		step,	double,	Input,	1,	,	
		goal,	double,	Input,	1e-6,	,	
		ra,	double,	Input,	0.01,	,	
		pv,	double,	Input,	1,	,	
		dt,	double,	Input,	0.01,	,	
		vg,	double,	Input,	0,	,	
		vs,	double,	Input,	0,	,	
		vb,	double,	Input,	0,	,	
		sd,	char,	Input,	'd',	,	
		rg,	double,	Input,	0.01,	,	
		Bvdss,	double *,	Output,	,	,	
		LKG,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <stdlib.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#include <ksox_def.h>
#include <kui_proto.h>
#include <ktxe_proto.h>
#include "LBC5_proto.h"
	END USRLIB MODULE INFORMATION
*/
void I2v_breakv(int d, int g, int s, int b, int g1, int g2, double vdL, double vdM, double vdH, double step, double goal, double ra, double pv, double dt, double vg, double vs, double vb, char sd, double rg, double * Bvdss, double * LKG);


/* USRLIB MODULE INFORMATION

	MODULE NAME: i2v_bv
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 20
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		gate,	int,	Input,	-1,	,	
		drain,	int,	Input,	-1,	,	
		source,	int,	Input,	-1,	,	
		bulk,	int,	Input,	-1,	,	
		gnd1,	int,	Input,	-1,	,	
		gnd2,	int,	Input,	-1,	,	
		vdstep,	double,	Input,	1,	,	
		ration,	double,	Input,	0.01,	,	
		type,	char,	Input,	,	,	
		ilimit,	double,	Input,	0.001,	,	
		vdsmin,	double,	Input,	0,	,	
		vdsmax,	double,	Input,	90,	,	
		vgate,	double,	Input,	0,	,	
		vbulk,	double,	Input,	0,	,	
		delaytime,	double,	Input,	0.0001,	,	
		plc_val,	double,	Input,	0,	,	
		mrange,	double,	Input,	0.07,	,	
		lorange,	double,	Input,	1e-8,	,	
		Bvdss,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <stdlib.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#include <ksox_def.h>
#include <kui_proto.h>
#include <ktxe_proto.h>
#include "LBC5_proto.h"
	END USRLIB MODULE INFORMATION
*/
void i2v_bv(char * devname, int gate, int drain, int source, int bulk, int gnd1, int gnd2, double vdstep, double ration, char type, double ilimit, double vdsmin, double vdsmax, double vgate, double vbulk, double delaytime, double plc_val, double mrange, double lorange, double * Bvdss);


/* USRLIB MODULE INFORMATION

	MODULE NAME: i2v_jfet
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
void i2v_jfet(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vdsmin, double vdsmax, double vdstep, double ilimit, double icomp, double intrange, double vbulk, double vgate, char sd, double mrange, double lorange, double * Bvdss, double * Id, double * Is);


/* USRLIB MODULE INFORMATION

	MODULE NAME: i2v_mos
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 27
	ARGUMENTS:
		d,	int,	Input,	-1,	,	
		g,	int,	Input,	-1,	,	
		s,	int,	Input,	-1,	,	
		b,	int,	Input,	-1,	,	
		g1,	int,	Input,	-1,	,	
		g2,	int,	Input,	-1,	,	
		g3,	int,	Input,	-1,	,	
		vdsmin,	double,	Input,	0,	-100,	100
		vdsmax,	double,	Input,	40,	-200,	200
		vdstep,	double,	Input,	1,	-3,	3
		Ith1,	double,	Input,	1e-8,	,	
		Ith2,	double,	Input,	1e-7,	,	
		Ith,	double,	Input,	1e-6,	,	
		ration,	double,	Input,	0.9,	0.01,	0.9
		plc_val,	double,	Input,	1,	0,	10
		mDelay,	double,	Input,	0.001,	0,	0.3
		hDelay,	double,	Input,	0.05,	0,	0.7
		vg,	double,	Input,	0,	-60,	60
		vs,	double,	Input,	0,	-40,	40
		vb,	double,	Input,	0,	-40,	40
		mode,	int,	Input,	1,	1,	5
		sd,	char,	Input,	'd',	,	
		mRange,	double,	Input,	1e-6,	-1,	1
		hRange,	double,	Input,	0.1,	-1,	1
		Ith_bv1,	double *,	Output,	,	,	
		Ith_bv2,	double *,	Output,	,	,	
		bvdss,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <stdlib.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#include <ksox_def.h>
#include <kui_proto.h>
#include <ktxe_proto.h>
#include "LBC5_proto.h"
	END USRLIB MODULE INFORMATION
*/
void i2v_mos(int d, int g, int s, int b, int g1, int g2, int g3, double vdsmin, double vdsmax, double vdstep, double Ith1, double Ith2, double Ith, double ration, double plc_val, double mDelay, double hDelay, double vg, double vs, double vb, int mode, char sd, double mRange, double hRange, double * Ith_bv1, double * Ith_bv2, double * bvdss);


/* USRLIB MODULE INFORMATION

	MODULE NAME: i4v
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 18
	ARGUMENTS:
		drain,	int,	Input,	-1,	,	
		gate,	int,	Input,	-1,	,	
		source,	int,	Input,	-1,	,	
		body,	int,	Input,	-1,	,	
		subst,	int,	Input,	-1,	,	
		gnd1,	int,	Input,	-1,	,	
		gnd2,	int,	Input,	-1,	,	
		plc_val,	double,	Input,	1,	,	
		ilimit,	double,	Input,	1e-6,	,	
		lo_range,	double,	Input,	1e-9,	,	
		delay_time,	double,	Input,	0.2,	,	
		vgs,	double,	Input,	5,	,	
		vds,	double,	Input,	0,	,	
		vsub,	double,	Input,	0,	,	
		vbody,	double,	Input,	0,	,	
		sweep,	char,	Input,	'p',	,	
		noise,	char,	Input,	'x',	,	
		Vstep,	double,	Input,	0.03,	,	
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
	END USRLIB MODULE INFORMATION
*/
double  i4v(int, int, int, int, int, int, int, double, double, double, double, double, double, double, double, char, char, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: i4v_all
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 22
	ARGUMENTS:
		devname,	char *,	Input,	"e2prom",	,	
		drain,	int,	Input,	-1,	,	
		gate,	int,	Input,	-1,	,	
		source,	int,	Input,	-1,	,	
		subst,	int,	Input,	-1,	,	
		g1,	int,	Input,	-1,	,	
		g2,	int,	Input,	-1,	,	
		plc_val,	double,	Input,	-1,	,	
		ilimit,	double,	Input,	1E-6,	,	
		lo_range,	double,	Input,	1E-8,	,	
		delay,	double,	Input,	0.2,	,	
		vds,	double,	Input,	0,	,	
		vgs,	double,	Input,	8,	,	
		vso,	double,	Input,	0,	,	
		vsu,	double,	Input,	0,	,	
		vdstep,	double,	Input,	0.1,	,	
		sdelay,	double,	Input,	0.005,	,	
		all_sweep,	int,	Input,	0,	,	
		sweep,	char,	Input,	's',	,	
		noise,	char,	Input,	'o',	,	
		sd,	char,	Input,	'd',	,	
		debug,	int,	Input,	1,	,	
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
	END USRLIB MODULE INFORMATION
*/
double i4v_all(char * devname, int drain, int gate, int source, int subst, int g1, int g2, double plc_val, double ilimit, double lo_range, double delay, double vds, double vgs, double vso, double vsu, double vdstep, double sdelay, int all_sweep, char sweep, char noise, char sd, int debug);


/* USRLIB MODULE INFORMATION

	MODULE NAME: i4v_bgr
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 26
	ARGUMENTS:
		drain,	int,	Input,	-1,	,	
		gate,	int,	Input,	-1,	,	
		source,	int,	Input,	-1,	,	
		body,	int,	Input,	-1,	,	
		subst,	int,	Input,	-1,	,	
		gnd1,	int,	Input,	-1,	,	
		gnd2,	int,	Input,	-1,	,	
		plc_val,	double,	Input,	1,	,	
		ilimit,	double,	Input,	1e-5,	,	
		lo_range,	double,	Input,	1e-8,	,	
		delay_time,	double,	Input,	0.2,	,	
		hold,	double,	Input,	0.1,	,	
		vgs,	double,	Input,	0,	,	
		vds,	double,	Input,	8,	,	
		vbs,	double,	Input,	0,	,	
		vbody,	double,	Input,	0,	,	
		sweep,	char,	Input,	,	,	
		noise,	char,	Input,	,	,	
		mmode,	char,	Input,	,	,	
		debug,	int,	Input,	1,	,	
		Vgate_m,	double *,	Output,	,	,	
		Vbody_m,	double *,	Output,	,	,	
		Vsub_m,	double *,	Output,	,	,	
		Ig_m,	double *,	Output,	,	,	
		Ibody_m,	double *,	Output,	,	,	
		Isub_m,	double *,	Output,	,	,	
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
	END USRLIB MODULE INFORMATION
*/
double i4v_bgr(int drain, int gate, int source, int body, int subst, int gnd1, int gnd2, double plc_val, double ilimit, double lo_range, double delay_time, double hold, double vgs, double vds, double vbs, double vbody, char sweep, char noise, char mmode, int debug, double * Vgate_m, double * Vbody_m, double * Vsub_m, double * Ig_m, double * Ibody_m, double * Isub_m);


/* USRLIB MODULE INFORMATION

	MODULE NAME: i4v_bv
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
void i4v_bv(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vdsmin, double vdsmax, double vdstep, double ilimit, double icomp, double intrange, double vbulk, double vgate, char sd, double mrange, double lorange, double * Bvdss, double * Id, double * Is);


/* USRLIB MODULE INFORMATION

	MODULE NAME: i4v_card
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 22
	ARGUMENTS:
		devname,	char *,	Input,	"LKG",	,	
		drain,	int,	Input,	-1,	,	
		gate,	int,	Input,	-1,	,	
		source,	int,	Input,	-1,	,	
		subst,	int,	Input,	-1,	,	
		gnd1,	int,	Input,	-1,	,	
		gnd2,	int,	Input,	-1,	,	
		plc_val,	double,	Input,	1,	,	
		ilimit,	double,	Input,	1e-5,	,	
		lo_range,	double,	Input,	1e-8,	,	
		delay_time,	double,	Input,	0.2,	,	
		vds,	double,	Input,	0,	,	
		vgs,	double,	Input,	8,	,	
		vso,	double,	Input,	0,	,	
		vsu,	double,	Input,	0,	,	
		vdstep,	double,	Input,	0.5,	,	
		sdelay,	double,	Input,	0.001,	,	
		sweep,	char,	Input,	'p',	,	
		mode,	char,	Input,	'l',	,	
		noise,	char,	Input,	'o',	,	
		sd,	char,	Input,	'd',	,	
		debug,	int,	Input,	1,	,	
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
	END USRLIB MODULE INFORMATION
*/
double i4v_card(char * devname, int drain, int gate, int source, int subst, int gnd1, int gnd2, double plc_val, double ilimit, double lo_range, double delay_time, double vds, double vgs, double vso, double vsu, double vdstep, double sdelay, char sweep, char mode, char noise, char sd, int debug);


/* USRLIB MODULE INFORMATION

	MODULE NAME: i4v_dd
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 21
	ARGUMENTS:
		devname,	char *,	Input,	"aaa",	,	
		drain,	int,	Input,	-1,	,	
		gate,	int,	Input,	-1,	,	
		source,	int,	Input,	-1,	,	
		body,	int,	Input,	-1,	,	
		subst,	int,	Input,	-1,	,	
		gnd1,	int,	Input,	-1,	,	
		gnd2,	int,	Input,	-1,	,	
		plc_val,	double,	Input,	1,	,	
		ilimit,	double,	Input,	1e-6,	,	
		lo_range,	double,	Input,	1e-9,	,	
		delay_time,	double,	Input,	0.2,	,	
		vgs,	double,	Input,	5,	,	
		vds,	double,	Input,	0,	,	
		vsub,	double,	Input,	0,	,	
		vbody,	double,	Input,	0,	,	
		sweep,	char,	Input,	'p',	,	
		noise,	char,	Input,	'x',	,	
		Vstep,	double,	Input,	0.03,	,	
		result_10n,	double *,	Output,	,	,	
		result_100n,	double *,	Output,	,	,	
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
	END USRLIB MODULE INFORMATION
*/
double i4v_dd(char * devname, int drain, int gate, int source, int body, int subst, int gnd1, int gnd2, double plc_val, double ilimit, double lo_range, double delay_time, double vgs, double vds, double vsub, double vbody, char sweep, char noise, double Vstep, double * result_10n, double * result_100n);


/* USRLIB MODULE INFORMATION

	MODULE NAME: i4v_e2prom
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 26
	ARGUMENTS:
		devname,	char *,	Input,	"e2prom",	,	
		drain,	int,	Input,	-1,	,	
		s_drain,	int,	Input,	-1,	,	
		s1_drain,	int,	Input,	-1,	,	
		gate,	int,	Input,	-1,	,	
		s_gate,	int,	Input,	-1,	,	
		source,	int,	Input,	-1,	,	
		subst,	int,	Input,	-1,	,	
		g1,	int,	Input,	-1,	,	
		g2,	int,	Input,	-1,	,	
		g3,	int,	Input,	-1,	,	
		plc_val,	double,	Input,	1,	,	
		ilimit,	double,	Input,	1e-5,	,	
		lo_range,	double,	Input,	1e-8,	,	
		delay_time,	double,	Input,	0.2,	,	
		vds,	double,	Input,	0,	,	
		vgs,	double,	Input,	8,	,	
		vso,	double,	Input,	0,	,	
		vsu,	double,	Input,	0,	,	
		vdstep,	double,	Input,	0.1,	,	
		sdelay,	double,	Input,	0.005,	,	
		mode,	int,	Input,	0,	,	
		sweep,	char,	Input,	's',	,	
		noise,	char,	Input,	'o',	,	
		sd,	char,	Input,	'd',	,	
		debug,	int,	Input,	1,	,	
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
	END USRLIB MODULE INFORMATION
*/
double  i4v_e2prom(char *, int, int, int, int, int, int, int, int, int, int, double, double, double, double, double, double, double, double, double, double, int, char, char, char, int);

/* USRLIB MODULE INFORMATION

	MODULE NAME: i4v_hfe
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 23
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		drain,	int,	Input,	-1,	,	
		gate,	int,	Input,	-1,	,	
		source,	int,	Input,	-1,	,	
		subst,	int,	Input,	-1,	,	
		g1,	int,	Input,	-1,	,	
		g2,	int,	Input,	-1,	,	
		g3,	int,	Input,	-1,	,	
		plc_val,	double,	Input,	1,	,	
		ilimit,	double,	Input,	1e-5,	,	
		lo_range,	double,	Input,	1e-8,	,	
		delay_time,	double,	Input,	0.2,	,	
		vds,	double,	Input,	0,	,	
		vgs,	double,	Input,	8,	,	
		vso,	double,	Input,	0,	,	
		vsu,	double,	Input,	0,	,	
		vdstep,	double,	Input,	0.5,	,	
		sdelay,	double,	Input,	0.001,	,	
		mode,	int,	Input,	0,	,	
		sweep,	char,	Input,	'p',	,	
		noise,	char,	Input,	'o',	,	
		sd,	char,	Input,	'd',	,	
		debug,	int,	Input,	1,	,	
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
	END USRLIB MODULE INFORMATION
*/
double i4v_hfe(char * devname, int drain, int gate, int source, int subst, int g1, int g2, int g3, double plc_val, double ilimit, double lo_range, double delay_time, double vds, double vgs, double vso, double vsu, double vdstep, double sdelay, int mode, char sweep, char noise, char sd, int debug);


/* USRLIB MODULE INFORMATION

	MODULE NAME: i4v_isub
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 23
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		drain,	int,	Input,	-1,	,	
		gate,	int,	Input,	-1,	,	
		source,	int,	Input,	-1,	,	
		subst,	int,	Input,	-1,	,	
		g1,	int,	Input,	-1,	,	
		g2,	int,	Input,	-1,	,	
		g3,	int,	Input,	-1,	,	
		plc_val,	double,	Input,	1,	,	
		ilimit,	double,	Input,	1e-5,	,	
		lo_range,	double,	Input,	1e-8,	,	
		delay_time,	double,	Input,	0.2,	,	
		vds,	double,	Input,	0,	,	
		vgs,	double,	Input,	8,	,	
		vso,	double,	Input,	0,	,	
		vsu,	double,	Input,	0,	,	
		vgstep,	double,	Input,	0.5,	,	
		sdelay,	double,	Input,	0.001,	,	
		mode,	int,	Input,	0,	,	
		sweep,	char,	Input,	'p',	,	
		noise,	char,	Input,	'o',	,	
		sd,	char,	Input,	'd',	,	
		debug,	int,	Input,	1,	,	
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
	END USRLIB MODULE INFORMATION
*/
double i4v_isub(char * devname, int drain, int gate, int source, int subst, int g1, int g2, int g3, double plc_val, double ilimit, double lo_range, double delay_time, double vds, double vgs, double vso, double vsu, double vgstep, double sdelay, int mode, char sweep, char noise, char sd, int debug);


/* USRLIB MODULE INFORMATION

	MODULE NAME: i4v_mos
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 23
	ARGUMENTS:
		devname,	char *,	Input,	"aa",	,	
		drain,	int,	Input,	-1,	,	
		gate,	int,	Input,	-1,	,	
		source,	int,	Input,	-1,	,	
		subst,	int,	Input,	-1,	,	
		g1,	int,	Input,	-1,	,	
		g2,	int,	Input,	-1,	,	
		g3,	int,	Input,	-1,	,	
		plc_val,	double,	Input,	1,	,	
		ilimit,	double,	Input,	1e-5,	,	
		lo_range,	double,	Input,	1e-9,	,	
		delay_time,	double,	Input,	0.2,	,	
		vds,	double,	Input,	10,	,	
		vgs,	double,	Input,	0,	,	
		vso,	double,	Input,	0,	,	
		vsu,	double,	Input,	0,	,	
		vdstep,	double,	Input,	0.3,	,	
		sdelay,	double,	Input,	0.03,	,	
		mode,	int,	Input,	0,	,	
		sweep,	char,	Input,	'p',	,	
		noise,	char,	Input,	'o',	,	
		sd,	char,	Input,	'd',	,	
		debug,	int,	Input,	1,	,	
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
	END USRLIB MODULE INFORMATION
*/
double i4v_mos(char * devname, int drain, int gate, int source, int subst, int g1, int g2, int g3, double plc_val, double ilimit, double lo_range, double delay_time, double vds, double vgs, double vso, double vsu, double vdstep, double sdelay, int mode, char sweep, char noise, char sd, int debug);


/* USRLIB MODULE INFORMATION

	MODULE NAME: i4v_org140515
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 18
	ARGUMENTS:
		drain,	int,	Input,	-1,	,	
		gate,	int,	Input,	-1,	,	
		source,	int,	Input,	-1,	,	
		body,	int,	Input,	-1,	,	
		subst,	int,	Input,	-1,	,	
		gnd1,	int,	Input,	-1,	,	
		gnd2,	int,	Input,	-1,	,	
		plc_val,	double,	Input,	1,	,	
		ilimit,	double,	Input,	1e-6,	,	
		lo_range,	double,	Input,	1e-9,	,	
		delay_time,	double,	Input,	0.2,	,	
		vgs,	double,	Input,	5,	,	
		vds,	double,	Input,	0,	,	
		vsub,	double,	Input,	0,	,	
		vbody,	double,	Input,	0,	,	
		sweep,	char,	Input,	'p',	,	
		noise,	char,	Input,	'x',	,	
		Vstep,	double,	Input,	0.03,	,	
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
	END USRLIB MODULE INFORMATION
*/
double i4v_org140515(int drain, int gate, int source, int body, int subst, int gnd1, int gnd2, double plc_val, double ilimit, double lo_range, double delay_time, double vgs, double vds, double vsub, double vbody, char sweep, char noise, double Vstep);


/* USRLIB MODULE INFORMATION

	MODULE NAME: i4v_otp
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 25
	ARGUMENTS:
		devname,	char *,	Input,	"aa",	,	
		drain,	int,	Input,	-1,	,	
		s_drain,	int,	Input,	-1,	,	
		gate,	int,	Input,	-1,	,	
		s_gate,	int,	Input,	-1,	,	
		source,	int,	Input,	-1,	,	
		subst,	int,	Input,	-1,	,	
		g1,	int,	Input,	-1,	,	
		g2,	int,	Input,	-1,	,	
		g3,	int,	Input,	-1,	,	
		plc_val,	double,	Input,	1,	,	
		ilimit,	double,	Input,	1e-5,	,	
		lo_range,	double,	Input,	1e-8,	,	
		delay_time,	double,	Input,	0.2,	,	
		vds,	double,	Input,	0,	,	
		vgs,	double,	Input,	8,	,	
		vso,	double,	Input,	0,	,	
		vsu,	double,	Input,	0,	,	
		vdstep,	double,	Input,	0.1,	,	
		sdelay,	double,	Input,	0.005,	,	
		mode,	int,	Input,	0,	,	
		sweep,	char,	Input,	's',	,	
		noise,	char,	Input,	'o',	,	
		sd,	char,	Input,	'd',	,	
		debug,	int,	Input,	1,	,	
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
	END USRLIB MODULE INFORMATION
*/
double i4v_otp(char * devname, int drain, int s_drain, int gate, int s_gate, int source, int subst, int g1, int g2, int g3, double plc_val, double ilimit, double lo_range, double delay_time, double vds, double vgs, double vso, double vsu, double vdstep, double sdelay, int mode, char sweep, char noise, char sd, int debug);


/* USRLIB MODULE INFORMATION

	MODULE NAME: i4v_sub
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 18
	ARGUMENTS:
		drain,	int,	Input,	-1,	,	
		gate,	int,	Input,	-1,	,	
		source,	int,	Input,	-1,	,	
		body,	int,	Input,	-1,	,	
		subst,	int,	Input,	-1,	,	
		gnd1,	int,	Input,	-1,	,	
		gnd2,	int,	Input,	-1,	,	
		plc_val,	double,	Input,	1,	,	
		ilimit,	double,	Input,	1e-6,	,	
		lo_range,	double,	Input,	1e-9,	,	
		delay_time,	double,	Input,	0.2,	,	
		vgs,	double,	Input,	5,	,	
		vds,	double,	Input,	0,	,	
		vsub,	double,	Input,	0,	,	
		vbody,	double,	Input,	0,	,	
		sweep,	char,	Input,	'p',	,	
		noise,	char,	Input,	'x',	,	
		Vstep,	double,	Input,	0.03,	,	
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
	END USRLIB MODULE INFORMATION
*/
double i4v_sub(int drain, int gate, int source, int body, int subst, int gnd1, int gnd2, double plc_val, double ilimit, double lo_range, double delay_time, double vgs, double vds, double vsub, double vbody, char sweep, char noise, double Vstep);


/* USRLIB MODULE INFORMATION

	MODULE NAME: id9
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 7
	ARGUMENTS:
		drain,	int,	Input,	-1,	,	
		gate,	int,	Input,	-1,	,	
		source,	int,	Input,	-1,	,	
		subst,	int,	Input,	-1,	,	
		vgs,	double,	Input,	0,	,	
		vds,	double,	Input,	1.8,	,	
		vbs,	double,	Input,	0,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double  id9(int, int, int, int, double, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: id9_org170412
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 7
	ARGUMENTS:
		drain,	int,	Input,	-1,	,	
		gate,	int,	Input,	-1,	,	
		source,	int,	Input,	-1,	,	
		subst,	int,	Input,	-1,	,	
		vgs,	double,	Input,	0,	,	
		vds,	double,	Input,	1.8,	,	
		vbs,	double,	Input,	0,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double  id9_org170412(int, int, int, int, double, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: id9_org2018615
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 7
	ARGUMENTS:
		drain,	int,	Input,	-1,	,	
		gate,	int,	Input,	-1,	,	
		source,	int,	Input,	-1,	,	
		subst,	int,	Input,	-1,	,	
		vgs,	double,	Input,	0,	,	
		vds,	double,	Input,	1.8,	,	
		vbs,	double,	Input,	0,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double  id9_org2018615(int, int, int, int, double, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: idh
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
double  idh(int, int, int, int, double, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: idh2
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
double  idh2(int, int, int, int, double, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: idh2_org170412
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
double  idh2_org170412(int, int, int, int, double, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: idh3
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
double  idh3(int, int, int, int, double, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: idh4
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
double  idh4(int, int, int, int, double, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: idh5
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
double  idh5(int, int, int, int, double, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: idh_6pad
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 9
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		gnd1,	int,	Input,	,	,	
		gnd2,	int,	Input,	,	,	
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
double  idh_6pad(int, int, int, int, int, int, double, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: idh_org170412
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
double  idh_org170412(int, int, int, int, double, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: ii4v
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 21
	ARGUMENTS:
		d,	int,	Input,	-1,	,	
		g,	int,	Input,	-1,	,	
		s,	int,	Input,	-1,	,	
		b,	int,	Input,	-1,	,	
		g1,	int,	Input,	-1,	,	
		g2,	int,	Input,	-1,	,	
		vdL,	double,	Input,	0,	,	
		vdM,	double,	Input,	20,	,	
		vdH,	double,	Input,	40,	-180,	180
		step,	double,	Input,	1,	,	
		goal,	double,	Input,	1e-6,	,	
		ra,	double,	Input,	0.01,	,	
		pv,	double,	Input,	1,	,	
		dt,	double,	Input,	0.01,	,	
		vg,	double,	Input,	0,	,	
		vs,	double,	Input,	0,	,	
		vb,	double,	Input,	0,	,	
		sd,	char,	Input,	'd',	,	
		rg,	double,	Input,	0.01,	,	
		Bvdss,	double *,	Output,	,	,	
		LKG,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <stdlib.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#include <ksox_def.h>
#include <kui_proto.h>
#include <ktxe_proto.h>
#include "LBC5_proto.h"
	END USRLIB MODULE INFORMATION
*/
void ii4v(int d, int g, int s, int b, int g1, int g2, double vdL, double vdM, double vdH, double step, double goal, double ra, double pv, double dt, double vg, double vs, double vb, char sd, double rg, double * Bvdss, double * LKG);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ii4v_mos
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 19
	ARGUMENTS:
		d,	int,	Input,	-1,	,	
		g,	int,	Input,	-1,	,	
		s,	int,	Input,	-1,	,	
		b,	int,	Input,	-1,	,	
		gnd1,	int,	Input,	-1,	,	
		gnd2,	int,	Input,	-1,	,	
		vdL,	double,	Input,	0,	,	
		vdM,	double,	Input,	20,	,	
		vdH,	double,	Input,	40,	-180,	180
		step,	double,	Input,	1,	,	
		goal,	double,	Input,	1e-6,	,	
		ration,	double,	Input,	0.01,	,	
		plc_val,	double,	Input,	1,	,	
		dt,	double,	Input,	0.01,	,	
		vg,	double,	Input,	0,	,	
		vs,	double,	Input,	0,	,	
		vb,	double,	Input,	0,	,	
		sd,	char,	Input,	'd',	,	
		range,	double,	Input,	0.01,	,	
	INCLUDES:
#include <stdio.h>
#include <stdlib.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#include <ksox_def.h>
#include <kui_proto.h>
#include <ktxe_proto.h>
#include "LBC5_proto.h"
	END USRLIB MODULE INFORMATION
*/
double ii4v_mos(int d, int g, int s, int b, int gnd1, int gnd2, double vdL, double vdM, double vdH, double step, double goal, double ration, double plc_val, double dt, double vg, double vs, double vb, char sd, double range);


/* USRLIB MODULE INFORMATION

	MODULE NAME: iLinearVt
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 20
	ARGUMENTS:
		Devname,	char *,	Input,	"vt",	,	
		Drain,	int,	Input,	-1,	,	
		Gate,	int,	Input,	-1,	,	
		Source,	int,	Input,	-1,	,	
		Sub,	int,	Input,	-1,	,	
		Body,	int,	Input,	-1,	,	
		VgDmin,	double,	Input,	0,	,	
		VgDmax,	double,	Input,	2,	,	
		VgDstep,	double,	Input,	0.003,	,	
		Ith,	double,	Input,	1e-6,	,	
		VgDs,	double,	Input,	0.1,	,	
		Vsubst,	double,	Input,	0,	,	
		Vbody,	double,	Input,	0,	,	
		rDelay,	double,	Input,	0.005,	0,	1
		iNtrange,	int,	Input,	1,	0,	10
		LoR,	double,	Input,	0,	,	
		nO,	char,	Input,	'x',	,	
		mO,	char,	Input,	'x',	,	
		dG,	char,	Input,	'G',	,	
		sP,	char,	Input,	's',	,	
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
#include <stdlib.h>    
	END USRLIB MODULE INFORMATION
*/
double iLinearVt(char * Devname, int Drain, int Gate, int Source, int Sub, int Body, double VgDmin, double VgDmax, double VgDstep, double Ith, double VgDs, double Vsubst, double Vbody, double rDelay, int iNtrange, double LoR, char nO, char mO, char dG, char sP);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ion
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 24
	ARGUMENTS:
		Devname,	char *,	Input,	"bvdss",	,	
		Drain,	int,	Input,	-1,	,	
		Gate,	int,	Input,	-1,	,	
		Source,	int,	Input,	-1,	,	
		Sub,	int,	Input,	-1,	,	
		Body,	int,	Input,	-1,	,	
		VgDmin,	double,	Input,	0,	,	
		VgDmax,	double,	Input,	2,	,	
		VgDstep,	double,	Input,	0.05,	,	
		Ith0,	double,	Input,	1e-9,	,	
		Ith1,	double,	Input,	100e-9,	,	
		Ith,	double,	Input,	1e-6,	,	
		VgDs,	double,	Input,	0.1,	,	
		Vsubst,	double,	Input,	0,	,	
		Vbody,	double,	Input,	0,	,	
		rDelay,	double,	Input,	0.005,	0,	1
		iNtrange,	int,	Input,	1,	0,	10
		LoR,	double,	Input,	0,	,	
		nO,	char,	Input,	'x',	,	
		mO,	char,	Input,	'x',	,	
		dG,	char,	Input,	'g',	,	
		sP,	char,	Input,	's',	,	
		Ith0bv,	double *,	Output,	,	,	
		Ith1bv,	double *,	Output,	,	,	
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
#include <stdlib.h>    
	END USRLIB MODULE INFORMATION
*/
double ion(char * Devname, int Drain, int Gate, int Source, int Sub, int Body, double VgDmin, double VgDmax, double VgDstep, double Ith0, double Ith1, double Ith, double VgDs, double Vsubst, double Vbody, double rDelay, int iNtrange, double LoR, char nO, char mO, char dG, char sP, double * Ith0bv, double * Ith1bv);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ionO
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 19
	ARGUMENTS:
		devname,	char *,	Input,	"ionO",	,	
		d,	int,	Input,	-1,	,	
		g,	int,	Input,	-1,	,	
		s,	int,	Input,	-1,	,	
		b,	int,	Input,	-1,	,	
		g1,	int,	Input,	-1,	,	
		g2,	int,	Input,	-1,	,	
		vmin,	double,	Input,	0,	,	
		vmax,	double,	Input,	40,	,	
		vstep,	double,	Input,	0.2,	,	
		Ith0,	double,	Input,	1E-9,	,	
		Ith,	double,	Input,	1E-6,	,	
		plc,	double,	Input,	1,	,	
		delay,	double,	Input,	0.01,	,	
		vgs,	double,	Input,	0,	,	
		vbs,	double,	Input,	0,	,	
		fun,	int,	Input,	0,	,	
		sd,	char,	Input,	'd',	,	
		mode,	char,	Input,	'l',	,	
	INCLUDES:
#include <stdio.h>
#include <stdlib.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#include <ksox_def.h>
#include <kui_proto.h>
#include <ktxe_proto.h>
#include "LBC5_proto.h"
	END USRLIB MODULE INFORMATION
*/
double ionO(char * devname, int d, int g, int s, int b, int g1, int g2, double vmin, double vmax, double vstep, double Ith0, double Ith, double plc, double delay, double vgs, double vbs, int fun, char sd, char mode);


/* USRLIB MODULE INFORMATION

	MODULE NAME: iso_swp
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 22
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		d,	int,	Input,	-1,	,	
		g,	int,	Input,	-1,	,	
		s,	int,	Input,	-1,	,	
		cb,	int,	Input,	-1,	,	
		iso,	int,	Input,	-1,	,	
		bg,	int,	Input,	-1,	,	
		gnd1,	int,	Input,	-1,	,	
		gnd2,	int,	Input,	-1,	,	
		Vdmin,	double,	Input,	0,	,	
		Vdmax,	double,	Input,	20,	,	
		Vdstep,	double,	Input,	0.1,	,	
		Ith,	double,	Input,	1e-6,	,	
		delaytime,	double,	Input,	0.01,	,	
		Vg,	double,	Input,	0,	,	
		Vs,	double,	Input,	0,	,	
		Vb,	double,	Input,	0,	,	
		Range,	double,	Input,	1e-3,	,	
		plc_val,	int,	Input,	0,	,	
		type,	char,	Input,	,	,	
		Vdxx,	double *,	Output,	,	,	
		Idxx,	double *,	Output,	,	,	
	INCLUDES:
#include <stdlib.h>
#include <stdio.h>	
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <par_util.h>
#include <ktxe_proto.h>
#include <ksox_def.h>
#include <kui_proto.h>
#include "LBC5_proto.h"
#include <math.h>  
	END USRLIB MODULE INFORMATION
*/
double iso_swp(char * devname, int d, int g, int s, int cb, int iso, int bg, int gnd1, int gnd2, double Vdmin, double Vdmax, double Vdstep, double Ith, double delaytime, double Vg, double Vs, double Vb, double Range, int plc_val, char type, double * Vdxx, double * Idxx);


/* USRLIB MODULE INFORMATION

	MODULE NAME: isub10
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
double isub10(int DRAIN, int GATE, int SOURCE, int SUBST, double VGS, double VDS, double VBS);


/* USRLIB MODULE INFORMATION

	MODULE NAME: isub9
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 23
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		drain,	int,	Input,	-1,	,	
		gate,	int,	Input,	-1,	,	
		source,	int,	Input,	-1,	,	
		subst,	int,	Input,	-1,	,	
		g1,	int,	Input,	-1,	,	
		g2,	int,	Input,	-1,	,	
		g3,	int,	Input,	-1,	,	
		plc_val,	double,	Input,	1,	,	
		ilimit,	double,	Input,	1e-5,	,	
		lo_range,	double,	Input,	1e-8,	,	
		delay_time,	double,	Input,	0.2,	,	
		vds,	double,	Input,	0,	,	
		vgs,	double,	Input,	8,	,	
		vso,	double,	Input,	0,	,	
		vsu,	double,	Input,	0,	,	
		vgstep,	double,	Input,	0.5,	,	
		sdelay,	double,	Input,	0.001,	,	
		mode,	int,	Input,	0,	,	
		sweep,	char,	Input,	'p',	,	
		noise,	char,	Input,	'o',	,	
		sd,	char,	Input,	'd',	,	
		debug,	int,	Input,	1,	,	
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
	END USRLIB MODULE INFORMATION
*/
double isub9(char * devname, int drain, int gate, int source, int subst, int g1, int g2, int g3, double plc_val, double ilimit, double lo_range, double delay_time, double vds, double vgs, double vso, double vsu, double vgstep, double sdelay, int mode, char sweep, char noise, char sd, int debug);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ivh
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 22
	ARGUMENTS:
		devname,	char *,	Input,	"ivh",	,	
		drain,	int,	Input,	-1,	,	
		gate,	int,	Input,	-1,	,	
		source,	int,	Input,	-1,	,	
		subst,	int,	Input,	-1,	,	
		g1,	int,	Input,	-1,	,	
		g2,	int,	Input,	-1,	,	
		s_drain,	int,	Input,	-1,	,	
		s_gate,	int,	Input,	-1,	,	
		type,	char,	Input,	'n',	,	
		plc,	double,	Input,	1,	,	
		ilimit,	double,	Input,	1e-6,	,	
		irange,	double,	Input,	1e-9,	,	
		delay,	double,	Input,	0.2,	,	
		vgs,	double,	Input,	0,	,	
		vds,	double,	Input,	1.8,	,	
		vsub,	double,	Input,	0,	,	
		sd,	char,	Input,	'd',	,	
		unitC,	int,	Input,	0,	,	
		width,	double,	Input,	0,	,	
		hp,	double,	Input,	0,	,	
		vstep,	double,	Input,	0.3,	,	
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
	END USRLIB MODULE INFORMATION
*/
double ivh(char * devname, int drain, int gate, int source, int subst, int g1, int g2, int s_drain, int s_gate, char type, double plc, double ilimit, double irange, double delay, double vgs, double vds, double vsub, char sd, int unitC, double width, double hp, double vstep);


/* USRLIB MODULE INFORMATION

	MODULE NAME: leak4_1n
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 6
	ARGUMENTS:
		hi,	int,	Input,	-1,	,	
		lo1,	int,	Input,	-1,	,	
		lo2,	int,	Input,	-1,	,	
		subst,	int,	Input,	-1,	,	
		v,	double,	Input,	7,	,	
		ilim,	double,	Input,	1e-6,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double  leak4_1n(int, int, int, int, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: leak4_1n_org
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 6
	ARGUMENTS:
		hi,	int,	Input,	-1,	,	
		lo1,	int,	Input,	-1,	,	
		lo2,	int,	Input,	-1,	,	
		subst,	int,	Input,	-1,	,	
		v,	double,	Input,	7,	,	
		ilim,	double,	Input,	1e-6,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double  leak4_1n_org(int, int, int, int, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: leak4_gate
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 11
	ARGUMENTS:
		hi,	int,	Input,	-1,	,	
		lo1,	int,	Input,	-1,	,	
		lo2,	int,	Input,	-1,	,	
		subst,	int,	Input,	-1,	,	
		g1,	int,	Input,	-1,	,	
		g2,	int,	Input,	-1,	,	
		g3,	int,	Input,	-1,	,	
		g4,	int,	Input,	-1,	,	
		g5,	int,	Input,	-1,	,	
		v,	double,	Input,	7,	,	
		ilim,	double,	Input,	1e-6,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double leak4_gate(int hi, int lo1, int lo2, int subst, int g1, int g2, int g3, int g4, int g5, double v, double ilim);


/* USRLIB MODULE INFORMATION

	MODULE NAME: leak4_h
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 6
	ARGUMENTS:
		hi,	int,	Input,	-1,	,	
		lo1,	int,	Input,	-1,	,	
		lo2,	int,	Input,	-1,	,	
		subst,	int,	Input,	-1,	,	
		v,	double,	Input,	7,	,	
		ilim,	double,	Input,	1e-6,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double  leak4_h(int, int, int, int, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: leak4_jm
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 6
	ARGUMENTS:
		hi,	int,	Input,	-1,	,	
		lo1,	int,	Input,	-1,	,	
		lo2,	int,	Input,	-1,	,	
		subst,	int,	Input,	-1,	,	
		v,	double,	Input,	7,	,	
		ilim,	double,	Input,	1e-6,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double  leak4_jm(int, int, int, int, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: leak4_lc_dd
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
double leak4_lc_dd(int hi, int lo1, int lo2, int subst, double v, double ilim);


/* USRLIB MODULE INFORMATION

	MODULE NAME: leak4_lc_dd_6
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 8
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo1,	int,	Input,	,	,	
		lo2,	int,	Input,	,	,	
		gnd1,	int,	Input,	,	,	
		gnd2,	int,	Input,	,	,	
		gnd3,	int,	Input,	,	,	
		v,	double,	Input,	,	,	
		ilim,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double leak4_lc_dd_6(int hi, int lo1, int lo2, int gnd1, int gnd2, int gnd3, double v, double ilim);


/* USRLIB MODULE INFORMATION

	MODULE NAME: leak_dio
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
void leak_dio(char * devname, int top, int bot, int sub, int chuckcon, double area, double per, char type, char btype, double vrev, double ilim, double icomp, double irange, double delaytime, double multiplier, double intrange, double mrange, double lorange, double * result);


total 8146
-rw-rw-r--   1 kthmgr   keithley    4818 Mar  8  2010 E1s_loop.c
-rw-rw-r--   1 kthmgr   keithley    9853 Mar  8  2010 vtext4_HV.c
-rw-rw-r--   1 kthmgr   keithley    5801 Jul  9  2010 basic.c
-rw-rw-r--   1 kthmgr   keithley    4710 Jul  9  2010 bvceo_pnp.c
-rw-rw-r--   1 kthmgr   keithley    5875 Jul  9  2010 bvceoI_pnp.c
-rw-rw-r--   1 kthmgr   keithley    2440 Jul  9  2010 eak4_lc_dd_6.c
-rw-rw-r--   1 kthmgr   keithley    8812 Jul  9  2010 linear_vt.c
-rw-rw-r--   1 kthmgr   keithley    5902 Jul 22  2010 i4v_sch_sub.c
-rw-rw-r--   1 kthmgr   keithley    4282 Aug  2  2010 *jet*
-rw-rw-r--   1 kthmgr   keithley  445440 Aug 13  2010 haemil_bak_100813_pt16.tar
-rw-rw-r--   1 kthmgr   keithley   13648 Oct 19 07:45 I2v.c_ORG
-rw-rw-r--   1 kthmgr   keithley  964608 Oct 20 12:41 aa.tar
-rw-rw-r--   1 kthmgr   keithley    8283 Nov  3 20:25 pinch_off.c_ORG
-rw-rw-r--   1 kthmgr   keithley   12783 Nov 19 20:20 Iis4v_mos.c
-rw-rw-r--   1 kthmgr   keithley 1979904 Jan 10 11:52 haemil_bak_110110.tar
-rw-rw-r--   1 kthmgr   keithley    8891 Jan 10 11:52 BVDS_mos3.c
-rw-rw-r--   1 kthmgr   keithley    5617 Jan 10 11:52 BVDSH_mos.c
-rw-rw-r--   1 kthmgr   keithley    6366 Jan 10 11:52 beta10.c
-rw-rw-r--   1 kthmgr   keithley    5387 Jan 10 11:52 Rsheet_wid_debug.c
-rw-rw-r--   1 kthmgr   keithley    5042 Jan 10 11:52 OnBvcex.c
-rw-rw-r--   1 kthmgr   keithley    8130 Jan 10 11:52 LinearGm.c
-rw-rw-r--   1 kthmgr   keithley    9391 Jan 10 11:52 I2v.c
-rw-rw-r--   1 kthmgr   keithley    9581 Jan 10 11:52 BVDS_mos_vswp.c
-rw-rw-r--   1 kthmgr   keithley    6084 Jan 10 11:52 BVDS_mos5.c
-rw-rw-r--   1 kthmgr   keithley    7071 Jan 10 11:52 bvdss1_hae.c
-rw-rw-r--   1 kthmgr   keithley    5860 Jan 10 11:52 bvds_sweep_mos.c
-rw-rw-r--   1 kthmgr   keithley    5018 Jan 10 11:52 bvcex.c
-rw-rw-r--   1 kthmgr   keithley    4059 Jan 10 11:52 bvceo_miho2_pnp.c
-rw-rw-r--   1 kthmgr   keithley    5018 Jan 10 11:52 bvceo.c
-rw-rw-r--   1 kthmgr   keithley    4709 Jan 10 11:52 bvcbo_pnp.c
-rw-rw-r--   1 kthmgr   keithley    6026 Jan 10 11:52 beta9_tmp.c
-rw-rw-r--   1 kthmgr   keithley    5900 Jan 10 11:52 beta9.c
-rw-rw-r--   1 kthmgr   keithley    4994 Jan 10 11:52 beta8.c
-rw-rw-r--   1 kthmgr   keithley    7210 Jan 10 11:52 bvdss_sch_tmp.c
-rw-rw-r--   1 kthmgr   keithley    6801 Jan 10 11:52 bvdss_sch_sub.c
-rw-rw-r--   1 kthmgr   keithley    6888 Jan 10 11:52 bvdss_sch_1.c
-rw-rw-r--   1 kthmgr   keithley    6814 Jan 10 11:52 bvdss_sch.c
-rw-rw-r--   1 kthmgr   keithley    6965 Jan 10 11:52 bvdss_hae.c
-rw-rw-r--   1 kthmgr   keithley    6959 Jan 10 11:52 bvdss5_mos.c
-rw-rw-r--   1 kthmgr   keithley    7791 Jan 10 11:52 bvdss4_mos.c
-rw-rw-r--   1 kthmgr   keithley    8030 Jan 10 11:52 bvdss2_mos.c
-rw-rw-r--   1 kthmgr   keithley    5258 Jan 10 11:52 bvdss2_jfet.c
-rw-rw-r--   1 kthmgr   keithley    9238 Jan 10 11:52 bvdss1_mos_hae.c
-rw-rw-r--   1 kthmgr   keithley    8634 Jan 10 11:52 i2v_bv.c
-rw-rw-r--   1 kthmgr   keithley   13327 Jan 10 11:52 i2v.c
-rw-rw-r--   1 kthmgr   keithley    4237 Jan 10 11:52 haemilpn2swp_vramp.c
-rw-rw-r--   1 kthmgr   keithley    4240 Jan 10 11:52 haemil_pn2swp_vramp.c
-rw-rw-r--   1 kthmgr   keithley    8364 Jan 10 11:52 gmlin5.c
-rw-rw-r--   1 kthmgr   keithley     887 Jan 10 11:52 gg.c
-rw-rw-r--   1 kthmgr   keithley    6578 Jan 10 11:52 ev_jfet.c
-rw-rw-r--   1 kthmgr   keithley    6525 Jan 10 11:52 ev5.c
-rw-rw-r--   1 kthmgr   keithley    6732 Jan 10 11:52 early_jfet.c
-rw-rw-r--   1 kthmgr   keithley    2574 Jan 10 11:52 idh.c
-rw-rw-r--   1 kthmgr   keithley    9451 Jan 10 11:52 i4v_mos.c
-rw-rw-r--   1 kthmgr   keithley    6911 Jan 10 11:52 i4v_isub.c
-rw-rw-r--   1 kthmgr   keithley    6764 Jan 10 11:52 i4v_hfe.c
-rw-rw-r--   1 kthmgr   keithley    9063 Jan 10 11:52 i4v_bv.c
-rw-rw-r--   1 kthmgr   keithley    6341 Jan 10 11:52 i4v_bgr.c
-rw-rw-r--   1 kthmgr   keithley   11415 Jan 10 11:52 i2v_mos.c
-rw-rw-r--   1 kthmgr   keithley    8472 Jan 10 11:52 i2v_jfet.c
-rw-rw-r--   1 kthmgr   keithley    8283 Jan 10 11:52 pinch_off.c
-rw-rw-r--   1 kthmgr   keithley    8424 Jan 10 11:52 linear_mos.c
-rw-rw-r--   1 kthmgr   keithley    8373 Jan 10 11:52 linear_gm.c
-rw-rw-r--   1 kthmgr   keithley    5943 Jan 10 11:52 leak_dio.c
-rw-rw-r--   1 kthmgr   keithley    2387 Jan 10 11:52 leak4_lc_dd_6.c
-rw-rw-r--   1 kthmgr   keithley    2245 Jan 10 11:52 leak4_lc_dd.c
-rw-rw-r--   1 kthmgr   keithley    2300 Jan 10 11:52 leak4_h.c
-rw-rw-r--   1 kthmgr   keithley    6896 Jan 10 11:52 isub9.c
-rw-rw-r--   1 kthmgr   keithley   12633 Jan 10 11:52 ii4v_mos.c
-rw-rw-r--   1 kthmgr   keithley   13124 Jan 10 11:52 ii4v.c
-rw-rw-r--   1 kthmgr   keithley    2225 Jan 10 11:52 resv1.c
-rw-rw-r--   1 kthmgr   keithley    4252 Jan 10 11:52 pn2swp_otp.c
-rw-rw-r--   1 kthmgr   keithley    4307 Jan 10 11:52 pn2swp_lvgg1.c
-rw-rw-r--   1 kthmgr   keithley    4142 Jan 10 11:52 pn2swp_lvg.c
-rw-rw-r--   1 kthmgr   keithley    4528 Jan 10 11:52 pn2swp_calc2.c
-rw-rw-r--   1 kthmgr   keithley    4322 Jan 10 11:52 pn2swp_calc1.c
-rw-rw-r--   1 kthmgr   keithley    3781 Jan 10 11:52 pn2swp_calc.c
-rw-rw-r--   1 kthmgr   keithley    3754 Jan 10 11:52 pn2swp4.c
-rw-rw-r--   1 kthmgr   keithley    8435 Jan 10 11:52 pinchoff.c
-rw-rw-r--   1 kthmgr   keithley    9550 Jan 10 11:52 vt_lin_hae.c
-rw-rw-r--   1 kthmgr   keithley    8445 Jan 10 11:52 vt_gm_mos.c
-rw-rw-r--   1 kthmgr   keithley    8510 Jan 10 11:52 vpin_jfet.c
-rw-rw-r--   1 kthmgr   keithley    8390 Jan 10 11:52 vp_jfet_r0.c
-rw-rw-r--   1 kthmgr   keithley    7056 Jan 10 11:52 vp_jfet.c
-rw-rw-r--   1 kthmgr   keithley    8032 Jan 10 11:52 v4i.c
-rw-rw-r--   1 kthmgr   keithley    7962 Jan 10 11:52 v2i_iramp.c
-rw-rw-r--   1 kthmgr   keithley    5212 Jan 10 11:52 uugoi.c
-rw-rw-r--   1 kthmgr   keithley    7221 Jan 10 11:52 sub_sch.c
-rw-rw-r--   1 kthmgr   keithley    4664 Jan 10 11:52 xbvdss2_jfet.c
-rw-rw-r--   1 kthmgr   keithley    5394 Jan 10 11:52 xbvdss2_hae.c
-rw-rw-r--   1 kthmgr   keithley    7248 Jan 10 11:52 vtsslp_sq_mos1.c
-rw-rw-r--   1 kthmgr   keithley    6688 Jan 10 11:52 vtsslp_mos1.c
-rw-rw-r--   1 kthmgr   keithley   10114 Jan 10 11:52 vtext4_hae.c
-rw-rw-r--   1 kthmgr   keithley    8451 Jan 10 11:52 vtext4_dptr.c
-rw-rw-r--   1 kthmgr   keithley    8448 Jan 10 11:52 vt_lin_raw.c
-rw-rw-r--   1 kthmgr   keithley   17232 Jan 10 11:52 vt_lin_mos_HV.c
-rw-rw-r--   1 kthmgr   keithley    8233 Jan 18 20:16 gmlin.c
-rw-rw-r--   1 kthmgr   keithley    5245 Feb 16 17:59 Rsheet_wid_hae.c
-rw-rw-r--   1 kthmgr   keithley    6084 Feb 25 09:40 BVDS_mos2.c
-rw-rw-r--   1 kthmgr   keithley    6295 Feb 25 10:25 LinearVt.c
-rw-rw-r--   1 kthmgr   keithley   10478 Mar  2 16:50 breakv.c
-rw-rw-r--   1 kthmgr   keithley    5874 Mar  2 16:58 i4v.c
-rw-rw-r--   1 kthmgr   keithley    2938 Mar  2 19:30 id9.c
-rw-rw-r--   1 kthmgr   keithley     109 Mar  2 20:47 haemil_settings.ini
-rw-rw-r--   1 kthmgr   keithley    5075 Mar  3 17:37 linearvt.c
void leak_dio(char * devname, int top, int bot, int sub, int chuckcon, double area, double per, char type, char btype, double vrev, double ilim, double icomp, double irange, double delaytime, double multiplier, double intrange, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: linear_mos
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 32
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		bulk,	int,	Input,	-1,	,	
		sub,	int,	Input,	-1,	,	
		chuckcon,	int,	Input,	-1,	,	
		gnd1,	int,	Input,	-1,	,	
		gnd2,	int,	Input,	-1,	,	
		width,	double,	Input,	1,	,	
		length,	double,	Input,	1,	,	
		type,	char,	Input,	'n',	,	
		vgmin,	double,	Input,	0,	,	
		vgmax,	double,	Input,	2,	,	
		idsearch,	double,	Input,	1e-7,	,	
		num,	double,	Input,	10,	,	
		steps,	double,	Input,	0.1,	,	
		vds,	double,	Input,	0.1,	,	
		vsu,	double,	Input,	0,	,	
		vso,	double,	Input,	0,	,	
		dcomp,	double,	Input,	1e-6,	,	
		gcomp,	double,	Input,	1e-6,	,	
		socomp,	double,	Input,	1e-6,	,	
		sucomp,	double,	Input,	1e-6,	,	
		multiplier,	double,	Input,	1,	,	
		delaytime,	double,	Input,	0.01,	,	
		plc_val,	double,	Input,	1,	,	
		debug,	int,	Input,	1,	,	
		mode,	char,	Input,	'p',	,	
		device,	char *,	Input,	"vt_lin",	,	
		Id,	double *,	Output,	,	,	
		Ig,	double *,	Output,	,	,	
		result,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#define MAX(a,b)	((a)>(b) ? (a) : (b))
#include <ktxe_proto.h>
#include <ksox_def.h>
#include <kui_proto.h>
	END USRLIB MODULE INFORMATION
*/
void linear_mos(int drain, int gate, int source, int bulk, int sub, int chuckcon, int gnd1, int gnd2, double width, double length, char type, double vgmin, double vgmax, double idsearch, double num, double steps, double vds, double vsu, double vso, double dcomp, double gcomp, double socomp, double sucomp, double multiplier, double delaytime, double plc_val, int debug, char mode, char * device, double * Id, double * Ig, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: LinearGm
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 16
	ARGUMENTS:
		drain,	int,	Input,	-1,	,	
		gate,	int,	Input,	-1,	,	
		source,	int,	Input,	-1,	,	
		body,	int,	Input,	-1,	,	
		subst,	int,	Input,	-1,	,	
		w,	double,	Input,	1,	,	
		l,	double,	Input,	1,	,	
		vlow,	double,	Input,	0,	,	
		vhigh,	double,	Input,	2,	,	
		vds,	double,	Input,	0.1,	,	
		vbs,	double,	Input,	0,	,	
		ithr,	double,	Input,	1e-6,	,	
		vstep,	double,	Input,	0.05,	,	
		npts,	int,	Input,	15,	,	
		slope,	double *,	Output,	 ,	,	
		kflag,	int *,	Output,	 ,	,	
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
double LinearGm(int drain, int gate, int source, int body, int subst, double w, double l, double vlow, double vhigh, double vds, double vbs, double ithr, double vstep, int npts, double * slope, int * kflag);


/* USRLIB MODULE INFORMATION

	MODULE NAME: linearvt
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 23
	ARGUMENTS:
		Devname,	char *,	Input,	"dev",	,	
		D,	int,	Input,	-1,	,	
		G,	int,	Input,	-1,	,	
		S,	int,	Input,	-1,	,	
		B,	int,	Input,	-1,	,	
		Su,	int,	Input,	-1,	,	
		Ch,	int,	Input,	-1,	,	
		Vgmin,	double,	Input,	0,	,	
		Vgmax,	double,	Input,	2,	,	
		Vgstep,	double,	Input,	0.01,	,	
		Ith,	double,	Input,	0.1e-6,	,	
		IthSS,	double,	Input,	0,	,	
		IthSH,	double,	Input,	0,	,	
		Vd,	double,	Input,	0.1,	,	
		VdSH,	double,	Input,	0,	,	
		Vs,	double,	Input,	0,	-200,	200
		Vb,	double,	Input,	0,	-200,	200
		Intrange,	double,	Input,	1,	0,	10
		Num,	int,	Input,	10,	0,	16
		Delaytime,	double,	Input,	0.01,	0,	0.5
		Multiplier,	double,	Input,	0,	,	
		SS,	double *,	Output,	,	,	
		SH,	double *,	Output,	,	,	
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
double linearvt(char * Devname, int D, int G, int S, int B, int Su, int Ch, double Vgmin, double Vgmax, double Vgstep, double Ith, double IthSS, double IthSH, double Vd, double VdSH, double Vs, double Vb, double Intrange, int Num, double Delaytime, double Multiplier, double * SS, double * SH);


/* USRLIB MODULE INFORMATION

	MODULE NAME: LinearVt
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 21
	ARGUMENTS:
		devname,	char *,	Input,	"const",	,	
		drain,	int,	Input,	-1,	,	
		gate,	int,	Input,	-1,	,	
		source,	int,	Input,	-1,	,	
		bulk,	int,	Input,	-1,	,	
		sub,	int,	Input,	-1,	,	
		chuckcon,	int,	Input,	-1,	,	
		width,	double,	Input,	1,	,	
		length,	double,	Input,	1,	,	
		type,	char,	Input,	'x',	,	
		vmin,	double,	Input,	0,	,	
		vmax,	double,	Input,	2,	,	
		idsearch,	double,	Input,	1E-8,	,	
		vds,	double,	Input,	0.1,	,	
		vbs,	double,	Input,	0,	,	
		multiplier,	double,	Input,	1,	,	
		intrange,	double,	Input,	1,	,	
		delaytime,	double,	Input,	0.005,	,	
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
#define MAX(a,b)	((a)>(b) ? (a) : (b))
#define CDELAY  125.E-12
#define ILEAK   20.E-12 
	END USRLIB MODULE INFORMATION
*/
void LinearVt(char * devname, int drain, int gate, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vmin, double vmax, double idsearch, double vds, double vbs, double multiplier, double intrange, double delaytime, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: LinearVt_native
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 21
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
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
void LinearVt_native(char * devname, int drain, int gate, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vmin, double vmax, double idsearch, double vds, double vbs, double multiplier, double intrange, double delaytime, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: LinearVtS
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 25
	ARGUMENTS:
		Devname,	char *,	Input,	"DEV",	,	
		D,	int,	Input,	-1,	,	
		G,	int,	Input,	-1,	,	
		S,	int,	Input,	-1,	,	
		B,	int,	Input,	-1,	,	
		G1,	int,	Input,	-1,	,	
		G2,	int,	Input,	-1,	,	
		Vgmin,	double,	Input,	0,	,	
		Vgmax,	double,	Input,	2,	,	
		Vgstep,	double,	Input,	0.01,	,	
		Ith,	double,	Input,	0.1e-6,	,	
		IthSS,	double,	Input,	0,	,	
		IthSH,	double,	Input,	0,	,	
		Vd,	double,	Input,	0.1,	,	
		VdSH,	double,	Input,	0,	,	
		Vs,	double,	Input,	0,	-200,	200
		Vb,	double,	Input,	0,	-200,	200
		Intrange,	double,	Input,	1,	0,	10
		Num,	int,	Input,	10,	0,	16
		Delaytime,	double,	Input,	0.01,	0,	0.5
		Multiplier,	int,	Input,	0,	,	
		Mode,	char,	Input,	'n',	,	
		Type,	char,	Input,	'n',	,	
		SLPS,	double *,	Output,	,	,	
		SFTS,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <stdlib.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#include <tidp1.h>
#include <ksox_def.h>

#define MAX(a,b)	((a)>(b) ? (a) : (b))
#define CDELAY  125.E-12
#define ILEAK   20.E-12 
	END USRLIB MODULE INFORMATION
*/
double LinearVtS(char * Devname, int D, int G, int S, int B, int G1, int G2, double Vgmin, double Vgmax, double Vgstep, double Ith, double IthSS, double IthSH, double Vd, double VdSH, double Vs, double Vb, double Intrange, int Num, double Delaytime, int Multiplier, char Mode, char Type, double * SLPS, double * SFTS);


/* USRLIB MODULE INFORMATION

	MODULE NAME: LKG_pn2swp_extra
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
double  LKG_pn2swp_extra(int, int, int, double, double, int, double, double, double, char, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: OnBvcex
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 22
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		em,	int,	Input,	-1,	,	
		ba,	int,	Input,	-1,	,	
		co,	int,	Input,	-1,	,	
		sub,	int,	Input,	-1,	,	
		poly,	int,	Input,	-1,	,	
		lowt,	int,	Input,	0,	,	
		vbe_in,	double,	Input,	0,	,	
		ipgm,	double,	Input,	1e-6,	,	
		vlimit,	double,	Input,	40,	,	
		ilimit,	double,	Input,	1e-3,	,	
		vlimit_poly,	double,	Input,	40,	,	
		range,	double,	Input,	0.1,	,	
		costep,	double,	Input,	100,	,	
		wtime,	double,	Input,	0.01,	,	
		vsub,	double,	Input,	0,	,	
		vpoly,	double,	Input,	0,	,	
		vbb,	double,	Input,	0,	,	
		plc_val,	double,	Input,	0,	,	
		type,	char,	Input,	,	,	
		vbe,	double *,	Output,	,	,	
		ipoly,	double *,	Output,	,	,	
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
double OnBvcex(char * devname, int em, int ba, int co, int sub, int poly, int lowt, double vbe_in, double ipgm, double vlimit, double ilimit, double vlimit_poly, double range, double costep, double wtime, double vsub, double vpoly, double vbb, double plc_val, char type, double * vbe, double * ipoly);


/* USRLIB MODULE INFORMATION

	MODULE NAME: pgm_eeprom8
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 10
	ARGUMENTS:
		CG,	int,	Input,	-1,	,	
		D,	int,	Input,	-1,	,	
		S,	int,	Input,	-1,	,	
		TG,	int,	Input,	-1,	,	
		ISO,	int,	Input,	-1,	,	
		PSUB,	int,	Input,	-1,	,	
		G1,	int,	Input,	-1,	,	
		G2,	int,	Input,	-1,	,	
		VCG,	double,	Input,	17,	,	
		TE,	double,	Input,	10,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "COM_usrlib.h"
	END USRLIB MODULE INFORMATION
*/
void pgm_eeprom8(int CG, int D, int S, int TG, int ISO, int PSUB, int G1, int G2, double VCG, double TE);


/* USRLIB MODULE INFORMATION

	MODULE NAME: pinch_off
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 29
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		bulk,	int,	Input,	,	,	
		sub,	int,	Input,	-1,	,	
		co_gate,	int,	Input,	-1,	,	
		gnd1,	int,	Input,	-1,	,	
		gnd2,	int,	Input,	-1,	,	
		gnd3,	int,	Input,	-1,	,	
		width,	double,	Input,	1,	,	
		length,	double,	Input,	1,	,	
		vgsmin,	double,	Input,	-4,	,	
		vgsmax,	double,	Input,	0,	,	
		vgstep,	double,	Input,	0.1,	,	
		loop,	int,	Input,	100,	,	
		ilimit,	double,	Input,	10e-9,	,	
		icomp,	double,	Input,	0.001,	,	
		intrange,	double,	Input,	0.,	,	
		sdelay,	double,	Input,	0.01,	,	
		sd,	char,	Input,	'd',	,	
		flag,	char,	Input,	'x',	,	
		type,	char,	Input,	'n',	,	
		vbulk,	double,	Input,	0.,	,	
		vdrain,	double,	Input,	0.,	,	
		debug,	int,	Input,	1,	,	
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
#include <ksox_def.h>
#include <kui_proto.h>
	END USRLIB MODULE INFORMATION
*/
void pinch_off(char * devname, int drain, int gate, int source, int bulk, int sub, int co_gate, int gnd1, int gnd2, int gnd3, double width, double length, double vgsmin, double vgsmax, double vgstep, int loop, double ilimit, double icomp, double intrange, double sdelay, char sd, char flag, char type, double vbulk, double vdrain, int debug, double * Bvdss, double * Id, double * Is);


/* USRLIB MODULE INFORMATION

	MODULE NAME: pinchoff
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 29
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		bulk,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		bgate,	int,	Input,	,	,	
		gnd1,	int,	Input,	,	,	
		gnd2,	int,	Input,	,	,	
		width,	double,	Input,	,	,	
		length,	double,	Input,	,	,	
		vdsmin,	double,	Input,	,	,	
		vdsmax,	double,	Input,	,	,	
		vdstep,	double,	Input,	,	,	
		ilimit,	double,	Input,	,	,	
		icomp,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		sdelay,	double,	Input,	,	,	
		steps,	double,	Input,	,	,	
		loop,	int,	Input,	,	,	
		sd,	char,	Input,	,	,	
		flag,	char,	Input,	,	,	
		type,	char,	Input,	,	,	
		vbulk,	double,	Input,	,	,	
		vdrain,	double,	Input,	,	,	
		debug,	int,	Input,	,	,	
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
#include <ksox_def.h>
#include <kui_proto.h>
	END USRLIB MODULE INFORMATION
*/
void pinchoff(char * devname, int drain, int gate, int source, int bulk, int sub, int bgate, int gnd1, int gnd2, double width, double length, double vdsmin, double vdsmax, double vdstep, double ilimit, double icomp, double intrange, double sdelay, double steps, int loop, char sd, char flag, char type, double vbulk, double vdrain, int debug, double * Bvdss, double * Id, double * Is);


/* USRLIB MODULE INFORMATION

	MODULE NAME: pinchoff9
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 29
	ARGUMENTS:
		devname,	char *,	Input,	"aa",	,	
		D,	int,	Input,	-1,	,	
		G,	int,	Input,	-1,	,	
		S,	int,	Input,	-1,	,	
		B,	int,	Input,	-1,	,	
		Sub,	int,	Input,	-1,	,	
		coG,	int,	Input,	-1,	,	
		G1,	int,	Input,	-1,	,	
		G2,	int,	Input,	-1,	,	
		G3,	int,	Input,	-1,	,	
		width,	double,	Input,	1,	,	
		length,	double,	Input,	1,	,	
		vgsmin,	double,	Input,	-4,	,	
		vgsmax,	double,	Input,	0,	,	
		vgstep,	double,	Input,	0.1,	,	
		loop,	int,	Input,	100,	,	
		ilimit,	double,	Input,	10e-9,	,	
		icomp,	double,	Input,	0.001,	,	
		intrange,	double,	Input,	0.,	,	
		sdelay,	double,	Input,	0.01,	,	
		sd,	char,	Input,	'd',	,	
		flag,	char,	Input,	'x',	,	
		type,	char,	Input,	'n',	,	
		VB,	double,	Input,	0.,	,	
		VD,	double,	Input,	0.,	,	
		Debug,	int,	Input,	1,	,	
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
#include <ksox_def.h>
#include <kui_proto.h>
	END USRLIB MODULE INFORMATION
*/
void pinchoff9(char * devname, int D, int G, int S, int B, int Sub, int coG, int G1, int G2, int G3, double width, double length, double vgsmin, double vgsmax, double vgstep, int loop, double ilimit, double icomp, double intrange, double sdelay, char sd, char flag, char type, double VB, double VD, int Debug, double * Bvdss, double * Id, double * Is);


/* USRLIB MODULE INFORMATION

	MODULE NAME: pn2swp10
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
double pn2swp10(int hi, int lo, int subst, double vstart, double vstop, int nstep, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: pn2swp11
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 10
	ARGUMENTS:
		hi,	int,	Input,	-1,	,	
		lo,	int,	Input,	-1,	,	
		subst,	int,	Input,	-1,	,	
		body,	int,	Input,	-1,	,	
		vstart,	double,	Input,	0,	,	
		vstop,	double,	Input,	10,	,	
		nstep,	int,	Input,	100,	,	
		ipgm,	double,	Input,	1e-6,	,	
		udelay,	double,	Input,	0.03,	,	
		type,	char,	Input,	'n',	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double pn2swp11(int hi, int lo, int subst, int body, double vstart, double vstop, int nstep, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: pn2swp4
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
double pn2swp4(int hi, int lo, int subst, double vstart, double vstop, int nstep, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: pn2swp6
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
double pn2swp6(int hi, int lo, int subst, double vstart, double vstop, int nstep, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: pn2swp9
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 9
	ARGUMENTS:
		hi,	int,	Input,	-1,	,	
		lo,	int,	Input,	-1,	,	
		subst,	int,	Input,	-1,	,	
		vstart,	double,	Input,	0,	,	
		vstop,	double,	Input,	10,	,	
		nstep,	int,	Input,	100,	,	
		ipgm,	double,	Input,	1e-6,	,	
		udelay,	double,	Input,	0.03,	,	
		type,	char,	Input,	'n',	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double pn2swp9(int hi, int lo, int subst, double vstart, double vstop, int nstep, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: pn2swp_calc
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
double pn2swp_calc(int hi, int lo, int subst, double vstart, double vstop, int nstep, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: pn2swp_calc1
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
double pn2swp_calc1(int hi, int lo, int subst, double vstart, double vstop, int nstep, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: pn2swp_calc2
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
double pn2swp_calc2(int hi, int lo, int subst, double vstart, double vstop, int nstep, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: pn2swp_ext
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
double  pn2swp_ext(int, int, int, double, double, int, double, double, char);

/* USRLIB MODULE INFORMATION

	MODULE NAME: pn2swp_lvg
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 13
	ARGUMENTS:
		drain,	int,	Input,	-1,	,	
		gate,	int,	Input,	-1,	,	
		source,	int,	Input,	-1,	,	
		subst,	int,	Input,	-1,	,	
		g1,	int,	Input,	-1,	,	
		g2,	int,	Input,	-1,	,	
		vgate,	double,	Input,	0,	,	
		vstart,	double,	Input,	0,	,	
		vstop,	double,	Input,	40,	,	
		nstep,	int,	Input,	201,	,	
		ipgm,	double,	Input,	1e-6,	,	
		udelay,	double,	Input,	0.02,	,	
		type,	char,	Input,	'n',	,	
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
double pn2swp_lvg(int drain, int gate, int source, int subst, int g1, int g2, double vgate, double vstart, double vstop, int nstep, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: pn2swp_lvgg1
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 11
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	-1,	,	
		subst,	int,	Input,	-1,	,	
		g1,	int,	Input,	-1,	,	
		g2,	int,	Input,	-1,	,	
		vstart,	double,	Input,	0,	,	
		vstop,	double,	Input,	40,	,	
		nstep,	int,	Input,	201,	,	
		ipgm,	double,	Input,	1e-6,	,	
		udelay,	double,	Input,	0.02,	,	
		type,	char,	Input,	'n',	,	
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
double pn2swp_lvgg1(int hi, int lo, int subst, int g1, int g2, double vstart, double vstop, int nstep, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: pn2swp_otp
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 15
	ARGUMENTS:
		drain,	int,	Input,	-1,	,	
		drain2,	int,	Input,	-1,	,	
		gate,	int,	Input,	-1,	,	
		source,	int,	Input,	-1,	,	
		subst,	int,	Input,	-1,	,	
		g1,	int,	Input,	-1,	,	
		g2,	int,	Input,	-1,	,	
		g3,	int,	Input,	-1,	,	
		vgate,	double,	Input,	0,	,	
		vstart,	double,	Input,	0,	,	
		vstop,	double,	Input,	40,	,	
		nstep,	int,	Input,	201,	,	
		ipgm,	double,	Input,	1e-6,	,	
		udelay,	double,	Input,	0.02,	,	
		type,	char,	Input,	'n',	,	
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
double pn2swp_otp(int drain, int drain2, int gate, int source, int subst, int g1, int g2, int g3, double vgate, double vstart, double vstop, int nstep, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: pn2swp_swp_log_ks
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
double pn2swp_swp_log_ks(int hi, int lo, int subst, double vstart, double vstop, int nstep, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: pn2swpS
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 9
	ARGUMENTS:
		hi,	int,	Input,	-1,	,	
		lo,	int,	Input,	-1,	,	
		subst,	int,	Input,	-1,	,	
		vstart,	double,	Input,	0,	,	
		vstop,	double,	Input,	10,	,	
		nstep,	int,	Input,	100,	,	
		ipgm,	double,	Input,	1e-6,	,	
		udelay,	double,	Input,	0.03,	,	
		type,	char,	Input,	'n',	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double pn2swpS(int hi, int lo, int subst, double vstart, double vstop, int nstep, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: res9
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 14
	ARGUMENTS:
		Devname,	char *,	Input,	,	,	
		FH,	int,	Input,	-1,	,	
		SH,	int,	Input,	-1,	,	
		SL,	int,	Input,	-1,	,	
		FL,	int,	Input,	-1,	,	
		SQ,	double,	Input,	0,	,	
		Limit,	double,	Input,	1,	,	
		FHV,	double,	Input,	0,	,	
		SHV,	double,	Input,	0,	,	
		SLV,	char,	Input,	'0',	,	
		FLV,	char,	Input,	'0',	,	
		delaytime,	double,	Input,	0.06,	,	
		intrange,	double,	Input,	0,	,	
		R,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void res9(char * Devname, int FH, int SH, int SL, int FL, double SQ, double Limit, double FHV, double SHV, char SLV, char FLV, double delaytime, double intrange, double * R);


/* USRLIB MODULE INFORMATION

	MODULE NAME: RES_2_I_RES1
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 14
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
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
void  RES_2_I_RES1(char *, int, int, int, double, double, double, double, double, double, double, double, double, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: RES_2_I_RES1_org
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 14
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
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
void  RES_2_I_RES1_org(char *, int, int, int, double, double, double, double, double, double, double, double, double, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: RES_2_I_RES1_SMU
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 14
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
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
void  RES_2_I_RES1_SMU(char *, int, int, int, double, double, double, double, double, double, double, double, double, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: resv1
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
double resv1(int hi, int lo, int subst, double v);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ron9
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
#include <ktxe_proto.h>
#include <ksox_def.h>
#include <kui_proto.h>
	END USRLIB MODULE INFORMATION
*/
double ron9(int DRN1, int DRN2, int GATE, int SRC1, int SRC2, int SUB, double VDS, double VGS);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Rsheet_wid_debug
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 24
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		forcehi,	int,	Input,	,	,	
		meashi,	int,	Input,	,	,	
		forcelo,	int,	Input,	,	,	
		measlo,	int,	Input,	,	,	
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
		FH_V,	double *,	Output,	,	,	
		FH_I,	double *,	Output,	,	,	
		SH_V,	double *,	Output,	,	,	
		SH_I,	double *,	Output,	,	,	
		result2,	double *,	Output,	,	,	
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
void Rsheet_wid_debug(char * devname, int forcehi, int meashi, int forcelo, int measlo, int sub, int bulk, int chuckcon, double width, double length, char type, double iforce, double vlimit, double vsub, double delaytime, double intrange, double mrange, double lorange, double * FH_V, double * FH_I, double * SH_V, double * SH_I, double * result2, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Rsheet_wid_hae
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
void Rsheet_wid_hae(char * devname, int forcehi, int meashi, int forcelo, int measlo, int sub, int bulk, int chuckcon, double width, double length, char type, double iforce, double vlimit, double vsub, double delaytime, double intrange, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: sslp
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
double sslp(int drain, int gate, int source, int subst, int grnd1, int grnd2, int grnd3, double vlow, double vhigh, double vds, double vbs, double ithr1, double ithr2, double ilim, double mult, int niter);


/* USRLIB MODULE INFORMATION

	MODULE NAME: sub_sch
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 23
	ARGUMENTS:
		anode,	int,	Input,	,	,	
		cathode,	int,	Input,	,	,	
		subst,	int,	Input,	-1,	,	
		chuckcon,	int,	Input,	-1,	,	
		bulk,	int,	Input,	-1,	,	
		gnd1,	int,	Input,	-1,	,	
		gnd2,	int,	Input,	-1,	,	
		ano_width,	double,	Input,	1,	,	
		ano_length,	double,	Input,	1,	,	
		vforce_subst,	double,	Input,	0,	,	
		vforce_cathode,	double,	Input,	0,	,	
		vstart,	double,	Input,	0,	,	
		vstop,	double,	Input,	50,	,	
		nstep,	int,	Input,	,	,	
		fipgm,	double,	Input,	1e-6,	,	
		ipgm,	double,	Input,	100e-6,	,	
		udelay,	double,	Input,	0.01,	,	
		type,	char,	Input,	,	,	
		sweep,	char,	Input,	,	,	
		spot,	char,	Input,	,	,	
		first_volt,	double *,	Output,	,	,	
		sub_imax,	double *,	Output,	,	,	
		sub_vpgm,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <ktxe_proto.h>
#include <ksox_def.h>
#include <kui_proto.h>
#include <stdlib.h>

	END USRLIB MODULE INFORMATION
*/
double sub_sch(int anode, int cathode, int subst, int chuckcon, int bulk, int gnd1, int gnd2, double ano_width, double ano_length, double vforce_subst, double vforce_cathode, double vstart, double vstop, int nstep, double fipgm, double ipgm, double udelay, char type, char sweep, char spot, double * first_volt, double * sub_imax, double sub_vpgm);


/* USRLIB MODULE INFORMATION

	MODULE NAME: sweep_data
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 2
	ARGUMENTS:
		fp_name,	char *,	Output,	,	,	
		info,	char *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <ksox_def.h>
	END USRLIB MODULE INFORMATION
*/
void sweep_data(char * fp_name, char * info);


/* USRLIB MODULE INFORMATION

	MODULE NAME: tester_search
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <unistd.h>
#include <netdb.h>
	END USRLIB MODULE INFORMATION
*/
double  tester_search();

/* USRLIB MODULE INFORMATION

	MODULE NAME: tmp_cap_2spo_cap
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
#include <kui_proto.h>
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
void tmp_cap_2spo_cap(char * devname, int hi, int lo, int sub, int chuckcon, double area, double per, char tty, char bty, double vacc, double vinv, double freq, double sig, double stray, int integ, int ddebug, double mrange, double lorange, double * ca, double * ta, double * ci, double * ti);


/* USRLIB MODULE INFORMATION

	MODULE NAME: uugoi
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 28
	ARGUMENTS:
		Top,	int,	Input,	,	,	
		Bot,	int,	Input,	,	,	
		Sub,	int,	Input,	,	,	
		Chuck,	int,	Input,	,	,	
		gnd1,	int,	Input,	,	,	
		gnd2,	int,	Input,	,	,	
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
void uugoi(int Top, int Bot, int Sub, int Chuck, int gnd1, int gnd2, double Vstart, double Vstop, double Vstep, double Hold, double Delay, double Hc, double Mc, double Lc, double Hr, double Mr, double Lr, double M_i, double M_q, int Intrange, double Area, double Per, char Tty, char Bty, double * Vlf, double * Vfail, double * Ifail, double * Qbd);


/* USRLIB MODULE INFORMATION

	MODULE NAME: v2i_iramp
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
void v2i_iramp(char * devname, int hi, int lo, int sub, int bulk, int chuckcon, double width, double length, char type, double istart, double istop, double istep, double vlimit, double vsub, double delta, double delaytime, double intrange, double mrange, double lorange, double * vbd, double * ibd, double * vbd1);


/* USRLIB MODULE INFORMATION

	MODULE NAME: v4i
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 23
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		drain,	int,	Input,	-1,	,	
		gate,	int,	Input,	-1,	,	
		source,	int,	Input,	-1,	,	
		subst,	int,	Input,	-1,	,	
		g1,	int,	Input,	-1,	,	
		g2,	int,	Input,	-1,	,	
		g3,	int,	Input,	-1,	,	
		plc_val,	double,	Input,	1,	,	
		ilimit,	double,	Input,	1e-5,	,	
		lo_range,	double,	Input,	1e-8,	,	
		delay_time,	double,	Input,	0.2,	,	
		vds,	double,	Input,	0,	,	
		vgs,	double,	Input,	8,	,	
		vso,	double,	Input,	0,	,	
		vsu,	double,	Input,	0,	,	
		vdstep,	double,	Input,	0.3,	,	
		sdelay,	double,	Input,	0.005,	,	
		mode,	int,	Input,	0,	,	
		sweep,	char,	Input,	'p',	,	
		noise,	char,	Input,	'o',	,	
		sd,	char,	Input,	'd',	,	
		debug,	int,	Input,	1,	,	
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
	END USRLIB MODULE INFORMATION
*/
double v4i(char * devname, int drain, int gate, int source, int subst, int g1, int g2, int g3, double plc_val, double ilimit, double lo_range, double delay_time, double vds, double vgs, double vso, double vsu, double vdstep, double sdelay, int mode, char sweep, char noise, char sd, int debug);


/* USRLIB MODULE INFORMATION

	MODULE NAME: vp_jfet
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 23
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		gate,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		bulk,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		bgate,	int,	Input,	,	,	
		width,	double,	Input,	,	,	
		length,	double,	Input,	,	,	
		vdsmin,	double,	Input,	,	,	
		vdsmax,	double,	Input,	,	,	
		vdstep,	double,	Input,	,	,	
		ilimit,	double,	Input,	,	,	
		icomp,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		vbulk,	double,	Input,	,	,	
		vgate,	double,	Input,	,	,	
		sd,	char,	Input,	,	,	
		Bvdss,	double *,	Output,	,	,	
		Id,	double *,	Output,	,	,	
		Is,	double *,	Output,	,	,	
		count1,	int,	Input,	,	,	
		type,	char,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <stdlib.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void vp_jfet(char * devname, int gate, int drain, int source, int bulk, int sub, int bgate, double width, double length, double vdsmin, double vdsmax, double vdstep, double ilimit, double icomp, double intrange, double vbulk, double vgate, char sd, double * Bvdss, double * Id, double * Is, int count1, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: vp_jfet_r0
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 27
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		bulk,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		bgate,	int,	Input,	,	,	
		width,	double,	Input,	,	,	
		length,	double,	Input,	,	,	
		vdsmin,	double,	Input,	,	,	
		vdsmax,	double,	Input,	,	,	
		vdstep,	double,	Input,	,	,	
		ilimit,	double,	Input,	,	,	
		icomp,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		sdelay,	double,	Input,	,	,	
		steps,	double,	Input,	,	,	
		loop,	int,	Input,	,	,	
		sd,	char,	Input,	,	,	
		flag,	char,	Input,	,	,	
		type,	char,	Input,	,	,	
		vbulk,	double,	Input,	,	,	
		vdrain,	double,	Input,	,	,	
		debug,	int,	Input,	,	,	
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
#include <ksox_def.h>
#include <kui_proto.h>
	END USRLIB MODULE INFORMATION
*/
void vp_jfet_r0(char * devname, int drain, int gate, int source, int bulk, int sub, int bgate, double width, double length, double vdsmin, double vdsmax, double vdstep, double ilimit, double icomp, double intrange, double sdelay, double steps, int loop, char sd, char flag, char type, double vbulk, double vdrain, int debug, double * Bvdss, double * Id, double * Is);


/* USRLIB MODULE INFORMATION

	MODULE NAME: vpin_jfet
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 28
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	-1,	,	
		bulk,	int,	Input,	-1,	,	
		sub,	int,	Input,	-1,	,	
		bgate,	int,	Input,	-1,	,	
		width,	double,	Input,	1,	,	
		length,	double,	Input,	1,	,	
		vdsmin,	double,	Input,	0,	,	
		vdsmax,	double,	Input,	2,	,	
		vdstep,	double,	Input,	0.1,	,	
		ilimit,	double,	Input,	0.001,	,	
		icomp,	double,	Input,	0.001,	,	
		intrange,	double,	Input,	1,	,	
		sdelay,	double,	Input,	0.01,	,	
		steps,	double,	Input,	0.1,	,	
		loop,	int,	Input,	100,	,	
		sd,	char,	Input,	's',	,	
		mode,	char,	Input,	'p',	,	
		type,	char,	Input,	'n',	,	
		vbulk,	double,	Input,	0,	,	
		vdrain,	double,	Input,	0,	,	
		vgate,	double,	Input,	0,	,	
		debug,	int,	Input,	1,	,	
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
#include <ksox_def.h>
#include <kui_proto.h>
	END USRLIB MODULE INFORMATION
*/
void vpin_jfet(char * devname, int drain, int gate, int source, int bulk, int sub, int bgate, double width, double length, double vdsmin, double vdsmax, double vdstep, double ilimit, double icomp, double intrange, double sdelay, double steps, int loop, char sd, char mode, char type, double vbulk, double vdrain, double vgate, int debug, double * Bvdss, double * Id, double * Is);


/* USRLIB MODULE INFORMATION

	MODULE NAME: vt_gm_mos
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
double vt_gm_mos(int drain, int gate, int source, int subst, double w, double l, double vlow, double vhigh, double vds, double vbs, double ithr, double vstep, int npts, double * slope, int * kflag);


/* USRLIB MODULE INFORMATION

	MODULE NAME: vt_icst_extra
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 23
	ARGUMENTS:
		drain,	int,	Input,	-1,	,	
		gate,	int,	Input,	-1,	,	
		source,	int,	Input,	-1,	,	
		sub,	int,	Input,	-1,	,	
		addp1,	int,	Input,	-1,	,	
		gnd1,	int,	Input,	-1,	,	
		gnd2,	int,	Input,	-1,	,	
		width,	double,	Input,	1,	,	
		length,	double,	Input,	1,	,	
		vmin,	double,	Input,	0,	,	
		vmax,	double,	Input,	2,	,	
		step,	int,	Input,	10,	,	
		idsearch,	double,	Input,	0.1e-6,	,	
		vds,	double,	Input,	0.1,	,	
		vsub,	double,	Input,	0,	,	
		vadd,	double,	Input,	0,	,	
		multiplier,	double,	Input,	1,	,	
		intrange,	int,	Input,	0,	,	
		delaytime,	double,	Input,	0.001,	,	
		lorange,	double,	Input,	1e-8,	,	
		intg,	int,	Input,	0,	,	
		mtime,	int,	Input,	0,	,	
		vtextra,	double *,	Output,	,	,	
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
	END USRLIB MODULE INFORMATION
*/
double  vt_icst_extra(int, int, int, int, int, int, int, double, double, double, double, int, double, double, double, double, double, int, double, double, int, int, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: vt_lin_hae
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
		bgate,	int,	Input,	,	,	
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
void vt_lin_hae(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, int bgate, double width, double length, char type, double vmin, double vmax, int npoints, double idmax, double vds, double vbs, double mvtx, double mk, double intrange, double mrange, double lorange, double * result_vt, double * result_k);


/* USRLIB MODULE INFORMATION

	MODULE NAME: vt_lin_mos_HV
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 28
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
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
		idcomp,	double,	Input,	,	,	
		igcomp,	double,	Input,	,	,	
		ibcomp,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vbs,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		sdelay,	double,	Input,	,	,	
		dbug,	int,	Input,	,	,	
		multiplier,	int,	Input,	,	,	
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
void vt_lin_mos_HV(char * devname, int drain, int gate, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vmin, double vmax, int npoints, double idmax, double idcomp, double igcomp, double ibcomp, double vds, double vbs, double intrange, double mrange, double lorange, double sdelay, int dbug, int multiplier, double * Vtl, double * K, double * Gm);


/* USRLIB MODULE INFORMATION

	MODULE NAME: vt_lin_raw
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
double vt_lin_raw(int drain, int gate, int source, int subst, double w, double l, double vlow, double vhigh, double vds, double vbs, double ithr, double vstep, int npts, double * slope, int * kflag);


/* USRLIB MODULE INFORMATION

	MODULE NAME: vtati_h
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
double vtati_h(int drain, int gate, int source, int subst, double vlow, double vhigh, double vds, double vbs, double ithr, int niter);


/* USRLIB MODULE INFORMATION

	MODULE NAME: vtext4_dptr
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
double vtext4_dptr(int drain, int gate, int source, int subst, double w, double l, double vlow, double vhigh, double vds, double vbs, double ithr, double vstep, int npts, double * slope, int * kflag);


/* USRLIB MODULE INFORMATION

	MODULE NAME: vtext4_hae
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 16
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
		init_v,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double vtext4_hae(int drain, int gate, int source, int subst, double w, double l, double vlow, double vhigh, double vds, double vbs, double ithr, double vstep, int npts, double * slope, int * kflag, double * init_v);


/* USRLIB MODULE INFORMATION

	MODULE NAME: vtext6
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
		vsubst,	double,	Input,	0,	,	
		ithr,	double,	Input,	40e-09,	,	
		vstep,	double,	Input,	0.05,	,	
		npts,	int,	Input,	15,	,	
		slope,	double *,	Output,	,	,	
		kflag,	int *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double vtext6(int drain, int gate, int source, int body, int subst, int g1, double w, double l, double vlow, double vhigh, double vds, double vsubst, double ithr, double vstep, int npts, double * slope, int * kflag);


/* USRLIB MODULE INFORMATION

	MODULE NAME: vtexts6
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 17
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		body,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		g1,	int,	Input,	,	,	
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
double vtexts6(int drain, int gate, int source, int body, int subst, int g1, double w, double l, double vlow, double vhigh, double vds, double vbs, double ithr, double vstep, int npts, double * slope, int * kflag);


/* USRLIB MODULE INFORMATION

	MODULE NAME: vtexts_hv
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
double vtexts_hv(int drain, int gate, int source, int subst, double w, double l, double vlow, double vhigh, double vds, double vbs, double ithr, double vstep, int npts, double * slope, int * kflag);


/* USRLIB MODULE INFORMATION

	MODULE NAME: vtextsS
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
double vtextsS(int drain, int gate, int source, int subst, double w, double l, double vlow, double vhigh, double vds, double vbs, double ithr, double vstep, int npts, double * slope, int * kflag);


/* USRLIB MODULE INFORMATION

	MODULE NAME: vti_eep
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 16
	ARGUMENTS:
		CG,	int,	Input,	-1,	,	
		TG,	int,	Input,	-1,	,	
		Drain,	int,	Input,	-1,	,	
		Source,	int,	Input,	-1,	,	
		STR,	int,	Input,	-1,	,	
		GND1,	int,	Input,	-1,	,	
		GND2,	int,	Input,	-1,	,	
		vlow,	double,	Input,	0,	,	
		vhigh,	double,	Input,	2,	,	
		ith,	double,	Input,	1e-6,	,	
		VDS,	double,	Input,	0.1,	,	
		VTG,	double,	Input,	0,	,	
		VSTR,	double,	Input,	0,	,	
		Delaytime,	double,	Input,	0.005,	,	
		Intrange,	int,	Input,	1,	,	
		vstep,	double,	Input,	0.05,	,	
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
double vti_eep(int CG, int TG, int Drain, int Source, int STR, int GND1, int GND2, double vlow, double vhigh, double ith, double VDS, double VTG, double VSTR, double Delaytime, int Intrange, double vstep);


/* USRLIB MODULE INFORMATION

	MODULE NAME: vti_eep2
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 19
	ARGUMENTS:
		Devname,	char *,	Input,	"EEP",	,	
		CG,	int,	Input,	-1,	,	
		TG,	int,	Input,	-1,	,	
		Drain,	int,	Input,	-1,	,	
		Source,	int,	Input,	-1,	,	
		Str,	int,	Input,	-1,	,	
		Gnd1,	int,	Input,	-1,	,	
		Gnd2,	int,	Input,	-1,	,	
		vlow,	double,	Input,	0,	,	
		vhigh,	double,	Input,	2,	,	
		Ith,	double,	Input,	1e-6,	,	
		Vds,	double,	Input,	0.1,	,	
		Vtg,	double,	Input,	0,	,	
		Vstr,	double,	Input,	0,	,	
		rDelay,	double,	Input,	0.005,	,	
		iNtrange,	int,	Input,	1,	,	
		LoR,	double,	Input,	0,	,	
		mode,	char,	Input,	'x',	,	
		vstep,	double,	Input,	0.05,	,	
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
	END USRLIB MODULE INFORMATION
*/
double vti_eep2(char * Devname, int CG, int TG, int Drain, int Source, int Str, int Gnd1, int Gnd2, double vlow, double vhigh, double Ith, double Vds, double Vtg, double Vstr, double rDelay, int iNtrange, double LoR, char mode, double vstep);


/* USRLIB MODULE INFORMATION

	MODULE NAME: vti_mosS
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 19
	ARGUMENTS:
		Devname,	char *,	Input,	"EEP",	,	
		Drain,	int,	Input,	-1,	,	
		Gate,	int,	Input,	-1,	,	
		Source,	int,	Input,	-1,	,	
		Sub,	int,	Input,	-1,	,	
		Body,	int,	Input,	-1,	,	
		Gnd1,	int,	Input,	-1,	,	
		Gnd2,	int,	Input,	-1,	,	
		Vgmin,	double,	Input,	0,	,	
		Vgmax,	double,	Input,	2,	,	
		ith,	double,	Input,	1e-6,	,	
		Vds,	double,	Input,	0.1,	,	
		Vsubst,	double,	Input,	0,	,	
		Vbody,	double,	Input,	0,	,	
		Delaytime,	double,	Input,	0.005,	,	
		Intrange,	int,	Input,	1,	,	
		lo_range,	double,	Input,	0,	,	
		mode,	char,	Input,	'x',	,	
		vstep,	double,	Input,	0.05,	,	
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
double vti_mosS(char * Devname, int Drain, int Gate, int Source, int Sub, int Body, int Gnd1, int Gnd2, double Vgmin, double Vgmax, double ith, double Vds, double Vsubst, double Vbody, double Delaytime, int Intrange, double lo_range, char mode, double vstep);


/* USRLIB MODULE INFORMATION

	MODULE NAME: vtlin_mos
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 19
	ARGUMENTS:
		Devname,	char *,	Input,	"EEP",	,	
		Drain,	int,	Input,	-1,	,	
		Gate,	int,	Input,	-1,	,	
		Source,	int,	Input,	-1,	,	
		Sub,	int,	Input,	-1,	,	
		Body,	int,	Input,	-1,	,	
		Gnd1,	int,	Input,	-1,	,	
		Gnd2,	int,	Input,	-1,	,	
		Vgmin,	double,	Input,	0,	,	
		Vgmax,	double,	Input,	2,	,	
		ith,	double,	Input,	1e-6,	,	
		Vds,	double,	Input,	0.1,	,	
		Vsubst,	double,	Input,	0,	,	
		Vbody,	double,	Input,	0,	,	
		Delaytime,	double,	Input,	0.005,	0,	1
		Intrange,	int,	Input,	1,	0,	10
		lo_range,	double,	Input,	0,	,	
		noise,	char,	Input,	'x',	,	
		vstep,	double,	Input,	0.05,	,	
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
#include <stdlib.h>    
	END USRLIB MODULE INFORMATION
*/
double  vtlin_mos(char *, int, int, int, int, int, int, int, double, double, double, double, double, double, double, int, double, char, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: vtlin_mos_org
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 19
	ARGUMENTS:
		Devname,	char *,	Input,	"EEP",	,	
		Drain,	int,	Input,	-1,	,	
		Gate,	int,	Input,	-1,	,	
		Source,	int,	Input,	-1,	,	
		Sub,	int,	Input,	-1,	,	
		Body,	int,	Input,	-1,	,	
		Gnd1,	int,	Input,	-1,	,	
		Gnd2,	int,	Input,	-1,	,	
		Vgmin,	double,	Input,	0,	,	
		Vgmax,	double,	Input,	2,	,	
		ith,	double,	Input,	1e-6,	,	
		Vds,	double,	Input,	0.1,	,	
		Vsubst,	double,	Input,	0,	,	
		Vbody,	double,	Input,	0,	,	
		Delaytime,	double,	Input,	0.005,	0,	1
		Intrange,	int,	Input,	1,	0,	10
		lo_range,	double,	Input,	0,	,	
		noise,	char,	Input,	'x',	,	
		vstep,	double,	Input,	0.05,	,	
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
#include <stdlib.h>    
	END USRLIB MODULE INFORMATION
*/
double  vtlin_mos_org(char *, int, int, int, int, int, int, int, double, double, double, double, double, double, double, int, double, char, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: vtsslp_mos1
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
void vtsslp_mos1(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vmin, double vmax, double idsearch, double vds, double vbs, double multiplier, double intrange, double delaytime, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: vtsslp_sq_mos1
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
void vtsslp_sq_mos1(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vmin, double vmax, double idsearch, double vds, double vbs, double multiplier, double intrange, double delaytime, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: xbvdss2_hae
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 22
	ARGUMENTS:
		d,	int,	Input,	,	,	
		g,	int,	Input,	,	,	
		s,	int,	Input,	,	,	
		sub,	int,	Input,	-1,	,	
		bulk,	int,	Input,	-1,	,	
		checkon,	int,	Input,	-1,	,	
		gnd1,	int,	Input,	-1,	,	
		gnd2,	int,	Input,	-1,	,	
		vgs,	double,	Input,	,	,	
		vgs_limit,	double,	Input,	,	,	
		plc_val,	double,	Input,	1,	,	
		vdsstart,	double,	Input,	0,	,	
		vdsstop,	double,	Input,	30,	,	
		nstep,	int,	Input,	300,	,	
		first_ipgm,	double,	Input,	1e-9,	,	
		ipgm,	double,	Input,	1e-6,	,	
		udelay,	double,	Input,	0.01,	,	
		type,	char,	Input,	'n',	,	
		mode,	char,	Input,	'p',	,	
		flag,	char,	Input,	'p',	,	
		debug,	int,	Input,	1,	,	
		first_value,	double *,	Output,	,	,	
	INCLUDES:
#include <math.h>		                
#include "lptdef.h"
#include <stdlib.h>
#include "PARLIB400_proto.h"
	
	END USRLIB MODULE INFORMATION
*/
double xbvdss2_hae(int d, int g, int s, int sub, int bulk, int checkon, int gnd1, int gnd2, double vgs, double vgs_limit, double plc_val, double vdsstart, double vdsstop, int nstep, double first_ipgm, double ipgm, double udelay, char type, char mode, char flag, int debug, double * first_value);


/* USRLIB MODULE INFORMATION

	MODULE NAME: xbvdss2_jfet
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 16
	ARGUMENTS:
		d,	int,	Input,	,	,	
		g,	int,	Input,	,	,	
		s,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		bgate,	int,	Input,	,	,	
		vdsstart,	double,	Input,	,	,	
		vdsstop,	double,	Input,	,	,	
		vbgate,	double,	Input,	,	,	
		vgate,	double,	Input,	,	,	
		ibgcomp,	double,	Input,	,	,	
		igcomp,	double,	Input,	,	,	
		nstep,	int,	Input,	,	,	
		debug,	int,	Input,	,	,	
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
double xbvdss2_jfet(int d, int g, int s, int sub, int bgate, double vdsstart, double vdsstop, double vbgate, double vgate, double ibgcomp, double igcomp, int nstep, int debug, double ipgm, double udelay, char type);


