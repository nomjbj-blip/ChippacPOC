/*

    lptparam_lowercase.h 

    Parameter definitions.
    Linear Parametric Test Library.


    Copyright (c) 1986, 1987, 1988, 1989, 1993 by Keithley Instruments, 
    Inc. Cleveland, Ohio

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
   $Source: /cm/test/build/S600/v420/PROJCOM/RCS/lptparam_lowercase.h,v $
 $Revision: 1.6 $
 Rev $Date: 2000/06/27 19:42:30 $

*/

#ifndef LPTPARAM_LOWERCASE_H
#define LPTPARAM_LOWERCASE_H

#include <lptparam.h>

#define sync_none 		SYNC_NONE
#define sync_full 		SYNC_FULL
#define sync_result 		SYNC_RESULT

#define ref_on			REF_ON
#define ref_off			REF_OFF
#define ref_refresh_meas	REF_REFRESH_MEAS
#define ref_refresh_intg	REF_REFRESH_INTG

#define minmods			MINMODS      

#define ki_trigmode		KI_TRIGMODE 
#define ki_avgnumber		KI_AVGNUMBER
#define ki_avgtime		KI_AVGTIME
#define ki_revimeas		KI_REVIMEAS
#define ki_measure		KI_MEASURE
#define ki_irange		KI_IRANGE
#define ki_qrange		KI_QRANGE
#define ki_zerocheck		KI_ZEROCHECK
#define ki_zerocorrect		KI_ZEROCORRECT
#define ki_suppress		KI_SUPPRESS
#define ki_vforce		KI_VFORCE
#define ki_settriga		KI_SETTRIGA
#define ki_settrigb		KI_SETTRIGB
#define ki_channela		KI_CHANNELA
#define ki_channelb		KI_CHANNELB
#define ki_setgate		KI_SETGATE
#define ki_pointswp		KI_POINTSWP
#define ki_smartclear		KI_SMARTCLEAR
#define ki_psclear		KI_PSCLEAR
#define ki_extinput		KI_EXTINPUT
#define ki_sauto		KI_SAUTO
#define ki_hires		KI_HIRES
#define ki_hicurrent		KI_HICURRENT
#define ki_intgplc		KI_INTGPLC
#define ki_range_settle		KI_RANGE_SETTLE
#define ki_range_delay		KI_RANGE_DELAY
#define ki_opmode		KI_OPMODE
#define ki_cc_auto              KI_CC_AUTO 
#define ki_cc_src_dly           KI_CC_SRC_DLY
#define ki_cc_comp_dly          KI_CC_COMP_DLY
#define ki_cc_meas_dly          KI_CC_MEAS_DLY
#define ki_filter               KI_FILTER 
#define ki_avgmode              KI_AVGMODE
#define ki_lim_indctr           KI_LIM_INDCTR
#define ki_lim_mode             KI_LIM_MODE
#define ki_lim_check            KI_LIM_CHECK
#define ki_ref_select           KI_REF_SELECT
#define ki_ref_move             KI_REF_MOVE
#define ki_mx_defmode   KI_MX_DEFMODE
#define ki_pcainccnt    KI_PCAINCCNT

#define maxmods			MAXMODS
#define nummods			NUMMODS

#define ki_off			KI_OFF
#define ki_on			KI_ON
#define ki_dcon			KI_DCON
#define ki_acon			KI_ACON
#define ki_atten1x		KI_ATTEN1X
#define ki_atten10x		KI_ATTEN10X
#define ki_flton		KI_FLTON
#define ki_fltoff		KI_FLTOFF
#define ki_pslope		KI_PSLOPE
#define ki_nslope		KI_NSLOPE
#define ki_amps			KI_AMPS
#define ki_coul			KI_COUL
#define ki_freqa		KI_FREQA
#define ki_freqb		KI_FREQB
#define ki_freqc		KI_FREQC
#define ki_perioda		KI_PERIODA
#define ki_avgperioda		KI_AVGPERIODA
#define ki_timeatob		KI_TIMEATOB
#define ki_pulsea		KI_PULSEA
#define ki_totabyb		KI_TOTABYB
#define ki_totcum		KI_TOTCUM
#define ki_absolute		KI_ABSOLUTE
#define ki_normal		KI_NORMAL
#define ki_integrate		KI_INTEGRATE
#define ki_average		KI_AVERAGE
#define ki_measx		KI_MEASX
#define ki_value                KI_VALUE
#define ki_indicator            KI_INDICATOR 
#define ki_high         KI_HIGH
#define ki_low          KI_LOW

#define nummodval		NUMMODVAL

#define ki_ioff			KI_IOFF
#define ki_ion			KI_ION
#define ki_idcon		KI_IDCON
#define ki_iacon		KI_IACON
#define ki_iatten1x		KI_IATTEN1X
#define ki_iatten10x    KI_IATTEN10X
#define ki_iflton		KI_IFLTON
#define ki_ifltoff		KI_IFLTOFF
#define ki_ipslope		KI_IPSLOPE
#define ki_inslope		KI_INSLOPE
#define ki_iamps		KI_IAMPS
#define ki_icoul		KI_ICOUL
#define ki_ifreqa		KI_IFREQA
#define ki_ifreqb		KI_IFREQB
#define ki_ifreqc		KI_IFREQC
#define ki_iperioda		KI_IPERIODA
#define ki_iavgperioda  KI_IAVGPERIODA
#define ki_itimeatob    KI_ITIMEATOB
#define ki_ipulsea		KI_IPULSEA
#define ki_itotabyb		KI_ITOTABYB
#define ki_itotcum		KI_ITOTCUM
#define ki_iabsolute    KI_IABSOLUTE
#define ki_inormal		KI_INORMAL
#define ki_iintegrate   KI_IINTEGRATE
#define ki_iaverage		KI_IAVERAGE
#define ki_imeasx		KI_IMEASX
#define ki_ivalue       KI_IVALUE
#define ki_iindicator   KI_IINDICATOR
#define ki_ihigh        KI_IHIGH
#define ki_ilow         KI_ILOW

#define minstats		MINSTATS

#define ki_ipvalue		KI_IPVALUE
#define ki_vpvalue		KI_VPVALUE
#define ki_cprange		KI_CPRANGE
#define ki_gprange		KI_GPRANGE
#define ki_iprange		KI_IPRANGE
#define ki_vprange		KI_VPRANGE
#define ki_carange		KI_CARANGE
#define ki_garange		KI_GARANGE
#define ki_iarange		KI_IARANGE
#define ki_varange		KI_VARANGE
#define ki_cmrange		KI_CMRANGE
#define ki_gmrange		KI_GMRANGE
#define ki_imrange		KI_IMRANGE
#define ki_vmrange		KI_VMRANGE
#define ki_ucbptr		KI_UCBPTR
#define ki_model		KI_MODEL
#define ki_complnc		KI_COMPLNC
#define ki_complnc_ever KI_COMPLNC_EVER
#define ki_pcastate		KI_PCASTATE
#define ki_ref_value    KI_REF_VALUE
#define ki_modelcode    KI_MODELCODE
#define ki_serialno     KI_SERIALNO
#define ki_revision     KI_REVISION
#define ki_mxmap        KI_MXMAP
#define ki_inslist      KI_INSLIST
#define ki_inslist      KI_INSLIST
#define ki_pcacountcycl KI_PCACOUNTCYCL
#define ki_pcacountlife KI_PCACOUNTLIFE
#define ki_pcauserser   KI_PCAUSERSER
#define ki_pcausertype  KI_PCAUSERTYPE
#define ki_pcausercom   KI_PCAUSERCOM

#define maxstats		MAXSTATS
#define numstats		NUMSTATS

#endif

