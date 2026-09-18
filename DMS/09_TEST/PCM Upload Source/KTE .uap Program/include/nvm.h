typedef struct nvm_struct {
  int       INITIALIZED;
  int       CH1TERM;
  int       CH2TERM;
  int       CH3TERM;
  int       CH4TERM;
  int       MAXFOHMS;
  double    CH1DELAY;
  double    CH2DELAY;
  double    CH3DELAY;
  double    CH4DELAY;
  double    DRAINPCT;
  double    SOURCEPCT;

} NVM_STRUCT;

NVM_STRUCT nvm;

#define TRUE 1
#define FALSE !TRUE
#define dBAD -999.999
#define iBAD -999
