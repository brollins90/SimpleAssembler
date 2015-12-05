// interrupt vector
MOVW a1, 0x1ffe
MOVT a1, 0xea00
MOVW a2, 0x0
STR a1, a2, 0x0

MOVW a1, 0x217d
MOVT a1, 0xea00
STR a1, a2, 0x4

MOVW a1, 0x217c
MOVT a1, 0xea00
STR a1, a2, 0x8

MOVW a1, 0x217b
MOVT a1, 0xea00
STR a1, a2, 0xc

MOVW a1, 0x217a
MOVT a1, 0xea00
STR a1, a2, 0x10

MOVW a1, 0x2179
MOVT a1, 0xea00
STR a1, a2, 0x14

MOVW a1, 0x2178
MOVT a1, 0xea00
STR a1, a2, 0x18

MOVW a1, 0x2137
MOVT a1, 0xea00
STR a1, a2, 0x1c



// MRS 0xc0
CPSID 0x111        // Disable interrupts 
CPS 0x12           // Switch to IRQ mode
MOVW sp, 0x8c18    // start IRQ stack at 0x8c18
CPS 0x13           // Switch to supervisor mode
MOVW sp, 0x9000    // start stack at 0x9000




MOVW v1, 0x0
MOVT v1, 0x3f20

MOVW v2, 0x1000
MOVT v2, 0x3f20

MOVW v3, 0xb200
MOVT v3, 0x3f00

MOVW v4, 0x5000
MOVT v4, 0x3f21

MOVW a1, 0xffff
MOVT a1, 0xffff

MOVW a1, 0xa //   '\r'
MOVW a2, 0x1000
MOVT a2, 0x3f20
STR a1, a2, 0x0 // write character to UART
STR a1, a2, 0x0 // write character to UART
MOVW a1, 0xd //   '3\n'
MOVW a2, 0x1000
MOVT a2, 0x3f20
STR a1, a2, 0x0 // write character to UART
STR a1, a2, 0x0 // write character to UART

MOVW a1, 0x32 //   '2'
MOVW a2, 0x1000
MOVT a2, 0x3f20
STR a1, a2, 0x0 // write character to UART

// 6a: Disable interrupts in the CPU
STR a1, v3, 0x1c  // 0x3f00 b21c <- 0xffff ffff
                  // IRQ_DISABLE1


MOVW a1, 0x32 //   '2'
MOVW a2, 0x1000
MOVT a2, 0x3f20
STR a1, a2, 0x0 // write character to UART

// 6b: Disable interrupts in the CPU
STR a1, v3, 0x20  // 0x3f00 b220 <- 0xffff ffff
                  // IRQ_DISABLE2

MOVW a1, 0x32 //   '2'
MOVW a2, 0x1000
MOVT a2, 0x3f20
STR a1, a2, 0x0 // write character to UART


// 6c: Enable interrupts in the CPU for UART
MOVW a1, 0x0
MOVT a1, 0x200
STR a1, v3, 0x14  // 0x3f00 b214 <- 0x0200 0000
                  // IRQ_ENABLE2

// ?????
// init gpio
//LDR a1, v1, 0x10
//ORRS a1, a1, 0x20
//STR a1, v1, 0x10

MOVW a1, 0x0
MOVT a1, 0x20
STR a1, v1, 0x10 // enable gpio
MOVW a1, 0x8000
STR a1, v1, 0x20 // turn on led

MOVW a1, 0x33 //   '3'
MOVW a2, 0x1000
MOVT a2, 0x3f20
STR a1, a2, 0x0 // write character to UART



//MOV a1, pc
//PUSH a1
//BL write_hex
//POP a1

// this works!!!!!!
//MOVW a1, 0x1c
//MOV pc, a1

//BAL loop


// 7: Set GPIO pins 14 and 15 to function 1
// fffdbfff
//LDR a1, v1, 0x4   // a1 <- 0x3f20 0004
MOVW a1, 0x4000
MOVT a1, 0x2      // 0x24000 (1<<15)|(1<<18)
//ORRRS a1, a1, a4  // a1 | 0x0002 4000
STR a1, v1, 0x4   // 0x3f20 0004 <- a1
                  // GPFSEL1


MOVW a1, 0x32 //   '2'
MOVW a2, 0x1000
MOVT a2, 0x3f20
STR a1, a2, 0x0 // write character to UART

// 8a Disable pull up/down for all pins
//LDR a1, v1, 0x94  // a1 <- 0x3f20 0094
//MOVW a4, 0xfffc
//MOVT a4, 0xffff
//ANDRS a1, a1, a4  // a1 & 0xffff fffc
MOVW a1, 0x0
STR a1, v1, 0x94  // 0x3f20 0094 <- 1a
                  // GPPUD

MOVW a3, 0x96     // delay for 150 cycles
PUSH a3
BL delay
POP a3

//MOVW a1, 0x32 //   '2'
//MOVW a2, 0x1000
//MOVT a2, 0x3f20
//STR a1, a2, 0x0 // write character to UART

// 8c: Clock GPIO pins 14 and 15
LDR a1, v1, 0x98  // a1 <- 0x3f20 0098
MOVW a4, 0xc000
ANDRS a1, a1, a4  // a1 & 0xc000
STR a1, v1, 0x98  // 0x3f20 0098 <- 1a
                  // GPPUDCLK0

MOVW a3, 0x96     // delay for 150 cycles
PUSH a3
BL delay
POP a3

MOVW a1, 0x32 //   '2'
MOVW a2, 0x1000
MOVT a2, 0x3f20
STR a1, a2, 0x0 // write character to UART

// 8e: Clear GPOIO pull up/down 
MOVW a1, 0x0
STR a1, v1, 0x94  // 0x3f20 0094 <- 0x0
                  // GPPUD

MOVW a3, 0x96     // delay for 150 cycles
PUSH a3
BL delay
POP a3

MOVW a1, 0x32 //   '2'
MOVW a2, 0x1000
MOVT a2, 0x3f20
STR a1, a2, 0x0 // write character to UART

// 8g: clock all gpio pins
//LDR a1, v1, 0x98  // a1 <- 0x3f20 0098
MOVW a1, 0x0
//ANDRS a1, a1, a4  // a1 & 0xffff 3fff
STR a1, v1, 0x98  // 0x3f20 0098 <- 1a
                  // GPPUDCLK0

MOVW a1, 0x34 //   '4'
MOVW a2, 0x1000
MOVT a2, 0x3f20
STR a1, a2, 0x0 // write character to UART

// 9: disable UART
MOVW a1, 0x0
STR a1, v2, 0x30

// 10: wait for UART to stop sending and receiving
loop10:
LDR a1, v2, 0x18
ANDS a1, a1, 0x8
BNE loop10


// 11 flush the FIFO
LDR a1, v2, 0x2c
MOVW a2, 0xffef
MOVT a2, 0xffff
ANDRS a1, a1, a2
STR a1, v2, 0x2c

// 12: clear any pending interrupts
MOVW a1, 0x7ff
STR a1, v2, 0x44  // 0x3f20 1044 <- 0x7ff  /// first 11 bits clear all interrupts
                  // UART0_ICR

// 13: set the baud rate
MOVW a1, #1       /// baud stuff
STR a1, v2, 0x24  // write(UART0_IBRD, #1)
MOVW a1, #40
STR a1, v2, 0x28  // write(UART0_FBRD, #40)

// enable fifos, bit 5 and 6 set 8 bit word size (do while uart is disabled)
MOVW a1, 0x60
STR a1, v2, 0x2c  // 0x3f20 102c <- 0x60   // 
                  // UART0_LCRH

// 15: enable uart (1<<0) enable tx and rx (1<<8)(1<<9) and set pins 14 and 15 to tx and rx
MOVW a1, 0x301
STR a1, v2, 0x30  // 0x3f20 1030 <- 0x301    /// 
                  // UART_CR


MOVW a1, 0x35 //   '5'
MOVW a2, 0x1000
MOVT a2, 0x3f20
STR a1, a2, 0x0 // write character to UART

// 16: set fifo interrupt on 1/8 full
MOVW a1, 0x0
STR a1, v2, 0x34  // 0x3f20 1034 <- 0x0   //  
                  // UART0_IFLS

// 17: enable fifo interrupts
MOVW a1, 0x10
STR a1, v2, 0x38  // 0x3f20 1038 <- 0x10 // set receive interrupt mask bit
                  // UART0_IMSC

// 18: enable CPU interrupts
//MOVW a1, 0xff7f
//MOVT a1, 0xffff
//word: 0xe12ff000
CPSIE 0x111


MOVW a1, 0x36 //   '6'
MOVW a2, 0x1000
MOVT a2, 0x3f20
STR a1, a2, 0x0 // write character to UART

//fffff7f

MOVW a1, 0x4
MOV pc, a1




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




address: 0x500 // 0x9100 // 0x2440
interrupt_handle:
SUBS lr, lr, 0x4
PUSH lr
PUSH a1

MOVW a1, 0xa //   '3'
MOVW a2, 0x1000
MOVT a2, 0x3f20
STR a1, a2, 0x0 // write character to UART
MOVW a1, 0xd //   '3'
MOVW a2, 0x1000
MOVT a2, 0x3f20
STR a1, a2, 0x0 // write character to UART
MOVW a1, 0x49 //   '3'
MOVW a2, 0x1000
MOVT a2, 0x3f20
STR a1, a2, 0x0 // write character to UART



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


//address: 0x1300 // 0x9300
//WORD: 0xea001ffe, 0xea00233a, 0xea002336, 0xea002332, 0xea00232e, 0xea00232a, 0xea002326, 0xea002322

address: 0x600
loop: 
MOVW v1, 0x0
MOVT v1, 0x3f20
MOVW v2, 0x8000

MOVW a1, 0x39 //   '9'
MOVW a2, 0x1000
MOVT a2, 0x3f20
STR a1, a2, 0x0 // write character to UART

STR v2, v1, 0x20
MOVW a1, 0x0
MOVT a1, 0x10
wait1: SUBS a1, a1, 0x01
BNE wait1

STR v2, v1, 0x2c

MOVW a1, 0x0
MOVT a1, 0x10
wait2: SUBS a1, a1, 0x01
BNE wait2

BAL loop
