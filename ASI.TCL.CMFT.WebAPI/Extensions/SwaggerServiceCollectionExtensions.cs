using System.Reflection;
using ASI.TCL.CMFT.WebAPI.Swagger;
using Microsoft.OpenApi.Models;

namespace ASI.TCL.CMFT.WebAPI.Extensions
{
    internal static class SwaggerServiceCollectionExtensions
    {
        public static IServiceCollection AddTsaLamsSwagger(this IServiceCollection services)
        {
            // 註冊「Swagger」
            services.AddSwaggerGen(c =>
            {
                //------------------------------------
                // ★ 基本資訊
                //------------------------------------
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "ASI.TCL.CMFT.WebAPI",
                    Description = "環狀線 CMFT API",
                });

                //------------------------------------
                // ★ Token 認證設定
                //------------------------------------
                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "在這裡輸入 JWT。格式：Bearer {token}",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };

                c.AddSecurityDefinition("Bearer", securityScheme);

                var securityRequirement = new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                };
                c.AddSecurityRequirement(securityRequirement);

                //------------------------------------
                // ★ GroupName 設定及排序（用 enum）
                //------------------------------------
                c.TagActionsBy(api =>
                {
                    // 讀取 Controller 上的 [SwaggerGroup(...)]
                    var attr = api.ActionDescriptor.EndpointMetadata
                        .OfType<SwaggerGroupAttribute>()
                        .FirstOrDefault();

                    if (attr != null)
                    {
                        // 直接用 enum 名稱當 Tag
                        return new[] { attr.Kind.ToString() };   // ex: "UserCommands"
                    }

                    // 沒標 Attribute 就退回 Controller 名稱
                    var controllerName = api.ActionDescriptor.RouteValues["controller"];
                    if (string.IsNullOrEmpty(controllerName))
                    {
                        controllerName = "Default";
                    }

                    return new[] { controllerName };
                });

                // 不過濾，全部放進 v1 這份 spec
                c.DocInclusionPredicate((docName, apiDesc) => true);

                // 排序：先 enum 值，再 Action 的 Order，再路徑
                c.OrderActionsBy(api =>
                {
                    var attr = api.ActionDescriptor.EndpointMetadata
                        .OfType<SwaggerGroupAttribute>()
                        .FirstOrDefault();

                    // 群組排序：用 enum 的 int 值，沒標就丟很後面
                    var groupOrder = attr != null ? (int)attr.Kind : 1000;
                    var groupName = attr != null
                        ? attr.Kind.ToString()
                        : api.ActionDescriptor.RouteValues["controller"] ?? "Default";

                    // Action 排序：用 [HttpPost("xxx", Order = n)]
                    var routeOrder = api.ActionDescriptor.AttributeRouteInfo?.Order ?? 0;

                    return $"{groupOrder:D3}_{routeOrder:D3}_{api.RelativePath}";
                });

                //------------------------------------
                // ★ 產生 XML 註解檔案給 Swagger 使用
                //------------------------------------
                var xmlFile = Assembly.GetExecutingAssembly().GetName().Name + ".xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    c.IncludeXmlComments(xmlPath);
                }
            });
            return services;
        }
    }
}
 