using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using LinqToTwitter;

namespace App.SocialNetworkService
{
    public class TwitterService : ISocialNetworkService
    {
        private IAuthorizer _userAuth;
        private IAuthorizer _appAuth;

        public string UserOAuth()
        {
            try
            {
                OAuth1().Wait();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return "Успешная авторизация пользователя " + _userAuth.CredentialStore.ScreenName;
        }

        public string AppOAuth()
        {
            try
            {
                OAuth2().Wait();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return "Успешная авторизация приложения";
        }

        public List<string> GetLastEntries(string username, int count)
        {
            var auth = _userAuth ?? _appAuth;

            using (var twitterCtx = new TwitterContext(auth))
            {
                var statusTweets =
                    from tweet in twitterCtx.Status
                    where tweet.Type == StatusType.User
                          && tweet.ScreenName == username
                          && tweet.Count == count
                    select tweet;

                return statusTweets.Select(i => i.Text).ToList();
            }
        }

        public string PostEntry(string message)
        {
            try
            {
                using (var twitterCtx = new TwitterContext(_userAuth))
                {
                    twitterCtx.TweetAsync(message).Wait();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return "Успешно добавлен ваш твит";
        }

        private async Task OAuth2()
        {
            _appAuth = new ApplicationOnlyAuthorizer
            {
                CredentialStore = new InMemoryCredentialStore()
                {
                    ConsumerKey = ConfigurationManager.AppSettings["twitterConsumerKey"],
                    ConsumerSecret = ConfigurationManager.AppSettings["twitterSecretKey"]
                }
            };

            await _appAuth.AuthorizeAsync();
        }

        private async Task OAuth1()
        {
            this._userAuth = new PinAuthorizer()
            {
                CredentialStore = new InMemoryCredentialStore
                {
                    ConsumerKey = ConfigurationManager.AppSettings["twitterConsumerKey"],
                    ConsumerSecret = ConfigurationManager.AppSettings["twitterSecretKey"]
                },
                GoToTwitterAuthorization = pageLink => Process.Start(pageLink),
                GetPin = () =>
                {
                    Console.WriteLine("Вы должны получить PIN.\n");
                    Console.Write("Введите PIN: ");
                    return Console.ReadLine();
                }
            };

            await _userAuth.AuthorizeAsync();
        }
    }
}