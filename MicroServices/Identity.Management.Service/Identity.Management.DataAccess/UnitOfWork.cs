using System;

namespace Identity.Management.DataAccess
{
    public interface IUnitOfWork :IDisposable{
    }
    public class UnitOfWork : IUnitOfWork
    {
        public void Dispose()
        {
        }
    }
}
