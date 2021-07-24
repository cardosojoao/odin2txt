using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace odin2txt
{
    public partial class ParserOdin2Text
    {
        SortedDictionary<int, StringOptions> instructions;
        SortedDictionary<int, StringOptions> operands;

        private void SetupMaps()
        {


            instructions = new SortedDictionary<int, StringOptions>();
            operands = new SortedDictionary<int, StringOptions>();

            instructions[0x80] = new StringOptions("ADC", "");
            instructions[0x81] = new StringOptions("ADD", "");
            instructions[0x82] = new StringOptions("AND", "");
            instructions[0x83] = new StringOptions("BIT", "");
            instructions[0x84] = new StringOptions("CALL", "");
            instructions[0x85] = new StringOptions("CCF", "");
            instructions[0x86] = new StringOptions("CP", "");
            instructions[0x87] = new StringOptions("CPD", "");
            instructions[0x88] = new StringOptions("CPDR", "");
            instructions[0x89] = new StringOptions("CPI", "");
            instructions[0x8a] = new StringOptions("CPIR", "");
            instructions[0x8b] = new StringOptions("CPL", "");
            instructions[0x8c] = new StringOptions("DAA", "");
            instructions[0x8d] = new StringOptions("DEC", "");
            instructions[0x8e] = new StringOptions("DB", "DEFB");
            instructions[0x8f] = new StringOptions("DZ", "DEFZ");
            instructions[0x90] = new StringOptions("DS", "DEFS");
            instructions[0x91] = new StringOptions("DW", "DEFW");
            instructions[0x92] = new StringOptions("DI", "");
            instructions[0x93] = new StringOptions("OPT", "");
            instructions[0x94] = new StringOptions("DJNZ", "");
            instructions[0x95] = new StringOptions("EI", "");
            instructions[0x96] = new StringOptions("ENT", "");
            instructions[0x97] = new StringOptions("EQU", "");
            instructions[0x98] = new StringOptions("EX", "");
            instructions[0x99] = new StringOptions("EXX", "");
            instructions[0x9a] = new StringOptions("HALT", "");
            instructions[0x9b] = new StringOptions("IM", "");
            instructions[0x9c] = new StringOptions("IN", "");
            instructions[0x9d] = new StringOptions("INC", "");
            instructions[0x9e] = new StringOptions("IND", "");
            instructions[0x9f] = new StringOptions("INDR", "");
            instructions[0xa0] = new StringOptions("INI", "");
            instructions[0xa1] = new StringOptions("INIR", "");
            instructions[0xa2] = new StringOptions("JP", "");
            instructions[0xa3] = new StringOptions("JR", "");
            instructions[0xa4] = new StringOptions("LD", "");
            instructions[0xa5] = new StringOptions("LDD", "");
            instructions[0xa6] = new StringOptions("LDDR", "");
            instructions[0xa7] = new StringOptions("LDI", "");
            instructions[0xa8] = new StringOptions("LDIR", "");
            instructions[0xa9] = new StringOptions("NEG", "");
            instructions[0xaa] = new StringOptions("NOP", "");
            instructions[0xab] = new StringOptions("OR", "");
            instructions[0xac] = new StringOptions("ORG", "");
            instructions[0xad] = new StringOptions("OTDR", "");
            instructions[0xae] = new StringOptions("OTIR", "");
            instructions[0xaf] = new StringOptions("OUT", "");
            instructions[0xb0] = new StringOptions("OUTD", "");
            instructions[0xb1] = new StringOptions("OUTI", "");
            instructions[0xb2] = new StringOptions("POP", "");
            instructions[0xb3] = new StringOptions("PUSH", "");
            instructions[0xb4] = new StringOptions("RES", "");
            instructions[0xb5] = new StringOptions("RET", "");
            instructions[0xb6] = new StringOptions("RETI", "");
            instructions[0xb7] = new StringOptions("RETN", "");
            instructions[0xb8] = new StringOptions("RL", "");
            instructions[0xb9] = new StringOptions("RLA", "");
            instructions[0xba] = new StringOptions("RLC", "");
            instructions[0xbb] = new StringOptions("RLCA", "");
            instructions[0xbc] = new StringOptions("RLD", "");
            instructions[0xbd] = new StringOptions("RR", "");
            instructions[0xbe] = new StringOptions("RRA", "");
            instructions[0xbf] = new StringOptions("RRC", "");
            instructions[0xc0] = new StringOptions("RRCA", "");
            instructions[0xc1] = new StringOptions("RRD", "");
            instructions[0xc2] = new StringOptions("RST", "");
            instructions[0xc3] = new StringOptions("SBC", "");
            instructions[0xc4] = new StringOptions("SCF", "");
            instructions[0xc5] = new StringOptions("SET", "");
            instructions[0xc6] = new StringOptions("SLA", "");
            instructions[0xc7] = new StringOptions("SRA", "");
            instructions[0xc8] = new StringOptions("SRL", "");
            instructions[0xc9] = new StringOptions("SUB", "");
            instructions[0xca] = new StringOptions("XOR", "");
            instructions[0xcb] = new StringOptions("SL1", "");
            instructions[0xcc] = new StringOptions("SWAP", "SWAPNIB");
            instructions[0xcd] = new StringOptions("MIRR", "MIRROR");
            instructions[0xce] = new StringOptions("TEST", "");
            instructions[0xcf] = new StringOptions("BSLA", "");
            instructions[0xd0] = new StringOptions("BSRA", "");
            instructions[0xd1] = new StringOptions("BSRL", "");
            instructions[0xd2] = new StringOptions("BSRF", "");
            instructions[0xd3] = new StringOptions("BRLC", "");
            instructions[0xd4] = new StringOptions("MUL", "");
            instructions[0xd5] = new StringOptions("OTIB", "OUTINB");
            instructions[0xd6] = new StringOptions("NREG", "NEXTREG");
            instructions[0xd7] = new StringOptions("PXDN", "PIXELDN");
            instructions[0xd8] = new StringOptions("PXAD", "PIXELAD");
            instructions[0xd9] = new StringOptions("STAE", "SETAE");
            instructions[0xda] = new StringOptions("LDIX", "");
            instructions[0xdb] = new StringOptions("LDWS", "");
            instructions[0xdc] = new StringOptions("LDDX", "");
            instructions[0xdd] = new StringOptions("LIRX", "LDIRX");
            instructions[0xde] = new StringOptions("LPRX", "LDPIRX");
            instructions[0xdf] = new StringOptions("LDRX", "LDDRX");
            instructions[0xe0] = new StringOptions("BIN", "INCBIN");
            instructions[0xe1] = new StringOptions("LOAD", "INCLUDE");
            instructions[0xe2] = new StringOptions("SAVE", "");
            instructions[0xe3] = new StringOptions("DC", "DEFC");

            instructions[0xff] = new StringOptions("UNKNOWN", ""); // unknown

            operands[0x80] = new StringOptions("B", "");
            operands[0x81] = new StringOptions("C", "");
            operands[0x82] = new StringOptions("D", "");
            operands[0x83] = new StringOptions("E", "");
            operands[0x84] = new StringOptions("H", "");
            operands[0x85] = new StringOptions("L", "");
            operands[0x86] = new StringOptions("AF'", "");
            operands[0x87] = new StringOptions("A", "");
            operands[0x88] = new StringOptions("BC", "");
            operands[0x89] = new StringOptions("DE", "");
            operands[0x8a] = new StringOptions("HL", "");
            operands[0x8b] = new StringOptions("SP", "");
            operands[0x8c] = new StringOptions("AF", "");
            operands[0x8d] = new StringOptions("I", "");
            operands[0x8e] = new StringOptions("IX", "");
            operands[0x8f] = new StringOptions("IY", "");
            operands[0x90] = new StringOptions("M", "");
            operands[0x91] = new StringOptions("NC", "");
            operands[0x92] = new StringOptions("NV", "");
            operands[0x93] = new StringOptions("NZ", "");
            operands[0x94] = new StringOptions("P", "");
            operands[0x95] = new StringOptions("PE", "");
            operands[0x96] = new StringOptions("PO", "");
            operands[0x97] = new StringOptions("R", "");
            operands[0x98] = new StringOptions("V", "");
            operands[0x99] = new StringOptions("Z", "");
            operands[0x9a] = new StringOptions("<<", "");
            operands[0x9b] = new StringOptions(">>", "");
            operands[0x9c] = new StringOptions("MOD", "");
            operands[0x9d] = new StringOptions("XH", "IXH");
            operands[0x9e] = new StringOptions("XL", "IXL");
            operands[0x9f] = new StringOptions("YH", "IYH");
            operands[0xa0] = new StringOptions("YL", "IYL");

            operands[0xff] = new StringOptions("UNKNOWN", ""); // unknown
        }



        private int findChar(string buffer, int size, char c)
        {

            return buffer.IndexOf(c);
            /*
                        int charPos = -1;

                        //C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent to pointers to value types:
                        //ORIGINAL LINE: byte* pos = (byte*)memchr(buffer, c, size);
                        //C++ TO C# CONVERTER TODO TASK: The memory management function 'memchr' has no equivalent in C#:
                        byte pos = (byte)memchr(buffer, c, size);
                        if (pos != null)
                        {
                            charPos = pos - buffer;
                        }

                        return charPos;
            */
        }



        //private void parseLine(ref byte buffer, int size, ofstream outFile, int lineNum, StrMap instructions, StrMap operands)
        //{
        //    bool insToken = true;
        //    //C++ TO C# CONVERTER TODO TASK: Pointer arithmetic is detected on this variable, so pointers on this variable are left unchanged:
        //    byte* pos = buffer;
        //    //C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent to pointers to value types:
        //    //ORIGINAL LINE: byte* end = buffer + size;
        //    byte end = (byte)(buffer + size);

        //    stringstream ss = new stringstream();

        //    for (; pos < end; ++pos)
        //    {
        //        if (*pos == 0) // end line
        //        {
        //            ss << '\n';
        //        }
        //        else if (*pos >= 0x0A && *pos <= (byte)' ') // insert spaces
        //        {
        //            int numSpaces;
        //            if (*pos == 0x0A) // next character determines the number of spaces
        //            {
        //                ++pos;
        //                numSpaces = (int)(*pos);
        //            }
        //            else // code determines the number of spaces
        //            {
        //                numSpaces = 33 - *pos;
        //            }

        //            ss << (string)(numSpaces, ' ');
        //        }
        //        else if (*pos > (byte)' ' && *pos <= (char)127) // standard ASCII characters
        //        {
        //            ss << *pos;
        //        }
        //        else // instruction and operand tokens
        //        {
        //            if (insToken)
        //            {
        //                StrMapConstIter iter = instructions.find((int)(*pos));
        //                if (iter != instructions.end())
        //                {
        //                    int index = (*(pos + 1) == 1 ? 1 : 0);
        //                    ss << iter.second.option[index];
        //                    pos += index;

        //                    if (*(pos + 1) > ' ')
        //                    {
        //                        ss << ' ';
        //                    }
        //                }

        //                insToken = false;
        //            }
        //            else // operand token
        //            {
        //                StrMapConstIter iter = operands.find((int)(*pos));
        //                if (iter != operands.end())
        //                {
        //                    int index = (*(pos + 1) == 1 ? 1 : 0);
        //                    ss << iter.second.option[index];
        //                    pos += index;
        //                }
        //            }
        //        }
        //    }

        //    if (lineNum > 1)
        //    {
        //        outFile << ss.str();
        //    }
        //}


    }
}

