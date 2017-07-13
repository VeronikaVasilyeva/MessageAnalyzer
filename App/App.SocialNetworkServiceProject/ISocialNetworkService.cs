using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace App.SocialNetworkService
{
    public interface ISocialNetworkService
    {
        /// <summary>
        /// Авторизация пользователя. Позволяет работать от имени пользователя
        /// Подробнее см.документацию Twitter OAuth
        /// </summary>
        /// <returns></returns>
        string UserOAuth();

        /// <summary>
        /// Авторизация приложения. Позволяет совершать действия от имени приложения.
        /// Подробнее см.документацию Twitter OAuth
        /// </summary>
        /// <returns></returns>
        string AppOAuth();

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
        string PostEntry(string message);

    }
}
