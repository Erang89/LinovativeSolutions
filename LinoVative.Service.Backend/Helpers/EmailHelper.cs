using System.Net.Mail;

namespace LinoVative.Service.Backend.Helpers
{
    public static class EmailHelper
    {
        public static bool IsValidEmailAddress(string emailAddress)
        {
            if (string.IsNullOrWhiteSpace(emailAddress))
                return false;

            // Practical length check (RFC 5321 local-part max 64, domain max 255)
            if (emailAddress.Length > 320)
                return false;

            try
            {
                // MailAddress internally parses and validates
                var mail = new MailAddress(emailAddress);
                return mail.Address.Equals(emailAddress, StringComparison.OrdinalIgnoreCase);
            }
            catch
            {
                return false;
            }
        }
    }
}
