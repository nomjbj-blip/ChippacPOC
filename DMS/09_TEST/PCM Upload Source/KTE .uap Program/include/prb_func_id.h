/* prb_func_id.h


*/
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

 File:     $Source: /cm/test/build/S600/v420/COMMON/RCS/prb_func_id.h,v $
 Current $Revision: 1.14 $
 Current    $State: REL $
 Last Rev    $Date: 2000/09/05 19:43:36 $

 Change       $Log: prb_func_id.h,v $
 Change       Revision 1.14  2000/09/05 19:43:36  rybka
 Change       pr13245 add PrGetProduct to prober lib
 Change
 Change       Revision 1.13  2000/07/14 17:15:17  rybka
 Change       added new function PrCassetteMask
 Change
 Change       Revision 1.12  2000/07/11 14:20:48  rybka
 Change       added new function for needle cleaning
 Change
 Change       Revision 1.11  2000/01/25 20:43:45  rybka
 Change       added 2 new functions 66 & 67
 Change
 Change       Revision 1.10  1999/09/01 14:05:18  rybka
 Change       added SMIF specific constants
 Change
 Change       Revision 1.9  1998/12/01 20:22:40  rybka
 Change       added # 62 & 63 set/query chuck temp
 Change
 Change       Revision 1.8  1998/06/24 18:57:48  djohnson
 Change       added wrapper to only load once
 Change
 Change       Revision 1.7  1998/06/24 12:22:03  rybka
 Change       moved < extern PRBFUNCS xref_prb_func[]; defined in PRBCOM/prb_dec.c
 Change       to prb_extern.h PR5286
 Change
 Change       Revision 1.6  1998/06/22 13:40:03  rybka
 Change       moved the init of array of structs to prb_dec.
 Change
 Change       Revision 1.5  1998/06/18 17:52:11  rybka
 Change       PR5286 added a table to xref the text prober name with the function constant
 Change       this is used in KTXEAddIn/KTXEprbErrHdlr.c
 Change
 Change       Revision 1.4  1998/03/18 15:04:04  rybka
 Change       added PRWRITEREADSRQ # 61
 Change
 Change       Revision 1.3  1998/02/06 21:33:59  rybka
 Change       added functions 55-60 for SMIF apps
 Change
 Change       Revision 1.2  1997/04/22 13:02:39  williamson
 Change       Moved from S600 projcom area
 Change
 Change       Revision 1.2  1997/02/07 22:34:56  rybka
 Change       added support for PrAbsMove
 Change
 * Revision 1.1  1996/10/25  17:19:31  witzke
 * Initial revision
 *
 * Revision 1.2  1996/03/12  18:50:21  jain
 * PR1580 fixed the #define numbers to match the index of the prober cmds
 * array in KSOX--dispatch.c.  This will fix the problem of KITT displaying
 * incorrect prober drivers in the Prober Commnads window in KITT.  Also
 * add the #defines for the new EG functions.
 *
 * Revision 1.1  1993/02/06  17:39:49  beecher
 * Initial revision
 *
...............................................................................
*/

#ifndef _prb_func_id
#define _prb_func_id 1
#define PRAUTOALIGN	1
#define PRBEGINPROBE	2
#define PRCASSETTEMAP	3
#define PRCHECKOPTIONS	4
#define PRCHUCK		5
#define PRCLEARPIPELINE	6
#define PRDISABLETRANSLOG	7
#define PRENABLETRANSLOG	8
#define PRERROR		9
#define PRGETNXTWAFER	10
#define PRGETWAFER	11
#define PRINIT		12
#define PRINK		13
#define PRLEARN		14
#define PRLOAD		15
#define	PRLOADFAILURE	16
#define PRLOADPRODUCT	17
#define PRMOVE		18
#define PRMOVNXT	19
#define PROFFLINE	20
#define PRPROFILE	21
#define PRPUTNXTSLOT	22
#define PRPUTWAFER	23
#define PRREADID	24
#define PRRELMOVE	25
#define	PRRELOAD	26
#define PRRELRETURN	27
#define PRREQWAFERINFO	28
#define PRRETURNERRORMESSAGE	29
#define PRSERIALPOLL	30
#define PRSETDIAM	31
#define PRSETDIESIZE    32
#define PRSETFLAT	33
#define PRSETMATRIX	34
#define PRSETMODE	35
#define PRSETMPROBE	36
#define PRSETPIPELINE	37
#define PRSETQUADRANT	38
#define PRSETREFDIE     39
#define PRSETSKIPDIE	40
#define PRSETSLOTSTATUS 41
#define PRSETTIME	42
#define PRSETUNITS      43
#define PRSSLEARN	44
#define PRSSLOCATION	45
#define PRSSMOVE	46
#define PRSSMOVNXT	47
#define PRSTATUS	48
#define PRUNLOAD	49
#define PRWAIT		50
#define PRWRITEREAD	51
#define PRZPARAMS	52
#define PRZTRAVEL	53
#define PRABSMOVE	54
#define PRSMIFLOCK	55
#define PRSMIFLOCKSTATUS	56
#define PRPROBERSTATUS	57
#define PRSENSEWAFER	58
#define PRSTOP		59
#define PRSTART		60
#define PRWRITEREADSRQ	61
#define PRSETCHUCKTEMP	62
#define PRQUERYCHUCKTEMP	63
#define PRSMIFSTATUS	64
#define PRSMIFCLAMP	65
#define PRLOWERBOAT	66
#define PRCLEARALL	67
#define PRNEEDLECLEAN	68
#define PRCASSETTEMASK	69
#define PRGETPRODUCT	70


#define PRMAXFUNCIDS	70 /*this must equal the last funct value */

/* the PRBFUNCS typedef is used in KTXEAddIn/KTXEPrbErrHdlr
 to determine, based on an input string function name "PrLoad",
 the constant associated with it, PRLOAD.  This will allow users
 to define their own prober error handled. 
*/

typedef struct {
	char c_func_name[64];
	int i_func_id;
} 
PRBFUNCS; 

#endif
