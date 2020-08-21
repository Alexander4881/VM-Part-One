using System;
using System.Collections.Generic;
using System.Text;

namespace VM_Part_One
{
    public enum MemoryAccess
    {
        ARGUMENT,
        LOCAL,
        STATIC,
        CONSTANT,
        THIS,
        THAT,
        POINTER,
        TEMP
    }
}
