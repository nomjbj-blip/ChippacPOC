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

int RptEquipStat(char *value, char *state);

void RptEndLot()
{
	/* USRLIB MODULE CODE */
	FILE *txt_fp;

	txt_fp = (FILE *)dpGetPointer("Anam_txt_fp", LONG_P);
	if (txt_fp != NULL)
	{
		printf("Lot_end_at_Anam_data!!!\n");
		fclose(txt_fp);
	}

	/* NULL file pointer in data pool */
	txt_fp = (FILE *)dpAddPointer("Anam_txt_fp", LONG_P, NULL);

	RptEquipStat("", "");
	/* USRLIB MODULE END  */
} /* End RptEndLot.c */
