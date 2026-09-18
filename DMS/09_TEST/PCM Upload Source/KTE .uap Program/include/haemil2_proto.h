/* haemil2 function prototype and KITT header file */

/* COPY OF USRLIB SETTINGS INFORMATION */
/* [VISIBILITY] */
/* HIDDEN=0 */
/* [DEPENDENCIES] */
/* LIBS=-loptlib */
/* OBJECTS= */
/* HEADERS= */
/* [NON-KULT SEARCH PATH] */
/* PATHS= */

/* USRLIB MODULE INFORMATION

	MODULE NAME: allgnd2
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		Hold,	double,	Input,	1,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <COM_usrlib.h>
	END USRLIB MODULE INFORMATION
*/
void  allgnd2(double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: betaG
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 25
	ARGUMENTS:
		devname,	char *,	Input,	"aaa",	,	
		em,	int,	Input,	-1,	,	
		ba,	int,	Input,	-1,	,	
		co,	int,	Input,	-1,	,	
		sub,	int,	Input,	-1,	,	
		vmin,	double,	Input,	0,	,	
		vmax,	double,	Input,	1,	,	
		vstep,	double,	Input,	0.05,	,	
		hfe_l,	double,	Input,	10e-9,	,	
		hfe_m,	double,	Input,	1e-9,	,	
		hfe_h,	double,	Input,	0.1e-6,	,	
		delay,	double,	Input,	0.01,	,	
		vc,	double,	Input,	0,	,	
		vb,	double,	Input,	0,	,	
		ve,	double,	Input,	0,	,	
		vsub,	double,	Input,	0,	,	
		plc,	int,	Input,	0,	,	
		type,	char,	Input,	'n',	,	
		ebc,	char,	Input,	'b',	,	
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
double  betaG(char *, int, int, int, int, double, double, double, double, double, double, double, double, double, double, double, int, char, char, double *, double *, double *, double *, double *, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: bvceo_miho6
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 13
	ARGUMENTS:
		devname,	char *,	Input,	"ceo",	,	
		em,	int,	Input,	-1,	,	
		ba,	int,	Input,	-1,	,	
		co,	int,	Input,	-1,	,	
		sub,	int,	Input,	-1,	,	
		lowt,	int,	Input,	0,	,	
		ipgm,	double,	Input,	1E-6,	,	
		vlimit,	double,	Input,	30,	,	
		nstep,	double,	Input,	200,	,	
		type,	char,	Input,	'n',	,	
		wtime,	double,	Input,	0.01,	,	
		vbb,	double,	Input,	0,	,	
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
double  bvceo_miho6(char *, int, int, int, int, int, double, double, double, char, double, double, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: bvdss_lkg
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
double  bvdss_lkg(char *, int, int, int, int, double, double, int, double, double, char);

/* USRLIB MODULE INFORMATION

	MODULE NAME: gmlin_thre2
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
double  gmlin_thre2(int, int, int, int, int, int, double, double, double, double, double, double, double, double, double, int, double, double, int, double *, int *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: ioh2
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 20
	ARGUMENTS:
		devname,	char *,	Input,	"ioh2",	,	
		d,	int,	Input,	-1,	,	
		g,	int,	Input,	-1,	,	
		s,	int,	Input,	-1,	,	
		b,	int,	Input,	-1,	,	
		g1,	int,	Input,	-1,	,	
		g2,	int,	Input,	-1,	,	
		vmin,	double,	Input,	0,	,	
		vmax,	double,	Input,	20,	,	
		vstep,	double,	Input,	0.3,	,	
		ith,	double,	Input,	1E-6,	,	
		vgs,	double,	Input,	0,	,	
		vbs,	double,	Input,	0,	,	
		type,	char,	Input,	'n',	,	
		plc,	double,	Input,	1,	,	
		delay,	double,	Input,	0.001,	,	
		fun,	int,	Input,	0,	,	
		sd,	char,	Input,	'd',	,	
		vds,	double,	Input,	0,	,	
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
double  ioh2(char *, int, int, int, int, int, int, double, double, double, double, double, double, char, double, double, int, char, double, char);

/* USRLIB MODULE INFORMATION

	MODULE NAME: ivh2
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 23
	ARGUMENTS:
		devname,	char *,	Input,	"ivh2",	,	
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
		unitc,	int,	Input,	0,	,	
		width,	double,	Input,	0,	,	
		hp,	double,	Input,	0,	,	
		vstep,	double,	Input,	0.3,	,	
		noise,	char,	Input,	'x',	,	
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
#include <COM_usrlib.h>
	END USRLIB MODULE INFORMATION
*/
double  ivh2(char *, int, int, int, int, int, int, int, int, char, double, double, double, double, double, double, double, char, int, double, double, double, char);

