﻿using System;
//using System.Collections.Generic;
//using System.Text;
using System.Threading.Tasks;

namespace Mine
{
    public static class TaskExtensions
    {
        // Note: Async void is intentional. This provides a way to call an async method
        // from the constructor while communicating intent to fire and forget and allow
        // handling of exceptions.
        public static async void SafeFireAndForget(this Task task,
            bool returnToCallingContext, Action<Exception> onException = null)
        {
            try
            {
                await task.ConfigureAwait(returnToCallingContext);
            }

            // If the provided action is not null, catch and pass the thrown
            // exception.
            catch (Exception ex) when (onException != null)
            {
                onException(ex);
            }
        }
    }
}
