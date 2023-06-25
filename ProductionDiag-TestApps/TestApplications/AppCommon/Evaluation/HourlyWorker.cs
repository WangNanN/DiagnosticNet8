using System;
using System.Collections.Generic;
using System.Text;

namespace AppCommon.Evaluation
{
    public class HourlyWorker : Worker
    {
        public HourlyWorker(int annualSalary)
            : base(annualSalary)
        {
            salary = annualSalary;//Verify constructor of a base class
        }
    }
}
