using System;
using System.Collections.Generic;
using System.Text;

namespace AppCommon.CallStack
{
    interface IEquatable<T>
    {
        bool Equals(T obj);
    }
}
