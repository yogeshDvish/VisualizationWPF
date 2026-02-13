namespace VisualizationWPF.Algorithms
{
    public class MergeSort : ISortAlgorithm
    {
        private readonly MainWindow ui;

        public MergeSort(MainWindow ui)
        {
            this.ui = ui;
        }

        public async Task Sort(int[] data)
        {
            await MergeSortInternal(data, 0, data.Length - 1);
        }

        async Task MergeSortInternal(int[] data, int left, int right)
        {
            if (left >= right) return;

            int mid = (left + right) / 2;

            await MergeSortInternal(data, left, mid);
            await MergeSortInternal(data, mid + 1, right);
            await Merge(data, left, mid, right);
        }

        async Task Merge(int[] data, int left, int mid, int right)
        {
            int[] temp = new int[right - left + 1];
            int i = left, j = mid + 1, k = 0;

            while (i <= mid && j <= right)
                temp[k++] = data[i] <= data[j] ? data[i++] : data[j++];

            while (i <= mid) temp[k++] = data[i++];
            while (j <= right) temp[k++] = data[j++];

            for (int x = 0; x < temp.Length; x++)
            {
                data[left + x] = temp[x];
                ui.UpdateBar(left + x);
                await ui.Delay();
            }
        }
    }
}
