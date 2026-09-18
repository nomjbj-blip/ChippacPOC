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
/* USRLIB MODULE HELP DESCRIPTION
Enters the start time of the wafer execution in the datapool.  The UAP is UAP_ENGINE_EXIT.
	END USRLIB MODULE HELP DESCRIPTION */
/* USRLIB MODULE PARAMETER LIST */
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <time.h>
#include <ksox_def.h>
#include <kdf.h>
#include <COM_usrlib.h>
#define debug 0

void NoPauseExit(  )
{
/* USRLIB MODULE CODE */
printf("Engine_exit!!!\n");
dpAddData("ktxe_disable_kui", INT, TRUE);
return;
/* USRLIB MODULE END  */
} 		/* End NoPauseExit.c */

