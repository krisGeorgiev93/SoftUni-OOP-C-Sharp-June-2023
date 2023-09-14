using EasterRaces.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasterRaces.Repositories.Entities
{
    public class Repository<T> : IRepository<T>
    {
        private ICollection<T> repository;
        public Repository() 
        {
            repository = new List<T>();
        }
        public void Add(T model)
        {
            repository.Add(model);
        }

        public IReadOnlyCollection<T> GetAll() => this.repository.ToList().AsReadOnly();
        

        public T GetByName(string name)
        {
            return repository.FirstOrDefault(x => x.GetType().Name == name);
        }

        public bool Remove(T model)
        {
           return repository.Remove(model);
        }
    }
}
