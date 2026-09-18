/* USRLIB MODULE INFORMATION

	MODULE NAME: wafer_end_pgm_DAC
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <time.h>
#include <kdf.h>
#include <COM_usrlib.h>
#include <stdlib.h>
#include <string.h>
#include <unistd.h>
#include <netdb.h>
#include <ktxe_proto.h>
#include <ksox_def.h>
#include <time.h>
#include "ktxe_types.h"
#include "kui_proto.h"
#include "COM_usrlib.h"
#include "ktxe_proto.h"

	END USRLIB MODULE INFORMATION
*/
/* USRLIB MODULE HELP DESCRIPTION
The UAP is UAP_WAFER_END.

	END USRLIB MODULE HELP DESCRIPTION */
/* USRLIB MODULE PARAMETER LIST */
#include  <stdio.h>
#include  <lptdef.h>
#include  <lptdef_lowercase.h>
#include  <math.h>
#include  <time.h>
#include  <kdf.h>
#include  <COM_usrlib.h>
#include  <stdlib.h>
#include  <string.h>
#include  <unistd.h>
#include  <netdb.h>
#include  <ktxe_proto.h>
#include  <ksox_def.h>
#include  "ktxe_types.h"
#include  "kui_proto.h"
#include  "COM_usrlib.h"
#include  "ktxe_proto.h"

int RptEquipStat(char *status, char *lotid, char *waferid, char *program);

void wafer_end_pgm_DAC()
{
    /* USRLIB MODULE CODE */
    FILE *src_fp = NULL;
    FILE *tar_fp = NULL;
    FILE *tester = NULL;

    int i, pcd_len;

    char hostname[MAXHOSTNAMELEN];
    char srcPath[100], tarPath[100], buffer[255];
    char srcFileName[50];
    char *filename, *pStr;
    char tdata[20], tester_name[20], tstPgm[100], tst_name[100];
    char err_msg[1024];
    time_t clock;
    struct tm *tm_p;
    static char_date[80];

    LOT *lot = (LOT *)dpGetPointer("lot", LONG_P);
    WAFER *wafer_pp = (WAFER *)dpGetPointer("wafer", LONG_P);

    clock = time((time_t *)0);
    tm_p = localtime(&clock);
    strftime((char *)char_date, sizeof(char_date), "%Y/%m/%d %T", tm_p);

    system("uname -n > /tmp/.tester_name");
    tester = fopen("/tmp/.tester_name", "r");
    fgets(tdata, 100, tester);
    sscanf(tdata, "%s", tester_name);
    fclose(tester);

    for (i = 0; i < strlen(tester_name); i++)
        tester_name[i] = toupper(tester_name[i]);

    if (gethostname(hostname, MAXHOSTNAMELEN) == -1)
        strcpy(hostname, lot->system);

    for (i = 0; i < strlen(hostname); i++)
        hostname[i] = toupper(hostname[i]);

    if (wafer_pp->id != NULL)
        printf("%s|%s|%s|E\n", char_date, hostname, wafer_pp->id);

    filename = (char *)dpGetPointer("Anam_txt_fpname_dac", CHAR_P);
    strcpy(srcFileName, filename);
    src_fp = (FILE *)dpGetPointer("Anam_txt_fp_dac", LONG_P);
    if (src_fp != NULL)
    {
        printf("Wafer end at DAC_DATA!! \n");
        fclose(src_fp);
    }

    sprintf(srcPath, "/opt/kiS600/ap_data/EQUIP/TEST/PCM_TMP/%s", srcFileName);
    if ((src_fp = fopen(srcPath, "r,ccs=UTF-8")) == NULL)
    {
        sprintf(err_msg, "DAC_DATA.Wafer_end_pgm_DAC: Can't open this file %s\n", srcPath);
        KTXEErrorMsg("%s", err_msg);
        OkMsgDlg(err_msg);
        dpAddData("UAP_abort_level", INT, KI_ABORT);
        return;
    }

    sprintf(tarPath, "/opt/kiS600/ap_data/EQUIP/TEST/PCM/%s/%s", tester_name, srcFileName);
    if ((tar_fp = fopen(tarPath, "w,ccs=UTF-8")) == NULL)
    {
        sprintf(err_msg, "DAC_DATA.Wafer_end_pgm_DAC: Can't open this file %s\n", tarPath);
        KTXEErrorMsg("%s", err_msg);
        OkMsgDlg(err_msg);
        dpAddData("UAP_abort_level", INT, KI_ABORT);
        return;
    }

    while (!feof(src_fp))
    {
        pStr = fgets(buffer, sizeof(buffer), src_fp);
        printf("%s", buffer);
        fputs(buffer, tar_fp);
    }

    if (src_fp != NULL)
    {
        printf("Wafer end at DAC_DATA output file transfer success.\n");
        fflush(src_fp);
        fclose(src_fp);
        src_fp = NULL;
    }

    if (tar_fp != NULL)
    {
        printf("Wafer end at DAC_DATA output file transfer success.\n");
        fflush(tar_fp);
        fclose(tar_fp);
        tar_fp = NULL;
    }

    src_fp = (FILE *)dpAddPointer("Anam_txt_fp_dac", LONG_P, NULL);
    remove(srcPath); /* Delete file existing in PCM_TMP path */

    filename = (char *)dpAddPointer("Anam_txt_fpname_dac", CHAR_P, NULL);

    /*--*/

    if (strstr(lot->testname, "ktxe") != NULL)
    {
        strcpy(buffer, lot->testname);
        strcpy(tst_name, strtok(buffer, " \n"));
        strcpy(tst_name, strtok(NULL, " \n"));
    }
    else
    {
        strcpy(tst_name, lot->testname);
    }
    
    /*--*/
    
    for(i = 0; i < strlen(tst_name); i++)
    {
        if(tst_name[i] == '_')
            break;   
        tstPgm[i] = tst_name[i];
    }

    RptEquipStat("WAFER_END", lot->id, wafer_pp->id, tstPgm);

    return;
    /* USRLIB MODULE END  */
} /* End wafer_end_pgm_DAC.c */
