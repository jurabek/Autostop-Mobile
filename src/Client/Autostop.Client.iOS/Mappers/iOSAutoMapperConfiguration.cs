using AutoMapper;

namespace Autostop.Client.iOS.Mappers
{
    // ReSharper disable once InconsistentNaming
    public class iOSAutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            });
        }
    }
}
