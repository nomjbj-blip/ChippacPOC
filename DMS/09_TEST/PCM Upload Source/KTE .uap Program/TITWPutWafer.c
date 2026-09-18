/* USRLIB MODULE INFORMATION

	MODULE NAME: TITWPutWafer
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

void TITWPutWafer(  )
{
/* USRLIB MODULE CODE */
FILE  *wfd, *rfd;
char *wfd_pipe = NULL, *rfd_pipe = NULL;
char recvbuf[256];
char progstr[256];
int status;
int i;
WAFER *wafer = NULL;
char *prober_wafer_id = NULL;
char tmp_wafer_id[41];
cpf_info_t *cpf_info;
char tmpprogram[MAXFILENAMELENGTH];         /* */
char tmppath[MAXFILENAMELENGTH];         /* scratch for path */
char tmpext[MAXFILENAMELENGTH];         /* scratch for file extension */
char *lotid;
char slid[16];         /* for SLID= <value> */
WDFRec *wdfptr;
int tmprot;
char rot[2];

int tw_online;

/* Check if TW is online */
tw_online = ( int )dpGetPointer( "TI_TW_ONLINE", INT );
if ( tw_online  == NULL ) return; 

/* Get file pointer from the data pool */
rfd = (FILE * ) dpGetPointer( "TI_tw_rfd", LONG_P );
wfd = (FILE * ) dpGetPointer( "TI_tw_wfd", LONG_P );

/* If file pointers are not null, mark end of wafer */
if ( ( wfd != NULL ) && ( rfd != NULL ) )
{
         fputs("E", wfd );
         fclose( wfd );
         fgets( recvbuf, 80, rfd );
         fclose( rfd );
}

/* Get testware pipe names */
wfd_pipe = getenv( "TI_TW_W_PIPE" );
rfd_pipe = getenv( "TI_TW_R_PIPE" );
if ( ( wfd_pipe == NULL ) || (rfd_pipe == NULL ) )
{
         KTXEErrorMsg( "TITWPutWafer:  Error TI_TW_W_PIPE  or TI_TW_R_PIPE not set.\n" );
         status = dpAddPointer( "TI_tw_rfd", LONG_P, rfd );
         status = dpAddPointer( "TI_tw_wfd", LONG_P, wfd );
         return;
}

/* Check for access to pipes */
if ( access( wfd_pipe,  F_OK ) != 0 )
{
         KTXEErrorMsg("TITWPutWafer: Error can not open pipe %s for write access.\n", wfd_pipe );
}

if ( access( rfd_pipe,  F_OK ) != 0 )
{
         KTXEErrorMsg("TITWPutWafer: Error can not open pipe %s for read access.\n", rfd_pipe );
}

/* Open pipes */
if ( (wfd = fopen( wfd_pipe, "a+" )) == NULL )
{
         KTXEErrorMsg("TITWPutWafer: Error can not open pipe %s for write.\n", wfd_pipe );
         return;
}
if ( ( rfd = fopen( rfd_pipe, "r" )) == NULL )
{
         KTXEErrorMsg("TITWPutWafer:  Error can not open  pipe %s for read.\n", rfd_pipe );
         return;
}

/* Generate Starting string */
/*strcpy( progstr,"S PROG=" ); */     /*jsm TW 1.4 change */
strcpy( progstr,"S PROBER=1 PROG=" );     /*jsm TW 1.4 change */

/* Create PROG string */
cpf_info = ( cpf_info_t * ) dpGetPointer( "cpf_info", LONG_P );
GetPathFileExt( cpf_info->cpfname, tmppath, tmpprogram, tmpext );

/* Remove _R from tmpprogram, if necessary - jwp 5/21/99 */
i=0;
while((tmpprogram[i]!='\0')&&(i<TI_MAX_PROGRAM_ID_LENGTH)) {
   if ((tmpprogram[i]=='_')||(tmpprogram[i]=='-')) {
      tmpprogram[i]='\0';
      break;
   }
   i++;
}

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
                KTXEErrorMsg( "TITWPutWafer: wafer structure pointer not found.\n" );
                return;
        }
        if ( wafer->id == NULL )
        {
                KTXEErrorMsg( "TITWPutWafer: wafer id is NULL.\n" );
                return;
        }
 
        strcpy( tmp_wafer_id, wafer->id );

}

/* REMOVE THE LINE BELOW for real wafer ids */
/*strcpy( tmp_wafer_id, "E-1234567-89-01" ); */

strcpy( slid, tmp_wafer_id );
strcat( progstr, slid );

/* Create ROT string */
strcat( progstr, " ROT=" );
wdfptr = ( WDFRec * ) dpGetPointer( "wdfptr", LONG_P );
switch( wdfptr->flat )
{
         case 0:
                  tmprot = 0;         /* down */
                  break;
         case 90:
                  tmprot = 3;         /* left */
                  break;
         case 180:
                  tmprot = 2;         /* top */
                  break;
         case 270:
                  tmprot = 1;         /* right */
                  break;
         default:
                  tmprot = 0;
}
sprintf( rot, "%d", tmprot );
strcat( progstr, rot );

fputs(progstr, wfd );
fflush( wfd );
fgets( recvbuf, 80, rfd );
if ( recvbuf[0] != TI_TW_ACK )
{
         KTXEErrorMsg( "TITWPutWafer: wafer start message acknowledge error.\n" );
         return;
}

/* Save file pointer in data pool for logging data */
status = dpAddPointer( "TI_tw_rfd", LONG_P, rfd );
status = dpAddPointer( "TI_tw_wfd", LONG_P, wfd );
/* USRLIB MODULE END  */
} 		/* End TITWPutWafer.c */

