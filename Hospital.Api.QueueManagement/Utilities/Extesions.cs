using Infrastructure.Utitlities;
using LinqKit;
//using LinqKit;
using Newtonsoft.Json;
using RestSharp;
using System.Linq.Expressions;
using System.Reflection;
using static Hospital.Shared.Utitlities.PublicEnumes;


namespace Hospital.Api.QueueManagement.Utilities
{
    /// <summary>
    /// Extesions
    /// </summary>
    public static class Extesions
    {
        /// <summary>
        /// ToPredicate
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="TypeOfClass"></param>
        /// <returns></returns>
        public static Expression ToPredicate<T>(this Dictionary<string, string> filter, Type TypeOfClass)
        {
            Dictionary<string, OperatorComparer> operatorComparer = new();
            //Expression<Func<T, bool>> predicate = null;
            var quoteDatePredicate = PredicateBuilder.New<T>();
            bool hadPreedicate = false;
            foreach (var prop in TypeOfClass.GetProperties())
            {
                var attrs = prop.GetCustomAttributes();
                var operatorComparerAttr = attrs.FirstOrDefault(p => p.GetType() == typeof(global::Hospital.Shared.Utitlities.Attributes.OperatorComparer));
                if (operatorComparerAttr != null)
                {
                    operatorComparer.Add(prop.Name,
                    ((global::Hospital.Shared.Utitlities.Attributes.OperatorComparer)((Attribute[])attrs)[0])._operatorName);
                }
                else
                {
                }

            }


            foreach (var pred in operatorComparer)
            {
                foreach (var req in filter)
                {
                    if (pred.Key.ToLower() == req.Key.ToLower())
                    {
                        hadPreedicate = true;
                        quoteDatePredicate.And(ExpressionBuilder.BuildPredicate<T>(req.Value, pred.Value, pred.Key));
                        //if (predicate == null)
                        //{
                        //    predicate = ExpressionBuilder.BuildPredicate<T>(req.Value, pred.Value, pred.Key);
                        //}
                        //else
                        //{
                        //    //predicate.And(ExpressionBuilder.BuildPredicate<T>(req.Value, pred.Value, pred.Key));
                        //    predicate.And(ExpressionBuilder.BuildPredicate<T>(req.Value, pred.Value, pred.Key));

                        //}
                    }
                }
            }
            //return predicate;
            if (hadPreedicate)
                return quoteDatePredicate;

            return null;

        }

        
    }
}


