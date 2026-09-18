/* USRLIB MODULE INFORMATION

	MODULE NAME: RptWaferId_dms_tmp
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
#include <string.h>
#include <time.h>
#include "COM_usrlib.h"
#include "ktxe_types.h"
#include "kui_proto.h"

void RptWaferId(  )
{
/* USRLIB MODULE CODE */

    FILE *txt_fp, *txt_fp1, *status_fp;
    LOT *lot;
    WAFER *wafer;
    char err_msg[1024], logfile[100], tst_name[100], buf[100];
    char statusfilename[100];
    time_t logtime;
    int stat;
    struct tm *timestruct;

    FILE *tester;
    char tdata[20];
    char tester_name[20];
	
    system("uname -n > /tmp/.tester_name");
    tester=fopen("/tmp/.tester_name","r");
    fgets(tdata,100,tester);
    sscanf(tdata,"%s",tester_name);

    lot = (LOT *)dpGetPointer("lot", LONG_P);
    wafer = (WAFER *)dpGetPointer("wafer", LONG_P);
    
    time(&logtime);
    timestruct = localtime(&logtime);

    /*--*/

    sprintf(statusfilename, "/opt/kiS600/ap_data/EQUIP/TEST/PCM/STATUS/%s", tester_name);
    if ((status_fp = fopen(statusfilename, "w")) == NULL)
    {
        sprintf(err_msg, "Equipment Status_TWInitLot: Unable to open status file %s\n", statusfilename);
        KTXEErrorMsg("%s", err_msg);
        OkMsgDlg(err_msg);
        dpAddData("UAP_abort_level", INT, KI_ABORT);
        return;
    }
    fprintf(status_fp, "%s", wafer->id);
    fflush(status_fp);
    fclose(status_fp);

    /*--*/

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

    printf("tester_name: %s\n", tester_name);
    /* Add 2019.08.23 dacrux tmp file create */
    sprintf(logfile, "/opt/kiS600/ap_data/EQUIP/TEST/PCM/%s/%s_%s_%.2d%.2d.txt", tester_name, lot->id, wafer->id, timestruct->tm_min, timestruct->tm_sec);
    if ((txt_fp1 = fopen(logfile, "a+")) == NULL)
    {
        sprintf(err_msg, "Anam_Data_DAcrux_TWInitLot: Unable to open logfile %s\n", logfile);
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

    fprintf(txt_fp1, "Logging Raw Data from Test  <%s>  Executing on: %s\n\n", tst_name, lot->starttime);
    fprintf(txt_fp1, "-------------------------------------\n");
    fprintf(txt_fp1, "Operator Name is : %s\n", lot->operator);
    fprintf(txt_fp1, "-------------------------------------\n");
    fprintf(txt_fp1, "Current Wafer = %s\n\n\n", wafer->id);

    /* save file pointer */
    dpAddPointer("Anam_txt_fp_dacrux", LONG_P, txt_fp1);

    /*--*/

    return;
/* USRLIB MODULE END  */
} 		/* End RptWaferId_dms_tmp.c */

