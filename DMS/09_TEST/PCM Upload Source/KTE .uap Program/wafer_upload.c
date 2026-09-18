#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
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

/* add by 2019.07.10 */
void wafer_upload()
{
    char err_msg[1024], nfsPath[100];
    char equipRawdata[256];
    /* char equipCfg[256], keys[100], values[100], equipid[100];  */
    char script[1024];

    DIR *equipDir = NULL;
    struct dirent *dirEntry;
    /* struct stat statBuffer;  */

    FILE *tester;
    char tdata[20];
    char tester_name[20];

    system("uname -n > /tmp/.tester_name");
    tester = fopen("/tmp/.tester_name", "r");
    fgets(tdata, 100, tester);
    sscanf(tdata, "%s", tester_name);

    sprintf(equipRawdata, "%s/FORMAT/DAcrux/%s", getenv("KIDB"), tester_name);
    equipDir = opendir(equipRawdata);
    if (equipDir == NULL)
    {
        sprintf(err_msg, "Can't access directory.: %s\n", equipRawdata);
        KTXEErrorMsg("%s", err_msg);
        OkMsgDlg(err_msg);
        dpAddData("UAP_abort_level", INT, KI_ABORT);
        return;
    }
    
    sprintf(nfsPath, "%s/EQUIP/TEST/PCM/%s", getenv("PCM_SERVER_PATH"), tester_name);
    while (dirEntry = readdir(equipDir))
    {
        /* exists directory: . ..  */
        if (strcmp(dirEntry->d_name, ".") == 0)
            continue;
        if (strcmp(dirEntry->d_name, "..") == 0)
            continue;

        if (strstr(dirEntry->d_name, ".txt") == NULL)
            continue;

        sprintf(script, "cp %s/%s %s/%s", equipRawdata, dirEntry->d_name, nfsPath, dirEntry->d_name);
        system(script);

        sprintf(script, "rm %s/%s", equipRawdata, dirEntry->d_name);
        system(script);
    }
}