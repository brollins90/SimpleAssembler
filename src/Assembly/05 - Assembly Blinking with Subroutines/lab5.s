MOVW sp, 0x9000 // start stack at 0x90000

MOVW v1, 0x0
MOVT v1, 0x3f20 // put 0x3f200000 in v1
MOVW v2, 0x0
MOVT v2, 0x20   // put 0x200000 in v2
MOVW v3, 0x8000 // put 0x8000 in v3

STR v2, v1, 0x10 // enable gpio

loop: STR v3, v1, 0x20 // turn on led

MOVW a1, 0x0
MOVT a1, 0x10 // put 0x100000 in a1
STRDB v4, sp, 0x0
BL subroutine
//LDRIA pop variables

STR v3, v1, 0x2c // turn off led
MOVW a1, 0x0
MOVT a1, 0x10 // put 0x100000 in a1
STRDB v4, sp, 0x0
BL subroutine
//LDRIA pop variables

BAL loop


subroutine: LDRIA a3, sp, 0x0

wait: SUBS a3, a3, 0x01
BNE wait
MOV pc, lr
