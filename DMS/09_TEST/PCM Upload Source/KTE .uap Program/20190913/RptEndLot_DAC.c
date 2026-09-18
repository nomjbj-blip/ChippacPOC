/* USRLIB MODULE INFORMATION

	MODULE NAME: RptEndLot
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
Close txt file

use this module at UAP_LOT_END
	END USRLIB MODULE HELP DESCRIPTION */
/* USRLIB MODULE PARAMETER LIST */
#include <stdio.h>
#include <string.h>
#include "COM_usrlib.h"

int RptEquipStat(char *state, char *value, char * mode);

void RptEndLot()
{
	/* USRLIB MODULE CODE */
	RptEquipStat("LOT_END", "", "LOT");
	/* USRLIB MODULE END  */
} /* End RptEndLot.c */
