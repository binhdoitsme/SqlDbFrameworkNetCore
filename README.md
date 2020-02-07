# SqlDbFrameworkNetCore
A sql query framework based on Dapper and Newtonsoft Json libraries.

# Usage
When creating a complex model object with associated fields, extend both itself and the associated entity from BaseEntity.
Then, decorate the field with [SqlDbFrameworkNetCore.Linq.ExplicitLoadingAttribute]:
```
	public class ExampleEntity : BaseEntity 
	{
		// code removed
		[ExplicitLoading]
		public IEnumerable<AssociatedEntity> AssociatedField { get; set; }
	}
```
Then when using the field from outside, use the Load(...) operation followed by setting the QueryBuilder to load the associated field:
```
	public static void Main(String[] args)
	{
		// code removed
		SqlDbFrameworkNetCore.Linq.IQueryBuilder queryBuilder;
		// code removed
		ExampleEntity entity = new ExampleEntity() { ... };
		// load the associated field
		entity.Load(() => entity.AssociatedField);

		Console.WriteLine(entity.AssociatedField); // returns not null value now
	}
```
If the model has no has-a or has-many relationship, the base class is not necessary.

Using the Repository class:
```
	DbConnection connection; 
	[...]
	IRepository repo = new Repository(connection); // non-generic
	IRepository<ExampleEntity> genericRepo = new Repository<ExampleEntity>(); // generic
```

# Note
If there are business operations concerning data that can be encapsulated, write a separate class extends Repository<T> then supply the business operations.
