/* Philips function prototype and KITT header file */

/* COPY OF USRLIB SETTINGS INFORMATION */
/* [VISIBILITY] */
/* HIDDEN=0 */
/* [DEPENDENCIES] */
/* LIBS= */
/* OBJECTS= */
/* HEADERS= */
/* [NON-KULT SEARCH PATH] */
/* PATHS= */

/* USRLIB MODULE INFORMATION

	MODULE NAME: des
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 3
	ARGUMENTS:
		a,	double,	Input,	,	,	
		c,	double *,	Output,	,	,	
		d,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void  des(double, double *, double *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: g130c
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 23
	ARGUMENTS:
		hi_pin,	int,	Input,	,	,	
		lo_pin,	int,	Input,	,	,	
		jun_pin,	int,	Input,	,	,	
		extra_pins,	char *,	Input,	,	,	
		select,	int,	Input,	,	,	
		jmax,	float,	Input,	,	,	
		vopmax,	float,	Input,	,	,	
		ehigh,	float,	Input,	,	,	
		vjun,	float,	Input,	,	,	
		total_step_time,	float,	Input,	,	,	
		total_time,	float,	Input,	,	,	
		area,	float,	Input,	,	,	
		tox,	float,	Input,	,	,	
		c_extra,	float,	Input,	,	,	
		criterium,	float,	Input,	,	,	
		pin_array1,	I_ARRAY_T,	Input,	,	,	
		pin_array1_size,	int,	Input,	,	,	
		pin_array2,	I_ARRAY_T,	Input,	,	,	
		pin_array2_size,	int,	Input,	,	,	
		meas_mode,	char,	Input,	,	,	
		tid,	int *,	Output,	,	,	
		param,	F_ARRAY_T,	Output,	,	,	
		param_size,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <time.h>
#include <phillib.h>
#include <system_config.h>
	END USRLIB MODULE INFORMATION
*/
void  g130c(int, int, int, char *, int, float, float, float, float, float, float, float, float, float, float, int *, int, int *, int, char, int *, float *, int);

/* USRLIB MODULE INFORMATION

	MODULE NAME: la100c
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 18
	ARGUMENTS:
		hi_pin,	int,	Input,	,	,	
		lo_pin,	int,	Input,	,	,	
		extra_pins,	char *,	Input,	extra,	 ,	 
		v_force,	float,	Input,	 ,	 ,	 
		icompl,	float,	Input,	 ,	 ,	 
		rmin,	float,	Input,	0,	 ,	 
		rtarget,	float,	Input,	0,	 ,	 
		rmax,	float,	Input,	0,	 ,	 
		nsect,	int,	Input,	1,	 ,	 
		cap_dut,	float,	Input,	0,	 ,	 
		pin_array1,	I_ARRAY_T,	Input,	 ,	 ,	 
		pin_array1_size,	int,	Input,	25,	 ,	 
		pin_array2,	I_ARRAY_T,	Input,	 ,	 ,	 
		pin_array2_size,	int,	Input,	25,	 ,	 
		meas_mode,	char,	Input,	mode,	 ,	 
		TID,	int *,	Output,	 ,	 ,	 
		param,	F_ARRAY_T,	Output,	 ,	 ,	 
		param_size,	int,	Input,	50,	 ,	 
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void la100c(int hi_pin, int lo_pin, char * extra_pins, float v_force, float icompl, float rmin, float rtarget, float rmax, int nsect, float cap_dut, int *pin_array1, int pin_array1_size, int *pin_array2, int pin_array2_size, char meas_mode, int * TID, float *param, int param_size);


/* USRLIB MODULE INFORMATION

	MODULE NAME: la200c
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 22
	ARGUMENTS:
		fhi_pin,	int,	Input,	,	,	
		flo_pin,	int,	Input,	,	,	
		shi_pin,	int,	Input,	,	,	
		slo_pin,	int,	Input,	,	,	
		extra_pins,	char *,	Input,	 ,	 ,	 
		v_force,	float,	Input,	 ,	 ,	 
		icompl,	float,	Input,	 ,	 ,	 
		rmin,	float,	Input,	 ,	 ,	 
		rtarget,	float,	Input,	 ,	 ,	 
		rmax,	float,	Input,	 ,	 ,	 
		nsect,	int,	Input,	 ,	 ,	 
		cap_dut,	float,	Input,	 ,	 ,	 
		rcont,	float,	Input,	 ,	 ,	 
		structure,	char *,	Input,	 ,	 ,	 
		pin_array1,	I_ARRAY_T,	Input,	 ,	 ,	 
		pin_array1_size,	int,	Input,	 ,	 ,	 
		pin_array2,	I_ARRAY_T,	Input,	 ,	 ,	 
		pin_array2_size,	int,	Input,	 ,	 ,	 
		meas_mode,	char,	Input,	 ,	 ,	 
		TID,	int *,	Output,	 ,	 ,	 
		param,	F_ARRAY_T,	Output,	 ,	 ,	 
		param_size,	int,	Input,	 ,	 ,	 
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void la200c(int fhi_pin, int flo_pin, int shi_pin, int slo_pin, char * extra_pins, float v_force, float icompl, float rmin, float rtarget, float rmax, int nsect, float cap_dut, float rcont, char * structure, int *pin_array1, int pin_array1_size, int *pin_array2, int pin_array2_size, char meas_mode, int * TID, float *param, int param_size);


/* USRLIB MODULE INFORMATION

	MODULE NAME: la210c
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 16
	ARGUMENTS:
		fhi_pin,	int,	Input,	,	,	
		flo_pin,	int,	Input,	,	,	
		shi_pin,	int,	Input,	,	,	
		slo_pin,	int,	Input,	,	,	
		extra_pins,	char *,	Input,	 ,	 ,	 
		v_force,	float,	Input,	 ,	 ,	 
		cap_dut,	float,	Input,	 ,	 ,	 
		structure,	char *,	Input,	 ,	 ,	 
		pin_array1,	I_ARRAY_T,	Input,	 ,	 ,	 
		pin_array1_size,	int,	Input,	 ,	 ,	 
		pin_array2,	I_ARRAY_T,	Input,	 ,	 ,	 
		pin_array2_size,	int,	Input,	 ,	 ,	 
		meas_mode,	char,	Input,	 ,	 ,	 
		TID,	int *,	Output,	 ,	 ,	 
		param,	F_ARRAY_T,	Output,	 ,	 ,	 
		param_size,	int,	Input,	 ,	 ,	 
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void la210c(int fhi_pin, int flo_pin, int shi_pin, int slo_pin, char * extra_pins, float v_force, float cap_dut, char * structure, int *pin_array1, int pin_array1_size, int *pin_array2, int pin_array2_size, char meas_mode, int * TID, float *param, int param_size);


/* USRLIB MODULE INFORMATION

	MODULE NAME: la300c
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 28
	ARGUMENTS:
		fhi_pin,	int,	Input,	,	,	
		flo_pin,	int,	Input,	,	,	
		shi_pin,	int,	Input,	,	,	
		slo_pin,	int,	Input,	,	,	
		gat1_pin,	int,	Input,	,	,	
		gat2_pin,	int,	Input,	,	,	
		extra_pins,	char *,	Input,	 ,	 ,	 
		v_force,	float,	Input,	 ,	 ,	 
		icompl,	float,	Input,	 ,	 ,	 
		rmin,	float,	Input,	 ,	 ,	 
		rtarget,	float,	Input,	 ,	 ,	 
		rmax,	float,	Input,	 ,	 ,	 
		nsect,	int,	Input,	 ,	 ,	 
		cap_dut,	float,	Input,	 ,	 ,	 
		rcont,	float,	Input,	 ,	 ,	 
		structure,	char *,	Input,	 ,	 ,	 
		vgate1,	float,	Input,	 ,	 ,	 
		igcompl1,	float,	Input,	 ,	 ,	 
		vgate2,	float,	Input,	 ,	 ,	 
		igcompl2,	float,	Input,	 ,	 ,	 
		pin_array1,	I_ARRAY_T,	Input,	 ,	 ,	 
		pin_array1_size,	int,	Input,	 ,	 ,	 
		pin_array2,	I_ARRAY_T,	Input,	 ,	 ,	 
		pin_array2_size,	int,	Input,	 ,	 ,	 
		meas_mode,	char,	Input,	 ,	 ,	 
		TID,	int *,	Output,	 ,	 ,	 
		param,	F_ARRAY_T,	Output,	 ,	 ,	 
		param_size,	int,	Input,	 ,	 ,	 
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void la300c(int fhi_pin, int flo_pin, int shi_pin, int slo_pin, int gat1_pin, int gat2_pin, char * extra_pins, float v_force, float icompl, float rmin, float rtarget, float rmax, int nsect, float cap_dut, float rcont, char * structure, float vgate1, float igcompl1, float vgate2, float igcompl2, int *pin_array1, int pin_array1_size, int *pin_array2, int pin_array2_size, char meas_mode, int * TID, float *param, int param_size);


/* USRLIB MODULE INFORMATION

	MODULE NAME: la400c
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 28
	ARGUMENTS:
		fhi_pin,	int,	Input,	,	,	
		flo_pin,	int,	Input,	,	,	
		shi_pin,	int,	Input,	,	,	
		slo_pin,	int,	Input,	,	,	
		gat1_pin,	int,	Input,	,	,	
		gat2_pin,	int,	Input,	,	,	
		extra_pins,	char *,	Input,	 ,	 ,	 
		iforce,	float,	Input,	 ,	 ,	 
		vcompl,	float,	Input,	 ,	 ,	 
		rmin,	float,	Input,	 ,	 ,	 
		rtarget,	float,	Input,	 ,	 ,	 
		rmax,	float,	Input,	 ,	 ,	 
		nsect,	int,	Input,	 ,	 ,	 
		cap_dut,	float,	Input,	 ,	 ,	 
		rcont,	float,	Input,	 ,	 ,	 
		structure,	char *,	Input,	 ,	 ,	 
		vgate1,	float,	Input,	 ,	 ,	 
		igcompl1,	float,	Input,	 ,	 ,	 
		vgate2,	float,	Input,	 ,	 ,	 
		igcompl2,	float,	Input,	 ,	 ,	 
		pin_array1,	I_ARRAY_T,	Input,	 ,	 ,	 
		pin_array1_size,	int,	Input,	 ,	 ,	 
		pin_array2,	I_ARRAY_T,	Input,	 ,	 ,	 
		pin_array2_size,	int,	Input,	 ,	 ,	 
		meas_mode,	char,	Input,	 ,	 ,	 
		TID,	int *,	Output,	 ,	 ,	 
		param,	F_ARRAY_T,	Output,	 ,	 ,	 
		param_size,	int,	Input,	 ,	 ,	 
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void la400c(int fhi_pin, int flo_pin, int shi_pin, int slo_pin, int gat1_pin, int gat2_pin, char * extra_pins, float iforce, float vcompl, float rmin, float rtarget, float rmax, int nsect, float cap_dut, float rcont, char * structure, float vgate1, float igcompl1, float vgate2, float igcompl2, int *pin_array1, int pin_array1_size, int *pin_array2, int pin_array2_size, char meas_mode, int * TID, float *param, int param_size);


/* USRLIB MODULE INFORMATION

	MODULE NAME: lb100c
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 20
	ARGUMENTS:
		anode,	int,	Input,	,	,	
		cathode,	int,	Input,	,	,	
		subs,	int,	Input,	,	,	
		extra_pins,	char *,	Input,	,	,	
		type,	int,	Input,	,	,	
		leakfl,	int,	Input,	,	,	
		fi_forw,	float,	Input,	,	,	
		fi_rev,	float,	Input,	,	,	
		v_max1,	float,	Input,	,	,	
		v_max2,	float,	Input,	,	,	
		i_max,	float,	Input,	,	,	
		cap_dut,	float,	Input,	,	,	
		pin_array1,	I_ARRAY_T,	Input,	,	,	
		pin_array1_len,	int,	Input,	,	,	
		pin_array2,	I_ARRAY_T,	Input,	,	,	
		pin_array2_len,	int,	Input,	,	,	
		meas_mode,	char,	Input,	,	,	
		TID,	int *,	Output,	,	,	
		param,	F_ARRAY_T,	Output,	,	,	
		param_len,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void lb100c(int anode, int cathode, int subs, char * extra_pins, int type, int leakfl, float fi_forw, float fi_rev, float v_max1, float v_max2, float i_max, float cap_dut, int *pin_array1, int pin_array1_len, int *pin_array2, int pin_array2_len, char meas_mode, int * TID, float *param, int param_len);


/* USRLIB MODULE INFORMATION

	MODULE NAME: lc100c
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 20
	ARGUMENTS:
		hi_pin,	int,	Input,	,	,	
		lo_pin,	int,	Input,	,	,	
		extra_pins,	char *,	Input,	 ,	 ,	 
		v_force,	float,	Input,	 ,	 ,	 
		icompl,	float,	Input,	 ,	 ,	 
		cap_dut,	float,	Input,	 ,	 ,	 
		curr_min,	float,	Input,	 ,	 ,	 
		curr_tar,	float,	Input,	 ,	 ,	 
		curr_max,	float,	Input,	 ,	 ,	 
		rseri,	float,	Input,	 ,	 ,	 
		pin_rseri_lo,	int,	Input,	 ,	 ,	 
		pin_rseri_hi,	int,	Input,	 ,	 ,	 
		pin_array1,	I_ARRAY_T,	Input,	 ,	 ,	 
		pin_array1_size,	int,	Input,	 ,	 ,	 
		pin_array2,	I_ARRAY_T,	Input,	 ,	 ,	 
		pin_array2_size,	int,	Input,	 ,	 ,	 
		meas_mode,	char,	Input,	 ,	 ,	 
		TID,	int *,	Output,	 ,	 ,	 
		param,	F_ARRAY_T,	Output,	 ,	 ,	 
		param_size,	int,	Input,	 ,	 ,	 
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void lc100c(int hi_pin, int lo_pin, char * extra_pins, float v_force, float icompl, float cap_dut, float curr_min, float curr_tar, float curr_max, float rseri, int pin_rseri_lo, int pin_rseri_hi, int *pin_array1, int pin_array1_size, int *pin_array2, int pin_array2_size, char meas_mode, int * TID, float *param, int param_size);


/* USRLIB MODULE INFORMATION

	MODULE NAME: lc200c
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 20
	ARGUMENTS:
		hi_pin,	int,	Input,	,	,	
		lo_pin,	int,	Input,	,	,	
		extra_pins,	char *,	Input,	 ,	 ,	 
		i_force,	float,	Input,	 ,	 ,	 
		vcompl,	float,	Input,	 ,	 ,	 
		cap_dut,	float,	Input,	 ,	 ,	 
		volt_min,	float,	Input,	 ,	 ,	 
		volt_tar,	float,	Input,	 ,	 ,	 
		volt_max,	float,	Input,	 ,	 ,	 
		rshunt,	float,	Input,	 ,	 ,	 
		pin_rshunt_lo,	int,	Input,	 ,	 ,	 
		pin_rshunt_hi,	int,	Input,	 ,	 ,	 
		pin_array1,	I_ARRAY_T,	Input,	 ,	 ,	 
		pin_array1_size,	int,	Input,	 ,	 ,	 
		pin_array2,	I_ARRAY_T,	Input,	 ,	 ,	 
		pin_array2_size,	int,	Input,	 ,	 ,	 
		meas_mode,	char,	Input,	 ,	 ,	 
		TID,	int *,	Output,	 ,	 ,	 
		param,	F_ARRAY_T,	Output,	 ,	 ,	 
		param_size,	int,	Input,	 ,	 ,	 
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void lc200c(int hi_pin, int lo_pin, char * extra_pins, float i_force, float vcompl, float cap_dut, float volt_min, float volt_tar, float volt_max, float rshunt, int pin_rshunt_lo, int pin_rshunt_hi, int *pin_array1, int pin_array1_size, int *pin_array2, int pin_array2_size, char meas_mode, int * TID, float *param, int param_size);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ld100c
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 18
	ARGUMENTS:
		sour,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		drai,	int,	Input,	,	,	
		subs,	int,	Input,	,	,	
		extra_pins,	char *,	Input,	,	 ,	 
		sign,	int,	Input,	,	,	
		paral,	int,	Input,	,	,	
		select,	int,	Input,	,	,	
		temp,	float,	Input,	,	,	
		leakflag,	int,	Input,	,	,	
		pin_array1,	I_ARRAY_T,	Input,	,	,	
		pin_array1_len,	int,	Input,	,	,	
		pin_array2,	I_ARRAY_T,	Input,	,	,	
		pin_array2_len,	int,	Input,	,	,	
		meas_mode,	char,	Input,	,	,	
		TID,	int *,	Output,	,	,	
		param,	F_ARRAY_T,	Output,	,	,	
		param_len,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void ld100c(int sour, int gate, int drai, int subs, char * extra_pins, int sign, int paral, int select, float temp, int leakflag, int *pin_array1, int pin_array1_len, int *pin_array2, int pin_array2_len, char meas_mode, int * TID, float *param, int param_len);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ld150c
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 19
	ARGUMENTS:
		source,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		extra_pins,	char *,	Input,	 ,	 ,	 
		sign,	int,	Input,	 ,	 ,	 
		paral,	int,	Input,	 ,	 ,	 
		select,	int,	Input,	 ,	 ,	 
		vdsprg,	float,	Input,	 ,	 ,	 
		temp,	float,	Input,	 ,	 ,	 
		leakflag,	int,	Input,	 ,	 ,	 
		pin_array1,	I_ARRAY_T,	Input,	 ,	 ,	 
		pin_array1_size,	int,	Input,	 ,	 ,	 
		pin_array2,	I_ARRAY_T,	Input,	 ,	 ,	 
		pin_array2_size,	int,	Input,	 ,	 ,	 
		meas_mode,	char,	Input,	 ,	 ,	 
		TID,	int *,	Output,	 ,	 ,	 
		param,	F_ARRAY_T,	Output,	 ,	 ,	 
		param_size,	int,	Input,	 ,	 ,	 
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void ld150c(int source, int gate, int drain, int sub, char * extra_pins, int sign, int paral, int select, float vdsprg, float temp, int leakflag, int *pin_array1, int pin_array1_size, int *pin_array2, int pin_array2_size, char meas_mode, int * TID, float *param, int param_size);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ld200c
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 25
	ARGUMENTS:
		sour,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		drai,	int,	Input,	,	,	
		subs,	int,	Input,	,	,	
		extra_pins,	char *,	Input,	 ,	 ,	 
		sign,	int,	Input,	 ,	 ,	 
		paral,	int,	Input,	 ,	 ,	 
		select,	int,	Input,	 ,	 ,	 
		temp,	float,	Input,	 ,	 ,	 
		vt,	float,	Input,	 ,	 ,	 
		k0,	float,	Input,	 ,	 ,	 
		k,	float,	Input,	 ,	 ,	 
		vsbx,	float,	Input,	 ,	 ,	 
		beta,	float,	Input,	 ,	 ,	 
		theta1,	float,	Input,	 ,	 ,	 
		theta2,	float,	Input,	 ,	 ,	 
		leakflag,	int,	Input,	 ,	 ,	 
		pin_array1,	I_ARRAY_T,	Input,	 ,	 ,	 
		pin_array1_size,	int,	Input,	 ,	 ,	 
		pin_array2,	I_ARRAY_T,	Input,	 ,	 ,	 
		pin_array2_size,	int,	Input,	 ,	 ,	 
		meas_mode,	char,	Input,	 ,	 ,	 
		TID,	int *,	Output,	 ,	 ,	 
		param,	F_ARRAY_T,	Output,	 ,	 ,	 
		param_size,	int,	Input,	 ,	 ,	 
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void ld200c(int sour, int gate, int drai, int subs, char * extra_pins, int sign, int paral, int select, float temp, float vt, float k0, float k, float vsbx, float beta, float theta1, float theta2, int leakflag, int *pin_array1, int pin_array1_size, int *pin_array2, int pin_array2_size, char meas_mode, int * TID, float *param, int param_size);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ld300c
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 21
	ARGUMENTS:
		sour,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		drai,	int,	Input,	,	,	
		subs,	int,	Input,	,	,	
		extra_pins,	char *,	Input,	 ,	 ,	 
		sign,	int,	Input,	 ,	 ,	 
		vgs,	float,	Input,	 ,	 ,	 
		vbs,	float,	Input,	 ,	 ,	 
		vdsmax,	float,	Input,	 ,	 ,	 
		i_force,	float,	Input,	 ,	 ,	 
		width,	float,	Input,	 ,	 ,	 
		path,	float,	Input,	 ,	 ,	 
		dut_cap,	float,	Input,	 ,	 ,	 
		pin_array1,	I_ARRAY_T,	Input,	 ,	 ,	 
		pin_array1_size,	int,	Input,	 ,	 ,	 
		pin_array2,	I_ARRAY_T,	Input,	 ,	 ,	 
		pin_array2_size,	int,	Input,	 ,	 ,	 
		meas_mode,	char,	Input,	 ,	 ,	 
		TID,	int *,	Output,	 ,	 ,	 
		param,	F_ARRAY_T,	Output,	 ,	 ,	 
		param_size,	int,	Input,	 ,	 ,	 
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void ld300c(int sour, int gate, int drai, int subs, char * extra_pins, int sign, float vgs, float vbs, float vdsmax, float i_force, float width, float path, float dut_cap, int *pin_array1, int pin_array1_size, int *pin_array2, int pin_array2_size, char meas_mode, int * TID, float *param, int param_size);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ld310c
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 22
	ARGUMENTS:
		sour,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		drai,	int,	Input,	,	,	
		subs,	int,	Input,	,	,	
		extra_pins,	char *,	Input,	 ,	 ,	 
		sign,	int,	Input,	 ,	 ,	 
		vdstrt,	float,	Input,	 ,	 ,	 
		vdmax,	float,	Input,	 ,	 ,	 
		vgs,	float,	Input,	 ,	 ,	 
		vbs,	float,	Input,	 ,	 ,	 
		i_square,	float,	Input,	 ,	 ,	 
		width,	float,	Input,	 ,	 ,	 
		length,	float,	Input,	 ,	 ,	 
		leakflag,	int,	Input,	 ,	 ,	 
		pin_array1,	I_ARRAY_T,	Input,	 ,	 ,	 
		pin_array1_size,	int,	Input,	 ,	 ,	 
		pin_array2,	I_ARRAY_T,	Input,	 ,	 ,	 
		pin_array2_size,	int,	Input,	 ,	 ,	 
		meas_mode,	char,	Input,	 ,	 ,	 
		TID,	int *,	Output,	 ,	 ,	 
		param,	F_ARRAY_T,	Output,	 ,	 ,	 
		param_size,	int,	Input,	 ,	 ,	 
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void ld310c(int sour, int gate, int drai, int subs, char * extra_pins, int sign, float vdstrt, float vdmax, float vgs, float vbs, float i_square, float width, float length, int leakflag, int *pin_array1, int pin_array1_size, int *pin_array2, int pin_array2_size, char meas_mode, int * TID, float *param, int param_size);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ld400c
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 18
	ARGUMENTS:
		sour,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		drai,	int,	Input,	,	,	
		subs,	int,	Input,	,	,	
		extra_pins,	char *,	Input,	 ,	 ,	 
		sign,	int,	Input,	 ,	 ,	 
		vds,	float,	Input,	 ,	 ,	 
		vgs,	float,	Input,	 ,	 ,	 
		vbs,	float,	Input,	 ,	 ,	 
		leakflag,	int,	Input,	 ,	 ,	 
		pin_array1,	I_ARRAY_T,	Input,	 ,	 ,	 
		pin_array1_size,	int,	Input,	 ,	 ,	 
		pin_array2,	I_ARRAY_T,	Input,	 ,	 ,	 
		pin_array2_size,	int,	Input,	 ,	 ,	 
		meas_mode,	char,	Input,	 ,	 ,	 
		TID,	int *,	Output,	 ,	 ,	 
		param,	F_ARRAY_T,	Output,	 ,	 ,	 
		param_size,	int,	Input,	 ,	 ,	 
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void ld400c(int sour, int gate, int drai, int subs, char * extra_pins, int sign, float vds, float vgs, float vbs, int leakflag, int *pin_array1, int pin_array1_size, int *pin_array2, int pin_array2_size, char meas_mode, int * TID, float *param, int param_size);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ld500c
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 20
	ARGUMENTS:
		sour,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		drai,	int,	Input,	,	,	
		subs,	int,	Input,	,	,	
		extra_pins,	char *,	Input,	 ,	 ,	 
		sign,	int,	Input,	 ,	 ,	 
		vds,	float,	Input,	 ,	 ,	 
		vgs,	float,	Input,	 ,	 ,	 
		vbs,	float,	Input,	 ,	 ,	 
		i_search,	float,	Input,	 ,	 ,	 
		leakflag,	int,	Input,	 ,	 ,	 
		ld500_flag,	int,	Input,	 ,	 ,	 
		pin_array1,	I_ARRAY_T,	Input,	 ,	 ,	 
		pin_array1_size,	int,	Input,	 ,	 ,	 
		pin_array2,	I_ARRAY_T,	Input,	 ,	 ,	 
		pin_array2_size,	int,	Input,	 ,	 ,	 
		meas_mode,	char,	Input,	 ,	 ,	 
		TID,	int *,	Output,	 ,	 ,	 
		param,	F_ARRAY_T,	Output,	 ,	 ,	 
		param_size,	int,	Input,	 ,	 ,	 
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void ld500c(int sour, int gate, int drai, int subs, char * extra_pins, int sign, float vds, float vgs, float vbs, float i_search, int leakflag, int ld500_flag, int *pin_array1, int pin_array1_size, int *pin_array2, int pin_array2_size, char meas_mode, int * TID, float *param, int param_size);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ld600c
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 22
	ARGUMENTS:
		sour,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		drai,	int,	Input,	,	,	
		subs,	int,	Input,	,	,	
		extra_pins,	char *,	Input,	 ,	 ,	 
		sign,	int,	Input,	 ,	 ,	 
		vds,	float,	Input,	 ,	 ,	 
		vgmin,	float,	Input,	 ,	 ,	 
		vgmax,	float,	Input,	 ,	 ,	 
		vbs,	float,	Input,	 ,	 ,	 
		i_square,	float,	Input,	 ,	 ,	 
		width,	float,	Input,	 ,	 ,	 
		length,	float,	Input,	 ,	 ,	 
		leakflag,	int,	Input,	 ,	 ,	 
		pin_array1,	I_ARRAY_T,	Input,	 ,	 ,	 
		pin_array1_size,	int,	Input,	 ,	 ,	 
		pin_array2,	I_ARRAY_T,	Input,	 ,	 ,	 
		pin_array2_size,	int,	Input,	 ,	 ,	 
		meas_mode,	char,	Input,	 ,	 ,	 
		TID,	int *,	Output,	 ,	 ,	 
		param,	F_ARRAY_T,	Output,	 ,	 ,	 
		param_size,	int,	Input,	 ,	 ,	 
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void ld600c(int sour, int gate, int drai, int subs, char * extra_pins, int sign, float vds, float vgmin, float vgmax, float vbs, float i_square, float width, float length, int leakflag, int *pin_array1, int pin_array1_size, int *pin_array2, int pin_array2_size, char meas_mode, int * TID, float *param, int param_size);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ld610c
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 21
	ARGUMENTS:
		source,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		extra_pins,	char *,	Input,	 ,	 ,	 
		sign,	int,	Input,	 ,	 ,	 
		vds,	float,	Input,	 ,	 ,	 
		vgs,	float,	Input,	 ,	 ,	 
		vbs,	float,	Input,	 ,	 ,	 
		i_square,	float,	Input,	 ,	 ,	 
		width,	float,	Input,	 ,	 ,	 
		length,	float,	Input,	 ,	 ,	 
		leakflag,	int,	Input,	 ,	 ,	 
		pin_array1,	I_ARRAY_T,	Input,	 ,	 ,	 
		pin_array1_size,	int,	Input,	 ,	 ,	 
		pin_array2,	I_ARRAY_T,	Input,	 ,	 ,	 
		pin_array2_size,	int,	Input,	 ,	 ,	 
		meas_mode,	char,	Input,	 ,	 ,	 
		TID,	int *,	Output,	 ,	 ,	 
		param,	F_ARRAY_T,	Output,	 ,	 ,	 
		param_size,	int,	Input,	 ,	 ,	 
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void ld610c(int source, int gate, int drain, int sub, char * extra_pins, int sign, float vds, float vgs, float vbs, float i_square, float width, float length, int leakflag, int *pin_array1, int pin_array1_size, int *pin_array2, int pin_array2_size, char meas_mode, int * TID, float *param, int param_size);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ld620c
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 21
	ARGUMENTS:
		source,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		extra_pins,	char *,	Input,	 ,	 ,	 
		sign,	int,	Input,	 ,	 ,	 
		vds,	float,	Input,	 ,	 ,	 
		vgmax,	float,	Input,	 ,	 ,	 
		vbs,	float,	Input,	 ,	 ,	 
		i_square,	float,	Input,	 ,	 ,	 
		width,	float,	Input,	 ,	 ,	 
		length,	float,	Input,	 ,	 ,	 
		leakflag,	int,	Input,	 ,	 ,	 
		pin_array1,	I_ARRAY_T,	Input,	 ,	 ,	 
		pin_array1_size,	int,	Input,	 ,	 ,	 
		pin_array2,	I_ARRAY_T,	Input,	 ,	 ,	 
		pin_array2_size,	int,	Input,	 ,	 ,	 
		meas_mode,	char,	Input,	 ,	 ,	 
		TID,	int *,	Output,	 ,	 ,	 
		param,	F_ARRAY_T,	Output,	 ,	 ,	 
		param_size,	int,	Input,	 ,	 ,	 
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void ld620c(int source, int gate, int drain, int sub, char * extra_pins, int sign, float vds, float vgmax, float vbs, float i_square, float width, float length, int leakflag, int *pin_array1, int pin_array1_size, int *pin_array2, int pin_array2_size, char meas_mode, int * TID, float *param, int param_size);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ld700c
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 20
	ARGUMENTS:
		source,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		extra_pins,	char *,	Input,	 ,	 ,	 
		vds,	float,	Input,	 ,	 ,	 
		vgs,	float,	Input,	 ,	 ,	 
		vbs,	float,	Input,	 ,	 ,	 
		is,	int,	Input,	 ,	 ,	 
		id,	int,	Input,	 ,	 ,	 
		ib,	int,	Input,	 ,	 ,	 
		ig,	int,	Input,	 ,	 ,	 
		pin_array1,	I_ARRAY_T,	Input,	 ,	 ,	 
		pin_array1_size,	int,	Input,	 ,	 ,	 
		pin_array2,	I_ARRAY_T,	Input,	 ,	 ,	 
		pin_array2_size,	int,	Input,	 ,	 ,	 
		meas_mode,	char,	Input,	 ,	 ,	 
		TID,	int *,	Output,	 ,	 ,	 
		param,	F_ARRAY_T,	Output,	 ,	 ,	 
		param_size,	int,	Input,	 ,	 ,	 
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void ld700c(int source, int gate, int drain, int sub, char * extra_pins, float vds, float vgs, float vbs, int is, int id, int ib, int ig, int *pin_array1, int pin_array1_size, int *pin_array2, int pin_array2_size, char meas_mode, int * TID, float *param, int param_size);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ld710c
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 20
	ARGUMENTS:
		source,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		extra_pins,	char *,	Input,	 ,	 ,	 
		vds,	float,	Input,	 ,	 ,	 
		vgs,	float,	Input,	 ,	 ,	 
		vbs,	float,	Input,	 ,	 ,	 
		is,	int,	Input,	 ,	 ,	 
		id,	int,	Input,	 ,	 ,	 
		ib,	int,	Input,	 ,	 ,	 
		ig,	int,	Input,	 ,	 ,	 
		pin_array1,	I_ARRAY_T,	Input,	 ,	 ,	 
		pin_array1_size,	int,	Input,	 ,	 ,	 
		pin_array2,	I_ARRAY_T,	Input,	 ,	 ,	 
		pin_array2_size,	int,	Input,	 ,	 ,	 
		meas_mode,	char,	Input,	 ,	 ,	 
		TID,	int *,	Output,	 ,	 ,	 
		param,	F_ARRAY_T,	Output,	 ,	 ,	 
		param_size,	int,	Input,	 ,	 ,	 
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void ld710c(int source, int gate, int drain, int sub, char * extra_pins, float vds, float vgs, float vbs, int is, int id, int ib, int ig, int *pin_array1, int pin_array1_size, int *pin_array2, int pin_array2_size, char meas_mode, int * TID, float *param, int param_size);


/* USRLIB MODULE INFORMATION

	MODULE NAME: le100c
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 19
	ARGUMENTS:
		source,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		sub,	int,	Input,	,	,	
		extra_pins,	char *,	Input,	 ,	 ,	 
		sign,	int,	Input,	 ,	 ,	 
		select,	int,	Input,	 ,	 ,	 
		istart,	float,	Input,	 ,	 ,	 
		vgmax,	float,	Input,	 ,	 ,	 
		temp,	int,	Input,	 ,	 ,	 
		leakflag,	int,	Input,	 ,	 ,	 
		pin_array1,	I_ARRAY_T,	Input,	 ,	 ,	 
		pin_array1_size,	int,	Input,	 ,	 ,	 
		pin_array2,	I_ARRAY_T,	Input,	 ,	 ,	 
		pin_array2_size,	int,	Input,	 ,	 ,	 
		meas_mode,	char,	Input,	 ,	 ,	 
		TID,	int *,	Output,	 ,	 ,	 
		param,	F_ARRAY_T,	Output,	 ,	 ,	 
		param_size,	int,	Input,	 ,	 ,	 
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void le100c(int source, int gate, int drain, int sub, char * extra_pins, int sign, int select, float istart, float vgmax, int temp, int leakflag, int *pin_array1, int pin_array1_size, int *pin_array2, int pin_array2_size, char meas_mode, int * TID, float *param, int param_size);


/* USRLIB MODULE INFORMATION

	MODULE NAME: le300c
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 22
	ARGUMENTS:
		sour,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		drai,	int,	Input,	,	,	
		subs,	int,	Input,	,	,	
		extra_pins,	char *,	Input,	 ,	 ,	 
		sign,	int,	Input,	 ,	 ,	 
		vds,	float,	Input,	 ,	 ,	 
		vgstrt,	float,	Input,	 ,	 ,	 
		vgmax,	float,	Input,	 ,	 ,	 
		vbs,	float,	Input,	 ,	 ,	 
		i_square,	float,	Input,	 ,	 ,	 
		width,	float,	Input,	 ,	 ,	 
		length,	float,	Input,	 ,	 ,	 
		leakflag,	int,	Input,	 ,	 ,	 
		pin_array1,	I_ARRAY_T,	Input,	 ,	 ,	 
		pin_array1_size,	int,	Input,	 ,	 ,	 
		pin_array2,	I_ARRAY_T,	Input,	 ,	 ,	 
		pin_array2_size,	int,	Input,	 ,	 ,	 
		meas_mode,	char,	Input,	 ,	 ,	 
		TID,	int *,	Output,	 ,	 ,	 
		param,	F_ARRAY_T,	Output,	 ,	 ,	 
		param_size,	int,	Input,	 ,	 ,	 
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void le300c(int sour, int gate, int drai, int subs, char * extra_pins, int sign, float vds, float vgstrt, float vgmax, float vbs, float i_square, float width, float length, int leakflag, int *pin_array1, int pin_array1_size, int *pin_array2, int pin_array2_size, char meas_mode, int * TID, float *param, int param_size);


/* USRLIB MODULE INFORMATION

	MODULE NAME: le500c
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 22
	ARGUMENTS:
		sour,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		drai,	int,	Input,	,	,	
		subs,	int,	Input,	,	,	
		extra_pins,	char *,	Input,	 ,	 ,	 
		sign,	int,	Input,	 ,	 ,	 
		vdstrt,	float,	Input,	 ,	 ,	 
		vdmax,	float,	Input,	 ,	 ,	 
		vgs,	float,	Input,	 ,	 ,	 
		vbs,	float,	Input,	 ,	 ,	 
		i_square,	float,	Input,	 ,	 ,	 
		width,	float,	Input,	 ,	 ,	 
		length,	float,	Input,	 ,	 ,	 
		leakflag,	int,	Input,	 ,	 ,	 
		pin_array1,	I_ARRAY_T,	Input,	 ,	 ,	 
		pin_array1_size,	int,	Input,	 ,	 ,	 
		pin_array2,	I_ARRAY_T,	Input,	 ,	 ,	 
		pin_array2_size,	int,	Input,	 ,	 ,	 
		meas_mode,	char,	Input,	 ,	 ,	 
		TID,	int *,	Output,	 ,	 ,	 
		param,	F_ARRAY_T,	Output,	 ,	 ,	 
		param_size,	int,	Input,	 ,	 ,	 
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void le500c(int sour, int gate, int drai, int subs, char * extra_pins, int sign, float vdstrt, float vdmax, float vgs, float vbs, float i_square, float width, float length, int leakflag, int *pin_array1, int pin_array1_size, int *pin_array2, int pin_array2_size, char meas_mode, int * TID, float *param, int param_size);


/* USRLIB MODULE INFORMATION

	MODULE NAME: lf350c
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 21
	ARGUMENTS:
		emit,	int,	Input,	,	,	
		base,	int,	Input,	,	,	
		coll,	int,	Input,	,	,	
		subs,	int,	Input,	,	,	
		extra_pins,	char *,	Input,	 ,	 ,	 
		ie,	float,	Input,	 ,	 ,	 
		vcb,	float,	Input,	 ,	 ,	 
		vsb,	float,	Input,	 ,	 ,	 
		sign,	int,	Input,	 ,	 ,	 
		ib,	int,	Input,	 ,	 ,	 
		ic,	int,	Input,	 ,	 ,	 
		is,	int,	Input,	 ,	 ,	 
		slow,	int,	Input,	 ,	 ,	 
		pin_array1,	I_ARRAY_T,	Input,	 ,	 ,	 
		pin_array1_size,	int,	Input,	 ,	 ,	 
		pin_array2,	I_ARRAY_T,	Input,	 ,	 ,	 
		pin_array2_size,	int,	Input,	 ,	 ,	 
		meas_mode,	char,	Input,	 ,	 ,	 
		TID,	int *,	Output,	 ,	 ,	 
		param,	F_ARRAY_T,	Output,	 ,	 ,	 
		param_size,	int,	Input,	 ,	 ,	 
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void lf350c(int emit, int base, int coll, int subs, char * extra_pins, float ie, float vcb, float vsb, int sign, int ib, int ic, int is, int slow, int *pin_array1, int pin_array1_size, int *pin_array2, int pin_array2_size, char meas_mode, int * TID, float *param, int param_size);


/* USRLIB MODULE INFORMATION

	MODULE NAME: lf355c
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 22
	ARGUMENTS:
		emit,	int,	Input,	,	,	
		base,	int,	Input,	,	,	
		coll,	int,	Input,	,	,	
		subs,	int,	Input,	,	,	
		extra_pins,	char *,	Input,	 ,	 ,	 
		ie,	F_ARRAY_T,	Input,	 ,	 ,	 
		ie_size,	int,	Input,	 ,	 ,	 
		vcb,	float,	Input,	 ,	 ,	 
		vsb,	float,	Input,	 ,	 ,	 
		sign,	int,	Input,	 ,	 ,	 
		ib,	int,	Input,	 ,	 ,	 
		ic,	int,	Input,	 ,	 ,	 
		is,	int,	Input,	 ,	 ,	 
		slow,	int,	Input,	 ,	 ,	 
		pin_array1,	I_ARRAY_T,	Input,	 ,	 ,	 
		pin_array1_size,	int,	Input,	 ,	 ,	 
		pin_array2,	I_ARRAY_T,	Input,	 ,	 ,	 
		pin_array2_size,	int,	Input,	 ,	 ,	 
		meas_mode,	char,	Input,	 ,	 ,	 
		TID,	int *,	Output,	 ,	 ,	 
		param,	F_ARRAY_T,	Output,	 ,	 ,	 
		param_size,	int,	Input,	 ,	 ,	 
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void lf355c(int emit, int base, int coll, int subs, char * extra_pins, float *ie, int ie_size, float vcb, float vsb, int sign, int ib, int ic, int is, int slow, int *pin_array1, int pin_array1_size, int *pin_array2, int pin_array2_size, char meas_mode, int * TID, float *param, int param_size);


/* USRLIB MODULE INFORMATION

	MODULE NAME: lf365c
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 22
	ARGUMENTS:
		emit,	int,	Input,	,	,	
		base,	int,	Input,	,	,	
		coll,	int,	Input,	,	,	
		subs,	int,	Input,	,	,	
		extra_pins,	char *,	Input,	 ,	 ,	 
		fvbe,	float,	Input,	 ,	 ,	 
		fvcb,	float,	Input,	 ,	 ,	 
		fvsb,	float,	Input,	 ,	 ,	 
		sign,	int,	Input,	 ,	 ,	 
		ib,	int,	Input,	 ,	 ,	 
		ic,	int,	Input,	 ,	 ,	 
		ie,	int,	Input,	 ,	 ,	 
		is,	int,	Input,	 ,	 ,	 
		slow,	int,	Input,	 ,	 ,	 
		pin_array1,	I_ARRAY_T,	Input,	 ,	 ,	 
		pin_array1_size,	int,	Input,	 ,	 ,	 
		pin_array2,	I_ARRAY_T,	Input,	 ,	 ,	 
		pin_array2_size,	int,	Input,	 ,	 ,	 
		meas_mode,	char,	Input,	 ,	 ,	 
		TID,	int *,	Output,	 ,	 ,	 
		param,	F_ARRAY_T,	Output,	 ,	 ,	 
		param_size,	int,	Input,	 ,	 ,	 
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void lf365c(int emit, int base, int coll, int subs, char * extra_pins, float fvbe, float fvcb, float fvsb, int sign, int ib, int ic, int ie, int is, int slow, int *pin_array1, int pin_array1_size, int *pin_array2, int pin_array2_size, char meas_mode, int * TID, float *param, int param_size);


/* USRLIB MODULE INFORMATION

	MODULE NAME: lf375c
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 29
	ARGUMENTS:
		emit,	int,	Input,	,	,	
		base,	int,	Input,	,	,	
		coll,	int,	Input,	,	,	
		subs,	int,	Input,	,	,	
		extra_pins,	char *,	Input,	 ,	 ,	 
		fvcb,	float,	Input,	 ,	 ,	 
		fvsb,	float,	Input,	 ,	 ,	 
		sign,	int,	Input,	 ,	 ,	 
		slow,	int,	Input,	 ,	 ,	 
		leakfl,	int,	Input,	 ,	 ,	 
		fvleak,	float,	Input,	 ,	 ,	 
		fvbe,	F_ARRAY_T,	Input,	 ,	 ,	 
		fvbe_size,	int,	Input,	 ,	 ,	 
		ib,	I_ARRAY_T,	Input,	 ,	 ,	 
		ib_size,	int,	Input,	 ,	 ,	 
		ic,	I_ARRAY_T,	Input,	 ,	 ,	 
		ic_size,	int,	Input,	 ,	 ,	 
		ie,	I_ARRAY_T,	Input,	 ,	 ,	 
		ie_size,	int,	Input,	 ,	 ,	 
		is,	I_ARRAY_T,	Input,	 ,	 ,	 
		is_size,	int,	Input,	 ,	 ,	 
		pin_array1,	I_ARRAY_T,	Input,	 ,	 ,	 
		pin_array1_size,	int,	Input,	 ,	 ,	 
		pin_array2,	I_ARRAY_T,	Input,	 ,	 ,	 
		pin_array2_size,	int,	Input,	 ,	 ,	 
		meas_mode,	char,	Input,	 ,	 ,	 
		tid,	int *,	Output,	 ,	 ,	 
		param,	F_ARRAY_T,	Output,	 ,	 ,	 
		param_size,	int,	Input,	 ,	 ,	 
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void lf375c(int emit, int base, int coll, int subs, char * extra_pins, float fvcb, float fvsb, int sign, int slow, int leakfl, float fvleak, float *fvbe, int fvbe_size, int *ib, int ib_size, int *ic, int ic_size, int *ie, int ie_size, int *is, int is_size, int *pin_array1, int pin_array1_size, int *pin_array2, int pin_array2_size, char meas_mode, int * tid, float *param, int param_size);


/* USRLIB MODULE INFORMATION

	MODULE NAME: lf380c
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 14
	ARGUMENTS:
		emit,	int,	Input,	,	,	
		base,	int,	Input,	,	,	
		coll,	int,	Input,	,	,	
		extra_pins,	char *,	Input,	 ,	 ,	 
		fib,	float,	Input,	 ,	 ,	 
		sign,	int,	Input,	 ,	 ,	 
		pin_array1,	I_ARRAY_T,	Input,	 ,	 ,	 
		pin_array1_size,	int,	Input,	 ,	 ,	 
		pin_array2,	I_ARRAY_T,	Input,	 ,	 ,	 
		pin_array2_size,	int,	Input,	 ,	 ,	 
		meas_mode,	char,	Input,	 ,	 ,	 
		tid,	int *,	Output,	 ,	 ,	 
		param,	F_ARRAY_T,	Output,	 ,	 ,	 
		param_size,	int,	Input,	 ,	 ,	 
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void lf380c(int emit, int base, int coll, char * extra_pins, float fib, int sign, int *pin_array1, int pin_array1_size, int *pin_array2, int pin_array2_size, char meas_mode, int * tid, float *param, int param_size);


/* USRLIB MODULE INFORMATION

	MODULE NAME: lf385c
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 15
	ARGUMENTS:
		emit,	int,	Input,	,	,	
		base,	int,	Input,	,	,	
		coll,	int,	Input,	,	,	
		extra_pins,	char *,	Input,	 ,	 ,	 
		sense_pin,	int,	Input,	 ,	 ,	 
		fib,	float,	Input,	 ,	 ,	 
		sign,	int,	Input,	 ,	 ,	 
		pin_array1,	I_ARRAY_T,	Input,	 ,	 ,	 
		pin_array1_size,	int,	Input,	 ,	 ,	 
		pin_array2,	I_ARRAY_T,	Input,	 ,	 ,	 
		pin_array2_size,	int,	Input,	 ,	 ,	 
		meas_mode,	char,	Input,	 ,	 ,	 
		tid,	int *,	Output,	 ,	 ,	 
		param,	F_ARRAY_T,	Output,	 ,	 ,	 
		param_size,	int,	Input,	 ,	 ,	 
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void lf385c(int emit, int base, int coll, char * extra_pins, int sense_pin, float fib, int sign, int *pin_array1, int pin_array1_size, int *pin_array2, int pin_array2_size, char meas_mode, int * tid, float *param, int param_size);


/* USRLIB MODULE INFORMATION

	MODULE NAME: lf500c
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 17
	ARGUMENTS:
		emit,	int,	Input,	,	,	
		base,	int,	Input,	,	,	
		coll,	int,	Input,	,	,	
		subs,	int,	Input,	,	,	
		extra_pins,	char *,	Input,	 ,	 ,	 
		sign,	int,	Input,	 ,	 ,	 
		ficstat,	float,	Input,	 ,	 ,	 
		fratio,	float,	Input,	 ,	 ,	 
		fv_max,	float,	Input,	 ,	 ,	 
		fvsubs,	float,	Input,	 ,	 ,	 
		fieea,	float,	Input,	 ,	 ,	 
		fvcbea1,	float,	Input,	 ,	 ,	 
		fvcbea2,	float,	Input,	 ,	 ,	 
		leakfl,	int,	Input,	 ,	 ,	 
		TID,	int *,	Output,	 ,	 ,	 
		param,	F_ARRAY_T,	Output,	 ,	 ,	 
		param_size,	int,	Input,	 ,	 ,	 
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void lf500c(int emit, int base, int coll, int subs, char * extra_pins, int sign, float ficstat, float fratio, float fv_max, float fvsubs, float fieea, float fvcbea1, float fvcbea2, int leakfl, int * TID, float *param, int param_size);


/* USRLIB MODULE INFORMATION

	MODULE NAME: lf560c
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 22
	ARGUMENTS:
		emit,	int,	Input,	,	,	
		base,	int,	Input,	,	,	
		coll,	int,	Input,	,	,	
		subs,	int,	Input,	,	,	
		extra_pins,	char *,	Input,	 ,	 ,	 
		sign,	int,	Input,	 ,	 ,	 
		ficstat,	float,	Input,	 ,	 ,	 
		fratio,	float,	Input,	 ,	 ,	 
		fv_max,	float,	Input,	 ,	 ,	 
		fvsubs,	float,	Input,	 ,	 ,	 
		fveea,	float,	Input,	 ,	 ,	 
		fvcbea1,	float,	Input,	 ,	 ,	 
		fvcbea2,	float,	Input,	 ,	 ,	 
		leakfl,	int,	Input,	 ,	 ,	 
		pin_array1,	I_ARRAY_T,	Input,	 ,	 ,	 
		pin_array1_size,	int,	Input,	 ,	 ,	 
		pin_array2,	I_ARRAY_T,	Input,	 ,	 ,	 
		pin_array2_size,	int,	Input,	 ,	 ,	 
		meas_mode,	char,	Input,	 ,	 ,	 
		TID,	int *,	Output,	 ,	 ,	 
		param,	F_ARRAY_T,	Output,	 ,	 ,	 
		param_size,	int,	Input,	 ,	 ,	 
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void lf560c(int emit, int base, int coll, int subs, char * extra_pins, int sign, float ficstat, float fratio, float fv_max, float fvsubs, float fveea, float fvcbea1, float fvcbea2, int leakfl, int *pin_array1, int pin_array1_size, int *pin_array2, int pin_array2_size, char meas_mode, int * TID, float *param, int param_size);


/* USRLIB MODULE INFORMATION

	MODULE NAME: lf600c
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 18
	ARGUMENTS:
		emit,	int,	Input,	,	,	
		base,	int,	Input,	,	,	
		coll,	int,	Input,	,	,	
		subs,	int,	Input,	,	,	
		extra_pins,	char *,	Input,	 ,	 ,	 
		sign,	int,	Input,	 ,	 ,	 
		open_pin,	int,	Input,	 ,	 ,	 
		ficeo,	float,	Input,	 ,	 ,	 
		fiebo,	float,	Input,	 ,	 ,	 
		ficbo,	float,	Input,	 ,	 ,	 
		ficso,	float,	Input,	 ,	 ,	 
		fvcemax,	float,	Input,	 ,	 ,	 
		fvebmax,	float,	Input,	 ,	 ,	 
		fvcbmax,	float,	Input,	 ,	 ,	 
		fvcsmax,	float,	Input,	 ,	 ,	 
		TID,	int *,	Output,	 ,	 ,	 
		param,	F_ARRAY_T,	Output,	 ,	 ,	 
		param_size,	int,	Input,	 ,	 ,	 
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void lf600c(int emit, int base, int coll, int subs, char * extra_pins, int sign, int open_pin, float ficeo, float fiebo, float ficbo, float ficso, float fvcemax, float fvebmax, float fvcbmax, float fvcsmax, int * TID, float *param, int param_size);


/* USRLIB MODULE INFORMATION

	MODULE NAME: lf610c
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 28
	ARGUMENTS:
		emit,	int,	Input,	,	,	
		base,	int,	Input,	,	,	
		coll,	int,	Input,	,	,	
		subs,	int,	Input,	,	,	
		extra_pins,	char *,	Input,	 ,	 ,	 
		sign,	int,	Input,	 ,	 ,	 
		open_pin,	int,	Input,	 ,	 ,	 
		ficeo,	float,	Input,	 ,	 ,	 
		fiebo,	float,	Input,	 ,	 ,	 
		ficbo,	float,	Input,	 ,	 ,	 
		ficso,	float,	Input,	 ,	 ,	 
		fvcemin,	float,	Input,	 ,	 ,	 
		fvcemax,	float,	Input,	 ,	 ,	 
		fvebmin,	float,	Input,	 ,	 ,	 
		fvebmax,	float,	Input,	 ,	 ,	 
		fvcbmin,	float,	Input,	 ,	 ,	 
		fvcbmax,	float,	Input,	 ,	 ,	 
		fvcsmin,	float,	Input,	 ,	 ,	 
		fvcsmax,	float,	Input,	 ,	 ,	 
		frslin,	float,	Input,	 ,	 ,	 
		pin_array1,	I_ARRAY_T,	Input,	 ,	 ,	 
		pin_array1_size,	int,	Input,	 ,	 ,	 
		pin_array2,	I_ARRAY_T,	Input,	 ,	 ,	 
		pin_array2_size,	int,	Input,	 ,	 ,	 
		meas_mode,	char,	Input,	 ,	 ,	 
		TID,	int *,	Output,	 ,	 ,	 
		param,	F_ARRAY_T,	Output,	 ,	 ,	 
		param_size,	int,	Input,	 ,	 ,	 
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void lf610c(int emit, int base, int coll, int subs, char * extra_pins, int sign, int open_pin, float ficeo, float fiebo, float ficbo, float ficso, float fvcemin, float fvcemax, float fvebmin, float fvebmax, float fvcbmin, float fvcbmax, float fvcsmin, float fvcsmax, float frslin, int *pin_array1, int pin_array1_size, int *pin_array2, int pin_array2_size, char meas_mode, int * TID, float *param, int param_size);


/* USRLIB MODULE INFORMATION

	MODULE NAME: lg100c
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 20
	ARGUMENTS:
		hi_pin,	int,	Input,	,	,	
		lo_pin,	int,	Input,	,	,	
		jun_pin,	int,	Input,	,	,	
		extra_pins,	char *,	Input,	 ,	 ,	 
		forcej,	float,	Input,	 ,	 ,	 
		vlo,	float,	Input,	 ,	 ,	 
		vhi,	float,	Input,	 ,	 ,	 
		vjun,	float,	Input,	 ,	 ,	 
		time_limit,	float,	Input,	 ,	 ,	 
		area,	float,	Input,	 ,	 ,	 
		tox,	float,	Input,	 ,	 ,	 
		c_extra,	float,	Input,	 ,	 ,	 
		pin_array1,	I_ARRAY_T,	Input,	 ,	 ,	 
		pin_array1_size,	int,	Input,	 ,	 ,	 
		pin_array2,	I_ARRAY_T,	Input,	 ,	 ,	 
		pin_array2_size,	int,	Input,	 ,	 ,	 
		meas_mode,	char,	Input,	 ,	 ,	 
		TID,	int *,	Output,	 ,	 ,	 
		param,	F_ARRAY_T,	Output,	 ,	 ,	 
		param_size,	int,	Input,	 ,	 ,	 
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void lg100c(int hi_pin, int lo_pin, int jun_pin, char * extra_pins, float forcej, float vlo, float vhi, float vjun, float time_limit, float area, float tox, float c_extra, int *pin_array1, int pin_array1_size, int *pin_array2, int pin_array2_size, char meas_mode, int * TID, float *param, int param_size);


/* USRLIB MODULE INFORMATION

	MODULE NAME: lg110c
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 21
	ARGUMENTS:
		hi_pin,	int,	Input,	,	,	
		lo_pin,	int,	Input,	,	,	
		jun_pin,	int,	Input,	,	,	
		extra_pins,	char *,	Input,	 ,	 ,	 
		jmax,	float,	Input,	 ,	 ,	 
		vopmax,	float,	Input,	 ,	 ,	 
		ehigh,	float,	Input,	 ,	 ,	 
		vjun,	float,	Input,	 ,	 ,	 
		total_step_time,	float,	Input,	 ,	 ,	 
		total_time,	float,	Input,	 ,	 ,	 
		area,	float,	Input,	 ,	 ,	 
		tox,	float,	Input,	 ,	 ,	 
		c_extra,	float,	Input,	 ,	 ,	 
		pin_array1,	I_ARRAY_T,	Input,	 ,	 ,	 
		pin_array1_size,	int,	Input,	 ,	 ,	 
		pin_array2,	I_ARRAY_T,	Input,	 ,	 ,	 
		pin_array2_size,	int,	Input,	 ,	 ,	 
		meas_mode,	char,	Input,	 ,	 ,	 
		TID,	int *,	Output,	 ,	 ,	 
		param,	F_ARRAY_T,	Output,	 ,	 ,	 
		param_size,	int,	Input,	 ,	 ,	 
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void lg110c(int hi_pin, int lo_pin, int jun_pin, char * extra_pins, float jmax, float vopmax, float ehigh, float vjun, float total_step_time, float total_time, float area, float tox, float c_extra, int *pin_array1, int pin_array1_size, int *pin_array2, int pin_array2_size, char meas_mode, int * TID, float *param, int param_size);


/* USRLIB MODULE INFORMATION

	MODULE NAME: lg120c
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 22
	ARGUMENTS:
		hi_pin,	int,	Input,	,	,	
		lo_pin,	int,	Input,	,	,	
		jun_pin,	int,	Input,	,	,	
		extra_pins,	char *,	Input,	 ,	 ,	 
		select,	int,	Input,	 ,	 ,	 
		jmax,	float,	Input,	 ,	 ,	 
		vopmax,	float,	Input,	 ,	 ,	 
		ehigh,	float,	Input,	 ,	 ,	 
		vjun,	float,	Input,	 ,	 ,	 
		total_step_time,	float,	Input,	 ,	 ,	 
		total_time,	float,	Input,	 ,	 ,	 
		area,	float,	Input,	 ,	 ,	 
		tox,	float,	Input,	 ,	 ,	 
		c_extra,	float,	Input,	 ,	 ,	 
		pin_array1,	I_ARRAY_T,	Input,	 ,	 ,	 
		pin_array1_size,	int,	Input,	 ,	 ,	 
		pin_array2,	I_ARRAY_T,	Input,	 ,	 ,	 
		pin_array2_size,	int,	Input,	 ,	 ,	 
		meas_mode,	char,	Input,	 ,	 ,	 
		TID,	int *,	Output,	 ,	 ,	 
		param,	F_ARRAY_T,	Output,	 ,	 ,	 
		param_size,	int,	Input,	 ,	 ,	 
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void lg120c(int hi_pin, int lo_pin, int jun_pin, char * extra_pins, int select, float jmax, float vopmax, float ehigh, float vjun, float total_step_time, float total_time, float area, float tox, float c_extra, int *pin_array1, int pin_array1_size, int *pin_array2, int pin_array2_size, char meas_mode, int * TID, float *param, int param_size);


/* USRLIB MODULE INFORMATION

	MODULE NAME: lg200c
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 18
	ARGUMENTS:
		hi_pin,	int,	Input,	,	,	
		lo_pin,	int,	Input,	,	,	
		extra_pins,	char *,	Input,	 ,	 ,	 
		add_pin,	int,	Input,	 ,	 ,	 
		sign,	int,	Input,	 ,	 ,	 
		ramp_rate,	float,	Input,	 ,	 ,	 
		jbd_lim,	float,	Input,	 ,	 ,	 
		vadd,	float,	Input,	 ,	 ,	 
		area,	float,	Input,	 ,	 ,	 
		tox,	float,	Input,	 ,	 ,	 
		pin_array1,	I_ARRAY_T,	Input,	 ,	 ,	 
		pin_array1_size,	int,	Input,	 ,	 ,	 
		pin_array2,	I_ARRAY_T,	Input,	 ,	 ,	 
		pin_array2_size,	int,	Input,	 ,	 ,	 
		meas_mode,	char,	Input,	 ,	 ,	 
		TID,	int *,	Output,	 ,	 ,	 
		param,	F_ARRAY_T,	Output,	 ,	 ,	 
		param_size,	int,	Input,	 ,	 ,	 
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void lg200c(int hi_pin, int lo_pin, char * extra_pins, int add_pin, int sign, float ramp_rate, float jbd_lim, float vadd, float area, float tox, int *pin_array1, int pin_array1_size, int *pin_array2, int pin_array2_size, char meas_mode, int * TID, float *param, int param_size);


/* USRLIB MODULE INFORMATION

	MODULE NAME: lg210c
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 21
	ARGUMENTS:
		sour,	int,	Input,	,	,	
		drai,	int,	Input,	,	,	
		gate,	int,	Input,	,	,	
		subs,	int,	Input,	,	,	
		extra_pins,	char *,	Input,	 ,	 ,	 
		area,	float,	Input,	 ,	 ,	 
		tox,	float,	Input,	 ,	 ,	 
		tz_test_field,	float,	Input,	 ,	 ,	 
		step_field,	float,	Input,	 ,	 ,	 
		start_field,	float,	Input,	 ,	 ,	 
		stop_field,	float,	Input,	 ,	 ,	 
		t_stress,	float,	Input,	 ,	 ,	 
		tzi_thresh,	float,	Input,	 ,	 ,	 
		pin_array1,	I_ARRAY_T,	Input,	 ,	 ,	 
		pin_array1_size,	int,	Input,	 ,	 ,	 
		pin_array2,	I_ARRAY_T,	Input,	 ,	 ,	 
		pin_array2_size,	int,	Input,	 ,	 ,	 
		meas_mode,	char,	Input,	 ,	 ,	 
		TID,	int *,	Output,	 ,	 ,	 
		param,	F_ARRAY_T,	Output,	 ,	 ,	 
		param_size,	int,	Input,	 ,	 ,	 
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void lg210c(int sour, int drai, int gate, int subs, char * extra_pins, float area, float tox, float tz_test_field, float step_field, float start_field, float stop_field, float t_stress, float tzi_thresh, int *pin_array1, int pin_array1_size, int *pin_array2, int pin_array2_size, char meas_mode, int * TID, float *param, int param_size);


/* USRLIB MODULE INFORMATION

	MODULE NAME: lh100c
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 18
	ARGUMENTS:
		hi_pin,	int,	Input,	,	,	
		lo_pin,	int,	Input,	,	,	
		gnd_pin,	int,	Input,	,	,	
		extra_pins,	char *,	Input,	 ,	 ,	 
		v_bias,	float,	Input,	 ,	 ,	 
		area,	float,	Input,	 ,	 ,	 
		eps_r,	float,	Input,	 ,	 ,	 
		cap_para,	float,	Input,	 ,	 ,	 
		max_dev,	float,	Input,	 ,	 ,	 
		min_cap,	float,	Input,	 ,	 ,	 
		pin_array1,	I_ARRAY_T,	Input,	 ,	 ,	 
		pin_array1_size,	int,	Input,	 ,	 ,	 
		pin_array2,	I_ARRAY_T,	Input,	 ,	 ,	 
		pin_array2_size,	int,	Input,	 ,	 ,	 
		meas_mode,	char,	Input,	 ,	 ,	 
		TID,	int *,	Output,	 ,	 ,	 
		param,	F_ARRAY_T,	Output,	 ,	 ,	 
		param_size,	int,	Input,	 ,	 ,	 
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void lh100c(int hi_pin, int lo_pin, int gnd_pin, char * extra_pins, float v_bias, float area, float eps_r, float cap_para, float max_dev, float min_cap, int *pin_array1, int pin_array1_size, int *pin_array2, int pin_array2_size, char meas_mode, int * TID, float *param, int param_size);


/* USRLIB MODULE INFORMATION

	MODULE NAME: lh200c
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 19
	ARGUMENTS:
		hi_pin,	int,	Input,	,	,	
		lo_pin,	int,	Input,	,	,	
		gnd_pin,	int,	Input,	,	,	
		extra_pins,	char *,	Input,	 ,	 ,	 
		v_max,	float,	Input,	 ,	 ,	 
		dimension,	float,	Input,	 ,	 ,	 
		v_dif,	float,	Input,	 ,	 ,	 
		cap_para,	float,	Input,	 ,	 ,	 
		max_dev,	float,	Input,	 ,	 ,	 
		min_cap,	float,	Input,	 ,	 ,	 
		area,	float,	Input,	 ,	 ,	 
		pin_array1,	I_ARRAY_T,	Input,	 ,	 ,	 
		pin_array1_size,	int,	Input,	 ,	 ,	 
		pin_array2,	I_ARRAY_T,	Input,	 ,	 ,	 
		pin_array2_size,	int,	Input,	 ,	 ,	 
		meas_mode,	char,	Input,	 ,	 ,	 
		TID,	int *,	Output,	 ,	 ,	 
		param,	F_ARRAY_T,	Output,	 ,	 ,	 
		param_size,	int,	Input,	 ,	 ,	 
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void lh200c(int hi_pin, int lo_pin, int gnd_pin, char * extra_pins, float v_max, float dimension, float v_dif, float cap_para, float max_dev, float min_cap, float area, int *pin_array1, int pin_array1_size, int *pin_array2, int pin_array2_size, char meas_mode, int * TID, float *param, int param_size);


/* USRLIB MODULE INFORMATION

	MODULE NAME: lh210c
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 23
	ARGUMENTS:
		hi_pin,	int,	Input,	,	,	
		lo_pin,	int,	Input,	,	,	
		gnd_pin,	int,	Input,	,	,	
		extra_pins,	char *,	Input,	 ,	 ,	 
		v_max,	float,	Input,	 ,	 ,	 
		dimension,	int,	Input,	 ,	 ,	 
		v_dif,	float,	Input,	 ,	 ,	 
		cap_para,	float,	Input,	 ,	 ,	 
		max_dev,	float,	Input,	 ,	 ,	 
		min_cap,	float,	Input,	 ,	 ,	 
		area,	float,	Input,	 ,	 ,	 
		cs_bot_array,	F_ARRAY_T,	Input,	 ,	 ,	 
		cs_bot_size,	int,	Input,	 ,	 ,	 
		dw_od,	float,	Input,	 ,	 ,	 
		loc_edge,	float,	Input,	 ,	 ,	 
		pin_array1,	I_ARRAY_T,	Input,	 ,	 ,	 
		pin_array1_size,	int,	Input,	 ,	 ,	 
		pin_array2,	I_ARRAY_T,	Input,	 ,	 ,	 
		pin_array2_size,	int,	Input,	 ,	 ,	 
		meas_mode,	char,	Input,	 ,	 ,	 
		TID,	int *,	Output,	 ,	 ,	 
		param,	F_ARRAY_T,	Output,	 ,	 ,	 
		param_size,	int,	Input,	 ,	 ,	 
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void lh210c(int hi_pin, int lo_pin, int gnd_pin, char * extra_pins, float v_max, int dimension, float v_dif, float cap_para, float max_dev, float min_cap, float area, float *cs_bot_array, int cs_bot_size, float dw_od, float loc_edge, int *pin_array1, int pin_array1_size, int *pin_array2, int pin_array2_size, char meas_mode, int * TID, float *param, int param_size);


/* USRLIB MODULE INFORMATION

	MODULE NAME: lh220c
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 27
	ARGUMENTS:
		hi_pin,	int,	Input,	,	,	
		lo_pin,	int,	Input,	,	,	
		gnd_pin,	int,	Input,	,	,	
		extra_pins,	char *,	Input,	 ,	 ,	 
		v_max,	float,	Input,	 ,	 ,	 
		dimension,	int,	Input,	 ,	 ,	 
		v_dif,	float,	Input,	 ,	 ,	 
		cap_para,	float,	Input,	 ,	 ,	 
		max_dev,	float,	Input,	 ,	 ,	 
		min_cap,	float,	Input,	 ,	 ,	 
		area,	float,	Input,	 ,	 ,	 
		cs_bot_array,	F_ARRAY_T,	Input,	 ,	 ,	 
		cs_bot_size,	int,	Input,	 ,	 ,	 
		cs_loc_array,	F_ARRAY_T,	Input,	 ,	 ,	 
		cs_loc_size,	int,	Input,	 ,	 ,	 
		dw_od,	float,	Input,	 ,	 ,	 
		loc_edge,	float,	Input,	 ,	 ,	 
		dw_ps,	float,	Input,	 ,	 ,	 
		gat_edge,	float,	Input,	 ,	 ,	 
		pin_array1,	I_ARRAY_T,	Input,	 ,	 ,	 
		pin_array1_size,	int,	Input,	 ,	 ,	 
		pin_array2,	I_ARRAY_T,	Input,	 ,	 ,	 
		pin_array2_size,	int,	Input,	 ,	 ,	 
		meas_mode,	char,	Input,	 ,	 ,	 
		TID,	int *,	Output,	 ,	 ,	 
		param,	F_ARRAY_T,	Output,	 ,	 ,	 
		param_size,	int,	Input,	 ,	 ,	 
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void lh220c(int hi_pin, int lo_pin, int gnd_pin, char * extra_pins, float v_max, int dimension, float v_dif, float cap_para, float max_dev, float min_cap, float area, float *cs_bot_array, int cs_bot_size, float *cs_loc_array, int cs_loc_size, float dw_od, float loc_edge, float dw_ps, float gat_edge, int *pin_array1, int pin_array1_size, int *pin_array2, int pin_array2_size, char meas_mode, int * TID, float *param, int param_size);


/* USRLIB MODULE INFORMATION

	MODULE NAME: lh300c
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 20
	ARGUMENTS:
		pinc1,	int,	Input,	,	,	
		pinc2,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		extra_pins,	char *,	Input,	 ,	 ,	 
		sign,	int,	Input,	 ,	 ,	 
		i_force,	float,	Input,	 ,	 ,	 
		vds_max,	float,	Input,	 ,	 ,	 
		v_bias,	float,	Input,	 ,	 ,	 
		v_step,	float,	Input,	 ,	 ,	 
		cref,	float,	Input,	 ,	 ,	 
		cref_pos,	int,	Input,	 ,	 ,	 
		pin_array1,	I_ARRAY_T,	Input,	 ,	 ,	 
		pin_array1_size,	int,	Input,	 ,	 ,	 
		pin_array2,	I_ARRAY_T,	Input,	 ,	 ,	 
		pin_array2_size,	int,	Input,	 ,	 ,	 
		meas_mode,	char,	Input,	 ,	 ,	 
		TID,	int *,	Output,	 ,	 ,	 
		param,	F_ARRAY_T,	Output,	 ,	 ,	 
		param_size,	int,	Input,	 ,	 ,	 
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void lh300c(int pinc1, int pinc2, int source, int drain, char * extra_pins, int sign, float i_force, float vds_max, float v_bias, float v_step, float cref, int cref_pos, int *pin_array1, int pin_array1_size, int *pin_array2, int pin_array2_size, char meas_mode, int * TID, float *param, int param_size);


/* USRLIB MODULE INFORMATION

	MODULE NAME: lh310c
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 18
	ARGUMENTS:
		pinc1,	int,	Input,	,	,	
		pinc2,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		extra_pins,	char *,	Input,	 ,	 ,	 
		sign,	int,	Input,	 ,	 ,	 
		i_force,	float,	Input,	 ,	 ,	 
		vds_max,	float,	Input,	 ,	 ,	 
		v_bias,	float,	Input,	 ,	 ,	 
		v_step,	float,	Input,	 ,	 ,	 
		pin_array1,	I_ARRAY_T,	Input,	 ,	 ,	 
		pin_array1_size,	int,	Input,	 ,	 ,	 
		pin_array2,	I_ARRAY_T,	Input,	 ,	 ,	 
		pin_array2_size,	int,	Input,	 ,	 ,	 
		meas_mode,	char,	Input,	 ,	 ,	 
		TID,	int *,	Output,	 ,	 ,	 
		param,	F_ARRAY_T,	Output,	 ,	 ,	 
		param_size,	int,	Input,	 ,	 ,	 
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void lh310c(int pinc1, int pinc2, int source, int drain, char * extra_pins, int sign, float i_force, float vds_max, float v_bias, float v_step, int *pin_array1, int pin_array1_size, int *pin_array2, int pin_array2_size, char meas_mode, int * TID, float *param, int param_size);


/* USRLIB MODULE INFORMATION

	MODULE NAME: lh320c
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 19
	ARGUMENTS:
		pinc1,	int,	Input,	,	,	
		pinc2,	int,	Input,	,	,	
		source,	int,	Input,	,	,	
		drain,	int,	Input,	,	,	
		extra_pins,	char *,	Input,	 ,	 ,	 
		sign,	int,	Input,	 ,	 ,	 
		i_force,	float,	Input,	 ,	 ,	 
		vds_max,	float,	Input,	 ,	 ,	 
		v_bias,	float,	Input,	 ,	 ,	 
		v_step,	float,	Input,	 ,	 ,	 
		niter,	int,	Input,	 ,	 ,	 
		pin_array1,	I_ARRAY_T,	Input,	 ,	 ,	 
		pin_array1_size,	int,	Input,	 ,	 ,	 
		pin_array2,	I_ARRAY_T,	Input,	 ,	 ,	 
		pin_array2_size,	int,	Input,	 ,	 ,	 
		meas_mode,	char,	Input,	 ,	 ,	 
		TID,	int *,	Output,	 ,	 ,	 
		param,	F_ARRAY_T,	Output,	 ,	 ,	 
		param_size,	int,	Input,	 ,	 ,	 
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void lh320c(int pinc1, int pinc2, int source, int drain, char * extra_pins, int sign, float i_force, float vds_max, float v_bias, float v_step, int niter, int *pin_array1, int pin_array1_size, int *pin_array2, int pin_array2_size, char meas_mode, int * TID, float *param, int param_size);


/* USRLIB MODULE INFORMATION

	MODULE NAME: li110c
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 18
	ARGUMENTS:
		beta,	F_ARRAY_T,	Input,	,	,	
		beta_len,	int,	Input,	,	,	
		w,	F_ARRAY_T,	Input,	,	,	
		w_len,	int,	Input,	,	,	
		l,	F_ARRAY_T,	Input,	,	,	
		l_len,	int,	Input,	,	,	
		dim,	int,	Input,	,	,	
		betasq,	float,	Input,	,	,	
		deltaw,	float,	Input,	,	,	
		deltal,	float,	Input,	,	,	
		minlim,	float,	Input,	,	,	
		maxlim,	float,	Input,	,	,	
		wmin,	float,	Input,	,	,	
		lmin,	float,	Input,	,	,	
		meas_mode,	char,	Input,	,	,	
		TID,	int *,	Output,	,	,	
		param,	F_ARRAY_T,	Output,	,	,	
		param_size,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void li110c(float *beta, int beta_len, float *w, int w_len, float *l, int l_len, int dim, float betasq, float deltaw, float deltal, float minlim, float maxlim, float wmin, float lmin, char meas_mode, int * TID, float *param, int param_size);


/* USRLIB MODULE INFORMATION

	MODULE NAME: li200c
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 18
	ARGUMENTS:
		inpar,	F_ARRAY_T,	Input,	,	,	
		inpar_len,	int,	Input,	,	,	
		w,	F_ARRAY_T,	Input,	,	,	
		w_len,	int,	Input,	,	,	
		l,	F_ARRAY_T,	Input,	,	,	
		l_len,	int,	Input,	,	,	
		dim,	int,	Input,	,	,	
		geo,	F_ARRAY_T,	Input,	,	,	
		geo_len,	int,	Input,	,	,	
		minlim,	float,	Input,	,	,	
		maxlim,	float,	Input,	,	,	
		wmin,	float,	Input,	,	,	
		lmin,	float,	Input,	,	,	
		func,	int,	Input,	,	,	
		meas_mode,	char,	Input,	,	,	
		TID,	int *,	Output,	,	,	
		param,	F_ARRAY_T,	Output,	,	,	
		param_size,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void li200c(float *inpar, int inpar_len, float *w, int w_len, float *l, int l_len, int dim, float *geo, int geo_len, float minlim, float maxlim, float wmin, float lmin, int func, char meas_mode, int * TID, float *param, int param_size);


/* USRLIB MODULE INFORMATION

	MODULE NAME: li205c
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 18
	ARGUMENTS:
		inpar,	F_ARRAY_T,	Input,	,	,	
		inpar_len,	int,	Input,	,	,	
		w,	F_ARRAY_T,	Input,	,	,	
		w_len,	int,	Input,	,	,	
		l,	F_ARRAY_T,	Input,	,	,	
		l_len,	int,	Input,	,	,	
		dim,	int,	Input,	,	,	
		geo,	F_ARRAY_T,	Input,	,	,	
		geo_len,	int,	Input,	,	,	
		minlim,	float,	Input,	,	,	
		maxlim,	float,	Input,	,	,	
		wmin,	float,	Input,	,	,	
		lmin,	float,	Input,	,	,	
		func,	int,	Input,	,	,	
		meas_mode,	char,	Input,	,	,	
		TID,	int *,	Output,	,	,	
		param,	F_ARRAY_T,	Output,	,	,	
		param_size,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void li205c(float *inpar, int inpar_len, float *w, int w_len, float *l, int l_len, int dim, float *geo, int geo_len, float minlim, float maxlim, float wmin, float lmin, int func, char meas_mode, int * TID, float *param, int param_size);


/* USRLIB MODULE INFORMATION

	MODULE NAME: li210c
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 21
	ARGUMENTS:
		inpar,	F_ARRAY_T,	Input,	,	,	
		inpar_len,	int,	Input,	,	,	
		w,	F_ARRAY_T,	Input,	,	,	
		w_len,	int,	Input,	,	,	
		l,	F_ARRAY_T,	Input,	,	,	
		l_len,	int,	Input,	,	,	
		dim,	int,	Input,	,	,	
		pn,	float,	Input,	,	,	
		dp,	float,	Input,	,	,	
		gp,	float,	Input,	,	,	
		geo,	F_ARRAY_T,	Input,	,	,	
		geo_len,	int,	Input,	,	,	
		minlim,	float,	Input,	,	,	
		maxlim,	float,	Input,	,	,	
		wmin,	float,	Input,	,	,	
		lmin,	float,	Input,	,	,	
		func,	int,	Input,	,	,	
		meas_mode,	char,	Input,	,	,	
		TID,	int *,	Output,	,	,	
		param,	F_ARRAY_T,	Output,	,	,	
		param_size,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void li210c(float *inpar, int inpar_len, float *w, int w_len, float *l, int l_len, int dim, float pn, float dp, float gp, float *geo, int geo_len, float minlim, float maxlim, float wmin, float lmin, int func, char meas_mode, int * TID, float *param, int param_size);


/* USRLIB MODULE INFORMATION

	MODULE NAME: li280c
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 9
	ARGUMENTS:
		mn,	float,	Input,	,	,	
		dmn,	float,	Input,	,	,	
		gmn,	float,	Input,	,	,	
		select,	int,	Input,	,	,	
		temp,	float,	Input,	,	,	
		TID,	int *,	Input,	,	,	
		meas_mode,	char,	Input,	,	,	
		param,	F_ARRAY_T,	Output,	,	,	
		param_size,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void li280c(float mn, float dmn, float gmn, int select, float temp, int * TID, char meas_mode, float *param, int param_size);


/* USRLIB MODULE INFORMATION

	MODULE NAME: li290c
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 5
	ARGUMENTS:
		select,	int,	Input,	,	,	
		meas_mode,	char,	Input,	,	,	
		TID,	int *,	Output,	,	,	
		param,	F_ARRAY_T,	Output,	,	,	
		param_size,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void li290c(int select, char meas_mode, int * TID, float *param, int param_size);


/* USRLIB MODULE INFORMATION

	MODULE NAME: li300c
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 10
	ARGUMENTS:
		linx,	F_ARRAY_T,	Input,	,	,	
		linx_len,	int,	Input,	,	,	
		liny,	F_ARRAY_T,	Input,	,	,	
		liny_len,	int,	Input,	,	,	
		dim,	int,	Input,	,	,	
		func,	int,	Input,	,	,	
		meas_mode,	char,	Input,	,	,	
		TID,	int *,	Output,	,	,	
		param,	F_ARRAY_T,	Output,	,	,	
		param_size,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void li300c(float *linx, int linx_len, float *liny, int liny_len, int dim, int func, char meas_mode, int * TID, float *param, int param_size);


/* USRLIB MODULE INFORMATION

	MODULE NAME: li300c_f
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 10
	ARGUMENTS:
		linx_in,	F_ARRAY_T,	Input,	 ,	 ,	 
		linx_len,	int,	Input,	,	,	
		liny_in,	F_ARRAY_T,	Input,	 ,	 ,	 
		liny_len,	int,	Input,	,	,	
		dim,	int,	Input,	,	,	
		func,	int,	Input,	,	,	
		meas_mode,	char,	Input,	,	,	
		TID,	int *,	Output,	,	,	
		param,	F_ARRAY_T,	Output,	 ,	 ,	 
		param_size,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void  li300c_f(float *, int, float *, int, int, int, char, int *, float *, int);

/* USRLIB MODULE INFORMATION

	MODULE NAME: li500c
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 9
	ARGUMENTS:
		res1,	F_ARRAY_T,	Input,	,	,	
		res1_len,	int,	Input,	,	,	
		res2,	F_ARRAY_T,	Input,	,	,	
		res2_len,	int,	Input,	,	,	
		select,	int,	Input,	,	,	
		meas_mode,	char,	Input,	,	,	
		TID,	int *,	Output,	,	,	
		param,	F_ARRAY_T,	Output,	,	,	
		param_size,	int,	Input,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void li500c(float *res1, int res1_len, float *res2, int res2_len, int select, char meas_mode, int * TID, float *param, int param_size);


/* USRLIB MODULE INFORMATION

	MODULE NAME: lk100c
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 21
	ARGUMENTS:
		e_sour,	int,	Input,	,	,	
		e_gate,	int,	Input,	,	,	
		e_drai,	int,	Input,	,	,	
		e_subs,	int,	Input,	,	,	
		s_gate,	int,	Input,	,	,	
		extra_pins,	char *,	Input,	 ,	 ,	 
		pulse_pin,	char,	Input,	 ,	 ,	 
		v_e_sour,	float,	Input,	 ,	 ,	 
		v_e_gate,	float,	Input,	 ,	 ,	 
		v_e_drai,	float,	Input,	 ,	 ,	 
		v_e_subs,	float,	Input,	 ,	 ,	 
		v_s_gate,	float,	Input,	 ,	 ,	 
		t_pulse,	float,	Input,	 ,	 ,	 
		pin_array1,	I_ARRAY_T,	Input,	 ,	 ,	 
		pin_array1_size,	int,	Input,	 ,	 ,	 
		pin_array2,	I_ARRAY_T,	Input,	 ,	 ,	 
		pin_array2_size,	int,	Input,	 ,	 ,	 
		meas_mode,	char,	Input,	 ,	 ,	 
		TID,	int *,	Output,	 ,	 ,	 
		param,	F_ARRAY_T,	Output,	 ,	 ,	 
		param_size,	int,	Input,	 ,	 ,	 
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void lk100c(int e_sour, int e_gate, int e_drai, int e_subs, int s_gate, char * extra_pins, char pulse_pin, float v_e_sour, float v_e_gate, float v_e_drai, float v_e_subs, float v_s_gate, float t_pulse, int *pin_array1, int pin_array1_size, int *pin_array2, int pin_array2_size, char meas_mode, int * TID, float *param, int param_size);


/* USRLIB MODULE INFORMATION

	MODULE NAME: lk110c
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 22
	ARGUMENTS:
		e_sour,	int,	Input,	,	,	
		e_gate,	int,	Input,	,	,	
		e_drai,	int,	Input,	,	,	
		e_subs,	int,	Input,	,	,	
		s_gate,	int,	Input,	,	,	
		extra_pins,	char *,	Input,	 ,	 ,	 
		pulse_pin,	char,	Input,	 ,	 ,	 
		v_e_sour,	float,	Input,	 ,	 ,	 
		v_e_gate,	float,	Input,	 ,	 ,	 
		v_e_drai,	float,	Input,	 ,	 ,	 
		v_e_subs,	float,	Input,	 ,	 ,	 
		v_s_gate,	float,	Input,	 ,	 ,	 
		t_pulse,	float,	Input,	 ,	 ,	 
		Icompl,	float,	Input,	 ,	 ,	 
		pin_array1,	I_ARRAY_T,	Input,	 ,	 ,	 
		pin_array1_size,	int,	Input,	 ,	 ,	 
		pin_array2,	I_ARRAY_T,	Input,	 ,	 ,	 
		pin_array2_size,	int,	Input,	 ,	 ,	 
		meas_mode,	char,	Input,	 ,	 ,	 
		TID,	int *,	Output,	 ,	 ,	 
		param,	F_ARRAY_T,	Output,	 ,	 ,	 
		param_size,	int,	Input,	 ,	 ,	 
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void lk110c(int e_sour, int e_gate, int e_drai, int e_subs, int s_gate, char * extra_pins, char pulse_pin, float v_e_sour, float v_e_gate, float v_e_drai, float v_e_subs, float v_s_gate, float t_pulse, float Icompl, int *pin_array1, int pin_array1_size, int *pin_array2, int pin_array2_size, char meas_mode, int * TID, float *param, int param_size);


/* USRLIB MODULE INFORMATION

	MODULE NAME: test
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
void  test();

