/* USRLIB MODULE INFORMATION

	MODULE NAME: Check_XYsize
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <string.h>
#include <unistd.h>
#include <netdb.h>
#include <math.h>
#include <ktxe_proto.h>
#include <ksox_def.h>
#include <time.h>
#include <wdf.h>
#include <prbgen_proto.h>
	END USRLIB MODULE INFORMATION
*/
/* USRLIB MODULE HELP DESCRIPTION

	END USRLIB MODULE HELP DESCRIPTION */
/* USRLIB MODULE PARAMETER LIST */
#include <stdio.h>
#include <string.h>
#include <unistd.h>
#include <netdb.h>
#include <math.h>
#include <ktxe_proto.h>
#include <ksox_def.h>
#include <time.h>
#include <wdf.h>
#include <prbgen_proto.h>

void Check_XYsize(  )
{
/* USRLIB MODULE CODE */
int sendstatus;
char    Cmd2Prober[32];
/* char    Cmd3Prober[32]; */
char    null = 0x0;        /* ASCII null char */
char    tempBuf[210];
char tmpname[32]; /* copy of product file name */
char x_file[32];
char y_file[32];
char ResBuf[210];
char tempx[32];
char tempy[32];


int i_srq = 64;
int i =0;
int    LenOfCmd;

double delta_xsize = 0.0;
double delta_ysize = 0.0;

int sys_stat;

WDFRec *wdfptr;


sys_stat = atoi(getenv("KI_LPT_STUB"));
if (sys_stat ==1) 
    return;
else/* start of else 12212009*/
{

    if ((wdfptr = (WDFRec *)dpGetPointer("wdfptr", LONG_P)) == NULL)
        return;

    sprintf(Cmd2Prober,"ku\r\n\0" );
    LenOfCmd = strlen( Cmd2Prober );
    sendstatus = PrWriteRead(Cmd2Prober, LenOfCmd, tempBuf, 200, 10, 2);

        for (i = 0; i < 28; i++)
            ResBuf[i] = tempBuf[i];
            
        ResBuf[i] = '\0';
        
        sprintf(x_file, "%c%c%c%c%c\0",ResBuf[18], ResBuf[19], ResBuf[20], ResBuf[21], ResBuf[22]);  
        sprintf(y_file, "%c%c%c%c%c\0",ResBuf[23], ResBuf[24], ResBuf[25], ResBuf[26], ResBuf[27]);

        
    delta_xsize = fabs( (atof(x_file) - (wdfptr->diesizex * 1000)));
    delta_ysize = fabs( (atof(y_file) - (wdfptr->diesizey * 1000)));

    /*
    sprintf(Cmd3Prober,"ur204\r\n\0" );
    LenOfCmd = strlen( Cmd3Prober );
    sendstatus = PrWriteRead(Cmd3Prober, LenOfCmd, tempBuf, 200, 10, 2);
    printf("ovdrv=%s\n",tempBuf);
    */

    /*printf("%g:%g\n", delta_xsize, delta_ysize);*/

    if ((delta_xsize < 2.0) && (delta_ysize < 2.0))
        {
            printf("TEST PROGRAM AND PROBER JOB FILE MATCHED X-Y size \n");
        }

    else 
        {
            OkMsgDlg("#  TEST PROGRAM and PROBER JOB FILE WAS NOT MATCHED!! \n#   CHECK JOBFILE AND TEST PROGRAM!!\n");
            exit(1);

            printf("\n\n");
            printf("#######################################################\n");
            printf("#                                                     #\n");
            printf("#  TEST PROGRAM and PROBER JOB FILE WAS NOT MATCHED!! #\n");
            printf("#                                                     #\n");
            printf("#          TEST PROGRAM NOT WORKED & EXITED           #\n");
            printf("#                                                     #\n");
            printf("#    CHECK PROBER JOB FILE & TEST PROGRAM !!!!!!!     #\n");
            printf("#                                                     #\n");        
            printf("#######################################################\n");
            printf("\n\n");
            exit(0);
    
        }
    }/* end of else 12212009*/
/* USRLIB MODULE END  */
} 		/* End Check_XYsize.c */

