/* HP4284 function prototype and KITT header file */

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

	MODULE NAME: c_forcev
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 2
	ARGUMENTS:
		cmtrx,	int,	Input,	,	,	
		voltage,	float,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "hp4284_internal.h"
extern int HP4284LibInit;
	END USRLIB MODULE INFORMATION
*/
int c_forcev(int cmtrx, float voltage);


/* USRLIB MODULE INFORMATION

	MODULE NAME: c_initialize
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 2
	ARGUMENTS:
		cmtrx,	int,	Input,	,	,	
		initfile,	char *,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "hp4284_internal.h"
#include "KI_license.h"
#define LINELEN 80
#define LIBNAME "HP4284Lib"
static int libNameLicensed = 0 ;
int HP4284LibInit = 0;
	END USRLIB MODULE INFORMATION
*/
int c_initialize(int cmtrx, char * initfile);


/* USRLIB MODULE INFORMATION

	MODULE NAME: c_meascg
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 3
	ARGUMENTS:
		cmtrx,	int,	Input,	,	,	
		reading1,	float *,	Output,	,	,	
		reading2,	float *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "hp4284_internal.h"
char TriggerReading[]="*TRG\n";
extern int HP4284LibInit;
	END USRLIB MODULE INFORMATION
*/
int  c_meascg(int, float *, float *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: setcmtr
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 4
	ARGUMENTS:
		cmtrx,	int,	Input,	,	,	
		subfcn,	int,	Input,	,	,	
		param1,	float,	Input,	,	,	
		param2,	float,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "hp4284_internal.h"
char FuncImp[10]="FUNC:IMP ";
char SelectMode[13][5]={"CsQ ","CsRs","RX  ","CpD ","CpQ ","CpG ","CpRp",
                        "CsD ","ZTD ","GB  ","YTR ","YTD ","ZTR "};
char FuncRng[15]="FUNC:IMP:RANG ";
char AutoRng[22]="FUNC:IMP:RANG:AUTO ON\n";
char IntegrationTime[4][11]={"APER SHOR","APER SHOR,","APER MED ,","APER LONG,"};
char CorrUse[10]="CORR:USE ";
char CorrectionCmd[4][16]={"CORR:OPEN:STAT ","CORR:LOAD:STAT ","CORR:SHOR:STAT "};
char Enable[4]="ON";
char Disable[4]="OFF";
char Freq[6] = "FREQ ";
extern int HP4284LibInit;
	END USRLIB MODULE INFORMATION
*/
int setcmtr(int cmtrx, int subfcn, float param1, float param2);


