using System;
using System.Security.Cryptography;
using System.Text;

namespace UnionTherapy.Application.Utilities
{
    public static class HashingHelper
    {
        /// <summary>
        /// BCrypt kullanarak şifre hash'leme (önerilen yöntem)
        /// </summary>
        /// <param name="password">Hash'lenecek şifre</param>
        /// <returns>Hash'lenmiş şifre</returns>
        public static string HashPasswordWithBCrypt(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Şifre boş olamaz.", nameof(password));

            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        /// <summary>
        /// BCrypt ile hash'lenmiş şifreyi doğrular
        /// </summary>
        /// <param name="password">Kontrol edilecek şifre</param>
        /// <param name="hashedPassword">Hash'lenmiş şifre</param>
        /// <returns>Şifre doğruysa true, yanlışsa false</returns>
        public static bool VerifyPasswordWithBCrypt(string password, string hashedPassword)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Şifre boş olamaz.", nameof(password));

            if (string.IsNullOrWhiteSpace(hashedPassword))
                throw new ArgumentException("Hash'lenmiş şifre boş olamaz.", nameof(hashedPassword));

            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
} 