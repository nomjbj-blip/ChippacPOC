/* USRLIB MODULE INFORMATION

	MODULE NAME: TIDefineWaferID_DMOS5
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <string.h>
#include <stdlib.h>
#include <unistd.h>
#include "kui_proto.h"
#include "ktxe_types.h"
#include "COM_usrlib.h"
#define TI_MAX_WAFER_ID_LENGTH 17
	END USRLIB MODULE INFORMATION
*/
/* USRLIB MODULE HELP DESCRIPTION
Purpose
	TIDefineWaferId_DMOS5 is provided to generate a wafer id based on the 11th and 12th character
of the prober ocr id. If the ocr id is not at least
12 characters long the slot number of the 
wafer on the chuck is used.

Format
	TIDefineWaferId_DMOS5()

Usage
	Used at UAP_VALIDATE_OCR.
	END USRLIB MODULE HELP DESCRIPTION */
/* USRLIB MODULE PARAMETER LIST */
#include <stdio.h>
#include <string.h>
#include <stdlib.h>
#include <unistd.h>
#include "kui_proto.h"
#include "ktxe_types.h"
#include "COM_usrlib.h"
#define TI_MAX_WAFER_ID_LENGTH 17

void TIDefineWaferID_DMOS5(  )
{
/* USRLIB MODULE CODE */
char *lotid;
char ti_wafer_id[TI_MAX_WAFER_ID_LENGTH];
WAFER  *wafer;
char *prober_wafer_id = NULL;

/* Get pointer to prober wafer id */

prober_wafer_id = ( char * ) dpGetPointer( "prober_wafer_id", CHAR_P );

if ( prober_wafer_id != NULL )
{
    if ( prober_wafer_id[7] == '-' )
    {
        /* WaferID Change 2017 : AK1234567-01-A0 --> 1234567-01 */
        strcpy(ti_wafer_id,prober_wafer_id);
        ti_wafer_id[10] = NULL;
    }
    else
    {
        if ( strlen( prober_wafer_id ) >= 12 )
        {
            strcpy(ti_wafer_id,prober_wafer_id);
            ti_wafer_id[12] = NULL;

        }
        else
        {
            /* Get slot number for wafer id since the prober version is not long enough. */
            wafer = ( WAFER * ) dpGetPointer( "wafer", LONG_P );
            sprintf( ti_wafer_id, "%02d", wafer->slot );
        }
    }
    
    strcpy( prober_wafer_id, ti_wafer_id );
}
/* USRLIB MODULE END  */
} 		/* End TIDefineWaferID_DMOS5.c */

