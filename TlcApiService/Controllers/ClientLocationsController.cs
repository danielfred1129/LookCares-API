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
    [RoutePrefix("api/ClientLocations")]
    public class ClientLocationsController : ApiController
    {
        [Route("ByClientKey/{id}")]
        public HttpResponseMessage GetByClientKey(int id = -1)
        {
            using (TLC_DBEntities entities = new TLC_DBEntities())
            {
                if(id > -1)
                    return Request.CreateResponse(HttpStatusCode.OK, entities.tbLookClientCustomers.Where(row => row.kLookClient == id).ToList());
                else
                    return Request.CreateResponse(HttpStatusCode.OK, entities.tbLookClientCustomers.ToList());
            }
        }

        [Route("{id}")]
        public HttpResponseMessage Get(int id)
        {
            using (TLC_DBEntities entities = new TLC_DBEntities())
            {
                var entity = entities.tbLookClientCustomers.FirstOrDefault(row => row.kLookClientCustomer == id);

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
