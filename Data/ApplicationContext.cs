using FactoryBrick.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;

namespace FactoryBrick.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Consumer> Consumers { get; set; } = null!;
        public DbSet<ConsumerType> ConsumerTypes { get; set; } = null!;
        public DbSet<ConsumptionData> ConsumptionDatas { get; set; } = null!;
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }
        public void SaveJsonToBase(JsonRoot root)
        {
            foreach (var consumer in root.Houses)
            {
                consumer.ConsumerTypeId = ConsumerType.houseId;
                if (!Consumers.Any(x => x.Id == consumer.Id))
                {
                    Consumers.Add(consumer);
                }
                foreach (var inputConsumpion in consumer.Consumptions)
                {
                    var consumtion = ConsumptionDatas.FirstOrDefault(x => x.Date == inputConsumpion.Date && x.ConsumerId == consumer.Id);
                    if (consumtion != null)
                    {
                        consumtion.ImportDateTime = DateTime.Now;
                        consumtion.Dependence = inputConsumpion.Dependence;
                        consumtion.Consumption = inputConsumpion.Consumption;
                    }
                    else
                    {
                        inputConsumpion.ImportDateTime = DateTime.Now;
                        inputConsumpion.ConsumerId = consumer.Id;
                        Add(inputConsumpion);
                    }
                }
            }
            foreach (var consumer in root.Plants)
            {
                consumer.ConsumerTypeId = ConsumerType.plantId;
                if (!Consumers.Any(x => x.Id == consumer.Id))
                {
                    Consumers.Add(consumer);
                }
                foreach (var inputConsumpion in consumer.Consumptions)
                {
                    var consumtion = ConsumptionDatas.FirstOrDefault(x => x.Date == inputConsumpion.Date && x.ConsumerId == consumer.Id);
                    if (consumtion != null)
                    {
                        consumtion.ImportDateTime = DateTime.Now;
                        consumtion.Dependence = inputConsumpion.Dependence;
                        consumtion.Consumption = inputConsumpion.Consumption;
                    }
                    else
                    {
                        inputConsumpion.ImportDateTime = DateTime.Now;
                        inputConsumpion.ConsumerId = consumer.Id;
                        Add(inputConsumpion);
                    }
                }
            }
            SaveChanges();
        }


        public DependencyGrpaphLabels GetDataLayerCake(int type)
        {
            var dependencyGrpaphL = new DependencyGrpaphLabels() { };
            var data = Consumers.Where(x => x.ConsumerTypeId == type).Include(x => x.Consumptions).ToList();
            dependencyGrpaphL.Lables = new List<object>();
            dependencyGrpaphL.DependencyGrpaphs = new List<DependencyGrpaph> { };
            foreach (var consumer in data)
            {
                dependencyGrpaphL.Lables.AddRange(consumer.Consumptions.Select(x => x.Date as object));
                dependencyGrpaphL.DependencyGrpaphs.Add(new DependencyGrpaph(consumer.Name, consumer.Consumptions.Select(x => new Dataset(x.Date, x.Consumption)).ToList()));
            }
            dependencyGrpaphL.Lables.Sort();
            dependencyGrpaphL.Lables = dependencyGrpaphL.Lables.Distinct().ToList();
            return dependencyGrpaphL;
        }

        public List<Consumer> GetConsumerWithData(int type, DateTime? dtFrom, DateTime? dtTo)
        {
            List<Consumer> consumers;
            if (dtFrom != null && dtTo != null)
            {
                consumers = Consumers.Where(x => x.ConsumerTypeId == type).Include(x => x.Consumptions.Where(x => x.Date >= dtFrom.Value && x.Date <= dtTo.Value)).ToList();
            }
            else if (dtFrom != null)
            {
                consumers = Consumers.Where(x => x.ConsumerTypeId == type).Include(x => x.Consumptions.Where(x => x.Date >= dtFrom.Value)).ToList();
            }
            else if (dtTo != null)
            {
                consumers = Consumers.Where(x => x.ConsumerTypeId == type).Include(x => x.Consumptions.Where(x => x.Date <= dtTo.Value)).ToList();
            }
            else
            {
                consumers = Consumers.Where(x => x.ConsumerTypeId == type).Include(x => x.Consumptions).ToList();
            }
            return consumers;
        }

    }
}
