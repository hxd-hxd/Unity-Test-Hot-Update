// -------------------------
// 创建日期：2021/8/3 16:52:10
// -------------------------

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;

using Object = UnityEngine.Object;
using static BatchOperationToolsUtility.BOTConstant;
using static BatchOperationToolsUtility.BOTGlobalVariable;
using System.Text;

namespace BatchOperationToolsUtility
{
    /// <summary>
    /// 批量操作工具的实用函数集
    /// </summary>
    public static partial class BOTUtility
    {
        /// <summary>
        /// 启用条件
        /// </summary>
        public static bool Enable 
        {
            get
            {
                return !EditorApplication.isPlaying;

//#if UNITY_EDITOR
//                return !EditorApplication.isPlaying;
//#else
//                return false;
//#endif
            }
        }

        /// <summary>
        /// 批量修改或添加命名空间工具的实用函数集
        /// </summary>
        public static class BMANUtility
        {
            /// <summary>
            /// 根据文本获取一个 操作项实例
            /// </summary>
            /// <param name="ta"></param>
            /// <returns></returns>
            public static OperationItem GetOperationItem(TextAsset ta)
            {
                OperationItem item = null;

                if (IsPerfect(ta))
                {
                    item = new OperationItem()
                    {
                        file = ta
                    };
                }

                return item;
            }

            /// <summary>
            /// 该文本文件是否符合条件
            /// </summary>
            /// <param name="ta"></param>
            /// <returns></returns>
            public static bool IsPerfect(TextAsset obj)
            {
                return BOTConstant.FileExtensionList.Contains(Path.GetExtension(AssetDatabase.GetAssetPath(obj)));
            }
            /// <summary>
            /// 该文本文件是否符合条件
            /// </summary>
            /// <param name="ta"></param>
            /// <returns></returns>
            public static bool IsPerfect(UnityEngine.Object obj)
            {
                return BOTConstant.FileExtensionList.Contains(Path.GetExtension(AssetDatabase.GetAssetPath(obj)));
            }

            /// <summary>
            /// 执行对命名空间的
            /// </summary>
            /// <param name="namespaceFileList"></param>
            public static void ExecutionNamespace(List<OperationItem> namespaceFileList)
            {

            }
        }



        /// <summary>
        /// 对操作项列表执行修改
        /// </summary>
        /// <param name="namespaceFileList"></param>
        public static void Execution(List<OperationItem> namespaceFileList)
        {
            for (int i = 0; i < namespaceFileList.Count; i++)
            {
                Execution(namespaceFileList[i].pathFull);
            }
        }
        public static void Execution(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);
            string newValue = "";

            if (CanExecutionOperationModify(lines))
            {
                newValue = ExecutionOperationModify(lines);
            }
            else if (CanExecutionOperationAdd(lines))
            {
                newValue = ExecutionOperationAdd(lines);
            }

            File.WriteAllText(filePath, newValue);
        }

        // 可否执行修改操作
        public static bool CanExecutionOperationModifyPath(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);
            return CanExecutionOperationModify(lines);
        }
        // 可否执行修改操作
        public static bool CanExecutionOperationModify(params string[] values)
        {
            if (modify)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    if (CanExecutionOperationModify(values[i]))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        // 可否执行修改操作
        public static bool CanExecutionOperationModify(string value)
        {
            if (modify)
            {
                return IsNamespaceLine(value);
            }

            return false;
        }

        // 可否执行添加操作
        public static bool CanExecutionOperationAddPath(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);
            return CanExecutionOperationAdd(lines);
        }
        // 可否执行添加操作
        public static bool CanExecutionOperationAdd(params string[] values)
        {
            if (add)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    if (CanExecutionOperationAdd(values[i]))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        // 可否执行添加操作
        public static bool CanExecutionOperationAdd(string value)
        {
            if (add)
            {
                return !IsNamespaceLine(value);
            }

            return false;
        }

        // 执行修改操作
        public static string ExecutionOperationModify(string[] values)
        {
            return NamespaceModify(namespaceValue, values);
        }
        // 执行添加操作
        public static string ExecutionOperationAdd(string[] values)
        {
            return NamespaceAdd(namespaceValue, values);
        }


        /// <summary>
        /// 是否是定义命名空间的行
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNamespaceLine(string value)
        {
            if (value.Contains(ns))
            {
                string[] strs = value.Split(ns);
                if (strs[0].Contains("/"))
                {
                    string[] vs = strs[0].Split("\r\n");
                    string last = vs[vs.Length - 1];
                    if (last.Contains("//"))
                    {
                        return false;
                    }

                    if (last.Contains("/*") && !last.Contains("*/"))
                    {
                        return false;
                    }

                    return true;
                }

                return true;
            }

            return false;
        }
        /// <summary>
        /// 是否已有命名空间
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        public static bool IsHaveNamespace(params string[] lines)
        {
            for (int i = 0; i < lines.Length; i++)
            {
                if (IsNamespaceLine(lines[i]))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 是否是定义类型的行
        /// <para>ps：class、struct、interface、enum</para>
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static bool IsTypeLine(string line)
        {
            if (line.ContainsOne("class", "struct", "interface", "enum"))
            {
                string[] split = null;

                if (line.Contains("//"))
                {
                    line = line.Split("//")[0];
                }
                //else if (line.Contains("/*"))
                //{
                //    line = line.Split("/*")[0];
                //}

                if (line.ToLower().Contains("class")) split = line.Split("class");
                else if (line.ToLower().Contains("struct")) split = line.Split("struct");
                else if (line.ToLower().Contains("interface")) split = line.Split("interface");
                else if (line.ToLower().Contains("enum")) split = line.Split("enum");


                if (split == null)
                {
                    return false;
                }

                if (split.Length <= 1)
                {
                    return false;
                }

                if (split[0].ContainsOne("//", "/*"))
                {
                    return false;
                }

                return true;
            }

            return false;
        }
        /// <summary>
        /// 是否是定义类的行
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static bool IsClassLine(string line)
        {
            if (line.ToLower().Contains("class"))
            {
                string[] split = line.Split("class");

                if (split.Length <= 1) return false;
                if (split[0].ContainsOne("//", "/*"))
                {
                    return false;
                }

                return true;
            }

            return false;
        }
        /// <summary>
        /// 是否是定义结构的行
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static bool IsStructLine(string line)
        {
            if (line.ToLower().Contains("struct"))
            {
                string[] split = line.Split("struct");

                if (split.Length <= 1) return false;
                if (split[0].ContainsOne("//", "/*"))
                {
                    return false;
                }

                return true;
            }

            return false;
        }
        /// <summary>
        /// 是否是定义接口的行
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static bool IsInterfaceLine(string line)
        {
            if (line.ToLower().Contains("interface"))
            {
                string[] split = line.Split("interface");

                if (split.Length <= 1) return false;
                if (split[0].ContainsOne("//", "/*"))
                {
                    return false;
                }

                return true;
            }

            return false;
        }
        /// <summary>
        /// 是否是定义枚举的行
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static bool IsEnumLine(string line)
        {
            if (line.ToLower().Contains("enum"))
            {
                string[] split = line.Split("enum");

                if (split.Length <= 1) return false;
                if (split[0].ContainsOne("//", "/*"))
                {
                    return false;
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// 是否是添加引用的行
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static bool IsUsingLine(string line)
        {
            if (line.ToLower().Contains("using"))
            {
                string[] split = line.Split("using");

                if (split.Length <= 1) return false;
                if (split[0].ContainsOne("//", "/*"))
                {
                    return false;
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// 是否是定义特性的行
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static bool IsAttributeLine(string line)
        {
            line = line.Remove(" ", "\t", "\r");
            if (line[0] != '[')
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 是否是注释行
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static bool IsCommentsLine(string line)
        {
            line = line.Remove(" ", "\t", "\r");
            if (line[0] == '/')
            {
                return false;
            }

            return true;
        }


        /// <summary>
        /// 获取命名空间名称
        /// </summary>
        /// <param name="line">包含命名空间的行</param>
        /// <returns></returns>
        public static string GetNamespaceValue(string line)
        {
            if (!IsNamespaceLine(line)) return null;

            string nsValue = "";// 修改后的命名空间名称

            string[] lines = line.Split(ns);
            string[] strs = null;

            // 得到命名空间关键字 namespace 两端的值
            string Front = lines[0];
            string queen = lines[1];

            char sign = '/';        // 拆分符
            if (queen.ContainsAll("/", "{"))
            {

                for (int i = 0; i < queen.Length; i++)
                {
                    if (queen[i] == '/')
                    {
                        sign = '/';
                        break;
                    }
                    else if (queen[i] == '{')
                    {
                        sign = '{';
                        break;
                    }
                }

                strs = queen.Split(sign);
            }
            else if (queen.Contains("{"))
            {
                strs = queen.Split("{");

                sign = '{';
            }
            else if (queen.Contains("/"))
            {
                strs = queen.Split("/");

                sign = '/';
            }
            else
            {
                nsValue = queen.RemoveEmpty();
            }

            if (strs != null)
            {
                nsValue = strs[0].RemoveEmpty();
            }

            return nsValue;
        }
        /// <summary>
        /// 修改命名空间
        /// </summary>
        /// <param name="line">包含命名空间的行</param>
        /// <param name="nsValue">要修改成命名空间名称</param>
        /// <returns></returns>
        public static string NamespaceModify(string line, string nsValue)
        {
            string nm = line;
            if (!IsNamespaceLine(line)) return nm;

            string nsValueNew = string.Format(" {0} ", nsValue);// 修改后的命名空间名称

            string[] lines = line.Split(ns);
            string[] strs = null;

            // 得到命名空间关键字 namespace 两端的值
            string Front = lines[0];
            string queen = lines[1];

            string newline = "\r\n";// 换行
            char sign = '/';        // 拆分符
            if (queen.ContainsAll("/", "{"))
            {

                for (int i = 0; i < queen.Length; i++)
                {
                    if (queen[i] == '/')
                    {
                        sign = '/';
                        newline = "";
                        break;
                    }
                    else if (queen[i] == '{')
                    {
                        sign = '{';
                        break;
                    }
                }

                strs = queen.Split(sign);
                nsValueNew += newline;
            }
            else if (queen.Contains("{"))
            {
                strs = queen.Split("{");
                nsValueNew += newline;

                sign = '{';
            }
            else if (queen.Contains("/"))
            {
                strs = queen.Split("/");
                //nsValueNew += newline;

                sign = '/';
            }
            else
            {
                //nsValueNew += newline;
            }

            StringBuilder sb = new StringBuilder();
            sb.Append(Front).Append(ns).Append(nsValueNew);

            if (strs != null)
            {
                for (int i = 1; i < strs.Length; i++)
                {
                    sb.Append(sign).Append(strs[i]);
                }
            }
            else
            {
                //sb.Append("\r\n");
            }

            nm = sb.ToString();

            return nm;
        }
        /// <summary>
        /// 修改命名空间
        /// </summary>
        /// <param name="line">包含命名空间的行</param>
        /// <param name="nsValue">要修改成命名空间名称</param>
        /// <returns></returns>
        public static string NamespaceModify(string nsValue, params string[] lines)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = NamespaceModify(lines[i], nsValue);
                sb.Append(lines[i]).Append("\r\n");
            }

            return sb.ToString();
        }

        /// <summary>
        /// 添加命名空间
        /// </summary>
        /// <param name="values">包含完整代码内容的数组</param>
        /// <param name="nsValue">命名空间值</param>
        /// <returns></returns>
        public static string NamespaceAdd(string nsValue, params string[] values)
        {
            StringBuilder sb = new StringBuilder();
            List<string> lines = new List<string>();

            #region 弃用
            //// 记录定义类的代码行的索引
            //// 说明可在此行之前添加命名空间
            //int indexClass = 0;

            //bool addFinish = false;

            //for (int i = 0; i < values.Length - 1; i++)
            //    {
            //    string value = values[i];
            //    if (!addFinish)
            //    {
            //        // 空行直接添加
            //        if (string.IsNullOrWhiteSpace(value))
            //        {
            //            lines.Add(value);
            //            continue;
            //        }

            //        // 是注释行
            //        if (IsCommentsLine(value))
            //        {
            //            lines.Add(value);
            //            continue;
            //        }
            //        // 判断特性
            //        if (IsAttributeLine(value))
            //        {
            //            lines.Add(value);
            //            continue;
            //        }

            //        // 引用名直接添加
            //        if (IsUsingLine(value))
            //        {
            //            lines.Add(value);
            //            continue;
            //        }

            //        // 添加命名空间
            //        //sb.Append(ns).Append(" ").Append(namespaceValue).Append("\r\n{\r\n\t");
            //        addFinish = true;
            //        indexClass = i;
            //    }
            //    else
            //    {
            //        lines.Add(value);
            //    }
            //}

            //sb.Append("}"); 
            #endregion

            if (!IsHaveNamespace(values))
            {
                sb.Append("namespace ").Append(nsValue).Append("\r\n{");
                for (int i = 0; i < values.Length; i++)
                {
                    sb.Append("\r\n\t").Append(values[i]);
                }
                sb.Append("\r\n}");
            }
            else
            {
                for (int i = 0; i < values.Length; i++)
                {
                    sb.Append(values[i]).Append("\r\n");
                }
            }

            return sb.ToString();
        }
    }
}