using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

public class TestTT
{
    [Fact]
    public void Test1()
    {
        Assert.Equal(2, 1 + 1);
    }

    [Theory]
    [InlineData(2, 3, 5)]
    [InlineData(5, 5, 10)]
    public void Test2(int a, int b, int expected)
    {
        Assert.Equal(expected, a + b);
    }
}