using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Bitdid.Core.Models;

namespace Bitdid.Storage.Core {

    public interface IBitdidStorage {

        #region Properties

        DbSet<Category> Categories { get; set; }

        DbSet<Currency> Currencies { get; set; }

        DbSet<CurrencyMetadata> CurrencyMetadata { get; set; }

        DbSet<CurrencyPrice> CurrencyPrices { get; set; }

        DbSet<Exchange> Exchanges { get; set; }

        DbSet<ExchangeMarketPair> ExchangeMarketPairs { get; set; }

        DbSet<MarketPair> MarketPairs { get; set; }

        DbSet<Tag> Tags { get; set; }

        #endregion


        Task AddAsync<TEntity>(TEntity entity) where TEntity : class;

        void AddRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;

        void BeginTransaction();

        void CommitTransaction();

        void RollbackTransaction();

        void MarkAsChanged<TEntity>(TEntity entity) where TEntity : class;

        void RemoveRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;

        int SaveChanges(bool acceptAllChangesOnSuccess);

        int SaveChanges();

        Task<int> SaveChangesAsync(
            bool acceptAllChangesOnSuccess, 
            CancellationToken cancellationToken = default);

        Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default);

        void SetAsNoTrackingQuery();

        bool EnsureCreated();
    }
}
