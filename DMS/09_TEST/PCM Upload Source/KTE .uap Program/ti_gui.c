/* USRLIB MODULE INFORMATION

	MODULE NAME: ti_gui
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>


#define _REENTRANT 
#include <stdlib.h>
#include <string.h>
#include <time.h>
#include "ktxe_types.h"
#include "ktxe_proto.h"
#include "kui_proto.h"
#include "kdf.h"
                       
#include "ksox_def.h"
#include "prb.h"
#include "prb_proto.h"
#include "prb_msg.h"
#include "guidedef.h"
#include "wtype.h"
#include <thread.h>
#define NDEBUG
#include <assert.h>
	END USRLIB MODULE INFORMATION
*/
/* USRLIB MODULE HELP DESCRIPTION

ti_gui.c

3/15/2000		M. Vincent
	- Added Lot Control checks.
	END USRLIB MODULE HELP DESCRIPTION */
/* USRLIB MODULE PARAMETER LIST */
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>


#define _REENTRANT 
#include <stdlib.h>
#include <string.h>
#include <time.h>
#include "ktxe_types.h"
#include "ktxe_proto.h"
#include "kui_proto.h"
#include "kdf.h"
                       
#include "ksox_def.h"
#include "prb.h"
#include "prb_proto.h"
#include "prb_msg.h"
#include "guidedef.h"
#include "wtype.h"
#include <thread.h>
#define NDEBUG
#include <assert.h>

int ti_gui(  )
{
/* USRLIB MODULE CODE */
/* Prototypes */
int VarMsgDlgWindow( char *) ;
int VarMsgDlgWindow0( char *) ;
int VarMsgDlgWindow2( char *) ;
int VarMsgDlgWindow3( char *) ;

LOT *lot ;
char temp[256];
char commentmsg[256];
char msg[ 128 ] ;
char FMAT[ 128 ] ;
char TMODE[ 128 ] ;
int button, status,online=0,dat1=1,dat0=0 ;
int i ;
char *stub;
char *p_lot_control_lot_arg;
char tmpmsg[ 128 ];
int lot_num_mismatch=-1;


/************************************************************
    If no-instruments env var set, set offline and return...
*************************************************************/
stub=getenv("KI_LPT_STUB");

if ( stub != NULL) {
      status = dpAddData("TI_TW_ONLINE", INT, 0);
      status = dpAddData("TI_AUTO_ONLINE",INT,  0 );
      return;
}

/*********************************************************
 Get environment variable pointer set from Lot Control...
**********************************************************/
p_lot_control_lot_arg = getenv("LOT_CONTROL_LOT_ARG");

KTXEDebugMsg("Checking Lot Control env var...\n");

/****************************************
    If the Lot Control env var exists...
*****************************************/
if( p_lot_control_lot_arg != NULL ) {


    KTXEDebugMsg("Lot Control var exists...\n");

    /*************************************
         Get lot number passed to ktxe...
    **************************************/
    lot = (LOT *) dpGetPointer("lot", LONG_P);

    /***************************************************
        Compare lot number passed to ktxe to lot number
        set in environment by Lot Control...
    ****************************************************/
    lot_num_mismatch = strcmp( (char *)lot, p_lot_control_lot_arg );

    /**************************************************
        If they do not match, someone may be trying to
        run ktxe without Lot Control. Don't allow...
    ***************************************************/
    if( lot_num_mismatch ) {
        sprintf( tmpmsg, "ERROR:\n");
        sprintf( tmpmsg, "%sLOT_CONTROL_LOT_ARG=%s\n\n", tmpmsg, p_lot_control_lot_arg);
        sprintf( tmpmsg, "%sLot number passed to ktxe=%s\n\n", tmpmsg, lot);
        sprintf( tmpmsg, "%sPlease use Lot Control for production.\n\n", tmpmsg);
        sprintf( tmpmsg, "%sAborting ktxe.", tmpmsg);
        OkMsgDlg( tmpmsg );
        exit(-1);
    } else {
        /*******************************************************
            If the lot numbers match, set for online (TestWare)
             and continue ktxe (return)...
        ********************************************************/
        printf("Passed Lot Control safeguard.\n");
        status = dpAddData( "TI_TEST_MODE", INT, 1);
        strcpy( FMAT, "Online" ) ;
        status = dpAddData( "TI_TW_ONLINE", INT, 1);
        status = dpAddData("TI_AUTO_ONLINE", INT, 0);
        sprintf( commentmsg, "TEST SELECTION:      FORMAT= %s ",FMAT ) ; 
        strcpy(lot->comment, commentmsg);
        return;
    }

} else {
    /************************************************************************
        If the Lot Control env var does not exist, assume engineering
        is trying to run program and display option dialog boxes normally...
    *************************************************************************/
/*    sprintf( tmpmsg, "INFORMATION MESSAGE:\nLot Control not detected\n");
    sprintf( tmpmsg, "%sPress OK to continue without Lot Control.", tmpmsg );
    OkMsgDlg( tmpmsg );*/ /* lot control is not used, remove this message 071900 */
}


strt:
     /* Display a new dialog : modified */
     
    setDlgTitle( "Fisrt Wafer Location" ) ;
    strcpy( msg, "Fisrt Wafer : Left mouse click 1st wafer location! " ) ;
    button = VarMsgDlgWindow( msg ) ;
    switch( button )
    {
    case 0: {strcpy( TMODE,"Cassette");
         status = dpAddData( "skip_first_wafer_load", INT, 0);
      break ;
            }
    case 1: {strcpy( TMODE,"Chuck");
         status = dpAddData( "skip_first_wafer_load", INT, 1);
         break ;
            }
    default: break ;
    }
    
    strcpy( TMODE,"Parametric");
    status = dpAddData( "TI_TEST_MODE", INT, 1);

    /* Display a new dialog
     */
    setDlgTitle( "FORMAT SELECTION WINDOW" ) ;
    strcpy( msg, "FORMAT: Left mouse click desired FORMAT button! " ) ;
    button = VarMsgDlgWindow2( msg ) ;
    switch( button )
    {

   case 0: {strcpy( FMAT, "Online" ) ;
      status = dpAddData( "TI_TW_ONLINE", INT, 1);
      status = dpAddData("TI_AUTO_ONLINE", INT, 0);
      online=1;
         break ;
            }
    case 1: {strcpy( FMAT, "Offline" ) ;
      status = dpAddData( "TI_TW_ONLINE", INT, 0);
      status = dpAddData("TI_AUTO_ONLINE",INT,  0 );
      online=0;    
         break ;
            }
    default: break ;
    } 

gui_end:
if(online==1) {
    setDlgTitle( "STARTUP VERIFICATION WINDOW" ) ;
    strcpy( msg, "Please verify 'STARTUP' has been launched! " ) ;
    button = VarMsgDlgWindow0( msg ) ;
    switch( button )
    {
    case 0: {
 
      break ;
            }
    case 1: {
      exit(KI_ABORT);
         break ;
            }
 
         default: break ;
    }
}

 lot= (LOT *) dpGetPointer("lot", LONG_P);
/*  sprintf( commentmsg, "TEST SELECTION:      TEST MODE= %s\t FORMAT= %s ",TMODE,FMAT ) ; */
  sprintf( commentmsg, "TEST SELECTION:      FORMAT= %s ",FMAT ) ; 
     strcpy(lot->comment, commentmsg); 

  }

int VarMsgDlgWindow0( char *msg )
{
    VarMsgDlgDataPtr  data;
    int selection;
    int i;

    /* Allocate some memory for the window data structure
     */
    data = (VarMsgDlgDataPtr)malloc(sizeof(VarMsgDlgDataRec));

    /* start the window defininitions...
     */
    data->no_buttons = 2 ; 
    data->no_lines = 8;

    /* allocate some space for the button labels
     */
    data->button_labels = (char **)malloc(sizeof(char *) * data->no_buttons);

    /* create button labels...
     */

    for(i=0; i<data->no_buttons; i++)
    {
        char buff[20];
if (i==0) { sprintf(buff, "Yes");
          data->button_labels[i] = strdup(buff);
         }
if (i==1) { sprintf(buff, "No");
          data->button_labels[i] = strdup(buff);
         }

    }



    /* define window title and window message contents
     */
    data->ted_string = strdup( msg );
    data->win_label = strdup("");
    
    selection = VarMsgDlg( data );

    /* Clean up memory
     * We don't like memory leaks...
     */
    free( data->button_labels ) ;
    free( data ) ;

    return( selection ) ;
}

int VarMsgDlgWindow( char *msg )
{
    VarMsgDlgDataPtr  data;
    int selection;
    int i;

    /* Allocate some memory for the window data structure
     */
    data = (VarMsgDlgDataPtr)malloc(sizeof(VarMsgDlgDataRec));

    /* start the window defininitions...
     */
    data->no_buttons = 2 ; 
    data->no_lines = 8;

    /* allocate some space for the button labels
     */
    data->button_labels = (char **)malloc(sizeof(char *) * data->no_buttons);

    /* create button labels...
     */

    for(i=0; i<data->no_buttons; i++)
    {
        char buff[20];
if (i==0) { sprintf(buff, "Cassette");
          data->button_labels[i] = strdup(buff);
         }
if (i==1) { sprintf(buff, "Chuck");
          data->button_labels[i] = strdup(buff);
         }

    }



    /* define window title and window message contents
     */
    data->ted_string = strdup( msg );
    data->win_label = strdup("");
    
    selection = VarMsgDlg( data );

    /* Clean up memory
     * We don't like memory leaks...
     */
    free( data->button_labels ) ;
    free( data ) ;

    return( selection ) ;
}

int VarMsgDlgWindow2( char *msg )
{
    VarMsgDlgDataPtr  data;
    int selection;
    int i;

    /* Allocate some memory for the window data structure
     */
    data = (VarMsgDlgDataPtr)malloc(sizeof(VarMsgDlgDataRec));

    /* start the window defininitions...
     */
    data->no_buttons = 2 ; 
    data->no_lines = 8;

    /* allocate some space for the button labels
     */
    data->button_labels = (char **)malloc(sizeof(char *) * data->no_buttons);

    /* create button labels...
     */

    for(i=0; i<data->no_buttons; i++)
    {
        char buff[20];
if (i==0) { sprintf(buff, "On-Line");
          data->button_labels[i] = strdup(buff);
         }
if (i==1) { sprintf(buff, "Off-Line");
          data->button_labels[i] = strdup(buff);
         }
/*
        sprintf(buff, "BUTTON %d", i);
        data->button_labels[i] = strdup(buff);
*/
    }


    /* define window title and window message contents
     */
    data->ted_string = strdup( msg);
    data->win_label = strdup("");
    
    selection = VarMsgDlg( data );

    /* Clean up memory
     * We don't like memory leaks...
     */
    free( data->button_labels ) ;
    free( data ) ;

    return( selection ) ;
}
int VarMsgDlgWindow3( char *msg )
{
    VarMsgDlgDataPtr  data;
    int selection;
    int i;

    /* Allocate some memory for the window data structure
     */
    data = (VarMsgDlgDataPtr)malloc(sizeof(VarMsgDlgDataRec));

    /* start the window defininitions...
     */
    data->no_buttons = 2 ; 
    data->no_lines = 8;

    /* allocate some space for the button labels
     */
    data->button_labels = (char **)malloc(sizeof(char *) * data->no_buttons);

    /* create button labels...
     */

    for(i=0; i<data->no_buttons; i++)
    {
        char buff[20];
if (i==0) { sprintf(buff, "OK");
          data->button_labels[i] = strdup(buff);
         }
if (i==1) { sprintf(buff, "CANCEL");
          data->button_labels[i] = strdup(buff);
         }
/*
        sprintf(buff, "BUTTON %d", i);
        data->button_labels[i] = strdup(buff);
*/
    }



    /* define window title and window message contents
     */
    data->ted_string = strdup( msg);
    data->win_label = strdup("");
    
    selection = VarMsgDlg( data );

    /* Clean up memory
     * We don't like memory leaks...
     */
    free( data->button_labels ) ;
    free( data ) ;

    return( selection ) ;

/* USRLIB MODULE END  */
} 		/* End ti_gui.c */

