using System;
using System.Collections.Generic;
using System.Text;

namespace A_STAR_Algorithm {
	public class Heap {
		public List<Node> items = new List<Node>();

		public void Add(Node item) {
			item.HeapIndex = items.Count;
			items.Add(item);
			SortUp(item);
		}

		public Node RemoveFirst() {
			Node firstItem = items[0];
			items[0] = items[items.Count - 1];
			items[0].HeapIndex = 0;
			items.RemoveAt(items.Count - 1);

			if (items.Count > 0)
				SortDown(items[0]);

			return firstItem;
		}

		public void UpdateItem(Node item) {
			SortUp(item);
		}

		public int Count {
			get {
				return items.Count;
			}
		}

		public bool Contains(Node item) {
			if (items.Count == 0) return false;
            for (int x = 0; x < item.Grid.Length; x++) {
                for (int y = 0; y < item.Grid[x].Length; y++) {
                    if (item.Grid[x][y] != items[item.HeapIndex].Grid[x][y]) {
                        return false;
                    }
                }
            }
            return true;
		}
		
		void SortDown(Node item) {
			while (true) {
				int childIndexLeft = item.HeapIndex * 2 + 1;
				int childIndexRight = item.HeapIndex * 2 + 2;
				int swapIndex = 0;

				if (childIndexLeft < items.Count) {
					swapIndex = childIndexLeft;

					if (childIndexRight < items.Count) {
						if (items[childIndexLeft].CompareTo(items[childIndexRight]) < 0) {
							swapIndex = childIndexRight;
						}
					}

					if (item.CompareTo(items[swapIndex]) < 0) {
						Swap(item, items[swapIndex]);
					} else return;

				} else return;
			}
		}

		void SortUp(Node item) {
			int parentIndex = (item.HeapIndex - 1) / 2;

			while (true) {
				Node parentItem = items[parentIndex];
				if (item.CompareTo(parentItem) > 0) {
					Swap(item, parentItem);
				} else break;

				parentIndex = (item.HeapIndex - 1) / 2;
			}
		}

		void Swap(Node itemA, Node itemB) {
			items[itemA.HeapIndex] = itemB;
			items[itemB.HeapIndex] = itemA;
			int itemAIndex = itemA.HeapIndex;
			itemA.HeapIndex = itemB.HeapIndex;
			itemB.HeapIndex = itemAIndex;
		}
	}
}
