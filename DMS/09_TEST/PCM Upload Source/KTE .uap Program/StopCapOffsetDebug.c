/* USRLIB MODULE INFORMATION

	MODULE NAME: StopCapOffsetDebug
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <COM_usrlib.h>
	END USRLIB MODULE INFORMATION
*/
/* USRLIB MODULE HELP DESCRIPTION

	END USRLIB MODULE HELP DESCRIPTION */
/* USRLIB MODULE PARAMETER LIST */
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <COM_usrlib.h>

void StopCapOffsetDebug(  )
{
/* USRLIB MODULE CODE */
FILE *fp;

/* Call at UAP_LOT_END */
/* This module closes the capoffset debug file */

if ((fp = (FILE *)dpGetPointer("debug_capoffset_file", LONG_P)) != NULL) {
   fprintf(fp, "\nStopCapOffsetDebug-> Closing capoffset debug file.");
   fclose (fp);
}

return;
/* USRLIB MODULE END  */
} 		/* End StopCapOffsetDebug.c */

