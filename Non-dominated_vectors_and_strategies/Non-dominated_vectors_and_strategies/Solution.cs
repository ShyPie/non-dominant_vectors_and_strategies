using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Non_dominated_vectors_and_strategies
{
    public class Solution
    //Класс хранящий решение задачи
    //vectorsTable - таблица векторов z(k,p)
    //sigmaTable - таблица допустимых векторов
    //nonDominatedVectors - недоминируемые вектора
    //nonDominatedStrategies - недоминируемые стратегии

    {
        VectorTable vectorTable = new VectorTable();
        SigmaTable sigmaTable = new SigmaTable();
        VectorSet nonDominatedVectors = new VectorSet();
        List<List<int>> nonDominatedStrategies = new List<List<int>>();


        public Solution(Task task)
        {
            VectorsAlgorithm vectorsAlgorithm = new VectorsAlgorithm(task);
            vectorsAlgorithm.Run(ref this.vectorTable, ref this.sigmaTable, ref this.nonDominatedVectors);
            StrategiesAlgorithm strategiesAlgorithm = new StrategiesAlgorithm(task);
            nonDominatedStrategies = strategiesAlgorithm.Run(sigmaTable,ref this.nonDominatedVectors);


        }

        public List<List<int>> NonDominatedStrategies
        {
            get
            {
                return nonDominatedStrategies;
            }
        }

        public VectorSet NonDominatedVectors
        {
            get
            {
                return nonDominatedVectors;
            }
        }

        public SigmaTable SigmaTable
        {
            get
            {
                return sigmaTable;
            }
        }
    }
}
