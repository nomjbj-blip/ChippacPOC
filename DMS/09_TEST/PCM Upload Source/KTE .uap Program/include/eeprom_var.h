/*> PROGRAM VARIABLES */

/*>>variables include file */

/** PROGRAM STRUCTURE VARIABLES  **********************************************/

/*** global program function return status flag for exit program test */
	 int   exit_status;

/*** loop, wafer & site  "do{ ... }while(xxxx_loop)"  continue flags */
	 int lot_loop, wafer_loop, site_loop;

/*** flag corresponding to wafer load action status */
	 int  wfr_load_flag;

/*** bound cassette number for which wafer id's can be entered */
	 int   max_cassette = 4;



/**  COMMAND LINE ARGUMENT VARIABLES  ****************************************/

/*** generic use program debugging flag */
	 int  debug = 0;

/*** program error report mode flag */
	 int  err_report_mode = LOG_ERR_MSGS;

/*** pointer to err msg log file name */
	 char *err_log_fname = "errmsgs.log";

/*** user dialog look and feel selection */
	 #ifdef _MSDOS
	 int  gui_look = DLG_LOOK_MSW;
	 #else
	 int  gui_look = DLG_LOOK_MOTIF;
	 #endif

/*** pointer to lot summary report options */
	 char *sum_report_options = NULL;

/*** character array to hold built up system call to lot summary */
	 char lotsummary_cmd_line[80];

/*** pointer to wafer description file name */
	 char *kwf_fname = NULL;

/*** user specified command line argument */
	 char user_arg[USER_ARG_LENGTH];


/*>> WAFER RELATED VARIABLES */

	 /* further reference "kdf.h" for sites and "kwf_proto.h" for subsites */

	 /*>>> total number of wafers to be tested */
	 int    total_wafers = 0;

	 /*>>> number of wafer being tested */
	 int    wafer_number = 0;

	 /*>>> total number of sites to be tested */
	 int    total_sites = 0;

	 /*>>> number of site being tested */
	 int    site_number  = 0;

	 /*>>> total number of subsites defined */
	 int    no_subsites = 0;

	 /*>>> physical dimensions & orientation */
	 float  diameter;
	 int    units;
	 float  x_die_size;
	 float  y_die_size;
	 int    flat;
	 int    origin;

	 /*>>> reference die column, row coordinates */
	 int    ref_die_column;
	 int    ref_die_row;
	 int    sec_die_column;
	 int    sec_die_row;

	 /*>> Subsite size */
	 float subsitex;
	 float subsitey;

	 /*>>> reference die test flag. 0 - don't test, <>0 - test it  */
	 int  test_ref_die = 1;
	 char wdfcomment[WDF_LINE_LENGTH];


/*>> MISC. */

	 /*>>> scratchpad string for scroll message dialogs, etc. */
	 char temp_str[80];

         /*>>> test number counter for automated parameter ID */
         int test_count;
         char TestID[25]; 
	 time_t time_ptr;
	 char start_time[80];
         char end_time[80];
         

/*> TEST PROGRAM DATA STORAGE INITIALIZATION */

	 /*>>> declare current data structure pointers */
	 LOT     *lot     = NULL;
	 WAFER   *wafer   = NULL;
	 SITE    *site    = NULL;
	 SUBSITE *subsite = NULL;
	 PARAM   *result  = NULL;

	 /*>>> declare link list next structure pointers */
	 WAFER   *next_wafer;
	 SITE    *next_site;
	 SUBSITE *next_subsite;
	 PARAM   *next_result;

