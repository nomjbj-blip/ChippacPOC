/* ktxe_types.h */

#ifndef KTXE_TYPESH 
#define KTXE_TYPESH 1


#include "ktxe_defs.h"
#include "wdf.h"
#include "kdf.h"
#include "doctree.h"

/* test abort levels */
enum abort_level_t { NOABORT, LOTABORT, WAFERABORT, SITEABORT, SUBSITEABORT, SUBSITEIDABORT };


/*
The ktm list structure is filled in with items from the wpf

for each wafer plan get defined macro info. Also convert siteplans to macros

*/

typedef struct _ktm_list
{
	char ktmfname[MAXFILENAMESIZE];
	char wafpatname[MAXWAFPATNAMESIZE];
	struct _ktm_list *next;
}
ktm_list_t;

/*
List of KTMs loaded into memory
*/
typedef struct _ksox_ktm_list
{
    char *ktmfname; 
    char *testseq;
    void *pdi_list;
    void *select_list;
    void *ts_list ;
    void *ts_hash ;
    void *array_list;
    void *result_list;
    struct _ksox_ktm_list *next;
    void **pdi_hlist;	/* ljl */
}
ksox_ktm_list_t;

/*
The Site Plan KTM List
*/

typedef struct _siteplan_ktm_list
{
	char ktmname[MAXFILENAMESIZE];
	struct _siteplan_ktm_list *next;
}
siteplan_ktm_list_t;

/*

The Site Plan list

*/

typedef struct _siteplan_list
{
	char siteplan_name[MAXSITEPLANNAMESIZE];
	char comment[MAXCOMMENTSTRSIZE];
	siteplan_ktm_list_t *siteplan_ktm_list;
	struct _siteplan_list *next;
}
siteplan_list_t;

/*
The wafer plan info structure is filled in with info from the wafer
plan file .wpf
Items not found in the wpf are set to NULL

Note: the cpf overrides the wdf in the wpf

*/

typedef struct _wpf_info
{
    char version[MAXVERSIZE];
    char wpfname[MAXFILENAMESIZE];
    char ascdatetime[MAXDATESTRSIZE];
    char comment[MAXCOMMENTSTRSIZE];
    char wdffname[MAXFILENAMESIZE];
    char klffname[MAXFILENAMESIZE];
    char pcffname[MAXFILENAMESIZE];
    
    int SortSSflag ;
    
    siteplan_list_t *siteplan_list;
    ktm_list_t *ktm_list;
} 
wpf_info_t;

/*

get site info from wafer plan file .wpf put into the whole wafer plan

*/

typedef struct _wwp_list
{
    int siteunits;		/* = english, = metric */
    char siteid[ SITE_ID_LENGTH ];
    float sitex;		/* eng. mil, met. mm */ 
    float sitey;
    char ssid[ SS_ID_LENGTH ];
    int ssunits;
    float ssitex;
    float ssitey;
    char ssktm[MAXKTMNAMESIZE];
    int testenabled;	/* 1 = test enabled, 0 = disabled */
    int testStatus ;	/* Status of the macro for this node
			 * Executed = OK_KTXE , or error...
			 */
    int testDisp;   /* test disposition */

    struct _wwp_list *next;
    struct _wwp_list *prev;
}
wwp_list_t;

/* 

The wafer plan list structure is filled with wafer plans from 
the cassette plan file .cpf.
The wpf_info pointer is entered when the wpf file is read.
Otherwise it should be NULL.
next_wpf points to the next linked list item.
Otherwise it should be NULL.

*/

typedef struct _wpf_list
{
	char wpfname[MAXFILENAMESIZE];
	wpf_info_t *wpf_info;
	struct _wpf_list *next;
}
wpf_list_t;

/*
The Slot List
*/

typedef struct _slot_list
{
    char	slot[SLOTNAMESIZE];
    char	wafer_id[ WAFER_ID_LENGTH];
    char	split_id[ WAFER_SPLIT_LENGTH];
    char	wpfname[MAXFILENAMESIZE];
    char	plan_wpfname[MAXFILENAMESIZE];
    struct _slot_list *next;
}
slot_list_t;

/*
The UAP list
*/
typedef struct _uap_list
{
	char uapname[MAXUAPNAMESIZE];
	char uaplibname[MAXFILENAMESIZE];
	char uaparglist[MAXUAPARGLISTSIZE];
	struct _uap_list *next;
}
uap_list_t;

/* 
The cassette plan info structure is filled in with info from the cassette
plan file .cpf
Items not found in cpf are set to NULL

*/

typedef struct _gdffname_list
{
	char gdffname[MAXFILENAMESIZE];
	struct _gdffname_list *next;
} gdffname_list_t;

/* PR 11488 start */
typedef struct _uapfname_list
{
	char uapfname[MAXFILENAMESIZE];
	struct _uapfname_list *next;
} uapfname_list_t;
/* PR 11488 end */

typedef struct
{
	char version[MAXVERSIZE];
	char cpfname[MAXFILENAMESIZE];
	char ascdatetime[MAXDATESTRSIZE];
	char comment[MAXCOMMENTSTRSIZE];
/*	char uapdefaults[MAXENGNAMESTRSIZE]; removed for PR11488 use uapfname_list below */
	char engine[MAXENGNAMESTRSIZE];
	char pcffname[MAXFILENAMESIZE];
	gdffname_list_t *gdffname_list;
	char kdffname[MAXFILENAMESIZE];
	char wdffname[MAXFILENAMESIZE];
	slot_list_t *slot_list;
	uap_list_t *uap_list;
	uapfname_list_t *uapfname_list; /*PR11488*/
}
cpf_info_t;

#define NORMAL 0
#define SPECIFIC 1

typedef struct
{
    char version[MAXVERSIZE];
    char krfname[ MAXFILENAMESIZE * 4 ];
    char ascdatetime[MAXDATESTRSIZE];
    char comment[MAXCOMMENTSTRSIZE];
    char revid[MAXCOMMENTSTRSIZE];
    char command[MAXFILENAMESIZE];
    char cpffname[MAXFILENAMESIZE];
    char klffname[MAXFILENAMESIZE];
    char wdffname[MAXFILENAMESIZE];

    /* yeah, I know, messy but easy to understand...
     */
    char usrfield1[MAXFILENAMESIZE];
    char usrfield2[MAXFILENAMESIZE];
    char usrfield3[MAXFILENAMESIZE];
    char usrfield4[MAXFILENAMESIZE];
    char usrfield5[MAXFILENAMESIZE];
    char usrfield6[MAXFILENAMESIZE];
    char usrfield7[MAXFILENAMESIZE];
    char usrfield8[MAXFILENAMESIZE];
    char usrfield9[MAXFILENAMESIZE];
    char usrfield10[MAXFILENAMESIZE];
    char usrfield11[MAXFILENAMESIZE];
    char usrfield12[MAXFILENAMESIZE];
    char usrfield13[MAXFILENAMESIZE];
    char usrfield14[MAXFILENAMESIZE];
    char usrfield15[MAXFILENAMESIZE];
    char usrfield16[MAXFILENAMESIZE];
    char usrfield17[MAXFILENAMESIZE];
    char usrfield18[MAXFILENAMESIZE];
    char usrfield19[MAXFILENAMESIZE];
    char usrfield20[MAXFILENAMESIZE];
    char usrfield21[MAXFILENAMESIZE];
    char usrfield22[MAXFILENAMESIZE];
    char usrfield23[MAXFILENAMESIZE];
    char usrfield24[MAXFILENAMESIZE];
    char usrfield25[MAXFILENAMESIZE];
    char usrfield26[MAXFILENAMESIZE];
    char usrfield27[MAXFILENAMESIZE];
    char usrfield28[MAXFILENAMESIZE];
    char usrfield29[MAXFILENAMESIZE];
    char usrfield30[MAXFILENAMESIZE];
    char usrfield31[MAXFILENAMESIZE];
    char usrfield32[MAXFILENAMESIZE];
    
    FileList docList;
    FileList libList;
    int type;
}
krf_info_t;

typedef struct _result_list
{
	char	id[PARAM_ID_LENGTH];
	float	value;
	int		log;	/* True or False Flag to determine if the result is logged*/
	int		user;	/* True or False Flag for User Data Logging */
	struct	_result_list	*next;
}
result_list_t;

typedef struct _failed_result_list
{
    char	id[PARAM_ID_LENGTH];
    float	high;
    float	low;
	char	abortflag[LIMIT_ABORTSTR_LENGTH];
	char	abortfield[LIMIT_ABORTSTR_LENGTH];
	float	value;
	struct	_failed_result_list	*next;
}
failed_result_list_t;

/* Probe Card File (pcf) Info structure */
/* Filled from the pcf file */

typedef struct _pin_list
{
	char pin_desc[MAXPINDESCSIZE];
	int  pin_datatype;
	int  pin_number; 
	struct _pin_list *next;
}
pin_list_t;

typedef struct _pcf_info
{
	char version[MAXVERSIZE];
	char pcfname[MAXFILENAMESIZE];
	char ascdatetime[MAXDATESTRSIZE];
	char comment[MAXCOMMENTSTRSIZE];
	char id[MAXCOMMENTSTRSIZE];
	pin_list_t *pin_list;
} 
pcf_info_t;

/* Global Data file (gdf) Info structure */
/* Filled from the gdf file */

typedef struct _gdf_list
{
    char    gdf_id[MAXIDSIZE];
    int     type;
    int	size;
    void    *valuep;
    struct  _gdf_list *next;
    struct  _gdf_list *nexth;
}
gdf_list_t;

typedef struct _gdf_info
{
	char version[MAXVERSIZE];
	char gdfname[MAXFILENAMESIZE];
	char ascdatetime[MAXDATESTRSIZE];
	char comment[MAXCOMMENTSTRSIZE];
	char id[MAXCOMMENTSTRSIZE];
	gdf_list_t *gdf_list;
} 
gdf_info_t;


/* Parameter Set Data File Structures
 */
typedef struct _psf_data
{
    char    id[ MAXIDSIZE ];
    int     type;
    void    *valuep;
    struct  _psf_data *next;
    struct  _psf_data *nexth;
}
psf_data_t;


typedef struct _psf_set
{
    char *setName ;
    char *MyModuleName ;
    psf_data_t *parmList ;
    struct _psf_set *next ;
}
psf_set_t ;

typedef struct _psf_mod
{
    char *moduleName ;
    psf_set_t *parmSet ;
    struct _psf_mod *next ;
}
psf_mod_t ;


typedef struct _psf_info
{
    char version[MAXVERSIZE];
    char psfname[MAXFILENAMESIZE];
    char ascdatetime[MAXDATESTRSIZE];
    char comment[MAXCOMMENTSTRSIZE];
    char id[MAXCOMMENTSTRSIZE];
    psf_mod_t *psfModList ;
} 
psf_info_t;

#endif
