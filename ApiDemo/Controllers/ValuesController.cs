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

namespace ApiDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        public async Task<ActionResult<string>> Get()
        {
            await WaitAndApologizeAsync();
            CommonDb db = new CommonDb();
            DataSet ds = new DataSet();
            db.spApiDemoNameList(1,0,"","", out ds);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                string returnJson = "{\"total\":" + ds.Tables[0].Rows.Count.ToString() + ",\"data\":" + JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented) + "}";
                db.CloseDatabase();
                return Ok(returnJson);
            }
            db.CloseDatabase();
            return NotFound();
        }
        // GET api/values/heartbeat
        [HttpGet("heartbeat")]
        public async Task<ActionResult<string>> heartBeat()
        {
            await WaitAndApologizeAsync();
            return Ok(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
 
        }
        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<ActionResult<string>> Get(int id)
        {
            await WaitAndApologizeAsync();
            CommonDb db = new CommonDb();
            DataSet ds = new DataSet();
            db.spApiDemoNameList(2,id, "","", out ds);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 )
            {
                DataRow dr = ds.Tables[0].Rows[0];
                string returnJson = "{\"id\": 1,\"name\": \""+ dr["name"].ToString()+ "\",\"value\": \"" + dr["value"].ToString() + "\",\"intime\": \"" + dr["intime"].ToString() + "\"}";
                db.CloseDatabase();
                return Ok(returnJson);
            }
            db.CloseDatabase();
            return NotFound();
        }
        
        // POST api/values
        [HttpPost]
        public async Task<ActionResult<string>> Post([FromForm] string value)
        {
            await WaitAndApologizeAsync();
            using (var reader = new StreamReader(Request.Body))
            {
                string body = await reader.ReadToEndAsync(); 
                JObject orderList = JObject.Parse(body);
                string Name = orderList["name"].ToString();
                string Value = orderList["value"].ToString();
                CommonDb db = new CommonDb();
                DataSet ds = new DataSet();
                db.spApiDemoNameList(3,0, Name , Value , out ds);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    db.CloseDatabase();
                    return Created("","");
                }
                db.CloseDatabase();
                return NotFound();
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<ActionResult<string>> Put(int id, [FromForm]string value)
        {
            await WaitAndApologizeAsync();
            using (var reader = new StreamReader(Request.Body))
            {
                string body = await reader.ReadToEndAsync();
                JObject orderList = JObject.Parse(body);
                string Name = orderList["name"].ToString();
                string Value = orderList["value"].ToString();
                CommonDb db = new CommonDb();
                DataSet ds = new DataSet();
                db.spApiDemoNameList(4,id , Name , Value, out ds);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    db.CloseDatabase();
                    return Ok(id.ToString());
                }
                db.CloseDatabase();
                return NotFound();
            }
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<string>> Delete(int id)
        {
            await WaitAndApologizeAsync();
            using (var reader = new StreamReader(Request.Body))
            {
                CommonDb db = new CommonDb();
                DataSet ds = new DataSet();
                db.spApiDemoNameList(5, id ,"","", out ds);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    db.CloseDatabase();
                    return Ok(id.ToString());
                }
                db.CloseDatabase();
                return NotFound();
            }
        }
        static async Task WaitAndApologizeAsync()
        {
            await Task.Delay(100);
        }

    }
}
