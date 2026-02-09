using System.Threading.Tasks;


namespace VisualizationWPF.Algorithms
{
    public interface ISortAlgorithm
    {
        Task Sort(int[] data);
    }
}
