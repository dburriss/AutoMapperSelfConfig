using AutoMapper;

namespace AutoMapper.SelfConfig.Tests.Models
{
    public class TestConfig : IHaveCustomMappings
    {
        public int Number { get; set; }
        public string Name { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Source, TestConfig>().ForMember(dest => dest.Number, opt => opt.MapFrom(src => src.Nr));
            configuration.CreateMap<TestConfig, Source>().ForMember(dest => dest.Nr, opt => opt.MapFrom(src => src.Number)); ;
        }
    }
}
