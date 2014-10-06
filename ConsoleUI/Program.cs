using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Autofac;
using ConsoleUI.Autofac;
using Domain.RepositoryFactories;

namespace ConsoleUI
{
  internal class Program
  {
    public static IContainer Container = AutofacModule.RegisterAutoFac();
    public static IRepositoryFactory RepositoryFactory;

    private static void Main(string[] args)
    {
      //задаем путь к нашему рабочему файлу XML
      string fileName = "Test";
      //читаем данные из файла
      XDocument doc = XDocument.Load(fileName);
      //проходим по каждому элементу в найшей library
      //(этот элемент сразу доступен через свойство doc.Root)
      //foreach (XElement el in doc.Root.Elements())
      //{
      //  //Выводим имя элемента и значение аттрибута id
      //  Console.WriteLine("{0} {1}", el.Name, el.Attribute("user").Value);
      //  Console.WriteLine("  Attributes:");
      //  //выводим в цикле все аттрибуты, заодно смотрим как они себя преобразуют в строку
      //  foreach (XAttribute attr in el.Attributes())
      //    Console.WriteLine("    {0}", attr);
      //  Console.WriteLine("  Elements:");
      //  //выводим в цикле названия всех дочерних элементов и их значения
      //  foreach (XElement element in el.Elements())
      //    Console.WriteLine("    {0}: {1}", element.Name, element.Value);
      //}

      XElement user = doc.Root.Elements().First();
      Console.WriteLine(user.Value);

      IEnumerable<XElement> words = doc.Root.Elements().Skip(1).Take(1);

      foreach (XElement word in words)
      {
        foreach (XElement wordElemets in word.Elements())
          //foreach (XElement elements in wordElemets.Elements())
          //{
          //  Console.WriteLine("{0}: {1}", elements.Name, elements.Value);
          //}
          Console.WriteLine(wordElemets.Element("rusWord").Value);
      }
    }
  }
}