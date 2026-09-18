/*
    
    kicommon.h

    Common header.
    Common library.
    
    Copyright (c) 1995 by Keithley Instruments, Inc. Cleveland, Ohio.
     
    This software is furnished under a license and may be used and copied 
    only in accordance with the terms of such license, and with the 
    inclusion of the above copyright notice.  This software or any other 
    copies hereof may not be provided or otherwise made available to any 
    other person.  No title to and ownership of the software is hereby 
    transferred.  The information in this software is subject to change 
    without notice, and should not be construed as a commitment by 
    Keithley Instruments, Inc.
     
    Keithley assumes no responsibility for the use or reliability of its 
    software on equipment which is not supplied by Keithley.

*/

/*

 $Revision: 1.22 $
     
 Rev $Date: 2000/08/08 13:28:19 $
     
 * $Log: kicommon.h,v $
 * Revision 1.22  2000/08/08 13:28:19  williamson
 * Added function to use groups instead of rel.allow file for privledged users
 *
 * Revision 1.21  2000/07/11 19:13:15  williamson
 * PR12961  Added proto for isUserExcludeLibName()...
 *
 * Revision 1.20  2000/06/13 14:04:32  psuwondo
 * Added prototype for Is_KI_libName();
 *
 * Revision 1.19  2000/04/28 15:38:48  mpomeran
 * validatePassword() [stub] replaced with "real" validateUserOrPwd
 *
 * Revision 1.18  2000/04/26 14:39:07  mpomeran
 * added prototype for validatePassword()
 *
 * Revision 1.17  1999/11/10 20:02:21  rybka
 * removed references to user defined types
 *
 * Revision 1.16  1999/11/10 15:32:58  rybka
 * PR8034 added prototypes for all common functions
 *
 * Revision 1.15  1999/01/15 21:00:48  witzke
 * PR8764 Added ConvertEscControl
 *
 * Revision 1.14  1999/01/12 16:08:18  jlilie
 * Removed #ifdef MSDOS stuff
 *
 * Revision 1.13  1998/09/24 20:14:08  rybka
 * PRs 7892 && 7893 added syntax checks for inputs of int, float and double
 * added prototypes for -> ki_atof, ki_atofd and ki_atofi
 *
 * Revision 1.12  1998/05/21 18:03:13  williamson
 * Removed fnamepub.h include
 *
 * Revision 1.11  1998/05/21 18:00:08  williamson
 * Removed GetEnvdir prototype...
 *
 * Revision 1.10  1997/09/15 13:35:59  aujla
 * Added TERMINATE_PICKER_PATH to NT code
 *
 * Revision 1.9  1997/08/28 20:29:30  williamson
 * Added  PATH_SEPARATOR back..
 *
 * Revision 1.8  1997/08/28 20:18:55  williamson
 * Another typo correction
 *
 * Revision 1.7  1997/08/28 20:14:28  williamson
 * Corrected typos...
 *
 Change Revision 1.6  1997/08/28 17:51:05  aujla
 Change Updated with the NT changes.
 Change
 Change Revision 1.5  1997/03/14 15:14:13  dev
 Change Remove WINMOVERESTORE_FUNC to winmoverestore.h
 Change
 Change Revision 1.4  1997/03/13 22:11:06  dev
 Change Add macro WINMOVERESTORE_FUNC.
 Change
 * Revision 1.3  1995/10/05  18:34:32  rong
 * added kte file header defines for kitt and kcat use now. it can added more
 * for all the kte data file in future.
 *
 * Revision 1.2  1995/08/10  14:12:08  rong
 * added TERMINATE_PICKER_PATH for file picker dialog box
 *
 * 
 *    Rev 1.3   28 Jun 1995 09:46:28   HAYES
 * Removed kisystem.h and put system prototype in kicommon.h
 * 
 *    Rev 1.2   31 May 1995 15:06:50   ROBERTS
 * Change prototype for GetEnvDir().
 * 
 *    Rev 1.1   08 May 1995 17:41:32   HAYES
 * 
 * Add prototype for GetEnvDir.
 * 
 *    Rev 1.0   21 Apr 1995 14:12:30   HAYES
 * Initial revision.
 *
 */

#ifndef _KICOMMON_H
#define _KICOMMON_H

/*
    ERR_LIB is required by the OI include files.  If one is not defined
    by the user it is not a problem to use any old name.  In this case
    we will define it to be the same as what the COMMON library uses.  
    This is just a convenience for the users of this library so he does
    not need to worry about defining ERR_LIB just to use the functions
    declared in this header file.
*/

#ifndef ERR_LIB
#define ERR_LIB KICOMMON
#endif


#ifdef WIN32
#define PATH_SEPARATOR          '\\'
#define PATH_SEPARATOR_STRING   "\\"
#define TERMINATE_PICKER_PATH	1	/* for file picker dialog box */
#else
#define PATH_SEPARATOR		'/'
#define PATH_SEPARATOR_STRING   "/"
#define TERMINATE_PICKER_PATH	1	/* for file picker dialog box */
#endif

/* kte file info */
#define DOT_CHAR			'.'
#define NEWLINE_CHAR		'\n'
#define	TAB_CHAR			'\t'
#define SPACE_CHAR			' '
#define	COMMA_CHAR			','
#define KPFDELIMITER		COMMA_CHAR
#define COMMENT_CHAR		'#'

#define KTE_FILETYPE		"#<FILE>"
#define KTE_VERSION			"#<VERSION>"
#define KTE_TIME			"#<TIME>"
#define KTE_FORMAT			"#<FORMAT>"
#define KTE_LABEL_DELIMITER	">"

/* KCAT file info */
#define KCAT_FILE_EXT		"kpf"			/* file extension */
#define KCAT_FILENAME		"Keithley Results File"	/* for both kitt and kcat */
#define KCAT_FORMAT_CHANGED	"2.5"			/* new format from kte 2.5 release */
#define KCAT_COLUMN_FORMAT	"col"			/* keyword for column format file */
#define KCAT_ROW_FORMAT		"row"			/* keyword for row format file */
#define KCAT_T_LABEL		"#<TITLE>"      /* Title */
#define KCAT_X_LABEL		"#<XLABEL>"     /* X label */
#define KCAT_Y_LABEL		"#<YLABEL>"     /* Y label */
#define KCAT_DELIMITER		"#<DELIMITER>"  /* DELIMITER */
#define KCAT_DATA           "#<DATA>"


/* Defines of group name for lookup.  Used to enable/disable functions
 * based upon users/groups
 */
#define KRMADM_GROUP         "krmadm"
#define PROBECARD_GROUP      "prbcard"


/* Other file comments info can be put here */






/* 
 * function prototypes starts here
 */ 
void RemoveWhiteSpace(char * input_str);
void ConvertEscControl( char *, char * );
#endif

/*S600 Stuff*/
/* COMMON/CnfgUtils.c */
char* GetQMO( char *iniFileName );
char* GetIPAddress( char *iniFileName );
char* GetSystemName( char *iniFileName );

/* COMMON/diagserv.c */
int checkDiagnosticTools();

/* COMMON/diagutil.c */
int sendCmd2Daemon(int tool_id);
char *GetDiagdPipe();
char *GetDiagdPipeClient(int in_pid);
int checkConnection();
int stop_server();
int DIAGD_Init();
void *call_clientpipe_thread(void *none);
int startCommServer();
int DIAGD_Exit();

/* COMMON/ini.c */
int iniCopy(FILE *src_file, char *destfile_name);
int iniEntryDelete (char *SectionName, char *KeyName, char *FileName);
int iniIntGet (char *SectionName, char *KeyName, int DefaultInt, char *FileName);
int iniIntWrite (char *SectionName, char *KeyName, int UpdateInt, char *FileName);
void iniQuotesDelete(char *s);
void iniTmpFileDelete(char *tmp_file_name);
int iniStringGet(char *SectionName, char *KeyName, char *DefaultString, char *ReturnedString, int SizeofReturnedString, char *FileName);
int iniStringWrite (char *SectionName, char *KeyName, char *UpdateString, char *FileName);
int iniSectionDelete (char *SectionName, char *FileName);

/* INTERSOL */
/* COMMON/com_dec.c */
void com_dec();

/* COMMON/getdevid.c */
long getdevid( char *namestr );

/* COMMON/getdevpt.c */
long *getdevptr( long devid, long *cbuffer );

/* COMMON/getstr.c */
int getstr(char *str);

/* COMMON/getword.c */
char *getword(char *instr);

/* COMMON/ins_com.c */
void ins_com();

/* COMMON/kdf_def.c */
void kdf_dec();

/* COMMON/load.c */
int load(char *nambuf, long *cbuffer);

/* COMMON/longswap.c */
void longswap( long *ilbuff, long *olbuff, int size);

/* COMMON/strip_et.c */
void strip_ether(unsigned char *cbuf);

/* COMMON/ translnm.c */
int trans_lnm( char *log_name, char *log_table, char *phys_name);

/* shared between S400 && S600 code */
/* COMMON/getwordprb.c */
char *getwordprb(char *instr, char *outstr);

/* COMMON/hash.c */ 
int hash(char *s, int h);

/* COMMON/COM_GetModuleAddr.c */
int (*COM_GetModuleAddr( char *lib_name, char *module_name ))();

/* COMMON/ConvertEscControl.c */
void ConvertEscControl ( char *instring, char *string );

/* COMMON/FileExist.c */
int FileExist(char name[]);

/* COMMON/GetIniString.c */
int GetIniString (char *component,char *item,char *value,int maxlen,
                  char *defvalue, char *initfile);
int GetIniStringOld(char *component,char *item,char *value,int maxlen,
                    char *defvalue, char *initfile);
int GetIniStringWithoutEnv(char *component,char *item,char *value,int maxlen,
                           char *defvalue, char *initfile);

/* COMMON/GetPathFileExt.c */
void GetPathFileExt( char *fullpath, char *path, char *name, char *ext );

/* COMMON/GetRealFileName.c */
void COM_GetRealFileName( char *infilename, char *filewext, char *filenoext, 
                char *ext);

/* COMMON/atohx.c */
unsigned long atohx(register char *str);

/* COMMON/atoul.c */
unsigned long atoul(register char *str);

/* COMMON/doctree.c */
int untar_docs(char *tarfilename);
int GetLine(FILE *f, char **str);
int RemoveDir(char * directory);

/* COMMON/get_msg_data.c */
int get_msg_data( char* ErrFile, int ErrVal, char *func_name, int NumErr, long *
ErrArray, char* err_str );

/* COMMON/kdferror.c */
void kdferror( int errval, ...);

/* COMMON/ki_atod.c */
int ki_atod(const char *double_str, double *d_res);

/* COMMON/ki_atof.c */
int ki_atof(const char *float_str, float *f_res);

/* COMMON/ki_atoi.c */
int ki_atoi(const char *int_str, int *i_res);

/* COMMON/ki_env_to_str.c */
char *ki_env_to_str_old(char *envstr);
char* ki_env_to_str (char* envstr);

/* COMMON/ki_futil.c */
unsigned int kte_get_mask(unsigned int mask);
FILE *kte_fileopen(char *filename, char *type, int *status, unsigned int mask);
int  kte_fileclose(FILE *fd, unsigned int mask);

/* COMMON/libLock.c */
void CreateLockName( char *libName, char *lockName );
int writeLocksExist( char *libname );
int lockExist( char *libName,
               int lockTypeRequested,
               int *readLock,
               int *writeLock );
int createReaderLock( char *libName );
int createWriterLock( char *libName );
void deleteReaderLock( char *libName );
void deleteWriterLock( char *libName );

/* COMMON/pr_sig_error.c */
void pr_sig_error( int ErrVal, char *func_name, int NumErr, long * ErrArray, char * ErrFile);
void pr_sig_errorx(FILE *fd, int ErrVal, char *func_name, int NumErr, long *ErrArray, char *ErrFile);

/* COMMON/rmvwhtsp.c */
void RemoveWhiteSpace(char * input_str);

/* COMMON/scompare.c */
int scompare( char *s, char *t);

/* COMMON/secnds.c */
double secnds( double seed_time);

/* COMMON/stoi.c */
int stoi(register char **instr);

/* COMMON/strip.c */
int strip_cmd(char *cbuf);

/* COMMON/syntax_common.c */
int     IsInvalidFloat_COM ( char *strptr );
int     IsInvalidInt_COM ( char *strptr );

/* COMMON/validpswd.c; see detailed description there;
 *	returns 0 if OK
 */
int validateUserOrPwd(const char* p_user, const char* p_fileAllowedUsers,
		      const char* p_pwd,  const char* p_userSuper,
		      char sysErr[], int sysErrSz);

/* COMMON/validpswd.c; see detailed description there;
 *	returns TRUE/FALSE
 */
int isUserInGroup( char *groupName ) ;

/* COMMON/Is_KI_libName.c */
int Is_KI_libName(char *libName);
int isUserExcludeLibName(char *libName);
