/* prbPS21 function prototype and KITT header file */

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

	MODULE NAME: Cnfg_PS21
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		teststation,	int,	Input,	,	,
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
#include        "prb_func_id.h"
#include        "prb_extern.h"
#include        "prb_msg.h"
#include        "prbPS21_proto.h"
#include        "PrPS21.h"
#include        "prb_drvadr.h"
	END USRLIB MODULE INFORMATION
*/
int Cnfg_PS21(int teststation);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrAbsMove_PS21
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 2
	ARGUMENTS:
		x_value,	double,	Input,	,	,
		y_value,	double,	Input,	,	,
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include        <string.h>
#include        "prb.h"
#include        "prb_msg.h"
#include        "PrPS21.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrAbsMove_PS21(double x_value, double y_value);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrAutoAlign_PS21
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
#include	"PrPS21.h"
#include	"prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrAutoAlign_PS21();


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrCassetteMap_PS21
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 2
	ARGUMENTS:
		cassette_number,	int,	Input
		cassette_map,	int *,	Output
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include	     	<stdlib.h>
#include        <string.h>
#include        "prb.h"
#include        "prb_msg.h"
#include        "PrPS21.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrCassetteMap_PS21(int cassette_number, int * cassette_map);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrCheckOptions_PS21
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
#include        "PrPS21.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrCheckOptions_PS21(int * OcrPresent, int * AutoAlnPresent, int * ProfilerPresent, int * HotchuckPresent, int * HandlerPresent, int * Probe2PadPresent);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrChuck_PS21
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
#include        "PrPS21.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrChuck_PS21(int chuck_position);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrError_PS21
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
#include    "PrPS21.h"
#include    "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrError_PS21();


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrInit_PS21
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
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>                  
#include	<string.h>
#include	"prb.h"
#include	"prb_msg.h"
#include	"PrPS21.h"
#include	"prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrInit_PS21(int mode, double x_die_size, double y_die_size, int x_start_position, int y_start_position, int units, int subprobtype);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrLoad_PS21
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
#include        "PrPS21.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrLoad_PS21();


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrLoadProduct_PS21
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
#include        "PrPS21.h"
#include        "prb_extern.h"
#ifdef WIN32
#include <windows.h>		// for Sleep()
#endif
	END USRLIB MODULE INFORMATION
*/
int PrLoadProduct_PS21(char * product_file_name, char * drive);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrMove_PS21
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
#include        "PrPS21.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrMove_PS21(int x_location, int y_location, int ink_number);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrProfile_PS21
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
#include	"PrPS21.h"
#include	"prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrProfile_PS21();


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrQueryChuckTemp_PS21
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
#include        "PrPS21.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrQueryChuckTemp_PS21(double * chuck_temp);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrReadId_PS21
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		user_buf,	char *,	Output,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include        <string.h>
#include        "prb.h"
#include        "prb_msg.h"
#include        "PrPS21.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrReadId_PS21(char * user_buf);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrRelMove_PS21
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 2
	ARGUMENTS:
		x_value,	double,	Input,	,	,
		y_value,	double,	Input,	,	,
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include        <string.h>
#include        "prb.h"
#include        "prb_msg.h"
#include        "PrPS21.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrRelMove_PS21(double x_value, double y_value);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrRelReturn_PS21
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
#include    "PrPS21.h"
#include    "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrRelReturn_PS21();


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrSerialPoll_PS21
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
#include	"PrPS21.h"
#include	"prb_msg.h"
#include	"prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrSerialPoll_PS21();


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrSetChuckTemp_PS21
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
#include        "PrPS21.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrSetChuckTemp_PS21(double chuck_temp);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrSetFlat_PS21
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
#include	"PrPS21.h"
#include	"prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrSetFlat_PS21(int flat_number);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrSetMode_PS21
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
#include        "PrPS21.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrSetMode_PS21(int mode);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrSetRefDie_PS21
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 2
	ARGUMENTS:
		x_start_position,	int,	Input
		y_start_position ,	int,	Input
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>                 
#include    <string.h>
#include    "prb.h"
#include    "prb_msg.h"
#include    "PrPS21.h"
#include    "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrSetRefDie_PS21(int x_start_position, int y_start_position );


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrSetTime_PS21
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
#include	"PrPS21.h"
#include	"prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrSetTime_PS21(int new_time);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrSetUnits_PS21
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
#include    "PrPS21.h"
#include    "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrSetUnits_PS21(int units);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrStatus_PS21
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
#include        "PrPS21.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrStatus_PS21(int * ready, int * x_location, int * y_location, int * chuck, int * mode);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrUnLoad_PS21
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
#include        "PrPS21.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrUnLoad_PS21();


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrWriteRead_PS21
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
#include    "PrPS21.h"
#include    "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrWriteRead_PS21(char * in_buf, int inbuf_len, char * out_buf, int oubuf_len, int terminator, int terminator_cnt);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PrWriteReadSRQ_PS21
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
#include    "PrPS21.h"
#include    "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int PrWriteReadSRQ_PS21(char * in_buf, int inbuf_len, char * out_buf, int oubuf_len, int timeout, int * i_srq);


