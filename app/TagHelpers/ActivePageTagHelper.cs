using System;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Routing;

namespace Emotify.TagHelpers
{
    [HtmlTargetElement("li", Attributes = _for)]
    [HtmlTargetElement("a", Attributes = _for)]
    public class ActiveItemTagHelper : TagHelper
    {
        private readonly IUrlHelper _urlHelper;
        private readonly IHttpContextAccessor _httpAccess;
        private readonly LinkGenerator _linkGenerator;
        private const string _for = "navigation-active-for";

        public ActiveItemTagHelper(
            IActionContextAccessor actionAccess,
            IUrlHelperFactory factory,
            IHttpContextAccessor httpAccess,
            LinkGenerator generator
        )
        {
            _urlHelper = factory.GetUrlHelper(actionAccess.ActionContext);
            _httpAccess = httpAccess;
            _linkGenerator = generator;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            // grab attribute value
            var targetPage = output.Attributes[_for].Value.ToString();
            // remove from html so user doesn't see it
            output.Attributes.Remove(output.Attributes[_for]);
            
            // get the URI that corresponds to the attribute value
            var targetUri = _linkGenerator.GetUriByPage(_httpAccess.HttpContext, page: targetPage);
            // get the URI that corresponds to the current page's action
            var currentUri = _urlHelper.ActionLink();

            // if they match, then add the "active" CSS class
            if (targetUri == currentUri) {
                output.AddClass("active", HtmlEncoder.Default);
            }
        }
    }
}