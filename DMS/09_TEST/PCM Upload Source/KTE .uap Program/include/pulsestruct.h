/*
 *	This is the PULSE internal header file PULSESTRUCT.HDR
 */

#define MAXIMUM_UNITS 3
#define MAXIMUM_CHANNELS MAXIMUM_UNITS * 2 /* number of units times 2 */


typedef struct _pulse_struct {
    int	     ieee_address;
    int      hp_chan_num;
    double   rise_time;       
    double   fall_time;
    double   width;
    double   delay;
    double   period;
    int      burst_cnt;
    double   height;
    double   offset ;
    double   amplitude;
    int	     timeout;
    int	     initialized;
    int      hw_triggered;
    int      mode ;
    int      unit_type ;
    int	     output_relay_delay ;
    char     init_file_name [256] ;

} pulse_struct;

#define ZLoad			50.0
#define MinTransition	4.0E-09
#define MaxTransition	25.0E-03
#define MinDelay		0.0
#define MaxDelay		999.0E-03
#define MinWidth		1.0E-08
#define MaxWidth		999.0E-03
#define VOutMin			-20.0
#define VOutMax			20.0
#define VXOver			10.0
#define MaxCount		9999
#define MinCount		1
#define Continuous		1
#define GPIB_Trig		2
#define Extern_Trig		3
#define Burst			4

/* Return codes */
#define PULSE_OK 1
#define NOT_INITIALIZED -1
#define INVALID_PARAMETER -2
#define NO_KTH_INI -3
#define ERROR_READING_KTHINI -4
#define NO_GPIB_ADDRESS -5
#define OPEN_FAIL_KTHINI -6
#define INVALID_CHANNEL -7
#define NO_UNIT_TYPE -8
#define NO_INIT_FILE -9
#define OPERATION_NOT_COMPLETE -10
#define GPIB_COMM_ERROR -11
#define NO_TIMEOUT -12
#define NO_HW_TRG -13
#define ERROR_READING_INIT_FILE -14
#define METER_ERROR -15
#define NO_OUTPUT_ENABLE_DELAY -16
#define PULSE_SINGLE 1
#define PULSE_DOUBLE 2
#ifndef S400
    #define KI_ENABLED 1.0
    #define KI_DISABLED 2.0
#else
    #define KI_ENABLED 2.0
    #define KI_DISABLED 1.0
#endif

/*
 *   DEFINE CONSTANTS FOR THE PCU TRIGGERS AND CHANNELS
 */

#define PCH1 64
#define PCH2 128
#define PCH3 256
#define PCH4 512
#define PCH5 1024
#define PCH6 2048
#define PTRIG1 4096
#define PTRIG2 8192
#define PTRIG3 16384
#define PTRIG4 32768

extern int		MAX_CHANNELS;		/* Maximum Number Channels */
