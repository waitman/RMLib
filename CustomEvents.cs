﻿/*
  RMLib: Nonvisual support classes used by multiple R&M Software programs
  Copyright (C) 2008-2014  Rick Parrish, R&M Software

  This file is part of RMLib.

  RMLib is free software: you can redistribute it and/or modify
  it under the terms of the GNU General Public License as published by
  the Free Software Foundation, either version 3 of the License, or
  any later version.

  RMLib is distributed in the hope that it will be useful,
  but WITHOUT ANY WARRANTY; without even the implied warranty of
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
  GNU General Public License for more details.

  You should have received a copy of the GNU General Public License
  along with RMLib.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Diagnostics;

namespace RandM.RMLib
{
    public class CommandLineParameterEventArgs : EventArgs
    {
        public char Key { get; set; }
        public string Value { get; set; }

        public CommandLineParameterEventArgs(char key, string value)
        {
            Key = key;
            Value = value;
        }
    }

    public class RMProcessStartAndWaitEventArgs : EventArgs
    {
        public bool Stop { get; set; }
        
        public RMProcessStartAndWaitEventArgs()
        {
            Stop = false;
        }
    }

    public class ExceptionEventArgs : EventArgs
    {
        public Exception Exception { get; set; }
        public string Message { get; set; }

        public ExceptionEventArgs(string message, Exception exception)
        {
            Message = message;
            Exception = exception;
        }
    }

    public class FtpUploadProgressEventArgs : EventArgs
    {
        public long BytesSent { get; set; }
        public long TotalBytesToSend { get; set; }

        public FtpUploadProgressEventArgs(long totalBytesToSend)
        {
            BytesSent = 0;
            TotalBytesToSend = totalBytesToSend;
        }
    }

    public class IntEventArgs : EventArgs
    {
        public int Value { get; set; }

        public IntEventArgs(int value)
        {
            Value = value;
        }
    }

    public class StringEventArgs : EventArgs
    {
        public string Text { get; set; }

        public StringEventArgs(string text)
        {
            Text = text;
        }
    }

    public class ProcessStartEventArgs : EventArgs
    {
        public int ProcessId { get; set; }

        public ProcessStartEventArgs(int processId)
        {
            ProcessId = processId;
        }
    }
}
