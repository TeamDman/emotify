using System.Text;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Emotify.TagHelpers
{
    [HtmlTargetElement("guh")]
    public class Guh : TagHelper
    {
        public string Name {get; set;}
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "CustumTagHelper";
            output.TagMode = TagMode.StartTagAndEndTag;
            var sb = new StringBuilder();
            sb.AppendFormat("<span>Hi {0}</span>", this.Name);
            output.PreContent.SetHtmlContent(sb.ToString());
        }
    }
}