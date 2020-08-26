using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace VM_Part_One
{
    class OutPutFile : IOutPut
    {
        string fileLocation;

        public OutPutFile(string localFile)
        {
            fileLocation = localFile;
        }


        public void Write(string[] data)
        {
            using (StreamWriter wr = new StreamWriter(fileLocation,true))
            {
                foreach (string line in data)
                {
                    wr.WriteLine(line);
                }
                wr.Close();
            }
        }

        /// <summary>
        /// copy temp file to a given file
        /// </summary>
        /// <param name="path"></param>
        /// <param name="name"></param>
        /// <param name="fileType"></param>
        public void SaveFile(string path,string name, string fileType)
        {
            File.Copy(fileLocation,$"{path}/{name}.{fileType}",true);
        }
    }
}
