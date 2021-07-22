using System;
using System.Linq;
using System.Reflection;
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

        public override void Process(
            TagHelperContext context,
            TagHelperOutput output
        )
        {
            // grab attribute value
            var targetPages = output.Attributes[_for].Value.ToString();
            // remove from html so user doesn't see it
            output.Attributes.Remove(output.Attributes[_for]);

            // get the URI that corresponds to the current page's action
            var currentUri = _urlHelper.ActionLink();

            // check all targets against current URI
            var active = targetPages.Split(";")
                .Select(target => _linkGenerator.GetUriByPage(_httpAccess.HttpContext, target))
                .Any(targetUri => targetUri == currentUri);
            
            // if there's a match, then add the "active" CSS class
            if (active)
            {
                output.AddClass("active", HtmlEncoder.Default);
            }
        }
    }
}