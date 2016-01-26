using AutoMapper;

namespace AutoMapperSelfConfig.Core
{
	public interface IHaveCustomMappings
	{
		void CreateMappings(IConfiguration configuration);
	}
}