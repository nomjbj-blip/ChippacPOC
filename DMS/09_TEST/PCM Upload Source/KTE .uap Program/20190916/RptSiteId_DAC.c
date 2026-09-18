/* USRLIB MODULE INFORMATION

	MODULE NAME: RptSiteId_DAC
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
#include "ktxe_types.h"
#include "kui_proto.h"
#include <string.h>
#include "COM_usrlib.h"
#include <string.h>

void RptSiteId_DAC()
{
/* USRLIB MODULE CODE */
    FILE *txt_fp;
    LOT *lot;
    SITE *site;
    char err_msg[1024], tst_name[100], buf[100];
    char flat_zone;
    wwp_list_t *current_wwp_list = NULL;
    WDFRec *wdfptr = NULL;

    lot = (LOT *)dpGetPointer("lot", LONG_P);
    site = (SITE *)dpGetPointer("site", LONG_P);
    current_wwp_list = (wwp_list_t *)dpGetPointer("current_wwp_list", LONG_P);
    wdfptr = ( WDFRec * ) dpGetPointer( "wdfptr", LONG_P );

    switch( wdfptr->flat )
    {
        case 0:
            flat_zone = 'B';         /* Bottom */
            break;
        case 90:
            flat_zone = 'L';         /* left */
            break;
        case 180:
            flat_zone = 'T';         /* top */
            break;
        case 270:
            flat_zone = 'R';         /* right */
            break;
    }


    txt_fp = (FILE *)dpGetPointer("Anam_txt_fp_dac", LONG_P);
    if (txt_fp == NULL)
    {
        sprintf(err_msg, "DAC_DATA.RptSiteid_DAC: Can't open file %s.txt\n", lot->id);
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

    fprintf(txt_fp, "-------------------------------------\n");
    fprintf(txt_fp, "Current Die = %s (%g, %g),   Current Die Type = %s\n", site->id, current_wwp_list->sitex, current_wwp_list->sitey, tst_name);
    fprintf(txt_fp, "Flat Zone = %c\n\n", flat_zone);
    fflush(txt_fp);
    return;
/* USRLIB MODULE END  */
} 		/* End RptSiteId_DAC.c */

