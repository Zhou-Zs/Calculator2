using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Calculator
{
    /// <summary>
    /// 计算服务
    /// </summary>
    public class CalculatorService
    {
        /// <summary>
        /// 组装中缀表达式
        /// </summary>
        /// <param name="strList"></param>
        /// <returns></returns>
        public static List<string> ToInorder(List<string> strList)
        {
            string expElement = string.Empty;       // 参与计算的数字
            var inorderList = new List<string>(); 
            var expStr = strList[0];                // 获取计算公式

            for (int i = 0; i < expStr.Length; i++)
            {
                var expchar = expStr.Substring(i, 1);

                // 替换字母为所赋的值
                for (int l = 1; l < strList.Count; l++)
                {
                    var oValue = Regex.Split(strList[l], "=");
                    if (expchar == oValue[0])
                    {
                        expchar = oValue[1];
                        break;
                    }
                }

                if (char.IsNumber(expStr, i) || expchar == "." || (expchar == "-" && (i == 0 || expStr.Substring(i - 1, 1) == "(")))
                {
                    // 拼接参与计算的数字
                    expElement += expchar;
                }
                else
                {
                    if (!string.IsNullOrEmpty(expElement))
                    {
                        inorderList.Add(expElement);
                    }

                    inorderList.Add(expchar);
                    expElement = string.Empty;
                }
            }

            if (!string.IsNullOrEmpty(expElement))
            {
                inorderList.Add(expElement);
            }

            return inorderList;
        }

        /// <summary>
        /// 中缀表达式转后缀表达式
        /// </summary>
        /// <param name="inorderList"></param>
        /// <returns></returns>
        public static List<string> InorderToPostorder(List<string> inorderList)
        {
            var postorderList = new List<string>();
            var operatorsStack = new Stack();

            foreach (string itemStr in inorderList)
            {
                if (IsNumber(itemStr))
                {
                    postorderList.Add(itemStr);
                }
                else
                {
                    switch (itemStr)
                    {
                        case "+":
                        case "-":
                        case "*":
                        case "/":
                            while (IsPop(itemStr, operatorsStack))
                            {
                                postorderList.Add(operatorsStack.Pop().ToString());
                            }
                            operatorsStack.Push(itemStr);
                            break;
                        case "(":
                            operatorsStack.Push(itemStr);
                            break;
                        case ")":
                            while (operatorsStack.Count != 0)
                            {
                                var operatorsStr = operatorsStack.Pop().ToString();
                                if (operatorsStr != "(")
                                {
                                    postorderList.Add(operatorsStr);
                                }
                                else
                                {
                                    break;
                                }
                            }
                            break;
                    }
                }
            }

            while (operatorsStack.Count != 0)
            {
                postorderList.Add(operatorsStack.Pop().ToString());
            }

            return postorderList;
        }

        /// <summary>
        /// 计算后缀表达式的值
        /// </summary>
        /// <param name="postorderList"></param>
        /// <returns></returns>
        public static double ExpValue(List<string> postorderList)
        {
            double result = 0;
            var num = new Stack();
            foreach (string item in postorderList)
            {
                if (IsNumber(item))
                {
                    num.Push(item);
                }
                else
                {
                    var num2 = Convert.ToDouble(num.Pop());
                    var num1 = Convert.ToDouble(num.Pop());
                    result = Arithmetic(num1, num2, item);
                    num.Push(result);
                }
            }
            return result;
        }

        /// <summary>
        /// 检查数字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNumber(string str)
        {
            bool isNumber = false;

            if (str == null || str.Length == 0)
            {
                return false;
            }
               
            var ascii = new ASCIIEncoding();
            byte[] bt = ascii.GetBytes(str);

            for (int i = 0; i < bt.Length; i++)
            {
                if ((bt[i] >= 48 && bt[i] <= 57) || bt[i] == 46)
                {
                    isNumber = true;
                }
                else
                {
                    if (bt[i] == 45 && bt.Length > 1)
                    {
                        isNumber = true;
                    }
                    else
                    {
                        isNumber = false;
                        break;
                    }
                }
            }

            return isNumber;
        }

        /// <summary>
        /// 一目计算
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="operators"></param>
        /// <returns></returns>
        public static double Arithmetic(double a, double b, string operators)
        {
            return operators switch
            {
                "+" => a + b,
                "-" => a - b,
                "*" => a * b,
                "/" => a / b,
                _ => 0,
            };
        }

        /// <summary>
        /// 优先级
        /// </summary>
        /// <param name="op"></param>
        /// <returns></returns>
        private static int Operatororder(string operators)
        {
            return operators switch
            {
                "+" => 1,
                "-" => 2,
                "*" => 3,
                "/" => 4,
                _ => 0,
            };
        }

        private static bool IsPop(string operators, Stack operatorsStack)
        {
            if (operatorsStack.Count == 0)
            {
                return false;
            }
            else
            {
                if (operatorsStack.Peek().ToString() == "(" || Operatororder(operators) > Operatororder(operatorsStack.Peek().ToString()))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
    }
}
