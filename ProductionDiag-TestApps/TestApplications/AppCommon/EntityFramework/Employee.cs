using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace AppCommon.EntityFramework
{
    public class Employee
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }


        public static IEnumerable<Employee> GenerateNew(int count, int seed)
        {
            Random random = new Random(seed);

            var allTypes = typeof(object).Assembly.GetTypes();

            for (int i = 1; i <= count; i++)
            {
                yield return new Employee { Id = i, FirstName = allTypes[random.Next(allTypes.Length)].Name, LastName = allTypes[random.Next(allTypes.Length)].Name };
            }
        }
    }
}
