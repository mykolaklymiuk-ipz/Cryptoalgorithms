using encryption_lab_1;
using System;
using System.Linq;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;

Console.WriteLine($"колонка крипто, рядок: шифр, текст: програмнезабезпечення");

var result = MatrixEncrypter.Encrypt("крипто", "шифр", "програмнезабезпечення");

Console.WriteLine("результат: ");

result.ToList().ForEach(raw => Console.WriteLine(raw));
