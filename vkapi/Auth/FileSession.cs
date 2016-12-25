using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using log4net;
using vkapi.Objects;

namespace vkapi.Auth
{
    public class FileSession: Configuration
    {
        private ILog log = LogHelper.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public UserAccessKey UserAccessKey;
        public string Filename = "account.bin";

        /// <summary>
        /// Сохраняем сессию пользователя в файл
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        public bool SaveSession(UserAccessKey session)
        {
            string path = @application.temp_folder + "/" + Filename;
            if (!File.Exists(path))
            {
                log.Error("Файл "+ Filename +" не найден.");
                log.Info("Создаем файл " + path);
                File.Create(path).Close();
            }

            try
            {
                log.Info("Сохраняем сессию пользователя ID: " + session.accessToken.user_id);
                IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
                formatter.Serialize(stream, session);
                return true;
            }
            catch (Exception e)
            {
                log.Error("Ошибка работы с файлом \n", e);
                return false;
            }
        }

        /// <summary>
        /// Загружаем сессию пользователя из файла
        /// </summary>
        /// <returns></returns>
        public bool LoadSession()
        {
            string path = @application.temp_folder + "/" + Filename;
            if (!File.Exists(path))
            {
                log.Error("Ошибка загрузки сессии. Файл " + Filename + " не найден");
                return false;
            }
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);

            try
            {
                UserAccessKey = (UserAccessKey) formatter.Deserialize(stream);
                stream.Close();
                log.Info("Сессия пользователя ID:" + UserAccessKey.accessToken.user_id + " успешно загруженна");
                return true;
            }
            catch (Exception e)
            {
                log.Error("Ошибка загрузка сессии из файла " + Filename, e);
                stream.Close();
                return false;
            }
        }
    }
}
