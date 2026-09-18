/* analog function prototype and KITT header file */

/* COPY OF USRLIB SETTINGS INFORMATION */
/* [VISIBILITY] */
/* HIDDEN=0 */
/* [DEPENDENCIES] */
/* LIBS=-lktest -lLBC5 */
/* OBJECTS= */
/* HEADERS= */
/* [NON-KULT SEARCH PATH] */
/* PATHS= */

/* USRLIB MODULE INFORMATION

	MODULE NAME: an_vtsslp_mos
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 21
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		gate,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		bulk,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		width,	double,	Input,	,	,	
		length,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
		vmin,	double,	Input,	,	,	
		vmax,	double,	Input,	,	,	
		idsearch,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vbs,	double,	Input,	,	,	
		multiplier,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		delaytime,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		result,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#include <tidp1.h>
#define MAX(a,b)	((a)>(b) ? (a) : (b))
#define CDELAY  125.E-12
#define ILEAK   20.E-12 
	END USRLIB MODULE INFORMATION
*/
void an_vtsslp_mos(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vmin, double vmax, double idsearch, double vds, double vbs, double multiplier, double intrange, double delaytime, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: analog_pn2swp_2goi
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 9
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		vstart,	double,	Input,	,	,	
		vstop,	double,	Input,	,	,	
		nstep,	int,	Input,	,	,	
		ipgm,	double,	Input,	,	,	
		udelay,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "lptdef.h"
#include "LBC5_proto.h"
	END USRLIB MODULE INFORMATION
*/
double analog_pn2swp_2goi(int hi, int lo, int subst, double vstart, double vstop, int nstep, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: BREAKV_ahn
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 12
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		top,	int,	Input,	,	,	
		bot,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		type,	char,	Input,	,	,	
		imax,	double,	Input,	,	,	
		vmin,	double,	Input,	,	,	
		vmax,	double,	Input,	,	,	
		vf,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		result,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#include <tidp1.h>
	END USRLIB MODULE INFORMATION
*/
void  BREAKV_ahn(char *, int, int, int, int, char, double, double, double, double, double, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: BREAKV_ahn_org
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 12
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		top,	int,	Input,	,	,	
		bot,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		type,	char,	Input,	,	,	
		imax,	double,	Input,	,	,	
		vmin,	double,	Input,	,	,	
		vmax,	double,	Input,	,	,	
		vf,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		result,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#include <tidp1.h>
	END USRLIB MODULE INFORMATION
*/
void  BREAKV_ahn_org(char *, int, int, int, int, char, double, double, double, double, double, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: BREAKV_an
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 11
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		top,	int,	Input,	,	,	
		bot,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		type,	char,	Input,	,	,	
		imax,	double,	Input,	,	,	
		vmin,	double,	Input,	,	,	
		vmax,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		result,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#include <tidp1.h>
	END USRLIB MODULE INFORMATION
*/
void  BREAKV_an(char *, int, int, int, int, char, double, double, double, double, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: bvdss1_mos_r1
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 24
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		gate,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		bulk,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		width,	double,	Input,	,	,	
		length,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
		vdsmin,	double,	Input,	,	,	
		vdsmax,	double,	Input,	,	,	
		vdstep,	double,	Input,	,	,	
		ilimit,	double,	Input,	,	,	
		icomp,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		vbulk,	double,	Input,	,	,	
		vgate,	double,	Input,	,	,	
		sd,	char,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		Bvdss,	double *,	Output,	,	,	
		Id,	double *,	Output,	,	,	
		Is,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <stdlib.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void bvdss1_mos_r1(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vdsmin, double vdsmax, double vdstep, double ilimit, double icomp, double intrange, double vbulk, double vgate, char sd, double mrange, double lorange, double * Bvdss, double * Id, double * Is);


/* USRLIB MODULE INFORMATION

	MODULE NAME: bvdss2_6
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 12
	ARGUMENTS:
		d,	int,	Input,	,	,	
		g,	int,	Input,	,	,	
		s,	int,	Input,	,	,	
		body,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		chuck,	int,	Input,	,	,	
		vdsstart,	double,	Input,	,	,	
		vdsstop,	double,	Input,	,	,	
		nstep,	int,	Input,	,	,	
		ipgm,	double,	Input,	,	,	
		udelay,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
	INCLUDES:
#include <math.h>		                
#include "lptdef.h"
#include <stdlib.h>
#include "LBC5_proto.h"
	
	END USRLIB MODULE INFORMATION
*/
double bvdss2_6(int d, int g, int s, int body, int sub, int chuck, double vdsstart, double vdsstop, int nstep, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: bvdss2_back
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 10
	ARGUMENTS:
		d,	int,	Input,	,	,	
		g,	int,	Input,	,	,	
		s,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		vdsstart,	double,	Input,	,	,	
		vdsstop,	double,	Input,	,	,	
		nstep,	int,	Input,	,	,	
		ipgm,	double,	Input,	,	,	
		udelay,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
	INCLUDES:
#include <math.h>		                
#include "lptdef.h"
#include <stdlib.h>
#include "LBC5_proto.h"
	
	END USRLIB MODULE INFORMATION
*/
double bvdss2_back(int d, int g, int s, int sub, double vdsstart, double vdsstop, int nstep, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: bvdss2gvsv
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 12
	ARGUMENTS:
		d,	int,	Input,	,	,	
		g,	int,	Input,	,	,	
		s,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		vdsstart,	double,	Input,	,	,	
		vdsstop,	double,	Input,	,	,	
		nstep,	int,	Input,	,	,	
		vg,	double,	Input,	,	,	
		vs,	double,	Input,	,	,	
		ipgm,	double,	Input,	,	,	
		udelay,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
	INCLUDES:
#include <math.h>		                
#include "lptdef.h"
#include <stdlib.h>
#include "LBC5_proto.h"
	
	END USRLIB MODULE INFORMATION
*/
double bvdss2gvsv(int d, int g, int s, int sub, double vdsstart, double vdsstop, int nstep, double vg, double vs, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: bvdss4
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 11
	ARGUMENTS:
		d,	int,	Input,	,	,	
		g,	int,	Input,	,	,	
		s,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		vgs,	double,	Input,	,	,	
		vdsstart,	double,	Input,	,	,	
		vdsstop,	double,	Input,	,	,	
		nstep,	int,	Input,	,	,	
		ipgm,	double,	Input,	,	,	
		udelay,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
	INCLUDES:
#include <math.h>		                
#include "lptdef.h"
#include <stdlib.h>
#include "LBC5_proto.h"
	
	END USRLIB MODULE INFORMATION
*/
double bvdss4(int d, int g, int s, int sub, double vgs, double vdsstart, double vdsstop, int nstep, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: bvdss_debug
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 12
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		body,	int,	Input,	,	,	
		vdsstart,	double,	Input,	,	,	
		vdsstop,	double,	Input,	,	,	
		nstep,	int,	Input,	,	,	
		ipgm,	double,	Input,	,	,	
		udelay,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
		debug,	int,	Input,	,	,	
	INCLUDES:
#include <math.h>		                
#include "lptdef.h"
#include <stdlib.h>
#include "LBC5_proto.h"
	
	END USRLIB MODULE INFORMATION
*/
double bvdss_debug(int drain, int gate, int source, int sub, int body, double vdsstart, double vdsstop, int nstep, double ipgm, double udelay, char type, int debug);


/* USRLIB MODULE INFORMATION

	MODULE NAME: bvdssgv_6
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 13
	ARGUMENTS:
		d,	int,	Input,	,	,	
		g,	int,	Input,	,	,	
		s,	int,	Input,	,	,	
		body,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		chuck,	int,	Input,	,	,	
		vdsstart,	double,	Input,	,	,	
		vdsstop,	double,	Input,	,	,	
		vg,	char,	Input,	,	,	
		nstep,	int,	Input,	,	,	
		ipgm,	double,	Input,	,	,	
		udelay,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
	INCLUDES:
#include <math.h>		                
#include "lptdef.h"
#include <stdlib.h>
#include "LBC5_proto.h"
	
	END USRLIB MODULE INFORMATION
*/
double bvdssgv_6(int d, int g, int s, int body, int sub, int chuck, double vdsstart, double vdsstop, char vg, int nstep, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: bvii_spot1_mos
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 19
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		gate,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		bulk,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		width,	double,	Input,	,	,	
		length,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
		vdd,	double,	Input,	,	,	
		vgg,	double,	Input,	,	,	
		vbs,	double,	Input,	,	,	
		xid,	double,	Input,	1.2,	,	
		delaytime,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		bvii,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void bvii_spot1_mos(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vdd, double vgg, double vbs, double xid, double delaytime, double intrange, double mrange, double lorange, double * bvii);


/* USRLIB MODULE INFORMATION

	MODULE NAME: bvii_spot_mos
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 19
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		gate,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		bulk,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		width,	double,	Input,	,	,	
		length,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
		vdd,	double,	Input,	,	,	
		vbs,	double,	Input,	,	,	
		xid,	double,	Input,	1.5,	,	
		delaytime,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		bvii,	double *,	Output,	,	,	
		isub,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void bvii_spot_mos(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vdd, double vbs, double xid, double delaytime, double intrange, double mrange, double lorange, double * bvii, double * isub);


/* USRLIB MODULE INFORMATION

	MODULE NAME: C_1M_500mv
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 6
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		vbias,	double,	Input,	,	,	
		c2,	double *,	Output,	,	,	
		z2,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <LBC5_proto.h>

#define CRTLIM 1.0E-6
#include <tidp1.h>
#include "ktxe_types.h"
#include "COM_usrlib.h"
#define MAX(a,b)        ((a)>(b) ? (a) : (b))
#include <cmtr_hp4284.h>
#define __4284__

#define Eo 8.854E-14
#define PI 3.14159265358979323846264338327950288419716939937510582
#define DONTCARE 0
#define CV_XMT 11


	END USRLIB MODULE INFORMATION
*/
double C_1M_500mv(int hi, int lo, int subst, double vbias, double * c2, double * z2);


/* USRLIB MODULE INFORMATION

	MODULE NAME: cap_2spo3_cap
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 22
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		area,	double,	Input,	,	,	
		per,	double,	Input,	,	,	
		tty,	char,	Input,	,	,	
		bty,	char,	Input,	,	,	
		vacc,	double,	Input,	,	,	
		vinv,	double,	Input,	,	,	
		freq,	double,	Input,	,	,	
		sig,	double,	Input,	,	,	
		stray,	double,	Input,	,	,	
		integ,	int,	Input,	,	,	
		ddebug,	int,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		ca,	double *,	Output,	,	,	
		ta,	double *,	Output,	,	,	
		ci,	double *,	Output,	,	,	
		ti,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#define CRTLIM 1.0E-6		                              
#include <tidp1.h>
#include "ktxe_types.h"
#include "COM_usrlib.h"
#define MAX(a,b)	((a)>(b) ? (a) : (b))
#include <cmtr_hp4284.h>
#define __4284__

#define Eo 8.854E-14
#define PI 3.14159265358979323846264338327950288419716939937510582
#define DONTCARE 0
#define CV_XMT 11
	END USRLIB MODULE INFORMATION
*/
void  cap_2spo3_cap(char *, int, int, int, int, double, double, char, char, double, double, double, double, double, int, int, double, double, double *, double *, double *, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: cap_4284_100K_30mv_nost
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 6
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		vbias,	double,	Input,	,	,	
		c2,	double *,	Output,	,	,	
		z2,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <LBC5_proto.h>
	END USRLIB MODULE INFORMATION
*/
double cap_4284_100K_30mv_nost(int hi, int lo, int subst, double vbias, double * c2, double * z2);


/* USRLIB MODULE INFORMATION

	MODULE NAME: erase_eeprom_TM
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 9
	ARGUMENTS:
		drn,	int,	Input,	,	,	
		cg,	int,	Input,	,	,	
		src,	int,	Input,	,	,	
		tr,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		vcg,	double,	Input,	,	,	
		vtr,	double,	Input,	,	,	
		width,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "COM_usrlib.h"
	END USRLIB MODULE INFORMATION
*/
void erase_eeprom_TM(int drn, int cg, int src, int tr, int well, double vcg, double vtr, double width, double holdtime);


/* USRLIB MODULE INFORMATION

	MODULE NAME: erase_init
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 12
	ARGUMENTS:
		drn,	int,	Input,	,	,	
		cg,	int,	Input,	,	,	
		src,	int,	Input,	,	,	
		tr,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		vcg,	double,	Input,	,	,	
		vtr,	double,	Input,	,	,	
		vcg_pg,	double,	Input,	,	,	
		vtr_pg,	double,	Input,	,	,	
		width,	double,	Input,	,	,	
		width_pg,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "COM_usrlib.h"
	END USRLIB MODULE INFORMATION
*/
void erase_init(int drn, int cg, int src, int tr, int well, double vcg, double vtr, double vcg_pg, double vtr_pg, double width, double width_pg, double holdtime);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Fetcheck_BV_ISUB_mos
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 26
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		bulk,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		width,	double,	Input,	,	,	
		length,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
		vdsf,	double,	Input,	,	,	
		vgs_off,	double,	Input,	,	,	
		vgs_on,	double,	Input,	,	,	
		vbsf,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		bv_ipgm,	double,	Input,	,	,	
		isub_vgs,	double,	Input,	,	,	
		isub_vds,	double,	Input,	,	,	
		isub_vbs,	double,	Input,	,	,	
		isub_enable,	int,	Input,	,	,	
		bvdss_enable,	int,	Input,	,	,	
		ioff,	double *,	Output,	,	,	
		ids,	double *,	Output,	,	,	
		bvdss,	double *,	Output,	,	,	
		isub,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#include <tidp1.h>	                
#include "lptdef.h"
#include "LBC5_proto.h"


double *bvdss_return;
double *isub_return;


double bvdss2_lo( int d, int g, int s, int sub, double vdsstart, double vdsstop, int nstep, double ipgm, double udelay, char type )
{
                        
    extern int devint();
    double    i_range;                                        
    double    aipgm;                                       
    double    avlow;                                      
    double    avhigh;                                      
    double    sdelay;                                             
    double    vsign;                                                
    double    temp = 0.0;                                    

    if ((type == 'N') || (type == 'n'))
        vsign = 1.0;
    else if ((type == 'P') || (type == 'p'))
        vsign = -1.0;
    else
        return(-1.0);                               

    aipgm = fabs(ipgm);
    avlow = fabs(vdsstart);
    avhigh = fabs(vdsstop);                                                             

    if (sub > 0)
        conpin(SMU1L, GND, s, g, sub, KI_EOC);
    else
        conpin(SMU1L, GND, s, g, KI_EOC);

    conpin(SMU1, d, KI_EOC);

                                                         

    i_range = 1.25*aipgm;                              
    setmode (SMU1, KI_LIM_MODE, KI_VALUE);                                                                          
    limiti(SMU1, i_range);
    rangei(SMU1, i_range);

                                                               

    if ((type == 'N') || (type == 'n'))
        trigig(SMU1, aipgm);
    else
        trigil(SMU1, -aipgm);                                         

    bsweepv(SMU1, vsign*avlow, vsign*avhigh, nstep, udelay, &temp);

    (void)devint();

                                                                  
    if (fabs(vsign*avlow - temp) < 1.E-3)
        temp = 1.E+21;
    if (fabs(vsign*avhigh - temp) < 1.E-3)
        temp = 2.E+21;
bvdss_return = &temp;
    return;
                        
} 		                  


double isub_lo( int DRAIN, int GATE, int SOURCE, int SUBST, double VGS, double VDS, double VBS )
{
                        
    extern int devint();
    double    SIGN;                                 
    double     IS;                           

    if(VDS >= 0.0) SIGN = -1.0;
    if (VDS < 0.0) SIGN = 1.0;

                       

    conpin (GND, SOURCE, SMU1L, SMU2L, SMU3L,0); 
    conpin (SMU3, SUBST,0);

    conpin ( SMU1, DRAIN,0 );
    conpin ( SMU2, GATE,0 );                        

    lorangei(SMU3, 100.0e-12);
    limiti ( SMU1, 50.0e-03);
    forcev ( SMU3, VBS );
    forcev ( SMU2, VGS );
    forcev ( SMU1, VDS );
    delay (100);                            

 intgi(SMU3, &IS);
    (void)devint();
    IS = fabs(IS)*SIGN;
    isub_return = &IS; 
    return;
                        
} 		                
	END USRLIB MODULE INFORMATION
*/
void Fetcheck_BV_ISUB_mos(int drain, int gate, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vdsf, double vgs_off, double vgs_on, double vbsf, double intrange, double mrange, double lorange, double bv_ipgm, double isub_vgs, double isub_vds, double isub_vbs, int isub_enable, int bvdss_enable, double * ioff, double * ids, double * bvdss, double * isub);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Fetcheck_el_mos
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 18
	ARGUMENTS:
		gate,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		bulk,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		width,	double,	Input,	,	,	
		length,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
		vdsf,	double,	Input,	,	,	
		vgs_off,	double,	Input,	,	,	
		vgs_on,	double,	Input,	,	,	
		vbsf,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		ioff,	double *,	Output,	,	,	
		ids,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#include <tidp1.h>	                
#include "lptdef.h"
#include "LBC5_proto.h"



double bvdss2_local( int d, int g, int s, int sub, double vdsstart, double vdsstop, int nstep, double ipgm, double udelay, char type )
{
                        
    extern int devint();
    double    i_range;                                        
    double    aipgm;                                       
    double    avlow;                                      
    double    avhigh;                                      
    double    sdelay;                                             
    double    vsign;                                                
    double    temp = 0.0;                                    

                                                              

    if ((type == 'N') || (type == 'n'))
        vsign = 1.0;
    else if ((type == 'P') || (type == 'p'))
        vsign = -1.0;
    else
        return(-1.0);                               

                                                                   

    aipgm = fabs(ipgm);
    avlow = fabs(vdsstart);
    avhigh = fabs(vdsstop);

                                                      

                                                      

                    

    if (sub > 0)
        conpin(SMU1L, GND, s, g, sub, KI_EOC);
    else
        conpin(SMU1L, GND, s, g, KI_EOC);

    conpin(SMU1, d, KI_EOC);

                                                         

    i_range = 1.25*aipgm;                              
    setmode (SMU1, KI_LIM_MODE, KI_VALUE);                                                                          
    limiti(SMU1, i_range);
    rangei(SMU1, i_range);

                                                               

    if ((type == 'N') || (type == 'n'))
        trigig(SMU1, aipgm);
    else
        trigil(SMU1, -aipgm);

                                             

    bsweepv(SMU1, vsign*avlow, vsign*avhigh, nstep, udelay, &temp);

    (void)devint();

                                                                  
    if (fabs(vsign*avlow - temp) < 1.E-3)
        temp = 1.E+21;
    if (fabs(vsign*avhigh - temp) < 1.E-3)
        temp = 2.E+21;

    return(temp);
                        
} 		                  









	END USRLIB MODULE INFORMATION
*/
void Fetcheck_el_mos(int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vdsf, double vgs_off, double vgs_on, double vbsf, double intrange, double mrange, double lorange, double * ioff, double * ids);


/* USRLIB MODULE INFORMATION

	MODULE NAME: id1_5_an
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 11
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		body,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		vgs,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vbs,	double,	Input,	,	,	
		hold,	double,	Input,	,	,	
		intrange,	int,	Input,	,	,	
		debug,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#define CRTLIM 50.0e-3
	END USRLIB MODULE INFORMATION
*/
double id1_5_an(int drain, int gate, int source, int body, int subst, double vgs, double vds, double vbs, double hold, int intrange, int debug);


/* USRLIB MODULE INFORMATION

	MODULE NAME: id1_6
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 9
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		body,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		chuck,	int,	Input,	,	,	
		vgs,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vbs,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#define CRTLIM 50.0e-3
	END USRLIB MODULE INFORMATION
*/
double id1_6(int drain, int gate, int source, int body, int subst, int chuck, double vgs, double vds, double vbs);


/* USRLIB MODULE INFORMATION

	MODULE NAME: id1_an
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 7
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		vgs,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vbs,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double id1_an(int drain, int gate, int source, int subst, double vgs, double vds, double vbs);


/* USRLIB MODULE INFORMATION

	MODULE NAME: idx
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 7
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		vgs,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vbs,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double idx(int drain, int gate, int source, int subst, double vgs, double vds, double vbs);


/* USRLIB MODULE INFORMATION

	MODULE NAME: iso_mos1
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 20
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		gate,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		bulk,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		width,	double,	Input,	,	,	
		length,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
		vds,	double,	Input,	0.1,	,	
		isrange,	double,	Input,	1e-09,	,	
		vgs,	double,	Input,	0.1,	,	
		vbs,	double,	Input,	0,	,	
		vss,	double,	Input,	0,	,	
		factor,	double,	Input,	1,	,	
		intrange,	double,	Input,	1,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		Is,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
	END USRLIB MODULE INFORMATION
*/
void iso_mos1(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vds, double isrange, double vgs, double vbs, double vss, double factor, double intrange, double mrange, double lorange, double * Is);


/* USRLIB MODULE INFORMATION

	MODULE NAME: isub6
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 9
	ARGUMENTS:
		DRAIN,	int,	Input,	,	,	
		GATE,	int,	Input,	,	,	
		SOURCE,	int,	Input,	,	,	
		BODY,	int,	Input,	,	,	
		SUBST,	int,	Input,	,	,	
		CHUCK,	int,	Input,	,	,	
		VGS,	double,	Input,	,	,	
		VDS,	double,	Input,	,	,	
		VBS,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double  isub6(int, int, int, int, int, int, double, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: isub_an
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 7
	ARGUMENTS:
		DRAIN,	int,	Input,	,	,	
		GATE,	int,	Input,	,	,	
		SOURCE,	int,	Input,	,	,	
		SUBST,	int,	Input,	,	,	
		VGS,	double,	Input,	,	,	
		VDS,	double,	Input,	,	,	
		VBS,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double isub_an(int DRAIN, int GATE, int SOURCE, int SUBST, double VGS, double VDS, double VBS);


/* USRLIB MODULE INFORMATION

	MODULE NAME: leak1_an
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 4
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		v,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double  leak1_an(int, int, int, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: leak1_an_org170412
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 4
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		v,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double  leak1_an_org170412(int, int, int, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: leak4
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 5
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo1,	int,	Input,	,	,	
		lo2,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		v,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double leak4(int hi, int lo1, int lo2, int subst, double v);


/* USRLIB MODULE INFORMATION

	MODULE NAME: leak4_an
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 5
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo1,	int,	Input,	,	,	
		lo2,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		v,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double  leak4_an(int, int, int, int, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: leak4_an_lc
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 6
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo1,	int,	Input,	,	,	
		lo2,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		v,	double,	Input,	,	,	
		ilim,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double  leak4_an_lc(int, int, int, int, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: leak4_lc_6
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 8
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo1,	int,	Input,	,	,	
		lo2,	int,	Input,	,	,	
		body,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		chuck,	int,	Input,	,	,	
		v,	double,	Input,	,	,	
		ilim,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double leak4_lc_6(int hi, int lo1, int lo2, int body, int subst, int chuck, double v, double ilim);


/* USRLIB MODULE INFORMATION

	MODULE NAME: leak4_lc_an
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 6
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo1,	int,	Input,	,	,	
		lo2,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		v,	double,	Input,	,	,	
		ilim,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double  leak4_lc_an(int, int, int, int, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: leak4_lc_an1
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 7
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo1,	int,	Input,	,	,	
		lo2,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		v,	double,	Input,	,	,	
		ilim,	double,	Input,	,	,	
		delaytime,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double  leak4_lc_an1(int, int, int, int, double, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: leak4_lc_an2
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 12
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo1,	int,	Input,	,	,	
		lo2,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		v,	double,	Input,	,	,	
		ilim,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		debug,	int,	Input,	,	,	
		intrange,	int,	Input,	,	,	
		hold,	double,	Input,	,	,	
		repeat,	int,	Input,	,	,	
		ioff,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <stdlib.h>
#include <par_util.h>
#include <ktxe_proto.h>
#include <ksox_def.h>
#include <kui_proto.h>


#define MAX(a,b)	((a)>(b) ? (a) : (b))
#define CDELAY  125.E-12
#define ILEAK   20.E-12 

	END USRLIB MODULE INFORMATION
*/
double  leak4_lc_an2(int, int, int, int, double, double, double, int, int, double, int, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: leak4_lc_an3
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 9
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo1,	int,	Input,	,	,	
		lo2,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		v,	double,	Input,	,	,	
		ilim,	double,	Input,	,	,	
		delaytime,	double,	Input,	,	,	
		dum1,	double *,	Output,	,	,	
		dum2,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double  leak4_lc_an3(int, int, int, int, double, double, double, double *, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: leak4_lc_an3_org170412
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 9
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo1,	int,	Input,	,	,	
		lo2,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		v,	double,	Input,	,	,	
		ilim,	double,	Input,	,	,	
		delaytime,	double,	Input,	,	,	
		dum1,	double *,	Output,	,	,	
		dum2,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double  leak4_lc_an3_org170412(int, int, int, int, double, double, double, double *, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: leak4_lc_an_org170412
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 6
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo1,	int,	Input,	,	,	
		lo2,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		v,	double,	Input,	,	,	
		ilim,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double  leak4_lc_an_org170412(int, int, int, int, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: leak4_lc_int
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 9
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo1,	int,	Input,	,	,	
		lo2,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		v,	double,	Input,	,	,	
		ilim,	double,	Input,	,	,	
		debug,	int,	Input,	,	,	
		intrange,	int,	Input,	,	,	
		ioff,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <stdlib.h>
#include <par_util.h>
#include <ktxe_proto.h>
#include <ksox_def.h>
#include <kui_proto.h>
#define MAX(a,b)	((a)>(b) ? (a) : (b))
#define CDELAY  125.E-12
#define ILEAK   20.E-12 

	END USRLIB MODULE INFORMATION
*/
double leak4_lc_int(int hi, int lo1, int lo2, int subst, double v, double ilim, int debug, int intrange, double * ioff);


/* USRLIB MODULE INFORMATION

	MODULE NAME: leak4_nognd
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 11
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo1,	int,	Input,	,	,	
		lo2,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		v_hi,	double,	Input,	,	,	
		v_lo1,	double,	Input,	,	,	
		v_lo2,	double,	Input,	,	,	
		v_sub,	double,	Input,	,	,	
		lorng_1,	double,	Input,	,	,	
		intrange,	int,	Input,	,	,	
		delay_1,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double  leak4_nognd(int, int, int, int, double, double, double, double, double, int, int);

/* USRLIB MODULE INFORMATION

	MODULE NAME: leak4_nognd_org170412
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 11
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo1,	int,	Input,	,	,	
		lo2,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		v_hi,	double,	Input,	,	,	
		v_lo1,	double,	Input,	,	,	
		v_lo2,	double,	Input,	,	,	
		v_sub,	double,	Input,	,	,	
		lorng_1,	double,	Input,	,	,	
		intrange,	int,	Input,	,	,	
		delay_1,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double  leak4_nognd_org170412(int, int, int, int, double, double, double, double, double, int, int);

/* USRLIB MODULE INFORMATION

	MODULE NAME: leak_6_3vv
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 11
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo1,	int,	Input,	,	,	
		lo2,	int,	Input,	,	,	
		lo3,	int,	Input,	,	,	
		lo4,	int,	Input,	,	,	
		lo5,	int,	Input,	,	,	
		v1,	double,	Input,	,	,	
		v2,	double,	Input,	,	,	
		v3,	double,	Input,	,	,	
		v4,	double,	Input,	,	,	
		ilim,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double leak_6_3vv(int hi, int lo1, int lo2, int lo3, int lo4, int lo5, double v1, double v2, double v3, double v4, double ilim);


/* USRLIB MODULE INFORMATION

	MODULE NAME: leak_6_3vv_o2
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 12
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo1,	int,	Input,	,	,	
		lo2,	int,	Input,	,	,	
		lo3,	int,	Input,	,	,	
		lo4,	int,	Input,	,	,	
		lo5,	int,	Input,	,	,	
		v1,	double,	Input,	,	,	
		v2,	double,	Input,	,	,	
		v3,	double,	Input,	,	,	
		v4,	double,	Input,	,	,	
		ilim,	double,	Input,	,	,	
		Ir2,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double leak_6_3vv_o2(int hi, int lo1, int lo2, int lo3, int lo4, int lo5, double v1, double v2, double v3, double v4, double ilim, double * Ir2);


/* USRLIB MODULE INFORMATION

	MODULE NAME: leak_an
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 5
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		v,	double,	Input,	,	,	
		ilim,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double leak_an(int hi, int lo, int subst, double v, double ilim);


/* USRLIB MODULE INFORMATION

	MODULE NAME: leak_an_org170412
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 5
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		v,	double,	Input,	,	,	
		ilim,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double  leak_an_org170412(int, int, int, double, double);

/* USRLIB MODULE INFORMATION

	MODULE NAME: LEAK_dio_log_ch
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 19
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		top,	int,	Input,	,	,	
		bot,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		area,	double,	Input,	,	,	
		per,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
		btype,	char,	Input,	,	,	
		vrev,	double,	Input,	,	,	
		ilim,	double,	Input,	,	,	
		icomp,	double,	Input,	,	,	
		irange,	double,	Input,	,	,	
		delaytime,	double,	Input,	,	,	
		multiplier,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		result,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#include <tidp1.h>
#include "kdf.h"
#include "ktxe_types.h"
#include "ksox_def.h"
#include "COM_usrlib.h"
	END USRLIB MODULE INFORMATION
*/
void LEAK_dio_log_ch(char * devname, int top, int bot, int sub, int chuckcon, double area, double per, char type, char btype, double vrev, double ilim, double icomp, double irange, double delaytime, double multiplier, double intrange, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: pgm_eeprom_TM
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 9
	ARGUMENTS:
		drn,	int,	Input,	,	,	
		cg,	int,	Input,	,	,	
		src,	int,	Input,	,	,	
		tr,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		vcg,	double,	Input,	,	,	
		vtr,	double,	Input,	,	,	
		width,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "COM_usrlib.h"
	END USRLIB MODULE INFORMATION
*/
void pgm_eeprom_TM(int drn, int cg, int src, int tr, int well, double vcg, double vtr, double width, double holdtime);


/* USRLIB MODULE INFORMATION

	MODULE NAME: pgm_erase_set
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 24
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		cgate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		tr,	int,	Input,	,	,	
		vcg_pg,	double,	Input,	,	,	
		vtr_pg,	double,	Input,	,	,	
		width_pg,	double,	Input,	,	,	
		vcg,	double,	Input,	,	,	
		vtr,	double,	Input,	,	,	
		width,	double,	Input,	,	,	
		vgstart,	double,	Input,	,	,	
		vgstop,	double,	Input,	,	,	
		vgstep,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vgs,	double,	Input,	,	,	
		vbs,	double,	Input,	,	,	
		idtarget,	double,	Input,	,	,	
		debug_print,	int,	Input,	,	0,	1
		holdtime,	double,	Input,	,	,	
		ide,	double *,	Output,	,	,	
		vte,	double *,	Output,	,	,	
		vtp,	double *,	Output,	,	,	
		iders,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "COM_usrlib.h"
#define CRTLIM 50.0e-3
	END USRLIB MODULE INFORMATION
*/
void pgm_erase_set(int drain, int cgate, int source, int well, int tr, double vcg_pg, double vtr_pg, double width_pg, double vcg, double vtr, double width, double vgstart, double vgstop, double vgstep, double vds, double vgs, double vbs, double idtarget, int debug_print, double holdtime, double * ide, double * vte, double * vtp, double * iders);


/* USRLIB MODULE INFORMATION

	MODULE NAME: pgm_init
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 12
	ARGUMENTS:
		drn,	int,	Input,	,	,	
		cg,	int,	Input,	,	,	
		src,	int,	Input,	,	,	
		tr,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		vcg,	double,	Input,	,	,	
		vtr,	double,	Input,	,	,	
		vcg_pg,	double,	Input,	,	,	
		vtr_pg,	double,	Input,	,	,	
		width,	double,	Input,	,	,	
		width_pg,	double,	Input,	,	,	
		holdtime,	double,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "COM_usrlib.h"
	END USRLIB MODULE INFORMATION
*/
void pgm_init(int drn, int cg, int src, int tr, int well, double vcg, double vtr, double vcg_pg, double vtr_pg, double width, double width_pg, double holdtime);


/* USRLIB MODULE INFORMATION

	MODULE NAME: pn2swp_2goi
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 11
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		vstart,	double,	Input,	,	,	
		vstop,	double,	Input,	,	,	
		nstep,	int,	Input,	,	,	
		ipgm,	double,	Input,	,	,	
		udelay,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
		i1,	double,	Input,	,	,	
		goi1,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include "lptdef.h"
#include "LBC5_proto.h"
	END USRLIB MODULE INFORMATION
*/
double pn2swp_2goi(int hi, int lo, int subst, double vstart, double vstop, int nstep, double ipgm, double udelay, char type, double i1, double * goi1);


/* USRLIB MODULE INFORMATION

	MODULE NAME: pn2swp_swp_log
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 9
	ARGUMENTS:
		hi,	int,	Input,	,	,	
		lo,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		vstart,	double,	Input,	,	,	
		vstop,	double,	Input,	,	,	
		nstep,	int,	Input,	,	,	
		ipgm,	double,	Input,	,	,	
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
double pn2swp_swp_log(int hi, int lo, int subst, double vstart, double vstop, int nstep, double ipgm, double udelay, char type);


/* USRLIB MODULE INFORMATION

	MODULE NAME: resv_exlimit
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

#define CRTLIM 100.0E-3

	END USRLIB MODULE INFORMATION
*/
double resv_exlimit(int hi, int lo, int subst, double v);


/* USRLIB MODULE INFORMATION

	MODULE NAME: Single_vth5_ide1
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 13
	ARGUMENTS:
		cgate,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		well,	int,	Input,	,	,	
		tr,	int,	Input,	,	,	
		vgstart,	double,	Input,	,	,	
		vgstop,	double,	Input,	,	,	
		vgstep,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		idtarget,	double,	Input,	,	,	
		ide,	double *,	Output,	,	,	
		vth,	double *,	Output,	,	,	
		debug_print,	int,	Input,	,	0,	1
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void Single_vth5_ide1(int cgate, int drain, int source, int well, int tr, double vgstart, double vgstop, double vgstep, double vds, double idtarget, double * ide, double * vth, int debug_print);


/* USRLIB MODULE INFORMATION

	MODULE NAME: vt_ispat
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 20
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		gate,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		bulk,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		width,	double,	Input,	,	,	
		length,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
		igmax,	double,	Input,	,	,	
		vgsmin,	double,	Input,	,	,	
		vgsmax,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vbs,	double,	Input,	,	,	
		multiplier,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		result,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#include <tidp1.h>
	END USRLIB MODULE INFORMATION
*/
void vt_ispat(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double igmax, double vgsmin, double vgsmax, double vds, double vbs, double multiplier, double intrange, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: vt_ispat_LD13
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 20
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		gate,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		bulk,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		width,	double,	Input,	,	,	
		length,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
		igmax,	double,	Input,	,	,	
		vgsmin,	double,	Input,	,	,	
		vgsmax,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vbs,	double,	Input,	,	,	
		multiplier,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		result,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#include <tidp1.h>
	END USRLIB MODULE INFORMATION
*/
void vt_ispat_LD13(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double igmax, double vgsmin, double vgsmax, double vds, double vbs, double multiplier, double intrange, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: vt_ispat_mos
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 20
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		gate,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		bulk,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		width,	double,	Input,	,	,	
		length,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
		igmax,	double,	Input,	,	,	
		vgsmin,	double,	Input,	,	,	
		vgsmax,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vbs,	double,	Input,	,	,	
		multiplier,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		result,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#include <tidp1.h>
	END USRLIB MODULE INFORMATION
*/
void  vt_ispat_mos(char *, int, int, int, int, int, int, double, double, char, double, double, double, double, double, double, double, double, double, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: vt_ispat_sq
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 20
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		gate,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		bulk,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		width,	double,	Input,	,	,	
		length,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
		igmax,	double,	Input,	,	,	
		vgsmin,	double,	Input,	,	,	
		vgsmax,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vbs,	double,	Input,	,	,	
		multiplier,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		result,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#include <tidp1.h>
	END USRLIB MODULE INFORMATION
*/
void vt_ispat_sq(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double igmax, double vgsmin, double vgsmax, double vds, double vbs, double multiplier, double intrange, double mrange, double lorange, double * result);


/* USRLIB MODULE INFORMATION

	MODULE NAME: VT_LSSG_an
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 23
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		width,	double,	Input,	,	,	
		length,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
		sat_vlow,	double,	Input,	,	,	
		sat_vhigh,	double,	Input,	,	,	
		sat_vds,	double,	Input,	,	,	
		sat_vbs,	double,	Input,	,	,	
		lin_vlow,	char,	Input,	,	,	
		lin_vhigh,	char,	Input,	,	,	
		lin_vds,	char,	Input,	,	,	
		lin_vbs,	char,	Input,	,	,	
		ss_idsearch1,	double,	Input,	,	,	
		ss_idsearch2,	double,	Input,	,	,	
		ss_vds,	char,	Input,	,	,	
		ss_enable,	int,	Input,	,	,	
		vti_return,	double *,	Output,	,	,	
		vts_return,	double *,	Output,	,	,	
		ss_return,	double *,	Output,	,	,	
		gm_return,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <stdlib.h>
#include <par_util.h>
#define MAX(a,b)	((a)>(b) ? (a) : (b))
#define CDELAY  125.E-12
#define ILEAK   20.E-12 
#include <tidp1.h>




double vtext4_lo( int drain, int gate, int source, int subst, double w, double l, double vlow, double vhigh, double vds, double vbs, double ithr, double vstep, int npts, double *slope, int *kflag )
{
                        

    extern int devint();
    double result=0.0;
    double    ithra;
    int       jmaxsl;                                                            
    int       j,jreg;                                 
    int       jold,jnew;                        
    int        niter;                                               
    int        maxpts = 100;                                                 
    double     ids[100];                                                 
    double    id[6];                                            
    double     vt;                                                      
    double    slpmax;                                                       
    double    splast;                                                   
    double    sumi;                                                        
    double    intcpt;                                                                                                               
    double     vslow,vshigh;                                         
    int     lowt;                                 

    maxpts = 100;                       
    niter  = npts-1;

    if (niter < 4) niter = 4;
    if (niter > maxpts) niter = maxpts;

                

    if (vstep == 0.0) {
        *kflag = 4;
        return(result);
    }

    *slope  = 0.0;
    slpmax  = 0.0;
    sumi    = 0.0;
    *kflag  = 0;    
    ithra   = ithr * (w/l);

    if (subst < 1)         
        conpin(GND, SMU1L, SMU2L, source, 0);
        else if (fabs(vbs) < 0.9e-3)
        conpin(GND, SMU1L, SMU2L, source, subst, 0);
    else {     
        conpin(GND, SMU1L, SMU2L, SMU3L, source, 0);
        conpin(SMU3, subst, 0);
    }            
    conpin(SMU2, gate, 0);
    conpin(SMU1, drain, 0);

    limiti(SMU1, 50e-3);
    limiti(SMU2, 50e-3);
    setmode(SMU2, KI_LIM_MODE, KI_VALUE);
    lowt = fndtrg(vlow, vhigh);
    if (lowt)                                  
        trigig(SMU1, ithra);
    else
        trigil(SMU1, ithra);

    if ((subst > 0) && (fabs(vbs) >= 0.9e-3)) 
            forcev (SMU3, vbs );
        forcev (SMU1, vds );
    searchv (SMU2, vlow, vhigh, 5, .01, &vt );
    setauto (SMU2);
    if ((vt==vlow) || (vt==vhigh)) {
        *kflag = 3;                             
         result = 0.0;
         (void)devint();                                              
         return result;                                  
    }            

    vslow = vt;
    vshigh = vt + niter * vstep;
    clrtrg();
    sintgi (SMU1,ids);
    sweepv (SMU2, vslow, vshigh, niter, 0.001);

    (void)devint();

                                                 
    for (j = 0; j <= niter; j++) {
        jreg = j;
        if (j>4) jreg = 5;
        id[jreg] = ids[j];                      
        sumi = sumi + id[jreg];                           
     
        if (j < 4)  continue;                                        
        if (j != 4)  {                                
           sumi = sumi - id[0];

                               
           for(jnew = 0; jnew <= 4; jnew++) {
             jold = jnew + 1;
                 id[jnew] = id[jold];    
               }                   
           splast = *slope;                          
            }

                                  
  
           *slope = (2.0*id[4]+id[3]-id[1]-2.0*id[0])*(0.1/vstep);
                                                      
                                         
        if (*slope < slpmax) {
               if ((splast < slpmax) && (*slope < slpmax * 0.98)) 
                   break;
               else
                   continue;
            }

        slpmax = *slope;
        intcpt = 0.2*sumi - slpmax * ((vslow + j*vstep)-2. * vstep);
                                                       
        jmaxsl = j;

      }                        

                                    

                                      
    
        *slope = slpmax;
    if (jmaxsl == 4)     *kflag = 1;                                 
        if (jmaxsl == niter) *kflag = 2;                                
    if (fabs(slpmax) < 1.0e-15)                                    
            return (0.0);
    
    if (intcpt >= 0.0) 
        result = -intcpt/slpmax + 0.05;               
    else
        result = -intcpt/slpmax - 0.05;              

        return(result);
                        
} 		                  





                                                                             
double vtexts_lo( int drain, int gate, int source, int subst, double w, double l, double vlow, double vhigh, double vds, double vbs, double ithr, double vstep, int npts, double *slope, int *kflag )
{
                        

    extern int devint();
    double result=0.0;
    double    ithra;
    int     jmaxsl;                                                            
    int    j,jreg;                                 
    int    jold,jnew;                        
    int    niter;                                               
    int     maxpts = 100;                                                 
    int    k;
    double     ids[100];                                       
                         
    double    id[6];                                            
    double     vt;                                                      
    double    slpmax;                                                       
    double    splast;                                                   
    double    sumi;                                                        
    double    intcpt;                                                                                                               
    double     vslow,vshigh;                                         
    int     lowt;                                 

maxpts = 100;
    niter  = npts-1;
    if (niter < 4) niter = 4;
    if (niter > maxpts) niter = maxpts;
    if (vstep == 0.0) {
        *kflag = 4;
        return(result);
    }
    *slope  = 0.0;
    slpmax  = 0.0;
    sumi    = 0.0;
    *kflag  = 0;    
    ithra   = ithr * (w/l);
    if (subst < 1)         
        conpin(GND, SMU1L, SMU2L, source, 0);
        else if (fabs(vbs) < 0.9e-3)
        conpin(GND, SMU1L, SMU2L, source, subst, 0);
    else {     
        conpin(GND, SMU1L, SMU2L, SMU3L, source, 0);
        conpin(SMU3, subst, 0);
    }            
    conpin(SMU2, gate, 0);
    conpin(SMU1, drain, 0);
    limiti(SMU1, 50e-3);
    limiti(SMU2, 50e-3);
    setmode(SMU2, KI_LIM_MODE, KI_VALUE);
    lowt = fndtrg(vlow, vhigh);
    if (lowt)                                  
        trigig(SMU1, ithra);
    else
        trigil(SMU1, ithra);

    if ((subst > 0) && (fabs(vbs) >= 0.9e-3)) 
            forcev (SMU3, vbs );
        forcev (SMU1, vds );
    searchv (SMU2, vlow, vhigh, 5, .01, &vt );
    setauto (SMU2);
    if ((vt==vlow) || (vt==vhigh)) {
        *kflag = 3;                             
         result = 0.0;
         (void)devint();                                              
         return result;                                  
    }            

    vslow = vt;
    vshigh = vt + niter * vstep;

    clrtrg();

    smeasi (SMU1,ids);
    sweepv (SMU2, vslow, vshigh, niter, 0.001);

    (void)devint();

    for( k = 0; k <= niter; k++) {
      ids[k] = sqrt(fabs(ids[k]));
        if (vhigh < 0.0) 
          ids[k] = ids[k] * -1.0;
        }                                                 
    for (j = 0; j <= niter; j++) {
        jreg = j;
        if (j>4) jreg = 5;
        id[jreg] = ids[j];                      
        sumi = sumi + id[jreg];                           
     
        if (j < 4)  continue;                                        
        if (j != 4)  {                                
           sumi = sumi - id[0];                  
           for(jnew = 0; jnew <= 4; jnew++) {
             jold = jnew + 1;
                 id[jnew] = id[jold];    
               }                   
           splast = *slope;                          
            }
           *slope = (2.0*id[4]+id[3]-id[1]-2.0*id[0])*(0.1/vstep);                                    
        if (*slope < slpmax) {
               if ((splast < slpmax) && (*slope < slpmax * 0.98)) 
                   break;
               else
                   continue;
            }

        slpmax = *slope;
        intcpt = 0.2*sumi - slpmax * ((vslow + j*vstep)-2. * vstep);
                                                       
        jmaxsl = j;

      }                        
  
        *slope = slpmax;
    if (jmaxsl == 4)     *kflag = 1;                                 
        if (jmaxsl == niter) *kflag = 2;                                
    if (fabs(slpmax) < 1.0e-15)                                    
            return (0.0);
    
        result = -intcpt/slpmax;

        return(result);
               
} 		                  







void vtsslp_mos( char *devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vmin, double vmax, double idsearch, double vds, double vbs, double multiplier, double intrange, double delaytime, double mrange, double lorange, double *result )
{
                        
  double starttime, finishtime;
  double status2 = 0.0;
  extern double secnds(double);
  int num;                                     
  double polarity,vt;
  double crtlmt = 100e-03;    
  double vmid, vhigh, vlow, ids;     
  int k;
  
  double dummy1, dummy2;
  
  if ((vmin<0) || (vmin>40 )) goto Par_error;
  if ((vmax<0) || (vmax>40 ) )goto Par_error;
  if (vmin>=vmax ) goto Par_error;
  if ((idsearch<0) || (idsearch>.1 )) goto Par_error;
  if ((vds<=0) || (vds>20 )) goto Par_error;
  if ((vbs<-20) || (vbs>20 ) )goto Par_error;
  if ((intrange<0) || (intrange>3 )) goto Par_error;
  
  if ((type == 'N') || (type == 'n') || (type == 'M') || (type == 'm') ||(type == 'A') || (type == 'a'))
    polarity = 1.0;
  else if ((type == 'P') || (type == 'p'))
    polarity = -1.0;
  else
    goto Par_error;                          
  
  if (debug != 0) {
    starttime = secnds(0.0);
  }
  
  vmin=fabs(vmin)*polarity;
  vmax=fabs(vmax)*polarity;
  vds=fabs(vds)*polarity;
  idsearch=fabs(idsearch)*polarity;
  
                                  
  num = 10;

                       
  if(sub > 0 || bulk > 0 || chuckcon > 0){
    conpin(gnd, smu1l, smu2l, smu3l, source, 0);
    conpin(smu3, sub, bulk, chuckcon, 0);
  }else{
    conpin(gnd, smu1l, smu2l, source, 0);
  }

  conpin  (smu2, gate, 0);
  conpin  (smu1, drain, 0);

  limiti(smu2,1e-3 );
  
  limiti(smu1, 10.0 * idsearch);                                
  rangei(smu1, 10.0 * idsearch);

  if (bulk > 0 || sub > 0 || chuckcon > 0 )     
    forcev (smu3, vbs);

  forcev (SMU1, vds);


  if (polarity == 1) {
    trigig(smu1, idsearch);                  
    searchv (SMU2, vmin, vmax, num, delaytime, &vt);
  } else {
    vlow = vmin;
    vhigh = vmax;
    for (k=0; k<num; k++) {
      vmid = (vhigh + vlow) / 2.0;
      forcev (smu2, vmid);
      
      if (delaytime >= 0.001) rdelay(delaytime);
      measi (smu1, &ids);
      
      if (fabs(ids) > fabs(idsearch)) {
    vhigh = vmid;
      } else {
    vlow = vmid;
      }
      
                                        
                                            
       if (debug != 0)
    printf ("\nk = %d, vmid = %g, ids = %g igs:%g isub:%g", k, vmid, ids, dummy1, dummy2); 
      
   }             
vt = vmid;

}

getstatus(SMU2, KI_COMPLNC, &status2); 

devint();

if(status2 == 2.0) {
   *result = 4.0e21;
   return;
} 

                                                                                                                                                       
                                                                                                                                                                  


                                                     
if (fabs(vt) > (fabs(vmax) - 0.02 * fabs(vmax-vmin))) {
   *result = 5e+21;
   return;
}

                                                                                               

  if (multiplier>1.e+20)  vt=fabs(vt*multiplier/1.e+40);
    else     vt=vt*multiplier;

*result=vt;
                                          
if (debug != 0) {
   finishtime = secnds(starttime);
   printf("\nVTF_S_MOS test time = %g, result = %g", finishtime, *result);
}

return;

                     
Par_error:
*result=1.0e+21;
return;

                        
} 		                      











	END USRLIB MODULE INFORMATION
*/
double VT_LSSG_an(int drain, int gate, int source, int subst, double width, double length, char type, double sat_vlow, double sat_vhigh, double sat_vds, double sat_vbs, char lin_vlow, char lin_vhigh, char lin_vds, char lin_vbs, double ss_idsearch1, double ss_idsearch2, char ss_vds, int ss_enable, double * vti_return, double * vts_return, double * ss_return, double * gm_return);


/* USRLIB MODULE INFORMATION

	MODULE NAME: vtati
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 10
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		vlow,	double,	Input,	,	,	
		vhigh,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vbs,	double,	Input,	,	,	
		ithr,	double,	Input,	,	,	
		niter,	int,	Input,	,	,	
	INCLUDES:
#include <math.h>	                
#include "lptdef.h"
#include "PARLIB400_proto.h"

#define MAX(a,b)	((a)>(b) ? (a) : (b))

	END USRLIB MODULE INFORMATION
*/
double vtati(int drain, int gate, int source, int subst, double vlow, double vhigh, double vds, double vbs, double ithr, int niter);


/* USRLIB MODULE INFORMATION

	MODULE NAME: vtati_an
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 10
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		vlow,	double,	Input,	,	,	
		vhigh,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vbs,	double,	Input,	,	,	
		ithr,	double,	Input,	,	,	
		niter,	int,	Input,	,	,	
	INCLUDES:
#include <math.h>	                
#include "lptdef.h"
#include "PARLIB400_proto.h"

#define MAX(a,b)	((a)>(b) ? (a) : (b))

	END USRLIB MODULE INFORMATION
*/
double vtati_an(int drain, int gate, int source, int subst, double vlow, double vhigh, double vds, double vbs, double ithr, int niter);


/* USRLIB MODULE INFORMATION

	MODULE NAME: vtati_loop_an
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 11
	ARGUMENTS:
		CG,	int,	Input,	,	,	
		DRAIN,	int,	Input,	,	,	
		SOURCE,	int,	Input,	,	,	
		TG,	int,	Input,	,	,	
		Loop,	long,	Input,	,	,	
		pgm_volt,	double,	Input,	,	,	
		pgm_time,	double,	Input,	,	,	
		erase_volt,	double,	Input,	,	,	
		erase_time,	double,	Input,	,	,	
		mod,	char *,	Input,	,	,	
		dut,	char *,	Input,	,	,	
	INCLUDES:
#include <math.h>	                
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <stdlib.h>
#include <ktxe_proto.h>
#include <ksox_def.h>

#include "COM_usrlib.h"
#include "LBC5_proto.h"
#include "lptdef.h"
#include "PARLIB400_proto.h"

#define MAX(a,b)	((a)>(b) ? (a) : (b))

static char const vcid[] ="$Id: Local $";





double leak4_local( int hi, int  lo1,int  lo2,int  subst, double v )
{

    extern int devint();
    double    i;                              
    double leak_fnc;                                
    double sign;
    if (v >= 0) 
        sign = 1.0;
    else
        sign = -1.0;

                             

    if (subst > 0) {
        conpin (SMU1L, lo1, lo2, subst, GND, KI_EOC);
    }
    else {
        conpin (SMU1L,lo1, lo2, GND, KI_EOC);
    }
    conpin (SMU1, hi, KI_EOC);


                        

    lorangei(SMU1, 100.0e-12);
    forcev (SMU1, v);
    delay (500);
    intgi (SMU1, &i);
    (void)devint();

     
    leak_fnc = sign * fabs(i);
  
    printf("leak func is %1.4e\n",leak_fnc);
 

    return leak_fnc;
   


}                        






double vtati_local( int drain, int gate, int source, int subst, double vlow, double vhigh, double vds, double vbs, double ithr, int niter )
{
                        
 
    extern     int fndtrg();                          

    int     lowt;                                         
    int    num;                                              
    double    avmax;                                                       
    double    sdelay;                                                     
    double    iltd;                                            
    double     crtlmt = 10.E-6;                       
    double    vtati_fnc;                                        
    double    trigger_current;                                  


                                    

    num = niter;
    if (num < 2) num = 2;
    if (num > 16) num = 16;




                         

    if (subst < 1)    {        
        conpin (GND, SMU1L, SMU2L, source, KI_EOC);
    }
    else if (fabs(vbs) < .9e-3) {     
        conpin (GND, SMU1L, SMU2L, source, subst, KI_EOC);
    }
     else {
        conpin (GND, SMU1L, SMU2L, SMU3L, source, KI_EOC);
        conpin (SMU3, subst, KI_EOC);
    }
    conpin  (SMU2, gate, KI_EOC);
    conpin  (SMU1, drain, KI_EOC);


                                                      

    lowt = fndtrg(vlow, vhigh);
    trigger_current = ithr;
    if (lowt) {
        trigig(SMU1, trigger_current);
    }
    else {
        trigil(SMU1, trigger_current);
    }
    limiti (SMU2, crtlmt);


                                                    

    avmax = MAX(fabs(vlow), fabs(vhigh));
    sdelay = tdelay(1, crtlmt, avmax);
                        

                      

    if (subst > 0 && fabs(vbs) >= .9e-3) forcev (SMU3, vbs);
      forcev (SMU1, vds);
    searchv (SMU2, vlow, vhigh, num, sdelay, &vtati_fnc);
    intgi (SMU2, &iltd);
    devint();


                                                                                                                                              

    if (fabs(vlow - vtati_fnc) <= 1.e-3) vtati_fnc = 1.e+21;
    if (fabs(vhigh - vtati_fnc) <= 1.e-3) vtati_fnc = 2.e+21;
    if (iltd == 7.0e+22) vtati_fnc = 4.0e+21;

    return(vtati_fnc);

                        
} 		                    






















void pgm_eeprom_local( int drn, int cg, int src, int tun, double vcg, int tp )
{
                        
    extern int devint();
    double    v;                              
    double    pgm_eeprom_fnc;                                

                         

    conpin( SMU1L, drn, src, tun, GND, KI_EOC);
    conpin (SMU1, cg, KI_EOC);


                        

    forcev(SMU1, vcg);
    delay (tp);

                                   
    (void)devint();

    return;
                        
} 		                      
void erase_eeprom_local( int drn, int cg, int src, int tun, double vtun, int te )
{
                        
    extern int devint();
    double    v;                              
    double    erase_eeprom_fnc;                                

                         

    conpin( SMU1L, drn, cg, src, GND, KI_EOC);
    conpin (SMU1, tun, KI_EOC);


                        

     forcev(SMU1, vtun);
     delay (te);

                                 
    (void)devint();

    return;
                        
} 		                        


















	END USRLIB MODULE INFORMATION
*/
double vtati_loop_an(int CG, int DRAIN, int SOURCE, int TG, long Loop, double pgm_volt, double pgm_time, double erase_volt, double erase_time, char * mod, char * dut);


/* USRLIB MODULE INFORMATION

	MODULE NAME: vtext4_an
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 15
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		w,	double,	Input,	,	,	
		l,	double,	Input,	,	,	
		vlow,	double,	Input,	,	,	
		vhigh,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vbs,	double,	Input,	,	,	
		ithr,	double,	Input,	,	,	
		vstep,	double,	Input,	,	,	
		npts,	int,	Input,	,	,	
		slope,	double *,	Output,	,	,	
		kflag,	int *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double vtext4_an(int drain, int gate, int source, int subst, double w, double l, double vlow, double vhigh, double vds, double vbs, double ithr, double vstep, int npts, double * slope, int * kflag);


/* USRLIB MODULE INFORMATION

	MODULE NAME: vtexts
	MODULE RETURN TYPE: double 
	NUMBER OF PARMS: 15
	ARGUMENTS:
		drain,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		subst,	int,	Input,	,	,	
		w,	double,	Input,	,	,	
		l,	double,	Input,	,	,	
		vlow,	double,	Input,	,	,	
		vhigh,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vbs,	double,	Input,	,	,	
		ithr,	double,	Input,	,	,	
		vstep,	double,	Input,	,	,	
		npts,	int,	Input,	,	,	
		slope,	double *,	Output,	,	,	
		kflag,	int *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
double vtexts(int drain, int gate, int source, int subst, double w, double l, double vlow, double vhigh, double vds, double vbs, double ithr, double vstep, int npts, double * slope, int * kflag);


/* USRLIB MODULE INFORMATION

	MODULE NAME: vtsslp_mos_sq_LD13
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 21
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		gate,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		bulk,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		width,	double,	Input,	,	,	
		length,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
		vmin,	double,	Input,	,	,	
		vmax,	double,	Input,	,	,	
		idsearch,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vbs,	double,	Input,	,	,	
		multiplier,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		delaytime,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		result,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#include <tidp1.h>
#define MAX(a,b)	((a)>(b) ? (a) : (b))
#define CDELAY  125.E-12
#define ILEAK   20.E-12 
	END USRLIB MODULE INFORMATION
*/
void  vtsslp_mos_sq_LD13(char *, int, int, int, int, int, int, double, double, char, double, double, double, double, double, double, double, double, double, double, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: VTXPL_mos_an
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 23
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		gate,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		bulk,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		width,	double,	Input,	,	,	
		length,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
		vmin,	double,	Input,	,	,	
		vmax,	double,	Input,	,	,	
		npoints,	int,	Input,	,	,	
		idmax,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vbs,	double,	Input,	,	,	
		mvtx,	double,	Input,	,	,	
		mk,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		result_vt,	double *,	Output,	,	,	
		result_k,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#include <tidp1.h>
#define MAX(a,b)	((a)>(b) ? (a) : (b))
#define CDELAY  125.E-12
#define ILEAK   20.E-12 
	END USRLIB MODULE INFORMATION
*/
void VTXPL_mos_an(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vmin, double vmax, int npoints, double idmax, double vds, double vbs, double mvtx, double mk, double intrange, double mrange, double lorange, double * result_vt, double * result_k);


/* USRLIB MODULE INFORMATION

	MODULE NAME: VTXPL_save
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 23
	ARGUMENTS:
		devname,	char *,	Input,	,	,	
		gate,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		bulk,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		chuckcon,	int,	Input,	,	,	
		width,	double,	Input,	,	,	
		length,	double,	Input,	,	,	
		type,	char,	Input,	,	,	
		vmin,	double,	Input,	,	,	
		vmax,	double,	Input,	,	,	
		npoints,	int,	Input,	,	,	
		idmax,	double,	Input,	,	,	
		vds,	double,	Input,	,	,	
		vbs,	double,	Input,	,	,	
		mvtx,	double,	Input,	,	,	
		mk,	double,	Input,	,	,	
		intrange,	double,	Input,	,	,	
		mrange,	double,	Input,	,	,	
		lorange,	double,	Input,	,	,	
		result_vt,	double *,	Output,	,	,	
		result_k,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#include <tidp1.h>

#include <stdlib.h>
#include <ktxe_proto.h>
#include <ksox_def.h>

#define MAX(a,b)	((a)>(b) ? (a) : (b))
#define CDELAY  125.E-12
#define ILEAK   20.E-12 


	END USRLIB MODULE INFORMATION
*/
void VTXPL_save(char * devname, int gate, int drain, int source, int bulk, int sub, int chuckcon, double width, double length, char type, double vmin, double vmax, int npoints, double idmax, double vds, double vbs, double mvtx, double mk, double intrange, double mrange, double lorange, double * result_vt, double * result_k);


