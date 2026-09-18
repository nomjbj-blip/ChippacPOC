/* prb_extern.h */
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

 File:     $Source: /cm/test/build/S600/v420/COMMON/RCS/prb_extern.h,v $
 Current $Revision: 1.22 $
 Current    $State: REL $
 Last Rev    $Date: 2000/09/08 17:19:51 $

 Change       $Log: prb_extern.h,v $
 Change       Revision 1.22  2000/09/08 17:19:51  rybka
 Change       added proto for prbsrqinit
 Change
 Change       Revision 1.21  2000/01/28 11:35:16  rybka
 Change       added proto for void getPRBerrfile( char*kierrmsg_filename );
 Change       for NT build
 Change
 Change       Revision 1.20  1999/11/15 16:07:56  rybka
 Change       PR10596 removed reference to strip_cmd proto moved to kicommon.h
 Change
 Change       Revision 1.19  1999/10/18 14:06:34  rybka
 Change       added proto for pruniversalio.c - generic pregio.c
 Change
 Change       Revision 1.18  1999/06/04 18:39:28  rybka
 Change       PR9465 added protos for prbdlginit & prbdlg
 Change
 Change       Revision 1.17  1999/03/19 12:41:26  williamson
 Change       Added ifdef WIN32 for prototypes
 Change
 Change       Revision 1.16  1999/03/18 20:08:53  trybak
 Change       Remove isdigit declaration
 Change
 Change       Revision 1.15  1999/03/18 14:11:40  rybka
 Change       added protos
 Change
 Change       Revision 1.10  1999/03/05 14:45:26  rybka
 Change       added protos for sendrec & prparseoptions
 Change
 Change       Revision 1.9  1998/06/24 18:56:57  djohnson
 Change       added include for prb_func_id and wrapper for single loading
 Change
 Change       Revision 1.8  1998/06/24 12:20:00  rybka
 Change       added extern PRBFUNCS xref_prb_func[] PR5286
 Change
 Change       Revision 1.7  1998/05/08 13:28:45  rybka
 Change       added protos for prb_debug_print,trans,cmnds
 Change       added proto for getprbStruct
 Change       removed ref to globals prb_debug_print,trans,cmnds
 Change
 Change       Revision 1.6  1997/12/16 18:16:14  rybka
 Change       PR5903 removed prototypes for getdsptab, getdrvadr and putdrvadr NT reasons
 Change       see prb_extern or prb_drvadr (new file)
 Change
 Change       Revision 1.5  1997/08/08 16:47:10  rybka
 Change       protos for pregio, prgpibcomsrq & prquerygpib
 Change       ./
 Change
 Change       Revision 1.4  1997/04/22 12:59:28  williamson
 Change       Moved from S600 projcom area
 Change
 Change       Revision 1.3  1997/02/20 15:53:04  rybka
 Change       added prototypes: Pr_nint & Pr_anint
 Change
 * Revision 1.2  1996/11/04  18:04:36  witzke
 * change function types from long to int
 *
 * Revision 1.2  1993/06/17  16:04:59  witzke
 * Added Cnfg_gpib for T19S support
 *
 * Revision 1.1  1993/02/06  17:39:49  beecher
 * Initial revision
 *
...............................................................................

 Function: prober external variables and internal routine prototypes

 16-dec-97 rybka
 The following function prototypes were movedform prb_extern. to prb_drvadr.h.
 The NT development required this because of issues with IMPORT/EXPORT.
		getdrvadr, putdrvadr, getdsptab

 Also the externs for global variables were moved from prb.h to prb_extern.h
   	 
.............................................................................*/
#ifndef _prb_extern
#define _prb_extern 1
#include "prb_func_id.h"
extern int (**drvtab[])();	/* array of ptrs to arrays of ptrs
				   to functions that return longs */

/* externs for global vars */

extern char xoutput_buffer[];
extern char xinput_buffer[];
extern int xoutput_buffer_len, xinput_buffer_len;

#ifndef WIN32
extern prb_t Prb[];
#endif

extern prb_ni_t prb_nameid[];   /* name and id table */

extern PRBFUNCS xref_prb_func[]; /* defined in PRBCOM/prb_dec.c */

int prb_debug_print(); /* function to access environment set debug, for stdout printing */
int prb_debug_trans(); /* function to access environment set debug, for stdout transactions */
int prb_debug_cmnds(); /* function to access environment set debug, for stdout commands */

int ProbFuncNotSup( );	/* routine that logs
				 "function not supported by prober" error */

void pr_sig_error( int ErrVal, char *func_name, int NumErr,
	 long *ErrArray, char *ErrFile );

void ProberError( int err_num, char *func_name, int opt_param);

void LogCmd( char *command_name );

void LogTrans( int trans_type, char *buf, int buflen );

void FilterNonPrintableChar( char *src, int src_len, char *dst, int dst_len );

void ErrMsg();

int Cnfg_tty( int teststation );

int Chng_tty_time( int teststation, int new_time );

void set_error_level( void );

void getprbdebug( void );

void getprberrlvl( void );

void GetDate(char *, int );

void GetTime(char *, int );

void PrDelay(int);

int Cnfg_gpib( int teststation );

double Pr_anint( double value );

int Pr_nint( double value );

int pregio( char *dataOut, int outLen, char *dataIn, int inLen, int term, int term_ct, int *i_srq, int timeout );

int pruniversalio( char *dataOut, int outLen, char *dataIn, int inLen, int term, int term_ct, int *i_srq, int timeout );

int prgpibcomsrq( char *dataOut, int outLen, char *dataIn, int inLen, int timeout, int *srq );

int prgpibcom( char *dataOut, int outLen, char *dataIn, int inLen );

int prquerygpib( int timeout );

int getPrbStruct( int datatype, long* lp_member );

int sendrec( char* inbuf, int inbuflen, char* outbuf, int outbuflen, int terminator, int num_terminators );
 
int prparseoptions(char* opt_str, int i_pos, char* s_result);

int KIGetUserName( char* Username, int n );

int prproberready( void );

int prspoll( void );

int prwaitsrqclear( void );

int prconnectionok( int retryCount );

#ifdef WIN32
char* strtok_r(char* yptr, char* xytok, char** cp_play);
#endif

void prbdlginit(  );

void prbdlg( int func, ... );

void getPRBerrfile( char*kierrmsg_filename );

int prbsrqinit();

/*
int MM40_io( char *dataOut, int outLen, char *dataIn, int inLen, int term, int term_ct, int *i_srq, int timeout );
*/

#endif
