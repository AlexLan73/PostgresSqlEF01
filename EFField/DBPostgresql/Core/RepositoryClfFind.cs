using DBPostgresql.Core;
using DBPostgresql.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBPostgresql.Core
{
    public interface IRepositoryClfFind
    {
        void Add(ClfFind entity);
        IEnumerable<ClfFind> Get();
        ClfFind GetById(long id);
    }

    public class RepositoryClfFind : IRepositoryClfFind
    {
        private readonly PostgresContext _context;
        private DbSet<ClfFind> _clf;

        public RepositoryClfFind(PostgresContext context)
        {
            _context = context;
            _clf = _context.Set<ClfFind>();
        }

        public void Add(ClfFind entity)
        {
            _clf.Add(entity);
            _context.SaveChanges();
        }
        public void Add(ClfFind[] entity)
        {
            _clf.AddRange(entity);
            _context.SaveChanges();
        }
        

        public IEnumerable<ClfFind> Get()
            => _clf.ToList();

        public ClfFind GetById(long id)
            => _clf.Find(id);

        //public void Remove(long id)
        //    => _context.Remove(_context.Set<T>().Find(id));

        //public void Save()
        //    => _context.SaveChanges();

        //public void Update(T entity)
        //    => _context.Set<T>().Attach(entity);
        //public void ClearTable<T>() where T : class=>
        //    _context.Set<T>().RemoveRange(_context.Set<T>());

        //        public void ClearTable<T>(DbSet<T> dbsourses) where T : class =>
        //            dbsourses.RemoveRange(dbsourses);

    }
}
