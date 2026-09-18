/* LBC5_1215 function prototype and KITT header file */

/* COPY OF USRLIB SETTINGS INFORMATION */
/* [VISIBILITY] */
/* HIDDEN=0 */
/* [DEPENDENCIES] */
/* LIBS=-lLBC5 */
/* OBJECTS= */
/* HEADERS= */
/* [NON-KULT SEARCH PATH] */
/* PATHS= */

/* USRLIB MODULE INFORMATION

	MODULE NAME: pn2swp_1215
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
	END USRLIB MODULE INFORMATION
*/
double  pn2swp_1215(int, int, int, double, double, int, double, double, char);

/* USRLIB MODULE INFORMATION

	MODULE NAME: pn2swp_rawdata
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
#include <ksox_def.h>

	END USRLIB MODULE INFORMATION
*/
double  pn2swp_rawdata(int, int, int, double, double, int, double, double, char);

/* USRLIB MODULE INFORMATION

	MODULE NAME: vtext4_6_1217
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 17
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		bulk,	int,	Input,	,	,	
		chuck,	int,	Input,	,	,	
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
#include <unistd.h>
#include <netdb.h>
	END USRLIB MODULE INFORMATION
*/
double  vtext4_6_1217(int, int, int, int, int, int, double, double, double, double, double, double, double, double, int, double *, int *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: vtext4_6_1217_org
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 17
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		bulk,	int,	Input,	,	,	
		chuck,	int,	Input,	,	,	
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
#include <unistd.h>
#include <netdb.h>
	END USRLIB MODULE INFORMATION
*/
double  vtext4_6_1217_org(int, int, int, int, int, int, double, double, double, double, double, double, double, double, int, double *, int *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: vtext4_6_ks
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 17
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		bulk,	int,	Input,	,	,	
		chuck,	int,	Input,	,	,	
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
#include <unistd.h>
#include <netdb.h>
	END USRLIB MODULE INFORMATION
*/
double  vtext4_6_ks(int, int, int, int, int, int, double, double, double, double, double, double, double, double, int, double *, int *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: vtext5_1215
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 16
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		body,	int,	Input,	,	,	
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
#include <unistd.h>
#include <netdb.h>
	END USRLIB MODULE INFORMATION
*/
double  vtext5_1215(int, int, int, int, int, double, double, double, double, double, double, double, double, int, double *, int *);

