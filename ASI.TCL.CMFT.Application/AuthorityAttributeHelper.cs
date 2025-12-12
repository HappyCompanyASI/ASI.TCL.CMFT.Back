using System.Reflection;

namespace ASI.TCL.CMFT.Application
{
    public static class AuthorityAttributeHelper
    {
        public static AuthorityAttribute? GetAuthorityAttribute(Type? type)
        {
            while (type != null)
            {
                var attr = type.GetCustomAttribute<AuthorityAttribute>();
                if (attr != null)
                    return attr;

                type = type.DeclaringType; //  取得外層 class（nested class ）
            }

            return null;
        }
    }
}