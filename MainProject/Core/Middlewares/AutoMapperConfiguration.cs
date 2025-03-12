using BasicDotnetTemplate.MainProject.Models.Api.Common.User;
using SqlServerDatabase = BasicDotnetTemplate.MainProject.Models.Database.SqlServer;
using AutoMapper;


namespace BasicDotnetTemplate.MainProject.Core.Middlewares;
public class AutoMapperConfiguration : Profile
{
    public AutoMapperConfiguration()
    {
        CreateMap<SqlServerDatabase.User, UserDto>();

    }
}
