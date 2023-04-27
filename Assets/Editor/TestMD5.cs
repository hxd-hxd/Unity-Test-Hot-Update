// -------------------------
// 创建日期：2023/3/1 16:16:29
// -------------------------

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Text;
using System.Linq;
using UnityEditor;

namespace Test
{
    public class TestMD5
    {
        [MenuItem("Test/MD5")]
        static void Test()
        {
            Debug.Log("-----------------------原始数据-----------------------");

            // 原始数据
            string text = "下九幽";
            byte[] data = Encoding.ASCII.GetBytes(text);

            // 预留 2位 md5 验证
            byte[] result = new byte[data.Length + 2];
            for (int i = 0; i < data.Length; i++)
            {
                result[i + 2] = data[i];
            }

            // 生成 md5 码
            byte[] md5Code = MD5Security.GetMD5Buffer(result);
            // 保留前两位
            result[0] = md5Code[0];
            result[1] = md5Code[1];
            

            // 解析 MD5 验证
            Verify(result);
        }
        static void Verify(byte[] result)
        {
            Debug.Log("-----------------------接收验证-----------------------");

            // 获取 md5 码
            byte[] md5Code =  { result[0], result[1] };
            result[0] = result[1] = 0;

            // 计算 md5
            byte[] verifyMD5 = MD5Security.GetMD5Buffer(result);

            // 比较两个 md5 是否相同
            Debug.Log(
                $"获取的 md5 ： {md5Code[0]}，{md5Code[1]}" +
                $"{Environment.NewLine}" +
                $"计算的 md5 ： {verifyMD5[0]}，{verifyMD5[1]}");
        }
    }

    /// <summary>
    /// MD5
    /// </summary>
    public static class MD5Security
    {
        
        /// <summary>
        /// 计算文件的MD5值
        /// </summary>
        /// <param name="md5File">MD5签名文件字符数组</param>
        /// <returns>计算结果</returns>
        public static byte[] GetMD5Buffer(byte[] md5File)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider get_md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            //byte[] hash_byte = get_md5.ComputeHash(md5File, 0, md5File.Length);
            byte[] hash_byte = get_md5.ComputeHash(md5File);


            StringBuilder sb = new StringBuilder("MD5 计算结果").Append("\t大小：").Append(hash_byte.Length).AppendLine();
            for (int i = 0; i < hash_byte.Length; i++)
            {
                sb.Append(hash_byte[i]).Append("\t");
            }
            Debug.Log(sb.ToString());

            return hash_byte;
        }

        /// <summary>
        /// 计算文件的MD5值
        /// </summary>
        /// <param name="md5File">MD5签名文件字符数组</param>
        /// <param name="index">计算起始位置</param>
        /// <param name="count">计算终止位置</param>
        /// <returns>计算结果</returns>
        private static string MD5Buffer(byte[] md5File, int index, int count)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider get_md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] hash_byte = get_md5.ComputeHash(md5File, index, count);
            string result = System.BitConverter.ToString(hash_byte);

            result = result.Replace("-", "");
            return result;
        } 

    }
}