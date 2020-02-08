# SqlDbFrameworkNetCore
A sql query framework based on Dapper and Newtonsoft Json libraries.

# Usage
### General Usage
When creating a complex model object with associated fields, extend both itself and the associated entity from BaseEntity.
Then, decorate the field with [SqlDbFrameworkNetCore.Linq.ExplicitLoadingAttribute]:
```
	public class ExampleEntity : BaseEntity 
	{
		// code removed
		[Key]
		public int Id { get; set; }
		
		[ExplicitLoading] [OneToMany]
		public IEnumerable<AssociatedEntity> AssociatedField { get; set; }
	}
```
Remember to specify the referenced foreign key using [System.ComponentModel.DataAnnotations.Schema.ForeignKeyAttribute]'s constructor:
```
	public class AssociatedEntity
	{
		// some code removed
		
		[ForeignKey("ExampleEntity.Id")]
		public int ExampleEntityId { get; set; }
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

### Attribute/Annotation
There are two mapping attributes: 
- [OneToMany]: Specified in referenced fields where the current type has a has-many relationship with.
- [ManyToMany(immediateType)]: Specified in referenced fields where the current type has a many-to-many relationship with. Needs to specify the ```immediateType```.

### Using the Repository class:
```
	DbConnection connection; 
	[...]
	IRepository repo = new Repository(connection); // non-generic
	IRepository<ExampleEntity> genericRepo = new Repository<ExampleEntity>(); // generic
```

# Note
If there are business operations concerning data that can be encapsulated, write a separate class extends Repository<T> then supply the business operations.
Also, foreign key must always be ```int```, other types are not supported and NOT RECOMMENDED. The foreign key specification convention is:
```
	[ForeignKey("{EntityTypeName}.{ReferencedFieldName}")]
	public int [EntityTypeName][ReferencedFieldName] { get; set; }
```
