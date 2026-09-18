/* prbFAKE function prototype and KITT header file */

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

	MODULE NAME: Cnfg_FAKE
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		teststation,	int,	Input,	,	,	
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <stdarg.h>
#include <string.h>
#include	"prb.h"
#include	"prb_func_id.h"
#include	"prb_extern.h"
#include 	"prbFAKE_proto.h"
#include    "prb_drvadr.h"
#include    "prb_msg.h"
#include "lptdef.h"
#include "lptdef_lowercase.h"
extern char* getenv(const char*);
	END USRLIB MODULE INFORMATION
*/
int Cnfg_FAKE(int teststation);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrAbsMove_FAKE
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
#include <math.h>
#include <prb.h>
#include "PrFAKE.h"
	END USRLIB MODULE INFORMATION
*/
int PrAbsMove_FAKE(double x_value, double y_value);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrAutoAlign_FAKE
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
#include	"prb.h"
#include	"prb_msg.h"
#include	"PrFAKE.h"
	END USRLIB MODULE INFORMATION
*/
int PrAutoAlign_FAKE();


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrCassetteMap_FAKE
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
#include <string.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include        "prb.h"
#include        "prb_msg.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrCassetteMap_FAKE(int cassette_number, int * cassette_map);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrCassetteMask_FAKE
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		cassette,	int,	Input,	,	,	
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include        "prb.h"
#include	"PrFAKE.h"
#include        "prb_msg.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrCassetteMask_FAKE(int cassette);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrCheckOptions_FAKE
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
#define _REENTRANT
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include        "prb.h"
#include	"PrFAKE.h"
#include        "prb_msg.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrCheckOptions_FAKE(int * OcrPresent, int * AutoAlnPresent, int * ProfilerPresent, int * HotchuckPresent, int * HandlerPresent, int * Probe2PadPresent);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrChuck_FAKE
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
#include	"prb.h"
#include	"PrFAKE.h"
#include	"prb_extern.h"
#include	"prb_msg.h"
	END USRLIB MODULE INFORMATION
*/
int PrChuck_FAKE(int chuck_position);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrClearPipeline_FAKE
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
#include	"prb.h"
#include	"PrFAKE.h"
#include	"prb_msg.h"
	END USRLIB MODULE INFORMATION
*/
int PrClearPipeline_FAKE();


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrError_FAKE
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
#include	"prb.h"
#include	"PrFAKE.h"
#include	"prb_msg.h"
	END USRLIB MODULE INFORMATION
*/
int PrError_FAKE();


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrGetNxtWafer_FAKE
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
#include	"prb.h"
#include	"PrFAKE.h"
#include	"prb_msg.h"
	END USRLIB MODULE INFORMATION
*/
int PrGetNxtWafer_FAKE(int cassette_number);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrGetProduct_FAKE
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
#include        "prb.h"
#include	"PrFAKE.h"
#include        "prb_msg.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrGetProduct_FAKE(char * product_file_name);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrGetWafer_FAKE
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
#include	"prb.h"
#include	"PrFAKE.h"
#include	"prb_msg.h"
	END USRLIB MODULE INFORMATION
*/
int PrGetWafer_FAKE(int cassette_number, int slot_number);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrInit_FAKE
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
#include	"prb.h"
#include	"PrFAKE.h"
#include	"prb_msg.h"
#include	"prb_extern.h"
#include "kui_proto.h"
	END USRLIB MODULE INFORMATION
*/
int PrInit_FAKE(int mode, double x_die_size, double y_die_size, int x_start_position, int y_start_position, int units, int subprobtype);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrLoad_FAKE
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
#include	"prb.h"
#include	"PrFAKE.h"
#include	"prb_msg.h"
#include "COM_usrlib.h"
	END USRLIB MODULE INFORMATION
*/
int PrLoad_FAKE();


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrLoadProduct_FAKE
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 2
	ARGUMENTS:
		product_file_name,	char *,	Input,	,	,	
		drive,	char *,	Input,	,	,	
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include        "prb.h"
#include	"PrFAKE.h"
#include        "prb_msg.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrLoadProduct_FAKE(char * product_file_name, char * drive);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrMove_FAKE
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 3
	ARGUMENTS:
		x_location,	int,	Input,	
		y_location,	int,	Input,	
		ink_number,	int,	Input,	
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include	"prb.h"
#include	"PrFAKE.h"
#include	"prb_extern.h"
#include	"prb_msg.h"
	END USRLIB MODULE INFORMATION
*/
int PrMove_FAKE(int x_location, int y_location, int ink_number);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrMovNxt_FAKE
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
#include	"prb.h"
#include	"PrFAKE.h"
#include	"prb_msg.h"
	END USRLIB MODULE INFORMATION
*/
int PrMovNxt_FAKE(int ink_number);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrNeedleClean_FAKE
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		clean_function,	int,	Input,	,	,	
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include	"prb.h"
#include	"PrFAKE.h"
#include	"prb_extern.h"
#include	"prb_msg.h"
	END USRLIB MODULE INFORMATION
*/
int PrNeedleClean_FAKE(int clean_function);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrProberStatus_FAKE
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 2
	ARGUMENTS:
		c_machine,	char *,	Output,	
		c_cassette,	char *,	Output,	
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include    "prb.h"
#include "PrFAKE.h"
#include    "prb_func_id.h"
#include    "prb_extern.h"
#include    "prb_drvadr.h"
	END USRLIB MODULE INFORMATION
*/
int PrProberStatus_FAKE(char * c_machine, char * c_cassette);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrProfile_FAKE
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
#include	"prb.h"
#include	"PrFAKE.h"
#include	"prb_msg.h"
	END USRLIB MODULE INFORMATION
*/
int PrProfile_FAKE();


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrPutNxtSlot_FAKE
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
#include	"prb.h"
#include	"PrFAKE.h"
#include	"prb_msg.h"
	END USRLIB MODULE INFORMATION
*/
int PrPutNxtSlot_FAKE(int cassette_number, int reason_code);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrPutWafer_FAKE
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
#include	"prb.h"
#include	"PrFAKE.h"
#include	"prb_msg.h"
	END USRLIB MODULE INFORMATION
*/
int PrPutWafer_FAKE(int cassette_number, int slot_number, int reason_code);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrQueryChuckTemp_FAKE
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		chuck_temp,	double *,	Output,	,	,
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include    "prb.h"
#include "PrFAKE.h"
#include    "prb_func_id.h"
#include    "prb_extern.h"
#include    "prb_drvadr.h"
	END USRLIB MODULE INFORMATION
*/
int PrQueryChuckTemp_FAKE(double * chuck_temp);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrReadId_FAKE
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
#include	<string.h>
#include	"prb.h"
#include	"PrFAKE.h"
#include	"prb_msg.h"
	END USRLIB MODULE INFORMATION
*/
int PrReadId_FAKE(char * user_buf);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrRelMove_FAKE
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
#include	"prb.h"
#include	"PrFAKE.h"
#include	"prb_msg.h"
	END USRLIB MODULE INFORMATION
*/
int PrRelMove_FAKE(double x_value, double y_value);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrRelReturn_FAKE
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
#include	"prb.h"
#include	"PrFAKE.h"
#include	"prb_msg.h"
	END USRLIB MODULE INFORMATION
*/
int PrRelReturn_FAKE();


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrSenseWafer_FAKE
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		i_wafer_present,	int *,	Output,	
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include    "prb.h"
#include "PrFAKE.h"
#include    "prb_func_id.h"
#include    "prb_extern.h"
#include    "prb_drvadr.h"
	END USRLIB MODULE INFORMATION
*/
int PrSenseWafer_FAKE(int * i_wafer_present);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrSerialPoll_FAKE
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
#include	"prb.h"
#include	"PrFAKE.h"
#include	"prb_msg.h"
	END USRLIB MODULE INFORMATION
*/
int PrSerialPoll_FAKE();


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrSetChuckTemp_FAKE
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		chuck_temp,	double,	Input,	,	,
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include    "prb.h"
#include "PrFAKE.h"
#include    "prb_func_id.h"
#include    "prb_extern.h"
#include    "prb_drvadr.h"
	END USRLIB MODULE INFORMATION
*/
int PrSetChuckTemp_FAKE(double chuck_temp);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrSetDiam_FAKE
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		diameter,	int,	Input,	
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include	"prb.h"
#include	"PrFAKE.h"
#include	"prb_msg.h"
#include	"prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrSetDiam_FAKE(int diameter);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrSetDieSize_FAKE
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 2
	ARGUMENTS:
		x_die_size,	double,	Input,	
		y_die_size,	double,	Input,	
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include    "prb.h"
#include	"PrFAKE.h"
#include    "prb_msg.h"
	END USRLIB MODULE INFORMATION
*/
int PrSetDieSize_FAKE(double x_die_size, double y_die_size);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrSetFlat_FAKE
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		flat_number,	int,	Input,	
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include	"prb.h"
#include	"PrFAKE.h"
#include	"prb_msg.h"
#include "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrSetFlat_FAKE(int flat_number);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrSetMode_FAKE
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		mode,	int,	Input,	
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include	"prb.h"
#include	"PrFAKE.h"
#include	"prb_extern.h"
#include	"prb_msg.h"
	END USRLIB MODULE INFORMATION
*/
int PrSetMode_FAKE(int mode);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrSetPipeline_FAKE
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		on_off,	int,	Input,	
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include        "prb.h"
#include	"PrFAKE.h"
#include        "prb_msg.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrSetPipeline_FAKE(int on_off);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrSetQuadrant_FAKE
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
#include	"prb.h"
#include	"PrFAKE.h"
#include	"prb_msg.h"
#include "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrSetQuadrant_FAKE(int quad_number);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrSetRefDie_FAKE
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 2
	ARGUMENTS:
		x_start_position,	int,	Input,	
		y_start_position,	int,	Input,	
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include    "prb.h"
#include	"PrFAKE.h"
#include    "prb_msg.h"
#include    "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrSetRefDie_FAKE(int x_start_position, int y_start_position);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrSetSlotStatus_FAKE
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 3
	ARGUMENTS:
		cassette,	int,	Input,	
		slot,	int,	Input,	
		status_code,	int,	Input,	
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include        "prb.h"
#include	"PrFAKE.h"
#include        "prb_msg.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrSetSlotStatus_FAKE(int cassette, int slot, int status_code);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrSetTime_FAKE
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
#include	"prb.h"
#include	"PrFAKE.h"
#include	"prb_msg.h"
#include "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrSetTime_FAKE(int new_time);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrSetUnits_FAKE
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		units,	int,	Input,	
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include    "prb.h"
#include	"PrFAKE.h"
#include    "prb_msg.h"
#include    "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrSetUnits_FAKE(int units);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrSmifClamp_FAKE
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 2
	ARGUMENTS:
		i_pod_number,	int,	Input,	,	,	
		i_clamp_state,	int,	Input,	,	,	
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include    "prb.h"
#include "PrFAKE.h"
#include    "prb_func_id.h"
#include    "prb_extern.h"
#include    "prb_drvadr.h"
	END USRLIB MODULE INFORMATION
*/
int PrSmifClamp_FAKE(int i_pod_number, int i_clamp_state);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrSmifLock_FAKE
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
#include    "prb.h"
#include "PrFAKE.h"
#include    "prb_func_id.h"
#include    "prb_extern.h"
#include    "prb_drvadr.h"
	END USRLIB MODULE INFORMATION
*/
int PrSmifLock_FAKE(int i_pod_number, int i_lock_state);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrSmifLockStatus_FAKE
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		i_lock_status,	int *,	Output,	
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include    "prb.h"
#include "PrFAKE.h"
#include    "prb_func_id.h"
#include    "prb_extern.h"
#include    "prb_drvadr.h"
	END USRLIB MODULE INFORMATION
*/
int PrSmifLockStatus_FAKE(int * i_lock_status);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrSmifStatus_FAKE
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
#include "PrFAKE.h"
#include    "prb_func_id.h"
#include    "prb_extern.h"
#include    "prb_drvadr.h"
#include    "prb_msg.h"
extern char* getenv(const char*);
extern int atoi();
	END USRLIB MODULE INFORMATION
*/
int PrSmifStatus_FAKE(int i_pod_number, int *i_status_array, int i_status_array_size);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrSSMovNxt_FAKE
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
#include    "prb.h"
#include    "prb_func_id.h"
#include    "prb_extern.h"
#include    "prb_drvadr.h"
#include "PrFAKE.h"
	END USRLIB MODULE INFORMATION
*/
int PrSSMovNxt_FAKE(int ink_number);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrStart_FAKE
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
#include "PrFAKE.h"
#include    "prb_func_id.h"
#include    "prb_extern.h"
#include    "prb_drvadr.h"
	END USRLIB MODULE INFORMATION
*/
int PrStart_FAKE();


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrStatus_FAKE
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
#include	"prb.h"
#include	"PrFAKE.h"
#include	"prb_extern.h"
#include	"prb_msg.h"
	END USRLIB MODULE INFORMATION
*/
int PrStatus_FAKE(int * ready, int * x_location, int * y_location, int * chuck, int * mode);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrStop_FAKE
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
#include "PrFAKE.h"
#include    "prb_func_id.h"
#include    "prb_extern.h"
#include    "prb_drvadr.h"
	END USRLIB MODULE INFORMATION
*/
int PrStop_FAKE();


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrUnLoad_FAKE
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
#include	"prb.h"
#include	"PrFAKE.h"
#include	"prb_msg.h"
	END USRLIB MODULE INFORMATION
*/
int PrUnLoad_FAKE();


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrWriteRead_FAKE
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 6
	ARGUMENTS:
		input_buf,	char *,	Input,	
		input_buf_len,	int,	Input,	
		output_buf,	char *,	Output,	
		output_buf_len,	int,	Input,	
		terminator,	int,	Input,	
		terminator_cnt,	int,	Input,	
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <string.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include	"prb.h"
#include	"PrFAKE.h"
#include	"prb_msg.h"
	END USRLIB MODULE INFORMATION
*/
int PrWriteRead_FAKE(char * input_buf, int input_buf_len, char * output_buf, int output_buf_len, int terminator, int terminator_cnt);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrWriteReadSRQ_FAKE
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 6
	ARGUMENTS:
		in_buf,	char *,	Input,	
		inbuf_len,	int,	Input,	
		out_buf,	char *,	Input,	
		outbuf_len,	int,	Input,	
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
#include	"PrFAKE.h"
#include    "prb_msg.h"
	END USRLIB MODULE INFORMATION
*/
int PrWriteReadSRQ_FAKE(char * in_buf, int inbuf_len, char * out_buf, int outbuf_len, int timeout, int * i_srq);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrZParams_FAKE
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 2
	ARGUMENTS:
		function,	int,	Input,	
		value,	int,	Input,	
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include	"prb.h"
#include	"PrFAKE.h"
#include	"prb_msg.h"
	END USRLIB MODULE INFORMATION
*/
int PrZParams_FAKE(int function, int value);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrZTravel_FAKE
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		number,	int,	Input,	
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include	"prb.h"
#include	"PrFAKE.h"
#include	"prb_msg.h"
#include	"prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrZTravel_FAKE(int number);


