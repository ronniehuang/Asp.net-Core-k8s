using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Data;
using Newtonsoft.Json;
using Models;

namespace ApiDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PythonDemoController : Controller
    {
        // GET api/PythonDemo
        [HttpGet]
        public async Task<ActionResult<string>> Get()
        {
            await WaitAndApologizeAsync();
            Response.Headers.Add("Content-Type", "application/json");
            List<MyCustomModel> listModel = new List<MyCustomModel> {
                new MyCustomModel()
                {
                    Id = 1234,
                    Name = "test1234",
                    Email = "test1234@test.com"
                },
                new MyCustomModel()
                {
                    Id = 6789,
                    Name = "test6789",
                    Email = "test6789@test.com"
                }
            };
            MyCustomModels returnJson = new MyCustomModels()
            {
                total = listModel.Count,
                customers = listModel
            };
            return Ok(returnJson);
            
        }
        // GET api/PythonDemo/heartbeat
        [HttpGet("heartbeat")]
        public async Task<ActionResult<string>> heartBeat()
        {
            await WaitAndApologizeAsync();
            return Ok(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
 
        }
        // GET api/PythonDemo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<string>> Get(int id)
        {
            await WaitAndApologizeAsync();
            MyCustomModel returnJson = new MyCustomModel()
            {
                Id = id,
                Name = "test"+ id,
                Email = "test"+ id + "@test.com"
            };
            return Ok(returnJson);
        }

        // POST api/PythonDemo
        [HttpPost]
        public async Task<ActionResult<string>> Post([FromForm] string value)
        {
            await WaitAndApologizeAsync();
            return Created("","");
        }

        // PUT api/PythonDemo/5
        [HttpPut("{id}")]
        public async Task<ActionResult<string>> Put(int id, [FromForm]string value)
        {
            await WaitAndApologizeAsync();
            using (var reader = new StreamReader(Request.Body))
            {
                string body = await reader.ReadToEndAsync();
                //Console.WriteLine(body);
                MyCustomModel myCustomModel = JsonConvert.DeserializeObject<MyCustomModel>(body);
                myCustomModel.Id = id;
                return Ok(myCustomModel);
            };

            return NotFound();
        }

        // DELETE api/PythonDemo/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<string>> Delete(int id)
        {
            await WaitAndApologizeAsync();
            return Ok("");
        }
        static async Task WaitAndApologizeAsync()
        {
            await Task.Delay(100);
        }

    }
}
