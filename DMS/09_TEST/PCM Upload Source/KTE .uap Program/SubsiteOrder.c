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
/* USRLIB MODULE HELP DESCRIPTION

	END USRLIB MODULE HELP DESCRIPTION */
/* USRLIB MODULE PARAMETER LIST */
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "ktxe_defs.h"
#include "kdf.h"
#include "ktxe_types.h"
#include "COM_usrlib.h"

void SubsiteOrder(  )
{
/* USRLIB MODULE CODE */

dpAddData ("ktxe_sort_subsite_ktms", INT, 0);

return;
/* USRLIB MODULE END  */
} 		/* End SubsiteOrder.c */

