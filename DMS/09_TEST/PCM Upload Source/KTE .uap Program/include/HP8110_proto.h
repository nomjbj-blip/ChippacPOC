/* HP8110 function prototype and KITT header file */

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

	MODULE NAME: CheckOPC
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		unit,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <string.h>
#include "pulsestruct.h"
pulse_struct pulse[MAXIMUM_CHANNELS+1];
pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
int CheckOPC(int unit);


/* USRLIB MODULE INFORMATION

	MODULE NAME: pulse_delay
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		delaytime,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "pulsestruct.h"
pulse_struct pulse[MAXIMUM_CHANNELS+1];
pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
int pulse_delay(double delaytime);


/* USRLIB MODULE INFORMATION

	MODULE NAME: pulse_dumptable
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "pulsestruct.h"
pulse_struct pulse[MAXIMUM_CHANNELS+1];

	END USRLIB MODULE INFORMATION
*/
void pulse_dumptable();


/* USRLIB MODULE INFORMATION

	MODULE NAME: pulse_fall
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		falltime,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "pulsestruct.h"
pulse_struct pulse[MAXIMUM_CHANNELS+1];
pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
int pulse_fall(double falltime);


/* USRLIB MODULE INFORMATION

	MODULE NAME: pulse_height
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		height,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "pulsestruct.h"
pulse_struct pulse[MAXIMUM_CHANNELS+1];
pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
int pulse_height(double height);


/* USRLIB MODULE INFORMATION

	MODULE NAME: pulse_init
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "pulsestruct.h"
#define LINELEN 81
#define TRUE 1
#define FALSE 0
int MAX_CHANNELS;
pulse_struct pulse[MAXIMUM_CHANNELS+1];
extern int pulse_init_file( char*, int );
	END USRLIB MODULE INFORMATION
*/
int pulse_init();


/* USRLIB MODULE INFORMATION

	MODULE NAME: pulse_init_file
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 2
	ARGUMENTS:
		filename,	char *,	Input,	,	,	
		unit,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "pulsestruct.h"
#include "KI_license.h"
#define LINELEN 81
#define TRUE 1
#define FALSE 0
#define LIBNAME "HP8110Lib"
pulse_struct pulse[MAXIMUM_CHANNELS+1];
pulse_struct *currentpulse;
static int libNameLicensed = 0 ;
	END USRLIB MODULE INFORMATION
*/
int pulse_init_file(char * filename, int unit);


/* USRLIB MODULE INFORMATION

	MODULE NAME: pulse_mode
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		pmode,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <string.h>
#include "pulsestruct.h"
#define TRUE 1
#define FALSE 0
pulse_struct pulse[MAXIMUM_CHANNELS+1];
pulse_struct * currentpulse;
	END USRLIB MODULE INFORMATION
*/
int pulse_mode(int pmode);


/* USRLIB MODULE INFORMATION

	MODULE NAME: pulse_offset
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 2
	ARGUMENTS:
		amplitude,	double,	Input,	,	,	
		offset,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "pulsestruct.h"
pulse_struct pulse[MAXIMUM_CHANNELS+1];
pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
int pulse_offset(double amplitude, double offset);


/* USRLIB MODULE INFORMATION

	MODULE NAME: pulse_period
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		period,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <string.h>
#include "pulsestruct.h"
pulse_struct pulse[MAXIMUM_CHANNELS+1];
pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
int pulse_period(double period);


/* USRLIB MODULE INFORMATION

	MODULE NAME: pulse_rise
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		risetime,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "pulsestruct.h"
pulse_struct pulse[MAXIMUM_CHANNELS+1];
pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
int pulse_rise(double risetime);


/* USRLIB MODULE INFORMATION

	MODULE NAME: pulse_trig
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "pulsestruct.h"
pulse_struct pulse[MAXIMUM_CHANNELS+1];
pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
int pulse_trig();


/* USRLIB MODULE INFORMATION

	MODULE NAME: pulse_trig_burst
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 2
	ARGUMENTS:
		unit,	int,	Input,	,	,	
		count,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "pulsestruct.h"
pulse_struct pulse[MAXIMUM_CHANNELS+1];
pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
int pulse_trig_burst(int unit, int count);


/* USRLIB MODULE INFORMATION

	MODULE NAME: pulse_trig_unit
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		unit,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "pulsestruct.h"
pulse_struct pulse[MAXIMUM_CHANNELS+1];
pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
int pulse_trig_unit(int unit);


/* USRLIB MODULE INFORMATION

	MODULE NAME: pulse_trig_unit_opc
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		unit,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "pulsestruct.h"
pulse_struct pulse[MAXIMUM_CHANNELS+1];
pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
int pulse_trig_unit_opc(int unit);


/* USRLIB MODULE INFORMATION

	MODULE NAME: pulse_width
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		width,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "pulsestruct.h"
pulse_struct pulse[MAXIMUM_CHANNELS+1];
pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
int pulse_width(double width);


/* USRLIB MODULE INFORMATION

	MODULE NAME: select_channel
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		new_chan,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "pulsestruct.h"
pulse_struct pulse[MAXIMUM_CHANNELS+1];
pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
int select_channel(int new_chan);


