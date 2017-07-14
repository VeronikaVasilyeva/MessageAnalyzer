using System;

namespace MessageAnalyzer.SocialNetworkService.Factory
{
    public static class SocialNetworkFactory
    {
        public static ISocialNetworkService Create(SocialNetworkType type)
        {
            switch (type)
            {
                case SocialNetworkType.Facebook:
                    return new FacebookService();
                case SocialNetworkType.Twitter:
                    return new TwitterService();
                case SocialNetworkType.Vkontakte:
                    return new VkontakteService();
                default:
                    throw new NotSupportedException("сорри");
            }
        }
    }
}