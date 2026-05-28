using GH.AppContext;
using GH.UserExceptions;
using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Filter;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using System;
using System.IO;

namespace GH.Utils
{
    public static class Logger
    {
        private static readonly bool _isConfigurated = Setup();

        private static ILog _log = null;
        private static ILog Log
        {
            get
            {
                if (_log == null)
                {
                    _log = LogManager.GetLogger(RunContext.ProcessName);
                }
                return _log;
            }
        }

        enum RollerType
        {
            Info,
            Error,
        }

        public static bool Setup()
        {
            if (!_isConfigurated)
            {

                Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository();
                //ConfigRoller(RollerType.Info, hierarchy);
                ConfigRoller(RollerType.Error, hierarchy);


                hierarchy.Root.Level = Level.Info;
                hierarchy.Configured = true;
                return true;
            }
            else
                return _isConfigurated;
        }

        private static void ConfigRoller(RollerType rollerType, Hierarchy hierarchy)
        {
            PatternLayout patternLayout = new PatternLayout();
            switch (rollerType)
            {
                case RollerType.Info:
                    patternLayout.ConversionPattern = "%date{yyyy-dd-MM HH:mm:ss}  %-5p %logger - %m%n";
                    break;
                case RollerType.Error:
                    patternLayout.ConversionPattern = "%date{yyyy-dd-MM HH:mm:ss} [%thread] %-5level %logger - %message%newline";
                    break;
                default:
                    break;
            }
            patternLayout.ActivateOptions();

            RollingFileAppender roller = new RollingFileAppender();

            LevelRangeFilter filter = new LevelRangeFilter();
            switch (rollerType)
            {
                case RollerType.Info:
                    filter.LevelMin = Level.Verbose;
                    filter.LevelMax = Level.Info;
                    break;
                case RollerType.Error:
                    filter.LevelMin = Level.Error;
                    filter.LevelMax = Level.Fatal;
                    break;
                default:
                    break;
            }
            filter.ActivateOptions();

            roller.AddFilter(filter);
            roller.AppendToFile = true;

            switch (rollerType)
            {
                case RollerType.Info:
                    roller.File = Path.Combine("Logs", "Events.log");
                    break;
                case RollerType.Error:
                    roller.File = Path.Combine("Logs", "Errors.log");
                    break;
                default:
                    break;
            }
            roller.Layout = patternLayout;
            roller.MaxSizeRollBackups = 5;
            roller.MaximumFileSize = "5MB";
            roller.RollingStyle = RollingFileAppender.RollingMode.Size;
            roller.StaticLogFileName = true;
            roller.ActivateOptions();

            hierarchy.Root.AddAppender(roller);
        }

        public static void Error(object message)
        {
            if (Skip(message))
                return;
            Log.Error(message);
        }

        private static bool Skip(object message)
        {
            return message is Exception ex && ex.InnerException is UserWantExit;
        }

        public static void Error(object message, Exception exception)
        {
            if (Skip(message))
                return;
            Log.Error(message, exception);
        }

        public static void ErrorFormatted(string format, params object[] args)
        {
            Log.ErrorFormat(format, args);
        }

        public static void Fatal(object message)
        {
            if (Skip(message))
                return;
            Log.Fatal(message);
        }

        public static void Fatal(object message, Exception exception)
        {
            if (Skip(message))
                return;
            Log.Fatal(message, exception);
        }

        public static void FatalFormatted(string format, params object[] args)
        {
            Log.FatalFormat(format, args);
        }
    }

    /*
    private static readonly object _locker = new object();
    private static RollingFileAppender _errorAppender;
    public static RollingFileAppender ErrorAppender
    {
        get
        {
            if (_errorAppender == null)
            {
                _errorAppender = _log.Logger.Repository.GetAppenders().Where(a => a.Name.StartsWith("Error")).FirstOrDefault() as RollingFileAppender;

                //if (_errorAppender == null)
                //{
                //    _errorAppender = new RollingFileAppender();
                //    _errorAppender.Name = "ErrorAppender";
                //    _errorAppender.File = "Logs\\Errors.log";
                //    _errorAppender.AppendToFile = true;
                //    _errorAppender.RollingStyle = RollingFileAppender.RollingMode.Size;
                //    _errorAppender.MaxSizeRollBackups = 10;
                //    _errorAppender.MaximumFileSize = "1MB";
                //    _errorAppender.StaticLogFileName = true;

                //    _errorAppender.LockingModel.CurrentAppender = new FileAppender(); "log4net.Appender.FileAppender+MinimalLock";
                //    _errorAppender.LockingModel.CurrentAppender.LockingModel = new FileAppender(); "log4net.Appender.FileAppender+MinimalLock";
                //      < lockingModel type="log4net.Appender.FileAppender+MinimalLock" />

                //    //_errorAppender.Layout.Header.Insert(0, "[Application Starts]&#13;&#10;");
                //    //_errorAppender.Layout.Footer.Insert(0, "[Application Stops]&#13;&#10;");

                //    //<layout type="log4net.Layout.PatternLayout">
                //    //    <param name="ConversionPattern" value="%date{yyyy-dd-MM HH:mm:ss} [%thread] %-5level %logger{3} - %message%newline" />
                //    //  </layout>
                //}
            }


            return _errorAppender;
        }
    }

    private static AdoNetAppenderParameter CreateParam(string name, object value)
    {
        AdoNetAppenderParameter parameter = new AdoNetAppenderParameter();
        parameter.ParameterName = name;
        return parameter;
    }

    public static IList Configs { get; set; }
    private static bool InitLogger()
    {
        if (!_isConfigurated)
        {
            lock (_locker)
            {
                if (!_isConfigurated)
                {
                    if (File.Exists("Log4Net.config"))
                        Configs = (IList)XmlConfigurator.ConfigureAndWatch(new FileInfo("Log4Net.config"));
                    else
                        Configs = (IList)XmlConfigurator.ConfigureAndWatch(new FileInfo(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile));

                }
            }
            return true;
        }
        return _isConfigurated;
    }
    
     TODO private static void ConfigureRunTimeFromBasic()
    {
        TraceAppender traceAppender = new TraceAppender();
        PatternLayout patternLayout = new PatternLayout
        {
            ConversionPattern = "%date{yyyy-MM-dd HH:mm:ss} [%thread] %-5level %logger{2} - %message%newline"
        };
        patternLayout.ActivateOptions();

        traceAppender.Layout = patternLayout;

        BasicConfigurator.Configure(traceAppender);
    }

    private static void ConfigureFromFile()
    {
        string fileConfiguration = ConfigurationManager.AppSettings.Get("Log4NetConfigurationFile");
        if (string.IsNullOrWhiteSpace(fileConfiguration))
            fileConfiguration = "log4net.config";
        XmlConfigurator.ConfigureAndWatch(new FileInfo(AppDomain.CurrentDomain.BaseDirectory + fileConfiguration));
    }
    private static bool InitLogger()
    {
        if (File.Exists("Log4Net.config"))
            XmlConfigurator.ConfigureAndWatch(new FileInfo("Log4Net.config"));
        else
            XmlConfigurator.ConfigureAndWatch(new FileInfo(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile));
        return true;
    }

    public static bool IsInitialized
    {
        get
        {
            return Logger._isInitialized;
        }
    }

    public static void Debug(object message)
    {
        Log.Debug(message);
    }

    public static void DebugFormatted(string format, params object[] args)
    {
        Log.DebugFormat(format, args);
    }

    public static void Info(object message)
    {
        Log.Info(message);
    }

    public static void InfoFormatted(string format, params object[] args)
    {
        Log.InfoFormat(format, args);
    }

    public static void Warn(object message)
    {
        Log.Warn(message);
    }

    public static void Warn(object message, Exception exception)
    {
        Log.Warn(message, exception);
    }

    public static void WarnFormatted(string format, params object[] args)
    {
        Log.WarnFormat(format, args);
    }

    public static bool IsDebugEnabled
    {
        get
        {
            return Log.IsDebugEnabled;
        }
    }

    public static bool IsInfoEnabled
    {
        get
        {
            return Log.IsInfoEnabled;
        }
    }

    public static bool IsWarnEnabled
    {
        get
        {
            return Log.IsWarnEnabled;
        }
    }

    public static bool IsErrorEnabled
    {
        get
        {
            return Log.IsErrorEnabled;
        }
    }

    public static bool IsFatalEnabled
    {
        get
        {
            return Log.IsFatalEnabled;
        }
    }

    }
    */
}
