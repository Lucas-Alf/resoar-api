using System.Linq.Expressions;
using Domain.Enums;

namespace Domain.Utils
{
    public class FilterBy<TEntity>
    {
        public List<Expression<Func<TEntity, bool>>> AndExpressions { get; private set; } = new List<Expression<Func<TEntity, bool>>>();
        public List<Expression<Func<TEntity, bool>>> OrExpressions { get; private set; } = new List<Expression<Func<TEntity, bool>>>();

        public FilterBy()
        {

        }

        public FilterBy(Expression<Func<TEntity, bool>> filter)
        {
            AndExpressions.Add(filter);
        }

        public void Add(Expression<Func<TEntity, bool>> filter)
        {
            AndExpressions.Add(filter);
        }

        public void AddOr(Expression<Func<TEntity, bool>> filter)
        {
            OrExpressions.Add(filter);
        }

        public Expression<Func<TEntity, bool>>? Compile()
        {
            var andExpressions = AndExpressions.Count() > 0
                ? AndExpressions.Aggregate((current, next) => Combine<TEntity>(current, next, CombineTypeEnum.And))
                : null;

            var orExpressions = OrExpressions.Count() > 0
                ? OrExpressions.Aggregate((current, next) => Combine<TEntity>(current, next, CombineTypeEnum.Or))
                : null;

            if (andExpressions != null && orExpressions != null)
                return Combine<TEntity>(andExpressions, orExpressions, CombineTypeEnum.Or);

            if (andExpressions != null)
                return andExpressions;

            if (orExpressions != null)
                return orExpressions;

            return null;
        }

        private Expression<Func<T, bool>> Combine<T>(Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2, CombineTypeEnum type)
        {
            var parameter = Expression.Parameter(typeof(T));

            var leftVisitor = new ReplaceExpressionVisitor(expr1.Parameters[0], parameter);
            var left = leftVisitor.Visit(expr1.Body);

            var rightVisitor = new ReplaceExpressionVisitor(expr2.Parameters[0], parameter);
            var right = rightVisitor.Visit(expr2.Body);

            if (type == CombineTypeEnum.And)
                return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(left!, right!), parameter);
            else
                return Expression.Lambda<Func<T, bool>>(Expression.OrElse(left!, right!), parameter);
        }

        private class ReplaceExpressionVisitor : ExpressionVisitor
        {
            private readonly Expression _oldValue;
            private readonly Expression _newValue;

            public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
            {
                _oldValue = oldValue;
                _newValue = newValue;
            }

            public override Expression? Visit(Expression? node)
            {
                if (node == _oldValue)
                    return _newValue;

                return base.Visit(node);
            }
        }
    }
}