
address: 0x1004
hello_string:
byte: 0x48, 0x65, 0x6c, 0x6c, 0x6f, 0x20, 0x57, 0x6f, 0x72, 0x6c, 0x64, 0xd, 0xa, 0x00

address: 0x0
MOVW sp, 0x9000 // start stack at 0x9000

MOVW a1, 0x0
MOVT a1, 0x3f20 // GPIO Base
MOVW a2, 0x1000
MOVT a2, 0x3f20 // UART Base

PUSH a1
PUSH a2
BL initialize_uart
POP a2
POP a1

MOVW a1, 0x48
PUSH a1
PUSH a2
BL write_char
POP a2
POP a1

main_loop:

PUSH a2
BL read_char
POP a2

PUSH a1
PUSH a2
BL write_char
POP a2
POP a1

BAL main_loop





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


//MOVW a3, #70 // enable fifo & 8 bit data transmission (1 stop bit, no parity)
MOVW a3, 0x60 // only enables 8-bit xfer, not FIFOs
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




read_char:
PUSH lr
PUSH a2
LDR a2, sp, 0x8 // UART base

read_char_loop:
LDR a1, a2, 0x18
ANDS a1, a1, 0x10
CMPI a1, 0x0
BNE read_char_loop
//IF a1 != 0x0 THEN read_char_continue ELSE read_char_loop
//read_char_continue:
LDR a1, a2, 0x0
ANDS a1, a1, 0xff

POP a2
POP lr
MOV pc, lr





// Writes a character to the UART
write_char:
PUSH lr
PUSH a1
PUSH a2
LDR a1, sp, 0x10 // character
LDR a2, sp, 0xc // UART base

STR a1, a2, 0x0 // write character to UART

MOVW a1, 0x0 // delay
MOVT a1, 0x10
PUSH a1
BL delay
POP a1

POP a2
POP a1
POP lr
MOV pc, lr





// waits for arg1 number of cycles
delay:
PUSH lr
PUSH a1
LDR a1, sp, 0x8

delay_loop:
SUBS a1, a1, 0x01
IF a1 != 0x01 THEN delay_loop ELSE delay_continue
delay_continue:

POP a1
POP lr
MOV pc, lr




write_hex:
PUSH lr
PUSH a1
PUSH a2
PUSH a3
PUSH a4
LDR a1, sp, 0x18 // character
LDR a2, sp, 0x14 // UART base
MOV a3, a1
MOVW a4, 0x8

write_hex_loop:
ROR a3, a3, #28
ANDS a1, a3, 0xf
if a1 >= 0xa THEN write_hex_loop_hex_digit ELSE write_hex_loop_num_digit
write_hex_loop_num_digit:
ADDI a1, a1, 0x30
BAL write_hex_write_char
write_hex_loop_hex_digit:
ADDI a1, a1, 0x37

write_hex_write_char:
PUSH a1
PUSH a2
BL write_char
POP a2
POP a1

SUBS a4, a4, 0x01
IF a4 != 0x0 THEN write_hex_loop ELSE write_hex_continue
write_hex_continue:

POP a4
POP a3
POP a2
POP a1
POP lr
MOV pc, lr
