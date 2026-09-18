/* 
    
    lptinstr_lowercase.h 
    
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
 
$Source: /cm/test/build/S600/v420/PROJCOM/RCS/lptinstr_lowercase.h,v $
$Revision: 1.3 $
Rev $Date: 2000/02/17 20:18:54 $

*/

#ifndef LPTINSTR_LOWERCASE_H
#define LPTINSTR_LOWERCASE_H

#include <lptinstr.h>

#define ki_eoc 		KI_EOC        
#define ki_noinstr 	KI_NOINSTR        

#define minname 	MINNAME

#define chuck		CHUCK
#define chuckf      CHUCKF
#define chuckm      CHUCKM
#define chuckg      CHUCKG
#define ki_system	KI_SYSTEM

#define smu1        	SMU1     
#define smu1h       	SMU1H
#define smu1l		SMU1L
#define smu2		SMU2
#define smu2h       	SMU2H
#define smu2l		SMU2L
#define smu3		SMU3
#define smu3h       	SMU3H
#define smu3l		SMU3L
#define smu4		SMU4
#define smu4h       	SMU4H
#define smu4l		SMU4L
#define smu5		SMU5
#define smu5h       	SMU5H
#define smu5l		SMU5L
#define smu6		SMU6
#define smu6h       	SMU6H
#define smu6l		SMU6L
#define smu7		SMU7
#define smu7h       	SMU7H
#define smu7l		SMU7L
#define smu8		SMU8
#define smu8h       	SMU8H
#define smu8l		SMU8L

#define cmtr1       	CMTR1     
#define cmtr1h      	CMTR1H
#define cmtr1l		CMTR1L
#define cmtr1v		CMTR1V
#define cmtr1r		CMTR1R
#define cmtr2		CMTR2
#define cmtr2h      	CMTR2H
#define cmtr2l		CMTR2L
#define cmtr2v		CMTR2V
#define cmtr2r		CMTR2R
#define cmtr3		CMTR3
#define cmtr3h      	CMTR3H
#define cmtr3l		CMTR3L
#define cmtr3v		CMTR3V
#define cmtr3r		CMTR3R
#define cmtr4		CMTR4
#define cmtr4h      	CMTR4H
#define cmtr4l		CMTR4L
#define cmtr4v		CMTR4V
#define cmtr4r		CMTR4R

#define imtr1       	IMTR1
#define imtr1h      	IMTR1H
#define imtr1l		IMTR1L
#define imtr2		IMTR2
#define imtr2h      	IMTR2H
#define imtr2l		IMTR2L
#define imtr3		IMTR3
#define imtr3h      	IMTR3H
#define imtr3l		IMTR3L
#define imtr4		IMTR4
#define imtr4v      	IMTR4V
#define imtr4r		IMTR4R

#define vmtr1       	VMTR1
#define vmtr1h      	VMTR1H
#define vmtr1l		VMTR1L
#define vmtr2		VMTR2
#define vmtr2h      	VMTR2H
#define vmtr2l		VMTR2L
#define vmtr3		VMTR3
#define vmtr3h      	VMTR3H
#define vmtr3l		VMTR3L
#define vmtr4		VMTR4
#define vmtr4h      	VMTR4H
#define vmtr4l		VMTR4L

#define vsrc1       	VSRC1
#define vsrc1h      	VSRC1H
#define vsrc1l		VSRC1L
#define vsrc2		VSRC2
#define vsrc2h      	VSRC2H
#define vsrc2l		VSRC2L
#define vsrc3		VSRC3
#define vsrc3h      	VSRC3H
#define vsrc3l		VSRC3L
#define vsrc4		VSRC4	
#define vsrc4h      	VSRC4H
#define vsrc4l		VSRC4L

#define isrc1       	ISRC1
#define isrc1h      	ISRC1H
#define isrc1l		ISRC1L
#define isrc2		ISRC2
#define isrc2h      	ISRC2H
#define isrc2l		ISRC2L

#define sio1       	SIO1    
#define sio2		SIO2
#define sio3		SIO3
#define sio4		SIO4
#define sio5		SIO5
#define sio6		SIO6

#define pio1        	PIO1     
#define pio2		PIO2
#define pio3		PIO3
#define pio4		PIO4
#define pio5		PIO5
#define pio6		PIO6
#define pio7		PIO7
#define pio8		PIO8
#define pio9		PIO9
#define pio10		PIO10


#define timer1      	TIMER1     
#define timer2		TIMER2
#define timer3		TIMER3
#define timer4		TIMER4

#define fake        	FAKE
#define fake1       	FAKE1
#define fake2		FAKE2
#define fake3		FAKE3
#define fake4		FAKE4

#define matrix1     	MATRIX1
#define matrix2    	MATRIX2
#define matrix3		MATRIX3
#define matrix4		MATRIX4

#define gpib1       	GPIB1
#define gpib2		GPIB2
#define gpib3		GPIB3
#define gpib4		GPIB4

#define fohm1       	FOHM1
#define fohm2		FOHM2
#define fohm3		FOHM3
#define fohm4		FOHM4
#define fohm5		FOHM5
#define fohm6		FOHM6
#define fohm7		FOHM7
#define fohm8		FOHM8
#define fohmterm	FOHMTERM

#define unused_pins UNUSED_PINS

#define vhi1    	VHI1         
#define mdb1		MDB1
#define paa1		PAA1
#define paa2		PAA2
#define paa3		PAA3
#define paa4		PAA4
#define paa5		PAA5
#define paa6		PAA6
#define paa7		PAA7
#define paa8		PAA8
#define paa9		PAA9
#define paa10		PAA10
#define paa11		PAA11
#define paa12		PAA12
#define paa13		PAA13
#define paa14		PAA14
#define paa15		PAA15
#define paa16		PAA16
#define sru1		SRU1
#define pca1		PCA1
#define pca2		PCA2
#define pca3		PCA3
#define pca4		PCA4
#define led1		LED1
#define led2		LED2
#define led3		LED3
#define led4		LED4
#define pae1		PAE1
#define pae2		PAE2
#define pae3		PAE3
#define pae4		PAE4
#define pae5		PAE5
#define pae6		PAE6
#define pae7		PAE7
#define pae8		PAE8
#define pae9		PAE9
#define pae10		PAE10
#define pae11		PAE11
#define pae12		PAE12
#define pae13		PAE13
#define pae14		PAE14
#define pae15		PAE15
#define pae16		PAE16

#define preamp1 	PREAMP1            
#define preamp2		PREAMP2
#define preamp3		PREAMP3
#define preamp4		PREAMP4
#define preamp5		PREAMP5
#define preamp6		PREAMP6
#define preamp7		PREAMP7
#define preamp8		PREAMP8
#define preamp9		PREAMP9
#define preamp10	PREAMP10
#define preamp11	PREAMP11
#define preamp12	PREAMP12
#define preamp13	PREAMP13
#define preamp14	PREAMP14
#define preamp15	PREAMP15
#define preamp16	PREAMP16
#define preamp17	PREAMP17
#define preamp18	PREAMP18
#define preamp19	PREAMP19
#define preamp20	PREAMP20
#define preamp21	PREAMP21
#define preamp22	PREAMP22
#define preamp23	PREAMP23
#define preamp24	PREAMP24
#define preamp25	PREAMP25
#define preamp26	PREAMP26
#define preamp27	PREAMP27
#define preamp28	PREAMP28
#define preamp29	PREAMP29
#define preamp30	PREAMP30
#define preamp31	PREAMP31
#define preamp32	PREAMP32
#define preamp33	PREAMP33
#define preamp34	PREAMP34
#define preamp35	PREAMP35
#define preamp36	PREAMP36
#define preamp37	PREAMP37
#define preamp38	PREAMP38
#define preamp39	PREAMP39
#define preamp40	PREAMP40
#define preamp41	PREAMP41
#define preamp42	PREAMP42
#define preamp43	PREAMP43
#define preamp44	PREAMP44
#define preamp45	PREAMP45
#define preamp46	PREAMP46
#define preamp47	PREAMP47
#define preamp48	PREAMP48
#define preamp49	PREAMP49
#define preamp50	PREAMP50
#define preamp51	PREAMP51
#define preamp52	PREAMP52
#define preamp53	PREAMP53
#define preamp54	PREAMP54
#define preamp55	PREAMP55
#define preamp56	PREAMP56
#define preamp57	PREAMP57
#define preamp58	PREAMP58
#define preamp59	PREAMP59
#define preamp60	PREAMP60
#define preamp61	PREAMP61
#define preamp62	PREAMP62
#define preamp63	PREAMP63
#define preamp64	PREAMP64
#define preamp65	PREAMP65
#define preamp66	PREAMP66
#define preamp67	PREAMP67
#define preamp68	PREAMP68
#define preamp69	PREAMP69
#define preamp70	PREAMP70
#define preamp71	PREAMP71
#define preamp72	PREAMP72
#define preamp73	PREAMP73
#define preamp74	PREAMP74
#define preamp75	PREAMP75
#define preamp76	PREAMP76
#define preamp77	PREAMP77
#define preamp78	PREAMP78
#define preamp79	PREAMP79
#define preamp80	PREAMP80
#define preamp81	PREAMP81
#define preamp82	PREAMP82
#define preamp83	PREAMP83
#define preamp84	PREAMP84
#define preamp85	PREAMP85
#define preamp86	PREAMP86
#define preamp87	PREAMP87
#define preamp88	PREAMP88
#define preamp89	PREAMP89
#define preamp90	PREAMP90
#define preamp91	PREAMP91
#define preamp92	PREAMP92
#define preamp93	PREAMP93
#define preamp94	PREAMP94
#define preamp95	PREAMP95
#define preamp96	PREAMP96
#define preamp97	PREAMP97
#define preamp98	PREAMP98
#define preamp99	PREAMP99
#define preamp100	PREAMP100
#define preamp101	PREAMP101
#define preamp102	PREAMP102
#define preamp103	PREAMP103
#define preamp104	PREAMP104
#define preamp105	PREAMP105
#define preamp106	PREAMP106
#define preamp107	PREAMP107
#define preamp108	PREAMP108
#define preamp109	PREAMP109
#define preamp110	PREAMP110
#define preamp111	PREAMP111
#define preamp112	PREAMP112
#define preamp113	PREAMP113
#define preamp114	PREAMP114
#define preamp115	PREAMP115
#define preamp116	PREAMP116
#define preamp117	PREAMP117
#define preamp118	PREAMP118
#define preamp119	PREAMP119
#define preamp120	PREAMP120
#define preamp121	PREAMP121
#define preamp122	PREAMP122
#define preamp123	PREAMP123
#define preamp124	PREAMP124
#define preamp125	PREAMP125
#define preamp126	PREAMP126
#define preamp127	PREAMP127
#define preamp128	PREAMP128
#define preamp129	PREAMP129
#define preamp130	PREAMP130
#define preamp131	PREAMP131
#define preamp132	PREAMP132
#define preamp133	PREAMP133
#define preamp134	PREAMP134
#define preamp135	PREAMP135
#define preamp136	PREAMP136
#define preamp137	PREAMP137
#define preamp138	PREAMP138
#define preamp139	PREAMP139
#define preamp140	PREAMP140
#define preamp141	PREAMP141
#define preamp142	PREAMP142
#define preamp143	PREAMP143
#define preamp144	PREAMP144
#define preamp145	PREAMP145
#define preamp146	PREAMP146
#define preamp147	PREAMP147
#define preamp148	PREAMP148
#define preamp149	PREAMP149
#define preamp150	PREAMP150
#define preamp151	PREAMP151
#define preamp152	PREAMP152
#define preamp153	PREAMP153
#define preamp154	PREAMP154
#define preamp155	PREAMP155
#define preamp156	PREAMP156
#define preamp157	PREAMP157
#define preamp158	PREAMP158
#define preamp159	PREAMP159
#define preamp160	PREAMP160
#define preamp161	PREAMP161
#define preamp162	PREAMP162
#define preamp163	PREAMP163
#define preamp164	PREAMP164
#define preamp165	PREAMP165
#define preamp166	PREAMP166
#define preamp167	PREAMP167
#define preamp168	PREAMP168
#define preamp169	PREAMP169
#define preamp170	PREAMP170
#define preamp171	PREAMP171
#define preamp172	PREAMP172
#define preamp173	PREAMP173
#define preamp174	PREAMP174
#define preamp175	PREAMP175
#define preamp176	PREAMP176
#define preamp177	PREAMP177
#define preamp178	PREAMP178
#define preamp179	PREAMP179
#define preamp180	PREAMP180
#define preamp181	PREAMP181
#define preamp182	PREAMP182
#define preamp183	PREAMP183
#define preamp184	PREAMP184
#define preamp185	PREAMP185
#define preamp186	PREAMP186
#define preamp187	PREAMP187
#define preamp188	PREAMP188
#define preamp189	PREAMP189
#define preamp190	PREAMP190
#define preamp191	PREAMP191
#define preamp192	PREAMP192
#define preamp193	PREAMP193
#define preamp194	PREAMP194
#define preamp195	PREAMP195
#define preamp196	PREAMP196
#define preamp197	PREAMP197
#define preamp198	PREAMP198
#define preamp199	PREAMP199
#define preamp200	PREAMP200
#define preamp201	PREAMP201
#define preamp202	PREAMP202
#define preamp203	PREAMP203
#define preamp204	PREAMP204
#define preamp205	PREAMP205
#define preamp206	PREAMP206
#define preamp207	PREAMP207
#define preamp208	PREAMP208
#define preamp209	PREAMP209
#define preamp210	PREAMP210
#define preamp211	PREAMP211
#define preamp212	PREAMP212
#define preamp213	PREAMP213
#define preamp214	PREAMP214
#define preamp215	PREAMP215
#define preamp216	PREAMP216
#define preamp217	PREAMP217
#define preamp218	PREAMP218
#define preamp219	PREAMP219
#define preamp220	PREAMP220
#define preamp221	PREAMP221
#define preamp222	PREAMP222
#define preamp223	PREAMP223
#define preamp224	PREAMP224
#define preamp225	PREAMP225
#define preamp226	PREAMP226
#define preamp227	PREAMP227
#define preamp228	PREAMP228
#define preamp229	PREAMP229
#define preamp230	PREAMP230
#define preamp231	PREAMP231
#define preamp232	PREAMP232
#define preamp233	PREAMP233
#define preamp234	PREAMP234
#define preamp235	PREAMP235
#define preamp236	PREAMP236
#define preamp237	PREAMP237
#define preamp238	PREAMP238
#define preamp239	PREAMP239
#define preamp240	PREAMP240
#define preamp241	PREAMP241
#define preamp242	PREAMP242
#define preamp243	PREAMP243
#define preamp244	PREAMP244
#define preamp245	PREAMP245
#define preamp246	PREAMP246
#define preamp247	PREAMP247
#define preamp248	PREAMP248
#define preamp249	PREAMP249
#define preamp250	PREAMP250
#define preamp251	PREAMP251
#define preamp252	PREAMP252
#define preamp253	PREAMP253
#define preamp254	PREAMP254
#define preamp255	PREAMP255
#define preamp256	PREAMP256

#endif

