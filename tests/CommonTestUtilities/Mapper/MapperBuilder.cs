using AutoMapper;
using FCG.Application.Mapper;
using Microsoft.Extensions.Logging.Abstractions;

namespace CommonTestUtilities.Mapper;
public class MapperBuilder
{
    public static IMapper Build()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new UserMapper());
        }, NullLoggerFactory.Instance);

        return config.CreateMapper();
    }
}
