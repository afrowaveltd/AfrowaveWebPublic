namespace Id.Services
{
    public class CookieService(IHttpContextAccessor httpContextAccessor) : ICookieService
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

		public void SetCookie(string key, string value, int expireTime = 0)
        {
            CookieOptions options = new CookieOptions
            {
                Expires = expireTime == 0 ? DateTime.Now.AddYears(1) : DateTime.Now.AddMinutes(expireTime),
                IsEssential = true,
                Secure = true,
            };

            _httpContextAccessor.HttpContext?.Response.Cookies.Append(key, value, options);
        }

        public void SetHttpOnlyCookie(string key, string value, int expireTime = 0)
        {
            CookieOptions options = new CookieOptions
            {
                Expires = expireTime == 0 ? DateTime.Now.AddYears(10) : DateTime.Now.AddMinutes(expireTime),
                IsEssential = true,
                Secure = true,
                HttpOnly = true,
            };
            _httpContextAccessor.HttpContext?.Response.Cookies.Append(key, value, options);
        }

        public string GetCookie(string key)
        {
            return _httpContextAccessor.HttpContext?.Request.Cookies[key] ?? string.Empty;
        }

        public void RemoveCookie(string key)
        {
            _httpContextAccessor.HttpContext?.Response.Cookies.Delete(key);
        }

        public void RemoveAllCookies()
        {
            foreach(string cookie in _httpContextAccessor.HttpContext.Request.Cookies.Keys)
            {
                _httpContextAccessor.HttpContext?.Response.Cookies.Delete(cookie);
            }
        }

        public string GetOrCreateCookie(string key, string value, int expireTime = 0)
        {
            if(string.IsNullOrEmpty(GetCookie(key)))
            {
                SetCookie(key, value, expireTime);
                return value;
            }
            else
            {
                return GetCookie(key);
            }
        }
    }
}