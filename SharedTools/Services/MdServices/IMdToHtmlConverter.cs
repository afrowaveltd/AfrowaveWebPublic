using SharedTools.Models.MdModels;

namespace SharedTools.Services.MdServices
{
	public interface IMdToHtmlConverter
	{
		string Convert(string markdown, MdConversionOptions? options = null);

		IAsyncEnumerable<string> StreamConvertAsync(string markdown, MdConversionOptions? options = null, CancellationToken cancellationToken = default);
	}
}