using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Betto.Helpers
{
    public class PasswordHasher : IPasswordHasher
    {
        private const int SaltLength = 32;
        private const int BytesToCheckAmount = 32;
        private const int HashIterationsAmount = 10000;

        public bool VerifyPassword(byte[] storedPasswordHash, string passedPassword)
        {
            var salt = new byte[SaltLength];
            Array.Copy(storedPasswordHash, 0, salt, 0, SaltLength);

            var passedPasswordHash = HashPassword(passedPassword, salt);

            return VerifyEncryptedBytes(storedPasswordHash, passedPasswordHash);
        }

        public byte[] EncodePassword(string passedPassword)
        {
            var cryptoProvider = new RNGCryptoServiceProvider();
            var salt = new byte[SaltLength];
            cryptoProvider.GetBytes(salt);

            var coreHash = HashPassword(passedPassword, salt);

            var passwordHash = new byte[SaltLength + BytesToCheckAmount];
            Array.Copy(salt, 0, passwordHash, 0, SaltLength);
            Array.Copy(coreHash, 0, passwordHash, SaltLength, BytesToCheckAmount);

            return passwordHash;
        }

        private static bool VerifyEncryptedBytes(IReadOnlyList<byte> storedPasswordHash, IReadOnlyList<byte> passwordHash)
        {
            var validBytes = 0;

            for (var i = 0; i < BytesToCheckAmount; i++)
            {
                if (storedPasswordHash[i + SaltLength] == passwordHash[i])
                {
                    validBytes++;
                }
            }

            return validBytes == BytesToCheckAmount;
        }

        private static byte[] HashPassword(string passedPassword, byte[] salt)
        {
            var key = new Rfc2898DeriveBytes(passedPassword, salt, HashIterationsAmount);
            return key.GetBytes(BytesToCheckAmount);
        }
    }
}
