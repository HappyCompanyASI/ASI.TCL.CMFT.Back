using ASI.TCL.CMFT.Application;
using ASI.TCL.CMFT.Application.SYS;
using ASI.TCL.CMFT.Messages.SYS;
using ASI.TCL.CMFT.WebAPI.RequestPipeline;
using ASI.TCL.CMFT.WebAPI.Swagger;
 
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASI.TCL.CMFT.WebAPI.Controllers.Role
{
    [ApiController]
    [Route("api/role")]
    [SwaggerGroup(SwaggerGroupKind.Role)]
    public class RoleQueryApi(IQueryService connection, ILogger<RoleQueryApi> logger) : ControllerBase
    {
        //----------------------------------------------------------
        // 讀取所有使用者
        //----------------------------------------------------------
        [Authorize(Policy = PermissionKey.CanViewRole)]
        [HttpGet("getAllRoles", Order = 1)]
        public async Task<IActionResult> Get([FromQuery] QueryModels.GetAllRoles request)
        {
            return await RequestHandler.HandleQuery(() => connection.Query(request), logger);
        }

        //----------------------------------------------------------
        // 讀取所有使用者2
        //----------------------------------------------------------
        [Authorize(Policy = PermissionKey.CanViewRole)]
        [HttpGet("getPagedRoles", Order = 2)]
        public async Task<IActionResult> Get([FromQuery] QueryModels.GetPagedRoles request)
        {
            
            return await RequestHandler.HandleQuery(() => connection.Query(request), logger);
        }

        //----------------------------------------------------------
        // 讀取使用者
        //----------------------------------------------------------
        [HttpGet("getRole", Order = 3)]
        [Authorize(Policy = PermissionKey.CanViewRole)]
        public async Task<IActionResult> Get([FromQuery] QueryModels.GetRole request)
            => await RequestHandler.HandleQuery(() => connection.Query(request), logger);


        
    }
}