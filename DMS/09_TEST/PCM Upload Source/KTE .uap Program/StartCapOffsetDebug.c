/* USRLIB MODULE INFORMATION

	MODULE NAME: StartCapOffsetDebug
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

void StartCapOffsetDebug(  )
{
/* USRLIB MODULE CODE */
FILE *fp;
char filename[128];

/* Call at UAP_PROG_ARGS */

/* If the environment variable KI_CAPOFFSET_DEBUG is set, open the debug
   file "debug_capoffset.log" and put the pointer to this file in the data pool. */

if (getenv ("KI_CAPOFFSET_DEBUG") != NULL) {
   if (getenv ("KILOG") != NULL) {
      strcpy (filename, (char *)getenv ("KILOG"));
   } else {
      strcpy (filename, "/tmp/");
   }
   strcat (filename, "/debug_capoffset.log");
   if ((fp = fopen(filename, "a")) != NULL) {
      fprintf(fp, "\nStartCapOffsetDebug-> Debug file opened.");
      dpAddPointer("debug_capoffset_file", LONG_P, fp);
   }
}

return;
/* USRLIB MODULE END  */
} 		/* End StartCapOffsetDebug.c */

