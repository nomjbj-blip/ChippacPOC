/* kjmtest function prototype and KITT header file */

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

	MODULE NAME: betajm
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
double betajm(int em, int base, int coll, int subst, double ic, double vce, double * vbeout, double * icout, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: betakjm
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 9
	ARGUMENTS:
		em,	int,	Input,	,	,	
		base,	int,	Input,	,	,	
		coll,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		ic,	double,	Input,	,	,	
		vce,	double,	Input,	,	,	
		vbeout,	double *,	Input,	,	,	
		icout,	double *,	Input,	,	,	
		type,	char,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double betakjm(int em, int base, int coll, int subst, double ic, double vce, double * vbeout, double * icout, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Fetcheck_DP1_mos
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
void Fetcheck_DP1_mos(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vdsf, double vgs_off, double vgs_on, double vbsf, double intrange, double mrange, double lorange, double * ioff, double * ids);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Fetcheck_DS1_mos
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
void Fetcheck_DS1_mos(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vdsf, double vgs_off, double vgs_on, double vbsf, double intrange, double mrange, double lorange, double * ioff, double * ids);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Fetcheck_x_mos
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
void Fetcheck_x_mos(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vdsf, double vgs_off, double vgs_on, double vbsf, double intrange, double mrange, double lorange, double * ioff, double * ids);


