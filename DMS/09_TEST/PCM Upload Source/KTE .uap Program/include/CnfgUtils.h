/* CnfgUtils.h*/
/*************************************************************************

       COPYRIGHT (C) 1996  by  KEITHLEY INSTRUMENTS, INC.
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

 File:     $Sourced$
 Current $Revision: 1.3 $
 Current    $State: REL $
 Last Rev    $Date: 1997/01/09 13:13:17 $

 Change       $Log: CnfgUtils.h,v $
 Change       Revision 1.3  1997/01/09 13:13:17  williamson
 Change       Removed GetTesterHostName reference
 Change
 * Revision 1.2  1996/12/12  21:24:20  williamson
 * Changed name of argument for better clarity
 *
 * Revision 1.1  1996/12/12  16:49:17  williamson
 * Initial revision
 *
 * Revision 1.1  1996/12/10  19:46:47  williamson
 * Initial revision
 *

...............................................................................

Prototypes for Utility programs for the S600 configuration file.

.............................................................................*/



char* GetQMO( char *iniFileName ) ;
char* GetIPAddress( char *iniFileName ) ;
enum BOOLN IsScramInstalled( char *iniFileName ) ;
char* GetSystemName( char *iniFileName ) ;

