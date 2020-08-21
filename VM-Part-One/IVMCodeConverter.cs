using System;
using System.Collections.Generic;
using System.Text;

namespace VM_Part_One
{
    interface IVMCodeConverter
    {
        string[] VMCodeConverter(string vmCode, int lineNumber);
    }
}
