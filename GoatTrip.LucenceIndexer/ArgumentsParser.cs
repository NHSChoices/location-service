using System;
using System.Collections.Generic;
using System.Linq;

namespace GoatTrip.LucenceIndexer
{
    public class ArgumentsParser
    {


        private Dictionary<string, Action<string>> _definedArguments;

        public ArgumentsParser(Dictionary<string, Action<string>> definedArguments)
        {
            _definedArguments = definedArguments;
        }

        public void ParseArgs(string[] args)
        {
            var currentIndex = -1;
            foreach (var arg in args)
            {
                currentIndex ++;
                var argKey = GetArgumnetKey(arg);
                if(!_definedArguments.ContainsKey(argKey)) continue;

                var value = GetArgValue(args, arg, currentIndex);

                _definedArguments[argKey].Invoke(RemoveQuotes(value));
            }
        }

        private string RemoveQuotes(string value)
        {
            return value.Replace("\"", "");
        }

        private string GetArgValue(string[] args, string arg, int currentIndex)
        {
            var value = "";
            if (ValueDeliminatedInSingleKey(arg))
                value = arg.Remove(0, arg.IndexOf('=') + 1);
            else if (args.Length > currentIndex + 1)
                value = args[currentIndex + 1];
            return value;
        }

        private static string GetArgumnetKey(string arg)
        {
            var argKey = "";
            if (ValueDeliminatedInSingleKey(arg))
                argKey = arg.Remove(arg.IndexOf('='), arg.Length - 1);
            else
                argKey = arg;
            return argKey;
        }

        private static bool ValueDeliminatedInSingleKey(string arg)
        {
            return arg.IndexOf('=') != -1;
        }
    }
}