using GrainInterfaces;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using WebApi.Service;

namespace WebApi.Controllers
{
    [RoutePrefix("")]
    public class MailController : ApiController
    {
        private IClientService _clientService;
        private const string domainName = "nomnio.com";

        public MailController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [Route("")]
        [HttpGet]
        public HttpResponseMessage Index()
        {
            var response = new HttpResponseMessage();
            response.Content = new StringContent("<h1>Nomnio.com email checker</h1><ul><li>Omogoca poizvedovanja po e-postnih naslovih<ul><li>Zahteva: GET https://my.site/matej@nomnio.com</li><li>Odgovor: NotFound ali OK</li></ul></li><li>Omogoca dodajanje e-postnih naslovov<ul><li>Zahteva: Create https://my.site/matej@nomnio.com</li><li>Odgovor: Created ali Conflict</li></ul></li></ul>");
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            return response;
        }

        [Route(@"{mail:regex(^([\w\.\-]+)@([\w\-]+)((\.(\w){2,})+)$)}")]
        [HttpGet]
        public IHttpActionResult GetMail(string mail)
        {
            var domainGrain = _clientService.GetClient().GetGrain<IDomainGrain>(domainName);
            
            bool mailExists = domainGrain.MailExists(mail).Result;
            if (!mailExists)
            {
                return NotFound();
            }

            return Ok("exists");
        }

        [Route(@"{mail:regex(^([\w\.\-]+)@([\w\-]+)((\.(\w){2,})+)$)}")]
        [HttpPut]
        public IHttpActionResult CreateMail(string mail)
        {
            var domainGrain = _clientService.GetClient().GetGrain<IDomainGrain>(domainName);
            
            bool mailExists = domainGrain.MailExists(mail).Result;
            if (mailExists)
            {
                return Conflict();
            }

            domainGrain.AddMail(mail);
            return Ok(mail);
        }
    }
}
