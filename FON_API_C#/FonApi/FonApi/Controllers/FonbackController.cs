using FonApi.DbConnection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using FonApi.Models;
using System.Text;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Newtonsoft.Json.Linq;
using FonApi.Kafka;
using FonApi.Services;
using Confluent.Kafka;
using System.ComponentModel.Design;

namespace FonApi.Controllers
{
    [ApiController]
    //[Route("[controller]")]
    public class FonbackController : ControllerBase
    {
        private IConfiguration _configuration;
        private readonly ChatService _chatService;

        public FonbackController(IConfiguration configuration, ChatService chatService)
        {
            _configuration = configuration;
            _chatService = chatService;
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
            var db = new DbApplication(_configuration);

            int nuevoId = -1;
            nuevoId = db.AddApp(app);

            var confluent = new Kafka.Confluent();
            confluent.CreateMessage(nuevoId.ToString(), app.Description);

            _ = ReadMessage(nuevoId);

            if (nuevoId > 0)
                return Ok(new { result = "Accepted" });
            else
                return Ok(new { result = "ERROR: Not Accepted" });
        }

        /// ///////////////////////////////////////////////////////////////////////////////////////////////////////


        private async Task ReadMessage(int IdApp)
        {
            var confluent = new Kafka.Confluent();

            string _red = confluent.ReadMessages();
            if (_red == "OK_READ")
            {
                int res = 0;
                string message;

                var db = new DbApplication(_configuration);
                res = db.AppToProcessed(IdApp); //  changes to "processed"

                if (res > 0)
                    message = "Accepted";
                //return Ok(new { result = "Accepted" });
                else
                    message = "Not Accepted";
                    //return Ok(new { result = "Not Accepted" });

                string jsonstring = System.Text.Json.JsonSerializer.Serialize(new { type = "[Kafka] Add message", message = message });
                await _chatService.SendMessage(jsonstring);
            }
        }

    }


}
