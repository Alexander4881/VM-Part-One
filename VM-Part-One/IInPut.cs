using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace VM_Part_One
{
    interface IInPut
    {
        StreamReader InPut(string path);
    }
}
