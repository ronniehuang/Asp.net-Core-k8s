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
        public ActionResult<string> Get()
        {
            CommonDb db = new CommonDb();
            DataSet ds = new DataSet();
            db.GetDataSet("select * from tNameList", out ds);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                string returnJson = "{\"total\":" + ds.Tables[0].Rows.Count.ToString() + ",\"data\":" + JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented) + "}";
                return Ok(returnJson);
            }
            else
                return NotFound();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            CommonDb db = new CommonDb();
            DataSet ds = new DataSet();
            db.GetDataSet("select top 1 * from tNameList where id="+id.ToString(), out ds);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 )
            {
                string returnJson =  JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented) ;
                return Ok(returnJson);
            }
            else
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
                string Name = orderList["name"].ToString();
                string Value = orderList["value"].ToString();
                CommonDb db = new CommonDb();
                DataSet ds = new DataSet();
                db.GetDataSet("select top 1 * from tNameList where name='"+ Name + "'", out ds);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return NotFound();
                }
                db.ExecSql("insert into tNameList (name,value) values('"+ Name + "','"+ Value + "')");
                return Created("","");
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
                CommonDb db = new CommonDb();
                DataSet ds = new DataSet();
                db.GetDataSet("select top 1 * from tNameList where id='" + id.ToString() + "'", out ds);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return NotFound();
                }
                db.GetDataSet("select top 1 * from tNameList where name='" + Name + "' and id<>"+id.ToString(), out ds);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return NotFound();
                }
                db.ExecSql("update tNameList set name='" + Name + "',value='" + Value + "' where id="+id.ToString());
                return Ok(id.ToString());
            }
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public ActionResult<string> Delete(int id)
        {
            CommonDb db = new CommonDb();
            DataSet ds = new DataSet();
            db.GetDataSet("select top 1 * from tNameList where id='" + id.ToString() + "'", out ds);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return NotFound();
            }
            
            db.ExecSql("delete from tNameList  where id=" + id.ToString());
            return Ok(id.ToString());
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
