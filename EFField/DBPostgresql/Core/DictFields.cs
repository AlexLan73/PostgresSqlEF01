using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using DBPostgresql.Model;

namespace DBPostgresql.Core
{
    public enum TypeDb
    {
        Project, Car, Mem, Trigger, FirstPath, SecondPath
    }
    public class DictFields
    {
        #region == Data ==
        #region ___ Dict ____
        private Dictionary<TypeDb, Dictionary<string, long>> _dDb { get; set; }
        #endregion
        private readonly PostgresContext _context;

        #endregion

        public DictFields(PostgresContext context)
        {
            _context = context;
            _dDb = new Dictionary<TypeDb, Dictionary<string, long>>();

            foreach (TypeDb _typeDb in Enum.GetValues(typeof(TypeDb)))
                _dDb[_typeDb] = new Dictionary<string, long>();
        }
        public void Set(TypeDb typeDb, Dictionary<string, long> d)
        { 
            _dDb[typeDb] = new Dictionary<string, long>(d);
        }

        public long Get(TypeDb typeDb, string key) => _dDb[typeDb].ContainsKey(key) ? _dDb[typeDb][key] : -1;

        public List<long> Get(TypeDb typeDb, string[] key)
        {
            List<long> _ls = new List<long>();
            key.ToList().ForEach(x => 
                {
                    if (_dDb[typeDb].ContainsKey(x))
                        _ls.Add(_dDb[typeDb][x]);
                });
            return _ls;
        }
         
        public string[] GetNotFields(TypeDb typeDb,  string[] key)
            => key.ToList().Where(x => !_dDb[typeDb].ContainsKey(x)).ToArray();


        public SecondPath TestRecDbOneSecond(string firstPath, string secondPath) //TestRecDbOneSecond
        {
            TestRecDb(TypeDb.FirstPath, firstPath);

            //            long _firstPathid = Get(TypeDb.FirstPath, firstPath);
            SecondPath? _secondPath = _context.RSecondPaths.GetName<SecondPath>(secondPath);
            if (_secondPath == null)
                _context.RSecondPaths.Add(new SecondPath { Name = secondPath, FirstPath = _context.RFirstPaths.GetName<FirstPath>(firstPath) });

            return ((SecondPath)_context.RSecondPaths.GetName<SecondPath>(secondPath));
        }

        //public long TestRecDbOneSecond(string firstPath, string secondPath) //TestRecDbOneSecond
        //{
        //    TestRecDb(TypeDb.FirstPath, firstPath);

        //    //            long _firstPathid = Get(TypeDb.FirstPath, firstPath);
        //    SecondPath? _secondPath = _context.RSecondPaths.GetName<SecondPath>(secondPath);
        //    if (_secondPath == null)
        //        _context.RSecondPaths.Add(new SecondPath { Name = secondPath, FirstPath = _context.RFirstPaths.GetName<FirstPath>(firstPath) });

        //    return ((SecondPath)_context.RSecondPaths.GetName<SecondPath>(secondPath)).SecondPathId;
        //}


        public void TestRecDb(List<(string, string)> key)
        {
            string[] _firstpaths = key.Select(x => x.Item1).Distinct().ToArray();
            TestRecDb(TypeDb.FirstPath, _firstpaths);
            string[] _secondpaths = key.Select(x => x.Item2).Distinct().ToArray();
            Dictionary<string, long> _sd = _context.RSecondPaths
                                                .GetName<SecondPath>(_secondpaths)
                                                .ToDictionary(x=>x.Name, z=>z.SecondPathId);

            SecondPath[] _lsec = key.Where(x => !_sd.ContainsKey(x.Item2))
                .Select(x=> new SecondPath { Name = x.Item2, FirstPath = _context.RFirstPaths.GetName<FirstPath>(x.Item1) }).ToArray();

            if (_lsec.Length > 0)
                _context.RSecondPaths.Add(_lsec);

            Set(TypeDb.SecondPath, _context.RSecondPaths.GetName<SecondPath>(_secondpaths).ToDictionary(x => x.Name, z => z.SecondPathId));

        }

        public void TestRecDb(List<string[]> key)
        {
            var triggers = key.SelectMany(p => p).Distinct().ToArray();
            TestRecDb(TypeDb.Trigger, triggers);
        }

        public void TestRecDb(TypeDb typeDb, string[] key)
        {
            string[] _keyNot = GetNotFields(typeDb, key);
            if (_keyNot.Length == 0)
                return;

            switch (typeDb)
            {
                case TypeDb.Project:
                    {
                        _context.RProjects.Add<Project>(_keyNot);
                        Set(typeDb, _context.RProjects.Get().ToDictionary(x => x.Name, z => z.ProjectId));
                        return;
                    }
                case TypeDb.Car:
                    {
                        _context.RCars.Add<Car>(_keyNot);
                        Set(typeDb, _context.RCars.Get().ToDictionary(x => x.Name, z => z.CarId));
                        return;
                    }

                case TypeDb.Mem:
                    {
                        _context.RMems.Add<Mem>(_keyNot);
                        Set(typeDb, _context.RMems.Get().ToDictionary(x => x.Name, z => z.MemId));
                        return;
                    }
                case TypeDb.Trigger:
                    {
                        _context.RTriggers.Add<Trigger>(_keyNot);
                        Set(typeDb, _context.RTriggers.Get().ToDictionary(x => x.Name, z => z.TriggerId));
                        return;
                    }
                case TypeDb.FirstPath:
                    {
                        _context.RFirstPaths.Add<FirstPath>(_keyNot);
                        Set(typeDb, _context.RFirstPaths.Get().ToDictionary(x => x.Name, z => z.FirstPathId));
                        return;
                    }

            }
        }



         public void TestRecDb(TypeDb typeDb, string key)
         {
            if (Get(typeDb, key) > 0)
                return;
            switch (typeDb)
            {
                case TypeDb.Project:
                    {
                        _context.RProjects.Add<Project>(key);
                        Set(typeDb, _context.RProjects.Get().ToDictionary(x => x.Name, z => z.ProjectId));
                        return;
                    }
                case TypeDb.Car:
                    {
                        _context.RCars.Add<Car>(key);
                        Set(typeDb, _context.RCars.Get().ToDictionary(x => x.Name, z => z.CarId));
                        return;
                    }
                   
                case TypeDb.Mem:
                    {
                        _context.RMems.Add<Mem>(key);
                        Set(typeDb, _context.RMems.Get().ToDictionary(x => x.Name, z => z.MemId));
                        return;
                    }
                case TypeDb.Trigger:
                    {
                        _context.RTriggers.Add<Trigger>(key);
                        Set(typeDb, _context.RTriggers.Get().ToDictionary(x => x.Name, z => z.TriggerId));
                        return;
                    }
                case TypeDb.FirstPath:
                    {
                        _context.RFirstPaths.Add<FirstPath>(key);
                        Set(typeDb, _context.RFirstPaths.Get().ToDictionary(x => x.Name, z => z.FirstPathId));
                        return;
                    }
                case TypeDb.SecondPath:
                    {
                        _context.RSecondPaths.Add<SecondPath>(key);
                        Set(typeDb, _context.RSecondPaths.Get().ToDictionary(x => x.Name, z => z.SecondPathId));
                        return;
                    }
            }


        }

    }
}
