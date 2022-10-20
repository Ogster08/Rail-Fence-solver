namespace Rail_Fence_Solver
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Concurrent;
    using System.ComponentModel;

    class Program
    {
        static void Main(string[] args)
        {
            while (true) 
            { 
                string text = "";
                while (text.Length < 1)
                {
                    Console.Write("Enter encrypted text (at least 200 characters long): ");
                    text = Console.ReadLine().ToLower();
                    Console.WriteLine();
                }

                for (int keyLength = 2; keyLength <= 15; keyLength++)
                {
                    char[][] railFence = new char[keyLength][];
                    for (int i = 0; i < railFence.Length; i++) { railFence[i] = new char[text.Length]; }
                    int[] indexes = new int[] { -1 , -1};
                    bool countUp = true;
                    Tuple<int[], bool> nextSet;
                    for (int i = 0; i < text.Length; i++)
                    {
                        nextSet = NextRailFence(indexes, keyLength, countUp);
                        indexes = nextSet.Item1;
                        countUp = nextSet.Item2;
                        railFence[indexes[0]][indexes[1]] = Convert.ToChar("|");
                    }

                    int count = 0;
                    for ( int x = 0; x < keyLength; x++)
                    {
                        for ( int y = 0; y < text.Length; y++)
                        {
                            if (railFence[x][y] == Convert.ToChar("|")) {
                                railFence[x][y] = text[count++]; 
                            }
                        }
                    }

                    char[] possibleDecryption = new char[text.Length];
                    indexes = new int[] { -1, -1 };
                    countUp = true;
                    for (int i = 0; i < text.Length; i++)
                    {
                        nextSet = NextRailFence(indexes, keyLength, countUp);
                        indexes = nextSet.Item1;
                        countUp = nextSet.Item2;
                        possibleDecryption[i] = railFence[indexes[0]][indexes[1]];
                    }

                    Console.WriteLine($"{string.Join("", possibleDecryption)} {keyLength}");
                    Console.WriteLine();

                }
            }
        }

        static Tuple<int[],bool> NextRailFence(int[] currentRailFence, int keyLength, bool countUp)
        {
            int[] railFence = new int[2];
            if (countUp & (currentRailFence[0] + 1 == keyLength)) { countUp = false; }
            if (!countUp & currentRailFence[0] == 0) { countUp = true; }
            if (countUp) { railFence[0] = currentRailFence[0] + 1; } else { railFence[0] = currentRailFence[0] - 1; }
            railFence[1] = currentRailFence[1] + 1;
            
            return Tuple.Create(railFence, countUp);
        }

        public static Dictionary<string, int> TextFrequency(string testText)
        {
            var characterCount = new Dictionary<string, int>() { { "a", 0 }, { "b", 0 }, { "c", 0 }, { "d", 0 }, { "e", 0 }, { "f", 0 }, { "g", 0 }, { "h", 0 }, { "i", 0 }, { "j", 0 }, { "k", 0 }, { "l", 0 }, { "m", 0 }, { "n", 0 }, { "o", 0 }, { "p", 0 }, { "q", 0 }, { "r", 0 }, { "s", 0 }, { "t", 0 }, { "u", 0 }, { "v", 0 }, { "w", 0 }, { "x", 0 }, { "y", 0 }, { "z", 0 } };

            foreach (char c in testText) { characterCount[c.ToString()]++; }
            return characterCount;
        }

        public static double ChiSquareTest(string testText)
        {
            var exspectedFrequencies = new Dictionary<string, double>() { { "e", 11.1607 }, { "a", 8.4966 }, { "r", 7.5809 }, { "i", 7.5448 }, { "o", 7.1635 }, { "t", 6.9509 }, { "n", 6.6544 }, { "s", 5.7351 }, { "l", 5.4893 }, { "c", 4.5388 }, { "u", 3.6308 }, { "d", 3.3844 }, { "p", 3.1671 }, { "m", 3.0129 }, { "h", 3.0034 }, { "g", 2.4705 }, { "b", 2.0720 }, { "f", 1.8121 }, { "y", 1.7779 }, { "w", 1.2899 }, { "k", 1.1016 }, { "v", 1.0074 }, { "x", 0.2902 }, { "z", 0.2722 }, { "j", 0.1965 }, { "q", 0.1962 } };
            Dictionary<string, int> textFrequencies = TextFrequency(testText);

            double score = 0;

            foreach (string d in textFrequencies.Keys)
            {
                string s = d.ToLower();
                double exspectedCount = exspectedFrequencies[s] / 100 * testText.Length;
                score += Math.Pow(textFrequencies[s] - exspectedCount, 2) / exspectedCount;
            }
            return score;
        }
    }
}