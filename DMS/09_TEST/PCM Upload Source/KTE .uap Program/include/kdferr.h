/*			k d f e r r . h			*/
/*************************************************************************

       COPYRIGHT (C) 1992  by  KEITHLEY INSTRUMENTS, INC.
       Cleveland, Ohio

       This software is furnished under a license and may
       be used and copied only in accordance with the terms
       of such license, and with the inclusion of the above
       COPYRIGHT notice.  This software or any other copies
       thereof may not be provided or otherwise made
       available to any other person.  No title to and
       ownership of the software is hereby transferred.
       The information in this software is subject to
       change without notice, and should not be construed
       as a commitment by KEITHLEY INSTRUMENTS, INC.

       KEITHLEY assumes no responsibility for the use or
       reliability of its software on equipment which is
       not supplied by KEITHLEY.

**************************************************************************

 File:     $Source: /cm/test/build/S600/v420/COMMON/RCS/kdferr.h,v $
 Current $Revision: 1.2 $
 Current    $State: REL $
 Last Rev    $Date: 1999/11/10 18:15:07 $

 Change       $Log: kdferr.h,v $
 Change       Revision 1.2  1999/11/10 18:15:07  williamson
 Change       PR 11038  Added usr tag support
 Change
 Change       Revision 1.1  1993/04/26 18:19:17  SCMO
 Change       Initial revision
 Change
 * Revision 1.1  1993/04/16  18:24:21  williams
 * Initial revision
 *
...............................................................................

Function:  This is the error file for the pr_sig_error error handling
    routine.  The error numbers match up with the error messages in the
    sun_kdfmsg.msg file.
.............................................................................*/

#define OK		  1  /* Successfull completeion */


/* .base 10 !input proceedures and function calling*/
#define EMPTY_LOTID    -10  /* Empty Lot Name */
#define EMPTY_CODEID   -11
#define BAD_LOTADD     -12  /* Lotadd was not CREATELOT or APPENDLOT */
#define INVALID_CHAR   -13  /* Found an invalid character in a field */
#define BAD_CALL_ORDER -14  /* Function call not made in the proper order */
			      /* (ie. PutWafer called before PutLot) */

/* .base 20   ! File handling */
#define ERR_OPEN_FILE  	-20  /* Error opening datafile */
#define ERR_WRITE_FILE 	-21  /* Error writing data to file */
#define ERR_CLOSE_FILE 	-22  /* Error closing file */
#define BAD_DATAFILE	-23  /* Data file corrupt */
#define RENAME_ERROR	-24  /* Error renaming file */
#define ILLEGAL_TAG     -25  /* illegal character in tag name */
#define TAG_STR_LEN_ERR -26  /* tag string too long */
#define TAG_NAM_LEN_ERR -27  /* tag name too long */

/* .base 30   !memory handling */
#define OUT_OF_MEMORY  	-30  /* Out of Memory error */

/* .base 200  !Lot Summary errors */
#define NO_WAFERS      	-200 /* Lot does not contain any wafers */ 
