/* opttmp function prototype and KITT header file */

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

	MODULE NAME: pn2swp_opt2
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
#include <unistd.h>
#include <netdb.h>
#include <stdlib.h>
	END USRLIB MODULE INFORMATION
*/
double  pn2swp_opt2(int, int, int, double, double, int, double, double, char);

