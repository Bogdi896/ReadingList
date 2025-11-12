using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingList.Domain.Common
{
    public readonly record struct Result(bool Ok, string? Error)
    {
        public static Result Success() => new(true, null);
        public static Result Fail(string error) => new(false, error);
    }

    public readonly record struct Result<T>(bool Ok, T? Value, string? Error)
    {
        public static Result<T> Success(T value) => new(true, value, null);
        public static Result<T> Fail(string error) => new(false, default, error);
    }
}
