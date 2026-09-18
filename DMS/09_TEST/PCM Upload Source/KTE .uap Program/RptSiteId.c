/* USRLIB MODULE INFORMATION

	MODULE NAME: RptSiteId
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include "ktxe_types.h"
#include "kui_proto.h"
#include <string.h>
#include "COM_usrlib.h"
#include <string.h>
	END USRLIB MODULE INFORMATION
*/
/* USRLIB MODULE HELP DESCRIPTION

Writes site id to txt file

use the module at UAP_SITE_CHANGE
	END USRLIB MODULE HELP DESCRIPTION */
/* USRLIB MODULE PARAMETER LIST */
#include <stdio.h>
#include <string.h>
#include "ktxe_types.h"
#include "kui_proto.h"
#include "COM_usrlib.h"

void RptSiteId()
{
    /* USRLIB MODULE CODE */

    FILE *txt_fp, *txt_fp1;
    LOT *lot;
    SITE *site;
    char err_msg[1024], tst_name[100], buf[100];

    lot = (LOT *)dpGetPointer("lot", LONG_P);
    site = (SITE *)dpGetPointer("site", LONG_P);

    txt_fp = (FILE *)dpGetPointer("Anam_txt_fp", LONG_P);
    if (txt_fp != NULL)
    {
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
        
        fprintf(txt_fp, "-------------------------------------\n");
        fprintf(txt_fp, "Current Die = %s,   Current Die Type = %s\n\n", site->id, tst_name);
    }
    else
    {
        sprintf(err_msg, "Anam_Data_RptSiteId: Can't open file %s.txt\n", lot->id);
        KTXEErrorMsg("%s", err_msg);
        OkMsgDlg(err_msg);
        dpAddData("UAP_abort_level", INT, KI_ABORT);
        return;
    }

    /*--*/

    /* add by 2019.07.02 */
    txt_fp1 = (FILE *)dpGetPointer("Anam_txt_fp_dacrux", LONG_P);
    if (txt_fp1 != NULL)
    {
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

        fprintf(txt_fp1, "-------------------------------------\n");
        fprintf(txt_fp1, "Current Die = %s,   Current Die Type = %s\n\n", site->id, tst_name);
    }
    else
    {
        sprintf(err_msg, "Anam_DACrux_Data_RptSiteId: Can't open file %s.txt\n", lot->id);
        KTXEErrorMsg("%s", err_msg);
        OkMsgDlg(err_msg);
        dpAddData("UAP_abort_level", INT, KI_ABORT);
        return;
    }

    return;
    /* USRLIB MODULE END  */
} /* End RptSiteId.c */
