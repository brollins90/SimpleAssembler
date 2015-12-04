
address: 0x1100 // 0x9100 // 0x2440
interrupt_handle:
//SUBS lr, lr, 0x4
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



address: 0x0


// interrupt vector
MOVW a1, 0x1ffe
MOVT a1, 0xea00
MOVW a2, 0x0
STR a1, a2, 0x0

MOVW a1, 0x233a
MOVT a1, 0xea00
MOVW a2, 0x4
STR a1, a2, 0x0

MOVW a1, 0x2336
MOVT a1, 0xea00
MOVW a2, 0x8
STR a1, a2, 0x0

MOVW a1, 0x2332
MOVT a1, 0xea00
MOVW a2, 0xc
STR a1, a2, 0x0

MOVW a1, 0x232e
MOVT a1, 0xea00
MOVW a2, 0x10
STR a1, a2, 0x0

MOVW a1, 0x232a
MOVT a1, 0xea00
MOVW a2, 0x14
STR a1, a2, 0x0

MOVW a1, 0x2326
MOVT a1, 0xea00
MOVW a2, 0x18
STR a1, a2, 0x0

MOVW a1, 0x2322
MOVT a1, 0xea00
MOVW a2, 0x1c
STR a1, a2, 0x0


CPSID 0x111        // Disable interrupts 
CPS 0x12          // Switch to IRQ mode
MOVW sp, 0x8c18   // start IRQ stack at 0x8c18
CPS 0x13          // Switch to supervisor mode
MOVW sp, 0x9000   // start stack at 0x9000



BL uart_init

main_loop:

MOVW a1, 0x2a // '*'
PUSH a1
BL write_char
POP a1

BL interrupt_handle

MOVW a1, 0x0
MOVT a1, 0x20
PUSH a1
BL delay
POP a1

BAL main_loop








uart_init:
PUSH lr

MOVW v1, 0x0
MOVT v1, 0x3f20

MOVW v2, 0x1000
MOVT v2, 0x3f20

MOVW v3, 0xb200
MOVT v3, 0x3f00

MOVW v4, 0x5000
MOVT v4, 0x3f21

// 6: Disable interrupts
MOVW a1, 0xffff
MOVT a1, 0xffff
STR a1, v3, 0x1c  // 0x3f00 b21c <- 0xffff ffff
                  // IRQ_DISABLE1

STR a1, v3, 0x20  // 0x3f00 b220 <- 0xffff ffff
                  // IRQ_DISABLE2
                  
MOVW a1, 0x0
MOVT a1, 0x200
STR a1, v3, 0x14  // 0x3f00 b214 <- 0x0200 0000
                  // IRQ_ENABLE2

// 7: Set GPIO pin 14 to TX and 15 to RX modes
LDR a1, v1, 0x4   // a1 <- 0x3f20 0004
MOVW a4, 0xbfff
MOVT a4, 0xfffd
ANDRS a1, a1, a4
MOVW a4, 0x4000
MOVT a4, 0x2      // 0x24000 (1<<15)|(1<<18)
ORRRS a1, a1, a4  // a1 | 0x0002 4000
STR a1, v1, 0x4   // 0x3f20 0004 <- a1
                  // GPFSEL1

// 8a: Turn off pull up/down for pins 14 and 15
LDR a1, v1, 0x94  // a1 <- 0x3f20 0094
MOVW a4, 0xffff
MOVT a4, 0xfffc
ANDRS a1, a1, a4  // a1 & 0xfffc ffff
STR a1, v1, 0x94  // 0x3f20 0094 <- 1a
                  // GPPUD

// 8b: delay
MOVW a3, 0x96
PUSH a3
BL delay
POP a3

// 8c: Clock CPIO pins
LDR a1, v1, 0x98  // a1 <- 0x3f20 0098
MOVW a4, 0x3fff
MOVT a4, 0xffff
ANDRS a1, a1, a4  // a1 & 0xffff 3fff
STR a1, v1, 0x98  // 0x3f20 0098 <- 1a
                  // GPPUDCLK0

// 8d: delay
MOVW a3, 0x96
PUSH a3
BL delay
POP a3

// 8e:
MOVW a4, 0x0
STR a1, v1, 0x94  // 0x3f20 0094 <- 0x0
                  // GPPUD

// 8f: delay
MOVW a3, 0x96
PUSH a3
BL delay
POP a3

// 8g: 
LDR a1, v1, 0x98  // a1 <- 0x3f20 0098
MOVW a4, 0x3fff
MOVT a4, 0xffff
ANDRS a1, a1, a4  // a1 & 0xffff 3fff
STR a1, v1, 0x98  // 0x3f20 0098 <- 1a
                  // GPPUDCLK0

// 9: disable the uart
MOVW a1, 0x0
STR a1, v2, 0x0

// 10: wait for uart to stop receiving

// 11: flush the transmit FIFO

// 12: Clear pending interrupts

// 13: Set the BAUD rate
MOVW a1, #1
STR a1, v2, 0x24 // write(UART0_IBRD, #1)
MOVW a1, #40
STR a1, v2, 0x28 // write(UART0_FBRD, #40)

// 14: Enable FIFOs
MOVW a1, 0x60
STR a1, v2, 0x2c  // 0x3f20 102c <- 0x60   // bit 5 and 6 set 8 bit word size (do while uart is disabled)
                  // UART0_LCRH

// 15: Enable The UART and enable pins 14 and 15
MOVW a1, 0x301
STR a1, v2, 0x30  // 0x3f20 1030 <- 0x301    /// enable uart (1<<0) enable tx and rx (1<<8)(1<<9)
                  // UART_CR

// 16: Set FIFO lengths to 1/8
MOVW a1, 0x0
STR a1, v2, 0x34  // 0x3f20 1034 <- 0x0   //  set fifo interrupt on 1/8 full
                  // UART0_IFLS

// 17: Enable FIFO interrupts
MOVW a1, 0x10
STR a1, v2, 0x38  // 0x3f20 1038 <- 0x10 // set receive interrupt mask bit
                  // UART0_IMSC

// 18: Turn on CPU interrupts
MOVW a1, 0xff7f
MOVT a1, 0xffff
//word: 0xe12ff000
CPSIE 0x111

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
