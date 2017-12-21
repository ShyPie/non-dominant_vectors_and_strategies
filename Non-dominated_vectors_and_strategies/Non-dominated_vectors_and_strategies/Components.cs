using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Non_dominated_vectors_and_strategies
{
    public struct Vector
    {
        int x;
        int y;
        public Vector(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
    public class VectorSet: List<Vector>
    {
        public VectorSet()
        {
            VectorSet vectorSet = new VectorSet();
        }
        public VectorSet(Vector vector)
        {
            VectorSet vectorSet = new VectorSet();
            vectorSet.Add(vector);
        }
    }

    public class Row : List<VectorSet>
    {
    }

    public class VectorTable : List<Row>
    {
    }

    public struct Sigma
    {
        Vector vector;
        int rowIndex;
        int columnIndex;
    }

    public class SigmaTable : List<Sigma>
    {
    }
}
