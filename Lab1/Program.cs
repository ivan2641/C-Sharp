using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Lab1
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine(
                "Информация по типам" +
                "\n1 – Общая информация по типам" +
                "\n2 – Выбрать из списка" +
                "\n3 – Ввести имя типа" +
                "\n4 – Параметры консоли" +
                "\n0 - Выход из программы");

                switch (Console.ReadKey(true).KeyChar)
                {
                    case '1': ShowAllTypeInfo(); break;
                    case '2': SelectType(); break;
                    case '3': EnterTypeName(); break;
                    case '4': ConsoleParams(); break;
                    case '0': return;
                    default: break;
                }
            }
        }

        private static void ConsoleParams()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine(@"1 - Цвет текста
2 - цвет фона
0 - вернуться в главное меню");

            }
        }


        public static void ShowAllTypeInfo()
        {
            Console.Clear();
            Assembly myAsm = Assembly.GetExecutingAssembly();
            Type[] thisAssemblyTypes = myAsm.GetTypes();
            Assembly[] refAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            List<Type> types = new List<Type>();
            foreach (Assembly asm in refAssemblies)
                types.AddRange(asm.GetTypes());
            int nRefTypes = 0;
            int nValueTypes = 0;
            int nInterfaceTypes = 0;
            int maxCnt = 0;
            String maxMeth = "";
            String maxMethName = "";
            String longestMethName = "";
            int maxArgsCnt = 0;
            string maxMethodArgsMeth = "";
            foreach (var t in types)
            {
                if (t.IsClass)
                    nRefTypes++;
                else if (t.IsValueType)
                    nValueTypes++;
                else if (t.IsInterface)
                    nInterfaceTypes++;
                if (t.GetMethods().Count() > maxCnt)
                {
                    maxMeth = t.Name;
                    maxCnt = t.GetMethods().Count();
                }
                if (t.Name.Length > longestMethName.Length)
                {
                    longestMethName = t.Name;
                }
                foreach (var i in t.GetMethods())
                {
                    if (i.GetParameters().Length > maxArgsCnt)
                    {
                        maxArgsCnt = i.GetParameters().Length;
                        maxMethodArgsMeth = i.Name;
                    }
                }
            }
            Console.WriteLine(
                $@"Общая информация по типам
Подключенные сборки: {refAssemblies.Length}
Всего типов по всем подключенным сборкам: {types.Count}
Ссылочные типы: {nRefTypes}
Значимые типы: {nValueTypes}
Типы-интерфейсы: {nInterfaceTypes}
Тип с максимальным числом методов: {maxMeth}
Самое длинное название метода: {longestMethName}
Метод с наибольшим числом аргументов: {maxMethName}
Нажмите любую клавишу, чтобы вернуться в главное меню"
            );
            Console.ReadKey();
        }

        public static void SelectType()
        {
            Console.Clear();
            Console.WriteLine(
                 "Информация по типам" +
                 "\n1 – uint" +
                 "\n2 – int" +
                 "\n3 – long" +
                 "\n4 – float" +
                 "\n5 - double" +
                 "\n6 - char" +
                 "\n7 - string" +
                 "\n8 - MyClass" +
                 "\n9 - MyStruct" +
                 "\n0 - Выход в главное меню");
            
            Type t = GetType();
            PrintType(t);
            Console.Clear();
        }


        private static void PrintType(Type t)
        {
            Console.Clear();
            Console.WriteLine(
            $@"Информация по типу: {t.FullName}
Значимый тип: {t.IsValueType}
Пространство имен: {t.Namespace}
Сборка: {t.Assembly.GetName().Name}
Общее число элементов: {t.GetMembers().Length}
Число методов: {t.GetMethods().Length}
Число свойств: {t.GetProperties().Length}
Число полей: {t.GetFields().Length}
Список полей: {string.Join(", ", GetFldProp(t.GetFields()))}
Список свойств: {string.Join(", ", GetFldProp(t.GetProperties()))}
Нажмите ‘M’ для вывода дополнительной информации по методам:
Нажмите ‘0’ для выхода в главное меню"
);
            switch (char.ToLower(Console.ReadKey(true).KeyChar))
            {
                case '0': return;
                case 'm':
                    ShowMethTypeInfo(t); return;
            }
        }

                     
        private static void ShowMethTypeInfo(Type t)  // Доделать параметры
        {
                Console.Clear();
                MethodInfo[] methods = t.GetMethods();
                var dic = new Dictionary<string, int>();
                foreach (var m in methods)
                {
                    if (dic.ContainsKey(m.Name))
                    {
                        dic[m.Name]++;
                    }
                    else
                    {
                        dic[m.Name] = 1;
                    }
                }
            Console.WriteLine($@"Методы типа {t.FullName},
Название    Число перегрузок    Число параметров"
);
            foreach (var kv in dic)
                {
                    Console.WriteLine(kv.Key + "\t" + "\t" + kv.Value);
                }
            Console.ReadKey(true);

        }

                     
        private static Type GetType()
        {
            while (true)
            {
                char typeName = Console.ReadKey(true).KeyChar; 
                switch (typeName)
                {
                    case '1': return typeof(uint);
                    case '2': return typeof(int);
                    case '3': return typeof(long);
                    case '4': return typeof(float);
                    case '5': return typeof(double);
                    case '6': return typeof(char);
                    case '7': return typeof(string);
                    case '8': return typeof(Program);
                    case '9': return typeof(MyStruct);
                    default: continue;
                }
            }

        }


        private static string[] GetFldProp(MemberInfo[] inf)
        {
            string[] fieldNames = new string[inf.Length];
            for (int i = 0; i < inf.Length; i++)
            {
                fieldNames[i] = inf[i].Name;
            }
            return fieldNames;
        }


        public static void EnterTypeName()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Введите полное имя типа:");
                string input = Console.ReadLine();
                if (input == "0")
                {
                    return;
                }
                Type t = GetTypeFromName(input);
                if (t == null)
                {
                    continue;
                }
                PrintType(t);
                return;
            }

        }

        private static Type GetTypeFromName(string input)
        {
            return Type.GetType(input);
        }
    }


        public struct MyStruct
    {
        public int x, y;

        public MyStruct(int p1, int p2)
        {
            x = p1;
            y = p2;
        }
    }
}
