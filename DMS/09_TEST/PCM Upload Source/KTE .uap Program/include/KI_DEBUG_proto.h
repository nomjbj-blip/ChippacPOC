/* KI_DEBUG function prototype and KITT header file */

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

	MODULE NAME: DBG_gdfCreate
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		gdffile,	char *,	Input,	,	 ,	 
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <time.h>
#include <string.h>
	END USRLIB MODULE INFORMATION
*/
void DBG_gdfCreate(char * gdffile);


/* USRLIB MODULE INFORMATION

	MODULE NAME: DBG_prbstruct
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <math.h>
#include "prb.h"
#include "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
void DBG_prbstruct();


/* USRLIB MODULE INFORMATION

	MODULE NAME: KI_PrintDataPool
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
void KI_PrintDataPool();


