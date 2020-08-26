using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        public ActionResult<string> Get()
        {
            string SaveFileFolder = "Data";
            string[] files = Directory.GetFiles(SaveFileFolder, "*.json", System.IO.SearchOption.AllDirectories);
            if ( files.Length <= 0 )
                return NotFound();
            string jsonString = "";
            for(int i=0;i<files.Length;i++)
            {
                jsonString += "{\"id\":\""+ files[i].Replace(".json","").Replace("Data/","")+"\",\"name\":\"" +
                    ReadJsonConfig(files[i], "name") +"\",\"value\":\""+
                    ReadJsonConfig(files[i], "value") + "\"}";
                if (i < files.Length - 1)
                    jsonString += ",";
            }
            return "{\"total\":\""+files.Length.ToString()+"\""+ ",\"data\":[" + jsonString + "]}";
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            string filename = "Data//" + id + ".json";
            if (System.IO.File.Exists(filename))
                return "{\"id\":\"" + id + "\",\"name\":\"" +
                    ReadJsonConfig(filename, "name") + "\",\"value\":\"" +
                    ReadJsonConfig(filename, "value") + "\"}";
            return NotFound();
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

        protected string ReadJsonConfig(string file, string strKey)
        {
            var config = new ConfigurationBuilder()
                 .AddJsonFile(file)
                 .Build();
            return config[strKey];

        }
    }
}
