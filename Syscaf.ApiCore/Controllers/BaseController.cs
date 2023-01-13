using Microsoft.AspNetCore.Mvc;

namespace Syscaf.ApiCore.Controllers
{
    public class BaseController : ControllerBase
    {
        public string UserId { get 
            {
                return this.User.Claims.First(i => i.Type == "Id").Value;
            } }
        
    
    }
}
