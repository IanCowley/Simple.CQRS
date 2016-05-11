using System;
using System.Collections.Generic;
using System.Linq;

using Simple.CQRS.Exceptions;

namespace Simple.CQRS.Domain
{
    public abstract class Entity 
    {
        public Guid Id { get; set; }

        protected TEntity GetEntity<TEntity>(Guid id, ICollection<TEntity> entities) where TEntity : IEntity
        {
            var entity = entities.FirstOrDefault(x => x.Id == id);

            if (entity == null)
            {
                throw new EntityNotFoundException(id, typeof(TEntity));
            }

            return entity;
        }
    }
}
