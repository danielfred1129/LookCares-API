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
    [RoutePrefix("api/Values")]
    public class ValuesController : ApiController
    {
        public HttpResponseMessage Get(string filter = "All")
        {
            string username = Thread.CurrentPrincipal.Identity.Name;

            using (TLC_DBEntities entities = new TLC_DBEntities())
            {
                switch (filter.ToLower())
                {
                    case "all":
                        return Request.CreateResponse(HttpStatusCode.OK, entities.tbLookClients.ToList());
                    case "male":
                        return Request.CreateResponse(HttpStatusCode.OK,
                            entities.tbLookClients.Where(row => row.vcClientName.ToLower() == "male").ToList());
                    default:
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Value for " + filter);
                }

            }
        }

        [Route("Locations")]
        public HttpResponseMessage GetLocations()
        {
            string username = Thread.CurrentPrincipal.Identity.Name;

            using (TLC_DBEntities entities = new TLC_DBEntities())
            {
                return Request.CreateResponse(HttpStatusCode.OK, entities.tbLookClientCustomers.ToList());
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
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Artwork not found with key " + id.ToString());
                }
            }
        }

        public HttpResponseMessage Post([FromBody]tbLookClient client)
        {
            try
            {
                using (TLC_DBEntities entities = new TLC_DBEntities())
                {
                    entities.tbLookClients.Add(client);
                    entities.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.Created, client);
                    message.Headers.Location = new Uri(Request.RequestUri + client.kLookClient.ToString());
                    return message;
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        public HttpResponseMessage Delete(int id)
        {
            try
            {
                using (TLC_DBEntities entities = new TLC_DBEntities())
                {
                    var entity = entities.tbLookClients.FirstOrDefault(row => row.kLookClient == id);

                    if (entity != null)
                    {
                        entities.tbLookClients.Remove(entity);
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Artwork not found with key " + id.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        public HttpResponseMessage Put(int id, [FromBody]tbLookClient client)
        {
            try
            {
                using (TLC_DBEntities entities = new TLC_DBEntities())
                {
                    var entity = entities.tbLookClients.FirstOrDefault(row => row.kLookClient == id);

                    if (entity != null)
                    {
                        entity.vcClientName = client.vcClientName;
                        entity.kLookClient = client.kLookClient;
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Artwork not found with key " + id.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
