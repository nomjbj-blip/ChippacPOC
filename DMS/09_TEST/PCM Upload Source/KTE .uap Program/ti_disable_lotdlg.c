/* USRLIB MODULE INFORMATION

	MODULE NAME: ti_disable_lotdlg
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "COM_usrlib.h"
	END USRLIB MODULE INFORMATION
*/
/* USRLIB MODULE HELP DESCRIPTION

3/15/2000  M. Vincent

ti_disable_lotdlg.c

Lot Control support.

Disables operator Lot Dialog box if lot control environment variable is detected.
	END USRLIB MODULE HELP DESCRIPTION */
/* USRLIB MODULE PARAMETER LIST */
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "COM_usrlib.h"

void ti_disable_lotdlg(  )
{
/* USRLIB MODULE CODE */
    int *lotDlg;
    char *p_lot_control_lot_arg;

    /****************************************
        Get environment variable pointer set
        from Lot Control...
    *****************************************/
    p_lot_control_lot_arg = getenv("LOT_CONTROL_LOT_ARG");

    /*********************************************
        If the Lot Control env var exists disable 
        the display of the Lot Dialog Screen...
    **********************************************/
    if( p_lot_control_lot_arg != NULL ) {
        KTXEDebugMsg("Lot Control detected; Disabling lot dialog display.\n");
        lotDlg = ( int *)dpGetPointer( "display_lotdlg", INT_P );
        *lotDlg = 0;
    } else {
        KTXEDebugMsg("Lot Control not detected; leaving lot dialog display on.\n");
    }

    return;
/* USRLIB MODULE END  */
} 		/* End ti_disable_lotdlg.c */

