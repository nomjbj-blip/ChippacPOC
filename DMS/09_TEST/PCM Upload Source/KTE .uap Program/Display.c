/* USRLIB MODULE INFORMATION

	MODULE NAME: Display
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <ktxe_proto.h>
#include <ksox_def.h>
	END USRLIB MODULE INFORMATION
*/
/* USRLIB MODULE HELP DESCRIPTION

	END USRLIB MODULE HELP DESCRIPTION */
/* USRLIB MODULE PARAMETER LIST */
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <ktxe_proto.h>
#include <ksox_def.h>

void Display(  )
{
/* USRLIB MODULE CODE */
result_list_t    *result;
char    buf[200];

if ((result = (result_list_t *)dpGetPointer("result_list", LONG_P)) == NULL)
    return;
for (; result; result = result->next) {
    if (!result->log)
        continue;
    sprintf(buf, "\t%-30s\t%g\n", result->id, result->value);
    ScrollMsgDlgMsg(buf);
}
/* USRLIB MODULE END  */
} 		/* End Display.c */

