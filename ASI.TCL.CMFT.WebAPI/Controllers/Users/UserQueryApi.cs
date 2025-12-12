using System.Data.Common;
using ASI.TCL.CMFT.Application;
using ASI.TCL.CMFT.Application.SYS;
using ASI.TCL.CMFT.Messages.SYS;
using ASI.TCL.CMFT.WebAPI.RequestPipeline;
using ASI.TCL.CMFT.WebAPI.Swagger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace ASI.TCL.CMFT.WebAPI.Controllers.Users
{
    [ApiController]
    [Route("api/user")]
    [SwaggerGroup(SwaggerGroupKind.User)]
    public class UserQueryApi(IQueryService connection, ILogger<UserQueryApi> logger) : ControllerBase
    {
        //----------------------------------------------------------
        // 讀取所有使用者
        //----------------------------------------------------------
        [Authorize(Policy = PermissionKey.CanViewUser)]
        [HttpGet("getAllUsers",Order = 1)]
        public async Task<IActionResult> Get([FromQuery] QueryModels.GetAllUsers request)
        {
            return await RequestHandler.HandleQuery(() => connection.Query(request), logger);
        }

        
    }
}