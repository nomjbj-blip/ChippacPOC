
#ifndef _KI_LICENSE
#define _KI_LICENSE

#define VC_FEATURE "versionControl"
#define RECIPE_FEATURE "versionControl"

#define SECS_GEM_FEATURE "secsGem"
#define PROBECARD_ID_FEATURE "probeCardID"
#define NVM_FLASH_FEATURE "nvmFlashOption"

#define PULSEGEN_FEATURE "pulseGenOption"
#define HP4284_FEATURE "hp4284Option"
#define SPECTRUM_FEATURE "spectrumOption"
#define FREQCOUNTER_FEATURE "freqCounterOption"
#define ADAPTTEST_FEATURE "adaptTestOption"

#define COPPER_FEATURE "copperParlib"


/* Returns TRUE if enabled FALSE if license could not be found!
 */
int get_KI_license( char *featureName ) ;

#endif
