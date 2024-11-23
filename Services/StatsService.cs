namespace aws_s3_file_validator.Utils
{
    public static class StatsService
    {
        private static int _totalLineCount;
        private static int _validLineCount;
        private static int _invalidLineCount;

        public static void IncrementTotalLineCount() => _totalLineCount++;
        public static void IncrementValidLineCount() => _validLineCount++;
        public static void IncrementInvalidLineCount() => _invalidLineCount++;

        public static int TotalLineCount => _totalLineCount;

        public static void PrintStats()
        {
            Console.WriteLine($"Total Lines: {_totalLineCount:n0}");
            Console.WriteLine($"Valid Lines: {_validLineCount:n0}");
            Console.WriteLine($"Invalid Lines: {_invalidLineCount:n0}");
        }

        public static void Clear()
        {
            _totalLineCount = 0;
            _validLineCount = 0;
            _invalidLineCount = 0;
        }
    }
}