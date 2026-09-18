/* USRLIB MODULE INFORMATION

	MODULE NAME: ti_tsk_device_load
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "ktxe_types.h"
#include "COM_usrlib.h"
#include "kui_proto.h"
#include "kdf.h"
	END USRLIB MODULE INFORMATION
*/
/* USRLIB MODULE HELP DESCRIPTION
ti_tsk_device_load.c

3/15/2000 

Lot Control support.

Implemented at UAP_PROBER_INIT

Usage:   ktxe -u "tskdevicefile storage_device_letter"

Example: ktxe -u "M785701S e"
Will retrieve the file M785701S from the network "e" drive.

NOTE: TSK Operation Settings/Group Management/Communications Mode MUST be set to "Remote" mode.
	END USRLIB MODULE HELP DESCRIPTION */
/* USRLIB MODULE PARAMETER LIST */
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "ktxe_types.h"
#include "COM_usrlib.h"
#include "kui_proto.h"
#include "kdf.h"

int ti_tsk_device_load(  )
{
/* USRLIB MODULE CODE */

char *p_user_arg ;
char product_file[17];
char *storage_device_letter;
char tmpmsg[256];
char prResponse[20];
char prMsg[79];
int i;
int i_SRQ;
int retry_cnt;
int istat;
int max_retry = 4;
char *p_lot_control_lot_arg;

/*********************************************************
     Get environment variable pointer set from Lot Control...
**********************************************************/
p_lot_control_lot_arg = getenv("LOT_CONTROL_LOT_ARG");

/********************************************************
    If the Lot Control env var does not exist, this
    implies Lot Control is not being used. Show
    information message and continue without sending
    prober device load request...
*********************************************************/
if( p_lot_control_lot_arg == NULL ) {
    sprintf(tmpmsg, "INFORMATION MESSAGE:\n");
    sprintf(tmpmsg, "%sLot Control not detected.\n", tmpmsg);
    sprintf(tmpmsg, "%sEnsure prober has correct ", tmpmsg);
    sprintf(tmpmsg, "%sdevice file loaded.\n", tmpmsg);
    sprintf(tmpmsg, "%sSelect OK to continue...", tmpmsg);
    OkMsgDlg( tmpmsg );
    return(KI_CONTINUE);
}    


/* Initialize string with spaces/terminator... */
for(i=0;i<=15;i++) product_file[i] = ' ';
product_file[16] = NULL ;

/* Retrieve -u option string...*/
p_user_arg = ( char *)dpGetPointer( "user_arg", CHAR_P ) ;

/* Build error message...*/
    sprintf(tmpmsg, "ERROR in TSK device file name or drive letter!\n");
    sprintf(tmpmsg, "%sExample: ktxe -u \"M785701S e\"\n", tmpmsg);
    sprintf(tmpmsg, "%sRetrieves the file M785701S from the ", tmpmsg);
    sprintf(tmpmsg, "%snetwork \"e\" drive.\n", tmpmsg);
    sprintf(tmpmsg, "%sAborting program...\n", tmpmsg);

/* Ensure string passed to engine contains at least one space...*/
if( strchr( p_user_arg, ' ' ) == NULL ) {
    printf("Bad -u option structure: %s\n", p_user_arg);
    printf("%s", tmpmsg);
    exit(-1);
}

/* Extract drive letter...*/
storage_device_letter = ( strchr( p_user_arg, ' ' ) + 1 );

/* Ensure drive letter is valid...*/
switch(*storage_device_letter) {
    case 'a':
        break;
    case 'c':
        break;
    case 'e':
        break;
    default:
        printf("Bad drive letter specified: %s\n", storage_device_letter);
        printf("%s", tmpmsg);
        exit(-1);
}

/* Extract device file name...*/
strncpy( product_file, p_user_arg, ( strlen( p_user_arg ) - 2 ) );

sprintf( tmpmsg, "Asking prober to load device file \"%s\" from network, please wait...", product_file);
KTXEUpdateStatusAbort( tmpmsg );

retry_cnt = 0;
istat = -1;

while ( ( retry_cnt < max_retry ) && ( istat < 0 ) )
{
    /* there is an error, so retry the profile */
    (void)PrError();  /* Flush (and ignore) any existing error */
    istat = PrLoadProduct(product_file, storage_device_letter);    /* retry the command */
    KTXEDebugMsg( "PrLoadProduct status = %d \n", istat );
    if (istat >= 0) 
       break;

    /* Force prober to alarm with mandatory 20 char message. */
    /* STB's 101 and 76 set to 0 on prober so no SRQ is expected. */
    i_SRQ = 0;
    PrWriteReadSRQ("emSee host display msg\r\n", 24, prResponse, 0, 5, &i_SRQ);

    sprintf( tmpmsg , "Prober error loading device file: %s\n", product_file );
    sprintf( tmpmsg , "%sEnsure device file exists in prober's ", tmpmsg );
    sprintf( tmpmsg , "%sGROUP MANAGEMENT list.\n", tmpmsg );
    sprintf( tmpmsg , "%sEnsure Op. Settings/Change Group MNG Comm Mode ", tmpmsg );
    sprintf( tmpmsg , "%sis set to REMOTE.\n", tmpmsg );
    sprintf( tmpmsg , "%sPress OK to retry (%d tries remaining).\n", tmpmsg, (max_retry - retry_cnt)-1 );
    sprintf( tmpmsg , "%sOr press ABORT in Status box to abort.", tmpmsg );
    OkMsgDlg( tmpmsg );

    retry_cnt++;
}

if (retry_cnt >= max_retry || istat < 0 )
{
    /* ask for assistance */
/*    sprintf( tmpmsg , "ERROR loading PRODUCT FILE:\n%s\n" */
/*        "Please clear the error and press CONTINUE.\n" */
/*        "Press ABORT to ABORT test program.", product_file); */
/*    KTXEProberErrorMessage(istat, tmpmsg , "PrLoadProduct"); */

    sprintf( tmpmsg , "ERROR loading device file: %s\n", product_file);
    sprintf( tmpmsg , "%sPress OK to ABORT test program.", tmpmsg );
    OkMsgDlg( tmpmsg );
    return(KI_ABORT);

}

return(KI_OK);
/* USRLIB MODULE END  */
} 		/* End ti_tsk_device_load.c */

