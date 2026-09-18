/* sweep_test function prototype and KITT header file */

/* COPY OF USRLIB SETTINGS INFORMATION */
/* [VISIBILITY] */
/* HIDDEN=0 */
/* [DEPENDENCIES] */
/* LIBS=-lHP4284 -lktest */
/* OBJECTS= */
/* HEADERS= */
/* [NON-KULT SEARCH PATH] */
/* PATHS= */

/* USRLIB MODULE INFORMATION

	MODULE NAME: aa
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 2
	ARGUMENTS:
		data,	char *,	Input,	,	,	
		input,	char *,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <kui_proto.h>
	END USRLIB MODULE INFORMATION
*/
void aa(char * data, char * input);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ask_cardtest_execute
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>


#define _REENTRANT 
#include <stdlib.h>
#include <string.h>
#include <time.h>
#include "ktxe_types.h"
#include "ktxe_proto.h"
#include "kui_proto.h"
#include "kdf.h"
                       
#include "ksox_def.h"
#include "prb.h"
#include "prb_proto.h"
#include "prb_msg.h"
#include "guidedef.h"
#include "wtype.h"
#include <thread.h>
#define NDEBUG
#include <assert.h>
	END USRLIB MODULE INFORMATION
*/
int ask_cardtest_execute();


/* USRLIB MODULE INFORMATION

	MODULE NAME: cap_check1
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 8
	ARGUMENTS:
		delaytime,	double,	Input,	,	,	
		mode,	double,	Input,	,	,	
		freq,	double,	Input,	,	,	
		level,	double,	Input,	,	,	
		current_spec,	double,	Input,	,	,	
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
void cap_check1(double delaytime, double mode, double freq, double level, double current_spec, double vstart, double vstop, int steps);


/* USRLIB MODULE INFORMATION

	MODULE NAME: cap_check_stray
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 13
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
		cap_spec,	double,	Input,	,	,	
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
void cap_check_stray(int channel_1, int channel_2, int channel_3, int channel_4, int channel_5, double mode, double frequency, double signal_level, double delay_time, double vstart, double vstop, int steps, double cap_spec);


/* USRLIB MODULE INFORMATION

	MODULE NAME: cap_diag
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 13
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
		cap_spec,	double,	Input,	,	,	
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
void cap_diag(int channel_1, int channel_2, int channel_3, int channel_4, int channel_5, double mode, double frequency, double signal_level, double delay_time, double vstart, double vstop, int steps, double cap_spec);


/* USRLIB MODULE INFORMATION

	MODULE NAME: cap_diag_re
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 14
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
		up_spec,	double,	Input,	,	,	
		lo_spec,	double,	Input,	,	,	
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
void cap_diag_re(int channel_1, int channel_2, int channel_3, int channel_4, int channel_5, double mode, double frequency, double signal_level, double delay_time, double vstart, double vstop, int steps, double up_spec, double lo_spec);


/* USRLIB MODULE INFORMATION

	MODULE NAME: cap_diag_re2
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
void cap_diag_re2(int channel_1, int channel_2, int channel_3, int channel_4, int channel_5, double mode, double frequency, double signal_level, double delay_time, double vstart, double vstop, int steps);


/* USRLIB MODULE INFORMATION

	MODULE NAME: cap_diag_re3
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
void cap_diag_re3(int channel_1, int channel_2, int channel_3, int channel_4, int channel_5, double mode, double frequency, double signal_level, double delay_time, double vstart, double vstop, int steps);


/* USRLIB MODULE INFORMATION

	MODULE NAME: cap_diag_re4
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
void cap_diag_re4(int channel_1, int channel_2, int channel_3, int channel_4, int channel_5, double mode, double frequency, double signal_level, double delay_time, double vstart, double vstop, int steps);


/* USRLIB MODULE INFORMATION

	MODULE NAME: cap_diag_re4_ch34
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
void cap_diag_re4_ch34(int channel_1, int channel_2, int channel_3, int channel_4, int channel_5, double mode, double frequency, double signal_level, double delay_time, double vstart, double vstop, int steps);


/* USRLIB MODULE INFORMATION

	MODULE NAME: cap_diag_re4_ch56
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
void cap_diag_re4_ch56(int channel_1, int channel_2, int channel_3, int channel_4, int channel_5, double mode, double frequency, double signal_level, double delay_time, double vstart, double vstop, int steps);


/* USRLIB MODULE INFORMATION

	MODULE NAME: cap_diag_re4_ch78
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
void cap_diag_re4_ch78(int channel_1, int channel_2, int channel_3, int channel_4, int channel_5, double mode, double frequency, double signal_level, double delay_time, double vstart, double vstop, int steps);


/* USRLIB MODULE INFORMATION

	MODULE NAME: cap_diag_re4_detail
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
void  cap_diag_re4_detail(int, int, int, int, int, double, double, double, double, double, double, int);

/* USRLIB MODULE INFORMATION

	MODULE NAME: cap_diag_re4_org141127
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
void cap_diag_re4_org141127(int channel_1, int channel_2, int channel_3, int channel_4, int channel_5, double mode, double frequency, double signal_level, double delay_time, double vstart, double vstop, int steps);


/* USRLIB MODULE INFORMATION

	MODULE NAME: cap_diag_rpt
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 13
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
		rpt,	int,	Input,	,	,	
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
void  cap_diag_rpt(int, int, int, int, int, double, double, double, double, double, double, int, int);

/* USRLIB MODULE INFORMATION

	MODULE NAME: card_leak
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 10
	ARGUMENTS:
		high,	int,	Input,	,	,	
		low,	int,	Input,	,	,	
		delaytime,	double,	Input,	,	,	
		intrange_value,	int,	Input,	,	1,	10
		vstart,	double,	Input,	,	,	
		vstop,	double,	Input,	,	,	
		v_force,	D_ARRAY_T,	Output,	,	,	
		v_steps,	int,	Input,	,	,	
		i_leak,	D_ARRAY_T,	Output,	,	,	
		i_steps,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void card_leak(int high, int low, double delaytime, int intrange_value, double vstart, double vstop, double *v_force, int v_steps, double *i_leak, int i_steps);


/* USRLIB MODULE INFORMATION

	MODULE NAME: card_leak_0616
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 10
	ARGUMENTS:
		high,	int,	Input,	,	,	
		low,	int,	Input,	,	,	
		delaytime,	double,	Input,	,	,	
		intrange_value,	int,	Input,	,	1,	10
		vstart,	double,	Input,	,	,	
		vstop,	double,	Input,	,	,	
		v_force,	D_ARRAY_T,	Output,	,	,	
		v_steps,	int,	Input,	,	,	
		i_leak,	D_ARRAY_T,	Output,	,	,	
		i_steps,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void card_leak_0616(int high, int low, double delaytime, int intrange_value, double vstart, double vstop, double *v_force, int v_steps, double *i_leak, int i_steps);


/* USRLIB MODULE INFORMATION

	MODULE NAME: card_leak_0616_constant
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 10
	ARGUMENTS:
		high,	int,	Input,	,	,	
		low,	int,	Input,	,	,	
		delaytime,	double,	Input,	,	,	
		intrange_value,	int,	Input,	,	1,	10
		vstart,	double,	Input,	,	,	
		vstop,	double,	Input,	,	,	
		v_force,	D_ARRAY_T,	Output,	,	,	
		v_steps,	int,	Input,	,	,	
		i_leak,	D_ARRAY_T,	Output,	,	,	
		i_steps,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void card_leak_0616_constant(int high, int low, double delaytime, int intrange_value, double vstart, double vstop, double *v_force, int v_steps, double *i_leak, int i_steps);


/* USRLIB MODULE INFORMATION

	MODULE NAME: card_leak_check
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 7
	ARGUMENTS:
		delaytime,	double,	Input,	,	,	
		intrange_value,	int,	Input,	,	1,	10
		lorange_value,	double,	Input,	,	,	
		current_spec,	double,	Input,	,	,	
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
	END USRLIB MODULE INFORMATION
*/
void  card_leak_check(double, int, double, double, double, double, int);

/* USRLIB MODULE INFORMATION

	MODULE NAME: card_leak_check_new
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 7
	ARGUMENTS:
		delaytime,	double,	Input,	,	,	
		intrange_value,	int,	Input,	,	1,	10
		lorange_value,	double,	Input,	,	,	
		current_spec,	double,	Input,	,	,	
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
	END USRLIB MODULE INFORMATION
*/
void card_leak_check_new(double delaytime, int intrange_value, double lorange_value, double current_spec, double vstart, double vstop, int steps);


/* USRLIB MODULE INFORMATION

	MODULE NAME: card_leak_check_org
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 7
	ARGUMENTS:
		delaytime,	double,	Input,	,	,	
		intrange_value,	int,	Input,	,	1,	10
		lorange_value,	double,	Input,	,	,	
		current_spec,	double,	Input,	,	,	
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
	END USRLIB MODULE INFORMATION
*/
void card_leak_check_org(double delaytime, int intrange_value, double lorange_value, double current_spec, double vstart, double vstop, int steps);


/* USRLIB MODULE INFORMATION

	MODULE NAME: card_leak_dly_int
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 10
	ARGUMENTS:
		high,	int,	Input,	,	,	
		low,	int,	Input,	,	,	
		delaytime,	double,	Input,	,	,	
		intrange_value,	int,	Input,	,	1,	10
		vstart,	double,	Input,	,	,	
		vstop,	double,	Input,	,	,	
		v_force,	D_ARRAY_T,	Output,	,	,	
		v_steps,	int,	Input,	,	,	
		i_leak,	D_ARRAY_T,	Output,	,	,	
		i_steps,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void card_leak_dly_int(int high, int low, double delaytime, int intrange_value, double vstart, double vstop, double *v_force, int v_steps, double *i_leak, int i_steps);


/* USRLIB MODULE INFORMATION

	MODULE NAME: card_leak_org
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 10
	ARGUMENTS:
		high,	int,	Input,	,	,	
		low,	int,	Input,	,	,	
		delaytime,	double,	Input,	,	,	
		intrange_value,	int,	Input,	,	1,	10
		vstart,	double,	Input,	,	,	
		vstop,	double,	Input,	,	,	
		v_force,	D_ARRAY_T,	Output,	,	,	
		v_steps,	int,	Input,	,	,	
		i_leak,	D_ARRAY_T,	Output,	,	,	
		i_steps,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void card_leak_org(int high, int low, double delaytime, int intrange_value, double vstart, double vstop, double *v_force, int v_steps, double *i_leak, int i_steps);


/* USRLIB MODULE INFORMATION

	MODULE NAME: card_leak_time
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 12
	ARGUMENTS:
		high,	int,	Input,	,	,	
		low,	int,	Input,	,	,	
		range_value,	double,	Input,	,	,	
		intrange_value,	int,	Input,	,	1,	10
		force_v,	double,	Input,	,	,	
		meas_mode,	int,	Input,	,	0,	1
		v_force,	D_ARRAY_T,	Output,	,	,	
		v_steps,	int,	Input,	,	,	
		i_leak,	D_ARRAY_T,	Output,	,	,	
		i_steps,	int,	Input,	,	,	
		t_meas,	D_ARRAY_T,	Input,	 ,	 ,	 
		t_steps,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void card_leak_time(int high, int low, double range_value, int intrange_value, double force_v, int meas_mode, double *v_force, int v_steps, double *i_leak, int i_steps, double *t_meas, int t_steps);


/* USRLIB MODULE INFORMATION

	MODULE NAME: card_leak_time1
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 15
	ARGUMENTS:
		high,	int,	Input,	,	,	
		low,	int,	Input,	,	,	
		mode,	double,	Input,	,	,	
		delaytime,	double,	Input,	,	,	
		intrange_value,	double,	Input,	,	,	
		rng_value,	double,	Input,	,	,	
		vfrc,	double,	Input,	,	,	
		v_force,	D_ARRAY_T,	Output,	,	,	
		v_steps,	int,	Input,	,	,	
		i_leak,	D_ARRAY_T,	Output,	,	,	
		i_steps,	int,	Input,	,	,	
		t_meas1,	D_ARRAY_T,	Output,	,	,	
		t_steps1,	int,	Input,	,	,	
		t_meas2,	D_ARRAY_T,	Output,	,	,	
		t_steps2,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void card_leak_time1(int high, int low, double mode, double delaytime, double intrange_value, double rng_value, double vfrc, double *v_force, int v_steps, double *i_leak, int i_steps, double *t_meas1, int t_steps1, double *t_meas2, int t_steps2);


/* USRLIB MODULE INFORMATION

	MODULE NAME: card_leak_txt
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 15
	ARGUMENTS:
		high,	int,	Input,	,	,	
		low,	int,	Input,	,	,	
		mode,	double,	Input,	,	,	
		delaytime,	double,	Input,	,	,	
		intrange_value,	double,	Input,	,	,	
		rng_value,	double,	Input,	,	,	
		vfrc,	double,	Input,	,	,	
		v_force,	D_ARRAY_T,	Output,	,	,	
		v_steps,	int,	Input,	,	,	
		i_leak,	D_ARRAY_T,	Output,	,	,	
		i_steps,	int,	Input,	,	,	
		t_meas1,	D_ARRAY_T,	Output,	,	,	
		t_steps1,	int,	Input,	,	,	
		t_meas2,	D_ARRAY_T,	Output,	,	,	
		t_steps2,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void card_leak_txt(int high, int low, double mode, double delaytime, double intrange_value, double rng_value, double vfrc, double *v_force, int v_steps, double *i_leak, int i_steps, double *t_meas1, int t_steps1, double *t_meas2, int t_steps2);


/* USRLIB MODULE INFORMATION

	MODULE NAME: cv_sweep2
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 12
	ARGUMENTS:
		high,	int,	Input,	,	,	
		lo1,	int,	Input,	,	,	
		lo2,	int,	Input,	,	,	
		vstart,	double,	Input,	,	,	
		vstop,	double,	Input,	,	,	
		vstep,	double,	Input,	,	,	
		area,	double,	Input,	,	,	
		freq,	double,	Input,	,	,	
		volt,	D_ARRAY_T,	Output,	,	,	
		vnpts,	int,	Input,	,	,	
		cap_array,	F_ARRAY_T,	Output,	,	,	
		cnpts,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include	<prb_proto.h>
#include <cmtr_hp4284.h>
	END USRLIB MODULE INFORMATION
*/
void cv_sweep2(int high, int lo1, int lo2, double vstart, double vstop, double vstep, double area, double freq, double *volt, int vnpts, float *cap_array, int cnpts);


/* USRLIB MODULE INFORMATION

	MODULE NAME: IdVd_new
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 16
	ARGUMENTS:
		devname,	char *,	Input,	"     ",	,	
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		delaytime,	double,	Input,	,	,	
		vdstart,	double,	Input,	,	,	
		vdstop,	double,	Input,	,	,	
		vdstep,	double,	Input,	,	,	
		vg,	double,	Input,	,	,	
		vs,	double,	Input,	,	,	
		area,	double,	Input,	,	,	
		volt,	D_ARRAY_T,	Output,	,	,	
		vnpts,	int,	Input,	,	,	
		id,	D_ARRAY_T,	Output,	,	,	
		inpts,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <stdlib.h>
#include <ktxe_proto.h>
#include <ksox_def.h>
#include <kui_proto.h>

	END USRLIB MODULE INFORMATION
*/
void IdVd_new(char * devname, int drain, int gate, int source, int sub, double delaytime, double vdstart, double vdstop, double vdstep, double vg, double vs, double area, double *volt, int vnpts, double *id, int inpts);


/* USRLIB MODULE INFORMATION

	MODULE NAME: JEDEC_I2_cap_swp_ks
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 27
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
		imin,	double,	Input,	,	,	
		imax,	double,	Input,	,	,	
		istop,	double,	Input,	,	,	
		imult,	double,	Input,	,	,	
		hold,	double,	Input,	,	,	
		stepdelay,	double,	Input,	,	,	
		vmax,	double,	Input,	,	,	
		ratio,	double,	Input,	,	,	
		m_i,	double,	Input,	,	,	
		m_q,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		vlf,	double *,	Output,	,	,	
		vhf,	double *,	Output,	,	,	
		vfail,	double *,	Output,	,	,	
		ifail,	double *,	Output,	,	,	
		qbd,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#define CRTLIM 1.0E-6		  

#include <stdlib.h>
#include <ktxe_proto.h>
#include <ksox_def.h>
#include <kui_proto.h>

#define MAX(a,b)	((a)>(b) ? (a) : (b))
#define CDELAY  125.E-12
#define ILEAK   20.E-12 
                            
	END USRLIB MODULE INFORMATION
*/
void JEDEC_I2_cap_swp_ks(char * devname, int hi, int lo, int sub, int chuckcon, double area, double per, char tty, char bty, double imin, double imax, double istop, double imult, double hold, double stepdelay, double vmax, double ratio, double m_i, double m_q, double intrange, double mrange, double lorange, double * vlf, double * vhf, double * vfail, double * ifail, double * qbd);


/* USRLIB MODULE INFORMATION

	MODULE NAME: resv_swp
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 13
	ARGUMENTS:
		high,	int,	Input,	,	,	
		low,	int,	Input,	,	,	
		delaytime,	double,	Input,	,	,	
		intrange_value,	int,	Input,	,	1,	10
		vstart,	double,	Input,	,	,	
		vstop,	double,	Input,	,	,	
		v_force,	D_ARRAY_T,	Output,	,	,	
		v_steps,	int,	Input,	,	,	
		i_leak,	D_ARRAY_T,	Output,	,	,	
		i_steps,	int,	Input,	,	,	
		rs,	D_ARRAY_T,	Output,	,	,	
		ArrSizeForParm11,	int,	Input,	,	,	
		sq,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void resv_swp(int high, int low, double delaytime, int intrange_value, double vstart, double vstop, double *v_force, int v_steps, double *i_leak, int i_steps, double *rs, int ArrSizeForParm11, double sq);


/* USRLIB MODULE INFORMATION

	MODULE NAME: tft_sweep_new
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 14
	ARGUMENTS:
		devname,	char *,	Input,	"     ",	,	
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		vgstart,	double,	Input,	,	,	
		vgstop,	double,	Input,	,	,	
		vgstep,	double,	Input,	,	,	
		vd,	double,	Input,	,	,	
		vs,	double,	Input,	,	,	
		area,	double,	Input,	,	,	
		volt,	D_ARRAY_T,	Output,	,	,	
		vnpts,	int,	Input,	,	,	
		id,	D_ARRAY_T,	Output,	,	,	
		inpts,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <stdlib.h>
#include <ktxe_proto.h>
#include <ksox_def.h>
#include <kui_proto.h>

	END USRLIB MODULE INFORMATION
*/
void tft_sweep_new(char * devname, int drain, int gate, int source, double vgstart, double vgstop, double vgstep, double vd, double vs, double area, double *volt, int vnpts, double *id, int inpts);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Vth_gm_new
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 14
	ARGUMENTS:
		devname,	char *,	Input,	"",	,	
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vs,	double,	Input,	,	,	
		vgstart,	double,	Input,	,	,	
		vgstop,	double,	Input,	,	,	
		vgstep,	double,	Input,	,	,	
		vg_max,	double *,	Output,	,	,	
		id_max,	double *,	Output,	,	,	
		vth,	double *,	Output,	,	,	
		maxgm,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <stdlib.h>
#include <ktxe_proto.h>
#include <ksox_def.h>
#include <kui_proto.h>
	END USRLIB MODULE INFORMATION
*/
void Vth_gm_new(char * devname, int drain, int gate, int source, int sub, double vds, double vs, double vgstart, double vgstop, double vgstep, double * vg_max, double * id_max, double * vth, double * maxgm);


