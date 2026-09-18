/* USRLIB MODULE INFORMATION

	MODULE NAME: RptData
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
#include <kdf.h>
#include <ksox_def.h>
#include <ktxe_proto.h>
	END USRLIB MODULE INFORMATION
*/
/* USRLIB MODULE HELP DESCRIPTION
Write data to txt file

use this module at UAP_TEST_DATA_LOG
	END USRLIB MODULE HELP DESCRIPTION */
/* USRLIB MODULE PARAMETER LIST */
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <time.h>
#include "ktxe_types.h"
#include "kui_proto.h"
#include "COM_usrlib.h"
#include <kdf.h>
#include <ksox_def.h>
#include <ktxe_proto.h>

void RptData()
{
    /* USRLIB MODULE CODE */
    FILE *txt_fp = NULL;
    FILE *txt_fp1 = NULL; // add by 2019.07.02
    LOT *lot;
    wwp_list_t *wwp_list;
    result_list_t *resp;
    LIMIT *limit, *limp;
    char tmpprogram[MAXFILENAMELENGTH]; /* */
    char tmppath[MAXFILENAMELENGTH];    /* scratch for path */
    char tmpext[MAXFILENAMELENGTH];     /* scratch for file extension */
    int i, j;
    char err_msg[1024];
    char par_id[100], lmt_name[100], buf[150], ssktm_name[100];

    lot = (LOT *)dpGetPointer("lot", LONG_P);
    limit = (LIMIT *)dpGetPointer("limit_list", LONG_P);

    wwp_list = (wwp_list_t *)dpGetPointer("current_wwp_list", LONG_P);
    strcpy(buf, wwp_list->ssktm);

    strcpy(ssktm_name, strtok(buf, ".\n"));
    strcpy(buf, ssktm_name);
    for (i = strlen(buf) - 1; i >= 0; i--)
        if (buf[i] == '/')
        {
            for (j = 1; j < strlen(buf) - i + 1; j++)
                ssktm_name[j - 1] = buf[i + j];
            ssktm_name[j - 1] = NULL;
            break;
        }
        else
            strcpy(ssktm_name, buf);
    /* opening a txt file*/
    txt_fp = (FILE *)dpGetPointer("Anam_txt_fp", LONG_P);
    if (txt_fp == NULL)
    {
        sprintf(err_msg, "Anam_Data_RptSiteId: Can't open file %s.txt\n", lot->id);
        KTXEErrorMsg("%s", err_msg);
        OkMsgDlg(err_msg);
        dpAddData("UAP_abort_level", INT, KI_ABORT);
        return;
    }

    /*--*/

    /* add by 2019.07.02*/
    txt_fp1 = (FILE *) dpGetPointer("Anam_txt_fp_dacrux", LONG_P);
    if(txt_fp1 == NULL)
    {
        sprintf(err_msg, "Anam_DACrux_Data_RptSiteId: Can't open file %s.txt\n", lot->id);
        KTXEErrorMsg("%s", err_msg);
        OkMsgDlg(err_msg);
        dpAddData("UAP_abort_level", INT, KI_ABORT);
        return;
    }

    /*--*/

    /* get result */
    resp = (result_list_t *)dpGetPointer("result_list", LONG_P);
    for (; resp; resp = resp->next)
        if (resp->log)
        {
            for (limp = limit; limp && strcmp(limp->id, resp->id); limp = limp->next)
                ;
            if (limp)
                strcpy(lmt_name, limp->name);
            else
                lmt_name[0] = NULL;
            strcpy(par_id, resp->id);
            for (i = 0; i < strlen(par_id); i++)
            {
                if (par_id[i] == 'x')
                    par_id[i] = ':';
            }
            fprintf(txt_fp, "%s:%s\t%g\t%s\n", ssktm_name, par_id, resp->value, lmt_name);

            /*--*/

            /* add data by 2019.07.02 */
            fprintf(txt_fp1, "%s:%s\t%g\t%s\n", ssktm_name, par_id, resp->value, lmt_name);

            /*--*/
        }

    /* save file pointer */
    dpAddPointer("Anam_txt_fp", LONG_P, txt_fp);

    /*--*/
    
    /* save file pointer by 2019.07.02 */
    dpAddPointer("Anam_txt_fp_dacrux", LONG_P, txt_fp1);

    /*--*/

    /* USRLIB MODULE END  */
} /* End RptData.c */
