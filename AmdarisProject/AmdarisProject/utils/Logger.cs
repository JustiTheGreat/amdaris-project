namespace AmdarisProject.utils
{
    public class Logger
    {
        public static string FileName = $"./Logs_{DateTime.Now.Date.ToString().Replace("/", "_").Replace(":", "_")}.txt";

        public static void Log(string methodName, bool success)
        {
            using FileStream fileStream = File.Open(FileName, FileMode.Append);
            using StreamWriter streamWriter = new(fileStream);
            streamWriter.WriteLineAsync($"{methodName} - {DateTime.Now} - {(success ? "SUCCESS" : "FAILURE")}");
        }
    }
}
