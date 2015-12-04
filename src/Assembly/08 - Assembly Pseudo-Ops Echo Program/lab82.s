
address: 0x1004
hello_string:
byte: 0x48, 0x65, 0x6c, 0x6c, 0x6f, 0x20, 0x57, 0x6f, 0x72, 0x6c, 0x64, 0xd, 0xa, 0x00

address: 0x1100 // 0x9100
interrupt_handle:
SUBS lr, lr, 0x4
PUSH lr
PUSH a1
//BL read_char
//POP a1
MOVW a1, 0x70
PUSH a1
BL write_char
POP a1
POP a1
POP lr
mov pc, lr




address: 0x1200 // 0x9200
interrupt_other:
PUSH lr
PUSH a1

MOVW a1, 0x80
PUSH a1
BL write_char
POP a1

MOVW a1, 0x4000
PUSH a1
BL delay
POP a1

POP a1
POP lr
BAL interrupt_other




// interrupt_vector
address: 0x1300 // 0x9300
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


MOVW a1, 0x61
MOVW a2, 0x1000
MOVT a2, 0x3f20
STR a1, a2, 0x0 // write character to UART
MOVW a1, 0x1000
aaaaaaa:
SUBS a1, a1, 0x01
IF a1 != 0x01 THEN aaaaaaa ELSE aaaaaaa_continue
aaaaaaa_continue:





BL initialize_interrupts


MOVW a1, 0x48
PUSH a1
BL write_char
POP a1

main_loop:

PUSH pc
BL write_hex
POP a1

MOVW a1, 0x4000
PUSH a1
BL delay
POP a1

BAL main_loop



initialize_interrupts:

MOVW a2, 0x9300 // this is where my interrupt code is stored
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

CPSID 0x11        // Disable interrupts 
CPS 0x12          // Switch to IRQ mode
MOVW sp, 0x8c18   // start IRQ stack at 0x8c18
CPS 0x13          // Switch to supervisor mode
MOVW sp, 0x9000   // start stack at 0x9000
PUSH lr           // Now I have a stack, so push the lr before I lose it

MOVW a1, 0xffff
MOVT a1, 0xffff
MOVW a2, 0xb21c   // Base + 0x21c 'Disable IRQs 1'
MOVT a2, 0x3f00
STR a1, a2, 0x0   // Write 0xffffffff to 0x3f20b21c to disable hardware interrupts

MOVW a1, 0xffff
MOVT a1, 0xfdff
MOVW a2, 0xb220   // Base + 0x21c 'Disable IRQs 2'
MOVT a2, 0x3f00
STR a1, a2, 0x0   // Write 0xfdffffff to 0x3f20b220 to disable hardware interrupts

MOVW a1, 0x0
MOVT a1, 0x200
MOVW a2, 0xb214   // Base + 0x21c 'Enable IRQs 2'
MOVT a2, 0x3f00
STR a1, a2, 0x0   // Write 0x200000 to 0x3f20b214 to Enable IRQs 2 (bit 25)

MOVW a1, 0x0
MOVT a1, 0x3f20 // GPIO Base
MOVW a2, 0x1000
MOVT a2, 0x3f20 // UART Base


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

// disable pull up/down for all GPIO pins
STR a3, a1, 0x94 // write(GGPUD, 0x0)

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



//LDR a1, a2, 0x4     // read RSRECH register
//ORRS a1, a1, 0x24000  // 7 set GPIO pin 14 to tx and 15 to rx
//STR a1, a2, 0x4

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




MOVW a1, 0x66
MOVW a2, 0x1000
MOVT a2, 0x3f20
STR a1, a2, 0x0 // write character to UART
MOVW a1, 0x1000
ffffff:
SUBS a1, a1, 0x01
IF a1 != 0x01 THEN ffffff ELSE ffffff_continue
ffffff_continue:




MOVW a2, 0x1000
MOVT a2, 0x3f20

MOVW a1, 0x0
STR a1, a2, 0x0 // disable the UART

MOVW a3, 0x8
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

MOVW a1, 0x60 // only enables 8-bit xfer, not FIFOs
STR a1, a2, 0x2c // write(UART0_LCHR, (1<<4) | (1<<5) | (1<<6))


MOVW a1, 0x301
STR a1, a2, 0x30 // enable rx and tx on the uart

MOVW a1, 0x0
STR a1, a1, 0x34 // set fifo lengths to 1/8 and 1/8

MOVW a1, 0x10
STR a1, a2, 0x38 // enable fifo interupts



MOVW a1, 0x67
MOVW a2, 0x1000
MOVT a2, 0x3f20
STR a1, a2, 0x0 // write character to UART
MOVW a1, 0x1000
gggggg:
SUBS a1, a1, 0x01
IF a1 != 0x01 THEN gggggg ELSE gggggg_continue
gggggg_continue:




CPSIE 0x111


MOVW a1, 0x68
MOVW a2, 0x1000
MOVT a2, 0x3f20
STR a1, a2, 0x0 // write character to UART
MOVW a1, 0x1000
hhhhhh:
SUBS a1, a1, 0x01
IF a1 != 0x01 THEN hhhhhh ELSE hhhhhh_continue
hhhhhh_continue:


POP lr
MOV pc, lr



read_char: PUSH lr
PUSH a1
PUSH a2
MOVW a2, 0x1000
MOVT a2, 0x3f20 // UART Base

read_char_loop_wait_for_uart_ready:
LDR a1, a2, 0x18
ANDS a1, a1, 0x10
CMPI a1, 0x0
BNE read_char_loop_wait_for_uart_ready
LDR a1, a2, 0x0                   // get char
ANDS a1, a1, 0xff                 // mask it

POP a2
POP a1
POP lr
PUSH a1
MOV pc, lr





// Writes a character to the UART
write_char: PUSH lr
PUSH a1
PUSH a2
LDR a1, sp, 0xc // character
MOVW a2, 0x1000
MOVT a2, 0x3f20 // UART Base
STR a1, a2, 0x0 // write character to UART
POP a2
POP a1
POP lr
MOV pc, lr





// waits for arg1 number of cycles
delay: PUSH lr
PUSH a1
LDR a1, sp, 0x8

delay_loop:
SUBS a1, a1, 0x01
IF a1 != 0x01 THEN delay_loop ELSE delay_continue
delay_continue:

POP a1
POP lr
MOV pc, lr




write_hex: PUSH lr
PUSH a1
PUSH a2
PUSH a3
LDR a1, sp, 0x10 // character
MOV a2, a1
MOVW a3, 0x8

write_hex_loop:
ROR a2, a2, #28
ANDS a1, a2, 0xf
if a1 >= 0xa THEN write_hex_loop_hex_digit ELSE write_hex_loop_num_digit
write_hex_loop_num_digit:
ADDI a1, a1, 0x30
BAL write_hex_write_char
write_hex_loop_hex_digit:
ADDI a1, a1, 0x37

write_hex_write_char:
PUSH a1
BL write_char
POP a1

SUBS a3, a3, 0x01
IF a3 != 0x0 THEN write_hex_loop ELSE write_hex_continue
write_hex_continue:

POP a3
POP a2
POP a1
POP lr
MOV pc, lr
