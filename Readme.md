> # Nota importante
> Al iniciar este proyecto cree una clase tarea en su nombre en ingles Task, pero esta es una mala practica porque Task es una palabra reservada utilizada para la programacion asincrona.
por lo que no hagan lo mismo que yo.


># Segunda nota importante.
>por un error humano la primera parte no se subio correctamemte, por lo que lo volvi a hacer a mano sin prueba. por lo que se encuentre simples error en el cogido. En cuanto a lo logica y teoria no deberian de tener problemas.


Para comenzar iniciemos un nuevo proyecto con la siguiente linea de codigo en su terminal.

    dotnet new web

Luego vamos a instalar las dependencias de nuget que estaremos usando para el proyecto

    dotnet add package Microsoft.EntityFrameworkCore --version 7.0.2
    dotnet add package Microsoft.EntityFrameworkCore.InMemory --version 7.0.2
    dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 7.0.2
    dotnet add package Microsoft.EntityFrameworkCore.Design --version 7.0.2

Con todo listo podemos crear los modelos que se van a converir en tablas en nuestras base de datos.

**Categoty.cs**

    using System.Text.Json.Serialization;

    namespace EF.Models
    {

        public class Categoty
        {
            public Guid Id { get; set; }

            public string Name { get; set; }

            public string Description { get; set; }

            [JsonIgnore]
            public virtual ICollection<Task> Tasks { get; set; }
        }
    }

**Task.cs**

    using System.ComponentModel.DataAnnotations;

    namespace EF.Models
    {

        public class Task
        {
            [Key]
            public Guid Id { get; set; }
            public Guid IdCategory { get; set; }

            public string Title { get; set; }

            public string Description { get; set; }
            public Priority PriorityTask { get; set; }
            public DateTime Date { get; set; }

            public virtual Categoty Categoty { get; set; }

            public string summary { get; set; }

        }

        public enum Priority
        {
            low,
            medium,
            high
        }
    }

>Nota: Task es una palabra reservada en C# por lo que puede dar problemas en el futuro.

Ahora vamos a crear el **contexto** donde van a ir todas las relaciones de los modelos que nosotros tenemos para poder transformarlo en colecciones que van a representarse dentro de la base de datos.

    namespace EF.Models
    {
        class TaskContext : DbContext
        {

            public DbSet<Categoty> Categoties { get; set; }

            public DbSet<Task> Tasks { get; set; }

            public TaskContext(DbContextOptions<TaskContext> options) : base(options) { }
    }

DBSet: Es un set o una asignación de datos del modelo que nosotros hemos creado previamente, básicamente esto va a representar lo que sería una tabla dentro del contexto de entity framework.
>Nota: Un DbSet representa la colección de todas las entidades en el contexto, que se puede consultar desde la base de datos, de un tipo determinado. Los objetos DbSet se crean a partir de DbContext mediante el método DbContext. set.

Con esto ya podriamos crear una base de datos, pero existen atributos o DataAnnotations que nos permiten configurar de una manera mas estricta y especifica de nuestra base de datos

**category.cs**

    public class Categoria
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Name { get; set; }

        public string Description { get; set; }
    }

**task.cs**

    public class Task
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey]
        public Guid IdCategory { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        public string Description { get; set; }
        public Priority PriorityTask { get; set; }
        public DateTime Date { get; set; }

        public virtual Categoty Categoty { get; set; }

        [NotMapped]
        public string summary { get; set; }
    }


Con estas categoria podremos especificar a cada columna:

- [Key] Define un tipo de dato primario como el ID.
- [Required] Define un dato de uso obligatorio. El usuario - deberá cargarlo si o si.
[MaxLength(<Int>)] Establece un número máximo de caracteres que se podrán cargar en la DDBB.
- [ForeignKey("<Str>")] Define una clave foránea y le da un nombre. Una clave foránea es un dato que se encuentra en otro documento, que es foráneo, que es “extranjero”.
- [NotMapped] Omite el mapeo en la base de datos de este atributo.

Con esto ya estamos listo para conectarnos a una base de datos real, pero antes de intentarlo vamos a comprobar que todo este andando correctamente. para ello conectemonos a una base de dato en memoria.

vamos a nuestro archivo program.cs

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using proyectoef;

    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddDbContext<TaskContext>(p => p.UseInMemoryDatabase("TaskDB"));

    var app = builder.Build();

    app.MapGet("/", () => "Hello World!");

    app.MapGet("/dbconexion", async ([FromServices] TaskContext dbContext) => 
    {
        dbContext.Database.EnsureCreated();
        return Results.Ok("Base de datos en memoria: " + dbContext.Database.IsInMemory());

    });

    app.Run();

con el primer app.MapGet() simplemente verifico si me olvide de algo para que Entity Framework puede generar la base de datos en memoria.

con el segundo app. MapGet() creo la base de datos en memorias y si todo fue creado correctamente me devolvera un mensaje en caso contrario podremos ver el error en nuestra consola.

A este Endpoind lo correremos en PostMan.

![](./Contents/Img/Postman.png)

>Nota: Para las personas que probaron con Postman y que al hacer click en Send les regresó el resultado ‘Could not get response’.
Lo que hay que hacer es ir a la pestaña ‘Settings’ dentro de postman y deshabilitar la primera opción que dice ‘Enable SSL Certificate Verification’.

Ahora podemos crear la base de datos. para ello comentemos nuestros codigo que nos permite crear la base de datos en memoria y agregurmos uno nuevo.

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using proyectoef;

    var builder = WebApplication.CreateBuilder(args);

    //builder.Services.AddDbContext<TaskContext>(p => p.UseInMemoryDatabase("TaskDB"));
    builder.Services.AddSqlServer<TaskContext>(builder.Configuration.GetConnectionString("SQLServer"));


    var app = builder.Build();

    app.MapGet("/", () => "Hello World!");

    app.MapGet("/dbconexion", async ([FromServices] TaskContext dbContext) => 
    {
        dbContext.Database.EnsureCreated();
        return Results.Ok("Base de datos en memoria: " + dbContext.Database.IsInMemory());

    });

    app.Run();

Luego en nuestro archivo appsetting.json copiaremos el siguiente codigo

    {
        "Logging": {
            "LogLevel": {
            "Default": "Information",
            "Microsoft.AspNetCore": "Warning"
            }
        },
        "AllowedHosts": "*",
        "ConnectionStrings": {
            "SQLServer": "Data Source=(local); Initial Catalog= TareasDb;Trusted_Connection=True; Integrated Security=True"
        }
    }

En mi caso no tenia guardada una contraseña y usuario si este no es tu caso, la cadena de concexion a la base de datos seria el siguiente.

    "Data Source=server;Initial Catalog=db;user id=sa; password=pass";

Volvamos a llamar al endpoint el resultado que nos traera sera **false** porque ya no estamos conectando en memoria, pero nos deberia crear la base de datos. Esto se puede comprobar en Visual manager.


## Fluen API

En algunos escenarios mas avanzados nos vamos a quedar cortos en la creacion de base de datos con los atributos.
Fuent Api nos va a permirit crear y configurar de una forma mucho mas avanzada nestros modelos para nuestra base de datos.

Comencemos eliminando los atributos en nuestros modelos para crear lo mismo con Fluen API.

Luego vamos a ir a nuestros TaskCintext.cs y vamos a agregar un poco de codigo.

    class TaskContext : DbContext
    {

        public DbSet<Categoty> Categoties { get; set; }

        public DbSet<Task> Tasks { get; set; }

        public TaskContext(DbContextOptions<TaskContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categoty>(categoty =>
            {
                categoty.ToTable("Category");
                categoty.HasKey(p => p.Id);

                categoty.Property(p => p.Name).IsRequired().HasMaxLength(150);
                categoty.Property(p => p.Salubrity).IsRequired();
                categoty.Property(p => p.Description).IsRequired(false);

                categoty.HasData(listCategory);
            });

            modelBuilder.Entity<Task>(task =>
            {
                task.ToTable("TasK");
                task.HasKey(p => p.Id);

                //aqui le digo a fluet que cree una base que tenga una relacion de unos a muchos y que la clave foranea va a ser idCategory
                task.HasOne(p => p.Categoty).WithMany(p => p.Tasks).HasForeignKey(p => p.IdCategory);

                task.Property(p => p.Title).IsRequired().HasMaxLength(150);
                task.Property(p => p.Description).IsRequired(false);
                task.Property(p => p.Date);
                task.Property(p => p.PriorityTask).HasConversion(x => x.ToString(), x => (Priority)Enum.Parse(typeof(Priority), x));
                task.Ignore(p => p.summary);

                task.HasData(listTask);
            });
        }
    }

Con estos cambios ahora podemos volver a recrear nuestra base de datos, para esto primero debemos eliminar la primera base de datos.

Luego de eliminarla corremos nuestro programa, una vez andando volvemos a Postman y corremos nuestro Endpoint. Vamos a visual management y comprovamos que nuestra base de datos este andando correctamente.

## Migraciones

Las migraciones es una herramienta muy util que entity framework nos brinda para facilitarnos cambios que haremos posteriormente en nuestra base de datos a medidas que iremos avanzando con nuestro proyectos.
Para empezar necesatemos descargar la dependecia en nuestro proyecto con el siguiente codigo.

    dotnet tool install --global dotnet-ef --version 7.0.1

Este comando nos descargara la herramienta que usaremos en este simple ejemplo. Sin embargo puede haber nuevas versiones en un futuro por lo que si desea revisar les dejo en este [link](https://www.nuget.org/packages/dotnet-ef#versions-body-tab "link") la pagina para descargar.
Luego de descargar esta herramienta descargaremos otra dependencia llamada EntityFrmaeworkCore.Design con la siguiente linea de comando.

    dotnet add package Microsoft.EntityFrameworkCore.Design --version 7.0.1

Nuevamente les dejo el link para que corroboren la ultima version.[Link](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Design#versions-body-tab "Link")

Ahora es importante no trabajar con una base de datos previamente creada, ya que eso tan solo nos dificultaria el trabajo. Por lo que si has seguido los pasos hasta ahora, vamos a tener que borrar la base de datos que hemos creado con Fluent antes de iniciar la siguiente linea de comando.

    dotnet ef migrations add InitialCreate

> Nota: InitialCreate es un nombre estandar que nos permitira saber no solo a nosotros si no al programador que vea que esta fue la primera version de nuestra migracion.

Con este comando se nos debio de haber creado una nueva carpeta llamada Migrations, en ella se van a encontrar las configuraciones que nos van a permitir trabajar en nuestro proyecto.
Ahora que tenemos lo necesario, para poder continuar vamos a tener que crear de nuevo la base de datos con el siguiente comando.

    dotnet ef database update

Ya creada nuestra base de datos, podemos ir a SQL management para confirmar la creacion de la base de datos. Ahora, si es que la base no esta, no te preocupes solo tienes que actualizar para encontrarla.
En nuestra nueva base de datos vamos a encontrar nuestras dos tablas iniciales a las que ya estamos acostumbrados y una tercera tabla nueva que va a contener las migraciones o cambios que se van a hacer en nuestro proyecto.

Ya sabemos crear una base de datos, pero avanzado nuestro proyecto nos topamos con la necesidad de modificar nuestra base de datos. en mi ejemplo voy a agregar una nueva porpiedad llamada **Salubrity ** en mi clase **Category**.

    public class Categoty
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
        public string Salubrity {get;set;}

        public virtual ICollection<Task> Tasks { get; set; }
    }

Luego voy a ir a mi archivo TaskContext para agregar este nuevo cambio.

	modelBuilder.Entity<Categoty>(categoty =>
	{
		categoty.ToTable("Category");
		categoty.HasKey(p => p.Id);

		categoty.Property(p => p.Name).IsRequired().HasMaxLength(150);
		categoty.Property(p => p.Salubrity).IsRequired();
		categoty.Property(p => p.Description);
	});

Hecho los cambios ahora necesitamos hacer una migracion para que estos cambios se vean reflejados en nuestra base de datos. Para ello usamos la siguiente linea de codigo en nuestra terminal.

    dotnet ef migrations add ColumnSalubrityCategory

Si todo esto fue hecho correctamente podras notar que en la carpeta Migrations ahora tiene los archivos nuevos con los cambios que hicimos, pero esto no termina aqui. Necesitamos subir estos cambios para que esten en la base de datos.
En nuestra terminal escribiremos entonces el comando.

	dotnet ef database update

Si es por esas casualidadades que necesitas remover la migracion por que falto algo. la solucion es muy simple solo escribiremos en la linea de comando
	dotnet ef migrations remove

Ahora si vamos a nuestra base de datos vemos que los datos tipo string como descrition, son campos no nulos.
Esto se debe a que C# lo configura de esta manera por defecto, para evitar esto lo unico que hay que hacer es agregar al metodo IsRequired(false)

	  categoty.Property(p => p.Description).IsRequired(false);

En el caso de que necesitemos datos iniciales para trabajar con nuestra base de datos, solo tendremos que crear una lista con los datos que necesitemos. siguiendo el ejemplo.

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
	List<Categoty> listCategory = new List<Categoty>();
	listCategory.Add(new Categoty() { Id = Guid.Parse("7b5e9399-8e95-4ae8-8745-9542a01e2cc0"), Name = "Asunto domesticos", Salubrity = "Insalubre" });
	listCategory.Add(new Categoty() { Id = Guid.Parse("0a9fa564-0604-4dfa-88df-3636fe395651"), Name = "Actividad recreativa", Salubrity = "sadudable y recomendable" });
    
    
	modelBuilder.Entity<Categoty>(categoty =>
	{
	categoty.ToTable("Category");
	categoty.HasKey(p => p.Id);

	categoty.Property(p => p.Name).IsRequired().HasMaxLength(150);
	categoty.Property(p => p.Salubrity).IsRequired();
	categoty.Property(p => p.Description).IsRequired(false);
	});

    
     	List<Task> listTask = new List<Task>();
      	listTask.Add(new Task() { Id = Guid.Parse("f5d327bf-be98-4786-81d5-0a2412b7807e"), IdCategory = Guid.Parse("7b5e9399-8e95-4ae8-8745-9542a01e2cc0"), Title = "Limpiar Baño", PriorityTask = Priority.medium, Date = DateTime.Now });
      	listTask.Add(new Task() { Id = Guid.Parse("629f9587-abc8-4c85-859f-acb762b754ed"), IdCategory = Guid.Parse("0a9fa564-0604-4dfa-88df-3636fe395651"), Title = "Practica con el arco", PriorityTask = Priority.medium, Date = DateTime.Now });
	  
	  modelBuilder.Entity<Task>(task =>
                {
                    task.ToTable("TasK");
                    task.HasKey(p => p.Id);
    
                    task.HasOne(p => p.Categoty).WithMany(p => p.Tasks).HasForeignKey(p => p.IdCategory);
    
                    task.Property(p => p.Title).IsRequired().HasMaxLength(150);
                    task.Property(p => p.Description).IsRequired(false);
                    task.Property(p => p.Date);
                    task.Property(p => p.PriorityTask).HasConversion(x => x.ToString(), x => (Priority)Enum.Parse(typeof(Priority), x));
                    task.Ignore(p => p.summary);
    
	});

Luego de haber echo los cambios y agregada la lista, es hora que agreguemos los cambios y lo subamos con los comando que ya conocemos.

	 dotnet ef migrations add InitialData
	 dotnet ef database update

> para esta altura  ya te habras dado cuenta que cada vez que hacemos un cambio usarmos el comando "Add" y cada vez que lo subimos a la base de datos usamos el comando "Update"

Ahora que tenemos una base de datos y la hemos cargados con unos datos de prueba vamos a ver si conseguimos traer los datos de la base de datos. Para hacerlo, usararemos el Map para crear un nuevo endpoin con el cual revisaremos de una manera rapida si los datos llegaron.

    app.MapGet("/api/tareas", async ([FromServices] TaskContext dbcontext) =>
    {
        return Results.Ok(dbcontext.Tasks);
    });

Corremos nuestro proyecto, entramos a la url que designamos y si en tu pantalla te aparece un archivo tipo json con los datos que cargamos anteriormente, Felicidades, losgramos conectarnos a la base de datos.
Supongamos ahora que queremos probar filtar informacion; esto seria tan sensillo como hacer uso de las sentencias Linq.

	return Results.Ok(dbcontext.Tasks.Where(p => p.PriorityTask == Priority.low));

> En este ejemplo debido a que copie y pegue, La dos veces que incerte datos en la lista lo hice con ambos con prioridad media. es por ello que cuando cambies el codigo, si no agregaste algo extra, te aparecera vacio. Es normal, porque no hay en la base de datos algun campo que cumpla con la condicion.

Ahora, supongamos que nosotros quisieramos comprobar que la relacion que teniamos entre la tablas Tasks y la tabla Categories. Eso seria tan simple como anteponer el metodo Include antes del Where.
	return Results.Ok(dbcontext.Tasks.Include(x => x.Categoty).Where(p => p.PriorityTask == Priority.medium));

Sin embargo, si seguiste este ejemplo encontraras que te dara un error. Este error es muy comun y se debe a que tenemos un nuestra clase Category una colleccion de Task, por lo que entrara en un blucle infinito.
Para evitar esto colocaremos el atributo jsonIgnore y listo ya estara andando.
	    public class Categoty
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
        public string Salubrity { get; set; }

        [JsonIgnore]:
        public virtual ICollection<Task> Tasks { get; set; }
    }

Bien, ya podemos obtener y fliltar elementos de la base de datos usando Entity Framework, pero eso no es lo unico que queremos probar.
Para insertar elementos a la base de datos necesitariamos entrar al metodo MapPost y especificar el tipo de datos que estariamos insertando en la base de datos.
Luego dentro de la funcion podremos inicializar los valores que vamos a ingresar en nuestra base de datos. Para ello vamos a ingresar a Postman y en nuestro espacio de trabajo cambiaremos el metodo por el Post, entraremos en la pestaña que dice body elegiremos la opcion rar  y json como esta en la siguiente imagen.

![Imagen](Contents/Img/Post.png "Imagen")
> Nota aqui estoy cargando desde Posman las demas propiedades. Solo para mostrar que se puede desde ambas partes.

Ahora solo nos faltan dos metodos mas que son el de actualizar y el de eliminar.
Para ello haremos cosas similares que con el metodo de crear. Para actualizar usaremos el metodo MapPut para buscar elementos de nuestra tarea y desde el Postman actualizaremos los dados.

	    app.MapPut("/api/tareas/{id}", async ([FromServices] TaskContext dbcontext, [FromBody] Platzi.Models.Task tarea, [FromRoute] Guid id) =>
    {
        var tareaActual = dbcontext.Tasks.Find(id);
        if (tareaActual != null)
        {
            tareaActual.Categoty = tarea.Categoty;
            tareaActual.Title = tarea.Title;
            tareaActual.PriorityTask = tarea.PriorityTask;
            tareaActual.Date = tarea.Date;
    
            await dbcontext.SaveChangesAsync();
            return Results.Ok();
        }
        return Results.NotFound();
    });
    
En el caso de no funcionar devolvera un error 404.

![Metodo Put](Contents/Img/Put.png "Metodo Put")

Por ultomo creamos el metodo para eliminar los datos de la base de datos y con esto terminariamos nuestra web minima.

	app.MapDelete("/api/tareas/{id}", async ([FromServices] TaskContext dbcontext,[FromRoute] Guid id )=>
	{
    var tareaActual = dbcontext.Tasks.Find(id);
    if (tareaActual != null)
    {
        dbcontext.Remove(tareaActual);
        await dbcontext.SaveChangesAsync();

        return Results.Ok();
    }

    return Results.NotFound();
	});

![Delete](Contents/Img/Delete.png "Delete")

Si llegaste hasta aqui, felicidades terminamos una API minima. ahora conoces los servicios basicos para la comunicacion de API web con ASP.NET Core y Entity Frmaework.
Si quieres saber mas aqui te dejo la [documentacion](https://learn.microsoft.com/es-es/aspnet/core/tutorials/min-web-api?view=aspnetcore-7.0&tabs=visual-studio-code "documentacion") sobre el tema.