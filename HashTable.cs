using System;
using System.Collections.Generic;
using System.Text;

namespace A_STAR_Algorithm {
    class HashTable {
        private Heap[] table = new Heap[9];

        public int Count {
            get {
                int count = 0;
                for (int i = 0; i < table.Length; i++) {
                    count += table[i].Count;
                }
                return count;
            }
        }

        public HashTable() {
            for (int i = 0; i < 9; i++) {
                table[i] = new Heap();
            }
        }

        public Node PopTop() {
            int smallest = int.MaxValue;
            int index = 0;
            for (int i = 0; i < table.Length; i++) {
                if (table[i].items.Count > 0) {
                    if (smallest > table[i].items[0].FCost) {
                        smallest = table[i].items[0].FCost;
                        index = i;
                    }
                }
            }
            return table[index].RemoveFirst();
        }

        public void Add(Node node) {
            int hash = HashGrid(node);
            table[hash].Add(node);
        }

        public int HashGrid(Node node) {
            int index = 0;
            bool found = false;
            for (int x = 0; x < node.Grid.Length; x++) {
                if (found) break;
                for (int y = 0; y < node.Grid[x].Length; y++) {
                    if (node.Grid[x][y] == 0) {
                        found = true;
                        break;
                    }
                    index++;
                }
            }
            return index;
        }

        public bool Contains(Node node) {
            int hash = HashGrid(node);
            if (table[hash].Contains(node))
                return true;
            else return false;
        }
    }
}
