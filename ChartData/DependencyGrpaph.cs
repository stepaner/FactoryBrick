using FactoryBrick.Data;
using FactoryBrick.Models;
using Microsoft.EntityFrameworkCore;
using static FactoryBrick.CalculationGraphing;

namespace FactoryBrick
{
    public class DependencyGrpaphLabels
    {
        public List<object> Lables { get; set; } = new List<object>();
        public List<DependencyGrpaph> DependencyGrpaphs { get; set; } = new List<DependencyGrpaph>();
        public Coefficients CoefficientsBZ { get; set; }
        public Coefficients CoefficientsAZ { get; set; }
        public static DependencyGrpaphLabels GetDependencyGrpaphLabels(List<Consumer> consumers)
        {
            List<DependencyGrpaph> dependencyGrpaphs = new List<DependencyGrpaph>();
            List<object> lables = new List<object>();

            foreach (var item in consumers)
            {
                var dataset = item.Consumptions.Select(x => new Dataset(Decimal.Round(x.Dependence), x.Consumption)).OrderBy(x => x.X).ToList();                
                dependencyGrpaphs.Add(new DependencyGrpaph(item.Name, dataset));                
                
                lables.AddRange(item.Consumptions.Select(x => (x.Dependence.GetType() == typeof(Decimal) ? Decimal.Round(x.Dependence) : x.Dependence) as object).ToList());
            }
            lables = lables.Distinct().OrderBy(lables => lables).ToList();
            return new DependencyGrpaphLabels() { DependencyGrpaphs = dependencyGrpaphs, Lables = lables };
        }
    }
    public class DependencyGrpaph
    {     
        public string Name { get; set; }
        public List<Dataset> Datasets { get; set; }
        public DependencyGrpaph(string name, List<Dataset> datasets)
        {
                Name = name;
                Datasets = datasets;
        }        
    }

    public class Dataset
    {
        public object X { get; set; }
        public decimal Y { get; set; }
        public Dataset(object x, decimal y) { X = x; Y = y; }
    }
    
}
