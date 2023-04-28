using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Shared.Utitlities
{
    public class PublicEnumes
    {
        public enum OperatorComparer
        {
            /// <summary>
            /// contain element
            /// </summary>
            Contains,
            /// <summary>
            /// start with
            /// </summary>
            StartsWith,
            /// <summary>
            /// ends With
            /// </summary>
            EndsWith,
            /// <summary>
            /// Equals
            /// </summary>
            Equals = ExpressionType.Equal,
            /// <summary>
            /// GreaterThan
            /// </summary>
            GreaterThan = ExpressionType.GreaterThan,
            /// <summary>
            /// GreaterThanOrEqual
            /// </summary>
            GreaterThanOrEqual = ExpressionType.GreaterThanOrEqual,
            /// <summary>
            /// LessThan
            /// </summary>
            LessThan = ExpressionType.LessThan,
            /// <summary>
            /// LessThanOrEqual
            /// </summary>
            LessThanOrEqual = ExpressionType.LessThan,
            /// <summary>
            /// NotEqual
            /// </summary>
            NotEqual = ExpressionType.NotEqual
        }
    }
}

