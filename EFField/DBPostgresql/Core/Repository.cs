using DBPostgresql.Core;
using DBPostgresql.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBPostgresql.Core
{
    public interface IRepository<T> where T : class,  new()
    {
        void Add(T entity);
        void Add<T>(string entity) where T : class, IBaseField, new();
        void Add<T>(string[] entity) where T : class, IBaseField, new();
        void AddTest<T>(T entity) where T : class, IBaseField;
        void AddTest<T>(T[] entity) where T : class, IBaseField;
        void AddTest<T>(string entity) where T : class, IBaseField, new();
        void AddTest<T>(string[] entity) where T : class, IBaseField, new();
        T GetById(long id);
        T GetName<T>(string name) where T : class, IBaseField, new();
        List<T> GetName<T>(string[] name) where T : class, IBaseField, new();
        IEnumerable<T> Get();
        void Remove(long id);
//        void Remove();
        void Save();
        void Update(T entity);
        void ClearTable<T>() where T : class;
    }

    public class Repository<T> : IRepository<T> where T : class, IBaseField, new()
    {
        private readonly PostgresContext _context;

        public Repository(PostgresContext context)
            => _context = context;

        #region ____ Add ____
        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
            _context.SaveChanges();
        }
        public void Add(T[] entity)
        {
            _context.Set<T>().AddRange(entity);
            _context.SaveChanges();

        }

        public void Add<T>(string entity) where T : class, IBaseField, new() 
        {
            _context.Set<T>().Add(new T() { Name = entity });
            _context.SaveChanges();
        }

        public void Add<T>(string[] entity) where T : class, IBaseField, new()
        {

            T[] f0(List<string> entityNo)
            {
                List<T> _ls = new List<T>();
                entityNo.ForEach(x => _ls.Add(new T() { Name = x }));
                return _ls.ToArray();
            }

            _context.Set<T>().AddRange(f0(entity.ToList()));
            _context.SaveChanges();

        }



        public void AddTest<T>(T entity) where T : class, IBaseField
        {
            if (_context.Set<T>().Count() == 0
                    || _context.Set<T>().FirstOrDefault<T>(x => x.Name == entity.Name) == null) //(entity as IBaseField).Name
            {
                _context.Set<T>().Add(entity);
                _context.SaveChanges();
            }
        }
        public void AddTest<T>(T[] entity) where T : class, IBaseField
        {
            Func<T, DbSet<T>, bool> fun = (t0, ts)
                => ts.FirstOrDefault(z => z.Name == t0.Name) == null ? true : false;

            if (_context.Set<T>().Count() > 0)
            {
                var entityNo = entity
                        //                        .ToList<T>()
                        .Where<T>(p => fun(p, _context.Set<T>()))
                        .ToList<T>();
                if (entityNo.Count() > 0)
                {
                    _context.Set<T>().AddRange(entityNo);
                    _context.SaveChanges();
                }
            }
            else
            {
                _context.Set<T>().AddRange(entity);
                _context.SaveChanges();
            }

        }

        public void AddTest<T>(string entity) where T : class, IBaseField, new()
        {
            if (_context.Set<T>().Count() == 0
                || _context.Set<T>().FirstOrDefault<T>(x => x.Name == entity) == null)
            {
                _context.Set<T>().Add(new T() { Name = entity });
                _context.SaveChanges();
            }

        }
        public void AddTest<T>(string[] entity) where T : class, IBaseField, new()
        {
            Func<string, DbSet<T>, bool> fun = (t0, ts)
                => ts.FirstOrDefault(z => z.Name == t0) == null ? true : false;

            T[] f0(List<string> entityNo)
            {
                List<T> _ls = new List<T>();
                entityNo.ForEach(x => _ls.Add(new T() { Name = x }));
                return _ls.ToArray();
            }

            if (_context.Set<T>().Count() > 0)
            {
                var entityNo = entity
                    .Where(p => fun(p, _context.Set<T>()))
                    .ToList();

                if (entityNo.Count() > 0)
                {
                    _context.Set<T>().AddRange(f0(entityNo));
                    _context.SaveChanges();
                }
            }
            else
            {
                _context.Set<T>().AddRange(f0(entity.ToList()));
                _context.SaveChanges();
            }

        }

        #endregion

        #region _____ Get  ____
        public IEnumerable<T> Get()
            => _context.Set<T>().ToList();

        public T GetName<T>(string name) where T : class, IBaseField, new() 
            => _context.Set<T>().FirstOrDefault(z => z.Name == name);

        public List<T> GetName<T>(string[] name) where T : class, IBaseField, new()
        {
            var _ls = new List<T>();
            name.ToList().ForEach(x => 
            {
                var _v = _context.Set<T>().FirstOrDefault<T>(z => z.Name == x);
                if (_v != null)
                    _ls.Add(_v);
            });
            return _ls;
        }
        public T GetById(long id)
            => _context.Set<T>().Find(id);


        #endregion

        #region ____   Clear Table ____
        public void ClearTable<T>() where T : class 
        {
            _context.Set<T>().RemoveRange(_context.Set<T>()); 
            _context.SaveChanges();
        }
        #endregion



        public void Remove(long id)
            => _context.Remove(_context.Set<T>().Find(id));

        public void Save()
            => _context.SaveChanges();

        public void Update(T entity)
            => _context.Set<T>().Attach(entity);


    }
}
