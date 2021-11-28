using Incidences.Data;
using System;
using Xunit;

namespace TestProject
{
    public class UnitTest1
    {
        private readonly ISqlBase sqlBase;
        public UnitTest1(ISqlBase sqlBase)
        {
            this.sqlBase = sqlBase;
        }

        [Fact]
        public void Test1()
        {
            Console.WriteLine(sqlBase.keyValue("barco", "<>", "coche"));
        }
    }
}
