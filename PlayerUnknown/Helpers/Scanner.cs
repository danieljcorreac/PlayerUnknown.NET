namespace PlayerUnknown.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Scanner
    {
        /// <summary>
        /// Gets or sets the process handle.
        /// </summary>
        private IntPtr g_hProcess
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the module buffer.
        /// </summary>
        private byte[] g_arrModuleBuffer
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the module base address.
        /// </summary>
        private ulong g_lpModuleBase
        {
            get; set;
        }

        /// <summary>
        /// Gets the dictionary of string patterns.
        /// </summary>
        private Dictionary<string, string> g_dictStringPatterns
        {
            get;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Scanner"/> class.
        /// </summary>
        /// <param name="Handle">The handle.</param>
        public Scanner(IntPtr Handle)
        {
            g_hProcess = Handle;
            g_dictStringPatterns = new Dictionary<string, string>();
        }

        /// <summary>
        /// Selects the module.
        /// </summary>
        /// <param name="hModule">The module.</param>
        /// <param name="SizeOfImage">The size of the image.</param>
        public bool SelectModule(IntPtr hModule, uint SizeOfImage)
        {
            g_lpModuleBase      = (ulong) hModule;
            g_arrModuleBuffer   = new byte[SizeOfImage];

            g_dictStringPatterns.Clear();

            return Memory.ReadProcessMemory(g_hProcess, g_lpModuleBase, g_arrModuleBuffer, SizeOfImage);
        }

        /// <summary>
        /// Adds the specified pattern.
        /// </summary>
        /// <param name="PatternName">Name of the sz pattern.</param>
        /// <param name="Pattern">The sz pattern.</param>
        public void AddPattern(string PatternName, string Pattern)
        {
            g_dictStringPatterns.Add(PatternName, Pattern);
        }

        /// <summary>
        /// Patterns the check.
        /// </summary>
        /// <param name="nOffset">The n offset.</param>
        /// <param name="arrPattern">The arr pattern.</param>
        /// <returns></returns>
        private bool PatternCheck(int Offset, byte[] Pattern)
        {
            return !Pattern.Where((T, I) => T != 0x0 && T != this.g_arrModuleBuffer[Offset + I]).Any();
        }

        /// <summary>
        /// Finds the pattern.
        /// </summary>
        /// <param name="Pattern">The pattern.</param>
        /// <exception cref="Exception">Selected module is null.</exception>
        public ulong FindPattern(string Pattern)
        {
            if (g_arrModuleBuffer == null || g_lpModuleBase == 0)
            {
                throw new Exception("Selected module is null.");
            }

            byte[] arrPattern = ParsePatternString(Pattern);

            for (int ModuleIndex = 0; ModuleIndex < g_arrModuleBuffer.Length; ModuleIndex++)
            {
                if (this.g_arrModuleBuffer[ModuleIndex] != arrPattern[0])
                {
                    continue;
                }

                if (PatternCheck(ModuleIndex, arrPattern))
                {
                    return g_lpModuleBase + (ulong) ModuleIndex;
                }
            }

            return 0;
        }

        /// <summary>
        /// Finds the patterns.
        /// </summary>
        /// <param name="lTime">The l time.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Selected module is null</exception>
        public Dictionary<string, ulong> FindPatterns()
        {
            if (g_arrModuleBuffer == null || g_lpModuleBase == 0)
            {
                throw new Exception("Selected module is null");
            }

            byte[][] arrBytePatterns = new byte[g_dictStringPatterns.Count][];
            ulong[] arrResult = new ulong[g_dictStringPatterns.Count];

            for (int Index = 0; Index < g_dictStringPatterns.Count; Index++)
            {
                arrBytePatterns[Index] = ParsePatternString(g_dictStringPatterns.ElementAt(Index).Value);
            }

            for (int nModuleIndex = 0; nModuleIndex < g_arrModuleBuffer.Length; nModuleIndex++)
            {
                for (int nPatternIndex = 0; nPatternIndex < arrBytePatterns.Length; nPatternIndex++)
                {
                    if (arrResult[nPatternIndex] != 0)
                    {
                        continue;
                    }

                    if (this.PatternCheck(nModuleIndex, arrBytePatterns[nPatternIndex]))
                    {
                        arrResult[nPatternIndex] = g_lpModuleBase + (ulong) nModuleIndex;
                    }
                }
            }

            Dictionary<string, ulong> dictResultFormatted = new Dictionary<string, ulong>();

            for (int PatternIndex = 0; PatternIndex < arrBytePatterns.Length; PatternIndex++)
            {
                dictResultFormatted[g_dictStringPatterns.ElementAt(PatternIndex).Key] = arrResult[PatternIndex];
            }

            return dictResultFormatted;
        }

        private byte[] ParsePatternString(string Pattern)
        {
            List<byte> patternbytes = new List<byte>();

            foreach (var Byte in Pattern.Split(' '))
            {
                patternbytes.Add(Byte == "?" ? (byte) 0x0 : Convert.ToByte(Byte, 16));
            }

            return patternbytes.ToArray();
        }
    }
}
