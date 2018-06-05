using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;

namespace Holidays.Common
{
    public class LogSimpleHelper : ISimpleLog
    {
        private string GetExceptionLogString(Exception ex, string prefix)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(prefix + "Message:" + ex.Message);
            if (ex is AException)
            {
                AException exception = ex as AException;
                builder.AppendLine(string.Format(prefix + "ErrorCode:{0:X2}", exception.ErrorCode));
                builder.AppendLine("EnvironmentParameters:");
                foreach (string str in exception.EnvironmentParameters.Keys)
                {
                    string input = exception.EnvironmentParameters[str];
                    string[] strArray = Regex.Split(input, "\r\n|\r|\n");
                    builder.AppendLine(prefix + "\t" + str + ":" + strArray[0]);
                    for (int i = 1; i < strArray.Length; i++)
                    {
                        builder.AppendLine(prefix + "\t\t" + strArray[i]);
                    }
                }
            }
            builder.AppendLine(prefix + "StackTace Of Exception:" + ex.StackTrace);
            builder.AppendLine(string.Format(prefix + "Occur error at {0:yyyy-MM-dd HH:mm:ss}", DateTime.Now));
            return builder.ToString();
        }

        private string GetLogDirectory(string logTypeName)
        {
            string relativeDirectory = string.Empty;
            if (!string.IsNullOrEmpty(this.RelativeDirectory))
            {
                relativeDirectory = this.RelativeDirectory;
            }
            else
            {
                relativeDirectory = AppDomain.CurrentDomain.BaseDirectory;
            }
            relativeDirectory = Path.Combine(relativeDirectory, @"Log\" + logTypeName + @"\" + string.Format(@"{0:yyyy\\\\MM}", DateTime.Now));
            if (!Directory.Exists(relativeDirectory))
            {
                Directory.CreateDirectory(relativeDirectory);
            }
            return relativeDirectory;
        }

        /// <summary>
        /// 日志
        /// </summary>
        /// <param name="ex"></param>
        public void Log(Exception ex)
        {
            if (ex != null)
            {
                string path = Path.Combine(this.GetLogDirectory("Error"), string.Format("{0:yyyyMMddHH}", DateTime.Now) + ".error");
                StringBuilder builder = new StringBuilder();
                builder.AppendLine("".PadLeft(30, '*') + ex.GetType() + "".PadLeft(30, '*'));
                builder.AppendLine(this.GetExceptionLogString(ex, ""));
                builder.AppendLine();
                ex = ex.InnerException;
                while (ex != null)
                {
                    builder.AppendLine(string.Empty.PadLeft(0x4b, '-'));
                    builder.AppendLine(this.GetExceptionLogString(ex, "\t"));
                    ex = ex.InnerException;
                }
                builder.AppendLine();
                builder.AppendLine(string.Empty.PadLeft(0x4b, '#'));
                builder.AppendLine();
                builder.AppendLine();
                using (FileStream stream = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.Read))
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(builder.ToString());
                    }
                }
            }
        }

        /// <summary>
        /// 跟踪信息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="detail"></param>
        public void Trace(string msg, string detail)
        {
            string path = Path.Combine(this.GetLogDirectory("Trace"), string.Format("{0:yyyyMMddHH}", DateTime.Now) + ".trace");
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("".PadLeft(0x4b, '*'));
            builder.AppendLine("Trace:" + msg);
            builder.AppendLine("Trace detail:" + detail);
            builder.AppendLine(string.Format("Occur error at {0:yyyy-MM-dd HH:mm:ss}", DateTime.Now));
            builder.AppendLine();
            builder.AppendLine(string.Empty.PadLeft(0x4b, '#'));
            builder.AppendLine();
            builder.AppendLine();
            using (FileStream stream = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.Read))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(builder.ToString());
                }
            }
        }

        /// <summary>
        /// 自定义输出信息
        /// </summary>
        /// <param name="msg"></param>
        public void Info(string msg)
        {
            string path = Path.Combine(this.GetLogDirectory("Info"), string.Format("{0:yyyyMMddHH}", DateTime.Now) + ".log");
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("".PadLeft(0x4b, '*'));
            builder.AppendLine("输出信息:" + msg);
            builder.AppendLine(string.Format("Occur error at {0:yyyy-MM-dd HH:mm:ss}", DateTime.Now));
            builder.AppendLine();
            builder.AppendLine(string.Empty.PadLeft(0x4b, '#'));
            builder.AppendLine();
            builder.AppendLine();
            using (FileStream stream = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.Read))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(builder.ToString());
                }
            }
        }

        // Properties
        public string RelativeDirectory { get; set; }
    }

    public class AException : Exception
    {
        protected AException(SerializationInfo info, StreamingContext contenxt)
            : base(info, contenxt)
        {
            this.ErrorCode = info.GetUInt32("ErrorCode");
            this.EnvironmentParameters = info.GetValue("EnvironmentParameters", typeof(Dictionary<string, string>)) as Dictionary<string, string>;
        }

        public AException(uint errorCode, string errorMsg) : this(errorCode, errorMsg, null) { }

        public AException(uint errorCode, string errorMsg, Exception ex)
            : base(errorMsg, ex)
        {
            this.ErrorCode = errorCode;
            this.EnvironmentParameters = new Dictionary<string, string>();
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("ErrorCode", this.ErrorCode);
            info.AddValue("EnvironmentParameters", this.EnvironmentParameters);
        }

        public Dictionary<string, string> EnvironmentParameters { get; private set; }

        public uint ErrorCode { get; set; }
    }

    public interface ISimpleLog
    {
        void Log(Exception ex);

        void Trace(string msg, string detail);

        void Info(string msg);

        string RelativeDirectory { get; set; }
    }
}
