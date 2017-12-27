namespace Autostop.Client.Abstraction.Facades
{
    public interface IAutoMapperFacade
    {
        TDestination Map<TDestination>(object source);
    }
}
