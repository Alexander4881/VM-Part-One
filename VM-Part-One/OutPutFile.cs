using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace VM_Part_One
{
    class OutPutFile : IOutPut
    {
        string fileLocation;
        StreamWriter writer;
        public StreamWriter Writer { get => writer; set => writer = value; }

        public OutPutFile(StreamWriter writer,string localFile)
        {
            Writer = writer;
            fileLocation = localFile;
        }


        public void Write(string[] data)
        {
            foreach (string line in data)
            {
                Writer.WriteLine(line);
            }
            Writer.Flush();
        }

        public void SaveFile(string path,string name, string fileType)
        {
            Writer.Close();
            File.Copy(fileLocation,$"{path}/{name}.{fileType}");
        }
    }
}
