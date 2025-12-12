namespace ASI.TCL.CMFT.Infrastructure.EFCore.MigrationTool
{
    /// <summary>
    /// 專門執行 Migration的專案
    /// 需要參考EFCore專案
    /// </summary>
    internal class Program
    {
        static void Main(string[] args)
        {
            var temp = Tool.FindProjectDirectory(Tool.WebApiProjectName);

            Console.WriteLine("Hello, World!");
        }
    }
}
