/* USRLIB MODULE INFORMATION

	MODULE NAME: RptWaferId_DAC
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <stdlib.h>
#include "ktxe_types.h"
#include "kui_proto.h"
#include "COM_usrlib.h"
#include "ktxe_proto.h"
	END USRLIB MODULE INFORMATION
*/
/* USRLIB MODULE HELP DESCRIPTION
Write Wafer id to txt file

use the module at UAP_WAFER_BEGIN
	END USRLIB MODULE HELP DESCRIPTION */
/* USRLIB MODULE PARAMETER LIST */
#include <stdio.h>
#include <stdlib.h>
#include "ktxe_types.h"
#include "kui_proto.h"
#include "COM_usrlib.h"
#include "ktxe_proto.h"

int RptEquipStat(char *status, char *lotid, char *waferid, char *program)
{
    /* USRLIB MODULE CODE */
    FILE *txt_Stat = NULL;
    char statFile[50];
    int i = 0;
    FILE *tester = NULL;
    char tdata[20];
    char tester_name[20];

    system("uname -n > /tmp/.tester_name");
    tester=fopen("/tmp/.tester_name","r");
    fgets(tdata, 100, tester);
    sscanf(tdata, "%s", tester_name);
    fclose(tester);

    for(i = 0; i < strlen(tester_name); i++)
        tester_name[i] = toupper(tester_name[i]);

    
    sprintf(statFile, "/opt/kiS600/ap_data/EQUIP/TEST/PCM/STATUS/%s", tester_name);
    if ((txt_Stat = fopen(statFile, "w,ccs=UTF-8")) == NULL)
    {
        printf("DAC_DATA.Equip_Status_TWInitLot: Unable to open status file %s\n", statFile);
        return(-1);
    }

    fprintf(txt_Stat, "%s,%s,%s,%s", status, lotid, waferid, program);
    fflush(txt_Stat);
    fclose(txt_Stat);

    return (0);   
    /* USRLIB MODULE END  */
}/* End RptEquipStat_DAC.c */