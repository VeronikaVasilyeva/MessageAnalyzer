using System.Collections.Generic;

namespace App.SocialNetworkService
{
    public interface ISocialNetworkService
    {
        /// <summary>
        /// Авторизация пользователя. Позволяет работать от имени пользователя
        /// Подробнее см.документацию Twitter OAuth
        /// </summary>
        /// <returns></returns>
        void UserOAuth();

        /// <summary>
        /// Авторизация приложения. Позволяет совершать действия от имени приложения.
        /// Подробнее см.документацию Twitter OAuth
        /// </summary>
        /// <returns></returns>
        void AppOAuth();

        /// <summary>
        /// LogOut для пользователя. 
        /// Подробнее см.документацию Twitter OAuth
        /// </summary>
        /// <returns></returns>
        void UserLogOut();

        /// <summary>
        /// Возвращает последние count постов у username
        /// </summary>
        /// <param name="username"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        List<string> GetLastEntries(string username, int count);

        /// <summary>
        /// Опубликовывает сообщение
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        void PostEntry(string message);

    }
}
