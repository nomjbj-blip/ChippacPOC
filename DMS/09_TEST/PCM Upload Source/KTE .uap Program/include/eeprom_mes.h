#include <math.h>
#include <string.h>

/*****************************************************************************************/
/* programming routines : assign -1 to pin-number if pin not connected or does not exist */
/*****************************************************************************************/

/*********************************************************************/
/* COMMENT SWITCH COMMAND AND COMPILE PROGRAM BEFORE USING STATION B */
/*********************************************************************/

#define MAX(a,b)   ((a)>(b) ? (a) : (b))

char my_prober = 'T';


int x27(int pad)
{
   if(my_prober != 'E')
	{
   		switch(pad)
			{
				case 1: return 23; break;
				case 2: return 24; break;
				case 3: return 25; break;
				case 4: return 26; break;
				case 5: return 27; break;
				case 6: return 28; break;
				case 7: return 29; break;
				case 8: return 30; break;
				case 9: return 31; break;
				case 10: return 32; break;
				case 11: return 5; break;
				case 12: return 6; break;
				case 13: return 7; break;
				case 14: return 8; break;
				case 15: return 9; break;
				case 16: return 10; break;
				case 17: return 11; break;
				case 18: return 12; break;
				case 19: return 13; break;
				case 20: return 14; break;
				default: return -1;
			}
	}
  else
	return pad;
} 


int x13(int pad)
{
  switch(pad)
	 {
		case  1: return 22; break;
		case  2: return 23; break;
		case  3: return 24; break;
		case  4: return 25; break;
		case  5: return 26; break;
		case  6: return 27; break;
		case  7: return 28; break;
		case  8: return 29; break;
		case  9: return 30; break;
		case 10: return 31; break;
		case 11: return 5; break;
		case 12: return 6; break;
		case 13: return 7; break;
		case 14: return 8; break;
		case 15: return 9; break;
		case 16: return 10; break;
		case 17: return 11; break;
		case 18: return 12; break;
		case 19: return 13; break;
		case 20: return 14; break;
		default: return 0;
	 }
}

int fndmax(float array[], int numpts)
{
  int marker, n;
  marker = 0;
  for(n=0; n<numpts; n++)
	 if(array[n]>array[marker])
		marker = n;
  return marker;
}

int fndlocextr(float array[], int numpts, int start)
{
  int marker, n;
  marker = start;
  for(n=start; n<numpts; n++)
	 if(fabs(array[n])>fabs(array[marker]))
		marker = n;
  return marker;
}


double cmt_slope(int g1, int g2, int s1, int s2, int ss1, int ss2, int d, int dummy, int sub, float v1, float v2, float is, float vslimit, int z)
{
 int n;
 float vsA, vsB;
 double slopeAB;


 setvmtr(VIMS2);

 conpin(VIMS1H, s1, KI_EOC);
 conpin(VIMS2H, ss1, KI_EOC);
 conpin(VIMS3H, g1, KI_EOC);

 if((dummy<0)&&(sub<0))
 	conpin(GND, g2, s2, ss2, d,             VIMS1L, VIMS2L, VIMS3L, VIMS4L, KI_EOC);
 if((dummy<0)&&(sub>0))
 	conpin(GND, g2, s2, ss2, d,        sub, VIMS1L, VIMS2L, VIMS3L, VIMS4L, KI_EOC);
 if((dummy>0)&&(sub<0))
 	conpin(GND, g2, s2, ss2, d, dummy,      VIMS1L, VIMS2L, VIMS3L, VIMS4L, KI_EOC);
 if((dummy>0)&&(sub>0))
 	conpin(GND, g2, s2, ss2, d, dummy, sub, VIMS1L, VIMS2L, VIMS3L, KI_EOC);

 limitv(VIMS1, vslimit);
 slopeAB = 0;
 		
 forcei(VIMS1, is);
 for(n=0; n<z; n++)
	{
		forcev(VIMS3, v1);
		intgv(VIMS2, &vsA);	
		forcev(VIMS3, v2);
		intgv(VIMS2, &vsB);	
		slopeAB = slopeAB + (v2-v1)/(vsB-vsA);
	}
 execut();
 return slopeAB/z;
}



void vtext4(int d, int g, int s, int b, int er, int nw, int test, float Vdstart, float Vdstop, float vds, float *gm, float *vt, int *flag)
{
  #undef NUMPTS
  #undef DELAY
  #define NUMPTS 140
  #define DELAY 2E-3

  FILE *out_fp;
  float Id[NUMPTS], Ids[NUMPTS];
  float Vg[NUMPTS];
  float yinter[NUMPTS];
  float r[NUMPTS];
  float slope1[NUMPTS];
  char file[13];
  int n;

  for(n=0;n<NUMPTS;n++)
	 Vg[n] = Vdstart + n*(Vdstop-Vdstart)/(NUMPTS-1);
  conpin(VIMS1H, d, 0);
  if(nw < 0)
	 conpin(VIMS2H, g, 0);
  else
	 conpin(VIMS2H, g, nw, 0);
  if(er < 0)
  	 conpin(GND, s, b, VIMS1L, VIMS2L, 0);
  else
  	 conpin(GND, s, b, er, VIMS1L, VIMS2L, 0);
  forcev(VIMS1, vds);
  smeasi(VIMS1, Ids);
  sweepv(VIMS2, Vdstart, Vdstop, NUMPTS-1, DELAY);
  execut();
  for(n=0;n<NUMPTS;n++)
	 Ids[n] = -Ids[n];
  smooth(Ids, Id, NUMPTS);
  fndslp(Vg, Id, NUMPTS, slope1, yinter, r);
  n = fndmax(slope1, NUMPTS);
  if((n>0)&&(n<NUMPTS))
	 {
		*vt = Vg[n] - Id[n]/slope1[n] - vds/2;
		*gm = slope1[n];
		*flag = 0;
	 }
  else
	 {
		*vt = 9999.99;
		*gm = 9999.99;
		*flag = 1;
	 }

/*#########################################################*/
 /* sprintf(file, "%i", test);
  strcat(file, ".txt");
  out_fp = fopen(file, "w");
  fprintf(out_fp, "%s\t%s\t%s\t%s\n", "Vg [V]", "Ids", "Id (smooth)", "slope1(Id)");
  for(n=0;n<NUMPTS;n++)
	 fprintf(out_fp, "%.15f\t%.15f\t%.15f\t%.15f\n", Vg[n], Ids[n], Id[n], slope1[n]);
  fclose(out_fp);
/*#########################################################*/

}


void my_vtext4(int d, int g, int s, int b, int er, int nw, int test, float Vdstart, float Vdstop, float vds, float *gm, float *vt, int samples)
{
  float dvt, dgm;
  int z, n, flag;

  *vt=*gm=0;
  z=0;
  for(n=0; n<samples;n++)
	 {
		vtext4(d, g, s, b, er, nw, test, Vdstart, Vdstop, vds, gm, vt, &flag);
		if(flag == 0)
		  {
			 dvt += *vt;
			 dgm += *gm;
			 z++;
		  }
	 }
  if(z>0)
	 {
		*vt = dvt/z;
		*gm = dgm/z;
	 }
  else
	 {
		*vt = 9999.99;
		*gm = 9999.99;
	 }
}


void vtext5(int d, int g, int s, int test, float Vdstart, float Vdstop, float vds, float *gm, float *vt, int *flag)
{
  #undef NUMPTS
  #undef DELAY
  #define NUMPTS 50 
  #define DELAY 2E-3

  FILE *out_fp;
  float Id[NUMPTS], Ids[NUMPTS];
  float Vg[NUMPTS];
  float yinter[NUMPTS];
  float r[NUMPTS];
  float slope1[NUMPTS];
  char file[13];
  int n;

  for(n=0;n<NUMPTS;n++)
	 Vg[n] = Vdstart + n*(Vdstop-Vdstart)/(NUMPTS-1);
  conpin(VIMS1H, d, 0);
  conpin(VIMS2H, g, 0);
  conpin(GND, s, VIMS1L, VIMS2L, 0);
  forcev(VIMS1, vds);
  smeasi(VIMS1, Ids);
  sweepv(VIMS2, Vdstart, Vdstop, NUMPTS-1, DELAY);
  execut();
  for(n=0;n<NUMPTS;n++)
	 Ids[n] = -Ids[n];
  smooth(Ids, Id, NUMPTS);
  fndslp(Vg, Id, NUMPTS, slope1, yinter, r);
  n = fndmax(slope1, NUMPTS);
  if((n>0)&&(n<NUMPTS))
	 {
		*vt = Vg[n] - Id[n]/slope1[n] - vds/2;
		*gm = slope1[n];
		*flag = 0;
	 }
  else
	 {
		*vt = 9999.99;
		*gm = 9999.99;
		*flag = 1;
	 }

/*#########################################################*/
/*  sprintf(file, "%i", test);
  strcat(file, ".txt");
  out_fp = fopen(file, "w");
  fprintf(out_fp, "%s\t%s\t%s\t%s\n", "Vg [V]", "Ids", "Id (smooth)", "slope1(Id)");
  for(n=0;n<NUMPTS;n++)
	 fprintf(out_fp, "%.15f\t%.15f\t%.15f\t%.15f\n", Vg[n], Ids[n], Id[n], slope1[n]);
  fclose(out_fp);
/*#########################################################*/

}

void my_vtext5(int d, int g, int s, int test, float Vdstart, float Vdstop, float vds, float *gm, float *vt, int samples)
{
  float dvt, dgm;
  int z, n, flag;

  *vt=*gm=0;
  z=0;
  for(n=0; n<samples;n++)
	 {
		vtext5(d, g, s, test, Vdstart, Vdstop, vds, gm, vt, &flag);
		if(flag == 0)
		  {
			 dvt += *vt;
			 dgm += *gm;
			 z++;
		  }
	 }
  if(z>0)
	 {
		*vt = dvt/z;
		*gm = dgm/z;
	 }
  else
	 {
		*vt = 9999.99;
		*gm = 9999.99;
	 }
}


void my_vtext3(int d, int g, int s, int b, float vlow, float vhigh, float vds, float vbs, int npts, float *gm, float *vt, int samples)
{
  float dvt, dgm;
  int z, n, flag;

  *vt=*gm=0;
  z=0;
  for(n=0; n<samples;n++)
	 {
		vtext3(d, g, s, b, vlow, vhigh, vds, vbs, npts, gm, vt, &flag);
		if(flag == 0)
		  {
			 dvt += *vt;
			 dgm += *gm;
			 z++;
		  }
	 }
  if(z>0)
	 {
		*vt = dvt/z;
		*gm = dgm/z;
	 }
  else
	 {
		*vt = 9999.99;
		*gm = 9999.99;
	 }
}


float my_vtati(int drain, int gate, int source, int subst, float vlow, float vhigh, float vds, float vbs, float ithr, int niter)
{
	extern 	int fndtrg();		/* int function */

	int 	lowt;			/* low trigger flag */
	int	num;			/* local copy of niter */
	float	avmax;			/* max of fabs(vlow) and fabs(vhigh) */
	float	sdelay;			/* delay for search, based on avmax */
	float	iltd;			/* measured gate current */
	float 	crtlmt = 10.E-6;	/* SET TO 10E-6 */
	float	vtati_fnc;		/* function return result */
	float	trigger_current;	/* signed trigger current */


	/* Check number of iterations */

	num = niter;
	if (num < 2) num = 2;
	if (num > 16) num = 16;


	/* Connect device */ 

	if (subst < 1)	{		
	    conpth (3, GND, VIMS1L, VIMS2L, source, KI_EOC);
	}
	else if (fabs(vbs) < .9e-3) { 	
	    conpth (3, GND, VIMS1L, VIMS2L, source, subst, KI_EOC);
	}
 	else {
	    conpth (3, GND, VIMS1L, VIMS2L, VIMS3L, source, KI_EOC);
	    conpth (4, VIMS3, subst, KI_EOC);
	}
	conpin  (VIMS2, gate, KI_EOC);
	conpin  (VIMS1, drain, KI_EOC);


	/* Set up trigger. Limit Gate current to 10.E-6 */

	lowt = fndtrg(vlow, vhigh);
	trigger_current = -ithr;
	if (lowt) {
	    trigil(VIMS1, trigger_current);
	}
	else {
	    trigig(VIMS1, trigger_current);
	}
	limiti (VIMS2, crtlmt);


	/* Calculate delay necessary for SEARCHV call */

	avmax = MAX(fabs(vlow), fabs(vhigh));
	sdelay = tdelay(1, crtlmt, avmax);


	/* Collect data */

	if (subst > 0 && fabs(vbs) >= .9e-3) forcev (VIMS3, vbs);
  	forcev (VIMS1, vds);
	searchv (VIMS2, vlow, vhigh, num, sdelay, &vtati_fnc);
	measi (VIMS2, &iltd);
	execut();


	/* Check triggered voltage. Return with error if within 98% of SEARCHV 	
	   limits or if Gate current is within 98% of limit value (crtlmt) */

	if (fabs(vlow - vtati_fnc) <= 1.e-3) vtati_fnc = 1.e+21;
	if (fabs(vhigh - vtati_fnc) <= 1.e-3) vtati_fnc = 2.e+21;
	if (fabs(iltd) >= (0.98*crtlmt)) vtati_fnc = 4.0e+21;

	return(vtati_fnc);
}

float my_vtati2(int drain, int gate_n, int gate_p, int source, int erase_n, int erase_p, int subst, float vlow, float vhigh, float vds, float vbs, float ithr, int niter)
{
	extern 	int fndtrg();		/* int function */

	int 	lowt;			/* low trigger flag */
	int	num;			/* local copy of niter */
	float	avmax;			/* max of fabs(vlow) and fabs(vhigh) */
	float	sdelay;			/* delay for search, based on avmax */
	float	iltd;			/* measured gate current */
	float 	crtlmt = 10.E-6;	/* SET TO 10E-6 */
	float	vtati_fnc;		/* function return result */
	float	trigger_current;	/* signed trigger current */


	/* Check number of iterations */

	num = niter;
	if (num < 2) num = 2;
	if (num > 16) num = 16;

	/* Connect device */ 

	if (subst < 1)	{		
	    conpth (3, GND, VIMS1L, VIMS2L, source, erase_n, erase_p, KI_EOC);
	}
	else if (fabs(vbs) < .9e-3) { 	
	    conpth (3, GND, VIMS1L, VIMS2L, source, erase_n, erase_p, subst, KI_EOC);
	}
 	else {
	    conpth (3, GND, VIMS1L, VIMS2L, VIMS3L, source, erase_n, erase_p, KI_EOC);
	    conpth (4, VIMS3, subst, KI_EOC);
	}
	conpin  (VIMS2, gate_n, gate_p, KI_EOC);
	conpin  (VIMS1, drain, KI_EOC);

	/* Set up trigger. Limit Gate current to 10.E-6 */

	lowt = fndtrg(vlow, vhigh);
	trigger_current = -ithr;
	if (lowt) {
	    trigil(VIMS1, trigger_current);
	}
	else {
	    trigig(VIMS1, trigger_current);
	}
	limiti (VIMS2, crtlmt);


	/* Calculate delay necessary for SEARCHV call */

	avmax = MAX(fabs(vlow), fabs(vhigh));
	sdelay = tdelay(1, crtlmt, avmax);


	/* Collect data */

	if (subst > 0 && fabs(vbs) >= .9e-3) forcev (VIMS3, vbs);
  	forcev (VIMS1, vds);
	searchv (VIMS2, vlow, vhigh, num, sdelay, &vtati_fnc);
	measi (VIMS2, &iltd);
	execut();


	/* Check triggered voltage. Return with error if within 98% of SEARCHV 	
	   limits or if Gate current is within 98% of limit value (crtlmt) */

	if (fabs(vlow - vtati_fnc) <= 1.e-3) vtati_fnc = 1.e+21;
	if (fabs(vhigh - vtati_fnc) <= 1.e-3) vtati_fnc = 2.e+21;
	if (fabs(iltd) >= (0.98*crtlmt)) vtati_fnc = 4.0e+21;

	return(vtati_fnc);
}

void progr_OTP(int d, int s, int sub, float vd, float tp)
{
  conpin(VIMS1H, d, KI_EOC);
  conpin(GND, VIMS1L, s, sub, KI_EOC);
  pulsev(VIMS1, vd, tp*1e-3);
  execut();
}

void progr_ACC_OTP(int d, int ag, int s, int sub, float vd, float vag, float tp)        /* OTP w/ access gate */
{
  conpin(VIMS1H, d, KI_EOC);
  conpin(VIMS2H, ag, KI_EOC);
  conpin(GND, VIMS1L, VIMS2L, s, sub, KI_EOC);
  forcev(VIMS2, vag);
  pulsev(VIMS1, vd, tp*1e-3);
  execut();
}

void progr_EEPROM(int d, int cg1, int cg2, int s, int tr1, int tr2, int sub, float vcg, float tp)
{
  conpin(VIMS1H, cg1, cg2, KI_EOC);
  conpin(GND, VIMS1L, d, s, tr1, tr2, sub, KI_EOC);
  pulsev(VIMS1, vcg, tp*1e-3);
  execut();
}

void erase_EEPROM(int d, int cg1, int cg2, int s, int tr1, int tr2, int sub, float vtr, float te)
{
  conpin(VIMS1H, tr1, tr2, KI_EOC);
  conpin(GND, VIMS1L, d, s, cg1, cg2, sub, KI_EOC);
  pulsev(VIMS1, vtr, te*1e-3);
  execut();
}

void pe_dbl_poly_ee(int d, int cg, int s, int sub, float vcg, float tp)
{
  conpin(VIMS1H, cg, KI_EOC);
  conpin(GND, VIMS1L, d, s, sub, KI_EOC);
  pulsev(VIMS1, vcg, tp*1e-3);
  execut();
}

void cycle_dbl_poly_ee(int d, int cg, int s, int sub, float vp, float tp, float ve, float te, int cycles)
{
  int n;

  conpin(VIMS1H, cg, KI_EOC);
  conpin(GND, VIMS1L, d, s, sub, KI_EOC);
  for(n=0; n<cycles; n++)
    {
      pulsev(VIMS1, vp, tp*1e-3);
      pulsev(VIMS1, ve, te*1e-3);
    }
  execut();
}

float fuse_resistance(int fuse, float vtest)
{
  float ifuse, resistance;
  int agate, igate1, igate2;   /* active and inactive gates */
  if((fuse == 1)||(fuse == 7) || (fuse == 9))
    {
      if(fuse == 1)
        {
          agate  = x27(1);
          igate1 = x27(8);
          igate2 = x27(10);
        }
      if(fuse == 7)
        {
          agate  = x27(8);
          igate1 = x27(1);
          igate2 = x27(10);
        }
      if(fuse == 9)
        {
          agate  = x27(10);
          igate1 = x27(8);
          igate2 = x27(1);
        }

      conpin(VIMS1H, x27(2), x27(7), x27(9), KI_EOC);
      conpin(GND, VIMS1L, VIMS2L, igate1, igate2, x27(20), KI_EOC);
      conpin(VIMS2H, agate, KI_EOC);
  
      forcev (VIMS1, vtest);
      forcev (VIMS2, 18);
      delay(50);
      measi  (VIMS1 , &ifuse);
      execut();

      if (ifuse != 0)
        resistance = fabs(vtest/ifuse);
      else
        resistance = 9999;
    }
  else
    resistance = -1;
  return resistance;
}

void blow_fuse(int fuse, float vblow)
{
  int agate, igate1, igate2;   /* active and inactive gates */
  if((fuse == 1)||(fuse == 7) || (fuse == 9))
    {
      if(fuse == 1)
        {
          agate  = x27(1);
          igate1 = x27(8);
          igate2 = x27(10);
        }
      if(fuse == 7)
        {
          agate  = x27(8);
          igate1 = x27(1);
          igate2 = x27(10);
        }
      if(fuse == 9)
        {
          agate  = x27(10);
          igate1 = x27(8);
          igate2 = x27(1);
        }

      conpin(VIMS1H, x27(2), x27(7), x27(9), KI_EOC);
      conpin(GND, VIMS1L, VIMS2L, igate1, igate2, x27(20), KI_EOC);
      conpin(VIMS2H, agate, KI_EOC);
  
      forcev (VIMS2, 0);
      forcev (VIMS1, 0);
      delay(5);
      forcev (VIMS1, vblow);
      delay(10);
      forcev (VIMS2, 18);
      delay(10);
      forcev (VIMS2, 0);
      forcev (VIMS1, 0);
      delay(50);
      execut();
    }
}

void blow_fuse_x1652(int fuse, float vblow)
{
  int agate, igate1, igate2;   /* active and inactive gates */
  if((fuse == 4)||(fuse == 5) || (fuse == 9))
    {
      if(fuse == 4)
        {
          agate  = 4;
          igate1 = 6;
          igate2 = 10;
        }
      if(fuse == 5)
        {
          agate  = 6;
          igate1 = 4;
          igate2 = 10;
        }
      if(fuse == 9)
        {
          agate  = 10;
          igate1 = 4;
          igate2 = 6;
        }

      conpin(VIMS1H, 5, KI_EOC);
      conpin(GND, VIMS1L, VIMS2L, igate1, igate2, 20, KI_EOC);
      conpin(VIMS2H, agate, KI_EOC);
  
      forcev (VIMS2, 0);
      forcev (VIMS1, 0);
      delay(5);
      forcev (VIMS1, vblow);
      delay(10);
      forcev (VIMS2, 18);
      delay(10);
      forcev (VIMS2, 0);
      forcev (VIMS1, 0);
      delay(50);
      execut();
    }
}

void blow_fuse_x1652b()
{
      conpin(VIMS1H, 14, KI_EOC);
      conpin(GND, VIMS1L, 13, KI_EOC);
  
      forcev (VIMS1, 10);
      delay(20);
      forcev (VIMS1, 0);
      delay(50);
      execut();
}

float OTP_id (int drain, int source, int subst, float vds, float vbs)
{
	float 	id;		/* local variable */

 	/* Connect device */

	if (subst < 1)	{			/*no substrate */
	    conpth (3, GND, source, VIMS1L, KI_EOC);
	}
	else if (fabs(vbs) < .9e-3) {		/*ground substrate */
	    conpth (3, GND, subst, source, VIMS1L, KI_EOC);
	}
	else {					/*otherwise bias it */
	    conpth (3, GND, source, VIMS1L, VIMS3L, KI_EOC);   
	    conpth (4, VIMS3, subst, KI_EOC);
	}
	conpin (VIMS1, drain, KI_EOC);


 	/* Do force and measure */

	if ((fabs(vbs) >= .9e-3) && (subst > 0)) forcev (VIMS3, vbs );
	forcev (VIMS1, vds);
	measi  (VIMS1 , &id);
	execut();

	return(-id);			/*convention: id > 0 when vds > 0. */
}

float ACC_OTP_id(int drain, int agate, int source, int subst, float vds, float vag, float vbs)  /* OTP w/ access gate */
{
	float 	id;		/* local variable */

 	/* Connect device */

	if (subst < 1)	{			/*no substrate */
	    conpth (3, GND, source, VIMS1L, VIMS2L, KI_EOC);
	}
	else if (fabs(vbs) < .9e-3) {		/*ground substrate */
	    conpth (3, GND, subst, source, VIMS1L, VIMS2L, KI_EOC);
	}
	else {					/*otherwise bias it */
	    conpth (3, GND, source, VIMS1L, VIMS2L, VIMS3L, KI_EOC);   
	    conpth (4, VIMS3, subst, KI_EOC);
	}
	conpin (VIMS1, drain, KI_EOC);
	conpin (VIMS2, agate, KI_EOC);


 	/* Do force and measure */

	if ((fabs(vbs) >= .9e-3) && (subst > 0)) forcev (VIMS3, vbs );
	forcev (VIMS1, vds);
	forcev (VIMS2, vag);
	measi  (VIMS1 , &id);
	execut();

	return(-id);			/*convention: id > 0 when vds > 0. */
}

float my_id1 (int drain, int cg1, int cg2, int source, int tr1, int tr2, int subst, float vgs, float vds, float vbs)
{
	float 	id;		/* local variable */

 	/* Connect device */

	if (subst < 1)	{			/*no substrate */
	    conpth (3, GND, source, tr1, tr2, VIMS1L, VIMS2L, KI_EOC);
	}
	else if (fabs(vbs) < .9e-3) {		/*ground substrate */
	    conpth (3, GND, subst, source, tr1, tr2, VIMS1L, VIMS2L, KI_EOC);
	}
	else {					/*otherwise bias it */
	    conpth (3, GND, source, tr1, tr2, VIMS1L, VIMS2L, VIMS3L, KI_EOC);   
	    conpth (4, VIMS3, subst, KI_EOC);
	}
	conpin (VIMS1, drain, KI_EOC);
	conpin (VIMS2, cg1, cg2, KI_EOC);


 	/* Do force and measure */

	if ((fabs(vbs) >= .9e-3) && (subst > 0)) forcev (VIMS3, vbs );
	forcev (VIMS2, vgs);
	forcev (VIMS1, vds);
	measi  (VIMS1 , &id);
	execut();

	return(-id);			/*convention: id > 0 when vds > 0. */
}
				 
float cplr(int d, int cg1, int cg2, int s, int tr1, int tr2, int sub, int d_ref, int g_ref, int s_ref, int sub_ref )
{
	float  vcg1, vcg2, ids1, ids2, vfg1, vfg2, r3;
	
	vcg1 = 2;
 	vcg2 = 4;

	ids1 = my_id1(d, cg1, cg2, s, tr1, tr2, sub, vcg1, 0.1, 0);
	ids2 = my_id1(d, cg1, cg2, s, tr1, tr2, sub, vcg2, 0.1, 0);
	vfg1 = my_vtati(d_ref, g_ref, s_ref, sub_ref, 0, 5, 0.1, 0, ids1, 10);
	vfg2 = my_vtati(d_ref, g_ref, s_ref, sub_ref, 0, 5, 0.1, 0, ids2, 10);
				
/*	printf("ids1 = %f\n", ids1);
	printf("ids2 = %f\n", ids2);
	printf("vfg1 = %f\n", vfg1);
	printf("vfg2 = %f\n", vfg2);*/

	r3 = (vfg2-vfg1)/(vcg2-vcg1);	
	return(r3);
}

/***********************************************************************************************/
/*			1k ARRAY TESTING ROUTINES			                       */
/***********************************************************************************************/

#define OUT0_1k	 7
#define OUT1_1k	 8 
#define OUT2_1k	 9 
#define OUT3_1k	10 
#define OUT4_1k	11 
#define OUT5_1k	12 
#define OUT6_1k	13 
#define OUT7_1k	14 
#define ROW0_1k	 4 
#define ROW1_1k	 5 
#define ROW2_1k	16 
#define ROW3_1k	17 
#define ROW4_1k	18 
#define COL0_1k  1
#define COL1_1k  2 
#define TR_1k	15 
#define CG_1k	 6 
#define GND_1k	20 
#define VDD_1k	19 
#define SCD_1k	 3 


void calculate_pin_connect(int row, int column, int *high, int *low)
{
  int mask, n, address;
  int row_pad[] = { ROW0_1k, ROW1_1k, ROW2_1k, ROW3_1k, ROW4_1k };
  int col_pad[] = { COL0_1k, COL1_1k };
 
  for(n=0; n<7; n++)
    {
	*(high+n) = *(low+n) = -1;
    }
  mask = 1;
  address = row;
  for(n=0; n<5;n++)
    {
  	if(address&mask)
		*(high+n) = x27(row_pad[n]);
	else
		*(low+n)  = x27(row_pad[n]);
	mask = mask*2;
    } 
  mask = 1;
  address = column;
  for(n=5; n<7;n++)
    {
  	if(address&mask)
		*(high+n) = x27(col_pad[n-5]);
	else
		*(low+n)  = x27(col_pad[n-5]);
	mask = mask*2;
    } 
}

float id_1k_array(int page, int col, int row)
{
	float id;

	int n;
	int high[7], low[7];
	int out[] = { OUT0_1k, OUT1_1k, OUT2_1k, OUT3_1k, OUT4_1k, OUT5_1k, OUT6_1k, OUT7_1k } ;

	calculate_pin_connect(row, col, high, low);

	/* Connect ground */ 
	conpin(x27(CG_1k), x27(TR_1k), x27(GND_1k), low[0], low[1], low[2], low[3], low[4], low[5], low[6], VIMS2L, VIMS3L, GND, KI_EOC);

        /* Connect output */
	conpin(VIMS1H, x27(out[page]), KI_EOC);

        /* Connect 3V */
	conpin(VIMS3H, x27(VDD_1k), high[0], high[1], high[2], high[3], high[4], high[5], high[6], KI_EOC);

  	forcev(VIMS1, 0.1);		
  	forcev(VIMS3, 3);		
	measi(VIMS1, &id);
	execut();
	return -id;
}

float id_1k_array_5V(int page, int col, int row)
{
	float id;

	int n;
	int high[7], low[7];
	int out[] = { OUT0_1k, OUT1_1k, OUT2_1k, OUT3_1k, OUT4_1k, OUT5_1k, OUT6_1k, OUT7_1k } ;

	calculate_pin_connect(row, col, high, low);

	/* Connect ground */ 
	conpin(x27(CG_1k), x27(TR_1k), x27(GND_1k), low[0], low[1], low[2], low[3], low[4], low[5], low[6], VIMS2L, VIMS3L, GND, KI_EOC);

        /* Connect output */
	conpin(VIMS1H, x27(out[page]), KI_EOC);

        /* Connect 5V */
	conpin(VIMS3H, x27(VDD_1k), high[0], high[1], high[2], high[3], high[4], high[5], high[6], KI_EOC);

  	forcev(VIMS1, 0.1);		
  	forcev(VIMS3, 5);		
	measi(VIMS1, &id);
	execut();
	return -id;
}

float id_1k_array_5V_1V(int page, int col, int row)
{
	float id;

	int n;
	int high[7], low[7];
	int out[] = { OUT0_1k, OUT1_1k, OUT2_1k, OUT3_1k, OUT4_1k, OUT5_1k, OUT6_1k, OUT7_1k } ;

	calculate_pin_connect(row, col, high, low);

	/* Connect ground */ 
	conpin(x27(CG_1k), x27(TR_1k), x27(GND_1k), low[0], low[1], low[2], low[3], low[4], low[5], low[6], VIMS2L, VIMS3L, GND, KI_EOC);

        /* Connect output */
	conpin(VIMS1H, x27(out[page]), KI_EOC);

        /* Connect 5V */
	conpin(VIMS3H, x27(VDD_1k), high[0], high[1], high[2], high[3], high[4], high[5], high[6], KI_EOC);

  	forcev(VIMS1, 1.0);		
  	forcev(VIMS3, 5);		
	measi(VIMS1, &id);
	execut();
	return -id;
}

float id_1k_array_3p3V(int page, int col, int row)
{
	float id;

	int n;
	int high[7], low[7];
	int out[] = { OUT0_1k, OUT1_1k, OUT2_1k, OUT3_1k, OUT4_1k, OUT5_1k, OUT6_1k, OUT7_1k } ;

	calculate_pin_connect(row, col, high, low);

	/* Connect ground */ 
	conpin(x27(CG_1k), x27(TR_1k), x27(GND_1k), low[0], low[1], low[2], low[3], low[4], low[5], low[6], VIMS2L, VIMS3L, GND, KI_EOC);

        /* Connect output */
	conpin(VIMS1H, x27(out[page]), KI_EOC);

        /* Connect 3.3V */
	conpin(VIMS3H, x27(VDD_1k), high[0], high[1], high[2], high[3], high[4], high[5], high[6], KI_EOC);

  	forcev(VIMS1, 0.1);		
  	forcev(VIMS3, 3.3);		
	measi(VIMS1, &id);
	execut();
	return -id;
}


float id_1k_array_3p3V_1V(int page, int col, int row)
{
	float id;

	int n;
	int high[7], low[7];
	int out[] = { OUT0_1k, OUT1_1k, OUT2_1k, OUT3_1k, OUT4_1k, OUT5_1k, OUT6_1k, OUT7_1k } ;

	calculate_pin_connect(row, col, high, low);

	/* Connect ground */ 
	conpin(x27(CG_1k), x27(TR_1k), x27(GND_1k), low[0], low[1], low[2], low[3], low[4], low[5], low[6], VIMS2L, VIMS3L, GND, KI_EOC);

        /* Connect output */
	conpin(VIMS1H, x27(out[page]), KI_EOC);

        /* Connect 3.3V */
	conpin(VIMS3H, x27(VDD_1k), high[0], high[1], high[2], high[3], high[4], high[5], high[6], KI_EOC);

  	forcev(VIMS1, 1.0);		
  	forcev(VIMS3, 3.3);		
	measi(VIMS1, &id);
	execut();
	return -id;
}


float vtati_1k_array(int page, int col, int row, float vlow, float vhigh)
{
	float	iltd;			/* measured gate current */
	float 	crtlmt = 10.E-6;	/* SET TO 10E-6 */
	float 	vt;

	int n;
	int high[7], low[7];
	int out[] = { OUT0_1k, OUT1_1k, OUT2_1k, OUT3_1k, OUT4_1k, OUT5_1k, OUT6_1k, OUT7_1k } ;


	calculate_pin_connect(row, col, high, low);

	/* Connect ground */ 
	conpin(x27(TR_1k), x27(GND_1k), low[0], low[1], low[2], low[3], low[4], low[5], low[6], VIMS1L, VIMS2L, VIMS3L, GND, KI_EOC);

        /* Connect output */
	conpin(VIMS1H, x27(out[page]), KI_EOC);

        /* Connect gates */
	conpin(VIMS2H, x27(CG_1k), KI_EOC);

        /* Connect 3V */
	conpin(VIMS3H, x27(VDD_1k), high[0], high[1], high[2], high[3], high[4], high[5], high[6], KI_EOC);

	trigil(VIMS1, -1e-6);
	limiti (VIMS1, 100e-6);
	limiti (VIMS2, crtlmt);
  	forcev(VIMS1, 0.1);		
 	forcev(VIMS3, 3);
	searchv(VIMS2, vlow, vhigh, 10, 1e-3, &vt);
	execut();
	return vt;
}


float vtati_1k_array_5V(int page, int col, int row, float vlow, float vhigh)
{
	float	iltd;			/* measured gate current */
	float 	crtlmt = 10.E-6;	/* SET TO 10E-6 */
	float 	vt;

	int n;
	int high[7], low[7];
	int out[] = { OUT0_1k, OUT1_1k, OUT2_1k, OUT3_1k, OUT4_1k, OUT5_1k, OUT6_1k, OUT7_1k } ;


	calculate_pin_connect(row, col, high, low);

	/* Connect ground */ 
	conpin(x27(TR_1k), x27(GND_1k), low[0], low[1], low[2], low[3], low[4], low[5], low[6], VIMS1L, VIMS2L, VIMS3L, GND, KI_EOC);

        /* Connect output */
	conpin(VIMS1H, x27(out[page]), KI_EOC);

        /* Connect gates */
	conpin(VIMS2H, x27(CG_1k), KI_EOC);

        /* Connect 5V */
	conpin(VIMS3H, x27(VDD_1k), high[0], high[1], high[2], high[3], high[4], high[5], high[6], KI_EOC);

	trigil(VIMS1, -1e-6);
	limiti (VIMS1, 100e-6);
	limiti (VIMS2, crtlmt);
  	forcev(VIMS1, 0.1);		
 	forcev(VIMS3, 5);
	searchv(VIMS2, vlow, vhigh, 10, 1e-3, &vt);
	execut();
	return vt;
}

void vtati_id_flash_array(int page, int col, int row, float vlow, float vhigh, char type, float vss, float vdid, float *vt, float *id)
{
	float	iltd;			/* measured gate current */
	float 	crtlmt = 10.E-6;	/* SET TO 10E-6 */
	float   vdvt;
	float   vttrig;

	int n;
	int high[7], low[7];
	int out[] = { OUT0_1k, OUT1_1k, OUT2_1k, OUT3_1k, OUT4_1k, OUT5_1k, OUT6_1k, OUT7_1k } ;

	if((type == 'N')||(type =='n'))
		{
			vss  = fabs(vss);
			vdid = fabs(vdid);
			vdvt = 0.1;
			vttrig = -1e-6;
		}
	else
		{
			vss  = -1*fabs(vss);
			vdid = -1*fabs(vdid);
			vdvt = -0.1;
			vttrig = 1e-6;
		}

	calculate_pin_connect(row, col, high, low);

	/* Connect ground */ 
	conpin(x27(TR_1k), x27(GND_1k), low[0], low[1], low[2], low[3], low[4], low[5], low[6], VIMS1L, VIMS2L, VIMS3L, GND, KI_EOC);

        /* Connect output */
	conpin(VIMS1H, x27(out[page]), KI_EOC);

        /* Connect gates */
	conpin(VIMS2H, x27(CG_1k), KI_EOC);

        /* Connect 5V */
	conpin(VIMS3H, x27(VDD_1k), high[0], high[1], high[2], high[3], high[4], high[5], high[6], KI_EOC);

	trigil(VIMS1, vttrig);
	limiti (VIMS1, 500e-6);   /* drain */
	limiti (VIMS2, crtlmt);   /* control gate   */
  	forcev(VIMS1, vdvt);		
 	forcev(VIMS3, vss);
	searchv(VIMS2, vlow, vhigh, 10, 1e-3, vt);
  	forcev(VIMS2, 0);		
  	forcev(VIMS1, vdid);		
	measi (VIMS1, id);
	execut();
}

float vtati_1k_array_3p3V(int page, int col, int row, float vlow, float vhigh)
{
	float	iltd;			/* measured gate current */
	float 	crtlmt = 10.E-6;	/* SET TO 10E-6 */
	float 	vt;

	int n;
	int high[7], low[7];
	int out[] = { OUT0_1k, OUT1_1k, OUT2_1k, OUT3_1k, OUT4_1k, OUT5_1k, OUT6_1k, OUT7_1k } ;


	calculate_pin_connect(row, col, high, low);

	/* Connect ground */ 
	conpin(x27(TR_1k), x27(GND_1k), low[0], low[1], low[2], low[3], low[4], low[5], low[6], VIMS1L, VIMS2L, VIMS3L, GND, KI_EOC);

        /* Connect output */
	conpin(VIMS1H, x27(out[page]), KI_EOC);

        /* Connect gates */
	conpin(VIMS2H, x27(CG_1k), KI_EOC);

        /* Connect 3p3V */
	conpin(VIMS3H, x27(VDD_1k), high[0], high[1], high[2], high[3], high[4], high[5], high[6], KI_EOC);

	trigil(VIMS1, -1e-6);
	limiti (VIMS1, 100e-6);
	limiti (VIMS2, crtlmt);
  	forcev(VIMS1, 0.1);		
 	forcev(VIMS3, 3.3);
	searchv(VIMS2, vlow, vhigh, 10, 1e-3, &vt);
	execut();
	return vt;
}


void program_1k_array(float vp, float tp)
{
	/* Connect ground */ 
	conpin(x27(TR_1k), x27(GND_1k), x27(ROW0_1k), x27(ROW1_1k), x27(ROW2_1k), x27(ROW3_1k), x27(ROW4_1k), x27(COL0_1k), x27(COL1_1k), x27(VDD_1k), VIMS1L, GND, KI_EOC);

        /* Connect gates */
	conpin(VIMS1H, x27(CG_1k), KI_EOC);

	pulsev(VIMS1, vp, tp*1e-3);

	execut();
}


void erase_1k_array(float ve, float te)
{
	/* Connect ground */ 
	conpin(x27(CG_1k), x27(GND_1k), x27(ROW0_1k), x27(ROW1_1k), x27(ROW2_1k), x27(ROW3_1k), x27(ROW4_1k), x27(COL0_1k), x27(COL1_1k), x27(VDD_1k), VIMS1L, GND, KI_EOC);

        /* Connect tunneling regions */
	conpin(VIMS1H, x27(TR_1k), KI_EOC);

	pulsev(VIMS1, ve, te*1e-3);

	execut();
}


float  debug_32x32(int page, int col, int row)
{
	float 	vt;

	int n;
	int high[7], low[7];
	int out[] = { OUT0_1k, OUT1_1k, OUT2_1k, OUT3_1k, OUT4_1k, OUT5_1k, OUT6_1k, OUT7_1k } ;


	calculate_pin_connect(row, col, high, low);

	/* Connect ground */ 
	conpin(x27(CG_1k), x27(TR_1k), x27(GND_1k), low[0], low[1], low[2], low[3], low[4], low[5], low[6], VIMS1L, VIMS2L, VIMS3L, GND, KI_EOC);

        /* Connect output */
	conpin(VIMS1H, x27(out[page]), KI_EOC);

        /* Connect internal node */
	conpin(VIMS2H, x27(SCD_1k), KI_EOC);

        /* Connect 5V */
	conpin(VIMS3H, x27(VDD_1k), high[0], high[1], high[2], high[3], high[4], high[5], high[6], KI_EOC);

        setvmtr(VIMS2);
  	forcev(VIMS1, 3.3); 
 	forcev(VIMS3, 3.3);
	measv(VIMS2, &vt);
	execut();
	return vt;
}

void program_1k_array_II(float vp, int tp)
{
	int n, numpts;
	float vfrc[256];

	/* calculate array for rise time */
	numpts = (int)(vp/.2)+1;
	for (n=0;n<numpts;n++)
		vfrc[n] = (n+1)*.2;

	/* Connect ground */ 
	conpin(x27(TR_1k), x27(GND_1k), x27(ROW0_1k), x27(ROW1_1k), x27(ROW2_1k), x27(ROW3_1k), x27(ROW4_1k), x27(COL0_1k), x27(COL1_1k), x27(VDD_1k), VIMS1L, GND, KI_EOC);

        /* Connect gates */
	conpin(VIMS1H, x27(CG_1k), KI_EOC);

        asweepv(VIMS1, numpts, 10e-6, vfrc);
	forcev(VIMS1, vp);
	delay(tp);

	execut();
}

void erase_1k_array_II(float ve, int te)
{
	int n, numpts;
	float vfrc[256];

	/* calculate array for rise time */
	numpts = (int)(ve/.2)+1;
	for (n=0;n<numpts;n++)
		vfrc[n] = (n+1)*.2;

	/* Connect ground */ 
	conpin(x27(CG_1k), x27(GND_1k), x27(ROW0_1k), x27(ROW1_1k), x27(ROW2_1k), x27(ROW3_1k), x27(ROW4_1k), x27(COL0_1k), x27(COL1_1k), x27(VDD_1k), VIMS1L, GND, KI_EOC);

        /* Connect tunneling regions */
	conpin(VIMS1H, x27(TR_1k), KI_EOC);

        asweepv(VIMS1, numpts, 10e-6, vfrc);
	forcev(VIMS1, ve);
	delay(te);

	execut();
}

void cycle_1k_array_II(float vp, int tp)
{
	int n, numpts;
	float vfrc[256];

	/* calculate array for rise time */
	numpts = (int)(vp/.2)+1;
	for (n=0;n<numpts;n++)
		vfrc[n] = (n+1)*.2;

	/* Connect ground */ 
	conpin(x27(GND_1k), x27(ROW0_1k), x27(ROW1_1k), x27(ROW2_1k), x27(ROW3_1k), x27(ROW4_1k), x27(COL0_1k), x27(COL1_1k), x27(VDD_1k), VIMS1L, VIMS2L, GND, KI_EOC);

        /* Connect gates */
	conpin(VIMS1H, x27(CG_1k), KI_EOC);

        /* Connect TRs */
	conpin(VIMS2H, x27(TR_1k), KI_EOC);

	forcev(VIMS1, 0);
        asweepv(VIMS2, numpts, 10e-6, vfrc);
	forcev(VIMS2, vp);
	delay(tp);
	forcev(VIMS2, 0);
        asweepv(VIMS1, numpts, 10e-6, vfrc);
	forcev(VIMS1, vp);
	delay(tp);

	execut();
}

