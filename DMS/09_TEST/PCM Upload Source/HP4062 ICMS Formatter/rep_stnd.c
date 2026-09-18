/* @(#) $Revision 1.50 $ IC-MS rep_stnd.c example user report"               */
/* (c) Copyright 1991-1995, Hewlett-Packard Company, all rights reserved.    */
/*****************************************************************************/

#include <stdio.h>
#include "accesslib.h"

int
standard_report(fp)
FILE *fp;
{
   int i;  /* counter for number of results */
   const char *temp;
   int num_results;
   int num_of_udcs;
   double *datap; /* for accessing data array's array of doubles */
   int size;      /* for accessing size of data array */
   int j;         /* counter for array access */
   char *namep;   /* temporary pointer to result name */


   fprintf (fp,"\n===============================================================================\n");
   fprintf(fp," Wafer Summary for Test   <%s>          Test Revision: %s\n", curr_test(), curr_testrev());
   fprintf (fp,"===============================================================================\n\n");
   fprintf(fp," Report for Wafer Type <%s>\n\n", curr_wafer());
   if ((i = get_udc_index ("Lot_ID")) >= 0)
      temp = get_udc_string_data (i, &size);
   else
      temp = "*** Lot_ID not found in User Constants ***";
   fprintf(fp," Lot ID:       %s\n", temp );
   fprintf(fp," Wafer ID:     %s\n", curr_waferid());
   if ((i = get_udc_index ("Comment")) >= 0)
      temp = get_udc_string_data (i, &size);
   else
      temp = "*** Comment not found in User Constants ***";
   fprintf(fp," Comment:      %s\n", temp);
   if ((i = get_udc_index ("Operator")) >= 0)
      temp = get_udc_string_data (i, &size);
   else
      temp = "*** Operator not found in User Constants ***";
   fprintf(fp," Operator:     %s\n", temp);
   fprintf(fp," Test System:  %s\n", curr_testsys());
   fprintf(fp," Total Die:    %d\n", total_die());
   fprintf(fp," Data Format:  %s\n", format(1));
   fprintf(fp," Test Start:   %s\n\n", test_start_time());

   fprintf (fp,"\n===============================================================================\n");
   fprintf (fp, " User Defined Constants\n\n");
   fprintf (fp, " Name               Value       Description\n");
   fprintf (fp, "--------------- --------------- ----------------------------------------------\n\n");
   
   num_of_udcs = number_of_udcs();
   for (i = 0; i < num_of_udcs; i++)
   {
      switch(get_udc_type(i)) {
         case(1):  /* REAL */
            fprintf(fp," %-15s %-15.15lg  %s\n", get_udc_name(i),
                    get_udc_double_data(i), get_udc_desc(i));
            break;
         case(2):  /* INT */
            fprintf(fp," %-15s %-15d  %s\n", get_udc_name(i),
                    get_udc_integer_data(i), get_udc_desc(i));
            break;
         case(3):  /* STRING */
            fprintf(fp," %-15s %-15.15s  %s\n", get_udc_name(i),
                    get_udc_string_data(i, &size), get_udc_desc(i));
            break;
         case(4):  /* ARRAY */
            size = get_udc_array_size(i);
            namep = get_udc_name(i);
            fprintf(fp," %-15s size[%d]         %s\n", namep, size,
                    get_udc_desc(i));
            for (j = 0; j < size; j++)
               fprintf(fp,"  [%d] %-15s %lg\n", j, namep, 
                        get_udc_array_data(i,j));
            break;
         case(5):  /* CHAR */
            fprintf(fp," %-15s %-15c  %s\n", get_udc_name(i),
                    get_udc_char_data(i), get_udc_desc(i));
            break;
         default:
            break;
      } /* end of switch */
   }
   fprintf (fp,"\n===============================================================================\n\n");
     

   fprintf(fp,"    %41s  %18s\n", "Tot#", "GOOD"); 
   fprintf(fp,"%-20s %-9s %-9s %4s %s\n", "Result Name", "Mean", "Std Dev", 
                "Dice"," 1    2    3    4    5    6    7");
   fprintf (fp,"-------------------- --------- --------- ---- ---  ---  ---  ---  ---  ---  ---\n");
   num_results = number_of_results();
   for (i = 0; i < num_results; i++)
    if (get_report_flag(i))
    {
      fprintf (fp, "%-20s %-9.2g %-9.2g %4d %3d  %3d  %3d  %3d  %3d  %3d  %3d\n", 
      get_data_name(i), get_mean(i), get_stddev(i), get_die_count(i), 
      get_bin1(i), get_bin2(i), get_bin3(i), get_bin4(i), get_bin5(i),
      get_bin6(i), get_bin7(i));
    }
   fprintf (fp,"\n===============================================================================\n\n");
   fprintf(fp,"\nEnd of Wafer Summary Report\n\n");
   return(0);
} 

