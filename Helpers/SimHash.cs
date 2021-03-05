using System;
using System.Data.HashFunction.Jenkins;
using System.Linq;

namespace Prism.Helpers
{
    public static class SimHash
    {

        public static uint SimHashOfString(string s)
        {
            //char[] delimiters = {' ', ',', '.'};
            //var words = s.ToLower().Split(delimiters);
            char[] toBeRemoved = {' ',',', '.', '!'};
            var words = NGrams(s.ToLower().Trim(toBeRemoved), 2);
            string[] hashedWords = new string[words.Length];
            var jenkins = JenkinsOneAtATimeFactory.Instance.Create();

            for (int i = 0; i < words.Length; i++)
            {
                hashedWords[i] = "";
                byte[] hash = jenkins.ComputeHash(System.Text.Encoding.UTF8.GetBytes(words[i])).Hash;
                for (int j = 0; j < 4; j++)
                {
                    hashedWords[i] += Convert.ToString(hash[j], 2).PadLeft(8, '0');
                }
            }

            string result = "";
            var sum = 0;

            for (int j = 0; j < 32; j++)
            {
                sum = 0;
                for (int i = 0; i < hashedWords.Length; i++)
                {
                    if (hashedWords[i][j].Equals('0'))
                    {
                        sum -= 1;
                    }
                    else
                    {
                        sum += 1;
                    }
                }
                if (sum < 0)
                {
                    result += "0";
                }
                else
                {
                    result += "1";
                }
            }

            int numOfBytes = result.Length / 8;
            byte[] bytes = new byte[numOfBytes];
            for (int i = 0; i < numOfBytes; i++)
            {
                bytes[i] = Convert.ToByte(result.Substring(8 * i, 8), 2);
            }

            return BitConverter.ToUInt32(bytes, 0);
        }
        
        private static string[] NGrams(string str, int chunkSize)
        {
            return Enumerable.Range(0, str.Length / chunkSize)
                .Select(i => str.Substring(i * chunkSize, chunkSize)).ToArray();
        }
    }
}