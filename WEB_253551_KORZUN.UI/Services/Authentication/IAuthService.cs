namespace WEB_253551_KORZUN.UI.Services.Authentication
{
    public interface IAuthService
    {
        /// <summary> 
        /// Регистрация пользователя на сервере аутентификации 
        /// </summary> 
        /// <param name="email">email нового пользователя</param> 
        /// <param name="password">пароль нового пользователя</param> 
        /// <param name="avatar">объект файла аватара пользователя</param> 
        /// <returns>Result - признак успешного добавления пользователя 
        /// ErrorMessage - сообщение об ошибке</returns> 
        Task<(bool Result, string ErrorMessage)> RegisterUserAsync(string email,
                                                             string password,
                                                             IFormFile? avatar);
    }
}
