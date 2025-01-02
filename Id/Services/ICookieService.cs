namespace Id.Services
{
    public interface ICookieService
    {
        public string GetCookie(string key);

        public string GetOrCreateCookie(string key, string value, int expireTime = 0);

        public void RemoveAllCookies();

        public void RemoveCookie(string key);

        public void SetCookie(string key, string value, int expireTime = 0);

        public void SetHttpOnlyCookie(string key, string value, int expireTime = 0);
    }
}