// See https://aka.ms/new-console-template for more information

using System.Security.Cryptography;
using System.Text;

Console.WriteLine("Hello, Hash Generator!");

string data = "Hello, world!";

using SHA256 sha256 = SHA256.Create();

byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(data));
string hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
Console.WriteLine(hash);
Console.WriteLine(hash.Length);