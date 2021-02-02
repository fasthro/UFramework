/*
 * @Author: fasthro
 * @Date: 2020/12/18 16:16:14
 * @Description:
 */

using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using UFramework;

namespace GameServer
{
    public class UserInfo
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public long uid { get; set; }
        public string username { get; set; }
        public string password { get; set; }
    }

    public class UserModel : BaseModel
    {
        public UserModel() : base("UserInfo")
        {
            BsonClassMap.RegisterClassMap<UserInfo>();
        }

        public long AddUser(UserInfo user)
        {
            // user.uid = Helper.NewGuidId();
            if (!Add<UserInfo>(user))
            {
                return 0L;
            }
            return user.uid;
        }
        
        public UserInfo GetUser(long uid)
        {
            return Exist<UserInfo>(u => u.uid == uid);
        }
        

        public long ExistUser(long uid)
        {
            var result = Exist<UserInfo>(u => u.uid == uid);
            if (result != null)
            {
                return result.uid;
            }
            return 0L;
        }
    }
}