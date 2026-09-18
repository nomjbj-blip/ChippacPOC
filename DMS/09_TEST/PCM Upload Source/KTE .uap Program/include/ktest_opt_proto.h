/* ktest_opt function prototype and KITT header file */

/* COPY OF USRLIB SETTINGS INFORMATION */
/* [VISIBILITY] */
/* HIDDEN=0 */
/* [DEPENDENCIES] */
/* LIBS=-leng_test -lHP4284 */
/* OBJECTS= */
/* HEADERS= */
/* [NON-KULT SEARCH PATH] */
/* PATHS= */

/* USRLIB MODULE INFORMATION

	MODULE NAME: cap_2spo_cvsweep
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 12
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		vstart,	double,	Input,	,	,	
		vstop,	double,	Input,	,	,	
		vstep,	double,	Input,	,	,	
		freq,	double,	Input,	,	,	
		level,	double,	Input,	,	,	
		debug,	int,	Input,	,	,	
		c_f,	double *,	Output,	,	,	
		g_f,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>                              
#include "ktxe_types.h"
#include "COM_usrlib.h"
#include <cmtr_hp4284.h>
#include <ksox_def.h>
#include <unistd.h>
#include <netdb.h>

#define Eo 8.854E-14
#define PI 3.14159265358979323846264338327950288419716939937510582
#define DONTCARE 0
#define CV_XMT 11

	END USRLIB MODULE INFORMATION
*/
void cap_2spo_cvsweep(int hi, int lo, int sub, int chuckcon, double vstart, double vstop, double vstep, double freq, double level, int debug, double * c_f, double * g_f);


/* USRLIB MODULE INFORMATION

	MODULE NAME: cap_2spo_cvswepGD
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 12
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		vstart,	double,	Input,	,	,	
		vstop,	double,	Input,	,	,	
		vstep,	double,	Input,	,	,	
		freq,	double,	Input,	,	,	
		level,	double,	Input,	,	,	
		debug,	int,	Input,	,	,	
		c_f,	double *,	Output,	,	,	
		g_f,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>                              
#include "ktxe_types.h"
#include "COM_usrlib.h"
#include <cmtr_hp4284.h>
#include <ksox_def.h>
#include <unistd.h>
#include <netdb.h>

#define Eo 8.854E-14
#define PI 3.14159265358979323846264338327950288419716939937510582
#define DONTCARE 0
#define CV_XMT 11

	END USRLIB MODULE INFORMATION
*/
void  cap_2spo_cvswepGD(int, int, int, int, double, double, double, double, double, int, double *, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: cap_2spo_cvswGND
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 12
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		vstart,	double,	Input,	,	,	
		vstop,	double,	Input,	,	,	
		vstep,	double,	Input,	,	,	
		freq,	double,	Input,	,	,	
		level,	double,	Input,	,	,	
		debug,	int,	Input,	,	,	
		c_f,	double *,	Output,	,	,	
		g_f,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>                              
#include "ktxe_types.h"
#include "COM_usrlib.h"
#include <cmtr_hp4284.h>
#include <ksox_def.h>
#include <unistd.h>
#include <netdb.h>

#define Eo 8.854E-14
#define PI 3.14159265358979323846264338327950288419716939937510582
#define DONTCARE 0
#define CV_XMT 11

	END USRLIB MODULE INFORMATION
*/
void  cap_2spo_cvswGND(int, int, int, int, double, double, double, double, double, int, double *, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: JEDEC_2p_cap
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 18
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		area,	double,	Input,	,	,	
		per,	double,	Input,	,	,	
		tty,	char,	Input,	,	,	
		bty,	char,	Input,	,	,	
		istart,	double,	Input,	,	,	
		ihigh,	double,	Input,	,	,	
		hold,	double,	Input,	,	,	
		stepdelay,	double,	Input,	,	,	
		vmax,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		vlf,	double *,	Output,	,	,	
		vhf,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#define CRTLIM 1.0E-6		                              
#define MAX(a,b)	((a)>(b) ? (a) : (b))
	END USRLIB MODULE INFORMATION
*/
void  JEDEC_2p_cap(int, int, int, int, double, double, char, char, double, double, double, double, double, double, double, double, double *, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: JEDEC_2p_cap_timer
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 18
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		area,	double,	Input,	,	,	
		per,	double,	Input,	,	,	
		tty,	char,	Input,	,	,	
		bty,	char,	Input,	,	,	
		istart,	double,	Input,	,	,	
		ihigh,	double,	Input,	,	,	
		hold,	double,	Input,	,	,	
		stepdelay,	double,	Input,	,	,	
		vmax,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		vlf,	double *,	Output,	,	,	
		vhf,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#define CRTLIM 1.0E-6		                              
#define MAX(a,b)	((a)>(b) ? (a) : (b))
	END USRLIB MODULE INFORMATION
*/
void  JEDEC_2p_cap_timer(int, int, int, int, double, double, char, char, double, double, double, double, double, double, double, double, double *, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: JEDEC_sw_cap
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 18
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		area,	double,	Input,	,	,	
		per,	double,	Input,	,	,	
		tty,	char,	Input,	,	,	
		bty,	char,	Input,	,	,	
		ipgm1,	double,	Input,	,	,	
		ipgm2,	double,	Input,	,	,	
		delaytime,	double,	Input,	,	,	
		vstep,	double,	Input,	,	,	
		vmax,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		vlf,	double *,	Output,	,	,	
		vhf,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#define CRTLIM 1.0E-6		                              
#define MAX(a,b)	((a)>(b) ? (a) : (b))
	END USRLIB MODULE INFORMATION
*/
void  JEDEC_sw_cap(int, int, int, int, double, double, char, char, double, double, double, double, double, double, double, double, double *, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: vgsweep2
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 11
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		vglow,	double,	Input,	,	,	
		vghigh,	double,	Input,	,	,	
		vd,	double,	Input,	,	,	
		nstep,	int,	Input,	,	,	
		idpgm,	double,	Input,	,	,	
		udelay,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double  vgsweep2(int, int, int, int, double, double, double, int, double, double, char);

/* USRLIB MODULE INFORMATION

	MODULE NAME: vgsweep3
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 11
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		vglow,	double,	Input,	,	,	
		vghigh,	double,	Input,	,	,	
		vd,	double,	Input,	,	,	
		nstep,	int,	Input,	,	,	
		idpgm,	double,	Input,	,	,	
		udelay,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double  vgsweep3(int, int, int, int, double, double, double, int, double, double, char);

