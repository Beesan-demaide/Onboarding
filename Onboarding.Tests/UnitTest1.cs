using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace Onboarding.Tests;

public class UnitTest1
{
    [Fact]
    public void Test1_Add()
    {
        Assert.Equal(4, Program.Add(2,2));
        Assert.Equal(5, Program.Subtract(10, 5));
    }
    [Fact]
    public void Test2_Subtract()
    {
        Assert.Equal(5, Program.Subtract(10, 5));

    }
}
