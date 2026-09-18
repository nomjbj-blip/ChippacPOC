/* @(#) $Revision 1.50 $ IC-MS daff_wfmap.c wafermap daffy"                  */
/* (c) Copyright 1991-1995, Hewlett-Packard Company, all rights reserved.    */

/*****************************************************************************/
/* Creates a flat ASCII file for stuffing into a database or for wafermapping*/
/*                                                                           */
/* The following set of 8 functions access internal IC-MS data during ex-    */
/* ecution of a test plan and outputs the data into a flat ascii file.       */
/*                                                                           */
/* The following occur:                                                      */
/*                                                                           */
/* 1) In the WAFERMAP directory, a new directory is created for each new     */
/*    WaferType                                                              */
/*                                                                           */
/* 2) In these WaferType directories, a new directory is created for each    */
/*    new Lot Name.                                                          */
/*                                                                           */
/* 3) IC-MS execution data is written out to the LotName directory to a      */
/*    data file named WaferId.WaferTest.TestStart.  The WAFERMAP directory   */
/*    is identified in the user's .icmsdefaults file or, if that one is not  */
/*    found, in the systems icmsdefaults file.                               */
/*                                                                           */
/* 4) The data file is an ascii file with this format:                       */
/*        - header at beginning of file containing WaferType, LotName,       */
/*              WaferID, WaferTest and Test Start Time                       */
/*        - label line containing the words LotName,WaferID,DieLabel,        */
/*              XYLoc and the names of all tested variables                  */
/*        - data lines (1 per die) containing the measured data for each die */
/*                                                                           */
/* 5) When a new Die Test occurs, a new label line is written before the     */
/*        data lines are written.                                            */
/*                                                                           */
/* 6) This file is now ready to be stuffed into a database, or plotted into  */
/*    a wafer map.                                                           */
/*                                                                           */
/*****************************************************************************/


#include <stdio.h>
#include <stdlib.h>
#include <macros.h>
#include <sys/types.h>   
#include <sys/stat.h>     
#include <unistd.h>                                  /* MLD 120793           */ 
#include <errno.h>                                   /* MLD 120793           */ 
#include <sys/wait.h>
#include "accesslib.h"                               /* access to ICMS data  */


/*        --- GLOBAL DECLARATION ---                                         */

extern int errno;
FILE *WAFMP_FP;                                        /* file pointer       */
char DAFF_WAFERID[19];                                 /* wafer ID           */
char DAFF_LOTID[51];                                   /* lot name-note size */
char DAFF_DATIME[14];                                  /* date and time      */
char DAFF_WAFERTEST[33];                               /* wafer test name    */
char DAFF_WAFERTYPE[33];                               /* wafer type name    */
char DAFF_DIELABEL[33];                                /* die label  name    */
int DAFF_DIEXLOC;                                      /* die X-Y loc        */
int DAFF_DIEYLOC;                                      /* die X-Y loc        */
int DAFF_NUMRESULTS;                                   /* number of results  */

 
int bot_wfmap()                                        /* Beginning of Test  */
/*****************************************************************************/
/* At Beginning of Test Function                                             */
/*                                                                           */
/* This function is invoked only at the beginning of each test.              */
/*                                                                           */
/* Not Used but needs to be defined.                                         */
/*****************************************************************************/
{                                                      /* BEGIN FUNCTION     */
   return(0);                                          /* normal exit        */
}                                                      /* END eot_wfmap      */

 
int bow_wfmap()                                        /* Begin of Wafer     */
/*****************************************************************************/
/* At Beginning of New Wafer                                                 */
/*                                                                           */
/* This function is invoked at the beginning of testing each wafer.          */
/*                                                                           */
/* 1) Get global data                                                        */
/* 2) Create (if needed) directories in WAFERMAP directory and below         */
/* 3) Create file and open for writing                                       */
/*****************************************************************************/
{                                                     /* BEGIN FUNCTION      */

   char wfmapfile[1024];                              /* ASCII file name     */
   int i,err;
   struct stat stat_buf;                              /* check if dir exists */
   mode_t mode, old_umask;                            /* create dir modes    */
   mode = S_IRWXU|S_IRWXG|S_IRWXO;
   old_umask =  umask (S_IWOTH);        /* only prohibit others from writing */

/* We need to output the data into a flat ascii file named after the waferid,*/
/*  the wafer test and the starting test time.
/* The wafermap_directory_name function returns our WAFERMAP directory. The  */
/* directories will be created as needed.                                    */ 
/* We decided if an error occurs to simply output the data to the IC-MS      */
/* STANDARD OUT window. This is an arbitary decision.                        */

   strcpy(DAFF_WAFERTYPE,curr_wafer());                /* get wafer type     */
   strcpy(DAFF_LOTID,get_lot_id());                    /* get lot name       */
   i = 0;
   while ((DAFF_LOTID[i] != '\0') && (i < 51)) {       /* replace blanks with*/
      if (DAFF_LOTID[i] == ' ')                        /*     underscores    */
          DAFF_LOTID[i] = '_';
      i++;
   }
   strcpy(DAFF_WAFERID,curr_waferid());                /* get wafer id       */
   strcpy(DAFF_WAFERTEST,curr_test());                 /* get wafer test     */

/* Most databases like the date/time in a specific format for calculations.  */
/* In this case, yymmdd.hhmm is the best choice. For another vendor          */
/* this may be different.                                                    */

   strcpy(DAFF_DATIME, present_time_short());          /* get yymmdd.hhmmss  */
   DAFF_DATIME[11]='\0';                               /* remove ss          */

   strcpy(wfmapfile,wafermap_directory_name());        /* our WAFERMAP dir   */

   strcat(wfmapfile,"/");                              /* for path name      */
   strcat(wfmapfile,DAFF_WAFERTYPE);                   /* wafer type         */

/* Does DAFF_WAFERTYPE dir exist?  If no, make it.  If yes, be sure it is dir*/

   if (stat(wfmapfile,&stat_buf) == -1)                /* file doesn't exist */
      {
      if (err = mkdir (wfmapfile,mode))                /* make the dir       */
         {
         printf ("ERROR: Could not create directory \n %s.  \n   Error value =  %d.  Check permissions and umask values.\n", wfmapfile, errno);
         }
      }
   else
      if (! (S_ISDIR(stat_buf.st_mode)))               /* file is not a dir */
         {
         printf ("ERROR: In WAFERMAP directory, a file named \n   %s \n    was found.  It should be a directory and is not.  Please move that file \n   so that a directory by that name may be created.\n",wfmapfile);
         return (-1);
         }
   strcat(wfmapfile,"/");                              /* for path name      */
   strcat(wfmapfile,DAFF_LOTID);                       /* lot name           */

/* Does DAFF_LOTID dir exist?  If yes, go on. If no, make it.                */

   if (stat(wfmapfile,&stat_buf) == -1)                /* file doesn't exist */
      {
      if (err = mkdir (wfmapfile,mode))                /* make the dir       */
         {
         printf ("ERROR: Could not create directory \n %s.  \n   Error value =  %d.  Check permissions and umask values.\n", wfmapfile, errno);
         }
      }
   else
      if (! (S_ISDIR(stat_buf.st_mode)))               /* file is not a dir */
         {
         printf ("ERROR: In WAFERMAP/%s directory, a file named \n   %s \n    was found.  It should be a directory and is not.  Please move that file \n   so that a directory by that name may be created.\n",DAFF_WAFERTYPE, wfmapfile);
         return (-1);
         }

   strcat(wfmapfile,"/");                              /* for path name      */
   strcat(wfmapfile,DAFF_WAFERID);                     /* wafer id           */
   strcat(wfmapfile,".");                              /* for file name      */
   strcat(wfmapfile,DAFF_WAFERTEST);                   /* wafer test         */
   strcat(wfmapfile,".");                              /* for file name      */
   strcat(wfmapfile,DAFF_DATIME);                      /* timestamp          */
   (void) umask (old_umask);                           /* restore umask      */
   if ( (WAFMP_FP=fopen(wfmapfile,"w")) == NULL) {     /* try to open file   */
      WAFMP_FP = stdout;                               /* if error use stdout*/
   };                                                  /* ENDIF              */

/* write the header line for identification                                  */
   fprintf (WAFMP_FP,"WafType=%s, LotID=%s, WaferID=%s, WafTest=%s, Time=%s\n",
      DAFF_WAFERTYPE,DAFF_LOTID,DAFF_WAFERID,DAFF_WAFERTEST,DAFF_DATIME);

   fprintf (WAFMP_FP,"\n");                           /* blank line          */
   return(0);                                         /* normal exit         */
}                                                     /* END bow_wfmap       */


int bop_wfmap()                                        /* Begin of Test Pair */
/*****************************************************************************/
/* At Beginning of Test Definition Pair                                      */
/*                                                                           */
/*****************************************************************************/
{                                                      /* BEGIN FUNCTION     */
   int i;

   DAFF_NUMRESULTS = number_of_results();

/* Output label line now                                                     */
   fprintf(WAFMP_FP,"Lot Wafer DieLabel DieXY #Results "); /* 1st half       */
   for (i = 0; i < DAFF_NUMRESULTS; i++) {            /* FOR num of parms    */
	   fprintf(WAFMP_FP,"%s ",                         /* output result name  */
                   get_data_name(i));              
   }
   fprintf (WAFMP_FP,"\n");
   
   return(0);                                          /* normal exit        */
}                                                      /* END bop_wfmap      */


int bod_wfmap()                                        /* Begin of Die       */
/*****************************************************************************/
/* At Beginning of Die                                                       */
/*                                                                           */
/* This function is invoked at the beginning of testing each die.            */
/*                                                                           */
/* Not used but needs to be defined.                                         */
/*****************************************************************************/
{                                                     /* BEGIN FUNCTION      */
  return(0);                                          /* normal exit         */
}                                                     /* END bod_wfmap       */


int eod_wfmap()                                       /* End of Die          */
/*****************************************************************************/
/*  At End of Die Execution                                                  */
/*                                                                           */
/*  This function is invoked at the end of each die level test.              */
/*                                                                           */
/* Outputs a wfmap table entry:                                              */
/*    The data file is an ascii file with this format:                       */
/*    lot-id|wafer-id|die-label|dieXYloc|#Results|parm1|...parmN             */
/*   where the | represents white space
/*                                                                           */
/* Each test result has a database flag associated with it. If the flag      */
/* is "do not send", we do not output this result.     ???                   */
/*                                                                           */
/* This example makes the assumption that we do not have an array-type       */
/* results.                                                                  */
/*                                                                           */
/*                                                                           */
/*****************************************************************************/
{                                                     /* BEGIN FUNCTION      */
   int i;                                             /* index               */
   int result_count;
   char temp[1024];                                   /* holds ascii data    */

   strcpy (DAFF_DIELABEL,curr_die());                 /* die label           */
   DAFF_DIEXLOC = curr_die_Xloc();                    /* die X loc           */
   DAFF_DIEYLOC = curr_die_Yloc();                    /* die X loc           */
   result_count = number_of_results();                /* number of parms     */

/* Output constant header before the result values                           */
   fprintf(WAFMP_FP,"%s %s %s %d,%d %d ",             /* 1st half            */
               DAFF_LOTID,                            /* lot id              */
               DAFF_WAFERID,                          /* wafer id            */
               DAFF_DIELABEL,                         /* die label           */
               DAFF_DIEXLOC,                          /* die column          */
               DAFF_DIEYLOC,                          /* die row             */
               DAFF_NUMRESULTS);                      /* number of results   */

/* Output result values for each die tested                                  */

   for (i = 0; i < DAFF_NUMRESULTS; i++) {            /* FOR num of parms    */
      strcpy (temp,get_ascii_data(i));                /* get result as ascii */
      if (strlen(temp) <= 0)                          /* check for null strng*/
         fprintf (WAFMP_FP,"%s ","NULL_STRING");      /* place holder        */
      else
         fprintf (WAFMP_FP,"%s ", temp);              /* output result       */
      }                                               /* ENDIF get_database  */
                                                      /* ENDFOR              */

   fprintf(WAFMP_FP,"\n");                            /* terminate record    */
   return(0);                                         /* normal exit         */
}                                                     /* END eod_wfmap       */


int eop_wfmap()                                       /* End of Die Pair     */
/*****************************************************************************/
/* At End of Test Definition Pair                                            */
/*                                                                           */
/* This function is invoked each time at the completion of a TD Pair.        */
/*                                                                           */
/*****************************************************************************/
{                                                     /* BEGIN FUNCTION      */
  fprintf (WAFMP_FP,"\n");                            /* blank line          */
  return(0);                                          /* normal exit         */
}                                                     /* END eop_wfmap       */

 
int eow_wfmap()                                       /* End of Wafer        */
/*****************************************************************************/
/* At End of Wafer                                                           */
/*                                                                           */
/* This function is invoked each time at the completion of a wafer.          */
/*                                                                           */
/*****************************************************************************/
{                                                     /* BEGIN FUNCTION      */
   if (WAFMP_FP != stdout)
      fclose(WAFMP_FP);                               /* close file          */
   return(0);                                         /* normal exit         */
}                                                     /* END eow_wfmap       */


int eot_wfmap()                                       /* End ot Test         */
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
   return(0);                                        /*                      */
}                                                    /* END eot_wfmap        */


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

struct format_func_struct daff_wfmap[] = {
     bot_wfmap,                                       /* begin of test func  */
     bow_wfmap,                                       /* begin of wafer func */
     bop_wfmap,                                       /* begin of pair func  */
     bod_wfmap,                                       /* begin of die func   */
     eod_wfmap,                                       /* end of die func     */
     eop_wfmap,                                       /* end of pair func    */
     eow_wfmap,                                       /* end of wafer func   */
     eot_wfmap                                        /* end of test func    */
};


