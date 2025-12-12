using System.ComponentModel;
using System.Net.Sockets;

namespace ASI.TCL.CMFT.Infrastructure.Serilog
{
    public static class ExceptionExtensions
    {
        public static ExceptionInfo ToInfo(this Exception ex, string level = "ERR")
        {
            var root = ex.GetBaseException();

            // SourceContext：優先用丟出類別；退回 ex.Source 或組件名
            var sourceContext =
                root.TargetSite?.DeclaringType?.FullName
                ?? ex.TargetSite?.DeclaringType?.FullName
                ?? root.Source
                ?? root.GetType().Assembly.GetName().Name
                ?? "?";

            // 錯誤代碼（Socket/Win32/HResult）
            var code = "";
            if (root is SocketException se) code = $"({(int)se.SocketErrorCode})";
            else if (root is Win32Exception we) code = $"({we.NativeErrorCode})";
            else if (root.HResult != 0) code = $"(0x{root.HResult:X8})";

            return new ExceptionInfo(
                Time: DateTimeOffset.Now,
                Level: level,
                SourceContext: sourceContext,
                ExceptionType: root.GetType().FullName ?? "?",
                ExceptionCode: code,
                ExceptionMessage: OneLine(root.Message, 400),
                Stack: string.Empty
            );
        }

        // 含 StackTrace（只把 StackTrace 放進欄位；ToString 仍是單行不顯示 Stack）
        public static ExceptionInfo ToInfoWithStackTrace(this Exception ex, string level = "ERR")
        {
            var info = ex.ToInfo(level);
            var root = ex.GetBaseException();
            return info with { Stack = root.StackTrace ?? string.Empty };
        }

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