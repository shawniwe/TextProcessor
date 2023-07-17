using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextProcessor.Abstract;
using TextProcessor.Abstract.Interfaces;

namespace TextProcessor.DataAccessLayer.Repositories
{
    public class DefaultRepository<T> : IRepository<T> where T: BaseEntity
    {
        protected readonly ApplicationDbContext context;

        public DefaultRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public void BeginTransaction()
        {
            context.Database.BeginTransaction();
        }

        public void BeginTransaction(IsolationLevel isolationLevel)
        {
            context.Database.BeginTransaction(isolationLevel);
        }

        public void CommitTransaction()
        {
            context.Database.CommitTransaction();
        }
        public T Read(long id)
        {
            var property = context.GetType().GetProperties().Where(x => x.PropertyType == typeof(DbSet<T>)).First();
            var list = (DbSet<T>)property.GetValue(context, null);
            var entry = list.Where(x => x.Id == id).FirstOrDefault();
            context.Entry(entry).Reload();
            return entry;
        }

        public IEnumerable<T> ReadAll()
        {
            var property = context.GetType().GetProperties().Where(x => x.PropertyType == typeof(DbSet<T>)).First();
            var list = (DbSet<T>)property.GetValue(context, null);
            list.ToList().ForEach(x => context.Entry(x).Reload());
            return list;
        }

        public void Create(T entity)
        {
            var property = context.GetType().GetProperties().Where(x => x.PropertyType == typeof(DbSet<T>)).First();
            var list = (DbSet<T>)property.GetValue(context, null);
            entity.Guid = Guid.NewGuid();
            list.Add(entity);
            context.SaveChanges();
            context.Entry(entity).Reload();
        }

        public void Update(T entity)
        {
            var property = context.GetType().GetProperties().Where(x => x.PropertyType == typeof(DbSet<T>)).First();
            var list = (DbSet<T>)property.GetValue(context, null);
            var entry = list.Where(x => x == entity).FirstOrDefault();
            foreach (var entryProperty in entry.GetType().GetProperties())
            {
                var entityProperty = entity.GetType().GetProperties().FirstOrDefault(x => x.Equals(entryProperty));
                entryProperty.SetValue(entry, entityProperty.GetValue(entity));
            }
            context.SaveChanges();
            context.Entry(entry).Reload();
        }

        public void DeleteAll()
        {
            var property = context.GetType().GetProperties().Where(x => x.PropertyType == typeof(DbSet<T>)).First();
            var list = (DbSet<T>)property.GetValue(context, null);
            list.RemoveRange(list);
            context.SaveChanges();
        }

        public void Delete(T entity)
        {
            var property = context.GetType().GetProperties().Where(x => x.PropertyType == typeof(DbSet<T>)).First();
            var list = (DbSet<T>)property.GetValue(context, null);
            list.Remove(entity);
            context.SaveChanges();
        }
    }
}
