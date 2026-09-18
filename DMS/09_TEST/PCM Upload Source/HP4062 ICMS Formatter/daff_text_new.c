/*****************************************************************************/
/* FILE: daff_text.c
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
#include <unistd.h>
#include <ctype.h>
#include "/usr/icms/include/accesslib.h"
/* access to ICMS dar */
#include <time.h> /* Timing Header File */

#ifdef XPOPUP
/*extern void pop_error_box();     /* X-Window popup error boxes are supported */
#endif

/*        --- GLOBAL DECLARATION ---                                         */
FILE *output_file;
FILE *fin;
static int formatter_abort = 0; /* Flag to abort formatter on error */
static int pgm_len = 0;         /* Flag to abort formatter on error */
static char LogFile[100];
static char LogFile_SVR2[100];
static char Testlog[100];
static char Wafer_id[20];
static char Input_lot_id[20];
static char Pc_id[20];
static char Date[30];
static char Time[30];
static char StartTime[60];
static char EndTime[60];
static char HostName[30];
static char cassette_test[10];
static char wafer_test[10];
static char wafer_FlatZone[5];
static char txt_new_file_name[20];

/*****************************************************************************/
/* user function definition
/*****************************************************************************/
int bow_FlatZone();
int bow_status_write();
int bow_text_new();
int eow_text_new();

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

int bot_text() /* Beginning of Test  */
/*****************************************************************************/
/* At Beginning of Test Function                                             */
/*                                                                           */
/* This function is invoked only at the beginning of each test.              */
/*****************************************************************************/
{ /* BEGIN FUNCTION     */
    char out_string[300];
    char lot_number[9];
    char Input_lot_n[9];
    char tfile[50]; /* Filename for output text file */
    int i, j, k;
    char pcid[20];
    int pc_id_len;
    char c, min[3], sec[3], op_name[30];

    time_t tnum;
    struct tm *ts;
    time(&tnum);
    ts = localtime(&tnum);
    sprintf(min, "%d", ts->tm_min);
    sprintf(sec, "%d", ts->tm_sec);

    formatter_abort = 0;
    /**************************************************************************/
    /* Build output filename */
    /**************************************************************************/
    strncpy(lot_number, get_lot_id(), 8);
    lot_number[8] = '\0';
    for (i = 0; i < 8; i++)
        if (lot_number[i] == ' ')
            lot_number[i] = '_'; /* Replace blank spaces with underscores*/
    strcpy(tfile, format_directory_name());
    strcat(tfile, "/");
    strcat(tfile, lot_number);
    strcat(tfile, "_");
    strcat(tfile, min);
    strcat(tfile, sec);
    strcat(tfile, ".txt");
    printf("Daff_text Output file Name == #%s#\n", tfile);
    /**************************************************************************/
    /* Open output file */
    /**************************************************************************/
    if ((output_file = fopen(tfile, "w")) == NULL)
    {
        sprintf(out_string, "ERROR: Cannot open output file %s\n\n", tfile);
        strcat(out_string, "\nClick OK to abort\n");
        pop_error_box(out_string);
        formatter_abort = 1;
        return (-1);
    }
    printf("wafer test=%s,cassette test=%s\n", curr_test(), curr_cassette());
    strcpy(cassette_test, curr_cassette());
    pgm_len = strlen(cassette_test);
    if (pgm_len < 4)
    {
        fprintf(output_file, "Logging Raw Data from Test  <%s>   ", curr_test());
        strcpy(cassette_test, curr_test());
    }
    else
    {
        fprintf(output_file, "Logging Raw Data from Test  <%s>   ", curr_cassette());
    }
    fprintf(output_file, "Executing on: %s\n\n", present_time());
    fprintf(output_file, "-------------------------------------\n");

    /* Modified by HP-Korea A.E. Ji-Won Seo for Operator NAME */
    k = number_of_udcs();
    for (j = 0; j < k; j++)
    {
        if (j == 0)
        {
            strcpy(Input_lot_id, get_udc_ascii_data(j));
            printf("input_lot_id=%s\n", get_udc_ascii_data(j));
        }
        if (j == 1)
        {
            strcpy(op_name, get_udc_ascii_data(j));
            fprintf(output_file, "Operator Name is : %s\n", op_name);
        }
        if (j == 2)
        {
            strcpy(pcid, get_udc_ascii_data(j));
            pc_id_len = strlen(pcid);
            printf("pcid=%s,pc id len = %d\n", pcid, pc_id_len);
            if (pc_id_len < 4)
                strcpy(Pc_id, "NONE");
            else
                sprintf(Pc_id, "%c%c%c%c", pcid[pc_id_len - 4], pcid[pc_id_len - 3], pcid[pc_id_len - 2], pcid[pc_id_len - 1]);

            printf("pc id=%s\n", Pc_id);
        }
    }
    fflush(output_file);

    bow_status_write(lot_number, "start");

    return (0); /* normal exit        */
} /* END bot_text       */

int bow_FlatZone()
/*****************************************************************************/
/* At Beginning of Test New Function                                         */
/*                                                                           */
/* This function is Wafer FlatZone information                               */
/* Add by 2019.08.22                                                         */
/*****************************************************************************/
{ /* BEGIN FUNCTION     */
    FILE *pgmFile = NULL, *flatFile = NULL;
    char buffer[256], *result, *pStr, *text;
    char out_string[300];
    char pgmFilePath[128], flatFilePath[128];
    int idx = 0;

    /* find to wafer type */
    sprintf(pgmFilePath, "/users/tye/WAFERTEST/%s", cassette_test);
    if((pgmFile = fopen(pgmFilePath, "r"))== NULL)
    {
        sprintf(out_string, "ERROR: Cannot open file %s\n\n", pgmFilePath);
        strcat(out_string, "\nClick OK to abort\n");
        pop_error_box(out_string);
        formatter_abort = 1;
        return (-1);
    }

    while (!feof(pgmFile))
    {
        result = fgets(buffer, sizeof(buffer), pgmFile);
        result = (char*)strstr(buffer, "WaferType:");
        if(result != NULL)
        {
            pStr = (char*)strtok(buffer, " ");
            while (pStr != NULL)
            {
                text = pStr;
                pStr = (char*)strtok(NULL, " ");
            }
            
            break;
        }
    }

    fclose(pgmFile);
    
    /* delete of new line character */
    /* 개행문자가 존재할 경우 정상적으로 파일을 open 하지 못함 */
    for(idx = strlen(text); idx > 0; idx--)
    {
        switch (text[idx])
        {
            case '\t':
            case '\r':
            case '\n':
                text[idx] = '\0';
                break;
            
            default:
                break;
        }   
    }

    /* find to flat zone */    
    sprintf(flatFilePath, "/users/tye/WAFER/%s", text);
    if((flatFile = fopen(flatFilePath, "r")) == NULL)
    {
        sprintf(out_string, "ERROR: Cannot open file %s\n\n", flatFilePath);
        strcat(out_string, "\nClick OK to abort\n");
        pop_error_box(out_string);
        formatter_abort = 1;
        return (-1);
    }

    while (!feof(flatFile))
    {
        result = fgets(buffer, sizeof(buffer), flatFile);
        result = (char*)strstr(buffer, "Flat");
        if(result != NULL)
        {
            pStr = (char*)strtok(buffer, " ");
            while (pStr != NULL)
            {
                text = pStr;
                pStr = (char*)strtok(NULL, " ");
            }
            
            break;
        }
    }

    fclose(flatFile);    

    /* delete of new line character  */
    for(idx = strlen(text); idx > 0; idx--)
    {
        switch (text[idx])
        {
            case '\t':
            case '\r':
            case '\n':
                text[idx] = '\0';
                break;
            
            default:
                break;
        }   
    }

    strcpy(wafer_FlatZone, text);

    return (0);
} /* END bow_text new       */

int bow_status_write(char *value, char *status)
/* Beginning of equipment status */
/*****************************************************************************/
/* At Beginning of Test New Function                                         */
/*                                                                           */
/* This function is equipment status write                                   */
/* Add by 2019.08.22                                                         */
/*****************************************************************************/
{ /* BEGIN FUNCTION     */

    FILE *status_fp;
    char statusfilename[100];
    char out_string[300];

    sprintf(statusfilename, "/users/ap_data/EQUIP/TEST/PCM/STATUS/%s", HostName);
    if ((status_fp = fopen(statusfilename, "w")) == NULL)
    {        
        sprintf(out_string, "ERROR: Cannot open output file %s\n\n", statusfilename);
        strcat(out_string, "\nClick OK to abort\n");
        pop_error_box(out_string);
        formatter_abort = 1;
        return (-1);
    }

    fprintf(status_fp, "%s,%s", value, status);
    fflush(status_fp);
    fclose(status_fp);

    return (0); /* normal exit        */
} /* END bow_text new       */

int bow_text_new() /* Beginning of Wafer new */
/*****************************************************************************/
/* At Beginning of Test New Function                                         */
/*                                                                           */
/* This function is invoked only at the beginning of each test.              */
/* Add by 2019.08.22                                                         */
/*****************************************************************************/
{ /* BEGIN FUNCTION     */
    char out_string[300];
    char lot_number[9];
    char Input_lot_n[9];
    char tfile[50]; /* Filename for output text file */
    int i, j, k;
    char pcid[20];
    int pc_id_len;
    char c, min[3], sec[3], op_name[30];

    time_t tnum;
    struct tm *ts;
    time(&tnum);
    ts = localtime(&tnum);
    sprintf(min, "%d", ts->tm_min);
    sprintf(sec, "%d", ts->tm_sec);

    formatter_abort = 0;
    /**************************************************************************/
    /* Build output filename */
    /**************************************************************************/
    strncpy(lot_number, get_lot_id(), 8);
    lot_number[8] = '\0';
    for (i = 0; i < 8; i++)
        if (lot_number[i] == ' ')
            lot_number[i] = '_';                     /* Replace blank spaces with underscores*/
    strcpy(tfile, "/users/ap_data/EQUIP/TEST/PCM_TMP/"); /* NFS Path */
    strcpy(txt_new_file_name, lot_number);
    strcat(txt_new_file_name, "_");
    strcat(txt_new_file_name, Wafer_id);
    strcat(txt_new_file_name, "_");
    strcat(txt_new_file_name, min);
    strcat(txt_new_file_name, sec);
    strcat(txt_new_file_name, ".txt");
    strcat(tfile, txt_new_file_name);
    printf("Daff_text Output file Name == #%s#\n", tfile);
    /**************************************************************************/
    /* Open output file */
    /**************************************************************************/
    if ((output_file = fopen(tfile, "w")) == NULL)
    {
        sprintf(out_string, "ERROR: Cannot open output file %s\n\n", tfile);
        strcat(out_string, "\nClick OK to abort\n");
        pop_error_box(out_string);
        formatter_abort = 1;
        return (-1);
    }
    printf("wafer test=%s,cassette test=%s\n", curr_test(), curr_cassette());
    fprintf(output_file, "Logging Raw Data from Test  <%s>   ", cassette_test);
    fprintf(output_file, "Executing on: %s\n\n", present_time());
    fprintf(output_file, "-------------------------------------\n");

    /* Modified by HP-Korea A.E. Ji-Won Seo for Operator NAME */
    k = number_of_udcs();
    for (j = 0; j < k; j++)
    {
        if (j == 0)
        {
            strcpy(Input_lot_id, get_udc_ascii_data(j));
            printf("input_lot_id=%s\n", get_udc_ascii_data(j));
        }
        if (j == 1)
        {
            strcpy(op_name, get_udc_ascii_data(j));
            fprintf(output_file_new, "Operator Name is : %s\n", op_name);
        }
        if (j == 2)
        {
            strcpy(pcid, get_udc_ascii_data(j));
            pc_id_len = strlen(pcid);
            printf("pcid=%s,pc id len = %d\n", pcid, pc_id_len);
            if (pc_id_len < 4)
                strcpy(Pc_id, "NONE");
            else
                sprintf(Pc_id, "%c%c%c%c", pcid[pc_id_len - 4], pcid[pc_id_len - 3], pcid[pc_id_len - 2], pcid[pc_id_len - 1]);

            printf("pc id=%s\n", Pc_id);
        }
    }
    fflush(output_file_new);

    return (0); /* normal exit        */
} /* END bow_text new       */

int bow_text() /* Begin of Wafer     */
/*****************************************************************************/
/* At Beginning of New Wafer                                                 */
/*                                                                           */
/* This function is invoked at the beginning of testing each wafer.          */
/*****************************************************************************/
{ /* BEGIN FUNCTION      */

    /* This is for the Time & Host name */
    char yeardate[30];
    char log_date[30];
    char time[30];
    char timedate[60]; /* YYYYMMDD HHMMSS */
    char hostname[30];
    /* This is for the Time & Host name */

    char ocrmap[50];
    char dir[50];
    char outdir[50];
    char wid[20];
    char waf_id[20];
    char data[30];
    int i, j, k, n;

    FILE *fp, *out, *curr;

    struct timeval tp;
    struct timezone tzp;
    struct tm *tm_p;

    /* To get time format : YYYY/MM/DD HH:MM:SS */
    gettimeofday(&tp, &tzp);
    tm_p = localtime((time_t *)&tp.tv_sec);
    sprintf(yeardate, "%d/%02d/%02d",
            (tm_p->tm_year + 1900), (tm_p->tm_mon + 1), tm_p->tm_mday);

    sprintf(log_date, "%d%02d%02d",
            (tm_p->tm_year + 1900), (tm_p->tm_mon + 1), tm_p->tm_mday);

    for (i = 0; i < strlen(yeardate); i++)
        if (yeardate[i] == ' ')
            yeardate[i] = '0';
    yeardate[30] = '\0';
    printf("YYYYMMDD == %s\n", yeardate);

    /* To get FILE time format : HHMMSS */
    sprintf(time, "%2d:%2d:%2d", tm_p->tm_hour, tm_p->tm_min, tm_p->tm_sec);
    for (i = 0; i < strlen(time); i++)
        if (time[i] == ' ')
            time[i] = '0';
    time[30] = '\0';
    printf("HH:MM:SS == %s\n", time);

    strcpy(timedate, yeardate);
    strcat(timedate, " ");
    strcat(timedate, time);
    printf("Result is == %s \n", timedate);

    /* To get the Host Name of Current System */
    j = gethostname(hostname, 30);
    for (i = 0; i < strlen(hostname); i++)
        hostname[i] = toupper(hostname[i]);
    printf("Status == %d hostname == %s\n", j, hostname);

    /* Modified By JI WON SEO of HP-KOREA & Hyun-Jeong LEE */
    sprintf(dir, "/tmp/.ocrMap-0");
    sprintf(outdir, "/tmp/.ocrMap-0.out");
    fp = fopen(dir, "r");
    out = fopen(outdir, "w");
    for (i = 1; i <= 9; i++)
    {
        printf("count = %d\n", i);
        j = fscanf(fp, "%s", ocrmap);
        if (i == 2)
        {
            sprintf(wid, ocrmap);
            printf("ocr_wf_id = %s\n", ocrmap);
        }
    }
    for (k = 0; k <= 12; k++)
    {
        if (k > 0)
        {
            fprintf(out, "%c", wid[k]);
            printf("wid[%d]=%c\n", k, wid[k]);
        }
    }
    fclose(out);
    curr = fopen(outdir, "r");
    for (n = 0; n < 2; n++)
    {
        fscanf(curr, "%s", waf_id);
        printf("n=%d,wf_id=%s\n", n, waf_id);
    }

    /* Copy the Current Wafer ID to Global Variables */
    strcpy(Wafer_id, waf_id);
    strcpy(StartTime, timedate);
    strcpy(HostName, hostname);

    /* Add by 2019.08.22 */
    bow_status_write(Wafer_id, "start");
    bow_text_new();

    fprintf(output_file, "-------------------------------------\n");
    fprintf(output_file, "Current Wafer = %s\n\n\n", waf_id);

    /* Add by 2019.08.22 */
    fprintf(output_file_new, "-------------------------------------\n");
    fprintf(output_file_new, "Current Wafer = %s\n\n\n", waf_id);

    fclose(fp);
    fclose(curr);

    fflush(output_file);

    /* Add by 2019.08.22 */
    fflush(output_file_new);

    /* Log File Name == AMPT01_20011115.log */
    strcpy(LogFile, "/users/tye/DCLOG/");
    strcat(LogFile, HostName);
    strcat(LogFile, "_");
    strcat(LogFile, log_date);
    strcat(LogFile, ".log");
    printf("Log File Name == #%s#\n", LogFile);

    strcpy(LogFile_SVR2, "/SVR2/tye/DCLOG/");
    strcat(LogFile_SVR2, HostName);
    strcat(LogFile_SVR2, "_");
    strcat(LogFile_SVR2, log_date);
    strcat(LogFile_SVR2, ".log");
    printf("Log File Name == #%s#\n", LogFile_SVR2);

    strcpy(Testlog, "/users/tye/Testlog/");
    strcat(Testlog, HostName);
    strcat(Testlog, "_");
    strcat(Testlog, log_date);
    strcat(Testlog, ".trc");
    printf("Log File Name == #%s#\n", Testlog);

    /* Making the Log File for Beginning of Wafer */
    /* 2001/11/15 17:06:22|AMPT01|WAFERID|S */

    return (0); /* normal exit         */
} /* END bow_text        */

int bop_text() /* Begin of Test Pair */
/*****************************************************************************/
/* At Beginning of Test Definition Pair                                      */
/*                                                                           */
/* Not used but needs to be defined.                                         */
/*****************************************************************************/
{               /* BEGIN FUNCTION     */
    return (0); /* normal exit        */
} /* END bop_text       */

int bod_text() /* Begin of Die       */
/*****************************************************************************/
/* At Beginning of Die                                                       */
/*                                                                           */
/* This function is invoked at the beginning of testing each die.            */
/*                                                                           */
/*****************************************************************************/
{ /* BEGIN FUNCTION      */
    /* Add by 2019.08.30 */
    bow_FlatZone();

    fprintf(output_file, "-------------------------------------\n");
    fprintf(output_file, "Current Die = %s (%d, %d, %s),   Current Die Type = %s\n\n",
            curr_die(), curr_die_Xloc(), curr_die_Yloc(), wafer_FlatZone, curr_die_type());

    /* Add by 2019.08.22 */
    fprintf(output_file_new, "-------------------------------------\n");
    fprintf(output_file_new, "Current Die = %s (%d, %d),   Current Die Type = %s\n",
            curr_die(), curr_die_Xloc(), curr_die_Yloc(), curr_die_type());
    fprintf(output_file_new, "Flat Zone = %s\n\n",
            wafer_FlatZone);

    return (0); /* normal exit         */
} /* END bod_text        */

int eod_text() /* End of Die          */
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
{                     /* BEGIN FUNCTION      */
    int result_count, /* number of results   */
        i;            /* index               */
    char data_val[30];
    int err = 0;

    result_count = number_of_results(); /* number of parms     */

    /* Output result values for each die tested                                  */

    for (i = 0; i < result_count; i++)
    { /* FOR num of parms    */
        if (get_database_flag(i))
        { /* save to database?   */

            if (get_tested(i))
                strcpy(data_val, get_ascii_data(i));
            else
                sprintf(data_val, "%g", get_not_legal_number());

            fprintf(output_file, "%s\t%s\t%s\n", get_data_name(i),
                    data_val, get_data_desc(i));

            /* Add by 2019.08.22 */
            fprintf(output_file_new, "%s\t%s\t%s\n", get_data_name(i),
                    data_val, get_data_desc(i));
        }
    }             /* ENDIF get_database  */
                  /* ENDFOR              */
    return (err); /* normal exit         */
} /* END eod_text        */

int eop_text() /* End of Die Pair     */
/*****************************************************************************/
/* At End of Test Definition Pair                                            */
/*                                                                           */
/* This function is invoked each time at the completion of a TD Pair.        */
/*                                                                           */
/* Not used but needs to be defined.                                         */
/*****************************************************************************/
{               /* BEGIN FUNCTION      */
    return (0); /* normal exit         */
} /* END eop_text        */

int eow_text() /* End of Wafer        */
/*****************************************************************************/
/* At End of Wafer                                                           */
/*                                                                           */
/* This function is invoked each time at the completion of a wafer.          */
/*                                                                           */
/* Not used but needs to be defined.                                         */
/*****************************************************************************/
{ /* BEGIN FUNCTION      */

    /* This is for the Time & Host name */
    char yeardate[30];
    char time[30];
    char timedate[60]; /* YYYYMMDD HHMMSS */
    char hostname[30];
    /* This is for the Time & Host name */

    int i, j;

    struct timeval tp;
    struct timezone tzp;
    struct tm *tm_p;

    /* To get time format : YYYY/MM/DD HH:MM:SS */
    gettimeofday(&tp, &tzp);
    tm_p = localtime((time_t *)&tp.tv_sec);
    sprintf(yeardate, "%d/%02d/%02d",
            (tm_p->tm_year + 1900), (tm_p->tm_mon + 1), tm_p->tm_mday);

    for (i = 0; i < strlen(yeardate); i++)
        if (yeardate[i] == ' ')
            yeardate[i] = '0';
    yeardate[30] = '\0';
    printf("YYYYMMDD == %s\n", yeardate);

    /* To get FILE time format : HHMMSS */
    sprintf(time, "%2d:%2d:%2d", tm_p->tm_hour, tm_p->tm_min, tm_p->tm_sec);
    for (i = 0; i < strlen(time); i++)
        if (time[i] == ' ')
            time[i] = '0';
    time[30] = '\0';
    printf("HH:MM:SS == %s\n", time);

    strcpy(timedate, yeardate);
    strcat(timedate, " ");
    strcat(timedate, time);
    printf("Result is == %s \n", timedate);

    /* To get the Host Name of Current System */
    j = gethostname(hostname, 30);
    for (i = 0; i < strlen(hostname); i++)
        hostname[i] = toupper(hostname[i]);
    printf("Status == %d hostname == %s\n", j, hostname);

    /* Modified By JI WON SEO of HP-KOREA & Hyun-Jeong LEE */
    sprintf(dir, "/tmp/.ocrMap-0");
    sprintf(outdir, "/tmp/.ocrMap-0.out");
    fp = fopen(dir, "r");
    out = fopen(outdir, "w");
    for (i = 1; i <= 9; i++)
    {
        printf("count = %d\n", i);
        j = fscanf(fp, "%s", ocrmap);
        if (i == 2)
        {
            sprintf(wid, ocrmap);
            printf("ocr_wf_id = %s\n", ocrmap);
        }
    }
    for (k = 0; k <= 12; k++)
    {
        if (k > 0)
        {
            fprintf(out, "%c", wid[k]);
            printf("wid[%d]=%c\n", k, wid[k]);
        }
    }
    fclose(out);
    curr = fopen(outdir, "r");
    for (n = 0; n < 2; n++)
    {
        fscanf(curr, "%s", waf_id);
        printf("n=%d,wf_id=%s\n", n, waf_id);
    }


    /* Copy The End Time */
    strcpy(Wafer_id, waf_id);
    strcpy(StartTime, timedate);
    strcpy(HostName, hostname);

    /* Making the Log File for End of Wafer */
    /* 2001/11/15 17:06:22|AMPT01|WAFERID|S */

    if ((fin = fopen(LogFile, "a")) == NULL)
    {
        printf("Cannot Open the log file (EOW) \n");
    }
    else
    {
        fprintf(fin, "%s|%s|%s|S\n", StartTime, HostName, Wafer_id);
        fprintf(fin, "%s|%s|%s|E\n", EndTime, HostName, Wafer_id);
    }
    fclose(fin);

    if ((fin = fopen(LogFile_SVR2, "a")) == NULL)
    {
        printf("Cannot Open the log file SVR2 (EOW) \n");
    }
    else
    {
        fprintf(fin, "%s|%s|%s|S\n", StartTime, HostName, Wafer_id);
        fprintf(fin, "%s|%s|%s|E\n", EndTime, HostName, Wafer_id);
    }
    fclose(fin);

    if ((fin = fopen(Testlog, "a")) == NULL)
    {
        printf("Cannot Open the log file Testlog (EOW) \n");
    }
    else
    {
        if (pgm_len < 4)
        {
            fprintf(fin, "%s|%s|%s|%s|S|%s|%s\n", StartTime, HostName, curr_test(), Wafer_id, Input_lot_id, Pc_id);
            fprintf(fin, "%s|%s|%s|%s|E|%s|%s\n", EndTime, HostName, curr_test(), Wafer_id, Input_lot_id, Pc_id);
        }
        else
        {
            fprintf(fin, "%s|%s|%s|%s|S|%s|%s\n", StartTime, HostName, curr_cassette(), Wafer_id, Input_lot_id, Pc_id);
            fprintf(fin, "%s|%s|%s|%s|E|%s|%s\n", EndTime, HostName, curr_cassette(), Wafer_id, Input_lot_id, Pc_id);
        }
    }
    fclose(fin);

    /* Add by 2019.08.22 */
    if (!formatter_abort)
    {
        fflush(output_file_new);
        fclose(output_file_new);
    }

    /* Add by 2019.08.22 */
    eow_text_new();
    bow_status_write(Wafer_id, "end");

    return (0); /* normal exit         */
} /* END eow_text        */

int eow_text_new()
{
    char targetPath[100];
}

int eot_text() /* End ot Test         */
/*****************************************************************************/
/* At End of Test Function                                                   */
/*                                                                           */
/* This function is invoked at the completion of a test.                     */
/*****************************************************************************/
{ /* BEGIN FUNCTION       */
    if (!formatter_abort)
    {
        fclose(output_file);
    }

    /* Add by 2019.08.22 */
    bow_status_write("", "");

    return (0);
} /* END eot_text         */

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

struct format_func_struct daff_text[] = {
    bot_text, /* begin of test func  */
    bow_text, /* begin of wafer func */
    bop_text, /* begin of pair func  */
    bod_text, /* begin of die func   */
    eod_text, /* end of die func     */
    eop_text, /* end of pair func    */
    eow_text, /* end of wafer func   */
    eot_text  /* end of test func    */
};
