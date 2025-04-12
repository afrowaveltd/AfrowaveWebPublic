namespace SharedTools.Services.MdServices;

public interface IMdParserService
{
	Task<string> ConvertToHtmlAsync(string markdown);
}