using System;
using UnityEngine;

namespace JakePerry.Unity
{
    /// <summary>
    /// This class can be used to group log messages from a particular
    /// package, plugin, code system, etc with the option to prepend all
    /// log messages with a common identifier.
    /// Additionally, logs of any <see cref="LogType"/> can be enabled or disabled
    /// for the log group independent of the underlying <see cref="ILogger"/>.
    /// </summary>
    public sealed class PackageLogger
    {
        private readonly string m_id;

        private byte m_logTypes = byte.MaxValue;

        /// <summary>
        /// The identifier prepended to log messages,
        /// or <see langword="null"/> if none was specified.
        /// </summary>
        public string GroupId => m_id;

        /// <param name="packageName">
        /// [Optional] Short identifier prepended to log messages.
        /// </param>
        public PackageLogger(string groupId = null)
        {
            if (!(groupId is null))
            {
                groupId = groupId.Trim();

                if (groupId.Length > 0)
                    m_id = groupId;
            }
        }

        /// <summary>
        /// Converts a <see cref="LogType"/> value to an integer in range [0-4]
        /// to be used in bitwise operations with <see cref="m_logTypes"/>.
        /// Guards against invalid values, defaults to <see cref="LogType.Log"/>.
        /// </summary>
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

        /// <summary>
        /// Indicates the enable state of the given <see cref="LogType"/> for this log group.
        /// </summary>
        public bool IsLogTypeEnabled(LogType type)
        {
            return (m_logTypes & (1 << GetByteOffset(type))) != 0;
        }

        /// <summary>
        /// Set the enable state of the given <see cref="LogType"/> for this log group.
        /// </summary>
        public void SetLogTypeEnabled(LogType type, bool enabled)
        {
            if (enabled)
                m_logTypes = (byte)(m_logTypes & (1 << GetByteOffset(type)));
            else
                m_logTypes = (byte)(m_logTypes | ~(1 << GetByteOffset(type)));
        }

        /// <summary>
        /// Set the enable state of all <see cref="LogType"/> values for this log group.
        /// </summary>
        public void SetAllLogTypesEnabled(bool enabled)
        {
            m_logTypes = enabled ? byte.MaxValue : byte.MinValue;
        }

        private void DoLog(LogType logType, object message, UnityEngine.Object context)
        {
            if (!IsLogTypeEnabled(logType))
                return;

            if (!(m_id is null))
                message = $"[{m_id}] {message}";

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
