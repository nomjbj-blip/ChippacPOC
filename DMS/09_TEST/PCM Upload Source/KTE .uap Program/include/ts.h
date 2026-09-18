#define TSF_DEVNAME_LEN 50
#define TSF_INFONAME_LEN 80

#include "ktxe_types.h"


/*
 * info_list_t and dev_list_t are used when the data file is initially read
 * in.  dev_list_t is a list of the various devices, containing an
 * info_list_t for each device.  KITT uses this to parse the file and match
 * up info for a given device.  KSOX uses this to later build the tech_info_t
 * list for internal use.  Currently, the info list leaves the value as a
 * string for 2 reasons.  KITT does not need the extra overhead of finding
 * the actual data, it only needs to display it as a string.  KSOX can do
 * this later when it creates the tech_info list.
 */

enum where_type {UNKNOWN,
		 G_PDI,
		 L_PDI,
		 TS,
		 GD,
		 PCF,
		 RESULTS,
		 KI_INT,
		 PARM_SET,
		 ENTERED_DATA,
                 EXPRESSION }; 

typedef struct _info_list  {
	char *name;
	char *value;
	int type;
	struct _info_list *nextp;
}  info_list_t;

typedef struct _dev_list  {
	char *devname;
	char *type;
	info_list_t *info;
	struct _dev_list *nextp;
}  dev_list_t;


typedef struct _tsf_info
{
    char version[MAXVERSIZE];
    char tsfname[MAXFILENAMESIZE];
    char ascdatetime[MAXDATESTRSIZE];
    char id[MAXCOMMENTSTRSIZE];
    char comment[MAXCOMMENTSTRSIZE];
    dev_list_t *dev_list ;
} 
tsf_info_t;


/* Returns 1 for success, 0 for failure
 */
int CreateDevList( tsf_info_t *tsf_info, dev_list_t **head );


void	RemoveDevList( dev_list_t *head );
int 	get_type( char *strptr );
info_list_t *GetTSInfo( info_list_t *head, char *name );
char *CreateTSName( dev_list_t *dev, char *parm_name );

