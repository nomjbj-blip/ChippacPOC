/* USRLIB MODULE INFORMATION

	MODULE NAME: RptWaferId
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include "ktxe_types.h"
#include "kui_proto.h"
#include <string.h>
#include "COM_usrlib.h"
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

void RptWaferId()
{
    /* USRLIB MODULE CODE */

    FILE *txt_fp, *txt_fp1;
    LOT *lot;
    WAFER *wafer;
    char err_msg[1024], logfile[100], logdata[100], buf[100];
    time_t logtime;
    struct tm *timestruct;

    lot = (LOT *)dpGetPointer("lot", LONG_P);
    wafer = (WAFER *)dpGetPointer("wafer", LONG_P);
    time(&logtime);
    timestruct = localtime(&logtime);

    txt_fp = (FILE *)dpGetPointer("Anam_txt_fp", LONG_P);
    if (txt_fp != NULL)
    {
        fprintf(txt_fp, "-------------------------------------\n");
        fprintf(txt_fp, "Current Wafer = %s\n\n\n", wafer->id);
    }
    else
    {
        sprintf(err_msg, "Anam_Data_RptWaferId: Can't open file %s.txt\n", lot->id);
        KTXEErrorMsg("%s", err_msg);
        OkMsgDlg(err_msg);
        dpAddData("UAP_abort_level", INT, KI_ABORT);
        return;
    }

    /*--*/

    /* Add 2019.07.02 dacrux tmp file create */
    sprintf(logfile, "%s/EQUIP/Data/%s_%s_%.2d%.2d.txt", getenv("KIDB"), lot->id, wafer->id, timestruct->tm_min, timestruct->tm_sec);
    if ((txt_fp1 = fopen(logfile, "a+")) == NULL)
    {
        sprintf(err_msg, "Anam_Data_DAcrux_TWInitLot: Unable to open logfile %s\n", logfile);
        KTXEErrorMsg("%s", err_msg);
        OkMsgDlg(err_msg);
        dpAddData("UAP_abort_level", INT, KI_ABORT);
        return;
    }

    if (strstr(lot->testname, "ktxe") != NULL)
    {
        strcpy(buf, lot->testname);
        strcpy(logdata, strtok(buf, " \n"));
        strcpy(logdata, strtok(NULL, " \n"));
    }
    else
    {
        strcpy(logdata, lot->testname);
    }

    fprintf(txt_fp1, "Logging Raw Data from Test  <%s>  Executing on: %s\n\n", logdata, lot->starttime);
    fprintf(txt_fp1, "-------------------------------------\n");
    fprintf(txt_fp1, "Operator Name is : %s\n", lot->operator);
    fprintf(txt_fp1, "-------------------------------------\n");
    fprintf(txt_fp1, "Current Wafer = %s\n\n\n", wafer->id);

    /* save file pointer */
    dpAddPointer("Anam_txt_fp_dacrux", LONG_P, txt_fp1);

    /*--*/

    return;
    /* USRLIB MODULE END  */
} /* End RptWaferId.c */
