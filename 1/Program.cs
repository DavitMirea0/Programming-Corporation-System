using System;

namespace Calc
{
    class Program
    {
        static float memory = 0; // Переменная для хранения значения в памяти
        static void Main(string[] args)
        {
            float one, two = 0, result;
            string operation; // Изменено с char на string для поддержки многосимвольных операций
            Console.WriteLine("Welcome to the Calc");
            Console.WriteLine("Available operations:");
            Console.WriteLine("Basic: +, -, *, /, %");
            Console.WriteLine("Advanced: s (x^2), r (√x), i (1/x)");
            Console.WriteLine("Memory: M+ (add to memory), M- (subtract from memory), MR (memory recall)");
            Console.Write("Input first number: ");
            one = Convert.ToSingle(Console.ReadLine());
            
            Console.Write("Input operation: ");
            operation = Console.ReadLine();
            
            // Операции, не требующие второго числа
            if (operation == "s") // x^2 (квадрат числа)
            {
                result = one * one;
                Console.WriteLine($"Square of {one} is: {result}");
                Console.WriteLine("To exit, press any key...");
                Console.ReadKey();
            }
            else if (operation == "r") // √x (квадратный корень)
            {
                if (one < 0)
                {
                    Console.WriteLine("Error: Cannot calculate square root of negative number!");
                }
                else
                {
                    result = (float)Math.Sqrt(one);
                    Console.WriteLine($"Square root of {one} is: {result}");
                }
                Console.WriteLine("To exit, press any key...");
                Console.ReadKey();
            }
            else if (operation == "i") // 1/x (обратное число)
            {
                if (one == 0)
                {
                    Console.WriteLine("Error: Cannot calculate 1/x when x is zero!");
                }
                else
                {
                    result = 1 / one;
                    Console.WriteLine($"1/{one} is: {result}");
                }
                Console.WriteLine("To exit, press any key...");
                Console.ReadKey();
            }
            // Операции с памятью
            else if (operation.ToUpper() == "M+") // M+ (добавить к памяти)
            {
                memory += one;
                Console.WriteLine($"Added {one} to memory. Memory now contains: {memory}");
                Console.WriteLine("To exit, press any key...");
                Console.ReadKey();
            }
            else if (operation.ToUpper() == "M-") // M- (вычесть из памяти)
            {
                memory -= one;
                Console.WriteLine($"Subtracted {one} from memory. Memory now contains: {memory}");
                Console.WriteLine("To exit, press any key...");
                Console.ReadKey();
            }
            else if (operation.ToUpper() == "MR") // MR (вспомнить из памяти)
            {
                Console.WriteLine($"Memory recall: {memory}");
                Console.WriteLine("To exit, press any key...");
                Console.ReadKey();
            }
            // Операции, требующие второго числа
            else
            {
                Console.Write("Input second number: ");
                two = Convert.ToSingle(Console.ReadLine());
                
                if (operation == "+")
                {
                    result = one + two;
                    Console.WriteLine("Sum is: " + result);
                    Console.WriteLine("To exit, press any key...");
                    Console.ReadKey();
                }
                else if (operation == "-")
                {
                    result = one - two;
                    Console.WriteLine("Difference is: " + result);
                    Console.WriteLine("To exit, press any key...");
                    Console.ReadKey();
                }
                else if (operation == "*")
                {
                    result = one * two;
                    Console.WriteLine("Product is: " + result);
                    Console.WriteLine("To exit, press any key...");
                    Console.ReadKey();
                }
                else if (operation == "/")
                {
                    if (two == 0)
                    {
                        Console.WriteLine("Error: Division by zero!");
                        Console.WriteLine("To exit, press any key...");
                        Console.ReadKey();
                    }
                    else
                    {
                        result = one / two;
                        Console.WriteLine("Quotient is: " + result);
                        Console.WriteLine("To exit, press any key...");
                        Console.ReadKey();
                    }
                }
                else if (operation == "%") // % (остаток от деления)
                {
                    if (two == 0)
                    {
                        Console.WriteLine("Error: Modulo by zero!");
                    }
                    else
                    {
                        result = one % two;
                        Console.WriteLine($"Remainder of {one} % {two} is: {result}");
                    }
                    Console.WriteLine("To exit, press any key...");
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("You entered an invalid operation!");
                    Console.WriteLine("To exit, press any key...");
                    Console.ReadKey();
                }
            }
        }
    }
}