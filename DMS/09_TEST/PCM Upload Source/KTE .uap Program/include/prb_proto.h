/* prb_proto.h for s600 */
/*************************************************************************

       COPYRIGHT (C) 1996  by  KEITHLEY INSTRUMENTS, INC.
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

 File:     $Source: /cm/test/build/S600/v420/COMMON/RCS/prb_proto.h,v $
 Current $Revision: 1.15 $
 Current    $State: REL $
 Last Rev    $Date: 2000/02/28 15:29:15 $

 Change       $Log: prb_proto.h,v $
 Change       Revision 1.15  2000/02/28 15:29:15  rybka
 Change       added smif specific protos
 Change
 Change       Revision 1.14  1999/03/05 14:47:06  rybka
 Change       removed the #ifdef for __STDC__ std c libraries
 Change       this used to be need for old non ANSI c
 Change
 Change       Revision 1.13  1998/12/01 20:22:40  rybka
 Change       added protos for PrSetChuckTemp and PrQueryChuckTemp
 Change
 Change       Revision 1.12  1997/04/22 13:06:23  williamson
 Change       Moved from S600 projcom area
 Change
 Change       Revision 1.4  1997/02/07 22:13:51  rybka
 Change       *** empty log message ***
 Change
 * Revision 1.3  1997/02/07  22:12:09  rybka
 * added PrAbsMove prototype
 *
 * Revision 1.2  1997/02/07  19:58:32  witzke
 * Changed floats to doubles
 *
 * Revision 1.1  1996/12/05  14:18:21  witzke
 * Initial revision
 *
...............................................................................

This file is used by KTXE for prototype of prober calls used by the engine.

*/

#ifndef _PRB_PROTO_H
#define _PRB_PROTO_H



/* prober driver */

int PrAbsMove( double x_value, double y_value );
int PrAutoAlign(  );
int PrBeginProbe(  );
int PrCassetteMap( int cassette_number, int *cassette_map );
int PrChuck( int chuck_position );
int PrClearPipeline(  );
int PrCheckOptions(int *OcrPresent, int *AutoAlnPresent,
		int *ProfilerPresent, int *HotchuckPresent,
		int *HandlerPresent, int *Probe2PadPresent );
int PrDisableTransLog();
int PrEnableTransLog();
int PrError( );
int PrGetNxtWafer( int cassette_number );
int PrGetWafer( int cassette_number, int slot_number );
int PrInit( int mode, double x_die_size, double y_die_size, 
	 int x_start_position, int y_start_position, int units, int probtype );
int PrInk( int ink_number );
int PrLearn( int function, int num_site, int *x_site, int *y_site  );
int PrLoad(  );
int PrLoadFailure(  );
int PrLoadProduct( char *file_name, char *drive_name);
int PrMovNxt( int ink_number );
int PrMove( int x_location, int y_location, int ink_number );
int PrOffline(  );
int PrProfile(  );
int PrPutNxtSlot( int cassette_number, int reason_code );
int PrPutWafer( int cassette_number, int slot_number, int reason_code );
int PrReadId( char *user_buf  );
int PrRecallLastIO( char *to_prober, int *to_prober_len, char *from_prober,
	int *from_prober_len);
int PrRelMove( double x_value, double y_value );
int PrRelReturn(  );
int PrReload(  );
int PrReqWaferInfo( int *cassette_number, int *slot_number );
int PrSetDieSize( double x_die_size, double y_die_size );
int PrSetRefDie( int x_start_position, int y_start_position );
int PrSetSlotStatus( int cassette, int slot, int status_code );
int PrSetUnits( int units );
int PrSSLearn( int num_site, int *x_site, int *y_site, int *subsite_num   );
int PrSSLocation( int *x_position, int *y_position, int *subsite_num );
int PrSSMovNxt( int ink_number );
int PrSSMove( int site_num  );
int PrSerialPoll(  );
int PrSetDiam( int diameter  );
int PrSetFlat( int flat_number );
int PrSetMProbe(  );
int PrSetMatrix(  );
int PrSetMode( int mode );
int PrSetPipeline( int on_off );
int PrSetQuadrant( int quad_number );
int PrSetSkipDie( int skip_switch );
int PrSetTime( int new_time );
int PrStatus( int *ready, int *x_location, int *y_location,
	 int *chuck, int *mode );
int PrLoad(  );
int PrUnload(  );
int PrWait(  );
int PrWriteRead( char *input_buf, int input_buf_len,
	 char *output_buf, int output_buf_len, int terminator, int terminator_cnt );
int PrZParams( int function, int value );
int PrZTravel( int number );
int PrSetChuckTemp( double chuck_temp );
int PrQueryChuckTemp( double *chuck_temp );

void EnableTransactionLogging(void);
void DisableTransactionLogging(void);
void EnableErrorLogging(int);

void PrDelay( int s );
int PrSmifClamp( int i_pod_number, int i_lock_state );
int PrSmifLock( int i_pod_number, int i_lock_state );
int PrSmifStatus( int pod_number, int *status_array, int status_array_size );
int PrStart(  );
int PrStop(  );
int PrUnLoad(  );
int PrWriteReadSRQ( char *input_buf, int input_buf_len, char *output_buf, int output_buf_len, int timeout, int *i_srq );

#endif 

