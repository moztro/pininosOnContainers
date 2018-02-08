using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using User.Data.Context;

namespace User.Data.Repository
{
    public abstract class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationContext context;
        private DbSet<T> entities;
        string errorMessage = string.Empty;

        public Repository(ApplicationContext context)
        {
            this.context = context;
            entities = context.Set<T>();
        }

        public async Task<int> Count(Expression<Func<T, bool>> where)
        {
            return await entities.Where(where).CountAsync();
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await entities.ToListAsync();
        }

        public async Task<T> Get(long id)
        {
            return entities.Find(id);
        }
        public async Task<int> Insert(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Add(entity);
            return await Save();
        }

        public async Task<int> Update(T entity)
        {
            try
            {
                entities.Attach(entity);
                context.Entry(entity).State = EntityState.Modified;

                return await Save();
            }
            catch
            {
                context.Entry(entity).State = EntityState.Detached;
                throw;
            }
        }

        public async Task<int> Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);
            return await Save();
        }

        public async Task<T> Get(Expression<Func<T, bool>> @where)
        {
            return await entities.Where(where).FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<T>> GetMany(Expression<Func<T, bool>> @where)
        {
            return await entities.Where(where).ToListAsync();
        }

        public async Task<IEnumerable<T>> GetMany(Expression<Func<T, bool>> @where, string order, bool descending, int take)
        {
            if (take <= 0)
            {
                if (descending)
                {
                    return await entities.OrderByDescending(order).Where(where).ToListAsync();
                }
                else
                {
                    return await entities.OrderBy(order).Where(where).ToListAsync();
                }
            }
            else
            {
                if (descending)
                {
                    return await entities.OrderByDescending(order).Where(where).Take(take).ToListAsync();
                }
                else
                {
                    return await entities.OrderBy(order).Where(where).Take(take).ToListAsync();
                }
            }
        }

        public async Task<int> Save()
        {
            return await context.SaveChangesAsync();
        }      

    }

    static class IOrderedQueryable
    {
        #region Order By String Column Name
        private static IOrderedQueryable<T> OrderingHelper<T>(IQueryable<T> source, string propertyName, bool descending, bool anotherLevel)
        {
            ParameterExpression param = Expression.Parameter(typeof(T), string.Empty); // I don't care about some naming
            MemberExpression property;

            if (propertyName.Contains('.'))
            {
                // support to be sorted on child fields. 
                String[] childProperties = propertyName.Split('.');
                var childProperty = typeof(T).GetProperty(childProperties[0]);
                property = Expression.MakeMemberAccess(param, childProperty);

                for (int i = 1; i < childProperties.Length; i++)
                {
                    Type t = childProperty.PropertyType;
                    if (!t.IsGenericType)
                    {
                        childProperty = t.GetProperty(childProperties[i]);
                    }
                    else
                    {
                        childProperty = t.GetGenericArguments().First().GetProperty(childProperties[i]);
                    }

                    property = Expression.MakeMemberAccess(property, childProperty);
                }
            }
            else
            {
                property = Expression.PropertyOrField(param, propertyName);
            }

            LambdaExpression sort = Expression.Lambda(property, param);

            MethodCallExpression call = Expression.Call(
                typeof(Queryable),
                (!anotherLevel ? "OrderBy" : "ThenBy") + (descending ? "Descending" : string.Empty),
                new[] { typeof(T), property.Type },
                source.Expression,
                Expression.Quote(sort));

            return (IOrderedQueryable<T>)source.Provider.CreateQuery<T>(call);
        }

        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName)
        {
            return OrderingHelper(source, propertyName, false, false);
        }

        public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string propertyName)
        {
            return OrderingHelper(source, propertyName, true, false);
        }

        public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, string propertyName)
        {
            return OrderingHelper(source, propertyName, false, true);
        }

        public static IOrderedQueryable<T> ThenByDescending<T>(this IOrderedQueryable<T> source, string propertyName)
        {
            return OrderingHelper(source, propertyName, true, true);
        }
        #endregion
    }
}
