using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;


namespace DbDynamic.DB
{
    public class MyDbSet:DbSet<Blog>
    {
        public string Name { get; set; }
        public DbSet<Blog> XDb { get; set; }
        public MyDbSet(string name) : base()
        {
            Name = name;
        }
    }
    public class DbTradeContext : DbContext
    {
        #region   ==> basic parameters
        private int _port;
        private string _dataBaseName;
        private string _username;
        private string _password;
        #endregion
        private string _connectDBparams;
        //        public DbSet<Blog> Blogs { get; set; }
//        public List<MyDbSet> LBlogs { get; set; }
        public List<DbSet<Blog>> LBlogs { get; set; }

        public DbTradeContext(int port = 9000, string dataBaseName = "DbTrade", string username = "postgres", string password = "123")
        {
            Console.WriteLine($" --->  {GetType().Name}  <----");

            _port = port;
            _dataBaseName = dataBaseName;
            _username = username;
            _password = password;
            _connectDBparams = $"Host=localhost; Port={_port}; Database={_dataBaseName}; Username={_username}; Password={_password}";
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
                    optionsBuilder.UseNpgsql(_connectDBparams);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var _x0 = new MyDbSet("Db1");
            LBlogs.Add(_x0.XDb);
            LBlogs.Add(new MyDbSet("Db2"));
            LBlogs.Add(new MyDbSet("Db3"));
            LBlogs.Add(new MyDbSet("Db4"));

            //modelBuilder.Entity<AuditEntry>();
            //            modelBuilder.Entity<Blog>().ToTable("blogs1",  "blogging1");
            //            modelBuilder.Entity<Blog>().ToTable("blogs2", schema: "blogging2");
            //            modelBuilder.Entity<Blog>().ToTable("blogs3", schema: "blogging3");
        }
    }
}
