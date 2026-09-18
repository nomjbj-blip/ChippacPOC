/* USRLIB MODULE INFORMATION

	MODULE NAME: GetWaferIdsFromOperator
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "kdf.h"
#include "ktxe_types.h"
#include "kui_proto.h"
#include "COM_usrlib.h"
	END USRLIB MODULE INFORMATION
*/
/* USRLIB MODULE HELP DESCRIPTION

	END USRLIB MODULE HELP DESCRIPTION */
/* USRLIB MODULE PARAMETER LIST */
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "kdf.h"
#include "ktxe_types.h"
#include "kui_proto.h"
#include "COM_usrlib.h"

void GetWaferIdsFromOperator(  )
{
/* USRLIB MODULE CODE */
WAFER *wafer = NULL ;
WAFER *First = NULL ;

long *tmp ;
slot_list_t *slot ;

int total_wafers ;
int dlg_ret ;
int *reader;
int *manual_id_entry_flag;


manual_id_entry_flag = (int *)dpGetPointer("manual_id_entry_flag", INT_P);
if (manual_id_entry_flag == NULL) {
   /* OCR being used - exit this routine */
   return;
}


/* Wafer ID's will be entered from keyboard, turn off the
   id_reader from the prober or else ktxe will replace the
   keyed in ID's with prober-read ID's */
reader = (int *)dpGetPointer("prober_has_id_reader", INT_P);
if (reader != NULL) {
   *reader = 0;  /* Reader OFF */
}


/* Put the GUI up to ask for Wafer ID's */
tmp = ( long *)dpGetPointer( "current_slot_list", LONG_P ) ;
slot = ( slot_list_t *)*tmp;

/* Create a wafer list based upon the slot list */
while( slot != NULL )
{
   	WAFER *newWafer ;

   	newWafer = CreateNewWafer() ;
   	if ( strlen( slot->wafer_id ) != 0 )
   	   	strcpy( newWafer->id, slot->wafer_id ) ;
   	else
   	   	strcpy( newWafer->id, slot->slot ) ; 

 /*  	printf( "Slot #%s  plan <%s>\n", slot->slot, slot->wpfname ) ;*/

   	newWafer->boat = 1 ;
   	newWafer->slot = atoi( slot->slot ) ;
   	newWafer->next = NULL ;
   	newWafer->prev = wafer ;
   	if ( wafer != NULL )
   	   	wafer->next = newWafer ;
   	else
   	   	First = newWafer ;

   	wafer = newWafer ;
   	slot = slot->next ;
}

/* Display wafer id dlg window so operator can enter ids */
wafer = First ;
dlg_ret = WfrIdsDlg( &wafer, 1, &total_wafers ) ;
if ( DLG_ABORT == dlg_ret )
   	exit( KI_ABORT ) ;

/* Copy entered wafer ids into slot wafer id position */
slot = ( slot_list_t *)*tmp;
while( wafer != NULL )
{
   	WAFER *cur ;

/*   	printf( "WAFER id: <%s>\n", wafer->id ) ;*/

   	if ( slot != NULL )
   	{
   	   	if ( wafer->slot == atoi( slot->slot ) )
   	   	   	strcpy( slot->wafer_id, wafer->id ) ;
   	   	slot = slot->next ;
   	}

   	cur = wafer ;
   	wafer = wafer->next ;

   	/* don't forget to free the memory we used earlier */
   	free( cur ) ;
}

/* USRLIB MODULE END  */
} 		/* End GetWaferIdsFromOperator.c */

