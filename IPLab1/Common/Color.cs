﻿using System;

namespace IPLab1.Common;

public struct Color
{
    public byte Red;
    public byte Green;
    public byte Blue;
    public byte Alpha;

    public Color(byte red, byte green, byte blue, byte alpha)
    {
        Red = red;
        Green = green;
        Blue = blue;
        Alpha = alpha;
    }

    public byte R => Red;
    public byte G => Green;
    public byte B => Blue;
    public byte A => Alpha;
    
    public byte I {
        get
        {
            if (R != G || G != B)
            {
                throw new InvalidOperationException("Must be grayscale color");
            }
            return R;
        }
    }
}
