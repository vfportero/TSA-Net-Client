using Rhino.Mocks;
using StructureMap;

namespace Terminis.Tests.Helpers
{
    public class BaseTests
    {


        protected T InitializeAndInject<T>() where T : class
        {
            T service = (T)MockRepository.GenerateStub(typeof(T));

            ObjectFactory.Inject(typeof(T), service);

            return service;
        }


    }
}
