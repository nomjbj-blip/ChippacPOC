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

void RptHead(  )
{
/* USRLIB MODULE CODE */
FILE * txt_fp = NULL;
LOT *lot;
cpf_info_t *cpf_info;
int *lotAdd;
char tmpprogram[MAXFILENAMELENGTH];      /* */
char tmppath[MAXFILENAMELENGTH];         /* scratch for path */
char tmpext[MAXFILENAMELENGTH];          /* scratch for file extension */

char        err_msg[1024];
char        logfile[100], tst_name[100], buf[100];    
int        tw_online;

time_t     logtime;
struct tm  *timestruct;

lot = (LOT *)dpGetPointer("lot", LONG_P);


time(&logtime);
timestruct = localtime(&logtime);    


/* opening a txt file*/
if ( getenv("KIDB") == NULL )
{
    sprintf(err_msg,"Anam_Data_RptHead: $KIDB environment variable not defined\n");
         KTXEErrorMsg("%s", err_msg );
    OkMsgDlg(err_msg);
    dpAddData("UAP_abort_level", INT, KI_ABORT);
    return;
}
else
{
    sprintf(logfile, "%s/FORMAT/%s_%.2d%.2d.txt", getenv("KIDB"), lot->id, timestruct->tm_min, timestruct->tm_sec);
    if ( (txt_fp = fopen(logfile, "a+")) == NULL ) {
        sprintf(err_msg, "Anam_Data_TWInitLot: Unable to open logfile %s\n", logfile);
             KTXEErrorMsg("%s", err_msg );
        OkMsgDlg(err_msg);
        dpAddData("UAP_abort_level", INT, KI_ABORT);
        return;
    }
}
if (strstr(lot->testname, "ktxe") != NULL)
{
    strcpy(buf, lot->testname);
    strcpy(tst_name, strtok(buf, " \n"));
    strcpy(tst_name, strtok(NULL, " \n"));
}
else
    strcpy(tst_name, lot->testname);
fprintf(txt_fp, "Logging Raw Data from Test  <%s>  Executing on: %s\n\n", tst_name, lot->starttime);
fprintf(txt_fp, "-------------------------------------\n");
fprintf(txt_fp, "Operator Name is : %s\n", lot->operator);

/* save file pointer */
dpAddPointer( "Anam_txt_fp", LONG_P, txt_fp);

/* USRLIB MODULE END  */
} 		/* End RptHead.c */

