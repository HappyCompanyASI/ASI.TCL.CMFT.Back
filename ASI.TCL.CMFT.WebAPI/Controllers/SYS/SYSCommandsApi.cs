using ASI.TCL.CMFT.Application.Auth;
using ASI.TCL.CMFT.Domain.SYS;
using ASI.TCL.CMFT.Messages.SYS;
using ASI.TCL.CMFT.WebAPI.RequestPipeline;
using ASI.TCL.CMFT.WebAPI.Swagger;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASI.TCL.CMFT.WebAPI.Controllers.SYS
{
    [ApiController]
    [Route("api/sys")]
    [SwaggerGroup(SwaggerGroupKind.SYS)]
    public class SYSCommandsApi(ApplicationService applicationService, ILogger<SYSCommandsApi> logger) 
        : ControllerBase
    {
        //----------------------------------------------------------
        // 新增角色
        //----------------------------------------------------------
        [Authorize(Policy = nameof(AuthPremission.SYSSetting))]
        [Route("createRole", Order = 4)]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Commands.CreateRole request)
        {
            return await RequestHandler.HandleCommand(request, applicationService.Handle, logger);
        }

        //----------------------------------------------------------
        // 編輯角色
        //----------------------------------------------------------
        [Authorize(Policy = nameof(AuthPremission.SYSSetting))]
        [Route("updateRole", Order = 5)]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Commands.UpdateRole request)
        {
            return await RequestHandler.HandleCommand(request, applicationService.Handle, logger);
        }

        //----------------------------------------------------------
        // 刪除角色
        //----------------------------------------------------------
        [Authorize(Policy = nameof(AuthPremission.SYSSetting))]
        [Route("deleteRole", Order = 6)]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Commands.DeleteRole request)
        {
            return await RequestHandler.HandleCommand(request, applicationService.Handle, logger);
        }


        //----------------------------------------------------------
        // 新增使用者
        //----------------------------------------------------------
        [Authorize(Policy = nameof(AuthPremission.SYSSetting))]
        [Route("createUser", Order = 2)]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Commands.CreateUser request)
            => await RequestHandler.HandleCommand(request, applicationService.Handle, logger);


        //----------------------------------------------------------
        // 刪除使用者
        //----------------------------------------------------------
        [Authorize(Policy = nameof(AuthPremission.SYSSetting))]
        [Route("deleteUser", Order = 3)]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Commands.DeleteUser request)
            => await RequestHandler.HandleCommand(request, applicationService.Handle, logger);
    }
}
