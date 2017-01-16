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
    public class StoreLocationsController : ApiController
    {
        public HttpResponseMessage Get()
        {
            using (TLC_DBEntities entities = new TLC_DBEntities())
            {
                return Request.CreateResponse(HttpStatusCode.OK, entities.tbInStoreLocations.ToList());
            }
        }

        public HttpResponseMessage Get(int id)
        {
            using (TLC_DBEntities entities = new TLC_DBEntities())
            {
                var entity = entities.tbInStoreLocations.FirstOrDefault(row => row.kInStoreLocation == id);

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
