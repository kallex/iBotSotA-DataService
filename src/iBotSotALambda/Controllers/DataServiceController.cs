using System;
using System.Threading.Tasks;
using DataServiceCore;
using DryIoc;
using Microsoft.AspNetCore.Mvc;

namespace iBotSotALambda.Controllers
{
    [Route("api/[controller]/[action]")]
    public class DataServiceController : ControllerBase
    {
        public DataServiceController()
        {
            var rules = DryIoc.Rules.Default
                .With(DryIoc.FactoryMethod.ConstructorWithResolvableArguments)
                .WithFactorySelector(DryIoc.Rules.SelectLastRegisteredFactory())
                .WithTrackingDisposableTransients();

            this.Container = new DryIoc.Container(rules);
            Container.Register<ISteamService, SteamService.SteamService>(Reuse.Singleton);
        }

        public Container Container { get; set; }

        [HttpPost]
        public void SubmitMatchData([FromBody] byte[] matchData)
        {
            //var container = new Container();
            //container.Register<ISteamService, SteamService.SteamService>(Reuse.Singleton);
            //var steamService = container.Resolve<ISteamService>();

            //steamService.InitService();
            //var authToken = steamService.GetAuthTokenA();
        }

        [HttpGet]
        public async Task<ActionResult> AuthTest([FromQuery] ulong steamIDValue, [FromQuery] string authDataHex)
        {
            string statusMessage = "";
            bool authenticated = false;
            try
            {
                var steamService = Container.Resolve<ISteamService>();
                var authData = authDataHex.ToBytes();
                authenticated = await steamService.ValidateAuthToken(steamIDValue, authData);
                statusMessage = "OK";
            }
            catch (Exception ex)
            {
                statusMessage = ex.ToString();
            }

            return new JsonResult(new
            {
                statusMessage = statusMessage,
                authenticated = authenticated
            });
        }
    }
}