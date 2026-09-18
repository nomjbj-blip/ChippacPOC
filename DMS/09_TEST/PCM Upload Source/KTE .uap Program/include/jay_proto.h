/* jay function prototype and KITT header file */

/* COPY OF USRLIB SETTINGS INFORMATION */
/* [VISIBILITY] */
/* HIDDEN=0 */
/* [DEPENDENCIES] */
/* LIBS=-lKI_DEBUG -lKI_UAPLIB -lktest */
/* OBJECTS= */
/* HEADERS= */
/* [NON-KULT SEARCH PATH] */
/* PATHS= */

/* USRLIB MODULE INFORMATION

	MODULE NAME: cap_check
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 22
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		area,	double,	Input,	,	,	
		per,	double,	Input,	,	,	
		tty,	char,	Input,	,	,	
		bty,	char,	Input,	,	,	
		vacc,	double,	Input,	,	,	
		vinv,	double,	Input,	,	,	
		freq,	double,	Input,	,	,	
		sig,	double,	Input,	,	,	
		stray,	double,	Input,	,	,	
		integ,	int,	Input,	,	,	
		ddebug,	int,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		ca,	double *,	Output,	,	,	
		ta,	double *,	Output,	,	,	
		ci,	double *,	Output,	,	,	
		ti,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#define CRTLIM 1.0E-6		                              
#include <tidp1.h>
#include "ktxe_types.h"
#include "COM_usrlib.h"
#define MAX(a,b)	((a)>(b) ? (a) : (b))
#include <cmtr_hp4284.h>
#define __4284__

#define Eo 8.854E-14
#define PI 3.14159265358979323846264338327950288419716939937510582
#define DONTCARE 0
#define CV_XMT 11
	END USRLIB MODULE INFORMATION
*/
void  cap_check(char *, int, int, int, int, double, double, char, char, double, double, double, double, double, int, int, double, double, double *, double *, double *, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: cap_check2
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 22
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		area,	double,	Input,	,	,	
		per,	double,	Input,	,	,	
		tty,	char,	Input,	,	,	
		bty,	char,	Input,	,	,	
		vacc,	double,	Input,	,	,	
		vinv,	double,	Input,	,	,	
		freq,	double,	Input,	,	,	
		sig,	double,	Input,	,	,	
		stray,	double,	Input,	,	,	
		integ,	int,	Input,	,	,	
		ddebug,	int,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		ca,	double *,	Output,	,	,	
		ta,	double *,	Output,	,	,	
		ci,	double *,	Output,	,	,	
		ti,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#define CRTLIM 1.0E-6		                              
#include <tidp1.h>
#include "ktxe_types.h"
#include "COM_usrlib.h"
#define MAX(a,b)	((a)>(b) ? (a) : (b))
#include <cmtr_hp4284.h>
#define __4284__

#define Eo 8.854E-14
#define PI 3.14159265358979323846264338327950288419716939937510582
#define DONTCARE 0
#define CV_XMT 11
	END USRLIB MODULE INFORMATION
*/
void  cap_check2(char *, int, int, int, int, double, double, char, char, double, double, double, double, double, int, int, double, double, double *, double *, double *, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: check_1
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <time.h>
#include <kdf.h>
#include <COM_usrlib.h>
#include <stdio.h>
#include <string.h>
#include <unistd.h>
#include <netdb.h>
#include <ktxe_proto.h>
#include <ksox_def.h>
#include <time.h>


#define debug 0
	END USRLIB MODULE INFORMATION
*/
void  check_1();

/* USRLIB MODULE INFORMATION

	MODULE NAME: gndall_20
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 21
	ARGUMENTS:
		pin1,	int,	Input,	,	,	
		pin2,	int,	Input,	,	,	
		pin3,	int,	Input,	,	,	
		pin4,	int,	Input,	,	,	
		pin5,	int,	Input,	,	,	
		pin6,	int,	Input,	,	,	
		pin7,	int,	Input,	,	,	
		pin8,	int,	Input,	,	,	
		pin9,	int,	Input,	,	,	
		pin10,	int,	Input,	,	,	
		pin11,	int,	Input,	,	,	
		pin12,	int,	Input,	,	,	
		pin13,	int,	Input,	,	,	
		pin14,	int,	Input,	,	,	
		pin15,	int,	Input,	,	,	
		pin16,	int,	Input,	,	,	
		pin17,	int,	Input,	,	,	
		pin18,	int,	Input,	,	,	
		pin19,	int,	Input,	,	,	
		pin20,	int,	Input,	,	,	
		delaytime,	double,	Input,	10,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <COM_usrlib.h>
	END USRLIB MODULE INFORMATION
*/
void  gndall_20(int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: jay_gndall
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <COM_usrlib.h>
	END USRLIB MODULE INFORMATION
*/
void  jay_gndall();

/* USRLIB MODULE INFORMATION

	MODULE NAME: man_gndall
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 20
	ARGUMENTS:
		p1,	int,	Input,	,	,	
		p2,	int,	Input,	,	,	
		p3,	int,	Input,	,	,	
		p4,	int,	Input,	,	,	
		p5,	int,	Input,	,	,	
		p6,	int,	Input,	,	,	
		p7,	int,	Input,	,	,	
		p8,	int,	Input,	,	,	
		p9,	int,	Input,	,	,	
		p10,	int,	Input,	,	,	
		p11,	int,	Input,	,	,	
		p12,	int,	Input,	,	,	
		p13,	int,	Input,	,	,	
		p14,	int,	Input,	,	,	
		p15,	int,	Input,	,	,	
		p16,	int,	Input,	,	,	
		p17,	int,	Input,	,	,	
		p18,	int,	Input,	,	,	
		p19,	int,	Input,	,	,	
		p20,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <COM_usrlib.h>
	END USRLIB MODULE INFORMATION
*/
void  man_gndall(int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int);

