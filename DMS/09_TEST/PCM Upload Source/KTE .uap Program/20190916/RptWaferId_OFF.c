/* USRLIB MODULE INFORMATION

	MODULE NAME: RptWaferId_OFF
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

int RptEquipStat(char *status, char *lotid, char *waferid, char *program)

void RptWaferId_OFF(  )
{
/* USRLIB MODULE CODE */

    FILE *txt_fp;
    LOT *lot;
    WAFER *wafer;
    char err_msg[1024], logfile[100], tst_name[100], buf[100];
    char statusfilename[100];
    time_t logtime;
    int stat;
    int idx;
    struct tm *timestruct;

    FILE *tester;
    char tdata[20];
    char tester_name[20];
    
    system("uname -n > /tmp/.tester_name");
    tester=fopen("/tmp/.tester_name","r");
    fgets(tdata,100,tester);
    sscanf(tdata,"%s",tester_name);

    for(idx = 0; idx < strlen(tester_name); idx++)
    {
        tester_name[idx] = toupper(tester_name[idx]);
    } 

    lot = (LOT *)dpGetPointer("lot", LONG_P);
    wafer = (WAFER *)dpGetPointer("wafer", LONG_P);

    RptEquipStat("OFFLINE", "", "", "");
    
    time(&logtime);
    timestruct = localtime(&logtime);

    printf("tester_name: %s\n", tester_name);
    /* Add 2019.08.23 dacrux tmp file create */
    sprintf(logfile, "/opt/kiS600/db/OFF_FORMAT/%s_%s_%s_%.2d%.2d.txt", tester_name, lot->id, wafer->id, timestruct->tm_min, timestruct->tm_sec);
    if ((txt_fp = fopen(logfile, "a+,ccs=UTF-8")) == NULL)
    {
        sprintf(err_msg, "DAC_DATA.RptWaferID_DAC_OFF_TWInitWafer: Unable to open logfile %s\n", logfile);
        KTXEErrorMsg("%s", err_msg);
        OkMsgDlg(err_msg);
        dpAddData("UAP_abort_level", INT, KI_ABORT);
        return;
    }

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
    fflush(txt_fp);
    
    /* save file pointer */
    dpAddPointer("Anam_txt_fp_dac", LONG_P, txt_fp);

    return;
/* USRLIB MODULE END  */
} 		/* End RptWaferId_OFF.c */

