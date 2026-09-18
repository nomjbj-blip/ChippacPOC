/* ktxe_defs.h */

#ifndef KTXE_DEFSH 
#define KTXE_DEFSH 1


#define MAXVERSIZE 32
#define MAXFILENAMESIZE 256
#define MAXDATESTRSIZE 100
#define MAXCOMMENTSTRSIZE 256
#define MAXENGNAMESTRSIZE 256
#define MAXUPDATEMSG 256

#define MAXWAFPATNAMESIZE 64

#define MAXSITEPLANNAMESIZE 64
#define MAXUAPNAMESIZE 32
#define MAXUAPARGLISTSIZE 256

#define MAXKTMNAMESIZE MAXFILENAMESIZE
#define MAXPRJNAMESIZE 32
#define SLOTNAMESIZE 7

#define MAXPINDESCSIZE 32

#define MAXIDSIZE 128

/* documentation/execution option string length
 */
#define DOC_OPTION_LEN   128

/* cassette plan mode types used in ktxe */
#define ALL_MODE	1
#define OPR_MODE	2
#define ABS_MODE	3
#define REL_MODE	4

/* Return values from ExecTEE */

#define OK_KTXE         1
#define NO_KTM          -10      /* unable to access the ktm */
#define NO_KULT_PATH    -11       /* KI_KULT_PATH not set */
#define NO_USRLIB       -12       /* Unable to open one or more of the usrlibs
				     required by the KTM */
#define INVLD_TECH              -13
#define KTM_FAILED              -14

#define OK_dpAdd	1
#define FAILED_dpAdd	-1

#define HASHSIZE 103	/* hash table length, ljl */

#endif
