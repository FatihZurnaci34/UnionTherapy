using System;
using System.Security.Cryptography;
using System.Text;

namespace UnionTherapy.Infrastructure.Utility.Security
{
    public static class HashingHelper
    {
        /// <summary>
        /// Şifreyi hash'ler ve salt değeri ile birlikte döner
        /// </summary>
        /// <param name="password">Hash'lenecek şifre</param>
        /// <param name="passwordHash">Çıktı: Hash'lenmiş şifre</param>
        /// <param name="passwordSalt">Çıktı: Salt değeri</param>
        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Şifre boş olamaz.", nameof(password));

            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        /// <summary>
        /// Girilen şifrenin hash'lenmiş şifre ile eşleşip eşleşmediğini kontrol eder
        /// </summary>
        /// <param name="password">Kontrol edilecek şifre</param>
        /// <param name="passwordHash">Veritabanındaki hash'lenmiş şifre</param>
        /// <param name="passwordSalt">Veritabanındaki salt değeri</param>
        /// <returns>Şifre doğruysa true, yanlışsa false</returns>
        public static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Şifre boş olamaz.", nameof(password));

            if (passwordHash.Length != 64)
                throw new ArgumentException("Geçersiz şifre hash uzunluğu (64 byte bekleniyor).", nameof(passwordHash));

            if (passwordSalt.Length != 128)
                throw new ArgumentException("Geçersiz şifre salt uzunluğu (128 byte bekleniyor).", nameof(passwordSalt));

            using var hmac = new HMACSHA512(passwordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != passwordHash[i])
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Basit şifre hash'leme (salt olmadan) - Daha az güvenli
        /// </summary>
        /// <param name="password">Hash'lenecek şifre</param>
        /// <returns>Hash'lenmiş şifre</returns>
        public static string HashPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Şifre boş olamaz.", nameof(password));

            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

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