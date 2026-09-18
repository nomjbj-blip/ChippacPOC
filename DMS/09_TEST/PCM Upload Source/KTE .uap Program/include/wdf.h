/* wdf.h */


#ifndef WDF_HEADER
#define WDF_HEADER 1

#include "kdf.h"
#include "ktxe_defs.h"

/* values for wdfrec mode */
#define SINGLEPROJ 1
#define MULTIPROJ 2

/* values for wdfrec flattype */
#define FLAT 1
#define NOTCH 2


#define LONGSTRING 256

typedef struct _prjsite_list
{
	char prjname[MAXPRJNAMESIZE];
	SITE *sitelist;
	struct _prjsite_list *next;
}
prjsite_list_t;

typedef struct _prjsubsite_list
{
	char prjname[MAXPRJNAMESIZE];
	char prjdesc[LONGSTRING];
	SUBSITE *subsitelist;
	struct _prjsubsite_list *next;
}
prjsubsite_list_t;

typedef struct _wafpat_list
{
	char wafpatname[MAXWAFPATNAMESIZE];
	prjsite_list_t *prjsite_list;
	struct _wafpat_list *next;
}
wafpat_list_t;

typedef struct _wdfstruct
{
	char	version[MAXVERSIZE];
	char	filename[LONGSTRING];
	char	date[LONGSTRING];
	char	comment[LONGSTRING];
	float 	diameter;
	int	mode;		/* single or multi project */
	float 	diesizex;
	float 	diesizey;
	int flattype;	/* flat or notch */
	int	flat;
	int	units;
	float	subsitex;
	float	subsitey;
	int	xoffset;
	int	yoffset;
	int 	axis;
	int	origincol;
	int	originrow;
	float	targetcol;
	float	targetrow;
	float	overtravel;
	float	autoalignx;
	float	autoaligny;
        int	optimize;
	wafpat_list_t *wafpat_list;
	prjsubsite_list_t *prjsubsite_list;
	
} WDFRec, *WDFPtr;


#define PRINTERRSAVE(errline)                       \
        fprintf(stderr, "WDF Error writing line "); \
        fprintf(stderr, errline);                   \
        fprintf(stderr, "\n");

#define PRINTERRLOAD(errline)                       \
        fprintf(stderr, "WDF Error reading line "); \
        fprintf(stderr, errline);                   \
        fprintf(stderr, "\n");



/*** expected line length in wafer description file */
#define WDF_LINE_LENGTH   256
#define VERSION	"Version, 1.0"
#define ENDOFHEADER	"<EOH>"
#define ENDOFSITES	"<EOSITES>"
#define ENDOFSUBSITES	"<EOSUBSITES>"

/** FUNCTION PROTOTYPES */

SUBSITE *CreateNewSubSite();
void     AddNewSubSite(SUBSITE *current, SUBSITE *new);
SUBSITE *FindFirstSubSite(SUBSITE *current);
SUBSITE *FindNextSubSite(SUBSITE *current);
SUBSITE *FindSubSiteId(SUBSITE *current, char *target_id );
int loadwdf(WDFPtr);
int savewdf(WDFPtr);

int readWDF(char    *wdf_file_name,
	    int     test_ref_die,
	    float   *diameter,
	    int     *units,
	    int     *flat,
	    int     *origin,
	    float  *x_die_size,
	    float  *y_die_size,
	    int     *ref_die_column, 
	    int     *ref_die_row,
	    int 	*sec_die_column,
	    int		*sec_die_row,
	    int     *no_sites,
	    int     *no_subsites,
	    SITE    **site_ptr, 
	    SUBSITE **subsite_ptr,
	    char	*comment,
	    float	*subsitex,
	    float	*subsitey);
	    
int writeWDF(char    *wdf_file_name,	/*WDF filename ptr	*/
	    int     sorted,				/* sorted? boolean flag	*/
	    float   *diameter,			
	    int     *units,
	    int     *flat,
	    int     *origin,
	    float  *x_die_size,
	    float  *y_die_size,
	    int     *ref_die_column, 
	    int     *ref_die_row,
	    int 	*sec_die_column,
	    int		*sec_die_row,
	    int     *no_sites,			/* #sites to be probed	*/
	    int     *no_subsites,
	    SITE    *site, 
	    SUBSITE *subsite,
	    char 	*comment,
	    float	*subsitex,
	    float	*subsitey);

#endif /* WDF_HEADER*/
