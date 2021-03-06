﻿using System;
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
        public int X
        {
            get
            {
                return x;
            }
            set
            {
                this.x = value;
            }
        }

        public int Y
        {
            get
            {
                return y;
            }
            set
            {
                this.y = value;
            }
        }
    }
    public class VectorSet : List<Vector>
    {
        public VectorSet()
        {
        }
        public VectorSet(Vector vector)
        {
            Add(vector);
        }
        public void Set(VectorSet vectorSet)
        {
            Clear();
            AddRange(vectorSet);
              

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

        public Sigma(Vector vector, int rowIndex, int columnIndex)
        {
            this.vector = vector;
            this.rowIndex = rowIndex;
            this.columnIndex = columnIndex;
        }
        public Vector Vector
        {
            get
            {
                return vector;
            }
        }

        public int Row
        {
            get
            {
                return rowIndex;
            }
        }

        public int Column
        {
            get
            {
                return columnIndex;
            }
        }
    }

    public class SigmaTable : List<Sigma>
    {
    }
}
