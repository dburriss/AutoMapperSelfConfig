namespace AutoMapperSelfConfig.Core.Tests.Models
{
    public class TestEasy : IMapTo<Source>, IMapFrom<Source>
    {
        public int Nr { get; set; }
        public string Name { get; set; }
    }
}
