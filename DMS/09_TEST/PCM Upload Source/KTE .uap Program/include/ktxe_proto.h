/* ktxe_proto.h */


#ifndef KTXEPROTOH
#define KTXEPROTOH 1

#include "ktxe_types.h"

int loadcpf( cpf_info_t *cpf_info );
cpf_info_t *CreateNewCPF( void );
cpf_info_t *RemoveCPF(cpf_info_t *cpf_info);

int loadkrf( krf_info_t *krf_info );
int FindRealRecipeName( krf_info_t *krf_info ) ;
void ProcessRecipeInfo( krf_info_t *krf_info,
			char *krf_fname,
			char **cpf_fname,
			char **cl_kwf_fname,
			LOT **lot ) ;

int  isPlanValid( char *cpfName, char *krfName, int guiMode ) ;

    
ktm_list_t *CreateNewKTMLIST( void );
ktm_list_t *CleanUpKTMLIST( ktm_list_t *ktm_list );
ktm_list_t *ExpandSiteplan(wpf_info_t *wpf_info );
ktm_list_t *RemoveKTMLIST(ktm_list_t *ktm_list);

slot_list_t *CreateNewSLOT( void );
slot_list_t *RemoveSLOT(slot_list_t *slot_list);

int AppendRESULTLIST( result_list_t *src_list, result_list_t **target_list_ptr );
void CopyRESULTLIST( result_list_t *src_list, result_list_t *dest_list );
result_list_t *CreateNewRESULTLIST( void );
result_list_t *CleanUpRESULTLIST( result_list_t *result_list );
void PrintRESULTLIST( result_list_t *result_list );
void PrintAllRESULTLIST( result_list_t *result_list );

prjsite_list_t *CreateNewPRJSITELIST( void );
prjsite_list_t *RemovePRJSITELIST( prjsite_list_t *prjsite_list );
prjsite_list_t *FindProjectName(prjsite_list_t *project_list, char *name);

prjsubsite_list_t *CreateNewPRJSUBSITELIST( void );
prjsubsite_list_t *RemovePRJSUBSITELIST( prjsubsite_list_t *prjsubsite_list );

uap_list_t *CreateNewUAPLIST( void );
uap_list_t *LoadDefaultUapList( char *name );
uap_list_t *RemoveFromDefaultUapList( uap_list_t *default_uap_list, char *name );
void GetUAPLIST( char *uap_name, uap_list_t *uap_list, 
	uap_list_t **target_uap_list );
enum abort_level_t ExecUAP( char *uap_name, cpf_info_t *cpf_info );

int loadwpf( wpf_info_t *wpf_info );
wpf_info_t *CreateNewWPF( void );

int loadpcf( pcf_info_t *pcf_info );
pcf_info_t *CreateNewPCF( void );
pcf_info_t *RemovePCF(pcf_info_t *pcf_info);

pin_list_t *CreateNewPINLIST( void );
pin_list_t *CleanUpPINLIST( pin_list_t *pin_list );
pin_list_t *RemovePINLIST(pin_list_t *pin_list);

int loadgdf( gdf_info_t *gdf_info );
gdf_info_t *CreateNewGDF( void );
gdf_info_t *RemoveGDF(gdf_info_t *gdf_info);

gdf_list_t *CreateNewGDFLIST( void );
gdf_list_t *CleanUpGDFLIST( gdf_list_t *gdf_list );
gdf_list_t *RemoveGDFLIST(gdf_list_t *gdf_list);

WDFRec *CreateNewWDF( void );

wafpat_list_t *CreateNewWAFPATLIST( void );
wafpat_list_t *RemoveWAFPATLIST( wafpat_list_t *wafpat_list );

siteplan_list_t *CreateNewSITEPLANLIST(void);
siteplan_list_t *CleanUpSITEPLANLIST( siteplan_list_t *list );
siteplan_ktm_list_t *CreateNewSITEPLANKTMLIST(void);
siteplan_list_t *FindSitePlanFromList(siteplan_list_t *current, char *name);	

wwp_list_t *CreateNewWWPLIST( void );
void CopyWWPLIST( wwp_list_t *original, wwp_list_t *new );
wwp_list_t *MergeNewWWP( wwp_list_t *original, wwp_list_t *new, int SortSS );
void AddNewWWP( wwp_list_t *original, wwp_list_t *new );
wwp_list_t *CleanUpWWPLIST( wwp_list_t *wwp_list );
void PrintWWPLIST( wwp_list_t *wwp_list );
void PrintAllWWPLIST( wwp_list_t *wwp_list );
int ExecWWP( wwp_list_t *wwp_list, LIMIT *limit_list, WDFRec *wdfptr, 
	LOT *lot, WAFER *wafer );

void *KTXEgetnewnode( int size ); 
void KTXEfreenode( void *ptr ); 


failed_result_list_t *CreateNewFAILEDRESULTLIST(void); 
failed_result_list_t *CleanUpFAILEDRESULTLIST( failed_result_list_t *failed_result_list ); 

failed_result_list_t *CheckLimitsforAbort( result_list_t *result_list, 
	LIMIT *limit_list, enum abort_level_t *abort_level );

void LogAbortReason( failed_result_list_t *failed_result_list );
void DisableTestsfromAbort( wwp_list_t *current_wwp_list, 
	enum abort_level_t  abort_level, 
	wwp_list_t *wwp_list );
void EnableTestsinWWP( wwp_list_t *wwp_list );

void AddKTMtoWWP( wwp_list_t **wwp_list, ktm_list_t *ktm_list, WDFRec *wdfptr, int SortSS );

int GetSiteCount(wwp_list_t *p);
int GetWaferCount(slot_list_t *p);



int GetSSXYfromWDF( WDFRec *wdfptr, char *prjname, 
	char *ssid, wwp_list_t *temp_wwp, wwp_list_t **wwp_list, int SortSS);

/* if errString is NULL, no message is retuned
 */
int GetSSIDfromKTM( char *ktmname, char *ssid, char *errString );

void GetProgramArgsKTXE(int argc, char *argv[], 
			int  *engine_debug,
			int  *engine_error_report_mode,
			char **err_log_fname,
			int  *engine_event_report_mode,
			char **evt_log_fname,
			int  *gui_look,
			LOT  **lot,
			char **sum_report_options,
			char **kwf_fname,
			char *user_arg,
			char **cpffname,
			char *doc_options,
			char **krf_fname );

int ExecTEE(char *str, int flag, result_list_t **ResultList_head);

int LoadKTMList(ktm_list_t *ktmlistptr);

void InitKTXEErrorLogging( int error_logging_mode, char *err_log_fname );
void InitKTXEEventLogging( int error_logging_mode, char *err_log_fname );

void KTXEEventMsg(char *fmt, ...);
void KTXEErrorMsg(char *fmt, ...);
void KTXEDebugMsg(char *fmt, ...);

void KTXEExitWithError(char *msg);
void KTXEProberErrorMessage(int errnum, char *msg, char *op);
void KTXERetryKDFDialog(char *func, int status);

void KTXEUpdateProbeCardCount( void );
void KTXEProbeContact( void );
int KTXEGetProbeContactCount( void );
#endif

