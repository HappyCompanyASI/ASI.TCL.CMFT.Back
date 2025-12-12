namespace ASI.TCL.CMFT.WebAPI.Swagger
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class SwaggerGroupAttribute(SwaggerGroupKind kind) : Attribute
    {
        public SwaggerGroupKind Kind { get; private set; } = kind;
    }
}