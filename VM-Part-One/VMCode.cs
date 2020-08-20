using System;
using System.Collections.Generic;
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
        private readonly static short globalPointer = 3020;

        Dictionary<string, short> stack = new Dictionary<string, short>()
        {
            { "local", localPointer },
            { "argument", argumentPointer },
            { "this", thisPointer },
            { "that", thatPointer },
            { "static", globalPointer }
        };

        public Dictionary<string, short> Stack { get => stack; set => stack = value; }

        public string[] VMCodeConverter(string vmCode)
        {
            // split input string into 3 parts 
            // push constant 10
            //
            // part one
            //
            // first part 
            // push
            // add
            // sub
            // neg
            // eq
            // get
            // lt // less than
            // and
            // or
            // not
            //
            // part 2
            //
            // local
            // argument
            // this
            // that
            // constant
            // static
            // temp
            // pointer


            // push ryk til stacken
            // pop ryk fra stacken og ned til

            throw new NotImplementedException();
        }
    }
}
