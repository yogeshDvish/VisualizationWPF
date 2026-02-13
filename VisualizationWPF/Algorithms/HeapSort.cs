namespace VisualizationWPF.Algorithms
{
    public class HeapSort : ISortAlgorithm
    {
        private readonly MainWindow ui;

        public HeapSort(MainWindow ui)
        {
            this.ui = ui;
        }

        public async Task Sort(int[] data)
        {
            int n = data.Length;

            // Step 1: Build Max Heap
            for (int i = n / 2 - 1; i >= 0; i--)
            {
                await Heapify(data, n, i);
            }

            // Step 2: Extract elements one by one
            for (int i = n - 1; i > 0; i--)
            {
                Swap(data, 0, i);

                ui.UpdateBar(0);
                ui.UpdateBar(i);
                await ui.Delay();

                await Heapify(data, i, 0);
            }
        }

        private async Task Heapify(int[] data, int heapSize, int rootIndex)
        {
            int largest = rootIndex;
            int left = 2 * rootIndex + 1;
            int right = 2 * rootIndex + 2;

            if (left < heapSize && data[left] > data[largest])
                largest = left;

            if (right < heapSize && data[right] > data[largest])
                largest = right;

            if (largest != rootIndex)
            {
                Swap(data, rootIndex, largest);

                ui.UpdateBar(rootIndex);
                ui.UpdateBar(largest);
                await ui.Delay();

                await Heapify(data, heapSize, largest);
            }
        }

        private void Swap(int[] data, int i, int j)
        {
            int temp = data[i];
            data[i] = data[j];
            data[j] = temp;
        }
    }
}
