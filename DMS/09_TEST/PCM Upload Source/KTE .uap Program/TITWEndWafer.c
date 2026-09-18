/* USRLIB MODULE INFORMATION

	MODULE NAME: TITWEndWafer
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <string.h>
#include "COM_usrlib.h"
#define TI_TW_ACK 6
	END USRLIB MODULE INFORMATION
*/
/* USRLIB MODULE HELP DESCRIPTION

	END USRLIB MODULE HELP DESCRIPTION */
/* USRLIB MODULE PARAMETER LIST */
#include <stdio.h>
#include <string.h>
#include "COM_usrlib.h"
#define TI_TW_ACK 6

void TITWEndWafer(  )
{
/* USRLIB MODULE CODE */
FILE  *wfd, *rfd;
char recvbuf[256];


/* Get file pointer from the data pool */
rfd = (FILE * ) dpGetPointer( "TI_tw_rfd", LONG_P );
wfd = (FILE * ) dpGetPointer( "TI_tw_wfd", LONG_P );

if ( wfd != NULL )
{
/*	fputs("E", wfd ); */ /*jsm TW1.4 change */
	fputs("E PROBER=1", wfd );  /*jsm TW1.4 change */
	fclose( wfd );
	fgets( recvbuf, 80, rfd );
	if ( recvbuf[0] != TI_TW_ACK )
	{
		KTXEErrorMsg( "TITWEndWafer: wafer end message acknowledge error.\n" );
		return;
	}
     	fclose( rfd );
}
/* NULL file pointer in data pool */
rfd = (FILE * ) dpAddPointer( "TI_tw_rfd", LONG_P, NULL );
wfd = (FILE * ) dpAddPointer( "TI_tw_wfd", LONG_P, NULL );

/* USRLIB MODULE END  */
} 		/* End TITWEndWafer.c */

