/* USRLIB MODULE INFORMATION

	MODULE NAME: RptWaferId_DAC
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include "ktxe_types.h"
#include "kui_proto.h"
#include <string.h>
#include <time.h>
#include "COM_usrlib.h"
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <unistd.h>
#include <netdb.h>
	END USRLIB MODULE INFORMATION
*/
/* USRLIB MODULE HELP DESCRIPTION
Write Wafer id to txt file

use the module at UAP_WAFER_BEGIN
	END USRLIB MODULE HELP DESCRIPTION */
/* USRLIB MODULE PARAMETER LIST */
#include <stdio.h>
#include "ktxe_types.h"
#include "kui_proto.h"
#include <string.h>
#include <time.h>
#include "COM_usrlib.h"
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <unistd.h>
#include <netdb.h>

int RptEquipStat(char *status, char *value, char *mode);

void RptWaferId_DAC()
{
    /* USRLIB MODULE CODE */
    FILE *txt_fp;
    LOT *lot;
    WAFER *wafer;
    char err_msg[1024], logfile[100], tst_name[100], buf[100], srcFileName[50];
    char *filename = NULL;
    time_t logtime;
    int stat;
    int idx;
    struct tm *timestruct;

    lot = (LOT *)dpGetPointer("lot", LONG_P);
    wafer = (WAFER *)dpGetPointer("wafer", LONG_P);
    
    time(&logtime);
    timestruct = localtime(&logtime);

    RptEquipStat("WAFER_START", wafer->id, "WAFER");

    /* Add 2019.08.23 dacrux tmp file create */
    sprintf(srcFileName, "%s_%s_%.2d%.2d.txt", lot->id, wafer->id, timestruct->tm_min, timestruct->tm_sec);
    sprintf(logfile, "/opt/kiS600/ap_data/EQUIP/TEST/PCM_TMP/%s", srcFileName);
    if ((txt_fp = fopen(logfile, "a+")) == NULL)
    {
        sprintf(err_msg, "DAC_DATA.RptWaferID_DAC_TWInitWafer: Unable to open logfile %s\n", logfile);
        KTXEErrorMsg("%s", err_msg);
        OkMsgDlg(err_msg);
        dpAddData("UAP_abort_level", INT, KI_ABORT);
        return;
    }

    /*--*/

    if (strstr(lot->testname, "ktxe") != NULL)
    {
        strcpy(buf, lot->testname);
        strcpy(tst_name, strtok(buf, " \n"));
        strcpy(tst_name, strtok(NULL, " \n"));
    }
    else
    {
        strcpy(tst_name, lot->testname);
    }

    fprintf(txt_fp, "Logging Raw Data from Test  <%s>  Executing on: %s\n\n", tst_name, lot->starttime);
    fprintf(txt_fp, "-------------------------------------\n");
    fprintf(txt_fp, "Operator Name is : %s\n", lot->operator);
    fprintf(txt_fp, "-------------------------------------\n");
    fprintf(txt_fp, "Current Wafer = %s\n\n\n", wafer->id);

    filename = (char *)malloc(256 * sizeof(char));
    strcpy(filename, srcFileName);

    /* save file pointer */
    dpAddPointer("Anam_txt_fp_dac", LONG_P, txt_fp);
    dpAddPointer("Anam_txt_fpname_dac", CHAR_P, filename);
    
    return;
    /* USRLIB MODULE END  */
}/* End RptWaferId_DAC.c */

