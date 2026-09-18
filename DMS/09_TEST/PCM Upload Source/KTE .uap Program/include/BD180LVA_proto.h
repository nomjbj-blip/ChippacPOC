/* BD180LVA function prototype and KITT header file */

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

	MODULE NAME: BVCS2
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 16
	ARGUMENTS:
		drain,	int,	Input,	-1,	,	
		gate,	int,	Input,	-1,	,	
		source,	int,	Input,	-1,	,	
		body,	int,	Input,	-1,	,	
		sub,	int,	Input,	-1,	,	
		iso,	int,	Input,	-1,	,	
		gnd1,	int,	Input,	-1,	,	
		gnd2,	int,	Input,	-1,	,	
		Itg,	double,	Input,	10e-9,	,	
		Vmax,	double,	Input,	99,	,	
		Vgate,	double,	Input,	0,	,	
		Vbody,	double,	Input,	0,	,	
		Viso,	double,	Input,	0,	,	
		Auto_range,	int,	Input,	1,	,	
		Delay,	double,	Input,	0.001,	,	
		Intrange,	int,	Input,	1,	,	
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
double  BVCS2(int, int, int, int, int, int, int, int, double, double, double, double, double, int, double, int);

/* USRLIB MODULE INFORMATION

	MODULE NAME: BVextra
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 19
	ARGUMENTS:
		drain,	int,	Input,	-1,	,	
		gate,	int,	Input,	-1,	,	
		source,	int,	Input,	-1,	,	
		body,	int,	Input,	-1,	,	
		sub,	int,	Input,	-1,	,	
		iso,	int,	Input,	-1,	,	
		gnd1,	int,	Input,	-1,	,	
		gnd2,	int,	Input,	-1,	,	
		Vstart,	double,	Input,	0,	,	
		Vstop,	double,	Input,	100,	,	
		Vstep,	double,	Input,	1,	,	
		Itg,	double,	Input,	1e-6,	,	
		Vgate,	double,	Input,	0,	,	
		Vbody,	double,	Input,	0,	,	
		Viso,	double,	Input,	0,	,	
		Auto_range,	int,	Input,	1,	,	
		Delay,	double,	Input,	0.001,	,	
		Extra,	int,	Input,	0,	,	
		Intrange,	int,	Input,	1,	,	
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
double  BVextra(int, int, int, int, int, int, int, int, double, double, double, double, double, double, double, int, double, int, int);

/* USRLIB MODULE INFORMATION

	MODULE NAME: id_opt
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 19
	ARGUMENTS:
		drain,	int,	Input,	-1,	,	
		gate,	int,	Input,	-1,	,	
		source,	int,	Input,	-1,	,	
		body,	int,	Input,	-1,	,	
		sub,	int,	Input,	-1,	,	
		iso,	int,	Input,	-1,	,	
		gnd1,	int,	Input,	-1,	,	
		gnd2,	int,	Input,	-1,	,	
		Icomp,	double,	Input,	1e-2,	,	
		Vdrain,	double,	Input,	0,	,	
		Vgate,	double,	Input,	0,	,	
		Vbody,	double,	Input,	0,	,	
		Viso,	double,	Input,	0,	,	
		Auto_range,	int,	Input,	1,	,	
		Id_range,	double,	Input,	1e-8,	,	
		Delay,	double,	Input,	0.01,	,	
		Intg,	int,	Input,	1,	,	
		unit_calc,	double,	Input,	1e+6,	,	
		width,	double,	Input,	1,	,	
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
double  id_opt(int, int, int, int, int, int, int, int, double, double, double, double, double, int, double, double, int, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: vticst_ext
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
		vgout,	double *,	Output,	,	,	
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
double  vticst_ext(int, int, int, int, int, int, int, double, double, double, double, int, double, double, double, double, double, int, double, double, int, int, double *);

