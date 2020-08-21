using System;
using System.Collections.Generic;
using System.Text;

namespace VM_Part_One
{
    class VMCode : IVMCodeConverter
    {
        private static short stackPointer = 256;
        private readonly static short localPointer = 300;
        private readonly static short argumentPointer = 400;
        private readonly static short thisPointer = 3000;
        private readonly static short thatPointer = 3010;
        private readonly static short globalPointer = 3020;

        Dictionary<string, short> stack = new Dictionary<string, short>()
        {
            { "local", localPointer },
            { "argument", argumentPointer },
            { "this", thisPointer },
            { "that", thatPointer },
            { "static", globalPointer }
        };

        public Dictionary<string, short> Stack { get => stack; set => stack = value; }

        public string[] VMCodeConverter(string vmCode, int lineNumber)
        {
            string[] split = vmCode.Split(" ");
            CommandType type = (CommandType)Enum.Parse(typeof(CommandType), split[0]);
            List<string> temp = new List<string>();

            switch (type)
            {
                case CommandType.ADD:
                    temp.Add("@SP");
                    temp.Add("AM=M-1");
                    temp.Add("D=M");
                    temp.Add("A=A-1");
                    temp.Add("M=D+M");

                    break;
                case CommandType.SUB:
                    temp.Add("@SP");
                    temp.Add("AM=M-1");
                    temp.Add("A=A-1");
                    temp.Add("D=M");
                    temp.Add("A=A+1");
                    temp.Add("D=D-M");
                    temp.Add("A=A-1");
                    temp.Add("M=D");

                    break;
                case CommandType.NEG:
                    temp.Add("@SP");
                    temp.Add("A=M-1");
                    temp.Add("D=M");

                    temp.Add("@LESSTHAN" + lineNumber + "");
                    temp.Add("D;JLT");

                    temp.Add("@SP");
                    temp.Add("A=M-1");
                    temp.Add("M=!M");
                    temp.Add("M=M-1");
                    temp.Add("@CONTINUE" + lineNumber + "");
                    temp.Add("0;JMP");

                    temp.Add("(LESSTHAN" + lineNumber + ")");
                    temp.Add("@SP");
                    temp.Add("A=M-1");
                    temp.Add("M=!M");
                    temp.Add("M=M+1");
                    temp.Add("(CONTINUE" + lineNumber + ")");
                    break;
                case CommandType.EQ:
                    temp.Add("@SP");
                    temp.Add("AM=M-1");
                    temp.Add("D=M");
                    temp.Add("A=A-1");
                    temp.Add("D=D-M");
                    temp.Add("@NOTEQUAL" + lineNumber + "");
                    temp.Add("D;JNE");
                    temp.Add("@SP");
                    temp.Add("A=M-1");
                    temp.Add("M=-1");
                    temp.Add("@CONTINUE" + lineNumber + "");
                    temp.Add("0;JMP");

                    temp.Add("(NOTEQUAL" + lineNumber + ")");
                    temp.Add("@SP");
                    temp.Add("A=M-1");
                    temp.Add("M=0");
                    temp.Add("(CONTINUE" + lineNumber + ")");
                    break;
                case CommandType.GT:
                    temp.Add("@SP");
                    temp.Add("AM=M-1");
                    temp.Add("D=M");
                    temp.Add("A=A-1");
                    temp.Add("D=D-M");
                    temp.Add("@GREATHERTHAN" + lineNumber + "");
                    temp.Add("D;JGE");
                    temp.Add("@SP");
                    temp.Add("A=M-1");
                    temp.Add("M=-1");
                    temp.Add("@CONTINUE" + lineNumber + "");
                    temp.Add("0;JMP");

                    temp.Add("(GREATHERTHAN" + lineNumber + ")");
                    temp.Add("@SP");
                    temp.Add("A=M-1");
                    temp.Add("M=0");
                    temp.Add("(CONTINUE" + lineNumber + ")");

                    break;
                case CommandType.LT:
                    temp.Add("@SP");
                    temp.Add("AM=M-1");
                    temp.Add("D=M");
                    temp.Add("A=A-1");
                    temp.Add("D=D-M");
                    temp.Add("@LESSTHAN" + lineNumber + "");
                    temp.Add("D;JLE");
                    temp.Add("@SP");
                    temp.Add("A=M-1");
                    temp.Add("M=-1");
                    temp.Add("@CONTINUE" + lineNumber + "");
                    temp.Add("0;JMP");

                    temp.Add("(LESSTHAN" + lineNumber + ")");
                    temp.Add("@SP");
                    temp.Add("A=M-1");
                    temp.Add("M=0");
                    temp.Add("(CONTINUE" + lineNumber + ")");
                    break;
                case CommandType.AND:
                    temp.Add("@SP");
                    temp.Add("AM=M-1");
                    temp.Add("D=M");
                    temp.Add("A=A-1");
                    temp.Add("M=D&M");

                    break;
                case CommandType.OR:
                    temp.Add("@SP");
                    temp.Add("AM=M-1");
                    temp.Add("D=M");
                    temp.Add("A=A-1");
                    temp.Add("M=D|M");
                    break;
                case CommandType.NOT:
                    temp.Add("@SP");
                    temp.Add("A=M-1");
                    temp.Add("M=!M");
                    break;
                case CommandType.PUSH:
                    MemoryAccess memoryAccess = (MemoryAccess)Enum.Parse(typeof(MemoryAccess), split[1].ToUpper());
                    switch (memoryAccess)
                    {
                        case MemoryAccess.ARGUMENT:
                            temp.Add("@" + split[2]);
                            temp.Add("D=A");
                            temp.Add("@ARG");
                            temp.Add("A=D+M");
                            temp.Add("D=M");

                            temp.Add("@SP");
                            temp.Add("A=M");
                            temp.Add("M=D");
                            temp.Add("@SP");
                            temp.Add("M=M+1");
                            break;
                        case MemoryAccess.LOCAL:
                            temp.Add("@" + split[2]);
                            temp.Add("D=A");
                            temp.Add("@LCL");
                            temp.Add("A=D+M");
                            temp.Add("D=M");

                            temp.Add("@SP");
                            temp.Add("A=M");
                            temp.Add("M=D");
                            temp.Add("@SP");
                            temp.Add("M=M+1");
                            break;
                        case MemoryAccess.STATIC:

                            temp.Add("@" + (int.Parse(split[2]) + 16));
                            temp.Add("D=M");

                            temp.Add("@SP");
                            temp.Add("A=M");
                            temp.Add("M=D");
                            temp.Add("@SP");
                            temp.Add("M=M+1");
                            break;
                        case MemoryAccess.CONSTANT:
                            temp.Add("@" + split[2]);
                            temp.Add("D=A");
                            temp.Add("@SP");
                            temp.Add("A=M");
                            temp.Add("M=D");
                            temp.Add("@SP");
                            temp.Add("M=M+1");
                            break;
                        case MemoryAccess.THIS:
                            temp.Add("@" + split[2]);
                            temp.Add("D=A");
                            temp.Add("@THIS");
                            temp.Add("A=D+M");
                            temp.Add("D=M");

                            temp.Add("@SP");
                            temp.Add("A=M");
                            temp.Add("M=D");
                            temp.Add("@SP");
                            temp.Add("M=M+1");
                            break;
                        case MemoryAccess.THAT:
                            temp.Add("@" + split[2]);
                            temp.Add("D=A");
                            temp.Add("@THAT");
                            temp.Add("A=D+M");
                            temp.Add("D=M");

                            temp.Add("@SP");
                            temp.Add("A=M");
                            temp.Add("M=D");
                            temp.Add("@SP");
                            temp.Add("M=M+1");
                            break;
                        case MemoryAccess.POINTER:

                            if (int.Parse(split[2]) == 0)
                            {
                                temp.Add("@THIS");
                            }
                            else
                            {
                                temp.Add("@THAT");
                            }
                            temp.Add("D=M");

                            temp.Add("@SP");
                            temp.Add("A=M");
                            temp.Add("M=D");
                            temp.Add("@SP");
                            temp.Add("M=M+1");

                            break;
                        case MemoryAccess.TEMP:
                            temp.Add("@R" + (int.Parse(split[2]) + 5));
                            temp.Add("D=M");

                            temp.Add("@SP");
                            temp.Add("A=M");
                            temp.Add("M=D");
                            temp.Add("@SP");
                            temp.Add("M=M+1");
                            break;
                    }
                    break;
                case CommandType.POP:

                    MemoryAccess memoryAccesss = (MemoryAccess)Enum.Parse(typeof(MemoryAccess), split[1].ToUpper());
                    switch (memoryAccesss)
                    {
                        case MemoryAccess.ARGUMENT:
                            temp.Add("@" + split[2]);
                            temp.Add("D=A");
                            temp.Add("@ARG");
                            temp.Add("D=D+M");
                            temp.Add("@R13");
                            temp.Add("M=D");

                            temp.Add("@SP");
                            temp.Add("AM=M-1");
                            temp.Add("D=M");
                            temp.Add("@R13");
                            temp.Add("A=M");
                            temp.Add("M=D");
                            break;
                        case MemoryAccess.LOCAL:
                            temp.Add("@" + split[2]);
                            temp.Add("D=A");
                            temp.Add("@LCL");
                            temp.Add("D=D+M");
                            temp.Add("@R13");
                            temp.Add("M=D");

                            temp.Add("@SP");
                            temp.Add("AM=M-1");
                            temp.Add("D=M");
                            temp.Add("@R13");
                            temp.Add("A=M");
                            temp.Add("M=D");
                            break;
                        case MemoryAccess.STATIC:

                            temp.Add("@" + (int.Parse(split[2]) + 16));
                            temp.Add("D=A");
                            temp.Add("@R13");
                            temp.Add("M=D");

                            temp.Add("@SP");
                            temp.Add("AM=M-1");
                            temp.Add("D=M");
                            temp.Add("@R13");
                            temp.Add("A=M");
                            temp.Add("M=D");
                            break;
                        case MemoryAccess.CONSTANT:
                            temp.Add("@" + split[2]);
                            temp.Add("D=A");
                            temp.Add("@SP");
                            temp.Add("A=M");
                            temp.Add("M=D");
                            temp.Add("@SP");
                            temp.Add("M=M+1");
                            break;
                        case MemoryAccess.THIS:
                            temp.Add("@" + split[2]);
                            temp.Add("D=A");
                            temp.Add("@THIS");
                            temp.Add("D=D+M");
                            temp.Add("@R13");
                            temp.Add("M=D");

                            temp.Add("@SP");
                            temp.Add("AM=M-1");
                            temp.Add("D=M");
                            temp.Add("@R13");
                            temp.Add("A=M");
                            temp.Add("M=D");
                            break;
                        case MemoryAccess.THAT:
                            temp.Add("@" + split[2]);
                            temp.Add("D=A");
                            temp.Add("@THAT");
                            temp.Add("D=D+M");
                            temp.Add("@R13");
                            temp.Add("M=D");

                            temp.Add("@SP");
                            temp.Add("AM=M-1");
                            temp.Add("D=M");
                            temp.Add("@R13");
                            temp.Add("A=M");
                            temp.Add("M=D");
                            break;
                        case MemoryAccess.POINTER:

                            temp.Add("@SP");
                            temp.Add("AM=M-1");
                            temp.Add("D=M");

                            if (int.Parse(split[2]) == 0)
                            {
                                temp.Add("@THIS");
                            }
                            else
                            {
                                temp.Add("@THAT");
                            }
                            temp.Add("M=D");

                            break;
                        case MemoryAccess.TEMP:

                            temp.Add("@R" + (int.Parse(split[2]) + 5));
                            temp.Add("D=A");
                            temp.Add("@R13");
                            temp.Add("M=D");

                            temp.Add("@SP");
                            temp.Add("AM=M-1");
                            temp.Add("D=M");
                            temp.Add("@R13");
                            temp.Add("A=M");
                            temp.Add("M=D");
                            break;
                    }
                    break;
            }
            return temp.ToArray();
        }
    }
}
