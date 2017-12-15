using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Non_dominated_vectors_and_strategies
{
    public class Task
    // Класс хранящий параметры задачи
    // dimension - размерность задачи (n)
    // limit - b из D(n,b)
    // firstCriterionCoefficients - список коэффициентов первого критерия
    // secondCriterionCoefficients - список коэффициентов второго критерия
    // limitationCoefficients - список коэффициентов ограничения
    {
        int demention, limit;
        List<int> firstCriterionCoefficients = new List<int>();
        List<int> secondCriterionCoefficients = new List<int>();
        List<int> limitationCoefficients = new List<int>();

        public void SetData(int demention, int limit, List<int>[] inputCoefficients)
        {
            this.demention = demention;
            this.limit = limit;
            this.firstCriterionCoefficients = inputCoefficients[0];
            this.secondCriterionCoefficients = inputCoefficients[1];
            this.limitationCoefficients = inputCoefficients[2];
        }
    }
}
