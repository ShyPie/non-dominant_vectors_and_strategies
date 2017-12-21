using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Non_dominated_vectors_and_strategies
{
    public class VectorsAlgorithm
    // Класс который строит таблицу всех множеств P(W(k,p))
    {
        Task task = new Task();
        public VectorsAlgorithm(Task task)
        {
            this.task = task;
        }
        VectorSet VectorsSum(VectorSet vectorSet, Vector vector)
        {
            VectorSet result = new VectorSet();
            //empty
            return result;
        }

        VectorSet VectorsFilter(VectorSet vectorSet)
        {
            VectorSet result = new VectorSet();
            //empty
            return result;
        }

        VectorSet RecursFill(VectorTable table, int n, int b)
        {
            Vector nullVector = new Vector(-1, -1);
            VectorSet nullVectorSet = new VectorSet(nullVector);
            if (table[n][b] != nullVectorSet)
            {
                //empty
                return table[n][b];
            }
            else
            {

                return (table[n][b]);
            }
        }

        VectorSet GetPrevious(VectorTable table, int x, int y)
        {
            if (x - task.LimitationCoefficients[y] >= 0)
            {
                Vector retVector = new Vector(task.FirstCriterion[y], task.SecondCriterion[y]);
                return VectorsSum(RecursFill(table, x - 1, y - task.LimitationCoefficients[x]), retVector);
            }
            else
            {
                Vector zeroVector = new Vector(0, 0);
                VectorSet zeroVectorSet = new VectorSet(zeroVector);
                return zeroVectorSet;
            }
                
        }
        public void Run(ref VectorTable vectorsTable, ref SigmaTable sigmaTable,
                        ref VectorSet nonDominatedVectors)
        {
            //prepare table
            Vector nullVector = new Vector(-1, -1);
            Vector zeroVector = new Vector(0, 0);
            VectorSet nullVectorSet = new VectorSet();
            nullVectorSet.Add(nullVector);
            for (int i = 0; i < task.Dimension; i++)
            {
                Row nullRow = new Row();
                for (int j = 0; j < task.Limit; j++)
                {
                    nullRow.Add(nullVectorSet);
                }
            }
            for (int i = 0; i < task.Limit; i++)
            {
                if (task.LimitationCoefficients[0] <= i + 1)
                {
                    Vector tmpVector = new Vector(task.FirstCriterion[0], task.SecondCriterion[0]);
                    VectorSet tmpVectorSet = new VectorSet(tmpVector);
                    vectorsTable[0][i] = tmpVectorSet;
                }
                else
                {
                    VectorSet zeroVectorSet = new VectorSet(zeroVector);
                    vectorsTable[0][i] = zeroVectorSet;
                }
            }

            //fill table
            RecursFill(vectorsTable, task.Dimension - 1, task.Limit - 1);
            nonDominatedVectors = vectorsTable[task.Dimension - 1][task.Limit - 1];
        }
    }
}
