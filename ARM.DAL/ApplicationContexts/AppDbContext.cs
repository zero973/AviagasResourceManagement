using System.Data;
using ARM.Core.Enums;
using ARM.Core.Helpers;
using ARM.DAL.Models.Entities;
using ARM.DAL.Models.Security;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;

namespace ARM.DAL.ApplicationContexts;

public class AppDbContext : DbContext
{

    private readonly IConfiguration _configuration;

    /// <summary>
    /// Пользователи
    /// </summary>
    public DbSet<AppUser> Users { get; set; }

    /// <summary>
    /// Шкафы
    /// </summary>
    public DbSet<Cabinet> Cabinets { get; set; }

    /// <summary>
    /// Детали шкафа
    /// </summary>
    public DbSet<CabinetPart> CabinetParts { get; set; }

    /// <summary>
    /// Комментарии к задачам
    /// </summary>
    public DbSet<Comment> Comments { get; set; }

    /// <summary>
    /// Сотрудники
    /// </summary>
    public DbSet<Employee> Employees { get; set; }
    
    /// <summary>
    /// Зарплаты сотрудников
    /// </summary>
    public DbSet<EmployeeSalary> EmployeeSalaries { get; set; }

    /// <summary>
    /// Задачи
    /// </summary>
    public DbSet<SystemTask> Taks { get; set; }

    /// <summary>
    /// Отработанное время сотрудников
    /// </summary>
    public DbSet<WorkedTime> WorkedTimes { get; set; }

    /// <summary>
    /// Сотрудники, которые прикреплены к задачам
    /// </summary>
    public DbSet<TaskEmployee> TaskEmployees { get; set; }

    /// <summary>
    /// Кол-во детали шкафа, которые требуются в задаче
    /// </summary>
    public DbSet<CabinetPartCounts> CabinetPartCounts { get; set; }
    
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    
    public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration configuration) : base(options)
    {
        _configuration = configuration;
        // dotnet tool update --global dotnet-ef
        // dotnet ef migrations add init -c AppDbContext --output-dir Migrations
        // dotnet ef database update -c AppDbContext
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        var isActualFilter = $"\"{nameof(BaseActualEntity.IsActual)}\" = true";

        builder.Entity<Employee>(entity =>
        {
            entity.HasIndex(e => e.Passport)
                .HasFilter(isActualFilter)
                .IsUnique();
            
            entity.HasIndex(e => new { e.FirstName, e.LastName, e.Patronymic })
                .HasFilter(isActualFilter);
        });
        
        builder.Entity<EmployeeSalary>(entity =>
        {
            // у одного работника может быть только одна зарплата
            entity.HasIndex(e => e.EmployeeId)
                .HasFilter(isActualFilter)
                .IsUnique();
            
            entity.HasOne(e => e.Employee)
                .WithOne(e => e.Salary)
                .HasForeignKey<EmployeeSalary>(e => e.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);
        });
        
        builder.Entity<AppUser>(entity =>
        {
            // к одной учётной записи может быть привязан только 1 сотрудник
            entity.HasIndex(e => e.EmployeeId)
                .HasFilter(isActualFilter)
                .IsUnique();
            
            entity.HasIndex(e => e.Login)
                .HasFilter(isActualFilter)
                .IsUnique();

            entity.HasIndex(e => new { e.Login, e.PasswordHash })
                .HasFilter(isActualFilter)
                .IsUnique();
            
            entity.HasOne(e => e.Employee)
                .WithOne(e => e.User)
                .HasForeignKey<AppUser>(e => e.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<Cabinet>(entity =>
        {
            entity.HasIndex(e => e.Name)
                .IsUnique();
            
            entity.HasIndex(e => new { e.Name, e.Fullname })
                .IsUnique();
        });

        builder.Entity<CabinetPart>(entity =>
        {
            entity.HasIndex(e => e.Name)
                .IsUnique();
        });

        builder.Entity<SystemTask>(entity =>
        {
            entity.HasIndex(e => e.Name)
                .HasFilter(isActualFilter);
            
            entity.HasOne(e => e.CurrentPerformer)
                .WithMany(e => e.Tasks)
                .HasForeignKey(e => e.CurrentPerformerId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<Comment>(entity =>
        {
            entity.HasIndex(e => new { e.EmployeeId, e.TaskId })
                .HasFilter(isActualFilter);
            
            entity.HasOne(e => e.Employee)
                .WithMany(e => e.Comments)
                .HasForeignKey(e => e.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);
            
            entity.HasOne(e => e.LinkedTask)
                .WithMany(e => e.Comments)
                .HasForeignKey(e => e.TaskId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<TaskEmployee>(entity =>
        {
            // одного и того же сотрудника можно прикрепить к задаче только 1 раз
            entity.HasIndex(e => new { e.EmployeeId, e.TaskId })
                .IsUnique();
            
            entity.HasOne(e => e.Employee)
                .WithMany(e => e.TaskEmployees)
                .HasForeignKey(e => e.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);
            
            entity.HasOne(e => e.LinkedTask)
                .WithMany(e => e.TaskEmployees)
                .HasForeignKey(e => e.TaskId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<WorkedTime>(entity =>
        {
            entity.HasIndex(e => new { e.EmployeeId, e.TaskId })
                .HasFilter(isActualFilter);
            
            entity.HasOne(e => e.Employee)
                .WithMany(e => e.WorkedTimes)
                .HasForeignKey(e => e.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);
            
            entity.HasOne(e => e.LinkedTask)
                .WithMany(e => e.WorkedTimes)
                .HasForeignKey(e => e.TaskId)
                .OnDelete(DeleteBehavior.Restrict);
        });
        
        builder.Entity<CabinetPartCounts>(entity =>
        {
            entity.HasIndex(e => new { e.CabinetPartId, e.TaskId })
                .HasFilter(isActualFilter)
                .IsUnique();
            
            entity.HasOne(e => e.CabinetPart)
                .WithMany(e => e.CabinetPartCounts)
                .HasForeignKey(e => e.CabinetPartId)
                .OnDelete(DeleteBehavior.Restrict);
            
            entity.HasOne(e => e.LinkedTask)
                .WithMany(e => e.CabinetPartCounts)
                .HasForeignKey(e => e.TaskId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_configuration.GetConnectionString("Postgres"));
        //optionsBuilder.EnableSensitiveDataLogging();
        //optionsBuilder.LogTo(message => System.Diagnostics.Trace.WriteLine(message));

        optionsBuilder.UseSeeding((context, _) =>
        {
            var cabinet1 = new Cabinet("ВРУ", "Вводно-распределительное устройство");
            var cabinet2 = new Cabinet("ИБП", "Источник бесперебойного питания");
            var cabinet3 = new Cabinet("ЩУПТ", "Щит управления подготовки теплоносителя");
            var cabinet4 = new Cabinet("ШВП", "Шкаф вторичных приборов");
            var cabinet5 = new Cabinet("ЩПС", "Щит пожарной сигнализации");
            var cabinet6 = new Cabinet("ЩР", "Щит распределительный");
            var cabinet7 = new Cabinet("ПУБО", "Пульт управления боксом операторной");
            
            context.Set<Cabinet>().AddRange(cabinet1, cabinet2, cabinet3, cabinet4, cabinet5, cabinet6, cabinet7);

            var cabinetPart1 = new CabinetPart("Выключатель автоматический", 1100);
            var cabinetPart2 = new CabinetPart("Блок питания", 20000);
            var cabinetPart3 = new CabinetPart("Информационная панель", 50000);
            var cabinetPart4 = new CabinetPart("Контроллер", 70000);
            var cabinetPart5 = new CabinetPart("Реле", 1300);
            var cabinetPart6 = new CabinetPart("Шина \"РЕ\"", 2400);
            var cabinetPart7 = new CabinetPart("Проходная клемма", 120);
            
            context.Set<CabinetPart>().AddRange(cabinetPart1, cabinetPart2, cabinetPart3, cabinetPart4, cabinetPart5, 
                cabinetPart6, cabinetPart7);
            
            // Админ
            var admin = new Employee("Сиразетдинов", "Айрат", "Айратович", new DateTime(1972, 06, 03), "9014813631")
                { PhotoUrl = "https://cdn.hackaday.io/images/5144291501640748594.png" };
            // Инженеры
            var employee2 = new Employee("Михоношин", "Дмитрий", "Владимирович", new DateTime(1989, 7, 17), "9211487653") 
                { PhotoUrl = "https://avatars.mds.yandex.net/i?id=04c7db6960c55da48ffd046c748e6d48_l-5236416-images-thumbs&n=13" };
            var employee3 = new Employee("Малакеев", "Сергей", "Владиславович", new DateTime(1978, 09, 28), "9223597656");
            var employee4 = new Employee("Мирошников", "Вячеслав", "Сергеевич", new DateTime(1978, 11, 28), "9223697657");
            var employee5 = new Employee("Безруков", "Евгений", "Петрович", new DateTime(1975, 04, 09), "9113687659");
            var employee6 = new Employee("Таланов", "Андреей", "Андреевич", new DateTime(1997, 08, 24), "9214977687");
            var employee7 = new Employee("Сулейманов", "Вильдан", "Рустемович", new DateTime(2001, 01, 17), "9214969121");
            var employee8 = new Employee("Пчелинцев", "Александр", "Евгеньевич", new DateTime(1991, 06, 11), "9112913621");
            // Чертёжник
            var draftsman = new Employee("Левкин", "Владимир", "Васильевич", new DateTime(1986, 04, 30), "9115933627");
            // Склад
            var storage = new Employee("Зайнуллина", "Вероника", "Олеговна", new DateTime(1990, 08, 22), "9135921628");
            
            context.Set<Employee>().AddRange(admin, employee2, employee3, employee4, employee5, employee6, 
                employee7, employee8, draftsman, storage);

            context.Set<EmployeeSalary>().AddRange(
                new EmployeeSalary(admin.Id, 500, new DateTime(2024, 1, 1), DateTime.MaxValue), 
                new EmployeeSalary(employee2.Id, 415, new DateTime(2024, 1, 1), DateTime.MaxValue),
                new EmployeeSalary(employee3.Id, 415, new DateTime(2024, 1, 1), DateTime.MaxValue),
                new EmployeeSalary(employee4.Id, 415, new DateTime(2024, 1, 1), DateTime.MaxValue),
                new EmployeeSalary(employee5.Id, 415, new DateTime(2024, 1, 1), DateTime.MaxValue),
                new EmployeeSalary(employee6.Id, 415, new DateTime(2024, 1, 1), DateTime.MaxValue),
                new EmployeeSalary(employee7.Id, 415, new DateTime(2024, 1, 1), DateTime.MaxValue),
                new EmployeeSalary(employee8.Id, 415, new DateTime(2024, 1, 1), DateTime.MaxValue),
                new EmployeeSalary(draftsman.Id, 400, new DateTime(2024, 1, 1), DateTime.MaxValue),
                new EmployeeSalary(storage.Id, 250, new DateTime(2024, 1, 1), DateTime.MaxValue));
            
            context.Set<AppUser>().AddRange(
                new AppUser("admin", Encryptor.EncryptString("admin"), UsersRoles.Admin, admin.Id), 
                new AppUser("engineer1", Encryptor.EncryptString("engineer1"), UsersRoles.Engineer, employee2.Id), 
                new AppUser("engineer2", Encryptor.EncryptString("engineer2"), UsersRoles.Engineer, employee3.Id), 
                new AppUser("engineer3", Encryptor.EncryptString("engineer3"), UsersRoles.Engineer, employee4.Id), 
                new AppUser("engineer4", Encryptor.EncryptString("engineer4"), UsersRoles.Engineer, employee5.Id), 
                new AppUser("engineer5", Encryptor.EncryptString("engineer5"), UsersRoles.Engineer, employee6.Id), 
                new AppUser("engineer6", Encryptor.EncryptString("engineer6"), UsersRoles.Engineer, employee7.Id), 
                new AppUser("engineer7", Encryptor.EncryptString("engineer7"), UsersRoles.Engineer, employee8.Id), 
                new AppUser("draftsman", Encryptor.EncryptString("draftsman"), UsersRoles.Draftsman, draftsman.Id), 
                new AppUser("storage", Encryptor.EncryptString("storage"), UsersRoles.Storage, storage.Id));

            var task1 = new SystemTask("Собрать шкаф ВРУ", cabinet1.Id, TaskStatuses.Installation, 
                employee5.Id, new DateTime(2024, 12, 24), 100) { CreateDate = new DateTime(2024, 09, 20) };
            var task2 = new SystemTask("Собрать шкаф ИБП", cabinet2.Id, TaskStatuses.AssemblingComponents, 
                storage.Id, new DateTime(2024, 11, 30), 120) { CreateDate = new DateTime(2024, 10, 2) };
            var task3 = new SystemTask("Собрать шкаф ЩУПТ", cabinet3.Id, TaskStatuses.Closed, 
                employee7.Id, new DateTime(2024, 12, 24), 90) 
                { 
                    CreateDate = new DateTime(2024, 10, 16), 
                    FinishDate = new DateTime(2024, 12, 2)
                };
            var task4 = new SystemTask("Собрать шкаф ПУБО", cabinet3.Id, TaskStatuses.Installation, 
                employee2.Id, new DateTime(2024, 12, 2), 80) { CreateDate = new DateTime(2024, 11, 11) };

            context.Set<SystemTask>().AddRange(task1, task2, task3, task4);

            context.Set<TaskEmployee>().AddRange(
                new TaskEmployee(draftsman.Id, task1.Id), 
                new TaskEmployee(storage.Id, task1.Id), 
                new TaskEmployee(employee5.Id, task1.Id),
                
                new TaskEmployee(draftsman.Id, task2.Id), 
                new TaskEmployee(storage.Id, task2.Id), 
                new TaskEmployee(employee4.Id, task2.Id),
                
                new TaskEmployee(draftsman.Id, task3.Id), 
                new TaskEmployee(storage.Id, task3.Id), 
                new TaskEmployee(employee7.Id, task3.Id),
                
                new TaskEmployee(draftsman.Id, task4.Id), 
                new TaskEmployee(storage.Id, task4.Id), 
                new TaskEmployee(employee2.Id, task4.Id));
            
            context.Set<CabinetPartCounts>().AddRange(
                new CabinetPartCounts(cabinetPart1.Id, task1.Id, 13) { CreateDate = task1.CreateDate }, 
                new CabinetPartCounts(cabinetPart2.Id, task1.Id, 2) { CreateDate = task1.CreateDate },
                new CabinetPartCounts(cabinetPart3.Id, task1.Id, 1) { CreateDate = task1.CreateDate },
                new CabinetPartCounts(cabinetPart4.Id, task1.Id, 1) { CreateDate = task1.CreateDate },
                new CabinetPartCounts(cabinetPart5.Id, task1.Id, 13) { CreateDate = task1.CreateDate }, 
                new CabinetPartCounts(cabinetPart6.Id, task1.Id, 1) { CreateDate = task1.CreateDate },
                new CabinetPartCounts(cabinetPart7.Id, task1.Id, 70) { CreateDate = task1.CreateDate },
                
                new CabinetPartCounts(cabinetPart1.Id, task2.Id, 3) { CreateDate = task2.CreateDate }, 
                new CabinetPartCounts(cabinetPart2.Id, task2.Id, 1) { CreateDate = task2.CreateDate },
                new CabinetPartCounts(cabinetPart3.Id, task2.Id, 1) { CreateDate = task2.CreateDate },
                new CabinetPartCounts(cabinetPart6.Id, task2.Id, 1) { CreateDate = task2.CreateDate },
                new CabinetPartCounts(cabinetPart7.Id, task2.Id, 70) { CreateDate = task2.CreateDate },
                
                new CabinetPartCounts(cabinetPart1.Id, task3.Id, 9) { CreateDate = task3.CreateDate }, 
                new CabinetPartCounts(cabinetPart2.Id, task3.Id, 3) { CreateDate = task3.CreateDate },
                new CabinetPartCounts(cabinetPart3.Id, task3.Id, 1) { CreateDate = task3.CreateDate },
                new CabinetPartCounts(cabinetPart4.Id, task3.Id, 2) { CreateDate = task3.CreateDate },
                new CabinetPartCounts(cabinetPart5.Id, task3.Id, 9) { CreateDate = task3.CreateDate }, 
                new CabinetPartCounts(cabinetPart6.Id, task3.Id, 1) { CreateDate = task3.CreateDate },
                new CabinetPartCounts(cabinetPart7.Id, task3.Id, 57) { CreateDate = task3.CreateDate },
                
                new CabinetPartCounts(cabinetPart1.Id, task4.Id, 2) { CreateDate = task4.CreateDate }, 
                new CabinetPartCounts(cabinetPart2.Id, task4.Id, 1) { CreateDate = task4.CreateDate },
                new CabinetPartCounts(cabinetPart3.Id, task4.Id, 1) { CreateDate = task4.CreateDate },
                new CabinetPartCounts(cabinetPart4.Id, task4.Id, 1) { CreateDate = task4.CreateDate },
                new CabinetPartCounts(cabinetPart5.Id, task4.Id, 5) { CreateDate = task4.CreateDate }, 
                new CabinetPartCounts(cabinetPart6.Id, task4.Id, 1) { CreateDate = task4.CreateDate },
                new CabinetPartCounts(cabinetPart7.Id, task4.Id, 24) { CreateDate = task4.CreateDate });

            context.Set<WorkedTime>().AddRange(
                new WorkedTime(storage.Id, task1.Id, new DateTime(2024, 9, 29), 4, false),
                new WorkedTime(employee5.Id, task1.Id, new DateTime(2024, 9, 30), 8, false),
                new WorkedTime(employee5.Id, task1.Id, new DateTime(2024, 10, 13), 8, false),
                new WorkedTime(employee5.Id, task1.Id, new DateTime(2024, 10, 27), 8, false),
                new WorkedTime(employee5.Id, task1.Id, new DateTime(2024, 11, 15), 8, false),
                new WorkedTime(employee5.Id, task1.Id, new DateTime(2024, 11, 14), 8, false),
                new WorkedTime(employee5.Id, task1.Id, new DateTime(2024, 11, 20), 8, false),
                new WorkedTime(employee5.Id, task1.Id, new DateTime(2024, 11, 29), 8, false),
                
                new WorkedTime(storage.Id, task2.Id, new DateTime(2024, 10, 30), 4, false),
                
                new WorkedTime(storage.Id, task3.Id, new DateTime(2024, 10, 23), 4, false),
                new WorkedTime(employee7.Id, task3.Id, new DateTime(2024, 10, 24), 8, false),
                new WorkedTime(employee7.Id, task3.Id, new DateTime(2024, 10, 30), 8, false),
                new WorkedTime(employee7.Id, task3.Id, new DateTime(2024, 11, 5), 8, false),
                new WorkedTime(employee7.Id, task3.Id, new DateTime(2024, 11, 6), 8, false),
                new WorkedTime(employee7.Id, task3.Id, new DateTime(2024, 11, 7), 8, false),
                new WorkedTime(employee7.Id, task3.Id, new DateTime(2024, 11, 8), 8, false),
                new WorkedTime(employee7.Id, task3.Id, new DateTime(2024, 11, 15), 8, false),
                new WorkedTime(employee7.Id, task3.Id, new DateTime(2024, 11, 28), 8, false),
                
                new WorkedTime(storage.Id, task4.Id, new DateTime(2024, 11, 15), 4, false),
                new WorkedTime(employee2.Id, task4.Id, new DateTime(2024, 11, 16), 8, false),
                new WorkedTime(employee2.Id, task4.Id, new DateTime(2024, 11, 17), 8, false),
                new WorkedTime(employee2.Id, task4.Id, new DateTime(2024, 11, 18), 8, false),
                new WorkedTime(employee2.Id, task4.Id, new DateTime(2024, 11, 19), 8, false),
                new WorkedTime(employee2.Id, task4.Id, new DateTime(2024, 11, 20), 8, false),
                new WorkedTime(employee2.Id, task4.Id, new DateTime(2024, 11, 21), 8, false),
                new WorkedTime(employee2.Id, task4.Id, new DateTime(2024, 11, 22), 8, false),
                new WorkedTime(employee2.Id, task4.Id, new DateTime(2024, 11, 23), 8, false),
                new WorkedTime(employee2.Id, task4.Id, new DateTime(2024, 11, 24), 8, true));

            context.Set<Comment>().AddRange(
                new Comment(draftsman.Id, task1.Id, "Ссылка на чертёж: https....."), 
                new Comment(storage.Id, task1.Id, "Нет позиции «Шина PE»"),
                
                new Comment(draftsman.Id, task2.Id, "Ссылка на чертёж: https....."),
                
                new Comment(draftsman.Id, task3.Id, "Ссылка на чертёж: https....."), 
                new Comment(employee7.Id, task3.Id, "Испытание прошло успешно"),
                
                new Comment(draftsman.Id, task4.Id, "Ссылка на чертёж: https....."));
            
            context.SaveChanges();
        });
    }
    
    public (IDbConnection connection, IDbTransaction? transaction) GetConnection()
    {
        var connection = Database.GetDbConnection();
        return (connection, Database.CurrentTransaction?.GetDbTransaction());
    }

    public async Task<string> GetDatabaseVersion()
    {
        var (connection, transaction) = GetConnection();
        return (await connection.ExecuteScalarAsync<string>("SELECT version()"))!;
    }
    
}