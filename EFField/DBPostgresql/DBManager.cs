//using DBPostgresql.Core;
//using DryIoc;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBPostgresql
{
    public static class DBManager
    {
//        public static Container DBContainer { get; set; }
        public static string ConnectDBparams { get; set; }
        static DBManager()
        {
//            DBContainer = new Container();
//            DBContainer.Register<IPostgresContext, PostgresContext>(setup: Setup.With(trackDisposableTransient: true));
            //            DBContainer.Register<IDBProject, DBProject>();

        }
    }
}
