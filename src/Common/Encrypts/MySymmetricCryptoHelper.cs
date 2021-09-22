using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Common.Encrypts
{
    /// <summary>
    /// 对称加密类，也是基于Base64的字符串转化
    /// </summary>
    public class MySymmetricCryptoHelper
    {
        #region 私有字段

        private readonly ICryptoTransform _encrypt;		// 加密器对象
        private readonly ICryptoTransform _decrypt;		// 解密器对象
        private readonly SymmetricAlgorithm _provider;
        private const int BufferSize = 1024;    //缓冲大小
        private Encoding _encodingMode = Encoding.UTF8; //字符编码格式，默认UTF8

        #endregion

        #region 属性

        public Encoding EncodingMode
        {
            get => _encodingMode;
            set => _encodingMode = value;
        }

        #endregion

        #region 构造函数

        //对称加密算法Helper类构造函数
        /// <summary>
        ///对称加密算法Helper类，可以自行制定加密算法，IV（要求8x8=64Bit），
        /// Key（要求8x16=128Bit）,默认采用TripleDES加密算法，
        /// 该算法支持从 128 位到 192 位（以 64 位递增）的密钥Key长度。 
        /// </summary>
        /// <param name="algorithmName">对称加密算法名称</param>
        /// <param name="iv">8byte[]的向量（要求8x8=64Bit）</param>
        /// <param name="key">16位byte[]或24byte[]的密码（要求8x16=128Bit）,默认采用TripleDES加密算法，该算法支持从 128 位到 192 位（以 64 位递增）的密钥Key长度。</param>
        public MySymmetricCryptoHelper(string algorithmName, byte[] iv, byte[] key)
        {
            try
            {
                //创建对称加密算法provider对象（默认TripleDES）
                //System.Security.Cryptography.Aes
                //System.Security.Cryptography.DES
                //System.Security.Cryptography.RC2
                //System.Security.Cryptography.Rijndael
                //System.Security.Cryptography.TripleDES
                _provider = SymmetricAlgorithm.Create(algorithmName);
                //赋值加密向量IV（默认_iv）
                _provider.IV = iv;
                //赋值加密Key（默认_key）
                _provider.Key = key;

                //由对称加密算法provider对象分别创建加密和解密的对象
                _encrypt = _provider.CreateEncryptor();
                _decrypt = _provider.CreateDecryptor();
            }
            catch (Exception)
            {
                throw;
            }

        }

        //对称加密算法Helper类构造函数
        /// <summary>
        ///对称加密算法Helper类，可以自行制定加密算法，IV（要求8x8=64Bit），
        /// Key（要求8x16=128Bit）,默认采用TripleDES加密算法，
        /// 该算法支持从 128 位到 192 位（以 64 位递增）的密钥Key长度。 
        /// </summary>
        /// <param name="algorithmName">对称加密算法名称</param>
        /// <param name="iv">8位字符构成的字符串，向量（要求8x8=64Bit）</param>
        /// <param name="key">16位或24位字符构成的字符串，密码（要求8x16=128Bit）,默认采用TripleDES加密算法，该算法支持从 128 位到 192 位（以 64 位递增）的密钥Key长度。</param>
        public MySymmetricCryptoHelper(string algorithmName, string iv, string key) : this(algorithmName, Encoding.UTF8.GetBytes(iv), Encoding.UTF8.GetBytes(key)) { }

        //默认的TripleDES对称加密算法Helper类
        /// <summary>
        /// 默认的TripleDES对称加密算法Helper类，IV（要求8x8=64Bit），Key（要求8x16=128Bit）
        /// </summary>
        /// <param name="iv">8位字符构成的字符串，向量（要求8x8=64Bit）</param>
        /// <param name="key">16位或24位字符构成的字符串，密码（要求8x16=128Bit）,默认采用TripleDES加密算法，该算法支持从 128 位到 192 位（以 64 位递增）的密钥Key长度。</param>
        public MySymmetricCryptoHelper(string iv, string key) : this("TripleDES", iv, key) { }

        //默认的TripleDES对称加密算法Helper类
        /// <summary>
        /// 默认的TripleDES对称加密算法Helper类
        /// </summary>
        /// <param name="iv">8位字符构成的字符串，向量（要求8x8=64Bit）</param>
        /// <param name="key">16位或24位字符构成的字符串，密码（要求8x16=128Bit）,默认采用TripleDES加密算法，该算法支持从 128 位到 192 位（以 64 位递增）的密钥Key长度。</param>
        public MySymmetricCryptoHelper(byte[] iv, byte[] key) : this("TripleDES", iv, key) { }

        /*
        安全考虑，强烈建议不使用默认提供的iv和key*/
        public MySymmetricCryptoHelper() : this("TripleDES", "kaj#gou^", "6adgbgi8JoG&D0g=") { }
        public MySymmetricCryptoHelper(string algorithmName) : this(algorithmName, "kaj#gou^", "6adgbgi8JoG&D0g=") { }


        #endregion

        #region 公开方法

        //加密字节流方法
        /// <summary>
        /// 对称加密算法，可以自行制定加密算法，IV（要求8x8=64Bit），
        /// Key（要求8x16=128Bit）,默认采用TripleDES加密算法，
        /// 该算法支持从 128 位到 192 位（以 64 位递增）的密钥Key长度。
        /// </summary>
        /// <param name="inputBytes">输入字节流</param>
        /// <returns>输出字节流</returns>
        public byte[] EncryptByte(byte[] inputBytes)
        {
            //var inputStream = new MemoryStream(inputBytes);//包装成输入流
            var outputStream = new MemoryStream();//创建空的输出流
            using (var inputStream = new MemoryStream(inputBytes))
            using (var cryptoStream = new CryptoStream(outputStream, _encrypt, CryptoStreamMode.Write))
            {
                // 用buffer读取inputStream
                // 并用buffer写入到outputStream
                int count = 0;
                byte[] buffer = new byte[BufferSize];
                while ((count = inputStream.Read(buffer, 0, BufferSize)) > 0)
                {
                    cryptoStream.Write(buffer, 0, count);
                }
                cryptoStream.FlushFinalBlock();
                byte[] outputBytes = outputStream.ToArray();
                return outputBytes;
            }
        }

        //解密密字节流方法
        /// <summary>
        /// 对称解密算法，可以自行制定解密算法，IV（要求8x8=64Bit），
        /// Key（要求8x16=128Bit）,默认采用TripleDES解密算法，
        /// 该算法支持从 128 位到 192 位（以 64 位递增）的密钥Key长度。
        /// </summary>
        /// <param name="inputBytes">输入字节流</param>
        /// <returns>输出字节流</returns>
        public byte[] DecryptByte(byte[] inputBytes)
        {
            MemoryStream inputStream = new MemoryStream(inputBytes);//包装成输入流
            //创建解密流
            using (MemoryStream outputStream = new MemoryStream())
            using (CryptoStream cryptoStream = new CryptoStream(inputStream, _decrypt, CryptoStreamMode.Read))
            {
                // 用buffer读取inputStream
                // 并用buffer写入到outputStream
                int count = 0;
                byte[] buffer = new byte[BufferSize];
                while ((count = cryptoStream.Read(buffer, 0, BufferSize)) > 0)
                {
                    outputStream.Write(buffer, 0, count);
                }

                outputStream.Flush();

                byte[] outputBytes = outputStream.ToArray();
                //将解密后的字节Array返回
                return outputBytes;
            }
        }

        // 加密字符流（字符串）算法，加密后的文本默认利用Base64转换
        /// <summary>
        ///  加密字符流（字符串）算法，加密后的文本默认利用Base64转换
        /// </summary>
        /// <param name="inputText">输入待加密的文本</param>
        /// <returns>加密后的文本（默认利用Base64转换）</returns>
        public string EncryptString(string inputText)
        {
            //获得输入的缓冲字节流
            byte[] inputBytes = _encodingMode.GetBytes(inputText);
            byte[] outputBytes = EncryptByte(inputBytes);
            //string resultText = _encodingMode.GetString(outputBytes);//利用Unicode表示的密文解密时报错：数据长度有问题，所以利用Base64转换
            string resultText = Convert.ToBase64String(outputBytes);
            return resultText;

        }

        //解密字符流（字符串）算法，解密后需要经过了Base64逆转换
        /// <summary>
        /// 解密字符流（字符串）算法，解密后需要经过了Base64逆转换。
        /// </summary>
        /// <param name="inputText">加密后的文本（默认需要经过了Base64可逆转换）</param>
        /// <returns>解密后的文本</returns>
        public string DecryptString(string inputText)
        {
            try
            {
                //由于上一步加密后的密文利用Base64转换，所以先利用Base64逆运算
                //如果不是合法的Base64字符串编码抛出自定义异常
                byte[] inputBytes = Convert.FromBase64String(inputText);
                byte[] outputBytes = DecryptByte(inputBytes);
                //将解密后的字节Array按照Unicode转换
                string resultText = _encodingMode.GetString(outputBytes);
                return resultText;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("不符合Base64字符串编码格式", ex);
            }

        }

        //加密指定的文件,如果成功返回True,否则false
        /// <summary>
        /// 加密指定的文件,如果成功返回True,否则false
        /// </summary>
        /// <param name="inputFilePath">输入文件</param>
        /// <param name="outputFilePath">输出文件</param>
        /// <returns>是否成功</returns>
        public bool EncryptFile(string inputFilePath, string outputFilePath)
        {
            return EncryptFile(inputFilePath, outputFilePath, true);
        }

        //加密指定的文件,如果成功返回True,否则false
        /// <summary>
        /// 加密指定的文件,如果成功返回True,否则false
        /// </summary>
        /// <param name="inputFilePath">输入文件</param>
        /// <param name="outputFilePath">输出文件</param>
        /// <param name="converBase64">是否需要转换成Base64，防止显示乱码</param>
        /// <returns>是否成功</returns>
        public bool EncryptFile(string inputFilePath, string outputFilePath, bool converBase64)
        {
            bool isSuccess = false;
            bool isExist = File.Exists(inputFilePath);
            if (isExist)//如果存在
            {
                //string inputString;
                //string outputString;

                byte[] inputBytes;
                byte[] outputBytes;

                try
                {
                    //inputString = File.ReadAllText(inputFilePath, _encodingMode);
                    inputBytes = File.ReadAllBytes(inputFilePath);

                }
                catch (Exception ex)
                {
                    throw new ApplicationException(ex.Message + " 读取文件内容出现异常误");//抛出自定义的异常
                }

                try
                {
                    //outputString = EncryptString(inputString);
                    outputBytes = EncryptByte(inputBytes);
                }
                catch (Exception ex)
                {
                    throw new ApplicationException(ex.Message + " 加密文件内容过程出现异常");//抛出自定义的异常
                }

                try
                {
                    if (converBase64)
                    {
                        string outputString = Convert.ToBase64String(outputBytes);
                        File.WriteAllText(outputFilePath, outputString, _encodingMode);
                    }
                    else
                    {
                        File.WriteAllBytes(outputFilePath, outputBytes);
                    }
                    isSuccess = true;
                }
                catch (Exception ex)
                {
                    throw new ApplicationException(ex.Message + " 把密文写入文件过程出现异常");//抛出自定义的异常
                }
                return isSuccess;
            }
            else
            {
                throw new ApplicationException("没有找到指定的文件，导致异常");
            }
        }

        //解密指定的文件,如果成功返回True,否则false
        /// <summary>
        /// 解密指定的文件,如果成功返回True,否则false
        /// </summary>
        /// <param name="inputFilePath">输入文件</param>
        /// <param name="outputFilePath">输出文件</param>
        /// <returns>是否成功</returns>
        public bool DecryptFile(string inputFilePath, string outputFilePath)
        {
            return DecryptFile(inputFilePath, outputFilePath, true);
        }

        //解密指定的文件,如果成功返回True,否则false
        /// <summary>
        /// 解密指定的文件,如果成功返回True,否则false
        /// </summary>
        /// <param name="inputFilePath">输入文件</param>
        /// <param name="outputFilePath">输出文件</param>
        /// <param name="converBase64">是否需要转换成Base64，防止显示乱码</param>
        /// <returns>是否成功</returns>
        public bool DecryptFile(string inputFilePath, string outputFilePath, bool converBase64)
        {
            bool isSuccess = false;
            bool isExist = File.Exists(inputFilePath);
            if (isExist)//如果存在
            {
                //string inputString;
                //string outputString;
                byte[] inputBytes;
                byte[] outputBytes;

                try
                {
                    if (converBase64)
                    {
                        string inputString = File.ReadAllText(inputFilePath, _encodingMode);
                        inputBytes = Convert.FromBase64String(inputString);
                    }
                    else
                    {
                        //inputString = File.ReadAllText(inputFilePath, _encodingMode);
                        inputBytes = File.ReadAllBytes(inputFilePath);
                    }
                }
                catch (Exception ex)
                {
                    throw new ApplicationException(ex.Message + " 读取文件内容出现异常误");//抛出自定义的异常
                }

                try
                {
                    //outputString = DecryptString(inputString);
                    outputBytes = DecryptByte(inputBytes);
                }
                catch (Exception ex)
                {
                    throw new ApplicationException(ex.Message + " 解密文件内容过程出现异常");//抛出自定义的异常
                }

                try
                {
                    //File.WriteAllText(outputFilePath, outputString, _encodingMode);
                    File.WriteAllBytes(outputFilePath, outputBytes);
                    isSuccess = true;
                }
                catch (Exception ex)
                {
                    throw new ApplicationException(ex.Message + " 把解密后的内容写入文件过程出现异常");//抛出自定义的异常
                }
                return isSuccess;
            }
            else
            {
                throw new ApplicationException("没有找到指定的文件，导致异常");
            }

        }

        //文件加密函数的重载版本,如果不指定输出路径,那么原来的文件将被加密后的文件覆盖
        /// <summary>
        /// 文件加密函数的重载版本,如果不指定输出路径,
        /// 那么原来的文件将被加密后的文件覆盖
        /// </summary>
        /// <param name="filePath">需要加密的文件名</param>
        public bool EncryptFile(string filePath)
        {
            return this.EncryptFile(filePath, filePath);
        }

        //文件解密函数的重载版本,如果不指定输出路径,那么原来的文件将被加密后的文件覆盖
        /// <summary>
        /// 解密文件的重载版本,如果没有给出解密后文件的输出路径,
        /// 则解密后的文件将覆盖先前的文件
        /// </summary>
        /// <param name="filePath"></param>
        public bool DecryptFile(string filePath)
        {
            return this.DecryptFile(filePath, filePath);
        }

        #endregion
    }
}
