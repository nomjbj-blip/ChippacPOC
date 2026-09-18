using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DACrux.SP.Common
{
    [Serializable]
    public sealed class ConnectionInformation
    {
        #region " MEMBER FIELD "

        private string name;

        private ServerType serverType;
        private HostType hostType;
        private string hostIP;
        private string hostName;
        private string databaseName;
        private string port;
        private string userID;
        private string password;

        #endregion

        #region " PROPERTY "

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public ServerType ServerType
        {
            get { return serverType; }
            set { serverType = value; }
        }

        public HostType HostType
        {
            get { return hostType; }
            set { hostType = value; }
        }

        public string HostIP
        {
            get { return hostIP; }
            set { hostIP = value; }
        }

        public string HostName
        {
            get { return hostName; }
            set { hostName = value; }
        }

        public string DatabaseName
        {
            get { return databaseName; }
            set { databaseName = value; }
        }

        public string Port
        {
            get { return port; }
            set { port = value; }
        }

        public string UserID
        {
            get { return userID; }
            set { userID = value; }
        }

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        #endregion

        public ConnectionInformation()
        {

        }
    }

    public enum ServerType
    {
        Oracle
    }

    public enum HostType
    {
        IP,
        Alias
    }
}
