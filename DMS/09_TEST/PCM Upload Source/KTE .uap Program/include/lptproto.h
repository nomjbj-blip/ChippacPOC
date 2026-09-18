/* 

    lptproto.h 
    
    Prototypes.
    Linear Parametric Test Library.


    Copyright (c) 1993, 1996 by Keithley Instruments, Inc. Cleveland, Ohio
     
    This software is furnished under a license and may be used and copied 
    only in accordance with the terms of such license, and with the 
    inclusion of the above copyright notice.  This software or any other 
    copies hereof may not be provided or otherwise made available to any 
    other person.  No title to and ownership of the software is hereby 
    transferred.  The information in this software is subject to change 
    without notice, and should not be construed as a commitment by 
    Keithley Instruments, Inc.
     
    Keithley assumes no responsibility for the use or reliability of its 
    software on equipment which is not supplied by Keithley.
*/
/*

 $Workfile:$
   $Source: /cm/test/build/S600/v420/PROJCOM/RCS/lptproto.h,v $
 $Revision: 1.32 $
 Rev $Date: 2000/10/09 17:11:10 $
 
 Change History
 * $Log: lptproto.h,v $
 * Revision 1.32  2000/10/09 17:11:10  hayes
 * PR13578 Changed unsigned int *len to int *len in insinfoXXX calls
 * to eliminate warnings from KITT practice task compiles.  This does
 * not fix anything, it just hides warnings.  The len parameter is
 * still interpreted as an unsigned int.
 *
 * Revision 1.31  2000/09/14 17:35:16  hayes
 * Added floatpinac prototype.
 *
 * Revision 1.30  2000/08/03 17:54:10  hayes
 * PR12889 moved prototypes for setimtr and setvmtr to supported section.
 *
 * Revision 1.29  1999/03/25 13:11:31  hayes
 * PR08795 Added insinfo, insinfo_int, and insinfo_double commands.
 *
 * Revision 1.28  1998/10/23 13:12:22  williamson
 * PR 8322  removed fimv fvmi protos
 *
 * Revision 1.27  1998/04/21 14:45:39  hayes
 * PR06104  Cleaned up and modified for compatibility with C++.  Also
 * added support for nsweepX and ssweepX functions.
 *
 * Revision 1.26  1998/03/24 20:18:20  hayes
 * Modified prototype for beep command.  This is in anticipation of changes
 * needed to the interface because one cannot here the beep on the S600.
 *
 * Revision 1.25  1997/12/31 20:41:01  chaplin
 * Added caldefault (PR4470)
 *
 * Revision 1.24  1997/11/07 21:25:24  hayes
 * Added getlpterror prototype.
 *
 * Revision 1.23  1997/10/21 21:43:42  hayes
 * PR04692  Added several new commands to LPT.  These are:
 *   beep
 *   measrh
 *   meastemp
 *   meascg
 *   avgcg
 *   intgcg
 *
 * Revision 1.22  1997/10/07 19:25:49  hayes
 * PR05311 Added prototype for insbind.
 *
 * Revision 1.21  1997/09/10 13:58:48  hayes
 * PR05055  Modified the prototype of kibrcv so that the rcvlen
 * parameter is an int* instead of an unsigned int*.
 *
 * Revision 1.20  1997/09/09 19:02:40  hayes
 * PR04943  Changed parameter types from long to int as the prototypes
 * were incorrect (lpt docs said they were supposed to be ints).
 *
 * Revision 1.19  1997/05/19 18:53:45  williamson
 * PR 4394  Added compclr function
 *
 * Revision 1.18  1997/04/22 21:02:32  hayes
 * Added resourceProbeStateGet prototype
 *
 * Revision 1.17  1997/02/12 16:43:13  hayes
 * Changed parameter types of things that represent unsigned
 * numbers from longs to unsigned ints.  Also alphabetized all
 * the prototypes.
 *
 * Revision 1.16  1997/01/06 15:50:25  hayes
 * Added refctrl support.
 *
 * Revision 1.15  1996/12/12 17:07:37  tobin
 * added prototypes for resourceProbeStateEnter and
 * resourceProbeStateRequest.
 *
 * Revision 1.14  1996/12/11  15:57:41  hayes
 * Added floatpin.  Changed preamp IDs to be their pin numbers.
 *
 * Revision 1.13  1996/12/03 19:24:14  tobin
 * added prototypes for lptStubModeSet and lptStubModeGet.
 *
 * Revision 1.12  1996/10/30  23:29:16  hayes
 * Revised prototype for ibup and ibupu.
 *
 * Revision 1.11  1996/10/29 15:49:16  hayes
 * Added prototypes for gettstn and puttstn.  Also fixed prototype for tstsel.
 *
 * Revision 1.10  1996/10/22 14:27:14  hayes
 * Added scnmeas and trigcomp prototypes.
 *
 * Revision 1.9  1996/10/08  22:05:46  hayes
 * Hide // style comments.
 * Changed include lines to use <> instead of "".  This will allow nested
 * includes to use the search path instead of the directory where this
 * file was loaded from.
 *
 * Revision 1.8  1996/10/08  12:58:35  tobin
 * added parameter names to all the prototypes
 *
 * Revision 1.7  1996/07/31  21:14:04  hayes
 * Added delay parameter to fvmi and fimv.
 *
 * Revision 1.6  1996/07/30  22:41:16  hayes
 * Adjusted the KIB prototypes.
 *
 * Revision 1.5  1996/07/26  20:22:41  hayes
 * More LPT functionality.  Also added fvmi and fimv.
 *
 * Revision 1.4  1996/06/18  21:41:42  chaplin
 * added prototypes for ibup() and ibupu()
 *
 * Revision 1.3  1996/01/04  20:33:28  hayes
 * Removed all xxx_ prototypes.
 * Added trigtl command.
 *
 * Revision 1.2  1995/12/28  16:59:52  chaplin
 * conpin has TWO required arguments, not just one
 *
 * Revision 1.1  1995/08/14  16:04:54  hayes
 * Initial revision
 *
*/

#ifndef LPTPROTO_H
#define LPTPROTO_H

#ifdef __cplusplus
extern "C" {
#endif

int addcon(int connect1, int connect2, ...);
int adelay(unsigned int delaypoints, double *delayarray);
int asweepc(int instr_id, unsigned int numpoints, double delay, double *forcearray);
int asweepf(int instr_id, unsigned int numpoints, double delay, double *forcearray);
int asweepg(int instr_id, unsigned int numpoints, double delay, double *forcearray);
int asweepi(int instr_id, unsigned int numpoints, double delay, double *forcearray);
int asweepq(int instr_id, unsigned int numpoints, double delay, double *forcearray);
int asweepr(int instr_id, unsigned int numpoints, double delay, double *forcearray);
int asweepv(int instr_id, unsigned int numpoints, double delay, double *forcearray);
int avgc(int instr_id, double *result, unsigned int count, double delay);
int avgcg(int instr_id, double *c, double *g, unsigned int count, double delay);
int avgf(int instr_id, double *result, unsigned int count, double delay);
int avgg(int instr_id, double *result, unsigned int count, double delay);
int avgi(int instr_id, double *result, unsigned int count, double delay);
int avgq(int instr_id, double *result, unsigned int count, double delay);
int avgr(int instr_id, double *result, unsigned int count, double delay);
int avgv(int instr_id, double *result, unsigned int count, double delay);
int beep(unsigned int duration, unsigned int frequency);
int bmeasc(int instr_id, double *results, unsigned int numrdgs, double delay, int timerid, double *timerdata);
int bmeasf(int instr_id, double *results, unsigned int numrdgs, double delay, int timerid, double *timerdata);
int bmeasg(int instr_id, double *results, unsigned int numrdgs, double delay, int timerid, double *timerdata);
int bmeasi(int instr_id, double *results, unsigned int numrdgs, double delay, int timerid, double *timerdata);
int bmeasq(int instr_id, double *results, unsigned int numrdgs, double delay, int timerid, double *timerdata);
int bmeasr(int instr_id, double *results, unsigned int numrdgs, double delay, int timerid, double *timerdata);
int bmeasv(int instr_id, double *results, unsigned int numrdgs, double delay, int timerid, double *timerdata);
int bsweepi(int instr_id, double startval, double endval, unsigned int numpoints, double delay, double *results);
int bsweepv(int instr_id, double startval, double endval, unsigned int numpoints, double delay, double *results);
int clrcon(void);
int clrscn(void);
int clrtrg(void);
int compclr(void);
int conpin(int connect1, int connect2, ...);
int conpth(int path, int connect1, int connect2, ...);
int delay(unsigned int msec);
int delcon(int connect1, ...);
int devclr(void);
int devint(void);
int disable(int instr_id);
int enable(int instr_id);
int execut(void);
#if 0
int fimv(int instr_id, double force_value, double delay, double *result);
#endif
int floatpin(int pin, ...);
int floatpinac(int pin, ...);
int forcec(int instr_id, double force_value);
int forcef(int instr_id, double force_value);
int forceg(int instr_id, double force_value);
int forcei(int instr_id, double force_value);
int forceq(int instr_id, double force_value);
int forcer(int instr_id, double force_value);
int forcev(int instr_id, double force_value);
#if 0
int fvmi(int instr_id, double force_value, double delay, double *result);
#endif
int getlpterr(void);
int getstatus(int instr_id, unsigned int data, double *value);
int ibup(int function, ...);
int ibupu(int unit, int function, ...);
int imeast(int instr_id, double *result);
int insbind(int instr1, int instr2);
int inshld(void);
int insinfo(int instr_id, unsigned int parameter, char *result, unsigned int maxlen, int *len);
int insinfo_int(int instr_id, unsigned int parameter, int *result, unsigned int maxlen, int *len);
int insinfo_double(int instr_id, unsigned int parameter, double *result, unsigned int maxlen, int *len);
int intgc(int instr_id, double *result);
int intgcg(int instr_id, double *c, double *g);
int intgf(int instr_id, double *result);
int intgg(int instr_id, double *result);
int intgi(int instr_id, double *result);
int intgq(int instr_id, double *result);
int intgr(int instr_id, double *result);
int intgv(int instr_id, double *result);
int kibcmd(unsigned int timeout, unsigned int bufflen, char *buffer);
int kibdefclr(int pri_addr, int sec_addr, unsigned int timeout, double delay, unsigned int bufflen, char *buffer);
int kibdefint(int pri_addr, int sec_addr, unsigned int timeout, double delay, unsigned int bufflen, char *buffer);
int kibrcv(int pri_addr, int sec_addr, char term, unsigned int timeout, unsigned int bufflen, int *rcv_count, char *buffer);
int kibsnd(int pri_addr, int sec_addr, unsigned int timeout, unsigned int bufflen, char * buffer);
int kibspl(int pri_addr, int sec_addr, unsigned int timeout, int * spoll_byte);
int kibsplw(int pri_addr, int sec_addr, unsigned int timeout, int * spoll_byte);
int limitc(int instr_id, double limit_value);
int limitf(int instr_id, double limit_value);
int limitg(int instr_id, double limit_value);
int limiti(int instr_id, double limit_value);
int limitq(int instr_id, double limit_value);
int limitr(int instr_id, double limit_value);
int limitv(int instr_id, double limit_value);
int lorangec(int instr_id, double range_value);
int lorangef(int instr_id, double range_value);
int lorangeg(int instr_id, double range_value);
int lorangei(int instr_id, double range_value);
int lorangeq(int instr_id, double range_value);
int loranger(int instr_id, double range_value);
int lorangev(int instr_id, double range_value);
int measc(int instr_id, double *result);
int meascg(int instr_id, double *c, double *g);
int measf(int instr_id, double *result);
int measg(int instr_id, double *result);
int measi(int instr_id, double *result);
int measq(int instr_id, double *result);
int measr(int instr_id, double *result);
int measrh(int instr_id, double *result);
int meast(int instr_id, double *result);
int meastemp(int instr_id, double *result);
int measv(int instr_id, double *result);
int mpulse(int instr_id, double amplitude, double duration, double *vmeas, double *imeas);
int nsweepc(int instr_id, double startval, double endval, unsigned int numsteps, double delay);
int nsweepi(int instr_id, double startval, double endval, unsigned int numsteps, double delay);
int nsweepf(int instr_id, double startval, double endval, unsigned int numsteps, double delay);
int nsweepg(int instr_id, double startval, double endval, unsigned int numsteps, double delay);
int nsweepq(int instr_id, double startval, double endval, unsigned int numsteps, double delay);
int nsweepr(int instr_id, double startval, double endval, unsigned int numsteps, double delay);
int nsweepv(int instr_id, double startval, double endval, unsigned int numsteps, double delay);
int pior(int instr_id, int *data);
int piorb(int instr_id, int bitpattern, int *result);
int piow(int instr_id, int data);
int piowait(int instr_id, int bitpattern, int state);
int piowb(int instr_id, int data, int state);
int pulsec(int instr_id, double value, double duration);
int pulseg(int instr_id, double value, double duration);
int pulsei(int instr_id, double value, double duration);
int pulser(int instr_id, double value, double duration);
int pulsev(int instr_id, double value, double duration);
int rangec(int instr_id, double range_value);
int rangef(int instr_id, double range_value);
int rangeg(int instr_id, double range_value);
int rangei(int instr_id, double range_value);
int rangeq(int instr_id, double range_value);
int ranger(int instr_id, double range_value);
int rangev(int instr_id, double range_value);
int rdelay(double sec);
int refctrl(int instr_id, int cmd);
int retmrstats(int instr_id, double *effective_resolution, double *max_value);
int rtfary(double *results);
int savgc(int instr_id, double *results, unsigned int count, double delay);
int savgf(int instr_id, double *results, unsigned int count, double delay);
int savgg(int instr_id, double *results, unsigned int count, double delay);
int savgi(int instr_id, double *results, unsigned int count, double delay);
int savgq(int instr_id, double *results, unsigned int count, double delay);
int savgr(int instr_id, double *results, unsigned int count, double delay);
int savgv(int instr_id, double *results, unsigned int count, double delay);
int scnmeas(void);
int searchc(int instr_id, double minval, double maxval, unsigned int iterations, double delay, double *result);
int searchf(int instr_id, double minval, double maxval, unsigned int iterations, double delay, double *result);
int searchg(int instr_id, double minval, double maxval, unsigned int iterations, double delay, double *result);
int searchi(int instr_id, double minval, double maxval, unsigned int iterations, double delay, double *result);
int searchq(int instr_id, double minval, double maxval, unsigned int iterations, double delay, double *result);
int searchr(int instr_id, double minval, double maxval, unsigned int iterations, double delay, double *result);
int searchv(int instr_id, double minval, double maxval, unsigned int iterations, double delay, double *result);
int setauto(int instr_id);
int setimtr(int instr_id);
int setmode(int instr_id, unsigned int modifier, double value);
int setvmtr(int instr_id);
int sintgc(int instr_id, double *results);
int sintgf(int instr_id, double *results);
int sintgg(int instr_id, double *results);
int sintgi(int instr_id, double *results);
int sintgq(int instr_id, double *results);
int sintgr(int instr_id, double *results);
int sintgv(int instr_id, double *results);
int smeasc(int instr_id, double *results);
int smeasi(int instr_id, double *results);
int smeasf(int instr_id, double *results);
int smeasg(int instr_id, double *results);
int smeasq(int instr_id, double *results);
int smeasr(int instr_id, double *results);
int smeast(int instr_id, double *results);
int smeasv(int instr_id, double *results);
int ssmeasc(int instr_id, double *result, double delta, unsigned int maxrdgs, double delay);
int ssmeasi(int instr_id, double *result, double delta, unsigned int maxrdgs, double delay);
int ssmeasf(int instr_id, double *result, double delta, unsigned int maxrdgs, double delay);
int ssmeasg(int instr_id, double *result, double delta, unsigned int maxrdgs, double delay);
int ssmeasq(int instr_id, double *result, double delta, unsigned int maxrdgs, double delay);
int ssmeasr(int instr_id, double *result, double delta, unsigned int maxrdgs, double delay);
int ssmeasv(int instr_id, double *result, double delta, unsigned int maxrdgs, double delay);
int ssweepc(int instr_id, double startval, double endval);
int ssweepi(int instr_id, double startval, double endval);
int ssweepf(int instr_id, double startval, double endval);
int ssweepg(int instr_id, double startval, double endval);
int ssweepq(int instr_id, double startval, double endval);
int ssweepr(int instr_id, double startval, double endval);
int ssweepv(int instr_id, double startval, double endval);
int sweepc(int instr_id, double startval, double endval, unsigned int numsteps, double delay);
int sweepi(int instr_id, double startval, double endval, unsigned int numsteps, double delay);
int sweepf(int instr_id, double startval, double endval, unsigned int numsteps, double delay);
int sweepg(int instr_id, double startval, double endval, unsigned int numsteps, double delay);
int sweepq(int instr_id, double startval, double endval, unsigned int numsteps, double delay);
int sweepr(int instr_id, double startval, double endval, unsigned int numsteps, double delay);
int sweepv(int instr_id, double startval, double endval, unsigned int numsteps, double delay);
int trigcg(int instr_id, double value);
int trigcl(int instr_id, double value);
int trigcomp(int instr_id, int mode);
int trigfg(int instr_id, double value);
int trigfl(int instr_id, double value);
int triggg(int instr_id, double value);
int triggl(int instr_id, double value);
int trigig(int instr_id, double value);
int trigil(int instr_id, double value);
int trigqg(int instr_id, double value);
int trigql(int instr_id, double value);
int trigrg(int instr_id, double value);
int trigrl(int instr_id, double value);
int trigtg(int instr_id, double value);
int trigtl(int instr_id, double value);
int trigvg(int instr_id, double value);
int trigvl(int instr_id, double value);

/*
    Obsolete functions.

    Note: some are included for compatability.
*/
#if 0
int atten(int instr_id, double value);
int extract_lpt_error(long *, long *, long);
int fltoff(int instr_id);
int flton(int instr_id);
#endif
int kfpabs(double *x, double *y);
int kfpadd(double *x, double *y, double *z);
int kfpdiv(double *x, double *y, double *z);
int kfpexp(double *x, double *y);
int kfplog(double *x, double *y);
int kfpmul(double *x, double *y, double *z);
int kfpneg(double *x, double *y);
int kfppwr(double *x, double *y, double *z);
int kfpsqrt(double *x, double *y);
int kfpsub(double *x, double *y, double *z);
#if 0
int log_lpt_error(long *, long);
int nslope(int instr_id);
int outebl(int instr_id);
int pslope(int instr_id);
int read_buffer_(int *, long, long *);
int rexcut(void);
int setac(int instr_id);
int setdc(int instr_id);
int setfilter(int instr_id, unsigned int filter);
int setgate(int instr_id, double value);
int settrig(int instr_id, double value);
int setvims(int instr_id);
int zckon(int instr_id);
int zckoff(int instr_id);
#endif


/*
    LPT internal functions.  These are not intended to be user callable.  They
    are included for AC internal use.
*/

int  gettstn(void);
int  lptStubModeGet(void);
void lptStubModeSet(int mode);
void puttstn(int teststation);
void resourceProbeStateGet(int *state);
void resourceProbeStateEnter(int state);
void resourceProbeStateRequest(int state);
int  tstsel(int teststation);
int  tstdsl(void);

#ifdef __cplusplus
}
#endif

#endif


