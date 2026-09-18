/* sample_ti function prototype and KITT header file */

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

	MODULE NAME: TWAbortExit
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <string.h>
#include "COM_usrlib.h"
	END USRLIB MODULE INFORMATION
*/
void TWAbortExit();


/* USRLIB MODULE INFORMATION

	MODULE NAME: TWDataDistributor
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <string.h>
#include "ktxe_types.h"
#include "kui_proto.h"
#include "COM_usrlib.h"
#define PARM_STRLEN 4096
#define PARM_NAME_LEN 30
	END USRLIB MODULE INFORMATION
*/
void TWDataDistributor();


/* USRLIB MODULE INFORMATION

	MODULE NAME: TWDefineWaferId
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <string.h>
#include <stdlib.h>
#include <unistd.h>
#include "kui_proto.h"
#include "ktxe_types.h"
#include "COM_usrlib.h"
#define TI_MAX_LOT_ID_LENGTH 8
	END USRLIB MODULE INFORMATION
*/
void TWDefineWaferId();


/* USRLIB MODULE INFORMATION

	MODULE NAME: TWEndLot
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <string.h>
#include "COM_usrlib.h"
	END USRLIB MODULE INFORMATION
*/
void TWEndLot();


/* USRLIB MODULE INFORMATION

	MODULE NAME: TWEndWafer
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <string.h>
#include "COM_usrlib.h"
#include "kui_proto.h"
#define OK 	0   
#define READPIPE_ERR         -1   
#define NAK_ERR -2  
#define TIMEOUT_ERR -3  
#define UNKNOWN_REPLY_ERR -4  
#define TWRESTART_ERR -5
	END USRLIB MODULE INFORMATION
*/
void TWEndWafer();


/* USRLIB MODULE INFORMATION

	MODULE NAME: TWGetAck
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		msg_str,	char *,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <string.h>
#include <stdlib.h>
#include <unistd.h>
#include <fcntl.h>
#include <signal.h>
#include "ktxe_types.h"
#include "kui_proto.h"
#include "COM_usrlib.h"
#define ACK 0x6
#define NAK 0x15
#define NUL 0x0
#define SOH 0x1
#define OK 	0   
#define READPIPE_ERR         -1   
#define NAK_ERR -2  
#define TIMEOUT_ERR -3  
#define UNKNOWN_REPLY_ERR -4  
#define TWRESTART_ERR -5
	END USRLIB MODULE INFORMATION
*/
int TWGetAck(char * msg_str);


/* USRLIB MODULE INFORMATION

	MODULE NAME: TWInitLot
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <time.h>
#include "ktxe_types.h"
#include "kui_proto.h"
#include "COM_usrlib.h"
	END USRLIB MODULE INFORMATION
*/
void TWInitLot();


/* USRLIB MODULE INFORMATION

	MODULE NAME: TWOprPutResult
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 2
	ARGUMENTS:
		result_id,	char *,	Input,	,	,	
		result_value,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include "ktxe_types.h"
#include "COM_usrlib.h"
	END USRLIB MODULE INFORMATION
*/
void TWOprPutResult(char * result_id, double result_value);


/* USRLIB MODULE INFORMATION

	MODULE NAME: TWPutResult
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 5
	ARGUMENTS:
		wfd,	int,	Input,	,	,	
		rfd,	int,	Input,	,	,	
		site_id,	char *,	Input,	,	,	
		parm_string,	char *,	Input,	,	,	
		get_ack,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <string.h>
#include <ktxe_types.h>
#include <COM_usrlib.h>
#include "kui_proto.h"
#define OK 	0   
#define READPIPE_ERR         -1   
#define NAK_ERR -2  
#define TIMEOUT_ERR -3  
#define UNKNOWN_REPLY_ERR -4  
#define TWRESTART_ERR -5
	END USRLIB MODULE INFORMATION
*/
void TWPutResult(int wfd, int rfd, char * site_id, char * parm_string, int get_ack);


/* USRLIB MODULE INFORMATION

	MODULE NAME: TWPutWafer
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <string.h>
#include <stdlib.h>
#include <unistd.h>
#include <fcntl.h>
#include "ktxe_types.h"
#include "kui_proto.h"
#include "COM_usrlib.h"
#define TI_MAX_PROGRAM_ID_LENGTH 30
#define TI_MAX_LOT_ID_LENGTH 8
#define TI_TW_ACK 6
#define ACK 0x6
#define NAK 0x15
#define OK 	0   
#define READPIPE_ERR         -1   
#define NAK_ERR -2  
#define TIMEOUT_ERR -3  
#define UNKNOWN_REPLY_ERR -4  
#define TWRESTART_ERR -5
#define TI_TW_W_PIPE  "/tmp/tester_in"
#define TI_TW_R_PIPE  "/tmp/tester_out"
	END USRLIB MODULE INFORMATION
*/
void TWPutWafer();


