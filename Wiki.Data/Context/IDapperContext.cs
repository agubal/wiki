using System;
using System.Data;

namespace Wiki.Data.Context
{
    public interface IDapperContext : IDisposable
    {
        IDbConnection Connection { get; }
    }
}
