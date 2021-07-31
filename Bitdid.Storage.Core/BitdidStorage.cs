using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Bitdid.Core.Models;

namespace Bitdid.Storage.Core {

    public class BitdidStorage : DbContext, IBitdidStorage {

        public BitdidStorage(DbContextOptions options): base(options) { }

        #region fields

        private IDbContextTransaction _transaction;

        #endregion

        #region Properties

        public DbSet<Category> Categories { get; set; }

        public DbSet<Currency> Currencies { get; set; }

        public DbSet<CurrencyMetadata> CurrencyMetadata { get; set; }

        public DbSet<CurrencyPrice> CurrencyPrices { get; set; }

        public DbSet<Exchange> Exchanges { get; set; }

        public DbSet<ExchangeMarketPair> ExchangeMarketPairs { get; set; }

        public DbSet<MarketPair> MarketPairs { get; set; }

        public DbSet<Tag> Tags { get; set; }

        #endregion

        #region Interface Methods

        public async Task AddAsync<TEntity>(TEntity entity) where TEntity : class
            => await Set<TEntity>().AddAsync(entity);

        public void AddRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class 
            => Set<TEntity>().AddRange(entities);
        
        public void BeginTransaction() 
            => _transaction = Database.BeginTransaction();
        
        public void CommitTransaction() {
            if (_transaction == null) {
                throw new NullReferenceException("Please call `BeginTransaction()` method first.");
            }
            _transaction.Commit();
        }

        public void RollbackTransaction() {
            if (_transaction == null) {
                throw new NullReferenceException("Please call `BeginTransaction()` method first.");
            }
            _transaction.Rollback();
        }

        public override void Dispose() {
            _transaction?.Dispose();
            base.Dispose();
        }

       
        public void MarkAsChanged<TEntity>(TEntity entity) where TEntity : class
            => Update(entity);

        public void RemoveRange<TEntity>(
            IEnumerable<TEntity> entities
        ) where TEntity : class => Set<TEntity>().RemoveRange(entities);

        public override int SaveChanges(bool acceptAllChangesOnSuccess) {
            ChangeTracker.DetectChanges();
            ChangeTracker.AutoDetectChangesEnabled = false;
            var result = base.SaveChanges(acceptAllChangesOnSuccess);
            ChangeTracker.AutoDetectChangesEnabled = true;
            return result;
        }


        public override int SaveChanges() {
            ChangeTracker.DetectChanges();
            ChangeTracker.AutoDetectChangesEnabled = false;
            var result = base.SaveChanges();
            ChangeTracker.AutoDetectChangesEnabled = true;
            return result;
        }

        public override async Task<int> SaveChangesAsync(
            bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default) {
            ChangeTracker.DetectChanges();
            ChangeTracker.AutoDetectChangesEnabled = false;
            var result = await base.SaveChangesAsync(
                acceptAllChangesOnSuccess,
                cancellationToken);
            ChangeTracker.AutoDetectChangesEnabled = true;
            return result;
        }

        public override async Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default) {
            ChangeTracker.DetectChanges();
            ChangeTracker.AutoDetectChangesEnabled = false;
            var result = await base.SaveChangesAsync(cancellationToken);
            ChangeTracker.AutoDetectChangesEnabled = true;
            return result;
        }

        public void SetAsNoTrackingQuery()
            => ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

        public bool EnsureCreated()
            => base.Database.EnsureCreated();

        //public void MigrateDb()
        //    => base.Database.Migrate();

        //public async Task MigrateDbAsync(CancellationToken cancellationToken = default)
        //    => await base.Database.MigrateAsync(cancellationToken);

        #endregion

        protected override void OnModelCreating(ModelBuilder builder) {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new CategoryConfiguration());
            builder.ApplyConfiguration(new CurrencyConfiguration());
            builder.ApplyConfiguration(new CurrencyMetadataConfiguration());
            builder.ApplyConfiguration(new CurrencyPriceConfiguration());
            builder.ApplyConfiguration(new ExchangeConfiguration());
            builder.ApplyConfiguration(new ExchangeMarketPairConfiguration());
            builder.ApplyConfiguration(new MarketPairConfiguration());
            builder.ApplyConfiguration(new TagConfiguration());
            builder.ApplyConfiguration(new CurrencyTagConfiguration());
        }
    }
}
