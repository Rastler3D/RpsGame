using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RpsGame
{
    internal class HMACGenerator
    {
        readonly HMAC _hmac = new HMACSHA256();

        public string GenerateHMAC(string key, string message)
        {
            _hmac.Key = Encoding.Default.GetBytes(key);

            var hmac = _hmac.ComputeHash(Encoding.Default.GetBytes(message));

            return Convert.ToHexString(hmac);
        }
    }
}
