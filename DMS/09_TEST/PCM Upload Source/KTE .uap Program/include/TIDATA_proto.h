/* TIDATA function prototype and KITT header file */

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

	MODULE NAME: TIAUTOEndWafer
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <string.h>
#include "COM_usrlib.h"
#define TI_TW_ACK 6
extern int auto_site_count;
	END USRLIB MODULE INFORMATION
*/
void TIAUTOEndWafer();


/* USRLIB MODULE INFORMATION

	MODULE NAME: TIAUTOEndWafernew
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <string.h>
#include "COM_usrlib.h"
#define TI_TW_ACK 6
	END USRLIB MODULE INFORMATION
*/
void TIAUTOEndWafernew();


/* USRLIB MODULE INFORMATION

	MODULE NAME: TIAUTOEndWaferold
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <string.h>
#include "COM_usrlib.h"
#define TI_TW_ACK 6
	END USRLIB MODULE INFORMATION
*/
void TIAUTOEndWaferold();


/* USRLIB MODULE INFORMATION

	MODULE NAME: TIAUTOPutResult
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 2
	ARGUMENTS:
		result_id,	char *,	Input,	 ,	 ,	 
		result_value,	double,	Input,	 ,	 ,	 
	INCLUDES:
#include <stdio.h>
#include <string.h>
#include <ktxe_types.h>
#include <COM_usrlib.h>
#define TI_TW_ACK 6
#define TI_AUTO_RESULT_MAX 18
extern char auto_result_buffer[2048];
extern int auto_result_count;
	END USRLIB MODULE INFORMATION
*/
void TIAUTOPutResult(char * result_id, double result_value);


/* USRLIB MODULE INFORMATION

	MODULE NAME: TIAUTOPutResultnew
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 5
	ARGUMENTS:
		wfd,	long *,	Input,	,	,	
		rfd,	long *,	Input,	,	,	
		site_id,	char *,	Input,	 ,	 ,	 
		result_id,	char *,	Input,	 ,	 ,	 
		result_value,	double,	Input,	 ,	 ,	 
	INCLUDES:
#include <stdio.h>
#include <string.h>
#include <ktxe_types.h>
#include <COM_usrlib.h>
#define TI_TW_ACK 6
	END USRLIB MODULE INFORMATION
*/
void TIAUTOPutResultnew(long * wfd, long * rfd, char * site_id, char * result_id, double result_value);


/* USRLIB MODULE INFORMATION

	MODULE NAME: TIAUTOPutResultold
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 5
	ARGUMENTS:
		wfd,	long *,	Input,	,	,	
		rfd,	long *,	Input,	,	,	
		site_id,	char *,	Input,	 ,	 ,	 
		result_id,	char *,	Input,	 ,	 ,	 
		result_value,	double,	Input,	 ,	 ,	 
	INCLUDES:
#include <stdio.h>
#include <string.h>
#include <ktxe_types.h>
#include <COM_usrlib.h>
#define TI_TW_ACK 6
	END USRLIB MODULE INFORMATION
*/
void TIAUTOPutResultold(long * wfd, long * rfd, char * site_id, char * result_id, double result_value);


/* USRLIB MODULE INFORMATION

	MODULE NAME: TIAUTOPutSite
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <string.h>
#include <ktxe_types.h>
#include <COM_usrlib.h>
#define TI_TW_ACK 6
#define TI_AUTO_SITE_MAX 9
char auto_result_buffer[2048];
int auto_result_count = 0;
int auto_site_count = 0;
char auto_site_id[SITE_ID_LENGTH];
	END USRLIB MODULE INFORMATION
*/
void TIAUTOPutSite();


/* USRLIB MODULE INFORMATION

	MODULE NAME: TIAUTOPutWafer
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <string.h>
#include <stdlib.h>
#include <unistd.h>
#include "ktxe_types.h"
#include "COM_usrlib.h"
#define TI_MAX_PROGRAM_ID_LENGTH 30
#define TI_MAX_LOT_ID_LENGTH 8
#define TI_TW_ACK 6
	END USRLIB MODULE INFORMATION
*/
void TIAUTOPutWafer();


/* USRLIB MODULE INFORMATION

	MODULE NAME: TIAUTOPutWaferold
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <string.h>
#include <stdlib.h>
#include <unistd.h>
#include "ktxe_types.h"
#include "COM_usrlib.h"
#define TI_MAX_PROGRAM_ID_LENGTH 9
#define TI_MAX_LOT_ID_LENGTH 8
#define TI_TW_ACK 6
	END USRLIB MODULE INFORMATION
*/
void TIAUTOPutWaferold();


/* USRLIB MODULE INFORMATION

	MODULE NAME: TIDataDistributor
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <string.h>
#include "ktxe_types.h"
#include "COM_usrlib.h"
	END USRLIB MODULE INFORMATION
*/
void TIDataDistributor();


/* USRLIB MODULE INFORMATION

	MODULE NAME: TIDataDistributor_backup
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <string.h>
#include "ktxe_types.h"
#include "COM_usrlib.h"
	END USRLIB MODULE INFORMATION
*/
void TIDataDistributor_backup();


/* USRLIB MODULE INFORMATION

	MODULE NAME: TIDataDistributorBK
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <string.h>
#include "ktxe_types.h"
#include "COM_usrlib.h"
	END USRLIB MODULE INFORMATION
*/
void TIDataDistributorBK();


/* USRLIB MODULE INFORMATION

	MODULE NAME: TIDefineWaferId
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
void TIDefineWaferId();


/* USRLIB MODULE INFORMATION

	MODULE NAME: TIDefineWaferID_DMOS5
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
#define TI_MAX_WAFER_ID_LENGTH 17
	END USRLIB MODULE INFORMATION
*/
void  TIDefineWaferID_DMOS5();

/* USRLIB MODULE INFORMATION

	MODULE NAME: TIDefineWaferID_DMOS5_opt
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
#define TI_MAX_WAFER_ID_LENGTH 17
	END USRLIB MODULE INFORMATION
*/
void  TIDefineWaferID_DMOS5_opt();

/* USRLIB MODULE INFORMATION

	MODULE NAME: TIDefineWaferID_DMOS5_org
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
#define TI_MAX_WAFER_ID_LENGTH 17
	END USRLIB MODULE INFORMATION
*/
void  TIDefineWaferID_DMOS5_org();

/* USRLIB MODULE INFORMATION

	MODULE NAME: TIDefineWaferID_DMOS5_sec
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
#define TI_MAX_WAFER_ID_LENGTH 17
	END USRLIB MODULE INFORMATION
*/
void  TIDefineWaferID_DMOS5_sec();

/* USRLIB MODULE INFORMATION

	MODULE NAME: TIDefineWaferID_DMOS5_sec_G
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
#define TI_MAX_WAFER_ID_LENGTH 17
	END USRLIB MODULE INFORMATION
*/
void  TIDefineWaferID_DMOS5_sec_G();

/* USRLIB MODULE INFORMATION

	MODULE NAME: TIOprPutResult
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 2
	ARGUMENTS:
		result_id,	char *,	Input,	 ,	 ,	 
		result_value,	double,	Input,	 ,	 ,	 
	INCLUDES:
#include <stdio.h>
#include "ktxe_types.h"
#include "COM_usrlib.h"
	END USRLIB MODULE INFORMATION
*/
void TIOprPutResult(char * result_id, double result_value);


/* USRLIB MODULE INFORMATION

	MODULE NAME: TIRAWEndLot
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <string.h>
#include "COM_usrlib.h"
	END USRLIB MODULE INFORMATION
*/
void TIRAWEndLot();


/* USRLIB MODULE INFORMATION

	MODULE NAME: TIRAWPutLot
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <time.h>
#include "ktxe_types.h"
#include "COM_usrlib.h"
	END USRLIB MODULE INFORMATION
*/
void TIRAWPutLot();


/* USRLIB MODULE INFORMATION

	MODULE NAME: TIRAWPutLot_NEW
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <time.h>
#include "ktxe_types.h"
#include "COM_usrlib.h"
	END USRLIB MODULE INFORMATION
*/
void TIRAWPutLot_NEW();


/* USRLIB MODULE INFORMATION

	MODULE NAME: TIRAWPutLotBK
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <time.h>
#include "ktxe_types.h"
#include "COM_usrlib.h"
	END USRLIB MODULE INFORMATION
*/
void TIRAWPutLotBK();


/* USRLIB MODULE INFORMATION

	MODULE NAME: TIRAWPutResult
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 4
	ARGUMENTS:
		rawfd,	long *,	Input,	,	,	
		result_id,	char *,	Input,	 ,	 ,	 
		result_value,	double,	Input,	 ,	 ,	 
		result_8id,	char *,	Input,	 ,	 ,	 
	INCLUDES:
#include <stdio.h>
#include <string.h>
#include "ktxe_types.h"
#include "COM_usrlib.h"
	END USRLIB MODULE INFORMATION
*/
void TIRAWPutResult(long * rawfd, char * result_id, double result_value, char * result_8id);


/* USRLIB MODULE INFORMATION

	MODULE NAME: TIRAWPutSite
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <time.h>
#include "ktxe_types.h"
#include "COM_usrlib.h"
	END USRLIB MODULE INFORMATION
*/
void TIRAWPutSite();


/* USRLIB MODULE INFORMATION

	MODULE NAME: TIRAWPutWafer
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <string.h>
#include "ktxe_types.h"
#include "COM_usrlib.h"
	END USRLIB MODULE INFORMATION
*/
void TIRAWPutWafer();


/* USRLIB MODULE INFORMATION

	MODULE NAME: TITWAbortExit
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <string.h>
#include "COM_usrlib.h"
	END USRLIB MODULE INFORMATION
*/
void TITWAbortExit();


/* USRLIB MODULE INFORMATION

	MODULE NAME: TITWEndWafer
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <string.h>
#include "COM_usrlib.h"
#define TI_TW_ACK 6
	END USRLIB MODULE INFORMATION
*/
void TITWEndWafer();


/* USRLIB MODULE INFORMATION

	MODULE NAME: TITWPutResult
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 5
	ARGUMENTS:
		wfd,	long *,	Input,	,	,	
		rfd,	long *,	Input,	,	,	
		site_id,	char *,	Input,	,	,	
		result_id,	char *,	Input,	,	,	
		result_value,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <string.h>
#include <ktxe_types.h>
#include <COM_usrlib.h>
#define TI_TW_ACK 6
	END USRLIB MODULE INFORMATION
*/
void TITWPutResult(long * wfd, long * rfd, char * site_id, char * result_id, double result_value);


/* USRLIB MODULE INFORMATION

	MODULE NAME: TITWPutResult_bak
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 5
	ARGUMENTS:
		wfd,	long *,	Input,	,	,	
		rfd,	long *,	Input,	,	,	
		site_id,	char *,	Input,	 ,	 ,	 
		result_id,	char *,	Input,	 ,	 ,	 
		result_value,	double,	Input,	 ,	 ,	 
	INCLUDES:
#include <stdio.h>
#include <string.h>
#include <ktxe_types.h>
#include <COM_usrlib.h>
#define TI_TW_ACK 6
	END USRLIB MODULE INFORMATION
*/
void TITWPutResult_bak(long * wfd, long * rfd, char * site_id, char * result_id, double result_value);


/* USRLIB MODULE INFORMATION

	MODULE NAME: TITWPutResult_old
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 5
	ARGUMENTS:
		wfd,	long *,	Input,	,	,	
		rfd,	long *,	Input,	,	,	
		site_id,	char *,	Input,	 ,	 ,	 
		result_id,	char *,	Input,	 ,	 ,	 
		result_value,	double,	Input,	 ,	 ,	 
	INCLUDES:
#include <stdio.h>
#include <string.h>
#include <ktxe_types.h>
#include <COM_usrlib.h>
#define TI_TW_ACK 6
	END USRLIB MODULE INFORMATION
*/
void TITWPutResult_old(long * wfd, long * rfd, char * site_id, char * result_id, double result_value);


/* USRLIB MODULE INFORMATION

	MODULE NAME: TITWPutResulta
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 5
	ARGUMENTS:
		wfd,	long *,	Input,	,	,	
		rfd,	long *,	Input,	,	,	
		site_id,	char *,	Input,	 ,	 ,	 
		result_id,	char *,	Input,	 ,	 ,	 
		result_value,	double,	Input,	 ,	 ,	 
	INCLUDES:
#include <stdio.h>
#include <string.h>
#include <ktxe_types.h>
#include <COM_usrlib.h>
#define TI_TW_ACK 6
	END USRLIB MODULE INFORMATION
*/
void TITWPutResulta(long * wfd, long * rfd, char * site_id, char * result_id, double result_value);


/* USRLIB MODULE INFORMATION

	MODULE NAME: TITWPutResultt
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 5
	ARGUMENTS:
		wfd,	long *,	Input,	,	,	
		rfd,	long *,	Input,	,	,	
		site_id,	char *,	Input,	 ,	 ,	 
		result_id,	char *,	Input,	 ,	 ,	 
		result_value,	double,	Input,	 ,	 ,	 
	INCLUDES:
#include <stdio.h>
#include <string.h>
#include <ktxe_types.h>
#include <COM_usrlib.h>
#define TI_TW_ACK 6
	END USRLIB MODULE INFORMATION
*/
void TITWPutResultt(long * wfd, long * rfd, char * site_id, char * result_id, double result_value);


/* USRLIB MODULE INFORMATION

	MODULE NAME: TITWPutWafer
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <string.h>
#include <stdlib.h>
#include <unistd.h>
#include "ktxe_types.h"
#include "COM_usrlib.h"
#define TI_MAX_PROGRAM_ID_LENGTH 30
#define TI_MAX_LOT_ID_LENGTH 8
#define TI_TW_ACK 6
	END USRLIB MODULE INFORMATION
*/
void TITWPutWafer();


