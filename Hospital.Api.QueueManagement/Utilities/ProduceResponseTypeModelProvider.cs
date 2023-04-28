using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Inventory.Api.Utilities
{
    /// <summary>
    /// ProduceResponseTypeModelProvider
    /// </summary>
    public class ProduceResponseTypeModelProvider : IApplicationModelProvider
    {
        /// <summary>
        /// Order
        /// </summary>
        public int Order => 3;
        /// <summary>
        /// OnProvidersExecuted
        /// </summary>
        /// <param name="context"></param>
        public void OnProvidersExecuted(ApplicationModelProviderContext context)
        {
        }
        /// <summary>
        /// OnProvidersExecuting
        /// </summary>
        /// <param name="context"></param>
        public void OnProvidersExecuting(ApplicationModelProviderContext context)
        {
            foreach (ControllerModel controller in context.Result.Controllers)
            {
                foreach (ActionModel action in controller.Actions)
                {
                    Type returnType = null;
                    if (action.ActionMethod.ReturnType.GenericTypeArguments.Any())
                    {
                        if (action.ActionMethod.ReturnType.GenericTypeArguments.FirstOrDefault().GetGenericArguments().Any())
                        {
                            returnType = action.ActionMethod.ReturnType.GenericTypeArguments.FirstOrDefault().GetGenericArguments()[0];
                        }
                    }

                    var methodVerbs = action.Attributes.OfType<HttpMethodAttribute>().SelectMany(x => x.HttpMethods).Distinct();
                    bool actionParametersExist = action.Parameters.Any();

                    AddUniversalStatusCodes(action, returnType);

                    if (actionParametersExist == true)
                    {
                        AddProducesResponseTypeAttribute(action, null, 404);
                    }
                    if (methodVerbs.Contains("POST"))
                    {
                        AddPostStatusCodes(action, returnType, actionParametersExist);
                    }
                }
            }
        }
        /// <summary>
        /// AddProducesResponseTypeAttribute
        /// </summary>
        /// <param name="action"></param>
        /// <param name="returnType"></param>
        /// <param name="statusCodeResult"></param>
        public void AddProducesResponseTypeAttribute(ActionModel action, Type returnType, int statusCodeResult)
        {
            if (returnType != null)
            {
                action.Filters.Add(new ProducesResponseTypeAttribute(returnType, statusCodeResult));
            }
            else if (returnType == null)
            {
                action.Filters.Add(new ProducesResponseTypeAttribute(statusCodeResult));
            }
        }
        /// <summary>
        /// AddUniversalStatusCodes
        /// </summary>
        /// <param name="action"></param>
        /// <param name="returnType"></param>
        public void AddUniversalStatusCodes(ActionModel action, Type returnType)
        {
            AddProducesResponseTypeAttribute(action, returnType, 200);
            AddProducesResponseTypeAttribute(action, null, 500);
        }
        /// <summary>
        /// AddPostStatusCodes
        /// </summary>
        /// <param name="action"></param>
        /// <param name="returnType"></param>
        /// <param name="actionParametersExist"></param>
        public void AddPostStatusCodes(ActionModel action, Type returnType, bool actionParametersExist)
        {
            AddProducesResponseTypeAttribute(action, returnType, 201);
            AddProducesResponseTypeAttribute(action, returnType, 400);
            if (actionParametersExist == false)
            {
                AddProducesResponseTypeAttribute(action, null, 404);
            }
        }
    }

}
