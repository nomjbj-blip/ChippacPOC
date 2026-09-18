/*****************************************************************************/
/* FILE: daff_tsk.c
/* 
/* DESCRIPTION:
/* 	Formatter (Data Access Format Functions) for a simple text
/*	file data collection.  This is a more elegant way of collecting 
/*	the data that is printed on the CRT.
/*
/* ACKNOWLEDGEMENT:
/*	Original code based on daff_dbms.c by Hewlett-Packard Co.
/*
/* HISTORY:
/*	08/05/95	Original code.			Glenn Schuette
/*
/*****************************************************************************/
 
/* SCCS identifier string - This should not be changed except by SCCS */
static char sccs_id[] = "@(#)daff_text.c	1.1	08/05/95";

#include <stdio.h>
#include <stdlib.h>
#include <macros.h>
#include <math.h>
#include "accesslib.h"
/* access to ICMS dar */
#include <time.h> /* Timing Header File */

#ifdef XPOPUP
/*extern void pop_error_box();     /* X-Window popup error boxes are supported */
#endif

/*        --- GLOBAL DECLARATION ---                                         */
FILE *output_tsk;
static int formatter_abort = 0;	         /* Flag to abort formatter on error */
static int first_die=0;
 
/*****************************************************************************/
/* If X-Window library support is not available, the popup windows are 
/* replaced with print strings to stderr.
/*****************************************************************************/
/*
/*#ifndef XPOPUP
/*
void pop_error_box(s)
char *s;
{
	fprintf(stderr, "%s\n", s);
}
#endif
*/
 
int bot_tsk()                                         /* Beginning of Test  */
/*****************************************************************************/
/* At Beginning of Test Function                                             */
/*                                                                           */
/* This function is invoked only at the beginning of each test.              */
/*****************************************************************************/
{                                                      /* BEGIN FUNCTION     */
   char out_string[300];
   char lot_number[9];
   char tfile_tsk[50];  /* Filename for output text file */
   int i,j,k;
   char c,min[3],sec[3],op_name[30];
   time_t tnum;
   struct tm *ts;
   time(&tnum);
   ts=localtime(&tnum);
   sprintf(min,"%d",ts->tm_min);
   sprintf(sec,"%d",ts->tm_sec);

   formatter_abort = 0;
   /**************************************************************************/
   /* Build output filename */
   /**************************************************************************/
   strncpy(lot_number, get_lot_id(), 8);
   lot_number[8] = '\0';
   for ( i = 0; i < 8; i++ )
	if ( lot_number[i] == ' ' )
		lot_number[i] = '_'; /* Replace blank spaces with underscores*/
   strcpy(tfile_tsk, "/home/wafer.rcp");

   /**************************************************************************/
   /* Open output file */
   /**************************************************************************/
   if ( (output_tsk = fopen(tfile_tsk,"w+")) == NULL ) {
	sprintf(out_string, "ERROR: Cannot open output file %s\n\n", tfile_tsk);
	strcat(out_string, "\nClick OK to abort\n");
	pop_error_box(out_string);
	formatter_abort = 1;
	return(-1);
   }

   fprintf(output_tsk, "%s\n", curr_test());
   fprintf(output_tsk, "Executing on: %s\n\n", present_time());
   
   /* Modified by HP-Korea A.E. Ji-Won Seo for Operator NAME */ 
   k=number_of_udcs();
  

   fflush(output_tsk);

   return(0);                                          /* normal exit        */
}                                                      /* END eot_text       */
 


int bow_tsk()                                         /* Begin of Wafer     */
/*****************************************************************************/
/* At Beginning of New Wafer                                                 */
/*                                                                           */
/* This function is invoked at the beginning of testing each wafer.          */
/*****************************************************************************/
{                                                     /* BEGIN FUNCTION      */
first_die = 1;

return(0);                                         /* normal exit         */
}                                                     /* END bow_text        */


int bop_tsk()                                         /* Begin of Test Pair */
/*****************************************************************************/
/* At Beginning of Test Definition Pair                                      */
/*                                                                           */
/* Not used but needs to be defined.                                         */
/*****************************************************************************/
{                                                      /* BEGIN FUNCTION     */
   return(0);                                          /* normal exit        */
}                                                      /* END bop_text       */


int bod_tsk()                                         /* Begin of Die       */
/*****************************************************************************/
/* At Beginning of Die                                                       */
/*                                                                           */
/* This function is invoked at the beginning of testing each die.            */
/*                                                                           */
/*****************************************************************************/
{                                                     /* BEGIN FUNCTION      */

FILE *diesize;
double icms_x;
double icms_y;
double tsk_x;
double tsk_y;
double delta_size_x=0.0;
double delta_size_y=0.0;
char wf_id[256];
char dum1[10];
char dum2[10];

if (first_die == 1) {
   icms_x = curr_die_Xsize(); /*2005_11_09*/
   icms_y = curr_die_Ysize(); /*2005_11_09*/
	 printf("bod i_Xsize == %g\n", icms_x);
	 printf("bod i_Ysize == %g\n", icms_y);

	    diesize=fopen("/home/tmp_die_size","r");
	    fgets(wf_id,256,diesize);
	    sscanf(wf_id,"%s%s",dum1,dum2);

	    tsk_x=atof(dum1);
	    tsk_y=atof(dum2);

	 printf("bod tsk_Xsize == %g\n", tsk_x);
	 printf("bod tsk_Ysize == %g\n", tsk_y);

	delta_size_x = fabs(icms_x - tsk_x);
	delta_size_y = fabs(icms_y - tsk_y);

	 printf("bod delta size x == %g\n", delta_size_x);
	 printf("bod delta size y == %g\n", delta_size_y);

	    if ((delta_size_x > 2) || (delta_size_y > 2)) {

		system("xdialog -fg red -bg yellow -geometry 800 -f '/users/tye/RECIPE/warning' -title Warning");
		system("xdialog -fg red -bg yellow -geometry 800 -f '/users/tye/RECIPE/warning' -title Warning");
	/*	system("/users/tye/PROGRAM/DEL_PRCS"); */

	    }
}

  return(0);                                          /* normal exit         */
}                                                     /* END bod_text        */


int eod_tsk()                                        /* End of Die          */
/*****************************************************************************/
/*  At End of Die Execution                                                  */
/*                                                                           */
/*  This function is invoked at the end of each die level test.              */
/*                                                                           */
/* Outputs a string to the text file with the following format:        */
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
                                                      /* ENDFOR              */
if (first_die) first_die=0;



return(0);                                       /* normal exit         */
}                                                     /* END eod_text        */


int eop_tsk()                                        /* End of Die Pair     */
/*****************************************************************************/
/* At End of Test Definition Pair                                            */
/*                                                                           */
/* This function is invoked each time at the completion of a TD Pair.        */
/*                                                                           */
/* Not used but needs to be defined.                                         */
/*****************************************************************************/
{                                                     /* BEGIN FUNCTION      */
  return(0);                                          /* normal exit         */
}                                                     /* END eop_text        */

 
int eow_tsk()                                        /* End of Wafer        */
/*****************************************************************************/
/* At End of Wafer                                                           */
/*                                                                           */
/* This function is invoked each time at the completion of a wafer.          */
/*                                                                           */
/* Not used but needs to be defined.                                         */
/*****************************************************************************/
{                                                     /* BEGIN FUNCTION      */
   return(0);                                         /* normal exit         */
}                                                     /* END eow_text        */


int eot_tsk()                                        /* End ot Test         */
/*****************************************************************************/
/* At End of Test Function                                                   */
/*                                                                           */
/* This function is invoked at the completion of a test.                     */
/*****************************************************************************/
{                                                    /* BEGIN FUNCTION       */
   if (!formatter_abort)
	fclose(output_tsk);
   return(0);
}                                                    /* END eot_text         */


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

struct format_func_struct daff_tsk[] = {
     bot_tsk,                                        /* begin of test func  */
     bow_tsk,                                        /* begin of wafer func */
     bop_tsk,                                        /* begin of pair func  */
     bod_tsk,                                        /* begin of die func   */
     eod_tsk,                                        /* end of die func     */
     eop_tsk,                                        /* end of pair func    */
     eow_tsk,                                        /* end of wafer func   */
     eot_tsk                                         /* end of test func    */
};
