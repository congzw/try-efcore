using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Common.Encrypts
{
    /// <summary>
    /// 基于安全的随机数发生器，用于高安全性要求，区别于伪随机数发生器类Random
    /// </summary>
    public class MyRandomHelper
    {
        private readonly RNGCryptoServiceProvider _provider;

        private byte[] _randomBytes;

        //随机数生成器的构造函数
        /// <summary>
        /// 随机数生成器的构造函数
        /// </summary>
        public MyRandomHelper()
        {
            _provider = new RNGCryptoServiceProvider();
        }

        //0~255之间的随机数
        /// <summary>
        /// 0~255之间的随机数
        /// </summary>
        /// <returns>0~255之间的随机数</returns>
        public int GetRandomNumber()
        {
            _randomBytes = new byte[1];
            _provider.GetBytes(_randomBytes);
            return Convert.ToInt32(_randomBytes[0]);
        }

        //非伪随机数发生器，实现了由RNG安全加密服务提供者产生的任意随机数（范围十进制，比如小于0~9,0~99,0~999,9999等）
        /// <summary>
        /// 非伪随机数发生器，实现了由RNG安全加密服务提供者产生的任意随机数（范围十进制，比如小于0~9,0~99,0~999,9999等）
        /// </summary>
        /// <param name="decimalCount">需要产生10^decimalCount的范围（1表示10^1位，0-9，2表示10^2位，0~99，以此类推）</param>
        /// <returns>按需产生的随机数</returns> 
        public int GetRandomNumber(int decimalCount)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < decimalCount; i++)
            {
                int rand = GetRandomNumber();
                rand = rand % 10;
                sb.Append(rand);
            }
            return Convert.ToInt32(sb.ToString());
        }

        //随机取得0~9之间的一个数的方法
        /// <summary>
        /// 取得0~9之间的任意一个数
        /// </summary>
        /// <returns>0~9之间的任意一个数</returns>
        public int GetANumber()
        {
            return GetRandomNumber(1);
        }

        //产生small和big之间的随机数（包括samll和big）
        /// <summary>
        /// 获取small和big之间的随机数（包括samll和big）
        /// </summary>
        /// <param name="small"></param>
        /// <param name="big"></param>
        /// <returns></returns>
        public int GetRandomBetween(int small, int big)
        {
            //如果输入顺序错误，纠正
            if (small > big)
            {
                int temp = small;
                small = big;
                big = temp;
            }
            //按大数和小数的离散区间的实际离散数量够造随机数，big-samll的结果长度已经代表了种子随机数的位数，不需要+1
            int rand = GetRandomNumber((big - small).ToString().Length);
            //根据需要取余数截断
            //(rand % (big - small + 1))中的“+1”是为了保证不会有随机产生的余数边界丢失情况
            rand = (rand % (big - small + 1)) + small;
            return rand;
        }

        //0~255之间的随机数(以Byte类型返回)
        /// <summary>
        /// 0~255之间的随机数(以Byte类型返回)
        /// </summary>
        /// <returns>0~255之间的随机数(以Byte类型返回)</returns>
        public byte GetRandomByte()
        {
            _randomBytes = new byte[1];
            _provider.GetBytes(_randomBytes);
            return _randomBytes[0];
        }

        //0~255之间的char随机数(以string类型返回)
        /// <summary>
        /// 0~255之间的char随机数(以string类型返回)
        /// </summary>
        /// <returns>0~255之间的char随机数(以string类型返回)</returns>
        public string GetRandomChar()
        {
            _randomBytes = new byte[1];
            _provider.GetBytes(_randomBytes);
            return Encoding.ASCII.GetString(_randomBytes);
        }

        //samllCharASCIICode~bigCharASCIICode之间的KeyChar随机数(以string类型返回),要求33~126之间，随机密码使用
        /// <summary>
        /// samllCharASCIICode~bigCharASCIICode之间的KeyChar随机数(以string类型返回),要求33~126之间，随机密码使用
        /// </summary>
        /// <param name="smallCharAsciiCode">ASCII码下界</param>
        /// <param name="bigCharAsciiCode">ASCII码上界</param>
        /// <returns>生成的随机字符，以string返回</returns>
        public string GetRandomKeyChar(int smallCharAsciiCode, int bigCharAsciiCode)
        {
            if (smallCharAsciiCode < 33 || bigCharAsciiCode > 126)
            {
                GetRandomKeyChar(33, 126);
            }
            _randomBytes = new byte[1];
            int temp = GetRandomBetween(smallCharAsciiCode, bigCharAsciiCode);
            byte[] bytes = new byte[] { (byte)temp };
            string charString = Encoding.ASCII.GetString(bytes);
            return charString;
        }

        // 默认的KeyChar随机数生成方法(以string类型返回),返回范围是要求33~126的之间，随机密码使用
        /// <summary>
        /// 默认的KeyChar随机数生成方法(以string类型返回),返回范围是要求33~126的之间，随机密码使用
        /// </summary>
        /// <returns>生成的随机字符，以string返回</returns>
        public string GetRandomKeyChar()
        {
            return GetRandomKeyChar(33, 126);
        }

        //samllCharASCIICode~bigCharASCIICode之间的KeyChar随机数组成的密码串儿(以string类型返回)，建议33~126间，随机密码使用
        /// <summary>
        ///  samllCharASCIICode~bigCharASCIICode之间的KeyChar随机数组成的密码串儿(以string类型返回)，建议33~126间，随机密码使用
        ///  A - Z, a -z(65 - 90, 97-122)
        /// </summary>
        /// <param name="keySize">密码位数</param>
        /// <param name="smallCharAsciiCode">ASCII码下界</param>
        /// <param name="bigCharAsciiCode">ASCII码上界</param>
        /// <returns>生成密码</returns>
        public string GetRandomKey(int keySize, int smallCharAsciiCode, int bigCharAsciiCode)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < keySize; i++)
            {
                sb.Append(GetRandomKeyChar(smallCharAsciiCode, bigCharAsciiCode));
            }
            return sb.ToString();
        }

        // 默认samllCharASCIICode~bigCharASCIICode之间的KeyChar随机数组成的密码串儿(以string类型返回)，采用建议的33~126间，随机密码使用
        /// <summary>
        /// 默认samllCharASCIICode~bigCharASCIICode之间的KeyChar随机数组成的密码串儿(以string类型返回)，采用建议的33~126间，随机密码使用
        /// </summary>
        /// <param name="keySize">密码位数</param>
        /// <returns>生成密码</returns>
        public string GetRandomKey(int keySize)
        {
            return GetRandomKey(keySize, 33, 126);
        }
        
        /// <summary>
        /// 生成一个随机的列表，内含从0到n个数中的随机值
        /// </summary>
        /// <param name="count"></param>
        /// <param name="randoms"></param>
        public void GetRandomList(int count, ref List<int> randoms)
        {
            if (randoms == null)
            {
                randoms = new List<int>();
            }

            int random = GetRandomBetween(0, count - 1);
            if (!randoms.Contains(random))
            {
                randoms.Add(random);
            }

            if (randoms.Count < count)
            {
                GetRandomList(count, ref randoms);
            }
            return;
        }

        /// <summary>
        /// 生成一个随机的列表，内含allCount个值，其中包含着partsCount个随机值
        /// </summary>
        /// <param name="partsCount"></param>
        /// <param name="allCount"></param>
        /// <param name="randoms"></param>
        public void GetRandomParts(int partsCount, int allCount, ref List<int> randoms)
        {
            if (randoms == null)
            {
                randoms = new List<int>();
            }

            int random = GetRandomBetween(0, allCount - 1);
            if (!randoms.Contains(random))
            {
                randoms.Add(random);
            }

            if (randoms.Count < partsCount)
            {
                GetRandomParts(partsCount, allCount, ref randoms);
            }
            return;
        }
    }

}
