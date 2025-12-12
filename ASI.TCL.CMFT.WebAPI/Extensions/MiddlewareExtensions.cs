namespace ASI.TCL.CMFT.WebAPI.Extensions
{
    internal static class MiddlewareExtensions
    {
        public static void UseMiddlewares(this WebApplication app)
        {
            // ------------------------------------------------------
            // 例外處理
            // ------------------------------------------------------
            if (app.Environment.IsDevelopment())
            {
                // 開發環境：直接顯示詳細錯誤頁
                http://localhost:5278/swagger/v1/swagger.json
                app.UseDeveloperExceptionPage();
                app.UseExceptionHandler();
            }
            else
            {
                // 正式 / 測試環境：用全域例外處理 + 強制 https
                app.UseExceptionHandler();
                app.UseHttpsRedirection();
            }

            // ------------------------------------------------------
            // Swagger：無論環境一律啟用
            // ------------------------------------------------------
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ASI.TCL.CMFT.WebAPI v1");
                c.ConfigObject.AdditionalItems["persistAuthorization"] = true;
            });

            // ------------------------------------------------------
            // Routing / CORS / Auth
            // ------------------------------------------------------
            app.UseRouting();

            app.UseCors("AllowVite5174");

            app.UseAuthentication();
            app.UseAuthorization();

            // ------------------------------------------------------
            // 診斷用 Middleware：看目前 HttpContext.User 狀態
            // ------------------------------------------------------
            app.Use(async (context, next) =>
            {
                Console.WriteLine("WebAPI Middleware 觸發：" + context.Request.Path);

                var user = context.User;
                var identity = user.Identity;

                var isAuthenticated = identity is { IsAuthenticated: true };
                var name = identity != null ? identity.Name : "null";

                Console.WriteLine("User.Identity.IsAuthenticated: " + isAuthenticated);
                Console.WriteLine("User.Identity.Name: " + name);

                var claims = string.Join(", ", user.Claims.Select(c => c.Type + ": " + c.Value));
                Console.WriteLine("User.Claims: " + claims);

                await next();
            });

            // ------------------------------------------------------
            // Endpoint 映射
            // ------------------------------------------------------
            app.MapControllers();
        }
    }
}