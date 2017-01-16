using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using TlcApiService.Providers;
using TlcDataAccess;

namespace TlcApiService.Controllers
{
    [BasicAuthorization]
    public class ClientsController : ApiController
    {
        public HttpResponseMessage Get()
        {
            //string username = Thread.CurrentPrincipal.Identity.Name;

            using (TLC_DBEntities entities = new TLC_DBEntities())
            {
                return Request.CreateResponse(HttpStatusCode.OK, entities.tbLookClients.ToList());
            }
        }

        public HttpResponseMessage Get(int id)
        {
            using (TLC_DBEntities entities = new TLC_DBEntities())
            {
                var entity = entities.tbLookClients.FirstOrDefault(row => row.kLookClient == id);

                if (entity != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Not found with key " + id.ToString());
                }
            }
        }
        
    }
}
