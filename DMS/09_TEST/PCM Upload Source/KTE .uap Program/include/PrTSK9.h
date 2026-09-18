/* PrTSK9.h */
/*************************************************************************

	   COPYRIGHT (C) 1993  by  KEITHLEY INSTRUMENTS, INC.
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
...............................................................................

 Function: PrTSK9.h - this file defines data structures and variables that 
	are general for the TSK9 prober
	 
.............................................................................*/



#define CR	0x0D

#define TSK9ERRORMIN		20000		/* base for returned prober-gpib errors */



/* --- FUNCTION PROTOTYPES --- */

int getProberPosition_TSK9( int *xp, int *yp );

/* Serial Poll Status Code Values */
#define TSK9_GOOD_SPOLL 	64
#define TSK9_XY_MC		65
#define TSK9_GOOD_SPOLL		64	/* spoll byte >=64 is a good spoll */

#define TSK9_CLEAR_READY 	64
#define TSK9_MC_X		65
#define TSK9_MC_Y		66
#define TSK9_CHUCK_UP		67
#define TSK9_CHUCK_DOWN		68
#define TSK9_INK_COMP		69
#define TSK9_AT_REF		70
#define TSK9_MC			71
#define TSK9_LOT_END		72
#define TSK9_BEGIN_TEST		73
#define TSK9_FIRST_WAFER	74
#define TSK9_EOW		75
#define TSK9_ALIGN_FAIL		76
#define TSK9_GEN_ERROR		76
#define TSK9_CONSEC_FAIL	77
#define TSK9_OPER_NEEDED	78
#define TSK9_MC_NOT_ON_WAFER	79
#define TSK9_FATAL		80
#define TSK9_Z_NOT_UP		81
#define TSK9_READY_FOR_DATA	82
#define TSK9_AT_Z_LIMIT		83

#define TSK9_SMIF_STOP_OK	85

#define TSK9_READY_FOR_L	96

#define TSK9_LOWER_A_CMD_OK	98
#define TSK9_SMIF_LOCK_OK	TSK9_LOWER_A_CMD_OK
#define TSK9_SMIF_SENSE_OK	TSK9_LOWER_A_CMD_OK

#define TSK9_LOWER_A_CMD_FAIL	99
#define TSK9_SMIF_LOCK_FAIL	TSK9_LOWER_A_CMD_FAIL

#define TSK9_WAFER_NOT_FOUND	101
#define	TSK9_MAN_TEST		104
#define TSK9_PROBE_PAD_ALIGN	105
#define TSK9_READY_CONTACT	106
#define TSK9_READY_FOR_D	106

#define TSK9_SMIF_RESUME_OK	120

#define TSK9_CHUCK_UP_ON	195
#define TSK9_CHUCK_DOWN_ON 	196
#define TSK9_SINGLE_CORRECT 	238
#define TSK9_SINGLE_ERROR	239
#define TSK9_COMMAND_SYNTAX	254
#define TSK9_COMMAND_ERROR	255

#define MAXSLOTS 105 /* index from 1 */
struct TSK9_slot_list_struct
  {
   	int slot_list[MAXSLOTS];
   	int state; /* 0=not inited, 1=inited, 2=first load done, 3=subsequent loads */
   	int curr_slot; /* if state ==1 || 2 || 3, this is the next slot to use */
        int i_load_status ; /* last srq from load next prtskio call */
  } typedef TSK9_slot_list_struct;
