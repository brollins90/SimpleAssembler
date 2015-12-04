// e8fd9fff // restore
// e92d5fff // save

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
MOVW a1, 0x70 // 'p'
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

MOVW a1, 0x74 // 't'
PUSH a1
BL write_char
POP a1

MOVW a1, 0x0
MOVT a1, 0x10
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


MOVW a1, 0x48 // 'H'
PUSH a1
BL write_char
POP a1

main_loop:

MOVW a1, 0x69
PUSH a1
BL write_hex
POP a1

BAL interrupt_handle

MOVW a1, 0x71
PUSH a1
BL write_hex
POP a1


MOVW a1, 0x0
MOVT a1, 0x10
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


// GPIOBASE   0x3f200000
// GPFSEL1    0x3f200004
// GPSET0     0x3f20001c
// GPCLR0     0x3f200028
// GPPUD      0x3f200094
// GPPUDCLK0  0x3f200098
// UARTBASE   0x3f201000
// UART_CR    0x3f201030
// UART0_ICR  0x3f201044
// UART0_IBRD 0x3f201024
// UART0_FBRD 0x3f201028

// AUX_ENABLES      0x3f215004
// AUX_MU_IO_REG    0x3f215040
// AUX_MU_IER_REG   0x3f215044
// AUX_MU_IIR_REG   0x3f215048
// AUX_MU_LCR_REG   0x3f21504c
// AUX_MU_MCR_REG   0x3f215050
// AUX_MU_LSR_REG   0x3f215054
// AUX_MU_MSR_REG   0x3f215058
// AUX_MU_SCRATCH   0x3f21505c
// AUX_MU_CNTL_REG   0x3f215060
// AUX_MU_STAT_REG   0x3f215064
// AUX_MU_BAUD_REG   0x3f215068

// IRQ_BASIC      0x3f00b200
// IRQ_PEND1      0x3f00b204
// IRQ_PEND2      0x3f00b208
// IRQ_FIQ_CONTROL  0x3f00b20c
// IRQ_ENABLE1   0x3f00b210
// IRQ_ENABLE2   0x3f00b214
// IRQ_ENABLE_BASIC   0x3f00b218
// IRQ_DISABLE1  0x3f00b21c
// IRQ_DISABLE2  0x3f00b220
// IRQ_DISABLE_BASIC  0x3f00b224




CPSID 0x11        // Disable interrupts 
CPS 0x12          // Switch to IRQ mode
MOVW sp, 0x8c18   // start IRQ stack at 0x8c18
CPS 0x13          // Switch to supervisor mode
MOVW sp, 0x9000   // start stack at 0x9000
PUSH lr           // Now I have a stack, so push the lr before I lose it


MOVW a2, 0x5000
MOVT a2, 0x3f21
MOVW a4, 0x0
MOVT a4, 0x3f20
MOVW v2, 0xb200
MOVT v2, 0x3f00

// IRQ_DISABLE1 <- (1<<29) 0x20000000
MOVW a1, 0x0
MOVT a1, 0x2000
STR a1, v2, 0x1c

// AUX_ENABLES <- 0x1
MOVW a1, 0x1
STR a1, a2, 0x4

// AUX_MU_IER_REG <- 0x0
MOVW a1, 0x0
STR a1, a2, 0x44

// AUX_MU_CNTL_REG <- 0x0
STR a1, a2, 0x60

// AUX_MU_LCR_REG <- 0x3
MOVW a1, 0x3
STR a1, a2, 0x4c

// AUX_MU_MCR_REG <- 0x0
MOVW a1, 0x0
STR a1, a2, 0x50

// AUX_MU_IER_REG <- 0x5 // enable rx interrupts
MOVW a1, 0x5
STR a1, a2, 0x44

// AUX_MU_IIR_REG <- 0xc6
MOVW a1, 0xc6
STR a1, a2, 0x48

// AUX_MU_BAUD_REG <- #270
STR a1, a2, 0x68

// a4 is now GPIOBASE
// a3 <- GPFSEL1
LDR a3, a4, 0x4

MOVW v1, 0x8fff
MOVT v1, 0xffff // ~(7<<12) gpio14
ANDRS a3, a3, v1

MOVW v1, 0x3000 //(2<<12) alt5
ORRRS a3, a3, v1

ANDRS a3, a3, v1 // ~(7<<15) gpio 15

MOVW v1, 0x3000 //(2<<15) alt5
ORRRS a3, a3, v1

// GPFSEL1 <- a3 (
STR a3, a4, 0x4

// GPPUD <- 0xo
MOVW a3, 0x0
STR a3, a4, 0x94

// delay for 150 cycles
MOVW a3, 0x96
PUSH a3
BL delay
POP a3

// GPPUDCLK0 <- (1<<14)|(1<<15)
MOVW a3, 0xc000
STR a3, a4, 0x98

// delay for 150 cycles
MOVW a3, 0x96
PUSH a3
BL delay
POP a3

// GPPUDCLK0 <- 0x0
MOVW a3, 0x0
STR a3, a4, 0x98


// AUX_MU_CNTL_REG <- 0x3
MOVW a1, 0x3
STR a1, a2, 0x60


// IRQ_ENABLE1 <- (1<<29) 0x20000000
MOVW a1, 0x0
MOVT a1, 0x2000
STR a1, v2, 0x10

MOVW a1, 0x66
MOVW a2, 0x1000
MOVT a2, 0x3f20
STR a1, a2, 0x0 // write character to UART
MOVW a1, 0x1000
ffffff:
SUBS a1, a1, 0x01
IF a1 != 0x01 THEN ffffff ELSE ffffff_continue
ffffff_continue:


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
