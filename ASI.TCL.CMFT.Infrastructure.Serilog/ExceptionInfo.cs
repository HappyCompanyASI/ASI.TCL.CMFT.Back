

namespace ASI.TCL.CMFT.Infrastructure.Serilog
{
    public record ExceptionInfo(
        DateTimeOffset Time,   // 時間
        string Level,          // ERR / WRN / INF ...
        string SourceContext,  // 例：ASI.TSA.LAMS.Infrastructure.Email.Pop3MessageFetcher
        string ExceptionType,  // 例：System.Net.Sockets.SocketException
        string ExceptionCode,  // 例：(10060) / (0x80131500)；拿不到給空字串
        string ExceptionMessage, // 單行訊息（已去換行）
        string Stack            // 只在 ToInfoWithStackTrace 填，否則空字串
    )
    {
        // 單行輸出（清單用；不包含 StackTrace）
        public override string ToString()
        {
            var code = string.IsNullOrWhiteSpace(ExceptionCode) ? "" : $" {ExceptionCode}";
            var head = $"{Time:yyyy-MM-dd HH:mm:ss.fff} [{Level}] {SourceContext} - {ExceptionType}{code}: {OneLine(ExceptionMessage, 400)}";
            return string.IsNullOrWhiteSpace(Stack) ? head : $"{head}{Environment.NewLine}{Stack}";
        }

        // 這裡留一份工具，避免外部重覆寫
        private static string OneLine(string s, int max)
        {
#if NET6_0_OR_GREATER
            s = (s ?? "").ReplaceLineEndings(" ");
#else
        s = (s ?? "").Replace("\r\n"," ").Replace("\n"," ").Replace("\r"," ");
#endif
            while (s.Contains("  ")) s = s.Replace("  ", " ");
            s = s.Trim();
            return s.Length > max ? s[..max] + "…" : s;
        }
    }
}