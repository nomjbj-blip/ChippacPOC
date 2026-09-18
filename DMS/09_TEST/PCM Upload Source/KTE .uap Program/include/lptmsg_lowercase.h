/*

    lptmsg_lowercase.h 
   
    Symbolic error codes.
    Linear Parametric Test Library.
          

    Copyright (c) 1993 by Keithley Instruments, Inc. Cleveland, Ohio
     
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
   $Source: /cm/test/build/S600/v420/PROJCOM/RCS/lptmsg_lowercase.h,v $
 $Revision: 1.3 $
 Rev $Date: 2000/09/27 22:08:34 $
     
*/

#ifndef LPTMSG_LOWERCASE_H
#define LPTMSG_LOWERCASE_H

#include <lptmsg.h>

#define not_ok NOT_OK
#define ok OK
#define lpt_normal LPT_NORMAL

#define ib_nocmd_byte_avail IB_NOCMD_BYTE_AVAIL
#define ib_cac_conflict IB_CAC_CONFLICT
#define ib_notcac IB_NOTCAC
#define ib_notsac IB_NOTSAC
#define ib_ifc_abort IB_IFC_ABORT
#define ib_timeout IB_TIMEOUT
#define ib_bad_func_code IB_BAD_FUNC_CODE
#define ib_tct_timeout IB_TCT_TIMEOUT
#define ib_nolistners_on_bus IB_NOLISTNERS_ON_BUS
#define ib_driver_problem IB_DRIVER_PROBLEM
#define ib_bad_slot_num IB_BAD_SLOT_NUM
#define ib_nolisten_address IB_NOLISTEN_ADDRESS
#define ib_notalk_address IB_NOTALK_ADDRESS
#define ib_sftwr_config_problem IB_SFTWR_CONFIG_PROBLEM
#define ib_no_util_func IB_NO_UTIL_FUNC

#define internal_err INTERNAL_ERR
#define mod_load_err MOD_LOAD_ERR
#define mod_fmt_err MOD_FMT_ERR
#define mod_notfound MOD_NOTFOUND

#define spawn_err SPAWN_ERR
#define network_err NETWORK_ERR
#define protocol_err PROTOCOL_ERR
#define file_read_err FILE_READ_ERR
#define file_write_err FILE_WRITE_ERR

#define diag_fault DIAG_FAULT
#define diag_fatal_fault DIAG_FATAL_FAULT

#define tapi_badchannel TAPI_BADCHANNEL
#define tapi_badtester TAPI_BADTESTER
#define tapi_notfound TAPI_NOTFOUND
#define tapi_refused TAPI_REFUSED
#define tapi_nofiber TAPI_NOFIBER
#define tapi_fibercon TAPI_FIBERCON

#define tapi_badgroupid TAPI_BADGROUPID
#define tapi_badtestid TAPI_BADTESTID
#define tapi_badlist TAPI_BADLIST
#define tapi_exec_busy TAPI_EXEC_BUSY

#define generic_err GENERIC_ERR
#define too_many_instr TOO_MANY_INSTR
#define mem_alloc_err MEM_ALLOC_ERR

#define lpt_preverr LPT_PREVERR
#define lpt_fatal LPT_FATAL
#define lpt_fatalintest LPT_FATALINTEST
#define lpt_abort LPT_ABORT

#define ki_sndtimout KI_SNDTIMOUT
#define ki_rcvtimout KI_RCVTIMOUT
#define ki_vmesnderr KI_VMESNDERR
#define ki_vmercverr KI_VMERCVERR
#define ki_badconfig KI_BADCONFIG

#define mx_invldcnt MX_INVLDCNT
#define lpt_invldcnt LPT_INVLDCNT
#define mx_nopin MX_NOPIN
#define mx_multicon MX_MULTICON
#define mx_dangcon MX_DANGCON
#define mx_unrecdev MX_UNRECDEV
#define mx_nopathasn MX_NOPATHASN
#define mx_pthpreasn MX_PTHPREASN
#define mx_notenfpath MX_NOTENFPATH
#define mx_argnotdef MX_ARGNOTDEF
#define mx_illgltsn MX_ILLGLTSN
#define mx_nognd MX_NOGND
#define mx_nolow MX_NOLOW
#define mx_noswitch MX_NOSWITCH
#define mx_illglcon MX_ILLGLCON
#define mx_opinvld MX_OPINVLD
#define mx_hotswitch MX_HOTSWITCH

#define ut_rsltoflw UT_RSLTOFLW
#define ut_trgtblovr UT_TRGTBLOVR
#define ut_invldprm UT_INVLDPRM
#define ut_scntblovr UT_SCNTBLOVR
#define ut_nouram UT_NOURAM
#define ut_spurint UT_SPURINT
#define ut_tmrivld UT_TMRIVLD
#define ut_trigtobig UT_TRIGTOBIG
#define ut_clrtblovr UT_CLRTBLOVR
#define ut_inttblovr UT_INTTBLOVR
#define ut_invldval UT_INVLDVAL
#define ut_tomanypts UT_TOMANYPTS

#define cb_devntfnd     CB_DEVNTFND
#define cb_unkfunc      CB_UNKFUNC
#define cb_badfunc      CB_BADFUNC
#define cb_notcfg       CB_NOTCFG
#define cb_notcnfg      CB_NOTCNFG
#define cb_unkdev       CB_UNKDEV
#define cb_nofile       CB_NOFILE
#define cb_format       CB_FORMAT
#define cb_devfail      CB_DEVFAIL
#define cb_devnotvxi    CB_DEVNOTVXI    
#define cb_funccnflct   CB_FUNCCNFLCT   
#define cb_baddpram     CB_BADDPRAM     
#define cb_invlderror   CB_INVLDERROR   
#define cb_invldevent   CB_INVLDEVENT   
#define cb_devinvldaddr CB_DEVINVLDADDR 
#define cb_svcnotsup    CB_SVCNOTSUP    
#define cb_insnotrec    CB_INSNOTREC    
#define cb_nohwsup      CB_NOHWSUP      
#define cb_notlic       CB_NOTLIC       
#define cb_fwmismatch   CB_FWMISMATCH   


#define fm_ftobg FM_FTOBG
#define fm_rngchng FM_RNGCHNG
#define fm_iltosml FM_ILTOSML
#define fm_iltobg FM_ILTOBG
#define fm_irtobg FM_IRTOBG
#define fm_mxerr FM_MXERR
#define fm_vltobg FM_VLTOBG
#define fm_vrtobg FM_VRTOBG
#define fm_setmtr FM_SETMTR
#define fm_limmtr FM_LIMMTR
#define fm_srcmtr FM_SRCMTR
#define fm_mivmtr FM_MIVMTR
#define fm_mvimtr FM_MVIMTR
#define fm_valtobig FM_VALTOBIG
#define fm_cmctobig FM_CMCTOBIG
#define fm_cmgtobig FM_CMGTOBIG
#define fm_cmnosrc FM_CMNOSRC
#define fm_nodrng FM_NODRNG
#define fm_soa FM_SOA
#define fm_oscdet FM_OSCDET
#define fm_shutdown FM_SHUTDOWN
#define fm_thermal FM_THERMAL
#define fm_pulse FM_PULSE
#define fm_limtobig FM_LIMTOBIG
#define fm_autolimit FM_AUTOLIMIT
#define fm_qrtobg FM_QRTOBG
#define fm_vltosml FM_VLTOSML
#define fm_rngtosml FM_RNGTOSML
#define fm_smuerr FM_SMUERR

#define ie_timout IE_TIMOUT
#define ie_nolisten IE_NOLISTEN
#define ie_nohrdwar IE_NOHRDWAR

#define sru_nosmu SRU_NOSMU
#define sru_nodmm SRU_NODMM
#define sru_gpibfail SRU_GPIBFAIL
#define sru_nocal SRU_NOCAL
#define sru_cmderr SRU_CMDERR
#define sru_hrdfail SRU_HRDFAIL
#define sru_kelvin SRU_KELVIN
#define sru_generr SRU_GENERR

#define kfp_divzero KFP_DIVZERO
#define kfp_logneg KFP_LOGNEG
#define kfp_sqrtneg KFP_SQRTNEG
#define kfp_pwrneg KFP_PWRNEG

#define klp_nolbldef KLP_NOLBLDEF
#define klp_lblredef KLP_LBLREDEF
#define klp_badlabel KLP_BADLABEL

#define ecp_protover ECP_PROTOVER

#define last_error LAST_ERROR

#define inst_overrange INST_OVERRANGE
#define current_overload CURRENT_OVERLOAD
#define osc_detect OSC_DETECT
#define thermal_shutdown THERMAL_SHUTDOWN
#define soa_exceeded SOA_EXCEEDED
#define pwid_too_short PWID_TOO_SHORT
#define source_limit SOURCE_LIMIT
#define meas_not_performed MEAS_NOT_PERFORMED

#endif

