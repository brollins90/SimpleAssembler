MOVW v1, 0x0
MOVT v1, 0x3f20 // put 0x3f200000 in v1
MOVW v2, 0x0
MOVT v2, 0x20   // put 0x200000 in v2
MOVW v3, 0x8000 // put 0x8000 in v3

STR v2, v1, 0x10 // enable gpio
STR v3, v1, 0x20 // turn on led
STR v3, v1, 0x2c // turn off led

// branch -2 to make an never ending loop
