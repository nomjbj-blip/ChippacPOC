/* USRLIB MODULE INFORMATION

	MODULE NAME: ti_data_process_com
	MODULE RETURN TYPE: void 
	NUMBER OF PARMS: 0
	ARGUMENTS:
	INCLUDES:
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#include "kdf.h"
#include <ktxe_types.h>
#include "COM_usrlib.h"
#include <sys/types.h>
#include <sys/stat.h>
#include <sys/errno.h>
#include <unistd.h>
#include <macros.h>
	END USRLIB MODULE INFORMATION
*/
/* USRLIB MODULE HELP DESCRIPTION

	END USRLIB MODULE HELP DESCRIPTION */
/* USRLIB MODULE PARAMETER LIST */
#include <stdio.h>
#include <lptdef.h>
#include <lptdef_lowercase.h>
#include <math.h>
#include <par_util.h>
#include "kdf.h"
#include <ktxe_types.h>
#include "COM_usrlib.h"
#include <sys/types.h>
#include <sys/stat.h>
#include <sys/errno.h>
#include <unistd.h>
#include <macros.h>

void ti_data_process_com(  )
{
/* USRLIB MODULE CODE */
FILE *iv_fp_data ,*iv_fp_tmp;
char empty[1]="\0";
static char empty2[1024]=" ";
char rdval[1024]=" ";
char rdval2[1024]=" ";
char wrtlinetmp[1024]; 
char wfrnum[2]=" ";
static int firstpass=0;  /* first pass flag */
static int maxarysize=0; /* max array size */
static int asize=0;      /* present array size */
static int line_len=0;   /* present line length */
static int maxary=2000; 
static int maxline=1024; 
static char wrtline[2000][1024]; 
static char init_line[1024]=" ";
static char ary_size[1024]=" ";
static char dummy_line[1024]=" ";
int mode=0,i=0,j=0,x=0,wfrcnt=0;
int lncnt=0;
char *path;
char *lotid;
char tmp_str[80];
char out_path[80];
char ti_datafilename[256];
char ti_tmpfilename[256];
long *tmp ;
long *tmp2 ;
slot_list_t *slot ; 
cpf_info_t *cpf_info ;
wwp_list_t *wwp ;
WAFER *wafer ;
LIMIT *limit_list ;
int stat=0;
char cpf_path[ 256 ] ;
char cpf_name[ 256 ] ;
char cpf_ext[ 256 ] ;
LOT *lot = (LOT *)dpGetPointer( "lot", LONG_P ) ;
LIMITCODE *limitcodestruct = (LIMITCODE *)malloc( sizeof( LIMITCODE ) ) ;

/*Get lot  name */
lot = ( LOT * )dpGetPointer( "lot", LONG_P ) ; 

/*Get site x,y loc */
wwp = (wwp_list_t*)dpGetPointer( "current_wwp_list", LONG_P ) ;

/*cpf name */
cpf_info = (cpf_info_t*)dpGetPointer( "cpf_info", LONG_P ) ;

/* separate file name from path and ext  */
GetPathFileExt( cpf_info->cpfname, cpf_path, cpf_name, cpf_ext );

/*slot number/ wafer number */
tmp = (long*)dpGetPointer( "current_slot_list", LONG_P ) ;
slot = (slot_list_t*) *tmp ;

/* init strings */
for(x=0; x<maxline; x++) {
      if(x==(maxline-1))wrtlinetmp[x]='\0';
      else wrtlinetmp[x]=' ';
}

strcpy(&empty2[0],&wrtlinetmp[0]);

for(x=0; x<maxary; x++) {
     strcpy(&wrtline[x][0],wrtlinetmp);
}


path = (char*)getenv( "TI_ARRAY_DATA_PATH" ); 
strcpy( out_path, path );
strcat( out_path, "/" ); 
strcat( out_path, cpf_name ); 
strcat( out_path, "/" ); 
strcat( out_path, lot); 

/* check to see if data directory is mounted */
if (access(out_path,X_OK)<0) {
  /* create output path for file */
  strcpy( out_path, path );
  strcat( out_path, "/" ); 
  strcat( out_path, cpf_name ); 
  strcpy( out_path, path );
  strcat( out_path, cpf_name ); 
  if (access(out_path,X_OK)<0) {
     mode =S_IREAD+S_IWRITE+S_IEXEC+S_IRGRP+S_IWGRP+S_IXGRP+S_IROTH+S_IXOTH+S_IWOTH;
     stat=mkdir(out_path,mode);
     /*perror("TI_ARRAY_DATA_PATH/cpf_name mkdir:"); */
     chmod(out_path,mode);
    }
 strcat( out_path, "/" );
 strcat( out_path, lot);
 if (access(out_path,X_OK)<0) {
     mode =S_IREAD+S_IWRITE+S_IEXEC+S_IRGRP+S_IWGRP+S_IXGRP+S_IROTH+S_IXOTH+S_IWOTH;
  /* mode =S_IREAD+S_IWRITE+S_IEXEC+S_IRGRP+S_IWGRP+S_IXGRP+S_IROTH+S_IXOTH; */
    stat=mkdir(out_path,mode);
    /*perror("TI_ARRAY_DATA_PATH/cpf_name/lot  mkdir:"); */
    chmod(out_path,mode);
  }
  strcat( out_path, "/" ); 
}else strcat( out_path, "/" ); 
 /* end dir mounted check */

/***************************************************/
/* create putput filename strings                                       */
/* TI_ARRAY_DATA_PATH= /user/ktest/kdata/iv_data/    */
/*example   datafile = iv.<prog name>.<lot id>.<wafer id> */
/***************************************************/
path = (char*)getenv( "TI_ARRAY_DATA_PATH" ); 
strcpy( out_path, path );
strcat( out_path, "/" ); 
strcat( out_path, cpf_name ); 
strcat( out_path, "/" ); 
strcat( out_path, lot); 
strcat( out_path, "/" ); 

strcpy(ti_datafilename,out_path);
strcat( ti_datafilename, "iv.");
strcat( ti_datafilename, cpf_name);
strcat( ti_datafilename, ".");
strcat( ti_datafilename,lot);
strcat( ti_datafilename, ".");
strcat( ti_datafilename, slot);

strcpy(ti_tmpfilename,out_path);
strcat( ti_tmpfilename, "iv.");
strcat( ti_tmpfilename, cpf_name);
strcat( ti_tmpfilename, ".");
strcat( ti_tmpfilename, lot);
strcat( ti_tmpfilename, ".tmp");
/*
strcat( ti_tmpfilename, slot);
strcat( ti_tmpfilename, ".tmp");
*/

/*
if ((iv_fp_tmp=fopen(ti_tmpfilename,"w"))==NULL) goto file_error;
fclose(iv_fp_tmp);
*/

if ((iv_fp_data=fopen(ti_datafilename,"r"))==NULL) goto file_error;

if ((iv_fp_tmp=fopen(ti_tmpfilename,"w"))==NULL) {
      fclose(iv_fp_data);
      goto file_error;
 }

fclose(iv_fp_tmp);

   lncnt=-1;
   j=0;
   firstpass=0;  /* first pass flag */
   maxarysize=0; /* max array size */
   asize=0;      /* present array size */
   line_len=0;   /* present line length */

 while(!feof(iv_fp_data)){

  strcpy(&rdval[0],&empty[0]);  /* int string */

  fgets(&rdval[0],1024,iv_fp_data); /* get next line */
  line_len=strlen( &rdval[0]); /* read line length */

  if (line_len == 0 ) {  /* end of file write last site lines*/
    if ((iv_fp_tmp=fopen(ti_tmpfilename,"a"))==NULL) goto file_error;
/* printf("last site maxarysize=%d line_len=%d  firstpass=%d rdval=%s \n\n",maxarysize,line_len,firstpass,&rdval[0]); */
   for(x=0; x<=(maxarysize-1); x++) {
          /*printf("x=%d   %s\n",x,&wrtline[x][0]);  */
          fprintf(iv_fp_tmp,"%s\n",&wrtline[x][0]); 
        } /* end for */
    fclose(iv_fp_tmp);
    /* last pass not needed */
   goto lpend; /* end of file */
  /* end if line_len */ 
 }  /* end line_len==0*/


if(rdval[0] == '$') { /* init var for new site arrays */
    if (firstpass !=0) { 
       /* end of site write last site lines*/
       if ((iv_fp_tmp=fopen(ti_tmpfilename,"a"))==NULL) goto file_error;
/* printf("new site  line_len=%d  firstpass=%d rdval=%s \n\n",line_len,firstpass,&rdval[0]);   */
       for(x=0; x<=(maxarysize-1); x++) {
         /* printf("x=%d maxarysize=%d   %s\n",x,maxarysize,&wrtline[x][0]);  */
          fprintf(iv_fp_tmp,"%s\n",&wrtline[x][0]); 
       } /* end for */
       fclose(iv_fp_tmp);
   }  /* end if not firstpass  */

  maxarysize=0;
  lncnt=-1; 
  firstpass=1; /* flag to show if first 'S' in wafer file*/
  asize=0;      /* present array size */
  line_len=0;   /* present line length */

  j=0;
  while (rdval[j+1] != '\n') {
      /*create init "base" line out */
      init_line[j]=rdval[j+1];
      j++;
  }  /* end while '\n'  */      
  init_line[j+1]='\0';
  x=0;
/* printf("maxary=%d\n",maxary); */
  for(x=0; x<=maxary; x++) {
        /* fill array with init line for die site */
        strcpy(&wrtline[x][0],&init_line[0]);
  }/* end for x */ 
  goto lpend;
} /* end '$' */

if(rdval[0] == '#') {/* get array size */
    asize=0;
    lncnt=-1; 
    j=0;
    strcpy(&ary_size[0],&empty2[0]);  /* int string */
    while (rdval[j+1] != '\n') {
      ary_size[j]=rdval[j+1];
/*     printf("# char ary_sizej=%d  [%c] \n",j,ary_size[j]);   */
      j++;
    }  /* end while '\n'  */      
    ary_size[j+1]='\0';
/*     printf("\n  asixe=%d  maxarysize=%d  # string ary_size=%s\n",asize,maxarysize, &ary_size);  */
    asize=atoi(ary_size); /* array size as read */
    if(asize > maxarysize) maxarysize=asize; /* max ary size for run */
/*printf("# end  asize=%d  max_ary_size=%d \n",asize,maxarysize);  */
  goto lpend;
 }   /* end if # */

    
  lncnt++; /* last line cnt of data zero=first */
   
  if(lncnt==0) { /* create dummy line of spaces length of line */
    for(j=0; j<=(line_len-1); j++) {
      dummy_line[j]=' ';
    }  /* end for j */      
    dummy_line[j+1]='\0';
/* printf(" line length=%d  dummy line=%s$$$$$$ \n",line_len, dummy_line); */
  } /* end if lncnt==0 */

  if(lncnt <= (asize-1)) { /* add real data line */
   strncat(&wrtline[lncnt][0],&rdval[0],line_len-1); 
  } 

/* if(lncnt== (asize-1)) printf("asize reached asize=%d maxarysize=%d\n",asize,maxarysize); */

 if(lncnt == (asize-1))  {
    for(x=asize; x<maxary; x++) { /*last ary line, add dummy lines of spaces */  
     strncat(&wrtline[x][0],&dummy_line[0],line_len-1); 
    } /* end for x */ 
 } /* if lncnt=(asize-1)     */
  


lpend:
j=j; /* dummy command */
} /* end while feof*/
fclose(iv_fp_data);


if ((iv_fp_data=fopen(ti_datafilename,"w"))==NULL) goto file_error;
if ((iv_fp_tmp=fopen(ti_tmpfilename,"r"))==NULL) goto file_error;
while(!feof(iv_fp_tmp)){/* write tmp file to data file */
 strcpy(&rdval[0],&empty[0]);  /* int string */
  fgets(&rdval[0],1024,iv_fp_tmp);
  i=strlen( &rdval[0]); 
  if(i==0) {
   /* last pass not needed */
  break; 
  }
   fputs(&rdval[0],iv_fp_data);
  } /* end while */
  fclose(iv_fp_data);
  fclose(iv_fp_tmp);

if ((iv_fp_tmp=fopen(ti_tmpfilename,"w"))==NULL) goto file_error;
  fclose(iv_fp_tmp);

sprintf(tmp_str,"rm -f  %s",ti_tmpfilename); /* rm tmp file */
system(tmp_str); 

return;

file_error:
/* error exit */
return;
/* USRLIB MODULE END  */
} 		/* End ti_data_process_com.c */

