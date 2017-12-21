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
        #region constants
        Vector nullVector = new Vector(-1, -1);
        Vector zeroVector = new Vector(0, 0);
        VectorSet nullVectorSet = new VectorSet(new Vector(-1, -1));
        VectorSet zeroVectorSet = new VectorSet(new Vector(0, 0));
        #endregion constants
        public VectorsAlgorithm(Task task)
        {
            this.task = task;
        }
        VectorSet VectorsSum(VectorSet vectorSet, Vector vector)
        {
            VectorSet result = new VectorSet();
            foreach (Vector vector2 in vectorSet)
            {
                Vector tmpVector = new Vector(vector2.X + vector.X, vector2.Y + vector.Y);
                result.Add(tmpVector);
            }
            return result;
        }

        VectorSet VectorsFilter(VectorSet vectorSet)
        {
            VectorSet result = new VectorSet();
            foreach (Vector vector1 in vectorSet)
            {
                bool notBad = true;
                foreach (Vector vector2 in vectorSet)
                {
                    if (vector1.X <= vector2.X && vector2.Y < vector2.Y || vector1.X < vector2.X && vector2.Y <= vector2.Y)
                        notBad = false;
                }
                if (notBad)
                    if (!result.Contains(vector1))
                        result.Add(vector1);
            }
            return result;
        }

        VectorSet RecursFill(ref VectorTable table, int n, int b)
        {
            if (table[n][b] != nullVectorSet)
            {
                return table[n][b];
            }
            else
            {
                VectorSet tmpVectorSet = new VectorSet();
                VectorSet tmpVectorSet2 = new VectorSet();
                tmpVectorSet = (RecursFill(ref table, n - 1, b));
                tmpVectorSet2 = GetPrevious(ref table, b, n);
                foreach (Vector vect in tmpVectorSet2)
                {
                    tmpVectorSet.Add(vect);
                }
                table[n][b] = VectorsFilter(tmpVectorSet);
                return (table[n][b]);
            }
        }

        VectorSet GetPrevious(ref VectorTable table, int x, int y)
        {
            if (x - task.LimitationCoefficients[y] >= 0)
            {
                Vector retVector = new Vector(task.FirstCriterion[y], task.SecondCriterion[y]);
                return VectorsSum(RecursFill(ref table, x - 1, y - task.LimitationCoefficients[x]), retVector);
            }
            else
            {
                return zeroVectorSet;
            }

        }
        public void Run(ref VectorTable vectorsTable, ref SigmaTable sigmaTable,
                        ref VectorSet nonDominatedVectors)
        {
            //prepare table
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
                    vectorsTable[0][i] = zeroVectorSet;
                }
            }

            //fill table
            RecursFill(ref vectorsTable, task.Dimension - 1, task.Limit - 1);
            nonDominatedVectors = vectorsTable[task.Dimension - 1][task.Limit - 1];

            //fill sigma-table
            for (int i = 0; i < task.Dimension; i++)
            {
                for (int j = 0; j < task.Limit; j++)
                {
                    if (vectorsTable[i][j] != zeroVectorSet)
                    {
                        for (int k = 0; k < vectorsTable[i][j].Count; k++)
                        {
                            Sigma newSigma = new Sigma(vectorsTable[i][j][k], i + 1, j + 1);
                            if (newSigma.Vector.X != 0 || newSigma.Vector.Y != 0) //check not empty
                            {
                                bool notExist = true;
                                foreach (Sigma oldSigma in sigmaTable)
                                {
                                    if (oldSigma.Vector.X == newSigma.Vector.X && oldSigma.Vector.Y == newSigma.Vector.Y)
                                        notExist = false;
                                }
                                if (notExist)
                                    sigmaTable.Add(newSigma);
                            }
                        }
                    }
                }
            }
        }
    }
}
