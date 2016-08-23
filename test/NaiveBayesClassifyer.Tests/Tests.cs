using System;
using Xunit;
using MachineLearning;

namespace Tests
{
    public class Tests
    {
        [Fact]
        public void Test1() 
        {
            Assert.True(true);
        }

        [Fact]
        public void TestGenerateSample(){
            
            var gen =  GenerateSample.MakeData(40);

            Assert.Equal(40,gen.Length);
        }
    }
}
