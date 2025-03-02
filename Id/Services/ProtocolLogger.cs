using MailKit;
using System.Text;

namespace Id.Services
{
	public class StringProtocolLogger : IProtocolLogger
	{
		private readonly StringBuilder _logBuilder = new();

		// Implementace vlastnosti z rozhraní IProtocolLogger
		public IAuthenticationSecretDetector AuthenticationSecretDetector { get; set; } = new MyCustomAuthenticationSecretDetector();

		public string GetLog() => _logBuilder.ToString();

		public void LogConnect(Uri uri)
		{
			_logBuilder.AppendLine($"Connected to {uri}");
		}

		public void LogClient(byte[] buffer, int offset, int count)
		{
			LogData("C", buffer, offset, count);
		}

		public void LogServer(byte[] buffer, int offset, int count)
		{
			LogData("S", buffer, offset, count);
		}

		private void LogData(string prefix, byte[] buffer, int offset, int count)
		{
			string data = Encoding.UTF8.GetString(buffer, offset, count);

			// Použije detekci tajných údajů, pokud je detektor nastavený
			if(AuthenticationSecretDetector != null)
			{
				var secrets = AuthenticationSecretDetector.DetectSecrets(buffer, offset, count);
				// Zde můžeš provést maskování tajných údajů podle potřeby
				data = MaskSecrets(data, secrets);
			}

			_logBuilder.AppendLine($"{prefix}: {data}");
		}

		private string MaskSecrets(string input, IList<AuthenticationSecret> secrets)
		{
			// Jednoduchá implementace maskování tajných údajů
			foreach(var secret in secrets)
			{
				// Převedeme AuthenticationSecret na řetězec a maskujeme ho
				string secretText = Encoding.UTF8.GetString(secret.Buffer, secret.Offset, secret.Count);
				input = input.Replace(secretText, "***");
			}
			return input;
		}

		public void Dispose()
		{
			// Nic se tu nezavírá, protože StringBuilder je jen v paměti
		}
	}

	// Vlastní implementace IAuthenticationSecretDetector
	public class MyCustomAuthenticationSecretDetector : IAuthenticationSecretDetector
	{
		public IList<AuthenticationSecret> DetectSecrets(byte[] buffer, int offset, int count)
		{
			var secrets = new List<AuthenticationSecret>();

			// Příklad: Detekce řetězce "password" v bajtovém poli
			string data = Encoding.UTF8.GetString(buffer, offset, count);
			if(data.Contains("password"))
			{
				// Pokud je detekován řetězec "password", vytvoříme instanci AuthenticationSecret
				secrets.Add(new AuthenticationSecret(buffer, offset, count));
			}

			return secrets;
		}
	}
}