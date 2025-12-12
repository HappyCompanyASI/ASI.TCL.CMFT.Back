namespace ASI.TCL.CMFT.Infrastructure.JWTAuthentication
{
    public class JWTSettings
    {
        public string Secret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public double ExpirationMinutes { get; set; } // JWT Token 過期時間 (分鐘)
    }
}