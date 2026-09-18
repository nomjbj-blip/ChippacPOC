/* ki776.h *****************************************************

Description:
	Header/include file for 776 support.

History:
	Created: djb 10-1-98 
	Last change: djb 10-1-98 

*****************************************************************/

/* General */
#define true -1
#define false 0
#define DEBUG 1	/* NODEBUG = 0, DEBUG = 1 */

/* ki776 common */
struct ki776_struct
{
	int gpib_address; 	/* GPIB address */
	int init_state;		/* 776 initialized */
};

extern struct ki776_struct ki776;

