using FactoryBrick.Models;
using Newtonsoft.Json;
using System.Data;

namespace FactoryBrick
{
    public class CalculationGraphing
    {
        public class Coefficients
        {
            public decimal A { get; set; }
            public decimal B {get; set; }
        }
        
                   

        public static DependencyGrpaphLabels GetGraph(List<ConsumptionData> data, int cnt)
        {
            //var res = new CalculationGraphing(data);
            var dependence = data.Select(d => d.Dependence).ToList();
           
            var consumption = data.Select(d => d.Consumption).ToList();

            var dataBZ = data.TakeWhile(x => x.Dependence < 0).ToList();
            var dataAZ = data.SkipWhile(x => x.Dependence < 0).ToList();

            Coefficients coefficientsBZ = new Coefficients(), coefficientsAZ = new Coefficients();

            if (dataBZ.Count() != 0)
                coefficientsBZ = GetСoefficients(dataBZ);
            if (dataAZ.Count() != 0)
                coefficientsAZ = GetСoefficients(dataAZ);

            var Graph = new DependencyGrpaphLabels();
            var dataset = new List<Dataset>();
            var max = Math.Round(dependence.Max());
            var min = Math.Round(dependence.Min());

            for (decimal i = min; i < max; i++)
            {
                Graph.Lables.Add(i);
                dataset.Add(new Dataset(i, GetY(coefficientsBZ, coefficientsAZ, i)* cnt));
            }
            Graph.CoefficientsBZ = coefficientsBZ;
            Graph.CoefficientsAZ = coefficientsAZ;
            Graph.DependencyGrpaphs.Add(new DependencyGrpaph("y = b*x + a", dataset));
            return Graph;

        }
       
        public static decimal GetY(Coefficients сoefficientsBZ, Coefficients сoefficientsAZ, decimal i)
        {
            return i < 0 ? сoefficientsBZ.B * i + сoefficientsBZ.A : сoefficientsAZ.B * i + сoefficientsAZ.A;
        }

        public static Coefficients GetСoefficients(List<ConsumptionData> consumptionDatas)
        {
            // Задаем исходные данные
            var dependence = consumptionDatas.Select(x => x.Dependence).ToList();
            var consumption = consumptionDatas.Select(x => x.Consumption).ToList();

            // Вычисляем среднее значение
            var averageDependence = dependence.Average();
            var averageConsumtion = consumption.Average();

            var b = SumOfProducts(dependence, consumption) / SumOfSquares(dependence);
            var a = averageConsumtion - b * averageDependence;

            return new Coefficients() { B = b, A = a };
        }

        // Метод для вычисления суммы произведений элементов двух массивов
        public static decimal SumOfProducts(List<decimal> data1, List<decimal> data2)
        {
            decimal sum = 0;
            for (int i = 0; i < data1.Count; i++)
                sum += data1[i] * data2[i];
            return sum;
        }

        // Метод для вычисления суммы квадратов элементов массива
        public static decimal SumOfSquares(List<decimal> data)
        {
            decimal sum = 0;
            foreach (decimal d in data)
                sum += d * d;
            return sum;
        }


    }
}
