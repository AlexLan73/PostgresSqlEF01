using System;
using DBPostgresql.Core;

namespace EFField
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Console.WriteLine(" ==> Start  <===== ");
            PostgresContext _dbContext = new PostgresContext();
            _dbContext.TestDb();

            int k = 1;


        }
    }
}
