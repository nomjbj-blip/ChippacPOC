/* USRLIB MODULE INFORMATION

	MODULE NAME: wafer_end_pgm
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
#include <sys/types.h>
#include <sys/stat.h>
#include <netdb.h>
#include <ktxe_proto.h>
#include <ksox_def.h>
#include <dirent.h> /* 2019.07.09 add */
#include "ktxe_types.h"
#include "kui_proto.h"
#include "COM_usrlib.h"
#include "ktxe_proto.h"

/* add by 2019.07.10 */
void transferEquipHome()
{
    char err_msg[1024], nfsPath[100];
    char equipRawdata[256];
    /* char equipCfg[256], keys[100], values[100], equipid[100];  */
    char script[1024];

    DIR *equipDir = NULL;
    /* FILE *fpCfg = NULL; */

    struct dirent *dirEntry;
    /* struct stat statBuffer;  */

    /*
    sprintf(equipCfg, "%s/EQUIP/equip.cfg", getenv("KIDB"));
    if ((fpCfg = fopen(equipCfg, "r")) == NULL)
    {
        sprintf(err_msg, "Can't find Equipment config file.: %s\n", equipCfg);
        KTXEErrorMsg("%s", err_msg);
        OkMsgDlg(err_msg);
        dpAddData("UAP_abort_level", INT, KI_ABORT);
        return;
    }

    while (!feof(fpCfg))
    {
        fscanf(fpCfg, "%s=%s", &keys, &values);
        printf("%s\t%s\n", keys, values);

        if (strcmp(keys, "EQID") == 0)
        {
            strcpy(equipid, values);
        }
        else if (strcmp(keys, "PATH") == 0)
        {
            strcpy(nfsPath, values);
        }
    }
    */


    sprintf(equipRawdata, "%s/EQUIP/Data/", getenv("KIDB"));
    equipDir = opendir(equipRawdata);
    if (equipDir == NULL)
    {
        sprintf(err_msg, "Can't access directory.: %s\n", equipRawdata);
        KTXEErrorMsg("%s", err_msg);
        OkMsgDlg(err_msg);
        dpAddData("UAP_abort_level", INT, KI_ABORT);
        return;
    }

    /* add by 2019.07.10 */
    sprintf(nfsPath, "%s", getenv("PCM_SERVER_PATH"));
    while (dirEntry = readdir(equipDir))
    {
        /* exists directory: . ..  */
        if(strcmp(dirEntry->d_name, ".") == 0)
            continue;
        if(strcmp(dirEntry->d_name, "..") == 0)
            continue;

        sprintf(script, "mv %s/%s %s/%s", equipRawdata, dirEntry->d_name, nfsPath, dirEntry->d_name);
        system(script);
    }
}

void wafer_end_pgm()
{
    /* USRLIB MODULE CODE */
    char cmd_line[256] = "xterm -T WAFER_TEST_TIMES -n TEST_TIME -e tail -f /user/tmp/WaferTestTimeLog &";

    WAFER *wafer_pp = (WAFER *)dpGetPointer("wafer", LONG_P);

    FILE *WaferTestTime = NULL;
    FILE *WaferTestPgm = NULL;
    FILE *txt_fp1 = NULL;

    long ttime;
    int i;
    int pcd_len;

    char err_msg[1024], buf[200];
    char tst_name[20], imsi[30], imsi1[30], pcd_name[20];

    LOT *lot = (LOT *)dpGetPointer("lot", LONG_P);

    char hostname[MAXHOSTNAMELEN], cfgFileInfo[100];
    time_t clock;
    struct tm *tm_p;
    struct tm *tm_p2;
    static char_date[80];
    static lot_date[80];
    clock = time((time_t *)0);
    tm_p = localtime(&clock);
    tm_p2 = localtime(&clock);
    strftime((char *)char_date, sizeof(char_date), "%Y/%m/%d %T", tm_p);
    strftime((char *)lot_date, sizeof(lot_date), "%Y%m%d", tm_p2);

    if (gethostname(hostname, MAXHOSTNAMELEN) == -1)
        strcpy(hostname, lot->system);

    for (i = 0; i < strlen(hostname); i++)
        hostname[i] = toupper(hostname[i]);

    if (wafer_pp->id != NULL)
        printf("%s|%s|%s|E\n", char_date, hostname, wafer_pp->id);

    WaferTestTime = (FILE *)dpGetPointer("Anam_time_log", LONG_P);
    WaferTestPgm = (FILE *)dpGetPointer("Anam_pgm_log", LONG_P);

    /*--*/

    /* add by 2019.07.02 */
    txt_fp1 = (FILE *)dpGetPointer("Anam_txt_fp_dacrux", LONG_P);
    if (txt_fp1 != NULL)
    {
        printf("Wafer end at anam DACrux data!! \n");
        fclose(txt_fp1);
    }
    txt_fp1 = (FILE *)dpAddPointer("Anam_txt_fp_dacrux", LONG_P, NULL);

    /*--*/
    transferEquipHome();
    /*--*/

    /*WaferTestTime = fopen(buf,"a");*/
    strcpy(imsi, lot->testname);
    sscanf(imsi, "%s%s", imsi1, tst_name);

    /* 2005 11 20  by ju  */

    lot = (LOT *)dpGetPointer("lot", LONG_P);
    strcpy(imsi1, lot->process);
    pcd_len = strlen(imsi1);
    if (pcd_len < 4)
        strcpy(pcd_name, "NONE");
    else
        sprintf(pcd_name, "%c%c%c%c", imsi1[pcd_len - 4], imsi1[pcd_len - 3], imsi1[pcd_len - 2], imsi1[pcd_len - 1]);

    /* 2005 11 20  by ju  */

    if (WaferTestTime != NULL && wafer_pp->id != NULL)
        fprintf(WaferTestTime, "%s|%s|%s|E\n", char_date, hostname, wafer_pp->id);
    fprintf(WaferTestPgm, "%s|%s|%s|%s|E|%s|%s\n", char_date, hostname, tst_name, wafer_pp->id, pcd_name, lot->id);
    fclose(WaferTestTime);
    fclose(WaferTestPgm);
    fflush(stdout);
    return;

    /* USRLIB MODULE END  */
} /* End wafer_end_pgm.c */
