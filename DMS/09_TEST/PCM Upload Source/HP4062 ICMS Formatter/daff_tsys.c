/*****************************************************************************/
/* FILE: daff_tsys.c
/* 
/* DESCRIPTION:
/* 	Formatter (Data Access Format Functions) for DP1/DM5 Test 
/*	Systems (TSYS) data collection.  Communicates to external 
/*	TSYS task through named pipes.
/*
/* ACKNOWLEDGEMENT:
/*	Original code based on daff_dbms.c by Hewlett-Packard Co.
/*
/* HISTORY:
/*	12/15/94	Original code.				Glenn Schuette
/*	01/14/95	Broke up data string to prevent
/*			overflow of Tsys input pipe.		GES
/*	03/28/95	Added better error checking in 
/*	        	eod_tsys function.  Increased # of 
/*	        	parms sent at one time to decrease 
/*	        	test time.      			GES
/*	05/10/95	Added popup dialog boxes when an
/*	        	error occurs in the formatter.		GES
/*	05/23/95	Strip down program string to allow
/*	        	for multiple tests to be stored under
/*	        	one program in database.  Also added 
/*	        	error checking on curr_die() and data 
/*	        	description.				GES
/*	08/07/95	Minor change to error message.		GES
/*	09/15/95	Removed data value limit of 32767.	GES
/*	06/20/97	Changed SITE= to UNIT= for TestWare.    Ed Russell
/*
/*****************************************************************************/
 
/* SCCS identifier string - This should not be changed except by SCCS */
static char sccs_id[] = "@(#)daff_tsys.c	1.7	09/15/95";

#include <stdio.h>
#include <stdlib.h>
#include <macros.h>
#include "accesslib.h"                                 /* access to ICMS dar */

#ifdef XPOPUP
extern void pop_error_box();     /* X-Window popup error boxes are supported */
#endif

/*        --- GLOBAL DECLARATION ---                                         */
char *tin  = "/tmp/tester_in";         /* named pipes for TSYS communication */
char *tout = "/tmp/tester_out";
char Lot_Name[21]; /* 2005_11_09*/
char Die_Xsize[21]; /* 2005_11_09 ju*/
char Die_Ysize[21]; /* 2005_11_09 ju*/
FILE *input_pipe, *output_pipe;

int ack_required = 1;                       /* Acknowledge needed from Tsys? */
#define PARM_STRLEN 1024       /* Maximum length of parm string sent to TSYS */
#define PARM_LEN    20              /* Maximum length of "parm=value" string */
#define TSYS_ACK  0x6                     /* Acknowledge byte from Tsys task */

#define TEST_PROG_LEN 8                 /* Length of valid Tsys test program */
#define LOT_NUM_LEN 7                          /* Length of valid lot number */
#define WAFER_ID_LEN 7                        /* Minimum length of wafer ID */

static int formatter_abort = 0;	         /* Flag to abort formatter on error */
static int first_die;     /* Flag set if we're on the first die of the wafer */

 
/*****************************************************************************/
/* If X-Window library support is not available, the popup windows are 
/* replaced with print strings to stderr.
/*****************************************************************************/
#ifndef XPOPUP
void pop_error_box(s)
char *s;
{
	fprintf(stderr, "%s\n", s);
}
#endif

 
int bot_tsys()                                         /* Beginning of Test  */
/*****************************************************************************/
/* At Beginning of Test Function                                             */
/*                                                                           */
/* This function is invoked only at the beginning of each test.              */
/*****************************************************************************/
{                                                      /* BEGIN FUNCTION     */
   formatter_abort = 0;
   strncpy(Lot_Name, get_lot_id(), 20); /*2005_11_09*/
   strncpy(Die_Xsize, curr_die_Xsize(), 20); /*2005_11_09*/
   strncpy(Die_Ysize, curr_die_Ysize(), 20); /*2005_11_09*/
   printf("BOW Lot_Name == %s\n", Lot_Name);
   printf("BOT Die_Xsize == %s\n", Die_Xsize);
   printf("BOT Die_Ysize == %s\n", Die_Ysize);
   return(0);                                          /* normal exit        */
}                                                      /* END eot_tsys       */

 
int bow_tsys()                                         /* Begin of Wafer     */
/*****************************************************************************/
/* At Beginning of New Wafer                                                 */
/*                                                                           */
/* This function is invoked at the beginning of testing each wafer.          */
/*****************************************************************************/
{                                                     /* BEGIN FUNCTION      */
   char reply[80];                                    /* reply from Tsys     */
   char out_string[300];
   char test_prog[21];
   char lot_number[21];
   char wafer_id[21];
   int test_prog_len, lot_number_len, wafer_id_len;
   int i,wfstr;
   char c;
   /* Modified By JW SEO of HP-Korea & Hyun-Jeong Lee*/
   int j,k,n;
   char wid[20],ocrmap[50];
   char dir[20];
   char outdir[20];
   char waf_id[21];
   FILE *fp,*out,*curr;
   sprintf(dir,"/tmp/.ocrMap-0");
   sprintf(outdir,"/tmp/.ocrMap-0.tsys");
   fp=fopen(dir,"r");
   out=fopen(outdir,"w");
     for (i=1;i<=9;i++) {
       j=fscanf(fp,"%s",ocrmap);
	 if (i==2){
	   sprintf(wid,ocrmap);
	   }
     }
     if (wid[8]=='-'){
        wfstr=10;
     }else{
        wfstr=12;
     }
     for (k=0;k<=wfstr;k++)
     {
     if (k>0){
	fprintf(out,"%c",wid[k]);
	}
     }
     fclose(out);
     curr=fopen(outdir,"r");
     for (n=0;n<2;n++){
     fscanf(curr,"%s",waf_id);
}
    fclose(fp);
    fclose(curr);
    /* HP-KOREA */
   first_die = 1;                            /* Set first die to true at BOW */

   /**************************************************************************/
   /* Open TSYS input and output named pipes */
   /**************************************************************************/
   if ( access(tin, 0x0) != 0 ) {
	sprintf(out_string, "ERROR: Cannot open TSYS pipe %s\n\n", tin);
	strcat(out_string, "To start TSYS software, enter 'startup'\n");
	strcat(out_string, "at any shell prompt.\n");
	strcat(out_string, "\nClick OK to abort\n");
	pop_error_box(out_string);
/*
	formatter_abort = 1;
	return(-1);
*/
   }
   input_pipe = fopen(tin, "w");

   if ( ack_required ) {
        if ( access(tout, 0x0) != 0 ) {
	sprintf(out_string, "ERROR: Cannot open TSYS pipe %s\n\n",tout);
		strcat(out_string, "To start TSYS software, enter 'startup'\n");
		strcat(out_string, "at any shell prompt.\n");
		strcat(out_string, "\nClick OK to abort\n");
		pop_error_box(out_string);
/*
		formatter_abort = 1;
		return(-1);
*/
        }
	output_pipe = fopen(tout, "r");
   }

   strncpy(test_prog, curr_cassette(), 20);

  /* strncpy(lot_number, get_lot_id(), 20); */ /* This is remarked for Multi Wafer Test 2005_11_09*/
 /* Modefiled by JW SEO */
 /*  strncpy(wafer_id, curr_waferid(), 20); */

   strncpy(wafer_id,waf_id,20); 

   /**************************************************************************/
   /* Strip test program of everything after an underscore.  This way,       */
   /* multiple test programs can be used to collect data under one           */
   /* program in the database.  For example, the following programs          */
   /* will all be stored under MPOTOMAC:                                     */
   /*        MPOTOMAC, MPOTOMAC_REPROBE, MPOTOMAC_GOI                        */
   /**************************************************************************/
   i = 0;
   while( (c = test_prog[i]) != NULL ) {
	if ( c == '_' ) {
		test_prog[i] = NULL; break;
	}
	i++;
   }

   /**************************************************************************/
   /*   Check for a valid slice start message:                               */
   /*           test program length must be = TEST_PROG_LEN                  */
   /*           lot number must be of length = LOT_NUM_LEN                   */
   /*           wafer ID must be at least WAFER_ID_LEN                       */
   /**************************************************************************/

/*   if ( (test_prog_len = strlen(test_prog)) != TEST_PROG_LEN ||
	(lot_number_len = strlen(lot_number)) != LOT_NUM_LEN ||
	(wafer_id_len = strlen(wafer_id)) < WAFER_ID_LEN ) {
		sprintf(out_string, "ERROR: Invalid slice start message\n\nProgram = %s\nLot number = %s\nWafer ID = %s\n", test_prog, lot_number, wafer_id);
		strcat(out_string, "\nClick OK to abort\n");
		pop_error_box(out_string);
		formatter_abort = 1;
		return(-1);
	}
*/

   /**************************************************************************/
   /*  Send the slice start message and wait for reply from Tsys             */
   /**************************************************************************/
  /*  sprintf(out_string, "S PROBER=1 PROG=%s LOT=%s SLID=%s", 
           test_prog, lot_number, wafer_id);
  */

   sprintf(out_string, "S PROBER=1 PROG=%s LOT=%s", 
           test_prog, Lot_Name);

   printf("Slice Send String = %s\n", out_string);

   fprintf(input_pipe, out_string);
   fflush(input_pipe);

   if ( ack_required ) {        
	reply[0] = '\0';
	fscanf( output_pipe, "%s", reply );
	printf("Slice Send Relpy = %s\n", reply[0]);
	if ( reply[0] != TSYS_ACK ) {
		strcat(out_string, "\n      ERROR on slice start\n");
		strcat(out_string, "The slice start message is not valid\n");
		strcat(out_string, "or TSYS is not responding\n");
		strcat(out_string, "\nClick OK to abort\n");
		pop_error_box(out_string);
/*
		formatter_abort = 1;
		return(-1);
*/
	}
   }

   return(0);                                         /* normal exit         */
}                                                     /* END bow_tsys        */


int bop_tsys()                                         /* Begin of Test Pair */
/*****************************************************************************/
/* At Beginning of Test Definition Pair                                      */
/*                                                                           */
/* Not used but needs to be defined.                                         */
/*****************************************************************************/
{                                                      /* BEGIN FUNCTION     */
   return(0);                                          /* normal exit        */
}                                                      /* END bop_tsys       */


int bod_tsys()                                         /* Begin of Die       */
/*****************************************************************************/
/* At Beginning of Die                                                       */
/*                                                                           */
/* This function is invoked at the beginning of testing each die.            */
/*                                                                           */
/* Not used but needs to be defined.                                         */
/*****************************************************************************/
{                                                     /* BEGIN FUNCTION      */
  return(0);                                          /* normal exit         */
}                                                     /* END bod_tsys        */


/*****************************************************************************/
/* Format and send out string of data to Tsys input pipe                     */
/*****************************************************************************/
int send_data_string(parm_string)
char *parm_string;
{
   char data_string[PARM_STRLEN+15];
   char reply[80];
   char out_string[300];
   char die_num[10];

   /**************************************************************************/
   /* Check if curr_die has a form of X,Y.  The die number must be a single  */
   /* digit and not a coordinate.                                            */
   /**************************************************************************/
   sprintf( die_num, "%s", curr_die() );
   if ( strchr( die_num, ',') ) {
	sprintf(out_string, "ERROR: Invalid die number %s\n\n", 
		die_num);
	strcat(out_string, "Die number must be a single digit,\n");
	strcat(out_string, "and not in the form X,Y.\n");
	strcat(out_string, "\nClick OK to abort\n");
	pop_error_box(out_string);
	formatter_abort = 1;
	return(-1);
   }

   /**************************************************************************/
   /* Send the data string to the Tsys input pipe and wait for a reply.      */
   /**************************************************************************/
/* sprintf(data_string, "D SITE=%s %s", die_num, parm_string);  */
   sprintf(data_string, "D PROBER=1 SITE=1 UNIT=%s %s", die_num, parm_string);

   fprintf(input_pipe, data_string);
   fflush(input_pipe);

   if ( ack_required ) {        
	fscanf( output_pipe, "%s", reply );
	if ( reply[0] != TSYS_ACK ) {
		sprintf(out_string, "ERROR sending data on wafer %s and die %s\n\n", curr_waferid(), die_num);
		strcat(out_string, "Check TSYS log file for details\n");
		strcat(out_string, "-> tail $LOGS/Hdcode\n");
		strcat(out_string, "\nClick OK to abort\n");
		pop_error_box(out_string);
/*
		formatter_abort = 1;
		return(-1);
*/
	}
   }
				       
   return(0);

}

/*****************************************************************************/
/* Check the validity of the data description
/*   The description must not exceed eight characters and must not
/*   contain spaces or the following characters: '+', '-', '.'
/*****************************************************************************/
int check_data_desc(i)
int i;
{
	char data_desc[30];
	char out_string[300];
	char module[30], device[30], var[30];
	char buff[90];
	int j;
	int valid_parm = 1;

	/* Scan data description for invalid characters */
	strncpy( data_desc, get_data_desc(i), 29 );

	if ( strlen(data_desc) > 8 ) valid_parm = 0;

	for ( j = 0; j < (strlen(data_desc)) && valid_parm; j++ )
		switch (data_desc[j]) {
			case '+' :
			case '.' :
			case '-' :
			case ' ' : valid_parm = 0; break;
			default  : break;
		}

	/*********************************************************************/
	/* Print out warning message if the data description is not valid.  
	/* Only do this on the first die to warn the user that the
	/* parameter will not be collected.
	/*********************************************************************/
	if ( !valid_parm && first_die ) {
		strcpy(out_string, "WARNING: data description contains an\n");
		strcat(out_string, "invalid character or exceeds the eight\n");
		strcat(out_string, "character limit\n");
		split_result_name(i, module, device, var);
		sprintf(buff, "\nResult name:   %s:%s:%s", module, device, var);
		strcat(out_string, buff );
		strcat(out_string, "\nData description:  ");
		strcat(out_string, data_desc);
		strcat(out_string, "\n\nData will be ignored\n");
		strcat(out_string, "Click OK to continue\n");
		pop_error_box(out_string);
	}
	return(valid_parm);

}

int eod_tsys()                                        /* End of Die          */
/*****************************************************************************/
/*  At End of Die Execution                                                  */
/*                                                                           */
/*  This function is invoked at the end of each die level test.              */
/*                                                                           */
/* Outputs a string to the tsys input pipe with the following format:        */
/*    "D SITE=curr_die parm_1=val_1 parm_2=val_2 ... parm_n=val_n"           */
/*                                                                           */
/* Each test result has a database flag associated with it. If the flag      */
/* is "do not send", we do not output this result.                           */
/*                                                                           */
/* This example makes the assumption that we do not have an array-type       */
/* results.                                                                  */
/*                                                                           */
/*                                                                           */
/*****************************************************************************/
{                                                     /* BEGIN FUNCTION      */
   int result_count,                                  /* number of results   */
       i,                                             /* index               */
       num_parms;
   char data_buffer[30];           /* temp storage for parameter,value pairs */
   char parm_string[PARM_STRLEN]; /* parm string passed to send_data_string()*/
   int  max_num_parms;
   char data_desc[30];
   int  valid_parm;
   int  err = 0;

   result_count = number_of_results();                /* number of parms     */
   max_num_parms = PARM_STRLEN / PARM_LEN;

   strcpy(parm_string, "");                          /* initialize string   */

/* Output result values for each die tested                                  */
   num_parms = 0;
   for (i = 0; i < result_count; i++) {               /* FOR num of parms    */
      if (get_database_flag(i)) {                     /* save to database?   */

	   valid_parm = check_data_desc(i);

	   if (valid_parm) {
           	sprintf(data_buffer, "%s=%s ", get_data_desc(i), 
				get_ascii_data(i));
           	strcat(parm_string, data_buffer);
	   	num_parms++;
	   }

	   /* Break up string to prevent overflow to input pipe */
	   if ( (num_parms >= max_num_parms) && !err ) {
		num_parms = 0;
		err = send_data_string(parm_string);
		strcpy(parm_string, ""); 
	   }
      }
   }                                                  /* ENDIF get_database  */
                                                      /* ENDFOR              */
   /* Send remaining data */
   if ( (num_parms > 0)  && !err ) {
   	err = send_data_string(parm_string);
   }

   if ( first_die ) first_die = 0;      /* We are no longer on the first die */

   return(err);                                       /* normal exit         */
}                                                     /* END eod_tsys        */


int eop_tsys()                                        /* End of Die Pair     */
/*****************************************************************************/
/* At End of Test Definition Pair                                            */
/*                                                                           */
/* This function is invoked each time at the completion of a TD Pair.        */
/*                                                                           */
/* Not used but needs to be defined.                                         */
/*****************************************************************************/
{                                                     /* BEGIN FUNCTION      */
  return(0);                                          /* normal exit         */
}                                                     /* END eop_tsys        */

 
int eow_tsys()                                        /* End of Wafer        */
/*****************************************************************************/
/* At End of Wafer                                                           */
/*                                                                           */
/* This function is invoked each time at the completion of a wafer.          */
/* 1) Send end of wafer message "E" to tsys pipe.
/* 2) Close tsys input and output pipes.  (These pipes could be closed
/*    at EOT, but are closed here to prevent hangups.)
/*****************************************************************************/
{                                                     /* BEGIN FUNCTION      */
   char reply[80];
   char out_string[200];

   /* Normally, ICMS will call EOW even if an error is returned by the BOW 
      function.  To prevent processing EOW, the formatter_abort is set to 1. */
   if (formatter_abort) return(0);  

   fprintf(input_pipe, "E PROBER=1");
   fflush(input_pipe);

   if ( ack_required ) {        
	fscanf( output_pipe, "%s", reply );
	if ( reply[0] != TSYS_ACK ) {
		strcpy(out_string, "ERROR sending slice end message");
		strcat(out_string, "\nClick OK to abort\n");
		pop_error_box(out_string);
/*
		return(-1);
*/
	}
   }
				       
   if ( ack_required ) 
       fclose(input_pipe);

   if ( ack_required ) 
       fclose(output_pipe);

   return(0);                                         /* normal exit         */
}                                                     /* END eow_tsys        */


int eot_tsys()                                        /* End ot Test         */
/*****************************************************************************/
/* At End of Test Function                                                   */
/*                                                                           */
/* This function is invoked at the completion of a test.                     */
/*                                                                           */
/* Not used but needs to be defined.                                         */
/*****************************************************************************/
{                                                    /* BEGIN FUNCTION       */
   return(0);                                        /*                      */
}                                                    /* END eot_tsys         */


/*****************************************************************************/
/* Declare 8 functions for this user format                                  */
/*                                                                           */
/* Enter the names of the eight functions used by each format.               */
/* All eight functions must be entered.  Do not set them to NULL.            */
/*                                                                           */
/* The format_func_struct name must appear in the function array             */
/* "format_defns[]" declared in /usr/icms/src/u_formats.c                    */
/*                                                                           */
/*                                                                           */
/*****************************************************************************/

struct format_func_struct daff_tsys[] = {
     bot_tsys,                                        /* begin of test func  */
     bow_tsys,                                        /* begin of wafer func */
     bop_tsys,                                        /* begin of pair func  */
     bod_tsys,                                        /* begin of die func   */
     eod_tsys,                                        /* end of die func     */
     eop_tsys,                                        /* end of pair func    */
     eow_tsys,                                        /* end of wafer func   */
     eot_tsys                                         /* end of test func    */
};
