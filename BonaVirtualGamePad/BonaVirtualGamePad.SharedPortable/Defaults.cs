using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BonaVirtualGamePad.Shared
{
    public static class Defaults
    {
        #region Server setup data
        public const String DEFAULT_SERVER_NAME = "Game Server";
        public const String DEFAULT_GAME_NAME = "Default Bona Virtual Gamepad Game";
        public const int DEFAULT_SERVER_PORT = 2000;
        public const int DEFAULT_SERVER_CAPACITY = 12;
        #endregion

        #region Client connection data
        // Number of times a connection request will be sent in order to get a response before it considered the connection a failure
        public const int CLIENT_CONNECTION_ATTEMPTS = 5;

        // Time between connection attempts in seconds
        public const float CLIENT_CONNECTION_ATTEMPT_TIMER = 1.0f;

        #endregion
    }
}
