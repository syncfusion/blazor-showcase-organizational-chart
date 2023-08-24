using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace OrganizationalChart.Shared
{
    public class SampleBaseComponent : ComponentBase
    {
        [Inject]
        protected IJSRuntime? jsRuntime { get; set; }

        [Inject]
        protected SampleService? Service { get; set; }

        // internal SampleData SampleDetails { get; set; } = new SampleData();
        protected async override void OnAfterRender(bool FirstRender)
        {
            await Task.Delay(3000).ConfigureAwait(true);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            Service.Update(new NotifyProperties() { HideSpinner = true, RestricMouseEvents = true });
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }
    }
}
