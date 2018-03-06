namespace PlayerUnknown.Leaker
{
    public static class Program
    {
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        public static void Main(string[] Args)
        {
            string Source = "";
            string Target = "TslGame";

            if (Args.Length > 0)
            {
                Source = Args[0];

                if (Args.Length > 1)
                {
                    Target = Args[1];
                }
            }

            FuckBattlEye.Run(Source, Target);

            System.Console.ReadKey(false);
        }
    }
}
