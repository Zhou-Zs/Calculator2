using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Calculator
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // 1、读取计算文件文件
                using var streamReader = new StreamReader("./input.txt");
                var lines = new List<string>();
                var line = string.Empty;

                while ((line = streamReader.ReadLine()) != null)
                {
                    lines.Add(line);
                }

                // 2、组装中缀表达式
                var inorderList =  CalculatorService.ToInorder(lines);

                // 3、中缀表达式转后缀表达式
                var postorderList = CalculatorService.InorderToPostorder(inorderList);

                // 4、计算值
                var result = CalculatorService.ExpValue(postorderList);

                Console.WriteLine(result);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadKey();
        }
    }
}


