using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
namespace Calc
{
    class Program
    {
        static float memory = 0;

        static void Main(string[] args)
        {
            float one, two, result;
            string sign;
            Console.WriteLine("Welcome to the Calc");
            Console.Write("Input first number: ");
            one = Convert.ToSingle(Console.ReadLine());
            Console.Write("Input action sign: ");
            sign = Console.ReadLine(); 
            Console.Write("Input second number: ");
            two = Convert.ToSingle(Console.ReadLine());
            
            if (sign == "+") 
            {
                result = one + two;
                Console.WriteLine("Sum is: " + result);
                Console.WriteLine("To exit, press any key... ");
                Console.ReadKey();
            }
            else if (sign == "-")
            {
                result = one - two;
                Console.WriteLine("Subtraction is: " + result);
                Console.WriteLine("To exit, press any key... ");
                Console.ReadKey();
            }
            else if (sign == "*")
            {
                result = one * two;
                Console.WriteLine("Product is: " + result); 
                Console.WriteLine("To exit press any key... ");
                Console.ReadKey();
            }
            else if (sign == "%")
            {
                if (two == 0) 
                {
                    Console.WriteLine("Error: Cannot divide by zero!");
                }
                else
                {
                    result = one % two;
                    Console.WriteLine("Remainder is: " + result); 
                }
                Console.WriteLine("To exit press any key...");
                Console.ReadKey();
            }
            else if (sign == "i") 
            {
                if (one == 0) 
                {
                    Console.WriteLine("Error: Cannot calculate 1/0!");
                }
                else
                {
                    result = 1 / one; 
                    Console.WriteLine("1/x result is: " + result); 
                }
                Console.WriteLine("To exit press any key...");
                Console.ReadKey();
            }
            else if (sign == "2") 
            {
                result = one * one;
                Console.WriteLine("This number squared is: " + result); 
                Console.WriteLine("To exit press any key...");
                Console.ReadKey();
            }
            else if (sign == "√") 
            {
                if (one < 0) 
                {
                    Console.WriteLine("Error: Cannot calculate square root of negative number!");
                }
                else
                {
                    result = (float)Math.Sqrt(one); 
                    Console.WriteLine("Square root is: " + result); 
                }
                Console.WriteLine("To exit press any key...");
                Console.ReadKey();
            }
            else if (sign.ToUpper() == "M+")
            {
                memory += one;
                Console.WriteLine("Added " + one + " to memory"); 
                Console.WriteLine("Memory now contains: " + memory); 
                Console.WriteLine("To exit press any key...");
                Console.ReadKey();
            }
            else if (sign.ToUpper() == "M-")
            {
                memory -= one;
                Console.WriteLine("Subtracted " + one + " from memory"); 
                Console.WriteLine("Memory now contains: " + memory); 
                Console.WriteLine("To exit press any key...");
                Console.ReadKey();
            }
            if (Math.Abs(memory) > 1e30f)
            {
                Console.WriteLine("Warning: Значение в памяти достигло предела!");
            }
            else if (sign.ToUpper() == "MR")
            {
                Console.WriteLine("Memory recall: " + memory); 
                Console.WriteLine("To exit press any key...");
                Console.ReadKey();
            }
            else if (sign == "/") 
            {
                if (two == 0) 
                {
                    Console.WriteLine("Error: Division by zero!");
                    Console.WriteLine("To exit press any key... ");
                    Console.ReadKey();
                }
                else
                {
                    result = one / two;
                    Console.WriteLine("Division result is: " + result); 
                    Console.WriteLine("To exit press any key... ");
                    Console.ReadKey();
                }
            }
            else
            {
                Console.WriteLine("You entered an invalid input");
                Console.WriteLine("Available operations: +, -, *, /, %, i, 2, √, M+, M-, MR");
                Console.WriteLine("To exit press any key... ");
                Console.ReadKey();
            }
        }
    }
}

