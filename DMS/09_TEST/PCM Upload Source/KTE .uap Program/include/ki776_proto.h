/* ki776 function prototype and KITT header file */
/* USRLIB MODULE INFORMATION

	MODULE NAME: atten_776
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		atten_value,	float,	Input,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <ki776.h>
	END USRLIB MODULE INFORMATION
*/
int  atten_776(float atten_value);


/* USRLIB MODULE INFORMATION

	MODULE NAME: ch_ro_freq
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 10
	ARGUMENTS:
		vforce,	double,	Input,	,	,	
		vdd_pin,	int,	Input,	,	,	
		lo_meas_pin,	int,	Input,	,	,	
		hi_meas_pin,	int,	Input,	,	,	
		enable_pin,	int,	Input,	,	,	
		nd24_pin,	int,	Input,	,	,	
		ngate_pin,	int,	Input,	,	,	
		nd12_pin,	int,	Input,	,	,	
		measured_freq,	double *,	Output,	,	,	
		test_status,	int *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <ki776.h>
	END USRLIB MODULE INFORMATION
*/
void  ch_ro_freq(double, int, int, int, int, int, int, int, double *, int *);

/* USRLIB MODULE INFORMATION

	MODULE NAME: ch_ro_freq2
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 10
	ARGUMENTS:
		vforce,	double,	Input,	,	,	
		vdd_pin,	int,	Input,	,	,	
		lo_meas_pin,	int,	Input,	,	,	
		hi_meas_pin,	int,	Input,	,	,	
		enable_pin,	int,	Input,	,	,	
		nd24_pin,	int,	Input,	,	,	
		ngate_pin,	int,	Input,	,	,	
		nd12_pin,	int,	Input,	,	,	
		measured_freq,	double *,	Output,	,	,	
		test_status,	int *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <ki776.h>
	END USRLIB MODULE INFORMATION
*/
void  ch_ro_freq2(double vforce, int vdd_pin, int lo_meas_pin, int hi_meas_pin, int enable_pin, int nd24_pin, int ngate_pin, int nd12_pin, double * measured_freq, int * test_status);


/* USRLIB MODULE INFORMATION

	MODULE NAME: fltoff_776
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <ki776.h>
	END USRLIB MODULE INFORMATION
*/
int  fltoff_776();


/* USRLIB MODULE INFORMATION

	MODULE NAME: flton_776
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <ki776.h>
	END USRLIB MODULE INFORMATION
*/
int  flton_776();


/* USRLIB MODULE INFORMATION

	MODULE NAME: init_776
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <ki776.h>
struct ki776_struct ki776 = {14,0}; 
	END USRLIB MODULE INFORMATION
*/
int  init_776();


/* USRLIB MODULE INFORMATION

	MODULE NAME: initc_776
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <ki776.h>
	END USRLIB MODULE INFORMATION
*/
int  initc_776();


/* USRLIB MODULE INFORMATION

	MODULE NAME: measf_776
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		meas_freq,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <string.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <ki776.h>
	END USRLIB MODULE INFORMATION
*/
int  measf_776(double * meas_freq);


/* USRLIB MODULE INFORMATION

	MODULE NAME: measfc_776
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		meas_freq,	double *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <string.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <ki776.h>
	END USRLIB MODULE INFORMATION
*/
int  measfc_776(double * meas_freq);


/* USRLIB MODULE INFORMATION

	MODULE NAME: nslope_776
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <ki776.h>
	END USRLIB MODULE INFORMATION
*/
int  nslope_776();


/* USRLIB MODULE INFORMATION

	MODULE NAME: pslope_776
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <ki776.h>
	END USRLIB MODULE INFORMATION
*/
int  pslope_776();


/* USRLIB MODULE INFORMATION

	MODULE NAME: ro_freq
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 13
	ARGUMENTS:
		vforce,	double,	Input,	,	,	
		vdd_pin,	int,	Input,	,	,	
		lo_meas_pin,	int,	Input,	,	,	
		hi_meas_pin,	int,	Input,	,	,	
		enable_pin,	int,	Input,	,	,	
		nd24_pin,	int,	Input,	,	,	
		ngate_pin,	int,	Input,	,	,	
		nd12_pin,	int,	Input,	,	,	
		imped_choice,	int,	Input,	,	,	
		channel_choice,	int,	Input,	,	,	
		ohm50_path,	int,	Input,	,	,	
		measured_freq,	double *,	Output,	,	,	
		test_status,	int *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void  ro_freq(double vforce, int vdd_pin, int lo_meas_pin, int hi_meas_pin, int enable_pin, int nd24_pin, int ngate_pin, int nd12_pin, int imped_choice, int channel_choice, int ohm50_path, double * measured_freq, int * test_status);


/* USRLIB MODULE INFORMATION

	MODULE NAME: setac_776
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <ki776.h>
	END USRLIB MODULE INFORMATION
*/
int  setac_776();


/* USRLIB MODULE INFORMATION

	MODULE NAME: setdc_776
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <ki776.h>
	END USRLIB MODULE INFORMATION
*/
int  setdc_776();


/* USRLIB MODULE INFORMATION

	MODULE NAME: setgate_776
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		gate_smpl_time,	double,	Input,	
	INCLUDES:
#include <stdio.h>
#include <string.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <ki776.h>
	END USRLIB MODULE INFORMATION
*/
int  setgate_776(double gate_smpl_time);


/* USRLIB MODULE INFORMATION

	MODULE NAME: setimped_776
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		imped_choice,	int,	Input,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <ki776.h>
	END USRLIB MODULE INFORMATION
*/
int  setimped_776(int imped_choice);


/* USRLIB MODULE INFORMATION

	MODULE NAME: settrig_776
	MODULE RETURN TYPE: int 
	NUMBER OF PARMS: 1
	ARGUMENTS:
		trig_voltage,	double,	Input,	
	INCLUDES:
#include <stdio.h>
#include <string.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <ki776.h>
	END USRLIB MODULE INFORMATION
*/
int  settrig_776(double trig_voltage);


/* USRLIB MODULE INFORMATION

	MODULE NAME: test776_drv
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 7
	ARGUMENTS:
		hi_meas_pin,	int,	Input,	
		lo_meas_pin,	int,	Input,	
		imped_choice,	int,	Input,
		channel_choice,	int,	Input,	
		ohm50_path,	int,	Input,	
		measured_freq,	double *,	Output,	
		test_status,	int *,	Output,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void  test776_drv(int hi_meas_pin, int lo_meas_pin, int imped_choice, int channel_choice, int ohm50_path, double * measured_freq, int * test_status);


/* USRLIB MODULE INFORMATION

	MODULE NAME: test776_drv2
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 7
	ARGUMENTS:
		hi_meas_pin,	int,	Input,	,	,	
		lo_meas_pin,	int,	Input,	,	,	
		imped_choice,	int,	Input,	,	,	
		channel_choice,	int,	Input,	,	,	
		ohm50_path,	int,	Input,	,	,	
		measured_freq,	double *,	Output,	,	,	
		test_status,	int *,	Output,	,	,	
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
	END USRLIB MODULE INFORMATION
*/
void  test776_drv2(int hi_meas_pin, int lo_meas_pin, int imped_choice, int channel_choice, int ohm50_path, double * measured_freq, int * test_status);


