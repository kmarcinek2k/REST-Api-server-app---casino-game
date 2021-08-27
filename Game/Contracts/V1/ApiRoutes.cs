using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Game.Contracts.V1
{
    public static class ApiRoutes
    {
        public const string Version = "v1";
        public const string Root = "api";
        public const string Base = Root + "/" + Version;

       
        public static class Identity
        {
            public const string Login = Base + "/identity/autorization";
            public const string Register = Base + "/identity/signup";

            public const string Refresh = Base + "/identity/refresh";
        }
        public static class Boards
        {
            public const string Create = Base + "/boards";
            public const string GetAll = Base + "/boards";
            public const string Get = Base + "/boards/{boardId}";
            public const string Delete = Base + "/boards/{boardId}";
            public const string Update = Base + "/boards/{boardId}";
            public const string Enter = Base + "/boards/{boardId}";
            public const string UpdateCurrUsers = Base + "/boards/config/{boardId}";

        }

        public static class Tags
        {
            public const string GetAll = Base + "/tags";
 
        }
        public static class Picks
        {
            public const string Get = Base + "/pick";

        }

        public static class Posts
        {
            public const string GetAll = Base + "/posts";
            public const string Get = Base + "/posts/{postId}";
            public const string Update = Base + "/posts/{postId}";
            public const string Delete = Base + "/posts/{postId}";
            public const string Create = Base + "/posts";

        }

    }
}
