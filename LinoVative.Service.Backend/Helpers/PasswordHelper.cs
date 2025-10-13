using System.Text.RegularExpressions;


namespace LinoVative.Service.Backend.Helpers
{
    public static class PasswordHelper
    {
        public static bool IsPasswordStrong(string passwordPlainText)
        {
            // 1. Memeriksa panjang minimum
            if (passwordPlainText.Length < 8)
            {
                return false;
            }

            // 2. Memeriksa keberadaan huruf besar dan kecil
            bool hasLower = passwordPlainText.Any(char.IsLower);
            bool hasUpper = passwordPlainText.Any(char.IsUpper);
            if (!hasLower || !hasUpper)
            {
                return false;
            }

            // 3. Memeriksa keberadaan angka
            bool hasDigit = passwordPlainText.Any(char.IsDigit);
            if (!hasDigit)
            {
                return false;
            }

            // 4. Memeriksa keberadaan karakter unik (non-alphanumeric)
            // Kita gunakan regex untuk cara yang ringkas.
            var hasSpecialChar = new Regex(@"[^\da-zA-Z]").IsMatch(passwordPlainText);
            if (!hasSpecialChar)
            {
                return false;
            }

            // 5. Jika semua kriteria terpenuhi, kata sandi dianggap kuat.
            return true;
        }



        public static string CreateHashedPassword(string password, Guid userId)
        {
            // BCrypt.HashPassword() secara otomatis menghasilkan salt yang unik
            // dan menggabungkannya ke dalam hash yang dihasilkan.
            // Faktor kerja (work factor) default adalah 11, yang cukup aman.
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(MergePasswordAndUserId(password, userId));

            return hashedPassword;
        }


        public static bool VerifyUserPassword(string plainTextPassword, string storedHashedPassword, Guid userId)
        {
            // BCrypt.Verify() secara otomatis mengekstrak salt dari
            // hash yang tersimpan dan memverifikasi kata sandi.
            bool isPasswordMatch = BCrypt.Net.BCrypt.Verify(MergePasswordAndUserId(plainTextPassword, userId), storedHashedPassword);

            return isPasswordMatch;
        }

        private static string MergePasswordAndUserId(string password, Guid userId) => $"{password}***{userId}";

    }
}
