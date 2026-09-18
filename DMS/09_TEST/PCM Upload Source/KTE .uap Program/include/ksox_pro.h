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
 **************************************************************************
 File:     $Source: /cm/test/build/S600/v420/COMMON/RCS/ksox_pro.h,v $
 Current $Revision: 1.24 $
 Curent     $State: REL $
 Last Rev    $Date: 1999/10/07 20:01:52 $

 * $Log: ksox_pro.h,v $
 * Revision 1.24  1999/10/07 20:01:52  williamson
 * PR 10580 added prototypes for div by zero error code support routines
 *
 * Revision 1.23  1999/07/12 20:22:50  witzke
 * PR09314 added remove_all_usrlibs
 *
 * Revision 1.22  1999/03/18 15:23:48  witzke
 * Changed ExecKTM prototype to be int since we are returning a status now.
 *
 * Revision 1.21  1998/10/13 14:09:34  witzke
 * PR8219 Added GetRealProtoName
 *
 * Revision 1.20  1998/05/08 15:29:01  williamson
 * Removed proto for RemoveWhiteSpace since it was incorrect and not needed...
 *
 * Revision 1.19  1998/04/30 14:11:50  williamson
 * Added prototypes for GetTSdelimiter and SetTSdelimiter functions
 *
 * Revision 1.18  1998/03/05 14:03:42  williamson
 * PR 6061  Added support for TSF  data
 *
 Change       Revision 1.17  1998/02/02 19:35:35  williamson
 Change       Changed ExecKTM to be type void to clean up purify warnings...
 Change
 Change       Revision 1.16  1997/09/24 17:38:32  moore
 Change       Changed proto for OpenUsrlib back to int
 Change
 Change       Revision 1.15  1997/09/24 17:34:04  moore
 Change       Changed prototype for OpenUsrlib from int to void
 Change
 Change       Revision 1.14  1997/04/15 18:16:34  jain
 Change       PR4151 added the second parameter "status" to the GetReusableParameter.
 Change
 Change       Revision 1.13  1997/04/04 20:34:38  jain
 Change       changes for kte3.2 for s400
 Change
 Change       Revision 1.12  1997/03/20 15:19:17  jain
 Change        PR4019 added another parameter to IsInvalidFloat function to distinguish between
 Change       float and double.
 Change
 Change       Revision 1.11  1996/11/18 22:24:26  jain
 Change       PR2672 changes for the s600.
 Change
 * Revision 1.10  1996/07/31  19:58:15  jain
 * PR1619 added prototype for CheckValidComment function.
 *
 * Revision 1.9  1996/05/08  15:11:39  witzke
 * PR1293 updated for kte30 modified logtokdf prototype
 *
 * Revision 1.8  1996/02/14  21:18:23  jain
 * modified the prototype for ExecKTM and ExecSequence to accomodate pcf files
 *
 * Revision 1.7  1995/08/10  13:38:05  tufte
 * pr520 etc -- added argument to add_constant
 *
 * Revision 1.6  1995/07/18  14:30:40  tufte
 * pr00549
 * added user argument to add_select prototype
 *
 * Revision 1.5  1995/06/29  18:59:00  szanto
 * PR305 - Unpublicized function exec_str(...).  Found a much more elegant way`
 * 	of allowing KITT rto detect 
 * 	errors.
 *
 * Revision 1.3  1995/02/28  22:34:45  szanto
 * SPR95U070 Changed prototype for add_constant, now takes value of global field as well
 *
 * Revision 1.2  1994/08/04  13:36:46  szanto
 * Moved items into COM_usrlib.h so they would exist in only one place.  See Version 1.1 of
 * COM_usrlib.h in COMMON for details.
 *
 * Revision 1.1  1994/06/17  16:59:20  szanto
 * Initial revision
 *
 */

#ifndef _KSOX_PRO_H
#define _KSOX_PRO_H

/* from arrays.c */
#ifdef LPTLIB600
void		add_array ( char *array_name, int numpoints, double *arrayp );
#else
void		add_array ( char *array_name, int numpoints, float *arrayp );
#endif
in_array_info_t	*GetArrayByName ( char *array_name );

/* from dispatch.c */
void	*GetReusableParm ( char *search_name, int *status );

/* from exec.c */
#if defined LPTLIB || defined LPTLIB600
int 	do_tstsel(long teststation);
#endif /*LPTLIB*/
int 	need_execut( void );
void	trunc_at_space ( char *str );
char	*ExecSequence ( char *testseq, int syntax, int hardwareoff, int
			timedsply, int loop, int tststation, char *resultfilename,
			int logtokdf, char *wdffile, char *pcffile, char *gdffile);
int ExecKTM ( char *filename, int syntax, int hardwareoff, int timedsply,
			int loop, int tststation, char *resultfilename,
			int logtokdf, char *wdffile, char *pcffile, char *gdffile );

cmd_table_t *cmdname_str_to_cmdptr ( char *s, int *return_table_num );

/* from ksox_dec.c */

/*	from kitt.c or exec_ktm.c */
#ifdef __STDC__
void outputf(char *fmt, ...);
#else
void outputf(va_alist) va_dcl;
#endif

/*	from load_ktm.c */
void	LoadKTM ( char *filename, char *outbuf, char *desc, int loadkdf );

#ifdef LPTLIB
/*	from lpt_wrap.c */
#include "lpt_wrap.h"

/*	from par_wrap.c */
#include "par_wrap.h"

#endif

#ifdef LPTLIB600
#include "lpt_wrap_s600.h"
#endif

/*	from results.c */

/*
 *	This prototype has been moved to COM_usrlib.h because it is
 *	REQUIRED for correct building of wrapper modules.
 
void			add_result(char *function_name, char *result_name, int
					result_type, void *resultp, int num_items);
*/

result_info_t	*GetResultByName ( char *result_name );
void			set_num_items(int num_items);
void			WriteResults ( char *resultfilename );
int				LogResultsToKDF ( LOT *lot, WAFER *wafer, SITE *site );

/*	from select.c */
void				clear_selections( void );
void				add_select ( char *name, int plot, int log, int user );
select_info_t		*GetSelectByName ( char *result_name );
void				add_constant ( char *name, int type, int global, int size,
						void *valuep );
void				clear_constants( void );
constants_info_t	*GetConstantByName ( char *constant_name );


void add_ts( char *name, int type, int size, void *valuep );
void clear_ts( void );
ts_info_t *GetTSByName( char *ts_name );
void tsPrintDataNode( ts_info_t *ts );
void tsPrintKTMData    ( char *ktm_name, char *name );
void tsPrintAllKTMData ( char *ktm_name );
void tsPrintDataAll ( void );

char GetTSdelimiter() ;
void SetTSdelimiter( char ) ;

double GetDivZeroValue() ;
void SetDivZeroValue( double ) ;

/* from strutil.c */
int str_to_int_array(char *s, int *int_array);

/* from syntax.c */
int		DuplicateConstResult ( void );
int		IsInvalidString ( char *parm, char *func_name, int parm_num );
int		IsInvalidChar ( char *parm, char *func_name, int parm_num );
int		IsInvalidFloat ( char *parm, char *func_name, int parm_num, int isfloat );
int		IsInvalidInt ( char *parm, char *func_name, int parm_num, int islong );
int		ContainsOperators ( char *parm, char *func_name, int parm_num );
int		IsInvalidParmName ( char *parm, char *func_name, int parm_num );
int		IsReservedProber ( char *strptr );
int		IsReservedParlib ( char *strptr );
int		IsReservedLPT ( char *strptr );
int		IsInvalidIdentifier ( char *strptr );
int		IsReservedANSIC ( char *strptr );
int 		CheckValidComment(char *s);


/* from trns_sym.c */
int translate_status ( char *s );
int translate_value ( char *s );
int translate_flag ( char *s );
int translate_all ( char *s );

/* from usrlib.c */
#ifdef USRLIB
usrlib_cmds_t	*FindUsrFunc ( char *func_name );
int				OpenUsrlib ( char *libnamestr );
usrlib_cmds_t	*FindUsrFuncInLib ( usrlib_t *searchlib, char *func_name );
usrlib_t		*GetUsrlibFromLibName ( char *libname );
void			remove_usrlib ( usrlib_t *oldlib );
void			remove_all_usrlibs ( void );
char			*GetRealLibName ( char *basename );
char			*GetRealProtoName ( char *basename );
int				LibraryAlreadyLoaded ( char *libname );
void			SaveCalledUsrlib ( usrlib_cmds_t *usrlib_cmdp );
void			ClearCalledUsrlib ( void );
usrlib_t		*GetCalledUsrlibs ( void );
#endif /*USRLIB*/

#endif /* ! _KSOX_PRO_H */
