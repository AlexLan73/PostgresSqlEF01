//using DBPostgresql.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using DBPostgresql.Model;
//using ImTools;

namespace DBPostgresql.Core
{
    public class PostgresContext:DbContext
    {
        #region Data    
        #region   ==> basic parameters
        private int _port;
        private string _dataBaseName;
        private string _username;
        private string _password;
        #endregion

        #region  ___ DB ____
        public DbSet<Project> Projects { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Mem> Mems { get; set; }
        public DbSet<Trigger> Triggers { get; set; }
        public DbSet<FirstPath> FirstPaths { get; set; }
        public DbSet<SecondPath> SecondPaths { get; set; }
        public DbSet<ClfFind> ClfFinds { get; set; }
        #endregion

        #region  ____ Repository  ______
        public Repository<Project> RProjects { get; set; }
        public Repository<Car> RCars { get; set; }
        public Repository<Mem> RMems { get; set; }
        public Repository<Trigger> RTriggers { get; set; }
        public Repository<FirstPath> RFirstPaths { get; set; }
        public Repository<SecondPath> RSecondPaths { get; set; }
        public RepositoryClfFind RClfFind { get; set; }
        #endregion

        private DictFields _dFiels;


        #endregion

        public PostgresContext(int port = 9000, string dataBaseName = "DbCLFEF", string username = "postgres", string password = "123")
        {
            Console.WriteLine($" --->  {GetType().Name}  <----");
            SetupParam(port, dataBaseName, username, password);
        }
        public void SetupParam(int port, string dataBaseName, string username, string password)
        {
            _port = port;
            _dataBaseName = dataBaseName;
            _username = username;
            _password = password;
            DBManager.ConnectDBparams = $"Host=localhost; Port={_port}; Database={_dataBaseName}; Username={_username}; Password={_password}";
            Database.EnsureCreated();
            RProjects = new Repository<Project>(this);
            RCars = new Repository<Car>(this);
            RMems = new Repository<Mem>(this);
            RTriggers = new Repository<Trigger>(this);
            RFirstPaths = new Repository<FirstPath>(this);
            RSecondPaths = new Repository<SecondPath>(this);
            RClfFind = new RepositoryClfFind(this);

            _dFiels = new DictFields(this);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
                    optionsBuilder.UseNpgsql(DBManager.ConnectDBparams);
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
/*            modelBuilder.Entity<SecondPath>()
                .HasOne(p => p.FirstPath);

            modelBuilder.Entity<ClfFind>()
                .HasOne(x => x.Project);
            modelBuilder.Entity<ClfFind>()
                .HasOne(x => x.Car);
            modelBuilder.Entity<ClfFind>()
                .HasOne(x => x.Mem);
            modelBuilder.Entity<ClfFind>()
                .HasOne(x => x.SecondPath);
*/
            //            modelBuilder.Entity<ClfFind>().HasMany(x=>x.Triggers).WithOne().HasForeignKey(o=>o.TriggerId);
                //.HasOne(x=>x.Triggers)
                //.WithOne(x => x.)

        }
        private void SetDictionary()
        {
            _dFiels.Set(TypeDb.Project, RProjects.Get().ToDictionary(x => x.Name, z => z.ProjectId));
            _dFiels.Set(TypeDb.Car, RCars.Get().ToDictionary(x => x.Name, z => z.CarId));
            _dFiels.Set(TypeDb.Mem, RMems.Get().ToDictionary(x => x.Name, z => z.MemId));
            _dFiels.Set(TypeDb.Trigger, RTriggers.Get().ToDictionary(x => x.Name, z => z.TriggerId));
            _dFiels.Set(TypeDb.FirstPath, RFirstPaths.Get().ToDictionary(x => x.Name, z => z.FirstPathId));
        }

        #region ----> Create  <------
        public void Create()
        {
            RProjects.AddTest<Project>(new[] { " ", "UMP" });
            RMems.AddTest(new Mem { Name = "M1" });
            RMems.AddTest(new Mem { Name = "M2" });
            RTriggers.AddTest<Trigger>(" ");
            RTriggers.AddTest<Trigger>(new[] {"RemKey1", "ABS_Lamp_1", "CCU_Failure_gt_0", "EBD_Lamp_1", "BLS_Fault_1",
                                        "EPB_FailureSts_1", "HvSystemFailure_gt_1", "MIL_OnRq_1", "stGbxMILReq_1",
                                        "TM_WarningInd_1", "CCU_Failure_gte_1", "HVC_CompStatus_3", "ERAG_FailureSts_gte_1"  });
            RFirstPaths.AddTest<FirstPath>(@"\\mlmsrv\MLServer");
            RFirstPaths.AddTest<FirstPath>(@"E:\MLserver\data");

            SetDictionary();
        }
        #endregion

        #region ---->  Get<T>()   <-------
        public List<T> GetAll<T>(DbSet<T> dbsourses) where T : class, new()
            => dbsourses.ToList() as List<T>;
        #endregion

        public void SetDan(string project, string car, string mem,
                DateTime dtStart, DateTime dtEnd, SecondPath secondpath, string[] triggers)
        {

            ClfFind clfFind = new ClfFind()
            {
                Project = RProjects.GetName<Project>(project),
                Car = RCars.GetName<Car>(car),
                Mem = RMems.GetName<Mem>(mem),
                DateTimeStart = dtStart,
                DateTimeEnd = dtEnd,
                SecondPath = secondpath,
                Triggers = _dFiels.Get(TypeDb.Trigger, triggers)
            };

            RClfFind.Add(clfFind);
        }

        private void SetDan(List<(string, string, string, DateTime, DateTime, string, string, string[])> sourse)
        {
            var _project = sourse.Select(x => x.Item1).Distinct().ToArray<string>();
            _dFiels.TestRecDb(TypeDb.Project, _project);

            var _car = sourse.Select(x => x.Item2).Distinct().ToArray<string>();
            _dFiels.TestRecDb(TypeDb.Car, _car);

            var _mem = sourse.Select(x => x.Item3).Distinct().ToArray<string>();
            _dFiels.TestRecDb(TypeDb.Mem, _mem);

            List<(string, string)> ps = sourse.Select(x => (x.Item6, x.Item7)).ToList<(string, string)>();
            _dFiels.TestRecDb(ps);

            List<string[]> rg = sourse.Select(x => x.Item8).ToList<string[]>();
            _dFiels.TestRecDb(rg);

            List<ClfFind> clfFinds = new List<ClfFind>();
            sourse.ForEach(x =>
            {
                clfFinds.Add(new ClfFind
                {
                    Project = RProjects.GetName<Project>(x.Item1),
                    Car = RCars.GetName<Car>(x.Item2),
                    Mem = RMems.GetName<Mem>(x.Item3),
                    DateTimeStart = x.Item4,
                    DateTimeEnd = x.Item5,
                    SecondPath = RSecondPaths.GetName<SecondPath>(x.Item7),
                    Triggers = _dFiels.Get(TypeDb.Trigger, x.Item8)
                });
            });
            RClfFind.Add(clfFinds.ToArray());
        }
        private void testdb01()
        {
            string project = "UMP";
            _dFiels.TestRecDb(TypeDb.Project, project);

            string car = "PS33SED";
            _dFiels.TestRecDb(TypeDb.Car, car);

            string mem = "M1";
            _dFiels.TestRecDb(TypeDb.Mem, mem);

            DateTime dtStart = new DateTime(2021, 02, 02, 09, 37, 55);
            DateTime dtEnd = new DateTime(2021, 02, 02, 12, 30, 58);
            string firstpath = @"\\mlmsrv\MLServer";
            string secondpath = @"\PS33SED\log\2021-02-02_12-31-20\CLF\PS33SED_M1_(2021-02-02_09-37-55)_(2021-02-02_12-30-58)";

            string[] triggers = new string[] {" 23-1", "EPB_FailureSts_1", "HvSystemFailure_gt_1", "MIL_OnRq_1", "stGbxMILReq_1",
                                        "TM_WarningInd_1", "CCU_Failure_gte_1", "HVC_CompStatus_3"};

            var _xtriggers = _dFiels.GetNotFields(TypeDb.Trigger, triggers);
            if (_xtriggers.Length > 0)
            {
                RTriggers.Add<Trigger>(_xtriggers);
                _dFiels.Set(TypeDb.Trigger, RTriggers.Get().ToDictionary(x => x.Name, z => z.TriggerId));
            }

            SetDan(project, car, mem, dtStart, dtEnd, _dFiels.TestRecDbOneSecond(firstpath, secondpath), triggers); //TestRecDbOneSecond

        }

        private void testdb02()
        {
            List<(string project, string car, string mem,
                            DateTime dtStart, DateTime dtEnd, string firstpath, string secondpath, string[] triggers)>
                            _ls = new List<(string project, string car, string mem,
                            DateTime dtStart, DateTime dtEnd, string firstpath, string secondpath, string[] triggers)>();
            string project = "UMP";
            string car = "PS33SED";
            string mem = "M1";

            DateTime dtStart = new DateTime(2021, 02, 02, 09, 37, 55);
            DateTime dtEnd = new DateTime(2021, 02, 02, 12, 30, 58);
            string firstpath = @"\\mlmsrv\MLServer";
            string secondpath = @"\PS33SED\log\2021-02-02_12-31-20\CLF\PS33SED_M1_(2021-02-02_09-37-55)_(2021-02-02_12-30-58)";

            string[] triggers = new string[] {" 23-1", "EPB_FailureSts_1", "HvSystemFailure_gt_1", "MIL_OnRq_1", "stGbxMILReq_1",
                                        "TM_WarningInd_1", "CCU_Failure_gte_1", "HVC_CompStatus_3"};
            //--------------------
            _ls.Add((project, car, mem, dtStart, dtEnd, firstpath, secondpath, triggers));
            _ls.Add((project, car, "M1", new DateTime(2021, 02, 03, 09, 37, 55), new DateTime(2021, 02, 03, 12, 30, 58)
                , firstpath,
                @"\PS33SED\log\2021-02-02_12-31-20\CLF\PS33SED_M1_(2021-02-03_09-37-55)_(2021-02-03_12-30-58)",
                new string[] { " 23-1", "CCU_Failure_gte_1", "HVC_CompStatus_3" }));


            _ls.Add((project, car, "M2", new DateTime(2021, 02, 04, 09, 37, 55), new DateTime(2021, 02, 04, 12, 30, 58)
                , firstpath,
                @"\PS33SED\log\2021-02-02_12-31-20\CLF\PS33SED_M2_(2021-02-04_09-37-55)_(2021-02-04_12-30-58)",
                new string[] {" 23-1", "EPB_FailureSts_1", "HvSystemFailure_gt_1", "MIL_OnRq_1", "stGbxMILReq_1",
                                        "TM_WarningInd_1", "CCU_Failure_gte_1", "HVC_CompStatus_3"}));

            _ls.Add((project, car, "M1", new DateTime(2021, 02, 05, 09, 37, 55), new DateTime(2021, 02, 05, 12, 30, 58)
                , firstpath,
                @"\PS33SED\log\2021-02-02_12-31-20\CLF\PS33SED_M1_(2021-02-05_09-37-55)_(2021-02-05_12-30-58)",
                new string[] { "HvSystemFailure_gt_1", "MIL_OnRq_1", "stGbxMILReq_1", "TM_WarningInd_1", "CCU_Failure_gte_1" }));

            _ls.Add((project, car, "M2", new DateTime(2021, 02, 06, 09, 37, 55), new DateTime(2021, 02, 06, 12, 30, 58)
                , firstpath,
                @"\PS33SED\log\2021-02-02_12-31-20\CLF\PS33SED_M2_(2021-02-06_09-37-55)_(2021-02-06_12-30-58)",
                new string[] { " 23-1", "HVC_CompStatus_3" }));

            _ls.Add((project, car, "M1", new DateTime(2021, 02, 07, 09, 37, 55), new DateTime(2021, 02, 07, 12, 30, 58)
                , firstpath,
                @"\PS33SED\log\2021-02-02_12-31-20\CLF\PS33SED_M1_(2021-02-07_09-37-55)_(2021-02-07_12-30-58)",
                new string[] { "HvSystemFailure_gt_1", "MIL_OnRq_1", "stGbxMILReq_1", "TM_WarningInd_1", "CCU_Failure_gte_1" }));

            _ls.Add((project, car, "M1", new DateTime(2021, 02, 08, 09, 37, 55), new DateTime(2021, 02, 08, 12, 30, 58)
                , firstpath,
                @"\PS33SED\log\2021-02-02_12-31-20\CLF\PS33SED_M1_(2021-02-08_09-37-55)_(2021-02-08_12-30-58)",
                new string[] { "MIL_OnRq_1", "stGbxMILReq_1", "TM_WarningInd_1", "CCU_Failure_gte_1", "HVC_CompStatus_3" }));

            _ls.Add((project, car, "M2", new DateTime(2021, 02, 10, 09, 37, 55), new DateTime(2021, 02, 10, 12, 30, 58)
                , firstpath,
                @"\PS33SED\log\2021-02-02_12-31-20\CLF\PS33SED_M2_(2021-02-10_09-37-55)_(2021-02-10_12-30-58)",
                new string[] { " 23-1" }));

            SetDan(_ls);
        }

        private void FindDb(Dictionary<TypeDb, string[]> finddb, DateTime[] dt = null)
        {
            Dictionary<TypeDb, long[]> _dtest = new Dictionary<TypeDb, long[]>()
            {
                { TypeDb.Project, new long[0]},
                { TypeDb.Car, new long[0] },
                { TypeDb.Mem, new long[0] },
                { TypeDb.Trigger, new long[0] }
            };

            foreach (var db in finddb)
                _dtest[db.Key] = _dFiels.Get(db.Key, db.Value).ToArray();

            Func<TypeDb, long, bool> f00 = (tx, d)
                => _dtest[tx].Length == 0 ? true : _dtest[tx].FirstOrDefault(x => x == d) == d;
            //.FindFirst(x => x == d) == d;
            Func<TypeDb, List<long>, bool> f01 = (tx, d)
                =>
                    {
                        if (_dtest[tx].Length == 0)
                            return true;
                        var duplicates = d.Intersect(_dtest[TypeDb.Trigger]);
                        if(duplicates.Count()>0)
                            return true;
                        return false;
                    };

            //            var _x = Set<ClfFind>().ToHashSet()
            DateTime dtstart = ClfFinds.ToHashSet().Max(x => x.DateTimeEnd);

            var _x = ClfFinds.ToHashSet()
                .Where(p => f00(TypeDb.Project, p.Project.ProjectId)
                    && f00(TypeDb.Car, p.Car.CarId)
                    && f00(TypeDb.Mem, p.Mem.MemId)
                    && f01(TypeDb.Trigger, p.Triggers)
                    )
                //.Where()
                .ToList();


        }

        public void TestDb()
        {
//            Create();
            SetDictionary();
            //            testdb01();
            //            testdb02();
            Dictionary<TypeDb, string[]> finddb = new Dictionary<TypeDb, string[]>();

            finddb.Add(TypeDb.Project, new[] { "UMP" });
            //            finddb.Add(TypeDb.Mem, new[] {"M2"});
            finddb.Add(TypeDb.Car, new[] { "PS33SED" });
//            finddb.Add(TypeDb.Trigger, new[] { " 23-1", "MIL_OnRq_1", "stGbxMILReq_1", "CCU_Failure_gte_1" });
            finddb.Add(TypeDb.Trigger, new[] { "TM_WarningInd_1" });
            FindDb(finddb);

        }
    }
}

