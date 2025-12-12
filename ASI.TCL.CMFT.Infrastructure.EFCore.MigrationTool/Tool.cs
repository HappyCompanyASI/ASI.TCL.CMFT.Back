namespace ASI.TCL.CMFT.Infrastructure.EFCore.MigrationTool
{
    public static class Tool
    {
        public static readonly string WebApiProjectName = "ASI.TCL.CMFT.WebAPI";

        private static string FindSolutionDirectory()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            while (currentDirectory != null)
            {
                if (Directory.GetFiles(currentDirectory, "*.sln").Any())
                {
                    return currentDirectory; // 找到 `.sln` 檔案
                }
                currentDirectory = Directory.GetParent(currentDirectory)?.FullName;
            }
            return null; // 沒找到 `.sln`
        }
        public static string FindProjectDirectory(string projectName)
        {
            var solutionDirectory = FindSolutionDirectory();
            if (string.IsNullOrEmpty(solutionDirectory))
            {
                return null;
            }

            // 直接在整個 solution 底下找「專案名稱.csproj」
            var csprojFileName = projectName + ".csproj";

            var csprojPath = Directory.GetFiles(
                solutionDirectory,
                csprojFileName,
                SearchOption.AllDirectories
            ).FirstOrDefault();

            if (string.IsNullOrEmpty(csprojPath))
            {
                return null;
            }

            return Path.GetDirectoryName(csprojPath);
        }
    }
}