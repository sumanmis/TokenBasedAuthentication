using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Security.Claims;
using System.IO;
using System.Web.Script.Serialization;
using Microsoft.Owin.Security.OAuth;
using System.Web;
using System.Collections;
using System.Text;
using Newtonsoft.Json.Linq;
using AuthenticationAPP.Models;
using System.Net.Http.Headers;

namespace AuthenticationAPP.Controllers
{
    public class DataController : ApiController
    {

        public const string WebApiNewsEndPoint = "https://api.harmony.epsilon.com/v1/contentBlocks";
        [AllowAnonymous]
        [HttpGet]
        [Route("api/data/foralluser")]
        public IHttpActionResult Get()
        {
            return Ok("Now Server time is:" + DateTime.Now.ToString());
        }

        [Authorize]
        [HttpGet]
        [Route("api/data/authenticate")]
        public IHttpActionResult GetForAllAuthenticate()
        {
            var identity = (ClaimsIdentity)User.Identity;
            return Ok("Hello" + identity.Name);
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        [Route("api/data/authorize")]
        public IHttpActionResult GetForAdmin()
        {
            var identity = (ClaimsIdentity)User.Identity;
            var roles = identity.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            return Ok("Hello" + identity.Name + "Role " + string.Join(",", (roles.ToList())));
        }
        /// <summary>
        /// As of now post string
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "admin")]
        [HttpPost]
        [Route("api/data/CallApi")]
        public IHttpActionResult CallApi()
        {

            
           
            WebRequest request = WebRequest.Create(WebApiNewsEndPoint);
            request.Method = "POST";
            if (HttpRuntime.Cache["TOKEN"] != null)
            {
                request.Headers.Add("Authorization", "Bearer " + HttpRuntime.Cache.Get("TOKEN"));
            }

            string postData = "This is a test that posts this string to a Web server.";
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            WebResponse response = request.GetResponse();
            dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            reader.Close();
            dataStream.Close();
            response.Close();
            return Ok(response);
        }
      
        

    }
}
