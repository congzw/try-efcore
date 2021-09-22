using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Common.Encrypts
{
    /// <summary>
    /// 该类提供了对hash算法的常见任务的封装
    /// 其中包含了Byte[]和string以及文件之间的
    /// hash运算，编码格式使用的是Base64
    /// </summary>
    public class MyHashHelper
    {
        #region 私有字段

        private HashAlgorithm _hashAlgorithm; //哈希算法

        #endregion

        #region 属性

        public HashAlgorithm HashAlgorithm
        {
            get { return _hashAlgorithm; }
            set { _hashAlgorithm = value; }
        }

        #endregion

        #region 构造函数

        //构造函数，可以自行指定哈希算法，MD4，MD5，SHA-0，SHA-1已被证明不安全，默认采用SHA-2算法
        /// <summary>
        /// 构造函数，默认采用SHA256Managed算法
        /// </summary>
        public MyHashHelper()
            : this(new SHA256Managed())
        { }

        //构造函数，可以自行指定哈希算法，MD4，MD5，SHA-0，SHA-1已被证明不安全，建议采用SHA2以上算法
        /// <summary>
        ///  构造函数，可以自行指定哈希算法，MD4，MD5，SHA-0，SHA-1已被证明不安全，建议采用SHA2以上算法
        /// </summary>
        /// <param name="hashAlgorithm">MD4，MD5，SHA-0，SHA-1已被证明不安全，建议采用SHA2以上算法</param>
        public MyHashHelper(HashAlgorithm hashAlgorithm)
        {
            _hashAlgorithm = hashAlgorithm;
        }

        #endregion

        #region 公开方法

        //哈希计算方法，以Byte[]为计算和输出单位
        /// <summary>
        /// 哈希计算方法，以Byte[]为计算和输出单位
        /// </summary>
        /// <param name="inputBytes">输入字节数组</param>
        /// <returns>代表哈希结果的字节数组</returns>
        public byte[] ComputeHash(byte[] inputBytes)
        {
            return _hashAlgorithm.ComputeHash(inputBytes);
        }

        //哈希计算方法，以Byte[]为计算单位，以string为输出单位
        /// <summary>
        /// 哈希计算方法，以string为输出单位
        /// </summary>
        /// <param name="inputBytes">输入字节数组</param>
        /// <returns>代表哈希结果的字节数组转换后的字符串</returns>
        public string ComputeHashReturnString(byte[] inputBytes)
        {
            return Convert.ToBase64String(ComputeHash(inputBytes));
        }

        //哈希计算方法，以string为计算和输出单位
        /// <summary>
        /// 哈希计算方法，以string为计算和输出单位，采用Encoding.Unicode编码格式提取
        /// </summary>
        /// <param name="inputString">输入String，采用Encoding.Unicode编码格式提取</param>
        /// <returns>代表哈希结果的String，采用Base64表示</returns>
        public string ComputeHash(string inputString)
        {
            return Convert.ToBase64String(ComputeHash(Encoding.Unicode.GetBytes(inputString)));
        }

        //哈希计算方法，以file为计算,byte[]为输出单位
        /// <summary>
        /// 哈希计算方法，以file为计算,byte[]为输出单位
        /// </summary>
        /// <param name="filePath">待hash文件路径</param>
        /// <returns>hash值</returns>
        public byte[] ComputeFileHash(string filePath)
        {
            byte[] hashBytes = null;
            bool isExist = File.Exists(filePath);
            if (isExist)//如果存在
            {
                byte[] inputBytes;
                try
                {
                    inputBytes = File.ReadAllBytes(filePath);

                }
                catch (Exception ex)
                {
                    throw new ApplicationException(ex.Message + " 读取文件内容出现异常误");//抛出自定义的异常
                }

                try
                {
                    hashBytes = ComputeHash(inputBytes);
                }
                catch (Exception ex)
                {
                    throw new ApplicationException(ex.Message + " 计算hash过程出现异常");//抛出自定义的异常
                }
                return hashBytes;
            }
            else
            {
                throw new ApplicationException("没有找到指定的文件，导致异常");
            }
        }

        //哈希计算方法，以file为计算,string为输出单位
        /// <summary>
        /// 哈希计算方法，以file为计算,string为输出单位
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>代表哈希结果的String，采用Base64表示</returns>
        public string ComputeFileHashReturnString(string filePath)
        {
            return Convert.ToBase64String(ComputeFileHash(filePath));
        }

        #endregion

        /// <summary>
        /// 匹配Java的MD5算法
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public string JavaMd5(string s)
        {
            MD5 md = new MD5CryptoServiceProvider();
            byte[] ss = md.ComputeHash(Encoding.UTF8.GetBytes(s));
            return ByteArrayToHexString(ss);  
        }
        private static readonly string[] HexCode = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b", "c", "d", "e", "f" };  
        public static string ByteToHexString(byte b)
        {
            int n = b;
            if (n < 0)
            {
                n = 256 + n;
            }
            int d1 = n / 16;
            int d2 = n % 16;
            return HexCode[d1] + HexCode[d2];
        }

        public static String ByteArrayToHexString(byte[] b)
        {
            String result = "";
            for (int i = 0; i < b.Length; i++)
            {
                result = result + ByteToHexString(b[i]);
            }
            return result;
        }  
    }
}
