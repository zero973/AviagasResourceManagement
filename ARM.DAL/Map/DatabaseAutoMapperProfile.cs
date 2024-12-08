using AutoMapper;

namespace ARM.DAL.Map;

public class DatabaseAutoMapperProfile : Profile
{
    public DatabaseAutoMapperProfile()
    {
        CreateMap<Core.Models.Entities.Cabinet, Models.Entities.Cabinet>()
            .ReverseMap();
        
        CreateMap<Core.Models.Entities.CabinetPart, Models.Entities.CabinetPart>()
            .ReverseMap();
        
        CreateMap<Core.Models.Entities.CabinetPartCounts, Models.Entities.CabinetPartCounts>()
            .ReverseMap();

        CreateMap<Core.Models.Entities.Comment, Models.Entities.Comment>()
            .ReverseMap();
        
        CreateMap<Core.Models.Entities.EmployeeAccount, Models.Entities.Employee>()
            .ReverseMap();
        
        CreateMap<Core.Models.Entities.SystemTask, Models.Entities.SystemTask>()
            .ReverseMap();
        
        CreateMap<Core.Models.Entities.TaskEmployee, Models.Entities.TaskEmployee>()
            .ReverseMap();
        
        CreateMap<Core.Models.Entities.WorkedTime, Models.Entities.WorkedTime>()
            .ReverseMap();
        
        CreateMap<Core.Models.Security.RefreshToken, Models.Security.RefreshToken>()
            .ReverseMap();
    }
}