MOVW sp, 0x9000 // start stack at 0x90000

MOVW v1, 0x0
MOVT v1, 0x3f20 // put 0x3f200000 in v1
MOVW v2, 0x0
MOVT v2, 0x20   // put 0x200000 in v2
MOVW v3, 0x8000 // put 0x8000 in v3

STR v2, v1, 0x10 // enable gpio

loop: 
STR v3, v1, 0x20 // turn on led

MOVW a1, 0x0
MOVT a1, 0x10 // put 0x100000 in a1
PUSH a1
BL subroutine
POP a1

STR v3, v1, 0x2c // turn off led
MOVW a1, 0x0
MOVT a1, 0x10 // put 0x100000 in a1
PUSH a1
BL subroutine
POP a1

BAL loop


subroutine:
PUSH lr
PUSH a3
POP a3

wait: SUBS a3, a3, 0x01
BNE wait
POP a3
POP lr
MOV pc, lr
