using EnvDTE;

namespace DialControllerTools
{
    public interface IContextAwareController
    {
        int GetContextRelevance(Window activeWindow);
    }
}
