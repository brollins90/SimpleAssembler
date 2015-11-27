

address: 0x2000
hello_string:
//byte: 0x48, 0x65, 0x6c, 0x6c, 0x6f, 0x20, 0x57, 0x6f, 0x72, 0x6c, 0x64, 0x00
byte: 0x00, 0x00, 0x00, 0x6c, 0x00, 0x00, 0x00, 0x6f, 0x00

//ADDRESS: 0x3000
//stack:
//BLOCK: #1024

address: 0x0
MOVW sp, 0x9000 // todo: make this use the label

MOVW a1, 0x0
MOVT a1, 0x3f20 // GPIO Base
MOVW a2, 0x1000
MOVT a2, 0x3f20 // UART Base

PUSH a1
PUSH a2
BL initialize_uart
POP a2
POP a1


loop:
MOVW v1, 0x0 // data is going to be at 0x8000 + 0x2000
MOVT v1, 0x1
inner_loop:
LDR a1, v1, 0x0
CMPI a1, 0x0
BEQ loop_delay

PUSH a1
PUSH a2
BL write_char
POP a2
POP a1

ADDI v1, v1, 0x4
BAL inner_loop



loop_delay:
PUSH lr
PUSH a1
MOVW a1, 0x0 // delay
MOVT a1, 0x10
PUSH a1
BL delay
POP a1
POP a1
POP lr
BAL loop



initialize_uart:
PUSH lr
PUSH a1
PUSH a2
PUSH a3
LDR a1, sp, 0x14
LDR a2, sp, 0x10

MOVW a3, 0x0
// disable UART0
STR a3, a2, 0x30 // write(UART_CR, 0x0)
// disable pull up/down for all GPIO pins
STR a3, a1, 0x94 // write(GGPUD, 0x0)

// delay for 150 cycles
MOVW a3, 0x96
PUSH a3
BL delay
POP a3

// disable pull up/down for pin 14 and 15
MOVW a3, 0xc000
STR a3, a1, 0x98 // write(GGPUDCLK0, (1<<14) | (1<< 15))
// delay for 150 cycles
MOVW a3, 0x96
PUSH a3
BL delay
POP a3

// write a 0 to GPPUDCLK0 to make it take effect
MOVW a3, 0x0
STR a3, a1, 0x98 // write(GGPUDCLK0, 0x0)

// clear pending interrupts
MOVW a3, 0x7ff
STR a3, a2, 0x44 // write(UART0_ICR, 0x7ff)

// set baud stuff
MOVW a3, #1
STR a3, a2, 0x24 // write(UART0_IBRD, #1)
MOVW a3, #40
STR a3, a2, 0x28 // write(UART0_FBRD, #40)

// enable fifo & 8 bit data transmission (1 stop bit, no parity)
MOVW a3, #70
STR a3, a2, 0x2c // write(UART0_LCHR, (1<<4) | (1<<5) | (1<<6))

// mask all interrupts
MOVW a3, 0x7f2
STR a3, a2, 0x38 // write(UART0_IMSC, (1<<1) | (1<<4) | (1<<5) | (1<<6) | (1<<7) | (1<<8) | (1<<9) | (1<<10))

// enable UART0, receive and transfer part of UART
MOVW a3, 0x301
STR a3, a2, 0x30 // write(UART0_CR, (1<<0) | (1<<8) | (1<<9))

POP a3
POP a2
POP a1
POP lr
MOV pc, lr




// Writes a character to the UART
write_char:
PUSH lr
PUSH a1
PUSH a2
PUSH a3
LDR a1, sp, 0x14 // character
LDR a2, sp, 0x10 // UART base

write_char_check_ready:
LDR a3, a2, 0x18 // get the UART status ??? which status?
ANDS a3, a3, 0x20 // check if the UART is ready
BNE write_char_check_ready

STR a1, a2, 0x0 // write character to UART

MOVW a1, 0x0 // delay
MOVT a1, 0x10
PUSH a1
BL delay
POP a1

POP a3
POP a2
POP a1
POP lr
MOV pc, lr





// waits for arg1 number of cycles
delay:
PUSH lr
PUSH a1
LDR a1, sp, 0x8

delay_wait: SUBS a1, a1, 0x01
BNE delay_wait

POP a1
POP lr
MOV pc, lr
