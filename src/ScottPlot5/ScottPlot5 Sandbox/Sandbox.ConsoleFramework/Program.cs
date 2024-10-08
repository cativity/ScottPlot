﻿using System;
using ScottPlot;

namespace Sandbox.ConsoleFramework;

public static class Program
{
    public static void Main()
    {
        TestDoubleToGeneric();
        TestGenericToDouble();
    }

    public static void TestDoubleToGeneric()
    {
        NumericConversion.DoubleToGeneric(123.45, out double vDouble);
        Console.WriteLine(vDouble);

        NumericConversion.DoubleToGeneric(123.45, out float vSingle);
        Console.WriteLine(vSingle);

        NumericConversion.DoubleToGeneric(123.45, out int vInt32);
        Console.WriteLine(vInt32);

        NumericConversion.DoubleToGeneric(123.45, out uint vUint32);
        Console.WriteLine(vUint32);

        NumericConversion.DoubleToGeneric(123.45, out long vInt64);
        Console.WriteLine(vInt64);

        NumericConversion.DoubleToGeneric(123.45, out ulong vUint64);
        Console.WriteLine(vUint64);

        NumericConversion.DoubleToGeneric(123.45, out short vInt16);
        Console.WriteLine(vInt16);

        NumericConversion.DoubleToGeneric(123.45, out ushort vUint16);
        Console.WriteLine(vUint16);

        NumericConversion.DoubleToGeneric(123.45, out decimal vDecimal);
        Console.WriteLine(vDecimal);

        NumericConversion.DoubleToGeneric(123.45, out byte vByte);
        Console.WriteLine(vByte);

        NumericConversion.DoubleToGeneric(45292, out DateTime vDateTime);
        Console.WriteLine(vDateTime);
    }

    public static void TestGenericToDouble()
    {
        double vDouble = 123.45;
        Console.WriteLine(NumericConversion.GenericToDouble(ref vDouble));

        float vSingle = 123.45f;
        Console.WriteLine(NumericConversion.GenericToDouble(ref vSingle));

        int vInt32 = 12345;
        Console.WriteLine(NumericConversion.GenericToDouble(ref vInt32));

        uint vUint32 = 12345;
        Console.WriteLine(NumericConversion.GenericToDouble(ref vUint32));

        long vInt64 = 12345;
        Console.WriteLine(NumericConversion.GenericToDouble(ref vInt64));

        ulong vUint64 = 12345;
        Console.WriteLine(NumericConversion.GenericToDouble(ref vUint64));

        short vInt16 = 12345;
        Console.WriteLine(NumericConversion.GenericToDouble(ref vInt16));

        ushort vUint16 = 12345;
        Console.WriteLine(NumericConversion.GenericToDouble(ref vUint16));

        decimal vDecimal = new decimal(123.45);
        Console.WriteLine(NumericConversion.GenericToDouble(ref vDecimal));

        byte vByte = 123;
        Console.WriteLine(NumericConversion.GenericToDouble(ref vByte));

        DateTime vDateTime = new DateTime(2024, 1, 1);
        Console.WriteLine(NumericConversion.GenericToDouble(ref vDateTime));
    }
}
