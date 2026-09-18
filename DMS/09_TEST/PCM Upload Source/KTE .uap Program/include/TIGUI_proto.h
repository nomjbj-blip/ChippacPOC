/* TIGUI function prototype and KITT header file */

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

	MODULE NAME: GetWaferEndTime
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <time.h>
#include <kdf.h>
#include <COM_usrlib.h>
#define debug 0
	END USRLIB MODULE INFORMATION
*/
void GetWaferEndTime();


/* USRLIB MODULE INFORMATION

	MODULE NAME: GetWaferEndTime_DMOS5
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <time.h>
#include <kdf.h>
#include <COM_usrlib.h>
#define debug 0
	END USRLIB MODULE INFORMATION
*/
void GetWaferEndTime_DMOS5();


/* USRLIB MODULE INFORMATION

	MODULE NAME: InitWaferStartTime
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <time.h>
#include <kdf.h>
#include <COM_usrlib.h>
#define debug 0
	END USRLIB MODULE INFORMATION
*/
void InitWaferStartTime();


/* USRLIB MODULE INFORMATION

	MODULE NAME: InitWaferStartTime_DMOS5
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <time.h>
#include <kdf.h>
#include <COM_usrlib.h>
#define debug 0
	END USRLIB MODULE INFORMATION
*/
void InitWaferStartTime_DMOS5();


/* USRLIB MODULE INFORMATION

	MODULE NAME: KillTestTimePS
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <time.h>
#include <ksox_def.h>
#include <kdf.h>
#include <COM_usrlib.h>
#define debug 0
	END USRLIB MODULE INFORMATION
*/
void KillTestTimePS();


/* USRLIB MODULE INFORMATION

	MODULE NAME: NoPauseExit
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <time.h>
#include <ksox_def.h>
#include <kdf.h>
#include <COM_usrlib.h>
#define debug 0
	END USRLIB MODULE INFORMATION
*/
void  NoPauseExit();

/* USRLIB MODULE INFORMATION

	MODULE NAME: quasi_Loadgdf
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <time.h>
#include <COM_usrlib.h>
	END USRLIB MODULE INFORMATION
*/
void quasi_Loadgdf();


/* USRLIB MODULE INFORMATION

	MODULE NAME: ti_disable_lotdlg
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "COM_usrlib.h"
	END USRLIB MODULE INFORMATION
*/
void ti_disable_lotdlg();


/* USRLIB MODULE INFORMATION

	MODULE NAME: ti_gui
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>


#define _REENTRANT 
#include <stdlib.h>
#include <string.h>
#include <time.h>
#include "ktxe_types.h"
#include "ktxe_proto.h"
#include "kui_proto.h"
#include "kdf.h"
                       
#include "ksox_def.h"
#include "prb.h"
#include "prb_proto.h"
#include "prb_msg.h"
#include "guidedef.h"
#include "wtype.h"
#include <thread.h>
#define NDEBUG
#include <assert.h>
	END USRLIB MODULE INFORMATION
*/
int  ti_gui();

/* USRLIB MODULE INFORMATION

	MODULE NAME: TI_loadgdf
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		gdffile,	char *,	Input,	 ,	 ,	 
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "ktxe_types.h"
#include <COM_usrlib.h>
	END USRLIB MODULE INFORMATION
*/
void TI_loadgdf(char * gdffile);


/* USRLIB MODULE INFORMATION

	MODULE NAME: TimeStamp
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		delay_time,	int,	Input,	,	,	
	INCLUDES:
#include <math.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <time.h>
#include "ktxe_types.h"
#include "COM_usrlib.h"
#include <kdf.h>
	END USRLIB MODULE INFORMATION
*/
void TimeStamp(int delay_time);


/* USRLIB MODULE INFORMATION

	MODULE NAME: TimeStampExit
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <math.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <time.h>
#include "ktxe_types.h"
#include "COM_usrlib.h"
	END USRLIB MODULE INFORMATION
*/
void TimeStampExit();


/* USRLIB MODULE INFORMATION

	MODULE NAME: unitSerialNumberGet
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 3
	ARGUMENTS:
		inst_id,	int,	Input,	,	,	
		sn_size,	int,	Input,	,	,	
		sn_string,	char *,	Output,	 ,	 ,	 
	INCLUDES:
#include <stdio.h>
#include <stdlib.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <resource.h>
	END USRLIB MODULE INFORMATION
*/
int unitSerialNumberGet(int inst_id, int sn_size, char * sn_string);


