﻿using MyShop.Core.Contracts;
using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DataAccess.InMemory
{
    public class InMemoryRepository<T> : IRepository<T> where T : BaseEntity
    {
        ObjectCache cache = MemoryCache.Default;
        List<T> items;
        string className;

        public InMemoryRepository()
        {
            className = typeof(T).Name;
            items = cache[className] as List<T>;
            if (items == null)
                items = new List<T>();
        }

        public void Commit()
        {
            cache[className] = items;
        }

        public void Insert(T t)
        {
            items.Add(t);
        }

        public void Update(T t)
        {
            T tToUpdate = items.Find(i => i.Id == t.Id);
            if (tToUpdate == null)
                throw new Exception(className + " Not Found");
            tToUpdate = t;
        }

        public T Find(string Id)
        {
            T t = items.Find(i => i.Id == Id);
            if (t == null)
                throw new Exception(className + " Not Found");
            return t;
        }

        public IQueryable<T> Collection() {
            return items.AsQueryable<T>();
        }

        public void Delete(string Id)
        {
            T tToDelete = items.Find(i => i.Id == Id);
            if (tToDelete == null)
                throw new Exception(className + " Not Found");
            items.Remove(tToDelete);
        }

    }
}
