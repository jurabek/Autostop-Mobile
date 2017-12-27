using AutoMapper;
using Autostop.Client.Abstraction.Facades;

namespace Autostop.Client.Core.Facades
{
    public class AutoMapperFacade : IAutoMapperFacade
    {
        public TDestination Map<TDestination>(object source)
        {
           return Mapper.Map<TDestination>(source);
        }
    }
}
