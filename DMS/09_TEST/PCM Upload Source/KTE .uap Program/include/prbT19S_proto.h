/* prbT19S function prototype and KITT header file */

/* COPY OF USRLIB SETTINGS INFORMATION */
/* [VISIBILITY] */
/* HIDDEN=1 */
/* [DEPENDENCIES] */
/* LIBS= */
/* OBJECTS= */
/* HEADERS= */
/* [NON-KULT SEARCH PATH] */
/* PATHS= */

/* USRLIB MODULE INFORMATION

	MODULE NAME: Cnfg_T19S
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		teststation,	int,	Input,	,	,
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include        "prb.h"
#include        "prb_func_id.h"
#include        "prb_extern.h"
#include		"prb_msg.h"
#include									"prbT19S_proto.h"
#include    "prb_drvadr.h"
	END USRLIB MODULE INFORMATION
*/
int Cnfg_T19S(int teststation);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrAbsMove_T19S
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 2
	ARGUMENTS:
		x_value,	double,	Input,	,	,
		y_value,	double,	Input,	,	,
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include        <string.h>
#include        "prb.h"
#include        "prb_msg.h"
#include        "PrT19S.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrAbsMove_T19S(double x_value, double y_value);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrCassetteMap_T19S
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 2
	ARGUMENTS:
		cassette_number,	int,	Input,	,	,
		cassette_map,	int *,	Output,	,	,
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include        "prb.h"
#include        "prb_msg.h"
#include        "PrT19S.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrCassetteMap_T19S(int cassette_number, int * cassette_map);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrCheckOptions_T19S
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 6
	ARGUMENTS:
		OcrPresent,	int *,	Output,	,	,	
		AutoAlnPresent,	int *,	Output,	,	,	
		ProfilerPresent,	int *,	Output,	,	,	
		HotchuckPresent,	int *,	Output,	,	,	
		HandlerPresent,	int *,	Output,	,	,	
		Probe2PadPresent,	int *,	Output,	,	,	
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include        "prb.h"
#include        "prb_msg.h"
#include        "PrT19S.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrCheckOptions_T19S(int * OcrPresent, int * AutoAlnPresent, int * ProfilerPresent, int * HotchuckPresent, int * HandlerPresent, int * Probe2PadPresent);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrChuck_T19S
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		chuck_position,	int,	Input,	,	,	
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include        <string.h>
#include        "prb.h"
#include        "prb_msg.h"
#include        "PrT19S.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrChuck_T19S(int chuck_position);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrError_T19S
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <stdlib.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include    <string.h>
#include    "prb.h"
#include    "prb_msg.h"
#include    "PrT19S.h"
#include    "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrError_T19S();


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrInit_T19S
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 6
	ARGUMENTS:
		mode,	int,	Input,	,	,
		x_die_size,	double,	Input,	,	,
		y_die_size,	double,	Input,	,	,
		x_start_position,	int,	Input,	,	,
		y_start_position,	int,	Input,	,	,
		units,	int,	Input,	,	,
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include	<string.h>
#include	"prb.h"
#include	"prb_msg.h"
#include	"PrT19S.h"
#include	"prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrInit_T19S(int mode, double x_die_size, double y_die_size, int x_start_position, int y_start_position, int units);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrInk_T19S
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		ink_number,	int,	Input,	,	,	
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include	<string.h>
#include	"prb.h"
#include	"prb_msg.h"
#include	"PrT19S.h"
#include	"prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrInk_T19S(int ink_number);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrLoad_T19S
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include        <string.h>
#include        <time.h>
#include        "prb.h"
#include        "prb_msg.h"
#include        "PrT19S.h"
#include        "prb_extern.h"
#define	POLLTIME	1	              
#define	MAX_ITERATIONS	100		                                        
	END USRLIB MODULE INFORMATION
*/
int PrLoad_T19S();


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrMove_T19S
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 3
	ARGUMENTS:
		x_pos,	int,	Input,	,	,	
		y_pos,	int,	Input,	,	,	
		ink_number,	int,	Input,	,	,	
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include        <string.h>
#include        "prb.h"
#include        "prb_msg.h"
#include        "PrT19S.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrMove_T19S(int x_pos, int y_pos, int ink_number);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrMovNxt_T19S
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		ink_number,	int,	Input,	,	,	
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include        <string.h>
#include        "prb.h"
#include        "prb_msg.h"
#include        "PrT19S.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrMovNxt_T19S(int ink_number);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrRelMove_T19S
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 2
	ARGUMENTS:
		x_value,	double,	Input,	,	,
		y_value,	double,	Input,	,	,
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include        <string.h>
#include        "prb.h"
#include        "prb_msg.h"
#include        "PrT19S.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrRelMove_T19S(double x_value, double y_value);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrRelReturn_T19S
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include        <string.h>
#include        "prb.h"
#include        "prb_msg.h"
#include        "PrT19S.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrRelReturn_T19S();


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrSerialPoll_T19S
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include	"ibup.h"
#include	"prb.h"
#include	"PrT19S.h"
#include	"prb_msg.h"
#include	"prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrSerialPoll_T19S();


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrSetTime_T19S
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		new_time,	int,	Input,	,	,	
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include	<string.h>
#include	"prb.h"
#include	"prb_msg.h"
#include	"PrT19S.h"
#include	"prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrSetTime_T19S(int new_time);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrStatus_T19S
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 5
	ARGUMENTS:
		ready,	int *,	Output,	,	,
		x_location,	int *,	Output,	,	,
		y_location,	int *,	Output,	,	,
		chuck,	int *,	Output,	,	,
		mode,	int *,	Output,	,	,
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include        <string.h>
#include        <stdlib.h>
#include        "prb.h"
#include        "prb_msg.h"
#include        "PrT19S.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrStatus_T19S(int * ready, int * x_location, int * y_location, int * chuck, int * mode);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrWriteRead_T19S
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 6
	ARGUMENTS:
		in_buf,	char *,	Input,	 ,	 ,	 
		inbuf_len,	int,	Input,	 ,	 ,	 
		out_buf,	char *,	Output,	 ,	 ,	 
		oubuf_len,	int,	Input,	 ,	 ,	 
		terminator,	int,	Input,	 ,	 ,	 
		terminator_cnt,	int,	Input,	 ,	 ,	 
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include    <string.h>
#include    "prb.h"
#include    "prb_msg.h"
#include    "PrT19S.h"
#include    "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrWriteRead_T19S(char * in_buf, int inbuf_len, char * out_buf, int oubuf_len, int terminator, int terminator_cnt);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrWriteReadSRQ_T19S
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 6
	ARGUMENTS:
		in_buf,	char *,	Input,	
		inbuf_len,	int,	Input,	
		out_buf,	char *,	Input,	
		oubuf_len,	int,	Input,	
		timeout,	int,	Input,	
		i_srq,	int *,	Output,	
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include    <string.h>
#include    "prb.h"
#include    "prb_msg.h"
#include    "PrT19S.h"
#include    "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrWriteReadSRQ_T19S(char * in_buf, int inbuf_len, char * out_buf, int oubuf_len, int timeout, int * i_srq);


