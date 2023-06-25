using System;
using System.Collections.Generic;
using System.Text;

namespace AppCommon.Evaluation
{
    public class Worker
    {
        public int salary;

        public Worker(int annualSalary)
        {
            salary = annualSalary; //Verify constructor for a parameter
        }

        public Worker(int weeklySalary, int numberOfWeeks)
        {
            salary = weeklySalary * numberOfWeeks;//Verify constructor for two parameters
        }
    }
}
