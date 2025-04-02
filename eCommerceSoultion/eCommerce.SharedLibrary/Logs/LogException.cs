using Serilog;

namespace eCommerce.SharedLibrary.Logs
{
    public static class LogException
    {
        public static void LogExceptions(Exception ex)
        {
            LogToFile(ex.Message);
            LogToConsole(ex.Message);
            LogToDebugger(ex.Message);
        }

        private static void LogToDebugger(string mssg) =>Log.Debug(mssg);

        private static void LogToConsole(string mssg) =>Log.Warning(mssg);

        private static void LogToFile(string mssg)=>Log.Information(mssg);
    }
}
