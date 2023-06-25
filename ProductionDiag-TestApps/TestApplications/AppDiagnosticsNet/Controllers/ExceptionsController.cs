using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppDiagnosticsNet6.Controllers
{
    public class MyException : Exception
    {
        public MyException(string message)
            : base(message, new MyInnerException()) 
        {
        }
    }

    public class MyInnerException : Exception 
    {
        public MyInnerException()
            : base("I am the inner exception.")
        {
        }
    }

    public class ExceptionsController : Controller
    {
        public void SafeExecute(Action action)
        {
            try
            {
                action();
            }
            catch
            {
            }
        }

        public async Task SafeExecute(Func<Task> func)
        {
            try
            {
                await func();
            }
            catch
            {

            }
        }

        public async Task<ActionResult> Exceptions()
        {
            SafeExecute(() => ThrowCaughtException()); // aka rethrown
            SafeExecute(() => ThrowInnerException());
            SafeExecute(() => ThrowNestedException());
            await SafeExecute(() => ThrowAggregateException());

            return View();
        }

        public void ThrowCaughtException()
        {
            try
            {
                throw new Exception("Rethrown");
            }
            catch
            {
                throw;
            }
        }

        public void ThrowInnerException()
        {
            throw new MyException("I have an inner exception.");
        }

        public void ThrowNestedException()
        {
            try
            {
                throw new Exception("First Exception");
            }
            catch
            {
                throw new Exception("Nested Exception");
            }
        }

        public Task ThrowAggregateException()
        {
            List<Task> tasks = new List<Task>();
            for (int i = 0; i < 5; i++)
            {
                int j = i; // needed to capture closure. Otherwise values get duplicated.
                tasks.Add(
                    Task.Run(() => {
                        throw new Exception($"Exception #{j}");
                    }));
            }
            // Task.WhenAll unwarps the aggregated exception and will return only the first exception.
            Task.WaitAll(tasks.ToArray());

            // This will never happen since Task.WaitAll will throw
            return Task.CompletedTask;
        }
    }
}
