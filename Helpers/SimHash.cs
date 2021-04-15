using System;
using System.Data.HashFunction.Jenkins;
using System.Linq;
using System.Collections;

namespace Prism.Helpers
{
    public static class SimHash
    {

        public static uint SimHashOfString(string s)
        {
            char[] delimiters = {' ', ',', '.', ':'};
            var words = s.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            string[] toRemove = {"de","pe","la","cu","a","se"};
            words = words.Where(x => !toRemove.Contains(x)).ToArray();
            //s = Regex.Replace( s, "[.,-:]", string.Empty);
            //var words = NGrams(s.ToLower(), 2);
            BitArray[] hashedWords = new BitArray[words.Length];
            //var jenkins = JenkinsOneAtATimeFactory.Instance.Create();
            var jenkins = JenkinsLookup2Factory.Instance.Create();

            for (int i = 0; i < words.Length; i++)
            {
                byte[] hash = jenkins.ComputeHash(System.Text.Encoding.UTF8.GetBytes(words[i])).Hash;
                hashedWords[i] = new BitArray(hash);
            }

            BitArray result = new BitArray(32);
            var sum = 0;
            for (int j = 0; j < 32; j++)
            {
                sum = 0;
                for (int i = 0; i < hashedWords.Length; i++)
                {
                    if ( hashedWords[i].Get(j) )
                    {
                        sum += 1;
                    }
                    else
                    {
                        sum -= 1;
                    }
                }

                if (sum < 0)
                {
                    result[j] = false;
                }
                else
                {
                    result[j] = true;
                }
            }
            return BitConverter.ToUInt32(BitArrayToByteArray(result), 0);
        }
        
        private static byte[] BitArrayToByteArray(BitArray bits)
        {
            byte[] result = new byte[(bits.Length - 1) / 8 + 1];
            bits.CopyTo(result, 0);
            return result;
        }
        
        private static string[] NGrams(string str, int chunkSize)
        {
            return Enumerable.Range(0, str.Length / chunkSize)
                .Select(i => str.Substring(i * chunkSize, chunkSize)).ToArray();
        }
    }
}
