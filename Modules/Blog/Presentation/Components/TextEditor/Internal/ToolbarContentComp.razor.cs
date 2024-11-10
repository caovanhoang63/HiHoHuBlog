using Microsoft.AspNetCore.Components;

namespace HiHoHuBlog.Modules.Blog.Presentation.Components.TextEditor.Internal
{
    public partial class ToolbarContentComp
    {
        [CascadingParameter] public Toolbar Toolbar { get; set; }
        [CascadingParameter] public List<string> Fonts { get; set; }
    }
}