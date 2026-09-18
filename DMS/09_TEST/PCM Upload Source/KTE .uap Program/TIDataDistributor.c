/* USRLIB MODULE INFORMATION

	MODULE NAME: TIDataDistributor
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <string.h>
#include "ktxe_types.h"
#include "COM_usrlib.h"
	END USRLIB MODULE INFORMATION
*/
/* USRLIB MODULE HELP DESCRIPTION

	END USRLIB MODULE HELP DESCRIPTION */
/* USRLIB MODULE PARAMETER LIST */
#include <stdio.h>
#include <string.h>
#include "ktxe_types.h"
#include "COM_usrlib.h"

void TIDataDistributor(  )
{
/* USRLIB MODULE CODE */
#define LIMITHASHED
#define TILONGVARLENGTH 80

FILE *raw_fd, *tw_wfd, *tw_rfd, *auto_wfd, *auto_rfd;
result_list_t *result_list = NULL;
LIMIT *limit_list = NULL;
LIMIT *tmp_limit_list;
SITE *site = NULL;
wwp_list_t *current_wwp_list = NULL;
char c;
char tmp_result_id[ TILONGVARLENGTH ];
char subsite_id[SS_ID_LENGTH];

#ifdef LIMITHASHED
LIMIT **limithashtab;
long hashloc = NULL;
limithashtab = ( LIMIT **) dpGetPointer( "limithashtab", LONG_P );
#endif

limit_list = ( LIMIT * ) dpGetPointer( "limit_list", LONG_P );
result_list = ( result_list_t * ) dpGetPointer( "result_list", LONG_P );
current_wwp_list = ( wwp_list_t * ) dpGetPointer( "current_wwp_list", LONG_P );

tmp_limit_list = limit_list ;

/* Get file pointers from the data pool */
raw_fd = ( FILE *) dpGetPointer( "TI_raw_fd", LONG_P );

tw_rfd = (FILE * ) dpGetPointer( "TI_tw_rfd", LONG_P );
tw_wfd = (FILE * ) dpGetPointer( "TI_tw_wfd", LONG_P );

auto_rfd = (FILE * ) dpGetPointer( "TI_auto_rfd", LONG_P );
auto_wfd = (FILE * ) dpGetPointer( "TI_auto_wfd", LONG_P );

site = (SITE *) dpGetPointer("site", LONG_P );

strcpy( subsite_id, current_wwp_list->ssid );

while(result_list != NULL)
{
    if( result_list->log == TRUE )
    {
         /* generate long variable name */
         /*strcpy( tmp_result_id, current_wwp_list->ssid );
         strcat( tmp_result_id, "x" );
         strcat( tmp_result_id, result_list->id );*/  
         /* above lines removed 071800 by M.Chao, now ktm result id must be the same as limit id */
         strcpy( tmp_result_id, result_list->id );


         

#ifdef LIMITHASHED
         hashloc = (long )hash( tmp_result_id, HASHSIZE ); 

         limit_list =  (LIMIT *)*(limithashtab + ( hashloc ) ) ; 
        c = *tmp_result_id;
#else
         limit_list = tmp_limit_list;
#endif

  /*jsm 061898 */

/* jwp
if (    limit_list == NULL)TIRAWPutResult( raw_fd,tmp_result_id, result_list->value, "KLF_ERROR" );                             
*/

         while(limit_list != NULL) {
#ifdef LIMITHASHED
                           if ( ( c == *limit_list->id )&&
                                    (strcmp( tmp_result_id,limit_list->id ) == 0) )
                           {
#else
                           if ((strcmp( tmp_result_id, limit_list->id ) == 0) ) 
                           {
#endif

                                    /* determine output locations */
                                    /* test for raw data output, ALWAYS LOG */

/* jwp 
                                    if ( TRUE && ( raw_fd != NULL ) )  
                                    {
                                             TIRAWPutResult( raw_fd, 
                                                      tmp_result_id, result_list->value, limit_list->name );
                                    }
*/
              
                                    /* test for operator output */
                                    if ( limit_list->critical !=0  )
                                    {
                                             TIOprPutResult( tmp_result_id, result_list->value );
                                    }

                                    /* test for tw (Oracle) output */
                                    if ( (strstr( limit_list->category, "t" ) != NULL ) && ( tw_rfd != NULL ) && ( tw_wfd != NULL ) )
                                    {
                                             TITWPutResult( tw_wfd, tw_rfd, site->id, 
                                                      limit_list->name, result_list->value );
                                    }

                                    /* test for mainframe output */

#ifdef READPIPE
                                    if ( (strstr( limit_list->category, "a" ) != NULL ) && ( auto_rfd != NULL ) && ( auto_wfd != NULL ) )
#else        
                           if ( (strstr( limit_list->category, "a" ) != NULL ) && ( auto_wfd != NULL ) )
#endif
                                    {
                                             TIAUTOPutResult( limit_list->name, result_list->value );
                                    }
                                break; 

          
                           }
#ifdef LIMITHASHED
                           limit_list = limit_list->nexth;
#else
                           limit_list = limit_list->next;
#endif
         }
/* jwp 
         fflush( raw_fd );
*/
    }
    result_list = result_list->next;
}
/* USRLIB MODULE END  */
} 		/* End TIDataDistributor.c */

