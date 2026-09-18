/* ti_common function prototype and KITT header file */

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

	MODULE NAME: AskWhereIdsComeFrom
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <guidedef.h>
#include <kui_proto.h>
#include <COM_usrlib.h>
	END USRLIB MODULE INFORMATION
*/
int AskWhereIdsComeFrom();


/* USRLIB MODULE INFORMATION

	MODULE NAME: changewpf
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 2
	ARGUMENTS:
		slotid,	char *,	Input,	,	,	
		wpfname,	char *,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <ktxe_proto.h>
#include <ksox_def.h>
	END USRLIB MODULE INFORMATION
*/
void  changewpf(char *, char *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: GetWaferIdsFromOperator
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "kdf.h"
#include "ktxe_types.h"
#include "kui_proto.h"
#include "COM_usrlib.h"
	END USRLIB MODULE INFORMATION
*/
void GetWaferIdsFromOperator();


/* USRLIB MODULE INFORMATION

	MODULE NAME: SubsiteOrder
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "ktxe_defs.h"
#include "kdf.h"
#include "ktxe_types.h"
#include "COM_usrlib.h"
	END USRLIB MODULE INFORMATION
*/
void SubsiteOrder();


/* USRLIB MODULE INFORMATION

	MODULE NAME: ti_data_process_com
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#include "kdf.h"
#include <ktxe_types.h>
#include "COM_usrlib.h"
#include <sys/types.h>
#include <sys/stat.h>
#include <sys/errno.h>
#include <unistd.h>
#include <macros.h>
	END USRLIB MODULE INFORMATION
*/
void ti_data_process_com();


/* USRLIB MODULE INFORMATION

	MODULE NAME: ti_srq
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void ti_srq();


/* USRLIB MODULE INFORMATION

	MODULE NAME: ti_tsk_device_load
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "ktxe_types.h"
#include "COM_usrlib.h"
#include "kui_proto.h"
#include "kdf.h"
	END USRLIB MODULE INFORMATION
*/
int ti_tsk_device_load();


/* USRLIB MODULE INFORMATION

	MODULE NAME: ti_tsk_log_ocr
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <time.h>
#include <stdlib.h>
#include "ktxe_types.h"
#include "COM_usrlib.h"

#include "kui_proto.h"
	END USRLIB MODULE INFORMATION
*/
int ti_tsk_log_ocr();


/* USRLIB MODULE INFORMATION

	MODULE NAME: ti_wafer_ink_query
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <kui_proto.h>
#include <COM_usrlib.h>
#include <ktxe_proto.h>
#include <kdf.h>
#include <time.h>
	END USRLIB MODULE INFORMATION
*/
int ti_wafer_ink_query();


