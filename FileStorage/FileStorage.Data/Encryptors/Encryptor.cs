using System.Security.Cryptography;

namespace FileStorage.DAL.Encryptors
{
    public static class Encryptor
    {
        private const int Iterations = 10000;

        public static EncryptedPassword Encrypt(string password)
        {
            var salt = new byte[16];

            using (RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetBytes(salt);
            }

            var rfcKeyGen = new Rfc2898DeriveBytes(password, salt, Iterations);
            byte[] encryptedPassword = rfcKeyGen.GetBytes(16);
            rfcKeyGen.Reset();

            return new EncryptedPassword()
            {
                HashPassword = encryptedPassword,
                Salt = salt
            };
        }

        public static byte[] EncryptWithSalt(string password, byte[] salt)
        {
            var rfcKeyGen = new Rfc2898DeriveBytes(password, salt, Iterations);
            byte[] encryptedPassword = rfcKeyGen.GetBytes(16);
            rfcKeyGen.Reset();

            return encryptedPassword;
        }
    }

    public class EncryptedPassword
    {
        public byte[] HashPassword { get; set; }
        public byte[] Salt { get; set; }
    }
}
