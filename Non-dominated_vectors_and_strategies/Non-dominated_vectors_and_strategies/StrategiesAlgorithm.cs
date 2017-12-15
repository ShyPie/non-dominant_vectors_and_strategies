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
        public StrategiesAlgorithm()
        {
        }

        
        public List<List<int>> Run(SigmaTable sigmaTable, VectorSet nonDominatedVectors) 
        {
            sigmaTable = new SigmaTable();
            List<List<int>> strategies = new List<List<int>>();
            int u, u0, ksi, eta;
            //here will be algorithm
            return strategies;
        }
    }

    
}
