/* USRLIB MODULE INFORMATION

	MODULE NAME: TIAUTOPutWafer
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <string.h>
#include <stdlib.h>
#include <unistd.h>
#include "ktxe_types.h"
#include "COM_usrlib.h"
#define TI_MAX_PROGRAM_ID_LENGTH 30
#define TI_MAX_LOT_ID_LENGTH 8
#define TI_TW_ACK 6
	END USRLIB MODULE INFORMATION
*/
/* USRLIB MODULE HELP DESCRIPTION

	END USRLIB MODULE HELP DESCRIPTION */
/* USRLIB MODULE PARAMETER LIST */
#include <stdio.h>
#include <string.h>
#include <stdlib.h>
#include <unistd.h>
#include "ktxe_types.h"
#include "COM_usrlib.h"
#define TI_MAX_PROGRAM_ID_LENGTH 30
#define TI_MAX_LOT_ID_LENGTH 8
#define TI_TW_ACK 6

void TIAUTOPutWafer(  )
{
/* USRLIB MODULE CODE */
FILE  *wfd, *rfd;
char *wfd_pipe = NULL, *rfd_pipe = NULL;
char recvbuf[256];
char progstr[256];
int status;
WAFER *wafer = NULL;
char *prober_wafer_id = NULL;
char tmp_wafer_id[41];
cpf_info_t *cpf_info;
char tmpprogram[MAXFILENAMELENGTH];     	/* */
char tmppath[MAXFILENAMELENGTH];     	/* scratch for path */
char tmpext[MAXFILENAMELENGTH];     	/* scratch for file extension */
char *lotid;
char slid[16];     	/* for SLID= <value> */
WDFRec *wdfptr;
int tmprot;
char rot[2];
int auto_online;

/* Check if AUTO is online */
auto_online = ( int )dpGetPointer( "TI_AUTO_ONLINE", INT );
if ( auto_online  == NULL ) return; 


/* Get file pointer from the data pool */
#ifdef READPIPE
rfd = (FILE * ) dpGetPointer( "TI_auto_rfd", LONG_P );
#endif

wfd = (FILE * ) dpGetPointer( "TI_auto_wfd", LONG_P );

/* If file pointers are not null, mark end of wafer */
#ifdef READPIPE
if ( ( wfd != NULL ) && ( rfd != NULL ) )
#else

if ( ( wfd != NULL )  )
#endif
{
     	fputs("E", wfd );
     	fclose( wfd );
#ifdef READPIPE
     	fgets( recvbuf, 80, rfd );
     	fclose( rfd );
#endif
}

/* Get testware pipe names */
wfd_pipe = getenv( "TI_AUTO_W_PIPE" );
#ifdef READPIPE
rfd_pipe = getenv( "TI_AUTO_R_PIPE" );
if ( ( wfd_pipe == NULL ) || (rfd_pipe == NULL ) )
#else
if ( ( wfd_pipe == NULL ) )
#endif
{
     	KTXEErrorMsg( "TIAUTOPutWafer:  Error TI_AUTO_W_PIPE  or TI_AUTO_R_PIPE not set.\n" );
     	status = dpAddPointer( "TI_auto_rfd", LONG_P, rfd );
     	status = dpAddPointer( "TI_auto_wfd", LONG_P, wfd );
     	return;
}

/* Check for access to pipes */
if ( access( wfd_pipe,  F_OK ) != 0 )
{
     	KTXEErrorMsg("TIAUTOPutWafer: Error can not open pipe %s for write access.\n", wfd_pipe );
     	return;
}

#ifdef READPIPE
if ( access( rfd_pipe,  F_OK ) != 0 )
{
     	KTXEErrorMsg("TIAUTOPutWafer: Error can not open pipe %s for read access.\n", rfd_pipe );
}
#endif

/* Open pipes */
if ( (wfd = fopen( wfd_pipe, "a+" )) == NULL )
{
     	KTXEErrorMsg("TIAUTOPutWafer: Error can not open pipe %s for write.\n", wfd_pipe );
     	return;
}

#ifdef READPIPE
if ( ( rfd = fopen( rfd_pipe, "r" )) == NULL )
{
     	KTXEErrorMsg("TIAUTOPutWafer:  Error can not open  pipe %s for read.\n", rfd_pipe );
     	return;
}
#endif

/* Generate Starting string */
strcpy( progstr,"S PROG=" );

/* Create PROG string */
cpf_info = ( cpf_info_t * ) dpGetPointer( "cpf_info", LONG_P );
GetPathFileExt( cpf_info->cpfname, tmppath, tmpprogram, tmpext );
strncat( progstr, tmpprogram, TI_MAX_PROGRAM_ID_LENGTH );

/* Create LOT string */
strcat( progstr, " LOT=" );
lotid = ( char * ) dpGetPointer( "lotid", CHAR_P );
strncat( progstr, lotid, TI_MAX_LOT_ID_LENGTH );

/* Create SLID string*/
strcat( progstr, " SLID=" );
prober_wafer_id = ( char * ) dpGetPointer( "prober_wafer_id", CHAR_P );

if ( prober_wafer_id != NULL )
{
     	/* use prober generate string */
        strcpy( tmp_wafer_id, prober_wafer_id );
}
else
{
        /* use wafer id in wafer structure */
        wafer = ( WAFER * ) dpGetPointer( "wafer", LONG_P );
        if ( wafer == NULL )
        {
                KTXEErrorMsg( "TIAUTOPutWafer: wafer structure pointer not found.\n" );
                return;
        }
        if ( wafer->id == NULL )
        {
                KTXEErrorMsg( "TIAUTOPutWafer: wafer id is NULL.\n" );
                return;
        }
 
        strcpy( tmp_wafer_id, wafer->id );

}

/* REMOVE THE LINE BELOW for real wafer ids */
/*strcpy( tmp_wafer_id, "E-1234567-89-01" ); */
/*strcpy( tmp_wafer_id, "C-9993355-01" ); */
strcpy( slid, tmp_wafer_id );
strcat( progstr, slid );

/* Create ROT string */
strcat( progstr, " ROT=" );
wdfptr = ( WDFRec * ) dpGetPointer( "wdfptr", LONG_P );
switch( wdfptr->flat )
{
     	case 0:
     	     	tmprot = 0;     	/* down */
     	     	break;
     	case 90:
     	     	tmprot = 3;     	/* left */
     	     	break;
     	case 180:
     	     	tmprot = 2;     	/* top */
     	     	break;
     	case 270:
     	     	tmprot = 1;     	/* right */
     	     	break;
     	default:
     	     	tmprot = 0;
}
sprintf( rot, "%d\n", tmprot );
strcat( progstr, rot );

fputs(progstr, wfd );
fflush( wfd );

#ifdef READPIPE
fgets( recvbuf, 80, rfd );
if ( recvbuf[0] != TI_TW_ACK )
{
     	KTXEErrorMsg( "TIAUTOPutWafer: wafer start message acknowledge error.\n" );
     	return;
}
#endif

/* Save file pointer in data pool for logging data */
#ifdef READPIPE
status = dpAddPointer( "TI_auto_rfd", LONG_P, rfd );
#endif
status = dpAddPointer( "TI_auto_wfd", LONG_P, wfd );
/* USRLIB MODULE END  */
} 		/* End TIAUTOPutWafer.c */

