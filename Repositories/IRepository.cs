using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SqlDbFrameworkNetCore.Repositories
{
    /// <summary>
    /// Non-generic version of Repository
    /// </summary>
    public partial interface IRepository
    {
        /// <summary>
        /// Gets all records from the corresponding entity table.
        /// </summary>
        /// <returns>A collection of entities of type T.</returns>
        IEnumerable<T> All<T>() where T : class;

        /// <summary>
        /// Find record in database using values of [Key] fields.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>The first (also the only) object matches the keys.</returns>
        T FindByKey<T>(Expression<Func<T, object>> key, object value) where T : class;
        /// <summary>
        /// Find the first record in database matching a set of conditions.
        /// </summary>
        /// <param name="predicate">Representing the conditions to lookup.</param>
        /// <returns>The first record matches <c>predicate</c>.</returns>
        T FindFirst<T>(Expression<Func<T, bool>> predicate) where T : class;
        /// <summary>
        /// Find all records in database matching a set of conditions.
        /// </summary>
        /// <param name="predicate">Representing the conditions to lookup.</param>
        /// <returns>The collection of eligible objects.</returns>
        IEnumerable<T> FindAll<T>(Expression<Func<T, bool>> predicate) where T : class;
        /// <summary>
        /// Retrieve the value of the target <c>item</c>
        /// </summary>
        /// <typeparam name="T">The item type</typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        T Retrieve<T>(T item) where T : class;
        /// <summary>
        /// Retrieve the values of the target <c>items</c>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        IEnumerable<T> RetrieveRange<T>(IEnumerable<T> items) where T : class;

        /// <summary>
        /// Lookup in the database to see if the item exists in the database.
        /// </summary>
        /// <param name="item">The item to lookup.</param>
        /// <returns>True if item exists in database, false otherwise</returns>
        bool Contains<T>(T item) where T : class;

        /// <summary>
        /// Return the number of distinct values of the specified column,
        /// or the number of rows if no column was specified.
        /// </summary>
        /// <typeparam name="T">The generic type</typeparam>
        /// <param name="column">The specified column</param>
        /// <returns></returns>
        long Count<T>(Expression<Func<T, object>> column = null) where T : class;

        // insert methods
        /// <summary>
        /// Add one item to the database.
        /// </summary>
        /// <param name="item">The item to add.</param>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        void Add<T>(T item) where T : class;
        /// <summary>
        /// Add a collection of items to the database.
        /// </summary>
        /// <param name="items">The items to add.</param>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        void AddRange<T>(IEnumerable<T> items) where T : class;

        // remove methods
        /// <summary>
        /// Remove one item from the database.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        void Remove<T>(T item) where T : class;
        /// <summary>
        /// Remove all items matching a set of conditions from the database.
        /// </summary>
        /// <param name="predicate">Representing the set of conditions.</param>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        void RemoveAll<T>(Expression<Func<T, bool>> predicate) where T : class;
        /// <summary>
        /// Remove a collection of items from the database.
        /// </summary>
        /// <param name="items">The item to remove.</param>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        void RemoveRange<T>(IEnumerable<T> items) where T : class;

        // update methods
        /// <summary>
        /// Update the object matching the value of <c>key</c> to be <c>newValue</c> in the database.
        /// </summary>
        /// <param name="predicate">Conditions of objects to be updated.</param>
        /// <param name="newValue">New value for the object(s).</param>
        void Set<T>(Expression<Func<T, bool>> predicate, object newValue) where T : class;
        /// <summary>
        /// Update the <c>oldValue</c> to be <c>newValue</c> in the database.
        /// </summary>
        /// <param name="oldValue">The object to be updated.</param>
        /// <param name="newValue">New values for the object.</param>
        void Set<T>(T oldValue, T newValue) where T : class;
    }

    /// <summary>
    /// The interface defining basic methods for easy database querying without knowing SQL.
    /// </summary>
    /// <typeparam name="T">The target entity type</typeparam>
    public partial interface IRepository<T> where T : class
    {
        // lookup methods
        /// <summary>
        /// Gets all records from the corresponding entity table.
        /// </summary>
        /// <returns>A collection of entities of type T.</returns>
        IEnumerable<T> All();

        /// <summary>
        /// Find record in database using values of [Key] fields.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>The first (also the only) object matches the keys.</returns>
        T FindByKey(Expression<Func<T, object>> key, object value);
        /// <summary>
        /// Find the first record in database matching a set of conditions.
        /// </summary>
        /// <param name="predicate">Representing the conditions to lookup.</param>
        /// <returns>The first record matches <c>predicate</c>.</returns>
        T FindFirst(Expression<Func<T, bool>> predicate);
        /// <summary>
        /// Find all records in database matching a set of conditions.
        /// </summary>
        /// <param name="predicate">Representing the conditions to lookup.</param>
        /// <returns>The collection of eligible objects.</returns>
        IEnumerable<T> FindAll(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Lookup in the database to see if the item exists in the database.
        /// </summary>
        /// <param name="item">The item to lookup.</param>
        /// <returns>True if item exists in database, false otherwise</returns>
        bool Contains(T item);

        /// <summary>
        /// Return the number of distinct values of the specified column,
        /// or the number of rows if no column was specified.
        /// </summary>
        /// <typeparam name="T">The generic type</typeparam>
        /// <param name="column">The specified column</param>
        /// <returns></returns>
        long Count(Expression<Func<T, object>> column = null);

        // insert methods
        /// <summary>
        /// Add one item to the database.
        /// </summary>
        /// <param name="item">The item to add.</param>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        void Add(T item);
        /// <summary>
        /// Add a collection of items to the database.
        /// </summary>
        /// <param name="items">The items to add.</param>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        void AddRange(IEnumerable<T> items);

        // remove methods
        /// <summary>
        /// Remove one item from the database.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        void Remove(T item);
        /// <summary>
        /// Remove all items matching a set of conditions from the database.
        /// </summary>
        /// <param name="predicate">Representing the set of conditions.</param>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        void RemoveAll(Expression<Func<T, bool>> predicate);
        /// <summary>
        /// Remove a collection of items from the database.
        /// </summary>
        /// <param name="items">The item to remove.</param>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        void RemoveRange(IEnumerable<T> items);

        // update methods
        /// <summary>
        /// Update the object matching the value of <c>key</c> to be <c>newValue</c> in the database.
        /// </summary>
        /// <param name="predicate">Conditions of objects to be updated.</param>
        /// <param name="newValue">New value for the object(s).</param>
        void Set(Expression<Func<T, bool>> predicate, object newValue);
        /// <summary>
        /// Update the <c>oldValue</c> to be <c>newValue</c> in the database.
        /// </summary>
        /// <param name="oldValue">The object to be updated.</param>
        /// <param name="newValue">New values for the object.</param>
        void Set(T oldValue, T newValue);
    }
}
