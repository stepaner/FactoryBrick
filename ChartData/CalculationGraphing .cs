using FactoryBrick.Models;
using Newtonsoft.Json;
using System.Data;

namespace FactoryBrick
{
    public class CalculationGraphing
    {
        private decimal a;
        private decimal b;
        private decimal max;
        private decimal min;

        private List<ConsumptionData> data;

        public static DependencyGrpaphLabels GetGraph(List<ConsumptionData> data)
        {
            var res = new CalculationGraphing(data);
            res.LinearRegression();
            var Graph = new DependencyGrpaphLabels();
            Graph.DependencyGrpaphs = new List<DependencyGrpaph>();
            var dataset = new List<Dataset>();
            Graph.Lables = new List<object>();
            for (decimal i = res.min; i < res.max; i++)
            {
                Graph.Lables.Add(i);
                dataset.Add(new Dataset(i, res.b * i + res.a));
            }
            Graph.DependencyGrpaphs.Add(new DependencyGrpaph("y = b*x + a", dataset));
            return Graph;

        }
        public CalculationGraphing(List<ConsumptionData> data) { this.data = data; }
        public void LinearRegression()
        {
            // Задаем исходные данные
            var dependence = data.Select(d => d.Dependence).ToList();
            var consumption = data.Select(d => d.Consumption).ToList();

            // Вычисляем среднее значение
            var averageDependence = dependence.Average();
            var averageConsumtion = consumption.Average();

            // Вычисляем коэффициенты a и b
            b = SumOfProducts(dependence, consumption) / SumOfSquares(dependence);
            a = averageConsumtion - b * averageDependence;
            max = dependence.Max();
            min = dependence.Min();

            // Выводим результаты
            // Console.WriteLine("Уравнение линейной зависимости: y = {0}x + {1}", b, a);
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
