MOVW sp, 0x9000 // start stack at 0x90000

MOVW v1, 0x0
MOVT v1, 0x3f20 // GPIO Base
MOVW v2, 0x1000
MOVT v2, 0x3f20 // UART Base

//MOVW a1, 0x0
//MOVT a1, 0x20

//STR a1, v1, 0x10 // enable gpio

MOVW a1, 0x0
// disable UART0
STR a1, v2, 0x30 // write(UART_CR, 0x0)
// disable pull up/down for all GPIO pins
STR a1, v1, 0x94 // write(GGPUD, 0x0)

// delay for 150 cycles
MOVW v4, 0x96
STRDB v4, sp, 0x0
BL delay

// disable pull up/down for pin 14 and 15
MOVW a1, 0xc000
STR a1, v1, 0x98 // write(GGPUDCLK0, (1<<14) | (1<< 15))
// delay for 150 cycles
MOVW v4, 0x96
STRDB v4, sp, 0x0
BL delay

// write a 0 to GPPUDCLK0 to make it take effect
MOVW a1, 0x0
STR a1, v1, 0x98 // write(GGPUDCLK0, 0x0)

// clear pending interrupts
MOVW a1, 0x7ff
STR a1, v2, 0x44 // write(UART0_ICR, 0x7ff)

// set baud stuff
MOVW a1, #1
STR a1, v2, 0x24 // write(UART0_IBRD, #1)
MOVW a1, #40
STR a1, v2, 0x28 // write(UART0_FBRD, #40)

// enable fifo & 8 bit data transmission (1 stop bit, no parity)
MOVW a1, #70
STR a1, v2, 0x2c // write(UART0_LCHR, (1<<4) | (1<<5) | (1<<6))

// mask all interrupts
MOVW a1, 0x7f2
STR a1, v2, 0x38 // write(UART0_IMSC, (1<<1) | (1<<4) | (1<<5) | (1<<6) | (1<<7) | (1<<8) | (1<<9) | (1<<10))

// enable UART0, receive and transfer part of UART
MOVW a1, 0x301
STR a1, v2, 0x30 // write(UART0_CR, (1<<0) | (1<<8) | (1<<9))


loop: MOVW a1, 0x0048
STR a1, v2, 0x0 // write 'H'
MOVW v4, 0x0
MOVT v4, 0x40
STRDB v4, sp, 0x0
BL delay

MOVW a1, 0x0069
STR a1, v2, 0x0 // write 'i'
MOVW v4, 0x0
MOVT v4, 0x40
STRDB v4, sp, 0x0
BL delay

MOVW a1, 0x0020
STR a1, v2, 0x0 // write ' '
MOVW v4, 0x0
MOVT v4, 0x40
STRDB v4, sp, 0x0
BL delay

BAL loop

delay: LDRIA a3, sp, 0x0

wait: SUBS a3, a3, 0x01
BNE wait
MOV pc, lr
