/* USRLIB MODULE INFORMATION

	MODULE NAME: wafer_start_pgm
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <stdlib.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <time.h>
#include <kdf.h>
#include <COM_usrlib.h>
#include <string.h>
#include <unistd.h>
#include <netdb.h>
#include <ktxe_proto.h>
#include <ksox_def.h>
#include <time.h>
#include "ktxe_types.h"
#include "kui_proto.h"
#include "COM_usrlib.h"
#include "ktxe_proto.h"


#define debug 0
	END USRLIB MODULE INFORMATION
*/
/* USRLIB MODULE HELP DESCRIPTION
The UAP is UAP_WAFER_BEGIN.

	END USRLIB MODULE HELP DESCRIPTION */
/* USRLIB MODULE PARAMETER LIST */
#include <stdio.h>
#include <stdlib.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <time.h>
#include <kdf.h>
#include <COM_usrlib.h>
#include <string.h>
#include <unistd.h>
#include <netdb.h>
#include <ktxe_proto.h>
#include <ksox_def.h>
#include <time.h>
#include "ktxe_types.h"
#include "kui_proto.h"
#include "COM_usrlib.h"
#include "ktxe_proto.h"


#define debug 0

void wafer_start_pgm(  )
{
/* USRLIB MODULE CODE */
char cmd_line[256]="xterm -T WAFER_TEST_TIMES -n TEST_TIME -e tail -f /user/tmp/WaferTestTimeLog &";
WAFER *wafer_pp = (WAFER *) dpGetPointer("wafer", LONG_P);
LOT *lot = (LOT *)dpGetPointer("lot", LONG_P);
static int pass=0;
FILE *WaferTestTime, *WaferTestPgm;
long ttime;
int hour, min, sec;
char buf[200],buf1[200], *p;
char *lotid;
char tst_name[20],imsi[30],imsi1[30],pcd_name[20];

int i;
int pcd_len;

char    hostname[MAXHOSTNAMELEN];
      
         time_t clock;
         struct tm *tm_p;
         struct tm *tm_p2;
         static char_date[80];
         static lot_date[80];
         clock = time((time_t *)0);
         tm_p = localtime(&clock);
         tm_p2 = localtime(&clock);

         strftime((char *)char_date, sizeof(char_date), "%Y/%m/%d %T", tm_p);
         strftime((char *)lot_date, sizeof(lot_date), "%Y%m%d", tm_p2);
if (gethostname(hostname, MAXHOSTNAMELEN) == -1)
    strcpy(hostname, lot->system);
    for(i = 0 ; i < strlen(hostname) ; i++) hostname[i] = toupper(hostname[i]);
if (wafer_pp->id != NULL) 
         printf("%s|%s|%s|S\n",  char_date,hostname,wafer_pp->id);
lotid = (char*)dpGetPointer("lotid",CHAR_P);
sprintf (buf,"/opt/kiS600/db/FORMAT/log/%s_%s.log",hostname,lot_date);
sprintf (buf1,"/opt/kiS600/db/FORMAT/Testlog/%s_%s.trc",hostname,lot_date);

/* 2005 11 20  by ju  */

lot = (LOT *)dpGetPointer("lot", LONG_P);
    strcpy(imsi1, lot->process);
    pcd_len = strlen(imsi1);
    if(pcd_len < 4)
    strcpy(pcd_name,"NONE");
    else
    sprintf(pcd_name,"%c%c%c%c",imsi1[pcd_len-4],imsi1[pcd_len-3],imsi1[pcd_len-2],imsi1[pcd_len-1]);
 
/* 2005 11 20  by ju  */

WaferTestTime = fopen(buf,"a");
if(WaferTestTime != NULL && wafer_pp->id != NULL)
fprintf(WaferTestTime,"%s|%s|%s|S\n",char_date,hostname,wafer_pp->id);       

strcpy(imsi,lot->testname);
sscanf(imsi,"%s%s",imsi1,tst_name);

WaferTestPgm = fopen(buf1,"a");
if(WaferTestPgm != NULL && wafer_pp->id != NULL)
fprintf(WaferTestPgm,"%s|%s|%s|%s|S|%s|%s\n",char_date,hostname,tst_name,wafer_pp->id,pcd_name,lot->id); 
/*fclose(WaferTestTime);*/

dpAddPointer("Anam_time_log",LONG_P,WaferTestTime);
dpAddPointer("Anam_pgm_log",LONG_P,WaferTestPgm);

/*fflush(stdout);*/
system("chmod 777 /opt/kiS600/db/FORMAT/log/*");
system("chmod 666 /opt/kiS600/db/FORMAT/Testlog/*");

return;




/* USRLIB MODULE END  */
} 		/* End wafer_start_pgm.c */

