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
                    if ((vector1.X <= vector2.X) && (vector2.Y < vector2.Y) || (vector1.X < vector2.X) && (vector1.Y <= vector2.Y))
                        notBad = false;
                }
                if (notBad)
                    if (!result.Contains(vector1))
                        result.Add(new Vector(vector1.X, vector1.Y));
            }
            return result;
        }

        VectorSet RecursFill(VectorTable table, int n, int b)
        {
            Vector nullVector = new Vector(-1, -1);
            VectorSet nullVectorSet = new VectorSet(nullVector);
            if ((table[n][b][0].X != nullVectorSet[0].X) && (table[n][b][0].Y != nullVectorSet[0].Y))
            {
                return  table[n][b];
            }
            else
            {
                VectorSet tmpVectorSet = new VectorSet();
                VectorSet tmpVectorSet2 = new VectorSet();
                tmpVectorSet.Set(RecursFill(table, n - 1, b));
                tmpVectorSet2.Set(GetPrevious(table, b, n));
                foreach (Vector vect in tmpVectorSet2)
                {
                    tmpVectorSet.Add(vect);
                }
                VectorSet filtered = new VectorSet();
                filtered.Set(VectorsFilter(tmpVectorSet));
                table[n][b].Set(filtered);
                return (table[n][b]);
            }
        }

        VectorSet GetPrevious(VectorTable table, int x, int y)
        {
            Vector zeroVector = new Vector(0, 0);           
            VectorSet zeroVectorSet = new VectorSet(zeroVector);
            if (x - task.LimitationCoefficients[y] >= 0)
            {
                Vector retVector = new Vector(task.FirstCriterion[y], task.SecondCriterion[y]);
                VectorSet recurs = new VectorSet();
                recurs.Set(RecursFill(table, y - 1, x - task.LimitationCoefficients[y]));
                return VectorsSum(recurs, retVector);
            }
            else
            {
                return zeroVectorSet;
            }

        }
        public void Run(ref VectorTable vectorsTable, ref SigmaTable sigmaTable,
                        ref VectorSet nonDominatedVectors)
        {
            Vector nullVector = new Vector(-1, -1);
            Vector zeroVector = new Vector(0, 0);
            VectorSet nullVectorSet = new VectorSet(nullVector);
            VectorSet zeroVectorSet = new VectorSet(zeroVector);
            //prepare table


            for (int i = 0; i < task.Dimension; i++)
            {
                Row nullRow = new Row();
                for (int j = 0; j < task.Limit; j++)
                {
                    nullRow.Add(new VectorSet(new Vector(-1, -1)));
                }
                vectorsTable.Add(nullRow);
            }
            for (int i = 0; i < task.Limit; i++)
            {
                if (task.LimitationCoefficients[0] <= i + 1)
                {
                    Vector tmpVector = new Vector(task.FirstCriterion[0], task.SecondCriterion[0]);
                    VectorSet tmpVectorSet = new VectorSet(tmpVector);
                    vectorsTable[0][i].Set(tmpVectorSet);
                }
                else
                {
                    vectorsTable[0][i].Set(new VectorSet(new Vector(0, 0)));
                }
            }

            //fill table
            RecursFill(vectorsTable, task.Dimension - 1, task.Limit - 1);
            nonDominatedVectors.Set(vectorsTable[task.Dimension - 1][task.Limit - 1]);

            //fill sigma-table
            for (int i = 0; i < task.Dimension; i++)
            {
                for (int j = 0; j < task.Limit; j++)
                {
                    if ((vectorsTable[i][j][0].X != zeroVectorSet[0].X) && (vectorsTable[i][j][0].Y != zeroVectorSet[0].Y) && (vectorsTable[i][j][0].X != nullVectorSet[0].X) && (vectorsTable[i][j][0].Y != nullVectorSet[0].Y))
                    {
                        for (int k = 0; k < vectorsTable[i][j].Count; k++)
                        {
                            Sigma newSigma = new Sigma(vectorsTable[i][j][k], i + 1, j + 1);
                            if (newSigma.Vector.X != 0 || newSigma.Vector.Y != 0) //check not empty
                            {
                                bool notExist = true;
                                foreach (Sigma oldSigma in sigmaTable)
                                {
                                    if ((oldSigma.Vector.X == newSigma.Vector.X) && (oldSigma.Vector.Y == newSigma.Vector.Y))
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
