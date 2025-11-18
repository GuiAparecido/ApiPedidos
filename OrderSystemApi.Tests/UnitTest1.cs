using Xunit;
using OrderSystemApi.Services;
using OrderSystemApi.Models;

namespace OrderSystemApi.Tests;

public class UnitTest1
{
    [Fact]
    public void Teste_Soma_Simples()
    {
        int a = 2;
        int b = 3;

        Assert.Equal(5, a + b);
    }
}
