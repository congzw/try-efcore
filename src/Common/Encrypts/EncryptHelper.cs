using Common.Utilities;

namespace Common.Encrypts
{
    public interface IEncryptHelper
    {
        string GenerateRandomString(int keySize, int smallCharAsciiCode, int bigCharAsciiCode);
        string Hash(string inputString);
        string EncryptSymmetric(string inputText);
        string DecryptSymmetric(string inputText);

        //todo
        //string EncryptAsymmetric(string inputText, string key);
        //string DecryptAsymmetric(string inputText, string key);
    }

    public class EncryptHelper : IEncryptHelper
    {
        #region for di extensions

        [LazySingleton]
        public static IEncryptHelper Instance => LazySingleton.Instance.Resolve(() => new EncryptHelper());

        #endregion
        
        private readonly MyHashHelper _hashHelper = new MyHashHelper();
        private readonly MyRandomHelper _randomHelper = new MyRandomHelper();
        private readonly MySymmetricCryptoHelper _symmetricHelper = new MySymmetricCryptoHelper();

        public string Hash(string inputString)
        {
            return _hashHelper.ComputeHash(inputString);
        }

        public string EncryptSymmetric(string inputText)
        {
            return _symmetricHelper.EncryptString(inputText);
        }

        public string DecryptSymmetric(string inputText)
        {
            return _symmetricHelper.DecryptString(inputText);
        }

        public string GenerateRandomString(int keySize, int smallCharAsciiCode, int bigCharAsciiCode)
        {
            return _randomHelper.GetRandomKey(keySize, smallCharAsciiCode, bigCharAsciiCode);
        }
    }
}
