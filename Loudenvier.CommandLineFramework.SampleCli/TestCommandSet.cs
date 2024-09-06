using CommandLine;
using Loudenvier.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Loudenvier.CommandLineFramework.SampleCli;

static class IntelbrasCommandSet
{
    internal class GetVerbs
    {
        [Verb("general-config", HelpText = "Retrieves the current general configuration")]
        internal class GeneralConfig;
        [Verb("network-config", HelpText = "Retrieves the current network configuration")]
        internal class NetworkConfig;
        [Verb("wlan-config", HelpText = "Retrieves the current WLAN configuration")]
        internal class WLanConfig;

        [Verb("upload-config", HelpText = "Gets the HTTP UPLOAD (online mode) configuration")]
        internal class GetHttpUploadConfig;
        [Verb("intelbras-config", HelpText = "Gets the Intelbras mode configuration")]
        internal class GetIntelbrasConfig;
        [Verb("time", HelpText = "Retrieves the device's current time")]
        internal class GetCurrentTime;
        [Verb("users", HelpText = "List users stored on the device")]
        internal class GetUsers
        {
            [Value(0, MetaName = "count", HelpText = "The ammount of users to retrieve")]
            public int Count { get; set; }
            [Value(1, MetaName = "offset", HelpText = "An offset to start retrieving users")]
            public int? Offset { get; set; }
        }
        [Verb("face", HelpText = "Retrieves the facial record for a user")]
        internal class GetFace
        {
            [Value(0, MetaName = "user id", HelpText = "The id of the user")]
            public string UserId { get; set; }
        }
    }
    internal static async Task GetCommand(object value, CommandLoop loop) {
        await Task.CompletedTask;
    }
}

