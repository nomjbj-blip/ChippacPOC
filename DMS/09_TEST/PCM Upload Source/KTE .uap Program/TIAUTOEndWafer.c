/* USRLIB MODULE INFORMATION

	MODULE NAME: TIAUTOEndWafer
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <string.h>
#include "COM_usrlib.h"
#define TI_TW_ACK 6
extern int auto_site_count;
	END USRLIB MODULE INFORMATION
*/
/* USRLIB MODULE HELP DESCRIPTION

	END USRLIB MODULE HELP DESCRIPTION */
/* USRLIB MODULE PARAMETER LIST */
#include <stdio.h>
#include <string.h>
#include "COM_usrlib.h"
#define TI_TW_ACK 6
extern int auto_site_count;

void TIAUTOEndWafer(  )
{
/* USRLIB MODULE CODE */
FILE  *wfd, *rfd;
char recvbuf[256];


/* Get file pointer from the data pool */
wfd = (FILE * ) dpGetPointer( "TI_auto_wfd", LONG_P );
#ifdef READPIPE
rfd = (FILE * ) dpGetPointer( "TI_auto_rfd", LONG_P );
#endif

if ( wfd != NULL )
{
     	TIAUTOPutSite();
     	fputs("E\n", wfd );
      	fclose( wfd );
#ifdef READPIPE
     	fgets( recvbuf, 80, rfd );
     	if ( recvbuf[0] != TI_TW_ACK )
	     {
		     	     KTXEErrorMsg( "TIAUTOEndWafer: wafer end message acknowledge error.\n" );
     	     	return;
	     }
     	fclose( rfd );
#endif
     	/* NULL file pointer in data pool */
     	wfd = (FILE * ) dpAddPointer( "TI_auto_wfd", LONG_P, NULL );
#ifdef READPIPE
     	rfd = (FILE * ) dpAddPointer( "TI_auto_rfd", LONG_P, NULL );
#endif

}

/* Reset site counter */
auto_site_count = 0;
/* USRLIB MODULE END  */
} 		/* End TIAUTOEndWafer.c */

