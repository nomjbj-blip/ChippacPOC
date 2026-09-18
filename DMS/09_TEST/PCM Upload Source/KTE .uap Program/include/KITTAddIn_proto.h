/* KITTAddIn function prototype and KITT header file */

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

	MODULE NAME: kiexard
	MODULE RETURN TYPE: double 
	ARGUMENTS:
		arr,	D_ARRAY_T,	Input
		arr_len,	int ,	Input
		index,	int ,	Input
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double kiexard(double *arr, int  arr_len, int  index);


/* USRLIB MODULE INFORMATION

	MODULE NAME: kiexarf
	MODULE RETURN TYPE: float 
	ARGUMENTS:
		arr,	F_ARRAY_T,	Input
		arr_len,	int ,	Input
		index,	int ,	Input
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
float kiexarf(float *arr, int  arr_len, int  index);


/* USRLIB MODULE INFORMATION

	MODULE NAME: kiexari
	MODULE RETURN TYPE: int 
	ARGUMENTS:
		arr,	I_ARRAY_T,	Input
		arr_len,	int ,	Input
		index,	int ,	Input
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <values.h>
	END USRLIB MODULE INFORMATION
*/
int kiexari(int *arr, int  arr_len, int  index);


/* USRLIB MODULE INFORMATION

	MODULE NAME: kiinard
	MODULE RETURN TYPE: void 
	ARGUMENTS:
		val,	double ,	Input
		arr,	D_ARRAY_T,	Input
		arr_len,	int ,	Input
		index,	int ,	Input
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void kiinard(double  val, double *arr, int  arr_len, int  index);


/* USRLIB MODULE INFORMATION

	MODULE NAME: kiinarf
	MODULE RETURN TYPE: void 
	ARGUMENTS:
		val,	float ,	Input
		arr,	F_ARRAY_T,	Input
		arr_len,	int ,	Input
		index,	int ,	Input
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void kiinarf(float  val, float *arr, int  arr_len, int  index);


/* USRLIB MODULE INFORMATION

	MODULE NAME: kiinari
	MODULE RETURN TYPE: void 
	ARGUMENTS:
		val,	int ,	Input
		arr,	I_ARRAY_T,	Input
		arr_len,	int ,	Input
		index,	int ,	Input
	INCLUDES:
#ifdef WIN32
#include "ktemalloc.h"
#endif
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void kiinari(int  val, int *arr, int  arr_len, int  index);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PT_loadgdf
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		gdffile,	char *,	Input,	
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
#include <COM_usrlib.h>
	END USRLIB MODULE INFORMATION
*/
void PT_loadgdf(char * gdffile);


/* USRLIB MODULE INFORMATION

	MODULE NAME: PT_loadpcf
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		pcffile,	char *,	Input,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>

#include "ktxe_types.h"
	END USRLIB MODULE INFORMATION
*/
void PT_loadpcf(char * pcffile);


