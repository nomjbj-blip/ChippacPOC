/* USRLIB MODULE INFORMATION

	MODULE NAME: ti_srq
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
/* USRLIB MODULE HELP DESCRIPTION

	END USRLIB MODULE HELP DESCRIPTION */
/* USRLIB MODULE PARAMETER LIST */
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>

void ti_srq(  )
{
/* USRLIB MODULE CODE */
/*
this function will accept a time out value in seconds
the BUS is serial polled until
1 - an SRQ (64 dec or greater) is rec'd
2 - a time out occurs
after one of these 2 events occurs the function will terminate
*/
prquerygpib(2);
prquerygpib(2);
prquerygpib(2);

/*
this function will continue to serial poll as long as the SRQ is asserted on the BUS
this function uses the TIMEOUT value in the prbcnfg_*.dat file

NOTE: do not change the TIMEOUT value in the prbcnfg_*.dat file because other
functions such as PrLoad depend on it being a large value
*/
prwaitsrqclear();
/* USRLIB MODULE END  */
} 		/* End ti_srq.c */

