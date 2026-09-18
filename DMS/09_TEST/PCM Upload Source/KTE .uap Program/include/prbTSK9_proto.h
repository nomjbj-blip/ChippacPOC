/* prbTSK9 function prototype and KITT header file */

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

	MODULE NAME: Cnfg_TSK9
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
#include        "prb.h"
#include        "prb_func_id.h"
#include        "prb_extern.h"
#include	     	 "prb_msg.h"
#include "prbTSK9_proto.h"
#include "PrTSK9.h"
#include    "prb_drvadr.h"
	END USRLIB MODULE INFORMATION
*/
int Cnfg_TSK9(int teststation);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrAbsMove_TSK9
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 2
	ARGUMENTS:
		x_value,	double,	Input,	,	,	
		y_value,	double,	Input,	,	,	
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
#include        "prb.h"
#include        "prb_msg.h"
#include        "PrTSK9.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrAbsMove_TSK9(double x_value, double y_value);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrCassetteMap_TSK9
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
#include        "PrTSK9.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrCassetteMap_TSK9(int cassette_number, int * cassette_map);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrCheckOptions_TSK9
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 6
	ARGUMENTS:
		OcrPresent,	int *,	Output,	
		AutoAlnPresent,	int *,	Output,	
		ProfilerPresent,	int *,	Output,	
		HotchuckPresent,	int *,	Output,	
		HandlerPresent,	int *,	Output,	
		Probe2PadPresent,	int *,	Output,	
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
#include        "prb.h"
#include        "prb_msg.h"
#include        "PrTSK9.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrCheckOptions_TSK9(int * OcrPresent, int * AutoAlnPresent, int * ProfilerPresent, int * HotchuckPresent, int * HandlerPresent, int * Probe2PadPresent);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrChuck_TSK9
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
#include        "PrTSK9.h"
#include        "prb_extern.h"
#ifdef WIN32
	_declspec (dllimport) prb_t Prb[];
#endif
	END USRLIB MODULE INFORMATION
*/
int PrChuck_TSK9(int chuck_position);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrClearAll_TSK9
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
#include    "PrTSK9.h"
	END USRLIB MODULE INFORMATION
*/
int PrClearAll_TSK9();


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrError_TSK9
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
#include    "PrTSK9.h"
#include    "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrError_TSK9();


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrGetNxtWafer_TSK9
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
#include        "PrTSK9.h"
#ifdef WIN32
#define	POLLTIME	2*1000		              
#else
#define	POLLTIME	2		              
#endif
#define	MAX_ITERATIONS	50		                                        
	END USRLIB MODULE INFORMATION
*/
int PrGetNxtWafer_TSK9(int cassette_number);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrGetProduct_TSK9
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		product_file_name,	char *,	Output,	,	,	
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
#include        "PrTSK9.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrGetProduct_TSK9(char * product_file_name);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrGetWafer_TSK9
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
#include "PrTSK9.h"
#ifdef WIN32
#define	POLLTIME	2*1000		              
#else
#define	POLLTIME	2		              
#endif
#define	MAX_ITERATIONS	50		                                        
	END USRLIB MODULE INFORMATION
*/
int PrGetWafer_TSK9(int cassette_number, int slot_number);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrInit_TSK9
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
#include	"PrTSK9.h"
#include	"prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrInit_TSK9(int mode, double x_die_size, double y_die_size, int x_start_position, int y_start_position, int units);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrInk_TSK9
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
#include	"PrTSK9.h"
#include	"prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrInk_TSK9(int ink_number);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrLoad_TSK9
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
#ifdef WIN32
#include "windows.h"
#endif
#include        "prb.h"
#include        "prb_msg.h"
#include        "PrTSK9.h"
#include        "prb_extern.h"
#ifdef WIN32
#define	POLLTIME	2*1000		              
#else
#define	POLLTIME	2		              
#endif
	                                        
extern pr_srq_list pr_srq[];

	END USRLIB MODULE INFORMATION
*/
int PrLoad_TSK9();


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrLoadProduct_TSK9
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 2
	ARGUMENTS:
		product_file_name,	char *,	Input,	,	 ,	 
		drive,	char *,	Input,	,	 ,	 
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
#include        "PrTSK9.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrLoadProduct_TSK9(char * product_file_name, char * drive);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrLowerBoat_TSK9
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
#include        "PrTSK9.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrLowerBoat_TSK9(int i_pod_number);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrMove_TSK9
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
#include        "PrTSK9.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrMove_TSK9(int x_pos, int y_pos, int ink_number);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrMovNxt_TSK9
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
#include        "PrTSK9.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrMovNxt_TSK9(int ink_number);


/* USRLIB MODULE INFORMATION

	MODULE NAME: prproberready_TSK9
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
#include        "PrTSK9.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int prproberready_TSK9();


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrReadId_TSK9
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		user_buf,	char *,	Output,	,	,	
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
#include        "PrTSK9.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrReadId_TSK9(char * user_buf);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrRelMove_TSK9
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
#include        "PrTSK9.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrRelMove_TSK9(double x_value, double y_value);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrRelReturn_TSK9
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
#include        "PrTSK9.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrRelReturn_TSK9();


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrSerialPoll_TSK9
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
#include	"PrTSK9.h"
#include	"prb_msg.h"
#include	"prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrSerialPoll_TSK9();


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrSetDiam_TSK9
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		diameter,	int,	Input,	,	,	
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
#include	"PrTSK9.h"
#include	"prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrSetDiam_TSK9(int diameter);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrSetDieSize_TSK9
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
#include    "PrTSK9.h"
#include    "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrSetDieSize_TSK9(double x_die_size, double y_die_size);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrSetFlat_TSK9
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		flat_number,	int,	Input,	,	,	
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
#include	"PrTSK9.h"
#include	"prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrSetFlat_TSK9(int flat_number);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrSetMode_TSK9
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		mode,	int,	Input,	,	,	
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
#include        "PrTSK9.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrSetMode_TSK9(int mode);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrSetRefDie_TSK9
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 2
	ARGUMENTS:
		x_start_position,	int,	Input,	,	,	
		y_start_position,	int,	Input,	,	,	
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
#include    "PrTSK9.h"
#include    "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrSetRefDie_TSK9(int x_start_position, int y_start_position);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrSetSlotStatus_TSK9
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
#include        "PrTSK9.h"
#include        "prb_extern.h"

TSK9_slot_list_struct TSK9_wafer_list;
	END USRLIB MODULE INFORMATION
*/
int PrSetSlotStatus_TSK9(int cassette, int slot, int status_code);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrSetTime_TSK9
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		new_time,	int,	Input,	
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
#include	"PrTSK9.h"
#include	"prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrSetTime_TSK9(int new_time);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrSmifClamp_TSK9
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
#include	"PrTSK9.h"
#include	"prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrSmifClamp_TSK9(int i_pod_number, int i_clamp_state);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrSmifLock_TSK9
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 2
	ARGUMENTS:
		i_pod_number,	int,	Input,	,	,	
		i_lock_state,	int,	Input,	,	,	
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
#include        "PrTSK9.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrSmifLock_TSK9(int i_pod_number, int i_lock_state);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrSmifStatus_TSK9
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
#include "PrTSK9.h"
#include    "prb_func_id.h"
#include    "prb_extern.h"
#include    "prb_drvadr.h"
#include    "prb_msg.h"
	END USRLIB MODULE INFORMATION
*/
int PrSmifStatus_TSK9(int i_pod_number, int *i_status_array, int i_status_array_size);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrStart_TSK9
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
#include        "PrTSK9.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrStart_TSK9();


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrStatus_TSK9
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 5
	ARGUMENTS:
		ready,	int *,	Input,	,	,	
		x_location,	int *,	Input,	,	,	
		y_location,	int *,	Input,	,	,	
		chuck,	int *,	Input,	,	,	
		mode,	int *,	Input,	,	,	
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
#include        "prb_extern.h"
#include 		<ctype.h>
#include        "prb_msg.h"
#include        "PrTSK9.h"
	END USRLIB MODULE INFORMATION
*/
int PrStatus_TSK9(int * ready, int * x_location, int * y_location, int * chuck, int * mode);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrStop_TSK9
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
#include        "PrTSK9.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrStop_TSK9();


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrUnLoad_TSK9
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
#include        "prb_extern.h"
#ifdef WIN32
#include "windows.h"
#endif
#include        "prb_msg.h"
#include        "PrTSK9.h"
#ifdef WIN32
#define	POLLTIME	2*1000		              
#else
#define	POLLTIME	2		              
#endif
#define	MAX_ITERATIONS	50		                                        
	END USRLIB MODULE INFORMATION
*/
int PrUnLoad_TSK9();


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrWriteRead_TSK9
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 6
	ARGUMENTS:
		in_buf,	char *,	Input,	
		inbuf_len,	int,	Input,	
		out_buf,	char *,	Input,	
		oubuf_len,	int,	Input,	
		terminator,	int,	Input,	
		terminator_cnt,	int,	Input,	
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
#include    "PrTSK9.h"
#include    "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrWriteRead_TSK9(char * in_buf, int inbuf_len, char * out_buf, int oubuf_len, int terminator, int terminator_cnt);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrWriteReadSRQ_TSK9
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 6
	ARGUMENTS:
		in_buf,	char *,	Input,	,	 ,	 
		inbuf_len,	int,	Input,	,	,	
		out_buf,	char *,	Input,	,	 ,	 
		oubuf_len,	int,	Input,	,	,	
		timeout,	int,	Input,	,	,	
		i_srq,	int *,	Output,	,	,	
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
#include    "PrTSK9.h"
#include    "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrWriteReadSRQ_TSK9(char * in_buf, int inbuf_len, char * out_buf, int oubuf_len, int timeout, int * i_srq);


