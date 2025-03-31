// See https://aka.ms/new-console-template for more information

using System.Security.Cryptography;

Console.WriteLine("Hello, Generate API Key!");

using RandomNumberGenerator random = RandomNumberGenerator.Create();

var byteArray = new byte[64];

random.GetBytes(byteArray);

Console.WriteLine(Convert.ToBase64String(byteArray));