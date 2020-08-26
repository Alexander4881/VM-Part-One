using System;
using System.Collections.Generic;
using System.IO;
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
        int labelNum;

        public string[] EndFile()
        {
            List<string> temp = new List<string>();

            temp.Add("(END)");
            temp.Add("@END");
            temp.Add("0;JMP");

            return temp.ToArray();
        }

        public string[] SetStartPointers()
        {
            List<string> temp = new List<string>();

            //Pointer
            temp.Add("@" + stackPointer);
            temp.Add("D=A");
            temp.Add("@SP");
            temp.Add("M=D");

            //Local
            temp.Add("@" + localPointer);
            temp.Add("D=A");
            temp.Add("@LCL");
            temp.Add("M=D");

            //ARG
            temp.Add("@" + argumentPointer);
            temp.Add("D=A");
            temp.Add("@ARG");
            temp.Add("M=D");

            //THIS
            temp.Add("@" + thisPointer);
            temp.Add("D=A");
            temp.Add("@THIS");
            temp.Add("M=D");

            //THAT
            temp.Add("@" + thatPointer);
            temp.Add("D=A");
            temp.Add("@THAT");
            temp.Add("M=D");

            return temp.ToArray();
        }

        public string[] VMCodeConverter(string vmCode, int lineNumber)
        {
            string[] split = vmCode.Split(" ");
            // remove - in the if-goto command
            if (split[0].Contains("-"))
            {
                split[0] = split[0].Replace("-", "");
            }

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
                    temp.Add("D=M");
                    temp.Add("A=A-1");
                    temp.Add("M=M-D");
                    break;

                case CommandType.NEG:
                    temp.Add("@SP");
                    temp.Add("A=M-1");
                    temp.Add("D=M");
                    // we user lineNumber as a uniqe identifyer
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
                    temp.Add("D=M-D");
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
                    temp.Add("D=M-D");
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
                    temp.Add("D=M-D");
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
                    MemoryAccess memoryAccess = (MemoryAccess)Enum.Parse(typeof(MemoryAccess), split[1]);
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

                    MemoryAccess memoryAccesss = (MemoryAccess)Enum.Parse(typeof(MemoryAccess), split[1]);
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
                            temp.Add("@LCL");
                            temp.Add("D=M");
                            temp.Add("@" + split[2]);
                            temp.Add("D=D+A");
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

                case CommandType.LABLE:
                    temp.Add($"({split[1]})");
                    break;

                case CommandType.GOTO:
                    temp.Add("@" + split[1]);
                    temp.Add("0;JMP");
                    break;

                case CommandType.IFGOTO:
                    temp.Add("@SP");
                    temp.Add("AM=M-1");
                    temp.Add("D=M");
                    temp.Add("A=A-1");
                    temp.Add("@" + split[1]);
                    temp.Add("D;JNE");
                    break;

                case CommandType.CALL:
                    // TODO: make call
                    string lable = "RETURN_LABEL" + labelNum;
                    labelNum++;

                    
                    temp.Add("@" + split[1]);
                    temp.Add("D=A");
                    temp.Add("@SP");
                    temp.Add("A=M");
                    temp.Add("M=D");
                    temp.Add("@SP");
                    temp.Add("M=M+1");
                    // push 
                    // push lcl
                    temp.Add("@LCL");
                    temp.Add("D=M");
                    temp.Add("@SP");
                    temp.Add("A=M");
                    temp.Add("M=D");
                    temp.Add("@SP");
                    temp.Add("M=M+1");
                    // ARG
                    temp.Add("@ARG");
                    temp.Add("D=M");
                    temp.Add("@SP");
                    temp.Add("A=M");
                    temp.Add("M=D");
                    temp.Add("@SP");
                    temp.Add("M=M+1");
                    // THIS
                    temp.Add("@THIS");
                    temp.Add("D=M");
                    temp.Add("@SP");
                    temp.Add("A=M");
                    temp.Add("M=D");
                    temp.Add("@SP");
                    temp.Add("M=M+1");
                    // THAT
                    temp.Add("@THAT");
                    temp.Add("D=M");
                    temp.Add("@SP");
                    temp.Add("A=M");
                    temp.Add("M=D");
                    temp.Add("@SP");
                    temp.Add("M=M+1");
                    

                    temp.Add("@SP");
                    temp.Add("D=M");
                    temp.Add("@5");
                    temp.Add("D=D-A");
                    temp.Add("@" + split[2]);
                    temp.Add("D=D-A");
                    temp.Add("@ARG");
                    temp.Add("M=D");
                    temp.Add("@SP");
                    temp.Add("D=M");
                    temp.Add("@LCL");
                    temp.Add("M=D");
                    temp.Add("@" + split[1]);
                    temp.Add("0;JMP");
                    temp.Add($"({split[1]})");


                    break;

                case CommandType.RETURN:
                    // TODO: make return
                    temp.Add("@LCL");
                    temp.Add("D=M");
                    temp.Add("@FRAME");
                    temp.Add("@5");
                    temp.Add("A=D-A");
                    temp.Add("D=M");
                    temp.Add("@RET");
                    temp.Add("M=D");
                    // POP FORMAT 1
                    temp.Add("@ARG");
                    temp.Add("D=M");
                    temp.Add("@0");
                    temp.Add("D=D+A");
                    temp.Add("@R13");
                    temp.Add("M=D");
                    temp.Add("@SP");
                    temp.Add("AM=M-1");
                    temp.Add("D=M");
                    temp.Add("@R13");
                    temp.Add("A=M");
                    temp.Add("M=D");
                    
                    
                    temp.Add("@ARG");
                    temp.Add("D=M");
                    temp.Add("@SP");
                    temp.Add("M=D+1");
                    temp.Add("@FRAME");
                    temp.Add("D=M-1");
                    temp.Add("AM=D");
                    temp.Add("D=M");
                    temp.Add("@THAT");
                    temp.Add("M=D");
                    temp.Add("@FRAME");
                    temp.Add("D=M-1");
                    temp.Add("AM=D");
                    temp.Add("D=M");
                    temp.Add("@THIS");
                    temp.Add("M=D");
                    temp.Add("@FRAME");
                    temp.Add("D=M-1");
                    temp.Add("AM=D");
                    temp.Add("D=M");
                    temp.Add("@ARG");
                    temp.Add("M=D");
                    temp.Add("@FRAME");
                    temp.Add("D=M-1");
                    temp.Add("AM=D");
                    temp.Add("D=M");
                    temp.Add("@LCL");
                    temp.Add("M=D");
                    temp.Add("@RET");
                    temp.Add("A=M");
                    temp.Add("0;JMP");
                    break;

                case CommandType.FUNCTION:
                    // TODO: make function
                    temp.Add($"({split[1]})");
                    for (int i = 0; i < int.Parse(split[2]); i++)
                    {
                        temp.Add("@0");
                        temp.Add("D=A");
                        temp.Add("@SP");
                        temp.Add("A=M");
                        temp.Add("M=D");
                        temp.Add("@SP");
                        temp.Add("M=M+1");
                    }
                    break;
            }
            return temp.ToArray();
        }
    }
}
