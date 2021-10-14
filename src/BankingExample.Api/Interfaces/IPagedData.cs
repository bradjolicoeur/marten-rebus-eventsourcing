﻿using System.Collections.Generic;

namespace BankingExample.Api.Interfaces
{
    public interface IPagedData<T>
    {
        long Count { get; }
        IEnumerable<T> Data { get; }
        int Page { get; }
    }
}
