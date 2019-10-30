using System.Diagnostics;
using System.Linq;
using ChartATask.Core.Requests;

namespace ChartATask.Interactors.Windows.Requests
{
    public class WindowsAppRunningRequest : IRequestEvaluator<AppRunningRequest, bool>
    {
        public bool Evaluate(AppRunningRequest request)
        {
            try
            {
                return Process.GetProcessesByName(request.Name).Any();
            }
            catch
            {
                // ignored
            }

            return false;
        }

        public void Dispose()
        {
        }

        public void Start()
        {
        }

        public void Stop()
        {
        }
    }
}