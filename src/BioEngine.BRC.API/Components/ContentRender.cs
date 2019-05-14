﻿using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BioEngine.Core.Entities;
using BioEngine.Core.Web;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace BioEngine.BRC.Api.Components
{
    public class ContentRender : IContentRender
    {
        private readonly IViewRenderService _renderService;

        public ContentRender(IViewRenderService renderService)
        {
            _renderService = renderService;
        }

        public async Task<string> RenderHtmlAsync(IContentEntity contentEntity,
            ContentEntityViewMode mode = ContentEntityViewMode.List)
        {
            return await _renderService.RenderToStringAsync("Content/Blocks",
                new ContentRendererModel(contentEntity, mode));
        }
    }

    public class ContentRendererModel
    {
        public ContentRendererModel(IContentEntity entity, ContentEntityViewMode mode)
        {
            Entity = entity;
            Mode = mode;
        }

        public IContentEntity Entity { get; }
        public ContentEntityViewMode Mode { get; }
    }

    public interface IViewRenderService
    {
        Task<string> RenderToStringAsync<TModel>(string viewName, TModel model);
    }

    [UsedImplicitly]
    public class ViewRenderService : IViewRenderService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IRazorViewEngine _viewEngine;
        private readonly ITempDataProvider _tempDataProvider;

        public ViewRenderService(IServiceScopeFactory scopeFactory, IRazorViewEngine razorViewEngine,
            ITempDataProvider tempDataProvider,
            IServiceProvider serviceProvider)
        {
            _scopeFactory = scopeFactory;
            _viewEngine = razorViewEngine;
            _tempDataProvider = tempDataProvider;
        }

        public async Task<string> RenderToStringAsync<TModel>(string viewName, TModel model)
        {
            var actionContext = GetActionContext();
            var view = FindView(actionContext, viewName);

            using (var output = new StringWriter())
            {
                var viewContext = new ViewContext(
                    actionContext,
                    view,
                    new ViewDataDictionary<TModel>(
                        new EmptyModelMetadataProvider(),
                        new ModelStateDictionary()) {Model = model},
                    new TempDataDictionary(
                        actionContext.HttpContext,
                        _tempDataProvider),
                    output,
                    new HtmlHelperOptions());

                await view.RenderAsync(viewContext);

                return output.ToString();
            }
        }

        private IView FindView(ActionContext actionContext, string viewName)
        {
            var getViewResult = _viewEngine.GetView(null, viewName, true);
            if (getViewResult.Success)
            {
                return getViewResult.View;
            }

            var findViewResult = _viewEngine.FindView(actionContext, viewName, true);
            if (findViewResult.Success)
            {
                return findViewResult.View;
            }

            var searchedLocations = getViewResult.SearchedLocations.Concat(findViewResult.SearchedLocations);
            var errorMessage = string.Join(
                Environment.NewLine,
                new[] {$"Unable to find view '{viewName}'. The following locations were searched:"}.Concat(
                    searchedLocations));

            throw new InvalidOperationException(errorMessage);
        }

        private ActionContext GetActionContext()
        {
            var httpContext = new DefaultHttpContext {RequestServices = _scopeFactory.CreateScope().ServiceProvider};
            return new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
        }
    }
}
