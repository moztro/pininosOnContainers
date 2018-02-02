using Containers.Models;
using User.Data.Context;
using User.Data.Repository;

namespace User.Services.Business
{
    public class PeopleService : Repository<Person>, IPeopleService
    {
        public PeopleService(ApplicationContext context)
            : base(context)
        { }
    }
}
