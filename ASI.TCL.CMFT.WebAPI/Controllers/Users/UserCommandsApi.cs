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
    public class UserCommandsApi(ApplicationService applicationService, ILogger<UserCommandsApi> logger)
        : ControllerBase
    {
        //----------------------------------------------------------
        // 新增使用者
        //----------------------------------------------------------
        [Authorize(Policy = PermissionKey.CanViewUser)]
        [Route("create",Order = 3)]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Commands.CreateUser request)
            => await RequestHandler.HandleCommand(request, applicationService.Handle, logger);

        //----------------------------------------------------------
        // 編輯使用者
        //----------------------------------------------------------
        //[Route("update", Order = 4)]
        //[HttpPost]
        //public async Task<IActionResult> Post([FromBody] Commands. request)
        //    => await RequestHandler.HandleCommand(request, applicationService.Handle, logger);

        //----------------------------------------------------------
        // 刪除使用者
        //----------------------------------------------------------
        [Route("delete", Order = 5)]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Commands.DeleteUser request)
            => await RequestHandler.HandleCommand(request, applicationService.Handle, logger);
    }
}
