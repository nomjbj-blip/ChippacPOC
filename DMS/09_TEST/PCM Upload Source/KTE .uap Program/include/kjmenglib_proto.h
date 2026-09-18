/* kjmenglib function prototype and KITT header file */

/* COPY OF USRLIB SETTINGS INFORMATION */
/* [VISIBILITY] */
/* HIDDEN=0 */
/* [DEPENDENCIES] */
/* LIBS=-lHP4284 */
/* OBJECTS= */
/* HEADERS= */
/* [NON-KULT SEARCH PATH] */
/* PATHS= */

/* USRLIB MODULE INFORMATION

	MODULE NAME: beta2_222
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 10
	ARGUMENTS:
		em,	int,	Input,	,	,	
		base,	int,	Input,	,	,	
		coll,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		ic,	double,	Input,	,	,	
		vce,	double,	Input,	,	,	
		vsub,	double,	Input,	,	,	
		vbeout,	double *,	Output,	,	,	
		icout,	double *,	Output,	,	,	
		type,	char,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <kjmenglib_proto.h>
	END USRLIB MODULE INFORMATION
*/
double  beta2_222(int, int, int, int, double, double, double, double *, double *, char);

/* USRLIB MODULE INFORMATION

	MODULE NAME: beta2_333
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 10
	ARGUMENTS:
		em,	int,	Input,	,	,	
		base,	int,	Input,	,	,	
		coll,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		ic,	double,	Input,	,	,	
		vce,	double,	Input,	,	,	
		vsub,	double,	Input,	,	,	
		vbeout,	double *,	Output,	,	,	
		icout,	double *,	Output,	,	,	
		type,	char,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <kjmenglib_proto.h>
	END USRLIB MODULE INFORMATION
*/
double  beta2_333(int, int, int, int, double, double, double, double *, double *, char);

/* USRLIB MODULE INFORMATION

	MODULE NAME: kdelay_kjm
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 3
	ARGUMENTS:
		npin,	int,	Input,	,	,	
		i,	double,	Input,	,	,	
		v,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>	                
#include <math.h>	              
#include "lptdef.h"
#define MAX(a,b)	((a)>(b) ? (a) : (b))

#define CDELAY 	100.0E-12
#define ILEAK  	1.E-12 
	END USRLIB MODULE INFORMATION
*/
void  kdelay_kjm(int, double, double);

