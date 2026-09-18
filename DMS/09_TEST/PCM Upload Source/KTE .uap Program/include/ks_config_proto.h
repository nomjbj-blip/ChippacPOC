/* ks_config function prototype and KITT header file */

/* COPY OF USRLIB SETTINGS INFORMATION */
/* [VISIBILITY] */
/* HIDDEN=0 */
/* [DEPENDENCIES] */
/* LIBS=-lA07 -lHP4284 -lktest -lLBC5 */
/* OBJECTS= */
/* HEADERS= */
/* [NON-KULT SEARCH PATH] */
/* PATHS= */

/* USRLIB MODULE INFORMATION

	MODULE NAME: all_pin_gnd
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
void  all_pin_gnd();

/* USRLIB MODULE INFORMATION

	MODULE NAME: cap_diag_pt16
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 12
	ARGUMENTS:
		channel_1,	int,	Input,	,	,	
		channel_2,	int,	Input,	,	,	
		channel_3,	int,	Input,	,	,	
		channel_4,	int,	Input,	,	,	
		channel_5,	int,	Input,	,	,	
		mode,	double,	Input,	,	,	
		frequency,	double,	Input,	,	,	
		signal_level,	double,	Input,	,	,	
		delay_time,	double,	Input,	,	,	
		vstart,	double,	Input,	,	,	
		vstop,	double,	Input,	,	,	
		steps,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <stdlib.h>
#include <string.h>
#include <time.h>
#include <tidp1.h>
#include <cmtr_hp4284.h>
#define __4284__
	END USRLIB MODULE INFORMATION
*/
void  cap_diag_pt16(int, int, int, int, int, double, double, double, double, double, double, int);

/* USRLIB MODULE INFORMATION

	MODULE NAME: vtext4_con
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 15
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
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
#include <par_util.h>
#include <stdlib.h>
#include <ktxe_proto.h>
#include <ksox_def.h>
#include <kui_proto.h>
#include <stdlib.h>
	END USRLIB MODULE INFORMATION
*/
double  vtext4_con(int, int, int, int, double, double, double, double, double, double, double, double, int, double *, int *);

