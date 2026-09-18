/* USRLIB MODULE INFORMATION

	MODULE NAME: RptHead
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <time.h>
#include "ktxe_types.h"
#include "kui_proto.h"
#include "COM_usrlib.h"
#include <string.h>
#include "ktxe_proto.h"
	END USRLIB MODULE INFORMATION
*/
/* USRLIB MODULE HELP DESCRIPTION
Write lot header to txt file

use this module at UAP_WRITE_LOT_INFO
	END USRLIB MODULE HELP DESCRIPTION */
/* USRLIB MODULE PARAMETER LIST */
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <time.h>
#include "ktxe_types.h"
#include "kui_proto.h"
#include "COM_usrlib.h"
#include <string.h>
#include "ktxe_proto.h"

int RptEquipStat(char *status, char *lotid, char *waferid, char *program);

void RptHead_DAC()
{
    /* USRLIB MODULE CODE */
    /* LOT *lot = (LOT *)dpGetPointer("lot", LONG_P); */
    /* RptEquipStat("LOT_START", lot->id, "LOT"); */
    /* USRLIB MODULE END  */
} /* End RptHead.c */
