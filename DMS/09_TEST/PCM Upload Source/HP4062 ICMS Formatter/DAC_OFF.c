/*****************************************************************************/
/* FILE: DAC_ON.c
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
static char sccs_id[] = "@(#)DAC_ON.c	1.1	08/05/95";

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
FILE *dac_output_file;
FILE *fin;
static int formatter_abort = 0; /* Flag to abort formatter on error */
static int pgm_len = 0;         /* Flag to abort formatter on error */
static char Lot_id[10];
static char Wafer_id[20];
static char Pc_id[20];
static char Time[30];
static char HostName[30];
static char cassette_test[10];
static char wafer_FlatZone[5];
static char filename[20];

/*****************************************************************************/
/* user function definition
/*****************************************************************************/
int dac_FlatZone_OFF();
int dac_status_write_off(char *status, char *lotid, char *waferid, char *pgm_name);


int bot_dac_off_text() /* Beginning of Test  */
/*****************************************************************************/
/* At Beginning of Test Function                                             */
/*                                                                           */
/* This function is invoked only at the beginning of each test.              */
/*****************************************************************************/
{ /* BEGIN FUNCTION     */
    char lot_number[9], hostname[30];
    int i, j;
    /**************************************************************************/
    /* Build output filename */
    /**************************************************************************/
    strncpy(lot_number, get_lot_id(), 8);
    lot_number[8] = '\0';
    for (i = 0; i < 8; i++)
        if (lot_number[i] == ' ')
            lot_number[i] = '_'; /* Replace blank spaces with underscores*/

            
    j = gethostname(hostname, 30);
    for(i = 0; i <strlen(hostname); i++)
        hostname[i] = toupper(hostname[i]);
    
    strcpy(Lot_id, lot_number);
    strcpy(HostName, hostname);

    return (0); /* normal exit        */
} /* END bot_text       */

int bow_dac_off_text() /* Begin of Wafer     */
/*****************************************************************************/
/* At Beginning of New Wafer                                                 */
/*                                                                           */
/* This function is invoked at the beginning of testing each wafer.          */
/*****************************************************************************/
{ /* BEGIN FUNCTION     */
    char out_string[300];
    char waf_id[20];
    char tfile[50]; /* Filename for output text file */
    int i, j, k, n, u, pc_id_len;
    char pcid[20];
    char hostname[30];
    char HostName[30];
    char min[3], sec[3], op_name[30];

    char dir[50], outdir[50], ocrmap[50], wid[20];
    FILE *fp, *out, *curr;

    time_t tnum;
    struct tm *ts;
    time(&tnum);
    ts = localtime(&tnum);
    sprintf(min, "%d", ts->tm_min);
    sprintf(sec, "%d", ts->tm_sec);

    formatter_abort = 0;

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

    fclose(fp);
    fclose(curr);
    strcpy(Wafer_id, waf_id);

    /* create new filename. modify date 2019.09.10 */
      j = gethostname(hostname, 30);
      for (u = 0; u < strlen(hostname); u++)
      hostname[u] = toupper(hostname[u]);
      strcpy(HostName, hostname);

    printf("hostname %s\n", hostname);
    if (strcmp(HostName,"AMPTSVR") == 0)
    {
    sprintf(filename, "%s_%s_%s%s_%s", Lot_id, Wafer_id, min, sec, HostName);
    sprintf(tfile, "/users/tye/OFF_FORMAT/%s", filename);
    }
    else
    {
    sprintf(filename, "%s_%s_%s%s_%s", Lot_id, Wafer_id, min, sec, HostName);
    sprintf(tfile, "/home/tye/OFF_FORMAT/%s", filename);
    }

    printf("Daff_text Output file Name == #%s#\n", tfile);
    /**************************************************************************/
    /* Open output file */
    /**************************************************************************/
    if ((dac_output_file = fopen(tfile, "w")) == NULL)
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
	fprintf(dac_output_file, "Logging Raw Data from Test  <%s>   ", curr_test());
        strcpy(cassette_test, curr_test());
    }
    else
    {
    fprintf(dac_output_file, "Logging Raw Data from Test  <%s>   ", cassette_test);
    }
    fprintf(dac_output_file, "Executing on: %s\n\n", present_time());
    fprintf(dac_output_file, "-------------------------------------\n");

    dac_status_write_off("OFFLINE", Lot_id, Wafer_id, cassette_test);
    /* Modified by HP-Korea A.E. Ji-Won Seo for Operator NAME */
    k = number_of_udcs();
    for (j = 0; j < k; j++)
    {
        if (j == 0)
        {
            printf("input_lot_id=%s\n", get_udc_ascii_data(j));
        }
        if (j == 1)
        {
            strcpy(op_name, get_udc_ascii_data(j));
            fprintf(dac_output_file, "Operator Name is : %s\n", op_name);
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
    fprintf(dac_output_file, "-------------------------------------\n");
    fprintf(dac_output_file, "Current Wafer = %s\n", waf_id);
    fprintf(dac_output_file, "Tester =  %s\n\n", HostName);

    fflush(dac_output_file);

    return (0); /* normal exit         */
} /* END bow_text        */

int bop_dac_off_text() /* Begin of Test Pair */
/*****************************************************************************/
/* At Beginning of Test Definition Pair                                      */
/*                                                                           */
/* Not used but needs to be defined.                                         */
/*****************************************************************************/
{               /* BEGIN FUNCTION     */
    return (0); /* normal exit        */
} /* END bop_text       */

int bod_dac_off_text() /* Begin of Die       */
/*****************************************************************************/
/* At Beginning of Die                                                       */
/*                                                                           */
/* This function is invoked at the beginning of testing each die.            */
/*                                                                           */
/*****************************************************************************/
{ /* BEGIN FUNCTION      */
    /* Add by 2019.08.30 */
    dac_FlatZone_OFF();

    /* Add by 2019.08.22 */
    fprintf(dac_output_file, "-------------------------------------\n");
    fprintf(dac_output_file, "Current Die = %s (%d, %d),   Current Die Type = %s\n",
            curr_die(), curr_die_Xloc(), curr_die_Yloc(), curr_die_type());
    fprintf(dac_output_file, "Flat Zone = %s\n\n",
            wafer_FlatZone);

    return (0); /* normal exit         */
} /* END bod_text        */

int eod_dac_off_text() /* End of Die          */
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

            fprintf(dac_output_file, "%s\t%s\t%s\n", get_data_name(i),
                    data_val, get_data_desc(i));
        }
    }             /* ENDIF get_database  */
                  /* ENDFOR              */
    return (err); /* normal exit         */
} /* END eod_text        */

int eop_dac_off_text() /* End of Die Pair     */
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

int eow_dac_off_text() /* End of Wafer        */
/*****************************************************************************/
/* At End of Wafer                                                           */
/*                                                                           */
/* This function is invoked each time at the completion of a wafer.          */
/*                                                                           */
/* Not used but needs to be defined.                                         */
/*****************************************************************************/
{ /* BEGIN FUNCTION      */
    char script[120];
    /* Add by 2019.08.22 */
    fprintf(dac_output_file, "\n End Time : %s ", present_time());
    if (!formatter_abort)
    {
        fflush(dac_output_file);
        fclose(dac_output_file);
    }

    dac_status_write_off("WAFER_END", Lot_id, Wafer_id, cassette_test);
    return (0); /* normal exit         */
} /* END eow_text        */

int eot_dac_off_text() /* End ot Test         */
/*****************************************************************************/
/* At End of Test Function                                                   */
/*                                                                           */
/* This function is invoked at the completion of a test.                     */
/*****************************************************************************/
{ /* BEGIN FUNCTION       */
    /* Add by 2019.08.22 */
    /* bow_status_write("", ""); */
    return (0);
} /* END eot_text         */

int dac_FlatZone_OFF()
/*****************************************************************************/
/* At Beginning of Test New Function                                         */
/*                                                                           */
/* This function is Wafer FlatZone information                               */
/* Add by 2019.08.22                                                         */
/*****************************************************************************/
{ /* BEGIN FUNCTION     */
    FILE *fp = NULL, *pgmFile = NULL, *flatFile = NULL;
    char buffer[256], *result, *pStr, *text;
    char out_string[300];
    char command[512];
    char pgmFilePath[128], flatFilePath[128];
    char hostname[30];
    char HostName[30];
    int idx = 0;
    int i, j;

    j = gethostname(hostname, 30);
    for (i = 0; i < strlen(hostname); i++)
        hostname[i] = toupper(hostname[i]);
    strcpy(HostName, hostname);


    /* Checked Cassette File*/
    if(strcmp(HostName, "AMPTSVR") == 0)
    {
        sprintf(command, "awk '{if(NR == 23) print $2;}' /users/tye/CASSETTE/%s", cassette_test);
    }
    else
    {
        sprintf(command, "awk '{if(NR == 23) print $2;}' /home/tye/CASSETTE/%s", cassette_test);
    }

    if((fp = popen(command, "r")) == NULL)
    {
        sprintf(out_string, "ERROR: Cannot open file %s\n\n", command);
        strcat(out_string, "\nClick OK to abort\n");
        pop_error_box(out_string);
        formatter_abort = 0;
        return (0);
    }

    while(fgets(buffer, 256, fp) != NULL)
    {
        text=buffer;
    }
    
    pclose(fp);

    for (idx = strlen(text); idx > 0; idx--)
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
    
    /* find to wafer type */
    if (strcmp(HostName, "AMPTSVR") == 0)
    {
        sprintf(pgmFilePath, "/users/tye/WAFERTEST/%s", text);
    }
    else
    {
        sprintf(pgmFilePath, "/home/tye/WAFERTEST/%s", text);
    }
    
    if ((pgmFile = fopen(pgmFilePath, "r")) == NULL)
    {
        sprintf(out_string, "ERROR: Cannot open file %s\n\n", pgmFilePath);
        strcat(out_string, "\nClick OK to abort\n");
        pop_error_box(out_string);
        formatter_abort = 0;
        return (0);
    }

    while (!feof(pgmFile))
    {
        result = fgets(buffer, sizeof(buffer), pgmFile);
        result = (char *)strstr(buffer, "WaferType:");
        if (result != NULL)
        {
            pStr = (char *)strtok(buffer, " ");
            while (pStr != NULL)
            {
                text = pStr;
                pStr = (char *)strtok(NULL, " ");
            }

            break;
        }
    }

    fclose(pgmFile);

    /* delete of new line character */
    /* 개행문자가 존재할 경우 정상적으로 파일을 open 하지 못함 */
    for (idx = strlen(text); idx > 0; idx--)
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
    if (strcmp(HostName, "AMPTSVR") == 0)
    {
        sprintf(flatFilePath, "/users/tye/WAFER/%s", text);
    }
    else
    {
        sprintf(flatFilePath, "/home/tye/WAFER/%s", text);
    }
    if ((flatFile = fopen(flatFilePath, "r")) == NULL)
    {
        sprintf(out_string, "ERROR: Cannot open file %s\n\n", flatFilePath);
        strcat(out_string, "\nClick OK to abort\n");
        pop_error_box(out_string);
        formatter_abort = 0;
        return (0);
    }

    while (!feof(flatFile))
    {
        result = fgets(buffer, sizeof(buffer), flatFile);
        result = (char *)strstr(buffer, "Flat");
        if (result != NULL)
        {
            pStr = (char *)strtok(buffer, " ");
            while (pStr != NULL)
            {
                text = pStr;
                pStr = (char *)strtok(NULL, " ");
            }

            break;
        }
    }

    fclose(flatFile);

    /* delete of new line character  */
    for (idx = strlen(text); idx > 0; idx--)
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

    printf("Flat Zone: %s\n", text);
    strcpy(wafer_FlatZone, text);

    return (0);
} /* END bow_text new       */

int dac_status_write_off(char *status, char *lotid, char *waferid, char *pgm_name)
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

    sprintf(statusfilename, "/users/ap_data/EQUIP/TEST/PCM/STATUS/%s", HostName);
    if ((status_fp = fopen(statusfilename, "w")) == NULL)
    {        
        printf("ERROR: Cannot open output file %s\n\n", statusfilename);
        formatter_abort = 0;
        return (-1);
    }

    fprintf(status_fp, "%s,%s,%s,%s", status, lotid, waferid, pgm_name);
    fflush(status_fp);
    fclose(status_fp);

    return (0); /* normal exit        */
} /* END bow_text new       */

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

struct format_func_struct DAC_OFF[] = {
    bot_dac_off_text, /* begin of test func  */
    bow_dac_off_text, /* begin of wafer func */
    bop_dac_off_text, /* begin of pair func  */
    bod_dac_off_text, /* begin of die func   */
    eod_dac_off_text, /* end of die func     */
    eop_dac_off_text, /* end of pair func    */
    eow_dac_off_text, /* end of wafer func   */
    eot_dac_off_text  /* end of test func    */
};
