using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Authenticators;
using UPS_Sample.restops.domain;

namespace UPS_Sample.restops.operations
{
    public class UserActions
    {

        private const string RESOURCE_NAME = "users/";

        public UserModel getUsers(int page, string searchTerm)
        {
            var url = Properties.Settings.Default.BaseRestUrl + RESOURCE_NAME + Properties.Settings.Default.FormatAndToken;

            if (page > 1)
            {
                url = url + "&page=" + page;
            }

            if (searchTerm != null && searchTerm.Length > 0)
            {
                url = url + "&name=" + searchTerm;
            }

            var client = new RestClient(url);

            var request = new RestRequest();

            var response = client.Get<UserModel>(request);

            return response.Data;
        }

        public bool deleteUser(int id)
        {
            var url = Properties.Settings.Default.BaseRestUrl + RESOURCE_NAME + id + Properties.Settings.Default.FormatAndToken;

            var client = new RestClient(url);

            var request = new RestRequest();

            var response = client.Delete<UserModel>(request);

            return response.IsSuccessful;
        }

        public User loadUser(int id)
        {
            var url = Properties.Settings.Default.BaseRestUrl + RESOURCE_NAME + id + Properties.Settings.Default.FormatAndToken;

            var client = new RestClient(url);

            var request = new RestRequest();

            var response = client.Get<UserModel>(request);

            return response.Data.data[0];
        }

        public bool updateUser(User userToOperate)
        {
            string url = Properties.Settings.Default.BaseRestUrl + RESOURCE_NAME + userToOperate.id + Properties.Settings.Default.FormatAndToken;

            var client = new RestClient(url);

            var request = new RestRequest().AddJsonBody(userToOperate);

            if (userToOperate.id > 0)
            {
                var response = client.Patch<UserModel>(request);
                return response.IsSuccessful;
            }

            return false;
        }

        public bool createUser(User userToOperate)
        {
            string url = Properties.Settings.Default.BaseRestUrl + RESOURCE_NAME + Properties.Settings.Default.FormatAndToken;

            var client = new RestClient(url);

            var request = new RestRequest().AddJsonBody(userToOperate);

            if (userToOperate.id == 0)
            {
                var response = client.Post<UserModel>(request);
                return response.IsSuccessful;
            }

            return false;
        }
    }
}
