MOVW v1, 0x0
MOVT v1, 0x3f20 // put 0x3f200000 in v1
MOVW v2, 0x0
MOVT v2, 0x20   // put 0x200000 in v2
MOVW v3, 0x8000 // put 0x8000 in v3

STR v2, v1, 0x10 // enable gpio
STR v3, v1, 0x20 // turn on led

MOVW a1, 0x0
MOVT a1, 0x10 // put 0x100000 in a1
SUBS a1, a1, #1 // subtract 1 from a1 and set the condition bits
BNE #-3 // go back to subs if not equal

STR v3, v1, 0x2c // turn off led
MOVW a1, 0x0
MOVT a1, 0x10 // put 0x100000 in a1
SUBS a1, a1, #1 // subtract 1 from a1 and set the condition bits
BNE #-3 // go back to subs if not equal

BAL -12 // branch back to turn led on
