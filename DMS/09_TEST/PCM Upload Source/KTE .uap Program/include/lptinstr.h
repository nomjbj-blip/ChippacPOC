/* 
    
    lptinstr.h 
    
    Instrument identification codes.
    Linear Parametric Test Library.


    Copyright (c) 1986-1989, 1993, 1996 by Keithley Instruments, 
    Inc. Cleveland, Ohio
     
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
 
$Source: /cm/test/build/S600/v420/PROJCOM/RCS/lptinstr.h,v $
$Revision: 1.21 $
Rev $Date: 2000/02/17 20:18:54 $

 Change History
 * $Log: lptinstr.h,v $
 * Revision 1.21  2000/02/17 20:18:54  hayes
 * PR11500 Added CHUCKG and CHUCKM pseudo-terminal identifiers.
 *
 * Revision 1.20  1999/01/26 13:57:54  hayes
 * PR02520 Added 60230-THP, 60130 PIO and 60230 PIO support.
 *
 * Revision 1.19  1998/11/30 17:43:39  hayes
 * Added Ids and model codes for 60260PAE and 60211PAA.
 *
 * Revision 1.18  1997/10/21 21:43:42  hayes
 * PR04692  Added ES1 through ES4 instrument ids.  Also added BEEPER1
 * through BEEPER4.
 *
 * Revision 1.17  1997/04/23 18:23:19  hayes
 * Added KI_UNKINS unknown instrument ID.
 *
 * Revision 1.16  1997/01/28 21:02:52  williamson
 * Changed c++ style comments to normal C comments
 *
 * Revision 1.15  1997/01/25  02:04:50  hayes
 * Added LED instrument(s).
 *
 * Revision 1.14  1996/12/11 15:57:41  hayes
 * Added floatpin.  Changed preamp IDs to be their pin numbers.
 *
 * Revision 1.13  1996/10/08 22:17:44  hayes
 * Formatting changes.  Hid or removed // style comments.
 * 
 * Revision 1.12  1996/09/27  13:33:08  furio
 * Added SRU1 and PCA1 components to instrument list.
 *
 * Revision 1.11  1996/08/09  19:04:13  hayes
 * Put MINNAME back.
 *
 * Revision 1.10  1996/08/09  17:48:03  hayes
 * Rearange IDs and add IDs for FOHM inputs.  IDs are now all enums and
 * as compatible with the S400 as is reasonable.
 *
 * Revision 1.9  1996/07/29  20:36:51  chaplin
 * added GPIB1-4 instruments
 *
 * Revision 1.8  1996/06/05  20:57:27  chaplin
 * added instrument IDs for 256 PREAMP instruments
 *
 * Revision 1.7  1996/04/01  14:22:55  hayes
 * First working MATRIX driver.
 *
 * Revision 1.6  1996/02/14  20:48:31  hayes
 * Add instrument IDs for hardware only (no dispatch table) devices.
 *
 * Revision 1.5  1996/01/22  22:19:16  hayes
 * Added TIMER1 and TIMER2
 *
 * Revision 1.4  1996/01/10  18:59:12  hayes
 * Added FAKE instrument name.
 *
 * Revision 1.3  1996/01/02  20:14:11  chaplin
 * added instrument ID for MATRIX
 *
 * Revision 1.2  1995/08/22  19:29:24  chaplin
 * Got rid of all the TCLib on Instruments defines, left in all the new
 * S600 defines.
 * Also, changed a PVCS keyword to its matching RCS keyword (I can't say
 * here what it is, or else RCS will expand it in my comment, heh heh).
 *
 * Revision 1.1  1995/08/14  16:03:36  hayes
 * Initial revision
 *
*/

#ifndef LPTINSTR_H
#define LPTINSTR_H

enum instruments
{
    KI_EOC      = 0,        /* End of connections */
    KI_NOINSTR  = 0,        /* No instrument */

    MINNAME     = 4096,
    GND         = MINNAME,  /* S400 compatible */
    gnd         = GND,

    CHUCK,
    CHUCKF      = CHUCK,
    KI_SYSTEM,

    SMU1        = 4100,     /* VIMS1 on S400 */
    SMU1H       = SMU1,
    SMU1L,
    SMU2,
    SMU2H       = SMU2,
    SMU2L,
    SMU3,
    SMU3H       = SMU3,
    SMU3L,
    SMU4,
    SMU4H       = SMU4,
    SMU4L,
    SMU5,
    SMU5H       = SMU5,
    SMU5L,
    SMU6,
    SMU6H       = SMU6,
    SMU6L,
    SMU7,
    SMU7H       = SMU7,
    SMU7L,
    SMU8,
    SMU8H       = SMU8,
    SMU8L,

    CMTR1       = 4116,     /* S400 compatible */
    CMTR1H      = CMTR1,
    CMTR1L,
    CMTR1V,
    CMTR1R,
    CMTR2,
    CMTR2H      = CMTR2,
    CMTR2L,
    CMTR2V,
    CMTR2R,
    CMTR3,
    CMTR3H      = CMTR3,
    CMTR3L,
    CMTR3V,
    CMTR3R,
    CMTR4,
    CMTR4H      = CMTR4,
    CMTR4L,
    CMTR4V,
    CMTR4R,

    IMTR1       = 4132,
    IMTR1H      = IMTR1,
    IMTR1L,
    IMTR2,
    IMTR2H      = IMTR2,
    IMTR2L,
    IMTR3,
    IMTR3H      = IMTR3,
    IMTR3L,
    IMTR4,
    IMTR4H      = IMTR4,
    IMTR4L,

    VMTR1       = 4140,
    VMTR1H      = VMTR1,
    VMTR1L,
    VMTR2,
    VMTR2H      = VMTR2,
    VMTR2L,
    VMTR3,
    VMTR3H      = VMTR3,
    VMTR3L,
    VMTR4,
    VMTR4H      = VMTR4,
    VMTR4L,

    /* on S400 8 VSRC IDs from 4148 through 4155 but did not include lows */
    VSRC1       = 4148,
    VSRC1H      = VSRC1,
    VSRC1L,
    VSRC2,
    VSRC2H      = VSRC2,
    VSRC2L,
    VSRC3,
    VSRC3H      = VSRC3,
    VSRC3L,
    VSRC4,
    VSRC4H      = VSRC4,
    VSRC4L,

    /* on S400 4 ISRC IDs from 4156 through 4159 but did not include lows */
    ISRC1       = 4156,
    ISRC1H      = ISRC1,
    ISRC1L,
    ISRC2,
    ISRC2H      = ISRC2,
    ISRC2L,

#if 0
    // on S400 GPT  IDs from 4160 through 4167
    // on S400 FMTR IDs from 4168 through 4171
    // on S400 SYMTR1 IDs at 4172 and 4173
#endif
    
    SIO1        = 4174,     /* S400 compatible */
    SIO2,
    SIO3,
    SIO4,
    SIO5,
    SIO6,

    /* NOTE on S400, ACCU1 used same IDs as SIO5 and SIO6 */

    PIO1        = 4180,     /* S400 compatible */
    PIO2,
    PIO3,
    PIO4,
    PIO5,
    PIO6,
    PIO7,
    PIO8,
    PIO9,
    PIO10,

    /* S400 has a bunch of stuff in this block */

    TIMER1      = 4256,     /* S400 compatible */
    TIMER2,
    TIMER3,
    TIMER4,

    FAKE        = 4260,
    FAKE1       = FAKE,
    FAKE2,
    FAKE3,
    FAKE4,

    MATRIX1     = 4264,
    MATRIX2,    /* reserve a couple for additional matricies */
    MATRIX3,
    MATRIX4,

    GPIB1       = 4268,
    GPIB2,
    GPIB3,
    GPIB4,

    FOHM1       = 4272,
    FOHM2,
    FOHM3,
    FOHM4,
    FOHM5       = FOHM3,
    FOHM6       = FOHM4,
    FOHM7,
    FOHM8,
    FOHMTERM,

    CHUCKM      = 4500,
    CHUCKG,

    UNUSED_PINS = 5000,     /* Represents all unused pins */

    KI_UNKINS   = 5120,

    BEEPER1     = 5121,     /* Beepers */
    BEEPER2,
    BEEPER3,
    BEEPER4,

    ES1         = 5125,     /* Environment sensors */
    ES2,
    ES3,
    ES4
};

enum hardware_only_instruments
{
    VHI1    = 8192,         /* Vxi Head Interface */
    MDB1,                   /* Matrix/Diagnostic Board */
    PAA1,                   /* PreAmp Assembly in slot 1 */
    PAA2,                   /* PreAmp Assembly in slot 2 */
    PAA3,                   /* PreAmp Assembly in slot 3 */
    PAA4,                   /* PreAmp Assembly in slot 4 */
    PAA5,                   /* PreAmp Assembly in slot 5 */
    PAA6,                   /* PreAmp Assembly in slot 6 */
    PAA7,                   /* PreAmp Assembly in slot 7 */
    PAA8,                   /* PreAmp Assembly in slot 8 */
    PAA9,                   /* PreAmp Assembly in slot 9 */
    PAA10,                  /* PreAmp Assembly in slot 10 */
    PAA11,                  /* PreAmp Assembly in slot 11 */
    PAA12,                  /* PreAmp Assembly in slot 12 */
    PAA13,                  /* PreAmp Assembly in slot 13 */
    PAA14,                  /* PreAmp Assembly in slot 14 */
    PAA15,                  /* PreAmp Assembly in slot 15 */
    PAA16,                  /* PreAmp Assembly in slot 16 */
    SRU1,
    PCA1,                   /* The only PCA card supported */
    PCA2,                   /* In case we support more later */
    PCA3,
    PCA4,
    LED1,                   /* The only LED status panel */
    LED2,                   /* In case there are more later */
    LED3,
    LED4,
    PAE1,                   /* PreAmp Extender in slot 1 */
    PAE2,                   /* PreAmp Extender in slot 2 */
    PAE3,                   /* PreAmp Extender in slot 3 */
    PAE4,                   /* PreAmp Extender in slot 4 */
    PAE5,                   /* PreAmp Extender in slot 5 */
    PAE6,                   /* PreAmp Extender in slot 6 */
    PAE7,                   /* PreAmp Extender in slot 7 */
    PAE8,                   /* PreAmp Extender in slot 8 */
    PAE9,                   /* PreAmp Extender in slot 9 */
    PAE10,                  /* PreAmp Extender in slot 10 */
    PAE11,                  /* PreAmp Extender in slot 11 */
    PAE12,                  /* PreAmp Extender in slot 12 */
    PAE13,                  /* PreAmp Extender in slot 13 */
    PAE14,                  /* PreAmp Extender in slot 14 */
    PAE15,                  /* PreAmp Extender in slot 15 */
    PAE16,                  /* PreAmp Extender in slot 16 */
    THP1                    /* Test Head Processor */
};

enum Preamps
{
    PREAMP1 = 1,            /* First individual preamp */
    PREAMP2,                /* Next individual preamp, and so on */
    PREAMP3,
    PREAMP4,
    PREAMP5,
    PREAMP6,
    PREAMP7,
    PREAMP8,
    PREAMP9,
    PREAMP10,
    PREAMP11,
    PREAMP12,
    PREAMP13,
    PREAMP14,
    PREAMP15,
    PREAMP16,
    PREAMP17,
    PREAMP18,
    PREAMP19,
    PREAMP20,
    PREAMP21,
    PREAMP22,
    PREAMP23,
    PREAMP24,
    PREAMP25,
    PREAMP26,
    PREAMP27,
    PREAMP28,
    PREAMP29,
    PREAMP30,
    PREAMP31,
    PREAMP32,
    PREAMP33,
    PREAMP34,
    PREAMP35,
    PREAMP36,
    PREAMP37,
    PREAMP38,
    PREAMP39,
    PREAMP40,
    PREAMP41,
    PREAMP42,
    PREAMP43,
    PREAMP44,
    PREAMP45,
    PREAMP46,
    PREAMP47,
    PREAMP48,
    PREAMP49,
    PREAMP50,
    PREAMP51,
    PREAMP52,
    PREAMP53,
    PREAMP54,
    PREAMP55,
    PREAMP56,
    PREAMP57,
    PREAMP58,
    PREAMP59,
    PREAMP60,
    PREAMP61,
    PREAMP62,
    PREAMP63,
    PREAMP64,
    PREAMP65,
    PREAMP66,
    PREAMP67,
    PREAMP68,
    PREAMP69,
    PREAMP70,
    PREAMP71,
    PREAMP72,
    PREAMP73,
    PREAMP74,
    PREAMP75,
    PREAMP76,
    PREAMP77,
    PREAMP78,
    PREAMP79,
    PREAMP80,
    PREAMP81,
    PREAMP82,
    PREAMP83,
    PREAMP84,
    PREAMP85,
    PREAMP86,
    PREAMP87,
    PREAMP88,
    PREAMP89,
    PREAMP90,
    PREAMP91,
    PREAMP92,
    PREAMP93,
    PREAMP94,
    PREAMP95,
    PREAMP96,
    PREAMP97,
    PREAMP98,
    PREAMP99,
    PREAMP100,
    PREAMP101,
    PREAMP102,
    PREAMP103,
    PREAMP104,
    PREAMP105,
    PREAMP106,
    PREAMP107,
    PREAMP108,
    PREAMP109,
    PREAMP110,
    PREAMP111,
    PREAMP112,
    PREAMP113,
    PREAMP114,
    PREAMP115,
    PREAMP116,
    PREAMP117,
    PREAMP118,
    PREAMP119,
    PREAMP120,
    PREAMP121,
    PREAMP122,
    PREAMP123,
    PREAMP124,
    PREAMP125,
    PREAMP126,
    PREAMP127,
    PREAMP128,
    PREAMP129,
    PREAMP130,
    PREAMP131,
    PREAMP132,
    PREAMP133,
    PREAMP134,
    PREAMP135,
    PREAMP136,
    PREAMP137,
    PREAMP138,
    PREAMP139,
    PREAMP140,
    PREAMP141,
    PREAMP142,
    PREAMP143,
    PREAMP144,
    PREAMP145,
    PREAMP146,
    PREAMP147,
    PREAMP148,
    PREAMP149,
    PREAMP150,
    PREAMP151,
    PREAMP152,
    PREAMP153,
    PREAMP154,
    PREAMP155,
    PREAMP156,
    PREAMP157,
    PREAMP158,
    PREAMP159,
    PREAMP160,
    PREAMP161,
    PREAMP162,
    PREAMP163,
    PREAMP164,
    PREAMP165,
    PREAMP166,
    PREAMP167,
    PREAMP168,
    PREAMP169,
    PREAMP170,
    PREAMP171,
    PREAMP172,
    PREAMP173,
    PREAMP174,
    PREAMP175,
    PREAMP176,
    PREAMP177,
    PREAMP178,
    PREAMP179,
    PREAMP180,
    PREAMP181,
    PREAMP182,
    PREAMP183,
    PREAMP184,
    PREAMP185,
    PREAMP186,
    PREAMP187,
    PREAMP188,
    PREAMP189,
    PREAMP190,
    PREAMP191,
    PREAMP192,
    PREAMP193,
    PREAMP194,
    PREAMP195,
    PREAMP196,
    PREAMP197,
    PREAMP198,
    PREAMP199,
    PREAMP200,
    PREAMP201,
    PREAMP202,
    PREAMP203,
    PREAMP204,
    PREAMP205,
    PREAMP206,
    PREAMP207,
    PREAMP208,
    PREAMP209,
    PREAMP210,
    PREAMP211,
    PREAMP212,
    PREAMP213,
    PREAMP214,
    PREAMP215,
    PREAMP216,
    PREAMP217,
    PREAMP218,
    PREAMP219,
    PREAMP220,
    PREAMP221,
    PREAMP222,
    PREAMP223,
    PREAMP224,
    PREAMP225,
    PREAMP226,
    PREAMP227,
    PREAMP228,
    PREAMP229,
    PREAMP230,
    PREAMP231,
    PREAMP232,
    PREAMP233,
    PREAMP234,
    PREAMP235,
    PREAMP236,
    PREAMP237,
    PREAMP238,
    PREAMP239,
    PREAMP240,
    PREAMP241,
    PREAMP242,
    PREAMP243,
    PREAMP244,
    PREAMP245,
    PREAMP246,
    PREAMP247,
    PREAMP248,
    PREAMP249,
    PREAMP250,
    PREAMP251,
    PREAMP252,
    PREAMP253,
    PREAMP254,
    PREAMP255,
    PREAMP256
};

#endif

