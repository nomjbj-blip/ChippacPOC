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

int RptEquipStat(char *state, char *value, char *mode);

void wafer_end_pgm_DAC(  )
{
    /* USRLIB MODULE CODE */
    FILE *txt_fp = NULL;
    FILE *target_txt_fp = NULL;
    FILE *tester = NULL;

    int i, pcd_len;
    
    char hostname[MAXHOSTNAMELEN];
    char *filename, script[200];
    char tdata[20];
    char tester_name[20];
    time_t clock;
    struct tm *tm_p;
    static char_date[80];

    LOT *lot = (LOT *)dpGetPointer("lot", LONG_P);    
    WAFER *wafer_pp = (WAFER *)dpGetPointer("wafer", LONG_P);
    
    clock = time((time_t *)0);
    tm_p = localtime(&clock);
    strftime((char *)char_date, sizeof(char_date), "%Y/%m/%d %T", tm_p);

    system("uname -n > /tmp/.tester_name");
    tester=fopen("/tmp/.tester_name","r");
    fgets(tdata,100,tester);
    sscanf(tdata,"%s",tester_name);

    for(i = 0; i <strlen(tester_name); i++)
        tester_name[i] = toupper(tester_name[i]);

    if (gethostname(hostname, MAXHOSTNAMELEN) == -1)
        strcpy(hostname, lot->system);

    for (i = 0; i < strlen(hostname); i++)
        hostname[i] = toupper(hostname[i]);

    if (wafer_pp->id != NULL)
        printf("%s|%s|%s|E\n", char_date, hostname, wafer_pp->id);

    filename = (char *)dpGetPointer("Anam_txt_fpname_dac", CHAR_P);
    txt_fp = (FILE *)dpGetPointer("Anam_txt_fp_dac", LONG_P);
    if (txt_fp != NULL)
    {
        printf("Wafer end at anam DACrux data!! \n");
        fclose(txt_fp);
    }
    txt_fp = (FILE *)dpAddPointer("Anam_txt_fp_dac", LONG_P, NULL);

    printf("filename: %s\n tester name: %s\n", filename, tester_name);

    sprintf(script, "cp /opt/kiS600/ap_data/EQUIP/TEST/PCM_TMP/%s /opt/kiS600/ap_data/EQUIP/TEST/PCM/%s/%s", filename, tester_name, filename);
    system(script);

    sprintf(script, "rm /opt/kiS600/ap_data/EQUIP/TEST/PCM_TMP/%s", filename);
    system(script);

    
    filename = (char *)dpAddPointer("Anam_txt_fpname_dac", CHAR_P, NULL);
    RptEquipStat("WAFER_END", wafer_pp->id, "WAFER");

    return;
    /* USRLIB MODULE END  */
}/* End wafer_end_pgm_DAC.c */

