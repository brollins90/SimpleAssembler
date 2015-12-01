
address: 0x1004
hello_string:
byte: 0x48, 0x65, 0x6c, 0x6c, 0x6f, 0x20, 0x57, 0x6f, 0x72, 0x6c, 0x64, 0xd, 0xa, 0x00

//address: 0x1014

// interrupt_handle
address: 0x1100
PUSH lr
PUSH a1
PUSH a2
MOVW a2, 0x1000
MOVT a2, 0x3f20 // UART Base

LDR a1, a2, 0x0 // get char
ANDS a1, a1, 0xff // mask it
interrupt_handle_loop:
LDR a1, a2, 0x18
ANDS a1, a1, 0x10
CMPI a1, 0x0
BNE interrupt_handle_loop
STR a1, a2, 0x0 // write character to UART
POP a2
POP a1
POP lr
mov pc, lr

// interrupt_handle_led_on
address: 0x1200
PUSH lr
PUSH a1
PUSH a2
MOVW a1, 0x0
MOVT a1, 0x3f20 // GPIO Base
MOVW a2, 0x0
MOVT a2, 0x20
STR a2, a1, 0x10 // enable gpio
MOVW a2, 0x8000
STR a2, a1, 0x20
POP a2
POP a1
POP lr
//MOV pc, lr
interrupt_handle_led_on_loop: BAL interrupt_handle_led_on_loop


// interrupt_vector
address: 0x1300
WORD: 0xea007ffe, 0xea0091fa, 0xea0091f6, 0xea0091f2, 0xea0091ee, 0xea0091e6, 0xea0091e2, 0xea0090e4
//BAL 0x8000 //0xea007ffe // 0x8000 - 0x0 - #2
//BAL 0x9200 //0xea0091fa // 0x9200 - 0x4 - #2
//BAL 0x9200 //0xea0091f6 // 0x9200 - 0x8 - #2
//BAL 0x9200 //0xea0091f2 // 0x9200 - 0xc - #2
//BAL 0x9200 //0xea0091ee // 0x9200 - 0x10 - #2
//BAL 0x9200 //0xea0091e6 // 0x9200 - 0x14 - #2
//BAL 0x9200 //0xea0091e2 // 0x9200 - 0x18 - #2
//BAL 0x9100 //0xea0090e4 // 0x9100 - 0x1c - #2





address: 0x0

BL initialize_interrupts

MOVW a2, 0x1000
MOVT a2, 0x3f20 // UART Base

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


initialize_interrupts:
//PUSH lr
//PUSH a1
//PUSH a2   // we dont have a stack yet

MOVW a2, 0x9300
MOVW a3, 0x0
LDR a1, a2, 0x0 // first interrupt
STR a1, a3, 0x0
LDR a1, a2, 0x4
STR a1, a3, 0x4
LDR a1, a2, 0xc
STR a1, a3, 0xc
LDR a1, a2, 0x10
STR a1, a3, 0x10
LDR a1, a2, 0x14
STR a1, a3, 0x14
LDR a1, a2, 0x18
STR a1, a3, 0x18
LDR a1, a2, 0x1c
STR a1, a3, 0x1c


CPS 0x12 // irq mode  //CPSR <- CPSR (last byte 0xd2)
MOVW sp, 0x8c18 // start IRQ stack at 0x8c18

CPS 0x13 // supervisor mode  //CPSR <- CPSR (last byte 0xd3)
MOVW sp, 0x9000 // start stack at 0x9000


MOVW a1, 0xffff
MOVT a1, 0xffff
MOVW a2, 0xb21c
MOVT a2, 0x3f00
STR a1, a2, 0x0 // disable hardware interrupts

MOVW a1, 0xffff
MOVT a1, 0xffff
MOVW a2, 0xb220
MOVT a2, 0x3f00
STR a1, a2, 0x0 // disable hardware interrupts

MOVW a1, 0x0
MOVT a1, 0x20  // is this 0x2000000 or 0x200000
MOVW a2, 0xb214
MOVT a2, 0x3f00
STR a1, a2, 0x0 // disable hardware interrupts

MOVW a2, 0x0
MOVT a2, 0x3f20 // GPIO Base

LDR a1, a2, 0x4
ORRS a1, a1, 0x24000  // 7 set GPIO pin 14 to tx and 15 to rx
STR a1, a2, 0x4

LDR a1, a2, 0x94
MOVW a3, 0xffff
MOVT a3, 0xfffc
ORRRS a1, a1, a3
STR a1, a2, 0x94 // turn off the pull up down for pins 14 and 15

// delay for 150 cycles
MOVW a3, 0x96
PUSH a3
BL delay
POP a3

LDR a1, a2, 0x98
MOVW a3, 0x3fff
MOVT a3, 0xffff
ORRRS a1, a1, a3
STR a1, a2, 0x98 // turn off the pull up down for pins 14 and 15

// delay for 150 cycles
MOVW a3, 0x96
PUSH a3
BL delay
POP a3

MOVW a1, 0x0
STR a1, a2, 0x94

// delay for 150 cycles
MOVW a3, 0x96
PUSH a3
BL delay
POP a3

LDR a1, a2, 0x98
MOVW a3, 0x3fff
MOVT a3, 0xffff
ORRRS a1, a1, a3
STR a1, a2, 0x98 // turn off the pull up down for pins 14 and 15


MOVW a2, 0x1000
MOVT a2, 0x3f20

MOVW a1, 0x0
STR a1, a2, 0x0 // disable the UART

MOVW a3, 0x8
MOVW a4, 0x0
uart_stop_sending_loop:
LDR a1, a2, 0x18
ANDRS a1, a1, a3
CMPI a1, 0x0
BNE uart_stop_sending_loop

LDR a1, a2, 0x2c
MOVW a3, 0xffef
MOVT a3, 0xffff
ANDRS a1, a1, a3
STR a1, a2, 0x2c  // flush transmit FIFO

MOVW a1, 0x7ff
STR a1, a2, 0x44 // clear any pending interrupts

// set baud stuff
MOVW a1, 0x1
STR a1, a2, 0x24 // write(UART0_IBRD, #1)
MOVW a1, #40
STR a1, a2, 0x28 // write(UART0_FBRD, #40)

//MOVW a3, #70 // enable fifo & 8 bit data transmission (1 stop bit, no parity)
MOVW a1, 0x60 // only enables 8-bit xfer, not FIFOs
STR a1, a2, 0x2c // write(UART0_LCHR, (1<<4) | (1<<5) | (1<<6))




MOVW a1, 0x301
STR a1, a2, 0x30 // enable rx and tx on the uart

MOVW a1, 0x0
STR a1, a1, 0x34 // set fifo lengths to 1/8 and 1/8

MOVW a1, 0x10
STR a1, a2, 0x38 // enable fifo interupts

CPSIE 0x111

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
