using Microsoft.Extensions.Localization;
using System.Text.RegularExpressions;


namespace LinoVative.Service.Backend.Helpers
{
    public static class PasswordHelper
    {
        public static (bool, List<string>) IsPasswordStrong(string passwordPlainText, IStringLocalizer localizer)
        {
            var errorList = new List<string>();
            // 1. Memeriksa panjang minimum
            if (passwordPlainText.Length < 8)
            {
                errorList.Add(localizer["Password.MinLenght", 8]);
            }

            // 2. Memeriksa keberadaan huruf besar dan kecil
            bool hasLower = passwordPlainText.Any(char.IsLower);
            bool hasUpper = passwordPlainText.Any(char.IsUpper);
            if (!hasLower || !hasUpper)
            {
                errorList.Add(localizer["Password.MustHasLowerOrUppercase"]);
            }

            // 3. Memeriksa keberadaan angka
            bool hasDigit = passwordPlainText.Any(char.IsDigit);
            if (!hasDigit)
            {
                errorList.Add(localizer["Password.MustHasNumber"]);
            }

            // 4. Memeriksa keberadaan karakter unik (non-alphanumeric)
            // Kita gunakan regex untuk cara yang ringkas.
            var hasSpecialChar = new Regex(@"[^\da-zA-Z]").IsMatch(passwordPlainText);
            if (!hasSpecialChar)
            {
                errorList.Add(localizer["Password.MustHasSpecialChar"]);
            }

            // 5. Jika semua kriteria terpenuhi, kata sandi dianggap kuat.
            return (errorList.Count() == 0, errorList);
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
