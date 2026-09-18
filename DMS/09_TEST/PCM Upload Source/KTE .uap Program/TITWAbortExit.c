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
/* USRLIB MODULE HELP DESCRIPTION
PURPOSE
	TITWAbortExit is provided to allow for completing transactions and closing pipes to the TW and AUTO data-stores.

FORMAT
	TITWAbortExit( )
USAGE
	Locate at UAP_ABORT_EXIT_HDLR.

	END USRLIB MODULE HELP DESCRIPTION */
/* USRLIB MODULE PARAMETER LIST */
#include <stdio.h>
#include <string.h>
#include "COM_usrlib.h"

void TITWAbortExit(  )
{
/* USRLIB MODULE CODE */

/* send end of wafer to data stores */
TITWEndWafer();
TIAUTOEndWafer();
/* USRLIB MODULE END  */
} 		/* End TITWAbortExit.c */

