using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Non_dominated_vectors_and_strategies
{


    class StrategiesAlgorithm
    //Класс который восстанавливает недоминируемые стратегии по недоминируемым векторам 
    {
        Task task = new Task();
        int ksi, eta;
        Vector u = new Vector();
        Vector zeroVector = new Vector(0, 0);
        public StrategiesAlgorithm(Task task)
        {
            this.task = task;
        }

        public void FindU(SigmaTable sigmaTable)
        {
            foreach (Sigma sigma in sigmaTable)
            {
                if (sigma.Vector.X == u.X && sigma.Vector.Y == u.Y && sigma.Row <= ksi && sigma.Column <= eta)
                {
                    this.u = sigma.Vector;
                    this.ksi = sigma.Row;
                    this.eta = sigma.Column;
                }
            }
        }



        public List<List<int>> Run(SigmaTable sigmaTable, ref VectorSet nonDominatedVectors)
        {
            List<List<int>> strategies = new List<List<int>>();




            foreach (Vector resultVector in nonDominatedVectors)
            {
                u = resultVector;
                ksi = task.Dimension;
                eta = task.Limit;
                List<int> strategy = new List<int>();
                for (int i = 0; i < task.Dimension; i++)
                    strategy.Add(0);
                while (u.X != 0 && u.Y != 0)
                {
                    strategy[ksi - 1] = 1;
                    u.X = u.X - task.FirstCriterion[ksi - 1];
                    u.Y = u.Y - task.SecondCriterion[ksi - 1];
                    if (u.X != 0 && u.Y != 0)
                    {
                        eta = eta - task.LimitationCoefficients[ksi - 1];
                        ksi = ksi - 1;
                        FindU(sigmaTable);
                    }
                }
                strategies.Add(strategy);
            }
            return strategies;
        }
    }


}
