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
        /// remove all comments starting with //
        /// </summary>
        /// <param name="reader">stream reader</param>
        /// <param name="writer">stream writer</param>
        public void RemoveComments(StreamReader reader, StreamWriter writer)
        {
            try
            {
                string line;
                short lineNum = 0;

                while ((line = reader.ReadLine()) != null)
                {
                    // check comment
                    if (line.Contains("//"))
                    {
                        line = RemoveComments(line);
                    }


                    if (!String.IsNullOrEmpty(line))
                    {
                        line = line.Trim();
                        writer.WriteLine(line);
                        lineNum++;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                writer.Close();
            }
        }


        /// <summary>
        /// remove the comments from the file
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private static string RemoveComments(string line)
        {
            int commentStart = line.IndexOf("//");
            line = line.Remove(commentStart);
            line = line.Trim();

            return line;
        }
    }
}
