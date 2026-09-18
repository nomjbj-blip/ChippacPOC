/* TTR_TIC05 function prototype and KITT header file */

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

	MODULE NAME: TTR_TIC05_fetcheck_mos
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
		result,	D_ARRAY_T,	Output,	 ,	 ,	 
		ArrSizeForParm22,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void  TTR_TIC05_fetcheck_mos(char *, int, int, int, int, int, int, double, double, char, double, double, double, double, double, double, double, double, double, double *, double *, double *, int);

