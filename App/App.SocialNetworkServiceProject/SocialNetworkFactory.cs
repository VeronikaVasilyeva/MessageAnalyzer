namespace App.SocialNetworkService
{
    static class SocialNetworkFactory
    {
        public enum SocialNetworkEnum
        {
            FacebookService,
            TwitterService,
            VkontakteService
        }

    public static ISocialNetworkService CreateInstance(SocialNetworkEnum socialNetwork)
    {
        return null;
    }

    }
}
