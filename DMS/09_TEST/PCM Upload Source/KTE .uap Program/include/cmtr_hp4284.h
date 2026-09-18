/* external header file for CMETER HP4284 */
#define MODE    1
#define RANGE   4
#define FREQUENCY  2
#define CHANNEL    3
#define APERTURE   5
#define OPEN_CORRECTION 6
#define LOAD_CORRECTION 7
#define SHORT_CORRECTION 8

/* Measurement mode */
#define MIN_IMPEDANCE_MODE 0.0
#define MAX_IMPEDANCE_MODE 12.0
 
/* Range */
/* #define AUTO 0.0  */
 
/* Integration time */
#define SHORT 1.0
#define MEDIUM 2.0
/* #define LONG 3.0  */
 
/* Correction channel */
#define NONE -1.0
#define MIN_CHANNEL 0.0
#define MAX_CHANNEL 127.0
 
/* Frequency */
#define MIN_FREQUENCY 20.0
#define MAX_FREQUENCY 1.0e6

#define CsQ  0.0
#define CsRs 1.0
#define RX   2.0
#define CpD  3.0
#define CpQ  4.0
#define CpG  5.0
#define CpRp 6.0
#define CsD  7.0
#define ZTD  8.0
#define GB   9.0
#define YTR 10.0
#define YTD 11.0
#define ZTR 12.0

int c_initialize(int,char *);
int c_forcev(int, float);
int c_meascg(int, float *, float *);
int setcmtr(int, int, float, float);
