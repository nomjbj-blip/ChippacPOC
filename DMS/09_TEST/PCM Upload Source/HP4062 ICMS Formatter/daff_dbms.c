/* @(#) $Revision 1.50 $ IC-MS daff_dbms.c sample daffy"                     */
/* (c) Copyright 1991-1995, Hewlett-Packard Company, all rights reserved.    */

/*****************************************************************************/
/* Creates a flat ASCII file for stuffing into a database                    */
/*                                                                           */
/* The following set of 8 functions access internal IC-MS data during ex-    */
/* ecution of a test plan and outputs the data into a flat ascii file.       */
/* Although this is a rather simple example, the same technique could be     */
/* used for your own unique requirements. The concepts of the 8 IC-MS data   */
/* access points and the IC-MS access library are covered in the IC-MS Users */
/* Guide in detail. Please read the appropriate section before analyzing     */
/* this code.                                                                */
/*                                                                           */
/* The following occur:                                                      */
/*                                                                           */
/* 1) IC-MS execution data is written out to your FORMAT directory to a      */
/*    data file with the same name as the lot id.  THe FORMAT directory      */
/*    is identified in the user's .icmsdefaults file or, if that one is not  */
/*    found, in the systems icmsdefaults file.                               */
/*                                                                           */
/* WARNING: This file will be overwritten for all identical lot names and    */
/*          will not change in mid_cassette, once this formatter starts.     */
/*          It would be safer to generate a unique file name using lot       */
/*          name, tester name and timestamp, for example.                    */
/*                                                                           */
/* 2) The data file is an ascii file with this format:                       */
/*    lot-id|wafer-id|die-label|test-prog|date|parm-cnt|parm-1|...parm-N     */
/*                                                                           */
/* 3) This file is now ready to be stuffed into a database.                  */
/*                                                                           */
/* Note:                                                                     */
/* In order to keep this example simple, we make the assumption that we do   */
/* not have any array-type results.                                          */
/*                                                                           */
/*****************************************************************************/


#include <stdio.h>
#include <stdlib.h>
#include <macros.h>
#include "accesslib.h"                                 /* access to ICMS dar */


/*        --- GLOBAL DECLARATION ---                                         */

FILE *DAFF_FP;                                         /* file pointer       */
char DAFF_LOTID[9];                                    /* lot name           */
char DAFF_DAME[14];                                    /* date and time      */
char DAFF_TESTPROG[33];                                /* test program name  */

 
int bot_dbms()                                         /* Beginning of Test  */
/*****************************************************************************/
/* At Beginning of Test Function                                             */
/*                                                                           */
/* This function is invoked only at the beginning of each test.              */
/*                                                                           */
/* 1) Create (if needed) flat ascii file in HOME directory                   */
/* 2) Get global data                                                        */
/*                                                                           */
/*****************************************************************************/
{                                                      /* BEGIN FUNCTION     */
   char dbfile[1024];                                  /* ASCII file name    */
   int i;

/* We need to output the data into a flat ascii file named after the lotid   */
/* The format_directory_name function returns our FORMAT directory.  The     */
/* data file name is the same as whatever the DAFF_LOTID is.                 */ 
/* We decided if an error occurs to simply output the data to the IC-MS      */
/* STANDARD OUT window. This is an arbitary decision.                        */

   strncpy(DAFF_LOTID,get_lot_id(),8 );                /* get lot name       */
/* if (strlen(DAFF_LOTID) == 8) */
      DAFF_LOTID[8] = '\0';
   for ( i = 0; i < 8; i++)
     if (DAFF_LOTID[i] == ' ')
        DAFF_LOTID[i] = '_'; /* replace blank spaces with underscores */
   strcpy(dbfile,format_directory_name());             /* our FORMATdirectory*/
   strcat(dbfile,"/");                                 /* for path name      */
   strcat(dbfile,DAFF_LOTID);                          /* lot name           */
   if ( (DAFF_FP=fopen(dbfile,"w")) == NULL) {         /* try to open file   */
      DAFF_FP = stdout;                                /* if error use stdout*/
   };                                                  /* ENDIF              */

/* Most databases like the date/time in a specific format for calculations.  */
/* In this case, yymmdd.hhmm is the best choice. For another vendor          */
/* this may be different.                                                    */

   strcpy(DAFF_DAME, present_time_short());            /* get yymmdd.hhmmss  */
   DAFF_DAME[11]='\0';                                 /* remove ss          */

/* Since test program name is constant throughout execution, we elected      */
/* to get the name in the beginning. We could have also just called this     */
/* function each time we wanted the data.                                    */

   strcpy(DAFF_TESTPROG, curr_test());                 /* get testprog name  */

   return(0);                                          /* normal exit        */
}                                                      /* END eot_dbms       */

 
int bow_dbms()                                         /* Begin of Wafer     */
/*****************************************************************************/
/* At Beginning of New Wafer                                                 */
/*                                                                           */
/* This function is invoked at the beginning of testing each wafer.          */
/*                                                                           */
/* Not Used but needs to be defined.                                         */
/*****************************************************************************/
{                                                     /* BEGIN FUNCTION      */

/* We could have called "strcpy(waferid,curr_waferid())" and passed waferid  */
/* as a global, but, in this case, we decided to access it at the end of die */
   
   return(0);                                         /* normal exit         */
}                                                     /* END bow_dbms        */


int bop_dbms()                                         /* Begin of Test Pair */
/*****************************************************************************/
/* At Beginning of Test Definition Pair                                      */
/*                                                                           */
/* Not used but needs to be defined.                                         */
/*****************************************************************************/
{                                                      /* BEGIN FUNCTION     */
   return(0);                                          /* normal exit        */
}                                                      /* END bop_dbms       */


int bod_dbms()                                         /* Begin of Die       */
/*****************************************************************************/
/* At Beginning of Die                                                       */
/*                                                                           */
/* This function is invoked at the beginning of testing each die.            */
/*                                                                           */
/* Not used but needs to be defined.                                         */
/*****************************************************************************/
{                                                     /* BEGIN FUNCTION      */
  return(0);                                          /* normal exit         */
}                                                     /* END bod_dbms        */


int eod_dbms()                                        /* End of Die          */
/*****************************************************************************/
/*  At End of Die Execution                                                  */
/*                                                                           */
/*  This function is invoked at the end of each die level test.              */
/*                                                                           */
/* Outputs a dbms table entry:                                               */
/*    The data file is an ascii file with this format:                       */
/*    lot-id|wafer-id|die-label|test-prog|date|parm-cnt|parm-1|...parm-N     */
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
       i;                                             /* index               */


   result_count = number_of_results();                /* number of parms     */

/* Output constant header before the result values                           */
   fprintf(DAFF_FP,"%s|%s|%s|%s|%s|%d|",              /* 1st half            */
               DAFF_LOTID,                            /* lot id              */
               curr_waferid(),                        /* wafer id            */
               curr_die(),                            /* die label           */
               DAFF_TESTPROG,                         /* test program name   */
               DAFF_DAME,                             /* date/time           */
               result_count);                         /* number of results   */

/* Output result values for each die tested                                  */

   for (i = 0; i < result_count; i++) {               /* FOR num of parms    */
      if (get_database_flag(i))                       /* save to database?   */
	   fprintf(DAFF_FP,"%s|",                          /* output result       */
                   get_ascii_data(i));                /* as ascii data       */
      }                                               /* ENDIF get_database  */
                                                      /* ENDFOR              */

   fprintf(DAFF_FP,"\n");                             /* terminate record    */
   return(0);                                         /* normal exit         */
}                                                     /* END eod_dbms        */


int eop_dbms()                                        /* End of Die Pair     */
/*****************************************************************************/
/* At End of Test Definition Pair                                            */
/*                                                                           */
/* This function is invoked each time at the completion of a TD Pair.        */
/*                                                                           */
/* Not used but needs to be defined.                                         */
/*****************************************************************************/
{                                                     /* BEGIN FUNCTION      */
  return(0);                                          /* normal exit         */
}                                                     /* END eop_dbms        */

 
int eow_dbms()                                        /* End of Wafer        */
/*****************************************************************************/
/* At End of Wafer                                                           */
/*                                                                           */
/* This function is invoked each time at the completion of a wafer.          */
/*                                                                           */
/* Not used but needs to be defined.                                         */
/*****************************************************************************/
{                                                     /* BEGIN FUNCTION      */
   return(0);                                         /* normal exit         */
}                                                     /* END eow_dbms        */


int eot_dbms()                                        /* End ot Test         */
/*****************************************************************************/
/* At End ot Test Function                                                   */
/*                                                                           */
/* This function is invoked when the test has ended.                         */
/*                                                                           */
/* 1) Close file                                                             */
/*                                                                           */
/*                                                                           */
/* We also could have invoked an unix script or program to automatically     */
/* process this lot's worth of data. E.g.,                                   */
/*                                                                           */
/*      #include <sys/types.h>                                               */
/*      #include <sys/stat.h>                                                */
/*      #include <sys/wait.h>                                                */
/*      strcpy(script,"$HOME/bin/db.stuffer ");                              */
/*      strcat(script,dbfile);                                               */
/*      stat =  system(script);                                              */
/*                                                                           */
/* Invokes unix script and passes it the name of lot data file.              */
/*                                                                           */
/*                                                                           */
/*                                                                           */
/*****************************************************************************/
{                                                    /* BEGIN FUNCTION       */
   if (DAFF_FP != stdout)
      fclose(DAFF_FP);                               /* close file           */
   return(0);                                        /*                      */
}                                                    /* END eot_dbms         */


/*****************************************************************************/
/* Declare 8 functions for this user format                                  */
/*                                                                           */
/* Enter the names of the eight functions used by each format.               */
/* All eight functions must be entered.  Do not set them to NULL.            */
/*                                                                           */
/* The format_func_struct name must appear in the function array             */
/* "format_defns[]" declared in /usr/icms/lib/src/u_formats.c                */
/*                                                                           */
/*                                                                           */
/*****************************************************************************/

struct format_func_struct daff_dbms[] = {
     bot_dbms,                                        /* begin of test func  */
     bow_dbms,                                        /* begin of wafer func */
     bop_dbms,                                        /* begin of pair func  */
     bod_dbms,                                        /* begin of die func   */
     eod_dbms,                                        /* end of die func     */
     eop_dbms,                                        /* end of pair func    */
     eow_dbms,                                        /* end of wafer func   */
     eot_dbms                                         /* end of test func    */
};


