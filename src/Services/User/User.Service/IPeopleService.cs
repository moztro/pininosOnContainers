using Containers.Models;
using System;
using System.Collections.Generic;
using System.Text;
using User.Data.Repository;

namespace User.Services.Business
{
    
    public interface IPeopleService : IRepository<Person>    
    {
    }
}
