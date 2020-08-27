﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Text;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
            return Ok("{\"total\":\""+files.Length.ToString()+"\""+ ",\"data\":[" + jsonString + "]}");
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            string filename = "Data/" + id + ".json";
            if (System.IO.File.Exists(filename))
                return Ok("{\"id\":\"" + id + "\",\"name\":\"" +
                    ReadJsonConfig(filename, "name") + "\",\"value\":\"" +
                    ReadJsonConfig(filename, "value") + "\"}");
            return NotFound();
        }
        
        // POST api/values
        [HttpPost]
        public async Task<ActionResult<string>> Post([FromForm] string value)
        {
            using (var reader = new StreamReader(Request.Body))
            {
                string body = await reader.ReadToEndAsync(); 
                JObject orderList = JObject.Parse(body);
                string id = orderList["id"].ToString();
                string Name = orderList["name"].ToString();
                string Value = orderList["value"].ToString();
                string filename = "Data/" + id + ".json";
                if (int.Parse(id)>=0)
                    return NotFound();
                if (System.IO.File.Exists(filename))
                    return NotFound();
                else
                {
                    string strReturn = "{\"id\":\"" + id + "\",\"name\":\"" +
                    Name + "\",\"value\":\"" +
                    Value + "\"}";
                    System.IO.File.WriteAllText(filename, strReturn);
                    return Created("",id);
                }

            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<ActionResult<string>> Put(int id, [FromForm]string value)
        {
            using (var reader = new StreamReader(Request.Body))
            {
                string body = await reader.ReadToEndAsync();
                JObject orderList = JObject.Parse(body);
                string Name = orderList["name"].ToString();
                string Value = orderList["value"].ToString();
                string filename = "Data/" + id + ".json";
                if (System.IO.File.Exists(filename))
                {
                    string strReturn = "{\"id\":\"" + id + "\",\"name\":\"" +
                    Name + "\",\"value\":\"" +
                    Value + "\"}";
                    System.IO.File.WriteAllText(filename, strReturn);
                    return Ok(strReturn);
                }
                else
                    return NotFound();

            }
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<string>> Delete(int id)
        {
            string filename = "Data/" + id + ".json";
            if (System.IO.File.Exists(filename))
            {
                System.IO.File.Delete(filename);
                return Ok(id.ToString());
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
