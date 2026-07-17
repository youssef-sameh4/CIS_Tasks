using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Week3_LINQ.Model;

namespace Week3_LINQ
{
    public class LinqQueries
    {
        
        public LinqQueries()
        {
            Repository.LoadAllData();
        }


        public List<Heater> FunctionalProgrammingMethod()
        {
            return Repository.Heaters
                             .Where(h => h.PowerValue > 1500)
                             .ToList();
        }
        public List<Heater> FunctionalProgrammingQuery()
        {
            var result =
                (from h in Repository.Heaters
                 where h.PowerValue > 1500
                 select h).ToList();

            return result;
        }
        public object ProjectionQueries()
        {
            var result = Repository.Houses
                .Select(h => new
                {
                    h.HouseId,

                    OwnerName = Repository.Owners
                        .First(o => o.OwnerId == h.OwnerId)
                        .OwnerName,

                    TotalMonthlyHours = h.DailyUsages
                        .Sum(d => d.HoursWorked)
                })
                .ToList();

            return result;
        }
        public object ProjectionSelectMany()
        {
            var result = Repository.Houses
                .SelectMany(h =>
                    h.DailyUsages.Select(d => new
                    {
                        h.Address,
                        d.UsageDate,
                        d.HoursWorked
                    }))
                .ToList();

            return result;
        }
        public List<House> SortHouse()
        {
            return Repository.Houses
                .OrderByDescending(h =>
                    h.DailyUsages.Sum(x => x.HoursWorked))
                .ToList();
        }
        public object SortHeaters()
        {
            return Repository.Houses
                .Select(h => new
                {
                    h.HouseId,

                    Heaters = h.Heaters
                        .OrderByDescending(x => x.PowerValue)
                        .ToList()
                })
                .ToList();
        }
        public bool AnyPower()
        {
            return Repository.Houses
                .Any(h => h.Heaters.Any(x => x.PowerValue > 2000));
        }
        public bool AllHours()
        {
            return Repository.Houses
                .First()
                .DailyUsages
                .All(x => x.HoursWorked <= 24);
        }
        public bool ContainsPower()
        {
            return Repository.Heaters
                .Select(h => h.PowerValue)
                .Contains(1500);
        }
        public object JoinQueries()
        {
            var result =
                Repository.Houses.Join(

                    Repository.Owners,

                    house => house.OwnerId,
                    owner => owner.OwnerId,

                    (house, owner) => new
                    {
                        owner.OwnerName,
                        house.Address
                    });

            return result.ToList();
        }
        public List<House> SkipTwo()
        {
            return Repository.Houses
                .Skip(2)
                .ToList();
        }
        public List<DailyUsage> TakeWhileQuery(double limit)
        {
            double total = 0;

            return Repository.DailyUsages
                .OrderBy(d => d.UsageDate)
                .TakeWhile(d =>
                {
                    total += d.HoursWorked;
                    return total <= limit;
                })
                .ToList();
        }
        public List<DailyUsage> SkipWhileQuery(double limit)
        {
            double total = 0;

            return Repository.DailyUsages
                .OrderBy(d => d.UsageDate)
                .SkipWhile(d =>
                {
                    total += d.HoursWorked;
                    return total <= limit;
                })
                .ToList();
        }
        public object GroupByQuery()
        {
            var result = Repository.DailyUsages
                .GroupBy(d => d.HouseId)
                .Select(g => new
                {
                    HouseId = g.Key,
                    AverageHours = g.Average(x => x.HoursWorked)
                });

            return result.ToList();
        }
        public ILookup<string, Heater> LookupQuery()
        {
            return Repository.Heaters
                .ToLookup(h => h.HeaterType);
        }
        public object GroupJoinQuery()
        {
            var result = Repository.Houses.GroupJoin(

                Repository.DailyUsages,

                house => house.HouseId,
                usage => usage.HouseId,

                (house, usages) => new
                {
                    house.Address,
                    TotalHours = usages.Sum(x => x.HoursWorked)
                });

            return result.ToList();
        }
        public List<DateTime> GenerateLast30Days()
        {
            return Enumerable.Range(0, 30)
                .Select(i => DateTime.Today.AddDays(-i))
                .ToList();
        }
        public object SelectIndex()
        {
            return Repository.DailyUsages
                .Select((usage, index) => new
                {
                    Index = index,
                    usage.UsageDate,
                    usage.HoursWorked
                })
                .ToList();
        }
        public void DeferredExecution()
        {
            var query = Repository.Heaters
                .Where(h => h.PowerValue > 1500);

            Repository.Heaters.Add(new Heater
            {
                HeaterId = Guid.NewGuid(),
                HouseId = Guid.NewGuid(),
                HeaterType = "Electric",
                PowerValue = 3000
            });

            foreach (var heater in query)
            {
                Console.WriteLine($"{heater.HeaterType} - {heater.PowerValue}");
            }
        }
        public void ImmediateExecution()
        {
            var query = Repository.Heaters
                .Where(h => h.PowerValue > 1500)
                .ToList();

            Repository.Heaters.Add(new Heater
            {
                HeaterId = Guid.NewGuid(),
                HouseId = Guid.NewGuid(),
                HeaterType = "Electric",
                PowerValue = 3000
            });

            foreach (var heater in query)
            {
                Console.WriteLine($"{heater.HeaterType} - {heater.PowerValue}");
            }
        }
    }
}
