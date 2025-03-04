﻿using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Reflection;
using MudBlazor.Extensions.Api;

namespace MainSample.WebAssembly.Shared;

public partial class Api
{
    private bool _loaded;
    private string _searchString;
    private MudExpansionPanel _methodPanel;
    private MudExpansionPanel _propertyPanel;

    [Parameter]
    public Type Type { get; set; }

    [Parameter] public bool ShowHeader { get; set; } = true;

    [Parameter] public bool ShowInheritance { get; set; } = true;
    [Parameter] public bool ShowTools { get; set; } = true;
    [Parameter] public bool ShowMethods { get; set; } = true;
    [Parameter] public bool ShowProperties { get; set; } = true;
    [Parameter] public bool ShowInherited { get; set; }
    [Parameter] public bool Compact { get; set; }
    [Parameter] public string[]? ShowOnly { get; set; }


    [Parameter]
    public bool IsInitiallyExpanded { get; set; } = true;

    [Parameter]
    public string Search { get; set; }

    private HashSet<ApiMemberInfo<PropertyInfo>>? Properties;
    private HashSet<ApiMemberInfo<MethodInfo>>? Methods;
    private Lazy<List<BreadcrumbItem>> _inheritance => new(GetInheritancePath(Type));

    private List<BreadcrumbItem> GetInheritancePath(Type type)
    {
        var breadcrumbItems = new List<BreadcrumbItem>();
        while (type != null)
        {
            breadcrumbItems.Insert(0, new BreadcrumbItem(ApiMemberInfo.GetGenericFriendlyTypeName(type), href: $"/a/{type.Name}", Type == type));
            type = type.BaseType;
        }
        breadcrumbItems.Reverse();
        return breadcrumbItems;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender && IsInitiallyExpanded)
        {
            await LoadInfos();
        }

        if (firstRender && !string.IsNullOrEmpty(Search))
        {
            _searchString = Search;
            StateHasChanged();
        }

        await base.OnAfterRenderAsync(firstRender);    
    }


    private async Task OnExpandedChanged(bool arg)
    {
        if (arg && (Properties == null || Methods == null))
        {
            await LoadInfos();
        }
    }

    private async Task LoadInfos()
    {
        _loaded = false;
        Properties ??= await ApiMemberInfo<PropertyInfo>.AllPropertiesOf(Type);
        Methods ??= await ApiMemberInfo<MethodInfo>.AllMethodsOf(Type); 
        _loaded = true;
        await InvokeAsync(StateHasChanged);
    }
}