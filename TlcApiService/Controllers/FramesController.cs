using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using TlcApiService.Models;
using TlcApiService.Providers;
using TlcDataAccess;

namespace TlcApiService.Controllers
{
    //[BasicAuthorization]
    [RoutePrefix("api/Frames")]
    public class FramesController : ApiController
    {
        [Route("{SerialNumber}")]
        public HttpResponseMessage Get(string serialNumber)
        {
            try
            {
                using (TLC_DBEntities entities = new TLC_DBEntities())
                {
                    var entity = entities.tbFrames.FirstOrDefault(row => row.vcSerialNumber == serialNumber);

                    if (entity != null)
                    {
                        FrameBindingModel frameModel = new FrameBindingModel
                        {
                            Frame = entity
                        };

                        if (entity.vcInstalled.ToString() != "Uninstalled")
                        {
                            frameModel.Fabrics = entities.tbFabrics.Where(row => row.kFrame == entity.kFrame).ToList();
                        }

                        return Request.CreateResponse(HttpStatusCode.OK, frameModel);
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Not found with SerialNumber " + serialNumber);
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [Route("Fabric/{SerialNumber}")]
        public HttpResponseMessage GetFabric(string serialNumber)
        {
            try
            {
                using (TLC_DBEntities entities = new TLC_DBEntities())
                {
                    var entity = entities.tbFabrics.FirstOrDefault(row => row.vcSerialNumber == serialNumber);

                    if (entity != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Not found with SerialNumber " + serialNumber);
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [Route("Fabric")]
        [HttpPost]
        public HttpResponseMessage AddFabric([FromBody]FabricBindingModel fabricModel)
        {
            try
            {
                using (TLC_DBEntities entities = new TLC_DBEntities())
                {
                    var frame = entities.tbFrames.FirstOrDefault(row => row.kFrame == fabricModel.FrameKey);
                    var fabric = entities.tbFabrics.FirstOrDefault(row => row.vcSerialNumber == fabricModel.SerialNumber);

                    if(fabric != null)
                    {
                        fabric.kLookClientCustomer = fabricModel.ClientLocationKey;
                        fabric.kFrame = fabricModel.FrameKey;
                        //entities.SaveChanges();

                        frame.vcInstalled = "Installed";
                        frame.vcInStoreLocation = fabricModel.InStoreLocation;
                        entities.SaveChanges();

                        return Request.CreateResponse(HttpStatusCode.OK, fabric);
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Not found with SerialNumber " + fabricModel.SerialNumber);
                    }
                    
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [Route("Fabric/{id}")]
        [HttpDelete]
        public HttpResponseMessage RemoveFabric(int id)
        {
            try
            {
                using (TLC_DBEntities entities = new TLC_DBEntities())
                {
                    var entity = entities.tbFabrics.FirstOrDefault(row => row.kFabric == id);
                    

                    if (entity != null)
                    {
                        var frame = entities.tbFrames.FirstOrDefault(row => row.kFrame == entity.kFrame);
                        var fabrics = entities.tbFabrics.Where(row => row.kFrame == entity.kFrame).ToList();
                        
                        entity.kLookClientCustomer = null;
                        entity.kFrame = null;

                        if (frame != null && fabrics.Count == 1)
                        {
                            frame.vcInstalled = "Uninstalled";
                            frame.vcInStoreLocation = null;
                        }
                        
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Not found with key " + id.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [Route("Upload")]
        [HttpPost]
        public async Task<HttpResponseMessage> PostFormData()
        {
            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string root = HttpContext.Current.Server.MapPath("~/App_Data/uploads");
            var provider = new MultipartFormDataStreamProvider(root);

            try
            {
                // Read the form data.
                await Request.Content.ReadAsMultipartAsync(provider);
                
                string guid = Guid.NewGuid().ToString();

                string fileName = guid + "_" + provider.FileData.First().Headers.ContentDisposition.FileName.Replace("\"", "");

                FileInfo fileInfo = new FileInfo(provider.FileData.First().LocalFileName);

                File.Move(fileInfo.FullName, Path.Combine(root, fileName));

                int kFrame = Int32.Parse(provider.FormData.GetValues("kFrame")[0]);

                using (TLC_DBEntities entities = new TLC_DBEntities())
                {
                    var frame = entities.tbFrames.FirstOrDefault(row => row.kFrame == kFrame);
                    
                    if (frame != null)
                    {
                        frame.vcFrameFileName = fileName;
                        
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, frame);
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Not found with key " + kFrame.ToString());
                    }
                }
            }
            catch (System.Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }
    }
}
