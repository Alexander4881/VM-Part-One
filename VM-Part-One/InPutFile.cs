using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace VM_Part_One
{
    class InPutFile : IInPut
    {
        public StreamReader InPut(string path)
        {
            return new StreamReader(path);
        }

        /// <summary>
        /// preps the file
        /// </summary>
        /// <param name="reader">a stream reader</param>
        /// <param name="writer">a writer stream</param>
        public void PrepFile(StreamReader reader, StreamWriter writer)
        {
            try
            {
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    line = line.Replace(" ", "");
                    writer.WriteLine(line);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Sorry file could not be read");
                Console.WriteLine(e.Message);
            }
            finally
            {
                reader.Close();
                writer.Flush();
            }
        }
    }
}
