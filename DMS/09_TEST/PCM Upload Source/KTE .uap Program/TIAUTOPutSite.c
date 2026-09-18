/* USRLIB MODULE INFORMATION

	MODULE NAME: TIAUTOPutSite
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <string.h>
#include <ktxe_types.h>
#include <COM_usrlib.h>
#define TI_TW_ACK 6
#define TI_AUTO_SITE_MAX 9
char auto_result_buffer[2048];
int auto_result_count = 0;
int auto_site_count = 0;
char auto_site_id[SITE_ID_LENGTH];
	END USRLIB MODULE INFORMATION
*/
/* USRLIB MODULE HELP DESCRIPTION

	END USRLIB MODULE HELP DESCRIPTION */
/* USRLIB MODULE PARAMETER LIST */
#include <stdio.h>
#include <string.h>
#include <ktxe_types.h>
#include <COM_usrlib.h>
#define TI_TW_ACK 6
#define TI_AUTO_SITE_MAX 9
char auto_result_buffer[2048];
int auto_result_count = 0;
int auto_site_count = 0;
char auto_site_id[SITE_ID_LENGTH];

void TIAUTOPutSite(  )
{
/* USRLIB MODULE CODE */

FILE  *wfd, *rfd;
SITE *site = NULL;

char recvbuf[256];
char datastr[2048];

/* limit sites logged for AUTO */
if ( auto_site_count > TI_AUTO_SITE_MAX ) 
{
     	/* only used by call through EndWafer if site max hit */
     	return;
}
/* Get file pointer from the data pool */
wfd = (FILE * ) dpGetPointer( "TI_auto_wfd", LONG_P );
if ( wfd == NULL ) 
     	/* Logging disabled to AUTO */
     	return;

/* Get file pointer from the data pool */
#ifdef READPIPE
rfd = (FILE * ) dpGetPointer( "TI_auto_rfd", LONG_P );
#endif

site = (SITE *) dpGetPointer("site", LONG_P );

if ( auto_result_count > 0 ) 
{
     	/* write results to pipe */
     	sprintf( datastr , "D SITE=%s %s\n", auto_site_id, auto_result_buffer );

      	fputs( datastr, (FILE *)wfd );
      	fflush( (FILE *)wfd );
#ifdef READPIPE
      	fgets( recvbuf, 80, (FILE *)rfd );
     	if ( recvbuf[0] != TI_TW_ACK )
     	{
     	     	KTXEErrorMsg( "TIAUTOPutSite: data message acknowledge error.\n" );
     	     	return;
     	}
#endif
}
/* clear buffer for next site */
strcpy( auto_result_buffer, "" );
auto_result_count = 0;

/* save the current site id for logging later */
strcpy( auto_site_id, site->id );

auto_site_count++;

/* limit sites logged for AUTO */
if ( auto_site_count > TI_AUTO_SITE_MAX ) 
{
     	TIAUTOEndWafer();
}
/* USRLIB MODULE END  */
} 		/* End TIAUTOPutSite.c */

