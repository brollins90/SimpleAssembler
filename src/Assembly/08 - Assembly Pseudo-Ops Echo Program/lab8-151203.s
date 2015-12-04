address: 0x1100 // 0x9100
interrupt_handle:
SUBS lr, lr, 0x4
PUSH lr
PUSH a1
//BL read_char
//POP a1
MOVW a1, 0x49 // 'I'
PUSH a1
BL write_char
POP a1

MOVW a1, 0x0
MOVT a1, 0x20
PUSH a1
BL delay
POP a1

MOV a1, lr
PUSH a1
BL write_hex
POP a1


POP a1
POP lr
mov pc, lr


address: 0x1300 // 0x9300
WORD: 0xea007ffe, 0xea0090fa, 0xea0090f6, 0xea0090f2, 0xea0090ee, 0xea0090ea, 0xea0090e6, 0xea0090e2



address: 0x0 // 0x8000

MOVW a1, 0x31 //   '1'
MOVW a2, 0x1000
MOVT a2, 0x3f20
STR a1, a2, 0x0 // write character to UART


BL initialize_interrupts



main_loop:

MOVW a1, 0x2a // '*'
PUSH a1
BL write_char
POP a1

//BL interrupt_handle

MOVW a1, 0x0
MOVT a1, 0x20
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

MOVW a1, 0x32 //   '2'
MOVW a2, 0x1000
MOVT a2, 0x3f20
STR a1, a2, 0x0 // write character to UART


// GPIOBASE   0x3f200000
// GPFSEL1    0x3f200004
// GPSET0     0x3f20001c
// GPCLR0     0x3f200028
// GPPUD      0x3f200094
// GPPUDCLK0  0x3f200098
// UARTBASE   0x3f201000
// UART0_IBRD 0x3f201024
// UART0_FBRD 0x3f201028
// UART0_LCRH 0x3f20102c
// UART0_CR   0x3f201030
// UART0_IFLS 0x3f201034
// UART0_IMSC 0x3f201038
// UART0_ICR  0x3f201044

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



// MRS 
// {cond}00110{R}10{mask}1111{imm12}

// {cond}00010{R}10{mask}111100000000{Rn}
//   111000010 1 10      1111000000001111


CPSID 0x111        // Disable interrupts 
CPS 0x12          // Switch to IRQ mode
MOVW sp, 0x8c18   // start IRQ stack at 0x8c18
CPS 0x13          // Switch to supervisor mode
MOVW sp, 0x9000   // start stack at 0x9000
PUSH lr           // Now I have a stack, so push the lr before I lose it

MOV a1, pc
PUSH a1
BL write_hex
POP a1



MOVW v1, 0x0
MOVT v1, 0x3f20

MOVW v2, 0x1000
MOVT v2, 0x3f20

MOVW v3, 0xb200
MOVT v3, 0x3f00

MOVW v4, 0x5000
MOVT v4, 0x3f21


CPS 0x12          // Switch to IRQ mode
MOV a1, sp
PUSH a1
BL write_hex
POP a1


MOVW a3, 0x96     // delay for 150 cycles
PUSH a3
BL delay
POP a3


CPS 0x13          // Switch to supervisor mode
MOV a1, sp
PUSH a1
BL write_hex
POP a1



MOVW a1, 0x33 //   '3'
STR a1, v2, 0x0 // write character to UART

MOVW a1, 0xffff
MOVT a1, 0xffff
STR a1, v3, 0x1c  // 0x3f00 b21c <- 0xffff ffff
                  // IRQ_DISABLE1

STR a1, v3, 0x20  // 0x3f00 b220 <- 0xffff ffff
                  // IRQ_DISABLE2

//MOVW a1, 0x0
//MOVT a1, 0x200
//STR a1, v3, 0x10  // 0x3f00 b210 <- 0x0200 0000
STR a1, v3, 0x14  // 0x3f00 b214 <- 0x0200 0000
                  // IRQ_ENABLE2

MOVW a1, 0x34 //   '4'
STR a1, v2, 0x0 // write character to UART


LDR a1, v1, 0x4   // a1 <- 0x3f20 0004
MOVW a4, 0x4000
MOVT a4, 0x2      // 0x24000 (1<<15)|(1<<18)
ORRRS a1, a1, a4  // a1 | 0x0002 4000
STR a1, v1, 0x4   // 0x3f20 0004 <- a1
                  // GPFSEL1

LDR a1, v1, 0x94  // a1 <- 0x3f20 0094
MOVW a4, 0xffff
MOVT a4, 0xfffc
ANDRS a1, a1, a4  // a1 & 0xfffc ffff
STR a1, v1, 0x94  // 0x3f20 0094 <- 1a
                  // GPPUD

MOVW a3, 0x96     // delay for 150 cycles
PUSH a3
BL delay
POP a3

MOVW a1, 0x35 //   '5'
STR a1, v2, 0x0 // write character to UART

LDR a1, v1, 0x98  // a1 <- 0x3f20 0098
MOVW a4, 0x3fff
MOVT a4, 0xffff
ANDRS a1, a1, a4  // a1 & 0xffff 3fff
STR a1, v1, 0x98  // 0x3f20 0098 <- 1a
                  // GPPUDCLK0

MOVW a3, 0x96     // delay for 150 cycles
PUSH a3
BL delay
POP a3

MOVW a1, 0x36 //   '6'
STR a1, v2, 0x0 // write character to UART


MOVW a1, 0x0
STR a1, v1, 0x94  // 0x3f20 0094 <- 0x0
                  // GPPUD

MOVW a3, 0x96     // delay for 150 cycles
PUSH a3
BL delay
POP a3

LDR a1, v1, 0x98  // a1 <- 0x3f20 0098
MOVW a4, 0x3fff
MOVT a4, 0xffff
ANDRS a1, a1, a4  // a1 & 0xffff 3fff
STR a1, v1, 0x98  // 0x3f20 0098 <- 1a
                  // GPPUDCLK0

MOVW a1, 0x37 //   '7'
STR a1, v2, 0x0 // write character to UART

//MOVW a1, 0x0
//STR a1, v2, 0x0   // 0x3f20 1000 <- 0x0
                  // UARTBASE
                  
MOVW a3, 0x96     // delay for 150 cycles
PUSH a3
BL delay
POP a3

MOVW a1, 0x37 //   '7'
STR a1, v2, 0x0 // write character to UART

//10
loop10:
LDR a1, v2, 0x18
ANDS a1, a1, 0x8
BNE loop10

MOVW a1, 0x37 //   '7'
STR a1, v2, 0x0 // write character to UART

// 11

LDR a1, v2, 0x2c
MOVW a2, 0xffef
MOVT a2, 0xffff
ANDRS a1, a1, a2
STR a1, v2, 0x2c

MOVW a1, 0x37 //   '7'
STR a1, v2, 0x0 // write character to UART

MOVW a1, 0x7ff
STR a1, v2, 0x44  // 0x3f20 1044 <- 0x7ff
                  // UART0_ICR

MOVW a1, 0x38 //   '8'
STR a1, v2, 0x0 // write character to UART


// 13
MOVW a1, 0x1
STR a1, v2, 0x24
MOVW a1, #40
STR a1, v2, 0x28

MOVW a1, 0x38 //   '8'
STR a1, v2, 0x0 // write character to UART


MOVW a1, 0x60
STR a1, v2, 0x2c  // 0x3f20 102c <- 0x60
                  // UART0_LCRH

MOVW a1, 0x301
STR a1, v2, 0x30  // 0x3f20 1030 <- 0x301
                  // UART_CR

MOVW a1, 0x0
STR a1, v2, 0x34  // 0x3f20 1034 <- 0x0
                  // UART0_IFLS

MOVW a1, 0x10
STR a1, v2, 0x38  // 0x3f20 1038 <- 0x10
                  // UART0_IMSC

MOVW a1, 0x39 //   '9'
STR a1, v2, 0x0 // write character to UART

CPSIE 0x111


MOV a1, sp
PUSH a1
BL write_hex
POP a1

MOV a1, pc
PUSH a1
BL write_hex
POP a1

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
POP a2 // should be a1, but then we lose it
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
