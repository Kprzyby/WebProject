namespace ADODB.DAL
{
    public interface ILogs
    {
        //Method for creating logs, "logType" defines the operation type which the log is created for,
        //"details" is a json including the details about the object the operation was executed on
        public void CreateLog(string logType, string details);
    }
}
