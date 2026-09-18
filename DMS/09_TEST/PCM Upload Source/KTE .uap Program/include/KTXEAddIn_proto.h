/* KTXEAddIn function prototype and KITT header file */

/* COPY OF USRLIB SETTINGS INFORMATION */
/* [VISIBILITY] */
/* HIDDEN=0 */
/* [DEPENDENCIES] */
/* LIBS= */
/* OBJECTS= */
/* HEADERS= */
/* [NON-KULT SEARCH PATH] */
/* PATHS= */

/* USRLIB MODULE INFORMATION

	MODULE NAME: KTXEAbortExitHdlr
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
#include "COM_usrlib.h"
#include "wtype.h"
#include "ki_gem.h"
	END USRLIB MODULE INFORMATION
*/
void KTXEAbortExitHdlr();


/* USRLIB MODULE INFORMATION

	MODULE NAME: KTXEAppendStopTime
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		lot,	long *,	Input,	,	,	
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "kdf.h"
	END USRLIB MODULE INFORMATION
*/
void KTXEAppendStopTime(long * lot);


/* USRLIB MODULE INFORMATION

	MODULE NAME: KTXEErrorMessage
	MODULE RETURN TYPE: void 
	ARGUMENTS:
		message,	char *,	Input
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "ktxe_proto.h"
	END USRLIB MODULE INFORMATION
*/
void KTXEErrorMessage(char * message);


/* USRLIB MODULE INFORMATION

	MODULE NAME: KTXEEventMessage
	MODULE RETURN TYPE: void 
	ARGUMENTS:
		message,	char *,	Input
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "ktxe_proto.h"
	END USRLIB MODULE INFORMATION
*/
void KTXEEventMessage(char * message);


/* USRLIB MODULE INFORMATION

	MODULE NAME: KTXEExtSetWaferId
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include "ktxe_types.h"
#include "kui_proto.h"

#include "COM_usrlib.h"
	END USRLIB MODULE INFORMATION
*/
void KTXEExtSetWaferId();


/* USRLIB MODULE INFORMATION

	MODULE NAME: KTXEGetProberData
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "prb.h"
#include "kui_proto.h"
	END USRLIB MODULE INFORMATION
*/
void KTXEGetProberData();


/* USRLIB MODULE INFORMATION

	MODULE NAME: KTXEGetProductFile
	MODULE RETURN TYPE: int 
	ARGUMENTS:
		lot,	long *,	Input
		product_file,	char *,	Input
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <string.h>
#include "kdf.h"
#include "kui_proto.h"
#include "guidedef.h"
	END USRLIB MODULE INFORMATION
*/
int KTXEGetProductFile(long * lot, char * product_file);


/* USRLIB MODULE INFORMATION

	MODULE NAME: KTXELogCPFname
	MODULE RETURN TYPE: void 
	ARGUMENTS:
		cpf_info,	long *,	Input
		lot,	long *,	Input
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "kdf.h"
#include "ktxe_types.h"
	END USRLIB MODULE INFORMATION
*/
void KTXELogCPFname(long * cpf_info, long * lot);


/* USRLIB MODULE INFORMATION

	MODULE NAME: KTXELogWaferOrientation
	MODULE RETURN TYPE: void 
	ARGUMENTS:
		wdfptr,	long *,	Input
		lot,	long *,	Input
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "kdf.h"
#include "wdf.h"
	END USRLIB MODULE INFORMATION
*/
void KTXELogWaferOrientation(long * wdfptr, long * lot);


/* USRLIB MODULE INFORMATION

	MODULE NAME: KTXELotEndTime
	MODULE RETURN TYPE: void 
	ARGUMENTS:
		lot_starttime,	long ,	Input
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <time.h>
#include <COM_usrlib.h>
	END USRLIB MODULE INFORMATION
*/
void KTXELotEndTime(long  lot_starttime);


/* USRLIB MODULE INFORMATION

	MODULE NAME: KTXELotStartTime
	MODULE RETURN TYPE: void 
	ARGUMENTS:
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <time.h>
#include <ksox_def.h>
	END USRLIB MODULE INFORMATION
*/
void KTXELotStartTime();


/* USRLIB MODULE INFORMATION

	MODULE NAME: KTXEOperatorLoadAlign
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
#include <kui_proto.h>
#include "prb.h"
#include "prb_extern.h"
#include <COM_usrlib.h>
	END USRLIB MODULE INFORMATION
*/
int KTXEOperatorLoadAlign();


/* USRLIB MODULE INFORMATION

	MODULE NAME: KTXEPrbErrHdlr
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
#include "kui_proto.h"
#include "COM_usrlib.h"
#include "prb.h"
#include "prb_extern.h"
#include "wtype.h"
#include "ki_gem.h"
#ifdef WIN32
_declspec (dllexport)
#endif
	END USRLIB MODULE INFORMATION
*/
void KTXEPrbErrHdlr();


/* USRLIB MODULE INFORMATION

	MODULE NAME: KTXEShowSlotList
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
#include <ktxe_types.h>
#include <COM_usrlib.h>
	END USRLIB MODULE INFORMATION
*/
void KTXEShowSlotList();


/* USRLIB MODULE INFORMATION

	MODULE NAME: KTXEShowSStimeEOW
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
#include <COM_usrlib.h>
	END USRLIB MODULE INFORMATION
*/
void KTXEShowSStimeEOW();


/* USRLIB MODULE INFORMATION

	MODULE NAME: KTXEShowSubsiteTime
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
#include <time.h>
#include <kdf.h>
#include <COM_usrlib.h>
	END USRLIB MODULE INFORMATION
*/
void KTXEShowSubsiteTime();


/* USRLIB MODULE INFORMATION

	MODULE NAME: KTXESkipNextWaferLoad
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
#include "kui_proto.h"
#include "COM_usrlib.h"
#include "prb.h"
#include "prb_extern.h"
#include "wtype.h"
#include "ki_gem.h"
#ifdef WIN32
_declspec (dllexport)
#endif
	END USRLIB MODULE INFORMATION
*/
void KTXESkipNextWaferLoad();


/* USRLIB MODULE INFORMATION

	MODULE NAME: KTXESummaryReport
	MODULE RETURN TYPE: int 
	ARGUMENTS:
		lotid,	char *,	Input
		sum_report_options,	char *,	Input
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "kdf.h"
	END USRLIB MODULE INFORMATION
*/
int KTXESummaryReport(char * lotid, char * sum_report_options);


/* USRLIB MODULE INFORMATION

	MODULE NAME: KTXEWaferEndTime
	MODULE RETURN TYPE: void 
	ARGUMENTS:
		wafer_starttime,	long ,	Input
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <time.h>
#include <kdf.h>
#include <COM_usrlib.h>
	END USRLIB MODULE INFORMATION
*/
void KTXEWaferEndTime(long  wafer_starttime);


/* USRLIB MODULE INFORMATION

	MODULE NAME: KTXEWaferStartTime
	MODULE RETURN TYPE: void 
	ARGUMENTS:
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <time.h>
#include <ksox_def.h>
	END USRLIB MODULE INFORMATION
*/
void KTXEWaferStartTime();


/* USRLIB MODULE INFORMATION

	MODULE NAME: ProbeCardChange
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <COM_usrlib.h>
#include <kui_proto.h>
#include <pcutil.h>
#include <KI_license.h>

int  scroll_msg_parent=0;                                     
int  stand_alone=0;                                    
char ktxe_will_now_exit[32];                            
                                
int  (*COM_GetModuleAddr( char *lib_name, char *module_name ))();
int  pccLeakageSetSelect(int *lkgset_id);
int  pccChangeInstructions(void);
int  pccKelvinTestRun(void);
void pccCleanup(void);
int  pccChangeInstructionsOverride(void);
int  pccIdentification(int *lkgset_id, ActiveProbecardInfo *info);
int  pccInfoDisplay(int lkgset_id, ActiveProbecardInfo *info);
int  pccLeakageSetSet(int lkgset_id);
int  pccInfoDlg(char *msg);
int  pccOverrideDlg(char *msg);
int  pccChangeDlg(char *msg);
	END USRLIB MODULE INFORMATION
*/
void ProbeCardChange();


/* USRLIB MODULE INFORMATION

	MODULE NAME: ProbeCardDetect
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <COM_usrlib.h>
#include <kui_proto.h>
#include <pcutil.h>
int (*COM_GetModuleAddr( char *lib_name, char *module_name ))();
	END USRLIB MODULE INFORMATION
*/
void ProbeCardDetect();


/* USRLIB MODULE INFORMATION

	MODULE NAME: ProbeCardIDConfirm
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <COM_usrlib.h>
#include <kui_proto.h>

#define MAXBUF	64
	END USRLIB MODULE INFORMATION
*/
void ProbeCardIDConfirm();


/* USRLIB MODULE INFORMATION

	MODULE NAME: ProbeCardQuery
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <COM_usrlib.h>
#include <kui_proto.h>
#include <pcutil.h>
	END USRLIB MODULE INFORMATION
*/
void ProbeCardQuery();


