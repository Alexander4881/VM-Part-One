using System;
using System.IO;

namespace VM_Part_One
{
    class Program
    {
        static void Main(string[] args)
        {
            InPutFile input = new InPutFile();
            OutPutFile outPut = new OutPutFile();
            VMCode vmCode = new VMCode();

            StreamReader reader = input.InPut(StartMenu());

            

        }
        private static string StartMenu()
        {
            bool validFile = false;
            string inputFilePath = "";

            Console.WriteLine("You are about to enroll on a great journey from your assembler code to binay");

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
            if (path.Contains(".txt") && File.Exists(path))
            {
                return true;
            }
            return false;
        }
    }
}