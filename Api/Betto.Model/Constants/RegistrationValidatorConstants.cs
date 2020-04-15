namespace Betto.Model.Constants
{
    public static class RegistrationValidatorConstants
    {
        public const int MaximumUsernameLength = 100;
        public const int MinimumUsernameLength = 3;
        public const string PasswordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,15}$";
        public const string MailAddressPattern = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
    }
}
