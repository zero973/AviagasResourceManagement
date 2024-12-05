using ARM.Core.Enums;
using ARM.Core.Helpers;
using ARM.DAL.Models.Entities;
using ARM.DAL.Models.Security;
using Microsoft.EntityFrameworkCore;
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
            entity.HasIndex(e => new { e.UserId, e.TaskId })
                .HasFilter(isActualFilter);
            
            entity.HasOne(e => e.User)
                .WithMany(e => e.Comments)
                .HasForeignKey(e => e.UserId)
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
            entity.HasIndex(e => new { e.UserId, e.TaskId })
                .HasFilter(isActualFilter);
            
            entity.HasOne(e => e.User)
                .WithMany(e => e.WorkedTimes)
                .HasForeignKey(e => e.UserId)
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
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.LogTo(message => System.Diagnostics.Trace.WriteLine(message));

        optionsBuilder.UseSeeding((context, _) =>
        {
            context.Set<Cabinet>().AddRange(new Cabinet("ВРУ", "Вводно-распределительное устройство"), 
                new Cabinet("ИБП", "Источник бесперебойного питания"), 
                new Cabinet("ЩУПТ", "Щит управления подготовки теплоносителя"), 
                new Cabinet("ШВП", "Шкаф вторичных приборов"), 
                new Cabinet("ЩПС", "Щит пожарной сигнализации"), 
                new Cabinet("ЩР", "Щит распределительный"), 
                new Cabinet("ПУБО", "Пульт управления боксом операторной"));
            
            context.Set<CabinetPart>().AddRange(new CabinetPart("Выключатель автоматический", 1100), 
                new CabinetPart("Блок питания", 20000), 
                new CabinetPart("Информационная панель", 50000), 
                new CabinetPart("Контроллер", 70000), 
                new CabinetPart("Реле", 1300), 
                new CabinetPart("Шина \"РЕ\"", 2400), 
                new CabinetPart("Проходная клемма", 120));
            
            // Админ
            var employee1 = new Employee("Сиразетдинов", "Айрат", "Айратович", new DateTime(1972, 06, 03), "9014813631");
            // Инженеры
            var employee2 = new Employee("Михоношин", "Дмитрий", "Владимирович", new DateTime(1989, 7, 17), "9211487653");
            var employee3 = new Employee("Малакеев", "Сергей", "Владиславович", new DateTime(1978, 09, 28), "9223597656");
            var employee4 = new Employee("Мирошников", "Вячеслав", "Сергеевич", new DateTime(1978, 11, 28), "9223697657");
            var employee5 = new Employee("Безруков", "Евгений", "Петрович", new DateTime(1975, 04, 09), "9113687659");
            var employee6 = new Employee("Таланов", "Андреей", "Андреевич", new DateTime(1997, 08, 24), "9214977687");
            var employee7 = new Employee("Сулейманов", "Вильдан", "Рустемович", new DateTime(2001, 01, 17), "9214969121");
            var employee8 = new Employee("Пчелинцев", "Александр", "Евгеньевич", new DateTime(1991, 06, 11), "9112913621");
            // Чертёжник
            var employee9 = new Employee("Левкин", "Владимир", "Васильевич", new DateTime(1986, 04, 30), "9115933627");
            // Склад
            var employee10 = new Employee("Зайнуллина", "Вероника", "Олеговна", new DateTime(1990, 08, 22), "9135921628");
            
            context.Set<Employee>().AddRange(employee1, employee2, employee3, employee4, employee5, employee6, 
                employee7, employee8, employee9, employee10);

            context.Set<EmployeeSalary>().AddRange(new EmployeeSalary(employee1.Id, 500, new DateTime(2024, 1, 1), DateTime.MaxValue), 
                new EmployeeSalary(employee2.Id, 415, new DateTime(2024, 1, 1), DateTime.MaxValue),
                new EmployeeSalary(employee3.Id, 415, new DateTime(2024, 1, 1), DateTime.MaxValue),
                new EmployeeSalary(employee4.Id, 415, new DateTime(2024, 1, 1), DateTime.MaxValue),
                new EmployeeSalary(employee5.Id, 415, new DateTime(2024, 1, 1), DateTime.MaxValue),
                new EmployeeSalary(employee6.Id, 415, new DateTime(2024, 1, 1), DateTime.MaxValue),
                new EmployeeSalary(employee7.Id, 415, new DateTime(2024, 1, 1), DateTime.MaxValue),
                new EmployeeSalary(employee8.Id, 415, new DateTime(2024, 1, 1), DateTime.MaxValue),
                new EmployeeSalary(employee9.Id, 400, new DateTime(2024, 1, 1), DateTime.MaxValue),
                new EmployeeSalary(employee10.Id, 250, new DateTime(2024, 1, 1), DateTime.MaxValue));

            var user1 = new AppUser("admin", Encryptor.EncryptString("admin"), UsersRoles.Admin, employee1.Id);
            var user2 = new AppUser("engineer1", Encryptor.EncryptString("engineer1"), UsersRoles.Engineer, employee2.Id);
            var user3 = new AppUser("engineer2", Encryptor.EncryptString("engineer2"), UsersRoles.Admin, employee3.Id);
            var user4 = new AppUser("engineer3", Encryptor.EncryptString("engineer3"), UsersRoles.Admin, employee4.Id);
            var user5 = new AppUser("engineer4", Encryptor.EncryptString("engineer4"), UsersRoles.Admin, employee5.Id);
            var user6 = new AppUser("engineer5", Encryptor.EncryptString("engineer5"), UsersRoles.Admin, employee6.Id);
            var user7 = new AppUser("engineer6", Encryptor.EncryptString("engineer6"), UsersRoles.Admin, employee7.Id);
            var user8 = new AppUser("engineer7", Encryptor.EncryptString("engineer7"), UsersRoles.Admin, employee8.Id);
            var user9 = new AppUser("draftsman", Encryptor.EncryptString("draftsman"), UsersRoles.Draftsman, employee9.Id);
            var user10 = new AppUser("storage", Encryptor.EncryptString("storage"), UsersRoles.Storage, employee10.Id);
            
            context.Set<AppUser>().AddRange(user1, user2, user3, user4, user5, user6, user7, user8, user9, user10);

            context.SaveChanges();
        });
    }
    
}