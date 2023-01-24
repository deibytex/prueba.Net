using Microsoft.AspNetCore.Mvc;

namespace Syscaf.ApiCore.Controllers
{
    public class BaseController : ControllerBase
    {
        public string UserId { get 
            {
                return this.User.Claims.First(i => i.Type == "Id").Value;
            } }

        public string UserName
        {
            get
            {
                return this.User.Claims.First(i => i.Type == "name").Value;
            }
        }

        public string User_Nombres
        {
            get
            {
                return this.User.Claims.First(i => i.Type == "Nombres").Value;
            }
        }


    }
}
