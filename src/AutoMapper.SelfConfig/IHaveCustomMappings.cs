using AutoMapper;

namespace AutoMapper.SelfConfig
{
	public interface IHaveCustomMappings
	{
		void CreateMappings(IConfiguration configuration);
	}
}