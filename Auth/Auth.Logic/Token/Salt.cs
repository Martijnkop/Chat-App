using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Logic.Token
{
    internal class Salt
    {
        internal static string CreateSalt(string input)
        {
            byte[] userBytes;
            string salt;
            userBytes = ASCIIEncoding.ASCII.GetBytes(input);
            long XORED = 0x00;

            foreach (int x in userBytes)
                XORED = XORED ^ x;

            Random rand = new Random(Convert.ToInt32(XORED));
            salt = rand.Next().ToString();
            salt += rand.Next().ToString();
            salt += rand.Next().ToString();
            salt += rand.Next().ToString();
            return salt;
        }

        internal static string ByteArrayToString(byte[] input)
        {
            StringBuilder output = new StringBuilder("");
            for (int i = 0; i < input.Length; i++)
            {
                output.Append(input[i].ToString("X2"));
            }
            return output.ToString();
        }
    }
}
