using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Erlè
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string Input;
                if (args.Length != 0)
                {
                    Input = File.ReadAllText(args[0]);
                }
                else
                {
                    Input = File.ReadAllText("Input.json");
                }

                ParameterContainer[] Containers = JsonSerializer.Deserialize<ParameterContainer[]>(Input);
                ParameterContainer[] SortedContainers = GetSortedContainers(Containers);
                int[] ParametersToShow = GetSortedParameters(SortedContainers);

                Console.WriteLine("Все параметры (контейнеры изначально расположены по принцепу пустой-не пустой):");
                ShowParameters(SortedContainers);

                Console.WriteLine($"\nМинимальное значение = {ParametersToShow[0]}" +
                    $"\nМаксимальное значение = {ParametersToShow[ParametersToShow.Length - 1]}");
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();

            void ShowParameters(params ParameterContainer[] Parameters)
            {
                foreach (ParameterContainer Container in Parameters)
                {
                    Console.WriteLine(Container.ToString());
                }
            }
            int[] GetSortedParameters(params ParameterContainer[] Parameters)
            {
                List<int> ToSort = new List<int>();
                foreach (ParameterContainer Container in Parameters)
                {
                    if (!Container.ParametersSetted())
                    {
                        continue;
                    }

                    int[] ToShow = Container.Parameters.ToList().Distinct().ToArray();
                    ToSort.AddRange(ToShow);
                }
                ToSort.Sort();
                return ToSort.ToArray();
            }
            ParameterContainer[] GetSortedContainers(params ParameterContainer[] Containers)
            {
                List<ParameterContainer> Output = Containers.ToList();
                Output.Sort();
                return Output.ToArray();
            }
        }
        [Serializable]
        class ParameterContainer : IComparable
        {
            public int[] Parameters { get; set; }
            public bool ParametersSetted() => Parameters != null;

            public override string ToString()
            {
                if (!ParametersSetted())
                {
                    return "<Пустой контейнер>";
                }

                return GetPrintableParameters();
            }
            public string GetPrintableParameters()
            {
                return string.Join(',', Parameters);
            }
            public int CompareTo(object obj)
            {
                ParameterContainer Container = obj as ParameterContainer;
                return Convert.ToInt16(Container.ParametersSetted()) - Convert.ToInt16(ParametersSetted());
            }
        }
    }
}
