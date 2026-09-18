/* algo_hc function prototype and KITT header file */

/* COPY OF USRLIB SETTINGS INFORMATION */
/* [VISIBILITY] */
/* HIDDEN=0 */
/* [DEPENDENCIES] */
/* LIBS=-lHP8110u -lktest -lLBC5 -loptlib */
/* OBJECTS= */
/* HEADERS= */
/* [NON-KULT SEARCH PATH] */
/* PATHS= */

/* USRLIB MODULE INFORMATION

	MODULE NAME: bv_ith_eng
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 17
	ARGUMENTS:
		Pm,	int,	Input,	-1,	,	
		PF2,	int,	Input,	-1,	,	
		PF3,	int,	Input,	-1,	,	
		PF4,	int,	Input,	-1,	,	
		gnd1,	int,	Input,	-1,	,	
		gnd2,	int,	Input,	-1,	,	
		gnd3,	int,	Input,	-1,	,	
		gnd4,	int,	Input,	-1,	,	
		type,	char,	Input,	'N',	,	
		ith,	double,	Input,	1e-6,	,	
		vmax,	double,	Input,	50,	0,	150
		PF2_v,	double,	Input,	0,	,	
		PF3_v,	double,	Input,	0,	,	
		PF4_v,	double,	Input,	0,	,	
		intrange,	int,	Input,	1,	,	
		delay,	double,	Input,	0.05,	,	
		debug,	int,	Input,	0,	,	
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
double  bv_ith_eng(int, int, int, int, int, int, int, int, char, double, double, double, double, double, int, double, int);

/* USRLIB MODULE INFORMATION

	MODULE NAME: BvceoS_eng
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
double  BvceoS_eng(int, int, int, int, double, double, double, double, char, double, double, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: BvceoS_eng2
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
double  BvceoS_eng2(int, int, int, int, double, double, double, double, char, double, double, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: DPeeprom_smu
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 12
	ARGUMENTS:
		cg,	int,	Input,	-1,	,	
		sg,	int,	Input,	-1,	,	
		drain,	int,	Input,	-1,	,	
		sub,	int,	Input,	-1,	,	
		gnd1,	int,	Input,	-1,	,	
		gnd2,	int,	Input,	-1,	,	
		gnd3,	int,	Input,	-1,	,	
		stress_t,	double,	Input,	0.01,	,	
		vcg,	double,	Input,	5,	,	
		vsg,	double,	Input,	0,	,	
		vdrain,	double,	Input,	0,	,	
		vsub,	double,	Input,	0,	,	
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
double  DPeeprom_smu(int, int, int, int, int, int, int, double, double, double, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: DPeeprom_sync_smu
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 13
	ARGUMENTS:
		cg,	int,	Input,	-1,	,	
		sg,	int,	Input,	-1,	,	
		drain,	int,	Input,	-1,	,	
		sub,	int,	Input,	-1,	,	
		gnd1,	int,	Input,	-1,	,	
		gnd2,	int,	Input,	-1,	,	
		gnd3,	int,	Input,	-1,	,	
		stress_t,	double,	Input,	0.01,	,	
		vcg,	double,	Input,	5,	,	
		vsg,	double,	Input,	0,	,	
		vdrain,	double,	Input,	0,	,	
		vsub,	double,	Input,	0,	,	
		drain_sub_sync,	char,	Input,	'n',	,	
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
double  DPeeprom_sync_smu(int, int, int, int, int, int, int, double, double, double, double, double, char);

/* USRLIB MODULE INFORMATION

	MODULE NAME: eeprom_smu
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 12
	ARGUMENTS:
		cg,	int,	Input,	-1,	,	
		tg,	int,	Input,	-1,	,	
		sg,	int,	Input,	-1,	,	
		sub,	int,	Input,	-1,	,	
		gnd1,	int,	Input,	-1,	,	
		gnd2,	int,	Input,	-1,	,	
		gnd3,	int,	Input,	-1,	,	
		stress_t,	double,	Input,	0.01,	,	
		vcg,	double,	Input,	5,	,	
		vtg,	double,	Input,	0,	,	
		vsg,	double,	Input,	0,	,	
		vsub,	double,	Input,	0,	,	
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
double  eeprom_smu(int, int, int, int, int, int, int, double, double, double, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: gmlin5_hc
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
double  gmlin5_hc(int, int, int, int, int, int, double, double, double, double, double, double, double, double, int, double *, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: i4v_2gate
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 19
	ARGUMENTS:
		drain,	int,	Input,	-1,	,	
		gate,	int,	Input,	-1,	,	
		source,	int,	Input,	-1,	,	
		body,	int,	Input,	-1,	,	
		subst,	int,	Input,	-1,	,	
		gate_add,	int,	Input,	-1,	,	
		gnd1,	int,	Input,	-1,	,	
		gnd2,	int,	Input,	-1,	,	
		plc_val,	double,	Input,	1,	,	
		ilimit,	double,	Input,	1e-3,	,	
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
double  i4v_2gate(int, int, int, int, int, int, int, int, double, double, double, double, double, double, double, double, char, char, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: pulse_2c_eng
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 25
	ARGUMENTS:
		Devname,	char *,	Input,	"ee",	,	
		hi,	int,	Input,	-1,	,	
		lo,	int,	Input,	-1,	,	
		s_hi,	int,	Input,	-1,	,	
		s_lo,	int,	Input,	-1,	,	
		Psmu1,	int,	Input,	-1,	,	
		Psmu2,	int,	Input,	-1,	,	
		Psmu3,	int,	Input,	-1,	,	
		Psmu4,	int,	Input,	-1,	,	
		g1,	int,	Input,	-1,	,	
		g2,	int,	Input,	-1,	,	
		g3,	int,	Input,	-1,	,	
		g4,	int,	Input,	-1,	,	
		hi_delay,	double,	Input,	0,	,	
		hi_height,	double,	Input,	1,	-20,	20
		hi_width,	double,	Input,	1e-3,	,	
		lo_delay,	double,	Input,	0,	,	
		lo_height,	double,	Input,	2,	-20,	20
		lo_width,	double,	Input,	1e-3,	,	
		rise,	double,	Input,	1e-8,	1e-8,	1e-7
		fall,	double,	Input,	1e-8,	1e-8,	1e-7
		smu1_v,	double,	Input,	0,	,	
		smu2_v,	double,	Input,	0,	,	
		smu3_v,	double,	Input,	0,	,	
		smu4_v,	double,	Input,	0,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <HP8110_proto.h>
#include "pulsestruct.h"
extern pulse_struct *currentpulse;
#define MAX(a,b)        ((a)>(b) ? (a) : (b))
	END USRLIB MODULE INFORMATION
*/
void  pulse_2c_eng(char *, int, int, int, int, int, int, int, int, int, int, int, int, double, double, double, double, double, double, double, double, double, double, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: sweep_v4p
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 19
	ARGUMENTS:
		p1,	int,	Input,	-1,	,	
		p2,	int,	Input,	-1,	,	
		p3,	int,	Input,	-1,	,	
		p4,	int,	Input,	-1,	,	
		g1,	int,	Input,	-1,	,	
		g2,	int,	Input,	-1,	,	
		g3,	int,	Input,	-1,	,	
		plc_val,	double,	Input,	0,	,	
		ilimit,	double,	Input,	1e-2,	,	
		lo_range,	double,	Input,	1e-8,	,	
		auto_range,	char,	Input,	'y',	,	
		delay_time,	double,	Input,	0.001,	,	
		vp1start,	double,	Input,	0,	,	
		vp1stop,	double,	Input,	10,	,	
		vp2,	double,	Input,	0,	,	
		vp3,	double,	Input,	0,	,	
		vp4,	double,	Input,	0,	,	
		Vstep,	double,	Input,	0.03,	,	
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
	END USRLIB MODULE INFORMATION
*/
double  sweep_v4p(int, int, int, int, int, int, int, double, double, double, char, double, double, double, double, double, double, double, char);

/* USRLIB MODULE INFORMATION

	MODULE NAME: vt_icst_eng
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 22
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		addp1,	int,	Input,	,	,	
		gnd1,	int,	Input,	,	,	
		gnd2,	int,	Input,	,	,	
		width,	double,	Input,	,	,	
		length,	double,	Input,	,	,	
		vmin,	double,	Input,	,	,	
		vmax,	double,	Input,	,	,	
		step,	int,	Input,	,	,	
		idsearch,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vsub,	double,	Input,	,	,	
		vadd,	double,	Input,	,	,	
		multiplier,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		delaytime,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		intg,	int,	Input,	,	,	
		mtime,	int,	Input,	,	,	
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
double  vt_icst_eng(int, int, int, int, int, int, int, double, double, double, double, int, double, double, double, double, double, double, double, double, int, int);

/* USRLIB MODULE INFORMATION

	MODULE NAME: vt_icst_eng2
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 23
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		gate_add,	int,	Input,	,	,	
		addp1,	int,	Input,	,	,	
		gnd1,	int,	Input,	,	,	
		gnd2,	int,	Input,	,	,	
		width,	double,	Input,	,	,	
		length,	double,	Input,	,	,	
		vmin,	double,	Input,	,	,	
		vmax,	double,	Input,	,	,	
		step,	int,	Input,	,	,	
		idsearch,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vsub,	double,	Input,	,	,	
		vadd,	double,	Input,	,	,	
		multiplier,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		delaytime,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		intg,	int,	Input,	,	,	
		mtime,	int,	Input,	,	,	
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
double  vt_icst_eng2(int, int, int, int, int, int, int, int, double, double, double, double, int, double, double, double, double, double, double, double, double, int, int);

/* USRLIB MODULE INFORMATION

	MODULE NAME: vt_icst_extra_eng
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 23
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		addp1,	int,	Input,	,	,	
		gnd1,	int,	Input,	,	,	
		gnd2,	int,	Input,	,	,	
		width,	double,	Input,	,	,	
		length,	double,	Input,	,	,	
		vmin,	double,	Input,	,	,	
		vmax,	double,	Input,	,	,	
		step,	int,	Input,	,	,	
		idsearch,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vsub,	double,	Input,	,	,	
		vadd,	double,	Input,	,	,	
		multiplier,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		delaytime,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		intg,	int,	Input,	,	,	
		mtime,	int,	Input,	,	,	
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
double  vt_icst_extra_eng(int, int, int, int, int, int, int, double, double, double, double, int, double, double, double, double, double, double, double, double, int, int, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: vt_icst_extra_eng2
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 24
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		gate_add,	int,	Input,	,	,	
		addp1,	int,	Input,	,	,	
		gnd1,	int,	Input,	,	,	
		gnd2,	int,	Input,	,	,	
		width,	double,	Input,	,	,	
		length,	double,	Input,	,	,	
		vmin,	double,	Input,	,	,	
		vmax,	double,	Input,	,	,	
		step,	int,	Input,	,	,	
		idsearch,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vsub,	double,	Input,	,	,	
		vadd,	double,	Input,	,	,	
		multiplier,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		delaytime,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		intg,	int,	Input,	,	,	
		mtime,	int,	Input,	,	,	
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
double  vt_icst_extra_eng2(int, int, int, int, int, int, int, int, double, double, double, double, int, double, double, double, double, double, double, double, double, int, int, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: vtext4_hc_eng
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
double  vtext4_hc_eng(int, int, int, int, double, double, double, double, double, double, double, double, int, double *, int *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: vtexts6_hc
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
double  vtexts6_hc(int, int, int, int, int, int, double, double, double, double, double, double, double, double, int, double *, int *);

