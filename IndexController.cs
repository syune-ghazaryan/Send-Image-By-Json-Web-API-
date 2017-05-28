using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace SendImageByJson.Controllers
{
    public class IndexController : ApiController
    {
      
        [HttpPost]
        [Route("api/Index/DataSender")]
        public HttpResponseMessage DataSender(string name, [FromBody]Data image)
        {
            return Request.CreateResponse(HttpStatusCode.OK,
              new
              {
                  Name = image.Name,
                  Image= image.Image
            });
        }
    }

}

