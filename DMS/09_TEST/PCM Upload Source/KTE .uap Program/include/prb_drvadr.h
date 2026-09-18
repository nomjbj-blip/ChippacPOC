/* prb_drvadr.h */
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
$REVISION$

...............................................................................

 Function: prober driver address function prototypes
 

 16-dec-97 rybka
 The following function prototypes were movedform prb_extern. to prb_drvadr.h.
 The NT development required this because of issues with IMPORT/EXPORT.
		getdrvadr, putdrvadr, getdsptab

 Also the externs for global variables were moved from prb.h to prb_extern.h
  
.............................................................................*/


int getdsptab( int );
int putdrvadr( int prober_id, int prober_function_id, int (*drvadr)() );
int (*getdrvadr( int teststation, int prober_function_id))();
