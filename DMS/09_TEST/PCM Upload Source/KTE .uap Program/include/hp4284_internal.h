/***************************************************************
 *
 * This is the internal header file for the HP4284 driver.
 *
 * Edit History:
 * 18-May-1993  Cloned by Xiaojing Wang
 *
 ***************************************************************/

typedef struct hp4284_struct {
	int ieee_addr;
} HP4284_STRUCT;

HP4284_STRUCT hp4284;

#define MIN_SUBFCN 1
#define MAX_SUBFCN 8

#define NOT_INITIALIZED -1

#define MIN_UNIT_NUMBER 1
#define MAX_UNIT_NUMBER 4

#define MODE	1
#define RANGE   4
#define FREQUENCY  2
#define CHANNEL	   3
#define APERTURE   5
#define OPEN_CORRECTION 6
#define LOAD_CORRECTION 7
#define SHORT_CORRECTION 8
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

#define KIeeeTimeout 200

/* Measurement mode */
#define MIN_IMPEDANCE_MODE 0.0
#define MAX_IMPEDANCE_MODE 12.0

/* Range */
#define AUTO 0.0

/* Integration time */
#define SHORT 1.0
#define MEDIUM 2.0
#define LONG 3.0

/* Correction channel */
#define NONE -1.0
#define MIN_CHANNEL 0.0
#define MAX_CHANNEL 127.0

/* Frequency */
#define MIN_FREQUENCY 20.0
#define MAX_FREQUENCY 1.0e6

