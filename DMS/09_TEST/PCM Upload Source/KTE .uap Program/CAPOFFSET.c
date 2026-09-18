/* USRLIB MODULE INFORMATION

	MODULE NAME: CAPOFFSET
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "kdf.h"
#include "ktxe_types.h"
#include "ksox_def.h"
#include "COM_usrlib.h"
#define DEBUG 1
#define _REENTRANT
	END USRLIB MODULE INFORMATION
*/
/* USRLIB MODULE HELP DESCRIPTION
CapOffset

DESCRIPTION:
   Call at UAP_WAFER_BEGIN.
   This module finds all "cmax" tests which will be
   called during the next wafer. Each cmax test
   is then executed. Since the probes are up at
   UAP_WAFER_BEGIN, the result of each test is the
   offset capacitance corresponding to each test's
   particular pin configuration. The cmax module
   stores these capacitance offsets in the data
   pool. Later, during the testing of the wafer,
   cmax will pull these offsets from the data pool,
   and adjust the measured results accordingly.

PROTOTYPE:
   void CapOffset(void);

INPUTS:
   None.

OUTPUTS:
   None.

HISTORY:
   9/9/97 - Created. (jwp, jw)
	END USRLIB MODULE HELP DESCRIPTION */
/* USRLIB MODULE PARAMETER LIST */
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "kdf.h"
#include "ktxe_types.h"
#include "ksox_def.h"
#include "COM_usrlib.h"
#define DEBUG 1
#define _REENTRANT

void CAPOFFSET(  )
{
/* USRLIB MODULE CODE */
ktm_list_t *KTMlist;
char *KTMline, *PlaceStr, *MacroNamePtr, *TestSeq;
char CapModule[16][256], ModFileName[256];
FILE *fp, *ModFile;
int *CapOffsetFlag, mods, index, length, status, log_debug_info = 0;
int ch_stat;

/* Check if debug information is being logged to file. */
if ((fp = (FILE *)dpGetPointer("debug_capoffset_file", LONG_P)) != NULL)
   log_debug_info = 1;
if (log_debug_info) fprintf(fp, "\nCAPOFFSET-> Starting CAPOFFSET");

/* Make sure probes are not in contact with wafer */
ch_stat = PrChuck(0);

/* This section guarantees this module is only executed prior to the first wafer,
    and not prior to all wafers. */
CapOffsetFlag = (int *)dpGetPointer ("offset_cap_flag", INT_P);
if (CapOffsetFlag != (int *)NULL) {
   if (log_debug_info) fprintf(fp, "\nCAPOFFSET-> CapOffsetFlag found in dp, exiting CAPOFFSET.");
   return;
} else {
   if (log_debug_info) fprintf(fp, "\nCAPOFFSET-> CapOffsetFlag not found in dp, adding CapOffsetFlag to dp.");
   CapOffsetFlag = (int *)malloc(sizeof(int));
   *CapOffsetFlag = 1;
   dpAddPointer("offset_cap_flag", INT_P, CapOffsetFlag);
} /* endif */

/* Update the status window */
KTXEUpdateStatusAbort("Measuring Offset Capacitances...");

/* Obtain the list of capacitance module names from file "module.cap" */
if (getenv("KI_KTXE_PLANS") != NULL) {
   strcpy (ModFileName, (char *)getenv ("KI_KTXE_PLANS"));
   strcat (ModFileName, "/module.cap");
   ModFile = fopen (ModFileName, "r");
   if (ModFile != (FILE *)NULL) {
      mods = 0;
      while (fgets(CapModule[mods], 256, ModFile) != (char *)NULL) {
         if (CapModule[mods][0] != '#') {
            /* Remove the \n character from the end of the string CapModule string. */
            length = strlen(CapModule[mods]);
            CapModule[mods][length -1] = CapModule[mods][length];
            mods ++;
            if (log_debug_info) fprintf (fp, "\nCAPOFFSET-> Module #%d is %s", mods, CapModule[mods-1]);
         }
      }
   } else {
      if (log_debug_info) fprintf (fp, "\nCAPOFFSET-> Error opening file module.cap. Exiting CAPOFFSET.");
      return;
   }
} else {
   if (log_debug_info) fprintf (fp, "\nCAPOFFSET-> Environment variable error. Exiting CAPOFFSET.");
   return;
}

/* Create space for the current macro name in the data pool */
if (log_debug_info) fprintf(fp, "\nCAPOFFSET-> Adding space for macro name to dp.");
MacroNamePtr = (char *)malloc(256 * sizeof(char));
dpAddPointer ("offset_cap_macro_name", CHAR_P, MacroNamePtr);

/* Get a pointer to the macro list */
if (log_debug_info) fprintf(fp, "\nCAPOFFSET-> Getting pointer to ktm_list.");
KTMlist = (ktm_list_t *) dpGetPointer ("ktm_list", LONG_P);

while (KTMlist != NULL) {
   strcpy (MacroNamePtr, KTMlist->ktmfname);  /* Copy ktm name to the data pool */
   if (log_debug_info) fprintf(fp, "\nCAPOFFSET-> MacroNamePtr = %s", MacroNamePtr);

   /* Get the test execution sequence for a macro */
   TestSeq = (char *)GetKTMExecBuff (KTMlist->ktmfname);
   if (TestSeq == NULL) {
      if (log_debug_info) fprintf(fp, "\nCAPOFFSET-> TestSeq = NULL.");
   } else {

      /* Read the next line in the test sequence */
         KTMline = (char *)strtok_r(TestSeq, "\n", &PlaceStr);
         while (KTMline != NULL) {

         /* If the current test is a cap test, execut it */
         for (index = 0; index < mods; index++) {
               if (strstr(KTMline, CapModule[index]) != NULL) {
                     extern result_info_t *results_head, *results_tail ;
               if (log_debug_info) fprintf(fp, "\nCAPOFFSET-> KTM line to execute: %s", KTMline);
                     results_head = results_tail = NULL ;
                  status = ExecTEE(KTMline, EXECKTXE_COMMAND, NULL);
               } /* endif */
         } /* endfor */
            KTMline = (char *)strtok_r(NULL, "\n", &PlaceStr);
         } /* endwhile */
         free (TestSeq); /* TestSeq is a copy of the actual test sequence ... erase it !  */
   } /* endif */
   KTMlist = KTMlist->next;
} /* endwhile */

strcpy (MacroNamePtr, "DONE_WITH_OFFSETS");
if (log_debug_info) fprintf(fp, "\nCAPOFFSET-> MacroNamePtr = %s", MacroNamePtr);

return;
/* USRLIB MODULE END  */
} 		/* End CAPOFFSET.c */

