/* USRLIB MODULE INFORMATION

	MODULE NAME: AskWhereIdsComeFrom
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <guidedef.h>
#include <kui_proto.h>
#include <COM_usrlib.h>
	END USRLIB MODULE INFORMATION
*/
/* USRLIB MODULE HELP DESCRIPTION
AskWhereIdsComeFrom.c

3/15/2000		M. Vincent
	- Added Lot Control environment variable checks.
	END USRLIB MODULE HELP DESCRIPTION */
/* USRLIB MODULE PARAMETER LIST */
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <guidedef.h>
#include <kui_proto.h>
#include <COM_usrlib.h>

int AskWhereIdsComeFrom(  )
{
/* USRLIB MODULE CODE */
    VarMsgDlgDataPtr  data;
    int selection;
    char msg[60];
    int i;        
    char button1[20];
       char button2[20];
    char title[40];
    int button;
    int *manual_id_entry_flag;

    char *p_lot_control_lot_arg;
    char tmpmsg[ 128 ];
    int lot_num_mismatch=-1;

    /*********************************************************
     Get environment variable pointer set from Lot Control...
    **********************************************************/
    p_lot_control_lot_arg = getenv("LOT_CONTROL_LOT_ARG");
    
    /********************************************************
        If the Lot Control env var exists, this implies that
        (library TIGUI) module ti_gui has allowed execution
        to this point. We can assume that Lot Control 
        is being used, therefore OCR mode is also needed...
    *********************************************************/
    if( p_lot_control_lot_arg != NULL ) {
        return(KI_CONTINUE);
    }    

    /* Allocate some memory for the window data structure
     */
    data = (VarMsgDlgDataPtr)malloc(sizeof(VarMsgDlgDataRec));

    /* start the window defininitions...
     */
    data->no_buttons = 2 ; 
    data->no_lines = 6;

    /* allocate some space for the button labels
     */
    data->button_labels = (char **)malloc(sizeof(char *) * data->no_buttons);

    /* create button labels...
     */

    strcpy(button1, "OCR");
    strcpy(button2, "Keyboard");
    data->button_labels[0] = strdup(button1);
    data->button_labels[1] = strdup(button2);

    /* define window title and window message contents
     */
    strcpy(msg, " Choose how wafer ID's will be entered. ");
    data->ted_string = strdup( msg );
    strcpy(title, " Wafer ID Entry ");
    data->win_label = strdup( title );
    
    selection = VarMsgDlg( data );

    /* Clean up memory
     * We don't like memory leaks...
     */
    free( data->button_labels ) ;
    free( data ) ;

    if (selection == 0) {

    /* OCR mode */

    return(KI_CONTINUE);

    } else {

    /* Keyboard mode - enter a flag in the data pool and
       tell the operator to verify OCR is off on the prober */

    button = OkCancelAbortMsgDlg("Is OCR off on the prober?\n\n"
                 "Press OK to continue or CANCEL to ABORT test program");
    switch( button )
    {
    /*
     * DLG_OK is returned when the user picks the OK button
     * DLG_NO is returned when the user picks ABORT, and then
     * picks NO when asked to confirm the abort
     */
    case DLG_OK: /* Continue with lot */
    case DLG_NO: /* Abort canceled, continue with lot */
    manual_id_entry_flag = (int *)malloc(sizeof(int));
    if (manual_id_entry_flag != NULL) {
       *manual_id_entry_flag = 1;
       dpAddPointer("manual_id_entry_flag", INT_P, manual_id_entry_flag);
    }
    return(KI_CONTINUE);
       break;
    case DLG_ABORT: /* Lot aborted */
    return(KI_ABORT);
       break ;
    }

    }

    return(KI_CONTINUE);
/* USRLIB MODULE END  */
} 		/* End AskWhereIdsComeFrom.c */

