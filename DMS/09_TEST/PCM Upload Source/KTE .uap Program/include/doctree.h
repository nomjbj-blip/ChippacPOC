/* 
    doctree.h 

    Copyright (c) 1999 by Keithley Instruments, Inc. Cleveland, Ohio.

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
 *  $Source: /cm/test/build/S600/v420/COMMON/RCS/doctree.h,v $
 *
 *  $Revision: 1.12 $
 *
 *  $Date: 2000/07/12 16:03:18 $
 *
 *  $Log: doctree.h,v $
 *  Revision 1.12  2000/07/12 16:03:18  jjarvis
 *  *** empty log message ***
 *
 *  Revision 1.11  2000/06/14 13:18:18  jjarvis
 *  added processTSF prototype
 *
 *  Revision 1.10  2000/06/13 20:20:59  jjarvis
 *  added processPSF
 *
 *  Revision 1.9  2000/06/06 13:02:32  jjarvis
 *  changed parameter to processLIBLIST and also added the declaration for
 *  another function
 *
 *  Revision 1.8  2000/05/08 21:18:26  jjarvis
 *  made changes to support usrlibs, etc.
 *
 *  Revision 1.7  2000/04/10 14:41:21  jjarvis
 *  defined BAIL_OUT
 *
 *  Revision 1.6  2000/04/04 13:35:13  jjarvis
 *  changed parameter in FileListDestroy
 *
 *  Revision 1.5  2000/03/24 17:00:49  mpomeran
 *  added a function allocFileListNode()
 *
 *  Revision 1.4  2000/03/23 20:41:44  jjarvis
 *  made changes
 *
 *  Revision 1.3  2000/02/24 13:11:05  jjarvis
 *  added revid to FileList struct
 *
 *  Revision 1.2  2000/02/16 20:02:14  jjarvis
 *  changed name of struct (there was a conflict with ND)
 *
 *  Revision 1.1  1999/08/17 21:20:10  psuwondo
 *  Initial revision
 *
 *
 */

#include <stdio.h>

#define LOCALMODE 0
#define ARCHIVEMODE 1
#define BAIL_OUT -23
#define FLISTARRAYSIZE ENDFLISTARRAY

typedef struct _TreeFileRec
{
        char *name;
        char *revid;
        struct _TreeFileRec *next;
}
TreeFileRec, *FileList;

typedef struct _LibRec
{
    char *name;
    struct _LibRec *next;
    FileList modlist;
}
LibRec, *LibList;

typedef struct _DepRec
{
    char *lib;
    struct _DepRec *next;
    FileList deplist;
}
DepRec, *DepList;

enum
{
    MASTERLIST, /* = 0 */
    LIBMASTERLIST,
    CPFLIST,
    UAPLIST,
    KTMLIST,
    PCFLIST,
    WDFLIST,
    GDFLIST,
    KLFLIST,
    LIBLIST,
    KRFLIST,
    WPFLIST,
    CURERROR,
    CURMASTER,
    CURLIBMASTER,
    ERRORLIST,
    LIBCUR,
    TSFLIST,
    ENDFLISTARRAY /* = 18 */
};

int FileListDestroy(FileList *head);
int LibListDestroy(LibList head);
int DepListDestroy(DepList head);
int GetDependencies(FileList file, DepList *dep);
int GetLine(FILE *f, char **str);
void initSuperFileList(FileList *superlist);
int doctree(char *fileinput, FileList *inlist, FileList *liblist, FileList *inerr, int mode, char *const_env);
int tar_docs(FileList tarlist, char *tarfilename);
int untar_docs(char *tarfilename);
int RemoveDir(char *directory);
void rmTempFiles(FileList removeList);
int processKRF(FileList *superlist, int mode, char *const_env);
int processCPF(FileList *superlist, LibList *alllibslist, int mode, char *const_env);
int processWPF(FileList *superlist, int mode, char *const_env);
int processUAP(FileList *superlist, LibList *alllibslist, int mode, char *const_env);
int processKTM(FileList *superlist, LibList *alllibslist, int mode, char *const_env);
int processPCF(FileList *superlist, int mode, char *const_env);
int processWDF(FileList *superlist, int mode, char *const_env);
int processGDF(FileList *superlist, int mode, char *const_env);
int processTSF(FileList *superlist, int mode, char *const_env);
int processLIBLIST(FileList *superlist, FileList revList, int mode);
int processPSF(FileList *superlist, int mode, char *const_env);
void copyFileList(FileList inlist, FileList *outlist);

FileList allocFileListNode(FileList* p_node, const char* p_name,
									const char* p_revID);
