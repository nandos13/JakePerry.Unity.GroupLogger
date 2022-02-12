using System;
using UnityEngine;

namespace JakePerry.Unity
{
    public sealed class PackageLogger
    {
        private readonly string m_packageName;
        private readonly bool m_prependLogs;

        private byte m_logTypes = 255;

        public string PackageName => m_packageName;

        /// <param name="packageName">
        /// [Optional] Short package identifier prepended to log messages.
        /// </param>
        public PackageLogger(string packageName = null)
        {
            m_packageName = packageName?.Trim();
            m_prependLogs = !string.IsNullOrEmpty(m_packageName);
        }

        private static int GetByteOffset(LogType type)
        {
            const int min = (int)LogType.Error; // 0
            const int max = (int)LogType.Exception; // 4
            const int @default = (int)LogType.Log; // default for invalid type arg

            var index = (int)type;

            return (index < min || index > max)
                ? @default
                : index;
        }

        public bool IsLogTypeEnabled(LogType type)
        {
            return (m_logTypes & (1 << GetByteOffset(type))) != 0;
        }

        public void SetLogTypeEnabled(LogType type, bool enabled)
        {
            if (enabled)
                m_logTypes = (byte)(m_logTypes & (1 << GetByteOffset(type)));
            else
                m_logTypes = (byte)(m_logTypes | ~(1 << GetByteOffset(type)));
        }

        private void DoLog(LogType logType, object message, UnityEngine.Object context)
        {
            if (!IsLogTypeEnabled(logType))
                return;

            if (m_prependLogs)
                message = $"[{m_packageName}] {message}";

            if (context is null)
            {
                Debug.unityLogger?.Log(logType, message);
            }
            else
            {
                Debug.unityLogger?.Log(logType, message, context);
            }
        }

        /// <seealso cref="Debug.Log(object, UnityEngine.Object)"/>
        public void Log(object message, UnityEngine.Object context = null)
        {
            DoLog(LogType.Log, message, context);
        }

        /// <seealso cref="Debug.LogWarning(object, UnityEngine.Object)"/>
        public void LogWarning(object message, UnityEngine.Object context = null)
        {
            DoLog(LogType.Warning, message, context);
        }

        /// <seealso cref="Debug.LogError(object, UnityEngine.Object)"/>
        public void LogError(object message, UnityEngine.Object context = null)
        {
            DoLog(LogType.Error, message, context);
        }

        /// <seealso cref="Debug.LogAssertion(object, UnityEngine.Object)"/>
        [System.Diagnostics.Conditional("UNITY_ASSERTIONS")]
        public void LogAssertion(object message, UnityEngine.Object context = null)
        {
            DoLog(LogType.Assert, message, context);
        }

        /// <seealso cref="Debug.LogException(Exception, UnityEngine.Object)"/>
        public void LogException(Exception exception, UnityEngine.Object context = null)
        {
            if (!IsLogTypeEnabled(LogType.Exception)) return;

            if (exception is null)
                return;

            if (context is null)
            {
                Debug.unityLogger?.LogException(exception);
            }
            else
            {
                Debug.unityLogger?.LogException(exception, context);
            }
        }
    }
}
