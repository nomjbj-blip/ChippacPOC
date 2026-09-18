/* prbEG2X function prototype and KITT header file */

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

	MODULE NAME: Cnfg_EG2X
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		teststation,	int,	Input,	
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
#include        "prbEG2X_proto.h"
#include    "prb_drvadr.h"
	END USRLIB MODULE INFORMATION
*/
int Cnfg_EG2X(int teststation);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrAbsMove_EG2X
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 2
	ARGUMENTS:
		x_value,	double,	Input,	
		y_value,	double,	Input,	
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
#include        "PrEG2X.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrAbsMove_EG2X(double x_value, double y_value);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrAutoAlign_EG2X
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
#include        "PrEG2X.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrAutoAlign_EG2X();


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrCassetteMap_EG2X
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 2
	ARGUMENTS:
		cassette_number,	int,	Input,	
		cassette_map,	int *,	Input,	
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include        <stdlib.h>
#include        <string.h>
#include        "prb.h"
#include        "prb_msg.h"
#include        "PrEG2X.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrCassetteMap_EG2X(int cassette_number, int * cassette_map);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrCheckOptions_EG2X
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 6
	ARGUMENTS:
		OcrPresent,	int *,	Input,	
		AutoAlnPresent,	int *,	Input,	
		ProfilerPresent,	int *,	Input,	
		HotchuckPresent,	int *,	Input,	
		HandlerPresent,	int *,	Input,	
		Probe2PadPresent,	int *,	Input,	
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
#include        "PrEG2X.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrCheckOptions_EG2X(int * OcrPresent, int * AutoAlnPresent, int * ProfilerPresent, int * HotchuckPresent, int * HandlerPresent, int * Probe2PadPresent);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrChuck_EG2X
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		chuck_position,	int,	Input
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
#include        "PrEG2X.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrChuck_EG2X(int chuck_position);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrClearPipeline_EG2X
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
#include        "PrEG2X.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrClearPipeline_EG2X();


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrError_EG2X
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
#include    <stdlib.h>
#include    "prb.h"
#include    "prb_msg.h"
#include    "PrEG2X.h"
#include    "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrError_EG2X();


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrGetNxtWafer_EG2X
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		cassette_number,	int,	Input,	
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
#include        "PrEG2X.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrGetNxtWafer_EG2X(int cassette_number);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrGetWafer_EG2X
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 2
	ARGUMENTS:
		cassette_number,	int,	Input,	
		slot_number,	int,	Input,	
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
#include        "PrEG2X.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrGetWafer_EG2X(int cassette_number, int slot_number);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrInit_EG2X
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 7
	ARGUMENTS:
		mode,	int,	Input,	,	,	
		x_die_size,	double,	Input,	,	,	
		y_die_size,	double,	Input,	,	,	
		x_start_position,	int,	Input,	,	,	
		y_start_position,	int,	Input,	,	,	
		units,	int,	Input,	,	,	
		subprobtype,	int,	Input,	,	,	
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
#include        "PrEG2X.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrInit_EG2X(int mode, double x_die_size, double y_die_size, int x_start_position, int y_start_position, int units, int subprobtype);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrInk_EG2X
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
#include        "PrEG2X.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrInk_EG2X(int ink_number);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrLoad_EG2X
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
#include        "PrEG2X.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrLoad_EG2X();


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrLoadProduct_EG2X
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 2
	ARGUMENTS:
		product_file_name,	char *,	Input
		drive,	char *,	Input
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#ifdef WIN32
#include <windows.h>
#endif
#include        <string.h>
#include        "prb.h"
#include        "prb_msg.h"
#include        "PrEG2X.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrLoadProduct_EG2X(char * product_file_name, char * drive);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrMove_EG2X
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 3
	ARGUMENTS:
		x_location,	int,	Input
		y_location,	int,	Input
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
#include        "PrEG2X.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrMove_EG2X(int x_location, int y_location, int ink_number);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrMovNxt_EG2X
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
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
#include        <stdlib.h>
#include        "prb.h"
#include        "prb_msg.h"
#include        "PrEG2X.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrMovNxt_EG2X(int ink_number);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrProfile_EG2X
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
#include        "PrEG2X.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrProfile_EG2X();


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrPutNxtSlot_EG2X
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 2
	ARGUMENTS:
		cassette_number,	int,	Input,	
		reason_code,	int,	Input,	
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
#include        "PrEG2X.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrPutNxtSlot_EG2X(int cassette_number, int reason_code);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrPutWafer_EG2X
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 3
	ARGUMENTS:
		cassette_number,	int,	Input,	
		slot_number,	int,	Input,	
		reason_code,	int,	Input,	
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
#include        "PrEG2X.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrPutWafer_EG2X(int cassette_number, int slot_number, int reason_code);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrReadId_EG2X
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		user_buf,	char *,	Input
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
#include        "PrEG2X.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrReadId_EG2X(char * user_buf);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrRelMove_EG2X
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 2
	ARGUMENTS:
		x_value,	double,	Input,	
		y_value,	double,	Input,	
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
#include        "PrEG2X.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrRelMove_EG2X(double x_value, double y_value);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrRelReturn_EG2X
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
#include    "PrEG2X.h"
#include    "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrRelReturn_EG2X();


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrSerialPoll_EG2X
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
#include	"PrEG2X.h"
#include	"prb_msg.h"
#include	"prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrSerialPoll_EG2X();


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrSetDiam_EG2X
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		diameter,	int,	Input
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
#include        "PrEG2X.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrSetDiam_EG2X(int diameter);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrSetDieSize_EG2X
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 2
	ARGUMENTS:
		x_die_size,	double,	Input,	,	,
		y_die_size,	double,	Input,	,	,
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
#include    "PrEG2X.h"
#include    "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrSetDieSize_EG2X(double x_die_size, double y_die_size);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrSetFlat_EG2X
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		flat_number,	int,	Input
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
#include        "PrEG2X.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrSetFlat_EG2X(int flat_number);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrSetMode_EG2X
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		mode,	int,	Input
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
#include        "PrEG2X.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrSetMode_EG2X(int mode);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrSetPipeline_EG2X
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		on_off,	int,	Input
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
#include        "PrEG2X.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrSetPipeline_EG2X(int on_off);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrSetQuadrant_EG2X
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		quad_number,	int,	Input,	
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
#include        "PrEG2X.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrSetQuadrant_EG2X(int quad_number);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrSetRefDie_EG2X
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 2
	ARGUMENTS:
		x_start_position,	int,	Input
		y_start_position,	int,	Input
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
#include    "PrEG2X.h"
#include    "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrSetRefDie_EG2X(int x_start_position, int y_start_position);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrSetSlotStatus_EG2X
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 3
	ARGUMENTS:
		cassette,	int,	Input
		slot,	int,	Input
		status_code,	int,	Input
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
#include        "PrEG2X.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrSetSlotStatus_EG2X(int cassette, int slot, int status_code);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrSetTime_EG2X
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
#include        <string.h>
#include        "prb.h"
#include        "prb_msg.h"
#include        "PrEG2X.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrSetTime_EG2X(int new_time);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrSetUnits_EG2X
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		units,	int,	Input
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
#include    "PrEG2X.h"
#include    "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrSetUnits_EG2X(int units);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrStatus_EG2X
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 5
	ARGUMENTS:
		ready,	int *,	Input,	
		x_location,	int *,	Input,	
		y_location,	int *,	Input,	
		chuck,	int *,	Input,	
		mode,	int *,	Input,	
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#define _REENTRANT
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include        <string.h>
#include        <stdlib.h>
#include        "prb.h"
#include        "prb_msg.h"
#include        "PrEG2X.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrStatus_EG2X(int * ready, int * x_location, int * y_location, int * chuck, int * mode);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrUnLoad_EG2X
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
#include        "PrEG2X.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrUnLoad_EG2X();


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrWriteRead_EG2X
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 6
	ARGUMENTS:
		in_buf,	char *,	Input
		inbuf_len,	int,	Input
		out_buf,	char *,	Input
		oubuf_len,	int,	Input
		terminator,	int,	Input
		terminator_cnt,	int,	Input
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
#include    "PrEG2X.h"
#include    "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrWriteRead_EG2X(char * in_buf, int inbuf_len, char * out_buf, int oubuf_len, int terminator, int terminator_cnt);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrWriteReadSRQ_EG2X
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
#include    "PrEG2X.h"
#include    "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrWriteReadSRQ_EG2X(char * in_buf, int inbuf_len, char * out_buf, int oubuf_len, int timeout, int * i_srq);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrZParams_EG2X
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 2
	ARGUMENTS:
		function,	int,	Input
		value,	int,	Input
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
#include        "PrEG2X.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrZParams_EG2X(int function, int value);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrZTravel_EG2X
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		number,	int,	Input
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
#include    "PrEG2X.h"
#include    "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrZTravel_EG2X(int number);


