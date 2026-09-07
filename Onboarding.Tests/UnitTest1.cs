using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace Onboarding.Tests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        Assert.Equal(10, Program.Add(2,2));
    }
}
