using EnlightedWorkService.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnlightedWorkService.Services
{
    public class DbOperations
    {
        private EnlightedDbContext _dbContext;
        private readonly string _connectionString;

        public DbOperations(string conn)
        {
            _connectionString = conn;
        }

        private DbContextOptions<EnlightedDbContext> GetAllOptions()
        {
            DbContextOptionsBuilder<EnlightedDbContext> optionsBuilder = new DbContextOptionsBuilder<EnlightedDbContext>();

            optionsBuilder.UseMySQL(this._connectionString);

            return optionsBuilder.Options;
        }

        public Data.Floor? GetFloorById(int floorId)
        {
            using (_dbContext = new EnlightedDbContext(GetAllOptions()))
            {
                try
                {
                    return _dbContext.Floors.FirstOrDefault(f=> f.Id == floorId);
                }
                catch (Exception ex)
                {
                   Console.WriteLine($"ERROR IN {nameof(GetFloorById)} :: {ex.Message}");
                    return null;
                }
            }
        }

        public Data.Fixture? GetFixtureById(int floorId)
        {
            using (_dbContext = new EnlightedDbContext(GetAllOptions()))
            {
                try
                {
                    return _dbContext.Fixtures.FirstOrDefault(f => f.Id == floorId);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ERROR IN {nameof(GetFixtureById)} :: {ex.Message}");
                    return null;
                }
            }
        }

        public async Task<int> SaveFloors(List<Data.Floor> floors)
        {
            using (_dbContext = new EnlightedDbContext(GetAllOptions()))
            {
                try
                {
                    Console.WriteLine($" fcount --> {floors.Count}");
                    _dbContext.Floors.AddRange(floors);
                  var r = await _dbContext.SaveChangesAsync();
                    Console.WriteLine($" r --> {r}");
                    return r;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ERROR IN {nameof(SaveFloors)} :: {ex.Message}");
                    return 0;
                }
            }
        }

        public async Task<int> SaveFixtures(List<Data.Fixture> fixtures)
        {
            using (_dbContext = new EnlightedDbContext(GetAllOptions()))
            {
                try
                {
                    await _dbContext.Fixtures.AddRangeAsync(fixtures);
                   return await _dbContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ERROR IN {nameof(SaveFixtures)} :: {ex.Message}");
                    return 0;
                }
            }
        }
    }
}
