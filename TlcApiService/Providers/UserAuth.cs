using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TlcDataAccess;

namespace TlcApiService.Providers
{
    public class UserAuth
    {
        public static bool Login(string username, string password)
        {
            using (TLC_DBEntities entities = new TLC_DBEntities())
            {
                return entities.tbUsers.Any(user => user.vcUserName == username && user.vcPassword == password);
            }
        }

    }
}