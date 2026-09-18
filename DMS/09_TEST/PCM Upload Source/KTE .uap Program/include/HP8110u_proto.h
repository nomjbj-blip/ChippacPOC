/* HP8110u function prototype and KITT header file */

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

	MODULE NAME: CheckOPC_u
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
int CheckOPC_u(int unit);


/* USRLIB MODULE INFORMATION

	MODULE NAME: pulse_delay_u
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
int pulse_delay_u(double delaytime);


/* USRLIB MODULE INFORMATION

	MODULE NAME: pulse_dumptable_u
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
void pulse_dumptable_u();


/* USRLIB MODULE INFORMATION

	MODULE NAME: pulse_fall_u
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
int pulse_fall_u(double falltime);


/* USRLIB MODULE INFORMATION

	MODULE NAME: pulse_height_u
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
int pulse_height_u(double height);


/* USRLIB MODULE INFORMATION

	MODULE NAME: pulse_init_file_u
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 2
	ARGUMENTS:
		filename,	char *,	Input,	,	,	
		unit,	int,	Input,	,	,	
	INCLUDES:
#include <keithley.h>
#include <stdlib.h>
#include <kdf.h>
#include "pulsestruct.h"
#define LINELEN 81
#define TRUE 1
#define FALSE 0
pulse_struct pulse[MAXIMUM_CHANNELS+1];
pulse_struct *currentpulse;
	END USRLIB MODULE INFORMATION
*/
int pulse_init_file_u(char * filename, int unit);


/* USRLIB MODULE INFORMATION

	MODULE NAME: pulse_init_u
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
extern int pulse_init_file_u( char*, int );

	END USRLIB MODULE INFORMATION
*/
int pulse_init_u();


/* USRLIB MODULE INFORMATION

	MODULE NAME: pulse_mode_u
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
int pulse_mode_u(int pmode);


/* USRLIB MODULE INFORMATION

	MODULE NAME: pulse_offset_u
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
int pulse_offset_u(double amplitude, double offset);


/* USRLIB MODULE INFORMATION

	MODULE NAME: pulse_period_u
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
int pulse_period_u(double period);


/* USRLIB MODULE INFORMATION

	MODULE NAME: pulse_rise_u
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
int pulse_rise_u(double risetime);


/* USRLIB MODULE INFORMATION

	MODULE NAME: pulse_trig_burst_u
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
int pulse_trig_burst_u(int unit, int count);


/* USRLIB MODULE INFORMATION

	MODULE NAME: pulse_trig_u
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
int pulse_trig_u();


/* USRLIB MODULE INFORMATION

	MODULE NAME: pulse_trig_unit_opc_u
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
int pulse_trig_unit_opc_u(int unit);


/* USRLIB MODULE INFORMATION

	MODULE NAME: pulse_trig_unit_u
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
int pulse_trig_unit_u(int unit);


/* USRLIB MODULE INFORMATION

	MODULE NAME: pulse_width_u
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
int pulse_width_u(double width);


/* USRLIB MODULE INFORMATION

	MODULE NAME: select_channel_u
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
int select_channel_u(int new_chan);


