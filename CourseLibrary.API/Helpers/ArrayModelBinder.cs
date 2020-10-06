using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace CourseLibrary.API.Helpers {
    public class ArrayModelBinder : IModelBinder {
        public Task BindModelAsync(ModelBindingContext bindingContext) {

            //our binder only work enumarable types
            if (!bindingContext.ModelMetadata.IsEnumerableType) {
                bindingContext.Result = ModelBindingResult.Failed();
                return Task.CompletedTask;
            }

            //get the inputted value through the value provider
            var value = bindingContext.ValueProvider.GetValue(
                bindingContext.ModelName).ToString();

            //if null or whitespace return null
            if (string.IsNullOrEmpty(value)) {
                bindingContext.Result = ModelBindingResult.Success(null);
                return Task.CompletedTask;
            }

            //the value is not null or whitespace 
            //and the type of the model is enumarable
            //get the enumarable's type , and a converter
            var elementType = bindingContext.ModelType.GetTypeInfo().GenericTypeArguments[0];
            var converter = TypeDescriptor.GetConverter(elementType);

            //Convert each item in the value list to the enumerable type
            var values = value.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).
                Select(x => converter.ConvertFromString(x.Trim())).
                ToArray();

            //create an array of that type,and set it as the model value
            var typedValues = Array.CreateInstance(elementType, values.Length);
            values.CopyTo(typedValues, 0);
            bindingContext.Model = typedValues;

            //return a successful result, passing in the Model
            bindingContext.Result = ModelBindingResult.Success(bindingContext.Model);
            return Task.CompletedTask;



        }
    }
}
