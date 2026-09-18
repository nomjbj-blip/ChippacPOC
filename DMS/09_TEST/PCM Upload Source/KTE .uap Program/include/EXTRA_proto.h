/* EXTRA function prototype and KITT header file */

/* COPY OF USRLIB SETTINGS INFORMATION */
/* [VISIBILITY] */
/* HIDDEN=0 */
/* [DEPENDENCIES] */
/* LIBS= */
/* OBJECTS= */
/* HEADERS= */
/* [NON-KULT SEARCH PATH] */
/* PATHS= */

/* USRLIB MODULE INFORMATION

	MODULE NAME: vticst_extra
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 21
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		gnd1,	int,	Input,	,	,	
		gnd2,	int,	Input,	,	,	
		width,	double,	Input,	,	,	
		length,	double,	Input,	,	,	
		vmin,	double,	Input,	,	,	
		vmax,	double,	Input,	,	,	
		step,	int,	Input,	,	,	
		idsearch,	double,	Input,	,	,	
		ilimit,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vsource,	double,	Input,	,	,	
		vsub,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		delaytime,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		intg,	int,	Input,	,	,	
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
double  vticst_extra(int, int, int, int, int, int, double, double, double, double, int, double, double, double, double, double, double, double, double, int, double *);

