/* KI_UAPLIB function prototype and KITT header file */

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

	MODULE NAME: check_maxmins
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <stdlib.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "kui_proto.h"
#include "COM_usrlib.h"
#include "ktxe_types.h"
	END USRLIB MODULE INFORMATION
*/
void check_maxmins();


/* USRLIB MODULE INFORMATION

	MODULE NAME: clr_scroll
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "kui_proto.h"
#include "guidedef.h"
	END USRLIB MODULE INFORMATION
*/
void clr_scroll();


/* USRLIB MODULE INFORMATION

	MODULE NAME: get_notch_loc
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "kui_proto.h"
#include "COM_usrlib.h"
	END USRLIB MODULE INFORMATION
*/
int get_notch_loc();


/* USRLIB MODULE INFORMATION

	MODULE NAME: get_notch_type
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "kui_proto.h"
#include "COM_usrlib.h"
	END USRLIB MODULE INFORMATION
*/
int get_notch_type();


/* USRLIB MODULE INFORMATION

	MODULE NAME: get_org_col
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "kui_proto.h"
#include "COM_usrlib.h"
	END USRLIB MODULE INFORMATION
*/
int get_org_col();


/* USRLIB MODULE INFORMATION

	MODULE NAME: get_org_row
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "kui_proto.h"
#include "COM_usrlib.h"
	END USRLIB MODULE INFORMATION
*/
int get_org_row();


/* USRLIB MODULE INFORMATION

	MODULE NAME: get_site_col
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <stdlib.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "kui_proto.h"
#include "COM_usrlib.h"
	END USRLIB MODULE INFORMATION
*/
int get_site_col();


/* USRLIB MODULE INFORMATION

	MODULE NAME: get_site_row
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "kui_proto.h"
#include "COM_usrlib.h"
	END USRLIB MODULE INFORMATION
*/
int get_site_row();


/* USRLIB MODULE INFORMATION

	MODULE NAME: get_tgt_col
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "kui_proto.h"
#include "COM_usrlib.h"
	END USRLIB MODULE INFORMATION
*/
int get_tgt_col();


/* USRLIB MODULE INFORMATION

	MODULE NAME: get_tgt_row
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "kui_proto.h"
#include "COM_usrlib.h"
	END USRLIB MODULE INFORMATION
*/
int get_tgt_row();


/* USRLIB MODULE INFORMATION

	MODULE NAME: KI_CheckResWithLimits
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 4
	ARGUMENTS:
		Result_Name,	char *,	Input
		Result_Value,	float,	Input
		Limit_Code,	int,	Input
		Limit_List,	char *,	Input
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "kdf.h"
#include "ksox_def.h"
	END USRLIB MODULE INFORMATION
*/
int KI_CheckResWithLimits(char * Result_Name, float Result_Value, int Limit_Code, char * Limit_List);


/* USRLIB MODULE INFORMATION

	MODULE NAME: KI_CreateLimitSubList
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 2
	ARGUMENTS:
		SubList_Name,	char *,	Input
		critical_flag,	int,	Input
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "kdf.h" 
#include "ksox_def.h"
	END USRLIB MODULE INFORMATION
*/
void KI_CreateLimitSubList(char * SubList_Name, int critical_flag);


/* USRLIB MODULE INFORMATION

	MODULE NAME: KI_Remove_SSTest_Passed_flag
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "ksox_def.h"
	END USRLIB MODULE INFORMATION
*/
void KI_Remove_SSTest_Passed_flag();


/* USRLIB MODULE INFORMATION

	MODULE NAME: KI_RemoveLimitSubList
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		SubList_Name,	char *,	Input
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "kdf.h"
#include "ksox_def.h"
	END USRLIB MODULE INFORMATION
*/
void KI_RemoveLimitSubList(char * SubList_Name);


/* USRLIB MODULE INFORMATION

	MODULE NAME: KI_SubsiteTest
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		Subsite_id,	char *,	Input
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "ktxe_types.h"
#include "ksox_def.h"
	END USRLIB MODULE INFORMATION
*/
void KI_SubsiteTest(char * Subsite_id);


/* USRLIB MODULE INFORMATION

	MODULE NAME: KRT_Wafmap_Init
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <stdlib.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "kui_proto.h"
#include "COM_usrlib.h"
#include "ktxe_types.h"
	END USRLIB MODULE INFORMATION
*/
void KRT_Wafmap_Init();


/* USRLIB MODULE INFORMATION

	MODULE NAME: KRT_Wafmap_Update
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		pass_test,	int,	Input
	INCLUDES:
#include <stdio.h>
#include <stdlib.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "kui_proto.h"
#include "COM_usrlib.h"
#include "ktxe_types.h"
	END USRLIB MODULE INFORMATION
*/
void KRT_Wafmap_Update(int pass_test);


/* USRLIB MODULE INFORMATION

	MODULE NAME: scroll_window
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 2
	ARGUMENTS:
		szText,	char *,	Input
		append,	int,	Input
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "kui_proto.h"
#include "guidedef.h"
	END USRLIB MODULE INFORMATION
*/
void scroll_window(char * szText, int append);


/* USRLIB MODULE INFORMATION

	MODULE NAME: set_scroll_win_title
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		title,	char *,	Input
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void set_scroll_win_title(char * title);


/* USRLIB MODULE INFORMATION

	MODULE NAME: varOKbox
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		szText,	char *,	Input
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void varOKbox(char * szText);


