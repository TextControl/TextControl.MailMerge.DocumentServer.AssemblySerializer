using System;

namespace myDummyClass
{
    public class Class1
    {
        public int int32Test { get; set; }
        public long  longTest { get; set; }
        public float floatTest { get; set; }
        public string stringTest { get; set; }
        public byte byteTest { get; set; }
        public decimal decimalTest { get; set; }
        public double doubleTest { get; set; }
        public uint uintTest { get; set; }
        public ulong ulongTest { get; set; }
        public short shortTest { get; set; }
        public ushort ushortTest { get; set; }

        public string GetString()
        {
            return "Hi";
        }

        public MyDummyClass Dummy { get; set; }
    }
}
