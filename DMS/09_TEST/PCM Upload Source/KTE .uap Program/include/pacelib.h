C=======================================================================
C Name        : PACELIB.HDR
C Version     : 001
C Date        : 01-Dec-1989
C Author      : F. Ketting
C Description : This file contains the declarations of variables which
C               are needed in allmost each PACELIB routine
C Change      : W. Peters	Date: 26-2-96
C	      : reduces the maximum sys_leak from 35 ro 5 pA
C=======================================================================

Cmb	INCLUDE '(PACE$INSDEF)/LIST'

	REAL*4
     >   VIMS_RANGE_DELAY(8), ! Delay time per range for VIMS in msec
     >   SYS_CAP,             ! Typical system capacitance 
     >   SYS_RES,             ! Typical system resistance
     >   SYS_LEAK,            ! Typical system leakage current
     >   T1,                  ! Start time in seconds
     >   IMAX_RANGE,          ! Max. current for a MeasI range (Function)
     >   PARAM(50)            ! Function Parameter(s)

        INTEGER*4
     >   PIN_ARRAY(2,25)      ! Pin declarations

        INTEGER*4
     >   TID                  ! Function Test-Identifier

	CHARACTER*1
     >   MEAS_MODE            ! measurement mode (Production/Development)

	DATA SYS_CAP   / 150.0E-12 /	
	DATA SYS_RES   / 100.0E9  /	
	DATA SYS_LEAK  /  35.0E-12  /
	DATA VIMS_RANGE_DELAY /1.0, 1.0, 1.0, 1.0, 1.0,10.0,15.0,85.0/

C=======================================================================
C	End of PACELIB.HDR
C=======================================================================

