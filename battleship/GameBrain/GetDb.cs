using DAL;
using Microsoft.EntityFrameworkCore;

namespace GameBrain
{
    public class GetDb
    {
        public static DbContextOptions<AppDbContext> GetDbContextOptions()
        {
            var db = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer(@"
                    Server=barrel.itcollege.ee,1533;
                    User Id=student;
                    Password=Student.Bad.password.0;
                    Database=hlepik_battleship;
                    MultipleActiveResultSets=true")
                    .Options;

            return db;
        }
    }
}