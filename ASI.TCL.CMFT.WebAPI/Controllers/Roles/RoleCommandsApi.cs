using ASI.TCL.CMFT.Application.Auth;
using ASI.TCL.CMFT.Messages.SYS;
using ASI.TCL.CMFT.WebAPI.RequestPipeline;
using ASI.TCL.CMFT.WebAPI.Swagger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASI.TCL.CMFT.WebAPI.Controllers.Roles
{
    [ApiController]
    [Route("api/role")]
    [SwaggerGroup(SwaggerGroupKind.Role)]
    public class RoleCommandsApi(
        ApplicationService applicationService,
        ILogger<RoleCommandsApi> logger)
        : ControllerBase
    {
        //----------------------------------------------------------
        // 新增角色
        //----------------------------------------------------------
        [Authorize(Policy = PermissionKey.CanUpdateRole)]
        [Route("create", Order = 4)]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Commands. request)
        {
            return await RequestHandler.HandleCommand(request, applicationService.Handle, logger);
        }

        //----------------------------------------------------------
        // 編輯角色
        //----------------------------------------------------------
        [Authorize(Policy = PermissionKey.CanUpdateRole)]
        [Route("update", Order = 5)]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Commands.request)
        {
            return await RequestHandler.HandleCommand(request, applicationService.Handle, logger);
        }

        //----------------------------------------------------------
        // 刪除角色
        //----------------------------------------------------------
        [Authorize(Policy = PermissionKey.CanUpdateRole)]
        [Route("delete", Order = 6)]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Commands.V1.DeleteRole request)
        {
            return await RequestHandler.HandleCommand(request, applicationService.Handle, logger);
        }
    }
}
