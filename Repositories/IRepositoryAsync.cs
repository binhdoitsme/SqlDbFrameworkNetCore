using SqlDbFrameworkNetCore.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SqlDbFrameworkNetCore.Repositories.Asynchronous
{
    public interface IRepository
    {
        /// <summary>
        /// Gets all records from the corresponding entity table.
        /// </summary>
        /// <returns>A collection of entities of type T.</returns>
        Task<IEnumerable<T>> All<T>() where T : class;

        /// <summary>
        /// Find record in database using values of [Key] fields.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>The first (also the only) object matches the keys.</returns>
        Task<T> FindByKey<T>(Expression<Func<T, object>> key, object value) where T : class;
        /// <summary>
        /// Find the first record in database matching a set of conditions.
        /// </summary>
        /// <param name="predicate">Representing the conditions to lookup.</param>
        /// <returns>The first record matches <c>predicate</c>.</returns>
        Task<T> FindFirst<T>(Expression<Func<T, bool>> predicate) where T : class;
        /// <summary>
        /// Find all records in database matching a set of conditions.
        /// </summary>
        /// <param name="predicate">Representing the conditions to lookup.</param>
        /// <returns>The collection of eligible objects.</returns>
        Task<IEnumerable<T>> FindAll<T>(Expression<Func<T, bool>> predicate) where T : class;

        /// <summary>
        /// Lookup in the database to see if the item exists in the database.
        /// </summary>
        /// <param name="item">The item to lookup.</param>
        /// <returns>True if item exists in database, false otherwise</returns>
        Task<bool> Contains<T>(T item) where T : class;

        /// <summary>
        /// Return the number of distinct values of the specified column,
        /// or the number of rows if no column was specified.
        /// </summary>
        /// <typeparam name="T">The generic type</typeparam>
        /// <param name="column">The specified column</param>
        /// <returns></returns>
        Task<long> Count<T>(Expression<Func<T, object>> column = null) where T : class;

        // insert methods
        /// <summary>
        /// Add one item to the database.
        /// </summary>
        /// <param name="item">The item to add.</param>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        Task Add<T>(T item) where T : class;
        /// <summary>
        /// Add a collection of items to the database.
        /// </summary>
        /// <param name="items">The items to add.</param>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        Task AddRange<T>(IEnumerable<T> items) where T : class;

        // remove methods
        /// <summary>
        /// Remove one item from the database.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        Task Remove<T>(T item) where T : class;
        /// <summary>
        /// Remove all items matching a set of conditions from the database.
        /// </summary>
        /// <param name="predicate">Representing the set of conditions.</param>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        Task RemoveAll<T>(Expression<Func<T, bool>> predicate) where T : class;
        /// <summary>
        /// Remove a collection of items from the database.
        /// </summary>
        /// <param name="items">The item to remove.</param>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        Task RemoveRange<T>(IEnumerable<T> items) where T : class;

        // update methods
        /// <summary>
        /// Update the object matching the value of <c>key</c> to be <c>newValue</c> in the database.
        /// </summary>
        /// <param name="predicate">Conditions of objects to be updated.</param>
        /// <param name="newValue">New value for the object(s).</param>
        Task Set<T>(Expression<Func<T, bool>> predicate, object newValue) where T : class;
        /// <summary>
        /// Update the <c>oldValue</c> to be <c>newValue</c> in the database.
        /// </summary>
        /// <param name="oldValue">The object to be updated.</param>
        /// <param name="newValue">New values for the object.</param>
        Task Set<T>(T oldValue, T newValue) where T : class;
    }

    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Gets all records from the corresponding entity table.
        /// </summary>
        /// <returns>A collection of entities of type T.</returns>
        Task<IEnumerable<T>> All();

        /// <summary>
        /// Find record in database using values of [Key] fields.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>The first (also the only) object matches the keys.</returns>
        Task<T> FindByKey(Expression<Func<T, object>> key, object value);
        /// <summary>
        /// Find the first record in database matching a set of conditions.
        /// </summary>
        /// <param name="predicate">Representing the conditions to lookup.</param>
        /// <returns>The first record matches <c>predicate</c>.</returns>
        Task<T> FindFirst(Expression<Func<T, bool>> predicate);
        /// <summary>
        /// Find all records in database matching a set of conditions.
        /// </summary>
        /// <param name="predicate">Representing the conditions to lookup.</param>
        /// <returns>The collection of eligible objects.</returns>
        Task<IEnumerable<T>> FindAll(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Lookup in the database to see if the item exists in the database.
        /// </summary>
        /// <param name="item">The item to lookup.</param>
        /// <returns>True if item exists in database, false otherwise</returns>
        Task<bool> Contains(T item);

        /// <summary>
        /// Return the number of distinct values of the specified column,
        /// or the number of rows if no column was specified.
        /// </summary>
        /// <typeparam name="T">The generic type</typeparam>
        /// <param name="column">The specified column</param>
        /// <returns></returns>
        Task<long> Count(Expression<Func<T, object>> column = null);

        // insert methods
        /// <summary>
        /// Add one item to the database.
        /// </summary>
        /// <param name="item">The item to add.</param>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        Task Add(T item);
        /// <summary>
        /// Add a collection of items to the database.
        /// </summary>
        /// <param name="items">The items to add.</param>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        Task AddRange(IEnumerable<T> items);

        // remove methods
        /// <summary>
        /// Remove one item from the database.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        Task Remove(T item);
        /// <summary>
        /// Remove all items matching a set of conditions from the database.
        /// </summary>
        /// <param name="predicate">Representing the set of conditions.</param>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        Task RemoveAll(Expression<Func<T, bool>> predicate);
        /// <summary>
        /// Remove a collection of items from the database.
        /// </summary>
        /// <param name="items">The item to remove.</param>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        Task RemoveRange(IEnumerable<T> items);

        // update methods
        /// <summary>
        /// Update the object matching the value of <c>key</c> to be <c>newValue</c> in the database.
        /// </summary>
        /// <param name="predicate">Conditions of objects to be updated.</param>
        /// <param name="newValue">New value for the object(s).</param>
        Task Set(Expression<Func<T, bool>> predicate, object newValue);
        /// <summary>
        /// Update the <c>oldValue</c> to be <c>newValue</c> in the database.
        /// </summary>
        /// <param name="oldValue">The object to be updated.</param>
        /// <param name="newValue">New values for the object.</param>
        Task Set(T oldValue, T newValue);
    }
}
