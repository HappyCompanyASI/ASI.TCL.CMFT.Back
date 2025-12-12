using ASI.TCL.CMFT.Application;
using ASI.TCL.CMFT.Application.SYS;
using ASI.TCL.CMFT.Domain.SYS;
using ASI.TCL.CMFT.Messages.SYS;
using ASI.TCL.CMFT.WebAPI.RequestPipeline;
using ASI.TCL.CMFT.WebAPI.Swagger;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASI.TCL.CMFT.WebAPI.Controllers.SYS
{
    [ApiController]
    [Route("api/role")]
    [SwaggerGroup(SwaggerGroupKind.SYS)]
    public class SYSQueryApi(IQueryService connection, ILogger<SYSQueryApi> logger) : ControllerBase
    {
        //----------------------------------------------------------
        // 讀取所有角色
        //----------------------------------------------------------
        [Authorize(Policy = nameof(AuthPremission.SYSSetting))]
        [HttpGet("getAllRoles", Order = 1)]
        public async Task<IActionResult> Get([FromQuery] QueryModels.GetAllRoles request)
        {
            return await RequestHandler.HandleQuery(() => connection.Query(request), logger);
        }

        //----------------------------------------------------------
        // 讀取所有角色(分頁)
        //----------------------------------------------------------
        [Authorize(Policy = nameof(AuthPremission.SYSSetting))]
        [HttpGet("getPagedRoles", Order = 2)]
        public async Task<IActionResult> Get([FromQuery] QueryModels.GetPagedRoles request)
        {
            return await RequestHandler.HandleQuery(() => connection.Query(request), logger);
        }

        //----------------------------------------------------------
        // 讀取使用者
        //----------------------------------------------------------
        [HttpGet("getRole", Order = 3)]
        [Authorize(Policy = nameof(AuthPremission.SYSSetting))]
        public async Task<IActionResult> Get([FromQuery] QueryModels.GetUser request)
            => await RequestHandler.HandleQuery(() => connection.Query(request), logger);


        //----------------------------------------------------------
        // 讀取所有使用者
        //----------------------------------------------------------
        [Authorize(Policy = nameof(AuthPremission.SYSSetting))]
        [HttpGet("getAllUsers", Order = 1)]
        public async Task<IActionResult> Get([FromQuery] QueryModels.GetAllUsers request)
        {
            return await RequestHandler.HandleQuery(() => connection.Query(request), logger);
        }
    }
}