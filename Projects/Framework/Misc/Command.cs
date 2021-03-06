﻿// Copyright (c) Multi-Emu.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Framework.Logging;

namespace Framework.Misc
{
    public class Command
    {
        public static T Read<T>(string[] args, int index)
        {
            try
            {
                return args[index].ChangeType<T>();
            }
            catch
            {
                Log.Error("Wrong arguments for the current command.");
            }

            return default(T);
        }
    }
}
