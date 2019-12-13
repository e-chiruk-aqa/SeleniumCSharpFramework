namespace Framework
{
    public class BaseSteps
    {
        public log4net.ILog Logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    }
}