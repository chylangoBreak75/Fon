using FonApi.DbConnection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using FonApi.Models;
using System.Text;

namespace FonApi.Controllers
{
    [ApiController]
    //[Route("[controller]")]
    public class FonbackController : ControllerBase
    {
        private IConfiguration _configuration;

        public FonbackController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        [Route("login")]
        public ActionResult Login(User user)
        {

            var db = new DbApplication(_configuration);
            var res = db.VerifyUser(user.Name, user.Pwd);
            
            if(res)
                return Ok(new { result= "Accepted" });
            else
                return Ok(new { result= "Not Accepted" });


        }

        [HttpGet]
        [Route("ListarApps")]
        public dynamic ListarTareas()
        {
            var db = new DbApplication(_configuration);
            var apps = db.GetApplications();
            return Newtonsoft.Json.JsonConvert.SerializeObject(apps);
        }

        [HttpGet]
        [Route("GetTypesApps")]
        public dynamic ListarTypesApps()
        {
            var db = new DbApplication(_configuration);
            var typeApps = db.GetTypesApps();
            return Newtonsoft.Json.JsonConvert.SerializeObject(typeApps);
        }

        [HttpGet]
        [Route("GetStatusApps")]
        public dynamic ListarStatusApps()
        {
            var db = new DbApplication(_configuration);
            var StatusApps = db.GetStatusApps();
            return Newtonsoft.Json.JsonConvert.SerializeObject(StatusApps);
        }

        [HttpPost]
        [Route("AddApp")]
        public ActionResult AddApp(App app)
        {
            int res = 0;

            var db = new DbApplication(_configuration);
            res = db.AddApp(app);

            if (res > 0)
                return Ok(new { result = "Accepted" });
            else
                return Ok(new { result = "Not Accepted" });
        }

        /// ///////////////////////////////////////////////////////////////////////////////////////////////////////

    }


}
