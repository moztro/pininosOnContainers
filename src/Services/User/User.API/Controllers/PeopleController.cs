using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Containers.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using User.Services.Business;

namespace User.API.Controllers
{
    [Produces("application/json")]
    [Route("api/People")]
    public class PeopleController : Controller
    {
        private IPeopleService peopleService;
        private ILogger<PeopleController> logger;
        public PeopleController(IPeopleService peopleService, ILogger<PeopleController> logger)
        {
            this.peopleService = peopleService;
            this.logger = logger;
        }

        // GET: api/People
        [HttpGet]
        public async Task<IEnumerable<Person>> Get()
        {
            return await peopleService.GetAll();
        }

        // GET: api/People/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<Person> Get(int id)
        {
            return await peopleService.Get(id);
        }
        
        // POST: api/People
        [HttpPost]
        public async void Post([FromBody]Person person)
        {
            if (person.Id == 0)
            {
                throw new Exception("entity");
            }

            var personOld = await peopleService.Get(person.Id);
            personOld.FirstName = person.FirstName;
            personOld.LastName = person.LastName;
            personOld.Age = person.Age;

            if (person == null)
            {
                throw new Exception("entity");
            }
            await peopleService.Update(personOld);
        }
        
        // PUT: api/People/5
        [HttpPut()]
        public async void Put([FromBody]Person person)
        {
            await peopleService.Insert(person);
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
