namespace Booking.API.Specifications
{
#pragma warning disable
    using System;
    using System.Linq.Expressions;


    public abstract class QuerySpecification<T>
    {

        public abstract Expression<Func<T, bool>> Criteria { get; }


        public bool IsSatisfiedBy(T entity)
        {
            Func<T, bool> predicate = Criteria.Compile();
            return predicate(entity);
        }
    }
}
