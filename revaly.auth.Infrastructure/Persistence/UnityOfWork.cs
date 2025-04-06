﻿
using Microsoft.EntityFrameworkCore.Storage;
using revaly.auth.Infrastructure.Context;

namespace revaly.auth.Infrastructure.Persistence
{
    public class UnityOfWork(
        MySQLContext mySQLContext    
    )
    {
        private readonly MySQLContext _mySQLContext = mySQLContext;
        private IDbContextTransaction _transaction;

        public async Task<int> CompleteAsync()
        {
            return await _mySQLContext.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            if (_transaction != null)
                return;

            _transaction = await _mySQLContext.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            if (_transaction == null)
                throw new InvalidOperationException("Transaction has not been started.");

            try
            {
                await _mySQLContext.SaveChangesAsync();
                await _transaction.CommitAsync();
            }
            catch (Exception)
            {
                await _transaction.RollbackAsync();
                throw;
            }
            finally
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _mySQLContext.Dispose();
                _transaction?.Dispose();
            }
        }
    }
}
