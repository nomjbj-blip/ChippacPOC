/* prbcom function prototype and KITT header file */

/* COPY OF USRLIB SETTINGS INFORMATION */
/* [VISIBILITY] */
/* HIDDEN=1 */
/* [DEPENDENCIES] */
/* LIBS= */
/* OBJECTS= */
/* HEADERS= */
/* [NON-KULT SEARCH PATH] */
/* PATHS= */

/* USRLIB MODULE INFORMATION

	MODULE NAME: Chng_tty_time
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 2
	ARGUMENTS:
		teststation,	int,	Input
		new_time,	int,	Input
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <string.h>
#include <fcntl.h>
#ifndef WIN32	
#include <termios.h>
#endif
#include "prb.h"
#include "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int Chng_tty_time(int teststation, int new_time);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Cnfg_gpib
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		teststation,	int,	Input
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <string.h>
#include "ibup.h"
#include "prb.h"
#include "prb_extern.h"
#define TALKMOD 0x40
#define LISTENMOD 0x20
	END USRLIB MODULE INFORMATION
*/
int Cnfg_gpib(int teststation);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Cnfg_tty
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		teststation,	int,	Input,	,	,	
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <string.h>
#include <fcntl.h>
#ifndef WIN32
#include <termios.h>
#include <sys/types.h>
#include <sys/stat.h>
#endif
#include "prb.h"
#include "prb_extern.h"
#define SERIALTIMEOUTINCR 1 
#ifdef WIN32
#include <windows.h>
#endif
	END USRLIB MODULE INFORMATION
*/
int Cnfg_tty(int teststation);


/* USRLIB MODULE INFORMATION

	MODULE NAME: DisplayErrorMessage
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		error_number,	int,	Input,	
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <stdlib.h>
#include <string.h>
#include "prb.h"
#include "prb_extern.h"
#include "kicommon.h"
	END USRLIB MODULE INFORMATION
*/
int DisplayErrorMessage(int error_number);


/* USRLIB MODULE INFORMATION

	MODULE NAME: drvadr
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include        <stdlib.h>
#include        "prb.h"
#include        "prb_msg.h"
#include        "prb_extern.h"
extern long drvfunc[MAXPROBERFUNCTIONS];
	END USRLIB MODULE INFORMATION
*/
void drvadr();


/* USRLIB MODULE INFORMATION

	MODULE NAME: errmsg
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <stdarg.h>
	END USRLIB MODULE INFORMATION
*/
void errmsg();


/* USRLIB MODULE INFORMATION

	MODULE NAME: filtnprn
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
	END USRLIB MODULE INFORMATION
*/
void filtnprn();


/* USRLIB MODULE INFORMATION

	MODULE NAME: GetConfigInfo
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 2
	ARGUMENTS:
		teststation,	int,	Input,	,	,	
		prober_type,	int *,	Output,	,	,	
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include "prb.h"
#include "prb_msg.h"
#include "prb_extern.h"
#include "kicommon.h"
#include <string.h>
#include <stdlib.h>
	END USRLIB MODULE INFORMATION
*/
int GetConfigInfo(int teststation, int * prober_type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: GetConfigItemI
	MODULE RETURN TYPE: int 
	ARGUMENTS:
		prb_cnfg_str_ptr,	char *,	Input
		totcnfgstr,	int ,	Input
		item_name,	char *,	Input
		itemloc,	int *,	Output
		def_item,	int ,	Input
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <string.h>
#include "prb.h"
#include "prb_msg.h"
	END USRLIB MODULE INFORMATION
*/
int GetConfigItemI(char * prb_cnfg_str_ptr, int  totcnfgstr, char * item_name, int * itemloc, int  def_item);


/* USRLIB MODULE INFORMATION

	MODULE NAME: GetConfigItemS
	MODULE RETURN TYPE: int 
	ARGUMENTS:
		prb_cnfg_str_ptr,	char *,	Input
		totcnfgstr,	int ,	Input
		item_name,	char *,	Input
		itemloc,	char *,	Output
		def_item,	char *,	Input
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <string.h>
#include "prb.h"
#include "prb_msg.h"
	END USRLIB MODULE INFORMATION
*/
int GetConfigItemS(char * prb_cnfg_str_ptr, int  totcnfgstr, char * item_name, char * itemloc, char * def_item);


/* USRLIB MODULE INFORMATION

	MODULE NAME: GetCurrentProberName
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		prbname,	char *,	Output,	
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <string.h>
#include "prb.h"
#include "prb_extern.h"
#include "lptdef.h"
#include "lptdef_lowercase.h"

extern prb_t   Prb[];
extern int     prb_configured[];
	END USRLIB MODULE INFORMATION
*/
void GetCurrentProberName(char * prbname);


/* USRLIB MODULE INFORMATION

	MODULE NAME: GetImageName
	MODULE RETURN TYPE: int 
	ARGUMENTS:
		ImageName,	char *,	Input
		n,	int ,	Input
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <string.h>
	END USRLIB MODULE INFORMATION
*/
int GetImageName(char * ImageName, int  n);


/* USRLIB MODULE INFORMATION

	MODULE NAME: GetPrbConfigItem
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 3
	ARGUMENTS:
		target_file,	char *,	Input,	,	,	
		target_item,	char *,	Input,	,	,	
		result_str,	char *,	Output,	,	,	
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include "prb.h"
#include "prb_msg.h"
#include "prb_extern.h"
#include "kicommon.h"
#include <string.h>
#include <stdlib.h>
	END USRLIB MODULE INFORMATION
*/
int GetPrbConfigItem(char * target_file, char * target_item, char * result_str);


/* USRLIB MODULE INFORMATION

	MODULE NAME: getprbdebug
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <stdlib.h>
#include <string.h>
#include "prb.h"
#include "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
void getprbdebug();


/* USRLIB MODULE INFORMATION

	MODULE NAME: getprberrlvl
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <stdlib.h>
#include <string.h>
#include "prb.h"
#include "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
void getprberrlvl();


/* USRLIB MODULE INFORMATION

	MODULE NAME: GetPrbErrMsg
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 3
	ARGUMENTS:
		error_number,	int,	Input,	,	,	
		c_func_name,	char *,	Input,	 ,	 ,	 
		c_err_msg,	char *,	Output,	 ,	 ,	 
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <stdlib.h>
#include <string.h>
#include "prb.h"
#include "prb_extern.h"
#include "prb_msg.h"
#include "kicommon.h"
	END USRLIB MODULE INFORMATION
*/
int GetPrbErrMsg(int error_number, char * c_func_name, char * c_err_msg);


/* USRLIB MODULE INFORMATION

	MODULE NAME: getPrbStruct
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 2
	ARGUMENTS:
		i_datatype,	int,	Input,	,	,	
		lp_member,	long *,	Output,	,	,	
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <string.h>
#include "ibup.h"
#include "prb.h"
#include "prb_msg.h"
#include "prb_extern.h"
#include "kui_proto.h"
	END USRLIB MODULE INFORMATION
*/
int getPrbStruct(int i_datatype, long * lp_member);


/* USRLIB MODULE INFORMATION

	MODULE NAME: KIGetUserName
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 2
	ARGUMENTS:
		UserName,	char *,	Input,	
		n,	int,	Input,	
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#ifndef WIN32
#include <pwd.h>
#include <unistd.h>
#endif
#include <string.h>
extern char* getenv(const char*);
	END USRLIB MODULE INFORMATION
*/
int KIGetUserName(char * UserName, int n);


/* USRLIB MODULE INFORMATION

	MODULE NAME: LogCmd
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		command_name,	char *,	Input,	,	,	
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "prb.h"
#include "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
void LogCmd(char * command_name);


/* USRLIB MODULE INFORMATION

	MODULE NAME: LogTime
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 2
	ARGUMENTS:
		cmd,	char *,	Input,	,	,	
		i_log,	int,	Input,	,	,	
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "prb.h"
#include "prb_extern.h"
#include <sys/types.h>
#include "wdf.h"
#include "ksox_def.h"
#include <time.h>
#include <sys/systeminfo.h>
#include "ktxe_types.h"

                                  
typedef struct _time_S600
{
long secs;
long usecs;
int inited;
int logging;
char c_function[48];
struct _time_S600 *next;
} time_S600_t ;

	END USRLIB MODULE INFORMATION
*/
int LogTime(char * cmd, int i_log);


/* USRLIB MODULE INFORMATION

	MODULE NAME: LogTrans
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 3
	ARGUMENTS:
		trans_type,	int,	Input,	
		buf,	char *,	Input,	
		buflen,	int,	Input,	
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <string.h>
#include "prb.h"
#include "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
void LogTrans(int trans_type, char * buf, int buflen);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Pr_anint
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		value,	double,	Input,	
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#ifdef WIN32
_declspec (dllexport)
#endif
	END USRLIB MODULE INFORMATION
*/
double Pr_anint(double value);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Pr_nint
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		value,	double,	Input,	
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <math.h>
#ifdef WIN32
_declspec (dllexport)
#endif
	END USRLIB MODULE INFORMATION
*/
int Pr_nint(double value);


/* USRLIB MODULE INFORMATION

	MODULE NAME: prabsinit
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 2
	ARGUMENTS:
		d_x_home,	double,	Input,	,	,
		d_y_home,	double,	Input,	,	,
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "prb.h"
#include "prb_extern.h"
#ifdef WIN32
_declspec (dllexport)
#endif
	END USRLIB MODULE INFORMATION
*/
void prabsinit(double d_x_home, double d_y_home);


/* USRLIB MODULE INFORMATION

	MODULE NAME: prb_dec
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include "prb.h"
int   drvfunc[MAXPROBERFUNCTIONS];
int    (**drvtab[MAXPROBERFUNCTIONS])();
#ifdef WIN32 
_declspec (dllexport) 
#endif 
prb_t   Prb[MAXPROBERS + 1];
#ifdef WIN32 
_declspec (dllexport) 
#endif 
int     prb_configured[ MAXPROBERS + 1 ] = {0}; 
FILE    *trans_log_fp, *err_log_fp;
char    *exec_filename; 
#ifdef WIN32 
_declspec (dllexport) 
#endif 
char    xoutput_buffer[ BUFFER_SIZE ]; 
#ifdef WIN32 
_declspec (dllexport) 
#endif 
char    xinput_buffer[ BUFFER_SIZE ]; 
#ifdef WIN32 
_declspec (dllexport) 
#endif 
int     xoutput_buffer_len; 
#ifdef WIN32 
_declspec (dllexport) 
#endif 
int		xinput_buffer_len;
int     prb_debug_print_flag;
int     prb_debug_trans_flag;
int     prb_debug_cmnds_flag;
int     prb_timing_flag = FALSE;
int     prb_timing_init = -1;

#include "prb_func_id.h"

                                                                                                                                                                                                                                           

PRBFUNCS xref_prb_func[PRMAXFUNCIDS+1] = {

{"PrAutoAlign", PRAUTOALIGN },
{"PrCassetteMap", PRCASSETTEMAP },
{"PrCheckOptions", PRCHECKOPTIONS },
{"PrChuck", PRCHUCK	 },
{"PrClearPipeline", PRCLEARPIPELINE },
{"PrError", PRERROR },
{"PrGetNxtWafer", PRGETNXTWAFER },
{"PrGetWafer", PRGETWAFER },
{"PrInit", PRINIT },
{"PrLoad", PRLOAD },
{"PrLoadProduct", PRLOADPRODUCT },
{"PrMove", PRMOVE },
{"PrProfile", PRPROFILE },
{"PrPutNxtSlot", PRPUTNXTSLOT },
{"PrPutWafer", PRPUTWAFER },
{"PrReadId", PRREADID },
{"PrRelMove", PRRELMOVE },
{"PrRelReturn", PRRELRETURN },
{"PrSerialPoll", PRSERIALPOLL },
{"PrSetDiam", PRSETDIAM },
{"PrSetDieSize", PRSETDIESIZE },
{"PrSetFlat", PRSETFLAT },
{"PrSetMode", PRSETMODE },
{"PrSetPipeline", PRSETPIPELINE },
{"PrSetQuadrant", PRSETQUADRANT },
{"PrSetRefDie", PRSETREFDIE },
{"PrSetSlotStatus", PRSETSLOTSTATUS },
{"PrSetTime", PRSETTIME },
{"PrSetUnits", PRSETUNITS },
{"PrStatus", PRSTATUS },
{"PrUnLoad", PRUNLOAD },
{"PrWriteRead", PRWRITEREAD },
{"PrZParams", PRZPARAMS },
{"PrZTravel",PRZTRAVEL },
{"PrAbsMove", PRABSMOVE },
{"PrSmifLock", PRSMIFLOCK },
{"PrSmifLockStatus", PRSMIFLOCKSTATUS },
{"PrProberStatus", PRPROBERSTATUS },
{"PrSenseWafer", PRSENSEWAFER },
{"PrStop", PRSTOP },
{"PrStart", PRSTART },
{"PrWriteReadSRQ", PRWRITEREADSRQ },
{"",0 }
};

	END USRLIB MODULE INFORMATION
*/
void prb_dec();


/* USRLIB MODULE INFORMATION

	MODULE NAME: prbdlginit
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <stdarg.h>
#include <string.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "prb.h"
#include "prb_func_id.h"
#include "kicommon.h"
#ifdef I4200
#include <windows.h>
#include "sharedClassName.h"
#endif
char prb_func_msg [PRMAXFUNCIDS+1][100];
extern int kloc_readfile(char* sz_in, char* c_name, char* c_val, int i_rec_no);
	END USRLIB MODULE INFORMATION
*/
void prbdlginit();


/* USRLIB MODULE INFORMATION

	MODULE NAME: prbsel
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		teststation,	int,	Input,	
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#ifdef S400
#include "lpt_proto.h"
#else
#include "lptproto.h"
#endif
	END USRLIB MODULE INFORMATION
*/
void prbsel(int teststation);


/* USRLIB MODULE INFORMATION

	MODULE NAME: prbsrqinit
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#define _REENTRANT
#include <stdio.h>
#include <stdarg.h>
#include <string.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "prb.h"
#include "prb_func_id.h"
#include "kicommon.h"
#include "prb_extern.h"
#include "prb_msg.h"
#ifdef I4200
#include <windows.h>
#endif
extern int kloc_readfile(char* sz_in, char* c_name, char* c_val, int i_rec_no);
#ifdef WIN32
	extern char *strtok_r(char *, char *, char **);
#endif

pr_srq_list pr_srq[PRMAXFUNCIDS+1];

	END USRLIB MODULE INFORMATION
*/
int prbsrqinit();


/* USRLIB MODULE INFORMATION

	MODULE NAME: prconnectionok
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		retryCount,	int,	Input,	
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <string.h>
#include <ctype.h>
#include "ibup.h"
#include "prb.h"
#include "prb_msg.h"
#include "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int prconnectionok(int retryCount);


/* USRLIB MODULE INFORMATION

	MODULE NAME: prdatetime
	MODULE RETURN TYPE: void 
	ARGUMENTS:
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <time.h>
	END USRLIB MODULE INFORMATION
*/
void prdatetime();


/* USRLIB MODULE INFORMATION

	MODULE NAME: pregio
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 8
	ARGUMENTS:
		dataOut,	char *,	Input,	
		outLen,	int,	Input,	
		dataIn,	char *,	Input,	
		inLen,	int,	Input,	
		term,	int,	Input,	
		term_ct,	int,	Input,	
		i_srq,	int *,	Output,	
		timeout,	int,	Input,	
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <string.h>
#include "ibup.h"
#include "prb.h"
#include "prb_msg.h"
#include "prb_extern.h"
#define LF 10                         
	END USRLIB MODULE INFORMATION
*/
int pregio(char * dataOut, int outLen, char * dataIn, int inLen, int term, int term_ct, int * i_srq, int timeout);


/* USRLIB MODULE INFORMATION

	MODULE NAME: prgpibcom
	MODULE RETURN TYPE: int 
	ARGUMENTS:
		dataOut,	char *,	Input
		outLen,	int ,	Input
		dataIn,	char *,	Input
		inLen,	int ,	Input
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <string.h>
#include "ibup.h"
#include "prb.h"
#include "prb_msg.h"
#include "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int prgpibcom(char * dataOut, int  outLen, char * dataIn, int  inLen);


/* USRLIB MODULE INFORMATION

	MODULE NAME: prgpibcomsrq
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 6
	ARGUMENTS:
		dataOut,	char *,	Input,	
		outLen,	int,	Input,	
		dataIn,	char *,	Input,	
		inLen,	int,	Input,	
		timeout,	int,	Input,	
		srq,	int *,	Output,	
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <string.h>
#include "ibup.h"
#include "prb.h"
#include "prb_msg.h"
#include "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int prgpibcomsrq(char * dataOut, int outLen, char * dataIn, int inLen, int timeout, int * srq);


/* USRLIB MODULE INFORMATION

	MODULE NAME: printtranscmnds
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <math.h>
int prb_debug_print_flag;
int prb_debug_trans_flag;
int prb_debug_cmnds_flag;
	END USRLIB MODULE INFORMATION
*/
void printtranscmnds();


/* USRLIB MODULE INFORMATION

	MODULE NAME: ProbConfig
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		teststation,	int *,	Input,	,	,	
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <dlfcn.h>
#include        "prb.h"
#include        "prb_msg.h"
#include        "prb_extern.h"
#include        "kicommon.h"
#include "lptdef.h"
#include "lptdef_lowercase.h"

#ifdef WIN32
#include <windows.h>
#include <winbase.h>
#endif
extern long    drvfunc[];
extern int     (**drvtab[])();
extern prb_t   Prb[];
extern prb_ni_t        prb_nameid[];
extern int     prb_configured[];
extern FILE    *trans_log_fp;
extern FILE    *err_log_fp;
extern char    *exec_filename;
extern char    xoutput_buffer[];
extern char    xinput_buffer[];
extern int     xoutput_buffer_len;
extern int 	   xinput_buffer_len;
	END USRLIB MODULE INFORMATION
*/
int ProbConfig(int * teststation);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ProbConfigByName
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		prober_name,	char *,	Input,	
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
o#endif
#include <stdio.h>
#include <stdlib.h>
#ifndef WIN32
#include <dlfcn.h>
#endif
#include "prb.h"
#include "prb_extern.h"
#include "prb_msg.h"
#include "kicommon.h"
extern int     prb_configured[];
extern prb_t Prb[];
	END USRLIB MODULE INFORMATION
*/
int ProbConfigByName(char * prober_name);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ProberError
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 3
	ARGUMENTS:
		err_num,	int,	Input,	,	,	
		func_name,	char *,	Input,	,	 ,	 
		opt_param,	int,	Input,	,	,	
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <string.h>
#include "prb.h"
#include "prb_extern.h"
#include "prb_msg.h"
#include "kicommon.h"
#include "lptdef.h"
#include "lptdef_lowercase.h"
#define MAX_PARAMS  8
#define MSG_TEXT_LEN    255
extern char* getenv(const char*);
extern void getPRBerrfile(char* );
	END USRLIB MODULE INFORMATION
*/
void ProberError(int err_num, char * func_name, int opt_param);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ProberLogging
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <string.h>
#include <stdlib.h>
#include "prb.h"
#include "prb_extern.h"
#include "kicommon.h"
#include "lptdef.h"
#include "lptdef_lowercase.h"
	END USRLIB MODULE INFORMATION
*/
void ProberLogging();


/* USRLIB MODULE INFORMATION

	MODULE NAME: ProbFuncNotSup
	MODULE RETURN TYPE: int 
	ARGUMENTS:
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include        "prb.h"
#include        "prb_extern.h"
#include        "prb_msg.h"
 
	END USRLIB MODULE INFORMATION
*/
int ProbFuncNotSup();


/* USRLIB MODULE INFORMATION

	MODULE NAME: ProbUnconfig
	MODULE RETURN TYPE: void 
	ARGUMENTS:
		teststation,	int ,	Input
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "prb.h"
#include "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
void ProbUnconfig(int  teststation);


/* USRLIB MODULE INFORMATION

	MODULE NAME: prparseoptions
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 3
	ARGUMENTS:
		s_to_parse,	char *,	Input
		i_position,	int,	Input
		s_result,	char *,	Output
	INCLUDES:
#define _REENTRANT
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include        <string.h>
                                                                                                                                     
	END USRLIB MODULE INFORMATION
*/
int prparseoptions(char * s_to_parse, int i_position, char * s_result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: prproberready
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include        <string.h>
#include        <time.h>
#include        "prb.h"
#include        "prb_msg.h"
#include        "prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int prproberready();


/* USRLIB MODULE INFORMATION

	MODULE NAME: prquerygpib
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		timeout,	int,	Input,	
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include        <time.h>
#include        "ibup.h"
#include        "prb.h"
#include        "prb_msg.h"
#include		"prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int prquerygpib(int timeout);


/* USRLIB MODULE INFORMATION

	MODULE NAME: prspoll
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include        <time.h>
#include        "ibup.h"
#include        "prb.h"
#include        "prb_msg.h"
#include		"prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int prspoll();


/* USRLIB MODULE INFORMATION

	MODULE NAME: pruniversalio
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 8
	ARGUMENTS:
		dataOut,	char *,	Input,	
		outLen,	int,	Input,	
		dataIn,	char *,	Input,	
		inLen,	int,	Input,	
		term,	int,	Input,	
		term_ct,	int,	Input,	
		i_srq,	int *,	Output,	
		timeout,	int,	Input,	
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <string.h>
#include "ibup.h"
#include "prb.h"
#include "prb_msg.h"
#include "prb_extern.h"
#define LF 10                         
	END USRLIB MODULE INFORMATION
*/
int pruniversalio(char * dataOut, int outLen, char * dataIn, int inLen, int term, int term_ct, int * i_srq, int timeout);


/* USRLIB MODULE INFORMATION

	MODULE NAME: prwaitsrqclear
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include        <time.h>
#include        "ibup.h"
#include        "prb.h"
#include        "prb_msg.h"
#include		"prb_extern.h"
	END USRLIB MODULE INFORMATION
*/
int prwaitsrqclear();


/* USRLIB MODULE INFORMATION

	MODULE NAME: ReadErrorMessage
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 3
	ARGUMENTS:
		error_number,	int,	Input,	
		error_msg,	char *,	Input,	
		n,	int,	Input,	
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <string.h>
#include <stdlib.h>
#include "prb.h"
#include "prb_extern.h"
#include "kicommon.h"
	END USRLIB MODULE INFORMATION
*/
int ReadErrorMessage(int error_number, char * error_msg, int n);


/* USRLIB MODULE INFORMATION

	MODULE NAME: sendrec
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 6
	ARGUMENTS:
		output_buffer,	char *,	Input,	"",	,	
		output_buffer_len,	int,	Input,	,	,	
		input_buffer,	char *,	Input,	"",	,	
		input_buffer_len,	int,	Input,	,	,	
		iterminator,	int,	Input,	,	,	
		terminator_count,	int,	Input,	,	,	
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <time.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <string.h>
#include <fcntl.h>

#ifndef WIN32
#include <termios.h>
#else
#include <windows.h> // WM
#endif
#include <memory.h>
#include "prb.h"
#include "prb_extern.h"
#ifndef WIN32
#include <errno.h>
#endif
  
#define ERR_GENERAL     (-1)
#define ERR_TIMEOUT     (-20)
	END USRLIB MODULE INFORMATION
*/
int sendrec(char * output_buffer, int output_buffer_len, char * input_buffer, int input_buffer_len, int iterminator, int terminator_count);


/* USRLIB MODULE INFORMATION

	MODULE NAME: set_error_level
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <stdlib.h>
#include <string.h>
#include "prb.h"
#include "prb_extern.h"
#include "kicommon.h"
	END USRLIB MODULE INFORMATION
*/
void set_error_level();


