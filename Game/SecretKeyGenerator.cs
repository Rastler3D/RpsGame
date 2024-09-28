using System.Security.Cryptography;


namespace RpsGame
{
    internal class SecretKeyGenerator
    {
        readonly RandomNumberGenerator _rng = RandomNumberGenerator.Create();

        public string GenerateKey()
        {
            var keyData = new byte[32];
            _rng.GetBytes(keyData);

            return Convert.ToHexString(keyData);
        }
    }
}
