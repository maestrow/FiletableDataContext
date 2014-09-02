# FiletableDataContext

## Requirements

- Sql Server Express With Advances Features. Внимание: SQL Server редакции Compact Edition не поддерживает Filetables. 


## Run Demo

- Поправьте строку соединения в App.config



## Usage


### Шаг 1: Создание пустой БД

Укажите строку соединения с БД:
```
  <connectionStrings>
    <add name="MyDb" connectionString="data source=.\SQLEXPRESS;Initial Catalog=Test2;Integrated Security=SSPI;" providerName="System.Data.SqlClient" />
  </connectionStrings>
```

Создайте контекст, использующий вашу строку соединения с БД:
```
    public class MyDbContext : DbContext //FiletableDbContext<Article>
    {
        public MyDbContext(): base("name=MyDb")
        {
        }
    }
```

С помощью команд миграции создайте пустую БД:

- `PM> Enable-Migrations`
- `PM> Update-Database`


### Шаг 2: Инициализация БД

1. Выполнить запрос, подставив вместо {0} имя вашей БД:
```
ALTER DATABASE {0} SET 
    FILESTREAM ( NON_TRANSACTED_ACCESS = FULL, DIRECTORY_NAME = '{0}' ),
    READ_COMMITTED_SNAPSHOT OFF
```
> На данный момент существует проблема - выполнение этого запроса из класса миграции завершается по таймауту, 
> но при запуске через SSMS корректно выполняется. 
> Поэтому вызов в InitDb закомментирован и нужно выполнить его вручную через SSMS.


2. `PM> Add-Migration "InitFilestream" -IgnoreChanges`

После чего в проекте будет создан пустой класс миграции, который необходимо модифицировать следующим образом:

- унаследовать его от InitDb
- реализовать свойство `ConnectionStringName`, указав имя вашуй БД
- реализовать свойство `RootPath`, указав путь до папки с базами данных SQL Server


```
public partial class InitFilestream : InitDb
{
    protected override string ConnectionStringName
    {
        get { return "MyDb"; }
    }
}
```

3. `PM> Update-Database`



### Шаг 3: Создание сущности Filetable

1. Создайте класс-наследник от FiletableEntityBase
```
public class Article : FiletableEntityBase {}
```

2. Создайте отдельный контекст для этой сущности
```
    public class ArticleContext : FiletableDbContext<Article>
    {
        public MyDbContext(): base("name=MyDb")
        {
        }
    }
```

3. `PM> Add-Migration "Article" -IgnoreChanges`
После чего в проекте будет создан пустой класс миграции, который необходимо модифицировать следующим образом:

- унаследовать от CreateFiletable
- реализовать свойства `ConnectionStringName` и `TableName`

4. `PM> Updata-Database`


## Make Nuget Package and Install Package from Local Source

Make package:

	c:\> cd FiletableDataContext
	c:\> nuget pack -OutputDirectory d:/path/to/local/repo

Install:

	PM> Install-Package FiletableDataContext -Source d:/path/to/local/repo

## Notes

### Code First Stored Procedures

В проекте использована библиотека [CodeFirstStoredProcs](https://www.nuget.org/packages/CodeFirstStoredProcs/) (см. [статью](http://www.codeproject.com/Articles/179481/Code-First-Stored-Procedures)).
Для данной библиотеки понадобился небольшой [патч](http://stackoverflow.com/questions/358835/getproperties-to-return-all-properties-for-an-interface-inheritance-hierarchy), поэтому я включил ее код в проект и добавил функцию `public static PropertyInfo[] GetPublicProperties(this Type type)`.
