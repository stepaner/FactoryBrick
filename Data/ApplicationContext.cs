﻿using FactoryBrick.Models;
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
            Database.Migrate();
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


        public DependencyGrpaphLabels GetDataLayerCake()
        {
           var dependencyGrpaphL = new DependencyGrpaphLabels() { };
            List<Consumer> data = Consumers.Include(x => x.Consumptions).ToList();
           var dataset = ConsumptionDatas.GroupBy(x => x.Date).Select(g => new Dataset(g.Key, g.Sum(x => x.Consumption))).ToList();
            dataset =  dataset.OrderBy(x => x.X).ToList();
            foreach (var consumer in data)
            {
                dependencyGrpaphL.Lables.AddRange(consumer.Consumptions.Select(x => x.Date as object));
                dependencyGrpaphL.DependencyGrpaphs.Add(new DependencyGrpaph(consumer.Name, consumer.Consumptions.Select(x => new Dataset(x.Date, x.Consumption)).ToList()));
            }

            dependencyGrpaphL.DependencyGrpaphs.Add(new DependencyGrpaph("Общее потребление", dataset));

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

        public List<ConsumptionData> GetConsumption(int type, DateTime? dtFrom = null, DateTime? dtTo = null)
        {
            IEnumerable<ConsumptionData> consumptionData;
            if (dtFrom != null && dtTo != null)
            {
                consumptionData = ConsumptionDatas.Where(x => x.Date >= dtFrom.Value && x.Date <= dtTo.Value && x.Consumer.ConsumerTypeId == type).Where(x => x.Consumer.ConsumerTypeId == type);
            }
            else if (dtFrom != null)
            {
                consumptionData = ConsumptionDatas.Where(x => x.Date >= dtFrom.Value && x.Consumer.ConsumerTypeId == type).Where(x => x.Consumer.ConsumerTypeId == type);
            }
            else if (dtTo != null)
            {
                consumptionData = ConsumptionDatas.Where(x => x.Date <= dtTo.Value && x.Consumer.ConsumerTypeId == type).Where(x => x.Consumer.ConsumerTypeId == type);
            }            
            else
            {               
                consumptionData = ConsumptionDatas.Where(x => x.Consumer.ConsumerTypeId == type);
            }
            return consumptionData.ToList();
        }

    }
}
