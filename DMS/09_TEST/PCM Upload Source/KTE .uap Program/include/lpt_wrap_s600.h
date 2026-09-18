/**************************************************************************
 *
 *       COPYRIGHT (C) 1992  by  KEITHLEY INSTRUMENTS, INC.
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
 *
 * File:     $Source: /cm/test/build/S600/v420/COMMON/RCS/lpt_wrap_s600.h,v $
 * Current $Revision: 1.15 $
 * Curent     $State: REL $
 * Last Rev    $Date: 2000/09/15 14:03:02 $
 *************************************************************************/

#ifdef LPTLIB600

#include "lptproto.h"

int w_asweepi(callinfo_t *callinfo);
int w_asweepv(callinfo_t *callinfo);
int w_asweepc(callinfo_t *callinfo);
int w_asweepg(callinfo_t *callinfo);
int w_asweepf(callinfo_t *callinfo);
int w_asweepr(callinfo_t *callinfo);
int w_asweepq(callinfo_t *callinfo);
int w_rtfary(callinfo_t *callinfo);
int w_adelay(callinfo_t *callinfo);
int w_avgi(callinfo_t *callinfo);
int w_avgv(callinfo_t *callinfo);
int w_avgc(callinfo_t *callinfo);
int w_avgcg(callinfo_t *callinfo);
int w_avgg(callinfo_t *callinfo);
int w_avgf(callinfo_t *callinfo);
int w_avgr(callinfo_t *callinfo);
int w_avgq(callinfo_t *callinfo);
int w_beep(callinfo_t *callinfo);
int w_bmeasi(callinfo_t *callinfo);
int w_bmeasv(callinfo_t *callinfo);
int w_bmeasc(callinfo_t *callinfo);
int w_bmeasg(callinfo_t *callinfo);
int w_bmeasf(callinfo_t *callinfo);
int w_bmeasr(callinfo_t *callinfo);
int w_bmeasq(callinfo_t *callinfo);
int w_bsweepv(callinfo_t *callinfo);
int w_bsweepi(callinfo_t *callinfo);
int w_delay(callinfo_t *callinfo);
int w_rdelay(callinfo_t *callinfo);
int w_devclr(callinfo_t *callinfo);
int w_devint(callinfo_t *callinfo);
int w_execut(callinfo_t *callinfo);
int w_rexcut(callinfo_t *callinfo);
int w_insclr(callinfo_t *callinfo);
int w_inshld(callinfo_t *callinfo);
int w_insint(callinfo_t *callinfo);
int w_insbind(callinfo_t *callinfo);
int w_insinfo(callinfo_t *callinfo);
int w_insinfo_int(callinfo_t *callinfo);
int w_insinfo_double(callinfo_t *callinfo);
int w_kfpabs(callinfo_t *callinfo);
int w_kfpadd(callinfo_t *callinfo);
int w_kfpdiv(callinfo_t *callinfo);
int w_kfpexp(callinfo_t *callinfo);
int w_kfplog(callinfo_t *callinfo);
int w_kfpmul(callinfo_t *callinfo);
int w_kfpneg(callinfo_t *callinfo);
int w_kfppwr(callinfo_t *callinfo);
int w_kfpsqrt(callinfo_t *callinfo);
int w_kfpsub(callinfo_t *callinfo);
int w_flton(callinfo_t *callinfo);
int w_fltoff(callinfo_t *callinfo);
int w_setfilter(callinfo_t *callinfo);
int w_forcei(callinfo_t *callinfo);
int w_forcev(callinfo_t *callinfo);
int w_forcec(callinfo_t *callinfo);
int w_forceg(callinfo_t *callinfo);
int w_forcef(callinfo_t *callinfo);
int w_forcer(callinfo_t *callinfo);
int w_forceq(callinfo_t *callinfo);
int w_fvmi(callinfo_t *callinfo);
int w_fimv(callinfo_t *callinfo);
int w_getlpterr(callinfo_t *callinfo);
int w_getstatus(callinfo_t *callinfo);
int w_intgi(callinfo_t *callinfo);
int w_intgv(callinfo_t *callinfo);
int w_intgc(callinfo_t *callinfo);
int w_intgcg(callinfo_t *callinfo);
int w_intgg(callinfo_t *callinfo);
int w_intgr(callinfo_t *callinfo);
int w_intgf(callinfo_t *callinfo);
int w_intgq(callinfo_t *callinfo);
int w_ibupu_write(callinfo_t *callinfo);
int w_ibupu_read(callinfo_t *callinfo);
int w_ibupu_clear(callinfo_t *callinfo);
int w_ibupu_trigger(callinfo_t *callinfo);
int w_ibupu_remote(callinfo_t *callinfo);
int w_ibupu_local(callinfo_t *callinfo);
int w_ibupu_srpoll(callinfo_t *callinfo);
int w_ibupu_srqwait(callinfo_t *callinfo);
int w_ibupu_define(callinfo_t *callinfo);
int w_ibupu_finish(callinfo_t *callinfo);
int w_kibcmd(callinfo_t *callinfo);
int w_kibrcv(callinfo_t *callinfo);
int w_kibsnd(callinfo_t *callinfo);
int w_kibspl(callinfo_t *callinfo);
int w_kibsplw(callinfo_t *callinfo);
int w_kibdefclr(callinfo_t *callinfo);
int w_kibdefint(callinfo_t *callinfo);
int w_limiti(callinfo_t *callinfo);
int w_limitv(callinfo_t *callinfo);
int w_limitc(callinfo_t *callinfo);
int w_limitg(callinfo_t *callinfo);
int w_limitf(callinfo_t *callinfo);
int w_limitr(callinfo_t *callinfo);
int w_limitq(callinfo_t *callinfo);
int w_measi(callinfo_t *callinfo);
int w_measv(callinfo_t *callinfo);
int w_measc(callinfo_t *callinfo);
int w_meascg(callinfo_t *callinfo);
int w_measg(callinfo_t *callinfo);
int w_measf(callinfo_t *callinfo);
int w_measr(callinfo_t *callinfo);
int w_measq(callinfo_t *callinfo);
int w_measrh(callinfo_t *callinfo);
int w_meast(callinfo_t *callinfo);
int w_meastemp(callinfo_t *callinfo);
int w_imeast(callinfo_t *callinfo);
int w_mpulse(callinfo_t *callinfo);
int w_conpin(callinfo_t *callinfo);
int w_floatpin(callinfo_t *callinfo);
int w_floatpinac(callinfo_t *callinfo);
int w_compclr(callinfo_t *callinfo);
int w_addcon(callinfo_t *callinfo);
int w_delcon(callinfo_t *callinfo);
int w_conpth(callinfo_t *callinfo);
int w_clrcon(callinfo_t *callinfo);
int w_outebl(callinfo_t *callinfo);
int w_enable(callinfo_t *callinfo);
int w_disable(callinfo_t *callinfo);
int w_pulsei(callinfo_t *callinfo);
int w_pulsev(callinfo_t *callinfo);
int w_pulsec(callinfo_t *callinfo);
int w_pulseg(callinfo_t *callinfo);
int w_pulser(callinfo_t *callinfo);
int w_rangei(callinfo_t *callinfo);
int w_rangev(callinfo_t *callinfo);
int w_rangec(callinfo_t *callinfo);
int w_rangeg(callinfo_t *callinfo);
int w_rangef(callinfo_t *callinfo);
int w_ranger(callinfo_t *callinfo);
int w_rangeq(callinfo_t *callinfo);
int w_lorangei(callinfo_t *callinfo);
int w_lorangev(callinfo_t *callinfo);
int w_lorangec(callinfo_t *callinfo);
int w_lorangef(callinfo_t *callinfo);
int w_lorangeg(callinfo_t *callinfo);
int w_lorangeq(callinfo_t *callinfo);
int w_loranger(callinfo_t *callinfo);
int w_setauto(callinfo_t *callinfo);
int w_setintr(callinfo_t *callinfo);
int w_setvmtr(callinfo_t *callinfo);
int w_savgi(callinfo_t *callinfo);
int w_savgv(callinfo_t *callinfo);
int w_savgc(callinfo_t *callinfo);
int w_savgg(callinfo_t *callinfo);
int w_savgf(callinfo_t *callinfo);
int w_savgr(callinfo_t *callinfo);
int w_savgq(callinfo_t *callinfo);
int w_searchi(callinfo_t *callinfo);
int w_searchv(callinfo_t *callinfo);
int w_searchc(callinfo_t *callinfo);
int w_searchg(callinfo_t *callinfo);
int w_searchf(callinfo_t *callinfo);
int w_searchr(callinfo_t *callinfo);
int w_searchq(callinfo_t *callinfo);
int w_setimtr(callinfo_t *callinfo);
int w_setvims(callinfo_t *callinfo);
int w_setvmtr(callinfo_t *callinfo);
int w_setmode(callinfo_t *callinfo);
int w_setac(callinfo_t *callinfo);
int w_setdc(callinfo_t *callinfo);
int w_pslope(callinfo_t *callinfo);
int w_nslope(callinfo_t *callinfo);
int w_settrig(callinfo_t *callinfo);
int w_setgate(callinfo_t *callinfo);
int w_atten(callinfo_t *callinfo);
int w_sintgi(callinfo_t *callinfo);
int w_sintgv(callinfo_t *callinfo);
int w_sintgc(callinfo_t *callinfo);
int w_sintgg(callinfo_t *callinfo);
int w_sintgf(callinfo_t *callinfo);
int w_sintgr(callinfo_t *callinfo);
int w_sintgq(callinfo_t *callinfo);
int w_smeasi(callinfo_t *callinfo);
int w_smeasv(callinfo_t *callinfo);
int w_smeasc(callinfo_t *callinfo);
int w_smeasg(callinfo_t *callinfo);
int w_smeasf(callinfo_t *callinfo);
int w_smeasr(callinfo_t *callinfo);
int w_smeasq(callinfo_t *callinfo);
int w_smeast(callinfo_t *callinfo);
int w_clrscn(callinfo_t *callinfo);
int w_ssmeasi(callinfo_t *callinfo);
int w_ssmeasv(callinfo_t *callinfo);
int w_ssmeasc(callinfo_t *callinfo);
int w_ssmeasg(callinfo_t *callinfo);
int w_ssmeasf(callinfo_t *callinfo);
int w_ssmeasr(callinfo_t *callinfo);
int w_ssmeasq(callinfo_t *callinfo);
int w_syncmode(callinfo_t *callinfo);
int w_retmrstats(callinfo_t *callinfo);
int w_scnmeas(callinfo_t *callinfo);
int w_sweepi(callinfo_t *callinfo);
int w_sweepv(callinfo_t *callinfo);
int w_sweepc(callinfo_t *callinfo);
int w_sweepg(callinfo_t *callinfo);
int w_sweepf(callinfo_t *callinfo);
int w_sweepr(callinfo_t *callinfo);
int w_sweepq(callinfo_t *callinfo);
int w_trigcomp(callinfo_t *callinfo);
int w_trigig(callinfo_t *callinfo);
int w_trigvg(callinfo_t *callinfo);
int w_trigcg(callinfo_t *callinfo);
int w_triggg(callinfo_t *callinfo);
int w_trigfg(callinfo_t *callinfo);
int w_trigrg(callinfo_t *callinfo);
int w_trigqg(callinfo_t *callinfo);
int w_trigtg(callinfo_t *callinfo);
int w_trigil(callinfo_t *callinfo);
int w_trigvl(callinfo_t *callinfo);
int w_trigcl(callinfo_t *callinfo);
int w_triggl(callinfo_t *callinfo);
int w_trigfl(callinfo_t *callinfo);
int w_trigrl(callinfo_t *callinfo);
int w_trigql(callinfo_t *callinfo);
int w_trigtl(callinfo_t *callinfo);
int w_clrtrg(callinfo_t *callinfo);
int w_tstsel(callinfo_t *callinfo);
int w_pior(callinfo_t *callinfo);
int w_piorb(callinfo_t *callinfo);
int w_piow(callinfo_t *callinfo);
int w_piowait(callinfo_t *callinfo);
int w_piowb(callinfo_t *callinfo);
int w_zckon(callinfo_t *callinfo);
int w_zckoff(callinfo_t *callinfo);

int w_refctrl(callinfo_t *callinfo);

#endif /*LPTLIB*/
