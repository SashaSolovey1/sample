using System;
using System.Text;
using ConsoleTools;

namespace Twofish.ConsoleRealization
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var hexKey = TwofishInterface.GenerateRandomKey();

            var menu = new ConsoleMenu(args, level: 0)
                .Add("Regenerate Key", () => hexKey = TwofishInterface.GenerateRandomKey())
                .Add("Crypt", () =>
                {
                    Console.Write("Input data: ");
                    var data = Console.ReadLine();
                    Console.WriteLine("Crypted: " + TwofishInterface.Crypt(data, hexKey));
                    Console.WriteLine("Press any key to back menu");
                    Console.ReadKey();
                })
                .Add("Set custom hex key", () =>
                {
                    byte[] hKey = null;
                    do
                    {
                        if (hKey != null)
                            Console.WriteLine("Incorrect key size (16 or 32 characters)!");
                        Console.Write("Input data (16 or 32 chars): ");
                        hKey = Encoding.UTF8.GetBytes(Console.ReadLine() ?? string.Empty);
                    } while (hKey.Length != 16 && hKey.Length != 32);

                    hexKey = Convert.ToBase64String(hKey);
                    Console.WriteLine("New Key: " + hexKey);
                    Console.WriteLine("Press any key to back menu");
                    Console.ReadKey();
                })
                .Add("Decrypt", () =>
                {
                    Console.Write("Input hex data: ");
                    var data = Console.ReadLine();
                    try
                    {
                        Console.WriteLine("Decrypted: " + TwofishInterface.Decrypt(data, hexKey));
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Can't decrypt, incorrect input data!");
                    }
                    
                    Console.WriteLine("Press any key to back menu");
                    Console.ReadKey();
                })
                .Add("Exit", () => Environment.Exit(0))
                .Configure(config =>
                {
                    config.Selector = "--> ";
                    config.EnableWriteTitle = true;
                    config.Title = "Twofish console realization";
                    config.EnableBreadcrumb = true;
                    config.WriteTitleAction = title => Console.WriteLine("Key: " + hexKey);
                });

            menu.Show();
        }
    }
}
