/* USRLIB MODULE INFORMATION

	MODULE NAME: wafer_chuck_gnd
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
#include <stdio.h>
#include <string.h>
#include <unistd.h>
#include <netdb.h>
#include <ktxe_proto.h>
#include <ksox_def.h>
#include <time.h>


#define debug 0
	END USRLIB MODULE INFORMATION
*/
/* USRLIB MODULE HELP DESCRIPTION
The UAP is UAP_WAFER_BEGIN.

	END USRLIB MODULE HELP DESCRIPTION */
/* USRLIB MODULE PARAMETER LIST */
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <time.h>
#include <kdf.h>
#include <COM_usrlib.h>
#include <stdio.h>
#include <string.h>
#include <unistd.h>
#include <netdb.h>
#include <ktxe_proto.h>
#include <ksox_def.h>
#include <time.h>


#define debug 0

void wafer_chuck_gnd(  )
{
/* USRLIB MODULE CODE */
printf("CHUCK GND Connection!!!!\n");
conpin(GND,CHUCK,KI_EOC);
rdelay(1);
devint();
/* USRLIB MODULE END  */
} 		/* End wafer_chuck_gnd.c */

