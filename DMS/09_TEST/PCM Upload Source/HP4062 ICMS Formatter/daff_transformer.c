/*****************************************************************************/
/* FILE: daff_transformer.c
/* 
/* DESCRIPTION:
/* 	transformer for a sfab device conversion
/*	file data transformer.  This is a more elegant way of collecting 
/*	the data that is printed on the CRT.
/*
/* ACKNOWLEDGEMENT:
/*	Original code based on daff_dbms.c by Hewlett-Packard Co.
/*
/* HISTORY:
/*	08/15/07	Original co	chunho-park  (Dongbu Hi-Tech. Korea)
/*
/*****************************************************************************/
 
/* SCCS identifier string - This should not be changed except by SCCS */
static char sccs_id[] = "@(#)daff_transformer.c	1.1	03/11/00";

#include <stdio.h>
#include <stdlib.h>
#include <macros.h>
#include <time.h>
#include "accesslib.h"                                 /* access to ICMS dar */

#ifdef XPOPUP
/*extern void pop_error_box();      X-Window popup error boxes are supported */
#endif

/*        --- GLOBAL DECLARATION ---                                         */
#define VERSION 5;
FILE *data_file,*head_file,*SFAB,*SFAB_head,*CON;
FILE *fp1,*fp2;

static int formatter_abort = 0;	         /* Flag to abort formatter on error */
static char waf_id[21];
char Con_dir[50];
char START_TIME[50];
char WF[14];
char Wafer_ID[14];
int xxxx;
extern char Start_time[] = "mm/dd/yyyy hh:mm:ss";
static char head_data_file[30],test_data_file[30],sfile[30],sfile_head[30];
extern int  data_desc_flag = 0 ; 
extern int  data_header_flag = 0 ; 
extern int die=0;
extern char TN[17];
 
/*****************************************************************************
/* If X-Window library support is not available, the popup windows are       */
/* replaced with print strings to stderr.                                    */
/*****************************************************************************/
/*
#ifndef XPOPUP                                                             
void pop_error_box(s)
char *s;
{
	fprintf(stderr, "%s\n", s);
}
#endif
*/
 
int bot_nonti()                                         /* Beginning of Test */
/*****************************************************************************/
/* At Beginning of Test Function                                             */ 
/*  This function is invoked only at the beginning of each test.             */ 
/*****************************************************************************/ 
{                                                      /* BEGIN FUNCTION     */
   char out_string[300];
   char test_prog[21];
   char wafer_id[21];
   char lot_number[9];
   char system_command[100];
   char system_command1[100];
   char non_TI_file[100];  /* Filename for output nonti file */
   char hostname[15];


   int i;
   char c;
   formatter_abort = 0;
   data_desc_flag = 0;
   data_header_flag = 0;
   die=0;

   /**************************************************************************/
   /* Build output filename */
   /**************************************************************************/

   strncpy(test_prog,curr_test(),20);
   strncpy(wafer_id,waf_id,20);
   strncpy(lot_number, get_lot_id(), 8);

   lot_number[8] = '\0';

   for ( i = 0; i < 8; i++ )
	if ( lot_number[i] == ' ' )
		lot_number[i] = '_'; /* Replace blank spaces with underscores*/

   printf("format dir : %s\n",format_directory_name());


   strcpy(non_TI_file, "/users/testware/");
   strcat(non_TI_file, "non_ti/parametric");
   strcat(non_TI_file, "/");
   strcat(non_TI_file, test_prog);
   strcat(non_TI_file, "/");
   strcat(non_TI_file, lot_number);

	sprintf(Con_dir,"%s",non_TI_file);
	sprintf(START_TIME,"%s",present_time());
	printf("Con_dir: %s , START_TIME: %s\n",Con_dir,START_TIME);



   printf("current tester %s \n",curr_testsys());
   printf("start tester name %s \n",test_start_time());

   printf("OUTPUT FILE  : %s\n",non_TI_file ); 


   strcpy(system_command, "mkdir ");
   strcat(system_command, "/users/testware/");
   strcat(system_command, "non_ti/parametric");
   strcat(system_command, "/");
   strcat(system_command, test_prog);
   strcat(system_command, "/");

   strcpy(system_command1, "mkdir ");
   strcat(system_command1, "/users/testware/");
   strcat(system_command1, "non_ti/parametric");
   strcat(system_command1, "/");
   strcat(system_command1, test_prog);
   strcat(system_command1, "/");
   strcat(system_command1, lot_number);
  

   printf("COMMAND : %s\n",system_command); 
   printf("COMMAND1 : %s\n",system_command1); 

	system(system_command);
	system(system_command1);

	strcpy(system_command1,"hostname"); 

   if ( (CON = fopen("/tmp/CON","w")) ==NULL ){
        sprintf(out_string, "ERROR: Cannot open output head file /tmp/CON\n\n");
        strcat(out_string, "\nClick OK to abort\n");
        pop_error_box(out_string);
        formatter_abort = 1;
        }
fclose(CON);

	return(0);                                     /* normal exit        */
}                                                      /* END eot_text       */
 

int bow_nonti()                                         /* Begin of Wafer    */
/*****************************************************************************/
/* At Beginning of New Wafer                                                 */
/*                                                                           */
/* This function is invoked at the beginning of testing each wafer.          */
/*****************************************************************************/
{                                                     /* BEGIN FUNCTION      */

   struct tm *ptr;
   time_t now;
   char device[20];
   char dfile[20];
   char make_device[50];
   char out_string[300];
   char test_prog[21];
   char wafer_id[21];
   char lot_number[9];
   char sms_device[21];
   char system_command[50],tdata[100],server[20],tester_name[10];
   char non_TI_file[50];  /* Filename for output nonti file */
   char c;
   int k,n;
   int result_count,                                  /* number of results   */
       i,j,k1,k2;                                     /* index               */
   char test_time[20];
   char wid[20],ocrmap[50];
   char dir[20];
   char outdir[20];
   FILE *fp,*out,*curr;
   FILE *fdevice,*tester;

  xxxx = xxxx + 1; 

   /* OCR Map data gathering for wafer number		*/
   /* Modified By JW SEO of HP-Korea & Hyun-Jeong Lee   */

   sprintf(dir,"/tmp/.ocrMap-0");
   sprintf(outdir,"/tmp/.ocrMap-0.out");

   fp=fopen(dir,"r");
   out=fopen(outdir,"w");
     for (i=1;i<=9;i++) {
       j=fscanf(fp,"%s",ocrmap);
         if (i==2){
           sprintf(wid,ocrmap);
           }
     }
     for (k=0;k<=12;k++)
     {
     if (k>0){
        fprintf(out,"%c",wid[k]);
	sprintf(WF,"%s",wid);
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

	/* OCR Map data gathering for wafer number		*/
sprintf(Wafer_ID,"%c%c%c%c%c%c%c%c%c%c%c%c",WF[1],WF[2],WF[3],WF[4],WF[5],WF[6],WF[7],WF[8],WF[9],WF[10],WF[11],WF[12]);

   formatter_abort = 0;
   data_desc_flag = 0;
   data_header_flag = 0;
   die=0;
   /**************************************************************************/
   /* Build output filename */
   /**************************************************************************/

   strncpy(test_prog,curr_test(),20);
   strcpy(sms_device,curr_test());
   strncpy(wafer_id,waf_id,20);
   strncpy(lot_number, get_lot_id(), 8);


   sprintf(head_data_file,"/tmp/.amptsvr_shead");
   sprintf(test_data_file,"/tmp/.amptsvr_data");
   sprintf(sfile,"/tmp/.amptsvr_sfile");
   sprintf(sfile_head,"/tmp/.amptsvr_sfile_head");



   time(&now);
   ptr = localtime(&now);
   strftime(test_time,20,"%m/%d/%Y %H:%M:%S",ptr);





   lot_number[8] = '\0';

   for ( i = 0; i < 8; i++ )
	if ( lot_number[i] == ' ' )
		lot_number[i] = '_'; /* Replace blank spaces with underscores*/

   strcpy(non_TI_file, "/users/testware/");

   strcat(non_TI_file, "non_ti/parametric");
   strcat(non_TI_file, "/");
   strcat(non_TI_file, test_prog);
   strcat(non_TI_file, "/");
   strcat(non_TI_file, lot_number);
   strcat(non_TI_file, "/");
   strcat(non_TI_file, wafer_id);

   sprintf(Start_time,"%s",present_time());


   if ( (SFAB = fopen(sfile,"w")) ==NULL ){
        sprintf(out_string, "ERROR: Cannot open output head file %s\n\n", sfile);
        strcat(out_string, "\nClick OK to abort\n");
        pop_error_box(out_string);
        formatter_abort = 1;
        }

   if ( (SFAB_head = fopen(sfile_head,"w")) ==NULL ){
        sprintf(out_string, "ERROR: Cannot open output head file %s\n\n", sfile_head);
        strcat(out_string, "\nClick OK to abort\n");
        pop_error_box(out_string);
        formatter_abort = 1;
        }

   if ( (head_file = fopen(head_data_file,"w")) ==NULL ){
        sprintf(out_string, "ERROR: Cannot open output head file %s\n\n", head_file);
        strcat(out_string, "\nClick OK to abort\n");
        pop_error_box(out_string);
        formatter_abort = 1;
        }





   return(0);                                         /* normal exit         */
}                                                     /* END bow_nonti       */


int bop_nonti()                                         /* Begin of Test Pair*/
/*****************************************************************************/
/* At Beginning of Test Definition Pair                                      */
/*                                                                           */
/* Not used but needs to be defined.                                         */
/*****************************************************************************/
{                                                      /* BEGIN FUNCTION     */
   return(0);                                          /* normal exit        */
}                                                      /* END bop_nonti      */


int bod_nonti()                                         /* Begin of Die      */
/*****************************************************************************/
/* At Beginning of Die                                                       */
/*                                                                           */
/* This function is invoked at the beginning of testing each die.            */
/*                                                                           */
/*****************************************************************************/
{                                                     /* BEGIN FUNCTION      */
  return(0);                                          /* normal exit         */
}                                                     /* END bod_nonti       */


int eod_nonti()                                        /* End of Die         */
/*****************************************************************************/
/*  At End of Die Execution                                                  */
/*                                                                           */
/*  This function is invoked at the end of each die level test.              */
/*                                                                           */
/* Outputs a string to the text file with the following format:       	     */
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
{                                                      /* BEGIN FUNCTION      */
   FILE *tester;
   char out_string[300];
   char test_prog[21];
   char tester_name[20];
   char wafer_id[21];
   char lot_number[9];
   char sms_device[21];
   char system_command[50];
   char non_TI_file[50];  /* Filename for output nonti file */
   char c;
   char data_value[30];
   int result_count,i,j,k1,k2;                         /* number of results   */
   char send_data[60],ans[10];
   float data_value_float;
   struct tm *ptr;
   time_t now;
   char test_time[20],make_data[150],tdata[100],server[20];
   char loop[40];
   char aaaa[90]; 
 
	data_header_flag = 1;
   	result_count = number_of_results();            /* number of parms     */


/* Output result values for each die tested                                  */





printf("data_flag = %d\n",data_desc_flag);
if(data_desc_flag == 0 ) {	
   for (i = 0; i < result_count; i++) {               /* FOR num of parms    */
	if (get_database_flag(i)) {                   /* save to database?   */

       		fprintf(SFAB_head,"%s\n",get_data_desc(i));         
		if(i == result_count-1){   die = 0;	
			fprintf(SFAB,"[ChipDataVal]\n");
		}
	}
   }
}



	




   for (i = 0; i < result_count; i++) {               /* FOR num of parms    */
	if (get_database_flag(i)) {                   /* save to database?   */

		if (get_tested(i))   
			data_value_float = atof(get_ascii_data(i));
		else
			sprintf(data_value, "%g", get_not_legal_number());

		fprintf(SFAB, "%d,%s,%s\n",i,get_data_desc(i),get_ascii_data(i)); 

		printf("%d,%s,%s\n",i,get_data_desc(i),get_ascii_data(i)); 
		if(i == result_count-1){
			
                         fprintf(SFAB,"\n");
		
		}
	}
    }


	data_desc_flag = 1; 
	die=die+data_desc_flag;



	if(die == total_die()) {	
	

		
/*   if ( (CON = fopen("/tmp/CON","a")) ==NULL ){
        sprintf(out_string, "ERROR: Cannot open output head file /tmp/CON\n\n");
        strcat(out_string, "\nClick OK to abort\n");
        pop_error_box(out_string);
        formatter_abort = 1;
        }  */
		fclose(SFAB);
		fclose(SFAB_head);

	   	strncpy(test_prog,curr_test(),20);
	   	strcpy(sms_device,curr_test());
	   	strncpy(wafer_id,waf_id,20);
	   	strncpy(lot_number, get_lot_id(), 8);
fprintf(CON,"\n");
fprintf(CON,"Wafer ID: %s 	#Slot: %d\n",lot_number,xxxx);
fprintf(CON,"\"Item Name               1( 3, 2)   2( 6, 1)   3( 3, 1)   4( 0, 1)   ");
fprintf(CON,"5( 3, 4)   6( 2, 0)   7( 5, 0)   8( 2, 3)   9( 5, 3)    Avg        Std        Min        Max    \"\n");

        fflush(CON);
	fclose(CON);
/*
sprintf(aaaa,"(/users/tye/PROGRAM/CONVERSION/data_add2.sh;/users/tye/PROGRAM/CONVERSION/xx)");
system(aaaa);
*/
	   	system("uname -a > /tmp/.tester_name");
	   	tester=fopen("/tmp/.tester_name","r");
	   	fgets(tdata,100,tester);
	   	sscanf(tdata,"%s%s",server,tester_name);
	   	fclose(tester);
	
	}
   return(0);                                         /* normal exit         */
}                                                     /* END eow_nonti       */


int eop_nonti()                                        /* End of Die Pair    */
/*****************************************************************************/
/* At End of Test Definition Pair                                            */
/*                                                                           */
/* This function is invoked each time at the completion of a TD Pair.        */
/*                                                                           */
/* Not used but needs to be defined.                                         */
/*****************************************************************************/
{
                                                      /* BEGIN FUNCTION      */
  return(0);                                          /* normal exit         */
}                                                     /* END eop_nonti       */

 
int eow_nonti()                                        /* End of Wafer       */
/*****************************************************************************/
/* At End of Wafer                                                           */
/*                                                                           */
/* This function is invoked each time at the completion of a wafer.          */
/*                                                                           */
/* Not used but needs to be defined.                                         */
/*****************************************************************************/
{
   char test_prog[21];
   char wafer_id[21];
   char lot_number[9];
   char send_data[150];
   char sms_device[21];
   char TR[50],TR1[30],TR2[150]; 
	   	strncpy(test_prog,curr_test(),20);
	   	strcpy(sms_device,curr_test());
	   	strncpy(wafer_id,waf_id,20);
	   	strncpy(lot_number, get_lot_id(), 8);


        	fprintf(head_file,"Lot ID      : %s\n",lot_number);
        	fprintf(head_file,"Cassette ID : %s\n","NONE");
        	fprintf(head_file,"Test ID     : %s\n",test_prog);
        	fprintf(head_file,"Start Time  : %s-%s\n",START_TIME,present_time()); 
        	fprintf(head_file,"Operator    : DTA002\n");



        	fflush(head_file);
		fclose(head_file);
/*
		sprintf(TR,"%s/%s",Con_dir,WF);
		sprintf(TR2,"/users/tye/PROGRAM/CONVERSION/data_add.sh -D %s ",TR);

		system(TR2);   
		printf("TR2 is : %s\n",TR2);
*/

   return(0);                     
}                                


int eot_nonti()                                        /* End ot Test        */
/*****************************************************************************/
/* At End of Test Function                                                   */
/*                                                                           */
/* This function is invoked at the completion of a test.                     */
/*****************************************************************************/
{
 if (!formatter_abort) {
  }
   return(0);
}                                                    /* END eot_nonti        */


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

struct format_func_struct daff_nonti[] = {
     bot_nonti,                                        /* begin of test func  */
     bow_nonti,                                        /* begin of wafer func */
     bop_nonti,                                        /* begin of pair func  */
     bod_nonti,                                        /* begin of die func   */
     eod_nonti,                                        /* end of die func     */
     eop_nonti,                                        /* end of pair func    */
     eow_nonti,                                        /* end of wafer func   */
     eot_nonti                                         /* end of test func    */
};


