using System;
using System.Diagnostics;
using System.Web.Mvc;
using AppCommon.CallStack;
using AppCommon.Evaluation;

namespace AppDesktop.Controllers
{
    public class TestCallStackController : Controller
    {
        public delegate double Function(double a, int b);

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            Main_orig();

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public static int Main_orig()
        {
            double doubleVal = 2.5;
            Function f = (double a, int b) =>
            { // first line in anonymous delegate method
                double result = 0; // Verify callstack in anonymous delegate method
                result = a * b;
                return result;
            };

            TestDelegate test = new TestDelegate(f);
            double delegateResult = doubleVal;
            for (int n = 0; n < 3; n++)
            {
                delegateResult = test.CallsDelegate(delegateResult, n);
            }
            Employee e1 = new Employee(30000);
            Employee e2 = new Employee(500, 52);
            Manager e3 = new Manager(10000);

            Car car = new Car();
            car.Equals(car);

            FuncTest(100);

            return 0;
        }


        public class TestDelegate
        {
            private Function _myDelegate;
            public TestDelegate(Function func)
            {
                _myDelegate = func; // first line in TestClass ctor
            }

            public double CallsDelegate(double a, int b)
            {
                return _myDelegate(a, b); // Begin stepinto delegate scenario
            }
        }

        public class Employee
        {
            public int salary;

            public Employee(int annualSalary)
            {
                salary = annualSalary; //Verify constructor for a parameter
            }

            public Employee(int weeklySalary, int numberOfWeeks)
            {
                salary = weeklySalary * numberOfWeeks;//Verify constructor for two parameters
            }
        }

        public class Manager : Employee
        {
            public Manager(int annualSalary)
                : base(annualSalary)
            {
                salary = annualSalary;//Verify constructor of a base class
            }
        }

        interface IEquatable<T>
        {
            bool Equals(T obj);
        }

        public class Car : IEquatable<Car>
        {
            public string Make { get; set; }
            public string Model { get; set; }
            public string Year { get; set; }

            // Implementation of IEquatable<T> interface
            public bool Equals(Car car)
            {
                if (this.Make == car.Make &&
                    this.Model == car.Model &&
                    this.Year == car.Year)
                {
                    return true; //Verify CallStackInInterfaceMethod
                }
                else
                    return false;
            }
        }

        static int FuncTest(int i)
        {
            int j = 0; //set a condition breakpoint here: i==5
            if (i == 0)
            {
                return 0;
            }
            else
            {
                return FuncTest(i - 1);
            }
        }
    }
}
