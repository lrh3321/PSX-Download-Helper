using System;

namespace PSXDH.HttpsHelp
{
    public abstract class DownloadHelperException : Exception
    {

    }

    public class NoCaptureRuleException : DownloadHelperException
    {
    }

    public class IPAddressNotBindException : DownloadHelperException
    {
    }

    public delegate void ExceptionHandler(Exception ex);
}
