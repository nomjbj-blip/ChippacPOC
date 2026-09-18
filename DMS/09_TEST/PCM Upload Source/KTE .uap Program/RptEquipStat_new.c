#include <stdio.h>
#include <stdlib.h>
#include "ktxe_types.h"
#include "kui_proto.h"
#include "COM_usrlib.h"
#include "ktxe_proto.h"

int RptEquipStat(char *value, char *state, char *mode)
{
    FILE *txt_Stat = NULL;
    char statFile[50], hostname[10];
    char err_msg[1024];
    
    FILE *tester = NULL;
    char tdata[20];
    char tester_name[20];

    system("uname -n > /tmp/.tester_name");
    tester=fopen("/tmp/.tester_name","r");
    fgets(tdata,100,tester);
    sscanf(tdata,"%s",tester_name);
    
    sprintf(statFile, "/opt/kiS600/ap_data/EQUIP/TEST/STATUS/%s", hostname);
    if ((txt_Stat = fopen(statFile, "w")) == NULL)
    {
        sprintf(err_msg, "Equipment Status_TWInitLot: Unable to open status file %s\n", statFile);
        KTXEErrorMsg("%s", err_msg);
        OkMsgDlg(err_msg);
        dpAddData("UAP_abort_level", INT, KI_ABORT);
        return(-1);
    }
    fprintf(txt_Stat, "%s,%s,%s", state, value, mode);
    fflush(txt_Stat);
    fclose(txt_Stat);

    return (0);   
}