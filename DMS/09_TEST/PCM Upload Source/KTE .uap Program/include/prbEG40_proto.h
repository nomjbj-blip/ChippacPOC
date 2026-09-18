/* prbEG40 function prototype and KITT header file */

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

	MODULE NAME: Cnfg_EG40
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
#include        "prbEG40_proto.h"
#include        "prb_drvadr.h"
	END USRLIB MODULE INFORMATION
*/
int Cnfg_EG40(int teststation);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrAbsMove_EG40
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 2
	ARGUMENTS:
		x_value,	double,	Input
		y_value,	double,	Input
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include        <string.h>
#include        "prb.h"
#include        "prb_msg.h"
#include        "PrEG40.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrAbsMove_EG40(double x_value, double y_value);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrAutoAlign_EG40
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include	<string.h>
#include	"prb.h"
#include	"prb_msg.h"
#include	"PrEG40.h"
#include	"prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrAutoAlign_EG40();


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrCassetteMap_EG40
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 2
	ARGUMENTS:
		cassette_number,	int,	Input,	
		cassette_map,	int *,	Output,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include	     	<stdlib.h>
#include        <string.h>
#include        "prb.h"
#include        "prb_msg.h"
#include        "PrEG40.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrCassetteMap_EG40(int cassette_number, int * cassette_map);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrCheckOptions_EG40
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 6
	ARGUMENTS:
		OcrPresent,	int *,	Output
		AutoAlnPresent,	int *,	Output
		ProfilerPresent,	int *,	Output
		HotchuckPresent,	int *,	Output
		HandlerPresent,	int *,	Output
		Probe2PadPresent,	int *,	Output
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include        <string.h>
#include        "prb.h"
#include        "prb_msg.h"
#include        "PrEG40.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrCheckOptions_EG40(int * OcrPresent, int * AutoAlnPresent, int * ProfilerPresent, int * HotchuckPresent, int * HandlerPresent, int * Probe2PadPresent);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrChuck_EG40
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		chuck_position,	int,	Input
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include        <string.h>
#include        "prb.h"
#include        "prb_msg.h"
#include        "PrEG40.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrChuck_EG40(int chuck_position);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrClearAll_EG40
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
#include    "PrEG40.h"
	END USRLIB MODULE INFORMATION
*/
int PrClearAll_EG40();


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrClearPipeline_EG40
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include        <string.h>
#include        "prb.h"
#include        "prb_msg.h"
#include        "PrEG40.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrClearPipeline_EG40();


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrError_EG40
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include    <string.h>
#include    "prb.h"
#include    "prb_msg.h"
#include    "PrEG40.h"
#include    "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrError_EG40();


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrGetNxtWafer_EG40
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		cassette_number,	int,	Input
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include        <string.h>
#include        "prb.h"
#include        "prb_msg.h"
#include        "PrEG40.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrGetNxtWafer_EG40(int cassette_number);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrGetProduct_EG40
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		product_file_name,	char *,	Input,	,	,	
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#include <windows.h>		// for Sleep()
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include        <string.h>
#include        "prb.h"
#include        "prb_msg.h"
#include        "PrEG40.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrGetProduct_EG40(char * product_file_name);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrGetWafer_EG40
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 2
	ARGUMENTS:
		cassette_number,	int,	Input
		slot_number,	int,	Input
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include        <string.h>
#include        "prb.h"
#include        "prb_msg.h"
#include        "PrEG40.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrGetWafer_EG40(int cassette_number, int slot_number);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrInit_EG40
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 7
	ARGUMENTS:
		mode,	int,	Input
		x_die_size,	double,	Input
		y_die_size,	double,	Input
		x_start_position,	int,	Input
		y_start_position,	int,	Input
		units,	int,	Input
		subprobtype,	int,	Input
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>                  
#include	<string.h>
#include	"prb.h"
#include	"prb_msg.h"
#include	"PrEG40.h"
#include	"prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrInit_EG40(int mode, double x_die_size, double y_die_size, int x_start_position, int y_start_position, int units, int subprobtype);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrInk_EG40
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		ink_number,	int,	Input
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include	<string.h>
#include	"prb.h"
#include	"prb_msg.h"
#include	"PrEG40.h"
#include	"prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrInk_EG40(int ink_number);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrLoad_EG40
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include        <string.h>
#include        "prb.h"
#include        "prb_msg.h"
#include        "PrEG40.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrLoad_EG40();


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrLoadProduct_EG40
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 2
	ARGUMENTS:
		product_file_name,	char *,	Input,	,	,	
		drive,	char *,	Input,	,	,	
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#include <windows.h>		// for Sleep()
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include        <string.h>
#include        "prb.h"
#include        "prb_msg.h"
#include        "PrEG40.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrLoadProduct_EG40(char * product_file_name, char * drive);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrLowerBoat_EG40
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
#include    "prb.h"
#include    "prb_msg.h"
#include    "prb_extern.h"
#include    "prb_drvadr.h"
#include    "PrEG40.h"
	END USRLIB MODULE INFORMATION
*/
int PrLowerBoat_EG40(int i_pod_number);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrMove_EG40
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 3
	ARGUMENTS:
		x_location,	int,	Input,	,	,	
		y_location,	int,	Input,	,	,	
		ink_number,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include        <string.h>
#include        "prb.h"
#include        "prb_msg.h"
#include        "PrEG40.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrMove_EG40(int x_location, int y_location, int ink_number);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrMovNxt_EG40
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		ink_number,	int,	Input
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include        <string.h>
#include        "prb.h"
#include        "prb_msg.h"
#include        "PrEG40.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrMovNxt_EG40(int ink_number);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrProfile_EG40
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include	<string.h>
#include	"prb.h"
#include	"prb_msg.h"
#include	"PrEG40.h"
#include	"prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrProfile_EG40();


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrPutNxtSlot_EG40
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 2
	ARGUMENTS:
		cassette_number,	int,	Input
		reason_code,	int,	Input
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include        <string.h>
#include        "prb.h"
#include        "prb_msg.h"
#include        "PrEG40.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrPutNxtSlot_EG40(int cassette_number, int reason_code);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrPutWafer_EG40
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 3
	ARGUMENTS:
		cassette_number,	int,	Input
		slot_number,	int,	Input
		reason_code,	int,	Input
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include        <string.h>
#include        "prb.h"
#include        "prb_msg.h"
#include        "PrEG40.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrPutWafer_EG40(int cassette_number, int slot_number, int reason_code);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrReadId_EG40
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		user_buf,	char *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include        <string.h>
#include        "prb.h"
#include        "prb_msg.h"
#include        "PrEG40.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrReadId_EG40(char * user_buf);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrRelMove_EG40
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 2
	ARGUMENTS:
		x_value,	double,	Input
		y_value,	double,	Input
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include        <string.h>
#include        "prb.h"
#include        "prb_msg.h"
#include        "PrEG40.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrRelMove_EG40(double x_value, double y_value);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrRelReturn_EG40
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include    <string.h>
#include    "prb.h"
#include    "prb_msg.h"
#include    "PrEG40.h"
#include    "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrRelReturn_EG40();


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrSerialPoll_EG40
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include	"ibup.h"
#include	"prb.h"
#include	"PrEG40.h"
#include	"prb_msg.h"
#include	"prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrSerialPoll_EG40();


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrSetDiam_EG40
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		diameter,	int,	Input
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include	<string.h>
#include	"prb.h"
#include	"prb_msg.h"
#include	"PrEG40.h"
#include	"prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrSetDiam_EG40(int diameter);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrSetDieSize_EG40
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 2
	ARGUMENTS:
		x_die_size,	double,	Input
		y_die_size,	double,	Input
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>                  
#include    <string.h>
#include    "prb.h"
#include    "prb_msg.h"
#include    "PrEG40.h"
#include    "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrSetDieSize_EG40(double x_die_size, double y_die_size);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrSetFlat_EG40
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		flat_number,	int,	Input
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include	<string.h>
#include	"prb.h"
#include	"prb_msg.h"
#include	"PrEG40.h"
#include	"prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrSetFlat_EG40(int flat_number);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrSetMode_EG40
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		mode,	int,	Input
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include        <string.h>
#include        "prb.h"
#include        "prb_msg.h"
#include        "PrEG40.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrSetMode_EG40(int mode);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrSetPipeline_EG40
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		on_off,	int,	Input
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include        <string.h>
#include        "prb.h"
#include        "prb_msg.h"
#include        "PrEG40.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrSetPipeline_EG40(int on_off);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrSetQuadrant_EG40
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		quad_number,	int,	Input
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include	<string.h>
#include	"prb.h"
#include	"prb_msg.h"
#include	"PrEG40.h"
#include	"prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrSetQuadrant_EG40(int quad_number);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrSetRefDie_EG40
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 2
	ARGUMENTS:
		x_start_position,	int,	Input,	,	,	
		y_start_position,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>                 
#include    <string.h>
#include    "prb.h"
#include    "prb_msg.h"
#include    "PrEG40.h"
#include    "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrSetRefDie_EG40(int x_start_position, int y_start_position);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrSetSlotStatus_EG40
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 3
	ARGUMENTS:
		cassette,	int,	Input
		slot,	int,	Input
		status_code,	int,	Input
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include        <string.h>
#include        "prb.h"
#include        "prb_msg.h"
#include        "PrEG40.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrSetSlotStatus_EG40(int cassette, int slot, int status_code);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrSetTime_EG40
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		new_time,	int,	Input
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include	<string.h>
#include	"prb.h"
#include	"prb_msg.h"
#include	"PrEG40.h"
#include	"prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrSetTime_EG40(int new_time);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrSetUnits_EG40
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		units,	int,	Input
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include    <math.h>                    
#include    <string.h>
#include    "prb.h"
#include    "prb_msg.h"
#include    "PrEG40.h"
#include    "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrSetUnits_EG40(int units);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrSmifClamp_EG40
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 2
	ARGUMENTS:
		i_pod_number,	int,	Input,	,	,	
		i_clamp_state,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>                  
#include	<string.h>
#include	"prb.h"
#include	"prb_msg.h"
#include	"PrEG40.h"
#include	"prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrSmifClamp_EG40(int i_pod_number, int i_clamp_state);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrSmifLock_EG40
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 2
	ARGUMENTS:
		i_pod_number,	int,	Input,	,	,	
		i_lock_state,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>                  
#include	<string.h>
#include	"prb.h"
#include	"prb_msg.h"
#include	"PrEG40.h"
#include	"prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrSmifLock_EG40(int i_pod_number, int i_lock_state);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrSmifStatus_EG40
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 3
	ARGUMENTS:
		i_pod_number,	int,	Input,	,	,	
		i_status_array,	I_ARRAY_T,	Input,	,	,	
		i_status_array_size,	int,	Input,	,	,	
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#define _REENTRANT
#include <stdio.h>
#include <string.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include    "prb.h"
#include "PrEG40.h"
#include    "prb_func_id.h"
#include    "prb_extern.h"
#include    "prb_drvadr.h"
#include    "prb_msg.h"
	END USRLIB MODULE INFORMATION
*/
int PrSmifStatus_EG40(int i_pod_number, int *i_status_array, int i_status_array_size);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrStatus_EG40
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 5
	ARGUMENTS:
		ready,	int *,	Output
		x_location,	int *,	Output
		y_location,	int *,	Output
		chuck,	int *,	Output
		mode,	int *,	Output
	INCLUDES:
#define _REENTRANT
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include        <string.h>
#include        "prb.h"
#include        "prb_msg.h"
#include        "PrEG40.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrStatus_EG40(int * ready, int * x_location, int * y_location, int * chuck, int * mode);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrUnLoad_EG40
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include        <string.h>
#include        "prb.h"
#include        "prb_msg.h"
#include        "PrEG40.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrUnLoad_EG40();


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrWriteRead_EG40
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
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include    <string.h>
#include    "prb.h"
#include    "prb_msg.h"
#include    "PrEG40.h"
#include    "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrWriteRead_EG40(char * in_buf, int inbuf_len, char * out_buf, int oubuf_len, int terminator, int terminator_cnt);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrWriteReadSRQ_EG40
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
#include    "PrEG40.h"
#include    "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrWriteReadSRQ_EG40(char * in_buf, int inbuf_len, char * out_buf, int oubuf_len, int timeout, int * i_srq);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrZParams_EG40
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 2
	ARGUMENTS:
		function,	int,	Input
		value,	int,	Input
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include	<string.h>
#include	"prb.h"
#include	"prb_msg.h"
#include	"PrEG40.h"
#include	"prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrZParams_EG40(int function, int value);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrZTravel_EG40
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		number,	int,	Input
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include    <string.h>
#include    "prb.h"
#include    "prb_msg.h"
#include    "PrEG40.h"
#include    "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrZTravel_EG40(int number);


