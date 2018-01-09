using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Collaboration.Api.IntegrationEvents.Events;
using Collaboration.Messaging.Models.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Collaboration.Api.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private IEventBus _eventBus;
        public ValuesController(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            var eventMessage = new ThreadUpdateIntegrationEvent(1);

            _eventBus.Publish(eventMessage);

            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
