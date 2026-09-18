
/**************************************************************************
 *
 *       COPYRIGHT (C) 2000  by  KEITHLEY INSTRUMENTS, INC.
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
 **************************************************************************

 ki_gem.h: This header file is used by both KTE and GEMS executables.
	   Changes in this file will trigger both KTE and GEMS builds.
	   Use "ktpgem.h" for SECS/GEM specific definitions.
 
 File:     $Source: /cm/test/build/S600/v420/COMMON/RCS/ki_gem.h,v $
 Current   $Revision: 1.2 $
 Curent    $State: REL $
 Last Rev  $Date: 2000/06/22 14:20:37 $

 * $Log: ki_gem.h,v $
 * Revision 1.2  2000/06/22 14:20:37  williamson
 * Added define of KI_Strncpy...
 *
 * Revision 1.1  2000/06/21 18:52:28  psuwondo
 * Initial revision
 *
 *
 **************************************************************************/

#ifndef _KI_GEM_H
#define _KI_GEM_H

/* 
 * SECS/GEM Alarms and abort codes.
 * Add new entries at the bottom of list only! 
 */
enum ALARMID
{
    KTPConfigError,
    KTPHardWError,
    KTPSoftWError,  
    PRBConfigError,
    PRBHardWError,
    PRBSoftWError,
    KTPDataOverflow,
    KTPDataSetError,
    KTPProberError,
    KTPNormalAbort,
    KTPNormalStop
    /* --> Add new entries here <--*/

}; 

#ifndef KI_Strncpy
#define KI_Strncpy(d,s,l) (void) strncpy(d,s,l-1); *(d+l-1) = (char) NULL;
#endif




#endif	/* _KI_GEM_H */
