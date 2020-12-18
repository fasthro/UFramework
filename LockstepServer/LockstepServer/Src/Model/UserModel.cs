﻿/*
 * @Author: fasthro
 * @Date: 2020/12/18 16:16:14
 * @Description:
 */

using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace LockstepServer.Src
{
    public class UserInfo
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public long uid { get; set; }
        public long username { get; set; }
        public long password { get; set; }
    }

    public class UserModel : BaseModel
    {
        public UserModel() : base("UserInfo")
        {
            BsonClassMap.RegisterClassMap<UserInfo>();
        }

        public long AddUser(UserInfo user)
        {
            user.uid = Helper.NewGuidId();
            if (!Add<UserInfo>(user))
            {
                return 0L;
            }
            return user.uid;
        }
    }
}