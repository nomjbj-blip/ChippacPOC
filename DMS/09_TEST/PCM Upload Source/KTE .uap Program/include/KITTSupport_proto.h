/* KITTSupport function prototype and KITT header file */

/* COPY OF USRLIB SETTINGS INFORMATION */
/* [VISIBILITY] */
/* HIDDEN=0 */
/* [DEPENDENCIES] */
/* LIBS= */
/* OBJECTS= */
/* HEADERS= */
/* [NON-KULT SEARCH PATH] */
/* PATHS= */

/**************************************************************************
 *
 *       COPYRIGHT (C) 1994  by  KEITHLEY INSTRUMENTS, INC.
 *       Cleveland, Ohio
 *
 *       This software is furnished under a license and may
 *       be used and copied only in accordance with the terms
 *       of such license, and with the inclusion of the above
 *       COPYRIGHT notice.  This software or any other copies
 *       thereof may not be provided or otherwise made
 *       available to any other person.  No title to and
 *       ownership of the software is hereby transferred.
 *       The information in this software is subject to
 *       change without notice, and should not be construed
 *       as a commitment by KEITHLEY INSTRUMENTS, INC.
 *
 *       KEITHLEY assumes no responsibility for the use or
 *       reliability of its software on equipment which is
 *       not supplied by KEITHLEY.
 *
 **************************************************************************/



/* USRLIB MODULE INFORMATION

	MODULE NAME: PutUserDataLogging
	MODULE RETURN TYPE: void 
	ARGUMENTS:
		lot,	long *,	Input
		wafer,	long *,	Input
		site,	long *,	Input
		result,	long *,	Input
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "kdf.h"
	END USRLIB MODULE INFORMATION
*/
void PutUserDataLogging(long * lot, long * wafer, long * site, long * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PutUserDataLoggingKTXE
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "kdf.h"
#include "COM_usrlib.h"
#include "ksox_def.h"
#include "ktxe_types.h"
	END USRLIB MODULE INFORMATION
*/
void PutUserDataLoggingKTXE();


