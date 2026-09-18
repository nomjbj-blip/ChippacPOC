/* @(#) $Revision 1.50 $ IC-MS rep_simple.c example user report"             */
/* (c) Copyright 1991-1995, Hewlett-Packard Company, all rights reserved.    */
/*****************************************************************************/

#include <stdio.h>
#include "accesslib.h"

int
simple_report(fp)
FILE *fp;
{
   int i;  /* counter for number of results */
   int num_results;
   double *datap; /* for accessing data array's array of doubles */
   int size;      /* for accessing size of data array */
   int j;         /* counter for array access */
   char *namep;   /* temporary pointer to result name */

   fprintf(fp,"\nResult Values Report\n\n");
   fprintf(fp," Report for Wafer <%s>\n", curr_wafer());
   fprintf(fp," ID <%s> Wafer Test <%s> \n", curr_waferid(), curr_test());
   fprintf(fp," Total Die   = %d\n", total_die());
   fprintf(fp," Current Die = <%s>\n\n", curr_die());

   fprintf(fp," %-20s %-15s  %s\n", "Result Name", "Value", "Description");
   num_results = number_of_results();
   for (i = 0; i < num_results; i++)
    if (get_report_flag(i))
    {
      switch(get_data_type(i)) {
         case(1):  /* REAL */
            fprintf(fp," %-20s %-15lg  %s\n", get_data_name(i),
                    get_double_data(i), get_data_desc(i));
            break;
         case(2):  /* INT */
            fprintf(fp," %-20s %-15ld  %s\n", get_data_name(i),
                    get_integer_data(i), get_data_desc(i));
            break;
         case(3):  /* STRING */
            fprintf(fp," %-20s %-15s  %s\n", get_data_name(i),
                    get_string_data(i, &size), get_data_desc(i));
            break;
         case(4):  /* ARRAY */
            size = get_array_size(i);
            namep = get_data_name(i);
            fprintf(fp," %-20s size[%d]         %s\n", namep, size,
                    get_data_desc(i));
            for (j = 0; j < size; j++)
               fprintf(fp,"  [%d] %-15s %lg\n", j, namep, 
                       get_array_data(i,j));
            break;
         case(5):  /* CHAR */
            fprintf(fp," %-20s %-15c  %s\n", get_data_name(i),
                    get_char_data(i), get_data_desc(i));
            break;
         default:
            break;
      } /* end of switch */
    }
   fprintf(fp,"\nEnd of Result Values Report\n\n");
   return(0);
} 

