﻿using System;
using ORMLib.Constants;

namespace ORMLib.Database
{
    class DatabaseFactory
    {
        public IDatabase Create(string ip, string port, string dbName, string username, string password, DatabaseType databaseType)
        {
            switch(databaseType)
            {
                case DatabaseType.MSSql:
                    return new MSSql(ip, port, dbName, username, password);
                case DatabaseType.MySql:
                    return new MySql(ip,dbName, username, password);
                case DatabaseType.PostgreSql:
                    return new PostgreSql(ip, dbName, username, password);
                default:
                    throw new ArgumentException($"DatabaseTypes {databaseType} not supported");
            }
        }
    }
}
