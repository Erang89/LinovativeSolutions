using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace LinoVative.Service.Backend.Extensions
{
    public static class ModelStateDictionaryExtension
    {
        public static Dictionary<string, object> GetErrorMessages(this ModelStateDictionary modelState)
        {
            var errors = new Dictionary<string, object>();

            foreach (var key in modelState.Keys)
            {
                var value = modelState[key];
                if (value.Errors.Any())
                {
                    errors[key] = value.Errors.Select(e => e.ErrorMessage).ToList();
                }
            }

            return errors;
        }
    }
}
