using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AuthServer
{
    public class UserModel
    {
        [JsonPropertyName("users")]
        public List<UserFromJson> Users {get;set;}

        public static List<UserFromJson> LoadData(string path)
        {
            var content = File.ReadAllText(path);
            var options = new JsonSerializerOptions{
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
            var model = JsonSerializer.Deserialize<UserModel>(content, options);
            return model.Users;
        }
    }


    public class UserFromJson
    {
       public string Name {get;set;}

       public string GivenName {get;set;}

       public string Email {get;set;}

       public string Role {get;set;}

       [JsonPropertyName("pass")]
       public string Password {get;set;}
    }
}