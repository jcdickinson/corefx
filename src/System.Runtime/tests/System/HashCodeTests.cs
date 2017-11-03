﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Xunit;

public static class HashCodeTests
{

#   if SYSTEM_HASHCODE_TESTVECTORS
    [Theory]
    [InlineData(0x02cc5d05U)]
    [InlineData(0xa3643705U, 0x64636261U)]
    [InlineData(0x4603e94cU, 0x64636261U, 0x33323130U)]
    [InlineData(0xd8a1e80fU, 0x64636261U, 0x33323130U, 0x68676665U)]
    [InlineData(0x4b62a7cfU, 0x64636261U, 0x33323130U, 0x68676665U, 0x37363534U)]
    [InlineData(0xc33a7641U, 0x64636261U, 0x33323130U, 0x68676665U, 0x37363534U, 0x6c6b6a69U)]
    [InlineData(0x1a794705U, 0x64636261U, 0x33323130U, 0x68676665U, 0x37363534U, 0x6c6b6a69U, 0x31303938U)]
    [InlineData(0x4d79177dU, 0x64636261U, 0x33323130U, 0x68676665U, 0x37363534U, 0x6c6b6a69U, 0x31303938U, 0x706f6e6dU)]
    [InlineData(0x59d79205U, 0x64636261U, 0x33323130U, 0x68676665U, 0x37363534U, 0x6c6b6a69U, 0x31303938U, 0x706f6e6dU, 0x35343332U)]
    [InlineData(0x49585aaeU, 0x64636261U, 0x33323130U, 0x68676665U, 0x37363534U, 0x6c6b6a69U, 0x31303938U, 0x706f6e6dU, 0x35343332U, 0x74737271U)]
    [InlineData(0x2f005ff1U, 0x64636261U, 0x33323130U, 0x68676665U, 0x37363534U, 0x6c6b6a69U, 0x31303938U, 0x706f6e6dU, 0x35343332U, 0x74737271U, 0x39383736U)]
    [InlineData(0x0ce339bdU, 0x64636261U, 0x33323130U, 0x68676665U, 0x37363534U, 0x6c6b6a69U, 0x31303938U, 0x706f6e6dU, 0x35343332U, 0x74737271U, 0x39383736U, 0x78777675U)]
    [InlineData(0xb31bd2ffU, 0x64636261U, 0x33323130U, 0x68676665U, 0x37363534U, 0x6c6b6a69U, 0x31303938U, 0x706f6e6dU, 0x35343332U, 0x74737271U, 0x39383736U, 0x78777675U, 0x33323130U)]
    [InlineData(0xa821efa3U, 0x64636261U, 0x33323130U, 0x68676665U, 0x37363534U, 0x6c6b6a69U, 0x31303938U, 0x706f6e6dU, 0x35343332U, 0x74737271U, 0x39383736U, 0x78777675U, 0x33323130U, 0x62617a79U)]
    public static void HashCode_Add(uint expected, params uint[] vector)
    {
        var hc = new HashCode();
        for (var i = 0; i < vector.Length; i++)
            hc.Add(vector[i]);

        Assert.Equal(expected, unchecked((uint)hc.ToHashCode()));
    }

    [Fact]
    public static void HashCode_Add_Generic()
    {
        var hc = new HashCode();
        hc.Add(1);
        hc.Add(100.2);

        Assert.Equal(-273013950, hc.ToHashCode());
    }
#   endif

    [Fact]
    public static void HashCode_Add_GenericEqualityComparer()
    {
        var hc = new HashCode();
        hc.Add(1);
        hc.Add("Hello", new ConstComparer());

        var expected = new HashCode();
        hc.Add(1);
        hc.Add(ConstComparer.ConstantValue);

        Assert.NotEqual(expected.ToHashCode(), hc.ToHashCode());
    }

    [Fact]
    public static void HashCode_Combine()
    {
        var hcs = new[]
        {
            HashCode.Combine(1),
            HashCode.Combine(1, 2),
            HashCode.Combine(1, 2, 3),
            HashCode.Combine(1, 2, 3, 4),
            HashCode.Combine(1, 2, 3, 4, 5),
            HashCode.Combine(1, 2, 3, 4, 5, 6),
            HashCode.Combine(1, 2, 3, 4, 5, 6, 7),
            HashCode.Combine(1, 2, 3, 4, 5, 6, 7, 8),

            HashCode.Combine(2),
            HashCode.Combine(2, 3),
            HashCode.Combine(2, 3, 4),
            HashCode.Combine(2, 3, 4, 5),
            HashCode.Combine(2, 3, 4, 5, 6),
            HashCode.Combine(2, 3, 4, 5, 6, 7),
            HashCode.Combine(2, 3, 4, 5, 6, 7, 8),
            HashCode.Combine(2, 3, 4, 5, 6, 7, 8, 9),
        };

        for (var i = 0; i < hcs.Length; i++)
            for (var j = 0; j < hcs.Length; j++)
            {
                if (i == j) continue;
                Assert.NotEqual(hcs[i], hcs[j]);
            }
    }

    [Fact]
    public static void HashCode_Combine_Add_1()
    {
        var hc = new HashCode();
        hc.Add(1);
        Assert.Equal(hc.ToHashCode(), HashCode.Combine(1));
    }

    [Fact]
    public static void HashCode_Combine_Add_2()
    {
        var hc = new HashCode();
        hc.Add(1);
        hc.Add(2);
        Assert.Equal(hc.ToHashCode(), HashCode.Combine(1, 2));
    }

    [Fact]
    public static void HashCode_Combine_Add_3()
    {
        var hc = new HashCode();
        hc.Add(1);
        hc.Add(2);
        hc.Add(3);
        Assert.Equal(hc.ToHashCode(), HashCode.Combine(1, 2, 3));
    }

    [Fact]
    public static void HashCode_Combine_Add_4()
    {
        var hc = new HashCode();
        hc.Add(1);
        hc.Add(2);
        hc.Add(3);
        hc.Add(4);
        Assert.Equal(hc.ToHashCode(), HashCode.Combine(1, 2, 3, 4));
    }

    [Fact]
    public static void HashCode_Combine_Add_5()
    {
        var hc = new HashCode();
        hc.Add(1);
        hc.Add(2);
        hc.Add(3);
        hc.Add(4);
        hc.Add(5);
        Assert.Equal(hc.ToHashCode(), HashCode.Combine(1, 2, 3, 4, 5));
    }

    [Fact]
    public static void HashCode_Combine_Add_6()
    {
        var hc = new HashCode();
        hc.Add(1);
        hc.Add(2);
        hc.Add(3);
        hc.Add(4);
        hc.Add(5);
        hc.Add(6);
        Assert.Equal(hc.ToHashCode(), HashCode.Combine(1, 2, 3, 4, 5, 6));
    }

    [Fact]
    public static void HashCode_Combine_Add_7()
    {
        var hc = new HashCode();
        hc.Add(1);
        hc.Add(2);
        hc.Add(3);
        hc.Add(4);
        hc.Add(5);
        hc.Add(6);
        hc.Add(7);
        Assert.Equal(hc.ToHashCode(), HashCode.Combine(1, 2, 3, 4, 5, 6, 7));
    }

    [Fact]
    public static void HashCode_Combine_Add_8()
    {
        var hc = new HashCode();
        hc.Add(1);
        hc.Add(2);
        hc.Add(3);
        hc.Add(4);
        hc.Add(5);
        hc.Add(6);
        hc.Add(7);
        hc.Add(8);
        Assert.Equal(hc.ToHashCode(), HashCode.Combine(1, 2, 3, 4, 5, 6, 7, 8));
    }

    public class ConstComparer : System.Collections.Generic.IEqualityComparer<string>
    {
        public const int ConstantValue = 1234;

        public bool Equals(string x, string y) => false;
        public int GetHashCode(string obj) => ConstantValue;
    }
}
