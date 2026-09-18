/* prbgen function prototype and KITT header file */

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

	MODULE NAME: PrAbsMove
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
#include    "prb.h"
#include    "prb_func_id.h"
#include    "prb_extern.h"
#include    "prb_drvadr.h"
	END USRLIB MODULE INFORMATION
*/
int PrAbsMove(double x_value, double y_value);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrAutoAlign
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
#include    "prb_func_id.h"
#include    "prb_extern.h"
#include    "prb_drvadr.h"
	END USRLIB MODULE INFORMATION
*/
int PrAutoAlign();


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrCassetteMap
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
#include	"prb.h"
#include	"prb_func_id.h"
#include	"prb_extern.h"
#include    "prb_drvadr.h"
	END USRLIB MODULE INFORMATION
*/
int PrCassetteMap(int cassette_number, int * cassette_map);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrCassetteMask
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 2
	ARGUMENTS:
		cassette,	int,	Input,	,	,	
		slot_list,	int *,	Input,	,	,	
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
	END USRLIB MODULE INFORMATION
*/
int PrCassetteMask(int cassette, int * slot_list);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrCheckOptions
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
#include    "prb.h"
#include    "prb_func_id.h"
#include    "prb_extern.h"
#include    "prb_drvadr.h"
	END USRLIB MODULE INFORMATION
*/
int PrCheckOptions(int * OcrPresent, int * AutoAlnPresent, int * ProfilerPresent, int * HotchuckPresent, int * HandlerPresent, int * Probe2PadPresent);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrChuck
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		chuck_position,	int,	Input,	0,	0,	1
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
	END USRLIB MODULE INFORMATION
*/
int PrChuck(int chuck_position);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrClearAll
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
#include    "prb_func_id.h"
#include    "prb_extern.h"
#include    "prb_drvadr.h"
	END USRLIB MODULE INFORMATION
*/
int PrClearAll();


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrClearPipeline
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
#include    "prb_func_id.h"
#include    "prb_extern.h"
#include    "prb_drvadr.h"
	END USRLIB MODULE INFORMATION
*/
int PrClearPipeline();


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrDelay
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		s,	int,	Input,	,	,	
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <time.h>
#ifdef WIN32
#include <windows.h>
#endif
#include "prb.h"
#include "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
void PrDelay(int s);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrError
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
#include    "prb_func_id.h"
#include    "prb_extern.h"
#include    "prb_drvadr.h"
	END USRLIB MODULE INFORMATION
*/
int PrError();


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrGetNxtWafer
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		cassette_number,	int,	Input,	,	,	
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
	END USRLIB MODULE INFORMATION
*/
int PrGetNxtWafer(int cassette_number);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrGetProduct
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		file_name,	char *,	Output,	,	,	
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include	"prb.h"
#include	"prb_func_id.h"
#include	"prb_extern.h"
#include "prb_drvadr.h"
	END USRLIB MODULE INFORMATION
*/
int PrGetProduct(char * file_name);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrGetWafer
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
#include    "prb.h"
#include    "prb_func_id.h"
#include    "prb_extern.h"
#include    "prb_drvadr.h"
	END USRLIB MODULE INFORMATION
*/
int PrGetWafer(int cassette_number, int slot_number);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrInit
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 7
	ARGUMENTS:
		mode,	int,	Input,	,	1,	6
		x_die_size,	double,	Input,	,	0,	
		y_die_size,	double,	Input,	,	0,	
		x_start_position,	int,	Input,	,	,	
		y_start_position,	int,	Input,	,	,	
		units,	int,	Input,	,	0,	1
		subprobtype,	int,	Input,	0,	0,	0
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
	END USRLIB MODULE INFORMATION
*/
int PrInit(int mode, double x_die_size, double y_die_size, int x_start_position, int y_start_position, int units, int subprobtype);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrLoad
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
#include    "prb_func_id.h"
#include    "prb_extern.h"
#include    "prb_drvadr.h"
	END USRLIB MODULE INFORMATION
*/
int PrLoad();


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrLoadProduct
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 2
	ARGUMENTS:
		file_name,	char *,	Input,	,	,	
		drive_name,	char *,	Input,	,	,	
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include	"prb.h"
#include	"prb_func_id.h"
#include	"prb_extern.h"
#include    "prb_drvadr.h"
	END USRLIB MODULE INFORMATION
*/
int PrLoadProduct(char * file_name, char * drive_name);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrLowerBoat
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		cassette_number,	int,	Input,	,	,	
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
	END USRLIB MODULE INFORMATION
*/
int PrLowerBoat(int cassette_number);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrMove
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 3
	ARGUMENTS:
		x_location,	int,	Input,	,	,	
		y_location,	int,	Input,	,	,	
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
	END USRLIB MODULE INFORMATION
*/
int PrMove(int x_location, int y_location, int ink_number);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrMovNxt
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		ink_number,	int,	Input,	0,	0,	15
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
	END USRLIB MODULE INFORMATION
*/
int PrMovNxt(int ink_number);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrNeedleClean
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		clean_function,	int,	Input,	0,	0,	6
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
	END USRLIB MODULE INFORMATION
*/
int PrNeedleClean(int clean_function);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrProfile
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
#include    "prb_func_id.h"
#include    "prb_extern.h"
#include    "prb_drvadr.h"
	END USRLIB MODULE INFORMATION
*/
int PrProfile();


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrPutNxtSlot
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 2
	ARGUMENTS:
		cassette_number,	int,	Input,	,	,	
		reason_code,	int,	Input,	,	,	
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
	END USRLIB MODULE INFORMATION
*/
int PrPutNxtSlot(int cassette_number, int reason_code);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrPutWafer
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 3
	ARGUMENTS:
		cassette_number,	int,	Input,	,	,	
		slot_number,	int,	Input,	,	,	
		reason_code,	int,	Input,	,	,	
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
	END USRLIB MODULE INFORMATION
*/
int PrPutWafer(int cassette_number, int slot_number, int reason_code);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrQueryChuckTemp
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
#include    "prb_func_id.h"
#include    "prb_extern.h"
#include    "prb_drvadr.h"
	END USRLIB MODULE INFORMATION
*/
int PrQueryChuckTemp(double * chuck_temp);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrReadId
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
#include    "prb.h"
#include    "prb_func_id.h"
#include    "prb_extern.h"
#include    "prb_drvadr.h"
	END USRLIB MODULE INFORMATION
*/
int PrReadId(char * user_buf);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrRelMove
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
#include    "prb.h"
#include    "prb_func_id.h"
#include    "prb_extern.h"
#include    "prb_drvadr.h"
	END USRLIB MODULE INFORMATION
*/
int PrRelMove(double x_value, double y_value);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrRelReturn
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
#include    "prb_func_id.h"
#include    "prb_extern.h"
#include    "prb_drvadr.h"
	END USRLIB MODULE INFORMATION
*/
int PrRelReturn();


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrSerialPoll
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
#include    "prb_func_id.h"
#include    "prb_extern.h"
#include    "prb_drvadr.h"
	END USRLIB MODULE INFORMATION
*/
int PrSerialPoll();


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrSetChuckTemp
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
#include    "prb_func_id.h"
#include    "prb_extern.h"
#include    "prb_drvadr.h"
	END USRLIB MODULE INFORMATION
*/
int PrSetChuckTemp(double chuck_temp);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrSetDiam
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
#include    "prb.h"
#include    "prb_func_id.h"
#include    "prb_extern.h"
#include    "prb_drvadr.h"
	END USRLIB MODULE INFORMATION
*/
int PrSetDiam(int diameter);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrSetDieSize
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
#include    "prb.h"
#include    "prb_func_id.h"
#include    "prb_extern.h"
#include    "prb_drvadr.h"
	END USRLIB MODULE INFORMATION
*/
int PrSetDieSize(double x_die_size, double y_die_size);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrSetFlat
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
#include    "prb.h"
#include    "prb_func_id.h"
#include    "prb_extern.h"
#include    "prb_drvadr.h"
	END USRLIB MODULE INFORMATION
*/
int PrSetFlat(int flat_number);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrSetMode
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
#include    "prb.h"
#include    "prb_func_id.h"
#include    "prb_extern.h"
#include    "prb_drvadr.h"
	END USRLIB MODULE INFORMATION
*/
int PrSetMode(int mode);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrSetPipeline
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		on_off,	int,	Input,	,	,	
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
	END USRLIB MODULE INFORMATION
*/
int PrSetPipeline(int on_off);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrSetQuadrant
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		quad_number,	int,	Input,	,	,	
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
	END USRLIB MODULE INFORMATION
*/
int PrSetQuadrant(int quad_number);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrSetRefDie
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
#include    "prb.h"
#include    "prb_func_id.h"
#include    "prb_extern.h"
#include    "prb_drvadr.h"
	END USRLIB MODULE INFORMATION
*/
int PrSetRefDie(int x_start_position, int y_start_position);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrSetSlotStatus
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 3
	ARGUMENTS:
		cassette,	int,	Input,	,	,	
		slot,	int,	Input,	,	,	
		status_code,	int,	Input,	,	,	
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
	END USRLIB MODULE INFORMATION
*/
int PrSetSlotStatus(int cassette, int slot, int status_code);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrSetTime
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
#include    "prb.h"
#include    "prb_func_id.h"
#include    "prb_extern.h"
#include    "prb_drvadr.h"
	END USRLIB MODULE INFORMATION
*/
int PrSetTime(int new_time);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrSetUnits
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		units,	int,	Input,	,	,	
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
	END USRLIB MODULE INFORMATION
*/
int PrSetUnits(int units);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrSmifClamp
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
#include    "prb_func_id.h"
#include    "prb_extern.h"
#include    "prb_drvadr.h"
	END USRLIB MODULE INFORMATION
*/
int PrSmifClamp(int i_pod_number, int i_lock_state);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrSmifLock
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
#include    "prb_func_id.h"
#include    "prb_extern.h"
#include    "prb_drvadr.h"
	END USRLIB MODULE INFORMATION
*/
int PrSmifLock(int i_pod_number, int i_lock_state);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrSmifStatus
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 3
	ARGUMENTS:
		pod_number,	int,	Input,	,	,	
		status_array,	I_ARRAY_T,	Output,	,	,	
		status_array_size,	int,	Input,	,	,	
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
	END USRLIB MODULE INFORMATION
*/
int PrSmifStatus(int pod_number, int *status_array, int status_array_size);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrSSMovNxt
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		ink_number,	int,	Input,	0,	0,	15
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
	END USRLIB MODULE INFORMATION
*/
int PrSSMovNxt(int ink_number);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrStart
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
#include    "prb_func_id.h"
#include    "prb_extern.h"
#include    "prb_drvadr.h"
	END USRLIB MODULE INFORMATION
*/
int PrStart();


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrStatus
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 5
	ARGUMENTS:
		ready,	int *,	Output,	,	,	
		x_location,	int *,	Output,	,	,	
		y_location,	int *,	Output,	,	,	
		chuck_position,	int *,	Output,	,	,	
		prober_mode,	int *,	Output,	,	,	
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
	END USRLIB MODULE INFORMATION
*/
int PrStatus(int * ready, int * x_location, int * y_location, int * chuck_position, int * prober_mode);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrStop
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
#include    "prb_func_id.h"
#include    "prb_extern.h"
#include    "prb_drvadr.h"
	END USRLIB MODULE INFORMATION
*/
int PrStop();


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrUnLoad
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
#include    "prb_func_id.h"
#include    "prb_extern.h"
#include    "prb_drvadr.h"
	END USRLIB MODULE INFORMATION
*/
int PrUnLoad();


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrWriteRead
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 6
	ARGUMENTS:
		input_buf,	char *,	Input,	,	,	
		input_buf_len,	int,	Input,	,	,	
		output_buf,	char *,	Output,	,	,	
		output_buf_len,	int,	Input,	,	,	
		terminator,	int,	Input,	,	,	
		terminator_cnt,	int,	Input,	,	,	
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include	"prb.h"
#include	"prb_func_id.h"
#include	"prb_extern.h"
#include    "prb_drvadr.h"
	END USRLIB MODULE INFORMATION
*/
int PrWriteRead(char * input_buf, int input_buf_len, char * output_buf, int output_buf_len, int terminator, int terminator_cnt);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrWriteReadSRQ
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 6
	ARGUMENTS:
		input_buf,	char *,	Input,	,	,	
		input_buf_len,	int,	Input,	,	,	
		output_buf,	char *,	Output,	,	,	
		output_buf_len,	int,	Input,	,	,	
		timeout,	int,	Input,	,	,	
		i_srq,	int *,	Input,	,	,	
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include	"prb.h"
#include	"prb_func_id.h"
#include	"prb_extern.h"
#include    "prb_drvadr.h"
	END USRLIB MODULE INFORMATION
*/
int PrWriteReadSRQ(char * input_buf, int input_buf_len, char * output_buf, int output_buf_len, int timeout, int * i_srq);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrZParams
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 2
	ARGUMENTS:
		function,	int,	Input,	,	,	
		value,	int,	Input,	,	,	
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
	END USRLIB MODULE INFORMATION
*/
int PrZParams(int function, int value);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrZTravel
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		number,	int,	Input,	,	,	
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
	END USRLIB MODULE INFORMATION
*/
int PrZTravel(int number);


