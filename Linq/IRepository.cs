using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace SqlDbFrameworkNetCore.Linq
{
    public interface IRepository<TEntity> where TEntity : class
    {
        // DB Query methods
        /// <summary>
        /// Get all the records of type TEntity from the database
        /// </summary>
        /// <returns>All records of type IEntity in database</returns>
        IEnumerable<TEntity> All();
        /// <summary>
        /// Gets the first record in database matching the predicate
        /// </summary>
        /// <param name="predicate">The search predicate</param>
        /// <returns></returns>
        TEntity FindOne(Expression<Func<TEntity, bool>> predicate);
        /// <summary>
        /// Returns all records from database matching the predicate
        /// </summary>
        /// <param name="predicate">The search predicate</param>
        /// <returns>The collection of records matching the predicate</returns>
        IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate);

        // joined query methods

        // insert methods
        /// <summary>
        /// Insert a new item into the database
        /// </summary>
        /// <param name="item">The item to add. Cannot be null</param>
        void InsertOne(TEntity item);
        /// <summary>
        /// Insert several new items into the database
        /// </summary>
        /// <param name="item">The items to add. Cannot be null</param>
        void InsertMany(TEntity[] items);

        // delete methods
        void Delete(TEntity item);
        void DeleteOne(Expression<Func<TEntity, bool>> predicate);
        void DeleteAll(Expression<Func<TEntity, bool>> predicate);
        void DeleteAll();

        // update methods
        void Update(TEntity oldObj, TEntity newObj);
        void UpdateBulk(dynamic updates, Expression<Func<TEntity, bool>> predicate);
    }
}
