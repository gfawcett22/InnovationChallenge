using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Collaboration.Api.IntegrationEvents.Events;
using Collaboration.Messaging.Models.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Collaboration.Api.Controllers
{
    [Route("api/")]
    public class ValuesController : Controller
    {
        private IEventBus _eventBus;
        public ValuesController(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }
        // GET api/values
        [HttpGet("threads")]
        public IEnumerable<string> Get()
        {
            var eventMessage = new ThreadUpdateIntegrationEvent(1, 2, "This is a new Thread", null);

            _eventBus.Publish(eventMessage);

            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("posts")]
        public string Get(int id)
        {
            var eventMessage = new PostUpdateIntegrationEvent(56, 2, "This is a post", "Garron", DateTime.Now);

            _eventBus.Publish(eventMessage);
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
