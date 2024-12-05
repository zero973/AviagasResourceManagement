using ARM.Core.Models.UI;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ARM.WebApi.Infrastructure.ModelBinders.Providers;

public class BaseListParamsModelBinderProvider : IModelBinderProvider
{

    private readonly IModelBinder _binder = new BaseListParamsModelBinder();

    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        return context.Metadata.ModelType == typeof(BaseListParams) ? _binder : null;
    }
    
}