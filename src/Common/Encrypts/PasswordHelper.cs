using System;
using Common.Utilities;

namespace Common.Encrypts
{
    public enum PasswordFormat
    {
        Clear = 0,
        Hashed = 1,
        Encrypted = 2
    }

    public interface IPasswordHelper
    {
        string GenerateRandomSalt();
        string Encrypt(string password, string salt, PasswordFormat format = PasswordFormat.Hashed);
    }
    
    public class PasswordHelper : IPasswordHelper
    {
        #region for di extensions

        [LazySingleton]
        public static IPasswordHelper Instance => LazySingleton.Instance.Resolve(() => new PasswordHelper());

        #endregion
        
        private readonly IEncryptHelper _helper = EncryptHelper.Instance;

        public string Encrypt(string password, string salt, PasswordFormat format = PasswordFormat.Hashed)
        {
            string encryptPass;
            switch (format)
            {
                case PasswordFormat.Clear:
                    encryptPass = password;
                    break;
                case PasswordFormat.Hashed:
                    encryptPass = _helper.Hash(password + salt);
                    break;
                case PasswordFormat.Encrypted:
                    encryptPass = _helper.EncryptSymmetric(password + salt);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(format));
            }
            return encryptPass;
        }

        public string GenerateRandomSalt()
        {
            return _helper.GenerateRandomString(4, 97, 122);
        }
    }
}