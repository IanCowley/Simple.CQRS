using System;
using System.Linq.Expressions;

using Simple.CQRS.Exceptions;
using Simple.CQRS.Extensions;

namespace Simple.CQRS.Query
{
    public interface ISortBy
    {
        SortDirection SortDirection { get; set; }

        string TargetProperty { get; set; }
    }

    public class SortBy<TTarget> : ISortBy
        where TTarget : class
    {
        public SortBy(string targetProperty, SortDirection sortDirection = SortDirection.Ascending)
        {
            this.SortDirection = sortDirection;
            this.TargetProperty = targetProperty;
            AssertPropertyExists(targetProperty);
        }

        public SortBy(Expression<Func<TTarget, object>> expression, SortDirection sortDirection = Query.SortDirection.Ascending)
        {
            this.SortDirection = sortDirection;

            this.TargetProperty = this.GetTargetPropertyName(expression);
        }

        public SortDirection SortDirection { get; set; }

        public string TargetProperty { get; set; }

        public override string ToString()
        {
            return "{0} {1}".FormatString(TargetProperty, this.SortDirection);
        }

        private void AssertPropertyExists(string targetProperty)
        {
            var property = typeof(TTarget).GetProperty(targetProperty);
            if (property == null)
            {
                throw new SortPropertyDoesNotExistException(typeof(TTarget), targetProperty);
            }
        }

        private string GetTargetPropertyName(Expression<Func<TTarget, object>> expression)
        {

            MemberExpression memberExpression;

            if (expression.Body is UnaryExpression)
            {
                var unaryExpression = expression.Body as UnaryExpression;
                memberExpression = unaryExpression.Operand as MemberExpression;
            }
            else
            {
                memberExpression = expression.Body as MemberExpression;
            }

            if (memberExpression == null)
            {
                throw new InvalidOperationException("Body was not a member expression.");
            }

            return memberExpression.Member.Name;
        }
    }
}
