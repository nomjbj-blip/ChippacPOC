/* USRLIB MODULE INFORMATION

	MODULE NAME: All_pin_gnd
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

void All_pin_gnd(  )
{
/* USRLIB MODULE CODE */
printf("All Pin Gnd Connection!!!!\n");

PrChuck(1);
conpin(GND,11,27,10,26,61,40,62,39,63,38,64,37,1,36,2,35,3,34,4,33,5,32,6,31,7,30,8,29,9,28,KI_EOC);
rdelay(5);
PrChuck(0);
devint();
/* USRLIB MODULE END  */
} 		/* End All_pin_gnd.c */

