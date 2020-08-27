using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

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
                jsonString += "{\"id\":\""+ ReadJsonConfig(files[i], "id") + "\",\"name\":\"" +
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
        public ActionResult<string> Post([FromBody]string value)
        {
            using (var reader = new StreamReader(Request.Body))
            {
                var body = reader.ReadToEnd();
                JObject orderList = JObject.Parse(body);
                string id = orderList["id"].ToString();
                string Name = orderList["name"].ToString();
                string Value = orderList["value"].ToString();
                string filename = "Data//" + id + ".json";
                if (System.IO.File.Exists(filename))
                    return NotFound();
                else
                {
                    string strReturn = "{\"id\":\"" + id + "\",\"name\":\"" +
                    Name + "\",\"value\":\"" +
                    Value + "\"}";
                    System.IO.File.WriteAllText(filename, strReturn);
                    return strReturn;
                }

            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public ActionResult<string> Put(int id, [FromBody]string value)
        {
            using (var reader = new StreamReader(Request.Body))
            {
                var body = reader.ReadToEnd();
                JObject orderList = JObject.Parse(body);
                string Name = orderList["name"].ToString();
                string Value = orderList["value"].ToString();
                string filename = "Data//" + id + ".json";
                if (System.IO.File.Exists(filename))
                {
                    string strReturn = "{\"id\":\"" + id + "\",\"name\":\"" +
                    Name + "\",\"value\":\"" +
                    Value + "\"}";
                    System.IO.File.WriteAllText(filename, strReturn);
                    return strReturn;
                }
                else
                    return NotFound();

            }
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public ActionResult<string> Delete(int id)
        {
            string filename = "Data//" + id + ".json";
            if (System.IO.File.Exists(filename))
            {
                System.IO.File.Delete(filename);
                return id.ToString();
            }
            else
                return NotFound();
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
