using System.Windows.Media;


namespace VisualizationWPF.Algorithms
{
    public class QuickSort : ISortAlgorithm
    {
        private readonly MainWindow ui;

        public QuickSort(MainWindow ui)
        {
            this.ui = ui;
        }

        public async Task Sort(int[] data)
        {
            await QuickSortInternal(data, 0, data.Length - 1);
        }

        async Task QuickSortInternal(int[] data, int low, int high)
        {
            if (low < high)
            {
                int p = await Partition(data, low, high);
                await QuickSortInternal(data, low, p - 1);
                await QuickSortInternal(data, p + 1, high);
            }
        }

        async Task<int> Partition(int[] data, int low, int high)
        {
            int pivot = data[high];
            ui.SetColor(high, Brushes.Red);

            int i = low - 1;

            for (int j = low; j < high; j++)
            {
                ui.Highlight(j, high);
                await ui.Delay();

                if (data[j] < pivot)
                {
                    i++;
                    (data[i], data[j]) = (data[j], data[i]);
                    ui.Swap(i, j);
                    await ui.Delay();
                }

                ui.ResetColor(j, high);
            }

            (data[i + 1], data[high]) = (data[high], data[i + 1]);
            ui.Swap(i + 1, high);
            ui.SetColor(high, Brushes.Lime);

            await ui.Delay();
            return i + 1;
        }
    }
}
