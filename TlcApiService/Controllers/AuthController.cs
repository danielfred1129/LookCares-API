using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using TlcApiService.Models;
using TlcDataAccess;

namespace TlcApiService.Controllers
{
    [RoutePrefix("api/Auth")]
    public class AuthController : ApiController
    {
        // POST api/Auth/Login
        [Route("Login")]
        [HttpPost]
        public HttpResponseMessage Login([FromBody]LoginBindingModel loginModel)
        {
            try
            {
                using (TLC_DBEntities entities = new TLC_DBEntities())
                {
                    tbUser tbUserModel = entities.tbUsers.Where(user => user.vcUserName == loginModel.UserName && user.vcPassword == loginModel.Password).FirstOrDefault();

                    if (tbUserModel != null)
                    {
                        string baseToken = loginModel.UserName + ':' + loginModel.Password;
                        string encodedToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(baseToken));

                        UserBindingModel userModel = new UserBindingModel
                        {
                            Token = encodedToken,
                            UserKey = tbUserModel.kUser.ToString(),
                            UserName = tbUserModel.vcUserName.ToString(),
                            ClientKey = tbUserModel.kLookClient.ToString()
                        };

                        return Request.CreateResponse(HttpStatusCode.OK, userModel);
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Invalid username or password");
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
