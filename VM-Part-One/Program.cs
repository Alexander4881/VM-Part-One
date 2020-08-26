using System;
using System.IO;

namespace VM_Part_One
{
    class Program
    {
        private static readonly string tempFile = "./temp.txt";
        private static readonly string tempFileWrite = "./tempWrite.txt";

        static void Main(string[] args)
        {
            InPutFile input = new InPutFile();
            OutPutFile outPut = new OutPutFile(tempFileWrite);
            VMCode vmCode = new VMCode();

            try
            {
                StreamReader reader = input.InPut(StartMenu());

                input.RemoveComments(reader, new StreamWriter(tempFile, false));

                reader = input.InPut(tempFile);

                try
                {
                    string line;
                    short lineNum = 0;

                    //outPut.Write(vmCode.SetStartPointers());
                    

                    while ((line = reader.ReadLine()) != null)
                    {
                        line = line.ToUpper();
                        //outPut.Write(new string[] { $"// {line}" });
                        string[] temp = vmCode.VMCodeConverter(line, lineNum);
                        outPut.Write(temp);
                        lineNum++;
                    }

                    //vmCode.EndFile();

                    Console.WriteLine("Done Done !!!!!");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    reader.Close();
                    outPut.SaveFile("./","testVMCode","asm");
                    File.Delete(tempFile);
                    File.Delete(tempFileWrite);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        private static string StartMenu()
        {
            bool validFile = false;
            string inputFilePath = "";

            Console.WriteLine("You are about to enroll on a great journey from your vm code to assembler");

            do
            {
                // user information
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Path to Your File :");
                Console.ForegroundColor = ConsoleColor.White;

                // file path
                string filePath = Console.ReadLine();

                // normalise the string input
                filePath = filePath.Trim();
                filePath = filePath.ToLower();

                // file check
                if (ValidFile(filePath))
                {
                    // end loop
                    validFile = true;
                    inputFilePath = filePath;
                }

            } while (!validFile);

            return inputFilePath;
        }

        private static bool ValidFile(string path)
        {
            if (path.Contains(".vm") && File.Exists(path))
            {
                return true;
            }
            return false;
        }
    }
}