using System;
using System.IO;

namespace HotelReservationSystem.Utils
{
    public static class Logger
    {
        private static readonly string logFilePath = "error.log";

        public static void LogError(string errorMessage)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(logFilePath, true))
                {
                    writer.WriteLine($"{DateTime.Now} - Error: {errorMessage}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to log error: {ex.Message}");
            }
        }
    }
}
