using System;
using System.Collections.Generic;
using System.Linq;

namespace Graphs
{
    // Реализовать алгоритм Дейкстры.
    // Константин Суворов. Санкт-Петербург.

    class Program
    {
        /// <summary>
        /// Связь между вершинами графа
        /// </summary>
        public class Link
        {
            public double Distance { get; set; }
            public string Node { get; set; }
        }

        /// <summary>
        /// Вершина графа
        /// </summary>
        public class Node
        {
            public List<Link> Links { get; set; } // ребра к другим вершинам

            public Node()
            {
                Links = new List<Link>();
            }
        }

        /// <summary>
        /// Вспомогательный класс для алгоритма Дейкстры
        /// </summary>
        public class Description
        {
            public bool Visited { get; set; }
            public double Distance { get; set; }

            public Description()
            {
                Visited = false;
                Distance = double.PositiveInfinity;
            }
        }

        /// <summary>
        /// Граф
        /// </summary>
        public class Graph
        {
            private Dictionary<string, Node> nodes;

            public Graph()
            {
                nodes = new Dictionary<string, Node>();
            }

            /// <summary>
            /// Добавить вершину графа
            /// </summary>
            /// <param name="name">Имя вершины графа</param>
            public void AddNode(string name)
            {
                nodes.Add(name, new Node());
            }

            /// <summary>
            /// Добавить ребро графа
            /// </summary>
            /// <param name="start">Начальная вершина графа</param>
            /// <param name="end">Конечная вершина граф</param>
            /// <param name="distance">расстояние</param>
            /// <param name="isOriented">двунаправленная связь</param>
            public void AddLinkToNode(string start, string end, double distance, bool isOriented)
            {
                nodes[start].Links.Add(new Link { Node = end, Distance = distance });
                if (!isOriented)
                    nodes[end].Links.Add(new Link { Node = start, Distance = distance });
            }

            /// <summary>
            /// Нахождение минимального расстояния
            /// </summary>
            /// <param name="start">Начальная вершина графа</param>
            /// <param name="end">Конечная вершина граф</param>
            /// <returns>расстояние</returns>
            public double FindShortestDistance(string start, string end)
            {
                // Алгоритм Дейкстры.
                Dictionary<string, Description> info = new Dictionary<string, Description>(this.nodes.Count);
                foreach (string current in this.nodes.Keys)
                    info.Add(current, new Description());
                info[start].Distance = 0;

                // Пока все вершины непосещенные.
                while (!info.Select(x => x.Value.Visited).Aggregate((x, y) => x & y))
                {
                    // Находим непосещенную вершину с минимальной меткой.
                    string current = info.Where(x => !x.Value.Visited
                            && x.Value.Distance == info.Where(y => !y.Value.Visited).Min(y => y.Value.Distance))
                            .First().Key;
                    // Находим все непосещенные соседние вершины для текущей вершины.
                    List<Link> neighbors = nodes[current].Links.Where(x => !info[x.Node].Visited).ToList();
                    // Рассматриваем новую длину пути для каждой соседней вершины.
                    foreach (Link link in neighbors)
                    {
                        double distance = info[current].Distance + link.Distance;
                        if (info[link.Node].Distance > distance)
                            info[link.Node].Distance = distance;
                    }
                    // Отмечаем текущую вершину как посещенная.
                    info[current].Visited = true;
                }
                return info[end].Distance;
            }

            // список всех вершин 
            public string ShowNodes()
            {
                string result = string.Empty;
                foreach (var node in nodes)
                {
                    result += node.Key + " ";
                }
                return result;
            }

        }

        static void Main(string[] args)
        {
            // Вершины графа
            string[] nodeNames = { "X0", "X1", "X2", "X3", "X4", "X5", "X6", "X7"};

            // Заполнение графа вершинами.
            Graph graph = new Graph();
            for (int i = 0; i < nodeNames.Length; i++)
                graph.AddNode(nodeNames[i]);

            // Создание у вершин связей.
            // Последнее значение говорит о том, что эта связь двунаправленная.
            graph.AddLinkToNode("X0", "X1", 4, false);
            graph.AddLinkToNode("X0", "X2", 3, false);
            graph.AddLinkToNode("X0", "X3", 3, false);
            graph.AddLinkToNode("X1", "X2", 1, false);
            graph.AddLinkToNode("X1", "X4", 8, false);
            graph.AddLinkToNode("X1", "X5", 6, false);
            graph.AddLinkToNode("X2", "X3", 8, false);
            graph.AddLinkToNode("X2", "X4", 2, false);
            graph.AddLinkToNode("X3", "X6", 4, false);
            graph.AddLinkToNode("X4", "X5", 2, false);
            graph.AddLinkToNode("X4", "X7", 5, false);
            graph.AddLinkToNode("X5", "X7", 3, false);
            graph.AddLinkToNode("X6", "X7", 2, false);

            string stationStart; // станция отправления
            string stationEnd; // станция назначения

            Console.WriteLine("Имеются следующие станции: {0}", graph.ShowNodes());

            Console.Write("Введите имя станции отправления: ");
            stationStart = Console.ReadLine();

            Console.Write("Введите имя станции назначения:  ");
            stationEnd = Console.ReadLine();

            double distance = graph.FindShortestDistance(stationStart, stationEnd);

            Console.WriteLine("Кратчайший маршрут от станции {0} до станции {1} = {2}", stationStart, stationEnd, distance);

            Console.ReadLine();
        }
    }
}