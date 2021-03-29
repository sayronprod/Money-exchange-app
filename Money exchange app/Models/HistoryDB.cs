using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Money_exchange_app.Models
{
    public class HistoryDB : DbContext
    {
        public DbSet<History> History { get; set; }
        public HistoryDB(DbContextOptions<HistoryDB> options)
        : base(options)
        {            
        }
    }

    public class History
    {
        public int Id { get; set; }
        public string FromCurrency { get; set; }
        public double FromAmount { get; set; }
        public string ToCurrency { get; set; }
        public double ToAmount { get; set; }
        public string Date { get; set; }
    }
}
