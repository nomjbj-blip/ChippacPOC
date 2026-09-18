/* vpinpot function prototype and KITT header file */

/* COPY OF USRLIB SETTINGS INFORMATION */
/* [VISIBILITY] */
/* HIDDEN=0 */
/* [DEPENDENCIES] */
/* LIBS=-lA07 -lHP4284 -lktest -lLBC5 */
/* OBJECTS= */
/* HEADERS= */
/* [NON-KULT SEARCH PATH] */
/* PATHS= */

/* USRLIB MODULE INFORMATION

	MODULE NAME: resv_set2
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 4
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		v,	double,	Input,	,	,	
	INCLUDES:
#include <math.h>
#include "lptdef.h"
#include <par_util.h>
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <tidp1.h>
#include "LBC5_proto.h"
#include <stdlib.h>
#include <ktxe_proto.h>
#include <ksox_def.h>

#define CRTLIM 100.0E-3
	END USRLIB MODULE INFORMATION
*/
double  resv_set2(int, int, int, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: vpin
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 10
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		vgs,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vsstart,	double,	Input,	,	,	
		vsstop,	double,	Input,	,	,	
		vsstep,	double,	Input,	,	,	
		Vpin,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "lptdef.h"
#include "LBC5_proto.h"
#include <stdlib.h>
#include <ktxe_proto.h>
#include <ksox_def.h>
	END USRLIB MODULE INFORMATION
*/
void vpin(int drain, int gate, int source, int sub, double vgs, double vds, double vsstart, double vsstop, double vsstep, double * Vpin);


/* USRLIB MODULE INFORMATION

	MODULE NAME: vpin2
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 10
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		vgs,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vsstart,	double,	Input,	,	,	
		vsstop,	double,	Input,	,	,	
		vsstep,	double,	Input,	,	,	
		Vpin,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "lptdef.h"
#include "LBC5_proto.h"
#include <stdlib.h>
#include <ktxe_proto.h>
#include <ksox_def.h>
	END USRLIB MODULE INFORMATION
*/
void vpin2(int drain, int gate, int source, int sub, double vgs, double vds, double vsstart, double vsstop, double vsstep, double * Vpin);


/* USRLIB MODULE INFORMATION

	MODULE NAME: vpin3
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 10
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		vgs,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vsstart,	double,	Input,	,	,	
		vsstop,	double,	Input,	,	,	
		vsstep,	double,	Input,	,	,	
		Vpin,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "lptdef.h"
#include "LBC5_proto.h"
#include <stdlib.h>
#include <ktxe_proto.h>
#include <ksox_def.h>
	END USRLIB MODULE INFORMATION
*/
void vpin3(int drain, int gate, int source, int sub, double vgs, double vds, double vsstart, double vsstop, double vsstep, double * Vpin);


/* USRLIB MODULE INFORMATION

	MODULE NAME: vpin4
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 10
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		vgs,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vsstart,	double,	Input,	,	,	
		vsstop,	double,	Input,	,	,	
		vsstep,	double,	Input,	,	,	
		Vpin,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "lptdef.h"
#include "LBC5_proto.h"
#include <stdlib.h>
#include <ktxe_proto.h>
#include <ksox_def.h>
	END USRLIB MODULE INFORMATION
*/
void vpin4(int drain, int gate, int source, int sub, double vgs, double vds, double vsstart, double vsstop, double vsstep, double * Vpin);


/* USRLIB MODULE INFORMATION

	MODULE NAME: vpot
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 9
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		vgs,	double,	Input,	,	,	
		vsstart,	double,	Input,	,	,	
		vsstop,	double,	Input,	,	,	
		vsstep,	double,	Input,	,	,	
		Vpot,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "lptdef.h"
#include "LBC5_proto.h"
#include <stdlib.h>
#include <ktxe_proto.h>
#include <ksox_def.h>
	END USRLIB MODULE INFORMATION
*/
void vpot(int drain, int gate, int source, int sub, double vgs, double vsstart, double vsstop, double vsstep, double * Vpot);


/* USRLIB MODULE INFORMATION

	MODULE NAME: vpot2
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 9
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		vgs,	double,	Input,	,	,	
		vsstart,	double,	Input,	,	,	
		vsstop,	double,	Input,	,	,	
		vsstep,	double,	Input,	,	,	
		Vpot,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "lptdef.h"
#include "LBC5_proto.h"
#include <stdlib.h>
#include <ktxe_proto.h>
#include <ksox_def.h>
	END USRLIB MODULE INFORMATION
*/
void vpot2(int drain, int gate, int source, int sub, double vgs, double vsstart, double vsstop, double vsstep, double * Vpot);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Vtsweep
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 11
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		Vd,	double,	Input,	,	,	
		vgstart,	double,	Input,	,	,	
		vgstop,	double,	Input,	,	,	
		nstep,	int,	Input,	,	,	
		ilimit,	double,	Input,	,	,	
		udelay,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "lptdef.h"
#include "LBC5_proto.h"
#include <stdlib.h>
#include <ktxe_proto.h>
#include <ksox_def.h>

	END USRLIB MODULE INFORMATION
*/
double  Vtsweep(int, int, int, int, double, double, double, int, double, double, char);

