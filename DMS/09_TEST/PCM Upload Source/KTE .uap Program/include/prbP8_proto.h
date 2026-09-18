/* prbP8 function prototype and KITT header file */

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

	MODULE NAME: Cnfg_P8
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
#include        "prb_msg.h"
#include        "prb_func_id.h"
#include        "prb_extern.h"
#include        "PrP8.h"
#include        "prbP8_proto.h"
#include    "prb_drvadr.h"
	END USRLIB MODULE INFORMATION
*/
int Cnfg_P8(int teststation);


/* USRLIB MODULE INFORMATION

	MODULE NAME: getProberPosition
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 2
	ARGUMENTS:
		xp,	int *,	Output,	
		yp,	int *,	Output,	
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <string.h>
#include <stdlib.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "prb.h"
#include	"prb_extern.h"
#include "prb_msg.h"
#include "PrP8.h"
	END USRLIB MODULE INFORMATION
*/
int getProberPosition(int * xp, int * yp);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrAbsMove_P8
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
#include        "PrP8.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrAbsMove_P8(double x_value, double y_value);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrCassetteMap_P8
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
#include     		<stdlib.h>
#include        <string.h>
#include        "prb.h"
#include        "prb_msg.h"
#include        "PrP8.h"
#include        "prb_extern.h"
#define LF     '\n'
	END USRLIB MODULE INFORMATION
*/
int PrCassetteMap_P8(int cassette_number, int * cassette_map);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrCassetteMask_P8
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		cassette,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include        <string.h>
#include        "prb.h"
#include        "prb_msg.h"
#include        "PrP8.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrCassetteMask_P8(int cassette);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrCheckOptions_P8
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 6
	ARGUMENTS:
		OcrPresent,	int *,	Output
		AutoAlnPresent,	int *,	Output
		ProfilerPresent,	int *,	Output
		HotchuckPresent,	int *,	Output
		HandlerPresent,	int *,	Output
		Probe2PadPresent ,	int *,	Output
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
#include        "PrP8.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrCheckOptions_P8(int * OcrPresent, int * AutoAlnPresent, int * ProfilerPresent, int * HotchuckPresent, int * HandlerPresent, int * Probe2PadPresent );


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrChuck_P8
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		chuck_position,	int,	Input,	
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <string.h>
#include        "prb.h"
#include        "prb_msg.h"
#include        "PrP8.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrChuck_P8(int chuck_position);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrClearAll_P8
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
#include    "prb.h"
#include    "prb_extern.h"
#include    "prb_drvadr.h"
#include    "prb_msg.h"
#include    "PrP8.h"
	END USRLIB MODULE INFORMATION
*/
int PrClearAll_P8();


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrError_P8
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
#include    <string.h>
#include    "prb.h"
#include    "prb_msg.h"
#include    "PrP8.h"
#include    "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrError_P8();


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrGetNxtWafer_P8
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		cassette_number,	int,	Input,	,	,	
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <stdlib.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include        <string.h>
#include        <time.h>
#include        "prb.h"
#include        "prb_extern.h"
#ifdef WIN32
#include "windows.h"
#endif
#include        "prb_msg.h"
#include        "PrP8.h"
#ifdef WIN32
#define	POLLTIME	2*1000		              
#else
#define	POLLTIME	2		              
#endif
#define	MAX_ITERATIONS	50		                                        
	END USRLIB MODULE INFORMATION
*/
int PrGetNxtWafer_P8(int cassette_number);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrGetWafer_P8
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 2
	ARGUMENTS:
		cassette_number,	int,	Input,	,	,	
		slot_number,	int,	Input,	,	,	
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <string.h>
#include "prb.h"
#include "prb_extern.h"
#ifdef WIN32
#include <windows.h>
#endif
#include "prb_msg.h"
#include "PrP8.h"
#ifdef WIN32
#define	POLLTIME	2*1000		              
#else
#define	POLLTIME	2		              
#endif
#define	MAX_ITERATIONS	50		                                        
	END USRLIB MODULE INFORMATION
*/
int PrGetWafer_P8(int cassette_number, int slot_number);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrInit_P8
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 7
	ARGUMENTS:
		mode,	int,	Input,	
		x_die_size,	double,	Input,	
		y_die_size,	double,	Input,	
		x_start_position,	int,	Input,	
		y_start_position,	int,	Input,	
		units,	int,	Input,	
		subprobtype,	int,	Input,	
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
#include	"PrP8.h"
#include	"prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrInit_P8(int mode, double x_die_size, double y_die_size, int x_start_position, int y_start_position, int units, int subprobtype);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrLoad_P8
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
#include        <string.h>
#include        <time.h>
#include <math.h>
#include        "prb.h"
#include        "prb_msg.h"
#include        "PrP8.h"
#include        "prb_extern.h"
#ifdef WIN32
#include <windows.h>
#endif
#ifdef WIN32
#define	POLLTIME	1 * 1000	// _sleep() is ms on NT!
#else
#define	POLLTIME	1
#endif
#define	MAX_ITERATIONS	100
	END USRLIB MODULE INFORMATION
*/
int PrLoad_P8();


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrLoadProduct_P8
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 2
	ARGUMENTS:
		product_file_name,	char *,	Input,	
		drive,	char *,	Input,	
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
#include        "PrP8.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrLoadProduct_P8(char * product_file_name, char * drive);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrLowerBoat_P8
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		i_pod_number,	int,	Input,	,	,	
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
#include        "PrP8.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrLowerBoat_P8(int i_pod_number);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrMove_P8
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 3
	ARGUMENTS:
		x_pos,	int,	Input,	
		y_pos,	int,	Input,	
		ink_number,	int,	Input,	
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
#include        "PrP8.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrMove_P8(int x_pos, int y_pos, int ink_number);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrMovNxt_P8
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		ink_number,	int,	Input
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
#include        "PrP8.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrMovNxt_P8(int ink_number);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrQueryChuckTemp_P8
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		chuck_temp,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include        <string.h>
#include        "prb.h"
#include        "prb_msg.h"
#include        "PrP8.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrQueryChuckTemp_P8(double * chuck_temp);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrReadId_P8
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		user_buf,	char *,	Output,	
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
#include        "PrP8.h"
#include        "prb_extern.h"
#define CR '\r'
#define LF '\n'
	END USRLIB MODULE INFORMATION
*/
int PrReadId_P8(char * user_buf);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrRelMove_P8
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
#include        "PrP8.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrRelMove_P8(double x_value, double y_value);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrRelReturn_P8
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
#include        "PrP8.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrRelReturn_P8();


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrSerialPoll_P8
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <string.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include	"ibup.h"
#include	"prb.h"
#include	"PrP8.h"
#include	"prb_msg.h"
#include	"prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrSerialPoll_P8();


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrSetChuckTemp_P8
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		chuck_temp,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include        <string.h>
#include        "prb.h"
#include        "prb_msg.h"
#include        "PrP8.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrSetChuckTemp_P8(double chuck_temp);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrSetSlotStatus_P8
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 3
	ARGUMENTS:
		cassette,	int,	Input,	,	,	
		slot,	int,	Input,	,	,	
		status_code,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include        <string.h>
#include        "prb.h"
#include        "prb_msg.h"
#include        "PrP8.h"
#include        "prb_extern.h"

P8_slot_list_struct P8_wafer_list;
	END USRLIB MODULE INFORMATION
*/
int PrSetSlotStatus_P8(int cassette, int slot, int status_code);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrSetTime_P8
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		new_time,	int,	Input
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
#include	"PrP8.h"
#include	"prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrSetTime_P8(int new_time);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrStatus_P8
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 5
	ARGUMENTS:
		ready,	int *,	Output,	
		x_location,	int *,	Output,	
		y_location,	int *,	Output,	
		chuck,	int *,	Output,	
		mode,	int *,	Output,	
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
#include        "PrP8.h"
#include        "prb_extern.h"
#include 		<ctype.h>
	END USRLIB MODULE INFORMATION
*/
int PrStatus_P8(int * ready, int * x_location, int * y_location, int * chuck, int * mode);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrUnLoad_P8
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
#include        "PrP8.h"
#include        "prb_extern.h"
#define	POLLTIME	2		              
#define	MAX_ITERATIONS	50		                                        
	END USRLIB MODULE INFORMATION
*/
int PrUnLoad_P8();


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrWriteRead_P8
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 6
	ARGUMENTS:
		in_buf,	char *,	Input
		inbuf_len,	int,	Input
		out_buf,	char *,	Output
		outbuf_len,	int,	Input
		terminator,	int,	Input
		terminator_cnt,	int,	Input
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include    <string.h>
#include    "prb.h"
#include    "prb_msg.h"
#include    "PrP8.h"
#include    "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrWriteRead_P8(char * in_buf, int inbuf_len, char * out_buf, int outbuf_len, int terminator, int terminator_cnt);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrWriteReadSRQ_P8
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
#include    "PrP8.h"
#include    "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrWriteReadSRQ_P8(char * in_buf, int inbuf_len, char * out_buf, int oubuf_len, int timeout, int * i_srq);


/* USRLIB MODULE INFORMATION

	MODULE NAME: setChuck
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 2
	ARGUMENTS:
		raiseChuck,	int,	Input,	
		i_srq,	int *,	Input,	
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "prb.h"
#include	"prb_extern.h"
#include "prb_msg.h"
#include "PrP8.h"
	END USRLIB MODULE INFORMATION
*/
int setChuck(int raiseChuck, int * i_srq);


/* USRLIB MODULE INFORMATION

	MODULE NAME: verifyNumeric
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 2
	ARGUMENTS:
		cp,	char *,	Input,	
		n,	int,	Input,	
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "prb.h"
#include	"prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int verifyNumeric(char * cp, int n);


