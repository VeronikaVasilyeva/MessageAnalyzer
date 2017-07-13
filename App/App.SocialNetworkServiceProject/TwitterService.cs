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

            return $"Успешная авторизация пользователя {_userAuth.CredentialStore.ScreenName}";
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

            try
            {
                using (var twitterCtx = new TwitterContext(auth))
                {
                    return twitterCtx.Status
                        .Where(tweet => tweet.Type == StatusType.User && tweet.ScreenName == username && tweet.Count == count)
                        .Select(i => i.Text)
                        .ToList();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
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
            _userAuth = new PinAuthorizer()
            {
                CredentialStore = new InMemoryCredentialStore
                {
                    ConsumerKey = ConfigurationManager.AppSettings["twitterConsumerKey"],
                    ConsumerSecret = ConfigurationManager.AppSettings["twitterSecretKey"]
                },
                GoToTwitterAuthorization = pageLink =>
                {
                    Console.WriteLine("Если автоматический переход в браузер не произошел, зайдите в браузер самостоятельно и авторизуйтесь по этой ссылке:\n" + pageLink + "\n");
                    Process.Start(pageLink);
                },
                GetPin = () =>
                {
                    Console.WriteLine("Вы должны разрешить приложению доступ к вашему аккаунту и получить PIN");
                    Console.WriteLine("Введите PIN: ");
                    return Console.ReadLine();
                }
            };

            await _userAuth.AuthorizeAsync();
        }
    }
}