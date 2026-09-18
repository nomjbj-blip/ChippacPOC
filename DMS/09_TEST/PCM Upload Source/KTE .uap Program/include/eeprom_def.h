/*> #INCLUDES */

#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <math.h>
#include <time.h>
#include <signal.h>
#include "/home/ki/customsub/controlC.c" 

#ifndef  KDF_VERSION
#include <kdf.h>
#endif

#include <kui_proto.h>
#include <kwf_proto.h>
#include <lptdef.h>


#define KI_Strncpy(d,s,l) (void) strncpy(d,s,l-1); *(d+l-1) = (char) NULL;

/*** flags returned by functions indicating success or failure */
#define KI_ABORT -32767
#define KI_ERROR -1
#define KI_OK     0

/*** constants  to set loop, wafer & site  "while(xxxx_loop)" continue flags */
#define LOOP -1
#define QUIT_LOOP 0

/*** command line err_report_mode argument constants */
#define NO_ERR_ACTION       0
#define DISP_ERR_MSGS       1
#define LOG_ERR_MSGS        2
#define LOG_DISP_ERR_MSGS   3
#define MAX_ERR_REPORT_MODE 3

/*** test station bounds */
#define MAX_TESTSTATION 4

/*** command line user argument length */
#define USER_ARG_LENGTH 81

/*** maximum slot number allowed */
#ifndef MAX_SLOT
#define MAX_SLOT 25
#endif

