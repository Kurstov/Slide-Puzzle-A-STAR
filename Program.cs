using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Diagnostics;

namespace A_STAR_Algorithm {

    public class Node {
        public Node Parent;

        private int[][] grid;

        private int hCost;
        private int gCost;

        public int[][] Grid { get { return grid; } }
        public int FCost { get { return hCost + gCost; } }
        public int GCost {
            get { return gCost; }
            set { gCost = value; }
        }

        public int HCost {
            get { return hCost; }
            set { hCost = value; }
        }

        public Node(int[][] grid) {
            this.grid = new int[grid.Length][];

            for (int x = 0; x < grid.Length; x++) {
                for (int y = 0; y < grid[x].Length; y++) {
                    this.grid[x] = new int[grid[x].Length];
                }
            }

            for (int x = 0; x < grid.Length; x++) {
                for (int y = 0; y < grid[x].Length; y++) {
                    this.grid[x][y] = grid[x][y];
                }
            }
        }

        private int heapIndex;

        public int HeapIndex {
            get { return heapIndex; }
            set { heapIndex = value; }
        }

        public int CompareTo(Node nodeToCompare) {
            int compare = FCost.CompareTo(nodeToCompare.FCost);
            return -compare;
        }

        public void PrintGrid() {
            for (int x = 0; x < grid.Length; x++) {
                for (int y = 0; y < grid[x].Length; y++) {
                    Console.Write(grid[x][y] + " ");
                }
                Console.WriteLine("");
            }
            Console.WriteLine("");
        }
    }

    struct Vector2 {
        private int x;
        private int y;

        public int X {
            get { return x; }
            set { x = value; }
        }

        public int Y {
            get { return y; }
            set { y = value; }
        }

        public Vector2(int x, int y) {
            this.x = x;
            this.y = y;
        }
    }

    class Program {

        static void Main(string[] args) {
            Run();
        }

        static void Run() {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            int[][] start = new int[3][]{
                new int[3] { 6, 0, 8 },
                new int[3] { 4, 3, 5 },
                new int[3] { 1, 2, 7 },
            };

            Node startNode = new Node(start);

            int[][] end = new int[3][] {
                new int[3] { 1, 2, 3 },
                new int[3] { 4, 5, 6 },
                new int[3] { 7, 8, 0 },
            };

            Node endNode = new Node(end);

            Heap openList = new Heap();
            HashTable closedList = new HashTable();

            openList.Add(startNode);

            while (openList.Count != 0) {
                Node currentNode = openList.RemoveFirst();
                closedList.Add(currentNode);
                // END
                if (Compare(currentNode.Grid, endNode.Grid)) {
                    sw.Stop();
                    TimeSpan ts = sw.Elapsed;
                    Console.WriteLine("SOLVED! " + ts.TotalSeconds + " seconds");
                    Console.WriteLine("");

                    Node cur = currentNode;

                    while (cur != null) {
                        for (int x = 0; x < cur.Grid.Length; x++) {
                            string line = "";
                            for (int y = 0; y < cur.Grid[x].Length; y++) {
                                line += cur.Grid[x][y] + " ";
                            }
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(line);
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        Console.WriteLine("");

                        cur = cur.Parent;
                    }
                    break;
                }

                foreach (Node possibleNode in GetPossibleMoves(currentNode)) {
                    if (closedList.Contains(possibleNode)) continue;
                    int moveCost = currentNode.GCost + GetDistance(currentNode, possibleNode);
                    bool inOpenList = openList.Contains(possibleNode);
                    if (moveCost < possibleNode.GCost || !inOpenList) {
                        possibleNode.GCost = moveCost;
                        possibleNode.HCost = GetDistance(possibleNode, endNode);
                        possibleNode.Parent = currentNode;

                        if (!inOpenList) {
                            openList.Add(possibleNode); // add only if no duplicate exists in open list
                        }
                    }
                }
            }
        }

        // Get all possible moves from blank tiles perspective
        private static List<Node> GetPossibleMoves(Node node) {
            List<Node> possibleMoves = new List<Node>();

            Vector2 curPos = new Vector2(0, 0);

            for (int x = 0; x < node.Grid.Length; x++) {
                for (int y = 0; y < node.Grid[x].Length; y++) {
                    if (node.Grid[x][y] == 0) {
                        curPos.X = x;
                        curPos.Y = y;
                        break;
                    }
                }
            }

            Vector2[] moves = {
                new Vector2(0, 1),
                new Vector2(0,-1),
                new Vector2(1, 0),
                new Vector2(-1,0),
            };

            foreach (Vector2 pos in moves) {
                Node pNode = new Node(node.Grid);

                int x = pos.X + curPos.X;
                int y = pos.Y + curPos.Y;

                if (x < node.Grid.Length && x >= 0) {
                    if (y < node.Grid[x].Length && y >= 0) {
                        // Swap
                        int temp = pNode.Grid[curPos.X][curPos.Y];
                        pNode.Grid[curPos.X][curPos.Y] = pNode.Grid[x][y];
                        pNode.Grid[x][y] = temp;

                        possibleMoves.Add(pNode);
                    }
                }
            }

            return possibleMoves;
        }

        // Get manhattan distance here
        private static int GetDistance(Node node, Node endNode) {

            int hCost = 0;

            for (int x = 0; x < node.Grid.Length; x++) {
                for (int y = 0; y < node.Grid[x].Length; y++) {

                    if (node.Grid[x][y] == 0 || node.Grid[x][y] == endNode.Grid[x][y])
                        continue;

                    // Find where the number should be
                    int number = node.Grid[x][y];
                    Vector2 pos = FindNumber(number, endNode);

                    int ix = Math.Abs(pos.X - x);
                    int iy = Math.Abs(pos.Y - y);

                    hCost += ix + iy;
                }
            }
            return hCost;
        }

        private static Vector2 FindNumber(int number, Node endNode) {
            for (int x = 0; x < endNode.Grid.Length; x++) {
                for (int y = 0; y < endNode.Grid[x].Length; y++) {
                    if (number == endNode.Grid[x][y])
                        return new Vector2(x, y);
                }
            }
            return new Vector2(0, 0);
        }

        private static bool Compare(int[][] one, int[][] two) {
            for (int x = 0; x < one.Length; x++) {
                for (int y = 0; y < one[x].Length; y++) {
                    if (one[x][y] != two[x][y])
                        return false;
                }
            }
            return true;
        }
    }
}
